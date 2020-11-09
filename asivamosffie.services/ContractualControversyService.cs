using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using asivamosffie.services.Interfaces;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Helpers.Constant;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.services
{
    public class ContractualControversyService : IContractualControversy
    {

        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        public ContractualControversyService(devAsiVamosFFIEContext context, ICommonService commonService)
        {

            _commonService = commonService;
            _context = context;
            //_settings = settings;
        }

        //CreateEditNuevaActualizacionTramite(ControversiaActuacion
        //“Registrar nueva actualización del trámite”

        public async Task<Respuesta> CreateEditNuevaActualizacionTramite(ControversiaActuacion controversiaActuacion)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearcontroversiaActuacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Controversia_Actuacion, (int)EnumeratorTipoDominio.Acciones);

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
                if (controversiaActuacion != null)
                {

                    if (string.IsNullOrEmpty(controversiaActuacion.ControversiaActuacionId.ToString()) || controversiaActuacion.ControversiaActuacionId == 0)
                    {
                        strCrearEditar = "REGISTRAR CONTROVERSIA ACTUACION";

                        //Auditoria
                        strCrearEditar = "REGISTRAR AVANCE COMPROMISOS";
                        controversiaActuacion.FechaCreacion = DateTime.Now;
                        //controversiaActuacion.UsuarioCreacion = compromisoSeguimiento.UsuarioCreacion;
                        controversiaActuacion.Observaciones= Helpers.Helpers.CleanStringInput(controversiaActuacion.Observaciones);
                        controversiaActuacion.ResumenPropuestaFiduciaria = Helpers.Helpers.CleanStringInput(controversiaActuacion.ResumenPropuestaFiduciaria);

                        controversiaActuacion.EsCompleto = ValidarRegistroCompletoControversiaActuacion(controversiaActuacion);

                        controversiaActuacion.Eliminado = false;
                        _context.ControversiaActuacion.Add(controversiaActuacion);

                    }

                    else
                    {
                        strCrearEditar = "EDIT CONTROVERSIA ACTUACION";

                        controversiaActuacion.Observaciones = Helpers.Helpers.CleanStringInput(controversiaActuacion.Observaciones);
                        controversiaActuacion.ResumenPropuestaFiduciaria = Helpers.Helpers.CleanStringInput(controversiaActuacion.ResumenPropuestaFiduciaria);

                        controversiaActuacion.FechaCreacion = DateTime.Now;
                        //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;
                        //controversiaActuacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;

                        //_context.Add(contratoPoliza);

                        controversiaActuacion.EsCompleto = ValidarRegistroCompletoControversiaActuacion(controversiaActuacion);
                        //contratoPoliza.ObservacionesRevisionGeneral = ValidarRegistroCompleto(cofinanciacion);

                        //LimpiarEntradasContratoPoliza(ref contratoPoliza);

                        //_context.ContratoPoliza.Add(contratoPoliza);
                        _context.ControversiaActuacion.Update(controversiaActuacion);

                    }

                    _context.SaveChanges();

                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContractualControversy.OperacionExitosa,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                            ConstantMessagesContractualControversy.OperacionExitosa,
                            //contratoPoliza
                            idAccionCrearcontroversiaActuacion
                            , controversiaActuacion.UsuarioModificacion
                            //"UsuarioCreacion"
                            , "EDITAR CONTROVERSIA ACTUACION"
                            //contratoPoliza.UsuarioCreacion, "REGISTRAR CONTRATO POLIZA"
                            )
                        };

                    //return _response = new Respuesta { IsSuccessful = true,
                    //    IsValidation = false, Data = cuentaBancaria,
                    //    Code = ConstantMessagesBankAccount.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContractualControversy.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContractualControversy.ErrorInterno };
            }

        }

        public async Task<Respuesta> CambiarEstadoControversiaContractual(int pControversiaContractualId, string pNuevoCodigoEstado, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Controversia_Contractual, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ControversiaContractual controversiaContractualOld;
                controversiaContractualOld = _context.ControversiaContractual.Find(pControversiaContractualId);
                controversiaContractualOld.UsuarioModificacion = pUsuarioModifica;
                controversiaContractualOld.FechaModificacion = DateTime.Now;
                controversiaContractualOld.EstadoCodigo = pNuevoCodigoEstado;

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, pUsuarioModifica, "CAMBIAR ESTADO CONTROVERSIA ACTUAL")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }

        }

        public async Task<Respuesta> CambiarEstadoControversiaActuacion (int pControversiaActuacionId, string pNuevoCodigoEstadoAvance, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Controversia_Actuacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ControversiaActuacion controversiaActuacionOld;
                controversiaActuacionOld = _context.ControversiaActuacion.Find(pControversiaActuacionId);
                controversiaActuacionOld.UsuarioModificacion = pUsuarioModifica;
                controversiaActuacionOld.FechaModificacion = DateTime.Now;
                controversiaActuacionOld.EstadoAvanceTramiteCodigo = pNuevoCodigoEstadoAvance;

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, pUsuarioModifica, "CAMBIAR ESTADO CONTROVERSIA ACTUACION")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }

        }


        public async Task<Respuesta> EliminarControversiaActuacion(int pControversiaActuacionId, string pUsuario)
        {                     
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Controversia_Actuacion, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            ControversiaActuacion controversiaActuacion = null;

            try
            {
                controversiaActuacion = await _context.ControversiaActuacion.Where(d => d.ControversiaActuacionId == pControversiaActuacionId).FirstOrDefaultAsync();

                if (controversiaActuacion != null)
                {
                    strCrearEditar = "Eliminar CONTROVERSIA ACTUACION";
                    controversiaActuacion.FechaModificacion = DateTime.Now;
                    //controversiaActuacion.UsuarioCreacion = disponibilidadPresupuestal.UsuarioCreacion;
                    controversiaActuacion.UsuarioModificacion = pUsuario;
                    controversiaActuacion.Eliminado = true;
                    _context.ControversiaActuacion.Update(controversiaActuacion);

                    _context.SaveChanges();

                }               

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = controversiaActuacion,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.EliminacionExitosa, idAccion, controversiaActuacion.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = controversiaActuacion,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, controversiaActuacion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }


        public async Task<Respuesta> EliminarControversiaContractual(int pControversiaContractualId, string pUsuario)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Controversia_Contractual, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            ControversiaContractual controversiaContractual = null;

            try
            {
                controversiaContractual = await _context.ControversiaContractual.Where(d => d.ControversiaContractualId == pControversiaContractualId).FirstOrDefaultAsync();

                if (controversiaContractual != null)
                {
                    strCrearEditar = "Eliminar CONTROVERSIA CONTRACTUAL";
                    controversiaContractual.FechaModificacion = DateTime.Now;
                    //controversiaContractual.UsuarioCreacion = disponibilidadPresupuestal.UsuarioCreacion;
                    controversiaContractual.UsuarioModificacion = pUsuario;
                    controversiaContractual.Eliminado = true;

                    _context.ControversiaContractual.Update(controversiaContractual);

                    _context.SaveChanges();

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = controversiaContractual,
                    Code = ConstantMessagesContractualControversy.EliminacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.EliminacionExitosa, idAccion, controversiaContractual.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = controversiaContractual,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, controversiaContractual.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }


        private bool ValidarRegistroCompletoControversiaActuacion(ControversiaActuacion controversiaActuacion)
        {
            if (string.IsNullOrEmpty(controversiaActuacion.EstadoAvanceTramiteCodigo)
             ||  string.IsNullOrEmpty(controversiaActuacion.ActuacionAdelantadaCodigo)
            || string.IsNullOrEmpty(controversiaActuacion.CantDiasVencimiento.ToString())
                || (controversiaActuacion.EsRequiereContratista==null)
                ||  (controversiaActuacion.EsRequiereInterventor == null)
               || (controversiaActuacion.EsRequiereJuridico == null)
                || (controversiaActuacion.EsRequiereSupervisor == null))           
            {
                return false;
            }

            return true;
        }

        private bool ValidarRegistroCompletoControversiaContractual(ControversiaContractual controversiaContractual)
        {
            //RutaSoporte
            //    EsRequiereComite

            //    MotivoJustificacionRechazo
            //    EsProcede

                
                
                
                

            if (string.IsNullOrEmpty(controversiaContractual.ConclusionComitePreTecnico)
             || string.IsNullOrEmpty(controversiaContractual.FechaComitePreTecnico.ToString())
            || string.IsNullOrEmpty(controversiaContractual.FechaSolicitud.ToString())
                )
            {
                return false;
            }

            return true;
        }

        public async Task<Respuesta> CreateEditarControversiaTAI(ControversiaContractual controversiaContractual )
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContractualControversy.OperacionExitosa, (int)EnumeratorTipoDominio.Acciones);

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
                if (controversiaContractual != null)
                {
                    if (string.IsNullOrEmpty(controversiaContractual.ControversiaContractualId.ToString()) || controversiaContractual.ControversiaContractualId == 0)
                    {
                        strCrearEditar = "REGISTRAR CONTROVERSIA CONTRACTUAL";

                        //Auditoria
                        strCrearEditar = "REGISTRAR AVANCE COMPROMISOS";
                        controversiaContractual.FechaCreacion = DateTime.Now;
                        //controversiaActuacion.UsuarioCreacion = compromisoSeguimiento.UsuarioCreacion;
                        controversiaContractual.MotivoJustificacionRechazo = Helpers.Helpers.CleanStringInput(controversiaContractual.MotivoJustificacionRechazo);
                        controversiaContractual.ConclusionComitePreTecnico = Helpers.Helpers.CleanStringInput(controversiaContractual.ConclusionComitePreTecnico);

                        controversiaContractual.EsCompleto = ValidarRegistroCompletoControversiaContractual(controversiaContractual);

                        //controversiaContractual.Eliminado = false;
                        _context.ControversiaContractual.Add(controversiaContractual);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        strCrearEditar = "EDITAR CONTROVERSIA CONTRACTUAL";
                        controversiaContractual.MotivoJustificacionRechazo = Helpers.Helpers.CleanStringInput(controversiaContractual.MotivoJustificacionRechazo);
                        controversiaContractual.ConclusionComitePreTecnico = Helpers.Helpers.CleanStringInput(controversiaContractual.ConclusionComitePreTecnico);
                        //ControversiaContractual.FechaCreacion = DateTime.Now;
                        //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;
                        //contratoPoliza.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;

                        //_context.Add(contratoPoliza);

                        //contratoPoliza.RegistroCompleo = ValidarRegistroCompletoContratoPoliza(contratoPoliza);
                        //contratoPoliza.ObservacionesRevisionGeneral = ValidarRegistroCompleto(cofinanciacion);

                        //LimpiarEntradasContratoPoliza(ref contratoPoliza);
                        controversiaContractual.FechaModificacion = DateTime.Now;
                        //_context.ContratoPoliza.Add(contratoPoliza);
                        _context.ControversiaContractual.Update(controversiaContractual);
                        await _context.SaveChangesAsync();
                    }

                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContractualControversy.OperacionExitosa,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                            ConstantMessagesContractualControversy.OperacionExitosa,
                            //contratoPoliza
                            idAccionCrearContratoPoliza
                            , controversiaContractual.UsuarioModificacion
                            //"UsuarioCreacion"
                            , "EDITAR CONTROVERSIA CONTRACTUAL"
                            //contratoPoliza.UsuarioCreacion, "REGISTRAR CONTRATO POLIZA"
                            )
                        };

                    //return _response = new Respuesta { IsSuccessful = true,
                    //    IsValidation = false, Data = cuentaBancaria,
                    //    Code = ConstantMessagesBankAccount.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContractualControversy.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContractualControversy.ErrorInterno };
            }

        }        


            public async Task<VistaContratoContratista> GetVistaContratoContratista(int pContratoId)
        {
            VistaContratoContratista vistaContratoContratista = new VistaContratoContratista();

            int IdContratistaTmp = 0;
             string NombreContratistaTmp = string.Empty;
             string NumeroContratoTmp = string.Empty;
            string PlazoFormatTmp = string.Empty;
            string FechaInicioContratoTmp = string.Empty;
            string FechaFinContratoTmp = string.Empty;

            try
            {
                Contrato contrato = null;
                contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

                Contratacion contratacion  = null;
                if (contrato != null)
                {
                    contratacion = _context.Contratacion.Where(r => r.ContratacionId == contrato.ContratacionId).FirstOrDefault();

                    FechaFinContratoTmp = contrato.FechaTerminacionFase2 != null ? Convert.ToDateTime(contrato.FechaTerminacionFase2).ToString("dd/MM/yyyy") : contrato.FechaTerminacionFase2.ToString();
                    FechaInicioContratoTmp = contrato.FechaActaInicioFase2 != null ? Convert.ToDateTime(contrato.FechaActaInicioFase2).ToString("dd/MM/yyyy") : contrato.FechaActaInicioFase2.ToString();

                    NumeroContratoTmp = contrato.NumeroContrato;
                    PlazoFormatTmp = Convert.ToInt32(contrato.PlazoFase2ConstruccionMeses).ToString("00") + " meses / " + Convert.ToInt32(contrato.PlazoFase2ConstruccionDias).ToString("00") + " dias ";


                }

                Contratista contratista  = null;
                if(contratacion != null)
                {
                    contratista = _context.Contratista.Where(r => r.ContratistaId == contratacion.ContratistaId).FirstOrDefault();

                    if (contratista != null)
                    {
                        IdContratistaTmp = contratista.ContratistaId;
                        NombreContratistaTmp = contratista.Nombre;
                    }

                }              
              
                vistaContratoContratista.IdContratista = IdContratistaTmp;
                vistaContratoContratista.FechaFinContrato = FechaFinContratoTmp;
                vistaContratoContratista.FechaInicioContrato = FechaInicioContratoTmp;
                vistaContratoContratista.IdContratista = IdContratistaTmp;
                vistaContratoContratista.NombreContratista = NombreContratistaTmp;
                vistaContratoContratista.NumeroContrato = NumeroContratoTmp;
                vistaContratoContratista.PlazoFormat = PlazoFormatTmp;

            }
            catch (Exception e)
            {
                 vistaContratoContratista = new VistaContratoContratista
                {
                     IdContratista = 0,
                     FechaFinContrato = e.InnerException.ToString(),
                     FechaInicioContrato = e.ToString(), 
                     NombreContratista = "ERROR",
                     NumeroContrato = "ERROR",
                     PlazoFormat= "ERROR",                
                };
                
            }
            return vistaContratoContratista;
        }

        public async Task<Respuesta> EnviarCorreoTecnicaJuridicaContratacion(string lstMails, string pMailServer, int pMailPort, string pPassword, string pSentender, int pContratoId,  int pIdTemplate)
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
                Contrato contrato=null;
                contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId && r.Eliminado==false).FirstOrDefault();
                                
                ControversiaContractual controversiaContractual =null;
                                
                //con.NumeroSolicitud;
                //con.ContratoId
                //contrato1.FechaFirmaFiduciaria
                //contrato1.Observaciones
                string strNumeroSolicitud = string.Empty;
                string strNumeroContrato = string.Empty;
                string strFechaFirmaFiduciaria = string.Empty;
                string strObservaciones = string.Empty;
                              
                if (contrato != null)
                {
                    controversiaContractual = _context.ControversiaContractual.Where(r => r.ContratoId == contrato.ContratoId).FirstOrDefault();

                    if(controversiaContractual!=null)
                        strNumeroSolicitud = controversiaContractual.NumeroSolicitud;

                    strNumeroContrato = contrato.NumeroContrato;
                    strFechaFirmaFiduciaria = contrato.FechaFirmaFiduciaria != null ? Convert.ToDateTime(contrato.FechaFirmaFiduciaria).ToString("dd/MM/yyyy") : contrato.FechaFirmaFiduciaria.ToString();
                    strObservaciones = contrato.Observaciones;
                    
                }                                

                //               Número de solicitud: 
                //Número de contrato: 
                //Fecha de firma de la fiduciaria: 
                //Observaciones: Campo 

                template = template.Replace("_Numero_solicitud_", strNumeroSolicitud);
                template = template.Replace("_Numero_contrato_", strNumeroContrato);
                template = template.Replace("_Fecha_firma_fiduciaria_", strFechaFirmaFiduciaria);
                template = template.Replace("_Observaciones_", strObservaciones);
                               
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
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, lstMails, "Gestión Pólizas");
                return respuesta;

            }
            catch (Exception ex)
            {

                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesContratoPoliza.Error };
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, lstMails, "Gestión Pólizas") + ": " + ex.ToString() + ex.InnerException;
                return respuesta;
            }

        }

        public async Task<List<GrillaTipoSolicitudControversiaContractual>> ListGrillaTipoSolicitudControversiaContractual()
        {
            //await AprobarContratoByIdContrato(1);

            List<GrillaTipoSolicitudControversiaContractual> ListControversiaContractualGrilla = new List<GrillaTipoSolicitudControversiaContractual>();
            //Fecha de firma del contrato ??? FechaFirmaContrato , [Contrato] -(dd / mm / aaaa)

            //Tipo de solicitud ??? ContratoPoliza - TipoSolicitudCodigo      
                        
            //List<ControversiaContractual> ListControversiaContractualGrilla = await _context.ControversiaContractual.Where(r => !(bool)r.EstadoCodigo).Distinct().ToListAsync();
            List<ControversiaContractual> ListControversiaContractual = await _context.ControversiaContractual.Distinct().ToListAsync();

            foreach (var controversia in ListControversiaContractual)
            {
                try
                {
                    Contrato contrato=null;

                    //contrato = await _commonService.GetContratoPolizaByContratoId(controversia.ContratoId);
                    contrato =  _context.Contrato.Where(r=>r.ContratoId==controversia.ContratoId).FirstOrDefault();

                    //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    string strEstadoCodigoControversia = "sin definir";
                    string strTipoControversiaCodigo = "sin definir";
                    string strTipoControversia = "sin definir";

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio EstadoCodigoControversia;
                    Dominio TipoControversiaCodigo;

                    if (contrato != null)
                    {
                        TipoControversiaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipo_de_controversia);
                        if (TipoControversiaCodigo != null)
                        {
                            strTipoControversiaCodigo = TipoControversiaCodigo.Nombre;
                            strTipoControversia = TipoControversiaCodigo.Codigo;

                        }
                            

                        //EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                        //if (EstadoSolicitudCodigoContratoPoliza != null)
                        //    strEstadoSolicitudCodigoContratoPoliza = EstadoSolicitudCodigoContratoPoliza.Nombre;

                    }


                    //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                    GrillaTipoSolicitudControversiaContractual RegistroControversiaContractual = new GrillaTipoSolicitudControversiaContractual
                    {
                         ControversiaContractualId=controversia.ControversiaContractualId,
                         NumeroSolicitud=controversia.NumeroSolicitud,
                         //FechaSolicitud=controversia.FechaSolicitud,
                         FechaSolicitud =controversia.FechaSolicitud != null ? Convert.ToDateTime(controversia.FechaSolicitud).ToString("dd/MM/yyyy") : controversia.FechaSolicitud.ToString(),
                         TipoControversia =strTipoControversia,
                         TipoControversiaCodigo= strTipoControversiaCodigo,
                         ContratoId = contrato.ContratoId,
                        EstadoControversia = "PENDIENTE",
                        RegistroCompletoNombre = (bool)controversia.EsCompleto ? "Completo" : "Incompleto",

                    };

                    //if (!(bool)proyecto.RegistroCompleto)
                    //{
                    //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                    //}
                    ListControversiaContractualGrilla.Add(RegistroControversiaContractual);
                }
                catch (Exception e)
                {
                    GrillaTipoSolicitudControversiaContractual RegistroControversiaContractual = new GrillaTipoSolicitudControversiaContractual
                    {
                        ControversiaContractualId = controversia.ControversiaContractualId,
                        NumeroSolicitud = controversia.NumeroSolicitud+" - "+ e.InnerException.ToString(),
                        //FechaSolicitud=controversia.FechaSolicitud,
                        FechaSolicitud = controversia.FechaSolicitud != null ? Convert.ToDateTime(controversia.FechaSolicitud).ToString("dd/MM/yyyy") : controversia.FechaSolicitud.ToString(),
                        TipoControversia = e.ToString(),
                        TipoControversiaCodigo = "ERROR",
                        ContratoId = 0,                                                                      

                    };
                    ListControversiaContractualGrilla.Add(RegistroControversiaContractual);
                }
            }
            return ListControversiaContractualGrilla.OrderByDescending(r => r.ControversiaContractualId).ToList();

        }

    }
}
