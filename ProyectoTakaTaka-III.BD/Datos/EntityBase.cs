using ProyectoTakaTaka_III.Shared.ENUM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka_III.BD.Datos
{
    public class EntityBase : IEntityBase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        public EnumEstadoRegistro EstadoRegistro { get; set; } = EnumEstadoRegistro.EnGrabacion;
    }
}
