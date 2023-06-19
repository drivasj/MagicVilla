using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos.DTO.Usuarios;
using MagicVilla_API.Modelos.Entidad;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_API.Repositorio.IRepositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext context;
        private string secretKey;

        public UsuarioRepositorio(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");   // Obtenemos el JWT del appsettings.json
        }

        public bool IsUsuarioUnico(string username)
        {
           var ususario = context.Usuarios.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());
            if (ususario == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower() &&
                                                                          u.Password == loginRequestDTO.Password);

            if (usuario == null)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    Usuario = null
                };
            }
            // si el usuario existe generamos el JWT

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserName.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Rol)
                }),

                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new()
            {
                Token = tokenHandler.WriteToken(token),
                Usuario = usuario
            };
            return loginResponseDTO;
        }

        public async Task<Usuario> Registrar(RegistroRequestDTO registroRequestDTO)
        {
            Usuario usuario = new()
            {
                UserName = registroRequestDTO.UserName,
                Password = registroRequestDTO.Password,
                Nombres = registroRequestDTO.Nombres,
                Rol = registroRequestDTO.Rol
            };

            await context.Usuarios.AddAsync(usuario);
            await context.SaveChangesAsync();
            usuario.Password = "";
            return usuario;
        }
    }
}
