using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Constants;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class RegisterValidateSpinOrderService : IRegisterValidateSpinOrderService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public RegisterValidateSpinOrderService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }

        public async Task<Respuesta> ChangueStatusOrdenGiro(OrdenGiro pOrdenGiro)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Orden_Giro, (int)EnumeratorTipoDominio.Acciones);

            try
            {


                _context.Set<OrdenGiro>()
                       .Where(o => o.OrdenGiroId == pOrdenGiro.OrdenGiroId)
                       .Update(o => new OrdenGiro
                       {
                           EstadoCodigo = pOrdenGiro.EstadoCodigo,
                           FechaModificacion = DateTime.Now,
                           UsuarioModificacion = pOrdenGiro.UsuarioCreacion
                       });





                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                                              (int)enumeratorMenu.Generar_Orden_de_giro,
                                              GeneralCodes.OperacionExitosa,
                                              idAccion,
                                              pOrdenGiro.UsuarioCreacion,
                                              ConstantCommonMessages.SpinOrder.CAMBIAR_ESTADO_ORDEN_GIRO)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Orden_de_giro, GeneralCodes.Error, idAccion, pOrdenGiro.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> CreateEditSpinOrderObservations(OrdenGiroObservacion pOrdenGiroObservacion)
        {
            if (pOrdenGiroObservacion.OrdenGiroObservacionId == 0)
            {
                pOrdenGiroObservacion.RegistroCompleto = ValidateCompleteRecordOrdenGiroObservacion(pOrdenGiroObservacion);
                await _context.OrdenGiroObservacion.AddAsync(pOrdenGiroObservacion);
            }
            else
            {
                await _context.Set<OrdenGiroObservacion>()
                          .Where(og => og.OrdenGiroObservacionId == pOrdenGiroObservacion.OrdenGiroObservacionId)
                          .UpdateAsync(og => new OrdenGiroObservacion
                          {
                              Observacion = pOrdenGiroObservacion.Observacion,
                              TieneObservacion = pOrdenGiroObservacion.TieneObservacion,
                              TipoObservacionCodigo = pOrdenGiroObservacion.TipoObservacionCodigo,
                              FechaModificacion = DateTime.Now,
                              IdPadre = pOrdenGiroObservacion.IdPadre,
                              RegistroCompleto = ValidateCompleteRecordOrdenGiroObservacion(pOrdenGiroObservacion)
                          });
            }
            return new Respuesta();
        }

        private bool ValidateCompleteRecordOrdenGiroObservacion(OrdenGiroObservacion pOrdenGiroObservacion)
        {
            if (pOrdenGiroObservacion.TieneObservacion == false)
                return false;
            else
                if (string.IsNullOrEmpty(pOrdenGiroObservacion.Observacion))
                return false;

            return true;
        }
         
        private async Task<string> ReplaceVariablesOrdenGiro(string pContenido , int pOrdenGiroId)
        {
            //OrdenGiro ordenGiro  = _context.OrdenGiro
            //    .Where(o=> o.OrdenGiroId == pOrdenGiroId)
            //    .Include(s=> s.SolicitudPago)


            //pContenido = pContenido
            //    .Replace("FECHA_ORDEN_GIRO", "")
            //    .Replace("NUMERO_ORDEN_GIRO", "")
            //    .Replace("MODALIDAD_CONTRATO", "")
            //    .Replace("NUMERO_CONTRATO", "")
            //    .Replace("VALOR_ORDEN_GIRO", "")
            //    .Replace("URL", " ");
             
            return pContenido; 
        }
    }
}
