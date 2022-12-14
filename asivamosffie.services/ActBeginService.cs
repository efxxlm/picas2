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
using System.Globalization;

namespace asivamosffie.services
{
    public class ActBeginService : IActBeginService
    {
        /// Querido colega Programador:
        /// Cuando camilo escribio este código, sólo Dios y el
        /// Sabian como funcionaba.
        /// Ahora, ¡sólo Dios lo sabe!
        /// 
        /// Así que si esta tratando de 'Optimizarlo'
        /// por favor, incremente el siguiente contador
        /// como una advertencia 
        /// para el siguiente colega:
        /// 
        /// total_horas_perdidas_aqui = 2

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

        public async Task<byte[]> GetPlantillaActaInicio(int pContratoId)
        {
            if (pContratoId == 0)
            {
                return Array.Empty<byte>();
            }

            VistaGenerarActaInicioContrato actaInicio;

            int pTipoContrato = Int32.Parse(ConstanCodigoTipoContrato.Interventoria);

            ////DOM 14 1   Obra            
            //pTipoContrato = 1;

            //DOM 14 2   interventoria            
            pTipoContrato = ConstanCodigoTipoContratacion.Interventoria;
            actaInicio = await getDataActaInicioAsync(pContratoId);

            string Vlrfase2ConstruccionObra = "", VlrFase1Preconstruccion = "";
            VlrFase1Preconstruccion = actaInicio.ValorFase1Preconstruccion;

            if (actaInicio.ContratacionId != null)
            {
                VlrFase1Preconstruccion = getSumVlrContratoComponente(Convert.ToInt32(actaInicio.ContratacionId),
                    "1"  //ConstanCodigoFaseContrato.Preconstruccion.ToString()
                    ).ToString();

            }

            pTipoContrato = ConstanCodigoTipoContratacion.Obra;
            if (actaInicio.NumeroDRP1 == "ERROR")
            {
                actaInicio = await getDataActaInicioAsync(pContratoId);
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

            Contrato contrato = null;
            contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

            Contratacion contratacion = null;
            if (contrato != null)
                contratacion = _context.Contratacion.Where(r => r.ContratacionId == contrato.ContratacionId).FirstOrDefault();

            Plantilla plantilla = null;

            if (contratacion.TipoSolicitudCodigo == ((int)ConstanCodigoTipoContratacion.Interventoria).ToString())
            //if (contrato.TipoContratoCodigo == ((int)ConstanCodigoTipoContratacion.Interventoria).ToString())
            {
                pTipoContrato = ConstanCodigoTipoContratacion.Interventoria;
                plantilla = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Acta_inicio_interventoria_PDF).ToString()).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();

            }

            //else if (contrato.TipoContratoCodigo == ((int)ConstanCodigoTipoContratacion.Obra).ToString())
            else if (contratacion.TipoSolicitudCodigo == ((int)ConstanCodigoTipoContratacion.Obra).ToString())
            {
                pTipoContrato = ConstanCodigoTipoContratacion.Obra;
                plantilla = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Acta_inicio_obra_PDF).ToString()).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();

            }
            if (plantilla != null)
                plantilla.Contenido = await ReemplazarDatosPlantillaActaInicio(plantilla.Contenido, actaInicio, "cdaza");
            return ConvertirPDF(plantilla);
        }

        private async Task<string> ReemplazarDatosPlantillaActaInicio(string strContenido, VistaGenerarActaInicioContrato pActaInicio, string usuario)
        {
            strContenido = strContenido.Replace("_Numero_Identificacion_Entidad_Contratista_Interventoria_", pActaInicio.NumeroIdentificacionEntidadContratistaObra);
            strContenido = strContenido.Replace("_Nombre_Representante_Contratista_Interventoria_", pActaInicio.NombreRepresentanteContratistaInterventoria);
            strContenido = strContenido.Replace("_Nombre_Entidad_Contratista_Interventoria_", pActaInicio.NombreEntidadContratistaSupervisorInterventoria);  //PENDIENTE

            strContenido = strContenido.Replace("_Nombre_Representante_Contratista_Obra_", pActaInicio.NombreRepresentanteContratistaObra);
            strContenido = strContenido.Replace("_Nombre_Representante_Contratista_Interventoria_", pActaInicio.NombreRepresentanteContratistaObra);

            strContenido = strContenido.Replace("_Nombre_Entidad_Contratista_Obra_", pActaInicio.NombreEntidadContratistaObra);
            strContenido = strContenido.Replace("_Nombre_Entidad_Contratista_Interventoria_", pActaInicio.NombreEntidadContratistaObra);

            strContenido = strContenido.Replace("_Numero_Identificacion_Entidad_Contratista_Obra_", pActaInicio.NumeroIdentificacionEntidadContratistaObra);
            strContenido = strContenido.Replace(" _Fecha_Prevista_Terminacion_", pActaInicio.FechaPrevistaTerminacion);
            strContenido = strContenido.Replace("_OBSERVACION_O_CONSIDERACIONES_ESPECIALES_", pActaInicio.ObservacionOConsideracionesEspeciales);
            strContenido = strContenido.Replace("_Plazo_Ejecucion_Fase_1_Preconstruccion_", pActaInicio.PlazoFase1PreMeses.ToString());
            strContenido = strContenido.Replace("_Plazo_Ejecucion_Fase_2_Construccion_", pActaInicio.PlazoFase2ConstruccionMeses.ToString());

            strContenido = strContenido.Replace("_Valor_Actual_Contrato_", formatValor(pActaInicio.ValorActualContrato));

            strContenido = strContenido.Replace("_Plazo_Inicial_Contrato_", formatValor(pActaInicio.PlazoInicialContratoSupervisor));
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

            Perfil perfil = null;

            if (UsuarioPerfil != null)
            {
                perfil = _context.Perfil.Where(y => y.PerfilId == UsuarioPerfil.PerfilId).FirstOrDefault();

            }
            if (UsuarioPerfil != null)
            {
                strContenido = strContenido.Replace("_Cargo_Usuario_", perfil.Nombre);
            }
            strContenido = strContenido.Replace("_Nombre_Supervisor_", "_Nombre_Supervisor_");
            strContenido = strContenido.Replace("_Numero_Identificacion_Supervisor_", pActaInicio.NumeroIdentificacionSupervisor);

            return strContenido;

        }

        public async Task<Respuesta> GuardarTieneObservacionesActaInicio(int pContratoId, string pObservacionesActa, string pUsuarioModificacion, bool pEsSupervisor, bool pEsActa)
        {
            Respuesta _response = new Respuesta();

            Contrato contrato;

            contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

            int idAccionCrearActaInicio = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Editar_Tiene_Observaciones_Acta_Inicio_Fase2, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                contrato.FechaModificacion = DateTime.Now;
                contrato.UsuarioModificacion = pUsuarioModificacion;
                contrato.ConObervacionesActa = true;

                ConstruccionObservacion construccionObservacion = new ConstruccionObservacion();

                //                6   Acta de inicio interventor
                //7   Acta de inicio supervisor
                construccionObservacion.Observaciones = pObservacionesActa;

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

                _context.ConstruccionObservacion.Add(construccionObservacion);

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
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesActaInicio.ErrorInterno, Message = ex.InnerException.ToString().Substring(0, 500) };
            }
        }

        public async Task<Respuesta> CreateEditObservacionesActaInicioConstruccion(ContratoConstruccion pContratoConstruccion, string pUsuarioCreacion)
        {
            Respuesta _response = new Respuesta();

            Contrato contrato = _context.Contrato.Find(pContratoConstruccion.ContratoId);

            int idAccionCrearActaInicio = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Editar_Tiene_Observaciones_Acta_Inicio_Fase2, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                contrato.FechaModificacion = DateTime.Now;
                contrato.UsuarioModificacion = pUsuarioCreacion;
                contrato.ConObervacionesActa = true;

                ConstruccionObservacion construccionObservacion = new ConstruccionObservacion();

                if (pContratoConstruccion.ConstruccionObservacion.FirstOrDefault().EsSupervision == true)
                    construccionObservacion.TipoObservacionConstruccion = "7"; //Acta de inicio supervisor
                else
                    construccionObservacion.TipoObservacionConstruccion = "6"; //Acta de inicio interventor

                if (pContratoConstruccion.ConstruccionObservacion.FirstOrDefault().ConstruccionObservacionId == 0 || string.IsNullOrEmpty(pContratoConstruccion.ConstruccionObservacion.FirstOrDefault().ConstruccionObservacionId.ToString()))
                {
                    ConstruccionObservacion observacion = pContratoConstruccion.ConstruccionObservacion.FirstOrDefault();

                    observacion.TipoObservacionConstruccion = construccionObservacion.TipoObservacionConstruccion;

                    observacion.UsuarioCreacion = pUsuarioCreacion;
                    observacion.FechaCreacion = DateTime.Now;

                    _context.ConstruccionObservacion.Add(observacion);
                }
                else
                {
                    ConstruccionObservacion observacion = _context.ConstruccionObservacion.Find(pContratoConstruccion.ConstruccionObservacion.FirstOrDefault().ConstruccionObservacionId);

                    observacion.Observaciones = pContratoConstruccion.ConstruccionObservacion.FirstOrDefault().Observaciones;
                }

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
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesActaInicio.ErrorInterno, Message = ex.InnerException.ToString().Substring(0, 500) };
            }
        }

        public async Task<Respuesta> GuardarPlazoEjecucionFase2Construccion(int pContratoId, int pPlazoFase2PreMeses, int pPlazoFase2PreDias, string pObservacionesConsideracionesEspeciales, string pUsuarioModificacion, DateTime pFechaActaInicioFase1, DateTime pFechaTerminacionFase2, bool pEsSupervisor, bool pEsActa)
        {
            Respuesta _response = new Respuesta();

            Contrato contrato;
            contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

            ContratoObservacion contratoObservacion = new ContratoObservacion();

            int idAccionCrearActaInicio = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Editar_Plazo_Ejecucion_Fase_2, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                contrato.PlazoFase2ConstruccionDias = pPlazoFase2PreDias;
                contrato.PlazoFase2ConstruccionMeses = pPlazoFase2PreMeses;

                if (contrato.TipoContratoCodigo == ((int)ConstanCodigoTipoContratacion.Obra).ToString())
                    contrato.EstadoActaFase2 = "14"; //14  Con acta preliminar generada
                else if (contrato.TipoContratoCodigo == ((int)ConstanCodigoTipoContratacion.Interventoria).ToString())
                    contrato.EstadoActaFase2 = "2";       //2   Con acta preliminar generada

                contrato.FechaActaInicioFase2 = pFechaActaInicioFase1;
                contrato.FechaTerminacionFase2 = pFechaTerminacionFase2;

                ConstruccionObservacion construccionObservacion = new ConstruccionObservacion();

                //                6   Acta de inicio interventor
                //7   Acta de inicio supervisor
                construccionObservacion.Observaciones = pObservacionesConsideracionesEspeciales;

                if (pEsSupervisor)
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

                _context.ConstruccionObservacion.Add(construccionObservacion);

                _context.Contrato.Update(contrato);

                //      CAMBIAR ESTADO “Sin acta generada” a “Con acta generada”.
                //DOM 60  1   Sin acta generada
                //DOM 60  3   Con acta generada
                //contrato.EstadoActa = "3";                

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

        public async Task<Respuesta> EditarContratoObservacion(int pContratoId, int pPlazoFase2PreMeses, int pPlazoFase2PreDias, string pObservacion,
                                                             string pUsuarioModificacion, DateTime pFechaActaInicioFase1, DateTime pFechaTerminacionFase2,
                                                             bool pEsSupervisor, bool pEsActa)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Editar_Plazo_Ejecucion_Fase_2, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                Contrato contrato = null;
                contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).Include(r => r.Contratacion).FirstOrDefault();

                if (contrato != null)
                {
                    ConstruccionObservacion construccionObservacion = new ConstruccionObservacion();

                    //                6   Acta de inicio interventor
                    //7   Acta de inicio supervisor
                    construccionObservacion.Observaciones = pObservacion;

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

                    contrato.FechaActaInicioFase2 = pFechaActaInicioFase1;
                    contrato.FechaTerminacionFase2 = pFechaTerminacionFase2;

                    contrato.PlazoFase2ConstruccionDias = pPlazoFase2PreDias;
                    contrato.PlazoFase2ConstruccionMeses = pPlazoFase2PreMeses;

                    if (contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                    {
                        contrato.EstadoActaFase2 = "14"; //Con acta preliminar generada 
                    }

                    ValidarFechasNulas(ref contrato);

                    _context.Contrato.Update(contrato);
                    _context.SaveChanges();

                }

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
          , IFormFile pFile, string pDirectorioBase, string pDirectorioActaInicio, string pUsuarioModificacion, AppSettingsService _appSettingsService
            )
        {
            //            Fecha de la firma del documento por parte del contratista de obra -FechaFirmaContratista - contrato
            //Fecha de la firma del documento por parte del contratista de interventoría -FechaFirmaActaContratistaInterventoria - contrato

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Editar_Cargar_Acta_Suscrita_Contrato_Fase_2, (int)EnumeratorTipoDominio.Acciones);

            Contrato contrato;

            contrato = _context.Contrato
                .Include(r => r.Contratacion).ThenInclude(r => r.ContratacionProyecto)
                .Where(r => r.ContratoId == pContratoId)
                .FirstOrDefault();

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
                actaInicio = await getDataActaInicioAsync(pContratoId);

                //Contrato contrato = null;
                //contrato = _context.Contrato.Where(r => r.NumeroContrato == pActaInicio.NumeroContrato).FirstOrDefault();


                List<UsuarioPerfil> listaUsuarioPerfil = getCorreos((int)EnumeratorPerfil.Supervisor);
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

                template = template.Replace("_Cantidad_Proyectos_Asociados_", contrato.Contratacion.ContratacionProyecto.Count(r => !r.Eliminado).ToString());

                template = template.Replace("_Fecha_Acta_Inicio_", actaInicio.FechaActaInicio);
                template = template.Replace("_Fecha_Prevista_Terminacion_", actaInicio.FechaPrevistaTerminacion);

                //datos basicos generales, aplican para los 4 mensajes

                //correo, _settings.Value.MailServer,
                //_settings.Value.MailPort, _settings.Value.Password, _settings.Value.Sender,
                //actaInicio, pIdTemplate
                string correo = "";
                listaUsuarioPerfil.ForEach(lu =>
                {
                    blEnvioCorreo = Helpers.Helpers.EnviarCorreo(lu.Usuario.Email, "Gestión Acta Inicio Fase II", template, _appSettingsService.Sender, _appSettingsService.Password, _appSettingsService.MailServer, _appSettingsService.MailPort);

                    correo = correo + lu.Usuario.Email + ";";
                });


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

        public async Task<Respuesta> EditarCargarActaSuscritaContrato(int pContratoId, DateTime pFechaFirmaContratista, DateTime pFechaFirmaActaContratistaInterventoria,
            string pUsuarioModificacion, IFormFile pFile, string pFilePatch
           )
        {
            //            Fecha de la firma del documento por parte del contratista de obra -FechaFirmaContratista - contrato
            //Fecha de la firma del documento por parte del contratista de interventoría -FechaFirmaActaContratistaInterventoria - contrato

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Editar_Cargar_Acta_Suscrita_Contrato_Fase_2, (int)EnumeratorTipoDominio.Acciones);

            ArchivoCargue archivoCarge = await _documentService.getSaveFile(pFile, pFilePatch, Int32.Parse(OrigenArchivoCargue.ActaSuscritaContrato), pContratoId);

            Contrato contrato;

            contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

            try
            {
                contrato.FechaFirmaContratista = pFechaFirmaContratista;
                contrato.FechaFirmaActaContratistaInterventoriaFase2 = pFechaFirmaActaContratistaInterventoria;
                contrato.UsuarioModificacion = pUsuarioModificacion;
                contrato.FechaModificacion = DateTime.Now;
                contrato.RutaActaSuscrita = contrato.RutaActaSuscrita = pFilePatch + "//" + archivoCarge.Nombre + ".pdf";

                _context.Contrato.Update(contrato);
                _context.SaveChanges();

                return
                 new Respuesta
                 {
                     IsSuccessful = true,
                     IsException = false,
                     IsValidation = false,
                     Code = ConstantGestionarActaInicioFase2.OperacionExitosa,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, ConstantGestionarActaInicioFase2.OperacionExitosa, idAccion, contrato.UsuarioModificacion, " EDITAR CARGAR ACTA SUSCRITA CONTRATO")
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
            Contrato contrato = null;
            Contratacion contratacion = null;
            int pIdTemplate = (int)enumeratorTemplate.ActaInicioFase2ObraInterventoria;

            contrato = _context.Contrato
                .Include(r => r.Contratacion).ThenInclude(r => r.ContratacionProyecto)
                .Where(r => r.ContratoId == pContratoId)
                .FirstOrDefault();

            contratacion = _context.Contratacion.Where(r => r.ContratacionId == contrato.ContratacionId).FirstOrDefault();

            VistaGenerarActaInicioContrato actaInicio = new VistaGenerarActaInicioContrato();
            if (contrato != null)
                //actaInicio = await getDataActaInicioAsync(pContratoId,Convert.ToInt32( contrato.TipoContratoCodigo));
                actaInicio = await getDataActaInicioAsync(pContratoId);

            //perfilId = 8; //  Supervisor
            //perfilId = (int)EnumeratorPerfil.Supervisor; //  Supervisor
            List<UsuarioPerfil> listaUsuarioPerfil = getCorreos(pPerfilId);
            string correo = "";

            try
            {
                int idAccionCorreoContrato = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Notificación_Acta_Inicio_Fase_II, (int)EnumeratorTipoDominio.Acciones);

                //error correo
                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesActaInicio.ErrorEnviarCorreo };

                bool blEnvioCorreo = false;

                Template TemplateRecoveryPassword = await _commonService.GetTemplateById(pIdTemplate);

                string template = TemplateRecoveryPassword.Contenido;

                int tipoContrato = 0;

                //tipoContrato = 2;
                tipoContrato = ConstanCodigoTipoContratacion.Interventoria;

                template = template.Replace("_Numero_Contrato_", actaInicio.NumeroContrato);
                template = template.Replace("_Fecha_Aprobacion_Poliza_", actaInicio.FechaAprobacionGarantiaPoliza);

                if (Convert.ToInt32(contratacion.TipoSolicitudCodigo) == ConstanCodigoTipoContratacion.Interventoria)
                    template = template.Replace("_Obra_O_Interventoria_", "interventoría");
                else if (Convert.ToInt32(contratacion.TipoSolicitudCodigo) == ConstanCodigoTipoContratacion.Obra)
                    template = template.Replace("_Obra_O_Interventoria_", "obra");

                template = template.Replace("_Cantidad_Proyectos_Asociados_", contrato.Contratacion.ContratacionProyecto.Count(r => !r.Eliminado).ToString());

                template = template.Replace("_Fecha_Acta_Inicio_", actaInicio.FechaActaInicio);
                template = template.Replace("_Fecha_Prevista_Terminacion_", actaInicio.FechaPrevistaTerminacion);

                //            1   Administrador  - //2   Técnica
                //3   Financiera - //4   Jurídica
                //5   Administrativa - //6   Miembros Comite
                //7   Secretario comité - //8   Supervisor


                foreach (var perfilUsuario in listaUsuarioPerfil)
                {
                    blEnvioCorreo = Helpers.Helpers.EnviarCorreo(perfilUsuario.Usuario.Email, "Gestión Acta Inicio Fase II", template, settings.Sender, settings.Password, settings.MailServer, settings.MailPort);

                    correo = correo + perfilUsuario.Usuario.Email + ";";
                }


                if (blEnvioCorreo)
                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantGestionarActaInicioFase2.CorreoEnviado };

                else
                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantGestionarActaInicioFase2.ErrorEnviarCorreo };

                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificación_Acta_Inicio_Fase_II), correo, "Acta Inicio Fase II");
                return respuesta;


                //ok correo
                respuesta = new Respuesta() { IsSuccessful = true, IsValidation = true, Code = ConstantMessagesActaInicio.CorreoEnviado };

                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificación_Acta_Inicio_Fase_II), correo, "Acta Inicio Fase II");
                return respuesta;

            }
            catch (Exception ex)
            {

                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesActaInicio.ErrorEnviarCorreo };
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificación_Acta_Inicio_Fase_II), correo, "Acta Inicio Fase II") + ": " + ex.ToString() + ex.InnerException;
                return respuesta;
            }

        }

        private List<UsuarioPerfil> getCorreos(int perfilId)
        {
            //[Usuario], [UsuarioPerfil] , [Perfil]
            string lstCorreos = "";

            //1   Administrador  - //2   Técnica
            //3   Financiera - //4   Jurídica
            //5   Administrativa - //6   Miembros Comite
            //7   Secretario comité - //8   Supervisor
            List<UsuarioPerfil> lstUsuariosPerfil = new List<UsuarioPerfil>();

            lstUsuariosPerfil = _context.UsuarioPerfil.Where(r => r.Activo == true && r.PerfilId == perfilId).Include(r => r.Usuario).ToList();

            return lstUsuariosPerfil;
        }

        private string formatValor(string valor)
        {
            if (valor != null)
            {
                if (valor.Length > 2)
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

            if (!string.IsNullOrEmpty(pPlantilla.Encabezado.Contenido))
            {
                strEncabezado = Helpers.Helpers.HtmlStringLimpio(pPlantilla.Encabezado.Contenido);
            }

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

        public async Task<Respuesta> EnviarCorreoGestionActaIncio(string lstMails, string pMailServer, int pMailPort, string pPassword, string pSentender, VistaGenerarActaInicioContrato pActaInicio, int pIdTemplate)
        {
            bool blEnvioCorreo = false;
            Respuesta respuesta = new Respuesta();

            try
            {
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById(pIdTemplate);

                string template = TemplateRecoveryPassword.Contenido;

                Contrato contrato = _context.Contrato
                    .Where(r => r.NumeroContrato == pActaInicio.NumeroContrato)
                    .Include(r => r.Contratacion).ThenInclude(r => r.ContratacionProyecto)
                    .FirstOrDefault();

                template = template.Replace("_Numero_Contrato_", pActaInicio.NumeroContrato);
                template = template.Replace("_Fecha_Aprobacion_Poliza_", pActaInicio.FechaAprobacionGarantiaPoliza);

                if (Convert.ToInt32(contrato.TipoContratoCodigo) == ConstanCodigoTipoContratacion.Interventoria)
                    template = template.Replace("_Obra_O_Interventoria_", "interventoría");
                else if (Convert.ToInt32(contrato.TipoContratoCodigo) == ConstanCodigoTipoContratacion.Obra)
                    template = template.Replace("_Obra_O_Interventoria_", "obra");

                template = template.Replace("_Cantidad_Proyectos_Asociados_", contrato.Contratacion.ContratacionProyecto.Count(r => !r.Eliminado).ToString());

                template = template.Replace("_Fecha_Acta_Inicio_", pActaInicio.FechaActaInicio);
                template = template.Replace("_Fecha_Prevista_Terminacion_", pActaInicio.FechaPrevistaTerminacion);

                blEnvioCorreo = Helpers.Helpers.EnviarCorreo(lstMails, "Gestión Acta Inicio Fase II", template, pSentender, pPassword, pMailServer, pMailPort);

                if (blEnvioCorreo)
                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantGestionarActaInicioFase2.CorreoEnviado };

                else
                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantGestionarActaInicioFase2.ErrorEnviarCorreo };

                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificación_Acta_Inicio_Fase_II), lstMails, "Acta Inicio Fase II");
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesActaInicio.ErrorEnviarCorreo };
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_acta_inicio_fase_2, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificación_Acta_Inicio_Fase_II), lstMails, "Gestión Acta Inicio Fase II") + ": " + ex.ToString() + ex.InnerException;
                return respuesta;
            }

        }

        public async Task<ContratoObservacion> GetContratoObservacionByIdContratoId(Contrato pContrato, bool pEsSupervisor)
        {
            ContratoObservacion contratoObservacion = new ContratoObservacion();
            List<ContratoObservacion> lstContratoObservacion = new List<ContratoObservacion>();

            contratoObservacion = pContrato.ContratoObservacion
                                                    .Where(r =>
                                                           r.EsSupervision == pEsSupervisor &&
                                                           r.EsActaFase2 == true &&
                                                           r.Archivado != true
                                                           )
                                                    .OrderByDescending(r => r.ContratoObservacionId)
                                                    ?.FirstOrDefault();

            return contratoObservacion;

        }

        public async Task<ConstruccionObservacion> GetConstruccionObservacionByIdContratoConstruccionId(int pContratoConstruccionId, bool pEsSupervisor)
        {
            ConstruccionObservacion observacion = new ConstruccionObservacion();
            List<ContratoObservacion> lstContratoObservacion = new List<ContratoObservacion>();

            observacion = _context.ConstruccionObservacion
                                        .Where(
                                                r => r.ContratoConstruccionId == pContratoConstruccionId &&
                                                r.EsSupervision == pEsSupervisor &&
                                                r.EsActa == true &&
                                                r.Archivada != true
                                                )
                                        .OrderByDescending(r => r.ConstruccionObservacionId)
                                        ?.FirstOrDefault();


            return observacion;

        }

        public async Task<ContratoObservacion> GetContratoObservacionByIdContratoIdUltimaArchivada(int pContratoId, bool pEsSupervisor)
        {
            ContratoObservacion contratoObservacion = new ContratoObservacion();
            List<ContratoObservacion> lstContratoObservacion = new List<ContratoObservacion>();

            Contrato contrato = await _context.Contrato.FindAsync(pContratoId);

            if (contrato != null)
            {
                //contratoObservacion.ContratoConstruccionId = contratoConstruccion.ContratoConstruccionId;

                contratoObservacion = _context.ContratoObservacion
                                                        .Where(
                                                                r => r.ContratoId == pContratoId &&
                                                                r.EsSupervision == pEsSupervisor &&
                                                                r.EsActaFase2 == true &&
                                                                r.Archivado == true
                                                               )
                                                        .OrderByDescending(r => r.ContratoObservacionId)
                                                        ?.FirstOrDefault();

            }
            return contratoObservacion;
        }

        public async Task<Contrato> GetContratoByIdContratoId(int pContratoId)
        {
            return await _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefaultAsync();
        }

        public async Task<Respuesta> InsertEditContratoObservacion(Contrato pContrato)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Contrato_Observacion, (int)EnumeratorTipoDominio.Acciones);

            Contrato contrato = _context.Contrato.Find(pContrato.ContratoId);

            contrato.ConObervacionesActaFase2 = pContrato.ConObervacionesActaFase2;

            string strCrearEditar = "";
            try
            {
                if (contrato.ConObervacionesActaFase2 == true)
                {
                    if (pContrato != null && pContrato.ContratoObservacion.Count() > 0)
                    {
                        pContrato.ContratoObservacion.ToList().ForEach(pContratoObservacion =>
                        {
                            if (pContratoObservacion.ContratoObservacionId == 0)
                            {
                                //Auditoria
                                strCrearEditar = "REGISTRAR CONTRATO OBSERVACION";

                                pContratoObservacion.UsuarioCreacion = pContrato.UsuarioCreacion;
                                pContratoObservacion.FechaCreacion = DateTime.Now;

                                pContratoObservacion.EsSupervision = true;

                                _context.ContratoObservacion.Add(pContratoObservacion);

                            }
                            else
                            {
                                strCrearEditar = "EDIT CONTRATO OBSERVACION";

                                ContratoObservacion contratoObservacion = _context.ContratoObservacion.Find(pContratoObservacion.ContratoObservacionId);

                                if (contratoObservacion != null)
                                {

                                    //Auditoria
                                    contratoObservacion.UsuarioModificacion = pContrato.UsuarioCreacion;
                                    contratoObservacion.FechaModificacion = DateTime.Now;

                                    contratoObservacion.Observaciones = pContratoObservacion.Observaciones;

                                }
                            }
                        });




                    }
                    else
                    {
                        return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesActaInicio.RecursoNoEncontrado };
                    }
                }

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
                                                    ConstantMessagesActaInicio.OperacionExitosa,
                                                    idAccionCrearContratoPoliza,
                                                    pContrato.UsuarioCreacion,
                                                    strCrearEditar
                                                )
                    };



            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesActaInicio.ErrorInterno, Message = ex.InnerException.ToString().Substring(0, 500) };
            }

        }

        private decimal getSumVlrContratoComponente(int contratacionId, string FaseCodigo)
        {


            Contratacion contratacion = _context.Contratacion.Where(r => r.ContratacionId == contratacionId).Include(r => r.ContratacionProyecto)
                .ThenInclude(r => r.ContratacionProyectoAportante).ThenInclude(r => r.ComponenteAportante).ThenInclude(r => r.ComponenteUso).FirstOrDefault();

            decimal vlrFase1 = 0, vlrFase2 = 0;
            foreach (var contratacionProyecto in contratacion.ContratacionProyecto)
            {
                foreach (var contratacionProyectoAportante in contratacionProyecto.ContratacionProyectoAportante)
                {
                    foreach (var componenteAportante in contratacionProyectoAportante.ComponenteAportante)
                    {
                        if (componenteAportante.FaseCodigo == ConstanCodigoFaseContrato.Preconstruccion)
                            vlrFase1 += componenteAportante.ComponenteUso.Sum(r => r.ValorUso);
                        else
                            vlrFase2 += componenteAportante.ComponenteUso.Sum(r => r.ValorUso);

                    }

                }

            }

            //var sum = _context.ComponenteUso.Where(
            //    x => x.ComponenteAportante.ContratacionProyectoAportante.ContratacionProyecto.ContratacionId == contratacionId
            //    && x.ComponenteAportante.FaseCodigo == FaseCodigo

            //    ).Sum(x => x.ValorUso);
            if (FaseCodigo == ConstanCodigoFaseContrato.Preconstruccion)
                return vlrFase1;
            else
                return vlrFase2;

        }

        public async Task<ContratoObservacion> GetContratoObservacionByIdContratoId(int pContratoId, bool pEsSupervisor)
        {
            ContratoObservacion contratoObservacion = new ContratoObservacion();
            List<ContratoObservacion> lstContratoObservacion = new List<ContratoObservacion>();

            Contrato contrato = await _context.Contrato.FindAsync(pContratoId);

            if (contrato != null)
            {
                contratoObservacion = _context.ContratoObservacion
                                                        .Where(
                                                                r => r.ContratoId == pContratoId &&
                                                                r.EsSupervision == pEsSupervisor &&
                                                                r.EsActaFase2 == true &&
                                                                r.Archivado != true
                                                               )
                                                        .OrderByDescending(r => r.ContratoObservacionId)
                                                        ?.FirstOrDefault();
            }
            return contratoObservacion;

        }

        public async Task<List<GrillaActaInicio>> GetListGrillaActaInicio(int pPerfilId)
        {
            List<GrillaActaInicio> lstActaInicio = new List<GrillaActaInicio>();

            try
            {
                List<Contrato> lstContratos = await _context.Contrato
                    .Where(r => r.EstadoVerificacionConstruccionCodigo == ConstanCodigoEstadoConstruccion.Con_requisitos_tecnicos_aprobados_por_supervisor
                      && !(bool)r.Eliminado)
                   .Include(r => r.Contratacion)
                   .Include(r => r.ContratoObservacion)
                   .ToListAsync();

                Dominio EstadoActaFase2Contrato = null;
                string strEstadoActaFase2Contrato = string.Empty;
                Dominio EstadoVerificacion = null;
                string strEstadoVerificacion = string.Empty;
                bool bTieneObservacionesSupervisor;
                string contratacionTipoContrato = string.Empty;

                List<Dominio> Listdominios =
                    await _context.Dominio.Where(d => d.Activo == true
                && (
                         d.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Verificacion_Contrato
                      || d.TipoDominioId == (int)EnumeratorTipoDominio.Estados_actas_inicio_interventoria
                      || d.TipoDominioId == (int)EnumeratorTipoDominio.Estados_actas_inicio_obra
                   )
                ).ToListAsync();

                foreach (var item in lstContratos)
                {
                    GrillaActaInicio actaInicio = new GrillaActaInicio();

                    EstadoVerificacion = Listdominios.Where(e => e.Codigo == item.EstadoVerificacionConstruccionCodigo && e.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Verificacion_Contrato).FirstOrDefault();

                    if (EstadoVerificacion != null)
                        strEstadoVerificacion = EstadoVerificacion.Nombre;

                    ContratoObservacion contratoObservacion = await GetContratoObservacionByIdContratoId(item, false);
                    bTieneObservacionesSupervisor = false;
                    if (contratoObservacion != null)
                        bTieneObservacionesSupervisor = true;

                    string nombreTipoContrato = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(ConstanCodigoTipoContratacionSTRING.Obra);

                    if (item.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Interventoria)
                        nombreTipoContrato = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(ConstanCodigoTipoContratacionSTRING.Interventoria);
                    actaInicio.TipoContratoCodigo = item.Contratacion.TipoSolicitudCodigo;
                    actaInicio.EstadoVerificacion = strEstadoVerificacion;
                    actaInicio.ContratoId = item.ContratoId;
                    actaInicio.NumeroContratoObra = item.NumeroContrato;
                    actaInicio.TipoContrato = nombreTipoContrato;
                    actaInicio.TieneObservacionesSupervisor = bTieneObservacionesSupervisor;
                    contratacionTipoContrato = item.Contratacion.TipoSolicitudCodigo.Trim();

                    if (item.EstadoActaFase2 != null)
                    {
                        if (contratacionTipoContrato == ConstanCodigoTipoContrato.Interventoria)
                            EstadoActaFase2Contrato = Listdominios.Where(r => r.Codigo == item.EstadoActaFase2.Trim() && r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_actas_inicio_interventoria).FirstOrDefault();
                        else if (contratacionTipoContrato == ((int)ConstanCodigoTipoContratacion.Obra).ToString())
                            EstadoActaFase2Contrato = Listdominios.Where(r => r.Codigo == item.EstadoActaFase2.Trim() && r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_actas_inicio_obra).FirstOrDefault();
                    }

                    if (EstadoActaFase2Contrato != null)
                    {
                        actaInicio.EstadoActaCodigo = EstadoActaFase2Contrato.Codigo;

                        if ((int)EnumeratorPerfil.Supervisor == pPerfilId)
                            strEstadoActaFase2Contrato = EstadoActaFase2Contrato.Descripcion;
                        else if ((int)EnumeratorPerfil.Tecnica == pPerfilId)
                            strEstadoActaFase2Contrato = EstadoActaFase2Contrato.Nombre;

                        actaInicio.EstadoActa = strEstadoActaFase2Contrato;
                    }

                    actaInicio.FechaAprobacionRequisitos = item.FechaAprobacionRequisitosConstruccionSupervisor != null ? Convert.ToDateTime(item.FechaAprobacionRequisitosConstruccionSupervisor).ToString("dd/MM/yyyy") : item.FechaAprobacionRequisitosConstruccionSupervisor.ToString();
                    lstActaInicio.Add(actaInicio);
                }
            }
            catch (Exception)
            {

            }

            return lstActaInicio;

        }

        public async Task GetDocumentoNoCargadoValue(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            List<Contrato> lstContratos = _context.Contrato
                .Where(r => string.IsNullOrEmpty(r.RutaDocumento))
                .Include(r => r.Contratacion).ThenInclude(r => r.ContratacionProyecto)
                .ToList();
            int pContratoId;

            foreach (Contrato contrato in lstContratos)
            {
                pContratoId = contrato.ContratoId;
                int pIdTemplate = (int)enumeratorTemplate.Dias2ObraOInterventoriaDocNoCargado;
                VistaGenerarActaInicioContrato actaInicio;
                actaInicio = await getDataActaInicioAsync(pContratoId);
                List<UsuarioPerfil> listaUsuarioPerfil = getCorreos((int)EnumeratorPerfil.Supervisor);
                bool blEnvioCorreo = false;
                Respuesta respuesta = new Respuesta();
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById(pIdTemplate);
                string template = TemplateRecoveryPassword.Contenido;

                template = template.Replace("_Numero_Contrato_", actaInicio.NumeroContrato);
                template = template.Replace("_Fecha_Aprobacion_Poliza_", actaInicio.FechaAprobacionGarantiaPoliza);

                if (Convert.ToInt32(contrato.TipoContratoCodigo) == ConstanCodigoTipoContratacion.Interventoria)
                    template = template.Replace("_Obra_O_Interventoria_", "interventoría");
                else if (Convert.ToInt32(contrato.TipoContratoCodigo) == ConstanCodigoTipoContratacion.Obra)
                    template = template.Replace("_Obra_O_Interventoria_", "obra");

                template = template.Replace("_Cantidad_Proyectos_Asociados_", contrato.Contratacion.ContratacionProyecto.Count(r => !r.Eliminado).ToString());
                template = template.Replace("_Fecha_Acta_Inicio_", actaInicio.FechaActaInicio);
                template = template.Replace("_Fecha_Prevista_Terminacion_", actaInicio.FechaPrevistaTerminacion);

                listaUsuarioPerfil.ForEach(up =>
                {
                    blEnvioCorreo = Helpers.Helpers.EnviarCorreo(up.Usuario.Email, "Gestión Acta Inicio Fase II", template, pSender, pPassword, pMailServer, pMailPort);
                });
            }
        }

        public async Task<string> SendNotificationWorkActFromComptroller(Contrato pContrato)
        {
            int pIdTemplate = (int)enumeratorTemplate.ActaInicioFase2ObraInterventoria;
            VistaGenerarActaInicioContrato actaInicio;
            actaInicio = await getDataActaInicioAsync(pContrato.ContratoId);

            Template TemplateRecoveryPassword = await _commonService.GetTemplateById(pIdTemplate);

            string template = TemplateRecoveryPassword.Contenido;

            template = template.Replace("_Numero_Contrato_", actaInicio.NumeroContrato);
            template = template.Replace("_Fecha_Aprobacion_Poliza_", actaInicio.FechaAprobacionGarantiaPoliza);

            if (Convert.ToInt32(pContrato.Contratacion.TipoSolicitudCodigo) == ConstanCodigoTipoContratacion.Interventoria)
                template = template.Replace("_Obra_O_Interventoria_", "interventoría");
            else if (Convert.ToInt32(pContrato.Contratacion.TipoSolicitudCodigo) == ConstanCodigoTipoContratacion.Obra)
                template = template.Replace("_Obra_O_Interventoria_", "obra");

            template = template.Replace("_Cantidad_Proyectos_Asociados_", actaInicio.CantidadProyectosAsociados.ToString());
            template = template.Replace("_Fecha_Acta_Inicio_", actaInicio.FechaActaInicio);
            template = template.Replace("_Fecha_Prevista_Terminacion_", actaInicio.FechaPrevistaTerminacion);

            return template;
        }

        public async Task<string> SendNotificationWorkActFromSupervisor(Contrato pContrato)
        {
            int pIdTemplate = (int)enumeratorTemplate.ActaInicioFase2ObraInterventoria;
            VistaGenerarActaInicioContrato actaInicio;
            actaInicio = await getDataActaInicioAsync(pContrato.ContratoId);

            Template TemplateRecoveryPassword = await _commonService.GetTemplateById(pIdTemplate);

            string template = TemplateRecoveryPassword.Contenido;

            template = template.Replace("_Numero_Contrato_", actaInicio.NumeroContrato);
            template = template.Replace("_Fecha_Aprobacion_Poliza_", actaInicio.FechaAprobacionGarantiaPoliza);

            if (Convert.ToInt32(pContrato.Contratacion.TipoSolicitudCodigo) == ConstanCodigoTipoContratacion.Interventoria)
                template = template.Replace("_Obra_O_Interventoria_", "interventoría");
            else if (Convert.ToInt32(pContrato.Contratacion.TipoSolicitudCodigo) == ConstanCodigoTipoContratacion.Obra)
                template = template.Replace("_Obra_O_Interventoria_", "obra");

            template = template.Replace("_Cantidad_Proyectos_Asociados_", actaInicio.CantidadProyectosAsociados.ToString());
            template = template.Replace("_Fecha_Acta_Inicio_", actaInicio.FechaActaInicio);
            template = template.Replace("_Fecha_Prevista_Terminacion_", actaInicio.FechaPrevistaTerminacion);
            return template;
        }

        public async Task<string> SendNotificationWithComentsFromSupervisor(Contrato pContrato)
        {
            int pIdTemplate = (int)enumeratorTemplate.ObservacionesActaInicioFase2;
            VistaGenerarActaInicioContrato actaInicio;
            actaInicio = await getDataActaInicioAsync(pContrato.ContratoId);

            Template TemplateRecoveryPassword = await _commonService.GetTemplateById(pIdTemplate);

            string template = TemplateRecoveryPassword.Contenido;

            template = template.Replace("_Numero_Contrato_", actaInicio.NumeroContrato);
            template = template.Replace("_Fecha_Aprobacion_Poliza_", actaInicio.FechaAprobacionGarantiaPoliza);

            if (Convert.ToInt32(pContrato.Contratacion.TipoSolicitudCodigo) == ConstanCodigoTipoContratacion.Interventoria)
                template = template.Replace("_Obra_O_Interventoria_", "interventoría");
            else if (Convert.ToInt32(pContrato.Contratacion.TipoSolicitudCodigo) == ConstanCodigoTipoContratacion.Obra)
                template = template.Replace("_Obra_O_Interventoria_", "obra");

            template = template.Replace("_Cantidad_Proyectos_Asociados_", actaInicio.CantidadProyectosAsociados.ToString());
            template = template.Replace("_Fecha_Acta_Inicio_", actaInicio.FechaActaInicio);
            template = template.Replace("_Fecha_Prevista_Terminacion_", actaInicio.FechaPrevistaTerminacion);


            return template;
        }

        private async Task enviarNotificaciones(Contrato pContrato, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            List<UsuarioPerfil> listaApoyo = getCorreos((int)EnumeratorPerfil.Apoyo);
            List<UsuarioPerfil> listaSupervisor = getCorreos((int)EnumeratorPerfil.Supervisor);
            List<UsuarioPerfil> listaInterventor = getCorreos((int)EnumeratorPerfil.Interventor);

            //Con acta en proceso de firma - obra
            if (pContrato.EstadoActaFase2 == "19" || pContrato.EstadoActaFase2 == "21")
            {
                Contratacion contratacion = _context.Contratacion
                                                            .Where(p => p.ContratacionId == pContrato.ContratacionId)
                                                            .Include(r => r.Contratista)
                                                                .ThenInclude(r => r.ProcesoSeleccionProponente)
                                                                .FirstOrDefault();

                List<UsuarioPerfil> listaFinal = new List<UsuarioPerfil>();
                listaFinal.AddRange(listaSupervisor);
                listaFinal.AddRange(listaApoyo);

                string template = await SendNotificationWorkActFromComptroller(pContrato);

                foreach (var ls in listaFinal)
                {
                    Helpers.Helpers.EnviarCorreo(ls.Usuario.Email, "Gestión Acta Inicio Fase II", template, pSender, pPassword, pMailServer, pMailPort);
                };

                if (contratacion?.Contratista?.ProcesoSeleccionProponente != null)
                {
                    Helpers.Helpers.EnviarCorreo(contratacion.Contratista.ProcesoSeleccionProponente.EmailProponente, "Gestión Acta Inicio Fase II", template, pSender, pPassword, pMailServer, pMailPort);
                }
            }
            if (pContrato.EstadoActaFase2 == "17" || pContrato.EstadoActaFase2 == "4")
            {
                List<UsuarioPerfil> listaFinal = new List<UsuarioPerfil>();
                listaFinal.AddRange(listaInterventor);
                foreach (var ls in listaFinal)
                {
                    string template = await SendNotificationWithComentsFromSupervisor(pContrato);

                    Helpers.Helpers.EnviarCorreo(ls.Usuario.Email, "Gestión Acta Inicio Fase II", template, pSender, pPassword, pMailServer, pMailPort);
                };
            }

            // Enviada por el supervisor - Interventoria
            if (pContrato.EstadoActaFase2 == "6")
            {
                List<UsuarioPerfil> listaFinal = new List<UsuarioPerfil>();
                listaFinal.AddRange(listaInterventor);
                listaFinal.AddRange(listaApoyo);
                foreach (var ls in listaFinal)
                {
                    string template = await SendNotificationWorkActFromComptroller(pContrato);
                    Helpers.Helpers.EnviarCorreo(ls.Usuario.Email, "Gestión Acta Inicio Fase II", template, pSender, pPassword, pMailServer, pMailPort);
                };
            }

        }

        public async Task<Respuesta> CambiarEstadoActa(int pContratoId, string pNuevoCodigoEstadoActa, string pUsuarioModifica, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Acta_Inicio_Fase_2, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                Contrato contrato = _context.Contrato
                                                .Where(r => r.ContratoId == pContratoId)
                                                .Include(r => r.Contratacion)
                                                .FirstOrDefault();


                contrato.UsuarioModificacion = pUsuarioModifica;
                contrato.EstadoActaFase2 = pNuevoCodigoEstadoActa.Trim();
                contrato.FechaModificacion = DateTime.Now;
                contrato.FechaCambioEstadoFase2 = DateTime.Now;

                await enviarNotificaciones(contrato, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);

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

        public async Task<VistaGenerarActaInicioContrato> GetListVistaGenerarActaInicio(int pContratoId, int pUserId)
        {
            VistaGenerarActaInicioContrato actaInicioConsolidado = new VistaGenerarActaInicioContrato();

            VistaGenerarActaInicioContrato actaInicioInterventoria = new VistaGenerarActaInicioContrato();
            VistaGenerarActaInicioContrato actaInicioObra = new VistaGenerarActaInicioContrato();

            actaInicioObra = await getDataActaInicioAsync(pContratoId);

            actaInicioConsolidado = await GetDataConsolidadoActaInicioAsync(actaInicioObra, actaInicioObra);

            //return actaInicioObra;
            return actaInicioConsolidado;

        }

        private async Task<VistaGenerarActaInicioContrato> getDataActaInicioAsync(int pContratoId)
        {
            VistaGenerarActaInicioContrato actaInicio;
            try
            {
                Contrato contrato = _context.Contrato.AsNoTracking()
                                                     .Where(r => r.ContratoId == pContratoId)
                                                     .Include(r => r.ContratoConstruccion).ThenInclude(r => r.ConstruccionObservacion)
                                                     .Include(ctr => ctr.Interventor)
                                                     .Include(ctr => ctr.Apoyo)
                                                     .Include(ctr => ctr.Supervisor)
                                                     .Include(ctr => ctr.Contratacion).ThenInclude(pc => pc.PlazoContratacion)
                                                     .Include(ctr => ctr.Contratacion).ThenInclude(pc => pc.DisponibilidadPresupuestal)
                                                     .FirstOrDefault();

                string strFechaPrevistaTerminacion = string.Empty;
                string strFechaActaInicio = string.Empty;

                DateTime FechaActaInicioFase1DateTime = new DateTime();
                DateTime FechaActaInicioFase2DateTime = new DateTime();

                DateTime FechaPrevistaTerminacionDateTime = new DateTime();

                string strContratoObservacion = string.Empty;
                int ContratacionId = 0;

                Contratacion contratacion = null;

                ContratoPoliza contratoPoliza = null;
                Usuario Supervisor = null;
                Usuario interventor = null;

                if (contrato != null)
                {
                    contratacion = _context.Contratacion.AsNoTracking()
                                                        .Where(c => c.ContratacionId == contrato.ContratacionId)
                                                        .Include(pc => pc.PlazoContratacion)
                                                        .Include(pc => pc.DisponibilidadPresupuestal)
                                                        .FirstOrDefault();

                    ContratacionId = contrato.ContratacionId;
                    strFechaActaInicio = contrato.FechaActaInicioFase2 != null ? Convert.ToDateTime(contrato.FechaActaInicioFase2).ToString("dd/MM/yyyy") : contrato.FechaActaInicioFase2.ToString();

                    FechaActaInicioFase1DateTime = Convert.ToDateTime(contrato.FechaActaInicioFase1);
                    FechaActaInicioFase2DateTime = Convert.ToDateTime(contrato.FechaActaInicioFase2);
                    FechaPrevistaTerminacionDateTime = Convert.ToDateTime(contrato.FechaActaInicioFase2);

                    if (contrato.FechaActaInicioFase2 != null && (contrato.PlazoFase2ConstruccionDias != null || contrato.PlazoFase2ConstruccionDias != null))
                    {
                        FechaPrevistaTerminacionDateTime = FechaPrevistaTerminacionDateTime.AddMonths((int)(contrato?.PlazoFase2ConstruccionMeses != null ? contrato?.PlazoFase2ConstruccionMeses : 0)).AddDays((int)(contrato?.PlazoFase2ConstruccionDias != null ? contrato?.PlazoFase2ConstruccionDias : 0));
                    }

                    strFechaPrevistaTerminacion = FechaPrevistaTerminacionDateTime != null ? Convert.ToDateTime(FechaPrevistaTerminacionDateTime).ToString("dd/MM/yyyy") : contrato?.FechaTerminacionFase2.ToString();

                    ConstruccionObservacion construccionObservacion = contrato.ContratoConstruccion?
                                                                                    .FirstOrDefault()?.ConstruccionObservacion?
                                                                                    .Where(r => r.TipoObservacionConstruccion == ConstanCodigoTipoObservacionConstruccion.Seis)
                                                                                    .OrderByDescending(r => r.ConstruccionObservacionId)
                                                                                    ?.FirstOrDefault();

                    if (construccionObservacion != null)
                        strContratoObservacion = construccionObservacion.Observaciones;

                    contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    contrato.UsuarioInterventoria = _context.Usuario.Find(contrato.InterventorId);
                    //if (contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString())
                    //contrato.UsuarioInterventoria = _context.Usuario.Find(contrato.SupervisorId);
                    Supervisor = _context.Usuario.Find(contrato.SupervisorId);
                    contrato.Supervisor = Supervisor;
                }
                string strTipoContratacion = ConstanMessages.SinDefinir;
                Dominio TipoContratacionCodigo;

                ContratacionProyecto contratacionProyecto = null;
                string strLlaveMENContrato = string.Empty;
                string strInstitucionEducativaLlaveMEN = string.Empty;
                string strDepartamentoYMunicipioLlaveMEN = string.Empty;

                Contratista contratista = null;
                string proponenteCodigo = string.Empty;
                string proponenteNombre = string.Empty;

                decimal ValorActualContratoTmp,
                    ValorFase1PreconstruccionTmp,
                    Valorfase2ConstruccionObraTmp;
                ValorActualContratoTmp = ValorFase1PreconstruccionTmp = Valorfase2ConstruccionObraTmp = 0;

                if (contratacion != null)
                {
                    Valorfase2ConstruccionObraTmp = getSumVlrContratoComponente(Convert.ToInt32(contratacion.ContratacionId), ConstanCodigoTipoContrato.Interventoria);
                    ValorFase1PreconstruccionTmp = getSumVlrContratoComponente(Convert.ToInt32(contratacion.ContratacionId), ConstanCodigoTipoContrato.Obra);
                    ValorActualContratoTmp = Valorfase2ConstruccionObraTmp + ValorFase1PreconstruccionTmp;

                    TipoContratacionCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratacion.TipoContratacionCodigo, (int)EnumeratorTipoDominio.Opcion_por_contratar);
                    if (TipoContratacionCodigo != null)
                        strTipoContratacion = TipoContratacionCodigo.Nombre;

                    contratacionProyecto = _context.ContratacionProyecto.Where(r => r.ContratacionId == contratacion.ContratacionId).FirstOrDefault();

                    contratista = _context.Contratista.Where(r => (bool)r.Activo && r.ContratistaId == contratacion.ContratistaId).FirstOrDefault();


                    if (contrato.Contratacion.TipoSolicitudCodigo == ((int)ConstanCodigoTipoContratacion.Obra).ToString())
                    {
                        proponenteCodigo = ((int)ConstanCodigoTipoContratacion.Obra).ToString();
                        proponenteNombre = ConstanMessages.Obra;
                    }
                    else if (contrato.Contratacion.TipoSolicitudCodigo == ((int)ConstanCodigoTipoContratacion.Interventoria).ToString())
                    {
                        proponenteCodigo = ((int)ConstanCodigoTipoContratacion.Interventoria).ToString();
                        proponenteNombre = ConstanMessages.Obra;
                    }
                }

                string strTipoIdentificacionCodigoContratista = ConstanMessages.SinDefinir;
                Dominio TipoIdentificacionCodigoContratista;

                if (contratista != null)
                {
                    TipoIdentificacionCodigoContratista = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratista.TipoIdentificacionCodigo, (int)EnumeratorTipoDominio.Tipo_Documento);
                    if (TipoIdentificacionCodigoContratista != null)
                        strTipoIdentificacionCodigoContratista = TipoIdentificacionCodigoContratista.Nombre;
                }

                string strVigencia = ConstanMessages.SinDefinir;



                if (contratoPoliza != null)
                    strVigencia = contrato.FechaTramite != null ? Convert.ToDateTime(contrato.FechaTramite).ToString("dd/MM/yyyy") : contrato.FechaTramite.ToString();



                Int32 intCantidadProyectosAsociados = 0;

                Proyecto proyecto = null;

                if (contratacionProyecto != null)
                {
                    intCantidadProyectosAsociados = _context.Proyecto.Where(r => r.ProyectoId == contratacionProyecto.ProyectoId).Count();
                    proyecto = _context.Proyecto.Where(r => r.ProyectoId == contratacionProyecto.ProyectoId).FirstOrDefault();
                }

                string InstitucionEducativaId = string.Empty;
                int InstitucionEducativaIdInt = 0;

                if (proyecto != null)
                {
                    InstitucionEducativaId = proyecto.InstitucionEducativaId.ToString();
                    InstitucionEducativaIdInt = Convert.ToInt32(proyecto.InstitucionEducativaId);
                }

                if (!string.IsNullOrEmpty(InstitucionEducativaId))
                    proyecto.InstitucionEducativa = await _commonService.GetInstitucionEducativaById(Convert.ToInt32(InstitucionEducativaId));

                if (proyecto != null)
                {
                    strLlaveMENContrato = proyecto.LlaveMen;
                    if (InstitucionEducativaId != null)
                        strInstitucionEducativaLlaveMEN = proyecto.InstitucionEducativa.Nombre;
                    strDepartamentoYMunicipioLlaveMEN = _commonService.GetNombreDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                }

                string rutaActaSuscrita = string.Empty;
                string strValor = string.Empty;
                decimal? valorActualContrato = 0;
                if (contrato != null)
                {
                    rutaActaSuscrita = contrato.RutaActaSuscrita;

                }

                if (contratacion != null)
                {
                    if (contratacion.DisponibilidadPresupuestal.Count() > 0)
                    {
                        strValor = contratacion.DisponibilidadPresupuestal.FirstOrDefault().ValorSolicitud.ToString();
                    }
                    VDrpXfaseContratacionId valor = _context.VDrpXfaseContratacionId.Where(r => r.ContratacionId == contratacion.ContratacionId).FirstOrDefault();
                    if (valor != null)
                        valorActualContrato = valor.ValorDrp != null ? valor.ValorDrp : 0;
                }

                actaInicio = new VistaGenerarActaInicioContrato
                {
                    NumeroContrato = contrato.NumeroContrato,
                    VigenciaContrato = strVigencia,
                    FechaFirmaContrato = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString(),
                    ProponenteCodigo = proponenteCodigo,
                    ProponenteNombre = proponenteNombre,
                    NumeroDRP1 = contrato.Contratacion.DisponibilidadPresupuestal.FirstOrDefault().NumeroDrp,
                    FechaGeneracionDRP1 = Convert.ToDateTime(contrato.Contratacion.DisponibilidadPresupuestal.FirstOrDefault().FechaDrp).ToString("dd/MM/yyyy") ?? ConstanMessages.SinDefinir,
                    NumeroDRP2 = contrato.Contratacion.DisponibilidadPresupuestal.FirstOrDefault().NumeroDrp,
                    FechaGeneracionDRP2 = Convert.ToDateTime(contrato.Contratacion.DisponibilidadPresupuestal.FirstOrDefault().FechaDrp).ToString("dd/MM/yyyy") ?? ConstanMessages.SinDefinir,
                    FechaAprobacionGarantiaPoliza = contratoPoliza.FechaAprobacion != null ? Convert.ToDateTime(contratoPoliza.FechaAprobacion).ToString("dd/MM/yyyy") : contratoPoliza.FechaAprobacion.ToString(),
                    FechaAprobacionRequisitosSupervisor = Convert.ToDateTime(contrato.FechaAprobacionRequisitosConstruccionSupervisor).ToString("dd/MM/yyyy") ?? ConstanMessages.SinDefinir,
                    FechaAprobacionRequisitosSupervisorDate = contrato.FechaAprobacionRequisitosConstruccionSupervisor,
                    Objeto = contrato.Contratacion.DisponibilidadPresupuestal.FirstOrDefault().Objeto,
                    ValorInicialContrato = formatValor(strValor),
                    ValorActualContrato = formatValor(valorActualContrato.ToString()),
                    ValorFase1Preconstruccion = ValorFase1PreconstruccionTmp.ToString(),
                    Valorfase2ConstruccionObra = Valorfase2ConstruccionObraTmp.ToString(),
                    PlazoInicialContratoSupervisor = contrato.Contratacion.DisponibilidadPresupuestal.FirstOrDefault().FechaSolicitud.ToString("dd/MM/yyyy"),
                    PlazoFase1PreMeses = contrato.PlazoFase1PreMeses,
                    PlazoFase2ConstruccionDias = contrato.PlazoFase2ConstruccionDias,
                    PlazoFase1PreDias = contrato.PlazoFase1PreDias,
                    PlazoFase2ConstruccionMeses = contrato.PlazoFase2ConstruccionMeses,
                    PlazoActualContratoMeses = contrato?.Contratacion?.PlazoContratacion?.PlazoMeses ?? 0,
                    PlazoActualContratoDias = contrato?.Contratacion?.PlazoContratacion?.PlazoDias ?? 0, 
                    NombreEntidadContratistaObra =  contratista.Nombre,
                    NombreEntidadContratistaSupervisorInterventoria = Supervisor?.PrimerNombre + " " + Supervisor?.PrimerApellido,
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
                    NumeroIdentificacionRepresentanteContratistaInterventoria = contratista?.NumeroIdentificacion,
                    RutaActaSuscrita = rutaActaSuscrita,
                    NumeroIdentificacionSupervisor = Supervisor?.NumeroIdentificacion,
                    NombreRepresentanteLegalInterventoria = contratista?.RepresentanteLegal,
                    NumeroIdentificacionEntidadContratistaObra = contratista?.RepresentanteLegalNumeroIdentificacion,
                    Contrato = contrato
                };
            }

            catch (Exception e)
            {
                actaInicio = new VistaGenerarActaInicioContrato();
            }
            return actaInicio;
        }

        private async Task<VistaGenerarActaInicioContrato> GetDataConsolidadoActaInicioAsync(VistaGenerarActaInicioContrato actaInicioObra, VistaGenerarActaInicioContrato actaInicioInterventoria)
        {
            //interventoria
            actaInicioObra.NumeroIdentificacionRepresentanteContratistaInterventoria = actaInicioInterventoria.NumeroIdentificacionRepresentanteContratistaInterventoria;// Contratista . numeroIdentificaionRepresentante
            actaInicioObra.NombreRepresentanteContratistaInterventoria = actaInicioInterventoria.NombreRepresentanteContratistaInterventoria;

            //obra
            actaInicioObra.NumeroIdentificacionRepresentanteContratistaObraInterventoria = actaInicioObra.NumeroIdentificacionRepresentanteContratistaObraInterventoria;
            actaInicioObra.NombreRepresentanteContratistaObra = actaInicioObra.NombreRepresentanteContratistaObra;

            actaInicioObra.NombreEntidadContratistaObra = actaInicioObra.NombreEntidadContratistaObra;
            actaInicioObra.NombreEntidadContratistaSupervisorInterventoria = actaInicioObra.NombreEntidadContratistaSupervisorInterventoria;

            actaInicioObra.ValorFase1Preconstruccion = formatValor(actaInicioInterventoria.ValorFase1Preconstruccion);

            actaInicioObra.Valorfase2ConstruccionObra = formatValor(actaInicioInterventoria.Valorfase2ConstruccionObra);

            //actaInicioObra.ValorActualContrato = formatValor(actaInicioInterventoria.ValorFase1Preconstruccion);

            actaInicioObra.PlazoActualContratoMeses = actaInicioObra.PlazoActualContratoMeses;

            actaInicioObra.PlazoActualContratoDias = actaInicioObra.PlazoActualContratoDias;





            return actaInicioObra;

        }

        public async Task GetDiasHabilesActaConstruccionEnviada(AppSettingsService appSettingsService)
        {
            List<Contrato> contratos = _context.Contrato
                .Where(r => (r.EstadoActaFase2 == "19" || r.EstadoActaFase2 == "6"))
               .ToList();

            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Interventor || x.PerfilId == (int)EnumeratorPerfil.Supervisor || x.PerfilId == (int)EnumeratorPerfil.Apoyo).Include(y => y.Usuario);
            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.ConActaSinDocumento319);

            foreach (var contrato in contratos)
            {
                DateTime RangoFechaConDiasHabiles = await _commonService.CalculardiasLaborales(2, contrato.FechaCambioEstadoFase2.HasValue ? contrato.FechaCambioEstadoFase2.Value : DateTime.Now);

                if ((DateTime.Now - RangoFechaConDiasHabiles).TotalDays > 2)
                {
                    int Dias = 0, Meses = 0;
                    Dias = contrato?.Contratacion?.PlazoContratacion?.PlazoDias ?? 0;
                    Meses = contrato?.Contratacion?.PlazoContratacion?.PlazoMeses ?? 0;

                    string template = TemplateRecoveryPassword.Contenido
                                .Replace("_LinkF_", appSettingsService.DominioFront)
                                .Replace("[TIPO_CONTRATO]", contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString() ? ConstanCodigoTipoContratacionSTRING.Obra : ConstanCodigoTipoContratacionSTRING.Interventoria)
                                .Replace("[NUMERO_CONTRATO]", contrato.NumeroContrato)
                                .Replace("[FECHA_PREVISTA_TERMINACION]", ((DateTime)contrato.Contratacion.DisponibilidadPresupuestal.FirstOrDefault().FechaSolicitud.AddDays(Dias).AddMonths(Meses)).ToString("dd-MM-yy"))
                                .Replace("[FECHA_POLIZA]", ((DateTime)contrato.ContratoPoliza.FirstOrDefault().FechaAprobacion).ToString("dd-MM-yy"))
                                .Replace("[FECHA_ACTA_INICIO]", contrato.FechaActaInicioFase1.HasValue ? ((DateTime)contrato.FechaActaInicioFase1).ToString("dd-MM-yy") : " ")
                                .Replace("[CANTIDAD_PROYECTOS]", contrato.Contratacion.ContratacionProyecto.Where(r => !r.Eliminado).Count().ToString());

                    foreach (var item in usuarios)
                    {
                        Helpers.Helpers.EnviarCorreo(item.Usuario.Email, "Tiene solicitudes pendientes por revisión", template, appSettingsService.Sender, appSettingsService.Password, appSettingsService.MailServer, appSettingsService.MailPort);
                    }
                }

            }


        }

        public async Task GetDiasHabilesActaRegistrada(AppSettingsService appSettingsService)
        {
            List<Contrato> contratos = _context.Contrato
                .Where(r => (r.EstadoActaFase2 == "14" || r.EstadoActaFase2 == "2" || r.EstadoActaFase2 == "3"))
               .ToList();

            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Interventor || x.PerfilId == (int)EnumeratorPerfil.Supervisor || x.PerfilId == (int)EnumeratorPerfil.Apoyo).Include(y => y.Usuario);
            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.ConActaSinDocumento319);

            foreach (var contrato in contratos)
            {
                DateTime RangoFechaConDiasHabiles = await _commonService.CalculardiasLaborales(2, contrato.FechaCambioEstadoFase2.HasValue ? contrato.FechaCambioEstadoFase2.Value : DateTime.Now);

                if ((DateTime.Now - RangoFechaConDiasHabiles).TotalDays > 2)
                {
                    int Dias = 0, Meses = 0;
                    Dias = contrato?.Contratacion?.PlazoContratacion?.PlazoDias ?? 0;
                    Meses = contrato?.Contratacion?.PlazoContratacion?.PlazoMeses ?? 0;

                    string template = TemplateRecoveryPassword.Contenido
                                .Replace("_LinkF_", appSettingsService.DominioFront)
                                .Replace("[TIPO_CONTRATO]", contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString() ? ConstanCodigoTipoContratacionSTRING.Obra : ConstanCodigoTipoContratacionSTRING.Interventoria)
                                .Replace("[NUMERO_CONTRATO]", contrato.NumeroContrato)
                                .Replace("[FECHA_PREVISTA_TERMINACION]", ((DateTime)contrato.Contratacion.DisponibilidadPresupuestal.FirstOrDefault().FechaSolicitud.AddDays(Dias).AddMonths(Meses)).ToString("dd-MM-yy"))
                                .Replace("[FECHA_POLIZA]", ((DateTime)contrato.ContratoPoliza.FirstOrDefault().FechaAprobacion).ToString("dd-MM-yy"))
                                .Replace("[FECHA_ACTA_INICIO]", contrato.FechaActaInicioFase1.HasValue ? ((DateTime)contrato.FechaActaInicioFase1).ToString("dd-MM-yy") : " ")
                                .Replace("[CANTIDAD_PROYECTOS]", contrato.Contratacion.ContratacionProyecto.Where(r => !r.Eliminado).Count().ToString());

                    foreach (var item in usuarios)
                    {
                        Helpers.Helpers.EnviarCorreo(item.Usuario.Email, "Tiene solicitudes pendientes por revisión", template, appSettingsService.Sender, appSettingsService.Password, appSettingsService.MailServer, appSettingsService.MailPort);
                    }
                }

            }
        }

    }
}
