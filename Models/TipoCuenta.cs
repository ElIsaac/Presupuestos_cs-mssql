using System.ComponentModel.DataAnnotations;
using ManejoPresupuesto.Validaciones;

namespace ManejoPresupuesto.Models
{
    public class TipoCuenta{
        public int Id {get; set;}
        [UpperAttribute]
        [Required]
        public string Nombre {get; set;}
        public int UsuarioId {get; set;}
        public int Orden {get; set;}

    }
}