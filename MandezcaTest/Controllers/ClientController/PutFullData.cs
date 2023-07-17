using MandezcaTest.Database;
using MandezcaTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace MandezcaTest.Controllers.ClientController
{
    [ApiController]
    [Route("url/mnt/[controller]")]
    public class PutFullData: ControllerBase
    {
        private readonly DataBaseContext dbContext;

        public PutFullData(DataBaseContext dbContext)
        {
            this.dbContext = dbContext;
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
