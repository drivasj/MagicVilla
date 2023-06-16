using Magic_Villa_Web.Models.DTO.Villa;

namespace Magic_Villa_Web.Services.IServices
{
    public interface IVillaService
    {
        //Definimos todos los metodos que vamos a utilizar

        Task<T> ObtenerTodos<T>();
       // Task<T> ObtenerTodosPaginado<T>(string token, int pageNumber = 1, int pageSize = 4);
        Task<T> Obtener<T>(int id);
        Task<T> Crear<T>(VillaCreateDTO dto);
        Task<T> Actualizar<T>(VillaUpdateDTO dto);
        Task<T> Remover<T>(int id);
    }
}
