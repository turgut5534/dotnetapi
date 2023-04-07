﻿using Forecast.Data;
using Forecast.Interfaces;
using Forecast.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Forecast.Controllers
{
    [Route("api/weather")]
    [ApiController]
    public class WeatherController : Controller
    {
        private readonly IWeatherRepository _weatherRepository;
        private readonly HttpClient _httpClient;
        private readonly DataContext _context;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };

        public WeatherController(IWeatherRepository weatherRepository, HttpClient httpClient, DataContext context)
        {
            _weatherRepository = weatherRepository;
            _httpClient = httpClient;
            _context = context;
        }

        [HttpGet]
        [Route("forecast")]
        public async Task<IActionResult> GetTomSawyerMetadataAsync(float lat, float lon, double dis, long utc)
        {

            //Veritabanındaki tüm konumları al
            var existingLocations = await _context.WeatherLocations
                .Include(wl => wl.Weathers)
                .ToListAsync();

            //En başta bu değişken boş, ileride en yakın konum bulunursa atanacak
            WeatherLocation closestLocation = null;

            //En yakın mesafe şimdilik boş, konum bulunursa atanacak
            double closestDistance = double.MaxValue;

            //Tüm konumları gez
            foreach (var location in existingLocations)
            {
                //Sıra sıra her konum için parametre olarak gelen lat ve lon konumu ile aralarındaki mesafeyi hesapla
                var distance = CalculateDistance(lat, lon, location.Latitude, location.Longitude);

                //Eğer parametre olarak gelen konum ile veritabanında bulunan konum arasında parametredeki kadar mesafe varsa (yakınsa) bulunan konumu işaretle ve diğer konumlara bakmadan geç
                if (distance < dis)
                {
                    closestLocation = location;
                    closestDistance = distance;
                    break;
                }
            }

            //Eğer yakın bir konum bulunduysa bu konumu döndür. (API'ye istek atmaya gerek kalmadı)
            if (closestLocation != null)
            {

                var matchingWeather = closestLocation.Weathers.FirstOrDefault(w => w.Dt == utc);

                if (matchingWeather != null)
                {
                    var options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve
                    };
                    // Return the matching weather data as JSON
                    return Json(matchingWeather, options);
                }
                else
                {
                    // No matching weather data found
                    return NotFound();
                }
            }

            //Eğer veritabanında aynı veya yakın bir konum bulunamazsa buraya gir ve API'ye istek at
            var response = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&appid=7fe4a9309691729c4c877c5559f1bdde&units=metric");

            //API'ye istek atarken hata çıkarsa hatayı döndür
            if (!response.IsSuccessStatusCode)
            {
                return Problem($"Bir hata oluştu. Hata Kodu: {response.StatusCode}");
            }

            //API'den gelen isteği string olarak oku
            var responseContent = await response.Content.ReadAsStringAsync();

            //Gelen cevabı JSON formatında al ve "WeatherResponse" clasında her bir JSON verisinin karşılığı olarak mapping işlemi yap
            var metadata = JsonSerializer.Deserialize<WeatherResponse>(responseContent);

            //Veritabanına eklemek üzere yeni konumu oluştur
            var weatherLocation = new WeatherLocation
            {
                Latitude = (float)lat,
                Longitude = (float)lon
            };

            //Verilen parametredeki zamana uyan tek bir hava durumunu çel
            var matchingWeatherFromJson = metadata.WeatherDataList.FirstOrDefault(w => w.Dt ==utc);

            //Saatlik olarak 4 günde toplam (24*4 =96 ) 96 saat için veri gelecek, bu verileri ilgili konuma kaydet
            for (int i = 0; i < metadata.Count; i++)
            {
                var weather = new Weather
                {
                    Dt = metadata.WeatherDataList[i].Dt,
                    Temperature = metadata.WeatherDataList[i].Main.Temperature,
                    TempMin = metadata.WeatherDataList[i].Main.MinimumTemperature,
                    TempMax = metadata.WeatherDataList[i].Main.MaximumTemperature,
                    Pressure = metadata.WeatherDataList[i].Main.Pressure,
                    SeaLevel = metadata.WeatherDataList[i].Main.SeaLevel,
                    GrndLevel = metadata.WeatherDataList[i].Main.GroundLevel,
                    Humidity = metadata.WeatherDataList[i].Main.Humidity,
                    WindSpeed = metadata.WeatherDataList[i].Wind.Speed,
                    WindDeg = metadata.WeatherDataList[i].Wind.Deg,
                    WindGust = metadata.WeatherDataList[i].Wind.Gust,
                    Clouds = metadata.WeatherDataList[i].Clouds.All,
                    Visibility = metadata.WeatherDataList[i].Visibility,
                    WeatherId = metadata.WeatherDataList[i].WeatherInfo[0].Id,
                    WeatherMain = metadata.WeatherDataList[i].WeatherInfo[0].Main,
                    WeatherDescription = metadata.WeatherDataList[i].WeatherInfo[0].Description,
                    WeatherIcon = metadata.WeatherDataList[i].WeatherInfo[0].Icon,
                    Rain1h = metadata.WeatherDataList[i].Rain?.VolumeLast1Hours ?? null,
                    Rain3h = metadata.WeatherDataList[i].Rain?.VolumeLast3Hours ?? null,
                    Snow1h = metadata.WeatherDataList[i].Snow?.VolumeLast1Hours ?? null,
                    Snow3h = metadata.WeatherDataList[i].Snow?.VolumeLast3Hours ?? null,
                    dt_txt = metadata.WeatherDataList[i].DateTimeText,
                    icon = metadata.WeatherDataList[i].WeatherInfo[0].Icon,
                    WeatherLocation = weatherLocation
                };

                //Hava durumunun konumu ata
                weather.WeatherLocation = weatherLocation;

                //Hava durumuna oluşan hava durumunu ata
                _context.Weathers.Add(weather);
            }

            //Veritabanına kaydet
            _context.SaveChanges();

            //Yukarıda çektiğimiz parametredeki zamanla eşleşen tek veriyi döndür
            return Ok(JsonSerializer.Serialize(matchingWeatherFromJson));
        }


        //Bu fonksiyon iki konum arasındaki mesafeyi geometrik olarak hesaplar
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth radius in km
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c;
            return d;
        }

        private static double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

    }
}