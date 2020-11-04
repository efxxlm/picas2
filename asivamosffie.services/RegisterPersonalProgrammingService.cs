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

        public async Task<List<dynamic>> GetListProyectos()
        {

            List<ContratoConstruccion> ListContratoConstruccion = await _context.ContratoConstruccion
                .Include(e => e.Proyecto)
                  .ThenInclude(i => i.InstitucionEducativa)
                .Include(e => e.Proyecto)
                   .ThenInclude(i => i.Sede)
                .Include(e => e.Proyecto)
                   .ThenInclude(i => i.ContratacionProyecto)
                      .ThenInclude(i => i.Contratacion)
                          .ThenInclude(i => i.Contrato).Distinct().OrderByDescending(r=> r.ContratoConstruccionId).ToListAsync();

            List<dynamic> LisDyna = new List<dynamic>();
            List<Dominio> ListDominioTipoIntervencion = await _commonService.GetListDominioByIdTipoDominio((int)EnumeratorTipoDominio.Tipo_de_Intervencion);
            List<Dominio> ListDominioEstadoProgramacionCodigo = await _commonService.GetListDominioByIdTipoDominio((int)EnumeratorTipoDominio.Tipo_de_Intervencion);

            foreach (var ContratoConstruccion in ListContratoConstruccion)
            {
                LisDyna.Add(new
                {
                    FechaFirmaActaInicio = ContratoConstruccion.ActaApropiacionFechaAprobacion.HasValue ? ((DateTime)ContratoConstruccion.ActaApropiacionFechaAprobacion).ToString("dd-MM-yyyy") : " ",
                    ContratoConstruccion.Proyecto.LlaveMen,
                    ContratoConstruccion.Proyecto.ContratacionProyecto.FirstOrDefault().Contratacion.Contrato.FirstOrDefault().NumeroContrato,
                    TipoIntervencion = ListDominioTipoIntervencion.Where(r => r.Codigo == ContratoConstruccion.Proyecto.TipoIntervencionCodigo).FirstOrDefault(), 
                    InstitucionEducativaSede = ContratoConstruccion.Proyecto.InstitucionEducativa.Nombre,
                    Sede = ContratoConstruccion.Proyecto.Sede.Nombre,
                    EstadoProgramacionInicial = !string.IsNullOrEmpty(ContratoConstruccion.Proyecto.EstadoProgramacionCodigo) ? ListDominioEstadoProgramacionCodigo.Where(r=> r.Codigo == ContratoConstruccion.Proyecto.EstadoProgramacionCodigo).FirstOrDefault().Nombre : ListDominioEstadoProgramacionCodigo.Where(r => r.Codigo == ConstanCodigoEstadoProgramacionInicial.Sin_ProgramacionPersona).FirstOrDefault().Nombre,
                    ContratoConstruccion.Proyecto.ProyectoId 
                });
            }

            return LisDyna;
        }


    }
}
