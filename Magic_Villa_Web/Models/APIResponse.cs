using System.Net;

namespace Magic_Villa_Web.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode {  get; set; }
        public bool IsExitoso { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public object Resultado { get; set; }
    }
}
