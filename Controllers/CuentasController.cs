using ManejoPresupuesto.Services;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto.Controllers
{
    public class CuentasController : Controller
    {
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        private readonly IRepositorioCuentas repositorioCuentas;
        private readonly IServicioUsuarios servicioUsuarios;
        public CuentasController(IRepositorioTiposCuentas repositorioTiposCuentas , IRepositorioCuentas repositorioCuentas , IServicioUsuarios servicioUsuarios)
        {
            this.repositorioTiposCuentas=repositorioTiposCuentas;
            this.repositorioCuentas=repositorioCuentas;
            this.servicioUsuarios=servicioUsuarios;
        }
        [HttpGet]
        public async Task<IActionResult> Crear(){
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tiposCuentas=await repositorioTiposCuentas.Obtener(usuarioId);
            var modelo = new CuentasCreacionViewModel();
            modelo.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
            return View(modelo);
        }
        [HttpPost]
         public async Task<IActionResult> Crear(CuentasCreacionViewModel cuenta){
             var usuarioId = servicioUsuarios.ObtenerUsuarioId();
             var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId(cuenta.TipoCuentaId, usuarioId);

             if(tipoCuenta is null){
                 return RedirectToAction("NoEncontrado", "Home");
                 
             }

             if(!ModelState.IsValid){
                 cuenta.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
                 return View(cuenta);
             }
             await repositorioCuentas.Crear(cuenta);
             return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerTiposCuentas(int usuarioId){
            var tiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);
            return tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }
    }
}