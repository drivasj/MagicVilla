using AutoMapper;
using Magic_Villa_Utilidad;
using Magic_Villa_Web.Models;
using Magic_Villa_Web.Models.DTO.Villa;
using Magic_Villa_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Magic_Villa_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVillaService villaService;
        private readonly IMapper mapper;

        public HomeController(ILogger<HomeController> logger,IVillaService villaService,IMapper mapper)
        {
            _logger = logger;
            this.villaService = villaService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<VillaDTO> villaList = new();

            var response = await villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));

            if (response != null)
            {
                villaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Resultado));
            }
            return View(villaList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}