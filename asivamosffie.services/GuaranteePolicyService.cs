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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

//using asivamosffie.model.APIModels;
//using asivamosffie.model.Models;

using asivamosffie.api;

using asivamosffie.services.Helpers;


namespace asivamosffie.services
{
    public class GuaranteePolicyService : IGuaranteePolicyService
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        //private readonly IOptions<AppSettings> _settings;

        public GuaranteePolicyService(devAsiVamosFFIEContext context, ICommonService commonService)
        {

            _commonService = commonService;
            _context = context;
            //_settings = settings;
        }
        //public async Task<ActionResult<List<PolizaGarantia>>> GetListPolizaGarantiaByContratoPolizaId(int pContratoPolizaId)
        public async Task<List<PolizaGarantia>> GetListPolizaGarantiaByContratoPolizaId(int pContratoPolizaId)

        {
            return await _context.PolizaGarantia.Where(r => r.ContratoPolizaId == pContratoPolizaId).ToListAsync();
        }


        public async Task<ContratoPoliza> GetContratoPolizaByIdContratoPolizaId(int pContratoPolizaId)
        {

            //includefilter
            ContratoPoliza contratoPoliza = new ContratoPoliza();

            //contratoPoliza = _context.ContratoPoliza.Where(r => !(bool)r.Eliminado && r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();
            contratoPoliza = _context.ContratoPoliza.Where(r => r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();

            if (contratoPoliza != null)
            {
                //List<PolizaGarantia> contratoPolizaGarantia = await _context.PolizaGarantia.Where(r => r.ContratoPolizaId == p && !(bool)r.Eliminado).IncludeFilter(r => r.CofinanciacionDocumento.Where(r => !(bool)r.Eliminado)).ToListAsync();
                List<PolizaGarantia> contratoPolizaGarantia = await _context.PolizaGarantia.Where(r => r.ContratoPolizaId == pContratoPolizaId).ToListAsync();

                contratoPoliza.PolizaGarantia = contratoPolizaGarantia;
            }

            PolizaObservacion polizaObservacion = new PolizaObservacion();
            //contratoPoliza = _context.ContratoPoliza.Where(r => !(bool)r.Eliminado && r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();
            polizaObservacion = _context.PolizaObservacion.Where(r => r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();

            if (contratoPoliza != null)
            {
                //List<PolizaGarantia> contratoPolizaGarantia = await _context.PolizaGarantia.Where(r => r.ContratoPolizaId == p && !(bool)r.Eliminado).IncludeFilter(r => r.CofinanciacionDocumento.Where(r => !(bool)r.Eliminado)).ToListAsync();
                List<PolizaObservacion> contratoPolizaObservacion = await _context.PolizaObservacion.Where(r => r.ContratoPolizaId == pContratoPolizaId).ToListAsync();

                contratoPoliza.PolizaObservacion = contratoPolizaObservacion;
            }

            return contratoPoliza;
        }


        //public async Task<ActionResult<List<PolizaObservacion>>> GetListPolizaObservacionByContratoPolizaId(int pContratoPolizaId)
        public async Task<List<PolizaObservacion>> GetListPolizaObservacionByContratoPolizaId(int pContratoPolizaId)
        {

            return await _context.PolizaObservacion.Where(r => r.ContratoPolizaId == pContratoPolizaId).ToListAsync();
        }


        public async Task<Respuesta> InsertPolizaGarantia([FromBody] PolizaGarantia polizaGarantia)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Cuenta_Bancaria, (int)EnumeratorTipoDominio.Acciones);

            //GUARDAR
            //PolizaObservacion - FechaRevision
            //    EstadoRevisionCodigo - PolizaObservacion

            try
            {
                if (polizaGarantia != null)
                {
                    //contratoPoliza.FechaCreacion = DateTime.Now;
                    //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;


                    //_context.Add(contratoPoliza);

                    //contratoPoliza.ObservacionesRevisionGeneral = ValidarRegistroCompleto(cofinanciacion);

                    _context.PolizaGarantia.Add(polizaGarantia);
                    await _context.SaveChangesAsync();

                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContratoPoliza.CreadoCorrrectamente,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias,
                            ConstantMessagesContratoPoliza.CreadoCorrrectamente,
                            //contratoPoliza
                            1
                            ,
                            "UsuarioCreacion", "REGISTRAR POLIZA OBSERVACION"
                            //contratoPoliza.UsuarioCreacion, "REGISTRAR POLIZA GARANTIA"
                            )
                        };

                    //return _response = new Respuesta { IsSuccessful = true,
                    //    IsValidation = false, Data = cuentaBancaria,
                    //    Code = ConstantMessagesBankAccount.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.ErrorInterno };
            }

        }
        public async Task<Respuesta> InsertPolizaObservacion([FromBody] PolizaObservacion polizaObservacion)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Cuenta_Bancaria, (int)EnumeratorTipoDominio.Acciones);

            //GUARDAR
            //PolizaObservacion - FechaRevision
            //    EstadoRevisionCodigo - PolizaObservacion

            try
            {
                if (polizaObservacion != null)
                {
                    //contratoPoliza.FechaCreacion = DateTime.Now;
                    //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;

                    //_context.Add(contratoPoliza);

                    //contratoPoliza.ObservacionesRevisionGeneral = ValidarRegistroCompleto(cofinanciacion);
                    polizaObservacion.Observacion = Helpers.Helpers.CleanStringInput(polizaObservacion.Observacion);
                    
                    _context.PolizaObservacion.Add(polizaObservacion);
                    await _context.SaveChangesAsync();

                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContratoPoliza.CreadoCorrrectamente,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias,
                            ConstantMessagesContratoPoliza.CreadoCorrrectamente,
                            //contratoPoliza
                            1
                            ,
                            "UsuarioCreacion", "REGISTRAR POLIZA OBSERVACION"
                            //contratoPoliza.UsuarioCreacion, "REGISTRAR CONTRATO POLIZA"
                            )
                        };

                    //return _response = new Respuesta { IsSuccessful = true,
                    //    IsValidation = false, Data = cuentaBancaria,
                    //    Code = ConstantMessagesBankAccount.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.ErrorInterno };
            }

        }

        
        public async Task<Respuesta> EditarContratoPoliza([FromBody] ContratoPoliza contratoPoliza)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContratoPoliza.CreadoCorrrectamente, (int)EnumeratorTipoDominio.Acciones);

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


            try
            {
                if (contratoPoliza != null)
                {
                    contratoPoliza.FechaCreacion = DateTime.Now;
                    //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;
                    //contratoPoliza.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;

                    //_context.Add(contratoPoliza);

                    contratoPoliza.RegistroCompleo = ValidarRegistroCompletoContratoPoliza(contratoPoliza);
                    //contratoPoliza.ObservacionesRevisionGeneral = ValidarRegistroCompleto(cofinanciacion);

                    LimpiarEntradasContratoPoliza(ref contratoPoliza);

                    //_context.ContratoPoliza.Add(contratoPoliza);
                    _context.ContratoPoliza.Update(contratoPoliza);
                    //await _context.SaveChangesAsync();


                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias,
                            ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente,
                            //contratoPoliza
                            idAccionCrearContratoPoliza
                            , contratoPoliza.UsuarioCreacion
                            //"UsuarioCreacion"
                            , "EDITAR CONTRATO POLIZA"
                            //contratoPoliza.UsuarioCreacion, "REGISTRAR CONTRATO POLIZA"
                            )
                        };

                    //return _response = new Respuesta { IsSuccessful = true,
                    //    IsValidation = false, Data = cuentaBancaria,
                    //    Code = ConstantMessagesBankAccount.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.ErrorInterno };
            }

        }

        public async Task<Respuesta> InsertContratoPoliza([FromBody] ContratoPoliza contratoPoliza, AppSettingsService appSettingsService)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContratoPoliza.CreadoCorrrectamente, (int)EnumeratorTipoDominio.Acciones);

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


            try
            {
                if (contratoPoliza != null)
                {
                    contratoPoliza.FechaCreacion = DateTime.Now;
                    //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;
                    //contratoPoliza.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;

                    //_context.Add(contratoPoliza);

                    contratoPoliza.RegistroCompleo = ValidarRegistroCompletoContratoPoliza(contratoPoliza);
                    //contratoPoliza.ObservacionesRevisionGeneral = ValidarRegistroCompleto(cofinanciacion);

                    LimpiarEntradasContratoPoliza(ref contratoPoliza);

                    //TipoSolicitudCodigo ="3" //si estado devuelta, correo supervisor
                    if (contratoPoliza.TipoSolicitudCodigo == "3")
                        await EnviarCorreoSupervisor(contratoPoliza,  appSettingsService);

                    _context.ContratoPoliza.Add(contratoPoliza);
                    await _context.SaveChangesAsync();


                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContratoPoliza.CreadoCorrrectamente,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias,
                            ConstantMessagesContratoPoliza.CreadoCorrrectamente,
                            //contratoPoliza
                            idAccionCrearContratoPoliza
                            , contratoPoliza.UsuarioCreacion
                            //"UsuarioCreacion"
                            , "REGISTRAR CONTRATO POLIZA"
                            //contratoPoliza.UsuarioCreacion, "REGISTRAR CONTRATO POLIZA"
                            )
                        };

                    //return _response = new Respuesta { IsSuccessful = true,
                    //    IsValidation = false, Data = cuentaBancaria,
                    //    Code = ConstantMessagesBankAccount.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.ErrorInterno };
            }

        }

        //enviar correo estado devuelto
        public async Task<Respuesta> EnviarCorreoSupervisor(ContratoPoliza contratoPoliza, AppSettingsService settings)
        {
            Respuesta respuesta = new Respuesta();
            string fechaFirmaContrato="";
            string correo = "cdaza@ivolucion.com";
            VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza;

            try
            {
                int idAccionEditarContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente, (int)EnumeratorTipoDominio.Acciones);                        
            
                objVistaContratoGarantiaPoliza = new VistaContratoGarantiaPoliza();

                List<VistaContratoGarantiaPoliza> ListVista = new List<VistaContratoGarantiaPoliza>();
                ListVista = await ListVistaContratoGarantiaPoliza();                       

                int pIdTemplate = (int)enumeratorTemplate.MsjSupervisorJuridicaGestionPoliza;                    

                NotificacionMensajeGestionPoliza msjNotificacion;
                msjNotificacion = new NotificacionMensajeGestionPoliza();

                getDataNotifMsjAseguradora(ref msjNotificacion, contratoPoliza, ref fechaFirmaContrato,ref objVistaContratoGarantiaPoliza, ListVista);
                            
                //PolizaObservacion polizaObservacion;           
           
                Task<Respuesta> result = EnviarCorreoGestionPoliza(correo, settings.MailServer,
                settings.MailPort, settings.Password, settings.Sender,
                objVistaContratoGarantiaPoliza, fechaFirmaContrato, pIdTemplate,msjNotificacion);

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
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, correo, "Gestión Pólizas");
                return respuesta;

            }
            catch (Exception ex)
            {

                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesUsuarios.ErrorGuardarCambios };
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, correo, "Gestión Pólizas") + ": " + ex.ToString() + ex.InnerException;
                return respuesta;
            }

        }

        private void getDataNotifMsjAseguradora(ref NotificacionMensajeGestionPoliza msjNotificacion, ContratoPoliza contratoPoliza, ref string fechaFirmaContrato, ref VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza , List<VistaContratoGarantiaPoliza> ListVista)
        {
            msjNotificacion.NombreAseguradora = contratoPoliza.NombreAseguradora;
            msjNotificacion.NumeroPoliza = contratoPoliza.NumeroPoliza;
            msjNotificacion.FechaAprobacion = contratoPoliza.FechaAprobacion.ToString("dd/MM/yyyy");
            msjNotificacion.Observaciones = contratoPoliza.Observaciones;
            //msjNotificacion.FechaAprobacion = contratoPoliza.FechaAprobacion != null ? Convert.ToDateTime(contratoPoliza.FechaAprobacion).ToString("dd/MM/yyyy") : contratoPoliza.FechaAprobacion.ToString();

            PolizaObservacion polizaObservacion;

            polizaObservacion = _context.PolizaObservacion.Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId).FirstOrDefault();

            if (polizaObservacion != null)
            {
                msjNotificacion.EstadoRevision = polizaObservacion.EstadoRevisionCodigo;
                msjNotificacion.FechaRevision = polizaObservacion.FechaRevision.ToString("dd/MM/yyyy");
            }

            Contrato contrato;
            contrato = _context.Contrato.Where(r => r.ContratoId == contratoPoliza.ContratoId).FirstOrDefault();

            Contratacion contratacion = null;
            if (contrato != null)
            {
                contratacion = _context.Contratacion.Where(r => r.ContratacionId == contrato.ContratacionId).FirstOrDefault();

                objVistaContratoGarantiaPoliza = ListVista.Where(x => x.IdContrato == contrato.ContratoId).FirstOrDefault();
                fechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();
            }

            DisponibilidadPresupuestal disponibilidadPresupuestal = null;

            if (contratacion != null)
            {
                disponibilidadPresupuestal = _context.DisponibilidadPresupuestal.Where(r => r.ContratacionId == contratacion.ContratacionId).FirstOrDefault();
            }

            if (disponibilidadPresupuestal != null)
                msjNotificacion.NumeroDRP = disponibilidadPresupuestal.NumeroDrp;
        }

        private static void LimpiarEntradasContratoPoliza(ref ContratoPoliza contratoPoliza)
        {
            contratoPoliza.DescripcionModificacion = Helpers.Helpers.CleanStringInput(contratoPoliza.DescripcionModificacion);
            contratoPoliza.NumeroPoliza = Helpers.Helpers.CleanStringInput(contratoPoliza.NumeroPoliza);
            contratoPoliza.Observaciones = Helpers.Helpers.CleanStringInput(contratoPoliza.Observaciones);
            contratoPoliza.ObservacionesRevisionGeneral = Helpers.Helpers.CleanStringInput(contratoPoliza.ObservacionesRevisionGeneral);
            contratoPoliza.ResponsableAprobacion = Helpers.Helpers.CleanStringInput(contratoPoliza.ResponsableAprobacion);
            contratoPoliza.NumeroCertificado = Helpers.Helpers.CleanStringInput(contratoPoliza.NumeroCertificado);
            contratoPoliza.NombreAseguradora = Helpers.Helpers.CleanStringInput(contratoPoliza.NombreAseguradora);

            contratoPoliza.UsuarioModificacion = "";
            contratoPoliza.UsuarioCreacion = "";
            contratoPoliza.EstadoPolizaCodigo = "";
            //contratoPoliza.contratopoliza = "";

        }
        public static bool ValidarRegistroCompletoContratoPoliza(ContratoPoliza contratoPoliza)
        {

            if (string.IsNullOrEmpty(contratoPoliza.NombreAseguradora.ToString()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(contratoPoliza.NumeroPoliza.ToString()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(contratoPoliza.NumeroCertificado.ToString()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(contratoPoliza.FechaExpedicion.ToString()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(contratoPoliza.Vigencia.ToString()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(contratoPoliza.VigenciaAmparo.ToString()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(contratoPoliza.ValorAmparo.ToString()))
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        
            public async Task<Respuesta> AprobarContratoByIdContrato(int pIdContrato, AppSettingsService settings)
        {
            //public void AprobarContrato(int pIdContrato)
        
            Respuesta respuesta = new Respuesta();
            int idAccionEditarContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = "";
            strCrearEditar = "EDITAR CONTRATO PÓLIZA";

            string correo = "cdaza@ivolucion.com";

            VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza;
            objVistaContratoGarantiaPoliza = new VistaContratoGarantiaPoliza();

            List<VistaContratoGarantiaPoliza> ListVista = new List<VistaContratoGarantiaPoliza>();
            ListVista = await ListVistaContratoGarantiaPoliza();

            objVistaContratoGarantiaPoliza = ListVista.Where(x => x.IdContrato == pIdContrato).FirstOrDefault();

            NotificacionMensajeGestionPoliza msjNotificacion;
            msjNotificacion = new NotificacionMensajeGestionPoliza();
            

            //get
            //string fechaFirmaContrato;
            int pIdTemplate = (int)enumeratorTemplate.MsjSupervisorJuridicaGestionPoliza;
            //NotificacionMensajeGestionPoliza objNotificacionAseguradora = null;
            //Task<Respuesta> result = EnviarCorreoGestionPoliza(correo, _settings.Value.Dominio, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender, pIdTemplate);

            string fechaFirmaContrato;

            ContratoPoliza contratoPoliza = new ContratoPoliza();
            Contrato contrato = new Contrato();

            try
            {                
                contrato = _context.Contrato.Where(r => r.ContratoId == pIdContrato).FirstOrDefault();
                           
                //contratoPoliza = _context.ContratoPoliza.Where(r => !(bool)r.Eliminado && r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();
                contratoPoliza = _context.ContratoPoliza.Where(r => r.ContratoId == contrato.ContratoId).FirstOrDefault();

                if (contratoPoliza != null)
                {
                    //cambiar a estado Con aprobación de pólizas
                    contratoPoliza.TipoSolicitudCodigo = "4";
                    _context.ContratoPoliza.Update(contratoPoliza);
                     
                    fechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();

                    getDataNotifMsjAseguradora(ref msjNotificacion, contratoPoliza, ref fechaFirmaContrato, ref objVistaContratoGarantiaPoliza, ListVista);

                    Task<Respuesta> result = EnviarCorreoGestionPoliza(correo, settings.MailServer,
               settings.MailPort, settings.Password, settings.Sender,
               objVistaContratoGarantiaPoliza, fechaFirmaContrato, pIdTemplate, msjNotificacion);

                    return respuesta = new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Data = contratoPoliza,
                        Code = ConstantMessagesProcesoSeleccion.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.OperacionExitosa, idAccionEditarContratoPoliza, contratoPoliza.UsuarioCreacion, strCrearEditar)

                    };

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesContratoPoliza.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.ErrorInterno, idAccionEditarContratoPoliza, contratoPoliza.UsuarioCreacion, "Error desconocido")
                };

            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesContratoPoliza.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.ErrorInterno, idAccionEditarContratoPoliza, contratoPoliza.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        
            //public async Task<Respuesta> RecoverPasswordByEmailAsync(Usuario pUsuario, string pDominio, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        public async Task<Respuesta> EnviarCorreoGestionPoliza(string lstMails, string pMailServer, int pMailPort, string pPassword, string pSentender, VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza, string fechaFirmaContrato, int pIdTemplate, NotificacionMensajeGestionPoliza objNotificacionAseguradora=null)
        {
            bool blEnvioCorreo = false;
            Respuesta respuesta = new Respuesta();

            //Si no llega Email
            //if (string.IsNullOrEmpty(pUsuario.Email))
            //{
            //    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesUsuarios.EmailObligatorio };
            //}
            try
            {
                //Usuario usuarioSolicito = _context.Usuario.Where(r => !(bool)r.Eliminado && r.Email.ToUpper().Equals(pUsuario.Email.ToUpper())).FirstOrDefault();

                //if (usuarioSolicito != null)
                //{
                    //if (usuarioSolicito.Activo == false)
                    //{
                    //    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesUsuarios.UsuarioInactivo };
                    //}
                    //else
                    //{
                        //string newPass = Helpers.Helpers.GeneratePassword(true, true, true, true, false, 8);
                        //usuarioSolicito.Contrasena = Helpers.Helpers.encryptSha1(newPass.ToString());
                        //usuarioSolicito.CambiarContrasena = true;
                        //usuarioSolicito.Bloqueado = false;
                        //usuarioSolicito.IntentosFallidos = 0;
                        //usuarioSolicito.Ip = pUsuario.Ip;

                        //Guardar Usuario
                        //await UpdateUser(usuarioSolicito);

                        //Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjSupervisorGestionPoliza);
                    Template TemplateRecoveryPassword = await _commonService.GetTemplateById(pIdTemplate);
                                       
                        string template = TemplateRecoveryPassword.Contenido;

                //string urlDestino = pDominio;
                //asent/img/logo  
                Contrato contrato;
                contrato = _context.Contrato.Where(r => r.ContratoId == objVistaContratoGarantiaPoliza.IdContrato).FirstOrDefault();
                
                fechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();

                //datos basicos generales, aplican para los 4 mensajes
                template = template.Replace("_Tipo_Contrato_", objVistaContratoGarantiaPoliza.TipoContrato);
                        template = template.Replace("_Numero_Contrato_", objVistaContratoGarantiaPoliza.NumeroContrato);
                        template = template.Replace("_Fecha_Firma_Contrato_", fechaFirmaContrato); //Formato (dd/MM/aaaa)
                        template = template.Replace("_Nombre_Contratista_", objVistaContratoGarantiaPoliza.NombreContratista);
                        template = template.Replace("_Valor_Contrato_", objVistaContratoGarantiaPoliza.ValorContrato);  //fomato miles .
                        template = template.Replace("_Plazo_", objVistaContratoGarantiaPoliza.PlazoContrato);

                    if(objNotificacionAseguradora!= null)
                    {
                        template = template.Replace("_Nombre_Aseguradora_", objNotificacionAseguradora.NombreAseguradora);
                        template = template.Replace("_Numero_Poliza_", objNotificacionAseguradora.NumeroPoliza);
                        template = template.Replace("_Fecha_Revision_", objNotificacionAseguradora.FechaRevision);
                        template = template.Replace("_Estado_Revision_", objNotificacionAseguradora.EstadoRevision);
                        template = template.Replace("_Observaciones_", objNotificacionAseguradora.Observaciones);

                        template = template.Replace("_Fecha_Aprobacion_Poliza", objNotificacionAseguradora.FechaAprobacion);
                                    
                        if ( !string.IsNullOrEmpty( objNotificacionAseguradora.NumeroDRP))
                            template = template.Replace("_NumeroDRP_", objNotificacionAseguradora.NumeroDRP);

                    }                        
                        blEnvioCorreo = Helpers.Helpers.EnviarCorreo(lstMails, "Gestión Poliza", template, pSentender, pPassword, pMailServer, pMailPort);

                        if (blEnvioCorreo)
                            respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.CorreoEnviado };
                        
                        else
                            respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.ErrorEnviarCorreo };

                    //}
                //}
                //else
                //{
                //    respuesta = new Respuesta() { IsSuccessful = true, IsValidation = true, Code = ConstantMessagesContratoPoliza.CorreoNoExiste };

                //}
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, lstMails, "Gestión Pólizas");
                return respuesta;

            }
            catch (Exception ex)
            {

                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesUsuarios.ErrorGuardarCambios };
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, lstMails, "Gestión Pólizas") + ": " + ex.ToString() + ex.InnerException;
                return respuesta;
            }

        }


        public async Task<List<GrillaContratoGarantiaPoliza>> ListGrillaContratoGarantiaPoliza()
        {
            //await AprobarContratoByIdContrato(1);

            List<GrillaContratoGarantiaPoliza> ListContratoGrilla = new List<GrillaContratoGarantiaPoliza>();
            //Fecha de firma del contrato ??? FechaFirmaContrato , [Contrato] -(dd / mm / aaaa)


            //Tipo de solicitud ??? ContratoPoliza - TipoSolicitudCodigo        
                           

            //List <Contrato> ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado).Include(r => r.FechaFirmaContrato).Include(r => r.NumeroContrato).Include(r => r.Estado).Distinct().ToListAsync();
            List<Contrato> ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado).Distinct().ToListAsync();

            foreach (var contrato in ListContratos)
            {
                try
                {
                    ContratoPoliza contratoPoliza;

                    contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);

                    //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    string strTipoSolicitudCodigoContratoPoliza = "sin definir";
                    string strEstadoSolicitudCodigoContratoPoliza = "sin definir";

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio TipoSolicitudCodigoContratoPoliza;
                    Dominio EstadoSolicitudCodigoContratoPoliza;

                    if (contratoPoliza != null) { 
                        TipoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_Modificacion_Contrato_Poliza);
                        if (TipoSolicitudCodigoContratoPoliza != null)
                            strTipoSolicitudCodigoContratoPoliza = TipoSolicitudCodigoContratoPoliza.Nombre;

                        EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                        if (EstadoSolicitudCodigoContratoPoliza != null)
                            strEstadoSolicitudCodigoContratoPoliza = EstadoSolicitudCodigoContratoPoliza.Nombre;

                    }                 
                   

                    //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                    GrillaContratoGarantiaPoliza contratoGrilla = new GrillaContratoGarantiaPoliza
                    {
                        ContratoId= contrato.ContratoId,
                        //FechaFirma = contrato.FechaFirmaContrato.ToString("dd/mm/yyyy")?"":"",
                        FechaFirma = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString(),
                        //FechaFirma = contrato.FechaFirmaContrato.ToString(),
                        NumeroContrato = contrato.NumeroContrato,
        //TipoSolicitud= contratoPoliza.TipoSolicitudCodigo
        //EstadoRegistro { get; set; }
                
                        //Departamento = departamento.Descripcion,
                        //Municipio = municipio.Descripcion,
                        //InstitucionEducativa = _context.InstitucionEducativaSede.Find(contrato.InstitucionEducativaId).Nombre,
                        //Sede = _context.InstitucionEducativaSede.Find(contrato.SedeId).Nombre,
                        TipoSolicitud = strTipoSolicitudCodigoContratoPoliza,

                        EstadoPoliza = strEstadoSolicitudCodigoContratoPoliza
                        ,RegistroCompleto= contrato.RegistroCompleto
                        
                        
                        //Fecha = contrato.FechaCreacion != null ? Convert.ToDateTime(contrato.FechaCreacion).ToString("yyyy-MM-dd") : proyecto.FechaCreacion.ToString(),
                        //,EstadoRegistro = "COMPLETO"
                    };

                    //if (!(bool)proyecto.RegistroCompleto)
                    //{
                    //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                    //}
                    ListContratoGrilla.Add(contratoGrilla);
                }
                catch (Exception e)
                {
                    GrillaContratoGarantiaPoliza proyectoGrilla = new GrillaContratoGarantiaPoliza
                    {

                        ContratoId = contrato.ContratoId,
                        FechaFirma = e.ToString(),
                        NumeroContrato = e.InnerException.ToString(),
                        //TipoSolicitud= contratoPoliza.TipoSolicitudCodigo
                        //EstadoRegistro { get; set; }

                        //Departamento = departamento.Descripcion,
                        //Municipio = municipio.Descripcion,
                        //InstitucionEducativa = _context.InstitucionEducativaSede.Find(contrato.InstitucionEducativaId).Nombre,
                        //Sede = _context.InstitucionEducativaSede.Find(contrato.SedeId).Nombre,
                        TipoSolicitud = "ERROR"
                        ,
                        RegistroCompleto = false

                    };
                    ListContratoGrilla.Add(proyectoGrilla);
                }
            }
            return ListContratoGrilla.OrderByDescending(r => r.TipoSolicitud).ToList();

        }


        public async Task<List<VistaContratoGarantiaPoliza>> ListVistaContratoGarantiaPoliza()
        {
            List<VistaContratoGarantiaPoliza> ListContratoGrilla = new List<VistaContratoGarantiaPoliza>();
            //Fecha de firma del contrato ??? FechaFirmaContrato , [Contrato] -(dd / mm / aaaa)


            //Tipo de solicitud ??? ContratoPoliza - TipoSolicitudCodigo          

            List<Contrato> ListContratos = new List<Contrato>();
            //ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado).Include(r => r.FechaFirmaContrato).Include(r => r.NumeroContrato).Include(r => r.Estado).Distinct().ToListAsync();

            //return await _context.CofinanciacionAportante.Where(r => !(bool)r.Eliminado && r.TipoAportanteId == pTipoAportanteID).Include(r => r.Cofinanciacion).ToListAsync();

            //ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado).Include(r => r.FechaFirmaContrato).ToListAsync();

            //item.CofinanciacionAportante = await _context.CofinanciacionAportante.Where(r => !(bool)r.Eliminado && r.CofinanciacionId == item.CofinanciacionId).IncludeFilter(r => r.CofinanciacionDocumento.Where(r => !(bool)r.Eliminado)).ToListAsync();

            ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado ).Distinct()
       
            .ToListAsync();

            //ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado)               

            // .Select( new
            //  {
            //      FechaFirmaContrato =r.FechaFirmaContrato.ToString()
            //  })
            // .ToListAsync();

            foreach (var contrato in ListContratos)
            {
                try
                {
                    ContratoPoliza contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    Contratacion contratacion = await _commonService.GetContratacionByContratacionId(contrato.ContratoId);

                    Contratista contratista = await _commonService.GetContratistaByContratistaId((Int32)contratacion.ContratistaId);

                    //TipoContrato = contrato.TipoContratoCodigo   ??? Obra  ????

                    //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    Int32 plazoDias, plazoMeses;
                    //25meses / 04 días

                    if (!string.IsNullOrEmpty(contrato.PlazoFase2ConstruccionDias.ToString()))

                        plazoDias = Convert.ToInt32( contrato.PlazoFase1PreDias);

                    else
                        plazoDias = Convert.ToInt32( contrato.PlazoFase2ConstruccionDias);

                    if (!string.IsNullOrEmpty(contrato.PlazoFase2ConstruccionMeses.ToString()))

                        plazoMeses = Convert.ToInt32(contrato.PlazoFase1PreMeses);

                    else
                        plazoMeses = Convert.ToInt32(contrato.PlazoFase2ConstruccionMeses);

                    string PlazoContratoFormat = plazoMeses.ToString("00") + " meses / " + plazoDias.ToString("00") + " dias ";

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio TipoContratoCodigoContrato = await _commonService.GetDominioByNombreDominioAndTipoDominio(contrato.TipoContratoCodigo, (int)EnumeratorTipoDominio.Tipo_Contrato);


                    //Dominio TipoModificacionCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoModificacionCodigo, (int)EnumeratorTipoDominio.Tipo_Modificacion_Contrato_Poliza);
                    VistaContratoGarantiaPoliza proyectoGrilla = new VistaContratoGarantiaPoliza
                    {
                        IdContrato= contrato.ContratoId,
                        TipoContrato = TipoContratoCodigoContrato.Nombre,
                        NumeroContrato = contrato.NumeroContrato,
                        ObjetoContrato = contrato.Objeto,
                        NombreContratista = contratista.Nombre,

                        //Nit  
                        NumeroIdentificacion = contratista.NumeroIdentificacion.ToString(),

        

         ValorContrato = contrato.Valor.ToString(),

                        PlazoContrato = PlazoContratoFormat,

         //EstadoRegistro 

                        //public bool? RegistroCompleto { get; set; } 

                        

         DescripcionModificacion ="resumen", // resumen   TEMPORAL REV

                        //TipoModificacion = TipoModificacionCodigoContratoPoliza.Nombre
                        TipoModificacion = "Tipo modificacion"

                        //TipoSolicitud= contratoPoliza.TipoSolicitudCodigo
                        //EstadoRegistro { get; set; }

                        //InstitucionEducativa = _context.InstitucionEducativaSede.Find(contrato.InstitucionEducativaId).Nombre,
                        //Sede = _context.InstitucionEducativaSede.Find(contrato.SedeId).Nombre,

                        //Fecha = contrato.FechaCreacion != null ? Convert.ToDateTime(contrato.FechaCreacion).ToString("yyyy-MM-dd") : proyecto.FechaCreacion.ToString(),
                        //EstadoRegistro = "COMPLETO"
                    };

                    //if (!(bool)proyecto.RegistroCompleto)
                    //{
                    //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                    //}
                    ListContratoGrilla.Add(proyectoGrilla);
                }
                catch (Exception e)
                {
                    VistaContratoGarantiaPoliza proyectoGrilla = new VistaContratoGarantiaPoliza
                    {
                        IdContrato=0,
                        TipoContrato = e.ToString(),
                        NumeroContrato = e.InnerException.ToString(),
                        ObjetoContrato = "ERROR",
                        NombreContratista = "ERROR",                       
                        
                        //Nit  
                        NumeroIdentificacion = "ERROR",
                        ValorContrato = "ERROR",

                        PlazoContrato = "ERROR",

         //EstadoRegistro 

                        //public bool? RegistroCompleto { get; set; } 

                        //TipoSolicitud = contratoPoliza.EstadoPolizaCodigo

                    DescripcionModificacion = "ERROR",

                    TipoModificacion = "ERROR"
                       
                    };
                    ListContratoGrilla.Add(proyectoGrilla);
                }
            }
            return ListContratoGrilla.OrderByDescending(r => r.TipoSolicitud).ToList();

        }

    }
}
