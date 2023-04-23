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
        public async Task<IActionResult> Index(){
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var cuentasConTipoCuenta = await repositorioCuentas.Buscar(usuarioId);
            var modelo = cuentasConTipoCuenta
                .GroupBy(x => x.TipoCuenta)
                .Select(grupo => new IndiceCuentasViewModel{
                    TipoCuenta = grupo.Key,
                    Cuentas = grupo.AsEnumerable()
                }).ToList();
            return View(modelo);

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
        [HttpGet]
        public async Task<IActionResult> Editar(int id){
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var cuenta = await repositorioCuentas.ObtenerPorId(id, usuarioId);
            if(cuenta is null){
                RedirectToAction("NoEncontrado", "Home");
            }
            var modelo = new CuentasCreacionViewModel(){
                Id = cuenta.Id,
                Nombre = cuenta.Nombre,
                Descripcion = cuenta.Descripcion,
                Balance = cuenta.Balance
            };
            modelo.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(CuentasCreacionViewModel cuentaEditar){
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var cuenta = await repositorioCuentas.ObtenerPorId(cuentaEditar.Id, usuarioId);
            if(cuenta is null){
                RedirectToAction("NoEncontrado", "Home");
            }

            var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId(cuenta.TipoCuentaId, usuarioId);

            if(tipoCuenta is null){
                RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioCuentas.ActualizarCuenta(cuentaEditar);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Borrar(Cuenta cuenta){
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var Existecuenta = await repositorioCuentas.ObtenerPorId(cuenta.Id, usuarioId);
            if(Existecuenta is null){
                RedirectToAction("NoEncontrado", "Home");
            }

            

            await repositorioCuentas.Borrar(cuenta);
            return RedirectToAction("Index");
        }
    }
}