using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto.Models
{
    public class CuentasCreacionViewModel:Cuenta
    {
        public IEnumerable<SelectListItem> TiposCuentas {get; set;}
    }
}