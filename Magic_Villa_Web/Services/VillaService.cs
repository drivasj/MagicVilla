using Magic_Villa_Utilidad;
using Magic_Villa_Web.Models.DTO.Villa;
using Magic_Villa_Web.Services.IServices;
using Magic_Villa_Web.Models;

namespace Magic_Villa_Web.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private readonly IHttpClientFactory httpClient;
        private string _villaUrl;

        public VillaService(IHttpClientFactory httpClient,IConfiguration configuration): base(httpClient)
        {
            this.httpClient = httpClient;
            _villaUrl = configuration.GetValue<string>("ServiceUrls:API_URL");
        }

        public Task<T> Actualizar<T>(VillaUpdateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.PUT,
                Datos = dto,
                Url = _villaUrl + "/api/v1/Villa/" + dto.Id,
                Token = token
            });
        }

        public Task<T> Crear<T>(VillaCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.POST,
                Datos = dto,
                Url = _villaUrl + "/api/v1/Villa",
                Token = token
            });
        }

        public Task<T> Obtener<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _villaUrl + "/api/v1/Villa/" + id,
                Token = token
            });
        }

        public Task<T> ObtenerTodos<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _villaUrl + "/api/v1/Villa",
                Token = token
            });
        }

        /// <summary>
        /// Obtener Todos Páginado
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<T> ObtenerTodosPaginado<T>(string token, int pageNumber =1, int pageSize = 4)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _villaUrl + "/api/v1/Villa/VillasPaginado",
                Token = token,
                Parametros = new Parametros() { PageNumber = pageNumber,PageSize = pageSize }
            });
        }


        public Task<T> Remover<T>(int id, string token )
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.DELETE,
                Url = _villaUrl + "/api/v1/Villa/" + id,
                Token = token
            });

        }
    }
}
