using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Constants;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class ReprogrammingService : IReprogrammingService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly ITechnicalRequirementsConstructionPhaseService _technicalRequirementsConstructionPhaseService;

        public ReprogrammingService(devAsiVamosFFIEContext context, ICommonService commonService, ITechnicalRequirementsConstructionPhaseService technicalRequirementsConstructionPhaseService)
        {
            _context = context;
            _commonService = commonService;
            _technicalRequirementsConstructionPhaseService = technicalRequirementsConstructionPhaseService;
        }

        #region GET
        /// <summary>
        /// Novedades disponibles para hacer reprogramación
        /// </summary>
        /// <returns>vista VAjusteProgramacion, donde trae las novedades disponibles para hacer reprogramación</returns>
        public async Task<List<VAjusteProgramacion>> GetAjusteProgramacionGrid()
        {
            List<VAjusteProgramacion> ajustes = _context.VAjusteProgramacion.ToList();

            return ajustes.OrderBy(x => x.AjusteProgramacionId)
                            .ToList();
        }

        /// <summary>
        ///  Traer un Objeto AjustePrgramación x id
        /// </summary>
        /// <param name="pAjusteProgramacionId"></param>
        /// <returns>Objeto AjusteProgramacion</returns>
        public async Task<AjusteProgramacion> GetAjusteProgramacionById(int pAjusteProgramacionId)
        {
            AjusteProgramacion ajusteProgramacion = await _context.AjusteProgramacion
                                                                .Include(x => x.AjustePragramacionObservacion)
                                                                .FirstOrDefaultAsync(x => x.AjusteProgramacionId == pAjusteProgramacionId);
            if (ajusteProgramacion != null)
            {
                ajusteProgramacion.ObservacionObra = GetObservacion(ajusteProgramacion, true, true, ajusteProgramacion.ArchivoCargueIdProgramacionObra);
                ajusteProgramacion.ObservacionObraHistorico = GetObservacionHistorico(ajusteProgramacion, true);
                ajusteProgramacion.ObservacionObraInterventor = GetObservacion(ajusteProgramacion, true, false, ajusteProgramacion.ArchivoCargueIdProgramacionObra);
                ajusteProgramacion.ObservacionFlujo = GetObservacion(ajusteProgramacion, false, true, ajusteProgramacion.ArchivoCargueIdFlujoInversion);
                ajusteProgramacion.ObservacionFlujoHistorico = GetObservacionHistorico(ajusteProgramacion, false);
                ajusteProgramacion.ObservacionFlujoInterventor = GetObservacion(ajusteProgramacion, false, false, ajusteProgramacion.ArchivoCargueIdFlujoInversion);
            }

            return ajusteProgramacion;
        }
        
        /// <summary>
        /// Obtener observaciones
        /// </summary>
        /// <param name="pAjusteProgramacion"></param>
        /// <param name="pEsObra"></param>
        /// <param name="pEsSupervisor"></param>
        /// <param name="pArchivoCargueId"></param>
        /// <returns></returns>
        private AjustePragramacionObservacion GetObservacion(AjusteProgramacion pAjusteProgramacion, bool? pEsObra, bool? pEsSupervisor, int? pArchivoCargueId)
        {
            AjustePragramacionObservacion ajustePragramacionObservacion = pAjusteProgramacion.AjustePragramacionObservacion.ToList()
                        .Where(r =>
                                    r.Archivada != true &&
                                    r.Eliminado != true &&
                                    r.EsObra == pEsObra &&
                                    r.EsSupervisor == pEsSupervisor &&
                                    r.ArchivoCargueId == pArchivoCargueId
                                )
                        .OrderByDescending(r => r.AjustePragramacionObservacionId)
                        .FirstOrDefault();

            return ajustePragramacionObservacion;
        }
        
        /// <summary>
        /// Traer el histórico de las observaciones
        /// </summary>
        /// <param name="pAjusteProgramacion"></param>
        /// <param name="pEsObra"></param>
        /// <returns></returns>
        private List<AjustePragramacionObservacion> GetObservacionHistorico(AjusteProgramacion pAjusteProgramacion, bool? pEsObra)
        {
            List<AjustePragramacionObservacion> ajustePragramacionObservacion = pAjusteProgramacion.AjustePragramacionObservacion.ToList()
                            .Where(r =>
                                        r.Archivada == true &&
                                        r.Eliminado != true &&
                                        r.EsObra == pEsObra &&
                                        r.EsSupervisor == true
                                    )
                            .OrderByDescending(r => r.AjustePragramacionObservacionId)
                            .ToList();

            return ajustePragramacionObservacion;
        }

        /// <summary>
        /// Trae los archivos que se han cargado en registrar ajuste a la programación - Programación de obra
        /// </summary>
        /// <param name="pAjusteProgramacionId"></param>
        /// <returns></returns>
        public async Task<List<ArchivoCargue>> GetLoadAdjustProgrammingGrid(int pAjusteProgramacionId)
        {
            List<ArchivoCargue> listaCargas = _context.ArchivoCargue
                                                            .Where(a => a.ReferenciaId == pAjusteProgramacionId &&
                                                                        a.Eliminado != true &&
                                                                        a.OrigenId == int.Parse(OrigenArchivoCargue.AjusteProgramacionObra))
                                                            .ToList();


            listaCargas.ForEach(archivo =>
            {
                archivo.estadoCargue = archivo.CantidadRegistros == archivo.CantidadRegistrosValidos && archivo.Activo == true ? "Válido" : "Fallido";
                archivo.TempAjustePragramacionObservacion = _context.AjustePragramacionObservacion.Where(r => r.AjusteProgramacionId == pAjusteProgramacionId && r.EsObra == true && r.ArchivoCargueId == archivo.ArchivoCargueId && r.Eliminado != true && r.EsSupervisor != true).FirstOrDefault();

            });

            return listaCargas;

        }

        /// <summary>
        /// Trae los archivos que se han cargado en registrar ajuste a la programación - Flujo de inversión
        /// </summary>
        /// <param name="pAjusteProgramacionId"></param>
        /// <returns></returns>
        public async Task<List<ArchivoCargue>> GetLoadAdjustInvestmentFlowGrid(int pAjusteProgramacionId)
        {
            List<ArchivoCargue> listaCargas = _context.ArchivoCargue
                                                            .Where(a => a.ReferenciaId == pAjusteProgramacionId &&
                                                                   a.Eliminado != true &&
                                                                   a.OrigenId == int.Parse(OrigenArchivoCargue.AjusteFlujoInversion))
                                                            .ToList();


            listaCargas.ForEach(archivo =>
            {
                archivo.estadoCargue = archivo.CantidadRegistros == archivo.CantidadRegistrosValidos && archivo.Activo == true ? "Válido" : "Fallido";
                archivo.TempAjustePragramacionObservacion = _context.AjustePragramacionObservacion.Where(r => r.AjusteProgramacionId == pAjusteProgramacionId && r.EsObra != true && r.ArchivoCargueId == archivo.ArchivoCargueId && r.Eliminado != true && r.EsSupervisor != true).FirstOrDefault();

            });

            return listaCargas;


        }

        /// <summary>
        /// Cuando se ha devuelto la reprogramación, busca el archivo que estaba antes de poner el valor del arxchivo en null, en la tabla AjusteProgramacion
        /// </summary>
        /// <param name="pAjusteProgramacionId"></param>
        /// <param name="esProgramacion"></param>
        /// <returns></returns>
        public async Task<ArchivoCargue> GetFileReturn(int pAjusteProgramacionId, bool esProgramacion)
        {
            return _context.ArchivoCargue.Where(a => a.ReferenciaId == pAjusteProgramacionId &&
                                                                                    a.Eliminado != true &&
                                                                                    a.Activo == false &&
                                                                                    a.CantidadRegistros == a.CantidadRegistrosValidos &&
                                                                                    a.OrigenId == (esProgramacion ? int.Parse(OrigenArchivoCargue.AjusteProgramacionObra) : int.Parse(OrigenArchivoCargue.AjusteFlujoInversion)))
                                                                        .OrderByDescending(r => r.ArchivoCargueId)
                                                                        .FirstOrDefault();


        }

        #endregion

        #region VALIDACIONES
        /// <summary>
        /// Valida el registro completo para la pantalla de validar reprogramación
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<bool> ValidarRegistroCompletoValidacionAjusteProgramacion(int id)
        {
            bool esCompleto = true;

            AjusteProgramacion ap = await _context.AjusteProgramacion
                                                        .Include(cc => cc.AjustePragramacionObservacion)
                                                        .FirstOrDefaultAsync(cc => cc.AjusteProgramacionId == id);


            ap.ObservacionObra = GetObservacion(ap, true, true, ap.ArchivoCargueIdProgramacionObra);
            ap.ObservacionFlujo = GetObservacion(ap, false, true, ap.ArchivoCargueIdFlujoInversion);

            if (
                 ap.TieneObservacionesProgramacionObra == null ||
                 (ap.TieneObservacionesProgramacionObra == true && string.IsNullOrEmpty(ap.ObservacionObra != null ? ap.ObservacionObra.Observaciones : null)) ||
                 ap.TieneObservacionesFlujoInversion == null ||
                 (ap.TieneObservacionesFlujoInversion == true && string.IsNullOrEmpty(ap.ObservacionFlujo != null ? ap.ObservacionFlujo.Observaciones : null))
               )
            {
                esCompleto = false;
            }

            return esCompleto;
        }

        /// <summary>
        /// Valida registro completo para la pantalla de registrar reprogramación
        /// </summary>
        /// <param name="pAjusteProgramacionId"></param>
        private void VerificarRegistroCompletoAjusteProgramacion(int pAjusteProgramacionId)
        {
            bool esCompleto = true;

            AjusteProgramacion ajusteProgramacion = _context.AjusteProgramacion.Find(pAjusteProgramacionId);

            if (
                    ajusteProgramacion.ArchivoCargueIdProgramacionObra == null ||
                    ajusteProgramacion.ArchivoCargueIdFlujoInversion == null
                )
            {
                esCompleto = false;
            }

            ajusteProgramacion.RegistroCompleto = esCompleto;

            _context.SaveChanges();
        }
        /// <summary>
        /// Validación cuando se carga un archivo de forma correcto, cambio de estado y validación de regitro completo -- Programación de obra
        /// </summary>
        /// <param name="pIdDocument"></param>
        /// <param name="pUsuarioModifico"></param>
        /// <param name="pAjusteProgramacionId"></param>
        /// <returns></returns>
        public async Task<Respuesta> ValidateReprogrammingFile(string pIdDocument, string pUsuarioModifico, int pAjusteProgramacionId)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Load_Data_Ajuste_Programacion_Obra, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = string.Empty;
            try
            {
                int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.AjusteProgramacionObra, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);

                ArchivoCargue archivoCargue = _context.ArchivoCargue
                                                .Where(r => r.OrigenId == int.Parse(OrigenArchivoCargue.AjusteProgramacionObra) &&
                                                        r.Nombre.Trim().ToUpper().Equals(pIdDocument.ToUpper().Trim())
                                                      )
                                                .FirstOrDefault();

                AjusteProgramacion ajusteProgramacion = _context.AjusteProgramacion.Find(pAjusteProgramacionId);
                if (ajusteProgramacion != null)
                {
                    ajusteProgramacion.ArchivoCargueIdProgramacionObra = archivoCargue.ArchivoCargueId;
                    ajusteProgramacion.EstadoCodigo = ConstanCodigoEstadoAjusteProgramacion.En_proceso_de_ajuste_a_la_programacion;

                    VerificarRegistroCompletoAjusteProgramacion(pAjusteProgramacionId);
                    _context.SaveChanges();
                }

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioModifico, CreateEdit)
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
                      Code = GeneralCodes.Error,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.Error, idAccion, pUsuarioModifico, ex.InnerException.ToString())
                  };
            }
        }

        /// <summary>
        /// Validación cuando se carga un archivo de forma correcto, cambio de estado y validación de regitro completo -- Flujo de inversión
        /// </summary>
        /// <param name="pIdDocument"></param>
        /// <param name="pUsuarioModifico"></param>
        /// <param name="pAjusteProgramacionId"></param>
        /// <returns></returns>
        public async Task<Respuesta> ValidateInvestmentFlowFile(string pIdDocument, string pUsuarioModifico, int pAjusteProgramacionId)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Load_Data_Ajuste_Flujo_Inversion, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = string.Empty;
            try
            {
                int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.AjusteFlujoInversion, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);

                ArchivoCargue archivoCargue = _context.ArchivoCargue
                                                .Where(r => r.OrigenId == int.Parse(OrigenArchivoCargue.AjusteFlujoInversion) &&
                                                        r.Nombre.Trim().ToUpper().Equals(pIdDocument.ToUpper().Trim())
                                                      )
                                                .FirstOrDefault();

                AjusteProgramacion ajusteProgramacion = _context.AjusteProgramacion.Find(pAjusteProgramacionId);

                if (ajusteProgramacion != null)
                {
                    List<dynamic> listaFechas = new List<dynamic>();

                    ajusteProgramacion.ArchivoCargueIdFlujoInversion = archivoCargue.ArchivoCargueId;
                    ajusteProgramacion.EstadoCodigo = ConstanCodigoEstadoAjusteProgramacion.En_proceso_de_ajuste_a_la_programacion;

                    VerificarRegistroCompletoAjusteProgramacion(ajusteProgramacion.AjusteProgramacionId);
                    _context.SaveChanges();

                }

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioModifico, CreateEdit)
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
                      Code = GeneralCodes.Error,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.Error, idAccion, pUsuarioModifico, ex.InnerException.ToString())
                  };
            }
        }

        #endregion

        #region OBSERVACIONES
        /// <summary>
        /// Crea observaciones para flujo de inversión y programación de obra 
        /// </summary>
        /// <param name="pObservacion"></param>
        /// <param name="pUsuarioCreacion"></param>
        /// <param name="esObra"></param>
        /// <returns></returns>
        private async Task<Respuesta> CreateEditObservacionAjusteProgramacion(AjustePragramacionObservacion pObservacion, string pUsuarioCreacion, bool esObra)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Observacion_Ajuste_Programacion, (int)EnumeratorTipoDominio.Acciones);

            Respuesta respuesta = new Respuesta();
            try
            {
                string strCrearEditar = "";
                if (pObservacion.AjustePragramacionObservacionId > 0)
                {
                    strCrearEditar = "EDITAR OBSERVACION AJUSTE PROGRAMACION";
                    AjustePragramacionObservacion ajustePragramacionObservacion = _context.AjustePragramacionObservacion.Find(pObservacion.AjustePragramacionObservacionId);

                    ajustePragramacionObservacion.FechaModificacion = DateTime.Now;
                    ajustePragramacionObservacion.UsuarioModificacion = pUsuarioCreacion;

                    ajustePragramacionObservacion.Observaciones = pObservacion.Observaciones;

                }
                else
                {
                    strCrearEditar = "CREAR OBSERVACION AJUSTE PROGRAMACION";
                    pObservacion.FechaCreacion = DateTime.Now;
                    pObservacion.UsuarioCreacion = pUsuarioCreacion;
                    _context.AjustePragramacionObservacion.Add(pObservacion);

                }

                return respuesta;
            }
            catch (Exception ex)
            {
                return
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = true,
                        IsValidation = false,
                        Code = GeneralCodes.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.Error, idAccion, pObservacion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        /// <summary>
        /// Actualiza o crea observaciones
        /// </summary>
        /// <param name="pAjusteProgramacion"></param>
        /// <param name="esObra"></param>
        /// <param name="pUsuario"></param>
        /// <returns></returns>
        public async Task<Respuesta> CreateEditObservacionFile(AjusteProgramacion pAjusteProgramacion, bool esObra, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Observacion_Ajuste_Programacion, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";

            try
            {
                CreateEdit = "AJUSTE AJUSTE PROGRAMACION";
                int idObservacion = 0;

                if (pAjusteProgramacion.AjustePragramacionObservacion.Count() > 0)
                    idObservacion = pAjusteProgramacion.AjustePragramacionObservacion.FirstOrDefault().AjusteProgramacionId.Value;

                AjusteProgramacion ajusteProgramacion = _context.AjusteProgramacion.Find(pAjusteProgramacion.AjusteProgramacionId);

                await CreateEditObservacionAjusteProgramacion(pAjusteProgramacion.AjustePragramacionObservacion.FirstOrDefault(), pUsuario, esObra);

                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.OperacionExitosa, idAccion, pUsuario, CreateEdit)
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
                        Code = GeneralCodes.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                    };
            }
        }

        /// <summary>
        /// Actualiza o crea observaciones
        /// </summary>
        /// <param name="pAjusteProgramacion"></param>
        /// <param name="esObra"></param>
        /// <param name="pUsuario"></param>
        /// <returns></returns>
        public async Task<Respuesta> CreateEditObservacionAjusteProgramacion(AjusteProgramacion pAjusteProgramacion, bool esObra, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Observacion_Ajuste_Programacion, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";

            try
            {
                CreateEdit = "AJUSTE AJUSTE PROGRAMACION";
                int idObservacion = 0;

                if (pAjusteProgramacion.AjustePragramacionObservacion.Count() > 0)
                    idObservacion = pAjusteProgramacion.AjustePragramacionObservacion.FirstOrDefault().AjusteProgramacionId.Value;

                AjusteProgramacion ajusteProgramacion = _context.AjusteProgramacion.Find(pAjusteProgramacion.AjusteProgramacionId);

                ajusteProgramacion.UsuarioModificacion = pUsuario;
                ajusteProgramacion.FechaModificacion = DateTime.Now;

                ajusteProgramacion.EstadoCodigo = ConstanCodigoEstadoAjusteProgramacion.En_proceso_de_validacion_a_la_programacion;

                if (esObra == true)
                {
                    ajusteProgramacion.TieneObservacionesProgramacionObra = pAjusteProgramacion.TieneObservacionesProgramacionObra;

                    if (ajusteProgramacion.TieneObservacionesProgramacionObra.HasValue ? ajusteProgramacion.TieneObservacionesProgramacionObra.Value : false)
                    {

                        await CreateEditObservacionAjusteProgramacion(pAjusteProgramacion.AjustePragramacionObservacion.FirstOrDefault(), pUsuario, esObra);
                    }
                    else
                    {
                        AjustePragramacionObservacion observacionDelete = _context.AjustePragramacionObservacion.Find(idObservacion);

                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
                    }

                }
                else
                {
                    ajusteProgramacion.TieneObservacionesFlujoInversion = pAjusteProgramacion.TieneObservacionesFlujoInversion;

                    if (ajusteProgramacion.TieneObservacionesFlujoInversion.HasValue ? ajusteProgramacion.TieneObservacionesFlujoInversion.Value : false)
                    {

                        await CreateEditObservacionAjusteProgramacion(pAjusteProgramacion.AjustePragramacionObservacion.FirstOrDefault(), pUsuario, esObra);
                    }
                    else
                    {
                        AjustePragramacionObservacion observacionDelete = _context.AjustePragramacionObservacion.Find(idObservacion);

                        if (observacionDelete != null)
                            observacionDelete.Eliminado = true;
                    }

                }

                ajusteProgramacion.RegistroCompletoValidacion = await ValidarRegistroCompletoValidacionAjusteProgramacion(ajusteProgramacion.AjusteProgramacionId);
                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.OperacionExitosa, idAccion, pUsuario, CreateEdit)
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
                        Code = GeneralCodes.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                    };
            }
        }
        #endregion

        #region CARGUE ARCHIVOS

        #region Programación obra
        /// <summary>
        /// Validaciones del archivo de programación de obra
        /// </summary>
        /// <param name="pFile"></param>
        /// <param name="pFilePatch"></param>
        /// <param name="pUsuarioCreo"></param>
        /// <param name="pAjusteProgramacionId"></param>
        /// <param name="pContratacionProyectId"></param>
        /// <param name="pNovedadContractualId"></param>
        /// <param name="pContratoId"></param>
        /// <param name="pProyectoId"></param>
        /// <returns></returns>
        public async Task<Respuesta> UploadFileToValidateAdjustmentProgramming(IFormFile pFile, string pFilePatch, string pUsuarioCreo,
                                                                        int pAjusteProgramacionId, int pContratacionProyectId, int pNovedadContractualId,
                                                                        int pContratoId, int pProyectoId)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Validar_Excel_Ajuste_Programacion_Obra, (int)EnumeratorTipoDominio.Acciones);

            if (pAjusteProgramacionId > 0)
            {
                List<AjusteProgramacion> ajusteProgramacions = new List<AjusteProgramacion>();
                ajusteProgramacions = _context.AjusteProgramacion.Where(r => r.ContratacionProyectoId == pContratacionProyectId
                                                                        && r.NovedadContractualId == pNovedadContractualId
                                                                        && r.AjusteProgramacionId != pAjusteProgramacionId
                                                                        && (r.EstadoCodigo == ConstanCodigoEstadoAjusteProgramacion.En_proceso_de_ajuste_a_la_programacion
                                                                            || r.EstadoCodigo == ConstanCodigoEstadoAjusteProgramacion.Sin_ajustes
                                                                            || string.IsNullOrEmpty(r.EstadoCodigo)
                                                                            || r.EstadoCodigo == "0")).ToList();
                foreach (var ajuste in ajusteProgramacions)
                {
                    List<AjusteProgramacionObra> ajusteProgramacionObras = _context.AjusteProgramacionObra.Where(r => r.AjusteProgramacionId == ajuste.AjusteProgramacionId).ToList(); 
                    if(ajusteProgramacionObras.Count() > 0)
                        _context.AjusteProgramacionObra.RemoveRange(ajusteProgramacionObras);
                }
                //eliminar los otros registros de ajuste a la programación, se estaban duplicando, de esta forma dejamos solo el que esta entrando
                if (ajusteProgramacions.Count() > 0)
                    _context.AjusteProgramacion.RemoveRange(ajusteProgramacions);
            }
            else
            {
                AjusteProgramacion ajusteProgramacionTemp = new AjusteProgramacion();
                ajusteProgramacionTemp = _context.AjusteProgramacion.Where(r => r.ContratacionProyectoId == pContratacionProyectId && r.NovedadContractualId == pNovedadContractualId).FirstOrDefault();

                if (ajusteProgramacionTemp == null)
                {
                    ajusteProgramacionTemp.ContratacionProyectoId = pContratacionProyectId;
                    ajusteProgramacionTemp.NovedadContractualId = pNovedadContractualId;
                    ajusteProgramacionTemp.EstadoCodigo = ConstanCodigoEstadoAjusteProgramacion.En_proceso_de_ajuste_a_la_programacion;
                    ajusteProgramacionTemp.FechaCreacion = DateTime.Now;
                    ajusteProgramacionTemp.UsuarioCreacion = pUsuarioCreo;

                    _context.AjusteProgramacion.Add(ajusteProgramacionTemp);
                    _context.SaveChanges();
                }

                pAjusteProgramacionId = ajusteProgramacionTemp.AjusteProgramacionId;
            }

            AjusteProgramacion ajusteProgramacion = _context.AjusteProgramacion.Find(pAjusteProgramacionId);
            VContratoProyectoFechaEstimadaFinalizacion datosFechas = _context.VContratoProyectoFechaEstimadaFinalizacion.Where(r => r.ContratacionProyectoId == pContratacionProyectId).FirstOrDefault();

            int CantidadResgistrosValidos = 0;
            int CantidadRegistrosInvalidos = 0;
            int cantidadRutaCritica = 0;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            DocumentService _documentService = new DocumentService(_context, _commonService);

            ArchivoCargue archivoCarge = await _documentService.getSaveFile(pFile, pFilePatch, Int32.Parse(OrigenArchivoCargue.AjusteProgramacionObra), pAjusteProgramacionId);
            List<VFechasValidacionAjusteProgramacion> vFechas = _context.VFechasValidacionAjusteProgramacion.Where(r => r.ContratacionProyectoId == pContratacionProyectId).ToList();

            if (archivoCarge != null)
            {
                using (var stream = new MemoryStream())
                {
                    await pFile.CopyToAsync(stream);

                    using var package = new ExcelPackage(stream);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                    bool estructuraValidaValidacionGeneral = true;
                    string mensajeRespuesta = string.Empty;

                    int cantidadActividades = 0;
                    int posicion = 2;
                    while (!string.IsNullOrEmpty(worksheet.Cells[posicion++, 1].Text))
                    {
                        cantidadActividades++;
                    }

                    if (string.IsNullOrEmpty(worksheet.Cells[1, 7].Text))
                    {
                        worksheet.Cells[1, 7].Value = "Avance Ejecutado Acumulado";
                        worksheet.Cells[1, 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(242, 242, 242));
                        worksheet.Cells[1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[1, 7].Style.TextRotation = 0;
                        worksheet.Cells[1, 7].Style.WrapText = true;
                        worksheet.Cells[1, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                    }

                    for (int i = 2; i <= cantidadActividades + 1; i++)
                    {
                        bool tieneErrores = false;
                        try
                        {

                            TempProgramacion temp = new TempProgramacion();
                            //Auditoria
                            temp.ArchivoCargueId = archivoCarge.ArchivoCargueId;
                            temp.EstaValidado = false;
                            temp.FechaCreacion = DateTime.Now;
                            temp.UsuarioCreacion = pUsuarioCreo;
                            temp.AjusteProgramacionId = pAjusteProgramacionId;

                            #region Tipo Actividad
                            // #1
                            //Tipo Actividad
                            if (string.IsNullOrEmpty(worksheet.Cells[i, 1].Text))
                            {
                                worksheet.Cells[i, 1].AddComment("Dato Obligatorio", "Admin");
                                worksheet.Cells[i, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }
                            else if (new string[3] { "C", "SC", "I" }.Where(r => r == worksheet.Cells[i, 1].Text).Count() == 0)
                            {
                                worksheet.Cells[i, 1].AddComment("Tipo de actividad invalido", "Admin");
                                worksheet.Cells[i, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }
                            else
                            {
                                temp.TipoActividadCodigo = worksheet.Cells[i, 1].Text;
                            }

                            #endregion Tipo Actividad

                            #region Actividad

                            //#2
                            //Actividad
                            if (!string.IsNullOrEmpty(worksheet.Cells[i, 2].Text))
                            {
                                temp.Actividad = worksheet.Cells[i, 2].Text;
                            }
                            else
                            {
                                worksheet.Cells[i, 2].AddComment("Dato Obligatorio", "Admin");
                                worksheet.Cells[i, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }

                            #endregion Actividad

                            #region Marca de ruta critica

                            //#3
                            //Marca de ruta critica
                            if (string.IsNullOrEmpty(worksheet.Cells[i, 3].Text))
                            {
                                temp.EsRutaCritica = false;
                            }
                            else
                            {
                                if (temp.TipoActividadCodigo == "I" && worksheet.Cells[i, 3].Text == "1")
                                {
                                    temp.EsRutaCritica = true;
                                    cantidadRutaCritica++;
                                }
                                else if (temp.TipoActividadCodigo != "I" && worksheet.Cells[i, 3].Text == "1")
                                {
                                    worksheet.Cells[i, 3].AddComment("No se puede asignar ruta critica a este tipo de actividad", "Admin");
                                    worksheet.Cells[i, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[i, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                    tieneErrores = true;
                                }

                            }

                            #endregion Marca de ruta critica

                            #region Fecha Inicio

                            //#4
                            //Fecha Inicio
                            DateTime fechaTemp;
                            if (string.IsNullOrEmpty(worksheet.Cells[i, 4].Text))
                            {
                                worksheet.Cells[i, 4].AddComment("Dato Obligatorio", "Admin");
                                worksheet.Cells[i, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }
                            else
                            {
                                temp.FechaInicio = DateTime.TryParse(worksheet.Cells[i, 4].Text, out fechaTemp) ? fechaTemp : DateTime.MinValue;
                            }

                            #endregion Fecha Inicio

                            #region Fecha final

                            //#5
                            //Fecha final
                            if (string.IsNullOrEmpty(worksheet.Cells[i, 5].Text))
                            {
                                worksheet.Cells[i, 5].AddComment("Dato Obligatorio", "Admin");
                                worksheet.Cells[i, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }
                            else
                            {
                                temp.FechaFin = DateTime.TryParse(worksheet.Cells[i, 5].Text, out fechaTemp) ? fechaTemp : DateTime.MinValue;
                            }

                            #endregion Fecha final

                            #region validacion fechas

                            // validacion fechas
                            if (temp.FechaInicio.Date > temp.FechaFin.Date)
                            {
                                worksheet.Cells[i, 5].AddComment("La fecha no puede ser inferior a la fecha inicial", "Admin");
                                worksheet.Cells[i, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;

                            }

                            // fechas contrato
                            if (temp.FechaInicio.Date < datosFechas.FechaInicioProyecto.Value.Date)
                            {
                                worksheet.Cells[i, 4].AddComment("La fecha Inicial de la actividad no puede ser inferior a la fecha inicial del proyecto", "Admin");
                                worksheet.Cells[i, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }

                            if (temp.FechaFin.Date > datosFechas.FechaEstimadaFinProyecto.Value.Date)
                            {
                                worksheet.Cells[i, 5].AddComment("La fecha final de la actividad no puede ser mayor a la fecha final del proyecto", "Admin");
                                worksheet.Cells[i, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }

                            #endregion validacion fechas

                            #region Duracion

                            //#6
                            //Duracion
                            if (string.IsNullOrEmpty(worksheet.Cells[i, 6].Text))
                            {
                                worksheet.Cells[i, 6].AddComment("Dato Obligatorio", "Admin");
                                worksheet.Cells[i, 6].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                tieneErrores = true;
                            }
                            else
                            {
                                temp.Duracion = Int32.Parse(worksheet.Cells[i, 6].Text);
                            }

                            #endregion Duracion


                            #region Avance ejecutado acumulado

                            //#7
                            //Avance ejecutado acumulado
                            bool validacionFecha = false;

                            VFechasValidacionAjusteProgramacion vFechasTmp = vFechas.Where(r => !((temp.FechaInicio.Date < r.FechaInicio.Value.Date && temp.FechaFin.Date < r.FechaInicio.Value.Date) || (temp.FechaInicio.Date > r.FechaFin.Value.Date))).FirstOrDefault();
                            worksheet.Cells[i, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells[i, 7].Style.WrapText = true;
                            worksheet.Cells[i, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                            if (vFechasTmp != null)
                            {
                                validacionFecha = true;
                                worksheet.Cells[i, 7].Value = vFechasTmp.AvanceFisicoSemanal;
                                worksheet.Cells[i, 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[i, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(226,239,218));
                                worksheet.Cells[i, 7].AddComment("En estas fechas ya hay actividades ejecutadas, el registro es válido, pero tenga presente que no se podrá reprogramar actividades en estas fechas.", "Admin");
                                worksheet.Cells[i, 7].Comment.AutoFit = true;
                            }

                            #endregion Avance ejecutado acumulado


                            //Guarda Cambios en una tabla temporal solo si las fechas no tuvieron ejecución previamente 

                            if (!tieneErrores && !validacionFecha)
                            {
                                _context.TempProgramacion.Add(temp);
                                _context.SaveChanges();
                            }

                            if (temp.TempProgramacionId > 0 || validacionFecha)
                            {
                                CantidadResgistrosValidos++;
                            }
                            else
                            {
                                CantidadRegistrosInvalidos++;
                            }

                        }
                        catch (Exception ex)
                        {
                            CantidadRegistrosInvalidos++;
                        }
                    }

                    if (cantidadRutaCritica == 0 && worksheet.Cells[1, 1].Comment == null)
                    {
                        worksheet.Cells[1, 1].AddComment("Debe existir por lo menos una ruta critica", "Admin");
                        worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        CantidadRegistrosInvalidos++;
                        CantidadResgistrosValidos--;
                        estructuraValidaValidacionGeneral = false;
                        mensajeRespuesta = "Debe existir por lo menos una ruta critica";
                    }

                    ArchivoCargueRespuesta archivoCargueRespuesta = new ArchivoCargueRespuesta();

                    if (estructuraValidaValidacionGeneral == true)
                    {
                        archivoCargueRespuesta = new ArchivoCargueRespuesta
                        {
                            CantidadDeRegistros = cantidadActividades.ToString(),
                            CantidadDeRegistrosInvalidos = CantidadRegistrosInvalidos.ToString(),
                            CantidadDeRegistrosValidos = CantidadResgistrosValidos.ToString(),
                            LlaveConsulta = archivoCarge.Nombre,
                            CargaValida = true,

                        };
                    }
                    else if (estructuraValidaValidacionGeneral == false)
                    {
                        CantidadResgistrosValidos = 0;

                        archivoCargueRespuesta = new ArchivoCargueRespuesta
                        {
                            CantidadDeRegistros = archivoCarge.CantidadRegistros.ToString(),
                            CantidadDeRegistrosInvalidos = "0",
                            CantidadDeRegistrosValidos = "0",
                            LlaveConsulta = archivoCarge.Nombre,
                            CargaValida = false,
                            Mensaje = mensajeRespuesta,

                        };
                    }

                    //Actualizo el archivoCarge con la cantidad de registros validos , invalidos , y el total;
                    //-2 ya los registros comienzan desde esta fila
                    _context.Set<ArchivoCargue>()
                                  .Where(r => r.ArchivoCargueId == archivoCarge.ArchivoCargueId)
                                                      .Update(r => new ArchivoCargue()
                                                      {
                                                          CantidadRegistrosInvalidos = CantidadRegistrosInvalidos,
                                                          CantidadRegistrosValidos = CantidadResgistrosValidos,
                                                          CantidadRegistros = cantidadActividades
                                                      });

                    byte[] bin = package.GetAsByteArray();
                    string pathFile = archivoCarge.Ruta + "/" + archivoCarge.Nombre + ".xlsx";
                    File.WriteAllBytes(pathFile, bin);


                    return new Respuesta
                    {
                        Data = archivoCargueRespuesta,
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioCreo, "VALIDAR EXCEL PROGRAMACION")
                    };
                }




            }
            else
            {
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesCargueElegibilidad.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.Error, idAccion, pUsuarioCreo, "VALIDAR EXCEL PROGRAMACION")
                };
            }


        }
        #endregion

        #region Flujo de inversión
        /// <summary>
        /// Validaciones del archivo flujo de inversión
        /// </summary>
        /// <param name="pFile"></param>
        /// <param name="pFilePatch"></param>
        /// <param name="pUsuarioCreo"></param>
        /// <param name="pAjusteProgramacionId"></param>
        /// <param name="pContratacionProyectId"></param>
        /// <param name="pNovedadContractualId"></param>
        /// <param name="pContratoId"></param>
        /// <param name="pProyectoId"></param>
        /// <returns></returns>
        public async Task<Respuesta> UploadFileToValidateAdjustmentInvestmentFlow(IFormFile pFile, string pFilePatch, string pUsuarioCreo,
                                                                                int pAjusteProgramacionId, int pContratacionProyectId, int pNovedadContractualId,
                                                                                int pContratoId, int pProyectoId)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Validar_Excel_Ajuste_Flujo_Inversion, (int)EnumeratorTipoDominio.Acciones);

            if (pAjusteProgramacionId > 0)
            {
                List<AjusteProgramacion> ajusteProgramacions = new List<AjusteProgramacion>();
                ajusteProgramacions = _context.AjusteProgramacion.Where(r => r.ContratacionProyectoId == pContratacionProyectId
                                                                        && r.NovedadContractualId == pNovedadContractualId
                                                                        && r.AjusteProgramacionId != pAjusteProgramacionId
                                                                        && (r.EstadoCodigo == ConstanCodigoEstadoAjusteProgramacion.En_proceso_de_ajuste_a_la_programacion
                                                                            || r.EstadoCodigo == ConstanCodigoEstadoAjusteProgramacion.Sin_ajustes
                                                                            || string.IsNullOrEmpty(r.EstadoCodigo)
                                                                            || r.EstadoCodigo == "0")).ToList();
                foreach (var ajuste in ajusteProgramacions)
                {
                    List<AjusteProgramacionObra> ajusteProgramacionObras = _context.AjusteProgramacionObra.Where(r => r.AjusteProgramacionId == ajuste.AjusteProgramacionId).ToList();
                    if (ajusteProgramacionObras.Count() > 0)
                        _context.AjusteProgramacionObra.RemoveRange(ajusteProgramacionObras);
                }
                //eliminar los otros registros de ajuste a la programación, se estaban duplicando, de esta forma dejamos solo el que esta entrando
                if (ajusteProgramacions.Count() > 0)
                    _context.AjusteProgramacion.RemoveRange(ajusteProgramacions);
            }
            else
            {
                AjusteProgramacion ajusteProgramacionTemp = new AjusteProgramacion();
                ajusteProgramacionTemp = _context.AjusteProgramacion.Where(r => r.ContratacionProyectoId == pContratacionProyectId && r.NovedadContractualId == pNovedadContractualId).FirstOrDefault();

                if (ajusteProgramacionTemp == null)
                {
                    ajusteProgramacionTemp.ContratacionProyectoId = pContratacionProyectId;
                    ajusteProgramacionTemp.NovedadContractualId = pNovedadContractualId;
                    ajusteProgramacionTemp.EstadoCodigo = ConstanCodigoEstadoAjusteProgramacion.En_proceso_de_ajuste_a_la_programacion;
                    ajusteProgramacionTemp.FechaCreacion = DateTime.Now;
                    ajusteProgramacionTemp.UsuarioCreacion = pUsuarioCreo;

                    _context.AjusteProgramacion.Add(ajusteProgramacionTemp);
                    _context.SaveChanges();
                }

                pAjusteProgramacionId = ajusteProgramacionTemp.AjusteProgramacionId;
            }

            int CantidadRegistrosVacios = 0;
            int CantidadResgistrosValidos = 0;
            int CantidadRegistrosInvalidos = 0;

            AjusteProgramacion ajusteProgramacion = _context.AjusteProgramacion.Find(pAjusteProgramacionId);

            VContratoProyectoValorEstimado datosAdicion = _context.VContratoProyectoValorEstimado.Where(r => r.ProyectoId == pProyectoId && r.ContratoId == pContratoId).FirstOrDefault();
            VContratoProyectoFechaEstimadaFinalizacion datosFechas = _context.VContratoProyectoFechaEstimadaFinalizacion.Where(r => r.ProyectoId == pProyectoId && r.ContratoId == pContratoId).FirstOrDefault();
            ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion.Where(r => r.ContratoId == pContratoId && r.ProyectoId == pProyectoId).FirstOrDefault();
            //Numero semanas
            int numberOfWeeks = datosFechas.SemanasEstimadasProyecto ?? datosFechas.SemanasProyecto ?? 0;

            //Capitulos cargados
            List<Programacion> listaProgramacion = _context.Programacion
                                                                .Where(
                                                                        p => p.ContratoConstruccionId == contratoConstruccion.ContratoConstruccionId &&
                                                                        p.TipoActividadCodigo == "C")
                                                                .ToList();
            //CAPITULOS TEMPORALES
            List<TempProgramacion> tempProgramacion = _context.TempProgramacion.Where(r => r.AjusteProgramacionId == pAjusteProgramacionId && r.TipoActividadCodigo == "C").ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.Proyecto, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);
            DocumentService _documentService = new DocumentService(_context, _commonService);

            ArchivoCargue archivoCarge = await _documentService.getSaveFile(pFile, pFilePatch, Int32.Parse(OrigenArchivoCargue.AjusteFlujoInversion), pAjusteProgramacionId);

            bool estructuraValidaValidacionGeneral = true;
            string mensajeRespuesta = string.Empty;

            if (archivoCarge != null)
            {
                using (var stream = new MemoryStream())
                {
                    await pFile.CopyToAsync(stream);

                    using var package = new ExcelPackage(stream);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                    int cantidadCapitulos = 0;
                    int cantidadSemnas = 0;

                    int posicion = 2;
                    while (!string.IsNullOrEmpty(worksheet.Cells[1, posicion++].Text))
                    {
                        cantidadSemnas++;
                    }

                    posicion = 2;
                    while (!string.IsNullOrEmpty(worksheet.Cells[posicion++, 1].Text))
                    {
                        cantidadCapitulos++;
                    }

                    bool tieneErrores = false;
                    bool validacionError = false;
                    // valida numero semanas
                    if (numberOfWeeks != cantidadSemnas)
                    {
                        validacionError = true;
                        worksheet.Cells[1, 1].AddComment("Numero de semanas no es igual al del proyecto", "Admin");
                        worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        tieneErrores = true;
                        estructuraValidaValidacionGeneral = false;
                        mensajeRespuesta = "Numero de semanas no es igual al del proyecto";
                    }

                    //valida numero capitulos
                    if ((listaProgramacion.Count() + tempProgramacion.Count()) != cantidadCapitulos)
                    {
                        worksheet.Cells[1, 1].AddComment("Numero de capitulos no es igual a la programacion", "Admin");
                        worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        tieneErrores = true;
                        estructuraValidaValidacionGeneral = false;
                        mensajeRespuesta = "Numero de capitulos no es igual a la programacion";
                    }

                    decimal sumaTotal = 0;

                    // Capitulos
                    //int i = 2;
                    for (int i = 2; i <= cantidadCapitulos + 1; i++)
                    {
                        bool tieneErroresCapitulo = false;

                        try
                        {
                            // semanas
                            //int k = 2;
                            for (int k = 2; k < cantidadSemnas + 2; k++)
                            {

                                TempFlujoInversion temp = new TempFlujoInversion();
                                //Auditoria
                                temp.ArchivoCargueId = archivoCarge.ArchivoCargueId;
                                temp.EstaValidado = false;
                                temp.FechaCreacion = DateTime.Now;
                                temp.UsuarioCreacion = pUsuarioCreo;
                                temp.AjusteProgramacionId = pAjusteProgramacionId;
                                temp.Posicion = k - 2;
                                temp.PosicionCapitulo = i - 2;

                                #region Mes

                                // Mes
                                if (string.IsNullOrEmpty(worksheet.Cells[1, k].Text))
                                {
                                    worksheet.Cells[1, k].AddComment("Dato Obligatorio", "Admin");
                                    worksheet.Cells[1, k].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[1, k].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                    tieneErrores = true;
                                    tieneErroresCapitulo = true;
                                }
                                else
                                {
                                    temp.Semana = worksheet.Cells[1, k].Text;
                                }

                                #endregion Mes

                                #region Capitulo

                                //Capitulo
                                if (string.IsNullOrEmpty(worksheet.Cells[i, 1].Text))
                                {
                                    worksheet.Cells[i, 1].AddComment("Dato Obligatorio", "Admin");
                                    worksheet.Cells[i, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[i, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                    tieneErrores = true;
                                    tieneErroresCapitulo = true;
                                }

                                #endregion Capitulo

                                string valorTemp = worksheet.Cells[i, k].Text;

                                valorTemp = valorTemp.Replace("$", "").Replace(".", "").Replace(" ", "");

                                //Valor
                                temp.Valor = string.IsNullOrEmpty(valorTemp) ? 0 : decimal.Parse(valorTemp);
                                sumaTotal += temp.Valor.Value;

                                //Guarda Cambios en una tabla temporal

                                if (!tieneErrores)
                                {
                                    _context.TempFlujoInversion.Add(temp);
                                    _context.SaveChanges();
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            CantidadRegistrosInvalidos++;
                        }

                        if (tieneErroresCapitulo == true || tieneErrores == true)
                        {
                            CantidadRegistrosInvalidos++;
                        }
                        else
                        {
                            CantidadResgistrosValidos++;
                        }
                    }

                    if (datosAdicion.ValorTotalProyecto != sumaTotal && !validacionError)
                    {
                        worksheet.Cells[1, 1].AddComment("La suma de los valores no es igual al valor total de obra del proyecto", "Admin");
                        worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        estructuraValidaValidacionGeneral = false;
                        mensajeRespuesta = "La suma de los valores no es igual al valor total de obra del proyecto";
                    }

                    ArchivoCargueRespuesta archivoCargueRespuesta = new ArchivoCargueRespuesta();

                    if (estructuraValidaValidacionGeneral == true)
                    {
                        archivoCargueRespuesta = new ArchivoCargueRespuesta
                        {
                            CantidadDeRegistros = cantidadCapitulos.ToString(),
                            CantidadDeRegistrosInvalidos = CantidadRegistrosInvalidos.ToString(),
                            CantidadDeRegistrosValidos = CantidadResgistrosValidos.ToString(),
                            LlaveConsulta = archivoCarge.Nombre,
                            CargaValida = true,
                            Mensaje = mensajeRespuesta,

                        };
                    }
                    else if (estructuraValidaValidacionGeneral == false)
                    {
                        CantidadResgistrosValidos = 0;

                        archivoCargueRespuesta = new ArchivoCargueRespuesta
                        {
                            CantidadDeRegistros = archivoCarge.CantidadRegistros.ToString(),
                            CantidadDeRegistrosInvalidos = archivoCarge.CantidadRegistrosInvalidos.ToString(),
                            CantidadDeRegistrosValidos = archivoCarge.CantidadRegistrosValidos.ToString(),
                            LlaveConsulta = archivoCarge.Nombre,
                            CargaValida = false,
                            Mensaje = mensajeRespuesta,

                        };
                    }

                    //Actualizo el archivoCarge con la cantidad de registros validos , invalidos , y el total;
                    //-2 ya los registros comienzan desde esta fila
                    archivoCarge.CantidadRegistrosInvalidos = CantidadRegistrosInvalidos;
                    archivoCarge.CantidadRegistrosValidos = CantidadResgistrosValidos;
                    archivoCarge.CantidadRegistros = cantidadCapitulos;
                    _context.ArchivoCargue.Update(archivoCarge);

                    byte[] bin = package.GetAsByteArray();
                    string pathFile = archivoCarge.Ruta + "/" + archivoCarge.Nombre + ".xlsx";
                    File.WriteAllBytes(pathFile, bin);

                    return new Respuesta
                    {
                        Data = archivoCargueRespuesta,
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioCreo, "VALIDAR EXCEL FLUJO IN")
                    };
                }
            }
            else
            {
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesCargueElegibilidad.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.Error, idAccion, pUsuarioCreo, "VALIDAR EXCEL FLUJO INV")
                };
            }


        }
        #endregion
        #endregion

        #region ELIMINAR ARCHIVOS
        /// <summary>
        /// Elimina un archivo desde registrar reprogramación
        /// </summary>
        /// <param name="pArchivoCargueId"></param>
        /// <param name="pAjusteProgramacionId"></param>
        /// <param name="pUsuario"></param>
        /// <returns></returns>
        public async Task<Respuesta> DeleteAdjustProgrammingOrInvestmentFlow(int pArchivoCargueId, int pAjusteProgramacionId, string pUsuario, bool esProgramacionObra)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Archivo_Cargue, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                string strCrearEditar = string.Empty;

                if (pArchivoCargueId > 0)
                {
                    strCrearEditar = "ELIMINAR ARCHIVO CARGE - AJUSTE PROGRAMACIÓN OBRA";

                    await _context.Set<ArchivoCargue>().Where(r => r.ArchivoCargueId == pArchivoCargueId)
                                               .UpdateAsync(r => new ArchivoCargue()
                                               {
                                                   FechaModificacion = DateTime.Now,
                                                   UsuarioModificacion = pUsuario,
                                                   Eliminado = true
                                               });


                    await _context.Set<AjustePragramacionObservacion>().Where(r => r.ArchivoCargueId == pArchivoCargueId)
                                               .UpdateAsync(r => new AjustePragramacionObservacion()
                                               {
                                                   FechaModificacion = DateTime.Now,
                                                   UsuarioModificacion = pUsuario,
                                                   Eliminado = true,
                                                   AjusteProgramacionId = pAjusteProgramacionId
                                               });

                    //se pueden borrar , no es necesario dejar el registro.

                    if (esProgramacionObra)
                    {
                        List<AjusteProgramacionObra> listaAjusteProgramacionObra = _context.AjusteProgramacionObra.Where(r => r.AjusteProgramacionId == pAjusteProgramacionId).ToList();
                        _context.AjusteProgramacionObra.RemoveRange(listaAjusteProgramacionObra);

                        List<TempProgramacion> listaTempAjusteProgramacionObra = _context.TempProgramacion.Where(r => r.AjusteProgramacionId == pAjusteProgramacionId).ToList();
                        _context.TempProgramacion.RemoveRange(listaTempAjusteProgramacionObra);
                    }
                    else
                    {
                        List<AjusteProgramacionFlujo> listaAjusteProgramacionFlujo = _context.AjusteProgramacionFlujo.Where(r => r.AjusteProgramacionId == pAjusteProgramacionId).ToList();
                        _context.AjusteProgramacionFlujo.RemoveRange(listaAjusteProgramacionFlujo);

                        List<TempFlujoInversion> listaTempFlujoInversion = _context.TempFlujoInversion.Where(r => r.AjusteProgramacionId == pAjusteProgramacionId).ToList();
                        _context.TempFlujoInversion.RemoveRange(listaTempFlujoInversion);

                        List<SeguimientoSemanalTemp> listaSeguimientoSemanalTemp = _context.SeguimientoSemanalTemp.Where(r => r.AjusteProgramaionId == pAjusteProgramacionId).ToList();
                        _context.SeguimientoSemanalTemp.RemoveRange(listaSeguimientoSemanalTemp);
                    }


                    bool state = await ValidarRegistroCompletoValidacionAjusteProgramacion(pAjusteProgramacionId);
                    AjusteProgramacion ajusteProgramacion = _context.AjusteProgramacion.Find(pAjusteProgramacionId);
                    if (ajusteProgramacion != null)
                    {
                        ajusteProgramacion.FechaModificacion = DateTime.Now;
                        ajusteProgramacion.UsuarioModificacion = pUsuario;
                        ajusteProgramacion.RegistroCompletoValidacion = state;
                        ajusteProgramacion.AjusteProgramacionId = pAjusteProgramacionId;

                        if (ajusteProgramacion.ArchivoCargueIdFlujoInversion == pArchivoCargueId)
                            ajusteProgramacion.ArchivoCargueIdFlujoInversion = null;

                        if (ajusteProgramacion.ArchivoCargueIdProgramacionObra == pArchivoCargueId)
                            ajusteProgramacion.ArchivoCargueIdProgramacionObra = null;

                        _context.AjusteProgramacion.Update(ajusteProgramacion);
                        VerificarRegistroCompletoAjusteProgramacion(pAjusteProgramacionId);
                    }

                }

                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.OperacionExitosa, idAccion, pUsuario, strCrearEditar)
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
                        Code = GeneralCodes.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                    };
            }
        }
        #endregion

        #region CAMBIO DE ESTADOS
        /// <summary>
        ///  Envío al supervisor desde registrar reprogramación
        /// </summary>
        /// <param name="pAjusteProgramacionId"></param>
        /// <param name="pUsuarioCreacion"></param>
        /// <returns></returns>
        public async Task<Respuesta> EnviarAlSupervisorAjusteProgramacion(int pAjusteProgramacionId, string pUsuarioCreacion)
        {
            string CreateEdit = string.Empty;
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.EnviarAlSupervisorAjusteProgramacion, (int)EnumeratorTipoDominio.Acciones);
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.EnviarReprogramacionASupervisor));
            string strContenido = template.Contenido;

            try
            {
                AjusteProgramacion ajusteProgramacion = _context.AjusteProgramacion.Find(pAjusteProgramacionId);
                if (ajusteProgramacion != null)
                {
                    ContratacionProyecto cp = _context.ContratacionProyecto.Where(r => r.ContratacionProyectoId == ajusteProgramacion.ContratacionProyectoId).Include(r => r.Proyecto).FirstOrDefault();
                    if (cp != null)
                    {
                        Contrato contrato = _context.Contrato.Where(r => r.ContratacionId == cp.ContratacionId).FirstOrDefault();
                        strContenido = strContenido
                                      .Replace("[LLAVE_MEN]", cp.Proyecto.LlaveMen)
                                      .Replace("[NUMERO_CONTRATO]", contrato.NumeroContrato)
                                      ;

                    }
                }

                List<EnumeratorPerfil> perfilsEnviarCorreo = new List<EnumeratorPerfil> { EnumeratorPerfil.Supervisor };

                _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);

                ajusteProgramacion.EstadoCodigo = ConstanCodigoEstadoAjusteProgramacion.Enviada_al_supervisor;
                ajusteProgramacion.TieneObservacionesFlujoInversion = null;
                ajusteProgramacion.TieneObservacionesProgramacionObra = null;

                /**
                 * Elimino los archivos fallidos y las observaciones del interventor al momento del envío
                */
                List<ArchivoCargue> listaCargas = _context.ArchivoCargue
                                                .Where(a => a.ReferenciaId == pAjusteProgramacionId &&
                                                       a.ArchivoCargueId != ajusteProgramacion.ArchivoCargueIdFlujoInversion &&
                                                       a.ArchivoCargueId != ajusteProgramacion.ArchivoCargueIdProgramacionObra &&
                                                       a.Eliminado != true &&
                                                       a.OrigenId == int.Parse(OrigenArchivoCargue.AjusteFlujoInversion) || a.OrigenId == int.Parse(OrigenArchivoCargue.AjusteProgramacionObra))
                                                .ToList();


                listaCargas.ForEach(archivo =>
                {
                    if ((archivo.CantidadRegistros != archivo.CantidadRegistrosValidos) || archivo.Activo != true)
                    {
                        archivo.Eliminado = true;
                        _context.Set<AjustePragramacionObservacion>()
                                          .Where(r => r.AjusteProgramacionId == pAjusteProgramacionId && r.EsSupervisor != true && r.ArchivoCargueId == archivo.ArchivoCargueId)
                                                              .Update(r => new AjustePragramacionObservacion()
                                                              {
                                                                  FechaModificacion = DateTime.Now,
                                                                  UsuarioModificacion = pUsuarioCreacion,
                                                                  Eliminado = true
                                                              });
                    }
                });

                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioCreacion, CreateEdit)
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
                        Code = GeneralCodes.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.Error, idAccion, pUsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        /// <summary>
        /// Envío al interventor desde validar reprogramación
        /// </summary>
        /// <param name="pAjusteProgramacionId"></param>
        /// <param name="pUsuarioCreacion"></param>
        /// <returns></returns>
        public async Task<Respuesta> EnviarAlInterventor(int pAjusteProgramacionId, string pUsuarioCreacion)
        {
            string CreateEdit = string.Empty;
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.EnviarAlSupervisorAjusteProgramacion, (int)EnumeratorTipoDominio.Acciones);
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.EnviarReprogramacionAInterventor));
            string strContenido = template.Contenido;
            try
            {
                AjusteProgramacion ajusteProgramacion = _context.AjusteProgramacion.Find(pAjusteProgramacionId);
                if (ajusteProgramacion != null)
                {
                    ContratacionProyecto cp = _context.ContratacionProyecto.Where(r => r.ContratacionProyectoId == ajusteProgramacion.ContratacionProyectoId).Include(r => r.Proyecto).FirstOrDefault();
                    if (cp != null)
                    {
                        Contrato contrato = _context.Contrato.Where(r => r.ContratacionId == cp.ContratacionId).FirstOrDefault();
                        strContenido = strContenido
                                      .Replace("[LLAVE_MEN]", cp.Proyecto.LlaveMen)
                                      .Replace("[NUMERO_CONTRATO]", contrato.NumeroContrato)
                                      ;

                    }
                }

                List<EnumeratorPerfil> perfilsEnviarCorreo = new List<EnumeratorPerfil> { EnumeratorPerfil.Interventor };

                _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
                ajusteProgramacion.EstadoCodigo = ConstanCodigoEstadoAjusteProgramacion.Con_observaciones_del_supervisor;
                ajusteProgramacion.RegistroCompleto = false;
                ajusteProgramacion.RegistroCompletoValidacion = false;

                //si se devuelve por flujo de inversion borro para que empiece nuevo ciclo
                if (ajusteProgramacion.TieneObservacionesFlujoInversion == true)
                {
                    await _context.Set<ArchivoCargue>()
                                  .Where(r => r.ArchivoCargueId == ajusteProgramacion.ArchivoCargueIdFlujoInversion)
                                                      .UpdateAsync(r => new ArchivoCargue()
                                                      {
                                                          FechaModificacion = DateTime.Now,
                                                          UsuarioModificacion = pUsuarioCreacion,
                                                          Activo = false,
                                                      });
                    ajusteProgramacion.ArchivoCargueIdFlujoInversion = null;

                    //se pueden borrar , no es necesario dejar el registro.

                    List<AjusteProgramacionFlujo> listaAjusteProgramacionFlujo = _context.AjusteProgramacionFlujo.Where(r => r.AjusteProgramacionId == pAjusteProgramacionId).ToList();
                    _context.AjusteProgramacionFlujo.RemoveRange(listaAjusteProgramacionFlujo);

                    List<TempFlujoInversion> listaTempFlujoInversion = _context.TempFlujoInversion.Where(r => r.AjusteProgramacionId == pAjusteProgramacionId).ToList();
                    _context.TempFlujoInversion.RemoveRange(listaTempFlujoInversion);

                    List<SeguimientoSemanalTemp> listaSeguimientoSemanalTemp = _context.SeguimientoSemanalTemp.Where(r => r.AjusteProgramaionId == pAjusteProgramacionId).ToList();
                    _context.SeguimientoSemanalTemp.RemoveRange(listaSeguimientoSemanalTemp);
                }

                //si se devuelve por programacion borro para que empiece nuevo ciclo

                if (ajusteProgramacion.TieneObservacionesProgramacionObra == true)
                {
                    await _context.Set<ArchivoCargue>()
                                  .Where(r => r.ArchivoCargueId == ajusteProgramacion.ArchivoCargueIdProgramacionObra)
                                                      .UpdateAsync(r => new ArchivoCargue()
                                                      {
                                                          FechaModificacion = DateTime.Now,
                                                          UsuarioModificacion = pUsuarioCreacion,
                                                          Activo = false
                                                      });
                    ajusteProgramacion.ArchivoCargueIdProgramacionObra = null;

                    //se pueden borrar , no es necesario dejar el registro.

                    List<AjusteProgramacionObra> listaAjusteProgramacionObra = _context.AjusteProgramacionObra.Where(r => r.AjusteProgramacionId == pAjusteProgramacionId).ToList();
                    _context.AjusteProgramacionObra.RemoveRange(listaAjusteProgramacionObra);

                    List<TempProgramacion> listaTempAjusteProgramacionObra = _context.TempProgramacion.Where(r => r.AjusteProgramacionId == pAjusteProgramacionId).ToList();
                    _context.TempProgramacion.RemoveRange(listaTempAjusteProgramacionObra);
                }

                await _context.Set<AjustePragramacionObservacion>()
                                  .Where(r => r.AjusteProgramacionId == ajusteProgramacion.AjusteProgramacionId && r.EsSupervisor == true)
                                                      .UpdateAsync(r => new AjustePragramacionObservacion()
                                                      {
                                                          FechaModificacion = DateTime.Now,
                                                          UsuarioModificacion = pUsuarioCreacion,
                                                          Archivada = true
                                                      });

                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.OperacionExitosa, idAccion, pUsuarioCreacion, CreateEdit)
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
                        Code = GeneralCodes.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_ajuste_a_la_programacion, GeneralCodes.Error, idAccion, pUsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        #endregion

        #region REPROGRAMACIÓN

        /// <summary>
        /// Aprobación de reprogramación x parte del supervisor
        /// </summary>
        /// <param name="pAjusteProgramacionId"></param>
        /// <param name="pUsuarioCreacion"></param>
        /// <returns></returns>
        public async Task<Respuesta> AprobarAjusteProgramacion(int pAjusteProgramacionId, string pUsuarioCreacion)
        {
            string CreateEdit = string.Empty;
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Aprobar_Ajuste_Programacion, (int)EnumeratorTipoDominio.Acciones);
            bool terminoProgramacion = false, terminoFlujo = false;
            try
            {
                AjusteProgramacion ajusteProgramacion = _context.AjusteProgramacion.Find(pAjusteProgramacionId);
                
                if (ajusteProgramacion != null)
                {
                    ContratacionProyecto contratacionProyecto = _context.ContratacionProyecto.Where(r => r.ContratacionProyectoId == ajusteProgramacion.ContratacionProyectoId).FirstOrDefault();

                    Contrato contrato = _context.Contrato.Where(r => r.ContratacionId == contratacionProyecto.ContratacionId).Include(r => r.Contratacion).FirstOrDefault();

                    ContratoConstruccion contratoConstruccion = _context.ContratoConstruccion
                                                    .Where(cc => cc.ContratoId == contrato.ContratoId && cc.ProyectoId == contratacionProyecto.ProyectoId)
                                                    .FirstOrDefault();

                    Proyecto proyecto = _technicalRequirementsConstructionPhaseService.CalcularFechaInicioContrato(contratoConstruccion.ContratoConstruccionId);
                    VContratoProyectoFechaEstimadaFinalizacion datosFechas = _context.VContratoProyectoFechaEstimadaFinalizacion.Where(r => r.ContratacionProyectoId == ajusteProgramacion.ContratacionProyectoId).FirstOrDefault();

                    DateTime fechaFinalContrato = datosFechas.FechaEstimadaFinContrato ?? proyecto.FechaFinEtapaObra;
                    proyecto.FechaFinEtapaObra = fechaFinalContrato;
                    proyecto.FechaInicioEtapaObra = datosFechas.FechaInicioProyecto ?? proyecto.FechaInicioEtapaObra;

                    if (contratacionProyecto != null && contrato != null && contratoConstruccion != null)
                    {
                        terminoProgramacion = await TransferMassiveLoadAdjustmentProgramming(pUsuarioCreacion, ajusteProgramacion,contratoConstruccion.ContratoConstruccionId, proyecto);
                        terminoFlujo =  await TransferMassiveLoadAdjustmentInvestmentFlow(pUsuarioCreacion, ajusteProgramacion, contratoConstruccion.ContratoConstruccionId, proyecto, contratacionProyecto.ContratacionProyectoId);
                    }

                    if(!terminoProgramacion || !terminoFlujo)
                    {
                        return
                            new Respuesta
                            {
                                IsSuccessful = false,
                                IsException = true,
                                IsValidation = false,
                                Code = GeneralCodes.Error,
                                Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Validar_ajuste_a_la_programación, GeneralCodes.Error, idAccion, pUsuarioCreacion, "Ha ocurrido un error")
                            };
                    }

                    //Cambiar el estado a aprobado
                    await _context.Set<AjusteProgramacion>().Where(r => r.AjusteProgramacionId == pAjusteProgramacionId)
                                       .UpdateAsync(r => new AjusteProgramacion()
                                       {
                                           FechaModificacion = DateTime.Now,
                                           UsuarioModificacion = pUsuarioCreacion,
                                           EstadoCodigo = ConstanCodigoEstadoAjusteProgramacion.Aprobada_por_supervisor
                                       });
                    //Registrar ajuste a la programación de personal de obra
                    await _context.Set<Proyecto>().Where(r => r.ProyectoId == proyecto.ProyectoId)
                   .UpdateAsync(r => new Proyecto()
                   {
                       FechaModificacion = DateTime.Now,
                       UsuarioModificacion = pUsuarioCreacion,
                       EstadoProgramacionCodigo = r.EstadoProgramacionCodigo != ConstanCodigoEstadoProgramacionInicial.Sin_Programacion_Personal && r.EstadoProgramacionCodigo != ConstanCodigoEstadoProgramacionInicial.En_registro_programacion ? ConstanCodigoEstadoProgramacionInicial.Sin_aprobacion_ajuste_programacion_obra : r.EstadoProgramacionCodigo
                   });
                    //Registrar avance semanal
                    await _context.Set<ContratacionProyecto>().Where(r => r.ContratacionProyectoId == contratacionProyecto.ContratacionProyectoId)
                        .UpdateAsync(r => new ContratacionProyecto()
                        {
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pUsuarioCreacion,
                            EstadoObraCodigo = ConstanCodigoEstadoObraSeguimientoSemanal.Suspendida
                        });
                    await EnviarCorreoSupervisor(contrato);
                }

                _context.SaveChanges();
                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Validar_ajuste_a_la_programación, GeneralCodes.OperacionExitosa, idAccion, pUsuarioCreacion, CreateEdit)
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
                        Code = GeneralCodes.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Validar_ajuste_a_la_programación, GeneralCodes.Error, idAccion, pUsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }
        

        /// <summary>
        /// Agrega y/o Modifica Programación de obra
        /// </summary>
        /// <param name="ajusteProgramacion"></param>
        /// <param name="pUsuarioModifico"></param>
        /// <returns></returns>
        public async Task<bool> TransferMassiveLoadAdjustmentProgramming(string pUsuarioModifico, AjusteProgramacion ajusteProgramacion, int contratoConstruccionId, Proyecto proyecto)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Load_Data_Ajuste_Programacion_Obra, (int)EnumeratorTipoDominio.Acciones);

            Respuesta respuesta = new Respuesta();
            bool terminoCorrectamente = false;
            try
            {
                int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.AjusteProgramacionObra, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);
                
                if (ajusteProgramacion != null)
                {

                    List<TempProgramacion> listTempProgramacion = await _context.TempProgramacion
                                                                    .Where(r => r.AjusteProgramacionId == ajusteProgramacion.AjusteProgramacionId && r.EstaValidado != true)
                                                                    .ToListAsync();
                    //ELIMINAR AJUSTE PROGRAMACION OBRA ANTERIORES
                    List<AjusteProgramacionObra> listaProgramacion = _context.AjusteProgramacionObra
                                                                                .Where(p => p.AjusteProgramacionId == ajusteProgramacion.AjusteProgramacionId)
                                                                                .ToList();

                    _context.AjusteProgramacionObra.RemoveRange(listaProgramacion);
                    _context.SaveChanges();

                    if (listTempProgramacion.Count() > 0)
                    {
                        //PASAR DE LA TABLA TEMPORAL A LA TABLA AJUSTE PROGRAMACION OBRA
                        //COPIA LA INFORMACIÓN
                        foreach (TempProgramacion tempProgramacion in listTempProgramacion)
                        {
                            AjusteProgramacionObra programacion = new AjusteProgramacionObra()
                            {
                                AjusteProgramacionId = tempProgramacion.AjusteProgramacionId.Value,
                                TipoActividadCodigo = tempProgramacion.TipoActividadCodigo,
                                Actividad = tempProgramacion.Actividad,
                                EsRutaCritica = tempProgramacion.EsRutaCritica,
                                FechaInicio = tempProgramacion.FechaInicio,
                                FechaFin = tempProgramacion.FechaFin,
                                Duracion = tempProgramacion.Duracion,
                                ContratoConstruccionId = contratoConstruccionId
                            };

                            _context.AjusteProgramacionObra.Add(programacion);

                            //Temporal proyecto update
                            tempProgramacion.EstaValidado = true;
                            tempProgramacion.FechaModificacion = DateTime.Now;
                            tempProgramacion.UsuarioModificacion = pUsuarioModifico;
                            _context.TempProgramacion.Update(tempProgramacion);
                        }
                        _context.SaveChanges();

                        List<MesEjecucion> meses = _context.MesEjecucion.Where(x => x.ContratoConstruccionId == contratoConstruccionId).ToList();

                        int numeroMes = 1;
                        int idMes = 0;
                        for (DateTime fecha = proyecto.FechaInicioEtapaObra.Date; fecha <= proyecto.FechaFinEtapaObra.Date; fecha = fecha.AddMonths(1))
                        {
                            DateTime fechaInicio = fecha;
                            DateTime fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
                            MesEjecucion mesExistente = meses.Where(r => r.FechaInicio.Date == fechaInicio.Date && r.FechaFin.Date <= fechaFin.Date).FirstOrDefault();

                            if (mesExistente != null)
                            {
                                if(mesExistente.FechaFin.Date < fechaFin.Date)
                                {
                                    _context.Set<MesEjecucion>()
                                            .Where(r => r.MesEjecucionId == mesExistente.MesEjecucionId)
                                                                .Update(r => new MesEjecucion()
                                                                {
                                                                    FechaFin = fechaFin.Date
                                                                });
                                }
                                idMes = mesExistente.MesEjecucionId;
                                numeroMes = mesExistente.Numero;
                            }
                            else
                            {
                                MesEjecucion mes = new MesEjecucion()
                                {
                                    ContratoConstruccionId = contratoConstruccionId,
                                    Numero = numeroMes,
                                    FechaInicio = fecha,
                                    FechaFin = fecha.AddMonths(1).AddDays(-1),

                                };

                                _context.MesEjecucion.Add(mes);
                            }
                            numeroMes++;
                        }
                        _context.SaveChanges();

                        terminoCorrectamente = true;
                    }
                }

                return terminoCorrectamente;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// Agrega y/o Modifica flujo de inversión
        /// </summary>
        /// <param name="ajusteProgramacion"></param>
        /// <param name="pUsuarioModifico"></param>
        /// <returns></returns>
        public async Task<bool> TransferMassiveLoadAdjustmentInvestmentFlow(string pUsuarioModifico, AjusteProgramacion ajusteProgramacion, int contratoConstruccionId, Proyecto proyecto, int contratacionProyectoId)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Load_Data_Ajuste_Flujo_Inversion, (int)EnumeratorTipoDominio.Acciones);

            Respuesta respuesta = new Respuesta();

            try
            {
                int OrigenId = await _commonService.GetDominioIdByCodigoAndTipoDominio(OrigenArchivoCargue.AjusteFlujoInversion, (int)EnumeratorTipoDominio.Origen_Documento_Cargue);

                //ELIMINAR FLUJOS DE INVERSIÓN ANTERIORES
                List<AjusteProgramacionFlujo> listaFlujo = _context.AjusteProgramacionFlujo
                                        .Where(p => p.AjusteProgramacionId == ajusteProgramacion.AjusteProgramacionId)
                                        .ToList();


                List<SeguimientoSemanalTemp> listaSeguimientos = _context.SeguimientoSemanalTemp
                                                            .Where(p => p.AjusteProgramaionId == ajusteProgramacion.AjusteProgramacionId)
                                                            .ToList();
                // elimina los existentes
                _context.SeguimientoSemanalTemp.RemoveRange(listaSeguimientos);
                _context.AjusteProgramacionFlujo.RemoveRange(listaFlujo);

                List<TempFlujoInversion> listTempFlujoInversion = await _context.TempFlujoInversion
                                                .Where(r => r.AjusteProgramacionId == ajusteProgramacion.AjusteProgramacionId && r.EstaValidado != true)
                                                .ToListAsync();
                List<MesEjecucion> meses = _context.MesEjecucion.Where(x => x.ContratoConstruccionId == contratoConstruccionId).ToList();


                if (listTempFlujoInversion.Count() > 0)
                {
                    List<SeguimientoSemanal> seguimientoSemanals = _context.SeguimientoSemanal.Where(x => x.ContratacionProyectoId == contratacionProyectoId).ToList();

                    int numeroSemana = 1;
                    int idSeguimiento = 0;
                    for (DateTime fecha = proyecto.FechaInicioEtapaObra.Date; fecha <= proyecto.FechaFinEtapaObra.Date; fecha = fecha.AddDays(7))
                    {
                        DateTime fechaInicio = fecha;
                        DateTime fechaFin = fecha.AddDays(6);
                        SeguimientoSemanal seguimientoSemanalExistente = seguimientoSemanals.Where(r => r.FechaInicio.Value.Date == fechaInicio.Date && r.FechaFin.Value.Date <= fechaFin.Date).FirstOrDefault();

                        if (seguimientoSemanalExistente != null)
                        {
                            if (seguimientoSemanalExistente.FechaFin.Value.Date < fechaFin.Date)
                            {
                                _context.Set<SeguimientoSemanal>()
                                        .Where(r => r.SeguimientoSemanalId == seguimientoSemanalExistente.SeguimientoSemanalId)
                                                            .Update(r => new SeguimientoSemanal()
                                                            {
                                                                FechaFin = fechaFin.Date
                                                            });
                            }
                            idSeguimiento = seguimientoSemanalExistente.SeguimientoSemanalId;
                            numeroSemana = seguimientoSemanalExistente.NumeroSemana;
                        }
                        else
                        {
                            SeguimientoSemanal seguimientoSemanal = new SeguimientoSemanal()
                            {
                                ContratacionProyectoId = contratacionProyectoId,
                                Eliminado = false,
                                UsuarioCreacion = pUsuarioModifico,
                                FechaCreacion = DateTime.Now,
                                NumeroSemana = numeroSemana,
                                FechaInicio = fechaInicio,
                                FechaFin = fechaFin,
                                RegistroCompleto = false,
                            };

                            _context.SeguimientoSemanal.Add(seguimientoSemanal);
                        }

                        //PARA DEJAR EL REGISTRO DE LO QUE SE CREÓ EN REPROGRAMACIÓN
                        SeguimientoSemanalTemp seguimientoSemanalTmp = new SeguimientoSemanalTemp()
                        {
                            ContratacionProyectoId = contratacionProyectoId,
                            AjusteProgramaionId = ajusteProgramacion.AjusteProgramacionId,
                            Eliminado = false,
                            UsuarioCreacion = pUsuarioModifico,
                            FechaCreacion = DateTime.Now,
                            NumeroSemana = numeroSemana,
                            FechaInicio = fechaInicio,
                            FechaFin = fechaFin,
                            RegistroCompleto = false,
                        };
                        _context.SeguimientoSemanalTemp.Add(seguimientoSemanalTmp);

                        numeroSemana++;
                    }
                    _context.SaveChanges();

                    List<SeguimientoSemanalTemp> listaSeguimientosTmp = _context.SeguimientoSemanalTemp
                                            .Where(p => p.AjusteProgramaionId == ajusteProgramacion.AjusteProgramacionId)
                                            .ToList();

                    //PASAR DE LA TABLA TEMPORAL A LA TABLA FLUJO DE INVERSIÓN
                    //COPIA LA INFORMACIÓN
                    foreach (TempFlujoInversion tempFlujoInversion in listTempFlujoInversion)
                    {
                        int idMes = 0;

                        MesEjecucion mesExistente = meses.Where(m => (listaSeguimientosTmp[tempFlujoInversion.Posicion.Value].FechaInicio.Value.Date >= m.FechaInicio.Date && listaSeguimientosTmp[tempFlujoInversion.Posicion.Value].FechaFin.Value.Date <= m.FechaFin.Date) || 
                                                                     (listaSeguimientosTmp[tempFlujoInversion.Posicion.Value].FechaInicio.Value.Date <= m.FechaFin && listaSeguimientosTmp[tempFlujoInversion.Posicion.Value].FechaFin.Value.Date >= m.FechaFin))
                                                                    .FirstOrDefault();

                        if (mesExistente != null)
                        {
                            idMes = mesExistente.MesEjecucionId;
                        }

                        AjusteProgramacionFlujo flujo = new AjusteProgramacionFlujo()
                        {
                            AjusteProgramacionId = tempFlujoInversion.AjusteProgramacionId.Value,
                            Semana = tempFlujoInversion.Semana,
                            Valor = tempFlujoInversion.Valor,
                            SeguimientoSemanalId = listaSeguimientosTmp[tempFlujoInversion.Posicion.Value].SeguimientoSemanalTempId,
                            MesEjecucionId = idMes
                        };

                        _context.AjusteProgramacionFlujo.Add(flujo);

                        //Temporal proyecto update
                        tempFlujoInversion.EstaValidado = true;
                        tempFlujoInversion.FechaModificacion = DateTime.Now;
                        tempFlujoInversion.UsuarioModificacion = pUsuarioModifico;
                        _context.TempFlujoInversion.Update(tempFlujoInversion);

                    }
                    _context.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion

        #region CORREOS
        private async Task<bool> EnviarCorreoSupervisor(Contrato contrato)
        {

            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.AprobarRequisitosTecnicosFase2));
            string strContenido = await ReplaceVariables(template.Contenido, contrato);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                            new List<EnumeratorPerfil>
                                                      {
                                                          EnumeratorPerfil.Supervisor
                                                      };
            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
        }

        private async Task<string> ReplaceVariables(string template, Contrato contrato)
        {

            template  = template.Replace("[NUMEROCONTRATO]", contrato.NumeroContrato).
                                Replace("[FECHAVERIFICACION]", DateTime.Now.ToString("dd/MM/yyyy")).
                                Replace("[CANTIDADPROYECTOSASOCIADOS]", _context.ContratoConstruccion.Where(x => x.RegistroCompleto == true && x.ContratoId == contrato.ContratoId).Count().ToString()).
                                Replace("[CANTIDADPROYECTOSVERIFICADOS]", _context.ContratoConstruccion.Where(x => x.RegistroCompleto == true && x.ContratoId == contrato.ContratoId).Count().ToString()).
                                Replace("[TIPOCONTRATO]", contrato.Contratacion.TipoSolicitudCodigo == "1" ? "obra" : "interventoría");//OBRA O INTERVENTORIA
            //string 

            return template;
        }
        #endregion
    }
}
