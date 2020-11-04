using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using lalupa.Authorization.JwtHelpers;
using asivamosffie.api.Controllers;
using AuthorizationTest.JwtHelpers;
using asivamosffie.services.Exceptions;
using asivamosffie.services.Helpers;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.model.APIModels;
using Newtonsoft.Json;
using iTextSharp.text.pdf.codec;

namespace asivamosffie.services
{
    public class RegisterPersonalProgrammingService : IRegisterPersonalProgrammingService
    {
        private readonly devAsiVamosFFIEContext _context;

        private readonly ICommonService _commonService;

        public RegisterPersonalProgrammingService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }

        public async Task<List<ProyectoGrilla>> GetListProyectos()
        {

            List<ProyectoGrilla> proyectoGrillas = await _context.ContratoConstruccion
                .Include(e => e.Proyecto)
                  .ThenInclude(i => i.InstitucionEducativa)
                .Include(e => e.Proyecto)
                   .ThenInclude(i => i.Sede)
                .Include(e => e.Proyecto)
                   .ThenInclude(i => i.ContratacionProyecto)
                      .ThenInclude(i => i.Contratacion)
                          .ThenInclude(i => i.Contrato)
                .Select(cc => new ProyectoGrilla {

                    LlaveMen = cc.Proyecto.LlaveMen,
                    NumeroContrato = cc.Proyecto.ContratacionProyecto.FirstOrDefault().Contratacion.Contrato.FirstOrDefault().NumeroContrato,



                }).ToListAsync();
              
             
            return proyectoGrillas;
        }


    }
}
