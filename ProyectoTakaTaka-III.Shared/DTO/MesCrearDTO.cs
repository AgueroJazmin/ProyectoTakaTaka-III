using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka_III.Shared.DTO
{
    public class MesCrearDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del mes es obligatorio")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string MesHabilitado { get; set; } = "";

        [Required(ErrorMessage = "El año es obligatorio")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Año { get; set; } = "";

        [Required(ErrorMessage = "El estado es obligatorio")]
        public int EstadoRegistro { get; set; }
    }
}
