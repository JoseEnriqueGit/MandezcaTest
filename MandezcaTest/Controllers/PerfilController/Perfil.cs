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
    public class PerfilController : ControllerBase
    {
        private readonly DataBaseContext dbContext;

        public PerfilController(DataBaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Perfil>> Get()
        {
            IEnumerable<Perfil> result = dbContext.Perfil.ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<Perfil> Get(int id)
        {
            Perfil result = dbContext.Perfil.Find(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<Perfil> Post(Perfil perfil)
        {
            dbContext.Perfil.Add(perfil);
            dbContext.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = perfil.PerfilId }, perfil);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Perfil updatedPerfil)
        {
            try
            {
                var existingPerfil = dbContext.Perfil.FirstOrDefault(p => p.PerfilId == id);

                if (existingPerfil == null)
                {
                    return NotFound("Perfil no encontrado.");
                }

                // Solo actualizamos las propiedades necesarias del perfil
                existingPerfil.PerfilTitle = updatedPerfil.PerfilTitle ?? existingPerfil.PerfilTitle;
                existingPerfil.PerfilDescription = updatedPerfil.PerfilDescription ?? existingPerfil.PerfilDescription;

                dbContext.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el perfil: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<Perfil> Delete(int id)
        {
            Perfil perfil = dbContext.Perfil.Find(id);
            if (perfil == null)
            {
                return NotFound();
            }
            dbContext.Perfil.Remove(perfil);
            dbContext.SaveChanges();
            return NoContent();
        }
    }
}
