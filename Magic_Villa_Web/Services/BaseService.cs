using Magic_Villa_Utilidad;
using Magic_Villa_Web.Models;
using Magic_Villa_Web.Services.IServices;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Web;

namespace Magic_Villa_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get ; set; }
        public IHttpClientFactory _httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
            _httpClient = httpClient;

        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = _httpClient.CreateClient("MagicAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                //  message.RequestUri = new Uri(apiRequest.Url);

                if (apiRequest.Parametros == null)
                {
                    message.RequestUri = new Uri(apiRequest.Url);
                }
                else
                {
                    // Armando URL: EJEMPLO  api/Villa/VillaPaginado/PageNumber=1&PageSize=4
                    var builder = new UriBuilder(apiRequest.Url);
                    var query = HttpUtility.ParseQueryString(builder.Query);
                    query["PageNumber"] = apiRequest.Parametros.PageNumber.ToString();
                    query["PageSize"] = apiRequest.Parametros.PageSize.ToString();
                    builder.Query = query.ToString();
                    string url = builder.ToString();      
                    message.RequestUri = new Uri(url);
                }


                if (apiRequest.Datos != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Datos),
                        Encoding.UTF8, "application/json");
                }

                switch (apiRequest.APITipo) 
                {
                    case DS.APITipo.POST:
                        message.Method = HttpMethod.Post; 
                        break;
                    case DS.APITipo.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case DS.APITipo.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default: 
                        message.Method = HttpMethod.Get;
                        break;                
                }
                HttpResponseMessage apiResponse = null;

                if(!string.IsNullOrEmpty(apiRequest.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiRequest.Token);
                }

                apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();

                try
                {
                    APIResponse response = JsonConvert.DeserializeObject<APIResponse>(apiContent);

                    if(response !=null &&(apiResponse.StatusCode == HttpStatusCode.BadRequest
                                       || apiResponse.StatusCode == HttpStatusCode.NotFound))
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.IsExitoso = false;
                        var res = JsonConvert.SerializeObject(response);
                        var obj = JsonConvert.DeserializeObject<T>(res);
                        return obj;
                    }
                }
                catch (Exception ex)
                {
                    var errorResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return errorResponse;
                }
                var APIresponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIresponse;
            }
            catch (Exception ex )
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string>
                    {
                        Convert.ToString(ex.Message)
                    },
                    IsExitoso = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var ResponseEX = JsonConvert.DeserializeObject<T>(res);
                return ResponseEX;
            }
        }
    }
}
