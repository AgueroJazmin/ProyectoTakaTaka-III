using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka_III.BD.Datos.Entity
{
    public class EventoOpcional : EntityBase
    {
        //clave foranea 
        public int OpcionalId { get; set; }
        public Opcional? Opcional { get; set; }
        //
        public int EventoId { get; set; }
        public Evento? Evento { get; set; }
    }
}
