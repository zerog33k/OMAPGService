using System;
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
            var lastPoke = context.Pokemon.OrderBy(p => p.idValue, OrderByDirection.Descending).FirstOrDefault()?.idValue ?? 0;
            ServiceLayer.SharedInstance.LastId = lastPoke;
            await ServiceLayer.SharedInstance.LoadData();
            Console.WriteLine($"Loaded {ServiceLayer.SharedInstance.Pokemon.Count} Pokemon, {ServiceLayer.SharedInstance.Gyms.Count} Gyms, and {ServiceLayer.SharedInstance.Raids.Count} Raids!");

            foreach(var p in ServiceLayer.SharedInstance.Pokemon)
            {
                var pokeInDB = context.Pokemon.Find(new object[] { p.idValue });
                if (pokeInDB == null)
                {
                    context.Pokemon.Add(p);
                } else {
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
            Console.WriteLine("Added pokemon to database!");

            return;
        }
    }
}
 