using asivamosffie.model.Models;
using FluentValidation;

namespace asivamosffie.services.Validators
{
    public class ProjectValidator: AbstractValidator<Proyecto>
    {
        public ProjectValidator()
        {      
            RuleFor(x => x.EnConvocatoria) 
                 .NotEmpty().WithMessage("Debe ingreasar este campo.");


        }
    }
}
