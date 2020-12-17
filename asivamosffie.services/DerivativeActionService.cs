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
    public class DerivativeActionService: IDerivativeActionService
    {

        private readonly devAsiVamosFFIEContext _context;

        private readonly ICommonService _commonService;

        public DerivativeActionService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }

        private bool ValidarRegistroCompletoSeguimientoActuacionDerivada(SeguimientoActuacionDerivada seguimientoActuacionDerivada)
        {
            
            if (string.IsNullOrEmpty(seguimientoActuacionDerivada.Observaciones)
                || string.IsNullOrEmpty(seguimientoActuacionDerivada.RutaSoporte)
                || string.IsNullOrEmpty(seguimientoActuacionDerivada.DescripciondeActuacionAdelantada.ToString())
                //|| !string.IsNullOrEmpty(sesionComiteTemaOld.RutaSoporte)
                || string.IsNullOrEmpty(seguimientoActuacionDerivada.EstadoActuacionDerivadaCodigo)
                               || (seguimientoActuacionDerivada.EsRequiereFiduciaria == null)             

                )
            {

                return false;
            }

            return true;

        }

        public async Task<Respuesta> CreateEditarSeguimientoActuacionDerivada(SeguimientoActuacionDerivada seguimientoActuacionDerivada)
        {
            Respuesta _response = new Respuesta();
            
            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_seguimiento_actuacion_derivada, (int)EnumeratorTipoDominio.Acciones);

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
                if (seguimientoActuacionDerivada != null)
                {
                    if (string.IsNullOrEmpty(seguimientoActuacionDerivada.SeguimientoActuacionDerivadaId.ToString()) || seguimientoActuacionDerivada.SeguimientoActuacionDerivadaId == 0)
                    {
                        strCrearEditar = "REGISTRAR SEGUIMIENTO ACTUACION DERIVADA";

                        //Auditoria                        
                        seguimientoActuacionDerivada.FechaCreacion = DateTime.Now;
                        //controversiaActuacion.UsuarioCreacion = compromisoSeguimiento.UsuarioCreacion;
                        seguimientoActuacionDerivada.DescripciondeActuacionAdelantada = Helpers.Helpers.CleanStringInput(seguimientoActuacionDerivada.DescripciondeActuacionAdelantada);
                        seguimientoActuacionDerivada.Observaciones = Helpers.Helpers.CleanStringInput(seguimientoActuacionDerivada.Observaciones);

                        seguimientoActuacionDerivada.EsCompleto = ValidarRegistroCompletoSeguimientoActuacionDerivada(seguimientoActuacionDerivada);

                        //controversiaContractual.Eliminado = false;
                        _context.SeguimientoActuacionDerivada.Add(seguimientoActuacionDerivada);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        strCrearEditar = "EDITAR SEGUIMIENTO ACTUACION DERIVADA";

                        SeguimientoActuacionDerivada seguimientoActuacionDerivadaOld = _context.SeguimientoActuacionDerivada
                            .Where(r => r.SeguimientoActuacionDerivadaId == seguimientoActuacionDerivada.SeguimientoActuacionDerivadaId).FirstOrDefault();

                        if (seguimientoActuacionDerivadaOld != null)
                        {
                            seguimientoActuacionDerivadaOld.DescripciondeActuacionAdelantada = Helpers.Helpers.CleanStringInput(seguimientoActuacionDerivada.DescripciondeActuacionAdelantada);
                            seguimientoActuacionDerivadaOld.Observaciones = Helpers.Helpers.CleanStringInput(seguimientoActuacionDerivada.Observaciones);
                            //ControversiaContractual.FechaCreacion = DateTime.Now;
                            //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;
                            //contratoPoliza.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;

                            //_context.Add(contratoPoliza);

                            //contratoPoliza.RegistroCompleo = ValidarRegistroCompletoContratoPoliza(contratoPoliza);
                            //contratoPoliza.ObservacionesRevisionGeneral = ValidarRegistroCompleto(cofinanciacion);

                            //LimpiarEntradasContratoPoliza(ref contratoPoliza);
                            seguimientoActuacionDerivadaOld.FechaModificacion = DateTime.Now;
                            seguimientoActuacionDerivadaOld.UsuarioModificacion = seguimientoActuacionDerivada.UsuarioCreacion;
                            //_context.ContratoPoliza.Add(contratoPoliza);
                            _context.SeguimientoActuacionDerivada.Update(seguimientoActuacionDerivadaOld);
                            await _context.SaveChangesAsync();

                        }
                       
                    }

                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesDerivativeAction.OperacionExitosa,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_actuaciones_controversias_contractuales,
                            ConstantMessagesDerivativeAction.OperacionExitosa,
                            //contratoPoliza
                            idAccionCrearContratoPoliza
                            , seguimientoActuacionDerivada.UsuarioCreacion
                            //"UsuarioCreacion"
                            , strCrearEditar
                            //contratoPoliza.UsuarioCreacion, "REGISTRAR CONTRATO POLIZA"
                            )
                        };

                    //return _response = new Respuesta { IsSuccessful = true,
                    //    IsValidation = false, Data = cuentaBancaria,
                    //    Code = ConstantMessagesBankAccount.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesDerivativeAction.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesDerivativeAction.ErrorInterno };
            }

        }
    }

}
