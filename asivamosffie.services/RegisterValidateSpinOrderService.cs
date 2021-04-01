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
        #region Constructor
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly ICommitteeSessionFiduciarioService _committeeSessionFiduciarioService;

        public RegisterValidateSpinOrderService(ICommitteeSessionFiduciarioService committeeSessionFiduciarioService, devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _committeeSessionFiduciarioService = committeeSessionFiduciarioService;
            _commonService = commonService;
            _context = context;
        }

        #endregion

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
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Observacion_Orden_Giro, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pOrdenGiroObservacion.OrdenGiroObservacionId == 0)
                {
                    pOrdenGiroObservacion.Archivada = false;
                    pOrdenGiroObservacion.RegistroCompleto = ValidateCompleteRecordOrdenGiroObservacion(pOrdenGiroObservacion);
                    _context.OrdenGiroObservacion.Add(pOrdenGiroObservacion);
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
                                           pOrdenGiroObservacion.UsuarioCreacion,
                                           ConstantCommonMessages.SpinOrder.CREAR_EDITAR_OBSERVACION_ORDEN_GIRO)
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
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Orden_de_giro, GeneralCodes.Error, idAccion, pOrdenGiroObservacion.UsuarioCreacion, ex.InnerException.ToString())
                   };
            } 
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

        public async Task<byte[]> GetListOrdenGiro(bool pBlRegistrosAprobados)
        {
            string TipoPlantilla = ((int)ConstanCodigoPlantillas.Tabla_Orden_Giro_Para_Tramitar).ToString();

            Plantilla Plantilla =
                _context.Plantilla
                .Where(r => r.Codigo == TipoPlantilla).
                Include(r => r.Encabezado).
                Include(r => r.PieDePagina)
                .AsNoTracking().FirstOrDefault();

            TipoPlantilla = ((int)ConstanCodigoPlantillas.Registros_Orden_Giro_Para_Tramitar).ToString();

            Plantilla PlantillaRegistros =
                _context.Plantilla
                .Where(r => r.Codigo == TipoPlantilla)
                .AsNoTracking().FirstOrDefault();

            List<int> ListOrdenGiroIds = new List<int>();

            if (pBlRegistrosAprobados)
            {
                ListOrdenGiroIds = _context.OrdenGiro
                    .Where(r => r.FechaRegistroCompletoAprobar.HasValue && !r.FechaRegistroCompletoTramitar.HasValue)
                    .Select(r => r.OrdenGiroId).ToList();
            }
            else
            {
                ListOrdenGiroIds = _context.OrdenGiro
                        .Where(r => r.FechaRegistroCompletoTramitar.HasValue)
                        .Select(r => r.OrdenGiroId).ToList();
            }

            string RegistrosOrdenGiro = string.Empty;

            foreach (var item in ListOrdenGiroIds)
            {
                RegistrosOrdenGiro += PlantillaRegistros.Contenido;
                RegistrosOrdenGiro = ReplaceVariablesOrdenGiro(RegistrosOrdenGiro, item);
            }

            Plantilla.Contenido = Plantilla.Contenido.Replace("[REGISTROS]", RegistrosOrdenGiro);

            return _committeeSessionFiduciarioService.ConvertirPDF(Plantilla);
        }

        private string ReplaceVariablesOrdenGiro(string pContenido, int pOrdenGiroId)
        {
            OrdenGiro ordenGiro =
                _context.OrdenGiro
                                .Where(o => o.OrdenGiroId == pOrdenGiroId)
                                 .Include(s => s.SolicitudPago).ThenInclude(s => s.Contrato)
                                .Include(s => s.OrdenGiroDetalle).ThenInclude(o => o.OrdenGiroSoporte)
                                .Include(s => s.OrdenGiroDetalle).ThenInclude(s => s.OrdenGiroDetalleTerceroCausacion)
                                .FirstOrDefault();

            decimal? ValorOrdenGiro = ordenGiro?.OrdenGiroDetalle?.FirstOrDefault()?.OrdenGiroDetalleTerceroCausacion.FirstOrDefault()?.ValorNetoGiro;
            string UrlSoporte = ordenGiro?.OrdenGiroDetalle?.FirstOrDefault()?.OrdenGiroSoporte.FirstOrDefault()?.UrlSoporte;
            List<Dominio> ListModalidadContrato = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Modalidad_Contrato).ToList();

            try
            {
                pContenido = pContenido
                       .Replace("[FECHA_ORDEN_GIRO]", ordenGiro.FechaCreacion != null ? ((DateTime)ordenGiro?.FechaCreacion).ToString("dd/MM/yyy") : " ")
                       .Replace("[NUMERO_ORDEN_GIRO]", ordenGiro.NumeroSolicitud)
                       .Replace("[MODALIDAD_CONTRATO]", ordenGiro?.SolicitudPago?.FirstOrDefault()?.Contrato?.ModalidadCodigo != null ? ListModalidadContrato.Where(r => r.Codigo == ordenGiro?.SolicitudPago?.FirstOrDefault()?.Contrato?.ModalidadCodigo).FirstOrDefault().Nombre : " ")
                       .Replace("[NUMERO_CONTRATO]", ordenGiro?.SolicitudPago?.FirstOrDefault().Contrato?.NumeroContrato != null ? ordenGiro?.SolicitudPago?.FirstOrDefault().Contrato?.NumeroContrato : " ")
                       .Replace("[VALOR_ORDEN_GIRO]", +ValorOrdenGiro != null ? "$ " + String.Format("{0:n0}", ValorOrdenGiro) : "$ 0")
                       .Replace("[URL]", UrlSoporte);
            }
            catch (Exception e)
            {

            }
            return pContenido;
        }

        public async Task<dynamic> GetObservacionOrdenGiroByMenuIdAndSolicitudPagoId(int pMenuId, int pOrdenGiroId, int pPadreId, string pTipoObservacionCodigo)
        {
            return await _context.OrdenGiroObservacion
                                           .Where(s => s.MenuId == pMenuId
                                               && s.OrdenGiroId == pOrdenGiroId
                                               && s.IdPadre == pPadreId
                                               && s.TipoObservacionCodigo == pTipoObservacionCodigo)
                                            .Select(p => new
                                            {
                                                p.OrdenGiroObservacionId,
                                                p.TieneObservacion,
                                                p.Archivada,
                                                p.FechaCreacion,
                                                p.Observacion,
                                                p.RegistroCompleto
                                            }).ToListAsync();
        }
    }
}
