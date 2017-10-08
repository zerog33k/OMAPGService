using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OMAPGService.Models;

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

            _context.Devices.Add(value);
			_context.SaveChanges();

            return CreatedAtRoute("GetDevice", new { id = value.Id }, value);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
