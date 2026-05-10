using Microsoft.AspNetCore.Mvc;
using ProyectoTakaTaka_III.Repositorio.Repositorios;
using ProyectoTakaTaka_III.Shared.DTO;
using Microsoft.EntityFrameworkCore;
using ProyectoTakaTaka_III.BD.Datos.Entity;

namespace ProyectoTakaTaka_III.Server.Controllers
{
    [ApiController]
    [Route("api/Horarios")]
    public class HorarioController : ControllerBase
    {
        private readonly IRepositorioHorario repositorio;

        public HorarioController(IRepositorioHorario repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet("ListadoHorarios")]
        public async Task<ActionResult<List<HorarioListadoDTO>>> Get()
        {
            var lista = await repositorio.SelectListadoHorarios();

            return Ok(lista);
        }

        [HttpGet("HorariosDisponibles")]
        public async Task<ActionResult<List<HorarioListadoDTO>>> GetDisponibles()
        {
            var lista = await repositorio.SelectHorariosDisponibles();

            return Ok(lista);
        }

        [HttpPost]
        public async Task<ActionResult> Post(HorarioCrearDTO dto)
        {
            try
            {
                var id = await repositorio.InsertHorario(dto);

                return Ok(new
                {
                    mensaje = $"Horario creado correctamente con ID {id}"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    mensaje = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, HorarioCrearDTO dto)
        {
            try
            {
                bool actualizado = await repositorio.UpdateHorario(id, dto);

                if (!actualizado)
                {
                    return NotFound(new
                    {
                        mensaje = $"No se encontró el horario {id}"
                    });
                }

                return Ok(new
                {
                    mensaje = $"Horario {id} actualizado correctamente"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    mensaje = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var ok = await repositorio.DeleteHorario(id);

            if (!ok)
            {
                return Conflict(new
                {
                    mensaje = $"No se puede eliminar el horario {id} porque está asignado a eventos"
                });
            }

            return Ok(new
            {
                mensaje = "Horario eliminado correctamente"
            });
        }
    }
}
