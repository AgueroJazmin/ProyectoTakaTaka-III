using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoTakaTaka_III.BD.Datos.Entity;
using ProyectoTakaTaka_III.Repositorio.Repositorios;
using ProyectoTakaTaka_III.Shared.DTO;

namespace ProyectoTakaTaka_III.Server.Controllers
{
    [ApiController]
    [Route("api/Opcionales")]
    public class OpcionalController : ControllerBase
    {
        private readonly IRepositorioOpcional repositorio;

        public OpcionalController(IRepositorioOpcional repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet("ListadoOpcional")]
        public async Task<ActionResult<List<ListadoOpcionalDTO>>> Get()
        {
            var lista = await repositorio.SelectListadoOpcionales();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ListadoOpcionalDTO>> Get(int id)
        {
            var opcional = await repositorio.SelectOpcionalById(id);

            if (opcional == null)
                return NotFound(new { mensaje = $"No se encontró el opcional {id}" });

            return Ok(opcional);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Post(CrearOpcionalDTO dto)
        {
            await repositorio.InsertOpcional(dto);

            return Ok(new
            {
                mensaje = "Opcional creado correctamente"
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, CrearOpcionalDTO dto)
        {
            await repositorio.UpdateOpcional(id, dto);

            return Ok(new
            {
                mensaje = "Opcional actualizado correctamente"
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await repositorio.DeleteOpcional(id);

            return Ok(new
            {
                mensaje = "Opcional eliminado correctamente"
            });
        }
    }
}
