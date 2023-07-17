using MandezcaTest.Database;
using MandezcaTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace MandezcaTest.Controllers.ClientController
{
    [ApiController]
    [Route("url/mnt/[controller]")]
    public class PostFullData: ControllerBase
    {
        private readonly DataBaseContext dbContext;

        public PostFullData(DataBaseContext dbContext)
        {
            this.dbContext = dbContext;
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
    }
}
