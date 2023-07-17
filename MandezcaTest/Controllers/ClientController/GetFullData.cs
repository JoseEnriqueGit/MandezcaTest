using MandezcaTest.Database;
using MandezcaTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace MandezcaTest.Controllers.ClientController
{
    [ApiController]
    [Route("url/mnt/[controller]")]
    public class GetFullData: ControllerBase
    {
        private readonly DataBaseContext dbContext;

        public GetFullData(DataBaseContext dbContext)
        {
            this.dbContext = dbContext;
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
    }
}
