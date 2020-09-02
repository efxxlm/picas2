using asivamosffie.model.Models;
using FluentValidation;
using System;
using asivamosffie.services.Helpers;
namespace asivamosffie.services.Validators
{
    class ContractingValidator : AbstractValidator<Contratacion>
    {
        public ContractingValidator()
        {
            RuleFor(x => x.TipoSolicitudCodigo)
                             .NotEmpty().WithMessage("Debe ingresar un tipo de solicitud.");

            RuleFor(x => x.EstadoSolicitudCodigo)
                             .NotEmpty().WithMessage("Debe ingresar un Estado de solicitud.");


        }
    }
}
