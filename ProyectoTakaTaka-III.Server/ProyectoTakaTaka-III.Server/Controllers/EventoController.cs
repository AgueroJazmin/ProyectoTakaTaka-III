using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoTakaTaka_III.BD.Datos.Entity;
using ProyectoTakaTaka_III.Repositorio.Repositorios;
using ProyectoTakaTaka_III.Shared.DTO;
using static ProyectoTakaTaka_III.Server.Client.Pages.Evento.EventoCrear;

namespace ProyectoTakaTaka_III.Server.Controllers
{
    [ApiController]
    [Route("api/Evento")]
    public class EventoController : ControllerBase
    {
        private readonly IRepositorioEvento repositorio;

        public EventoController(IRepositorioEvento repositorio)
        {
            this.repositorio = repositorio;
        }

        //api/Evento/ListadoEvento
        //IaActionResult es el resultado del get que devuelve una colecccion o una lista de tipo string con 3 datos
        [HttpGet("ListadoEvento")]
        public async Task<ActionResult<List<ListadoEventoDTO>>> GetListadoE()
        {
            var lista = await repositorio.SelectListadoEventos();

            return Ok(lista);
        }

        [HttpGet("CantidadEvento")]
        public async Task<ActionResult<List<EventoCantidadDTO>>> GetCantidadE()
        {
            var lista = await repositorio.SelectListadoEventos();

            return Ok(lista);
        }

        [HttpPost]
        public async Task<ActionResult<int>> PostCrear(EventoCrearDTO DTO)
        {
            try
            {
                var id = await repositorio.InsertarEvento(DTO);

                return Ok(id);
            }
            catch (Exception e)
            {
                var explicate = e.InnerException?.Message ?? e.Message;

                return BadRequest(new
                {
                    mensaje = $"Error al crear el evento: {explicate}"
                });
            }
        }

        [HttpPost("CrearCompleto")]
        public async Task<ActionResult<int>> CrearCompleto(EventoCrearCompletoDTO dto)
        {
            try
            {
                var id = await repositorio.InsertarEventoCompleto(dto);

                return Ok(id);
            }
            catch (Exception ex)
            {
                var detalle = ex.InnerException?.Message ?? ex.Message;

                return BadRequest(new
                {
                    mensaje = $"Error al crear el evento: {detalle}"
                });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var eliminar = await repositorio.BorrarEvento(id);

            if (!eliminar)
            {
                return NotFound(new
                {
                    mensaje = $"No se encontró el evento {id}"
                });
            }

            return Ok(new
            {
                mensaje = $"Evento {id} eliminado correctamente"
            });
        }

        [HttpGet("PorMes")]
        public async Task<ActionResult<List<EventoCantidadDTO>>> PorMes(int mes, int año)
        {
            var lista = await repositorio.SelectEventosPorMes(mes, año);

            return Ok(lista);
        }

        [HttpGet("PorFecha")]
        public async Task<ActionResult<List<HorarioDiaDTO>>> PorFecha(string fecha)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fecha))
                {
                    return BadRequest(new
                    {
                        mensaje = "Fecha no proporcionada"
                    });
                }

                DateOnly f;

                if (!DateOnly.TryParseExact(
                    fecha,
                    new[] { "yyyy-MM-dd", "dd/MM/yyyy", "dd-MM-yyyy" },
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out f))
                {
                    return BadRequest(new
                    {
                        mensaje = $"Formato de fecha inválido: {fecha}"
                    });
                }

                var lista = await repositorio.SelectHorariosPorFecha(f);

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
        }
    }
}
