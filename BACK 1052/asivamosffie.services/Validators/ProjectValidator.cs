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
            /*RuleFor(x => x.FechaSesionJunta)
                  .NotEmpty().WithMessage("Debe ingresar una fecha no superior a la fecha actual.")
                  .LessThan(p => DateTime.Now).WithMessage("No puede ser superior a la fecha actual.");


            RuleFor(x => Convert.ToString(x.NumeroActaJunta))
                    .NotEmpty().WithMessage("Debe ingresar el numero de acta.")
                    .Matches("[0-9]").WithMessage("Solo  números")
                    .Length(1, 4).WithMessage("La longitud debe ser entre 1 y 4 números");
                    //Poner maximo 4

            RuleFor(x => x.TipoIntervencionCodigo)
                    .NotEmpty().WithMessage("Debe ingresar un tipo de intervencion.");

            RuleFor(x => x.LlaveMen)
                    .NotEmpty().WithMessage("Debe ingresar llave MEN.")
                    .Length(8, 100).WithMessage("La longitud debe ser entre 1 y 8 caracteres")
                    .Matches(@"[^a-zA-Z\-0-9]").WithMessage("La cadena de entrada debe contener un (-) guion");

            RuleFor(x => x.LocalizacionIdMunicipio)
                    .NotEmpty().WithMessage("Debe ingresar Municipio.");

            RuleFor(x => x.InstitucionEducativaId)
                    .NotEmpty().WithMessage("Debe ingresar Institución Educativa.");
             
            RuleFor(x => x.SedeId)
                    .NotEmpty().WithMessage("Debe ingresar Sede.");

            RuleFor(x => x.EnConvocatoria)
                 .NotEmpty().WithMessage("Debe ingresar convocatoria.");

            RuleFor(x => x.CantPrediosPostulados)
                .NotEmpty().WithMessage("Debe ingresar cantidad de predios.")
                .InclusiveBetween(0,3).WithMessage("El rango es de 1 hasta 3 predios");
               //Ojo maximo 3 
               */
          //  RuleFor(r=> r.pres)

        }
    }
}
