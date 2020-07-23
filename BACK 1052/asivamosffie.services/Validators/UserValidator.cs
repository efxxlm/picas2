using asivamosffie.model.Models;
using FluentValidation;

namespace asivamosffie.services.Validators
{
    public class UserValidator : AbstractValidator<Usuario>
    {
        public UserValidator()
        {      
            /*RuleFor(x => x.Contrasena)
                .Length(8, 15).WithMessage("Lo sentimos, la nueva contraseña no cumple con los estándares de seguridad.")
                .NotEmpty().WithMessage("Por favor Especifique la contraseña actual.");*/
        }


    }
}
