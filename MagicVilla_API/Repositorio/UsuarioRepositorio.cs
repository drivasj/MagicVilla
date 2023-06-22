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

namespace MagicVilla_API.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;
        private readonly UserManager<UsuarioAplicacion> userManager;
        private readonly IMapper mapper;
        private readonly RoleManager<IdentityRole> roleManager;
        private string secretKey;

        public UsuarioRepositorio(ApplicationDbContext context, 
            IConfiguration configuration,
            UserManager<UsuarioAplicacion> userManager,
            IMapper mapper,
            RoleManager<IdentityRole> roleManager )
        {
            this.context = context;
            this.configuration = configuration;
            this.userManager = userManager;
            this.mapper = mapper;
            this.roleManager = roleManager;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");   // Obtenemos el JWT del appsettings.json
        }

        public bool IsUsuarioUnico(string username)
        {
            var ususario = context.UsuariosAplicacion.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());
            if (ususario == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var usuario = await context.UsuariosAplicacion.FirstOrDefaultAsync(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

            bool isValido = await userManager.CheckPasswordAsync(usuario, loginRequestDTO.Password);

            if (usuario == null || isValido == false)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    Usuario = null
                };
            }
            // si el usuario existe generamos el JWT
            var roles = await userManager.GetRolesAsync(usuario);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new()
            {
                Token = tokenHandler.WriteToken(token),
                Usuario = mapper.Map<UsuarioDTO>(usuario)
              
            };
            return loginResponseDTO;
        }

        public async Task<UsuarioDTO> Registrar(RegistroRequestDTO registroRequestDTO)
        {
            UsuarioAplicacion usuario = new()
            {
                UserName = registroRequestDTO.UserName,
                Email = registroRequestDTO.UserName,
                NormalizedEmail = registroRequestDTO.UserName.ToUpper(),
                Nombres = registroRequestDTO.Nombres,

            };
            try
            {
                var resultado = await userManager.CreateAsync(usuario, registroRequestDTO.Password);
                if (resultado.Succeeded)
                {
                    if(!roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await roleManager.CreateAsync(new IdentityRole("admin"));
                        await roleManager.CreateAsync(new IdentityRole("cliente"));

                    }


                    await userManager.AddToRoleAsync(usuario, "admin");
                    var usuarioAp = context.UsuariosAplicacion.FirstOrDefault(u => u.UserName == registroRequestDTO.UserName);
                    return mapper.Map<UsuarioDTO>(usuarioAp);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return new UsuarioDTO();
        }
    }
}
