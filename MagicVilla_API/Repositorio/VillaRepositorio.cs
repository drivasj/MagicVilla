using MagicVilla_API.Datos;
using MagicVilla_API.Modelos.Entidad;
using MagicVilla_API.Repositorio.IRepositorio;

namespace MagicVilla_API.Repositorio
{
    public class VillaRepositorio : Repositorio<Villa>, IVillaRepositorio
    {
        private readonly ApplicationDbContext dbContext;

        public VillaRepositorio(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Villa> Actualizar(Villa entidad)
        {
            entidad.FechaCreacion = entidad.FechaCreacion;
            entidad.FechaActualizacion = DateTime.Now;
            dbContext.Villas.Update(entidad);
            await dbContext.SaveChangesAsync();
            return entidad;
        }
    }
}
