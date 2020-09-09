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
using DocumentFormat.OpenXml.Wordprocessing;

namespace asivamosffie.services
{
    public class ManageContractualProcessesService : IManageContractualProcessesService
    {
        private readonly devAsiVamosFFIEContext _context;

        private readonly ICommonService _commonService;

        public ManageContractualProcessesService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }

        public async Task<List<SesionComiteSolicitud>> GetListSesionComiteSolicitud()
        {
            // Estado de la sesionComiteSolicitud
            //• Recibidas sin tramitar ante Fiduciaria
            //• Enviadas a la fiduciaria
            //• Registradas por la fiduciaria

            //Se listan las que tengan con acta de sesion aprobada  

            //    List<SesionComiteSolicitud> ListSesionComiteSolicitud = await _context.SesionComiteSolicitud
            //.Where(r => r.TipoSolicitud == ConstanCodigoTipoSolicitud.Contratacion
            //&& r.EstadoDelRegistro == ConstanCodigoEstadoComite.Con_Acta_De_Sesion_Aprobada
            //)
            //.ToListAsync(); 
            List<SesionComiteSolicitud> ListSesionComiteSolicitud = await _context.SesionComiteSolicitud
                .Where(r => !(bool)r.Eliminado
                && r.EstadoCodigo == ConstanCodigoEstadoComite.Con_Acta_De_Sesion_Aprobada
                )
                    .ToListAsync(); 
            List<Dominio> ListasParametricas = _context.Dominio.ToList();

            List<Contratacion> ListContratacion = _context
                .Contratacion
                .Where(r => !(bool)r.Eliminado)
                //.Include(r => r.ContratacionProyecto)
                //.ThenInclude(r => r.Proyecto)
                //.ThenInclude(r => r.DisponibilidadPresupuestalProyecto)
                //    .ThenInclude(r => r.Proyecto) 
                .ToList(); 
            List<Contratista> ListContratista = _context.Contratista.ToList();
             
            foreach (var sesionComiteSolicitud in ListSesionComiteSolicitud)
            {
                switch (sesionComiteSolicitud.TipoSolicitudCodigo)
                {
                    case ConstanCodigoTipoSolicitud.Contratacion:
                        Contratacion contratacion = ListContratacion.Where(r => r.ContratacionId == sesionComiteSolicitud.SolicitudId).FirstOrDefault();

                      //  sesionComiteSolicitud.Contratacion = contratacion;

                        sesionComiteSolicitud.FechaSolicitud = (DateTime)contratacion.FechaTramite;

                        sesionComiteSolicitud.NumeroSolicitud = contratacion.NumeroSolicitud;

                        sesionComiteSolicitud.TipoSolicitud = ListasParametricas
                            .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud
                            && r.Codigo == ConstanCodigoTipoSolicitud.Contratacion
                            ).FirstOrDefault().Nombre;

                        if (!(bool)contratacion.RegistroCompleto || contratacion.RegistroCompleto == null)
                        {
                            sesionComiteSolicitud.EstadoDelRegistro = "Incompleto";
                        }
                        else
                        {
                            sesionComiteSolicitud.EstadoDelRegistro = "Completo";
                        } 
                        break;

                    default:
                        break;
                }
            }
            return ListSesionComiteSolicitud.OrderByDescending(r=> r.SesionComiteSolicitudId).ToList();
        }

        public async Task<Contratacion> GetContratacionByContratacionId(int pContratacionId) {
             
            //TODO: PENDIENTE Numero comite Fiducuario Fecha Comite Fiducuario
            return await _context.Contratacion.Where(r => r.ContratacionId == pContratacionId)
                    .Include(r => r.DisponibilidadPresupuestal)
                    .Include(r => r.Contratista)
                    .Include(r => r.ContratacionProyecto)
                        .ThenInclude(r => r.Proyecto)
                               .ThenInclude(r => r.ProyectoAportante) 
                                  .ThenInclude(r => r.Aportante)
                                  
                     .FirstOrDefaultAsync();

        }
    }
}
