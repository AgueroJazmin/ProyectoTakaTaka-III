using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka_III.Shared.DTO
{
    public class CumpleaneroListadoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public required DateOnly FechaNacimiento { get; set; }

        public int Edad { get; set; }
    }
}
