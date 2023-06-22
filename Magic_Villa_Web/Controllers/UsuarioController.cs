
using Magic_Villa_Utilidad;
using Magic_Villa_Web.Models;
using Magic_Villa_Web.Models.DTO.Usuario;
using Magic_Villa_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Magic_Villa_Web.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            this.usuarioService = usuarioService;
        }

        /// <summary>
        /// Login Vista
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO modelo)
        {
            var response = await usuarioService.Login<APIResponse>(modelo);

            if (response != null && response.IsExitoso == true)
            {
                LoginResponseDTO loginResponse = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Resultado));

                //Leer Token
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(loginResponse.Token);


                //Claims
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(c => c.Type== "unique_name").Value)); // obtenemos el ususario
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(c => c.Type == "role").Value));// obtenemos el rol
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,  principal);

                // Session
                HttpContext.Session.SetString(DS.SessionToken, loginResponse.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                return View(modelo);
            }


            return View();
        }

        /// <summary>
        /// Registro Vista
        /// </summary>
        /// <returns></returns>
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task  <IActionResult> Registrar(RegistroRequestDTO modelo)
        {
            var response = await usuarioService.Registrar<APIResponse>(modelo);

            if(response !=null && response.IsExitoso)
            {
                return RedirectToAction("login");
            }

            return View();
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        public async Task <IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(DS.SessionToken, "");

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Acceso Denegado
        /// </summary>
        /// <returns></returns>
        public IActionResult AccesoDenegado()
        {
            return View();
        }
    }
}
