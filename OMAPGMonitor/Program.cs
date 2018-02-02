using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Newtonsoft.Json;
using OMAPGMap;
using OMAPGServiceData.Models;
using GeoCoordinatePortable;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;

namespace OMAPGMonitor
{
    class Program
    {
        static string notifyContent = @"
{
 ""notification_content"" : {

    ""name"" : ""Pokemon Found"",
    ""title"" : ""notify_title"",
    ""body"" : ""notify_body"",
    ""custom_data"" : {""pokemon_id"": ""poke_id"", ""expires"": ""expires_time"", ""lat"": ""poke_lat"", ""lon"": ""poke_lon"", ""sound"" : ""default""}
  },
    ""notification_target"" : {
    ""type"" : ""devices_target"",
    ""devices"" : [""device_id""]
    }
}";

        static async Task Main(string[] args)
        {
            var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("Config.json"));
            ServiceLayer.SharedInstance.Username = config.Username;
            ServiceLayer.SharedInstance.Password = config.Password;
            var minTimestamp = (long)(DateTime.UtcNow.AddMinutes(-5.0).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var context = new OMAPGContext();
            context.ConnectString = config.DataAccessPostgresqlProvider;
            var lastTimestamp = minTimestamp;
            if (context.Pokemon.Count() > 0)
            {
                var maxTimestamp = context.Pokemon.Max(p => p.timestamp);
                lastTimestamp = minTimestamp > maxTimestamp ? minTimestamp : maxTimestamp;
            }
            Console.WriteLine("Loading data...");
            await ServiceLayer.SharedInstance.LoadData(lastTimestamp);
            Console.WriteLine($"Loaded {ServiceLayer.SharedInstance.Pokemon.Count} Pokemon, {ServiceLayer.SharedInstance.Gyms.Count} Gyms, and {ServiceLayer.SharedInstance.Raids.Count} Raids!");

            foreach (var p in ServiceLayer.SharedInstance.Pokemon)
            {
                var pokeInDB = context.Pokemon.Find(new object[] { p.idValue });
                if (pokeInDB == null)
                {
                    context.Pokemon.Add(p);
                }
                else
                {
                    //update it maybe?
                }
            }
            try
            {
                context.SaveChanges();
                Console.WriteLine("Added \tpokemon to database!");
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            using (var client = new HttpClient())
            {
                foreach (var dev in context.Devices.Where(d => d.NotifyEnabled).ToList())
                {
                    context.Entry<Device>(dev).Reload();
                    var sent = 0;
                    var toNotify = new List<Pokemon>();
                    if (dev.Notify90)
                    {
                        var plus90 = ServiceLayer.SharedInstance.Pokemon.Where(p => p.iv > 0.9 && p.level > dev.MinLevelAlert);
                        toNotify.AddRange(plus90);
                    }
                    else if (dev.Notify100)
                    {
                        var hundred = ServiceLayer.SharedInstance.Pokemon.Where(p => p.iv > 0.99 && p.level > dev.MinLevelAlert);
                        toNotify.AddRange(hundred);
                    }
                    foreach (var np in dev.NotifyPokemon)
                    {
                        toNotify.AddRange(ServiceLayer.SharedInstance.Pokemon.Where(p => p.pokemon_id == np));
                    }
                    if (dev.IgnorePokemon.Count() > 0)
                    {
                        toNotify = toNotify.Where(p => dev.IgnorePokemon.Contains(p.pokemon_id)).ToList();
                    }
                    foreach (var p in toNotify.Distinct())
                    {
                        var pLoc = new GeoCoordinate(p.lat, p.lon);
                        var dLoc = new GeoCoordinate(dev.LocationLat, dev.LocationLon);
                        var dist = pLoc.GetDistanceTo(dLoc) * 0.00062137;
                        if (((dist < dev.DistanceAlert && p.iv < 0.99) || p.pokemon_id == 201 || p.iv > 0.99)  && dist < dev.MaxDistance )
                        {
                            var content = "";
                            DateTime cstTime = p.ExpiresDate.AddHours(-6.0);
                            var dsTime = cstTime.ToString("h:mm:ss");
                            if(p.pokemon_id == 201)
                            {
                                var letter = ((Char)(65 + (p.form - 1))).ToString();
                                var iv = p.iv * 100;
                                content = notifyContent.Replace("notify_title", $"{iv.ToString("F1")}% {p.name} Found, letter {letter}!");
                                content = content.Replace("notify_body", $"{dist.ToString("F1")} miles away! ({p.atk}/{p.def}/{p.sta}) - Level {p.level}, CP {p.cp}, available till {dsTime}.");
                            }
                            else if (p.iv > 0.9)
                            {
                                var iv = p.iv * 100;
                                content = notifyContent.Replace("notify_title", $"{iv.ToString("F1")}% {p.name} Found!");
                                content = content.Replace("notify_body", $"{dist.ToString("F1")} miles away! ({p.atk}/{p.def}/{p.sta}) - Level {p.level}, CP {p.cp}, available till {dsTime}.");
                            }
                            else
                            {
                                content = notifyContent.Replace("notify_title", $"{p.name} Found!");
                                content = content.Replace("notify_body", $"{dist.ToString("F1")} miles away! Available till {dsTime}.");
                            }

                            content = content.Replace("device_id", dev.DeviceId);
                            content = content.Replace("poke_id", p.id);
                            content = content.Replace("expires_time", p.expires_at.ToString());
                            content = content.Replace("poke_lat", p.lat.ToString("F"));
                            content = content.Replace("poke_lon", p.lon.ToString("F"));

                            var strContent = new StringContent(content, Encoding.UTF8, "application/json");
                            strContent.Headers.Add("X-API-Token", config.AppCenterToken);
                            try
                            {
                                var response = await client.PostAsync("https://appcenter.ms/api/v0.1/apps/zerogeek/Omaha-PG-Map/push/notifications", strContent);
                                if (!response.IsSuccessStatusCode)
                                {
                                    Console.WriteLine($"Push notification failed with code {response.StatusCode}");
                                }
                                else
                                {
                                    sent++;
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Error sending push notification for device {dev.Id}");
                            }
                        }
                    }
                    Console.WriteLine($"Sent {sent} notifications for device {dev.Id}");
                }
            }
            ServiceLayer.SharedInstance.Pokemon.RemoveRange(0, ServiceLayer.SharedInstance.Pokemon.Count());
        }
    }
}
 