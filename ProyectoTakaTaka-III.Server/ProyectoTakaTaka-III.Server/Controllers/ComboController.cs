using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoTakaTaka_III.BD.Datos.Entity;
using ProyectoTakaTaka_III.Repositorio.Repositorios;
using ProyectoTakaTaka_III.Shared.DTO;

namespace ProyectoTakaTaka_III.Server.Controllers
{
    [ApiController]
    [Route("api/Combos")]
    public class ComboController : ControllerBase
    {
        private readonly IRepositorioCombo repositorio;

        public ComboController(IRepositorioCombo repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet("ListadoCombos")]
        public async Task<ActionResult<List<ListadoComboDTO>>> Get()
        {
            var lista = await repositorio.SelectListadoCombos();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ListadoComboDTO>> Get(int id)
        {
            var combo = await repositorio.SelectComboById(id);

            if (combo == null)
                return NotFound(new { mensaje = $"No se encontró el combo {id}" });

            return Ok(combo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Post(CrearComboDTO dto)
        {
            await repositorio.InsertCombo(dto);

            return Ok(new
            {
                mensaje = "Combo creado correctamente"
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, CrearComboDTO dto)
        {
            await repositorio.UpdateCombo(id, dto);

            return Ok(new
            {
                mensaje = "Combo actualizado correctamente"
            });
        }
        //Antes tenia esto porque etaba usando el policy pero ahora que estoy usando el rol de admin, no necesito el policy, solo el rol de admin
        //[Authorize(Policy = "AdminOnly")]
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await repositorio.DeleteCombo(id);

            return Ok(new
            {
                mensaje = "Combo eliminado correctamente"
            });
        }
    }
}
