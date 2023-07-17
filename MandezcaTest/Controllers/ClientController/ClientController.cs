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

        [HttpPost]
        public IActionResult AddClient([FromBody] NewDataModel newClientData)
        {
            // Creamos un nuevo cliente con los datos proporcionados
            var newClient = new Client
            {
                client_name = newClientData.ClientName,
                client_email = newClientData.ClientEmail,
                client_phone = newClientData.ClientPhone
            };

            // Agregamos el nuevo cliente a la tabla "Client" y guardamos los cambios para generar el client_id
            dbContext.Client.Add(newClient);
            dbContext.SaveChanges();

            // Creamos un nuevo perfil con los datos proporcionados y el client_id generado
            var newPerfil = new Perfil
            {
                ClientId = newClient.client_id,
                PerfilTitle = newClientData.PerfilTitle,
                PerfilDescription = newClientData.PerfilDescription
            };

            // Agregamos el nuevo perfil a la tabla "Perfil"
            dbContext.Perfil.Add(newPerfil);

            // Creamos una nueva dirección con los datos proporcionados y el client_id generado
            var newAddress = new Address
            {
                ClientId = newClient.client_id,
                AddressLine = newClientData.AddressLine,
                City = newClientData.City,
                State = newClientData.State,
                Country = newClientData.Country,
                PostalCode = newClientData.PostalCode
            };

            // Agregamos la nueva dirección a la tabla "Address"
            dbContext.Address.Add(newAddress);

            // Guardamos los cambios para finalizar la transacción
            dbContext.SaveChanges();

            return Ok();
        }

        [HttpPut("{clientId}")]
        public IActionResult UpdateClient(int clientId, [FromBody] NewDataModel modifiedClientData)
        {
            var client = dbContext.Client.FirstOrDefault(c => c.client_id == clientId);

            if (client == null)
            {
                return NotFound(); // Retorna código 404 si el cliente no existe
            }

            bool isUpdated = false;

            // Actualizar campos del cliente si hay cambios
            if (modifiedClientData.ClientName != null && modifiedClientData.ClientName != client.client_name)
            {
                client.client_name = modifiedClientData.ClientName;
                isUpdated = true;
            }

            if (modifiedClientData.ClientEmail != null && modifiedClientData.ClientEmail != client.client_email)
            {
                client.client_email = modifiedClientData.ClientEmail;
                isUpdated = true;
            }

            if (modifiedClientData.ClientPhone != null && modifiedClientData.ClientPhone != client.client_phone)
            {
                client.client_phone = modifiedClientData.ClientPhone;
                isUpdated = true;
            }

            // Actualizar campos del perfil si hay cambios
            var perfil = dbContext.Perfil.FirstOrDefault(p => p.ClientId == clientId);
            if (perfil != null)
            {
                if (modifiedClientData.PerfilTitle != null && modifiedClientData.PerfilTitle != perfil.PerfilTitle)
                {
                    perfil.PerfilTitle = modifiedClientData.PerfilTitle;
                    isUpdated = true;
                }

                if (modifiedClientData.PerfilDescription != null && modifiedClientData.PerfilDescription != perfil.PerfilDescription)
                {
                    perfil.PerfilDescription = modifiedClientData.PerfilDescription;
                    isUpdated = true;
                }
            }

            // Actualizar campos de la dirección si hay cambios
            var address = dbContext.Address.FirstOrDefault(a => a.ClientId == clientId);
            if (address != null)
            {
                if (modifiedClientData.AddressLine != null && modifiedClientData.AddressLine != address.AddressLine)
                {
                    address.AddressLine = modifiedClientData.AddressLine;
                    isUpdated = true;
                }

                if (modifiedClientData.City != null && modifiedClientData.City != address.City)
                {
                    address.City = modifiedClientData.City;
                    isUpdated = true;
                }

                if (modifiedClientData.State != null && modifiedClientData.State != address.State)
                {
                    address.State = modifiedClientData.State;
                    isUpdated = true;
                }

                if (modifiedClientData.Country != null && modifiedClientData.Country != address.Country)
                {
                    address.Country = modifiedClientData.Country;
                    isUpdated = true;
                }

                if (modifiedClientData.PostalCode != null && modifiedClientData.PostalCode != address.PostalCode)
                {
                    address.PostalCode = modifiedClientData.PostalCode;
                    isUpdated = true;
                }
            }

            if (isUpdated)
            {
                dbContext.SaveChanges(); // Guardar cambios solo si hubo actualizaciones
            }

            return NoContent(); // Retorna código 204 para indicar éxito en la actualización
        }






    }
}
