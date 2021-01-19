using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace asivamosffie.services
{
    public class RegisterValidatePaymentRequierementsService : IRegisterValidatePaymentRequierementsService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;

        public RegisterValidatePaymentRequierementsService(IDocumentService documentService, devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _documentService = documentService;
            _commonService = commonService;
            _context = context;
        }

        #region Get
        public async Task<dynamic> GetContratoByTipoSolicitudCodigoModalidadContratoCodigoOrNumeroContrato(string pTipoSolicitud, string pModalidadContrato, string pNumeroContrato)
        {
            return await _context.Contrato
                                          .Include(c => c.Contratacion)
                                          .Where(c => c.NumeroContrato.Trim().ToLower().Contains(pNumeroContrato.Trim().ToLower()) && c.Contratacion.TipoSolicitudCodigo == pTipoSolicitud)
                                                      .Select(r => new
                                                      {
                                                          r.ContratoId,
                                                          r.NumeroContrato
                                                      }).ToListAsync();
        }

        public async Task<Contrato> GetContratoByContratoId(int pContratoId)
        {
            return await _context.Contrato
                 .Where(c => c.ContratoId == pContratoId)
                 .Include(c => c.Contratacion)
                    .ThenInclude(c => c.Contratista)
                 .Include(c => c.Contratacion)
                    .ThenInclude(cp => cp.DisponibilidadPresupuestal)
                    .FirstOrDefaultAsync();
        }

        public async Task<dynamic> GetProyectosByIdContrato(int pContratoId)
        {
            List<dynamic> dynamics = new List<dynamic>();

            var resultContrato = _context.Contrato
                .Where(r => r.ContratoId == pContratoId)
                .Include(cp => cp.ContratoPoliza)
                            .Select(c => new
                            {
                                c.NumeroContrato,
                                c.ContratoPoliza.FirstOrDefault().FechaAprobacion,
                                PlazoDias = c.PlazoFase1PreDias + c.PlazoFase2ConstruccionDias,
                                PlazoMeses = c.PlazoFase1PreMeses + c.PlazoFase2ConstruccionMeses
                            }).FirstOrDefault();

            var resultProyectos = await _context.VProyectosXcontrato
                        .Where(p => p.ContratoId == pContratoId)
                                                                .Select(p => new
                                                                {
                                                                    p.LlaveMen,
                                                                    p.TipoIntervencion,
                                                                    p.Departamento,
                                                                    p.Municipio,
                                                                    p.InstitucionEducativa,
                                                                    p.Sede
                                                                }).ToListAsync();
            dynamics.Add(resultContrato);
            dynamics.Add(resultProyectos);

            return dynamics;

        }

    #endregion
}
}
