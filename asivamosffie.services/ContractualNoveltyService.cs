using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services
{
    //CU 4_4_1 Registrar actuaciones de controversias contractuales

    public class ContractualNoveltyService /*: IContractualControversy*/
    {

        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;


        public ContractualNoveltyService(devAsiVamosFFIEContext context, ICommonService commonService)
        {

            _commonService = commonService;
            _context = context;
            
            //_settings = settings;
        }


        public async Task<Respuesta> CreateEditNovedadContractual(NovedadContractual novedadContractual)
        {
            Respuesta _response = new Respuesta(); /*NovedadContractual novedadContractual*/

            int idAccionCrearEditarNovedadContractual = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Novedad_Contractual, (int)EnumeratorTipoDominio.Acciones);

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

            string strCrearEditar = string.Empty;

            try
            {
                if (novedadContractual != null)
                {

                    if (string.IsNullOrEmpty(novedadContractual.NovedadContractualId.ToString()) || novedadContractual.NovedadContractualId == 0)
                    {
                        strCrearEditar = "REGISTRAR NOVEDAD CONTRACTUAL";

                        //Auditoria
                        //strCrearEditar = "REGISTRAR AVANCE COMPROMISOS";
                        //novedadContractual.FechaCreacion = DateTime.Now;
                        //controversiaActuacion.UsuarioCreacion = compromisoSeguimiento.UsuarioCreacion;
                        //novedadContractual.FechaSolictud ;
                        //novedadContractual.InstanciaCodigo = 
                        //novedadContractual.TipoNovedadCodigo
                        //    novedadContractual.MotivoNovedadCodigo
                        //    novedadContractual.ResumenJustificacion
                        //    novedadContractual.EsDocumentacionSoporte
                        //    novedadContractual.ConceptoTecnico
                        //    novedadContractual.FechaConcepto
                        //    //novedadContractual.radi

                            //NumeroRadicadoFfie RequisitoTecnicoRadicado
                        //novedadContractual.RegistroCompleto = ValidarRegistroCompletoControversiaActuacion(novedadContractual);

                        novedadContractual.Eliminado = false;
                        _context.NovedadContractual.Add(novedadContractual);

                    }

                    else
                    {
                        strCrearEditar = "EDIT NOVEDAD CONTRACTUAL";

                        //novedadContractual.Observaciones = Helpers.Helpers.CleanStringInput(novedadContractual.Observaciones);
                        //novedadContractual.ResumenPropuestaFiduciaria = Helpers.Helpers.CleanStringInput(novedadContractual.ResumenPropuestaFiduciaria);

                        novedadContractual.FechaCreacion = DateTime.Now;
                        //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;
                        //controversiaActuacion.UsuarioCreacion = novedadContractual.UsuarioModificacion;

                        //_context.Add(contratoPoliza);

                        //novedadContractual.EsCompleto = ValidarRegistroCompletoControversiaActuacion(novedadContractual);
                        //contratoPoliza.ObservacionesRevisionGeneral = ValidarRegistroCompleto(cofinanciacion);

                        //LimpiarEntradasContratoPoliza(ref contratoPoliza);

                        //_context.ContratoPoliza.Add(contratoPoliza);
                        _context.NovedadContractual.Update(novedadContractual);

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
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_actuaciones_controversias_contractuales,
                            ConstantMessagesContractualNovelty.OperacionExitosa,
                            //contratoPoliza
                            idAccionCrearEditarNovedadContractual
                            , novedadContractual.UsuarioModificacion
                            //"UsuarioCreacion"
                            , "EDITAR NOVEDAD CONTRACTUAL"
                            //contratoPoliza.UsuarioCreacion, "REGISTRAR CONTRATO POLIZA"
                            )
                        };

                    //return _response = new Respuesta { IsSuccessful = true,
                    //    IsValidation = false, Data = cuentaBancaria,
                    //    Code = ConstantMessagesBankAccount.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContractualNovelty.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContractualNovelty.ErrorInterno };
            }

        }
    }
}
