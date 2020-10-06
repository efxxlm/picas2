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

namespace asivamosffie.services
{
   public  class ActBeginService
    {


        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        private readonly IOptions<AppSettingsService> _settings;

        private readonly IDocumentService _documentService;




        

        public ActBeginService(devAsiVamosFFIEContext context, ICommonService commonService, IOptions<AppSettingsService> settings, IDocumentService documentService)
        {

            _commonService = commonService;
            _context = context;
            _settings = settings;
        _documentService = documentService;
    }

        async Task<Respuesta> GuardarTieneObservacionesActaInicio(int pContratoId, string pObservacionesActa, string pUsuarioModificacion)
        {
            Respuesta _response = new Respuesta();

            //            ConObervacionesActa - Contrato
            Contrato contrato;
            contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId && r.Estado == true).FirstOrDefault();

            int idAccionCrearActaInicio = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesActaInicio.EditadoCorrrectamente, (int)EnumeratorTipoDominio.Acciones);

            try
            {

                contrato.FechaModificacion = DateTime.Now;
                contrato.UsuarioModificacion = pUsuarioModificacion;
                contrato.ConObervacionesActa = true;
                contrato.Observaciones = pObservacionesActa;

                //      CAMBIAR ESTADO “Sin acta generada” a “Con acta generada”.
                //DOM 60  1   Sin acta generada
                //DOM 60  3   Con acta generada
                //contrato.EstadoActa = "3";

                _context.Contrato.Update(contrato);

                            return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesActaInicio.EditadoCorrrectamente,
                    Message =
                    await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias,
                    ConstantMessagesActaInicio.EditadoCorrrectamente, idAccionCrearActaInicio
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


        async Task<Respuesta> GuardarPlazoEjecucionFase2Construccion(int pContratoId, int pPlazoFase2PreMeses, int pPlazoFase2PreDias, string pObservacionesConsideracionesEspeciales, string pUsuarioModificacion)
        {
            Respuesta _response = new Respuesta();

            //            ConObervacionesActa - Contrato
            Contrato contrato;
            contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId && r.Estado == true).FirstOrDefault();

            ContratoObservacion contratoObservacion= new ContratoObservacion();

            int idAccionCrearActaInicio = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesActaInicio.EditadoCorrrectamente, (int)EnumeratorTipoDominio.Acciones);

            try
            {

                //contrato.FechaModificacion = DateTime.Now;
                //contrato.UsuarioModificacion = pUsuarioModificacion;
                //contrato.ConObervacionesActa = true;
                //contrato.Observaciones = pObservacionesConsideracionesEspeciales;

                contratoObservacion.Observaciones=pObservacionesConsideracionesEspeciales;
                contratoObservacion.ContratoId = contrato.ContratoId;
                contratoObservacion.EsActaFase2 = true;
                contratoObservacion.EsActa = true;

                contratoObservacion.FechaCreacion = DateTime.Now;
                contratoObservacion.UsuarioModificacion = pUsuarioModificacion;

                //      CAMBIAR ESTADO “Sin acta generada” a “Con acta generada”.
                //DOM 60  1   Sin acta generada
                //DOM 60  3   Con acta generada
                //contrato.EstadoActa = "3";                

                _context.Add(contratoObservacion);
                await _context.SaveChangesAsync();


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
               Code = ConstantMessagesActaInicio.EditadoCorrrectamente,
               Message =
               await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias,
               ConstantMessagesActaInicio.EditadoCorrrectamente, idAccionCrearActaInicio
               , contrato.UsuarioModificacion, " GUARDAR OBSERVACION CONTRATO ACTA"
               )
           };

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesActaInicio.ErrorInterno, Message = ex.InnerException.ToString().Substring(0, 500) };
            }
        }


        public async Task<Respuesta> GuardarCargarActaSuscritaContrato(int pContratoId, DateTime pFechaFirmaContratista, DateTime pFechaFirmaActaContratistaInterventoria
            /* archivo pdf */ , IFormFile pFile, string pDirectorioBase, string pDirectorioActaInicio, string pUsuarioModificacion
            )
        {
            //            Fecha de la firma del documento por parte del contratista de obra -FechaFirmaContratista - contrato
            //Fecha de la firma del documento por parte del contratista de interventoría -FechaFirmaActaContratistaInterventoria - contrato
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Registrar_Tramite_Contratacion, (int)EnumeratorTipoDominio.Acciones);

            Contrato contrato;
            contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId && r.Estado == true).FirstOrDefault();

            try
            {

                contrato.FechaFirmaContratista = pFechaFirmaContratista;
                contrato.FechaFirmaActaContratistaInterventoriaFase2 = pFechaFirmaActaContratistaInterventoria;
                contrato.UsuarioModificacion = pUsuarioModificacion;


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

                contrato.RutaActaSuscrita = strFilePatch + "//" + pFile.FileName;
                //contratacionOld.RegistroCompleto = ValidarCamposContratacion(contratacionOld);

                return
                 new Respuesta
                 {
                     IsSuccessful = true,
                     IsException = false,
                     IsValidation = false,
                     Code = ConstantGestionarProcesosContractuales.OperacionExitosa,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantGestionarProcesosContractuales.OperacionExitosa, idAccion, contrato.UsuarioModificacion, "CARGAR ACTA SUSCRITA CONTRATO")
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
                  Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantGestionarProcesosContractuales.Error, idAccion, contrato.UsuarioModificacion, ex.InnerException.ToString())
              };
            }
        }


public void replaceTags()
        {
            string str="";
            string valor="";

            str = str.Replace("_Numero_Identificacion_Entidad_Contratista_Interventoria_", valor);
            str = str.Replace("_Nombre_Representante_Contratista_Interventoria_", valor);
            str = str.Replace("_Nombre_Entidad_Contratista_Interventoria_", valor);
            str = str.Replace("_Nombre_Representante_Contratista_Obra_", valor);
            str = str.Replace("_Nombre_Entidad_Contratista_Obra _", valor);
            str = str.Replace("_Numero_Identificacion_Entidad_Contratista_Obra_", valor);
            str = str.Replace(" _Fecha_Prevista_Terminacion_", valor);
            str = str.Replace("_OBSERVACION_O_CONSIDERACIONES_ESPECIALES_", valor);
            str = str.Replace("_Plazo_Ejecucion_Fase_1_Preconstruccion_", valor);
            str = str.Replace("_Plazo_Ejecucion_Fase_2_Construccion_", valor);
            str = str.Replace("_Valor_Actual_Contrato_", valor);
            str = str.Replace("_Plazo_Inicial_Contrato_", valor);
            str = str.Replace("_Valor_Fase_1_preconstruccion_", valor);
            str = str.Replace("_Valor_Fase_2_Construccion_Obra_", valor);
            str = str.Replace("_Nombre_Entidad_Contratista_Obra_", valor);
            str = str.Replace("_Valor_Inicial_Contrato_", valor);
            str = str.Replace("_Fecha_Aprobacion_Garantia_Poliza_", valor);
            str = str.Replace("_Objeto_", valor);
            str = str.Replace("_Numero_DRP_", valor);
            str = str.Replace("_Fecha_Generación_DRP_", valor);
            str = str.Replace("_Institucion_Educativa_Llave_MEN_", valor);
            str = str.Replace("_Departamento_y_Municipio_Llave_MEN_", valor);
            str = str.Replace("_Fecha_Acta_Inicio_", valor);
            str = str.Replace("_Llave_MEN_Contrato_", valor);
            str = str.Replace("_Numero_Identificacion_Entidad_Contratista_Obra_", valor);
            str = str.Replace("_Numero_Contrato_Obra_ ", valor);
            str = str.Replace("_Nombre_Entidad_Contratista_Obra_", valor);
            str = str.Replace("_Numero_Identificacion_Contratista_Interventoria_", valor);
            str = str.Replace("_Nombre_Representante_Contratista_Obra_", valor);
            str = str.Replace("_Nombre_Representante_Contratista_Interventoria_", valor);
            str = str.Replace("_Nombre_Entidad_Contratista_Interventoria_", valor);
            str = str.Replace("_Fecha_Acta_Inicio_", valor);
            str = str.Replace("_Numero_Contrato_Obra_", valor);
            str = str.Replace("", valor);



        }

        public async Task<VistaGenerarActaInicioContrato> GetVistaGenerarActaInicio(int pContratoId )
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

            return actaInicioConsolidado;

        }

        private async Task<VistaGenerarActaInicioContrato> GetDataConsolidadoActaInicioAsync(VistaGenerarActaInicioContrato actaInicioObra, VistaGenerarActaInicioContrato actaInicioInterventoria)
        {

            VistaGenerarActaInicioContrato actaInicioConsolidado = new VistaGenerarActaInicioContrato();

            //interventoria
            actaInicioConsolidado.NumeroIdentificacionRepresentanteContratistaInterventoria = "";// Contratista . numeroIdentificaionRepresentante
            actaInicioConsolidado.NombreRepresentanteContratistaInterventoria = actaInicioInterventoria.NombreRepresentanteContratistaInterventoria;

            //obra
            actaInicioConsolidado.NumeroIdentificacionRepresentanteContratistaObraInterventoria = actaInicioObra.NumeroIdentificacionRepresentanteContratistaObraInterventoria;
            actaInicioConsolidado.NombreRepresentanteContratistaObra = actaInicioObra.NombreRepresentanteContratistaObra;

            actaInicioConsolidado.NombreEntidadContratistaObra = actaInicioObra.NombreEntidadContratistaObra;
            actaInicioConsolidado.NombreEntidadContratistaSupervisorInterventoria = actaInicioObra.NombreEntidadContratistaSupervisorInterventoria;                

            return actaInicioConsolidado;

        }

        private async Task<VistaGenerarActaInicioContrato> getDataActaInicioAsync(int pContratoId, int pTipoContrato)
        {
            //VistaGenerarActaInicioContrato actaInicio = new VistaGenerarActaInicioContrato();
            VistaGenerarActaInicioContrato actaInicio ;
            try
            {
                //List <Contrato> ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado).Include(r => r.FechaFirmaContrato).Include(r => r.NumeroContrato).Include(r => r.Estado).Distinct().ToListAsync();
                Contrato contrato = _context.Contrato.Where(r => !(bool)r.Estado && r.ContratoId == pContratoId && r.TipoContratoCodigo==pTipoContrato.ToString()).FirstOrDefault();
                //cofinanciacion = _context.Cofinanciacion.Where(r => !(bool)r.Eliminado && r.CofinanciacionId == idCofinanciacion).FirstOrDefault();

                Contratacion contratacion;
                contratacion = _context.Contratacion.Where(r => !(bool)r.Estado && r.ContratacionId == contrato.ContratacionId).FirstOrDefault();

                string strTipoContratacion = "sin definir";
                //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                Dominio TipoContratacionCodigo;

                if (contratacion != null)
                {
                    TipoContratacionCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratacion.TipoContratacionCodigo, (int)EnumeratorTipoDominio.Opcion_por_contratar);
                    if (TipoContratacionCodigo != null)
                        strTipoContratacion = TipoContratacionCodigo.Nombre;

                }                      

                Contratista contratista;
                contratista = _context.Contratista.Where(r => (bool)r.Activo && r.ContratistaId == contratacion.ContratistaId).FirstOrDefault();

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


                ContratoPoliza contratoPoliza;
                contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);

                //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                string strTipoSolicitudCodigoContratoPoliza = "sin definir";
                string strEstadoSolicitudCodigoContratoPoliza = "sin definir";

                //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                Dominio TipoSolicitudCodigoContratoPoliza;
                Dominio EstadoSolicitudCodigoContratoPoliza;

                if (contratoPoliza != null)
                {
                    TipoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_Modificacion_Contrato_Poliza);
                    if (TipoSolicitudCodigoContratoPoliza != null)
                        strTipoSolicitudCodigoContratoPoliza = TipoSolicitudCodigoContratoPoliza.Nombre;

                    EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                    if (EstadoSolicitudCodigoContratoPoliza != null)
                        strEstadoSolicitudCodigoContratoPoliza = EstadoSolicitudCodigoContratoPoliza.Nombre;

                }

                //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                //VistaGenerarActaInicioContrato
                actaInicio = new VistaGenerarActaInicioContrato
                {
                    //FechaAprobacionRequisitos="[FechaAprobacionRequisitos] [contrato] FechaAprobacionRequisitos",
                    NumeroContrato = contrato.NumeroContrato,
                    VigenciaContrato = "2021 PENDIENTE",
                    FechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString(),
                    
                    NumeroDRP1 = "DisponibilidadPresupuestal - NumeroDrp PENDIENTE",

                    FechaGeneracionDRP1 = "2021 PENDIENTE",

                    NumeroDRP2 = "DisponibilidadPresupuestal - NumeroDrp PENDIENTE",
                    FechaGeneracionDRP2 = " PENDIENTE",
                    FechaAprobacionGarantiaPoliza = contratoPoliza.FechaAprobacion.ToString("dd/MM/yyyy"),
                    Objeto = contrato.Objeto,
                    ValorInicialContrato = contrato.Valor.ToString(),
                    ValorActualContrato = " PENDIENTE",
                    ValorFase1Preconstruccion = " PENDIENTE",
                    Valorfase2ConstruccionObra = " PENDIENTE",
                    PlazoInicialContratoSupervisor = contrato.Plazo.ToString(),

                    NombreEntidadContratistaObra = contratista.Nombre,
                    NombreEntidadContratistaSupervisorInterventoria = contratista.Nombre








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

                    //ContratoId = contrato.ContratoId,
                    //FechaFirma = e.ToString(),
                    //NumeroContrato = e.InnerException.ToString(),

                    //TipoSolicitud = "ERROR"
                    //,
                    //RegistroCompleto = false

                };
            }
            return actaInicio;
        }
    }
    }
