using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using asivamosffie.services.Helpers.Constants;

namespace asivamosffie.services
{
    public class DerivativeActionService: IDerivativeActionService
    {
        //seguimiento/reclamacion controversia actuacion derivada - redux: actuacion derivada

        private readonly devAsiVamosFFIEContext _context;

        private readonly ICommonService _commonService;

        public DerivativeActionService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }

        private bool ValidarRegistroCompletoSeguimientoActuacionDerivada(SeguimientoActuacionDerivada seguimientoActuacionDerivada)
        {
            
            if (string.IsNullOrEmpty(seguimientoActuacionDerivada.Observaciones)
                || string.IsNullOrEmpty(seguimientoActuacionDerivada.RutaSoporte)
                || string.IsNullOrEmpty(seguimientoActuacionDerivada.DescripciondeActuacionAdelantada.ToString())
                //|| !string.IsNullOrEmpty(sesionComiteTemaOld.RutaSoporte)
                || string.IsNullOrEmpty(seguimientoActuacionDerivada.EstadoActuacionDerivadaCodigo)
                               || (seguimientoActuacionDerivada.EsRequiereFiduciaria == null)             

                )
            {

                return false;
            }

            return true;

        }
        public async Task<List<GrillaTipoActuacionDerivada>> ListGrillaTipoActuacionDerivada()
        {
            //await AprobarContratoByIdContrato(1);

            List<GrillaTipoActuacionDerivada> LstActuacionDerivadaGrilla = new List<GrillaTipoActuacionDerivada>();
            //Fecha de firma del contrato ??? FechaFirmaContrato , [Contrato] -(dd / mm / aaaa)

            //Tipo de solicitud ??? ContratoPoliza - TipoSolicitudCodigo      

            List<ControversiaActuacion> lstControversiaActuacion = await _context.ControversiaActuacion
                .Where(r=>!(bool)r.Eliminado)                .ToListAsync();
            //List<ControversiaContractual> ListControversiaContractualGrilla = await _context.ControversiaContractual.Where(r => !(bool)r.EstadoCodigo).Distinct().ToListAsync();
            List<SeguimientoActuacionDerivada> lstActuacionDerivada;

            //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
            string strEstadoActuacionDerivadaCodigo = "1";
            string strEstadoActuacionDerivada = "Sin reporte";
            //string strEstadoAvanceTramite = "sin definir";

            //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
            Dominio EstadoActuacionDerivada;

            foreach (var controversiaActuacion in lstControversiaActuacion)
            {
                try
                {
                    lstActuacionDerivada = await _context.SeguimientoActuacionDerivada
                .Where(r => !(bool)r.Eliminado &&r.ControversiaActuacionId==controversiaActuacion.ControversiaActuacionId).ToListAsync();

                    if(lstActuacionDerivada.Count()>0)
                    {

                        foreach (var actuacionDerivada in lstActuacionDerivada)
                        {

                            //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                             strEstadoActuacionDerivadaCodigo = "1";
                             strEstadoActuacionDerivada = "Sin reporte";
                            //string strEstadoAvanceTramite = "sin definir";

                            //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                            //EstadoActuacionDerivada = null;

                            EstadoActuacionDerivada = await _commonService.GetDominioByNombreDominioAndTipoDominio(actuacionDerivada.EstadoActuacionDerivadaCodigo, (int)EnumeratorTipoDominio.Estado_Actuacion_Derivada_4_4_1);
                            if (EstadoActuacionDerivada != null)
                            {
                                strEstadoActuacionDerivada = EstadoActuacionDerivada.Nombre;
                                strEstadoActuacionDerivadaCodigo = EstadoActuacionDerivada.Codigo;

                            }

                            //EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                            //if (EstadoSolicitudCodigoContratoPoliza != null)
                            //    strEstadoSolicitudCodigoContratoPoliza = EstadoSolicitudCodigoContratoPoliza.Nombre;

                            //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                            GrillaTipoActuacionDerivada RegistroActuacionDerivada = new GrillaTipoActuacionDerivada
                            {
                                FechaActualizacion = actuacionDerivada.FechaActuacionDerivada != null ? Convert.ToDateTime(actuacionDerivada.FechaActuacionDerivada).ToString("dd/MM/yyyy") : actuacionDerivada.FechaActuacionDerivada.ToString(),

                                Actuacion = "Actuación " + controversiaActuacion.ControversiaActuacionId.ToString("0000"),
                                NumeroActuacion="ACT_derivada"+actuacionDerivada.SeguimientoActuacionDerivadaId.ToString("0000"),
                                EstadoRegistroActuacionDerivada = (bool)actuacionDerivada.EsCompleto ? "Completo" : "Incompleto",
                                EstadoActuacionDerivada=strEstadoActuacionDerivada,
                                EstadoActuacionDerivadaCodigo=strEstadoActuacionDerivadaCodigo,
                                
                                ControversiaActuacionId=controversiaActuacion.ControversiaActuacionId,
                                SeguimientoActuacionDerivadaId=actuacionDerivada.SeguimientoActuacionDerivadaId,


                            };

                            //if (!(bool)proyecto.RegistroCompleto)
                            //{
                            //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                            //}
                            LstActuacionDerivadaGrilla.Add(RegistroActuacionDerivada);
                        }
                    }

                    else
                    {
                        GrillaTipoActuacionDerivada RegistroActuacionDerivada = new GrillaTipoActuacionDerivada
                        {
                            FechaActualizacion = controversiaActuacion.FechaModificacion != null ? Convert.ToDateTime(controversiaActuacion.FechaModificacion).ToString("dd/MM/yyyy") : controversiaActuacion.FechaModificacion.ToString(),

                            Actuacion = "Actuación " + controversiaActuacion.ControversiaActuacionId.ToString("0000"),
                            NumeroActuacion = "ACT_derivada" + "0000",
                            EstadoRegistroActuacionDerivada = (bool)controversiaActuacion.EsCompleto ? "Completo" : "Incompleto",
                            EstadoActuacionDerivada = strEstadoActuacionDerivada,
                            EstadoActuacionDerivadaCodigo = strEstadoActuacionDerivadaCodigo,

                            ControversiaActuacionId = controversiaActuacion.ControversiaActuacionId,
                            SeguimientoActuacionDerivadaId = 0,


                        };

                        //if (!(bool)proyecto.RegistroCompleto)
                        //{
                        //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                        //}
                        LstActuacionDerivadaGrilla.Add(RegistroActuacionDerivada);
                    }
                    
                }
                catch (Exception e)
                {
                    GrillaTipoActuacionDerivada RegistroActuacionSeguimiento = new GrillaTipoActuacionDerivada
                    {                   
                        FechaActualizacion = "ERROR",
                        Actuacion = "Actuación " + controversiaActuacion.ControversiaActuacionId.ToString("0000"),
                        NumeroActuacion = e.InnerException.ToString(),
                        EstadoRegistroActuacionDerivada = e.ToString(),
                        EstadoActuacionDerivada = "ERROR",
                        EstadoActuacionDerivadaCodigo = "ERROR",

                        ControversiaActuacionId = 0,
                        SeguimientoActuacionDerivadaId = 0,                       

                    };
                    LstActuacionDerivadaGrilla.Add(RegistroActuacionSeguimiento);
                }
            }
            return LstActuacionDerivadaGrilla.OrderByDescending(r => r.SeguimientoActuacionDerivadaId).ToList();

        }

        //tomado de controversia contractual

        public async Task<List<GrillaTipoActuacionDerivada>> ListGrillaTipoActuacionDerivada2()
        {
            //await AprobarContratoByIdContrato(1);

            List<GrillaTipoActuacionDerivada> LstActuacionDerivadaGrilla = new List<GrillaTipoActuacionDerivada>();
            //Fecha de firma del contrato ??? FechaFirmaContrato , [Contrato] -(dd / mm / aaaa)

            //Tipo de solicitud ??? ContratoPoliza - TipoSolicitudCodigo      

            List<ControversiaActuacion> lstControversiaActuacion;
            //List<ControversiaContractual> ListControversiaContractualGrilla = await _context.ControversiaContractual.Where(r => !(bool)r.EstadoCodigo).Distinct().ToListAsync();
            List<SeguimientoActuacionDerivada> lstActuacionDerivada;

            //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
            string strEstadoActuacionDerivadaCodigo = "1";
            string strEstadoActuacionDerivada = "Sin reporte";
            //string strEstadoAvanceTramite = "sin definir";


            List<GrillaTipoSolicitudControversiaContractual> ListControversiaContractualGrilla = new List<GrillaTipoSolicitudControversiaContractual>();
            //Fecha de firma del contrato ??? FechaFirmaContrato , [Contrato] -(dd / mm / aaaa)

            //Tipo de solicitud ??? ContratoPoliza - TipoSolicitudCodigo      

            //List<ControversiaContractual> ListControversiaContractualGrilla = await _context.ControversiaContractual.Where(r => !(bool)r.EstadoCodigo).Distinct().ToListAsync();
            List<ControversiaContractual> ListControversiaContractual = await _context.ControversiaContractual.Distinct().ToListAsync();


            //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
            Dominio EstadoActuacionDerivada;

            foreach (var controversiaContractual in ListControversiaContractual)
            {
                try
                {

                     lstControversiaActuacion = await _context.ControversiaActuacion
              .Where(r => !(bool)r.Eliminado && r.ControversiaContractualId == controversiaContractual.ControversiaContractualId).ToListAsync();

                    if (lstControversiaActuacion.Count() > 0)
                    {

                        foreach (var controversiaActuacion in lstControversiaActuacion)
                        {

                            Contrato contrato = null;

                            //contrato = await _commonService.GetContratoPolizaByContratoId(controversia.ContratoId);
                            contrato = _context.Contrato.Where(r => r.ContratoId == controversiaContractual.ContratoId).FirstOrDefault();

                            //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                            string strEstadoCodigoControversia = "sin definir";
                            string strTipoControversiaCodigo = "sin definir";
                            string strTipoControversia = "sin definir";

                            //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                            Dominio EstadoCodigoControversia;
                            Dominio TipoControversiaCodigo;


                            TipoControversiaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaContractual.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipo_de_controversia);
                            if (TipoControversiaCodigo != null)
                            {
                                strTipoControversiaCodigo = TipoControversiaCodigo.Nombre;
                                strTipoControversia = TipoControversiaCodigo.Codigo;

                            }

                            //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                            GrillaTipoSolicitudControversiaContractual RegistroControversiaContractual = new GrillaTipoSolicitudControversiaContractual
                            {
                                ControversiaContractualId = controversiaActuacion.ControversiaContractualId,
                                NumeroSolicitud = controversiaContractual.NumeroSolicitud,
                                //FechaSolicitud=controversia.FechaSolicitud,
                                FechaSolicitud = controversiaContractual.FechaSolicitud != null ? Convert.ToDateTime(controversiaContractual.FechaSolicitud).ToString("dd/MM/yyyy") : controversiaContractual.FechaSolicitud.ToString(),
                                TipoControversia = strTipoControversia,
                                TipoControversiaCodigo = strTipoControversiaCodigo,
                                ContratoId = contrato.ContratoId,
                                EstadoControversia = "PENDIENTE",
                                RegistroCompletoNombre = (bool)controversiaActuacion.EsCompleto ? "Completo" : "Incompleto",

                                //cu 4.4.1
                                //Actuacion = "Actuación " + actuacionSeguimiento.ActuacionSeguimientoId.ToString()
                                Actuacion = controversiaActuacion.ActuacionAdelantadaCodigo,
                                FechaActuacion =  controversiaActuacion.FechaActuacion != null ?Convert.ToDateTime(controversiaActuacion.FechaActuacion).ToString("dd/MM/yyyy"): "",
                                EstadoActuacion = controversiaActuacion.EstadoActuacionReclamacionCodigo,


                            };
                        }
                    }

                    //else    //sin registro
                    //{
                    //    GrillaTipoActuacionDerivada RegistroActuacionDerivada = new GrillaTipoActuacionDerivada
                    //    {
                    //        FechaActualizacion = controversiaContractual.FechaModificacion != null ? Convert.ToDateTime(controversiaContractual.FechaModificacion).ToString("dd/MM/yyyy") : controversiaContractual.FechaModificacion.ToString(),

                    //        Actuacion = "Actuación " + controversiaContractual.ControversiaActuacionId.ToString("0000"),
                    //        NumeroActuacion = "ACT_derivada" + "0000",
                    //        EstadoRegistroActuacionDerivada = (bool)controversiaContractual.EsCompleto ? "Completo" : "Incompleto",
                    //        EstadoActuacionDerivada = strEstadoActuacionDerivada,
                    //        EstadoActuacionDerivadaCodigo = strEstadoActuacionDerivadaCodigo,

                    //        ControversiaActuacionId = controversiaContractual.ControversiaActuacionId,
                    //        SeguimientoActuacionDerivadaId = 0,


                    //    };

                    //    //if (!(bool)proyecto.RegistroCompleto)
                    //    //{
                    //    //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                    //    //}
                    //    LstActuacionDerivadaGrilla.Add(RegistroActuacionDerivada);
                    //}

                }
                catch (Exception e)
                {
                    GrillaTipoActuacionDerivada RegistroActuacionSeguimiento = new GrillaTipoActuacionDerivada
                    {
                        FechaActualizacion = "ERROR",
                        Actuacion = "Actuación " + controversiaContractual.ControversiaContractualId.ToString("0000"),
                        NumeroActuacion = e.InnerException.ToString(),
                        EstadoRegistroActuacionDerivada = e.ToString(),
                        EstadoActuacionDerivada = "ERROR",
                        EstadoActuacionDerivadaCodigo = "ERROR",

                        ControversiaActuacionId = 0,
                        SeguimientoActuacionDerivadaId = 0,

                    };
                    LstActuacionDerivadaGrilla.Add(RegistroActuacionSeguimiento);
                }
            }
            return LstActuacionDerivadaGrilla.OrderByDescending(r => r.SeguimientoActuacionDerivadaId).ToList();

        }
        public async Task<Respuesta> EliminarControversiaActuacionDerivada(int pId, string pUsuario)
        {
            Respuesta respuesta = new Respuesta(); 
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Controversia_Actuacion_Derivada, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            //SeguimientoActuacionDerivada
            SeguimientoActuacionDerivada actuacionDerivada= null;

            try
            {
                actuacionDerivada = await _context.SeguimientoActuacionDerivada
                            .Where(d => d.ControversiaActuacionId == pId).FirstOrDefaultAsync();

                if (actuacionDerivada != null)
                {
                    strCrearEditar = "Eliminar CONTROVERSIA ACTUACION DERIVADA";
                    actuacionDerivada.FechaModificacion = DateTime.Now;
                    //controversiaActuacion.UsuarioCreacion = disponibilidadPresupuestal.UsuarioCreacion;
                    actuacionDerivada.UsuarioModificacion = pUsuario;
                    actuacionDerivada.Eliminado = true;
                    _context.SeguimientoActuacionDerivada.Update(actuacionDerivada);

                    _context.SaveChanges();

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = actuacionDerivada
,
                    Code = ConstantMessagesDerivativeAction.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_actuaciones_controversias_contractuales, ConstantMessagesDerivativeAction.EliminacionExitosa, idAccion, actuacionDerivada.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = actuacionDerivada
,
                    Code = ConstantMessagesDerivativeAction.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_actuaciones_controversias_contractuales, ConstantMessagesDerivativeAction.Error, idAccion, actuacionDerivada
.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

                
        //Usuario Jurídica Controversias contractuales

        //estado de la actuación derivada sea "Cumplida" y el estado del registro sea completo
        public async Task<Respuesta> EnviarCorreoJuridica(int pControversiaContractualId, AppSettingsService settings)
        {
            Respuesta respuesta = new Respuesta();
            string fechaFirmaContrato = "";
            string correo = "cdaza@ivolucion.com";

            ControversiaActuacion controversiaActuacion=null;

            ControversiaContractual controversiaContractual=_context.ControversiaContractual
                .Where(r=>r.ControversiaContractualId== pControversiaContractualId).FirstOrDefault();
                        
            if (controversiaContractual != null)
            {
                controversiaActuacion = _context.ControversiaActuacion
             .Where(r => r.ControversiaContractualId == controversiaContractual.ControversiaContractualId).FirstOrDefault();
            }

            SeguimientoActuacionDerivada actuacionDerivada=null;
            if (controversiaActuacion != null)
            {
                 actuacionDerivada = _context.SeguimientoActuacionDerivada
                .Where(r => r.ControversiaActuacionId == controversiaActuacion.ControversiaActuacionId).FirstOrDefault();

            }
            int perfilId = 0;

            perfilId = (int)EnumeratorPerfil.Juridica; //  Supervisor
            //correo = getCorreos(perfilId);

            try
            {
                int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesDerivativeAction.CorreoEnviado, (int)EnumeratorTipoDominio.Acciones);
                                                         

                int pIdTemplate = (int)enumeratorTemplate.AlertaJuridicaDerivadaCumplida4_4_1;                                                             

                //PolizaObservacion polizaObservacion;           
                correo = "cdaza@ivolucion.com";

                //Task<Respuesta> result = EnviarCorreoGestionPoliza(correo, settings.MailServer,
                //settings.MailPort, settings.Password, settings.Sender,
                //objVistaContratoGarantiaPoliza, fechaFirmaContrato, pIdTemplate, msjNotificacion);

                bool blEnvioCorreo = false;
                //Respuesta respuesta = new Respuesta();

                Template TemplateRecoveryPassword = await _commonService.GetTemplateById(pIdTemplate);

                string template = TemplateRecoveryPassword.Contenido;

                Contrato contrato;
                contrato = _context.Contrato.Where(r => r.ContratoId == controversiaContractual.ContratoId).FirstOrDefault();

                string strTipoControversiaCodigo = "";
                string strTipoControversia = "";

                Dominio TipoControversiaCodigo;


                TipoControversiaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaContractual.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipo_de_controversia);
                if (TipoControversiaCodigo != null)
                {
                    strTipoControversiaCodigo = TipoControversiaCodigo.Codigo;
                    strTipoControversia =  TipoControversiaCodigo.Nombre;
                }

                template = template.Replace("_Numero_Contrato_", contrato.NumeroContrato);
                template = template.Replace("_Fecha_solicitud_controversia_", Convert.ToDateTime(controversiaContractual.FechaSolicitud).ToString("dd/MM/yyyy"));                                    
                template = template.Replace("_Numero_solicitud_", controversiaContractual.NumeroSolicitud);
                template = template.Replace("_Tipo_controversia_", strTipoControversia);  //fomato miles .                
                DateTime? fechaNull = null;
                fechaNull = actuacionDerivada != null ? actuacionDerivada.FechaActuacionDerivada : null;
                template = template.Replace("_Fecha_actuacion_derivada_", fechaNull == null ? "" : Convert.ToDateTime(fechaNull).ToString("dd/MM/yyyy"));
                                
                template = template.Replace("_Descripcion_actuacion_adelantada_", actuacionDerivada.DescripciondeActuacionAdelantada);
                               
                List<UsuarioPerfil> lstUsuariosPerfil = new List<UsuarioPerfil>();

                lstUsuariosPerfil = _context.UsuarioPerfil.Where(r => r.Activo == true && r.PerfilId == perfilId).ToList();

                List<Usuario> lstUsuarios = new List<Usuario>();

                foreach (var item in lstUsuariosPerfil)
                {
                    lstUsuarios = _context.Usuario.Where(r => r.UsuarioId == item.UsuarioId).ToList();

                    foreach (var usuario in lstUsuarios)
                    {                        

                        blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuario.Email, "Gestionar controversias contractuales", template, settings.Sender, settings.Password, settings.MailServer, settings.MailPort);
                    }
                }

                if (blEnvioCorreo)
                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesDerivativeAction.CorreoEnviado };

                else
                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesDerivativeAction.ErrorEnviarCorreo };

                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_actuaciones_controversias_contractuales, respuesta.Code, Convert.ToInt32( ConstantCodigoAcciones.Notificacion_Actuacion_Derivada), correo, "Gestionar controversias contractuales");
                //return respuesta;

                //blEnvioCorreo = Helpers.Helpers.EnviarCorreo(lstMails, "Gestión Poliza", template, pSentender, pPassword, pMailServer, pMailPort);

                //if (blEnvioCorreo)
                //    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.CorreoEnviado };

                //else
                //    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.ErrorEnviarCorreo };

                //}
                //}
                //else
                //{
                //    respuesta = new Respuesta() { IsSuccessful = true, IsValidation = true, Code = ConstantMessagesContratoPoliza.CorreoNoExiste };

                //}
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_actuaciones_controversias_contractuales, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificacion_Actuacion_Derivada), correo, "Gestionar controversias contractuales");
                return respuesta;

            }
            catch (Exception ex)
            {

                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesUsuarios.ErrorGuardarCambios };
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_actuaciones_controversias_contractuales, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificacion_Actuacion_Derivada), correo, "Gestionar controversias contractuales") + ": " + ex.ToString() + ex.InnerException;
                return respuesta;
            }

        }


        public async Task<Respuesta> CambiarEstadoControversiaActuacionDerivada(int pActuacionDerivadaId, string pCodigoEstado, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_actuacion_derivada, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                //{[EstadoActuacionDerivadaCodigo]
                //SeguimientoActuacionDerivada actuacionDerivada
                SeguimientoActuacionDerivada actuacionDerivadaOld;
                actuacionDerivadaOld = _context.SeguimientoActuacionDerivada.Find(pActuacionDerivadaId);
                actuacionDerivadaOld.UsuarioModificacion = pUsuarioModifica;
                actuacionDerivadaOld.FechaModificacion = DateTime.Now;
                actuacionDerivadaOld.EstadoActuacionDerivadaCodigo = pCodigoEstado;

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesDerivativeAction.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_actuaciones_controversias_contractuales, ConstantMessagesDerivativeAction.OperacionExitosa, idAccion, pUsuarioModifica, "CAMBIAR ESTADO CONTROVERSIA ACTUACION DERIVADA")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesDerivativeAction.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_actuaciones_controversias_contractuales, ConstantMessagesDerivativeAction.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }

        }

        public async Task<SeguimientoActuacionDerivada> GetSeguimientoActuacionDerivadaById(int id)
        {
            return await _context.SeguimientoActuacionDerivada.FindAsync(id);
        }

        //public async Task<Respuesta> InsertEditPolizaObservacion(PolizaObservacion polizaObservacion, AppSettingsService appSettingsService)
        //public async Task<Respuesta> CreateEditarSeguimientoActuacionDerivada(SeguimientoActuacionDerivada seguimientoActuacionDerivada)
        public async Task<Respuesta> CreateEditarSeguimientoActuacionDerivada(SeguimientoActuacionDerivada seguimientoActuacionDerivada, AppSettingsService settings)
        {
            Respuesta _response = new Respuesta();
            
            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_seguimiento_actuacion_derivada, (int)EnumeratorTipoDominio.Acciones);

            //try
            //{
            //    Respuesta respuesta = new Respuesta();
            //    string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
            //    respuesta = await _Cofinancing.EliminarCofinanciacionByCofinanciacionId(pCofinancicacionId, pUsuarioModifico);

            //    return Ok(respuesta);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.ToString());
            //}
            string strCrearEditar = string.Empty;
            try
            {
                if (seguimientoActuacionDerivada != null)
                {
                    if (string.IsNullOrEmpty(seguimientoActuacionDerivada.SeguimientoActuacionDerivadaId.ToString()) || seguimientoActuacionDerivada.SeguimientoActuacionDerivadaId == 0)
                    {
                        strCrearEditar = "REGISTRAR SEGUIMIENTO ACTUACION DERIVADA";

                        //Auditoria                        
                        seguimientoActuacionDerivada.FechaCreacion = DateTime.Now;
                        //controversiaActuacion.UsuarioCreacion = compromisoSeguimiento.UsuarioCreacion;
                        seguimientoActuacionDerivada.DescripciondeActuacionAdelantada = Helpers.Helpers.CleanStringInput(seguimientoActuacionDerivada.DescripciondeActuacionAdelantada);
                        seguimientoActuacionDerivada.Observaciones = Helpers.Helpers.CleanStringInput(seguimientoActuacionDerivada.Observaciones);

                        seguimientoActuacionDerivada.EsCompleto = ValidarRegistroCompletoSeguimientoActuacionDerivada(seguimientoActuacionDerivada);

                        //controversiaContractual.Eliminado = false;
                        _context.SeguimientoActuacionDerivada.Add(seguimientoActuacionDerivada);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        strCrearEditar = "EDITAR SEGUIMIENTO ACTUACION DERIVADA";

                        SeguimientoActuacionDerivada seguimientoActuacionDerivadaOld = _context.SeguimientoActuacionDerivada
                            .Where(r => r.SeguimientoActuacionDerivadaId == seguimientoActuacionDerivada.SeguimientoActuacionDerivadaId).FirstOrDefault();

                        if (seguimientoActuacionDerivadaOld != null)
                        {
                            seguimientoActuacionDerivadaOld.DescripciondeActuacionAdelantada = Helpers.Helpers.CleanStringInput(seguimientoActuacionDerivada.DescripciondeActuacionAdelantada);
                            seguimientoActuacionDerivadaOld.Observaciones = Helpers.Helpers.CleanStringInput(seguimientoActuacionDerivada.Observaciones);
                            //ControversiaContractual.FechaCreacion = DateTime.Now;
                            //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;
                            //contratoPoliza.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;

                            //_context.Add(contratoPoliza);

                            //contratoPoliza.RegistroCompleo = ValidarRegistroCompletoContratoPoliza(contratoPoliza);
                            //contratoPoliza.ObservacionesRevisionGeneral = ValidarRegistroCompleto(cofinanciacion);
                            seguimientoActuacionDerivadaOld.EsRequiereFiduciaria = seguimientoActuacionDerivada.EsRequiereFiduciaria;
                            seguimientoActuacionDerivadaOld.EstadoActuacionDerivadaCodigo = seguimientoActuacionDerivada.EstadoActuacionDerivadaCodigo;
                            seguimientoActuacionDerivadaOld.RutaSoporte = seguimientoActuacionDerivada.RutaSoporte;

                            //LimpiarEntradasContratoPoliza(ref contratoPoliza);
                            seguimientoActuacionDerivadaOld.FechaModificacion = DateTime.Now;
                            seguimientoActuacionDerivadaOld.UsuarioModificacion = seguimientoActuacionDerivada.UsuarioCreacion;
                            //_context.ContratoPoliza.Add(contratoPoliza);
                            _context.SeguimientoActuacionDerivada.Update(seguimientoActuacionDerivadaOld);
                            await _context.SaveChangesAsync();

                        }

                    }

                    //enviar correo
                    //estado de la actuación derivada sea "Cumplida" 
                    //y el estado del registro sea completo
                    if (seguimientoActuacionDerivada.EstadoActuacionDerivadaCodigo == ConstanCodigoEstadoActuacionDerivada.Cumplida
                                            && seguimientoActuacionDerivada.EsCompleto==true) {                                                
                    
                        Respuesta respuesta = new Respuesta();
                        string fechaFirmaContrato = "";
                        string correo = "cdaza@ivolucion.com";

                        ControversiaActuacion controversiaActuacion = null;

                        ControversiaContractual controversiaContractual = null;

                        if (controversiaContractual != null)
                        {
                            controversiaActuacion = _context.ControversiaActuacion
                         .Where(r => r.ControversiaContractualId == controversiaContractual.ControversiaContractualId).FirstOrDefault();
                        }

                        SeguimientoActuacionDerivada actuacionDerivada = null;
                        if (controversiaActuacion != null)
                        {
                            actuacionDerivada = _context.SeguimientoActuacionDerivada
                           .Where(r => r.ControversiaActuacionId == controversiaActuacion.ControversiaActuacionId).FirstOrDefault();

                            controversiaContractual = _context.ControversiaContractual
                           .Where(r => r.ControversiaContractualId == controversiaActuacion.ControversiaContractualId).FirstOrDefault();

                        }
                        int perfilId = 0;

                        perfilId = (int)EnumeratorPerfil.Juridica; //  Supervisor
                                                                   //correo = getCorreos(perfilId);

                    //try
                    //{
                        int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesDerivativeAction.CorreoEnviado, (int)EnumeratorTipoDominio.Acciones);


                        int pIdTemplate = (int)enumeratorTemplate.AlertaJuridicaDerivadaCumplida4_4_1;

                        //PolizaObservacion polizaObservacion;           
                        correo = "cdaza@ivolucion.com";

                        //Task<Respuesta> result = EnviarCorreoGestionPoliza(correo, settings.MailServer,
                        //settings.MailPort, settings.Password, settings.Sender,
                        //objVistaContratoGarantiaPoliza, fechaFirmaContrato, pIdTemplate, msjNotificacion);

                        bool blEnvioCorreo = false;
                        //Respuesta respuesta = new Respuesta();

                        Template TemplateRecoveryPassword = await _commonService.GetTemplateById(pIdTemplate);

                        string template = TemplateRecoveryPassword.Contenido;

                        Contrato contrato;
                        contrato = _context.Contrato.Where(r => r.ContratoId == controversiaContractual.ContratoId).FirstOrDefault();

                        string strTipoControversiaCodigo = "";
                        string strTipoControversia = "";

                        Dominio TipoControversiaCodigo;

                        TipoControversiaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaContractual.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipo_de_controversia);
                        if (TipoControversiaCodigo != null)
                        {
                            strTipoControversiaCodigo = TipoControversiaCodigo.Codigo;
                            strTipoControversia = TipoControversiaCodigo.Nombre;
                        }

                        template = template.Replace("_Numero_Contrato_", contrato.NumeroContrato);
                        template = template.Replace("_Fecha_solicitud_controversia_", Convert.ToDateTime(controversiaContractual.FechaSolicitud).ToString("dd/MM/yyyy"));
                        template = template.Replace("_Numero_solicitud_", controversiaContractual.NumeroSolicitud);
                        template = template.Replace("_Tipo_controversia_", strTipoControversia);  //fomato miles .                
                        DateTime? fechaNull = null;
                        fechaNull = actuacionDerivada != null ? actuacionDerivada.FechaActuacionDerivada : null;
                        template = template.Replace("_Fecha_actuacion_derivada_", fechaNull == null ? "" : Convert.ToDateTime(fechaNull).ToString("dd/MM/yyyy"));

                        template = template.Replace("_Descripcion_actuacion_adelantada_", actuacionDerivada.DescripciondeActuacionAdelantada);

                        List<UsuarioPerfil> lstUsuariosPerfil = new List<UsuarioPerfil>();

                        lstUsuariosPerfil = _context.UsuarioPerfil.Where(r => r.Activo == true && r.PerfilId == perfilId).ToList();

                        List<Usuario> lstUsuarios = new List<Usuario>();

                        foreach (var item in lstUsuariosPerfil)
                        {
                            lstUsuarios = _context.Usuario.Where(r => r.UsuarioId == item.UsuarioId).ToList();

                            foreach (var usuario in lstUsuarios)
                            {

                                blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuario.Email, "Gestionar controversias contractuales", template, settings.Sender, settings.Password, settings.MailServer, settings.MailPort);
                            }
                        }

                        if (blEnvioCorreo)
                            respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesDerivativeAction.CorreoEnviado };

                        else
                            respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesDerivativeAction.ErrorEnviarCorreo };

                        respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_actuaciones_controversias_contractuales, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificacion_Actuacion_Derivada), correo, "Gestionar controversias contractuales");
                        //return respuesta;

                        //blEnvioCorreo = Helpers.Helpers.EnviarCorreo(lstMails, "Gestión Poliza", template, pSentender, pPassword, pMailServer, pMailPort);

                        //if (blEnvioCorreo)
                        //    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.CorreoEnviado };

                        //else
                        //    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.ErrorEnviarCorreo };

                        //}
                        //}
                        //else
                        //{
                        //    respuesta = new Respuesta() { IsSuccessful = true, IsValidation = true, Code = ConstantMessagesContratoPoliza.CorreoNoExiste };

                        //}
                        respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_actuaciones_controversias_contractuales, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificacion_Actuacion_Derivada), correo, "Gestionar controversias contractuales");
                        //return respuesta;
                    }

                    //}
                    //catch (Exception ex)
                    //{

                    //    respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesUsuarios.ErrorGuardarCambios };
                    //    respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_actuaciones_controversias_contractuales, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificacion_Actuacion_Derivada), correo, "Gestionar controversias contractuales") + ": " + ex.ToString() + ex.InnerException;
                    //    return respuesta;
                    //}

                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesDerivativeAction.OperacionExitosa,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_actuaciones_controversias_contractuales,
                            ConstantMessagesDerivativeAction.OperacionExitosa,
                            //contratoPoliza
                            idAccionCrearContratoPoliza
                            , seguimientoActuacionDerivada.UsuarioCreacion
                            //"UsuarioCreacion"
                            , strCrearEditar
                            //contratoPoliza.UsuarioCreacion, "REGISTRAR CONTRATO POLIZA"
                            )
                        };

                    //return _response = new Respuesta { IsSuccessful = true,
                    //    IsValidation = false, Data = cuentaBancaria,
                    //    Code = ConstantMessagesBankAccount.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesDerivativeAction.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesDerivativeAction.ErrorInterno };
            }

        }
    }

}
