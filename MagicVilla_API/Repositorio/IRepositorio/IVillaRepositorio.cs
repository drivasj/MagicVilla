using MagicVilla_API.Datos;
using MagicVilla_API.Modelos.Entidad;

namespace MagicVilla_API.Repositorio.IRepositorio
{
    public interface IVillaRepositorio :IRepositorio<Villa>
    {
        Task<Villa> Actualizar(Villa entidad);
    }
}
