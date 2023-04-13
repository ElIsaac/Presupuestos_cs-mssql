using System.ComponentModel.DataAnnotations;
namespace ManejoPresupuesto.Validaciones
{
    public class UpperAttribute : ValidationAttribute{
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null || string.IsNullOrEmpty(value.ToString())){
                return ValidationResult.Success;
            }
            

            var primeraletra = value.ToString()[0].ToString();
            if(primeraletra != primeraletra.ToUpper()){
                return new ValidationResult("La primer letra debe de ser mayuscula");
            }

            return ValidationResult.Success;
        }
        

    }
}

