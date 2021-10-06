using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class ReleaseBalanceService : IReleaseBalanceService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IRequestBudgetAvailabilityService _requestBudgetAvailabilityService;


        public ReleaseBalanceService(devAsiVamosFFIEContext context, ICommonService commonService, IRequestBudgetAvailabilityService requestBudgetAvailabilityService)
        {
            _commonService = commonService;
            _context = context;
            _requestBudgetAvailabilityService = requestBudgetAvailabilityService;
        }

        //traer los DRP
        public async Task<List<dynamic>> GetDrpByProyectoId(int pProyectoId)
        {
            List<dynamic> drps = new List<dynamic>();

            List<ContratacionProyecto> contratacionProyectos = _context.ContratacionProyecto.Where(r => r.ProyectoId == pProyectoId).ToList();
            if (contratacionProyectos != null)
            {
                foreach (var cp in contratacionProyectos)
                {
                    DisponibilidadPresupuestal drp = _context.DisponibilidadPresupuestal
                        .Where(r => r.Eliminado != true && r.ContratacionId == cp.ContratacionId && r.NumeroDrp != null && (r.EstadoSolicitudCodigo == "5" || r.EstadoSolicitudCodigo == "8")).FirstOrDefault();
                    if (drp != null)
                    {
                        List<VSaldoAliberar> datosAportante = _context.VSaldoAliberar.Where(r => r.ProyectoId == pProyectoId && r.DisponibilidadPresupuestalId == drp.DisponibilidadPresupuestalId).ToList();
                        datosAportante.ForEach(r =>{
                            CofinanciacionAportante ca = _context.CofinanciacionAportante.Find(r.CofinanciacionAportanteId);
                            if(ca != null)
                                r.NombreAportante = _requestBudgetAvailabilityService.getNombreAportante(ca);
                            r.NombreFuente = _context.Dominio.Where(x => x.Codigo == r.FuenteRecursosCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre;

                        });
                        drps.Add(new
                        {
                            NumeroDrp = drp.NumeroDrp,
                            DisponibilidadPresupuestalId = drp.DisponibilidadPresupuestalId,
                            ProyectoId = cp.ProyectoId,
                            ContratacionId = cp.ContratacionId,
                            AportantesGrid = datosAportante
                        });
                    }

                }
            }

            return drps;
        }

    }
}
