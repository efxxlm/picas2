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
        public async Task<List<NovedadContractual>> GetListGrillaNovedadContractual()
        {
            List<NovedadContractual> ListContratos = new List<NovedadContractual>();
            List<Dominio> ListDominioTipoDominio = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Novedad_Modificacion_Contractual).ToList();
            List<Dominio> ListDominioEstado = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Novedad_Contractual).ToList();

            try
            {
                ListContratos = await _context.NovedadContractual.Where(r => r.Eliminado != true).ToListAsync();

                //ListContratos.ForEach(c =>
                //{
                //    c.TipoNovedadNombre = ListDominioTipoDominio.Where(r => r.Codigo == c.TipoNovedadCodigo)?.FirstOrDefault()?.Nombre;
                //    c.EstadoNovedadNombre = ListDominioEstado.Where(r => r.Codigo == c.EstadoCodigo)?.FirstOrDefault()?.Nombre;
                //});

                return ListContratos.OrderByDescending(r => r.FechaSolictud).ToList();
            }
            catch (Exception ex)
            {
                return ListContratos;
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

            NovedadContractual novedadContractual = _context.NovedadContractual
                                                                .Where( r => r.NovedadContractualId == pId)
                                                                .Include( r => r.Contrato )
                                                                    .ThenInclude( r => r.Contratacion )
                                                                        .ThenInclude( r => r.Contratista )
                                                                .Include( r => r.NovedadContractualDescripcion )
                                                                    .ThenInclude( r => r.NovedadContractualClausula )
                                                                .Include(r => r.NovedadContractualDescripcion)
                                                                    .ThenInclude(r => r.NovedadContractualDescripcionMotivo)
                                                                .FirstOrDefault();

            novedadContractual.ProyectosContrato = _context.VProyectosXcontrato
                                                                .Where(r => r.ContratoId == novedadContractual.ContratoId)
                                                                .ToList();
            
            novedadContractual.ProyectosSeleccionado = _context.VProyectosXcontrato
                                                                    .Where(r => r.ProyectoId == novedadContractual.ProyectoId && r.ContratoId == novedadContractual.ContratoId )
                                                                    .FirstOrDefault();

            foreach( NovedadContractualDescripcion novedadContractualDescripcion in novedadContractual.NovedadContractualDescripcion)
            {
                novedadContractualDescripcion.NombreTipoNovedad = listDominioTipoNovedad
                                                                        .Where(r => r.Codigo == novedadContractualDescripcion.TipoNovedadCodigo)
                                                                        .FirstOrDefault()
                                                                        .Nombre;
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

            int idAccionCrearEditarNovedadContractual = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Novedad_Contractual, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;

            try
            {
                if (novedadContractual != null)
                {

                    if (string.IsNullOrEmpty(novedadContractual.NovedadContractualId.ToString()) || novedadContractual.NovedadContractualId == 0)
                    {

                        //Auditoria
                        strCrearEditar = "REGISTRAR NOVEDAD CONTRACTUAL";
                        novedadContractual.FechaCreacion = DateTime.Now;
                        novedadContractual.UsuarioCreacion = novedadContractual.UsuarioCreacion;

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

                        if (novedadContractual.NovedadContractualDescripcion != null)
                        {
                            foreach (NovedadContractualDescripcion descripcion in novedadContractual.NovedadContractualDescripcion)
                            {
                                descripcion.UsuarioCreacion = novedadContractual.UsuarioCreacion;
                                descripcion.NovedadContractualId = novedadContractual.NovedadContractualId;

                                await CreateEditNovedadContractualDescripcion(descripcion);
                            }
                        }
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

        #endregion business

        #region private 

        #endregion private 

    }


}
