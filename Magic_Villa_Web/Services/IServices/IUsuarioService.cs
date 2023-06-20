using Magic_Villa_Web.Models.DTO.Usuario;

namespace Magic_Villa_Web.Services.IServices
{
    public interface IUsuarioService
    {
        // mthos

        Task<T> Login<T>(LoginRequestDTO dto);
        Task<T> Registrar<T>(RegistroRequestDTO dto);

    }

}
