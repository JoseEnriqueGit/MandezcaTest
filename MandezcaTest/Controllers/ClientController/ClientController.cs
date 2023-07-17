using Microsoft.AspNetCore.Mvc;
using MandezcaTest.Database;
using MandezcaTest.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MandezcaTest.Controllers.ClientController
{
    [ApiController]
    [Route("url/mnt/[controller]")]
    public class ClientController: ControllerBase
    {
        private readonly DataBaseContext dbContext;

        public ClientController(DataBaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Client>> Get()
        {
            IEnumerable<Client> result = dbContext.Client.ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<Client> Get(int id)
        {
            Client result = dbContext.Client.Find(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<Client> Post(Client client)
        {
            dbContext.Client.Add(client);
            dbContext.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = client.client_id }, client);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Client updatedClient)
        {
            try
            {
                var existingClient = dbContext.Client.FirstOrDefault(c => c.client_id == id);

                if (existingClient == null)
                {
                    return NotFound("Cliente no encontrado.");
                }

                // Solo actualizamos las propiedades necesarias del cliente
                existingClient.client_name = updatedClient.client_name ?? existingClient.client_name;
                existingClient.client_email = updatedClient.client_email ?? existingClient.client_email;
                existingClient.client_phone = updatedClient.client_phone ?? existingClient.client_phone;

                dbContext.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el cliente: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<Client> Delete(int id)
        {
            Client client = dbContext.Client.Find(id);
            if (client == null)
            {
                return NotFound();
            }
            dbContext.Client.Remove(client);
            dbContext.SaveChanges();
            return NoContent();
        }

    }
}
