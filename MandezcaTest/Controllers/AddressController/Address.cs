using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using MandezcaTest.Database;
using MandezcaTest.Models;

namespace MandezcaTest.Controllers
{
    [ApiController]
    [Route("url/mnt/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly DataBaseContext dbContext;

        public AddressController(DataBaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Address>> Get()
        {
            IEnumerable<Address> result = dbContext.Address.ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<Address> Get(int id)
        {
            Address result = dbContext.Address.Find(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<Address> Post(Address address)
        {
            dbContext.Address.Add(address);
            dbContext.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = address.AddressId }, address);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Address updatedAddress)
        {
            try
            {
                var existingAddress = dbContext.Address.FirstOrDefault(a => a.AddressId == id);

                if (existingAddress == null)
                {
                    return NotFound("Dirección no encontrada.");
                }

                // Solo actualizamos las propiedades de la dirección
                existingAddress.AddressLine = updatedAddress.AddressLine ?? existingAddress.AddressLine;
                existingAddress.City = updatedAddress.City ?? existingAddress.City;
                existingAddress.State = updatedAddress.State ?? existingAddress.State;
                existingAddress.Country = updatedAddress.Country ?? existingAddress.Country;
                existingAddress.PostalCode = updatedAddress.PostalCode ?? existingAddress.PostalCode;

                dbContext.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la dirección: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<Address> Delete(int id)
        {
            Address address = dbContext.Address.Find(id);
            if (address == null)
            {
                return NotFound();
            }
            dbContext.Address.Remove(address);
            dbContext.SaveChanges();
            return NoContent();
        }
    }
}
