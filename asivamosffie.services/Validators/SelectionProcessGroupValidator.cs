using asivamosffie.model.Models;
using FluentValidation;

namespace asivamosffie.services.Validators
{
    public class SelectionProcessGroupValidator: AbstractValidator<ProcesoSeleccionGrupo>
    {
        public SelectionProcessGroupValidator()
        {
            RuleFor(x => x.ProcesoSeleccionId)
                 .NotEmpty().WithMessage("Debe ingresar el proceso de selección.");

            RuleFor(x => x.NombreGrupo)
                    .NotEmpty().WithMessage("Debe ingresar el nombre del grupo.");


            //.Length(minLength, isMinLengthOnly);
            RuleFor(x => x.TipoPresupuestoCodigo)
                    .NotEmpty().WithMessage("Debe ingresar eltipo de presupuesto.");

            RuleFor(x => x.PlazoMeses)
                    .NotEmpty().WithMessage("Debe ingresar el plazo en meses");

            RuleFor(x => x.FechaCreacion)
                .NotEmpty().WithMessage("Debe ingresar la fecha de creación.");

            RuleFor(x => x.UsuarioCreacion)
               .NotEmpty().WithMessage("Debe ingresar el usuario creación.");

        }
    }
}
