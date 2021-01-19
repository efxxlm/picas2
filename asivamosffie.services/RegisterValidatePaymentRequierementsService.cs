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

        public async Task<dynamic> GetContratoByTipoSolicitudCodigoModalidadContratoCodigoOrNumeroContrato(
            string pTipoSolicitud,
            string pModalidadContrato, 
            int pNumeroContrato)
        {
            return await _context.Contrato
                                          .Include(c => c.Contratacion)
                                          .Where(c => c.Contratacion.TipoContratacionCodigo == pTipoSolicitud
                                                   && c.NumeroContrato.Trim().Contains(pNumeroContrato.ToString()))
                                                      .Select(r => new
                                                      {
                                                          r.ContratoId,
                                                          r.NumeroContrato
                                                      }).ToListAsync();
        }
    }
}
