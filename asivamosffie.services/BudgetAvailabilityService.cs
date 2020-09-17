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

                DisponibilidadPresupuestalGrilla disponibilidadPresupuestalGrilla = new DisponibilidadPresupuestalGrilla
                {

                    FechaSolicitud = DisponibilidadPresupuestal.FechaSolicitud.ToString(),
                    EstadoRegistro = strEstadoRegistro,
                    TipoSolicitud = strTipoSolicitud,
                    DisponibilidadPresupuestalId = DisponibilidadPresupuestal.DisponibilidadPresupuestalId,
                    NumeroSolicitud = DisponibilidadPresupuestal.NumeroSolicitud
                };

                ListDisponibilidadPresupuestalGrilla.Add(disponibilidadPresupuestalGrilla);
            }

            return ListDisponibilidadPresupuestalGrilla.OrderByDescending(r => r.EstadoRegistro).ToList();
        }


        public async Task<DisponibilidadPresupuestal> GetDisponibilidadPresupuestalByID(int pDisponibilidadPresupuestalId)
        {
            //las tabla DisponibilidadPresupuestalProyecto no tiene campos de auditoria
            return await _context.DisponibilidadPresupuestal.Where(r => r.DisponibilidadPresupuestalId == pDisponibilidadPresupuestalId).Include(r => r.DisponibilidadPresupuestalProyecto).FirstOrDefaultAsync();

        }


        public async Task<FuenteFinanciacion> GetFuenteFinanciacionByIdAportanteId(int pAportanteId)
        { 
            return await _context.FuenteFinanciacion.Where(r => r.AportanteId == pAportanteId).IncludeFilter(r => r.GestionFuenteFinanciacion.Where(r => !(bool)r.Eliminado)).FirstOrDefaultAsync();
        }


        public async Task<List<DisponibilidadPresupuestalGrilla>> GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud(string pCodigoEstadoSolicitud) {

            List<DisponibilidadPresupuestal> ListDisponibilidadPresupuestal = await _context.DisponibilidadPresupuestal.Where(r => !(bool)r.Eliminado && r.EstadoSolicitudCodigo.Equals(pCodigoEstadoSolicitud)).ToListAsync();

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

                DisponibilidadPresupuestalGrilla disponibilidadPresupuestalGrilla = new DisponibilidadPresupuestalGrilla
                {

                    FechaSolicitud = DisponibilidadPresupuestal.FechaSolicitud.ToString(),
                    EstadoRegistro = strEstadoRegistro,
                    TipoSolicitud = strTipoSolicitud,
                    DisponibilidadPresupuestalId = DisponibilidadPresupuestal.DisponibilidadPresupuestalId,
                    NumeroSolicitud = DisponibilidadPresupuestal.NumeroSolicitud
                }; 
                ListDisponibilidadPresupuestalGrilla.Add(disponibilidadPresupuestalGrilla);
            } 
            return ListDisponibilidadPresupuestalGrilla.OrderByDescending(r => r.DisponibilidadPresupuestalId).ToList();

        }

        
        /*autor: jflorez
            descripci�n: objeto para entregar a front los datos ordenados de disponibilidades
        impacto: CU 3.3.3*/
        public async Task<List<EstadosDisponibilidad>> GetListGenerarDisponibilidadPresupuestal()
        {
            List<EstadosDisponibilidad> estadosdisponibles = new List<EstadosDisponibilidad>();
            var estados = _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud_Presupuestal && x.Activo == true).ToList();
            foreach (var estado in estados)
            {                
                estadosdisponibles.Add(new EstadosDisponibilidad{DominioId=estado.DominioId,NombreEstado=estado.Nombre,DisponibilidadPresupuestal=await this.GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud(estado.Codigo)});
            }
            return estadosdisponibles;
        }

        public async Task<Respuesta> SetCancelDisponibilidadPresupuestal(int pId,string pUsuarioModificacion, string pObservacion)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Find(pId);
            try
            {
                DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                DisponibilidadCancelar.UsuarioModificacion = pUsuarioModificacion;
                DisponibilidadCancelar.EstadoSolicitudCodigo = EnumeratorEstadoSolicitudPresupuestal.Con_disponibilidad_cancelada.ToString();
                DisponibilidadCancelar.Observacion = pObservacion;
               _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesGenerateBudget.CanceladoCorrrectamente,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.CanceladoCorrrectamente, pId, pUsuarioModificacion, "CANCELAR DISPONIBILIDAD PRESUPUESTAL")
                };
            }
            catch (Exception ex)
            {
                return
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesGenerateBudget.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, pId, pUsuarioModificacion, ex.InnerException.ToString().Substring(0, 500))
                             };
            }
        }

        public async Task<Respuesta> CreateDDP(int pId, string pUsuarioModificacion, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Find(pId);
            /*busco usuario Juridico*/
            var usuarioJuridico = _context.UsuarioPerfil.Where(x=>x.PerfilId==(int)enumeratorPerfil.Juridica).Include(y=>y.Usuario).FirstOrDefault();
            try
            {
                DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                DisponibilidadCancelar.UsuarioModificacion = pUsuarioModificacion;
                DisponibilidadCancelar.EstadoSolicitudCodigo = EnumeratorEstadoSolicitudPresupuestal.Con_validacion_presupuestal.ToString();
                
                _context.SaveChanges();
                //envio correo a juridica
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.DisponibilidadPresupuestalGenerada);
                string template = TemplateRecoveryPassword.Contenido;

                //template = template.Replace("_Link_", urlDestino);                

                bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioJuridico.Usuario.Email, "DDP Generado", template, pSentender, pPassword, pMailServer, pMailPort);
                if (blEnvioCorreo)
                {
                   return new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantMessagesGenerateBudget.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.OperacionExitosa, pId, pUsuarioModificacion, "GENERAR DDP DISPONIBILIDAD PRESUPUESTAL")
                    };
                }
                else
                {
                    return new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantMessagesGenerateBudget.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, pId, pUsuarioModificacion, "ERROR ENVIO MAIL GENERAR DDP DISPONIBILIDAD PRESUPUESTAL")
                    };
                }
            }
            catch (Exception ex)
            {
                return
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesGenerateBudget.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, pId, pUsuarioModificacion, ex.InnerException.ToString().Substring(0, 500))
                             };
            }
        }

        public async Task<Respuesta> returnDDP(int pId, string pUsuarioModificacion, string pObservacion)
        {
            var DisponibilidadCancelar = _context.DisponibilidadPresupuestal.Find(pId);
            try
            {
                DisponibilidadCancelar.FechaModificacion = DateTime.Now;
                DisponibilidadCancelar.UsuarioModificacion = pUsuarioModificacion;
                DisponibilidadCancelar.EstadoSolicitudCodigo = EnumeratorEstadoSolicitudPresupuestal.Devuelta_por_coordinacion_financiera.ToString();
                DisponibilidadCancelar.Observacion = pObservacion;
                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesGenerateBudget.DevueltoCorrrectamente,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.DevueltoCorrrectamente, pId, pUsuarioModificacion, "DEVOLVER DISPONIBILIDAD PRESUPUESTAL")
                };
            }
            catch (Exception ex)
            {
                return
                             new Respuesta
                             {
                                 IsSuccessful = true,
                                 IsException = false,
                                 IsValidation = false,
                                 Code = ConstantMessagesGenerateBudget.Error,
                                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GenerarDisponibilidadPresupuestal, ConstantMessagesGenerateBudget.Error, pId, pUsuarioModificacion, ex.InnerException.ToString().Substring(0, 500))
                             };
            }
        }

        public Task<byte[]> GetPDFDDP(int id, string pUsurioGenero)
        {
            Contratacion contratacion = await _IProjectContractingService.GetAllContratacionByContratacionId(pContratacionId);

            string TipoPlantilla = ((int)ConstanCodigoPlantillas.Ficha_De_Contratacion).ToString();

            Plantilla Plantilla = _context.Plantilla.Where(r => r.Codigo == TipoPlantilla).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();
            Plantilla.Contenido = ReemplazarDatosPlantillaContratacion(Plantilla.Contenido, contratacion);
            return PDF.Convertir(Plantilla);
        }

        //    public async Task<Respuesta> CreateEditDisponibilidadPresupuestal(DisponibilidadPresupuestal pDisponibilidadPresupuestal) {

        //        try
        //        {
        //            return
        //          new Respuesta
        //          {
        //              IsSuccessful = true,
        //              IsException = false,
        //              IsValidation = true,
        //              Code = ConstantMessagesProyecto.OperacionExitosa,
        //              Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.OperacionExitosa, idAccionCrearProyecto, pProyecto.UsuarioCreacion, "CREAR PROYECTO")
        //          };
        //        }
        //        catch (Exception)
        //        {
        //            return
        //      new Respuesta
        //      {
        //          IsSuccessful = false,
        //          IsException = true,
        //          IsValidation = false,
        //          Code = ConstantMessagesProyecto.Error,
        //          Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Proyecto, ConstantMessagesProyecto.Error, idAccionCrearProyecto, pProyecto.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
        //      };
        //        }

        //    }

    }
}