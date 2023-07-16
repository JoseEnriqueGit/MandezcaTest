using Microsoft.AspNetCore.Mvc;
using MandezcaTest.Database;
using MandezcaTest.Models;
using Microsoft.EntityFrameworkCore;

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

        //[HttpGet("{id}")]
        //public ActionResult<Client> Get(int id)
        //{
        //    Client result = dbContext.Client.Find(id);
        //    if (result == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(result);
        //}

        [HttpGet("{clientId}")]
        public ActionResult<IEnumerable<FullData>> GetAllData(int clientId)
        {
            var data = dbContext.Client
                .Where(client => client.client_id == clientId)
                .Join(
                    dbContext.Perfil,
                    client => client.client_id,
                    perfil => perfil.ClientId,
                    (client, perfil) => new { Client = client, Perfil = perfil }
                )
                .Join(
                    dbContext.Address,
                    cp => cp.Client.client_id,
                    address => address.ClientId,
                    (cp, address) => new FullData
                    {
                        ClientId = cp.Client.client_id,
                        ClientName = cp.Client.client_name,
                        ClientEmail = cp.Client.client_email,
                        ClientPhone = cp.Client.client_phone,
                        PerfilId = cp.Perfil.PerfilId,
                        PerfilTitle = cp.Perfil.PerfilTitle,
                        PerfilDescription = cp.Perfil.PerfilDescription,
                        AddressId = address.AddressId,
                        AddressLine = address.AddressLine,
                        City = address.City,
                        State = address.State,
                        Country = address.Country,
                        PostalCode = address.PostalCode
                    }
                )
                .ToList();

            return Ok(data);
        }


        [HttpGet("all-data")]
        public ActionResult<IEnumerable<FullData>> GetAllData()
        {
            var data = dbContext.Client
                .Join(
                    dbContext.Perfil,
                    client => client.client_id,
                    perfil => perfil.ClientId,
                    (client, perfil) => new { Client = client, Perfil = perfil }
                )
                .Join(
                    dbContext.Address,
                    cp => cp.Client.client_id,
                    address => address.ClientId,
                    (cp, address) => new FullData
                    {
                        ClientId = cp.Client.client_id,
                        ClientName = cp.Client.client_name,
                        ClientEmail = cp.Client.client_email,
                        ClientPhone = cp.Client.client_phone,
                        PerfilId = cp.Perfil.PerfilId,
                        PerfilTitle = cp.Perfil.PerfilTitle,
                        PerfilDescription = cp.Perfil.PerfilDescription,
                        AddressId = address.AddressId,
                        AddressLine = address.AddressLine,
                        City = address.City,
                        State = address.State,
                        Country = address.Country,
                        PostalCode = address.PostalCode
                    }
                )
                .ToList();

            return Ok(data);
        }

        [HttpDelete("{clientId}")]
        public IActionResult DeleteClient(int clientId)
        {
            var client = dbContext.Client.FirstOrDefault(c => c.client_id == clientId);

            if (client == null)
            {
                return NotFound(); // Retorna código 404 si el cliente no existe
            }

            // Elimina los registros de la tabla "Perfil" asociados al cliente
            var perfils = dbContext.Perfil.Where(p => p.ClientId == clientId).ToList();
            dbContext.Perfil.RemoveRange(perfils);

            // Elimina los registros de la tabla "Address" asociados al cliente
            var addresses = dbContext.Address.Where(a => a.ClientId == clientId).ToList();
            dbContext.Address.RemoveRange(addresses);

            // Elimina el cliente de la tabla "Client"
            dbContext.Client.Remove(client);

            // Guarda los cambios en la base de datos
            dbContext.SaveChanges();

            return NoContent(); // Retorna código 204 para indicar que la eliminación fue exitosa
        }






    }
}
