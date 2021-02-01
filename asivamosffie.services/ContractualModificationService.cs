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
using Microsoft.AspNetCore.Mvc;

namespace asivamosffie.services
{
    public class ContractualModificationService : IContractualModification
    {

        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        public ContractualModificationService(devAsiVamosFFIEContext context, ICommonService commonService)
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
            List<Dominio> ListDominioEstado = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Novedad_Modificacion_Contractual).ToList();

            try
            {
                ListContratos = await _context.NovedadContractual.Where(r => r.Eliminado != true).ToListAsync();

                ListContratos.ForEach(c =>
                {
                    c.TipoNovedadNombre = ListDominioTipoDominio.Where(r => r.Codigo == c.TipoNovedadCodigo)?.FirstOrDefault()?.Nombre;
                    c.EstadoNovedadNombre = ListDominioEstado.Where(r => r.Codigo == c.EstadoCodigo)?.FirstOrDefault()?.Nombre;
                });

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
        public async Task<ActionResult<List<Contrato>>> GetListContract(int userID)
        {
            var contratos = _context.Contrato
                                        .Where(x =>/*x.UsuarioInterventoria==userID*/ !(bool)x.Eliminado)
                                        .Include(x => x.Contratacion)
                                        .ThenInclude(x => x.Contratista)
                                        .ToList();

            List<Dominio> listDominioTipoDocumento = _context.Dominio.Where(x => x.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Documento).ToList();

            foreach (var contrato in contratos)
            {
                if ( contrato?.Contratacion?.Contratista != null)
                {
                    contrato.Contratacion.Contratista.Contratacion = null;//para bajar el peso del consumo
                    contrato.Contratacion.Contratista.TipoIdentificacionNotMapped = contrato.Contratacion.Contratista.TipoIdentificacionCodigo == null ? "" : listDominioTipoDocumento.Where(x => x.Codigo == contrato.Contratacion.Contratista.TipoIdentificacionCodigo)?.FirstOrDefault()?.Nombre;
                    //contrato.TipoIntervencion no se de donde sale, preguntar, porque si es del proyecto, cuando sea multiproyecto cual traigo?
                }

            }
            return contratos;
        }

        #endregion Gets

        #region CreateEdit 

        /// <summary>
        /// CU 4.1.3 - Create or edit contractual modification
        /// </summary>
        /// <param name="pNovedadContractual"></param>
        /// <returns></returns>
        public async Task<Respuesta> CreateEditarModification(NovedadContractual pNovedadContractual)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearcontroversiaActuacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Controversia_Actuacion, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;

            try
            {
                if (pNovedadContractual != null)
                {

                    if (pNovedadContractual.NovedadContractualId == null || pNovedadContractual.NovedadContractualId == 0)
                    {
                        //Auditoria
                        strCrearEditar = "REGISTRAR NOVEDAD CONTRACTUAL";
                        pNovedadContractual.FechaCreacion = DateTime.Now;
                        pNovedadContractual.Eliminado = false;
                        //PENDIENTE REGISTRO COMPLETO                        
                        _context.NovedadContractual.Add(pNovedadContractual);
                    }
                    else
                    {
                        strCrearEditar = "EDIT NOVEDAD CONTRACTUAL";
                        //PENDIENTE REGISTRO COMPLETO
                        _context.NovedadContractual.Update(pNovedadContractual);
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
                            , ""
                            , strCrearEditar
                            )
                        };
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
                    //pNovedadContractualId.FechaModificacion = DateTime.Now;
                    //pNovedadContractualId.UsuarioModificacion = pUsuario;
                    //pNovedadContractualId.Eliminado = true;
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
