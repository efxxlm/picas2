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

namespace asivamosffie.services
{
    public class DailyFollowUpService : IDailyFollowUpService
    {
        private readonly devAsiVamosFFIEContext _context;

        public DailyFollowUpService(devAsiVamosFFIEContext context)
        {
            _context = context;
        }

        public async void gridRegisterDailyFollowUp()
        {
            List<Dominio> ListParametricas = _context.Dominio.ToList();
            List<Localizacion> Listlocalizacion = _context.Localizacion.ToList();
                

            Contrato contrato = await _context.Contrato
                     .Include(r => r.Contratacion)
                        .ThenInclude(r => r.ContratacionProyecto)
                              .ThenInclude(r => r.Proyecto)
                                 .ThenInclude(r => r.InstitucionEducativa)
                    .Include(r => r.Contratacion)
                        .ThenInclude(r => r.ContratacionProyecto)
                              .ThenInclude(r => r.Proyecto)
                                  .ThenInclude(r => r.Sede)
                     .Include(r => r.Contratacion)
                         .ThenInclude(r => r.Contratista)
                    .FirstOrDefaultAsync();

            foreach (var ContratacionProyecto in contrato.Contratacion.ContratacionProyecto)
                {
                    Localizacion Municipio = Listlocalizacion.Where(r => r.LocalizacionId == ContratacionProyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                    ContratacionProyecto.Proyecto.Departamento = Listlocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault().Descripcion;
                    ContratacionProyecto.Proyecto.Municipio = Municipio.Descripcion;
                    ContratacionProyecto.Proyecto.TipoIntervencionCodigo = ListParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion && r.Codigo == ContratacionProyecto.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
                }
        }


    }
}
