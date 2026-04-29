using ProyectoTakaTaka_III.Shared.DTO;

namespace ProyectoTakaTaka_III.Repositorio.Repositorios
{
    public interface IRepositorioCombo
    {
        Task DeleteCombo(int id);
        Task InsertCombo(CrearComboDTO dto);
        Task<ListadoComboDTO?> SelectComboById(int id);
        Task<List<ListadoComboDTO>> SelectListadoCombos();
        Task UpdateCombo(int id, CrearComboDTO dto);
    }
}