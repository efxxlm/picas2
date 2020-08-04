using asivamosffie.model.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.services.Validators
{
    public class SelectionProcessScheduleValidator: AbstractValidator<ProcesoSeleccionCronograma>
    {
        public SelectionProcessScheduleValidator()
        {
            RuleFor(x => x.ProcesoSeleccionId)
                  .NotEmpty().WithMessage("Debe ingresar el proceso de selección.");

            RuleFor(x => x.NumeroActividad)
                  .NotEmpty().WithMessage("Debe ingresar el numero de actividad.");

            RuleFor(x => x.Descripcion)
                    .NotEmpty().WithMessage("Debe ingresar la descripcion.");

            RuleFor(x => x.FechaMaxima)
                    .NotEmpty().WithMessage("Debe ingresar la fecha maxima.");

            RuleFor(x => x.EstadoActividadCodigo)
                    .NotEmpty().WithMessage("Debe ingresar el codigo de estado de la actividad.");

            RuleFor(x => x.FechaCreacion)
                    .NotEmpty().WithMessage("Debe ingresar la fecha creación.");

            RuleFor(x => x.UsuarioCreacion)
                    .NotEmpty().WithMessage("Debe ingresar usuario creacion.");

            //
        }
    }
}
