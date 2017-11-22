﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Newtonsoft.Json;
using OMAPGMap;
using OMAPGServiceData.Models;

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
                ServiceLayer.SharedInstance.Pokemon.RemoveRange(0, ServiceLayer.SharedInstance.Pokemon.Count());
                Console.WriteLine("Added pokemon to database!");
                await Task.Delay(20000);
            }
        }
    }
}
 