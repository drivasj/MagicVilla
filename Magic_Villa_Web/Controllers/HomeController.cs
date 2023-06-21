using AutoMapper;
using Magic_Villa_Utilidad;
using Magic_Villa_Web.Models;
using Magic_Villa_Web.Models.DTO.Villa;
using Magic_Villa_Web.Models.ViewModel;
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
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index(int pageNumber=1)
        {
            List<VillaDTO> villaList = new();
            VillaPaginadoViewModel villaVM = new VillaPaginadoViewModel(); // PAGINACION

            if (pageNumber < 1) pageNumber = 1;

            var response = await villaService.ObtenerTodosPaginado<APIResponse>(HttpContext.Session.GetString(DS.SessionToken),pageNumber,4);

            if (response != null)
            {
                villaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Resultado));

                //Paginación, llenar ViewModel
                villaVM = new VillaPaginadoViewModel()
                {
                    VillaList = villaList,
                    PageNumber = pageNumber,
                    TotalPaginas  = JsonConvert.DeserializeObject<int>(Convert.ToString(response.TotalPaginas))
                };

                if (pageNumber > 1) villaVM.Previo = "";
                if (villaVM.TotalPaginas <= pageNumber) villaVM.Siguiente = "disable";
            }
            return View(villaVM);
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