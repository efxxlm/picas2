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

        /*
         * creado por: jflorez
         * fecha: 20201203
         */
        public async Task<Respuesta> CreateEditarModification(NovedadContractual pNovedadContractual)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearcontroversiaActuacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Controversia_Actuacion, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;

            try
            {
                if (pNovedadContractual != null)
                {

                    if (pNovedadContractual.NovedadContractualId==null || pNovedadContractual.NovedadContractualId == 0)
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



        public async Task<List<NovedadContractual>> GetListGrillaNovedadContractual()
        {
            List<NovedadContractual> ListContratos = new List<NovedadContractual>();

            try
            {
                ListContratos=await _context.NovedadContractual.Where(r => !(bool)r.Eliminado).ToListAsync();     
                                
                return ListContratos.OrderByDescending(r => r.FechaSolictud).ToList();
            }
            catch (Exception ex)
            {
                return ListContratos;
            }
        }

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

        /*autor: jflorez
           descripción: trae listado de contratos asignados al usuario logeado para el autocompletar
           impacto: CU 4.1.3*/
        public async Task<ActionResult<List<Contrato>>> GetListContract(int userID)
        {
            var contratos= _context.Contrato.Where(x =>//x.UsuarioInterventoria==userID
            !(bool)x.Eliminado
            ).Include(x=>x.Contratacion).ToList();
            foreach(var contrato in contratos)
            {
                var contratista = _context.Contratista.Find(contrato.Contratacion.ContratistaId);
                if (contratista != null)
                {
                    contratista.Contratacion = null;//para bajar el peso del consumo
                    contratista.TipoIdentificacionNotMapped = contratista.TipoIdentificacionCodigo==null?"":_context.Dominio.Where(x => x.Codigo == contratista.TipoIdentificacionCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Documento).FirstOrDefault().Nombre;
                    contrato.Contratacion.Contratista = contratista;
                    //contrato.TipoIntervencion no se de donde sale, preguntar, porque si es del proyecto, cuando sea multiproyecto cual traigo?
                }

            }
            return contratos;
        }
    }
}
