using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.Data.SqlClient;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Services;
namespace ManejoPresupuesto.controllers{
    public class TiposCuentasController: Controller 
    {
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
	private readonly IServicioUsuarios servicioUsuarios;

        public TiposCuentasController(IRepositorioTiposCuentas repositorioTiposCuentas, IServicioUsuarios servicioUsuarios)
        {
            this.repositorioTiposCuentas = repositorioTiposCuentas;
        }
	public async Task<IActionResult>Index(){
		var usuarioId = servicioUsuarios.ObtenerUsuarioId();
		var tiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);
		return View(tiposCuentas); 
	}
        public IActionResult Crear(){
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta){
            if(!ModelState.IsValid){
                return View(tipoCuenta);
            }
            tipoCuenta.UsuarioId=servicioUsuarios.ObtenerUsuarioId();
             
            var yaExiste = await repositorioTiposCuentas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);
            if(yaExiste){
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe");
                return View(tipoCuenta);
            }

            await repositorioTiposCuentas.Crear(tipoCuenta);
            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            
	    var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var yaExisteTipoCuenta = await repositorioTiposCuentas.Existe(nombre, usuarioId);
            if(yaExisteTipoCuenta){
                return Json($"El nombre {nombre} ya exsiste");
            }
            return Json(true);
        }
    }
}
