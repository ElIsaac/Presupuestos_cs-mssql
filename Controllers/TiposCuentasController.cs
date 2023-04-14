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
	    this.servicioUsuarios = servicioUsuarios;
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
	[HttpGet]
	public async Task<ActionResult> Editar(int id){
	    var usuarioId = servicioUsuarios.ObtenerUsuarioId();
	    var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId(id, usuarioId);
	    if(tipoCuenta is null){
		    return RedirectToAction("NoEncontrado", "Home");
	    }
	    return View(tipoCuenta);
	}
	[HttpPost]
	public async Task<ActionResult> Editar(TipoCuenta tipoCuenta){
	    var usuarioId = servicioUsuarios.ObtenerUsuarioId();
	    var tipoCuentaExiste = await repositorioTiposCuentas.ObtenerPorId(tipoCuenta.Id, usuarioId);
	    if(tipoCuentaExiste is null){
		    return RedirectToAction("NoEncontrado", "Home");
	    }
	    await repositorioTiposCuentas.Actualizar(tipoCuenta);
	    return RedirectToAction("Index");
	   
    }
}
}
