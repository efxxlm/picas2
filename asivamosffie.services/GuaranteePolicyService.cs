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


    
        public async Task<NotificacionMensajeGestionPoliza> GetNotificacionContratoPolizaByIdContratoId(int pContratoId)
        {
            NotificacionMensajeGestionPoliza msjNotificacion=new NotificacionMensajeGestionPoliza();
            Contrato contrato = null;

            //contratoPoliza = _context.ContratoPoliza.Where(r => !(bool)r.Eliminado && r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();
            contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

            //includefilter
            ContratoPoliza contratoPoliza = new ContratoPoliza();
            if (contrato != null)
            {
                //contratoPoliza = _context.ContratoPoliza.Where(r => !(bool)r.Eliminado && r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();
                contratoPoliza = _context.ContratoPoliza.Where(r => r.ContratoId == pContratoId
                //&&(bool)r.Estado==true&&r.Eliminado==0
                ).OrderByDescending(r => r.ContratoPolizaId).FirstOrDefault();

            }

            PolizaObservacion polizaObservacion = null;
            //contratoPoliza = _context.ContratoPoliza.Where(r => !(bool)r.Eliminado && r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();

            if (contratoPoliza != null)
            {
                //List<PolizaGarantia> contratoPolizaGarantia = await _context.PolizaGarantia.Where(r => r.ContratoPolizaId == p && !(bool)r.Eliminado).IncludeFilter(r => r.CofinanciacionDocumento.Where(r => !(bool)r.Eliminado)).ToListAsync();
                List<PolizaGarantia> contratoPolizaGarantia = await _context.PolizaGarantia.Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId).ToListAsync();

                contratoPoliza.PolizaGarantia = contratoPolizaGarantia;

                polizaObservacion = _context.PolizaObservacion.Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId).FirstOrDefault();

                //List<PolizaGarantia> contratoPolizaGarantia = await _context.PolizaGarantia.Where(r => r.ContratoPolizaId == p && !(bool)r.Eliminado).IncludeFilter(r => r.CofinanciacionDocumento.Where(r => !(bool)r.Eliminado)).ToListAsync();
                List<PolizaObservacion> contratoPolizaObservacion = await _context.PolizaObservacion.Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId).ToListAsync();

                contratoPoliza.PolizaObservacion = contratoPolizaObservacion;

                msjNotificacion.NombreAseguradora = contratoPoliza.NombreAseguradora;
                msjNotificacion.NumeroPoliza = contratoPoliza.NumeroPoliza;
                //msjNotificacion.FechaAprobacion = contratoPoliza.FechaAprobacion.ToString("dd/MM/yyyy");
                //msjNotificacion.Observaciones = contratoPoliza.Observaciones;
                msjNotificacion.Observaciones = contratoPoliza.ObservacionesRevisionGeneral;
                msjNotificacion.FechaAprobacion = contratoPoliza.FechaAprobacion != null ? Convert.ToDateTime(contratoPoliza.FechaAprobacion).ToString("dd/MM/yyyy") : contratoPoliza.FechaAprobacion.ToString();

                //PolizaObservacion polizaObservacion;

                //polizaObservacion = _context.PolizaObservacion.Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId).FirstOrDefault();

                if (polizaObservacion != null)
                {
                    msjNotificacion.EstadoRevision = polizaObservacion.EstadoRevisionCodigo;
                    msjNotificacion.FechaRevision = polizaObservacion.FechaRevision.ToString("dd/MM/yyyy");
                    msjNotificacion.FechaRevisionDateTime = polizaObservacion.FechaRevision;
                    
                }
            }

            return msjNotificacion;
        }

        public async Task<ContratoPoliza> GetContratoPolizaByIdContratoId(int pContratoId)
        {
            Contrato contrato =null;

            //contratoPoliza = _context.ContratoPoliza.Where(r => !(bool)r.Eliminado && r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();
            contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

            //includefilter
            ContratoPoliza contratoPoliza = new ContratoPoliza();
            if (contrato != null)
            { 
                //contratoPoliza = _context.ContratoPoliza.Where(r => !(bool)r.Eliminado && r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();
            contratoPoliza = _context.ContratoPoliza.Where(r => r.ContratoId == pContratoId
            //&&(bool)r.Estado==true&&r.Eliminado==0
            ).OrderByDescending(r=>r.ContratoPolizaId).FirstOrDefault();

            }

            PolizaObservacion polizaObservacion=null;
            //contratoPoliza = _context.ContratoPoliza.Where(r => !(bool)r.Eliminado && r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();
            
            if (contratoPoliza != null)
            {
                //List<PolizaGarantia> contratoPolizaGarantia = await _context.PolizaGarantia.Where(r => r.ContratoPolizaId == p && !(bool)r.Eliminado).IncludeFilter(r => r.CofinanciacionDocumento.Where(r => !(bool)r.Eliminado)).ToListAsync();
                List<PolizaGarantia> contratoPolizaGarantia = await _context.PolizaGarantia.Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId).ToListAsync();

                contratoPoliza.PolizaGarantia = contratoPolizaGarantia;

                polizaObservacion = _context.PolizaObservacion.Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId).FirstOrDefault();
            
                //List<PolizaGarantia> contratoPolizaGarantia = await _context.PolizaGarantia.Where(r => r.ContratoPolizaId == p && !(bool)r.Eliminado).IncludeFilter(r => r.CofinanciacionDocumento.Where(r => !(bool)r.Eliminado)).ToListAsync();
                List<PolizaObservacion> contratoPolizaObservacion = await _context.PolizaObservacion.Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId).ToListAsync();

                contratoPoliza.PolizaObservacion = contratoPolizaObservacion;
            }

            return contratoPoliza;
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


        public async Task<Respuesta> InsertEditPolizaGarantia(PolizaGarantia polizaGarantia)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Poliza_Garantia, (int)EnumeratorTipoDominio.Acciones);

            //GUARDAR
            //PolizaObservacion - FechaRevision
            //    EstadoRevisionCodigo - PolizaObservacion
            string strCrearEditar;
            try
            {
                if (polizaGarantia != null)
                {
                    if (polizaGarantia.PolizaGarantiaId == 0)
                    {
                        //Auditoria
                        strCrearEditar = "REGISTRAR POLIZA GARANTIA";
                        _context.PolizaGarantia.Add(polizaGarantia);
                        //await _context.SaveChangesAsync();
                        _context.SaveChanges();

                    }
                    else
                    {
                        strCrearEditar = "EDIT POLIZA GARANTIA";
                        _context.PolizaGarantia.Update(polizaGarantia);

                        //_context.CuentaBancaria.Update(cuentaBancariaAntigua);
                    }
                    //contratoPoliza.FechaCreacion = DateTime.Now;
                    //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;


                    //_context.Add(contratoPoliza);

                    //contratoPoliza.ObservacionesRevisionGeneral = ValidarRegistroCompleto(cofinanciacion);


                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContratoPoliza.CreadoCorrrectamente,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias,
                            ConstantMessagesContratoPoliza.OperacionExitosa,
                            //contratoPoliza
                            1
                            ,
                            "UsuarioCreacion", strCrearEditar
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
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.ErrorInterno, Message = ex.InnerException.ToString().Substring(0, 500) };
            }

        }
        public async Task<Respuesta> InsertEditPolizaObservacion(PolizaObservacion polizaObservacion)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Poliza_Observacion, (int)EnumeratorTipoDominio.Acciones);

            //GUARDAR
            //PolizaObservacion - FechaRevision
            //    EstadoRevisionCodigo - PolizaObservacion

            string strCrearEditar;
            try
            {
                polizaObservacion.Observacion = Helpers.Helpers.CleanStringInput(polizaObservacion.Observacion);
                if (polizaObservacion != null)
                {

                    if (polizaObservacion.PolizaObservacionId == 0)
                    {
                        //Auditoria
                        strCrearEditar = "REGISTRAR POLIZA OBSERVACION";
                        _context.PolizaObservacion.Add(polizaObservacion);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        strCrearEditar = "EDIT POLIZA OBSERVACION";
                        PolizaObservacion polizaObservacionBD = null;

                        polizaObservacionBD = _context.PolizaObservacion.Where(r => r.PolizaObservacionId == polizaObservacion.PolizaObservacionId).FirstOrDefault();
                        if (polizaObservacion != null)
                        {
                            polizaObservacionBD.Observacion = polizaObservacion.Observacion;
                            polizaObservacionBD.FechaRevision = polizaObservacion.FechaRevision;
                            polizaObservacionBD.EstadoRevisionCodigo = polizaObservacion.EstadoRevisionCodigo;
                            _context.PolizaObservacion.Update(polizaObservacionBD);

                        }

                        //_context.CuentaBancaria.Update(cuentaBancariaAntigua);
                    }
                    //contratoPoliza.FechaCreacion = DateTime.Now;
                    //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;

                    //_context.Add(contratoPoliza);

                    //contratoPoliza.ObservacionesRevisionGeneral = ValidarRegistroCompleto(cofinanciacion);


                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContratoPoliza.OperacionExitosa,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias,
                            ConstantMessagesContratoPoliza.OperacionExitosa,
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
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.ErrorInterno, Message = ex.InnerException.ToString().Substring(0, 500) };
            }

        }


        public async Task<Respuesta> EditarContratoPoliza(ContratoPoliza contratoPoliza)
        {
            Respuesta _response = new Respuesta();

            //int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente, (int)EnumeratorTipoDominio.Acciones);
            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Editar_Contrato_Poliza, (int)EnumeratorTipoDominio.Acciones);


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
                    contratoPoliza.FechaModificacion = DateTime.Now;

                    //_context.Add(contratoPoliza);

                    contratoPoliza.RegistroCompleto = ValidarRegistroCompletoContratoPoliza(contratoPoliza);
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
                            Code = ConstantMessagesContratoPoliza.OperacionExitosa,
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
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.ErrorInterno, Message = ex.InnerException.ToString().Substring(0, 500) };
            }

        }

        public async Task<Respuesta> InsertContratoPoliza(ContratoPoliza contratoPoliza, AppSettingsService appSettingsService)
        {
            Respuesta _response = new Respuesta();

            //int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContratoPoliza.OperacionExitosa, (int)EnumeratorTipoDominio.Acciones);
            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Contrato_Poliza, (int)EnumeratorTipoDominio.Acciones);

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
                    DateTime? dt = null;
                    //DateTime? dt;
                    //DateTime dt = new DateTime(1957, 1, 1);
                    //Nullable<DateTime> dt = null;
                    //Nullable<DateTime> dt;
                    //x  dt = null;

                    //DateTime dt = new DateTime();

                    //contratoPoliza.FechaAprobacion = dt.Value;
                    //contratoPoliza.FechaAprobacion = default(DateTime);
                    //contratoPoliza.FechaAprobacion = dt.GetValueOrDefault();
                    //if (contratoPoliza.FechaAprobacion.Year==1)
                    //contratoPoliza.FechaAprobacion = (DateTime)dt;
                    //contratoPoliza.FechaAprobacion.Year= 1977;

                    //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;
                    //contratoPoliza.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;

                    //_context.Add(contratoPoliza);

                    contratoPoliza.RegistroCompleto = ValidarRegistroCompletoContratoPoliza(contratoPoliza);
                    //contratoPoliza.ObservacionesRevisionGeneral = ValidarRegistroCompleto(cofinanciacion);

                    LimpiarEntradasContratoPoliza(ref contratoPoliza);

                    //TipoSolicitudCodigo ="3" //si estado devuelta, correo supervisor
                    //if (contratoPoliza.TipoSolicitudCodigo == "3")

                    //ConstanCodigoTipoSolicitud.Actualizacion_Cronograma_Proceso_Seleccion)
                    //if (contratoPoliza.TipoSolicitudCodigo == ((int)EnumeratorEstadoPoliza.Con_poliza_observada_y_devuelta).ToString())
                    //    contratoPoliza.TipoSolicitudCodigo = ((int)EnumeratorEstadoPoliza.Con_aprobacion_de_polizas).ToString();

                    //contratoPoliza.TipoSolicitudCodigo = "4";                   
                                      
                    //_context.ExecuteStoreCommand("SET IDENTITY_INSERT [dbo].[MyUser] ON");

                    //guardar por primera vez EstadoPolizaCodigo DOM 51  2   En revisión de pólizas
                    contratoPoliza.EstadoPolizaCodigo = ((int)EnumeratorEstadoPoliza.En_revision_de_polizas).ToString();

                    _context.ContratoPoliza.Add(contratoPoliza);
                    //await _context.SaveChangesAsync();
                    _context.SaveChanges();

                    //await EnviarCorreoSupervisor(contratoPoliza, appSettingsService);

                    Respuesta respuesta = new Respuesta();
                    string fechaFirmaContrato = "";
                    string correo = "cdaza@ivolucion.com";
                    VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza;

                    int perfilId = 0;

                    perfilId = 8; //  Supervisor
                    correo = getCorreos(perfilId);

                    try
                    {
                        int idAccionEditarContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente, (int)EnumeratorTipoDominio.Acciones);

                        objVistaContratoGarantiaPoliza = new VistaContratoGarantiaPoliza();

                        List<VistaContratoGarantiaPoliza> ListVista = new List<VistaContratoGarantiaPoliza>();
                        ListVista = await ListVistaContratoGarantiaPoliza();

                        int pIdTemplate = (int)enumeratorTemplate.MsjSupervisorJuridicaGestionPoliza;

                        NotificacionMensajeGestionPoliza msjNotificacion;
                        msjNotificacion = new NotificacionMensajeGestionPoliza();

                        getDataNotifMsjAseguradora(ref msjNotificacion, contratoPoliza, ref fechaFirmaContrato, ref objVistaContratoGarantiaPoliza, ListVista);

                        //PolizaObservacion polizaObservacion;           
                        //correo = "cdaza@ivolucion.com";

                        //Task<Respuesta> result = EnviarCorreoGestionPoliza(correo, appSettingsService.MailServer,
                        //appSettingsService.MailPort, appSettingsService.Password, appSettingsService.Sender,
                        //objVistaContratoGarantiaPoliza, fechaFirmaContrato, pIdTemplate, msjNotificacion);


                        ///////////////////////////////////
                        ///

                        bool blEnvioCorreo = false;
                        //Respuesta respuesta = new Respuesta();

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

                            if (msjNotificacion != null)
                            {
                                template = template.Replace("_Nombre_Aseguradora_", msjNotificacion.NombreAseguradora);
                                template = template.Replace("_Numero_Poliza_", msjNotificacion.NumeroPoliza);
                                template = template.Replace("_Fecha_Revision_", msjNotificacion.FechaRevision);
                                template = template.Replace("_Estado_Revision_", msjNotificacion.EstadoRevision);
                                template = template.Replace("_Observaciones_", msjNotificacion.Observaciones);

                                template = template.Replace("_Fecha_Aprobacion_Poliza", msjNotificacion.FechaAprobacion);

                                if (!string.IsNullOrEmpty(msjNotificacion.NumeroDRP))
                                    template = template.Replace("_NumeroDRP_", msjNotificacion.NumeroDRP);

                            }
                            blEnvioCorreo = Helpers.Helpers.EnviarCorreo(correo, "Gestión Poliza", template, appSettingsService.Sender, appSettingsService.Password, appSettingsService.MailServer, appSettingsService.MailPort);

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
                            respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, correo, "Gestión Pólizas");
                            return respuesta;

                        }
                        catch (Exception ex)
                        {
                            respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesContratoPoliza.Error };
                            respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, correo, "Gestión Pólizas") + ": " + ex.ToString() + ex.InnerException;
                            return respuesta;
                        }


                        //////////////////////////////////////////


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


                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContratoPoliza.OperacionExitosa,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias,
                            ConstantMessagesContratoPoliza.OperacionExitosa,
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
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.ErrorInterno, Message = ex.InnerException.ToString().Substring(0, 500) };
            }

        }

        public async Task<Respuesta> CambiarEstadoPolizaByContratoId(int pContratoId, string pCodigoNuevoEstadoPoliza, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Gestion_Poliza, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                Contrato contrato=null;
                contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

                ContratoPoliza contratoPoliza= null;
                if (contrato != null)
                {
                    //contratoPoliza = _context.ContratoPoliza.Find(contrato.ContratoId);                    
                    contratoPoliza = await _commonService.GetLastContratoPolizaByContratoId(contrato.ContratoId);
                }                

                if(contratoPoliza!=null)
                {
                    //SesionComiteSolicitud sesionComiteSolicitudOld = _context.SesionComiteSolicitud.Find(/*pSesionComiteSolicitud*/);
                    contratoPoliza.UsuarioModificacion = pUsuarioModifica;
                    contratoPoliza.FechaModificacion = DateTime.Now;
                    contratoPoliza.EstadoPolizaCodigo = pCodigoNuevoEstadoPoliza;

                    contratoPoliza.RegistroCompleto = ValidarRegistroCompletoContratoPoliza(contratoPoliza);

                    _context.SaveChanges();

                }                

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContratoPoliza.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.OperacionExitosa, idAccion, pUsuarioModifica, "CAMBIAR ESTADO POLIZA")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContratoPoliza.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }

        }

        public async Task<Respuesta> CambiarEstadoPoliza(int pContratoPolizaId, string pCodigoNuevoEstadoPoliza, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Gestion_Poliza, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ContratoPoliza contratoPoliza = _context.ContratoPoliza.Find(pContratoPolizaId);

                //SesionComiteSolicitud sesionComiteSolicitudOld = _context.SesionComiteSolicitud.Find(/*pSesionComiteSolicitud*/);
                contratoPoliza.UsuarioModificacion = pUsuarioModifica;
                contratoPoliza.FechaModificacion = DateTime.Now;
                contratoPoliza.EstadoPolizaCodigo = pCodigoNuevoEstadoPoliza;

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContratoPoliza.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.OperacionExitosa, idAccion, pUsuarioModifica, "CAMBIAR ESTADO POLIZA")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContratoPoliza.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }

        }

        //enviar correo estado devuelto
        public async Task<Respuesta> EnviarCorreoSupervisor(ContratoPoliza contratoPoliza, AppSettingsService settings)
        {
            Respuesta respuesta = new Respuesta();
            string fechaFirmaContrato = "";
            string correo = "cdaza@ivolucion.com";
            VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza;

            int perfilId = 0;

            perfilId = 8; //  Supervisor
            correo = getCorreos(perfilId);

            try
            {
                int idAccionEditarContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente, (int)EnumeratorTipoDominio.Acciones);

                objVistaContratoGarantiaPoliza = new VistaContratoGarantiaPoliza();

                List<VistaContratoGarantiaPoliza> ListVista = new List<VistaContratoGarantiaPoliza>();
                ListVista = await ListVistaContratoGarantiaPoliza();

                int pIdTemplate = (int)enumeratorTemplate.MsjSupervisorJuridicaGestionPoliza;

                NotificacionMensajeGestionPoliza msjNotificacion;
                msjNotificacion = new NotificacionMensajeGestionPoliza();

                getDataNotifMsjAseguradora(ref msjNotificacion, contratoPoliza, ref fechaFirmaContrato, ref objVistaContratoGarantiaPoliza, ListVista);

                //PolizaObservacion polizaObservacion;           
                 correo = "cdaza@ivolucion.com";

                Task<Respuesta> result = EnviarCorreoGestionPoliza(correo, settings.MailServer,
                settings.MailPort, settings.Password, settings.Sender,
                objVistaContratoGarantiaPoliza, fechaFirmaContrato, pIdTemplate, msjNotificacion);

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

        private void getDataNotifMsjAseguradora(ref NotificacionMensajeGestionPoliza msjNotificacion, ContratoPoliza contratoPoliza, ref string fechaFirmaContrato, ref VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza, List<VistaContratoGarantiaPoliza> ListVista)
        {
            msjNotificacion.NombreAseguradora = contratoPoliza.NombreAseguradora;
            msjNotificacion.NumeroPoliza = contratoPoliza.NumeroPoliza;
            //msjNotificacion.FechaAprobacion = contratoPoliza.FechaAprobacion.ToString("dd/MM/yyyy");
            msjNotificacion.Observaciones = contratoPoliza.Observaciones;
            msjNotificacion.FechaAprobacion = contratoPoliza.FechaAprobacion != null ? Convert.ToDateTime(contratoPoliza.FechaAprobacion).ToString("dd/MM/yyyy") : contratoPoliza.FechaAprobacion.ToString();

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

            //contratoPoliza.UsuarioModificacion = "";
            //contratoPoliza.UsuarioCreacion = "";
            //contratoPoliza.EstadoPolizaCodigo = "";
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

            else if (string.IsNullOrEmpty(contratoPoliza.CumpleDatosAsegurado.ToString()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(contratoPoliza.CumpleDatosBeneficiario.ToString()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(contratoPoliza.CumpleDatosTomador.ToString()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(contratoPoliza.IncluyeReciboPago.ToString()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(contratoPoliza.IncluyeCondicionesGenerales.ToString()))
            {
                return false;
            }

            else if (string.IsNullOrEmpty(contratoPoliza.IncluyeCondicionesGenerales.ToString()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(contratoPoliza.FechaAprobacion.ToString()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(contratoPoliza.ResponsableAprobacion.ToString()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(contratoPoliza.EstadoPolizaCodigo.ToString()))
            {
                return false;
            }

            else
            {
                return true;
            }

        }


        public async Task<Respuesta> AprobarContratoByIdContrato(int pIdContrato, AppSettingsService settings, string pUsuario)
        {
            //public void AprobarContrato(int pIdContrato)

            Respuesta respuesta = new Respuesta();
            int idAccionEditarContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = "";
            strCrearEditar = "APROBAR CONTRATO PÓLIZA";

            string correo = "cdaza@ivolucion.com";

            VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza;
            objVistaContratoGarantiaPoliza = new VistaContratoGarantiaPoliza();

            List<VistaContratoGarantiaPoliza> ListVista = new List<VistaContratoGarantiaPoliza>();
            ListVista = await ListVistaContratoGarantiaPoliza(pIdContrato);

            objVistaContratoGarantiaPoliza = ListVista.Where(x => x.IdContrato == pIdContrato).FirstOrDefault();

            NotificacionMensajeGestionPoliza msjNotificacion;
            msjNotificacion = new NotificacionMensajeGestionPoliza();

            //notificación al responsable jurídico, al supervisor del equipo técnico del FFIE, 
            //y al interventor para que revise el gestor de tareas,
            int perfilId = 0;

            perfilId = 4; //  Jurídica
            correo = getCorreos(perfilId);

            perfilId = 8; //    Supervisor
            correo = correo += ";" + getCorreos(perfilId);

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
                contratoPoliza = _context.ContratoPoliza.Where(r => r.ContratoId == contrato.ContratoId)
                    .OrderByDescending(r => r.ContratoPolizaId).FirstOrDefault();

                if (contratoPoliza != null)
                {
                    //cambiar a estado Con aprobación de pólizas
                    //contratoPoliza.TipoSolicitudCodigo = "4";
                    contratoPoliza.UsuarioModificacion = pUsuario;
                    contratoPoliza.TipoSolicitudCodigo = ((int)EnumeratorEstadoPoliza.Con_aprobacion_de_polizas).ToString();
                    _context.ContratoPoliza.Update(contratoPoliza);

                    fechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();

                    getDataNotifMsjAseguradora(ref msjNotificacion, contratoPoliza, ref fechaFirmaContrato, ref objVistaContratoGarantiaPoliza, ListVista);

                    //     Task<Respuesta> result = EnviarCorreoGestionPoliza(correo, settings.MailServer,
                    //settings.MailPort, settings.Password, settings.Sender,
                    //objVistaContratoGarantiaPoliza, fechaFirmaContrato, pIdTemplate, msjNotificacion);

                    //////////////////////////////////  EnviarCorreoGestionPoliza
                    bool blEnvioCorreo = false;
                    //Respuesta respuesta = new Respuesta();

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
                        //Contrato contrato;
                        contrato = _context.Contrato.Where(r => r.ContratoId == objVistaContratoGarantiaPoliza.IdContrato).FirstOrDefault();

                        fechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();

                        //datos basicos generales, aplican para los 4 mensajes
                        template = template.Replace("_Tipo_Contrato_", objVistaContratoGarantiaPoliza.TipoContrato);
                        template = template.Replace("_Numero_Contrato_", objVistaContratoGarantiaPoliza.NumeroContrato);
                        template = template.Replace("_Fecha_Firma_Contrato_", fechaFirmaContrato); //Formato (dd/MM/aaaa)
                        template = template.Replace("_Nombre_Contratista_", objVistaContratoGarantiaPoliza.NombreContratista);
                        template = template.Replace("_Valor_Contrato_", objVistaContratoGarantiaPoliza.ValorContrato);  //fomato miles .
                        template = template.Replace("_Plazo_", objVistaContratoGarantiaPoliza.PlazoContrato);

                        if (msjNotificacion != null)
                        {
                            template = template.Replace("_Nombre_Aseguradora_", msjNotificacion.NombreAseguradora);
                            template = template.Replace("_Numero_Poliza_", msjNotificacion.NumeroPoliza);
                            template = template.Replace("_Fecha_Revision_", msjNotificacion.FechaRevision);
                            template = template.Replace("_Estado_Revision_", msjNotificacion.EstadoRevision);
                            template = template.Replace("_Observaciones_", msjNotificacion.Observaciones);

                            template = template.Replace("_Fecha_Aprobacion_Poliza", msjNotificacion.FechaAprobacion);

                            if (!string.IsNullOrEmpty(msjNotificacion.NumeroDRP))
                                template = template.Replace("_NumeroDRP_", msjNotificacion.NumeroDRP);

                        }
                        blEnvioCorreo = Helpers.Helpers.EnviarCorreo(correo, "Gestión Poliza", template, settings.Sender, settings.Password, settings.MailServer, settings.MailPort);

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
                        respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, correo, "Gestión Pólizas");
                        return respuesta;

                    }
                    catch (Exception ex)
                    {

                        respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesContratoPoliza.Error };
                        respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, correo, "Gestión Pólizas") + ": " + ex.ToString() + ex.InnerException;
                        return respuesta;
                    }

                    ///////////////////////////////////////////////////

                    return respuesta = new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Data = contratoPoliza,
                        Code = ConstantMessagesContratoPoliza.OperacionExitosa,
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

        private string getCorreos(int perfilId)
        {
            //[Usuario], [UsuarioPerfil] , [Perfil]
            string lstCorreos = "";

            //            1   Administrador  - //2   Técnica
            //3   Financiera - //4   Jurídica
            //5   Administrativa - //6   Miembros Comite
            //7   Secretario comité - //8   Supervisor
            List<UsuarioPerfil> lstUsuariosPerfil = new List<UsuarioPerfil>();

            lstUsuariosPerfil = _context.UsuarioPerfil.Where(r => r.Activo == true && r.PerfilId == perfilId).ToList();

            List<Usuario> lstUsuarios = new List<Usuario>();

            foreach (var item in lstUsuariosPerfil)
            {
                lstUsuarios = _context.Usuario.Where(r => r.UsuarioId == item.UsuarioId).ToList();

                foreach (var usuario in lstUsuarios)
                {
                    lstCorreos = lstCorreos += usuario.Email + "";
                }
            }
            return lstCorreos;
        }


        //public async Task<Respuesta> RecoverPasswordByEmailAsync(Usuario pUsuario, string pDominio, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        public async Task<Respuesta> EnviarCorreoGestionPoliza(string lstMails, string pMailServer, int pMailPort, string pPassword, string pSentender, VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza, string fechaFirmaContrato, int pIdTemplate, NotificacionMensajeGestionPoliza objNotificacionAseguradora = null)
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

                if (objNotificacionAseguradora != null)
                {
                    template = template.Replace("_Nombre_Aseguradora_", objNotificacionAseguradora.NombreAseguradora);
                    template = template.Replace("_Numero_Poliza_", objNotificacionAseguradora.NumeroPoliza);
                    template = template.Replace("_Fecha_Revision_", objNotificacionAseguradora.FechaRevision);
                    template = template.Replace("_Estado_Revision_", objNotificacionAseguradora.EstadoRevision);
                    template = template.Replace("_Observaciones_", objNotificacionAseguradora.Observaciones);

                    template = template.Replace("_Fecha_Aprobacion_Poliza", objNotificacionAseguradora.FechaAprobacion);

                    if (!string.IsNullOrEmpty(objNotificacionAseguradora.NumeroDRP))
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

                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesContratoPoliza.Error };
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
            List<Contrato> ListContratos = await _context.Contrato.Where(r => r.Contratacion.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.Registrados.ToString()&& !(bool)r.Eliminado).ToListAsync();

            foreach (var contrato in ListContratos)
            {
                try
                {
                    Contratacion contratacion = null;
                    contratacion = _context.Contratacion.Where(r => r.ContratacionId == contrato.ContratacionId).FirstOrDefault();

                    string strNumeroSolicitudContratacion="";
                    string strTipoConContratacion = "";

                    Dominio TipoSolicitudCodigoContratacion= null;
                    string strTipoSolicitudContratacion = "";
                    string strTipoSolicitudCodigoContratacion = "";

                    if (contratacion != null)
                    {
                        strNumeroSolicitudContratacion = contratacion.NumeroSolicitud;

                        TipoSolicitudCodigoContratacion = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratacion.EstadoSolicitudCodigo.Trim(), (int)EnumeratorTipoDominio.Estado_Solicitud);
                        if (TipoSolicitudCodigoContratacion != null)
                        {
                            strTipoSolicitudCodigoContratacion = TipoSolicitudCodigoContratacion.Codigo;
                            strTipoSolicitudContratacion = TipoSolicitudCodigoContratacion.Nombre;
                        }                                             
                                                    

                    }                        

                    ContratoPoliza contratoPoliza=null;
                    contratoPoliza = await _commonService.GetLastContratoPolizaByContratoId(contrato.ContratoId);

                    int ContratoPolizaIdValor = 0;

                    //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    string strTipoSolicitudCodigoContratoPoliza = (Convert.ToInt32( ConstanCodigoTipoSolicitud.Contratacion)).ToString();
                    string strTipoSolicitudNombreContratoPoliza = "Contratación"; //contratacion o modif contractual - 

                    string strEstadoSolicitudCodigoContratoPoliza = ((int)EnumeratorEstadoPoliza.Sin_radicacion_polizas).ToString();
                    string strEstadoSolicitudNombreContratoPoliza = "Sin radicación de pólizas";

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio TipoSolicitudCodigoContratoPoliza;
                    Dominio EstadoSolicitudCodigoContratoPoliza;

                    if (contratoPoliza != null)
                    {
                        ContratoPolizaIdValor = contratoPoliza.ContratoPolizaId;
                        if (contratoPoliza.TipoSolicitudCodigo != null)
                        {
                            //TipoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_Modificacion_Contrato_Poliza);
                            TipoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo.Trim(), (int)EnumeratorTipoDominio.Tipo_de_Solicitud);
                        
                        if (TipoSolicitudCodigoContratoPoliza != null)
                            {
                                strTipoSolicitudCodigoContratoPoliza = TipoSolicitudCodigoContratoPoliza.Nombre;
                                strTipoSolicitudNombreContratoPoliza= TipoSolicitudCodigoContratoPoliza.Codigo;
                            }                            
                    }                    

                        EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.EstadoPolizaCodigo.Trim(), (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                        if (EstadoSolicitudCodigoContratoPoliza != null)
                        {
                            strEstadoSolicitudCodigoContratoPoliza = EstadoSolicitudCodigoContratoPoliza.Codigo;
                            strEstadoSolicitudNombreContratoPoliza = EstadoSolicitudCodigoContratoPoliza.Nombre;

                        }                            

                    }
                    bool bRegistroCompleto=false;
                    string strRegistroCompleto= "Incompleto";

                    if (contrato.RegistroCompleto != null)
                    {
                        strRegistroCompleto = (bool)contrato.RegistroCompleto ? "Completo" : "Incompleto";

                    }                         

                    //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                    GrillaContratoGarantiaPoliza contratoGrilla = new GrillaContratoGarantiaPoliza
                    {
                        ContratoId = contrato.ContratoId,
                        ContratoPolizaId = ContratoPolizaIdValor,
                        //FechaFirma = contrato.FechaFirmaContrato.ToString("dd/mm/yyyy")?"":"",
                        FechaFirma = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString(),
                        //FechaFirma = contrato.FechaFirmaContrato.ToString(),
                        NumeroContrato = contrato.NumeroContrato,
                        //TipoSolicitud= contratoPoliza.TipoSolicitudCodigo
                        //EstadoRegistro { get; set; }
                        NumeroSolicitudContratacion=strNumeroSolicitudContratacion,
                        //Departamento = departamento.Descripcion,
                        //Municipio = municipio.Descripcion,
                        //InstitucionEducativa = _context.InstitucionEducativaSede.Find(contrato.InstitucionEducativaId).Nombre,
                        //Sede = _context.InstitucionEducativaSede.Find(contrato.SedeId).Nombre,
                        TipoSolicitud = strTipoSolicitudNombreContratoPoliza,
                        TipoSolicitudCodigo = strTipoSolicitudCodigoContratoPoliza,

                        TipoSolicitudCodigoContratacion=strTipoSolicitudCodigoContratacion,
                        TipoSolicitudContratacion=strTipoSolicitudContratacion,

                        EstadoPoliza = strEstadoSolicitudNombreContratoPoliza
                        ,
                        EstadoPolizaCodigo= strEstadoSolicitudCodigoContratoPoliza,
                        RegistroCompleto = contrato.RegistroCompleto,
                        RegistroCompletoNombre = strRegistroCompleto,

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
                        ContratoPolizaId= 0,
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
                        RegistroCompleto = false,
                        RegistroCompletoNombre="ERROR",

                    };
                    ListContratoGrilla.Add(proyectoGrilla);
                }
            }
            return ListContratoGrilla.OrderByDescending(r => r.TipoSolicitud).ToList();

        }


        public async Task<List<VistaContratoGarantiaPoliza>> ListVistaContratoGarantiaPoliza(int pContratoId=0)
        {
            List<VistaContratoGarantiaPoliza> ListContratoGrilla = new List<VistaContratoGarantiaPoliza>();
            //Fecha de firma del contrato ??? FechaFirmaContrato , [Contrato] -(dd / mm / aaaa)

            //Tipo de solicitud ??? ContratoPoliza - TipoSolicitudCodigo          

            List<Contrato> ListContratos = new List<Contrato>();
            //ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado).Include(r => r.FechaFirmaContrato).Include(r => r.NumeroContrato).Include(r => r.Estado).Distinct().ToListAsync();

            //return await _context.CofinanciacionAportante.Where(r => !(bool)r.Eliminado && r.TipoAportanteId == pTipoAportanteID).Include(r => r.Cofinanciacion).ToListAsync();

            //ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado).Include(r => r.FechaFirmaContrato).ToListAsync();

            //item.CofinanciacionAportante = await _context.CofinanciacionAportante.Where(r => !(bool)r.Eliminado && r.CofinanciacionId == item.CofinanciacionId).IncludeFilter(r => r.CofinanciacionDocumento.Where(r => !(bool)r.Eliminado)).ToListAsync();

            if (pContratoId == 0)
            {
                ListContratos = await _context.Contrato.Where(r => (bool)r.Eliminado==false).Distinct()
            .ToListAsync();

            }
            else
            {
                ListContratos = await _context.Contrato.Where(r => (bool)r.Eliminado==false && r.ContratoId == pContratoId).Distinct()
          .ToListAsync();

            }     
                     

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
                    ContratoPoliza contratoPoliza=null;
                    contratoPoliza = await _commonService.GetLastContratoPolizaByContratoId(contrato.ContratoId);

                    Dominio TipoSolicitudCodigoContratoPoliza=null;
                    string strTipoSolicitudCodigoContratoPoliza = "Sin radicación de pólizas";
                    string strFechaFirmaContrato = string.Empty;

                    if (contratoPoliza != null)
                    {
                        if (contratoPoliza.TipoSolicitudCodigo != null)
                        {
                            TipoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo.Trim(), (int)EnumeratorTipoDominio.Tipo_de_Solicitud);

                            if (TipoSolicitudCodigoContratoPoliza != null)
                                strTipoSolicitudCodigoContratoPoliza = TipoSolicitudCodigoContratoPoliza.Nombre;
                        }
                    }
                    strFechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();

                   //ContratoPoliza contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    Contratacion contratacion = null;
                    contratacion = await _commonService.GetContratacionByContratacionId(contrato.ContratacionId);

                    string strContratistaNombre = string.Empty;
                    string strContratistaNumeroIdentificacion = string.Empty;

                    Dominio TipoDocumentoCodigoContratista;
                    string strTipoDocumentoContratista = string.Empty;

                    Contratista contratista=null;
                    if (contratacion != null)
                    {
                        if(contratacion.ContratistaId!=null)
                        contratista = await _commonService.GetContratistaByContratistaId((Int32)contratacion.ContratistaId);

                        if (contratista != null)
                        {
                            strContratistaNombre = contratista.Nombre;
                            //Nit  
                            strContratistaNumeroIdentificacion = contratista.NumeroIdentificacion.ToString();

                             TipoDocumentoCodigoContratista = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratista.TipoIdentificacionCodigo, (int)EnumeratorTipoDominio.Tipo_Documento);

                            if (TipoDocumentoCodigoContratista != null)
                                strTipoDocumentoContratista = TipoDocumentoCodigoContratista.Nombre;                            
                        }
                    }

                    //TipoContrato = contrato.TipoContratoCodigo   ??? Obra  ????

                    //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    Int32 plazoDias, plazoMeses;
                    //25meses / 04 días

                    if (!string.IsNullOrEmpty(contrato.PlazoFase2ConstruccionDias.ToString()))
                        plazoDias = Convert.ToInt32(contrato.PlazoFase1PreDias);

                    else
                        plazoDias = Convert.ToInt32(contrato.PlazoFase2ConstruccionDias);

                    if (!string.IsNullOrEmpty(contrato.PlazoFase2ConstruccionMeses.ToString()))
                        plazoMeses = Convert.ToInt32(contrato.PlazoFase1PreMeses);
                    else
                        plazoMeses = Convert.ToInt32(contrato.PlazoFase2ConstruccionMeses);

                    string PlazoContratoFormat = plazoMeses.ToString("00") + " meses / " + plazoDias.ToString("00") + " dias ";

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio TipoContratoCodigoContrato = await _commonService.GetDominioByNombreDominioAndTipoDominio(contrato.TipoContratoCodigo, (int)EnumeratorTipoDominio.Tipo_Contrato);
                                                                                
                    string strTipoContratoCodigoContratoNombre = string.Empty;

                    if (TipoContratoCodigoContrato != null)
                        strTipoContratoCodigoContratoNombre = TipoContratoCodigoContrato.Nombre;

                    //Dominio TipoModificacionCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoModificacionCodigo, (int)EnumeratorTipoDominio.Tipo_Modificacion_Contrato_Poliza);
                    VistaContratoGarantiaPoliza proyectoGrilla = new VistaContratoGarantiaPoliza
                    {
                        IdContrato = contrato.ContratoId,
                        TipoContrato = strTipoContratoCodigoContratoNombre,
                        NumeroContrato = contrato.NumeroContrato,
                        ObjetoContrato = contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault().Objeto,
                        NombreContratista = strContratistaNombre,
                        TipoDocumento= strTipoDocumentoContratista,

                        //Nit  
                        NumeroIdentificacion = strContratistaNumeroIdentificacion,

                        ValorContrato = contrato.Valor.ToString(),

                        PlazoContrato = PlazoContratoFormat,

                        //EstadoRegistro 
                        //public bool? RegistroCompleto { get; set; }                         

                        DescripcionModificacion = "resumen", // resumen   TEMPORAL REV

                        //TipoModificacion = TipoModificacionCodigoContratoPoliza.Nombre
                        TipoModificacion = "Tipo modificacion",

                        TipoSolicitud = strTipoSolicitudCodigoContratoPoliza,

                        FechaFirmaContrato = strFechaFirmaContrato,

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
                        IdContrato = 0,
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
                        TipoModificacion = "ERROR", 
                        TipoDocumento="ERROR"

                    };
                    ListContratoGrilla.Add(proyectoGrilla);
                }
            }
            return ListContratoGrilla.OrderByDescending(r => r.TipoSolicitud).ToList();

        }

    }
}
