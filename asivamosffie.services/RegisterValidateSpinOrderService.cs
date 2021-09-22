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
        private readonly IPaymentRequierementsService _paymentRequierementsService;


        public RegisterValidateSpinOrderService(IPaymentRequierementsService paymentRequierementsService, ICommitteeSessionFiduciarioService committeeSessionFiduciarioService, devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _paymentRequierementsService = paymentRequierementsService;
            _committeeSessionFiduciarioService = committeeSessionFiduciarioService;
            _commonService = commonService;
            _context = context;
        }

        #endregion


        private void ValidateValorNeto(int pOrdenGiroId)
        {
            OrdenGiro ordenGiro1 =
                _context.OrdenGiro
                .Where(o => o.OrdenGiroId == pOrdenGiroId)
                .Include(o => o.OrdenGiroDetalle).ThenInclude(o => o.OrdenGiroDetalleTerceroCausacion)
                .AsNoTracking()
                .FirstOrDefault();

            if (ordenGiro1.ValorNetoGiro == null)
            {
                foreach (var OrdenGiroDetalle in ordenGiro1.OrdenGiroDetalle)
                {
                    foreach (var OrdenGiroDetalleTerceroCausacion in OrdenGiroDetalle.OrdenGiroDetalleTerceroCausacion)
                    {

                        if (OrdenGiroDetalleTerceroCausacion.ValorNetoGiro != null)
                        {
                            _context.Set<OrdenGiro>()
                                    .Where(o => o.OrdenGiroId == pOrdenGiroId)
                                    .Update(o => new OrdenGiro
                                    {
                                        ValorNetoGiro = OrdenGiroDetalleTerceroCausacion.ValorNetoGiro
                                    });
                        }
                    }
                }
            }
        }
         
        public async Task<Respuesta> ChangueStatusOrdenGiro(OrdenGiro pOrdenGiro)
        {
            ValidateValorNeto(pOrdenGiro.OrdenGiroId);
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Orden_Giro, (int)EnumeratorTipoDominio.Acciones);

            int intEstadoCodigo = Int32.Parse(pOrdenGiro.EstadoCodigo);

            //Reinicia Orden giro si devuelven la solicitud
            if (intEstadoCodigo >= (int)EnumEstadoOrdenGiro.Orden_de_giro_devuelta_por_verificacion
              && intEstadoCodigo <= (int)EnumEstadoOrdenGiro.Orden_de_giro_devuelta_por_tramite_fiduciario)
                ReturnOrdenGiro(pOrdenGiro);

            //Crear URl Verificar
            if (intEstadoCodigo == (int)EnumEstadoOrdenGiro.Enviada_Para_Aprobacion_Orden_Giro)
                UpdateUrlVerify(pOrdenGiro);

            //Crear URl Aprobar
            if (intEstadoCodigo == (int)EnumEstadoOrdenGiro.Enviada_para_tramite_ante_fiduciaria)
                UpdateUrlAproved(pOrdenGiro);

            if ((intEstadoCodigo == (int)EnumEstadoOrdenGiro.Enviada_Para_Verificacion_Orden_Giro))
                ResetObservations(pOrdenGiro);

            if ((intEstadoCodigo == (int)EnumEstadoOrdenGiro.Con_Orden_de_Giro_Tramitada))
                UpdateDateRegistroCompletoTramitar(pOrdenGiro);

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

        private void UpdateDateRegistroCompletoTramitar(OrdenGiro pOrdenGiro)
        {
            _context.Set<OrdenGiro>()
                 .Where(o => o.OrdenGiroId == pOrdenGiro.OrdenGiroId)
                 .Update(o => new OrdenGiro
                 {
                     FechaRegistroCompletoTramitar = DateTime.Now
                 });
        }

        private void ResetObservations(OrdenGiro pOrdenGiro)
        {
            _context.Set<OrdenGiroObservacion>()
                    .Where(o => o.OrdenGiroId == pOrdenGiro.OrdenGiroId)
                    .Update(o => new OrdenGiroObservacion
                    {
                        Archivada = true
                    });
        }

        private void UpdateUrlVerify(OrdenGiro pOrdenGiro)
        {
            _context.Set<OrdenGiro>()
                          .Where(o => o.OrdenGiroId == pOrdenGiro.OrdenGiroId)
                          .Update(o => new OrdenGiro
                          {
                              UrlSoporteFirmadoVerificar = pOrdenGiro.UrlSoporteFirmadoVerificar,
                              FechaModificacion = DateTime.Now,
                              UsuarioModificacion = pOrdenGiro.UsuarioCreacion
                          });
        }

        private void UpdateUrlAproved(OrdenGiro pOrdenGiro)
        {
            _context.Set<OrdenGiro>()
                          .Where(o => o.OrdenGiroId == pOrdenGiro.OrdenGiroId)
                          .Update(o => new OrdenGiro
                          {
                              UrlSoporteFirmadoAprobar = pOrdenGiro.UrlSoporteFirmadoAprobar,
                              FechaModificacion = DateTime.Now,
                              UsuarioModificacion = pOrdenGiro.UsuarioCreacion
                          });
        }

        private void ReturnOrdenGiro(OrdenGiro pOrdenGiro)
        {
            _context.Set<OrdenGiro>()
                        .Where(o => o.OrdenGiroId == pOrdenGiro.OrdenGiroId)
                        .Update(o => new OrdenGiro
                        {
                            RegistroCompleto = false,
                            RegistroCompletoAprobar = false,
                            RegistroCompletoVerificar = false,
                            RegistroCompletoTramitar = false,
                            FechaRegistroCompleto = null,
                            FechaRegistroCompletoAprobar = null,
                            FechaRegistroCompletoTramitar = null,
                            FechaRegistroCompletoVerificar = null,
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pOrdenGiro.UsuarioCreacion
                        });
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
                Respuesta respuesta =
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
                if (pOrdenGiroObservacion.Archivada != true)
                    await ValidateCompleteObservation(pOrdenGiroObservacion, pOrdenGiroObservacion.UsuarioCreacion);


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
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Orden_de_giro, GeneralCodes.Error, idAccion, pOrdenGiroObservacion.UsuarioCreacion, ex.InnerException.ToString())
                   };
            }
        }

        private async Task<bool> ValidateCompleteObservation(OrdenGiroObservacion pOrdenGiroObservacion, string pUsuarioMod)
        {
            try
            {
                int intCantidadDependenciasOrdenGiro = 1;

                //if ((int)enumeratorMenu.Tramitar_orden_de_giro == pOrdenGiroObservacion.MenuId)
                //    intCantidadDependenciasOrdenGiro = 3;



                OrdenGiro ordenGiro = _context.OrdenGiro
                    .Where(r => r.OrdenGiroId == pOrdenGiroObservacion.OrdenGiroId)
                    .Include(r => r.OrdenGiroDetalle).ThenInclude(r => r.OrdenGiroDetalleDescuentoTecnica)
                    .Include(r => r.OrdenGiroDetalle).ThenInclude(r => r.OrdenGiroDetalleTerceroCausacion).ThenInclude(o => o.OrdenGiroDetalleTerceroCausacionDescuento)
                    .AsNoTracking()
                    .FirstOrDefault();

                //foreach (var OrdenGiroDetalle in ordenGiro.OrdenGiroDetalle)
                //{
                //    foreach (var item in OrdenGiroDetalle.OrdenGiroDetalleDescuentoTecnica.Where(r => r.Eliminado != true).ToList())
                //    {
                //        intCantidadDependenciasOrdenGiro++;
                //    }

                //    foreach (var OrdenGiroDetalleTerceroCausacion in OrdenGiroDetalle.OrdenGiroDetalleTerceroCausacion)
                //    {
                //        intCantidadDependenciasOrdenGiro++;

                //    }
                //}


                int intCantidadObservaciones = _context.OrdenGiroObservacion.Where(r => r.OrdenGiroId == pOrdenGiroObservacion.OrdenGiroId
                                                              && r.MenuId == pOrdenGiroObservacion.MenuId
                                                              && r.Eliminado != true
                                                              && r.RegistroCompleto == true
                                                              && r.Archivada != true).Count();

                bool TieneObservacion =
                                 _context.OrdenGiroObservacion.Any
                                                            (r => r.OrdenGiroId == pOrdenGiroObservacion.OrdenGiroId
                                                            && r.MenuId == pOrdenGiroObservacion.MenuId
                                                            && r.Eliminado != true
                                                            && r.Archivada != true
                                                            && r.TieneObservacion == true
                                                            );

                //Valida si la cantidad de relaciones  
                //es igual a la cantidad de observaciones  
                bool blRegistroCompleto = false;

                DateTime? FechaRegistroCompleto = null;
                if (intCantidadObservaciones >= intCantidadDependenciasOrdenGiro)
                {
                    FechaRegistroCompleto = DateTime.Now;
                    blRegistroCompleto = true;
                }


                switch (pOrdenGiroObservacion.MenuId)
                {
                    case (int)enumeratorMenu.Verificar_orden_de_giro:
                        await _context.Set<OrdenGiro>()
                        .Where(o => o.OrdenGiroId == ordenGiro.OrdenGiroId)
                        .UpdateAsync(r => new OrdenGiro()
                        {
                            TieneObservacion = TieneObservacion,
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pUsuarioMod,
                            EstadoCodigo = ((int)EnumEstadoOrdenGiro.En_Proceso_de_Verificacion_Orden_Giro).ToString(),
                            RegistroCompletoVerificar = blRegistroCompleto,
                            FechaRegistroCompletoVerificar = FechaRegistroCompleto
                        });
                        break;

                    case (int)enumeratorMenu.Aprobar_orden_de_giro:
                        await _context.Set<OrdenGiro>()
                        .Where(o => o.OrdenGiroId == ordenGiro.OrdenGiroId)
                        .UpdateAsync(r => new OrdenGiro()
                        {
                            TieneObservacion = TieneObservacion,
                            EstadoCodigo = ((int)EnumEstadoSolicitudPago.En_Proceso_de_Aprobacion_Orden_Giro).ToString(),
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pUsuarioMod,
                            RegistroCompletoAprobar = blRegistroCompleto,
                            FechaRegistroCompletoAprobar = FechaRegistroCompleto
                        });
                        break;

                    case (int)enumeratorMenu.Tramitar_orden_de_giro:
                        await _context.Set<OrdenGiro>()
                       .Where(o => o.OrdenGiroId == ordenGiro.OrdenGiroId)
                       .UpdateAsync(r => new OrdenGiro()
                       {
                           TieneObservacion = TieneObservacion,
                           EstadoCodigo = ((int)EnumEstadoSolicitudPago.En_Proceso_de_tramite_ante_fiduciaria).ToString(),
                           FechaModificacion = DateTime.Now,
                           UsuarioModificacion = pUsuarioMod,
                           RegistroCompletoTramitar = blRegistroCompleto,
                           FechaRegistroCompletoTramitar = FechaRegistroCompleto
                       });
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool ValidateCompleteRecordOrdenGiroObservacion(OrdenGiroObservacion pOrdenGiroObservacion)
        {
            if (pOrdenGiroObservacion.TieneObservacion == false)
                return true;
            else
                if (string.IsNullOrEmpty(pOrdenGiroObservacion.Observacion))
                return false;

            return true;
        }

        public async Task<byte[]> GetListOrdenGiro(DescargarOrdenGiro pDescargarOrdenGiro)
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

            if (pDescargarOrdenGiro.RegistrosAprobados)
            {
                ListOrdenGiroIds = await _context.OrdenGiro
                    .Include(s => s.SolicitudPago)
                    .Where(r => r.FechaRegistroCompletoTramitar.HasValue
                       && r.SolicitudPago.Count() > 0
                    )
                    .Select(r => r.OrdenGiroId)
                    .ToListAsync();
            }
            else
            {
                ListOrdenGiroIds = await _context.OrdenGiro
                      .Include(s => s.SolicitudPago)
                        .Where(r => r.FechaRegistroCompletoAprobar.HasValue
                            && r.FechaRegistroCompletoAprobar >= pDescargarOrdenGiro.FechaInicial
                            && r.FechaRegistroCompletoAprobar <= pDescargarOrdenGiro.FechaFinal
                            && r.SolicitudPago.Count() > 0)
                        .Select(r => r.OrdenGiroId).ToListAsync();
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
            OrdenGiro ordenGiro = _context.OrdenGiro
                    .Where(o => o.OrdenGiroId == pOrdenGiroId)
                        .Include(r => r.SolicitudPago).ThenInclude(r => r.Contrato)
                        .Include(t => t.OrdenGiroTercero).ThenInclude(o => o.OrdenGiroTerceroChequeGerencia)
                        .Include(t => t.OrdenGiroTercero).ThenInclude(o => o.OrdenGiroTerceroTransferenciaElectronica)
                        .Include(d => d.OrdenGiroDetalle).ThenInclude(e => e.OrdenGiroDetalleEstrategiaPago)
                        .Include(d => d.OrdenGiroDetalle).ThenInclude(e => e.OrdenGiroDetalleTerceroCausacion).ThenInclude(r => r.OrdenGiroDetalleTerceroCausacionDescuento)
                        .Include(d => d.OrdenGiroDetalle).ThenInclude(e => e.OrdenGiroDetalleTerceroCausacion).ThenInclude(r => r.OrdenGiroDetalleTerceroCausacionAportante).ThenInclude(r => r.FuenteFinanciacion).ThenInclude(r => r.CuentaBancaria)
                        .Include(d => d.OrdenGiroDetalle).ThenInclude(e => e.OrdenGiroDetalleTerceroCausacion).ThenInclude(r => r.OrdenGiroDetalleTerceroCausacionAportante).ThenInclude(r => r.CuentaBancaria)
                        .Include(d => d.OrdenGiroDetalle).ThenInclude(e => e.OrdenGiroSoporte)
                        .Include(d => d.OrdenGiroDetalle).ThenInclude(e => e.OrdenGiroDetalleObservacion)
                        .Include(d => d.OrdenGiroDetalle).ThenInclude(e => e.OrdenGiroDetalleDescuentoTecnica).ThenInclude(e => e.OrdenGiroDetalleDescuentoTecnicaAportante)
                        .Include(d => d.SolicitudPago)
                    .AsNoTracking().FirstOrDefault();

            foreach (var OrdenGiroDetalle in ordenGiro.OrdenGiroDetalle)
            {
                if (OrdenGiroDetalle.OrdenGiroDetalleDescuentoTecnica.Count > 0)
                    OrdenGiroDetalle.OrdenGiroDetalleDescuentoTecnica = OrdenGiroDetalle.OrdenGiroDetalleDescuentoTecnica.Where(r => r.Eliminado != true).ToList();

                foreach (var OrdenGiroDetalleDescuentoTecnica in OrdenGiroDetalle.OrdenGiroDetalleDescuentoTecnica)
                {
                    if (OrdenGiroDetalleDescuentoTecnica.OrdenGiroDetalleDescuentoTecnicaAportante.Count() > 0)
                        OrdenGiroDetalleDescuentoTecnica.OrdenGiroDetalleDescuentoTecnicaAportante = OrdenGiroDetalleDescuentoTecnica.OrdenGiroDetalleDescuentoTecnicaAportante.Where(r => r.Eliminado != true).ToList();
                }

                foreach (var OrdenGiroDetalleTerceroCausacion in OrdenGiroDetalle.OrdenGiroDetalleTerceroCausacion)
                {
                    if (OrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionAportante.Count() > 0)
                        OrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionAportante = OrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionAportante.Where(r => r.Eliminado != true).ToList();

                    if (OrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionDescuento.Count() > 0)
                        OrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionDescuento = OrdenGiroDetalleTerceroCausacion.OrdenGiroDetalleTerceroCausacionDescuento.Where(r => r.Eliminado != true).ToList();
                }

            }

            decimal? ValorOrdenGiro = ordenGiro.OrdenGiroDetalle?.Sum(r => r.OrdenGiroDetalleTerceroCausacion?.Sum(r => r.OrdenGiroDetalleTerceroCausacionAportante?.Sum(r => r.ValorDescuento)));
            string UrlSoporte = ordenGiro?.OrdenGiroDetalle?.FirstOrDefault()?.OrdenGiroSoporte.FirstOrDefault()?.UrlSoporte;
            List<Dominio> ListModalidadContrato = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Modalidad_Contrato).ToList();
             
            Decimal? Descuentos = ordenGiro.OrdenGiroDetalle?.Sum(r => r.OrdenGiroDetalleTerceroCausacion?.Sum(r => r.OrdenGiroDetalleTerceroCausacionDescuento?.Sum(r => r.ValorDescuento)));
            // Descuentos += ordenGiro.OrdenGiroDetalle?.Sum(r => r.OrdenGiroDetalleTerceroCausacion?.Sum(r => r.OrdenGiroDetalleTerceroCausacionAportante?.Sum(r => r.ValorDescuento)));
            Descuentos += ordenGiro.OrdenGiroDetalle?.Sum(r => r.OrdenGiroDetalleDescuentoTecnica?.Sum(r => r.OrdenGiroDetalleDescuentoTecnicaAportante?.Sum(r => r.ValorDescuento)));


            UrlSoporte = "<a href='" + UrlSoporte + "' target='_blank'>Link</a>";
            try
            {
                pContenido = pContenido
                       .Replace("[FECHA_ORDEN_GIRO]", ordenGiro.FechaCreacion != null ? ((DateTime)ordenGiro?.FechaCreacion).ToString("dd/MM/yyy") : " ")
                       .Replace("[NUMERO_ORDEN_GIRO]", ordenGiro.NumeroSolicitud)
                       .Replace("[MODALIDAD_CONTRATO]", ordenGiro?.SolicitudPago?.FirstOrDefault()?.Contrato?.ModalidadCodigo != null ? ListModalidadContrato.Where(r => r.Codigo == ordenGiro?.SolicitudPago?.FirstOrDefault()?.Contrato?.ModalidadCodigo).FirstOrDefault().Nombre : " ")
                       .Replace("[NUMERO_CONTRATO]", (ordenGiro?.SolicitudPago?.FirstOrDefault()?.Contrato?.NumeroContrato) ?? " ")
                       .Replace("[VALOR_ORDEN_GIRO]", +ValorOrdenGiro != null ? "$ " + String.Format("{0:n0}", ValorOrdenGiro) : "$ 0")
                       .Replace("[VALOR_DESCUENTO]", +Descuentos != null ? "$ " + String.Format("{0:n0}", Descuentos) : "$ 0")
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
                                             //  && s.IdPadre == pPadreId
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
