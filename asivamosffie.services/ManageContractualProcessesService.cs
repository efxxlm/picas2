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
using Microsoft.AspNetCore.Http;
using System.IO;

namespace asivamosffie.services
{
    public class ManageContractualProcessesService : IManageContractualProcessesService
    {
        private readonly devAsiVamosFFIEContext _context;

        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;

        public ManageContractualProcessesService(devAsiVamosFFIEContext context, ICommonService commonService, IDocumentService documentService)
        {
            _context = context;
            _documentService = documentService;
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
            // 2   Aprobada por comité fiduciario


            List<SesionComiteSolicitud> ListSesionComiteSolicitud = await _context.SesionComiteSolicitud
                .Where(r => !(bool)r.Eliminado
                   && r.EstadoCodigo == ConstanCodigoEstadoSolicitudContratacion.Aprobada_comite_fiduciario 
                ).ToListAsync();




            ListSesionComiteSolicitud = ListSesionComiteSolicitud.Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion
            || r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Modificacion_Contractual).ToList();
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

                        sesionComiteSolicitud.EstadoDelRegistro = contratacion.RegistroCompleto.ToString();
                        //if (!(bool)contratacion.RegistroCompleto || contratacion.RegistroCompleto == null)
                        //{
                        //    sesionComiteSolicitud.EstadoDelRegistro = "Incompleto";
                        //}
                        //else
                        //{
                        //    sesionComiteSolicitud.EstadoDelRegistro = "Completo";
                        //}
                       break;

                    case ConstanCodigoTipoSolicitud.Modificacion_Contractual:

                        sesionComiteSolicitud.TipoSolicitud = ListasParametricas
                       .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud
                        && r.Codigo == ConstanCodigoTipoSolicitud.Modificacion_Contractual)
                       .FirstOrDefault().Nombre;
                        break;


                    default:
                        break;
                }
            }
            return ListSesionComiteSolicitud.OrderByDescending(r => r.SesionComiteSolicitudId).ToList();
        }

        public async Task<Contratacion> GetContratacionByContratacionId(int pContratacionId)
        {

            //TODO: PENDIENTE por FAber Numero comite Fiduciario Fecha Comite Fiduciario

            List<Dominio> LisParametricas = _context.Dominio.ToList();


            Contratacion contratacion = await _context.Contratacion.Where(r => r.ContratacionId == pContratacionId)
                      .Include(r => r.DisponibilidadPresupuestal)
                      .Include(r => r.Contratista)
                      .Include(r => r.ContratacionProyecto)
                          .ThenInclude(r => r.Proyecto)
                            .ThenInclude(r => r.ProyectoAportante) 
                    .Include(r => r.ContratacionProyecto)
                          .ThenInclude(r => r.Proyecto)
                              .ThenInclude(r => r.InstitucionEducativa)
                 .FirstOrDefaultAsync();

            SesionComiteSolicitud sesionComiteSolicitud = _context.SesionComiteSolicitud
                .Where(r => r.SolicitudId == contratacion.ContratacionId && r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion)
                .Include(r => r.ComiteTecnico).FirstOrDefault();

            if (sesionComiteSolicitud.ComiteTecnico.EsComiteFiduciario == null || !(bool)sesionComiteSolicitud.ComiteTecnico.EsComiteFiduciario)
            {
                sesionComiteSolicitud = null;
            }

            if (sesionComiteSolicitud != null)
            {
                if (sesionComiteSolicitud.FechaComiteFiduciario != null)
                {
                    contratacion.DisponibilidadPresupuestal.FirstOrDefault().FechaComiteFiduciario = ((DateTime)sesionComiteSolicitud.FechaComiteFiduciario).ToString("dd-MM-yy");
                }
                contratacion.DisponibilidadPresupuestal.FirstOrDefault().NumeroComiteFiduciario = sesionComiteSolicitud.ComiteTecnico.NumeroComite;
            }

            if (!string.IsNullOrEmpty(contratacion.Contratista.TipoIdentificacionCodigo))
            {
                contratacion.Contratista.TipoIdentificacionCodigo = LisParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Documento && r.Codigo == contratacion.Contratista.TipoIdentificacionCodigo).FirstOrDefault().Nombre;
            }


            return contratacion;
        }
         
        public async Task<Respuesta> RegistrarTramiteContratacion(Contratacion pContratacion, IFormFile pFile, string pDirectorioBase, string pDirectorioMinuta)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Registrar_Tramite_Contratacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                //Save Files  
                string strFilePatch = Path.Combine(pDirectorioBase, pDirectorioMinuta, pContratacion.ContratacionId.ToString());
                await _documentService.SaveFileContratacion(pFile, strFilePatch, pContratacion.ContratacionId);


                Contratacion contratacionOld = _context.Contratacion.Find(pContratacion.ContratacionId);
                //Auditoria
                contratacionOld.FechaModificacion = DateTime.Now;
                contratacionOld.UsuarioModificacion = pContratacion.UsuarioCreacion;
                //Registros
                contratacionOld.RutaMinuta = strFilePatch;
                contratacionOld.RegistroCompleto = pContratacion.RegistroCompleto;
                contratacionOld.FechaEnvioDocumentacion = pContratacion.FechaEnvioDocumentacion;
                contratacionOld.Observaciones = pContratacion.Observaciones;
                contratacionOld.RutaMinuta = pContratacion.RutaMinuta;

                await _context.SaveChangesAsync();

                return
                  new Respuesta
                  {
                      IsSuccessful = true,
                      IsException = false,
                      IsValidation = false,
                      Code = ConstantGestionarProcesosContractuales.OperacionExitosa,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantGestionarProcesosContractuales.OperacionExitosa, idAccion, pContratacion.UsuarioCreacion, "REGISTRAR SOLICITUD")
                  };

            }
            catch (Exception ex)
            {
                return
              new Respuesta
              {
                  IsSuccessful = false,
                  IsException = true,
                  IsValidation = false,
                  Code = ConstantGestionarProcesosContractuales.Error,
                  Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantGestionarProcesosContractuales.Error, idAccion, pContratacion.UsuarioCreacion, ex.InnerException.ToString())
              };
            }
        }


    }

}
