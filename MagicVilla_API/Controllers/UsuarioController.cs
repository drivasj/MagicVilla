using Azure;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.DTO.Usuarios;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepositorio usuarioRepositorio;
        private APIResponse response;

        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            this.usuarioRepositorio = usuarioRepositorio;
            response = new APIResponse();
        }

        [HttpPost("login")] // api/usuario/login 
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO modelo)
        {
            var loginResponse = await usuarioRepositorio.Login(modelo);

            if (loginResponse.Usuario == null || String.IsNullOrEmpty(loginResponse.Token))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsExitoso = false;
                response.ErrorMessages.Add("UserName o Password son Incorrectos");
                return BadRequest(response);
            }
            response.IsExitoso = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Resultado = loginResponse;
           
            return Ok(response);
        }

        [HttpPost("registrar")] // api/usuario/login 
        public async Task<IActionResult> Registrar([FromBody] RegistroRequestDTO modelo)
        {
            bool IsUsuarioUnico = usuarioRepositorio.IsUsuarioUnico(modelo.UserName);

            if (!IsUsuarioUnico)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsExitoso = false;
                response.ErrorMessages.Add("Usuario ya existe!");
                return BadRequest(response);
            }
            var usuario = await usuarioRepositorio.Registrar(modelo);
            if (usuario == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsExitoso = false;
                response.ErrorMessages.Add("Error al registrar usuario!");
                return BadRequest(response);
            }
            response.StatusCode = HttpStatusCode.OK;
            response.IsExitoso = true;
            return Ok(response);
        }
    }
}
