using System.ComponentModel.DataAnnotations;
using ManejoPresupuesto.Validaciones;
using Microsoft.AspNetCore.Mvc;
namespace ManejoPresupuesto.Models
{
    public class TipoCuenta{
        public int Id {get; set;}
        [UpperAttribute]
        [Required]
        [Remote(action: "VerificarExisteTipoCuenta", controller: "TipoCuenta")]
        public string Nombre {get; set;}
        public int UsuarioId {get; set;}
        public int Orden {get; set;}

    }
}