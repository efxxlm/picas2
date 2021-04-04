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
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class RegisterContractualLiquidationRequestService : IRegisterContractualLiquidationRequestService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly ITechnicalRequirementsConstructionPhaseService _technicalRequirementsConstructionPhaseService;

        public RegisterContractualLiquidationRequestService(devAsiVamosFFIEContext context, ICommonService commonService, ITechnicalRequirementsConstructionPhaseService technicalRequirementsConstructionPhaseService)
        {
            _context = context;
            _commonService = commonService;
            _technicalRequirementsConstructionPhaseService = technicalRequirementsConstructionPhaseService;
        }

        public async Task<List<VContratacionProyectoSolicitudLiquidacion>> gridRegisterContractualLiquidationObra()
        {
            return await _context.VContratacionProyectoSolicitudLiquidacion.Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString()).ToListAsync();
        }

        public async Task<List<VContratacionProyectoSolicitudLiquidacion>> gridRegisterContractualLiquidationInterventoria()
        {
            return await _context.VContratacionProyectoSolicitudLiquidacion.Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString()).ToListAsync();
        }

    }
}
