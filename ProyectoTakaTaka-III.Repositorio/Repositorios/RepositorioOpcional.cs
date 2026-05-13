using Microsoft.EntityFrameworkCore;
using ProyectoTakaTaka_III.BD.Datos;
using ProyectoTakaTaka_III.BD.Datos.Entity;
using ProyectoTakaTaka_III.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTakaTaka_III.Repositorio.Repositorios
{
    public class RepositorioOpcional : Repositorio<Opcional>, IRepositorioOpcional
    {
        private readonly MiDbContext context;

        public RepositorioOpcional(MiDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<List<ListadoOpcionalDTO>> SelectListadoOpcionales()
        {
            return await context.Opcionales
                .Select(o => new ListadoOpcionalDTO
                {
                    Id = o.Id,
                    NomOpcional = o.NomOpcional,
                    Precio = o.Precio
                })
                .ToListAsync();
        }

        public async Task<ListadoOpcionalDTO?> SelectOpcionalById(int id)
        {
            return await context.Opcionales
                .Where(o => o.Id == id)
                .Select(o => new ListadoOpcionalDTO
                {
                    Id = o.Id,
                    NomOpcional = o.NomOpcional,
                    Precio = o.Precio
                })
                .FirstOrDefaultAsync();
        }

        public async Task InsertOpcional(CrearOpcionalDTO dto)
        {
            var existe = await context.Opcionales.AnyAsync(o => o.NomOpcional == dto.NomOpcional);

            if (existe)
            {
                throw new Exception("Ya existe un opcional con ese nombre");
            }

            var entidad = new Opcional
            {
                NomOpcional = dto.NomOpcional,
                Precio = dto.Precio
            };

            context.Opcionales.Add(entidad);

            await context.SaveChangesAsync();
            /* var entidad = new Opcional
             {
                 NomOpcional = dto.NomOpcional,
                 Precio = dto.Precio
             };

             context.Opcionales.Add(entidad);
             await context.SaveChangesAsync();*/
        }

        public async Task UpdateOpcional(int id, CrearOpcionalDTO dto)
        {
            var entidad = await context.Opcionales.FindAsync(id);
            if (entidad == null) return;

            entidad.NomOpcional = dto.NomOpcional;
            entidad.Precio = dto.Precio;

            await context.SaveChangesAsync();
        }

        public async Task DeleteOpcional(int id)
        {
            var usado = await context.EventosOpcionales.AnyAsync(eo => eo.OpcionalId == id);

            if (usado)
            {
                throw new Exception(
                    "No se puede eliminar el opcional porque está asociado a eventos");
            }

            var entidad = await context.Opcionales.FindAsync(id);

            if (entidad == null)
            {
                throw new Exception("Opcional no encontrado");
            }

            context.Opcionales.Remove(entidad);

            await context.SaveChangesAsync();
            /*var entidad = await context.Opcionales.FindAsync(id);
            if (entidad == null) return;

            context.Opcionales.Remove(entidad);
            await context.SaveChangesAsync();*/
        }
    }
}
