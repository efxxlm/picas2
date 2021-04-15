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
    public class RegisterContractSettlementService : IRegisterContractSettlementService
    {
        #region constructor
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public RegisterContractSettlementService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;

        }
        #endregion

        public async Task<List<dynamic>> GetListContractSettlemen()
        {
            List<VRegistrarLiquidacionContrato> liquidacionContratos = _context.VRegistrarLiquidacionContrato.ToList();
            return new List<dynamic>
            {
                liquidacionContratos.Where(r => r.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.Registrados).ToList(),
                liquidacionContratos.Where(r => r.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.LiquidacionEnProcesoDeFirma || r.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.En_firma_fiduciaria).ToList(),
                liquidacionContratos.Where(r => r.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.Liquidado).ToList()
            };
        }

        public async Task<Respuesta> CreateEditContractSettlement(Contratacion pContratacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Liquidacion_Contrato, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                string estadoSolicitud = ValidarEstadoSolicitud(pContratacion);

                await _context.Set<Contratacion>()
                              .Where(c => c.ContratacionId == pContratacion.ContratacionId)
                              .UpdateAsync(c => new Contratacion
                              {
                                  EstadoSolicitudCodigo = estadoSolicitud,

                                  UsuarioModificacion = pContratacion.UsuarioModificacion,
                                  FechaModificacion = DateTime.Now,

                                  FechaTramiteLiquidacion = DateTime.Now,

                                  FechaFirmaEnvioContratista = pContratacion.FechaFirmaEnvioContratista,
                                  FechaFirmaContratista = pContratacion.FechaFirmaContratista,

                                  FechaFirmaEnvioFiduciaria = pContratacion.FechaFirmaEnvioFiduciaria,
                                  FechaFirmaFiduciaria = pContratacion.FechaFirmaFiduciaria,

                                  ObservacionesLiquidacion = pContratacion.ObservacionesLiquidacion,
                                  UrlDocumentoLiquidacion = pContratacion.UrlDocumentoLiquidacion,

                                  RegistroCompletoLiquidacion = ValidateCompleteRecordContractSettlement(pContratacion)
                              });

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                                               (int)enumeratorMenu.Registrar_liquidacion_contrato,
                                               GeneralCodes.OperacionExitosa,
                                               idAccion,
                                               pContratacion.UsuarioModificacion,
                                               ConstantCommonMessages.ContractSettlement.CREAR_LIQUIDACION)
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
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                             (int)enumeratorMenu.Registrar_liquidacion_contrato,
                             GeneralCodes.Error,
                             idAccion,
                             pContratacion.UsuarioModificacion,
                             ex.InnerException.ToString())
                     };
            }
        }

        private string ValidarEstadoSolicitud(Contratacion pContratacion)
        {
            string estadoSolicitud = pContratacion.EstadoSolicitudCodigo;
            bool RegistroCompleto = ValidateCompleteRecordContractSettlement(pContratacion);
            if (!RegistroCompleto)
            {
                if (
                          pContratacion.FechaFirmaEnvioContratista.HasValue
                      && !pContratacion.FechaFirmaEnvioFiduciaria.HasValue)
                    estadoSolicitud = ConstanCodigoEstadoSolicitudContratacion.En_proceso_de_firmas;

                if (
                     pContratacion.FechaFirmaEnvioFiduciaria.HasValue)
                    estadoSolicitud = ConstanCodigoEstadoSolicitudContratacion.En_firma_fiduciaria;
            }
            else
                estadoSolicitud = ConstanCodigoEstadoSolicitudContratacion.Liquidado;


            return estadoSolicitud;
        }

        private bool ValidateCompleteRecordContractSettlement(Contratacion pContratacion)
        {
            if (  
                !pContratacion.FechaFirmaEnvioContratista.HasValue
             || !pContratacion.FechaFirmaContratista.HasValue

             || !pContratacion.FechaFirmaEnvioFiduciaria.HasValue
             || !pContratacion.FechaFirmaFiduciaria.HasValue

             || string.IsNullOrEmpty(pContratacion.ObservacionesLiquidacion)
             || string.IsNullOrEmpty(pContratacion.UrlDocumentoLiquidacion)
             ) return false;

            return true;
        }

    }
}
