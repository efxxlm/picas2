using asivamosffie.model.Models;
using FluentValidation;

namespace asivamosffie.services.Validators
{
    public class SelectionProcessValidator: AbstractValidator<ProcesoSeleccion>
    {
        public SelectionProcessValidator()
        {
            RuleFor(x => x.NumeroProceso)
                  .NotEmpty().WithMessage("Debe ingresar el Numero de proceso.");

            RuleFor(x => x.Objeto)
                    .NotEmpty().WithMessage("Debe ingresar el Objeto.");

            RuleFor(x => x.AlcanceParticular)
                    .NotEmpty().WithMessage("Debe ingresar el Alcance particular.");

            RuleFor(x => x.Justificacion)
                    .NotEmpty().WithMessage("Debe ingresar la justificacion");

            RuleFor(x => x.TipoIntervencionCodigo)
                    .NotEmpty().WithMessage("Debe ingresar el tipo intervencion.");

            RuleFor(x => x.TipoAlcanceCodigo)
                    .NotEmpty().WithMessage("Debe ingresar tipo de alcance.");

            RuleFor(x => x.EsDistribucionGrupos)
                    .NotEmpty().WithMessage("Debe ingresar el tipo de distribucion(Grupos).");

            RuleFor(x => x.EsCompleto)
                 .NotEmpty().WithMessage("Debe ingresar es completo."); // ?

            RuleFor(x => x.EstadoProcesoSeleccionCodigo)
                .NotEmpty().WithMessage("Debe ingresar el estado del proceso.");

            RuleFor(x => x.FechaCreacion)
                .NotEmpty().WithMessage("Debe ingresar la fecha de creación.");

            RuleFor(x => x.UsuarioCreacion)
               .NotEmpty().WithMessage("Debe ingresar el usuario creación.");

        }
    }
}
