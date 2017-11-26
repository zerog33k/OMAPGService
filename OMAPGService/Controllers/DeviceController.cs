using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMAPGServiceData.Models;

namespace OMAPGService.Controllers
{
    [Route("api/[controller]")]
    public class DeviceController : Controller
    {
        private readonly OMAPGContext _context;

        public DeviceController(OMAPGContext context)
        {
            _context = context;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Device> Get()
        {
            return _context.Devices;
        }

		[HttpGet("{id}", Name = "GetDevice")]
		public IActionResult GetById(long id)
		{
            var item = _context.Devices.FirstOrDefault(t => t.Id == id);
			if (item == null)
			{
				return NotFound();
			}
			return new ObjectResult(item);
		}



        [HttpPut]
        public IActionResult Create([FromBody]Device value)
        {
            if (value == null)
			{
				return BadRequest();
			}

            if(_context.Devices.Where(d => d.DeviceId == value.DeviceId).Any())
            {
                var dev = _context.Devices.Where(d => d.DeviceId == value.DeviceId).First();
                dev.LocationLat = value.LocationLat;
                dev.LocationLon = value.LocationLon;
                dev.NotifyPokemonStr = value.NotifyPokemonStr;
                dev.NotifyEnabled = value.NotifyEnabled;
                dev.DistanceAlert = value.DistanceAlert;
                _context.Entry(dev).State = EntityState.Modified;
                _context.SaveChanges();
                Console.WriteLine($"Device {dev.Id} updated");
                return CreatedAtRoute("GetDevice", new { id = dev.Id }, dev);
            } else
            {
                value.CreatedAt = DateTime.UtcNow;
                _context.Devices.Add(value);
                _context.SaveChanges();
                Console.WriteLine($"Device {value.Id} created");
                return CreatedAtRoute("GetDevice", new { id = value.Id }, value);
            }

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
