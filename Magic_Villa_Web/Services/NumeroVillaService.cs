using Magic_Villa_Utilidad;
using Magic_Villa_Web.Models.DTO.Villa;
using Magic_Villa_Web.Services.IServices;
using Magic_Villa_Web.Models;
using Magic_Villa_Web.Models.DTO.NumeroVilla;

namespace Magic_Villa_Web.Services
{
    public class NumeroVillaService : BaseService, INumeroVillaService
    {
        private readonly IHttpClientFactory httpClient;
        private string _villaUrl;

        public NumeroVillaService(IHttpClientFactory httpClient,IConfiguration configuration): base(httpClient)
        {
            this.httpClient = httpClient;
            _villaUrl = configuration.GetValue<string>("ServiceUrls:API_URL");
        }

        public Task<T> Actualizar<T>(NumeroVillaUpdateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.PUT,
                Datos = dto,
                Url = _villaUrl + "/api/v1/NumeroVilla/" + dto.VillaNo,
                Token = token
            });
        }

        public Task<T> Crear<T>(NumeroVillaCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.POST,
                Datos = dto,
                Url = _villaUrl + "/api/v1/NumeroVilla",
                Token = token
            });
        }

        public Task<T> Obtener<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _villaUrl + "/api/v1/NumeroVilla/" + id,
                Token = token
            });
        }

        public Task<T> ObtenerTodos<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _villaUrl + "/api/v1/NumeroVilla",
                Token = token
            });
        }

        //public Task<T> ObtenerTodosPaginado<T>(string token, int pageNumber = 1, int pageSize = 4)
        //{
        //}

        public Task<T> Remover<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.DELETE,
                Url = _villaUrl + "/api/v1/NumeroVilla/" + id,
                Token = token
            });

        }
    }
}
