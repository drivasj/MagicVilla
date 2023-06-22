using AutoMapper;
using Magic_Villa_Utilidad;
using Magic_Villa_Web.Models;
using Magic_Villa_Web.Models.DTO.NumeroVilla;
using Magic_Villa_Web.Models.DTO.Villa;
using Magic_Villa_Web.Models.ViewModel;
using Magic_Villa_Web.Services;
using Magic_Villa_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;
using System.Reflection;

namespace Magic_Villa_Web.Controllers
{
    public class NumeroVillaController : Controller
    {
        private readonly INumeroVillaService numeroVillaService;
        private readonly IVillaService villaService;
        private readonly IMapper mapper;

        public NumeroVillaController(INumeroVillaService numeroVillaService, IVillaService villaService, IMapper mapper)
        {
            this.numeroVillaService = numeroVillaService;
            this.villaService = villaService;
            this.mapper = mapper;
        }
        [Authorize(Roles = "admin")]
        public async Task <IActionResult> IndexNumeroVilla()
        {
            List<NumeroVillaDTO> numeroVillaList = new();

            var response = await numeroVillaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));

            if(response != null && response.IsExitoso)
            {
                numeroVillaList = JsonConvert.DeserializeObject<List<NumeroVillaDTO>>(Convert.ToString(response.Resultado));
            }

            return View(numeroVillaList);
        }

        // GET Crear
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CrearNumeroVilla()
        {
            NumeroVillaViewModel numeroVillaVM = new NumeroVillaViewModel();
            var response = await villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));

            if (response!= null && response.IsExitoso)
            {
                numeroVillaVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Resultado)).
                                          Select(v => new SelectListItem
                                          {
                                              Text = v.Nombre,
                                              Value = v.Id.ToString()
                                          });
            }

            return View(numeroVillaVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> CrearNumeroVilla(NumeroVillaViewModel modelo)
        {
            if(ModelState.IsValid)
            {
                var response = await numeroVillaService.Crear<APIResponse>(modelo.NumeroVilla, HttpContext.Session.GetString(DS.SessionToken));
                if(response != null && response.IsExitoso)
                {
                    TempData["exitoso"] = "Villa creada exitosamente";
                    return RedirectToAction(nameof(IndexNumeroVilla));
                }
                else
                {
                    if(response.ErrorMessages.Count>0)
                    {
                        ModelState.AddModelError("ErrorMessasges", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }
            /// Devuelve a la vista si el modelo no es valido
            
            var res = await villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));

            if (res != null && res.IsExitoso)
            {
                modelo.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(res.Resultado)).
                                          Select(v => new SelectListItem
                                          {
                                              Text = v.Nombre,
                                              Value = v.Id.ToString()
                                          });
            }

            return View(modelo);

        }

        //GET UPDATE
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ActualizarNumeroVilla(int villaNo)
        {
            NumeroVillaUpdateViewModel numeroVillaVM = new();

            var response = await numeroVillaService.Obtener<APIResponse>(villaNo, HttpContext.Session.GetString(DS.SessionToken));
            if (response != null && response.IsExitoso)
            {
                NumeroVillaDTO modelo = JsonConvert.DeserializeObject<NumeroVillaDTO>(Convert.ToString(response.Resultado));
                numeroVillaVM.NumeroVilla = mapper.Map<NumeroVillaUpdateDTO>(modelo);
            }
            response = await villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));

            if (response != null && response.IsExitoso)
            {
                numeroVillaVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Resultado)).
                                          Select(v => new SelectListItem
                                          {
                                              Text = v.Nombre,
                                              Value = v.Id.ToString()
                                          });

                return View(numeroVillaVM);
            }
            return NotFound();
        }

        //[POST]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> ActualizarNumeroVilla(NumeroVillaUpdateViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                var response = await numeroVillaService.Actualizar<APIResponse>(modelo.NumeroVilla, HttpContext.Session.GetString(DS.SessionToken));
                if (response != null && response.IsExitoso)
                {
                    TempData["exitoso"] = "Villa actualizada exitosamente";
                    return RedirectToAction(nameof(IndexNumeroVilla));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessasges", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }
            /// Devuelve a la vista si el modelo no es valido

            var res = await villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));

            if (res != null && res.IsExitoso)
            {
                modelo.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(res.Resultado)).
                                          Select(v => new SelectListItem
                                          {
                                              Text = v.Nombre,
                                              Value = v.Id.ToString()
                                          });
            }

            return View(modelo);
        }

        //GET remove
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RemoverNumeroVilla(int villaNo)
        {
            NumeroVillaDeleteViewModel numeroVillaVM = new();

            var response = await numeroVillaService.Obtener<APIResponse>(villaNo, HttpContext.Session.GetString(DS.SessionToken));
            if (response != null && response.IsExitoso)
            {
                NumeroVillaDTO modelo = JsonConvert.DeserializeObject<NumeroVillaDTO>(Convert.ToString(response.Resultado));
                numeroVillaVM.NumeroVilla = modelo;
            }
            response = await villaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DS.SessionToken));

            if (response != null && response.IsExitoso)
            {
                numeroVillaVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Resultado)).
                                          Select(v => new SelectListItem
                                          {
                                              Text = v.Nombre,
                                              Value = v.Id.ToString()
                                          });

                return View(numeroVillaVM);
            }
            return NotFound();
        }


        [HttpPost]  
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoverNumeroVilla(NumeroVillaDeleteViewModel modelo)
        {
            var responsive = await numeroVillaService.Remover<APIResponse>(modelo.NumeroVilla.VillaNo, HttpContext.Session.GetString(DS.SessionToken));
            if(responsive != null && responsive.IsExitoso)
            {
                TempData["exitoso"] = "Número Villa eliminado    exitosamente";
                return RedirectToAction(nameof(IndexNumeroVilla));
            }
            TempData["error"] = "Un Error Ocurrio al Remover";
            return View(modelo);
        }


    }
}
