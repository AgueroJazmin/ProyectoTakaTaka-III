using ProyectoTakaTaka_III.Shared.DTO;

namespace ProyectoTakaTaka_III.Repositorio.Repositorios
{
    public interface IRepositorioCumpleanero
    {
        Task DeleteCumpleanero(int id);
        Task InsertCumpleanero(CumpleaneroCrearDTO dto);
        Task<CumpleaneroListadoDTO?> SelectCumpleaneroById(int id);
        Task<List<CumpleaneroListadoDTO>> SelectListadoCumpleaneros();
        Task UpdateCumpleanero(int id, CumpleaneroCrearDTO dto);
    }
}