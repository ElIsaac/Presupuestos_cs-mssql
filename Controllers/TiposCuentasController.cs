using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.Data.SqlClient;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Services;
namespace ManejoPresupuesto.controllers{
    public class TiposCuentasController: Controller 
    {
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;

        public TiposCuentasController(IRepositorioTiposCuentas repositorioTiposCuentas)
        {
            this.repositorioTiposCuentas = repositorioTiposCuentas;
        }
        public IActionResult Crear(){
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta){
             if(!ModelState.IsValid){
                return View(tipoCuenta);
             }
             tipoCuenta.UsuarioId=1;
             await repositorioTiposCuentas.Crear(tipoCuenta);
             return View();
        }
    }
}