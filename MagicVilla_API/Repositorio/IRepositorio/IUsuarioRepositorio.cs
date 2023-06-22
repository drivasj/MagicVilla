using MagicVilla_API.Modelos.DTO.Usuarios;
using MagicVilla_API.Modelos.Entidad;

namespace MagicVilla_API.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        // Metodos

        bool IsUsuarioUnico(string userName);

        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);

        Task<UsuarioDTO> Registrar(RegistroRequestDTO registroRequestDTO);
    }
}
