using AutoMapper;
using Magic_Villa_Utilidad;
using Magic_Villa_Web.Models;
using Magic_Villa_Web.Models.DTO.Villa;
using Magic_Villa_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Reflection;

namespace Magic_Villa_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService villaService;
        private readonly IMapper mapper;

        public VillaController(IVillaService villaService,IMapper mapper)
        {
            this.villaService = villaService;
            this.mapper = mapper;
        }

        [Authorize(Roles = "admin")]

        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> villaList = new();

            var response = await villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));

            if (response !=null && response.IsExitoso)
            {
                villaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Resultado));
            }

            return View(villaList);
        }

        //GET
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CrearVilla()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> CrearVilla(VillaCreateDTO modelo)
        {
            if (ModelState.IsValid)
            {
                var responsive = await villaService.Crear<APIResponse>(modelo, HttpContext.Session.GetString(DS.SessionToken));
                if (responsive != null && responsive.IsExitoso)
                {
                    TempData["exitoso"] = "Villa creada exitosamente";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            return View(modelo);
        }

        //GET UPDATE
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ActualizarVilla(int villaId)
        {
            var response = await villaService.Obtener<APIResponse>(villaId, HttpContext.Session.GetString(DS.SessionToken));

            if(response != null && response.IsExitoso)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Resultado));
                return View(mapper.Map<VillaUpdateDTO>(model));
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarVilla(VillaUpdateDTO modelo)
        {
            if (ModelState.IsValid)
            {
                var response = await villaService.Actualizar<APIResponse>(modelo, HttpContext.Session.GetString(DS.SessionToken));
                if(response != null && response.IsExitoso)
                {
                    TempData["exitoso"] = "Villa actualizada exitosamente";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            return View(modelo);
        }

        //GET Remove
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RemoverVilla(int villaId)
        {
            var response = await villaService.Obtener<APIResponse>(villaId, HttpContext.Session.GetString(DS.SessionToken));

            if (response != null && response.IsExitoso)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Resultado));
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoverVilla(VillaDTO modelo)
        {
          
            var response = await villaService.Remover<APIResponse>(modelo.Id, HttpContext.Session.GetString(DS.SessionToken));
            if (response != null && response.IsExitoso)
            {
                TempData["exitoso"] = "Villa eliminada exitosamente";
                return RedirectToAction(nameof(IndexVilla));
            }
           
             return View(modelo);
        }


    }
}
