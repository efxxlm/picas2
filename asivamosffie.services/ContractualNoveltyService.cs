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

namespace asivamosffie.services
{
    //CU 4_4_1 Registrar actuaciones de controversias contractuales

    public class ContractualNoveltyService : IContractualNoveltyService
    {

        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;


        public ContractualNoveltyService(devAsiVamosFFIEContext context, ICommonService commonService)
        {

            _commonService = commonService;
            _context = context;

            //_settings = settings;
        }



        #region Grids

        /// <summary>
        /// CU 4.1.3 - get list of information about contractual modification 
        /// </summary>
        /// <returns></returns>
        public async Task<List<VNovedadContractual>> GetListGrillaNovedadContractualObra()
        {
            List<VNovedadContractual> ListNovedades = new List<VNovedadContractual>();

            try
            {
                ListNovedades = await _context.VNovedadContractual
                                                    .Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                                                    .ToListAsync();

                return ListNovedades.OrderByDescending(r => r.FechaSolictud).ToList();
            }
            catch (Exception ex)
            {
                return ListNovedades;
            }
        }

        public async Task<List<VNovedadContractual>> GetListGrillaNovedadContractualInterventoria()
        {
            List<VNovedadContractual> ListNovedades = new List<VNovedadContractual>();

            try
            {
                ListNovedades = await _context.VNovedadContractual
                                                    .Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString())
                                                    .ToListAsync();

                return ListNovedades.OrderByDescending(r => r.FechaSolictud).ToList();
            }
            catch (Exception ex)
            {
                return ListNovedades;
            }
        }

        public async Task<List<VNovedadContractual>> GetListGrillaNovedadContractualGestionar()
        {
            List<VNovedadContractual> ListNovedades = new List<VNovedadContractual>();

            try
            {
                ListNovedades = await _context.VNovedadContractual
                                                    .Where(r => r.RegistroCompletoValidacion == true)
                                                    .ToListAsync();

                return ListNovedades.OrderByDescending(r => r.FechaSolictud).ToList();
            }
            catch (Exception ex)
            {
                return ListNovedades;
            }
        }

        #endregion Grids

        #region Gets

        /// <summary>
        /// CU 4.1.3 - get list of contracts assign to logged user for autocomplete
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<List<Contrato>> GetListContract(int userID)
        {
            var contratos = _context.Contrato
                                        .Where(x =>/*x.UsuarioInterventoria==userID*/ !(bool)x.Eliminado)
                                        .Include(x => x.Contratacion)
                                            .ThenInclude(x => x.Contratista)
                                        .ToList();

            List<Dominio> listDominioTipoDocumento = _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Documento).ToList();

            foreach (var contrato in contratos)
            {

                if (contrato?.Contratacion?.Contratista != null)
                {
                    contrato.Contratacion.Contratista.Contratacion = null;//para bajar el peso del consumo
                    contrato.Contratacion.Contratista.TipoIdentificacionNotMapped = contrato.Contratacion.Contratista.TipoIdentificacionCodigo == null ? "" : listDominioTipoDocumento.Where(x => x.Codigo == contrato.Contratacion.Contratista.TipoIdentificacionCodigo)?.FirstOrDefault()?.Nombre;
                    //contrato.TipoIntervencion no se de donde sale, preguntar, porque si es del proyecto, cuando sea multiproyecto cual traigo?
                }

            }
            return contratos;
        }

        public async Task<List<VProyectosXcontrato>> GetProyectsByContract(int pContratoId)
        {
            List<VProyectosXcontrato> listProyectos = _context.VProyectosXcontrato
                                        .Where(x => x.ContratoId == pContratoId)
                                        .ToList();

            return listProyectos;
        }

        public async Task<NovedadContractual> GetNovedadContractualById(int pId)
        {
            List<Dominio> listDominioTipoDocumento = _context.Dominio
                                                                .Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Documento)
                                                                .ToList();

            List<Dominio> listDominioTipoNovedad = _context.Dominio
                                                                .Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Novedad_Modificacion_Contractual)
                                                                .ToList();

            List<Dominio> listDominioMotivos = _context.Dominio
                                                                .Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Motivos_Novedad_contractual)
                                                                .ToList();

            List<Dominio> listDominioInstancias = _context.Dominio
                                                                .Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Instancias_de_seguimiento_tecnico)
                                                                .ToList();

            NovedadContractual novedadContractual = _context.NovedadContractual
                                                                .Where(r => r.NovedadContractualId == pId)
                                                                .Include(r => r.NovedadContractualObservaciones)
                                                                .Include(r => r.Contrato)
                                                                    .ThenInclude(r => r.Contratacion)
                                                                        .ThenInclude(r => r.Contratista)
                                                                .Include(r => r.NovedadContractualDescripcion)
                                                                    .ThenInclude(r => r.NovedadContractualClausula)
                                                                .Include(r => r.NovedadContractualDescripcion)
                                                                    .ThenInclude(r => r.NovedadContractualDescripcionMotivo)
                                                                .Include(r => r.Contrato)
                                                                    .ThenInclude(r => r.Contratacion)
                                                                        .ThenInclude(r => r.DisponibilidadPresupuestal)
                                                                .FirstOrDefault();

            novedadContractual.ProyectosContrato = _context.VProyectosXcontrato
                                                                .Where(r => r.ContratoId == novedadContractual.ContratoId)
                                                                .ToList();

            novedadContractual.ProyectosSeleccionado = _context.VProyectosXcontrato
                                                                    .Where(r => r.ProyectoId == novedadContractual.ProyectoId && r.ContratoId == novedadContractual.ContratoId)
                                                                    .FirstOrDefault();

            novedadContractual.InstanciaNombre = listDominioInstancias
                                                        .Where(r => r.Codigo == novedadContractual.InstanciaCodigo)
                                                        ?.FirstOrDefault()
                                                        ?.Nombre;

            foreach (NovedadContractualDescripcion novedadContractualDescripcion in novedadContractual.NovedadContractualDescripcion)
            {
                novedadContractualDescripcion.NombreTipoNovedad = listDominioTipoNovedad
                                                                        .Where(r => r.Codigo == novedadContractualDescripcion.TipoNovedadCodigo)
                                                                        ?.FirstOrDefault()
                                                                        ?.Nombre;
                foreach (NovedadContractualDescripcionMotivo motivo in novedadContractualDescripcion.NovedadContractualDescripcionMotivo)
                {
                    motivo.NombreMotivo = listDominioMotivos.Where(r => r.Codigo == motivo.MotivoNovedadCodigo)?.FirstOrDefault()?.Nombre;
                }


            }

            if (novedadContractual?.Contrato?.Contratacion?.Contratista != null)
            {
                novedadContractual.Contrato.Contratacion.Contratista.Contratacion = null;//para bajar el peso del consumo
                novedadContractual.Contrato.Contratacion.Contratista.TipoIdentificacionNotMapped = novedadContractual.Contrato.Contratacion.Contratista.TipoIdentificacionCodigo == null ? "" : listDominioTipoDocumento.Where(x => x.Codigo == novedadContractual.Contrato.Contratacion.Contratista.TipoIdentificacionCodigo)?.FirstOrDefault()?.Nombre;
                //contrato.TipoIntervencion no se de donde sale, preguntar, porque si es del proyecto, cuando sea multiproyecto cual traigo?
            }

            novedadContractual.ObservacionApoyo = getObservacion(novedadContractual, false, null);
            novedadContractual.ObservacionSupervisor = getObservacion(novedadContractual, true, null);
            novedadContractual.ObservacionTramite = getObservacion(novedadContractual, null, true);

            novedadContractual.ObservacionDevolucion = _context.NovedadContractualObservaciones.Find(novedadContractual.ObervacionSupervisorId);
            novedadContractual.ObservacionDevolucionTramite = _context.NovedadContractualObservaciones.Find(novedadContractual.ObservacionesDevolucionId);

            return novedadContractual;
        }


        #endregion Gets

        #region CreateEdit 

        public async Task<Respuesta> CreateEditNovedadContractual(NovedadContractual novedadContractual)
        {
            Respuesta _response = new Respuesta(); /*NovedadContractual novedadContractual*/

            Contrato contrato = _context.Contrato
                                            .Where(r => r.ContratoId == novedadContractual.ContratoId)
                                            .Include(r => r.Contratacion)
                                            .FirstOrDefault();

            int idAccionCrearEditarNovedadContractual = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Novedad_Contractual, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;

            try
            {
                if (novedadContractual != null)
                {

                    if (string.IsNullOrEmpty(novedadContractual.NovedadContractualId.ToString()) || novedadContractual.NovedadContractualId == 0)
                    {

                        int cantidadNovedades = _context.NovedadContractual.ToList().Count();

                        //Auditoria
                        strCrearEditar = "REGISTRAR NOVEDAD CONTRACTUAL";
                        novedadContractual.FechaCreacion = DateTime.Now;
                        novedadContractual.UsuarioCreacion = novedadContractual.UsuarioCreacion;
                        novedadContractual.NumeroSolicitud = "NOV-" + cantidadNovedades.ToString("000");

                        if (contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                        {
                            novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.En_proceso_de_registro;
                        }
                        if (contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString())
                        {
                            novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.En_proceso_de_verificacion;
                        }


                        novedadContractual.Eliminado = false;

                        if (novedadContractual.NovedadContractualDescripcion != null)
                        {
                            foreach (NovedadContractualDescripcion descripcion in novedadContractual.NovedadContractualDescripcion)
                            {
                                descripcion.UsuarioCreacion = novedadContractual.UsuarioCreacion;
                                descripcion.NovedadContractualId = novedadContractual.NovedadContractualId;

                                await CreateEditNovedadContractualDescripcion(descripcion);
                            }
                        }

                        novedadContractual.RegistroCompleto = Registrocompleto(novedadContractual);
                        _context.NovedadContractual.Add(novedadContractual);

                    }

                    else
                    {
                        strCrearEditar = "EDIT NOVEDAD CONTRACTUAL";

                        NovedadContractual novedadContractualOld = _context.NovedadContractual.Find(novedadContractual.NovedadContractualId);

                        novedadContractualOld.FechaModificacion = DateTime.Now;
                        novedadContractualOld.UsuarioModificacion = novedadContractual.UsuarioCreacion;

                        novedadContractualOld.FechaSolictud = novedadContractual.FechaSolictud;
                        novedadContractualOld.InstanciaCodigo = novedadContractual.InstanciaCodigo;
                        novedadContractualOld.FechaSesionInstancia = novedadContractual.FechaSesionInstancia;
                        novedadContractualOld.UrlSoporte = novedadContractual.UrlSoporte;

                        if (contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString())
                        {
                            novedadContractualOld.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.En_proceso_de_verificacion;
                        }

                        if (novedadContractual.NovedadContractualDescripcion != null)
                        {
                            foreach (NovedadContractualDescripcion descripcion in novedadContractual.NovedadContractualDescripcion)
                            {
                                descripcion.UsuarioCreacion = novedadContractual.UsuarioCreacion;
                                descripcion.NovedadContractualId = novedadContractual.NovedadContractualId;

                                await CreateEditNovedadContractualDescripcion(descripcion);
                            }
                        }

                        novedadContractualOld.RegistroCompleto = Registrocompleto(novedadContractualOld);
                    }

                    _context.SaveChanges();

                    return
                        new Respuesta
                        {
                            Data = novedadContractual,
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContractualNovelty.OperacionExitosa,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_solicitud_novedad_contractual,
                            ConstantMessagesContractualNovelty.OperacionExitosa,
                            idAccionCrearEditarNovedadContractual
                            , novedadContractual.UsuarioModificacion
                            , "EDITAR NOVEDAD CONTRACTUAL"
                            )
                        };

                }
                else
                {
                    return _response = new Respuesta
                    {
                        IsSuccessful = false,
                        IsValidation = false,
                        Data = null,
                        Code = ConstantMessagesContractualNovelty.RecursoNoEncontrado,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_solicitud_novedad_contractual, ConstantMessagesContractualNovelty.RecursoNoEncontrado, idAccionCrearEditarNovedadContractual, novedadContractual.UsuarioCreacion, strCrearEditar)
                    };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_solicitud_novedad_contractual, GeneralCodes.Error, idAccionCrearEditarNovedadContractual, novedadContractual.UsuarioCreacion, strCrearEditar)
                };
            }
        }

        public async Task<Respuesta> CreateEditNovedadContractualDescripcion(NovedadContractualDescripcion novedadContractualDescripcion)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Novedad_Contractual, (int)EnumeratorTipoDominio.Acciones);//ERROR VALIDAR ACCIONES

            string strCrearEditar = "";
            try
            {

                if (string.IsNullOrEmpty(novedadContractualDescripcion.NovedadContractualDescripcionId.ToString()) || novedadContractualDescripcion.NovedadContractualDescripcionId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR NOVEDAD CONTRACTUAL DESCRIPCION";
                    novedadContractualDescripcion.FechaCreacion = DateTime.Now;
                    novedadContractualDescripcion.Eliminado = false;

                    _context.NovedadContractualDescripcion.Add(novedadContractualDescripcion);

                }
                else
                {
                    strCrearEditar = "EDIT NOVEDAD CONTRACTUAL DESCRIPCION";
                    NovedadContractualDescripcion novedadDescripcionOld = _context.NovedadContractualDescripcion.Find(novedadContractualDescripcion.NovedadContractualDescripcionId);

                    //Auditoria
                    novedadDescripcionOld.FechaModificacion = DateTime.Now;
                    novedadDescripcionOld.UsuarioCreacion = novedadContractualDescripcion.UsuarioCreacion;

                    //Registros

                    novedadDescripcionOld.MotivoNovedadCodigo = novedadContractualDescripcion.MotivoNovedadCodigo;
                    novedadDescripcionOld.ResumenJustificacion = novedadContractualDescripcion.ResumenJustificacion;
                    novedadDescripcionOld.EsDocumentacionSoporte = novedadContractualDescripcion.EsDocumentacionSoporte;
                    novedadDescripcionOld.ConceptoTecnico = novedadContractualDescripcion.ConceptoTecnico;
                    novedadDescripcionOld.FechaConcepto = novedadContractualDescripcion.FechaConcepto;
                    novedadDescripcionOld.FechaInicioSuspension = novedadContractualDescripcion.FechaInicioSuspension;
                    novedadDescripcionOld.FechaFinSuspension = novedadContractualDescripcion.FechaFinSuspension;
                    novedadDescripcionOld.PresupuestoAdicionalSolicitado = novedadContractualDescripcion.PresupuestoAdicionalSolicitado;
                    novedadDescripcionOld.PlazoAdicionalDias = novedadContractualDescripcion.PlazoAdicionalDias;
                    novedadDescripcionOld.PlazoAdicionalMeses = novedadContractualDescripcion.PlazoAdicionalMeses;
                    novedadDescripcionOld.ClausulaModificar = novedadContractualDescripcion.ClausulaModificar;
                    novedadDescripcionOld.AjusteClausula = novedadContractualDescripcion.AjusteClausula;
                    novedadDescripcionOld.RegistroCompleto = novedadContractualDescripcion.RegistroCompleto;
                    novedadDescripcionOld.NumeroRadicado = novedadContractualDescripcion.NumeroRadicado;

                    _context.NovedadContractualDescripcion.Update(novedadDescripcionOld);
                }

                foreach (NovedadContractualClausula clausula in novedadContractualDescripcion.NovedadContractualClausula)
                {
                    clausula.UsuarioCreacion = novedadContractualDescripcion.UsuarioCreacion;
                    //clausula.NovedadContractualDescripcionId = novedadContractualDescripcion.NovedadContractualDescripcionId;

                    await CreateEditNovedadContractualDescripcionClausula(clausula);
                }

                //las borro los motivos para crearlos de nuevo
                List<NovedadContractualDescripcionMotivo> listaMotivos = _context.NovedadContractualDescripcionMotivo
                                                                                       .Where(r => r.NovedadContractualDescripcionId == novedadContractualDescripcion.NovedadContractualDescripcionId)
                                                                                       .ToList();

                _context.NovedadContractualDescripcionMotivo.RemoveRange(listaMotivos);

                // creo los motivos de nuevo
                foreach (NovedadContractualDescripcionMotivo motivo in novedadContractualDescripcion.NovedadContractualDescripcionMotivo)
                {
                    motivo.UsuarioCreacion = novedadContractualDescripcion.UsuarioCreacion;
                    motivo.FechaCreacion = DateTime.Now;

                    _context.NovedadContractualDescripcionMotivo.Add(motivo);
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Grupo, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccion, novedadContractualDescripcion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> CreateEditNovedadContractualDescripcionClausula(NovedadContractualClausula novedadContractualClausula)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Novedad_Contractual, (int)EnumeratorTipoDominio.Acciones);//ERROR VALIDAR ACCIONES

            string strCrearEditar = "";
            try
            {

                if (string.IsNullOrEmpty(novedadContractualClausula.NovedadContractualClausulaId.ToString()) || novedadContractualClausula.NovedadContractualClausulaId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR NOVEDAD CONTRACTUAL CLAUSULA";
                    novedadContractualClausula.FechaCreacion = DateTime.Now;
                    novedadContractualClausula.Eliminado = false;

                    _context.NovedadContractualClausula.Add(novedadContractualClausula);
                }
                else
                {
                    strCrearEditar = "EDIT NOVEDAD CONTRACTUAL CLAUSULA";
                    NovedadContractualClausula clausulaOld = _context.NovedadContractualClausula.Find(novedadContractualClausula.NovedadContractualClausulaId);

                    //Auditoria
                    clausulaOld.FechaModificacion = DateTime.Now;
                    clausulaOld.UsuarioCreacion = novedadContractualClausula.UsuarioCreacion;

                    //Registros

                    clausulaOld.AjusteSolicitadoAclausula = novedadContractualClausula.AjusteSolicitadoAclausula;
                    clausulaOld.ClausulaAmodificar = novedadContractualClausula.ClausulaAmodificar;

                    _context.NovedadContractualClausula.Update(clausulaOld);
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesProcesoSeleccion.ErrorInterno,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Grupo, ConstantMessagesProcesoSeleccion.ErrorInterno, idAccion, novedadContractualClausula.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> CreateEditObservacion(NovedadContractual pNovedadContractual, bool? esSupervisor, bool? esTramite)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Novedad_Contractual, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";

            try
            {
                CreateEdit = "EDIT NOVEDAD CONTRACTUAL";
                int idObservacion = 0;

                if (pNovedadContractual.NovedadContractualObservaciones.Count() > 0)
                    idObservacion = pNovedadContractual.NovedadContractualObservaciones.FirstOrDefault().NovedadContractualObservacionesId;

                NovedadContractual novedadContractual = _context.NovedadContractual.Find(pNovedadContractual.NovedadContractualId);

                novedadContractual.UsuarioModificacion = pNovedadContractual.UsuarioCreacion;
                novedadContractual.FechaModificacion = DateTime.Now;

                if (esTramite == true)
                {

                }
                else
                {
                    if (esSupervisor == true)
                    {
                        novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.En_proceso_de_validacion;

                        novedadContractual.TieneObservacionesSupervisor = pNovedadContractual.TieneObservacionesSupervisor;

                        if (novedadContractual.TieneObservacionesSupervisor.HasValue ? novedadContractual.TieneObservacionesSupervisor.Value : false)
                        {

                            await CreateEditObservacionSeguimientoDiario(pNovedadContractual.NovedadContractualObservaciones.FirstOrDefault(), pNovedadContractual.UsuarioCreacion);
                        }
                        else
                        {
                            NovedadContractualObservaciones observacionDelete = _context.NovedadContractualObservaciones.Find(idObservacion);

                            if (observacionDelete != null)
                                observacionDelete.Eliminado = true;
                        }

                        novedadContractual.RegistroCompletoValidacion = await ValidarRegistroCompletoValidacion(novedadContractual.NovedadContractualId, esSupervisor, esTramite);
                        if (novedadContractual.RegistroCompletoValidacion.Value)
                        {
                            novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.En_proceso_de_validacion;
                        }
                        else
                        {
                            novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.Sin_validar;
                        }


                    }
                    else
                    {
                        novedadContractual.TieneObservacionesApoyo = pNovedadContractual.TieneObservacionesApoyo;

                        if (novedadContractual.TieneObservacionesApoyo.HasValue ? novedadContractual.TieneObservacionesApoyo.Value : true)
                        {
                            await CreateEditObservacionSeguimientoDiario(pNovedadContractual.NovedadContractualObservaciones.FirstOrDefault(), pNovedadContractual.UsuarioCreacion);
                        }
                        else
                        {
                            NovedadContractualObservaciones observacionDelete = _context.NovedadContractualObservaciones.Find(idObservacion);

                            if (observacionDelete != null)
                                observacionDelete.Eliminado = true;
                        }

                        novedadContractual.RegistroCompletoVerificacion = await ValidarRegistroCompletoVerificacion(novedadContractual.NovedadContractualId, esSupervisor, esTramite);
                        if (novedadContractual.RegistroCompletoVerificacion.Value)
                        {
                            novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.En_proceso_de_verificacion;
                        }
                        else
                        {
                            novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.Con_novedad_aprobada_por_interventor;
                        }
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_seguimiento_diario, GeneralCodes.OperacionExitosa, idAccion, pNovedadContractual.UsuarioCreacion, CreateEdit)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_seguimiento_diario, GeneralCodes.Error, idAccion, pNovedadContractual.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> CreateEditNovedadContractualTramite(NovedadContractual novedadContractual)
        {
            Respuesta _response = new Respuesta(); /*NovedadContractual novedadContractual*/

            Contrato contrato = _context.Contrato
                                            .Where(r => r.ContratoId == novedadContractual.ContratoId)
                                            .Include(r => r.Contratacion)
                                            .FirstOrDefault();

            int idAccionCrearEditarNovedadContractual = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Novedad_Contractual_Tramite, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;

            try
            {
                strCrearEditar = "EDIT NOVEDAD CONTRACTUAL TRAMITE";

                NovedadContractual novedadContractualOld = _context.NovedadContractual.Find(novedadContractual.NovedadContractualId);

                novedadContractualOld.FechaModificacion = DateTime.Now;
                novedadContractualOld.UsuarioModificacion = novedadContractual.UsuarioCreacion;

                novedadContractualOld.FechaEnvioGestionContractual = novedadContractual.FechaEnvioGestionContractual;
                novedadContractualOld.EstadoProcesoCodigo = novedadContractual.EstadoProcesoCodigo;
                novedadContractualOld.FechaAprobacionGestionContractual = novedadContractual.FechaAprobacionGestionContractual;
                novedadContractualOld.AbogadoRevisionId = novedadContractual.AbogadoRevisionId;
                novedadContractualOld.DeseaContinuar = novedadContractual.DeseaContinuar;
                novedadContractualOld.FechaEnvioActaContratistaObra = novedadContractual.FechaEnvioActaContratistaObra;
                novedadContractualOld.FechaFirmaActaContratistaObra = novedadContractual.FechaFirmaActaContratistaObra;
                novedadContractualOld.FechaEnvioActaContratistaInterventoria = novedadContractual.FechaEnvioActaContratistaInterventoria;
                novedadContractualOld.FechaFirmaContratistaInterventoria = novedadContractual.FechaFirmaContratistaInterventoria;
                novedadContractualOld.FechaEnvioActaApoyo = novedadContractual.FechaEnvioActaApoyo;
                novedadContractualOld.FechaFirmaApoyo = novedadContractual.FechaFirmaApoyo;
                novedadContractualOld.FechaEnvioActaSupervisor = novedadContractual.FechaEnvioActaSupervisor;
                novedadContractualOld.FechaFirmaSupervisor = novedadContractual.FechaFirmaSupervisor;
                novedadContractualOld.UrlSoporteFirmas = novedadContractual.UrlSoporteFirmas;

                novedadContractualOld.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.En_proceso_de_aprobacion;

                novedadContractualOld.RegistroCompletoTramiteNovedades = RegistrocompletoTramite(novedadContractualOld);

                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        Data = novedadContractual,
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = ConstantMessagesContractualNovelty.OperacionExitosa,
                        Message =
                        await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_solicitud_novedad_contractual,
                        ConstantMessagesContractualNovelty.OperacionExitosa,
                        idAccionCrearEditarNovedadContractual
                        , novedadContractual.UsuarioModificacion
                        , "EDITAR NOVEDAD CONTRACTUAL"
                        )
                    };


            }
            catch (Exception ex)
            {
                return _response = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_solicitud_novedad_contractual, GeneralCodes.Error, idAccionCrearEditarNovedadContractual, novedadContractual.UsuarioCreacion, strCrearEditar)
                };
            }
        }

        #endregion CreateEdit 

        #region business

        /// <summary>
        /// CU 4.1.3 - Delete a contractual modification 
        /// </summary>
        /// <param name="pNovedadContractualId"></param>
        /// <param name="pUsuario"></param>
        /// <returns></returns>
        public async Task<Respuesta> EliminarNovedadContractual(int pNovedadContractualId, string pUsuario)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Controversia_Actuacion, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            NovedadContractual novedadContractual = _context.NovedadContractual.Find(pNovedadContractualId);

            try
            {

                if (novedadContractual != null)
                {
                    strCrearEditar = "Eliminar NOVEDAD CONTRACTUAL";
                    novedadContractual.FechaModificacion = DateTime.Now;
                    novedadContractual.UsuarioModificacion = pUsuario;
                    novedadContractual.Eliminado = true;
                    _context.NovedadContractual.Update(novedadContractual);

                    _context.SaveChanges();

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.EliminacionExitosa,
                    idAccion,
                    "",//controversiaActuacion.UsuarioCreacion,
                    strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.Error,
                    idAccion,
                    "",//controversiaActuacion.UsuarioCreacion, 
                    ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> AprobarSolicitud(int pNovedadContractualId, string pUsuario)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.AprobarNovedadContractual, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            NovedadContractual novedadContractual = _context.NovedadContractual.Find(pNovedadContractualId);

            try
            {

                if (novedadContractual != null)
                {
                    strCrearEditar = "APROBAR NOVEDAD CONTRACTUAL";
                    novedadContractual.FechaModificacion = DateTime.Now;
                    novedadContractual.UsuarioModificacion = pUsuario;
                    novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.Con_novedad_aprobada_por_interventor;
                    _context.NovedadContractual.Update(novedadContractual);

                    _context.SaveChanges();

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.OperacionExitosa,
                    idAccion,
                    "",//controversiaActuacion.UsuarioCreacion,
                    strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.Error,
                    idAccion,
                    "",//controversiaActuacion.UsuarioCreacion, 
                    ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> EnviarAlSupervisor(int pNovedadContractualId, string pUsuario)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.VerificarNovedadContractual, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            NovedadContractual novedadContractual = _context.NovedadContractual.Find(pNovedadContractualId);

            try
            {

                if (novedadContractual != null)
                {
                    strCrearEditar = "VERIFICAR NOVEDAD CONTRACTUAL";
                    novedadContractual.FechaModificacion = DateTime.Now;
                    novedadContractual.UsuarioModificacion = pUsuario;
                    novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.Sin_validar;
                    _context.NovedadContractual.Update(novedadContractual);

                    _context.SaveChanges();

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.OperacionExitosa,
                    idAccion,
                    "",//controversiaActuacion.UsuarioCreacion,
                    strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.Error,
                    idAccion,
                    "",//controversiaActuacion.UsuarioCreacion, 
                    ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> TramitarSolicitud(int pNovedadContractualId, string pUsuario)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Tramitar_Novedad_Contractual, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            NovedadContractual novedadContractual = _context.NovedadContractual.Find(pNovedadContractualId);

            try
            {

                if (novedadContractual != null)
                {
                    strCrearEditar = "TRAMITAR NOVEDAD CONTRACTUAL";
                    novedadContractual.FechaModificacion = DateTime.Now;
                    novedadContractual.UsuarioModificacion = pUsuario;
                    novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.Sin_tramite;
                    _context.NovedadContractual.Update(novedadContractual);

                    _context.SaveChanges();

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.OperacionExitosa,
                    idAccion,
                    "",//controversiaActuacion.UsuarioCreacion,
                    strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.Error,
                    idAccion,
                    "",//controversiaActuacion.UsuarioCreacion, 
                    ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> DevolverSolicitud(int pNovedadContractualId, string pUsuario)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Devolver_Novedad_Contractual, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = "DEVOLVER NOVEDAD CONTRACTUAL";

            try
            {

                NovedadContractual novedadContractual = _context.NovedadContractual
                                                                    .Where(r => r.NovedadContractualId == pNovedadContractualId)
                                                                    .Include(r => r.NovedadContractualObservaciones)
                                                                    .FirstOrDefault();

                novedadContractual.UsuarioModificacion = pUsuario;
                novedadContractual.FechaModificacion = DateTime.Now;

                if (novedadContractual.TieneObservacionesApoyo == true)
                {

                    NovedadContractualObservaciones observacionesApoyo = getObservacion(novedadContractual, false, null);

                    if (observacionesApoyo != null)
                        observacionesApoyo.Archivado = true;

                }

                if (novedadContractual.TieneObservacionesSupervisor == true)
                {

                    NovedadContractualObservaciones observacionesSupervisor = getObservacion(novedadContractual, true, null);

                    if (observacionesSupervisor != null)
                    {
                        observacionesSupervisor.Archivado = true;
                        novedadContractual.ObervacionSupervisorId = observacionesSupervisor.NovedadContractualObservacionesId;
                    }

                }

                novedadContractual.EstadoCodigo = ConstanCodigoEstadoNovedadContractual.Con_observaciones_del_supervisor;
                novedadContractual.TieneObservacionesApoyo = null;
                novedadContractual.TieneObservacionesSupervisor = null;
                novedadContractual.RegistroCompleto = null;
                novedadContractual.RegistroCompletoValidacion = null;
                novedadContractual.RegistroCompletoVerificacion = null;

                _context.SaveChanges();



                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = novedadContractual,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.OperacionExitosa,
                    idAccion,
                    "",//controversiaActuacion.UsuarioCreacion,
                    strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = ex,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                    ConstantMessagesContractualControversy.Error,
                    idAccion,
                    "",//controversiaActuacion.UsuarioCreacion, 
                    ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        #endregion business

        #region private 

        private async Task<Respuesta> CreateEditObservacionSeguimientoDiario(NovedadContractualObservaciones pObservacion, string pUsuarioCreacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Novedad_Contractual, (int)EnumeratorTipoDominio.Acciones);

            Respuesta respuesta = new Respuesta();
            try
            {
                string strCrearEditar = "";
                if (pObservacion.NovedadContractualObservacionesId > 0)
                {
                    strCrearEditar = "EDITAR OBSERVACION NOVEDAD CONTRACTUAL";
                    NovedadContractualObservaciones novedadContractualObservaciones = _context.NovedadContractualObservaciones.Find(pObservacion.NovedadContractualObservacionesId);

                    novedadContractualObservaciones.FechaModificacion = DateTime.Now;
                    novedadContractualObservaciones.UsuarioModificacion = pUsuarioCreacion;

                    novedadContractualObservaciones.Observaciones = pObservacion.Observaciones;

                }
                else
                {
                    strCrearEditar = "CREAR OBSERVACION NOVEDAD CONTRACTUAL";

                    NovedadContractualObservaciones novedadContractualObservaciones = new NovedadContractualObservaciones
                    {
                        FechaCreacion = DateTime.Now,
                        UsuarioCreacion = pUsuarioCreacion,

                        NovedadContractualId = pObservacion.NovedadContractualId,
                        Observaciones = pObservacion.Observaciones,
                        EsSupervision = pObservacion.EsSupervision,
                        EsTramiteNovedades = pObservacion.EsTramiteNovedades,
                    };

                    _context.NovedadContractualObservaciones.Add(novedadContractualObservaciones);
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Verificar_seguimiento_diario, GeneralCodes.Error, idAccion, pObservacion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }


        private NovedadContractualObservaciones getObservacion(NovedadContractual pNovedadContractual, bool? pEsSupervicion, bool? pEsTramite)
        {
            NovedadContractualObservaciones novedadContractualObservaciones = pNovedadContractual.NovedadContractualObservaciones.ToList()
                        .Where(r => r.EsSupervision == pEsSupervicion &&
                                    r.Archivado != true &&
                                    r.Eliminado != true &&
                                    r.EsTramiteNovedades == pEsTramite
                              )
                        .FirstOrDefault();

            return novedadContractualObservaciones;
        }


        private bool Registrocompleto(NovedadContractual pNovedadContractual)
        {
            bool esCompleto = true;

            if (
                    pNovedadContractual.FechaSolictud == null ||
                    string.IsNullOrEmpty(pNovedadContractual.InstanciaCodigo) ||
                    pNovedadContractual.FechaSesionInstancia == null ||
                    pNovedadContractual.NovedadContractualDescripcion == null ||
                    pNovedadContractual.NovedadContractualDescripcion.Count() == 0 ||
                    string.IsNullOrEmpty(pNovedadContractual.UrlSoporte)
                )
            {
                esCompleto = false;
            }

            foreach (NovedadContractualDescripcion descripcion in pNovedadContractual.NovedadContractualDescripcion)
            {
                // Suspension - Prórroga a la Suspensión
                if (descripcion.TipoNovedadCodigo == "1" || descripcion.TipoNovedadCodigo == "2")
                {
                    if (
                            descripcion.FechaInicioSuspension == null ||
                            descripcion.FechaFinSuspension == null
                        )
                    {
                        esCompleto = false;
                    }

                }

                // adicion
                if (descripcion.TipoNovedadCodigo == "3")
                {
                    if (
                            descripcion.PresupuestoAdicionalSolicitado == null
                        )
                    {
                        esCompleto = false;
                    }

                }

                // Prorroga
                if (descripcion.TipoNovedadCodigo == "4")
                {
                    if (
                            descripcion.PlazoAdicionalDias == null ||
                            descripcion.PlazoAdicionalMeses == null
                        )
                    {
                        esCompleto = false;
                    }

                }

                if (
                        descripcion.NovedadContractualDescripcionMotivo == null ||
                        descripcion.NovedadContractualDescripcionMotivo.Count() == 0 ||
                        string.IsNullOrEmpty(descripcion.ResumenJustificacion) ||
                        descripcion.EsDocumentacionSoporte == null ||
                        string.IsNullOrEmpty(descripcion.ConceptoTecnico) ||
                        descripcion.FechaConcepto == null ||
                        string.IsNullOrEmpty(descripcion.NumeroRadicado)

                    )
                {
                    esCompleto = false;
                }

                //Modificación de Condiciones Contractuales
                if (descripcion.TipoNovedadCodigo == "5")
                {
                    descripcion.NovedadContractualClausula.ToList().ForEach(c =>
                   {
                       if (
                            string.IsNullOrEmpty(c.ClausulaAmodificar) ||
                            string.IsNullOrEmpty(c.AjusteSolicitadoAclausula)
                       )
                       {

                       }
                   });
                }
            }

            return esCompleto;
        }

        private bool RegistrocompletoTramite(NovedadContractual pNovedadContractual)
        {
            bool esCompleto = true;

            if (
                    pNovedadContractual.FechaEnvioGestionContractual == null ||
                    string.IsNullOrEmpty(pNovedadContractual.EstadoProcesoCodigo) ||
                    pNovedadContractual.FechaAprobacionGestionContractual == null ||
                    pNovedadContractual.AbogadoRevisionId == null ||
                    pNovedadContractual.DeseaContinuar == null ||
                    (pNovedadContractual.DeseaContinuar == true && pNovedadContractual.FechaEnvioActaContratistaObra == null) ||
                    (pNovedadContractual.DeseaContinuar == true && pNovedadContractual.FechaFirmaActaContratistaObra == null) ||
                    (pNovedadContractual.DeseaContinuar == true && pNovedadContractual.FechaEnvioActaContratistaInterventoria == null) ||
                    (pNovedadContractual.DeseaContinuar == true && pNovedadContractual.FechaFirmaContratistaInterventoria == null) ||
                    (pNovedadContractual.DeseaContinuar == true && pNovedadContractual.FechaEnvioActaApoyo == null) ||
                    (pNovedadContractual.DeseaContinuar == true && pNovedadContractual.FechaFirmaApoyo == null) ||
                    (pNovedadContractual.DeseaContinuar == true && pNovedadContractual.FechaEnvioActaSupervisor == null) ||
                    (pNovedadContractual.DeseaContinuar == true && pNovedadContractual.FechaFirmaSupervisor == null) ||
                    string.IsNullOrEmpty(pNovedadContractual.UrlSoporteFirmas)
                )
            {
                esCompleto = false;
            }

            foreach (NovedadContractualDescripcion descripcion in pNovedadContractual.NovedadContractualDescripcion)
            {
                // adicion
                if (descripcion.TipoNovedadCodigo == "3")
                {
                    pNovedadContractual.NovedadContractualAportante.ToList().ForEach(na =>
                   {
                       if (
                            na.CofinanciacionAportanteId == null ||
                            na.ValorAporte == null
                        )
                       {
                           esCompleto = false;
                       }

                       na.ComponenteAportanteNovedad.ToList().ForEach(ca =>
                      {
                          if (
                            ca.FaseCodigo == null ||
                            ca.TipoComponenteCodigo == null
                        )
                          {
                              esCompleto = false;
                          }

                          ca.ComponenteUsoNovedad.ToList().ForEach(cu =>
                         {
                             if (
                                    cu.TipoUsoCodigo == null ||
                                    cu.ValorUso == null
                        )
                             {
                                 esCompleto = false;
                             }
                         });

                      });

                   });

                }

            }

            return esCompleto;
        }

        private async Task<bool> ValidarRegistroCompletoVerificacion(int id, bool? pEsSupervicion, bool? pEsTramite)
        {
            bool esCompleto = true;

            NovedadContractual nc = await _context.NovedadContractual.Where(cc => cc.NovedadContractualId == id)
                                                                .FirstOrDefaultAsync();


            nc.ObservacionApoyo = getObservacion(nc, pEsSupervicion, pEsTramite);

            if (nc.TieneObservacionesApoyo == null ||
                 (nc.TieneObservacionesApoyo == true && string.IsNullOrEmpty(nc.ObservacionApoyo != null ? nc.ObservacionApoyo.Observaciones : null))
               )
            {
                esCompleto = false;
            }

            return esCompleto;
        }

        private async Task<bool> ValidarRegistroCompletoValidacion(int id, bool? pEsSupervicion, bool? pEsTramite)
        {
            bool esCompleto = true;

            NovedadContractual nc = await _context.NovedadContractual.Where(cc => cc.NovedadContractualId == id)
                                                                .FirstOrDefaultAsync();


            nc.ObservacionSupervisor = getObservacion(nc, pEsSupervicion, pEsTramite);

            if (nc.TieneObservacionesSupervisor == null ||
                 (nc.TieneObservacionesSupervisor == true && string.IsNullOrEmpty(nc.ObservacionSupervisor != null ? nc.ObservacionSupervisor.Observaciones : null))
               )
            {
                esCompleto = false;
            }

            return esCompleto;
        }


        #endregion private 

    }


}
