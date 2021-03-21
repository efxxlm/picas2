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

        public async Task<NovedadContractual> GetNovedadContractualById( int pId)
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
                                                                .Where( r => r.NovedadContractualId == pId)
                                                                .Include( r => r.Contrato )
                                                                    .ThenInclude( r => r.Contratacion )
                                                                        .ThenInclude( r => r.Contratista )
                                                                .Include( r => r.NovedadContractualDescripcion )
                                                                    .ThenInclude( r => r.NovedadContractualClausula )
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
                                                                    .Where(r => r.ProyectoId == novedadContractual.ProyectoId && r.ContratoId == novedadContractual.ContratoId )
                                                                    .FirstOrDefault();

            novedadContractual.InstanciaNombre = listDominioInstancias
                                                        .Where(r => r.Codigo == novedadContractual.InstanciaCodigo)
                                                        ?.FirstOrDefault()
                                                        ?.Nombre;

            foreach( NovedadContractualDescripcion novedadContractualDescripcion in novedadContractual.NovedadContractualDescripcion)
            {
                novedadContractualDescripcion.NombreTipoNovedad = listDominioTipoNovedad
                                                                        .Where(r => r.Codigo == novedadContractualDescripcion.TipoNovedadCodigo)
                                                                        ?.FirstOrDefault()
                                                                        ?.Nombre;
                foreach( NovedadContractualDescripcionMotivo motivo in novedadContractualDescripcion.NovedadContractualDescripcionMotivo)
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

                        if ( contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
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
                            foreach( NovedadContractualDescripcion descripcion in novedadContractual.NovedadContractualDescripcion)
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
                    return _response = new Respuesta { 
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
                return _response = new Respuesta {
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
                    
                    _context.NovedadContractualDescripcion.Add( novedadContractualDescripcion );

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

                    _context.NovedadContractualDescripcion.Update( novedadDescripcionOld );
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

        public async Task<Respuesta> AprobarSolicitud( int pNovedadContractualId, string pUsuario)
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

        #endregion business

        #region private 

        private bool Registrocompleto( NovedadContractual pNovedadContractual)
        {
            bool esCompleto = true;

            if (
                    pNovedadContractual.FechaSolictud == null ||
                    string.IsNullOrEmpty(pNovedadContractual.InstanciaCodigo) ||
                    pNovedadContractual.FechaSesionInstancia == null ||
                    pNovedadContractual.NovedadContractualDescripcion == null ||
                    pNovedadContractual.NovedadContractualDescripcion.Count() == 0 ||
                    string.IsNullOrEmpty( pNovedadContractual.UrlSoporte )
                )
            {
                esCompleto = false;
            }

            foreach( NovedadContractualDescripcion descripcion in pNovedadContractual.NovedadContractualDescripcion)
            {
                // Suspension - Prórroga a la Suspensión
                if ( descripcion.TipoNovedadCodigo == "1" || descripcion.TipoNovedadCodigo == "2")
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
                        string.IsNullOrEmpty( descripcion.ResumenJustificacion ) ||
                        descripcion.EsDocumentacionSoporte == null ||
                        string.IsNullOrEmpty(descripcion.ConceptoTecnico ) ||
                        descripcion.FechaConcepto == null ||
                        string.IsNullOrEmpty( descripcion.NumeroRadicado )

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
                            string.IsNullOrEmpty( c.ClausulaAmodificar ) ||
                            string.IsNullOrEmpty( c.AjusteSolicitadoAclausula )
                       )
                       {

                       }
                   });
                }
            }

            return esCompleto;
        }

        #endregion private 

    }


}
