using Magic_Villa_Utilidad;
using Magic_Villa_Web.Models;
using Magic_Villa_Web.Models.DTO.Usuario;
using Magic_Villa_Web.Services.IServices;

namespace Magic_Villa_Web.Services
{
    public class UsuarioService : BaseService, IUsuarioService
    {
        private readonly IHttpClientFactory httpClient;
        private readonly IConfiguration configuration;
        private string villaUrl;

        public UsuarioService(IHttpClientFactory httpClient, IConfiguration configuration):base(httpClient)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
            villaUrl = configuration.GetValue<string>("ServiceUrls:API_URL");
        }
        public Task<T> Login<T>(LoginRequestDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.POST,
                Datos = dto,
                Url = villaUrl + "/api/usuario/login"
            });
        }

        public Task<T> Registrar<T>(RegistroRequestDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.POST,
                Datos = dto,
                Url = villaUrl + "/api/usuario/registrar"
            });
        }
    }
}
