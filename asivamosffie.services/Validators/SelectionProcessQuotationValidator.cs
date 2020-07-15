using asivamosffie.model.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.services.Validators
{
    public class SelectionProcessQuotationValidator: AbstractValidator<ProcesoSeleccionCotizacion>
    {
        public SelectionProcessQuotationValidator()
        {
            RuleFor(x => x.ProcesoSeleccionId)
                .NotEmpty().WithMessage("Debe ingresar el proceso de selección.");

            RuleFor(x => x.NombreOrganizacion)
                    .NotEmpty().WithMessage("Debe ingresar el nombre de la organizacion.");

            RuleFor(x => x.ValorCotizacion)
                    .NotEmpty().WithMessage("Debe ingresar el valor de la Cotizacion.");


            RuleFor(x => x.Descripcion)
                    .NotEmpty().WithMessage("Debe ingresar la descripcion.");

            RuleFor(x => x.FechaCreacion)
                   .NotEmpty().WithMessage("Debe ingresar la fecha creacion.");

            RuleFor(x => x.UsuarioCreacion)
                   .NotEmpty().WithMessage("Debe ingresar el usuario creacion.");

        }
    }
}
