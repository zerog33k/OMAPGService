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
    ""custom_data"" : {""pokemon_id"": ""poke_id"", ""expires"": ""expires_time"", ""lat"": ""poke_lat"", ""lon"": ""poke_lon""}
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
            var lastPoke = 0;
            var context = new OMAPGContext();
            context.ConnectString = config.DataAccessPostgresqlProvider;

            while (true)
            {
                if (lastPoke == 0)
                {
                    lastPoke = context.Pokemon.Max(p => p.idValue);
                }
                Console.WriteLine("Loading data...");
                await ServiceLayer.SharedInstance.LoadData(lastPoke);
                Console.WriteLine($"Loaded {ServiceLayer.SharedInstance.Pokemon.Count} Pokemon, {ServiceLayer.SharedInstance.Gyms.Count} Gyms, and {ServiceLayer.SharedInstance.Raids.Count} Raids!");
                lastPoke = ServiceLayer.SharedInstance.Pokemon?.MaxBy(p => p.idValue)?.idValue ?? 0;

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
                    Console.WriteLine("Added pokemon to database!");
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
                using (var client = new HttpClient())
                {
                    foreach (var dev in context.Devices.ToList())
                    {
                        var sent = 0;
                        foreach (var np in dev.NotifyPokemon)
                        {
                            var toNotify = ServiceLayer.SharedInstance.Pokemon.Where(p => p.pokemon_id == np);
                            foreach (var p in toNotify)
                            {
                                var pLoc = new GeoCoordinate(p.lat, p.lon);
                                var dLoc = new GeoCoordinate(dev.LocationLat, dev.LocationLon);
                                var dist = pLoc.GetDistanceTo(dLoc) * 0.00062137;
                                if ((dist < dev.DistanceAlert || p.pokemon_id == 201) && p.ExpiresDate > DateTime.UtcNow)
                                {
                                    var content = notifyContent.Replace("notify_title", $"{p.name} Found!");
                                    content = content.Replace("notify_body", $"{p.name} Found {dist.ToString("F1")} miles away!");
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
                        }
                        Console.WriteLine($"Sent {sent} notifications for device {dev.Id}");
                    }
                }
                ServiceLayer.SharedInstance.Pokemon.RemoveRange(0, ServiceLayer.SharedInstance.Pokemon.Count());

                await Task.Delay(20000);
            }
        }
    }
}
 