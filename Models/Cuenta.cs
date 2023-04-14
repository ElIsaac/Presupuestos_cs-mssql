namespace ManejoPresupuesto.Models
{
    public class Cuenta
    {
        public int Id { get; set;}
        
        public string Nombre { get; set;}

        public int TipoCuentaId { get; set;}
        public decimal Balance { get; set;}
        public string Descripcion { get; set;}

    }
}