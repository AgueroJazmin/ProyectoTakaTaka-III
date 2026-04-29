using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka_III.Shared.DTO
{
    public class HorarioListadoDTO
    {
        public int HorarioId { get; set; }
        public TimeOnly HInicio { get; set; }
        public TimeOnly HFin { get; set; }
        public bool Disponible { get; set; }
        public string? Nota { get; set; }
    }
}
