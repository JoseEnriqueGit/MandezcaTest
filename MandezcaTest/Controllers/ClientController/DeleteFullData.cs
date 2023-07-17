using MandezcaTest.Database;
using Microsoft.AspNetCore.Mvc;

namespace MandezcaTest.Controllers.ClientController
{
    [ApiController]
    [Route("url/mnt/[controller]")]
    public class DeleteFullData: ControllerBase
    {
        private readonly DataBaseContext dbContext;

        public DeleteFullData(DataBaseContext dbContext)
        {
            this.dbContext = dbContext;
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
