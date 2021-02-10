﻿using asivamosffie.model.APIModels;
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
            NotificacionMensajeGestionPoliza msjNotificacion = new NotificacionMensajeGestionPoliza();
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
                msjNotificacion.Observaciones = contratoPoliza.ObservacionesRevisionGeneral;
                msjNotificacion.FechaAprobacion = contratoPoliza.FechaAprobacion != null ? Convert.ToDateTime(contratoPoliza.FechaAprobacion).ToString("dd/MM/yyyy") : contratoPoliza.FechaAprobacion.ToString();

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
                contratoPoliza.ContratacionId = 0;
                //List<PolizaGarantia> contratoPolizaGarantia = await _context.PolizaGarantia.Where(r => r.ContratoPolizaId == p && !(bool)r.Eliminado).IncludeFilter(r => r.CofinanciacionDocumento.Where(r => !(bool)r.Eliminado)).ToListAsync();
                List<PolizaGarantia> contratoPolizaGarantia = await _context.PolizaGarantia.Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId).ToListAsync();

                contratoPoliza.PolizaGarantia = contratoPolizaGarantia;

                polizaObservacion = _context.PolizaObservacion.Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId).FirstOrDefault();

                //List<PolizaGarantia> contratoPolizaGarantia = await _context.PolizaGarantia.Where(r => r.ContratoPolizaId == p && !(bool)r.Eliminado).IncludeFilter(r => r.CofinanciacionDocumento.Where(r => !(bool)r.Eliminado)).ToListAsync();
                List<PolizaObservacion> contratoPolizaObservacion = await _context.PolizaObservacion.Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId).ToListAsync();

                contratoPoliza.PolizaObservacion = contratoPolizaObservacion;

                if (contrato != null)
                {
                    //contratoPoliza = _context.ContratoPoliza.Where(r => !(bool)r.Eliminado && r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();
                    contratoPoliza = _context.ContratoPoliza.Where(r => r.ContratoId == pContratoId
                    //&&(bool)r.Estado==true&&r.Eliminado==0
                    ).OrderByDescending(r => r.ContratoPolizaId).FirstOrDefault();

                    if (contratoPoliza != null)
                    {
                        contratoPoliza.ContratacionId = contrato.ContratacionId;

                    }

                }
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
                    //este dato no se esta enviando por frontend, me parece que es mas facil por aca
                    //jflroez 20201124
                    //if (polizaGarantia.PolizaGarantiaId == 0)
                    var polizaGarantiaExiste = _context.PolizaGarantia.Where(x => x.TipoGarantiaCodigo == polizaGarantia.TipoGarantiaCodigo && x.ContratoPolizaId == polizaGarantia.ContratoPolizaId).FirstOrDefault();
                    if (polizaGarantiaExiste == null)
                    {
                        //Auditoria
                        strCrearEditar = "REGISTRAR POLIZA GARANTIA";
                        polizaGarantia.FechaCreacion = DateTime.Now;
                        _context.PolizaGarantia.Add(polizaGarantia);
                        //await _context.SaveChangesAsync();
                        _context.SaveChanges();

                    }
                    else
                    {
                        strCrearEditar = "EDIT POLIZA GARANTIA";
                        PolizaGarantia polizaGarantiaBD = null;
                        polizaGarantiaBD = polizaGarantiaExiste;

                        if (polizaGarantiaBD != null)
                        {
                            //PolizaGarantia poli
                            polizaGarantia.FechaModificacion = DateTime.Now;
                            polizaGarantiaBD.TipoGarantiaCodigo = polizaGarantia.TipoGarantiaCodigo;
                            polizaGarantiaBD.EsIncluidaPoliza = polizaGarantia.EsIncluidaPoliza;
                            _context.PolizaGarantia.Update(polizaGarantiaBD);

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
                            Code = ConstantMessagesContratoPoliza.CreadoCorrrectamente,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias,
                            ConstantMessagesContratoPoliza.OperacionExitosa,
                            //contratoPoliza
                            idAccionCrearContratoPoliza
                            , polizaGarantia.UsuarioCreacion
                            //"UsuarioCreacion"
                            , strCrearEditar
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

        public async Task<Respuesta> InsertEditPolizaObservacion(PolizaObservacion polizaObservacion, AppSettingsService appSettingsService)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Poliza_Observacion, (int)EnumeratorTipoDominio.Acciones);

            //GUARDAR
            //PolizaObservacion - FechaRevision
            //    EstadoRevisionCodigo - PolizaObservacion

            string strCrearEditar, strUsuario = "";
            try
            {
                //polizaObservacion.Observacion = Helpers.Helpers.CleanStringInput(polizaObservacion.Observacion);
                if (polizaObservacion != null)
                {
                    int id = 0;
                    if (polizaObservacion.PolizaObservacionId == 0)
                    {
                        //Auditoria
                        strCrearEditar = "REGISTRAR POLIZA OBSERVACION";
                        strUsuario = polizaObservacion.UsuarioCreacion;
                        polizaObservacion.FechaCreacion = DateTime.Now;

                        _context.PolizaObservacion.Add(polizaObservacion);
                        await _context.SaveChangesAsync();
                        id = polizaObservacion.ContratoPolizaId;
                    }
                    else
                    {
                        strCrearEditar = "EDIT POLIZA OBSERVACION";
                        strUsuario = polizaObservacion.UsuarioModificacion;
                        PolizaObservacion polizaObservacionBD = null;

                        polizaObservacionBD = _context.PolizaObservacion.Where(r => r.PolizaObservacionId == polizaObservacion.PolizaObservacionId).FirstOrDefault();
                        if (polizaObservacion != null)
                        {
                            polizaObservacion.FechaModificacion = DateTime.Now;
                            polizaObservacionBD.Observacion = polizaObservacion.Observacion;
                            polizaObservacionBD.FechaRevision = polizaObservacion.FechaRevision;
                            polizaObservacionBD.EstadoRevisionCodigo = polizaObservacion.EstadoRevisionCodigo;
                            _context.PolizaObservacion.Update(polizaObservacionBD);

                        }
                        id = polizaObservacionBD.ContratoPolizaId;
                        //_context.CuentaBancaria.Update(cuentaBancariaAntigua);
                    }

                    //    dependiendo del estado debo enviar coprreo
                    Template TemplateRecoveryPassword = new Template();
                    if (polizaObservacion.EstadoRevisionCodigo == ConstanCodigoEstadoRevision.aprobada)
                    {
                        TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjFiduciariaJuridicaGestionPoliza);
                    }
                    else
                    {
                        TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjSupervisorGestionPoliza);
                    }

                    string template = TemplateRecoveryPassword.Contenido;

                    //string urlDestino = pDominio;
                    //asent/img/logo  
                    var contratopoliza = _context.ContratoPoliza.Where(x => x.ContratoPolizaId == id).
                        Include(x => x.Contrato).FirstOrDefault();
                    var ListVista = ListVistaContratoGarantiaPoliza(contratopoliza.Contrato.ContratoId).Result.FirstOrDefault();
                    var fechaFirmaContrato = contratopoliza.Contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contratopoliza.Contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : "";

                    //datos basicos generales, aplican para los 4 mensajes
                    template = template.Replace("_Tipo_Contrato_", ListVista.TipoContrato);
                    template = template.Replace("_Numero_Contrato_", ListVista.NumeroContrato);
                    template = template.Replace("_Fecha_Firma_Contrato_", ListVista.FechaFirmaContrato);
                    template = template.Replace("_Nombre_Contratista_", ListVista.NombreContratista);
                    template = template.Replace("_Valor_Contrato_", string.Format("${0:#,0}", ListVista.ValorContrato.ToString()));  //fomato miles .
                    template = template.Replace("_Plazo_", ListVista.PlazoContrato);
                    template = template.Replace("_LinkF_", appSettingsService.DominioFront);

                    template = template.Replace("_Fecha_Revision_", polizaObservacion.FechaRevision.ToString("dd/MM/yyyy"));
                    template = template.Replace("_Estado_Revision_", _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Revision_Poliza && x.Codigo == polizaObservacion.EstadoRevisionCodigo).Select(x => x.Nombre).FirstOrDefault());
                    template = template.Replace("_Observaciones_", Helpers.Helpers.HtmlStringLimpio(polizaObservacion.Observacion));
                    template = template.Replace("_Nombre_Aseguradora_", contratopoliza.NombreAseguradora);
                    template = template.Replace("_Numero_Poliza_", contratopoliza.NumeroPoliza);

                    if (polizaObservacion.EstadoRevisionCodigo == ConstanCodigoEstadoRevision.aprobada)
                    {
                        //no envio correo porque existe otro evento para ello                    
                    }
                    else
                    {
                        string destinatario = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Supervisor && (bool)x.Activo && (bool)x.Usuario.Activo)
                            .Select(x => x.Usuario.Email).FirstOrDefault();//esto va a cambiar en fase 2
                        var blEnvioCorreo = Helpers.Helpers.EnviarCorreo(destinatario, "Gestión Poliza", template, appSettingsService.Sender, appSettingsService.Password, appSettingsService.MailServer, appSettingsService.MailPort);
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
                            , polizaObservacion.UsuarioCreacion
                            //"UsuarioCreacion"
                            , "REGISTRAR POLIZA OBSERVACION"
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
            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Editar_Contrato_Poliza, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (contratoPoliza != null)
                {
                    ContratoPoliza contratoPolizaBD = null;
                    contratoPolizaBD = _context.ContratoPoliza.Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId).FirstOrDefault();

                    bool ContratoEsDevuelto = false;

                    if (contratoPolizaBD != null)
                    {
                         
                        Contrato contrato = _context.Contrato.Where(r => r.ContratoId == contratoPoliza.ContratoId).FirstOrDefault();

                        if (contrato != null)
                            if (contrato.EstaDevuelto != null)
                                ContratoEsDevuelto = Convert.ToBoolean(contrato.EstaDevuelto);

           
                        if (contratoPolizaBD.RegistroCompleto == true)
                            contratoPolizaBD.RegistroCompleto = await ValidarRegistroCompletoSeguros(contratoPoliza);

                        contratoPolizaBD.FechaModificacion = DateTime.Now;
                        contratoPolizaBD.UsuarioModificacion = contratoPoliza.UsuarioCreacion;
                        contratoPolizaBD.RegistroCompleto = ValidarRegistroCompletoContratoPoliza(contratoPoliza, ContratoEsDevuelto);

                        contratoPolizaBD.NombreAseguradora = contratoPoliza.NombreAseguradora;
                        contratoPolizaBD.Observaciones = contratoPoliza.Observaciones;
                        contratoPolizaBD.NumeroPoliza = contratoPoliza.NumeroPoliza;
                        contratoPolizaBD.NumeroCertificado = contratoPoliza.NumeroCertificado;
                        contratoPolizaBD.ObservacionesRevisionGeneral = contratoPoliza.ObservacionesRevisionGeneral;
                        contratoPolizaBD.ResponsableAprobacion = contratoPoliza.ResponsableAprobacion;
                        contratoPolizaBD.EstadoPolizaCodigo = contratoPoliza.EstadoPolizaCodigo;
                        contratoPolizaBD.FechaExpedicion = contratoPoliza.FechaExpedicion;
                        contratoPolizaBD.Vigencia = contratoPoliza.Vigencia;
                        contratoPolizaBD.VigenciaAmparo = contratoPoliza.VigenciaAmparo;
                        contratoPolizaBD.ValorAmparo = contratoPoliza.ValorAmparo;
                        contratoPolizaBD.CumpleDatosAsegurado = contratoPoliza.CumpleDatosAsegurado;
                        contratoPolizaBD.CumpleDatosBeneficiario = contratoPoliza.CumpleDatosBeneficiario;
                        contratoPolizaBD.CumpleDatosTomador = contratoPoliza.CumpleDatosTomador;
                        contratoPolizaBD.IncluyeReciboPago = contratoPoliza.IncluyeReciboPago;
                        contratoPolizaBD.IncluyeCondicionesGenerales = contratoPoliza.IncluyeCondicionesGenerales;
                        contratoPolizaBD.FechaAprobacion = contratoPoliza.FechaAprobacion;
                        contratoPolizaBD.Estado = contratoPoliza.Estado;
                        LimpiarEntradasContratoPoliza(ref contratoPolizaBD);
                        _context.ContratoPoliza.Update(contratoPolizaBD);

                    }
                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContratoPoliza.OperacionExitosa,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                                                                                            (int)enumeratorMenu.GestionarGarantias,
                                                                                            ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente,
                                                                                            idAccionCrearContratoPoliza,
                                                                                            contratoPoliza.UsuarioCreacion,
                                                                                            "EDITAR CONTRATO POLIZA"
                                                                                          )
                        };

                }
                else
                {
                    return new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.ErrorInterno, Message = ex.InnerException.ToString().Substring(0, 500) };
            }

        }

        public async Task<Respuesta> InsertContratoPoliza(ContratoPoliza contratoPoliza, AppSettingsService appSettingsService)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Contrato_Poliza, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                //Valida si llega el objeto ContratoPoliza
                if (contratoPoliza == null)
                {
                    return new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = false,
                        IsValidation = true,
                        Code = ConstantMessagesContratoPoliza.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.Error, idAccion, contratoPoliza.UsuarioCreacion, "ERROR PETICIÓN SERVICIO")
                    };
                }


                //Validar Contrato Poliza Duplicado
                if (_context.ContratoPoliza.Where(r => r.ContratoId == contratoPoliza.ContratoId).ToList().Count() > 1)
                {
                    return new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = false,
                        IsValidation = true,
                        Code = ConstantMessagesContratoPoliza.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.ErrorDuplicarPoliza, idAccion, contratoPoliza.UsuarioCreacion, "ERROR PETICIÓN SERVICIO")
                    };
                }


                contratoPoliza.FechaCreacion = DateTime.Now;
                contratoPoliza.Eliminado = false;
                Contrato contrato = _context.Contrato.Where(r => r.ContratoId == contratoPoliza.ContratoId).FirstOrDefault();
                bool ContratoEsDevuelto = false;

                if (contrato != null)
                    if (contrato.EstaDevuelto != null)
                        ContratoEsDevuelto = Convert.ToBoolean(contrato.EstaDevuelto);

                contratoPoliza.RegistroCompleto = ValidarRegistroCompletoContratoPoliza(contratoPoliza, ContratoEsDevuelto);

                LimpiarEntradasContratoPoliza(ref contratoPoliza);

                _context.ContratoPoliza.Add(contratoPoliza);

                _context.SaveChanges();

                return await EnviarCorreosCrearPoliza(contratoPoliza, contrato, appSettingsService);
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContratoPoliza.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.Error, idAccion, contratoPoliza.UsuarioCreacion, ex.InnerException.ToString())
                };
            }
        }

        private async Task<Respuesta> EnviarCorreosCrearPoliza(ContratoPoliza contratoPoliza, Contrato contrato, AppSettingsService appSettingsService)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_Correo_Crear_Contrato_Poliza, (int)EnumeratorTipoDominio.Acciones);


            try
            {
                string fechaFirmaContrato = string.Empty;
                string correo = string.Empty;

                VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza;

                List<Usuario> ListUsuarioCorreos = getCorreos((int)EnumeratorPerfil.Supervisor);

                int idAccionEditarContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente, (int)EnumeratorTipoDominio.Acciones);

                objVistaContratoGarantiaPoliza = new VistaContratoGarantiaPoliza();

                List<VistaContratoGarantiaPoliza> ListVista = new List<VistaContratoGarantiaPoliza>();
                ListVista = await ListVistaContratoGarantiaPoliza(contratoPoliza.ContratacionId);

                NotificacionMensajeGestionPoliza msjNotificacion = new NotificacionMensajeGestionPoliza();
                getDataNotifMsjAseguradora(ref msjNotificacion, contratoPoliza, ref fechaFirmaContrato, ref objVistaContratoGarantiaPoliza, ListVista);


                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjSupervisorJuridicaGestionPoliza);

                string template = TemplateRecoveryPassword.Contenido;

                contrato = _context.Contrato.Where(r => r.ContratoId == objVistaContratoGarantiaPoliza.IdContrato).FirstOrDefault();

                fechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();

                //datos basicos generales, aplican para los 4 mensajes
                template = template.Replace("_Tipo_Contrato_", objVistaContratoGarantiaPoliza.TipoContrato);
                template = template.Replace("_Numero_Contrato_", objVistaContratoGarantiaPoliza.NumeroContrato);
                template = template.Replace("_Fecha_Firma_Contrato_", fechaFirmaContrato); //Formato (dd/MM/aaaa)
                template = template.Replace("_Nombre_Contratista_", objVistaContratoGarantiaPoliza.NombreContratista);
                template = template.Replace("_Valor_Contrato_", string.Format("${0:#,0}", objVistaContratoGarantiaPoliza.ValorContrato.ToString()));  //fomato miles .
                template = template.Replace("_Plazo_", objVistaContratoGarantiaPoliza.PlazoContrato);
                template = template.Replace("_LinkF_", appSettingsService.DominioFront);

                if (msjNotificacion != null)
                {
                    template = template.Replace("_Nombre_Aseguradora_", msjNotificacion.NombreAseguradora);
                    template = template.Replace("_Numero_Poliza_", msjNotificacion.NumeroPoliza);
                    template = template.Replace("_Fecha_Revision_", msjNotificacion.FechaRevision);
                    template = template.Replace("_Estado_Revision_", _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Revision_Poliza && x.Codigo == msjNotificacion.EstadoRevision).Select(x => x.Nombre).FirstOrDefault());
                    template = template.Replace("_Observaciones_", msjNotificacion.Observaciones);
                    template = template.Replace("_Fecha_Aprobacion_Poliza", msjNotificacion.FechaAprobacion);
                    template = template.Replace("_NumeroDRP_", msjNotificacion.NumeroDRP);

                }

                bool blEnvioCorreo = true;

                foreach (var Usuario in ListUsuarioCorreos)
                {
                    if (Helpers.Helpers.EnviarCorreo(Usuario.Email, "Gestión Poliza", template, appSettingsService.Sender, appSettingsService.Password, appSettingsService.MailServer, appSettingsService.MailPort) == false)
                        blEnvioCorreo = false;
                }

                if (blEnvioCorreo)
                    return new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.CorreoEnviado };

                else
                    return new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.ErrorEnviarCorreo };

            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContratoPoliza.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.Error, idAccion, contratoPoliza.UsuarioCreacion, ex.InnerException.ToString())
                };
            }

        }

        public async Task<Respuesta> CambiarEstadoPolizaByContratoId(int pContratoId, string pCodigoNuevoEstadoPoliza, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Gestion_Poliza, (int)EnumeratorTipoDominio.Acciones);


            bool ContratoEsDevuelto = false;
            try
            {
                Contrato contrato = null;
                contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

                ContratoPoliza contratoPoliza = null;
                if (contrato != null)
                {
                    //contratoPoliza = _context.ContratoPoliza.Find(contrato.ContratoId);                    
                    contratoPoliza = await _commonService.GetLastContratoPolizaByContratoId(contrato.ContratoId);
                }

                if (contratoPoliza != null)
                {
                    //SesionComiteSolicitud sesionComiteSolicitudOld = _context.SesionComiteSolicitud.Find(/*pSesionComiteSolicitud*/);
                    contratoPoliza.UsuarioModificacion = pUsuarioModifica;
                    contratoPoliza.FechaModificacion = DateTime.Now;
                    contratoPoliza.EstadoPolizaCodigo = pCodigoNuevoEstadoPoliza;

                    contrato = _context.Contrato.Where(r => r.ContratoId == contratoPoliza.ContratoId).FirstOrDefault();

                    if (contrato != null)
                    {
                        if (contrato.EstaDevuelto != null)
                        {
                            ContratoEsDevuelto = Convert.ToBoolean(contrato.EstaDevuelto);
                        }
                    }

                    contratoPoliza.RegistroCompleto = ValidarRegistroCompletoContratoPoliza(contratoPoliza, ContratoEsDevuelto);

                    if (contratoPoliza.RegistroCompleto == true)
                        contratoPoliza.RegistroCompleto = await ValidarRegistroCompletoSeguros(contratoPoliza);

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
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_Correo_Crear_Contrato_Poliza, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                string fechaFirmaContrato = string.Empty;

                int idAccionEditarContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente, (int)EnumeratorTipoDominio.Acciones);
                VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza = new VistaContratoGarantiaPoliza();
                List<VistaContratoGarantiaPoliza> ListVista = new List<VistaContratoGarantiaPoliza>();
                ListVista = await ListVistaContratoGarantiaPoliza(0);

                NotificacionMensajeGestionPoliza msjNotificacion;
                msjNotificacion = new NotificacionMensajeGestionPoliza();

                getDataNotifMsjAseguradora(ref msjNotificacion, contratoPoliza, ref fechaFirmaContrato, ref objVistaContratoGarantiaPoliza, ListVista);


                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjSupervisorJuridicaGestionPoliza);

                string template = TemplateRecoveryPassword.Contenido;

                Contrato contrato;
                contrato = _context.Contrato.Where(r => r.ContratoId == objVistaContratoGarantiaPoliza.IdContrato).FirstOrDefault();

                fechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();

                //datos basicos generales, aplican para los 4 mensajes
                template = template.Replace("_Tipo_Contrato_", objVistaContratoGarantiaPoliza.TipoContrato);
                template = template.Replace("_Numero_Contrato_", objVistaContratoGarantiaPoliza.NumeroContrato);
                template = template.Replace("_Fecha_Firma_Contrato_", fechaFirmaContrato); //Formato (dd/MM/aaaa)
                template = template.Replace("_Nombre_Contratista_", objVistaContratoGarantiaPoliza.NombreContratista);
                template = template.Replace("_Valor_Contrato_", string.Format("${0:#,0}", objVistaContratoGarantiaPoliza.ValorContrato.ToString()));  //fomato miles .
                template = template.Replace("_Plazo_", objVistaContratoGarantiaPoliza.PlazoContrato);
                template = template.Replace("_LinkF_", settings.DominioFront);

                if (msjNotificacion != null)
                {
                    template = template.Replace("_Nombre_Aseguradora_", msjNotificacion.NombreAseguradora);
                    template = template.Replace("_Numero_Poliza_", msjNotificacion.NumeroPoliza);
                    template = template.Replace("_Fecha_Revision_", msjNotificacion.FechaRevision);
                    template = template.Replace("_Estado_Revision_", _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Revision_Poliza && x.Codigo == msjNotificacion.EstadoRevision).Select(x => x.Nombre).FirstOrDefault());
                    template = template.Replace("_Observaciones_", msjNotificacion.Observaciones);
                    template = template.Replace("_Fecha_Aprobacion_Poliza", msjNotificacion.FechaAprobacion);
                    template = template.Replace("_NumeroDRP_", msjNotificacion.NumeroDRP);

                }

                List<UsuarioPerfil> lstUsuariosPerfil = new List<UsuarioPerfil>();

                lstUsuariosPerfil = _context.UsuarioPerfil.Where(r => r.Activo == true && r.PerfilId == (int)EnumeratorPerfil.Supervisor).ToList();

                List<Usuario> lstUsuarios = new List<Usuario>();
                bool blEnvioCorreo = true;
                foreach (var item in lstUsuariosPerfil)
                {
                    lstUsuarios = _context.Usuario.Where(r => r.UsuarioId == item.UsuarioId).ToList();

                    foreach (var usuario in lstUsuarios)
                    {
                        if (Helpers.Helpers.EnviarCorreo(usuario.Email, "Gestión Poliza", template, settings.Sender, settings.Password, settings.MailServer, settings.MailPort) == false)
                            blEnvioCorreo = false;
                    }
                }

                if (blEnvioCorreo)
                    return new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.CorreoEnviado };

                else
                    return new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.ErrorEnviarCorreo };

            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContratoPoliza.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.Error, idAccion, contratoPoliza.UsuarioCreacion, ex.InnerException.ToString())
                };
            }

        }

        //public async Task GetConsignationValue(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        //paquete 2: estado diferente a Aprobado
        public async Task EnviarCorreoSupervisor4dPolizaNoAprobada2(string dominioFront, string mailServer, int mailPort, bool enableSSL, string password, string sender)
        {
            Respuesta respuesta = new Respuesta();
            string fechaFirmaContrato = "";

            VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza;

            List<ContratoPoliza> lstContratoPoliza;
            lstContratoPoliza = _context.ContratoPoliza
                .Where(r => r.TipoSolicitudCodigo != ((int)EnumeratorEstadoPoliza.Con_aprobacion_de_polizas)
                .ToString() && (bool)r.Eliminado == false).ToList();

            DateTime? FechaFirmaContrato_dt;
            DateTime RangoFechaConDiasHabiles;

            foreach (ContratoPoliza contratoPoliza in lstContratoPoliza)
            {
                RangoFechaConDiasHabiles = await _commonService.CalculardiasLaboralesTranscurridos(4, DateTime.Now);

                FechaFirmaContrato_dt = contratoPoliza?.FechaModificacion;

                if (FechaFirmaContrato_dt != null)

                    if (FechaFirmaContrato_dt <= RangoFechaConDiasHabiles)
                    {
                        try
                        {
                            int idAccionEditarContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente, (int)EnumeratorTipoDominio.Acciones);

                            objVistaContratoGarantiaPoliza = new VistaContratoGarantiaPoliza();

                            List<VistaContratoGarantiaPoliza> ListVista = new List<VistaContratoGarantiaPoliza>();
                            ListVista = await ListVistaContratoGarantiaPoliza(0);

                            //int pIdTemplate = (int)enumeratorTemplate.MsjSupervisorJuridicaGestionPoliza;
                            int pIdTemplate = (int)enumeratorTemplate.MsjFiduciariaJuridicaGestionPoliza;

                            NotificacionMensajeGestionPoliza msjNotificacion;
                            msjNotificacion = new NotificacionMensajeGestionPoliza();

                            getDataNotifMsjAseguradora(ref msjNotificacion, contratoPoliza, ref fechaFirmaContrato, ref objVistaContratoGarantiaPoliza, ListVista);


                            bool blEnvioCorreo = false;

                            Template TemplateRecoveryPassword = await _commonService.GetTemplateById(pIdTemplate);

                            string template = TemplateRecoveryPassword.Contenido;

                            Contrato contrato;
                            contrato = _context.Contrato.Where(r => r.ContratoId == objVistaContratoGarantiaPoliza.IdContrato).FirstOrDefault();

                            fechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();

                            //datos basicos generales, aplican para los 4 mensajes
                            template = template.Replace("_Tipo_Contrato_", objVistaContratoGarantiaPoliza.TipoContrato);
                            template = template.Replace("_Numero_Contrato_", objVistaContratoGarantiaPoliza.NumeroContrato);
                            template = template.Replace("_Fecha_Firma_Contrato_", fechaFirmaContrato); //Formato (dd/MM/aaaa)
                            template = template.Replace("_Nombre_Contratista_", objVistaContratoGarantiaPoliza.NombreContratista);
                            template = template.Replace("_Valor_Contrato_", string.Format("${0:#,0}", objVistaContratoGarantiaPoliza.ValorContrato.ToString()));  //fomato miles .
                            template = template.Replace("_Plazo_", objVistaContratoGarantiaPoliza.PlazoContrato);

                            if (msjNotificacion != null)
                            {
                                template = template.Replace("_Nombre_Aseguradora_", msjNotificacion.NombreAseguradora);
                                template = template.Replace("_Numero_Poliza_", msjNotificacion.NumeroPoliza);

                                if (!string.IsNullOrEmpty(msjNotificacion.NumeroDRP))
                                    template = template.Replace("_NumeroDRP_", msjNotificacion.NumeroDRP);

                            }

                            //1   Administrador  - 
                            //2   Técnica
                            //3   Financiera - 
                            //4   Jurídica
                            //5   Administrativa - 
                            //6   Miembros Comite
                            //7   Secretario comité - 
                            //8   Supervisor

                            List<UsuarioPerfil> lstUsuariosPerfil = _context.UsuarioPerfil
                                .Where(r => r.Activo == true && r.PerfilId == (int)EnumeratorPerfil.Supervisor)
                                .Include(u => u.Usuario)
                                .ToList();
                            lstUsuariosPerfil.ForEach(user =>
                            {
                                blEnvioCorreo = Helpers.Helpers.EnviarCorreo(user.Usuario.Email, "Gestión Poliza", template, sender, password, mailServer, mailPort);

                            });
                            if (blEnvioCorreo)
                                respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.CorreoEnviado };

                            else
                                respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.ErrorEnviarCorreo };

                            respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, sender, "Gestión Pólizas");

                            respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, sender, "Gestión Pólizas");


                        }
                        catch (Exception ex)
                        {
                            respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesUsuarios.ErrorGuardarCambios };
                            respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, sender, "Gestión Pólizas") + ": " + ex.ToString() + ex.InnerException;

                        }
                    }
            }

        }

        //paquete 1: no tienen registro inicial contrato poliza
        public async Task EnviarCorreoSupervisor4dPolizaNoAprobada(string dominioFront, string mailServer, int mailPort, bool enableSSL, string password, string sender)
        {
            Respuesta respuesta = new Respuesta();
            string fechaFirmaContrato = "";
            DateTime? FechaFirmaContrato_dt;
            VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza;

            List<Contrato> lstContrato;

            lstContrato = _context.Contrato.Where(r => !(bool)r.Eliminado
            && r.Contratacion.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.Registrados.ToString()
            ).ToList();

            int cntPolizas = 0;
            DateTime RangoFechaConDiasHabiles;

            foreach (Contrato contrato in lstContrato)
            {
                cntPolizas = _context.ContratoPoliza.Where(r => r.ContratoId == contrato.ContratoId).Count();

                if (cntPolizas == 0)
                {
                    RangoFechaConDiasHabiles = await _commonService.CalculardiasLaboralesTranscurridos(4, DateTime.Now);

                    FechaFirmaContrato_dt = contrato != null ? contrato.FechaFirmaContrato : null;

                    if (FechaFirmaContrato_dt != null)
                        if (FechaFirmaContrato_dt <= RangoFechaConDiasHabiles)
                        {
                            ContratoPoliza contratoPoliza = new ContratoPoliza();

                            try
                            {
                                int idAccionEditarContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente, (int)EnumeratorTipoDominio.Acciones);

                                objVistaContratoGarantiaPoliza = new VistaContratoGarantiaPoliza();

                                List<VistaContratoGarantiaPoliza> ListVista = new List<VistaContratoGarantiaPoliza>();
                                ListVista = await ListVistaContratoGarantiaPoliza(0);

                                int pIdTemplate = (int)enumeratorTemplate.MsjFiduciariaJuridicaGestionPoliza;

                                NotificacionMensajeGestionPoliza msjNotificacion;
                                msjNotificacion = new NotificacionMensajeGestionPoliza();

                                getDataNotifMsjAseguradora(ref msjNotificacion, contratoPoliza, ref fechaFirmaContrato, ref objVistaContratoGarantiaPoliza, ListVista);

                                bool blEnvioCorreo = false;

                                Template TemplateRecoveryPassword = await _commonService.GetTemplateById(pIdTemplate);

                                string template = TemplateRecoveryPassword.Contenido;

                                fechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();

                                //datos basicos generales, aplican para los 4 mensajes
                                template = template.Replace("_Tipo_Contrato_", objVistaContratoGarantiaPoliza.TipoContrato);
                                template = template.Replace("_Numero_Contrato_", objVistaContratoGarantiaPoliza.NumeroContrato);
                                template = template.Replace("_Fecha_Firma_Contrato_", fechaFirmaContrato); //Formato (dd/MM/aaaa)
                                template = template.Replace("_Nombre_Contratista_", objVistaContratoGarantiaPoliza.NombreContratista);
                                template = template.Replace("_Valor_Contrato_", string.Format("${0:#,0}", objVistaContratoGarantiaPoliza.ValorContrato.ToString()));  //fomato miles .
                                template = template.Replace("_Plazo_", objVistaContratoGarantiaPoliza.PlazoContrato);

                                if (msjNotificacion != null)
                                {
                                    template = template.Replace("_Nombre_Aseguradora_", msjNotificacion.NombreAseguradora);
                                    template = template.Replace("_Numero_Poliza_", msjNotificacion.NumeroPoliza);

                                    //template = template.Replace("_Fecha_Revision_", msjNotificacion.FechaRevision);
                                    //template = template.Replace("_Estado_Revision_", msjNotificacion.EstadoRevision);
                                    //template = template.Replace("_Observaciones_", msjNotificacion.Observaciones);

                                    //template = template.Replace("_Fecha_Aprobacion_Poliza", msjNotificacion.FechaAprobacion);

                                    if (!string.IsNullOrEmpty(msjNotificacion.NumeroDRP))
                                        template = template.Replace("_NumeroDRP_", msjNotificacion.NumeroDRP);

                                }
                                //1   Administrador  - 
                                //2   Técnica
                                //3   Financiera - //4   Jurídica
                                //5   Administrativa - //6   Miembros Comite
                                //7   Secretario comité - //8   Supervisor

                                List<UsuarioPerfil> lstUsuariosPerfil = _context.UsuarioPerfil
                                    .Where(r => r.Activo == true && r.PerfilId == (int)EnumeratorPerfil.Supervisor)
                                    .Include(u => u.Usuario)
                                    .ToList();

                                lstUsuariosPerfil.ForEach(user =>
                                {
                                    blEnvioCorreo = Helpers.Helpers.EnviarCorreo(user.Usuario.Email, "Gestión Poliza", template, sender, password, mailServer, mailPort);
                                });



                                if (blEnvioCorreo)
                                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.CorreoEnviado };

                                else
                                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.ErrorEnviarCorreo };

                                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, sender, "Gestión Pólizas");

                                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, sender, "Gestión Pólizas");

                            }
                            catch (Exception ex)
                            {

                                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesUsuarios.ErrorGuardarCambios };
                                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, sender, "Gestión Pólizas") + ": " + ex.ToString() + ex.InnerException;

                            }
                        }
                }
            }
        }

        private void getDataNotifMsjAseguradora(ref NotificacionMensajeGestionPoliza msjNotificacion, ContratoPoliza contratoPoliza, ref string fechaFirmaContrato, ref VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza, List<VistaContratoGarantiaPoliza> ListVista)
        {
            msjNotificacion.NombreAseguradora = contratoPoliza.NombreAseguradora;
            msjNotificacion.NumeroPoliza = contratoPoliza.NumeroPoliza;
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
            contratoPoliza.ResponsableAprobacion = Helpers.Helpers.CleanStringInput(contratoPoliza.ResponsableAprobacion);
            contratoPoliza.NumeroCertificado = Helpers.Helpers.CleanStringInput(contratoPoliza.NumeroCertificado);
            contratoPoliza.NombreAseguradora = Helpers.Helpers.CleanStringInput(contratoPoliza.NombreAseguradora);
        }

        public async Task<bool> ValidarRegistroCompletoSeguros(ContratoPoliza contratoPoliza)
        {
            PolizaGarantia polizaGarantiaSeguro;
            List<PolizaGarantia> lstPolizaGarantia;

            lstPolizaGarantia = _context.PolizaGarantia.Where(r => r.ContratoPolizaId == contratoPoliza.ContratoPolizaId).ToList();

            List<Dominio> TipoGarantiaSeguroContratoPoliza = null;

            if (contratoPoliza.TipoSolicitudCodigo != null)
            {
                TipoGarantiaSeguroContratoPoliza = await _commonService.GetListDominioByIdTipoDominio(Convert.ToInt32(EnumeratorTipoDominio.Tipo_Garantia_Poliza));

            }
            int cntSi = 0;


            //jflorez, las polizas pueden estar en no, lo importante es que tenga almenos una
            if (lstPolizaGarantia.Count() > 0)
            {
                return true;
            }

            return false;
        }

        public static bool ValidarRegistroCompletoContratoPoliza(ContratoPoliza contratoPoliza, bool EsContratoDevuelto)
        {
            //si es devuelta no validar: FechaAprobacion, ResponsableAprobacion
            //jflorez, cambio la condición porque no entiendo que tiene que ver el contrato devuelto con el contrato poliza devuelto
            //if (!EsContratoDevuelto)
            if (contratoPoliza.EstadoPolizaCodigo == ConstanCodigoEstadoRevision.aprobada)
            {
                if (string.IsNullOrEmpty(contratoPoliza.FechaAprobacion.ToString()))
                    return false;
                if (contratoPoliza.ResponsableAprobacion != null)
                    if (string.IsNullOrEmpty(contratoPoliza.ResponsableAprobacion.ToString()))
                        return false;
            }

            if (
                   string.IsNullOrEmpty(contratoPoliza.NombreAseguradora.ToString())
                || string.IsNullOrEmpty(contratoPoliza.NumeroPoliza.ToString())
                || string.IsNullOrEmpty(contratoPoliza.NumeroCertificado.ToString())
                || string.IsNullOrEmpty(contratoPoliza.FechaExpedicion.ToString())
                || string.IsNullOrEmpty(contratoPoliza.Vigencia.ToString())
                || string.IsNullOrEmpty(contratoPoliza.VigenciaAmparo.ToString())
                || string.IsNullOrEmpty(contratoPoliza.ValorAmparo.ToString())
                || contratoPoliza.CumpleDatosAsegurado.HasValue
                || contratoPoliza.CumpleDatosBeneficiario.HasValue
                || contratoPoliza.CumpleDatosTomador.HasValue
                || contratoPoliza.IncluyeReciboPago.HasValue
                || contratoPoliza.IncluyeCondicionesGenerales.HasValue
                || string.IsNullOrEmpty(contratoPoliza.EstadoPolizaCodigo.ToString())
                )
                return false;
            //JMC 09/02/2021
            //Si no esta la lista de chequeo true no pasa a completo 
            if (
                   contratoPoliza.CumpleDatosAsegurado != true
                 || contratoPoliza.CumpleDatosBeneficiario != true
                 || contratoPoliza.CumpleDatosTomador != true
                 || contratoPoliza.IncluyeReciboPago != true
                 || contratoPoliza.IncluyeCondicionesGenerales != true
                )
                return false;



            return true;
        }

        public async Task<bool> ConsultarRegistroCompletoCumple(int ContratoPolizaId)
        {
            bool respuesta = false;
            ContratoPoliza contratoPoliza = null;

            //return respuesta;

            contratoPoliza = await _context.ContratoPoliza.Where(r => r.ContratoPolizaId == ContratoPolizaId).FirstOrDefaultAsync();

            if (contratoPoliza != null)
            {
                if (string.IsNullOrEmpty(contratoPoliza.CumpleDatosAsegurado.ToString()))
                {
                    return respuesta;
                }
                else if (string.IsNullOrEmpty(contratoPoliza.CumpleDatosBeneficiario.ToString()))
                {
                    return respuesta;
                }
                else if (string.IsNullOrEmpty(contratoPoliza.CumpleDatosTomador.ToString()))
                {
                    return respuesta;
                }
                else
                {
                    respuesta = true;
                    return true;
                }

            }
            else
            {
                respuesta = false;
                return respuesta;
            }

        }

        public async Task<Respuesta> AprobarContratoByIdContrato(int pIdContrato, AppSettingsService settings, string pUsuario)
        {
            int idAccionEditarContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza;
                objVistaContratoGarantiaPoliza = new VistaContratoGarantiaPoliza();

                List<VistaContratoGarantiaPoliza> ListVista = new List<VistaContratoGarantiaPoliza>();
                ListVista = await ListVistaContratoGarantiaPoliza(pIdContrato);

                objVistaContratoGarantiaPoliza = ListVista.Where(x => x.IdContrato == pIdContrato).FirstOrDefault();

                NotificacionMensajeGestionPoliza msjNotificacion;
                msjNotificacion = new NotificacionMensajeGestionPoliza();

                string fechaFirmaContrato;

                ContratoPoliza contratoPoliza = new ContratoPoliza();
                Contrato contrato = new Contrato();
                contrato = _context.Contrato.Where(r => r.ContratoId == pIdContrato).FirstOrDefault();
                if (contrato != null)
                {
                    contrato.EstaDevuelto = false;
                    _context.Update(contrato);
                    _context.SaveChanges();
                }

                contratoPoliza = _context.ContratoPoliza.Where(r => r.ContratoId == contrato.ContratoId)
                   .OrderByDescending(r => r.ContratoPolizaId).FirstOrDefault();


                contratoPoliza.UsuarioModificacion = pUsuario;
                contratoPoliza.TipoSolicitudCodigo = ((int)EnumeratorEstadoPoliza.Con_aprobacion_de_polizas).ToString();
                _context.ContratoPoliza.Update(contratoPoliza);

                fechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();

                getDataNotifMsjAseguradora(ref msjNotificacion, contratoPoliza, ref fechaFirmaContrato, ref objVistaContratoGarantiaPoliza, ListVista);


                Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjSupervisorJuridicaGestionPoliza);

                string template = TemplateRecoveryPassword.Contenido;

                contrato = _context.Contrato.Where(r => r.ContratoId == objVistaContratoGarantiaPoliza.IdContrato).FirstOrDefault();

                fechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();

                //datos basicos generales, aplican para los 4 mensajes
                template = template.Replace("_Tipo_Contrato_", objVistaContratoGarantiaPoliza.TipoContrato);
                template = template.Replace("_Numero_Contrato_", objVistaContratoGarantiaPoliza.NumeroContrato);
                template = template.Replace("_Fecha_Firma_Contrato_", fechaFirmaContrato); //Formato (dd/MM/aaaa)
                template = template.Replace("_Nombre_Contratista_", objVistaContratoGarantiaPoliza.NombreContratista);
                template = template.Replace("_Valor_Contrato_", string.Format("${0:#,0}", objVistaContratoGarantiaPoliza.ValorContrato.ToString()));  //fomato miles .
                template = template.Replace("_Plazo_", objVistaContratoGarantiaPoliza.PlazoContrato);
                template = template.Replace("_LinkF_", settings.DominioFront);

                if (msjNotificacion != null)
                {
                    template = template.Replace("_Nombre_Aseguradora_", msjNotificacion.NombreAseguradora);
                    template = template.Replace("_Numero_Poliza_", msjNotificacion.NumeroPoliza);
                    template = template.Replace("_Fecha_Revision_", msjNotificacion.FechaRevision);
                    template = template.Replace("_Estado_Revision_", _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Revision_Poliza && x.Codigo == msjNotificacion.EstadoRevision).Select(x => x.Nombre).FirstOrDefault());
                    template = template.Replace("_Observaciones_", msjNotificacion.Observaciones);

                    template = template.Replace("_Fecha_Aprobacion_Poliza", msjNotificacion.FechaAprobacion);

                    if (!string.IsNullOrEmpty(msjNotificacion.NumeroDRP))
                        template = template.Replace("_NumeroDRP_", msjNotificacion.NumeroDRP);

                }

                //Enviar Correo  INTERVENTOR - JURIDICA - SUPERVISOR
                List<Usuario> lstUsuarios = _context.UsuarioPerfil
                                                                    .Where(r => r.Activo == true &&
                                                                        (r.PerfilId == (int)EnumeratorPerfil.Interventor
                                                                        || r.PerfilId == (int)EnumeratorPerfil.Juridica
                                                                        || r.PerfilId == (int)EnumeratorPerfil.Supervisor))
                                                                     .Include(r => r.Usuario).Select(r => r.Usuario).ToList();
                bool blEnvioCorreo = true;
                foreach (var usuario in lstUsuarios)
                {
                    if (false == Helpers.Helpers.EnviarCorreo(usuario.Email, "Gestión Poliza", template, settings.Sender, settings.Password, settings.MailServer, settings.MailPort))
                        blEnvioCorreo = false;
                }

                if (blEnvioCorreo)
                    return new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.CorreoEnviado };

                else
                    return new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.ErrorEnviarCorreo };

            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesContratoPoliza.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias, ConstantMessagesContratoPoliza.ErrorInterno, idAccionEditarContratoPoliza, pUsuario, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        private List<Usuario> getCorreos(int perfilId)
        {
            return _context.UsuarioPerfil
                                        .Where(r => r.Activo == true && r.PerfilId == perfilId)
                                        .Include(r => r.Usuario)
                                        .Select(r => r.Usuario)
                                                              .ToList();
        }

        public async Task<Respuesta> EnviarCorreoGestionPoliza(string lstMails, string pMailServer, int pMailPort, string pPassword,
           string pSentender, VistaContratoGarantiaPoliza objVistaContratoGarantiaPoliza, string fechaFirmaContrato, int pIdTemplate,
           string fronturl, NotificacionMensajeGestionPoliza objNotificacionAseguradora = null)
        {
            bool blEnvioCorreo = false;
            Respuesta respuesta = new Respuesta();
            try
            {
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById(pIdTemplate);

                string template = TemplateRecoveryPassword.Contenido;

                Contrato contrato;
                contrato = _context.Contrato.Where(r => r.ContratoId == objVistaContratoGarantiaPoliza.IdContrato).FirstOrDefault();

                fechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();

                //datos basicos generales, aplican para los 4 mensajes
                template = template.Replace("_Tipo_Contrato_", objVistaContratoGarantiaPoliza.TipoContrato);
                template = template.Replace("_Numero_Contrato_", objVistaContratoGarantiaPoliza.NumeroContrato);
                template = template.Replace("_Fecha_Firma_Contrato_", fechaFirmaContrato); //Formato (dd/MM/aaaa)
                template = template.Replace("_Nombre_Contratista_", objVistaContratoGarantiaPoliza.NombreContratista);
                template = template.Replace("_Valor_Contrato_", string.Format("${0:#,0}", objVistaContratoGarantiaPoliza.ValorContrato.ToString()));  //fomato miles .
                template = template.Replace("_Plazo_", objVistaContratoGarantiaPoliza.PlazoContrato);
                template = template.Replace("_LinkF_", fronturl);

                if (objNotificacionAseguradora != null)
                {
                    template = template.Replace("_Nombre_Aseguradora_", objNotificacionAseguradora.NombreAseguradora);
                    template = template.Replace("_Numero_Poliza_", objNotificacionAseguradora.NumeroPoliza);
                    template = template.Replace("_Fecha_Revision_", objNotificacionAseguradora.FechaRevision);
                    template = template.Replace("_Estado_Revision_", _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Revision_Poliza && x.Codigo == objNotificacionAseguradora.EstadoRevision).Select(x => x.Nombre).FirstOrDefault());
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

        public async Task<List<VGestionarGarantiasPolizas>> ListGrillaContratoGarantiaPolizaOptz()
        {
            return await _context.VGestionarGarantiasPolizas.ToListAsync();
        }

        public async Task<List<GrillaContratoGarantiaPoliza>> ListGrillaContratoGarantiaPoliza()
        {
            //await AprobarContratoByIdContrato(1);

            List<GrillaContratoGarantiaPoliza> ListContratoGrilla = new List<GrillaContratoGarantiaPoliza>();
            //Fecha de firma del contrato ??? FechaFirmaContrato , [Contrato] -(dd / mm / aaaa)


            //Tipo de solicitud ??? ContratoPoliza - TipoSolicitudCodigo        


            //List <Contrato> ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado).Include(r => r.FechaFirmaContrato).Include(r => r.NumeroContrato).Include(r => r.Estado).Distinct().ToListAsync();
            List<Contrato> ListContratos = await _context.Contrato.Where(r => r.Contratacion.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.Registrados.ToString() && !(bool)r.Eliminado).ToListAsync();

            foreach (var contrato in ListContratos)
            {
                try
                {
                    Contratacion contratacion = null;
                    contratacion = _context.Contratacion.Where(r => r.ContratacionId == contrato.ContratacionId).FirstOrDefault();

                    string strNumeroSolicitudContratacion = "";
                    string strTipoConContratacion = "";

                    Dominio TipoSolicitudCodigoContratacion = null;
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

                    ContratoPoliza contratoPoliza = null;
                    contratoPoliza = await _commonService.GetLastContratoPolizaByContratoId(contrato.ContratoId);

                    int ContratoPolizaIdValor = 0;

                    //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    string strTipoSolicitudCodigoContratoPoliza = (Convert.ToInt32(ConstanCodigoTipoSolicitud.Contratacion)).ToString();
                    string strTipoSolicitudNombreContratoPoliza = ConstanStringTipoSolicitudContratacion.contratacion; //contratacion o modif contractual - 

                    string strEstadoSolicitudCodigoContratoPoliza = ((int)EnumeratorEstadoPoliza.Sin_radicacion_polizas).ToString();
                    string strEstadoSolicitudNombreContratoPoliza = "Sin radicación de pólizas";

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio TipoSolicitudCodigoContratoPoliza;
                    Dominio EstadoSolicitudCodigoContratoPoliza;

                    string strRegistroCompletoPolizaNombre = "Incompleto";
                    bool bRegistroCompletoPoliza = false;

                    if (contratoPoliza != null)
                    {
                        if (contrato.EstaDevuelto != null)
                        {
                            if ((bool)contrato.EstaDevuelto == false)
                            {
                                if (contratoPoliza.RegistroCompleto != null)
                                {
                                    strRegistroCompletoPolizaNombre = (bool)contratoPoliza.RegistroCompleto ? "Completo" : "Incompleto";
                                    bRegistroCompletoPoliza = (bool)contratoPoliza.RegistroCompleto;
                                }
                                else
                                {
                                    strRegistroCompletoPolizaNombre = "Incompleto";
                                    bRegistroCompletoPoliza = false;
                                }
                            }
                        }
                        else /*if (contrato.EstaDevuelto == null)*/
                        {
                            if (contratoPoliza.RegistroCompleto != null)
                            {
                                strRegistroCompletoPolizaNombre = (bool)contratoPoliza.RegistroCompleto ? "Completo" : "Incompleto";
                                bRegistroCompletoPoliza = (bool)contratoPoliza.RegistroCompleto;
                            }
                            else
                            {
                                strRegistroCompletoPolizaNombre = "Incompleto";
                                bRegistroCompletoPoliza = false;

                            }
                        }


                        ContratoPolizaIdValor = contratoPoliza.ContratoPolizaId;
                        if (contratoPoliza.TipoSolicitudCodigo != null)
                        {
                            //TipoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_Modificacion_Contrato_Poliza);
                            TipoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo.Trim(), (int)EnumeratorTipoDominio.Tipo_Solicitud);

                            if (TipoSolicitudCodigoContratoPoliza != null)
                            {
                                strTipoSolicitudCodigoContratoPoliza = TipoSolicitudCodigoContratoPoliza.Codigo;
                                strTipoSolicitudNombreContratoPoliza = TipoSolicitudCodigoContratoPoliza.Nombre;
                            }
                        }

                        EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.EstadoPolizaCodigo.Trim(), (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                        if (EstadoSolicitudCodigoContratoPoliza != null)
                        {
                            strEstadoSolicitudCodigoContratoPoliza = EstadoSolicitudCodigoContratoPoliza.Codigo;
                            strEstadoSolicitudNombreContratoPoliza = EstadoSolicitudCodigoContratoPoliza.Nombre;

                        }

                    }
                    bool bRegistroCompleto = false;
                    string strRegistroCompleto = "Incompleto";

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
                        FechaCreacionContrato = contrato.FechaCreacion,
                        //FechaFirma = contrato.FechaFirmaContrato.ToString(),
                        NumeroContrato = contrato.NumeroContrato,
                        //TipoSolicitud= contratoPoliza.TipoSolicitudCodigo
                        //EstadoRegistro { get; set; }
                        NumeroSolicitudContratacion = strNumeroSolicitudContratacion,
                        //Departamento = departamento.Descripcion,
                        //Municipio = municipio.Descripcion,
                        //InstitucionEducativa = _context.InstitucionEducativaSede.Find(contrato.InstitucionEducativaId).Nombre,
                        //Sede = _context.InstitucionEducativaSede.Find(contrato.SedeId).Nombre,
                        //jflorez 20201124 no modelado, dejo el dato de contratos (puede ser contratacion o modificaion contractual)
                        TipoSolicitud = ConstanStringTipoSolicitudContratacion.contratacion,
                        TipoSolicitudCodigo = strTipoSolicitudCodigoContratoPoliza,

                        TipoSolicitudCodigoContratacion = strTipoSolicitudCodigoContratacion,


                        TipoSolicitudContratacion = strTipoSolicitudContratacion,

                        EstadoPoliza = strEstadoSolicitudNombreContratoPoliza
                        ,
                        EstadoPolizaCodigo = strEstadoSolicitudCodigoContratoPoliza,
                        RegistroCompleto = contrato.RegistroCompleto,
                        RegistroCompletoNombre = strRegistroCompleto,

                        RegistroCompletoPoliza = bRegistroCompletoPoliza,
                        RegistroCompletoPolizaNombre = strRegistroCompletoPolizaNombre,

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
                        ContratoPolizaId = 0,
                        FechaFirma = e.ToString(),
                        NumeroContrato = e.InnerException.ToString(),
                        //TipoSolicitud= contratoPoliza.TipoSolicitudCodigo
                        //EstadoRegistro { get; set; }

                        //Departamento = departamento.Descripcion,
                        //Municipio = municipio.Descripcion,
                        //InstitucionEducativa = _context.InstitucionEducativaSede.Find(contrato.InstitucionEducativaId).Nombre,
                        //Sede = _context.InstitucionEducativaSede.Find(contrato.SedeId).Nombre,
                        TipoSolicitud = "ERROR",
                        FechaCreacionContrato = DateTime.Now,
                        RegistroCompleto = false,
                        RegistroCompletoNombre = "ERROR",

                    };
                    ListContratoGrilla.Add(proyectoGrilla);
                }
            }
            return ListContratoGrilla.OrderByDescending(r => r.FechaFirma).ToList();

        }

        public async Task<List<VistaContratoGarantiaPoliza>> ListVistaContratoGarantiaPoliza(int pContratoId)
        {
            List<VistaContratoGarantiaPoliza> ListContratoGrilla = new List<VistaContratoGarantiaPoliza>();

            List<Contrato> ListContratos = new List<Contrato>();

            if (pContratoId == 0)
            {
                ListContratos = await _context.Contrato.Where(r => (bool)r.Eliminado == false).Distinct()
            .ToListAsync();

            }
            else
            {
                ListContratos = await _context.Contrato.Where(r => (bool)r.Eliminado == false && r.ContratoId == pContratoId).Distinct().ToListAsync();
            }
            foreach (var contrato in ListContratos)
            {
                try
                {
                    ContratoPoliza contratoPoliza = null;
                    contratoPoliza = await _commonService.GetLastContratoPolizaByContratoId(contrato.ContratoId);

                    Dominio TipoSolicitudCodigoContratoPoliza = null;
                    string strTipoSolicitudCodigoContratoPoliza = "Sin radicación de pólizas";
                    string strFechaFirmaContrato = string.Empty;
                    int contratacionIdValor = 0;

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

                    Dominio TipoContratoCodigoContrato = null;

                    Contratista contratista = null;
                    decimal vlrContratoComponenteUso = 0;

                    if (contratacion != null)
                    {
                        if (contratacion.ContratistaId != null)
                            contratista = await _commonService.GetContratistaByContratistaId((Int32)contratacion.ContratistaId);

                        if (contratista != null)
                        {
                            strContratistaNombre = contratista.Nombre;
                            //Nit  
                            strContratistaNumeroIdentificacion = contratista.NumeroIdentificacion.ToString();

                            TipoDocumentoCodigoContratista = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratista.TipoIdentificacionCodigo, (int)EnumeratorTipoDominio.Tipo_Documento);
                            //TipoDocumentoCodigoContratista = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratista.TipoIdentificacionCodigo, (int)EnumeratorTipoDominio.Tipo_Solicitud);

                            if (TipoDocumentoCodigoContratista != null)
                                strTipoDocumentoContratista = TipoDocumentoCodigoContratista.Nombre;
                        }
                        //jflorez -20201124 ajusto tipo dominio
                        //TipoContratoCodigoContrato = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratacion.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_Solicitud);
                        TipoContratoCodigoContrato = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratacion.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Opcion_por_contratar);

                        contratacionIdValor = contratacion.ContratacionId;

                        vlrContratoComponenteUso = getSumVlrContratoComponente(contratacion.ContratacionId);

                    }

                    DisponibilidadPresupuestal disponibilidadPresupuestal = null;

                    if (contratacion != null)
                    {
                        disponibilidadPresupuestal = _context.DisponibilidadPresupuestal.Where(r => r.ContratacionId == contratacion.ContratacionId).FirstOrDefault();
                    }
                    string contratoObjeto = "";

                    //contratacion = _context.Contratacion.Where(r => r.ContratacionId == contrato.ContratacionId).FirstOrDefault();

                    //TipoContrato = contrato.TipoContratoCodigo   ??? Obra  ????

                    //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    Int32 plazoDias, plazoMeses;
                    //25meses / 04 días

                    //if (!string.IsNullOrEmpty(contrato.PlazoFase2ConstruccionDias.ToString()))
                    //    plazoDias = Convert.ToInt32(contrato.PlazoFase1PreDias);

                    //else
                    //    plazoDias = Convert.ToInt32(contrato.PlazoFase2ConstruccionDias);

                    //if (!string.IsNullOrEmpty(contrato.PlazoFase2ConstruccionMeses.ToString()))
                    //    plazoMeses = Convert.ToInt32(contrato.PlazoFase1PreMeses);
                    //else
                    //    plazoMeses = Convert.ToInt32(contrato.PlazoFase2ConstruccionMeses);

                    string PlazoContratoFormat = "";/* = plazoMeses.ToString("00") + " meses / " + plazoDias.ToString("00") + " dias "*/;
                    plazoMeses = 0;
                    plazoDias = 0;
                    if (disponibilidadPresupuestal != null)
                    {
                        contratoObjeto = disponibilidadPresupuestal.Objeto;

                        if (!string.IsNullOrEmpty(disponibilidadPresupuestal.PlazoDias.ToString()))
                            plazoDias = Convert.ToInt32(disponibilidadPresupuestal.PlazoDias);

                        //else
                        //    plazoDias = Convert.ToInt32(contrato.PlazoFase2ConstruccionDias);

                        if (!string.IsNullOrEmpty(disponibilidadPresupuestal.PlazoMeses.ToString()))
                            plazoMeses = Convert.ToInt32(disponibilidadPresupuestal.PlazoMeses);
                        //else
                        //    plazoMeses = Convert.ToInt32(contrato.PlazoFase2ConstruccionMeses);

                        PlazoContratoFormat = plazoMeses.ToString("00") + " meses / " + plazoDias.ToString("00") + " dias ";

                    }
                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    //Dominio TipoContratoCodigoContrato = await _commonService.GetDominioByNombreDominioAndTipoDominio(contrato.TipoContratoCodigo, (int)EnumeratorTipoDominio.Tipo_Contrato);

                    string strTipoContratoCodigoContratoNombre = string.Empty;

                    if (TipoContratoCodigoContrato != null)
                        strTipoContratoCodigoContratoNombre = TipoContratoCodigoContrato.Nombre;

                    //string contratoObjeto = "";
                    //if (contrato.Objeto != null)
                    //    contratoObjeto = contrato.Objeto;
                    //Dominio TipoModificacionCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoModificacionCodigo, (int)EnumeratorTipoDominio.Tipo_Modificacion_Contrato_Poliza);
                    VistaContratoGarantiaPoliza proyectoGrilla = new VistaContratoGarantiaPoliza
                    {
                        ContratacionId = contratacionIdValor,
                        IdContrato = contrato.ContratoId,
                        TipoContrato = strTipoContratoCodigoContratoNombre,
                        NumeroContrato = contrato.NumeroContrato,
                        ObjetoContrato = contratoObjeto,
                        NombreContratista = strContratistaNombre,
                        TipoDocumento = strTipoDocumentoContratista,

                        //Nit  
                        NumeroIdentificacion = strContratistaNumeroIdentificacion,

                        //ValorContrato = contrato.Valor.ToString(),
                        ValorContrato = vlrContratoComponenteUso,


                        PlazoContrato = PlazoContratoFormat,

                        //EstadoRegistro 
                        //public bool? RegistroCompleto { get; set; }                         

                        DescripcionModificacion = "resumen", // resumen   TEMPORAL REV

                        //TipoModificacion = TipoModificacionCodigoContratoPoliza.Nombre
                        TipoModificacion = "Tipo modificacion",
                        //jflorez 20201124 ajusto el tipo de solicitud, es contratación o modificacion contractual, en este momento no existe modelo de modificaciones contractuales por lo tanto envio el string plano
                        TipoSolicitud = ConstanStringTipoSolicitudContratacion.contratacion,

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
                        ValorContrato = 0,
                        PlazoContrato = "ERROR",

                        //EstadoRegistro 
                        //public bool? RegistroCompleto { get; set; } 
                        //TipoSolicitud = contratoPoliza.EstadoPolizaCodigo

                        DescripcionModificacion = "ERROR",
                        TipoModificacion = "ERROR",
                        TipoDocumento = "ERROR"

                    };
                    ListContratoGrilla.Add(proyectoGrilla);
                }
            }
            return ListContratoGrilla.OrderByDescending(r => r.TipoSolicitud).ToList();

        }

        //private async Task<decimal> getSumVlrContratoComponente(int contratacionId)
        /*jflorez, ajusto la suma*/
        private decimal getSumVlrContratoComponente(int contratacionId)
        {
            /* ContratacionProyecto contratacionProyecto = null;
             contratacionProyecto = _context.ContratacionProyecto.Where(r => r.ContratacionId == contratacionId).FirstOrDefault();

             ContratacionProyectoAportante contratacionProyectoAportante = null;
             if (contratacionProyecto != null)
                 contratacionProyectoAportante = _context.ContratacionProyectoAportante.Where(r => r.ContratacionProyectoId == contratacionProyecto.ContratacionProyectoId).FirstOrDefault();

             ComponenteAportante componenteAportante = null;
             componenteAportante = _context.ComponenteAportante.Where(r => r.ContratacionProyectoAportanteId == contratacionProyecto.ContratacionProyectoId).FirstOrDefault();

             ComponenteUso componenteUso = null;

             decimal SumVlrContratoComponente = 0;

             if(componenteAportante != null) 
                 SumVlrContratoComponente =  _context.ComponenteUso.Where(x => x.ComponenteAportanteId == componenteAportante.ComponenteAportanteId).Sum(x => x.ValorUso);

             return SumVlrContratoComponente;
             */

            var sum = _context.ComponenteUso.Where(x => x.ComponenteAportante.ContratacionProyectoAportante.ContratacionProyecto.ContratacionId == contratacionId).Sum(x => x.ValorUso);
            return sum;

        }
    }
}
