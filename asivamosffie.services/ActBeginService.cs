using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.api;
using asivamosffie.services.Helpers.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace asivamosffie.services
{
   public  class ActBeginService : IActBeginService
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        private readonly IOptions<AppSettingsService> _settings;

        private readonly IDocumentService _documentService;
        public readonly IConverter _converter;

        public ActBeginService(devAsiVamosFFIEContext context, ICommonService commonService, IOptions<AppSettingsService> settings, IDocumentService documentService, IConverter converter)
        {
            _commonService = commonService;
            _context = context;
            _settings = settings;
            _documentService = documentService;
            _converter = converter;
        }

        public async Task<Respuesta> GuardarTieneObservacionesActaInicio(int pContratoId, string pObservacionesActa, string pUsuarioModificacion, bool pEsSupervisor, bool pEsActa)
        {
            Respuesta _response = new Respuesta();

            //            ConObervacionesActa - Contrato
            Contrato contrato;
            //contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId && r.Estado == true).FirstOrDefault();
            contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId ).FirstOrDefault();

            int idAccionCrearActaInicio = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Editar_Tiene_Observaciones_Acta_Inicio_Fase2, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                contrato.FechaModificacion = DateTime.Now;
                contrato.UsuarioModificacion = pUsuarioModificacion;
                contrato.ConObervacionesActa = true;
                //contrato.Observaciones = pObservacionesActa;

                ConstruccionObservacion construccionObservacion = new ConstruccionObservacion();

                //                6   Acta de inicio interventor
                //7   Acta de inicio supervisor
                construccionObservacion.Observaciones = Helpers.Helpers.CleanStringInput(pObservacionesActa);

                if (pEsSupervisor)
                    //if (construccionObservacion.EsSupervision)
                    construccionObservacion.TipoObservacionConstruccion = "7";
                else
                    construccionObservacion.TipoObservacionConstruccion = "6";

                construccionObservacion.EsSupervision = pEsSupervisor;
                construccionObservacion.EsActa = pEsActa;

                //construccionObservacion.UsuarioCreacion = construccionObservacion.UsuarioCreacion;
                //construccionObservacion.UsuarioModificacion = construccionObservacion.UsuarioModificacion;
                //construccionObservacion.EsActa = construccionObservacion.UsuarioModificacion;
                construccionObservacion.UsuarioCreacion = pUsuarioModificacion;
                construccionObservacion.FechaCreacion = DateTime.Now;

                ContratoConstruccion contratoConstruccion = null;
                contratoConstruccion = _context.ContratoConstruccion.Where(r => r.ContratoId == contrato.ContratoId).FirstOrDefault();
                construccionObservacion.ContratoConstruccionId = contratoConstruccion.ContratoConstruccionId;
                //ContratoObservacion contratoObservacion=new ContratoObservacion();
                //contratoObservacion.Observaciones = pObservacionesActa;
                //contratoObservacion.UsuarioCreacion = pUsuarioModificacion;
                //contratoObservacion.FechaCreacion = DateTime.Now;
                //contratoObservacion.EsActa = true;
                //contratoObservacion.EsActaFase2 = true;
                //contratoObservacion.ContratoId = pContratoId;

                //_context.ContratoObservacion.Add(contratoObservacion);
                _context.ConstruccionObservacion.Add(construccionObservacion);
                //await _context.SaveChangesAsync();

                //      CAMBIAR ESTADO “Sin acta generada” a “Con acta generada”.
                //DOM 60  1   Sin acta generada
                //DOM 60  3   Con acta generada
                //contrato.EstadoActa = "3";

                _context.Contrato.Update(contrato);
                await _context.SaveChangesAsync();
                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesActaInicio.OperacionExitosa,
                    Message =
                    await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2,
                    ConstantMessagesActaInicio.OperacionExitosa, idAccionCrearActaInicio
                    , contrato.UsuarioModificacion, " GUARDAR OBSERVACION CONTRATO ACTA"
                    )
                };

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesActaInicio.ErrorInterno,Message= ex.InnerException.ToString().Substring(0, 500) };
            }
        }
        //_context.Add(contratoPoliza);
                
        public async Task<Respuesta> GuardarPlazoEjecucionFase2Construccion(int pContratoId, int pPlazoFase2PreMeses, int pPlazoFase2PreDias, string pObservacionesConsideracionesEspeciales, string pUsuarioModificacion, DateTime pFechaActaInicioFase1 , DateTime pFechaTerminacionFase2, bool pEsSupervisor, bool pEsActa)
        {
            Respuesta _response = new Respuesta();

            //            ConObervacionesActa - Contrato
            Contrato contrato;
            //contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId && r.Estado == true).FirstOrDefault();
            contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

            ContratoObservacion contratoObservacion= new ContratoObservacion();

            int idAccionCrearActaInicio = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Editar_Plazo_Ejecucion_Fase_2, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                contrato.PlazoFase2ConstruccionDias = pPlazoFase2PreDias;
                contrato.PlazoFase2ConstruccionMeses = pPlazoFase2PreMeses;
                //contrato.EstadoActaFase2 = ((int)EnumeratorEstadoActa.Con_acta_preliminar_generada).ToString();                                                  
                     
                if (contrato.TipoContratoCodigo == ((int)ConstanCodigoTipoContratacion.Obra).ToString())
                    contrato.EstadoActaFase2 = "14"; //14  Con acta preliminar generada
                else if (contrato.TipoContratoCodigo == ((int)ConstanCodigoTipoContratacion.Interventoria).ToString())
                    contrato.EstadoActaFase2 = "2";       //2   Con acta preliminar generada

                contrato.FechaActaInicioFase1 = pFechaActaInicioFase1;
                 contrato.FechaTerminacionFase2 = pFechaTerminacionFase2;
                //contrato.ContratoObservacion = pObservacionesConsideracionesEspeciales;
                //contrato.Observaciones = Helpers.Helpers.CleanStringInput( pObservacionesConsideracionesEspeciales);

                ConstruccionObservacion construccionObservacion = new ConstruccionObservacion();

                //                6   Acta de inicio interventor
                //7   Acta de inicio supervisor
                construccionObservacion.Observaciones = Helpers.Helpers.CleanStringInput(pObservacionesConsideracionesEspeciales);

                if (pEsSupervisor)
                    //if (construccionObservacion.EsSupervision)
                    construccionObservacion.TipoObservacionConstruccion = "7";
                else
                    construccionObservacion.TipoObservacionConstruccion = "6";

                construccionObservacion.EsSupervision = pEsSupervisor;
                construccionObservacion.EsActa = pEsActa;

                construccionObservacion.UsuarioCreacion = pUsuarioModificacion;
                construccionObservacion.FechaCreacion = DateTime.Now;

                ContratoConstruccion contratoConstruccion = null;
                contratoConstruccion = _context.ContratoConstruccion.Where(r => r.ContratoId == contrato.ContratoId).FirstOrDefault();
                construccionObservacion.ContratoConstruccionId = contratoConstruccion.ContratoConstruccionId;

                //_context.ContratoObservacion.Update(contratoObservacion);
                _context.ConstruccionObservacion.Add(construccionObservacion);

                _context.Contrato.Update(contrato);
                //contrato.FechaModificacion = DateTime.Now;
                //contrato.UsuarioModificacion = pUsuarioModificacion;
                //contrato.ConObervacionesActa = true;
                //contrato.Observaciones = pObservacionesConsideracionesEspeciales;

                //contratoObservacion.Observaciones=pObservacionesConsideracionesEspeciales;
                //contratoObservacion.ContratoId = contrato.ContratoId;
                //contratoObservacion.EsActaFase2 = true;
                //contratoObservacion.EsActa = true;

                //contratoObservacion.FechaCreacion = DateTime.Now;
                //contratoObservacion.UsuarioCreacion = pUsuarioModificacion;

                //      CAMBIAR ESTADO “Sin acta generada” a “Con acta generada”.
                //DOM 60  1   Sin acta generada
                //DOM 60  3   Con acta generada
                //contrato.EstadoActa = "3";                

                //_context.Add(contratoObservacion);
                //await _context.SaveChangesAsync();

                //            Plazo de ejecución fase 1 – Preconstrucción: Meses: 4 Días: 3 - PlazoFase1PreMeses - PlazoFase1PreDias - contrato

                //---- - guardar    OK
                //  Plazo de ejecución fase 2 – Construcción: Meses: xx Días: xx - PlazoFase2PreMeses - PlazoFase2PreDias - contrato

                //Observaciones o consideraciones especiales   Observaciones - contrato

                return
             new Respuesta
           {
               IsSuccessful = true,
               IsException = false,
               IsValidation = false,
               Code = ConstantMessagesActaInicio.OperacionExitosa,
               Message =
               await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2,
               ConstantMessagesActaInicio.OperacionExitosa, idAccionCrearActaInicio
               , contrato.UsuarioModificacion, " GUARDAR OBSERVACION CONTRATO ACTA"
               )
           };

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesActaInicio.ErrorInterno, Message = ex.InnerException.ToString().Substring(0, 500) };
            }
        }


        public async Task<Respuesta> EditarContratoObservacion(int pContratoId, int pPlazoFase2PreMeses, int pPlazoFase2PreDias, string pObservacion, string pUsuarioModificacion, DateTime pFechaActaInicioFase1, DateTime pFechaTerminacionFase2, bool pEsSupervisor, bool pEsActa)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Editar_Plazo_Ejecucion_Fase_2, (int)EnumeratorTipoDominio.Acciones);

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
                //if (contratoObservacion != null)
                //{

                //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;
                //contratoPoliza.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;

                //_context.Add(contratoPoliza);

                //contratoPoliza.RegistroCompleo = ValidarRegistroCompletoContratoPoliza(contratoPoliza);

                //contratoObservacion.Observaciones = contratoObservacion.Observaciones;
                Contrato contrato=null;
                contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

                if(contrato!= null)
                {
                    //contrato.Observaciones = Helpers.Helpers.CleanStringInput(pObservacion);

                    //ContratoObservacion contratoObservacion = new ContratoObservacion();
                    //contratoObservacion = await GetContratoObservacionByIdContratoId(pContratoId);

                    //contratoObservacion.Observaciones = Helpers.Helpers.CleanStringInput(pObservacion);
                    //contratoObservacion.UsuarioCreacion = pUsuarioModificacion;
                    //contratoObservacion.FechaCreacion = DateTime.Now;
                    //contratoObservacion.EsActa = true;
                    //contratoObservacion.EsActaFase2 = true;
                    //contratoObservacion.ContratoId = pContratoId;

                    ConstruccionObservacion construccionObservacion = new ConstruccionObservacion();

                    //                6   Acta de inicio interventor
                    //7   Acta de inicio supervisor
                    construccionObservacion.Observaciones = Helpers.Helpers.CleanStringInput(pObservacion);

                    if (pEsSupervisor)
                        //if (construccionObservacion.EsSupervision)
                        construccionObservacion.TipoObservacionConstruccion = "7";
                    else
                        construccionObservacion.TipoObservacionConstruccion = "6";

                    construccionObservacion.EsSupervision = pEsSupervisor;
                    construccionObservacion.EsActa = pEsActa;
               
                    construccionObservacion.UsuarioCreacion = pUsuarioModificacion;
                    construccionObservacion.FechaCreacion = DateTime.Now;

                    ContratoConstruccion contratoConstruccion = null;
                    contratoConstruccion = _context.ContratoConstruccion.Where(r => r.ContratoId == contrato.ContratoId).FirstOrDefault();
                    construccionObservacion.ContratoConstruccionId = contratoConstruccion.ContratoConstruccionId;
                   
                    //_context.ContratoObservacion.Update(contratoObservacion);
                    _context.ConstruccionObservacion.Update(construccionObservacion);
                    _context.SaveChanges();

                    contrato.UsuarioCreacion = pUsuarioModificacion;
                    contrato.FechaModificacion = DateTime.Now;

                    contrato.FechaActaInicioFase1 = pFechaActaInicioFase1;
                    contrato.FechaTerminacionFase2 = pFechaTerminacionFase2;

                    contrato.PlazoFase2ConstruccionDias = pPlazoFase2PreDias;
                    contrato.PlazoFase2ConstruccionMeses = pPlazoFase2PreMeses;

                    ValidarFechasNulas(ref contrato);

                    _context.Contrato.Update(contrato);
                    _context.SaveChanges();

                }
                    //contratoObservacion.Observaciones = Helpers.Helpers.CleanStringInput(contratoObservacion.Observaciones);
                    //contratoObservacion.Observaciones = Helpers.Helpers.ConvertToUpercase(contratoObservacion.Observaciones).ToString();
                    
                    //contratoObservacion.ContratoId = contratoObservacion.ContratoId;}
                    //if(contratoObservacion.EsActaFase2==null)
                    //contratoObservacion.EsActaFase2 = true;

                    //if (contratoObservacion.EsActa == null)
                    //    contratoObservacion.EsActa = true;

                    //contratoObservacion.FechaCreacion = DateTime.Now;
                    //contratoObservacion.UsuarioModificacion = contratoObservacion.UsuarioModificacion;                                       

                    //_context.ContratoPoliza.Add(contratoPoliza);
                    //_context.ContratoObservacion.Update(contratoObservacion);
                    //await _context.SaveChangesAsync();

                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesActaInicio.OperacionExitosa,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2,
                            ConstantMessagesActaInicio.OperacionExitosa,
                            //contratoPoliza
                            idAccionCrearContratoPoliza
                            , contrato.UsuarioModificacion
                            //"UsuarioCreacion"
                            , "EDITAR CONTRATO OBSERVACION"
                            //contratoPoliza.UsuarioCreacion, "REGISTRAR CONTRATO POLIZA"
                            )
                        };

                    //return _response = new Respuesta { IsSuccessful = true,
                    //    IsValidation = false, Data = cuentaBancaria,
                    //    Code = ConstantMessagesBankAccount.OperacionExitosa };
                //}
                //else
                //{
                //    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesActaInicio.RecursoNoEncontrado }; 
                //}

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesActaInicio.ErrorInterno };
            }

        }

        private void ValidarFechasNulas(ref Contrato contrato)
        {
            if (contrato.FechaActaInicioFase1.ToString().Contains("0001"))
                contrato.FechaActaInicioFase1 = null;

            if (contrato.FechaActaInicioFase2.ToString().Contains("0001"))
                contrato.FechaActaInicioFase2 = null;

            if (contrato.FechaAprobacionRequisitos.ToString().Contains("0001"))
                contrato.FechaAprobacionRequisitos = null;

            if (contrato.FechaCreacion.ToString().Contains("0001"))
                contrato.FechaCreacion = null;

            if (contrato.FechaEnvioFirma.ToString().Contains("0001"))
                contrato.FechaEnvioFirma = null;

            if (contrato.FechaEnvioFirmaFormat.ToString().Contains("0001"))
                contrato.FechaEnvioFirmaFormat = null;

            if (contrato.FechaFirmaActaContratista.ToString().Contains("0001"))
                contrato.FechaFirmaActaContratista = null;

            if (contrato.FechaFirmaActaContratistaFase1.ToString().Contains("0001"))
                contrato.FechaFirmaActaContratistaFase1 = null;

            if (contrato.FechaFirmaActaContratistaFase2.ToString().Contains("0001"))
                contrato.FechaFirmaActaContratistaFase2 = null;

            if (contrato.FechaFirmaActaContratistaInterventoria.ToString().Contains("0001"))
                contrato.FechaFirmaActaContratistaInterventoria = null;

            if (contrato.FechaFirmaActaContratistaInterventoriaFase1.ToString().Contains("0001"))
                contrato.FechaFirmaActaContratistaInterventoriaFase1 = null;

            if (contrato.FechaFirmaActaContratistaInterventoriaFase2.ToString().Contains("0001"))
                contrato.FechaFirmaActaContratistaInterventoriaFase2 = null;

            if (contrato.FechaFirmaContratista.ToString().Contains("0001"))
                contrato.FechaFirmaContratista = null;

            if (contrato.FechaFirmaContratistaFormat.ToString().Contains("0001"))
                contrato.FechaFirmaContratistaFormat = null;

            if (contrato.FechaFirmaContrato.ToString().Contains("0001"))
                contrato.FechaFirmaContrato = null;

            if (contrato.FechaFirmaContratoFormat.ToString().Contains("0001"))
                contrato.FechaFirmaContratoFormat = null;

            if (contrato.FechaFirmaFiduciaria.ToString().Contains("0001"))
                contrato.FechaFirmaFiduciaria = null;

            if (contrato.FechaFirmaFiduciariaFormat.ToString().Contains("0001"))
                contrato.FechaFirmaFiduciariaFormat = null;

            if (contrato.FechaModificacion.ToString().Contains("0001"))
                contrato.FechaModificacion = null;

            if (contrato.FechaTerminacion.ToString().Contains("0001"))
                contrato.FechaTerminacion = null;

            if (contrato.FechaTerminacionFase2.ToString().Contains("0001"))
                contrato.FechaTerminacionFase2 = null;

            if (contrato.FechaTramite.ToString().Contains("0001"))
                contrato.FechaTramite = null;

        }

        public async Task<Respuesta> GuardarCargarActaSuscritaContrato(int pContratoId, DateTime pFechaFirmaContratista, DateTime pFechaFirmaActaContratistaInterventoria
            /* archivo pdf */ , IFormFile pFile, string pDirectorioBase, string pDirectorioActaInicio, string pUsuarioModificacion, AppSettingsService _appSettingsService
            )
        {
            //            Fecha de la firma del documento por parte del contratista de obra -FechaFirmaContratista - contrato
            //Fecha de la firma del documento por parte del contratista de interventoría -FechaFirmaActaContratistaInterventoria - contrato
                        
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Editar_Cargar_Acta_Suscrita_Contrato_Fase_2, (int)EnumeratorTipoDominio.Acciones);

            Contrato contrato;

            //contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId && r.Estado == true).FirstOrDefault();
            contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId ).FirstOrDefault();

            try
            {
                contrato.FechaFirmaContratista = pFechaFirmaContratista;
                contrato.FechaFirmaActaContratistaInterventoriaFase2 = pFechaFirmaActaContratistaInterventoria;
                contrato.UsuarioModificacion = pUsuarioModificacion;  
                contrato.FechaModificacion = DateTime.Now;

                string strFilePatch = "";
                //Save Files  
                if (pFile == null)
                {
                }
                else
                {
                    if (pFile.Length > 0)
                    {
                        strFilePatch = Path.Combine(pDirectorioBase, pDirectorioActaInicio, pContratoId.ToString());
                        await _documentService.SaveFileContratacion(pFile, strFilePatch, pFile.FileName);

                    }
                }

                //Auditoria
                //contratacionOld.FechaModificacion = DateTime.Now;
                //contratacionOld.UsuarioModificacion = pContratacion.UsuarioCreacion;
                //Registros
                //contratacionOld.RutaMinuta = strFilePatch;
                //contratacionOld.RegistroCompleto = pContratacion.RegistroCompleto;
                //contratacionOld.RutaMinuta = strFilePatch + "//" + pFile.FileName;
                contrato.EstadoActaFase2 = ((int)EnumeratorEstadoActa.Con_acta_suscrita_y_cargada).ToString(); 
                contrato.RutaActaSuscrita = strFilePatch + "//" + pFile.FileName;
                //contratacionOld.RegistroCompleto = ValidarCamposContratacion(contratacionOld);
                _context.Contrato.Update(contrato);
                 _context.SaveChanges();

                //                notificación de alerta al interventor, al apoyo a la
                //supervisión y al supervisor

                //notificación al interventor y al apoyo a la supervisión.

                int pIdTemplate = (int)enumeratorTemplate.ActaInicioFase2ObraInterventoria;
                //perfilId = 8; //  Supervisor

                VistaGenerarActaInicioContrato actaInicio;

                //pTipoContrato = 2;
                actaInicio = await getDataActaInicioAsync(pContratoId, Convert.ToInt32( contrato.TipoContratoCodigo));

                //Contrato contrato = null;
                //contrato = _context.Contrato.Where(r => r.NumeroContrato == pActaInicio.NumeroContrato).FirstOrDefault();

                string correo = "cdaza@ivolucion.com";

                correo = getCorreos((int)EnumeratorPerfil.Supervisor);
                //_settings = _appSettingsService;
                //_appSettingsService = toAppSettingsService(_settings);
                //   Task<Respuesta> result = EnviarCorreoGestionActaIncio(correo, _settings.Value.MailServer,
                //_settings.Value.MailPort, _settings.Value.Password, _settings.Value.Sender,
                //actaInicio, pIdTemplate);

                bool blEnvioCorreo = false;
                Respuesta respuesta = new Respuesta();

                //Si no llega Email
                //if (string.IsNullOrEmpty(pUsuario.Email))
                //{
                //    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesUsuarios.EmailObligatorio };
                //}
               
                    //Guardar Usuario                

                    //Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjSupervisorGestionPoliza);
                    Template TemplateRecoveryPassword = await _commonService.GetTemplateById(pIdTemplate);

                    string template = TemplateRecoveryPassword.Contenido;

                    int tipoContrato = 0;

                    //Contrato contrato = null;
                    contrato = _context.Contrato.Where(r => r.NumeroContrato == actaInicio.NumeroContrato).FirstOrDefault();

                    //tipoContrato = 2;
                    tipoContrato = ConstanCodigoTipoContratacion.Interventoria;

                    template = template.Replace("_Numero_Contrato_", actaInicio.NumeroContrato);
                    template = template.Replace("_Fecha_Aprobacion_Poliza_", actaInicio.FechaAprobacionGarantiaPoliza);

                    if (Convert.ToInt32(contrato.TipoContratoCodigo) == ConstanCodigoTipoContratacion.Interventoria)
                        template = template.Replace("_Obra_O_Interventoria_", "interventoría");
                    else if (Convert.ToInt32(contrato.TipoContratoCodigo) == ConstanCodigoTipoContratacion.Obra)
                        template = template.Replace("_Obra_O_Interventoria_", "obra");

                    template = template.Replace("_Cantidad_Proyectos_Asociados_", actaInicio.CantidadProyectosAsociados.ToString());

                    template = template.Replace("_Fecha_Acta_Inicio_", actaInicio.FechaActaInicio);
                    template = template.Replace("_Fecha_Prevista_Terminacion_", actaInicio.FechaPrevistaTerminacion);

                //datos basicos generales, aplican para los 4 mensajes

                //correo, _settings.Value.MailServer,
                //_settings.Value.MailPort, _settings.Value.Password, _settings.Value.Sender,
                //actaInicio, pIdTemplate
                 //correo = "cdaza@ivolucion.com";
                blEnvioCorreo = Helpers.Helpers.EnviarCorreo(correo, "Gestión Acta Inicio Fase II", template, _appSettingsService.Sender, _appSettingsService.Password, _appSettingsService.MailServer, _appSettingsService.MailPort);

                    if (blEnvioCorreo)
                        respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantGestionarActaInicioFase2.CorreoEnviado };

                    else
                        respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantGestionarActaInicioFase2.ErrorEnviarCorreo };

                    //}
                    //}
                    //else
                    //{
                    //    respuesta = new Respuesta() { IsSuccessful = true, IsValidation = true, Code = ConstantMessagesContratoPoliza.CorreoNoExiste };

                    //}
                    respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificación_Acta_Inicio_Fase_II), correo, "Acta Inicio Fase II");
                    return respuesta;

                //}
                //catch (Exception ex)
                //{

                //    respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesActaInicio.ErrorEnviarCorreo };
                //    respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificación_Acta_Inicio_Fase_II), lstMails, "Gestión Acta Inicio Fase II") + ": " + ex.ToString() + ex.InnerException;
                //    return respuesta;
                //}

                return
                 new Respuesta
                 {
                     IsSuccessful = true,
                     IsException = false,
                     IsValidation = false,
                     Code = ConstantGestionarActaInicioFase2.OperacionExitosa,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, ConstantGestionarActaInicioFase2.OperacionExitosa, idAccion, contrato.UsuarioModificacion, "CARGAR ACTA SUSCRITA CONTRATO")
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
                  Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, ConstantGestionarActaInicioFase2.Error, idAccion, contrato.UsuarioModificacion, ex.InnerException.ToString())
              };
            }
        }
        

        public async Task<Respuesta> EnviarCorreoSupervisorContratista(int pContratoId, AppSettingsService settings, int pPerfilId)
        {
            Respuesta respuesta = new Respuesta();
            
            string correo = "cdaza@ivolucion.com";
            

            int perfilId = 0;

            int pIdTemplate = (int)enumeratorTemplate.ActaInicioFase2ObraInterventoria;
            Contrato contrato=null;
            contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();
            
            VistaGenerarActaInicioContrato actaInicio=new VistaGenerarActaInicioContrato();
            if(contrato!=null)
                actaInicio = await getDataActaInicioAsync(pContratoId,Convert.ToInt32( contrato.TipoContratoCodigo));

            //perfilId = 8; //  Supervisor
            //perfilId = (int)EnumeratorPerfil.Supervisor; //  Supervisor
            perfilId = pPerfilId;

            correo = getCorreos(perfilId);

            try
            {
                int idAccionCorreoContrato = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Notificación_Acta_Inicio_Fase_II, (int)EnumeratorTipoDominio.Acciones);

               //error correo
                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesActaInicio.ErrorEnviarCorreo };

                //Task<Respuesta> result = EnviarCorreoGestionActaIncio(correo, settings.MailServer,
                //settings.MailPort, settings.Password, settings.Sender,
                //actaInicio,  pIdTemplate );

                bool blEnvioCorreo = false;
                //Respuesta respuesta = new Respuesta();

                //Si no llega Email
                //if (string.IsNullOrEmpty(pUsuario.Email))
                //{
                //    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesUsuarios.EmailObligatorio };
                //}

                //Guardar Usuario                

                //Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjSupervisorGestionPoliza);
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById(pIdTemplate);

                string template = TemplateRecoveryPassword.Contenido;

                int tipoContrato = 0;

                //Contrato contrato = null;
                contrato = _context.Contrato.Where(r => r.NumeroContrato == actaInicio.NumeroContrato).FirstOrDefault();

                //tipoContrato = 2;
                tipoContrato = ConstanCodigoTipoContratacion.Interventoria;

                template = template.Replace("_Numero_Contrato_", actaInicio.NumeroContrato);
                template = template.Replace("_Fecha_Aprobacion_Poliza_", actaInicio.FechaAprobacionGarantiaPoliza);

                if (Convert.ToInt32(contrato.TipoContratoCodigo) == ConstanCodigoTipoContratacion.Interventoria)
                    template = template.Replace("_Obra_O_Interventoria_", "interventoría");
                else if (Convert.ToInt32(contrato.TipoContratoCodigo) == ConstanCodigoTipoContratacion.Obra)
                    template = template.Replace("_Obra_O_Interventoria_", "obra");

                template = template.Replace("_Cantidad_Proyectos_Asociados_", actaInicio.CantidadProyectosAsociados.ToString());

                template = template.Replace("_Fecha_Acta_Inicio_", actaInicio.FechaActaInicio);
                template = template.Replace("_Fecha_Prevista_Terminacion_", actaInicio.FechaPrevistaTerminacion);

                //datos basicos generales, aplican para los 4 mensajes

                //correo, _settings.Value.MailServer,
                //_settings.Value.MailPort, _settings.Value.Password, _settings.Value.Sender,
                //actaInicio, pIdTemplate
                //correo = "cdaza@ivolucion.com";
                blEnvioCorreo = Helpers.Helpers.EnviarCorreo(correo, "Gestión Acta Inicio Fase II", template, settings.Sender, settings.Password, settings.MailServer, settings.MailPort);

                if (blEnvioCorreo)
                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantGestionarActaInicioFase2.CorreoEnviado };

                else
                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantGestionarActaInicioFase2.ErrorEnviarCorreo };

                //}
                //}
                //else
                //{
                //    respuesta = new Respuesta() { IsSuccessful = true, IsValidation = true, Code = ConstantMessagesContratoPoliza.CorreoNoExiste };

                //}
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificación_Acta_Inicio_Fase_II), correo, "Acta Inicio Fase II");
                return respuesta;

                //}
                //catch (Exception ex)
                //{

                //    respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesActaInicio.ErrorEnviarCorreo };
                //    respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificación_Acta_Inicio_Fase_II), lstMails, "Gestión Acta Inicio Fase II") + ": " + ex.ToString() + ex.InnerException;
                //    return respuesta;
                //}

                //ok correo

                respuesta = new Respuesta() { IsSuccessful = true, IsValidation = true, Code = ConstantMessagesActaInicio.CorreoEnviado };

                

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
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, respuesta.Code, Convert.ToInt32( ConstantCodigoAcciones.Notificación_Acta_Inicio_Fase_II), correo, "Acta Inicio Fase II");
                return respuesta;

            }
            catch (Exception ex)
            {

                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesActaInicio.ErrorEnviarCorreo };
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, respuesta.Code, Convert.ToInt32( ConstantCodigoAcciones.Notificación_Acta_Inicio_Fase_II), correo, "Acta Inicio Fase II") + ": " + ex.ToString() + ex.InnerException;
                return respuesta;
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
                    //lstCorreos = lstCorreos += usuario.Email + ";";
                    lstCorreos = lstCorreos += usuario.Email ;
                }
            }
            return lstCorreos;
        }


        public async Task<byte[]> GetPlantillaActaInicio(int pContratoId)
        {
            if (pContratoId == 0)
            {
                return Array.Empty<byte>();
            }

            VistaGenerarActaInicioContrato actaInicio;

            int pTipoContrato = 2;

            //               --Contratacion.TipoContratacionCodigo  14 DOM, tipoidentificacion

            ////DOM 14 1   Obra            
            //pTipoContrato = 1;

            //DOM 14 2   interventoria            
            pTipoContrato = ConstanCodigoTipoContratacion.Interventoria;
            actaInicio = await getDataActaInicioAsync(pContratoId, pTipoContrato);
            
            string Vlrfase2ConstruccionObra="", VlrFase1Preconstruccion="";            
            VlrFase1Preconstruccion = actaInicio.ValorFase1Preconstruccion;

            if(actaInicio.ContratacionId!=null)
            {
                VlrFase1Preconstruccion = getSumVlrContratoComponente(Convert.ToInt32(actaInicio.ContratacionId),
                    "1"  //ConstanCodigoFaseContrato.Preconstruccion.ToString()
                    ).ToString();
                
            }            

            pTipoContrato = ConstanCodigoTipoContratacion.Obra;
            if (actaInicio.NumeroDRP1 == "ERROR")
            {
                actaInicio = await getDataActaInicioAsync(pContratoId, pTipoContrato);
                Vlrfase2ConstruccionObra = actaInicio.Valorfase2ConstruccionObra;

                if (actaInicio.ContratacionId != null)
                {
                    Vlrfase2ConstruccionObra = getSumVlrContratoComponente(Convert.ToInt32(actaInicio.ContratacionId), "2"  //ConstanCodigoFaseContrato.Preconstruccion.ToString()
                        ).ToString();
                    //if (ComponenteAportante.FaseCodigo =="1"  //ConstanCodigoFaseContrato.Preconstruccion.ToString()

                }

            }                

            if (actaInicio == null)
            {
                return Array.Empty<byte>();
            }

            Contrato contrato=null;
            contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

            Plantilla plantilla=null;

            if (contrato.TipoContratoCodigo == ((int)ConstanCodigoTipoContratacion.Interventoria).ToString())
            {
                pTipoContrato = ConstanCodigoTipoContratacion.Interventoria;
                plantilla = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Acta_inicio_interventoria_PDF).ToString()).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();

            }

           else if (contrato.TipoContratoCodigo == ((int)ConstanCodigoTipoContratacion.Obra).ToString())
            {
                pTipoContrato = ConstanCodigoTipoContratacion.Obra;
                plantilla = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Acta_inicio_obra_PDF).ToString()).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();

            }                                   

            //Plantilla plantilla = new Plantilla();
            //plantilla.Contenido = "";
            if(plantilla!=null)
            plantilla.Contenido = await ReemplazarDatosPlantillaActaInicio(plantilla.Contenido, actaInicio,"cdaza");
            return ConvertirPDF(plantilla);
        }

        private async Task<string> ReemplazarDatosPlantillaActaInicio(string strContenido, VistaGenerarActaInicioContrato pActaInicio, string usuario)
        {
            string str = "";
            string valor = "";

            strContenido = strContenido.Replace("_Numero_Identificacion_Entidad_Contratista_Interventoria_", pActaInicio.NumeroIdentificacionEntidadContratistaObra);
            strContenido = strContenido.Replace("_Nombre_Representante_Contratista_Interventoria_", pActaInicio.NombreRepresentanteContratistaInterventoria);
            strContenido = strContenido.Replace("_Nombre_Entidad_Contratista_Interventoria_", pActaInicio.NombreEntidadContratistaSupervisorInterventoria);

            strContenido = strContenido.Replace("_Nombre_Representante_Contratista_Obra_", pActaInicio.NombreRepresentanteContratistaObra);
            strContenido = strContenido.Replace("_Nombre_Representante_Contratista_Interventoria_", pActaInicio.NombreRepresentanteContratistaObra);

            strContenido = strContenido.Replace("_Nombre_Entidad_Contratista_Obra_", pActaInicio.NombreEntidadContratistaObra);
            strContenido = strContenido.Replace("_Nombre_Entidad_Contratista_Interventoria_", pActaInicio.NombreEntidadContratistaObra);

            strContenido = strContenido.Replace("_Numero_Identificacion_Entidad_Contratista_Obra_", pActaInicio.NumeroIdentificacionEntidadContratistaObra);
            strContenido = strContenido.Replace(" _Fecha_Prevista_Terminacion_", pActaInicio.FechaPrevistaTerminacion);
            strContenido = strContenido.Replace("_OBSERVACION_O_CONSIDERACIONES_ESPECIALES_", pActaInicio.ObservacionOConsideracionesEspeciales);
            strContenido = strContenido.Replace("_Plazo_Ejecucion_Fase_1_Preconstruccion_", pActaInicio.PlazoFase1PreMeses.ToString());
            strContenido = strContenido.Replace("_Plazo_Ejecucion_Fase_2_Construccion_", pActaInicio.PlazoFase2ConstruccionMeses.ToString());
                       
            strContenido = strContenido.Replace("_Valor_Actual_Contrato_", formatValor(pActaInicio.ValorActualContrato) );
            
            strContenido = strContenido.Replace("_Plazo_Inicial_Contrato_", formatValor( pActaInicio.PlazoInicialContratoSupervisor));
            strContenido = strContenido.Replace("_Valor_Fase_1_preconstruccion_", formatValor(pActaInicio.ValorFase1Preconstruccion));
            strContenido = strContenido.Replace("_Valor_Fase_2_Construccion_Obra_", formatValor(pActaInicio.Valorfase2ConstruccionObra));
            strContenido = strContenido.Replace("_Nombre_Entidad_Contratista_Obra_", pActaInicio.NombreEntidadContratistaObra);
            strContenido = strContenido.Replace("_Valor_Inicial_Contrato_", formatValor(pActaInicio.ValorInicialContrato));
            strContenido = strContenido.Replace("_Fecha_Aprobacion_Garantia_Poliza_", pActaInicio.FechaAprobacionGarantiaPoliza);
            strContenido = strContenido.Replace("_Objeto_", pActaInicio.Objeto);
            strContenido = strContenido.Replace("_Numero_DRP_", pActaInicio.NumeroDRP1);
            strContenido = strContenido.Replace("_Fecha_Generación_DRP_", pActaInicio.FechaGeneracionDRP1);
            strContenido = strContenido.Replace("_Institucion_Educativa_Llave_MEN_", pActaInicio.InstitucionEducativaLlaveMEN);
            strContenido = strContenido.Replace("_Departamento_y_Municipio_Llave_MEN_", pActaInicio.DepartamentoYMunicipioLlaveMEN);
            strContenido = strContenido.Replace("_Fecha_Acta_Inicio_", pActaInicio.FechaActaInicio);
            strContenido = strContenido.Replace("_Llave_MEN_Contrato_", pActaInicio.LlaveMENContrato);
            strContenido = strContenido.Replace("_Numero_Identificacion_Entidad_Contratista_Obra_", pActaInicio.NumeroIdentificacionEntidadContratistaObra);
            strContenido = strContenido.Replace("_Numero_Contrato_Obra_", pActaInicio.NumeroContrato);
            strContenido = strContenido.Replace("_Numero_Contrato_Interventoria_", pActaInicio.NumeroContrato);
            
            strContenido = strContenido.Replace("_Nombre_Entidad_Contratista_Obra_", pActaInicio.NombreEntidadContratistaObra);
            strContenido = strContenido.Replace("_Numero_Identificacion_Contratista_Interventoria_", pActaInicio.NumeroIdentificacionContratistaInterventoria);
            //strContenido = strContenido.Replace("_Nombre_Representante_Contratista_Obra_", pActaInicio.NombreRepresentanteContratistaObra);
            //strContenido = strContenido.Replace("_Nombre_Representante_Contratista_Interventoria_", valor);
            //strContenido = strContenido.Replace("_Nombre_Entidad_Contratista_Interventoria_", valor);
            //strContenido = strContenido.Replace("_Fecha_Acta_Inicio_", valor);
            //strContenido = strContenido.Replace("_Numero_Contrato_Obra_", valor);

            //datos exclusivos interventoria

            UsuarioPerfil UsuarioPerfil = _context.UsuarioPerfil.Where(y => y.Usuario.Email == usuario).Include(y => y.Perfil).FirstOrDefault();

            Perfil perfil=null;

            if (UsuarioPerfil != null) { 
             perfil = _context.Perfil.Where(y => y.PerfilId == UsuarioPerfil.PerfilId).FirstOrDefault();

            }
            if (UsuarioPerfil != null)
            {
                strContenido = strContenido.Replace("_Cargo_Usuario_", perfil.Nombre);
            }
            strContenido = strContenido.Replace("_Nombre_Supervisor_", "_Nombre_Supervisor_");          

            return strContenido;

        }

        private string formatValor(string valor)
        {
            if (valor != null)
            {
                if (valor.Length >2)
                    valor = valor.Remove(valor.Length - 3);            
            return valor;

            }
            return string.Empty;      
        }

        public byte[] ConvertirPDF(Plantilla pPlantilla)
        {
            string strEncabezado = " encabezado";

            //pendiente
            //if (!string.IsNullOrEmpty(pPlantilla.Encabezado.Contenido))
            //{
                pPlantilla.Contenido = pPlantilla.Contenido.Replace("[RUTA_ICONO]", Path.Combine(Directory.GetCurrentDirectory(), "assets", "img-FFIE.png"));
            //    pPlantilla.Encabezado.Contenido = pPlantilla.Encabezado.Contenido.Replace("[RUTA_ICONO]", Path.Combine(Directory.GetCurrentDirectory(), "assets", "img-FFIE.png"));
            //    strEncabezado = Helpers.Helpers.HtmlStringLimpio(pPlantilla.Encabezado.Contenido);
            //}

            var globalSettings = new GlobalSettings
            {
                ImageQuality = 1080,
                PageOffset = 0,
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings
                {
                    Top = pPlantilla.MargenArriba,
                    Left = pPlantilla.MargenIzquierda,
                    Right = pPlantilla.MargenDerecha,
                    Bottom = pPlantilla.MargenAbajo
                },
                DocumentTitle = DateTime.Now.ToString(),
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = pPlantilla.Contenido,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "pdf-styles.css") },
                HeaderSettings = { FontName = "Roboto", FontSize = 8, Center = strEncabezado, Line = false, Spacing = 18 },
                FooterSettings = { FontName = "Ariel", FontSize = 10, Center = "[page]" },
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };

            return _converter.Convert(pdf);
        }

        public async Task<Respuesta> EnviarCorreoGestionActaIncio(string lstMails, string pMailServer, int pMailPort, string pPassword, string pSentender, VistaGenerarActaInicioContrato pActaInicio,  int pIdTemplate)
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

                //Guardar Usuario
                //await UpdateUser(usuarioSolicito);

                //Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjSupervisorGestionPoliza);
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById(pIdTemplate);

                string template = TemplateRecoveryPassword.Contenido;

                //string urlDestino = pDominio;
                //asent/img/logo      

                //VistaGenerarActaInicioContrato pActaInicio = new VistaGenerarActaInicioContrato();

                int tipoContrato = 0;               

                Contrato contrato=null;
                contrato = _context.Contrato.Where(r => r.NumeroContrato == pActaInicio.NumeroContrato).FirstOrDefault();
                //template = template.Replace("_Numero_Contrato_", objVistaContratoGarantiaPoliza.NumeroContrato);
                //objVistaContratoGarantiaPoliza.FEC

                //tipoContrato = 2;
                tipoContrato = ConstanCodigoTipoContratacion.Interventoria;

                //if (contrato != null)
                //{
                //    pActaInicio = await getDataActaInicioAsync(contrato.ContratoId, Convert.ToInt32( contrato.TipoContratoCodigo));
                //}                

                template = template.Replace("_Numero_Contrato_", pActaInicio.NumeroContrato);
                template = template.Replace("_Fecha_Aprobacion_Poliza_", pActaInicio.FechaAprobacionGarantiaPoliza);
                
                if(Convert.ToInt32(contrato.TipoContratoCodigo)==ConstanCodigoTipoContratacion.Interventoria)
                    template = template.Replace("_Obra_O_Interventoria_", "interventoría");
                else if (Convert.ToInt32(contrato.TipoContratoCodigo) == ConstanCodigoTipoContratacion.Obra)
                    template = template.Replace("_Obra_O_Interventoria_", "obra");

                template = template.Replace("_Cantidad_Proyectos_Asociados_", pActaInicio.CantidadProyectosAsociados.ToString());

                template = template.Replace("_Fecha_Acta_Inicio_", pActaInicio.FechaActaInicio);
                template = template.Replace("_Fecha_Prevista_Terminacion_", pActaInicio.FechaPrevistaTerminacion);

                //datos basicos generales, aplican para los 4 mensajes

                blEnvioCorreo = Helpers.Helpers.EnviarCorreo(lstMails, "Gestión Acta Inicio Fase II", template, pSentender, pPassword, pMailServer, pMailPort);

                if (blEnvioCorreo)
                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantGestionarActaInicioFase2.CorreoEnviado };

                else
                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantGestionarActaInicioFase2.ErrorEnviarCorreo };

                //}
                //}
                //else
                //{
                //    respuesta = new Respuesta() { IsSuccessful = true, IsValidation = true, Code = ConstantMessagesContratoPoliza.CorreoNoExiste };

                //}
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, respuesta.Code, Convert.ToInt32( ConstantCodigoAcciones.Notificación_Acta_Inicio_Fase_II), lstMails, "Acta Inicio Fase II");
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesActaInicio.ErrorEnviarCorreo };
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificación_Acta_Inicio_Fase_II), lstMails, "Gestión Acta Inicio Fase II") + ": " + ex.ToString() + ex.InnerException;
                return respuesta;
            }

        }

        //public  Task<Respuesta> EnviarCorreoGestionActaIncio2(string lstMails, string pMailServer, int pMailPort, string pPassword, string pSentender, VistaGenerarActaInicioContrato pActaInicio, int pIdTemplate)
        //{
        //    bool blEnvioCorreo = false;
        //    Respuesta respuesta = new Respuesta();

        //    //Si no llega Email
        //    //if (string.IsNullOrEmpty(pUsuario.Email))
        //    //{
        //    //    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesUsuarios.EmailObligatorio };
        //    //}
        //    try
        //    {               
        //        //Guardar Usuario                

        //        //Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjSupervisorGestionPoliza);
        //        Template TemplateRecoveryPassword =  _commonService.GetTemplateById(pIdTemplate);

        //        string template = TemplateRecoveryPassword.Contenido;              

        //        int tipoContrato = 0;

        //        Contrato contrato = null;
        //        contrato = _context.Contrato.Where(r => r.NumeroContrato == pActaInicio.NumeroContrato).FirstOrDefault();

        //        //tipoContrato = 2;
        //        tipoContrato = ConstanCodigoTipoContratacion.Interventoria;                      

        //        template = template.Replace("_Numero_Contrato_", pActaInicio.NumeroContrato);
        //        template = template.Replace("_Fecha_Aprobacion_Poliza_", pActaInicio.FechaAprobacionGarantiaPoliza);

        //        if (Convert.ToInt32(contrato.TipoContratoCodigo) == ConstanCodigoTipoContratacion.Interventoria)
        //            template = template.Replace("_Obra_O_Interventoria_", "interventoría");
        //        else if (Convert.ToInt32(contrato.TipoContratoCodigo) == ConstanCodigoTipoContratacion.Obra)
        //            template = template.Replace("_Obra_O_Interventoria_", "obra");

        //        template = template.Replace("_Cantidad_Proyectos_Asociados_", pActaInicio.CantidadProyectosAsociados.ToString());

        //        template = template.Replace("_Fecha_Acta_Inicio_", pActaInicio.FechaActaInicio);
        //        template = template.Replace("_Fecha_Prevista_Terminacion_", pActaInicio.FechaPrevistaTerminacion);

        //        //datos basicos generales, aplican para los 4 mensajes

        //        blEnvioCorreo = Helpers.Helpers.EnviarCorreo(lstMails, "Gestión Acta Inicio Fase II", template, pSentender, pPassword, pMailServer, pMailPort);

        //        if (blEnvioCorreo)
        //            respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantGestionarActaInicioFase2.CorreoEnviado };

        //        else
        //            respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantGestionarActaInicioFase2.ErrorEnviarCorreo };

        //        //}
        //        //}
        //        //else
        //        //{
        //        //    respuesta = new Respuesta() { IsSuccessful = true, IsValidation = true, Code = ConstantMessagesContratoPoliza.CorreoNoExiste };

        //        //}
        //        respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificación_Acta_Inicio_Fase_II), lstMails, "Acta Inicio Fase II");
        //        return respuesta;


        //    }
        //    catch (Exception ex)
        //    {

        //        respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesActaInicio.ErrorEnviarCorreo };
        //        respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificación_Acta_Inicio_Fase_II), lstMails, "Gestión Acta Inicio Fase II") + ": " + ex.ToString() + ex.InnerException;
        //        return respuesta;
        //    }

        //}

        //public async Task<ContratoObservacion> GetContratoObservacionByIdContratoId(int pContratoId)
        public async Task<ConstruccionObservacion> GetContratoObservacionByIdContratoId(int pContratoId, bool pEsSupervisor)
        {

            //includefilter
            //ContratoObservacion contratoObservacion = new ContratoObservacion();
            //List<ContratoObservacion> lstContratoObservacion = new List<ContratoObservacion>();
            //lstContratoObservacion=_context.ContratoObservacion.Where(r => r.ContratoId == pContratoId && r.EsActaFase2==true).ToList();
            //lstContratoObservacion = lstContratoObservacion.OrderByDescending(r => r.ContratoObservacionId).ToList();

            ////contratoPoliza = _context.ContratoPoliza.Where(r => !(bool)r.Eliminado && r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();
            //contratoObservacion = lstContratoObservacion.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

            ConstruccionObservacion contratoObservacion = new ConstruccionObservacion();
            List<ConstruccionObservacion> lstContratoObservacion = new List<ConstruccionObservacion>();

            ContratoConstruccion contratoConstruccion = null;
            contratoConstruccion = _context.ContratoConstruccion.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

            if (contratoConstruccion != null)
            {
                contratoObservacion.ContratoConstruccionId = contratoConstruccion.ContratoConstruccionId;

                lstContratoObservacion = _context.ConstruccionObservacion.Where(r => r.ContratoConstruccionId == contratoConstruccion.ContratoConstruccionId && r.EsSupervision == pEsSupervisor&&r.EsActa==true).ToList();
                lstContratoObservacion = lstContratoObservacion.OrderByDescending(r => r.ConstruccionObservacionId).ToList();

                //contratoPoliza = _context.ContratoPoliza.Where(r => !(bool)r.Eliminado && r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();
                contratoObservacion = lstContratoObservacion.Where(r => r.ContratoConstruccionId == contratoConstruccion.ContratoConstruccionId).FirstOrDefault();
                return contratoObservacion;
            }
            return null;
        }

        public async Task<Contrato> GetContratoByIdContratoId(int pContratoId)
        {

            //includefilter
            Contrato contrato = new Contrato();

            contrato= _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();   
          
            return contrato;
        }

        public async Task<Respuesta> InsertEditContratoObservacion(ConstruccionObservacion construccionObservacion)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Contrato_Observacion, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar;
            try
            {
                if (construccionObservacion != null)
                {
                    if ((bool)construccionObservacion.EsSupervision)
                        //if (construccionObservacion.EsSupervision)
                        construccionObservacion.TipoObservacionConstruccion = "7";
                    else
                        construccionObservacion.TipoObservacionConstruccion = "6";

                    if (construccionObservacion.ConstruccionObservacionId == 0)
                    {
                        //Auditoria
                        strCrearEditar = "REGISTRAR CONTRATO OBSERVACION";

                        //ConstruccionObservacion construccionObservacion = new ConstruccionObservacion();

                        //                6   Acta de inicio interventor
                        //7   Acta de inicio supervisor
                        construccionObservacion.Observaciones = Helpers.Helpers.CleanStringInput(construccionObservacion.Observaciones);
                                      
                        //construccionObservacion.EsSupervision = pEsSupervisor;
                        //construccionObservacion.EsActa = pEsActa;

                        //construccionObservacion.UsuarioCreacion = construccionObservacion.UsuarioCreacion;
                        //construccionObservacion.UsuarioModificacion = construccionObservacion.UsuarioModificacion;
                        //construccionObservacion.EsActa = construccionObservacion.UsuarioModificacion;
                        //construccionObservacion.UsuarioCreacion = pUsuarioModificacion;
                        construccionObservacion.FechaCreacion = DateTime.Now;

                        ContratoConstruccion contratoConstruccion = null;

                        contratoConstruccion = _context.ContratoConstruccion.Where(r => r.ContratoConstruccionId == construccionObservacion.ContratoConstruccionId).FirstOrDefault();

                        if (contratoConstruccion != null)
                        {
                            construccionObservacion.ContratoConstruccionId = contratoConstruccion.ContratoConstruccionId;

                            _context.ConstruccionObservacion.Add(construccionObservacion);
                            await _context.SaveChangesAsync();

                        }                       

                    }
                    else
                    {
                        strCrearEditar = "EDIT CONTRATO OBSERVACION";
                        ConstruccionObservacion construccionObservacion2=null;

                        ContratoConstruccion contratoConstruccion2 = null;

                        contratoConstruccion2 = _context.ContratoConstruccion.Where(r => r.ContratoConstruccionId == construccionObservacion.ContratoConstruccionId).FirstOrDefault();

                        if (contratoConstruccion2 != null)
                        {
                            //obtener ultima obserbacion
                            construccionObservacion2 = await GetContratoObservacionByIdContratoId(contratoConstruccion2.ContratoId, Convert.ToBoolean(construccionObservacion.EsSupervision));

                            //sin alterar los ids
                            construccionObservacion2.EsActa = construccionObservacion.EsActa;
                            construccionObservacion2.EsSupervision = construccionObservacion.EsSupervision;
                            //construccionObservacion2.FechaCreacion = construccionObservacion.FechaCreacion;
                            //construccionObservacion2.FechaModificacion = construccionObservacion.FechaModificacion;
                            construccionObservacion2.FechaModificacion = DateTime.Now;
                            construccionObservacion2.Observaciones = Helpers.Helpers.CleanStringInput(construccionObservacion.Observaciones);
                            //construccionObservacion2.UsuarioCreacion = construccionObservacion.UsuarioCreacion;
                            

                            //construccionObservacion.ConstruccionObservacionId = construccionObservacion2.ConstruccionObservacionId;
                            //construccionObservacion.ContratoConstruccionId = construccionObservacion2.ContratoConstruccionId;
                            //construccionObservacion.id

                            _context.ConstruccionObservacion.Update(construccionObservacion2);
                            await _context.SaveChangesAsync();
                        }
                    }
                    //contratoPoliza.FechaCreacion = DateTime.Now;
                    //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;                                     

                    //contratoPoliza.ObservacionesRevisionGeneral = ValidarRegistroCompleto(cofinanciacion);

                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesActaInicio.OperacionExitosa,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2,
                            ConstantMessagesActaInicio.OperacionExitosa,
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
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesActaInicio.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesActaInicio.ErrorInterno, Message = ex.InnerException.ToString().Substring(0, 500) };
            }

        }

        //public async Task<Respuesta> InsertEditContratoObservacion(ContratoObservacion contratoObservacion)
        //{
        //    Respuesta _response = new Respuesta();

        //    int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Contrato_Observacion, (int)EnumeratorTipoDominio.Acciones);

        //    string strCrearEditar;
        //    try
        //    {
        //        if (contratoObservacion != null)
        //        {
        //            if (contratoObservacion.ContratoObservacionId == 0)
        //            {
        //                //Auditoria
        //                strCrearEditar = "REGISTRAR CONTRATO OBSERVACION";
        //                _context.ContratoObservacion.Add(contratoObservacion);
        //                await _context.SaveChangesAsync();

        //            }
        //            else
        //            {
        //                strCrearEditar = "EDIT CONTRATO OBSERVACION";
        //                _context.ContratoObservacion.Update(contratoObservacion);
        //                await _context.SaveChangesAsync();


        //            }
        //            //contratoPoliza.FechaCreacion = DateTime.Now;
        //            //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;                                     

        //            //contratoPoliza.ObservacionesRevisionGeneral = ValidarRegistroCompleto(cofinanciacion);

        //            return
        //                new Respuesta
        //                {
        //                    IsSuccessful = true,
        //                    IsException = false,
        //                    IsValidation = false,
        //                    Code = ConstantMessagesActaInicio.OperacionExitosa,
        //                    Message =
        //                    await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2,
        //                    ConstantMessagesActaInicio.OperacionExitosa,
        //                    //contratoPoliza
        //                    1
        //                    ,
        //                    "UsuarioCreacion", strCrearEditar
        //                    //contratoPoliza.UsuarioCreacion, "REGISTRAR POLIZA GARANTIA"
        //                    )
        //                };

        //            //return _response = new Respuesta { IsSuccessful = true,
        //            //    IsValidation = false, Data = cuentaBancaria,
        //            //    Code = ConstantMessagesBankAccount.OperacionExitosa };
        //        }
        //        else
        //        {
        //            return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesActaInicio.RecursoNoEncontrado };
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesActaInicio.ErrorInterno, Message = ex.InnerException.ToString().Substring(0, 500) };
        //    }

        //}


        //Task<ActionResult<VistaGenerarActaInicioContrato>> GetListVistaGenerarActaInicio(int pContratoId);
        //GetListVistaGenerarActaInicio

        private decimal getSumVlrContratoComponente(int contratacionId, string FaseCodigo)
        {

            //if (ComponenteAportante.FaseCodigo =="1"  //ConstanCodigoFaseContrato.Preconstruccion.ToString()
            //    )
            //{
            //    contrato.ValorFase1 += ComponenteAportante.ComponenteUso.Sum(r => r.ValorUso);
            //    contrato.TieneFase1 = true;
            //}
            //else
            //{
            //    contrato.ValorFase2 += ComponenteAportante.ComponenteUso.Sum(r => r.ValorUso);
            //    contrato.TieneFase2 = true;
            //}

            var sum = _context.ComponenteUso.Where(
                x => x.ComponenteAportante.ContratacionProyectoAportante.ContratacionProyecto.ContratacionId == contratacionId
                && x.ComponenteAportante.FaseCodigo == FaseCodigo

                ).Sum(x => x.ValorUso);
            return sum;

        }
        public async Task<List<GrillaActaInicio>> GetListGrillaActaInicio(int pPerfilId)
        {
            //            Número del contrato de obra DisponibilidadPresupuestal? contrato - numeroContrato
            //Estado del acta - CONTRATO - EstadoActa - DOM 60
           List<GrillaActaInicio> lstActaInicio = new List<GrillaActaInicio>();
            GrillaActaInicio actaInicio = new GrillaActaInicio();
            List<Contrato> lstContratos = new List<Contrato>();
            //lstContratos = _context.Contrato.Where(r => r.Eliminado == false).ToList();
            //pContrato.EstadoVerificacionCodigo = ConstanCodigoEstadoVerificacionContrato.Sin_aprobacion_de_requisitos_tecnicos;
            //lstContratos = await _context.Contrato.Where(r => r.Contratacion.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.Registrados.ToString() && !(bool)r.Eliminado).ToListAsync();
            lstContratos = await _context.Contrato.Where(
                r => r.EstadoVerificacionConstruccionCodigo == ConstanCodigoEstadoVerificacionContrato.Con_requisitos_tecnicos_aprobados.ToString() 
                && !(bool)r.Eliminado).ToListAsync();
            Contratacion contratacion;

            Dominio EstadoActaFase2Contrato=null;
            string strEstadoActaFase2Contrato="";
            string strEstadoActaCodigoFase2Contrato = "";

            Dominio EstadoVerificacion=null;
            string strEstadoVerificacion = "";           

            Dominio TipoContratoCodigoContrato ;
            string strTipoContratoCodigo = "";
            bool bTieneObservacionesSupervisor;

            string contratacionTipoContrato = "";
           
            foreach (var item in lstContratos)
            {
                actaInicio = new GrillaActaInicio();                               

                //EstadoActaFase2Contrato = await _commonService.GetDominioByNombreDominioAndTipoDominio(item.EstadoActaFase2, (int)EnumeratorTipoDominio.Estado_Acta_Contrato);

                EstadoVerificacion = await _commonService.GetDominioByNombreDominioAndTipoDominio(item.EstadoVerificacionConstruccionCodigo, (int)EnumeratorTipoDominio.Estado_Verificacion_Contrato);

                //EstadoActaFase2Contrato = await _commonService.GetDominioByNombreDominioAndTipoDominio(item.EstadoActa, (int)EnumeratorTipoDominio.Estado_Acta_Contrato);
                                             
                if (EstadoVerificacion != null)
                    strEstadoVerificacion = EstadoVerificacion.Nombre;

                //ContratoObservacion contratoObservacion = null;
                //contratoObservacion = await GetContratoObservacionByIdContratoId(item.ContratoId, false);

                ConstruccionObservacion contratoObservacion = null;
                contratoObservacion = await GetContratoObservacionByIdContratoId(item.ContratoId, false);

                if (contratoObservacion != null)
                    bTieneObservacionesSupervisor = true;
                else
                    bTieneObservacionesSupervisor = false;

                string proponenteCodigo = "";

                if (strTipoContratoCodigo == ((int)ConstanCodigoTipoContratacion.Obra).ToString())
                    proponenteCodigo = "Obra";
                else if (strTipoContratoCodigo == ((int)ConstanCodigoTipoContratacion.Interventoria).ToString())
                    proponenteCodigo = "Interventoria";

                actaInicio.EstadoActaCodigo = strEstadoActaCodigoFase2Contrato;
                actaInicio.EstadoVerificacion = strEstadoVerificacion;
                actaInicio.EstadoActa = strEstadoActaFase2Contrato;
                actaInicio.ContratoId = item.ContratoId;
                actaInicio.NumeroContratoObra = item.NumeroContrato;
                actaInicio.TipoContrato = strTipoContratoCodigo;
                actaInicio.ProponenteCodigo = proponenteCodigo;
                actaInicio.TieneObservacionesSupervisor = bTieneObservacionesSupervisor;
                //actaInicio.FechaAprobacionRequisitos= item.FechaAprobacionRequisitos.ToString("dd/MM/yyyy");

                contratacion = _context.Contratacion.Where(r => r.ContratacionId == item.ContratacionId).FirstOrDefault();

                if (contratacion != null)
                    contratacionTipoContrato = contratacion.TipoSolicitudCodigo.Trim();

                if (item.EstadoActaFase2 != null)
                {
                    //if (item.TipoContratoCodigo == ((int)ConstanCodigoTipoContratacion.Interventoria).ToString())
                    if (contratacionTipoContrato == ((int)ConstanCodigoTipoContratacion.Interventoria).ToString())
                        EstadoActaFase2Contrato = await _commonService.GetDominioByNombreDominioAndTipoDominio(item.EstadoActaFase2.Trim(), (int)EnumeratorTipoDominio.Estados_actas_inicio_interventoria);
                    //else if (item.TipoContratoCodigo == ((int)ConstanCodigoTipoContratacion.Obra).ToString())
                    else if (contratacionTipoContrato == ((int)ConstanCodigoTipoContratacion.Obra).ToString())
                        EstadoActaFase2Contrato = await _commonService.GetDominioByNombreDominioAndTipoDominio(item.EstadoActaFase2.Trim(), (int)EnumeratorTipoDominio.Estados_actas_inicio_obra);
                }

                if (EstadoActaFase2Contrato != null)
                {
                    strEstadoActaCodigoFase2Contrato = EstadoActaFase2Contrato.Codigo;                   
                    
                    actaInicio.EstadoActa = EstadoActaFase2Contrato.Nombre;

                    if ((int)EnumeratorPerfil.Supervisor == pPerfilId)
                    {
                        strEstadoActaFase2Contrato = EstadoActaFase2Contrato.Descripcion;

                    }
                    else if ((int)EnumeratorPerfil.Tecnica == pPerfilId)
                        strEstadoActaFase2Contrato = EstadoActaFase2Contrato.Nombre;
                }

                TipoContratoCodigoContrato = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratacion.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_Contrato);
                if (TipoContratoCodigoContrato != null)
                    strTipoContratoCodigo = TipoContratoCodigoContrato.Nombre;

                //actaInicio.FechaAprobacionRequisitos = contratacion.FechaAprobacion.ToString("dd/MM/yyyy");
                //actaInicio.FechaAprobacionRequisitos = contratacion.FechaAprobacion != null ? Convert.ToDateTime(contratacion.FechaAprobacion).ToString("dd/MM/yyyy") : contratacion.FechaAprobacion.ToString();
                //actaInicio.FechaAprobacionRequisitos = item.FechaAprobacionRequisitos != null ? Convert.ToDateTime(item.FechaAprobacionRequisitos).ToString("dd/MM/yyyy") : item.FechaAprobacionRequisitos.ToString();
                actaInicio.FechaAprobacionRequisitos = item.FechaAprobacionRequisitosConstruccionSupervisor != null ? Convert.ToDateTime(item.FechaAprobacionRequisitosConstruccionSupervisor).ToString("dd/MM/yyyy") : item.FechaAprobacionRequisitosConstruccionSupervisor.ToString();
                lstActaInicio.Add(actaInicio);
            }

            return lstActaInicio;

        }

        public async Task GetDocumentoNoCargadoValue(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            //Contrato contrato;
           List<Contrato> lstContratos= _context.Contrato.Where(r=>r.RutaDocumento=="").ToList();
            int pContratoId;

            foreach (Contrato contrato in lstContratos)
            {
                //contrato = _context.Contrato.Where(r => r.ContratoId == contrato.ContratoId).FirstOrDefault();
                pContratoId = contrato.ContratoId;
                int pIdTemplate = (int)enumeratorTemplate.Dias2ObraOInterventoriaDocNoCargado;
                //perfilId = 8; //  Supervisor

                VistaGenerarActaInicioContrato actaInicio;

                //pTipoContrato = 2;
                actaInicio = await getDataActaInicioAsync(pContratoId, Convert.ToInt32(contrato.TipoContratoCodigo));

                //Contrato contrato = null;
                //contrato = _context.Contrato.Where(r => r.NumeroContrato == pActaInicio.NumeroContrato).FirstOrDefault();

                string correo = "cdaza@ivolucion.com";

                correo = getCorreos((int)EnumeratorPerfil.Supervisor);
                //_settings = _appSettingsService;
                //_appSettingsService = toAppSettingsService(_settings);
                //   Task<Respuesta> result = EnviarCorreoGestionActaIncio(correo, _settings.Value.MailServer,
                //_settings.Value.MailPort, _settings.Value.Password, _settings.Value.Sender,
                //actaInicio, pIdTemplate);

                bool blEnvioCorreo = false;
                Respuesta respuesta = new Respuesta();

                Template TemplateRecoveryPassword = await _commonService.GetTemplateById(pIdTemplate);

                string template = TemplateRecoveryPassword.Contenido;

                int tipoContrato = 0;
                
                //contrato = _context.Contrato.Where(r => r.NumeroContrato == actaInicio.NumeroContrato).FirstOrDefault();

                //tipoContrato = 2;
                tipoContrato = ConstanCodigoTipoContratacion.Interventoria;

                template = template.Replace("_Numero_Contrato_", actaInicio.NumeroContrato);
                template = template.Replace("_Fecha_Aprobacion_Poliza_", actaInicio.FechaAprobacionGarantiaPoliza);

                if (Convert.ToInt32(contrato.TipoContratoCodigo) == ConstanCodigoTipoContratacion.Interventoria)
                    template = template.Replace("_Obra_O_Interventoria_", "interventoría");
                else if (Convert.ToInt32(contrato.TipoContratoCodigo) == ConstanCodigoTipoContratacion.Obra)
                    template = template.Replace("_Obra_O_Interventoria_", "obra");

                template = template.Replace("_Cantidad_Proyectos_Asociados_", actaInicio.CantidadProyectosAsociados.ToString());

                template = template.Replace("_Fecha_Acta_Inicio_", actaInicio.FechaActaInicio);
                template = template.Replace("_Fecha_Prevista_Terminacion_", actaInicio.FechaPrevistaTerminacion);

                //datos basicos generales, aplican para los 4 mensajes

                //correo, _settings.Value.MailServer,
                //_settings.Value.MailPort, _settings.Value.Password, _settings.Value.Sender,
                //actaInicio, pIdTemplate
                //correo = "cdaza@ivolucion.com";
                //blEnvioCorreo = Helpers.Helpers.EnviarCorreo(correo, "Gestión Acta Inicio Fase II", template, _appSettingsService.Sender, _appSettingsService.Password, _appSettingsService.MailServer, _appSettingsService.MailPort);
                blEnvioCorreo = Helpers.Helpers.EnviarCorreo(correo, "Gestión Acta Inicio Fase II", template, pSender, pPassword, pMailServer, pMailPort);
                    
            }
        }

        public async Task<Respuesta> CambiarEstadoActa(int pContratoId, string pNuevoCodigoEstadoActa, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Acta_Inicio_Fase_2, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                Contrato contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();
                contrato.UsuarioModificacion = pUsuarioModifica;
                contrato.EstadoActaFase2 =  pNuevoCodigoEstadoActa.Trim();
                contrato.FechaModificacion= DateTime.Now;               

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantGestionarActaInicioFase2.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, ConstantGestionarActaInicioFase2.OperacionExitosa, idAccion, pUsuarioModifica, "CAMBIAR ESTADO ACTA INICIO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, ConstantGestionarActaInicioFase2.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }

        }

        public async Task<Respuesta> CambiarEstadoVerificacionActa(int pContratoId, string pNuevoCodigoEstadoVerificacionActa, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Verificacion_Acta_Inicio_Fase_2, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                Contrato contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();
                contrato.UsuarioModificacion = pUsuarioModifica;
                contrato.EstadoVerificacionConstruccionCodigo = pNuevoCodigoEstadoVerificacionActa;
                contrato.FechaModificacion = DateTime.Now;

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantGestionarActaInicioFase2.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, ConstantGestionarActaInicioFase2.OperacionExitosa, idAccion, pUsuarioModifica, "CAMBIAR ESTADO VERIFICACION ACTA INICIO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, ConstantGestionarActaInicioFase2.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }

        }

        //public async Task<VistaGenerarActaInicioContrato> GetListVistaGenerarActaInicio(int pContratoId )
        public async Task<VistaGenerarActaInicioContrato> GetListVistaGenerarActaInicio(int pContratoId)
        {
            VistaGenerarActaInicioContrato actaInicioConsolidado = new VistaGenerarActaInicioContrato();

            VistaGenerarActaInicioContrato actaInicioInterventoria = new VistaGenerarActaInicioContrato();
            VistaGenerarActaInicioContrato actaInicioObra = new VistaGenerarActaInicioContrato();
            int pTipoContrato =0;

            //               --Contratacion.TipoContratacionCodigo  14 DOM, tipoidentificacion
            
            //DOM 14 1   Obra            
            pTipoContrato = 1;
            actaInicioObra = await getDataActaInicioAsync(pContratoId, pTipoContrato);
            
            //DOM 14 2   Interventoría
            pTipoContrato = 2;
            actaInicioInterventoria = await getDataActaInicioAsync(pContratoId, pTipoContrato);

            actaInicioConsolidado = await GetDataConsolidadoActaInicioAsync(actaInicioObra, actaInicioInterventoria);
            
            //return actaInicioObra;
            return actaInicioConsolidado;

        }

        private async Task<VistaGenerarActaInicioContrato> GetDataConsolidadoActaInicioAsync(VistaGenerarActaInicioContrato actaInicioObra, VistaGenerarActaInicioContrato actaInicioInterventoria)
        {

            VistaGenerarActaInicioContrato actaInicioConsolidado = new VistaGenerarActaInicioContrato();

            if (actaInicioObra.NumeroDRP1 == "ERROR" && actaInicioInterventoria.NumeroDRP1 == "ERROR")            
            {
                return actaInicioConsolidado;
            }
                
            else if (actaInicioObra.NumeroDRP1 == "ERROR")
            {
                actaInicioConsolidado = actaInicioInterventoria;
            }
            else if (actaInicioInterventoria.NumeroDRP1 == "ERROR")
            {
                actaInicioConsolidado = actaInicioObra;
            }
            else
                {                    
                //interventoria
                actaInicioConsolidado.NumeroIdentificacionRepresentanteContratistaInterventoria = actaInicioInterventoria.NumeroIdentificacionRepresentanteContratistaInterventoria;// Contratista . numeroIdentificaionRepresentante
                actaInicioConsolidado.NombreRepresentanteContratistaInterventoria = actaInicioInterventoria.NombreRepresentanteContratistaInterventoria;

                //obra
                actaInicioConsolidado.NumeroIdentificacionRepresentanteContratistaObraInterventoria = actaInicioObra.NumeroIdentificacionRepresentanteContratistaObraInterventoria;
                actaInicioConsolidado.NombreRepresentanteContratistaObra = actaInicioObra.NombreRepresentanteContratistaObra;

                actaInicioConsolidado.NombreEntidadContratistaObra = actaInicioObra.NombreEntidadContratistaObra;
                actaInicioConsolidado.NombreEntidadContratistaSupervisorInterventoria = actaInicioObra.NombreEntidadContratistaSupervisorInterventoria;
                       
                actaInicioConsolidado.ValorFase1Preconstruccion = formatValor(actaInicioInterventoria.ValorFase1Preconstruccion) ;

                actaInicioConsolidado.Valorfase2ConstruccionObra = formatValor(actaInicioInterventoria.Valorfase2ConstruccionObra) ;
                     
                actaInicioConsolidado.ValorActualContrato = formatValor(actaInicioInterventoria.ValorFase1Preconstruccion) ;
                //actaInicioConsolidado.Valorfase2ConstruccionObra = actaInicioConsolidado.Valorfase2ConstruccionObra + actaInicioConsolidado.ValorFase1Preconstruccion;

                //    Proyecto   ????
                //             decimal? ValorObra 
                // decimal? ValorInterventoria 
                // decimal? ValorTotal 

                //ValorFase1Preconstruccion = " PENDIENTE",
                //        Valorfase2ConstruccionObra

            }

       

            return actaInicioConsolidado;

        }

        private async Task<VistaGenerarActaInicioContrato> getDataActaInicioAsync(int pContratoId, int pTipoContrato)
        {
            //VistaGenerarActaInicioContrato actaInicio = new VistaGenerarActaInicioContrato();
            VistaGenerarActaInicioContrato actaInicio ;
            try
            {
                //List <Contrato> ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado).Include(r => r.FechaFirmaContrato).Include(r => r.NumeroContrato).Include(r => r.Estado).Distinct().ToListAsync();
                //Contrato contrato = _context.Contrato.Where(r => !(bool)r.Eliminado && r.ContratoId == pContratoId && r.TipoContratoCodigo == pTipoContrato.ToString()).FirstOrDefault();
                Contrato contrato = _context.Contrato.Where(r => !(bool)r.Eliminado && r.ContratoId == pContratoId && r.Contratacion.TipoSolicitudCodigo == pTipoContrato.ToString()).FirstOrDefault();
                //cofinanciacion = _context.Cofinanciacion.Where(r => !(bool)r.Eliminado && r.CofinanciacionId == idCofinanciacion).FirstOrDefault();
                string strFechaPrevistaTerminacion = "";
                string strFechaActaInicio = "";

                DateTime FechaActaInicioFase1DateTime=new DateTime();
                DateTime FechaActaInicioFase2DateTime = new DateTime();

                DateTime FechaPrevistaTerminacionDateTime=new DateTime();                

                string strContratoObservacion = "";
                int ContratacionId = 0;

                Contratacion contratacion = null;

                //ContratoObservacion contratoObservacion = null;
                ConstruccionObservacion construccionObservacion = null;

                ContratoPoliza contratoPoliza=null;
                
                if (contrato != null)
                {
                    //contratacion = _context.Contratacion.Where(r => !(bool)r.Estado && r.ContratacionId == contrato.ContratacionId).FirstOrDefault();
                    contratacion = _context.Contratacion.Where(r => r.ContratacionId == contrato.ContratacionId).FirstOrDefault();
                    //strFechaActaInicio = contrato.FechaActaInicioFase2.ToString("dd/MM/yyyy");
                    ContratacionId= contrato.ContratacionId;
                    strFechaActaInicio = contrato.FechaActaInicioFase2 != null ? Convert.ToDateTime(contrato.FechaActaInicioFase2).ToString("dd/MM/yyyy") : contrato.FechaActaInicioFase2.ToString();
                    
                    FechaActaInicioFase1DateTime = Convert.ToDateTime(contrato.FechaActaInicioFase1);
                    FechaActaInicioFase2DateTime = Convert.ToDateTime(contrato.FechaActaInicioFase2);

                    FechaPrevistaTerminacionDateTime = Convert.ToDateTime(contrato.FechaTerminacionFase2);

                    strFechaPrevistaTerminacion = contrato.FechaTerminacionFase2 != null ? Convert.ToDateTime(contrato.FechaTerminacionFase2).ToString("dd/MM/yyyy") : contrato.FechaTerminacionFase2.ToString();

                    //contratoObservacion = _context.ContratoObservacion.Where(r => r.ContratoId == contrato.ContratoId).FirstOrDefault();
                    construccionObservacion =  await GetContratoObservacionByIdContratoId( contrato.ContratoId,false);

                    if (construccionObservacion != null)
                    {
                        strContratoObservacion = construccionObservacion.Observaciones;
                    }

                    //strContratoObservacion = contrato.Observaciones;

                    contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);

                }

                string strTipoContratacion = "sin definir";
                //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                Dominio TipoContratacionCodigo;

                ContratacionProyecto contratacionProyecto = null;
                string strLlaveMENContrato = "";
                string strInstitucionEducativaLlaveMEN = "";
                string strDepartamentoYMunicipioLlaveMEN = "";

                Contratista contratista=null;

                DisponibilidadPresupuestal disponibilidadPresupuestal=null;
                string proponenteCodigo = "";
                string proponenteNombre = "";

                decimal ValorActualContratoTmp, ValorFase1PreconstruccionTmp, Valorfase2ConstruccionObraTmp;
                ValorActualContratoTmp= ValorFase1PreconstruccionTmp= Valorfase2ConstruccionObraTmp=0;

                if (contratacion != null)
                {

                    //Vlrfase2ConstruccionObra = getSumVlrContratoComponente(Convert.ToInt32(actaInicio.ContratacionId)).ToString();
                    Valorfase2ConstruccionObraTmp = getSumVlrContratoComponente(Convert.ToInt32(contratacion.ContratacionId),
                  "2"  //ConstanCodigoFaseContrato.Preconstruccion.ToString()
                  );
                    ValorFase1PreconstruccionTmp = getSumVlrContratoComponente(Convert.ToInt32(contratacion.ContratacionId),
                 "1"  //ConstanCodigoFaseContrato.Preconstruccion.ToString()
                 );
                    ValorActualContratoTmp = Valorfase2ConstruccionObraTmp + ValorFase1PreconstruccionTmp;

                    TipoContratacionCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratacion.TipoContratacionCodigo, (int)EnumeratorTipoDominio.Opcion_por_contratar);
                    if (TipoContratacionCodigo != null)
                        strTipoContratacion = TipoContratacionCodigo.Nombre;

                    contratacionProyecto = _context.ContratacionProyecto.Where(r => r.ContratacionId == contratacion.ContratacionId).FirstOrDefault();

                    contratista = _context.Contratista.Where(r => (bool)r.Activo && r.ContratistaId == contratacion.ContratistaId).FirstOrDefault();

                    disponibilidadPresupuestal = _context.DisponibilidadPresupuestal.Where(r => r.ContratacionId == contratacion.ContratacionId).FirstOrDefault();
                      
                    if (contrato.Contratacion.TipoSolicitudCodigo == ((int)ConstanCodigoTipoContratacion.Obra).ToString())
                    {
                        proponenteCodigo = ((int)ConstanCodigoTipoContratacion.Obra).ToString();
                        proponenteNombre = "Obra";
                    }                        
                    else if (contrato.Contratacion.TipoSolicitudCodigo == ((int)ConstanCodigoTipoContratacion.Interventoria).ToString())
                    {
                        proponenteCodigo = ((int)ConstanCodigoTipoContratacion.Interventoria).ToString();
                        proponenteNombre = "Obra";

                    }
                        
                }                            
                
                string strTipoIdentificacionCodigoContratista = "sin definir";
                Dominio TipoIdentificacionCodigoContratista;

                //30  Tipo Documento
                //        tipoIdentificacionCodigo - contratista                   

                if (contratista != null)
                {
                    TipoIdentificacionCodigoContratista = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratista.TipoIdentificacionCodigo, (int)EnumeratorTipoDominio.Tipo_Documento);
                    if (TipoIdentificacionCodigoContratista != null)
                        strTipoIdentificacionCodigoContratista = TipoIdentificacionCodigoContratista.Nombre;
                                        
                }
                //contratista obra
                //TipoContratoCodigo 12,37
                //1   Obra                              

                //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                string strTipoSolicitudCodigoContratoPoliza = "sin definir";
                string strEstadoSolicitudCodigoContratoPoliza = "sin definir";

                string strVigencia = "sin definir";

                //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                Dominio TipoSolicitudCodigoContratoPoliza;
                Dominio EstadoSolicitudCodigoContratoPoliza;

                if (contratoPoliza != null)
                {
                    TipoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_Modificacion_Contrato_Poliza);
                    if (TipoSolicitudCodigoContratoPoliza != null)
                        strTipoSolicitudCodigoContratoPoliza = TipoSolicitudCodigoContratoPoliza.Nombre;

                    EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.EstadoPolizaCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                    if (EstadoSolicitudCodigoContratoPoliza != null)
                        strEstadoSolicitudCodigoContratoPoliza = EstadoSolicitudCodigoContratoPoliza.Nombre;

                    //strVigencia = contratoPoliza.Vigencia.ToString();                    

                        //strVigencia = contratoPoliza.Vigencia != null ? Convert.ToDateTime(contratoPoliza.Vigencia).ToString("yyyy") : contratoPoliza.Vigencia.ToString();

                    strVigencia = contrato.FechaTramite != null ? Convert.ToDateTime(contrato.FechaTramite).ToString("dd/MM/yyyy") : contrato.FechaTramite.ToString();
                }
               
                string strNumeroDRP1 = "sin definir";
                string strFechaGeneracionDRP = "sin definir";
                string strObjetoDisponibilidadPresupuestal="";

                string strFecSolicitudDisponibPresup = "";

                int PlazoDias=0, PlazoMeses=0;

                if (disponibilidadPresupuestal != null)
                {
                    strNumeroDRP1 = disponibilidadPresupuestal.NumeroDrp;
                    //strFechaGeneracionDRP = disponibilidadPresupuestal.FechaDdp.ToString("dd/MM/yyyy");
                    strFechaGeneracionDRP = disponibilidadPresupuestal.FechaDdp != null ? Convert.ToDateTime(disponibilidadPresupuestal.FechaDdp).ToString("dd/MM/yyyy") : disponibilidadPresupuestal.FechaDdp.ToString();

                    strObjetoDisponibilidadPresupuestal = disponibilidadPresupuestal.Objeto;

                    strFecSolicitudDisponibPresup = disponibilidadPresupuestal.FechaSolicitud.ToString("dd/MM/yyyy");

                    PlazoDias = Convert.ToInt32( disponibilidadPresupuestal.PlazoDias);
                    PlazoMeses = Convert.ToInt32(disponibilidadPresupuestal.PlazoMeses);
                }
                //disponibilidadPresupuestal = _context.DisponibilidadPresupuestal.Where(r => r.ContratacionId == contratacion.ContratacionId).FirstOrDefault();
                //                [DisponibilidadPresupuestalProyecto] - [ProyectoId]
                //[ProyectoRequisitoTecnico][ProyectoId]


                //DisponibilidadPresupuestalProyecto disponibilidadPresupuestalProyecto
                // disponibilidadPresupuestalProyecto= _context.DisponibilidadPresupuestalProyecto.Where(r => r.ProyectoId == disponibilidadPresupuestal.id).FirstOrDefault();

                //contratacion.id

                Int32 intCantidadProyectosAsociados = 0;

                Proyecto proyecto=null;

                if (contratacionProyecto != null)
                {
                    intCantidadProyectosAsociados = _context.Proyecto.Where(r => r.ProyectoId == contratacionProyecto.ProyectoId).Count();
                    proyecto = _context.Proyecto.Where(r => r.ProyectoId == contratacionProyecto.ProyectoId).FirstOrDefault();
                }

                string InstitucionEducativaId = null;
                int InstitucionEducativaIdInt = 0;

                if (proyecto != null)
                {
                     InstitucionEducativaId = proyecto.InstitucionEducativaId.ToString();
                     InstitucionEducativaIdInt = Convert.ToInt32(proyecto.InstitucionEducativaId);
                }

                if (InstitucionEducativaId != null)
                    proyecto.InstitucionEducativa = await _commonService.GetInstitucionEducativaById( Convert.ToInt32( InstitucionEducativaId));
                //proyecto.InstitucionEducativa = await _commonService.GetInstitucionEducativaById(InstitucionEducativaIdInt);

                if (proyecto!= null)
                {
                     strLlaveMENContrato =proyecto.LlaveMen;
                    if (InstitucionEducativaId != null)
                        strInstitucionEducativaLlaveMEN = proyecto.InstitucionEducativa.Nombre;
                    //[LocalizacionIdMunicipio] [InstitucionEducativaId]
                    strDepartamentoYMunicipioLlaveMEN =_commonService.GetNombreDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    
                }
                //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                //VistaGenerarActaInicioContrato

                string strValor="";
                if (contrato != null)
                {
                    strValor = contrato.Valor.ToString();
                    if(strValor.Length>3)
                        strValor = strValor.Remove(strValor.Length - 3);       

                }
                actaInicio = new VistaGenerarActaInicioContrato();
                actaInicio.NumeroDRP1 = "ERROR";

                


                if (contrato != null)
                    actaInicio = new VistaGenerarActaInicioContrato
                    {
                        //FechaAprobacionRequisitos="[FechaAprobacionRequisitos] [contrato] FechaAprobacionRequisitos",
                        NumeroContrato = contrato.NumeroContrato,
                        VigenciaContrato = strVigencia, // "2021 PENDIENTE",
                        FechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString(),
                        ProponenteCodigo= proponenteCodigo,
                        ProponenteNombre= proponenteNombre,
                        NumeroDRP1 = strNumeroDRP1,

                        FechaGeneracionDRP1 = strFechaGeneracionDRP,

                        NumeroDRP2 = strNumeroDRP1,
                        FechaGeneracionDRP2 = strFechaGeneracionDRP,
                        //FechaAprobacionGarantiaPoliza = contratoPoliza.FechaAprobacion.ToString("dd/MM/yyyy"),
                        FechaAprobacionGarantiaPoliza = contratoPoliza.FechaAprobacion != null ? Convert.ToDateTime(contratoPoliza.FechaAprobacion).ToString("dd/MM/yyyy") : contratoPoliza.FechaAprobacion.ToString(),
                        FechaAprobacionRequisitosSupervisor = contrato.FechaAprobacionRequisitosSupervisor != null ? Convert.ToDateTime(contrato.FechaAprobacionRequisitosSupervisor).ToString("dd/MM/yyyy") : contrato.FechaAprobacionRequisitosSupervisor.ToString(),

                        Objeto = strObjetoDisponibilidadPresupuestal,
                        ValorInicialContrato = strValor,                         
                         ValorActualContrato = ValorActualContratoTmp.ToString(),
                        ValorFase1Preconstruccion = ValorFase1PreconstruccionTmp.ToString(),
                        Valorfase2ConstruccionObra = Valorfase2ConstruccionObraTmp.ToString(),
                        //PlazoInicialContratoSupervisor = contrato.Plazo.ToString(),

                        //PlazoInicialContratoSupervisor = contrato.Plazo != null ? Convert.ToDateTime(contrato.Plazo).ToString("dd/MM/yyyy") : contrato.Plazo.ToString(),
                        //PlazoInicialContratoSupervisor = contrato.FechaAprobacionRequisitosSupervisor != null ? Convert.ToDateTime(contrato.FechaAprobacionRequisitosSupervisor).ToString("dd/MM/yyyy") : contrato.FechaAprobacionRequisitosSupervisor.ToString(),
                        PlazoInicialContratoSupervisor = strFecSolicitudDisponibPresup,

                        PlazoFase1PreMeses = contrato.PlazoFase1PreMeses,
                        PlazoFase2ConstruccionDias = contrato.PlazoFase2ConstruccionDias,

                        PlazoFase1PreDias = contrato.PlazoFase1PreDias,
                        PlazoFase2ConstruccionMeses = contrato.PlazoFase2ConstruccionMeses,

                        PlazoActualContratoMeses= PlazoMeses,
                        PlazoActualContratoDias = PlazoDias,

                        NombreEntidadContratistaObra = contratista.Nombre,
                        NombreEntidadContratistaSupervisorInterventoria = contratista.Nombre,

                        FechaActaInicio = strFechaActaInicio,
                        FechaActaInicioFase1DateTime = FechaActaInicioFase1DateTime,
                        FechaActaInicioFase2DateTime = FechaActaInicioFase2DateTime,

                        FechaPrevistaTerminacion = strFechaPrevistaTerminacion,
                        FechaPrevistaTerminacionDateTime = FechaPrevistaTerminacionDateTime,                       

                        ObservacionOConsideracionesEspeciales = strContratoObservacion,

                        LlaveMENContrato = strLlaveMENContrato,
                        DepartamentoYMunicipioLlaveMEN = strDepartamentoYMunicipioLlaveMEN,
                        InstitucionEducativaLlaveMEN = strInstitucionEducativaLlaveMEN,

                        CantidadProyectosAsociados = intCantidadProyectosAsociados,
                        NumeroIdentificacionRepresentanteContratistaInterventoria = contratista.RepresentanteLegalNumeroIdentificacion,

                        //RegistroCompleto = contrato.RegistroCompleto

                        //,EstadoRegistro = "COMPLETO"
                    };

                //if (!(bool)proyecto.RegistroCompleto)
                //{
                //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                //}

            }
            catch (Exception e)
            {
                //VistaGenerarActaInicioContrato actaInicio = new VistaGenerarActaInicioContrato
                 actaInicio = new VistaGenerarActaInicioContrato
                {
                     NumeroContrato = e.ToString(),
                     VigenciaContrato = e.ToString(), // "2021 PENDIENTE",
                     FechaFirmaContrato = e.InnerException.ToString(),
                     NumeroDRP1 =  "ERROR",
                     //,
                     //RegistroCompleto = false

                 };
            }
            return actaInicio;
        }     
                
    }
    }
