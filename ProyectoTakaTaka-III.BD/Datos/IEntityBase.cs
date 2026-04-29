using ProyectoTakaTaka_III.Shared.ENUM;

namespace ProyectoTakaTaka_III.BD.Datos
{
    public interface IEntityBase
    {
        EnumEstadoRegistro EstadoRegistro { get; set; }
        int Id { get; set; }
    }
}