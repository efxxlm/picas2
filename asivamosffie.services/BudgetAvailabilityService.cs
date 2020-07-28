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
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class BudgetAvailabilityService : IBudgetAvailabilityService
    {
        private readonly devAsiVamosFFIEContext _context;

        private readonly ICommonService _commonService;

        public BudgetAvailabilityService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }

        public async Task<List<DisponibilidadPresupuestalGrilla>> GetListDisponibilidadPresupuestal()
        {
            List<DisponibilidadPresupuestal> ListDisponibilidadPresupuestal = await _context.DisponibilidadPresupuestal.Where(r => !(bool)r.Eliminado).ToListAsync();
           
            List<DisponibilidadPresupuestalGrilla> ListDisponibilidadPresupuestalGrilla = new List<DisponibilidadPresupuestalGrilla>();


            foreach (var DisponibilidadPresupuestal in ListDisponibilidadPresupuestal)
            {
                string strEstadoRegistro = "";
                string strTipoSolicitud = "";

                if (string.IsNullOrEmpty(DisponibilidadPresupuestal.EstadoSolicitudCodigo))
                {
                    strEstadoRegistro = await _commonService.GetNombreDominioByCodigoAndTipoDominio(DisponibilidadPresupuestal.EstadoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Solicitud_Presupuestal);
                }
                 
                if (string.IsNullOrEmpty(DisponibilidadPresupuestal.TipoSolicitudCodigo))
                {
                    strTipoSolicitud = await _commonService.GetNombreDominioByCodigoAndTipoDominio(DisponibilidadPresupuestal.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Solicitud_Presupuestal);
                }

                DisponibilidadPresupuestalGrilla disponibilidadPresupuestalGrilla = new DisponibilidadPresupuestalGrilla {

                    FechaSolicitud = DisponibilidadPresupuestal.FechaSolicitud.ToString(),
                    EstadoRegistro = strEstadoRegistro,
                    TipoSolicitud = strTipoSolicitud,
                    DisponibilidadPresupuestalId = DisponibilidadPresupuestal.DisponibilidadPresupuestalId,
                    NumeroSolicitud = DisponibilidadPresupuestal.NumeroSolicitud 
                };

                ListDisponibilidadPresupuestalGrilla.Add(disponibilidadPresupuestalGrilla);
            }

            return ListDisponibilidadPresupuestalGrilla.OrderBy(r=> r.DisponibilidadPresupuestalId).ToList();
        }


         public async Task<DisponibilidadPresupuestal> GetDisponibilidadPresupuestalByID(int pDisponibilidadPresupuestalId) { 
            return await _context.DisponibilidadPresupuestal.Where(r => r.DisponibilidadPresupuestalId == pDisponibilidadPresupuestalId).Include(r=> r.DisponibilidadPresupuestalProyecto).FirstOrDefaultAsync();
        
        }
    }
}
