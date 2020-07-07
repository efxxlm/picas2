using asivamosffie.model.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.services.Validators
{
    public class ContributorValidator : AbstractValidator<Aportante>
    {
        public ContributorValidator()
        {
            RuleFor(x => x.TipoAportanteCodigo).NotEmpty().WithMessage("101");
            RuleFor(x => x.CantidadDocumentos).NotEmpty().WithMessage("101");
            RuleFor(x => x.ValorTotal).NotEmpty().WithMessage("101");
            RuleFor(x => x.AcuerdoCofinanciacionId).NotEmpty().WithMessage("101");
            RuleFor(x => x.FechaCreacion).NotEmpty().WithMessage("101");
            RuleFor(x => x.UsuarioCreacion).NotEmpty().WithMessage("101");
        }
    }
}
