using MagicVilla_API.Modelos.Entidad;

namespace MagicVilla_API.Modelos.DTO.Usuarios
{
    public class LoginResponseDTO
    {
        public UsuarioDTO Usuario { get; set; }

        public string Token { get; set; }

        //public string Rol { get; set; }
    }
}
