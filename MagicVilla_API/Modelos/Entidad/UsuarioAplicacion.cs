using Microsoft.AspNetCore.Identity;

namespace MagicVilla_API.Modelos.Entidad
{
    public class UsuarioAplicacion: IdentityUser
    {
        public string Nombres { get; set; }
    }
}
