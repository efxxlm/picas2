using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.model.APIModels;
using System.Globalization;
using Microsoft.Extensions.Hosting;

namespace asivamosffie.services
{
    public class FinancialBalanceService : IFinalBalanceService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly ITechnicalRequirementsConstructionPhaseService _technicalRequirementsConstructionPhaseService;

        public FinancialBalanceService(devAsiVamosFFIEContext context,
                                       ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }

        public async Task<dynamic> GetOrdenGiroBy(string pTipoSolicitudCodigo, string pNumeroOrdenGiro)
        { 
            if (string.IsNullOrEmpty(pNumeroOrdenGiro))
            {
                return (
                    _context.VOrdenGiro
                    .Where(v => v.TipoSolicitudCodigo == pTipoSolicitudCodigo && v.RegistroCompletoTramitar)
                    .Select(v => new
                    {
                     
                        v.FechaAprobacionFinanciera,
                
                        v.TipoSolicitud,
                        v.NumeroSolicitudOrdenGiro,
                        v.OrdenGiroId 
                    }));
            }
            else
            {
                return (
                    _context.VOrdenGiro
                    .Where(v => v.NumeroSolicitudOrdenGiro == pNumeroOrdenGiro && v.RegistroCompletoTramitar)
                    .Select(v => new
                    {
                        v.NumeroSolicitudOrdenGiro,
                        v.OrdenGiroId
                    })); 
            } 
        }

    }
}
