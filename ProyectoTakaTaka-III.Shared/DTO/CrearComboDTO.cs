using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka_III.Shared.DTO
{
    public class CrearComboDTO
    {
        [Required(ErrorMessage = "El nombre del combo es obligatorio")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string NomCombo { get; set; } = "";

        [Range(1, 99999999,
           ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }
    }
}
