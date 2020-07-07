using asivamosffie.model.Models;
using FluentValidation;
using System;
using asivamosffie.services.Helpers;
namespace asivamosffie.services.Validators
{
    public class ProjectValidator : AbstractValidator<Proyecto>
    {
        public ProjectValidator()
        {
            RuleFor(x => x.FechaSesionJunta)
                    .LessThan(p => DateTime.Now).WithMessage("No puede ser superior a la fecha actual.")
                    .NotEmpty().WithMessage("Debe ingresar una fecha no superior a la fecha actual.");

            RuleFor(x => x.NumeroActaJunta)
                    .NotEmpty().WithMessage("Debe ingresar el numero de acta.");
            //Poner maximo 4

            RuleFor(x => x.TipoIntervencionCodigo)
                    .NotEmpty().WithMessage("Debe ingresar un tipo de intervencion.");

            RuleFor(x => x.LlaveMen)
                    .NotEmpty().WithMessage("Debe ingresar llave MEN.");

            RuleFor(x => x.LocalizacionIdMunicipio)
                    .NotEmpty().WithMessage("Debe ingresar Municipio.");

            RuleFor(x => x.InstitucionEducativa)
                    .NotEmpty().WithMessage("Debe ingresar Institución Educativa.");
             
            RuleFor(x => x.Sede)
                    .NotEmpty().WithMessage("Debe ingresar Sede.");

            RuleFor(x => x.EnConvocatoria)
                 .NotEmpty().WithMessage("Debe ingresar convocatoria.");

            RuleFor(x => x.CantPrediosPostulados)
                .NotEmpty().WithMessage("Debe ingresar cantidad de predios.");
               //Ojo maximo 3 

          //  RuleFor(r=> r.pres)
        }
    }
}
