using MagicVilla_API.Datos;
using MagicVilla_API.Modelos.Entidad;
using MagicVilla_API.Repositorio.IRepositorio;

namespace MagicVilla_API.Repositorio
{
    public class NumeroVillaRepositorio : Repositorio<NumeroVilla>, INumeroVillaRepositorio
    {
        private readonly ApplicationDbContext dbContext;

        public NumeroVillaRepositorio(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<NumeroVilla> Actualizar(NumeroVilla entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            dbContext.NumeroVillas.Update(entidad);
            await dbContext.SaveChangesAsync();
            return entidad;
        }
    }
}
