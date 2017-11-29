﻿using System;
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
        static async Task Main(string[] args)
        {
            var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("Config.json"));
            ServiceLayer.SharedInstance.Username = config.Username;
            ServiceLayer.SharedInstance.Password = config.Password;
            var context = new OMAPGContext();
            context.ConnectString = config.DataAccessPostgresqlProvider;

            string notifyContent = @"
{
 ""notification_content"" : {

    ""name"" : ""Pokemon Found"",
    ""title"" : ""notify_title"",
    ""body"" : ""notify_body""
  },
    ""notification_target"" : {
    ""type"" : ""devices_target"",
    ""devices"" : [""device_id""]
    }
}";

            while (true)
            {
                var lastPoke = context.Pokemon.OrderBy(p => p.idValue, OrderByDirection.Descending).FirstOrDefault()?.idValue ?? 0;
                await ServiceLayer.SharedInstance.LoadData(lastPoke);
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
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
                using (var client = new HttpClient())
                {
                    foreach (var dev in context.Devices)
                    {
                        var toNotify = ServiceLayer.SharedInstance.Pokemon.Where(p => dev.NotifyPokemon.Contains(p.pokemon_id));
                        foreach (var p in toNotify)
                        {
                            var pLoc = new GeoCoordinate(p.lat, p.lon);
                            var dLoc = new GeoCoordinate(dev.LocationLat, dev.LocationLon);
                            var dist = pLoc.GetDistanceTo(dLoc) * 0.00062137;
                            var content = notifyContent.Replace("notify_title", $"{p.name} Found!");
                            content = content.Replace("notify_body", $"{p.name} Found {dist.ToString("F1")} miles away!");
                            var strContent = new StringContent(content, Encoding.UTF8, "application/json");
                            strContent.Headers.Add("X-API-Token", config.AppCenterToken);
                            try
                            {
                                var response = client.PostAsync("https://appcenter.ms/api/v0.1/apps/zerogeek/Omaha-PG-Map/push/notifications", strContent);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Error sending push notification for device {dev.Id}");
                            }
                        }
                        Console.WriteLine($"Sent {toNotify.Count()} notifications for device {dev.Id}");
                    }
                }
                ServiceLayer.SharedInstance.Pokemon.RemoveRange(0, ServiceLayer.SharedInstance.Pokemon.Count());
                Console.WriteLine("Added pokemon to database!");
                await Task.Delay(20000);
            }
        }
    }
}
 