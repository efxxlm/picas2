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
    public class UpdatePoliciesGuaranteesService : IUpdatePoliciesGuaranteesService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public UpdatePoliciesGuaranteesService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }

        #region Get
        public async Task<dynamic> GetContratoByNumeroContrato(string pNumeroContrato)
        {
            try
            {
                return await _context.Contrato
                               .Include(c => c.Contratacion).ThenInclude(c => c.Contratista)
                               .Include(c => c.ContratoPoliza)
                               .Where(c => c.NumeroContrato.ToLower().Trim().Contains(pNumeroContrato.ToLower().Trim()) && c.ContratoPoliza.Count() > 0)
                                      .Select(r => new
                                      {
                                          r.Contratacion.Contratista.Nombre,
                                          r.Contratacion.TipoSolicitudCodigo,
                                          r.ContratoPoliza.FirstOrDefault().ContratoPolizaId,
                                          r.NumeroContrato,
                                          r.ContratoId,
                                      }).ToListAsync();

            }
            catch (Exception ex)
            {
                return new { };
            }
        }

        public async Task<ContratoPoliza> GetContratoPoliza(int pContratoPolizaId)
        {
            ContratoPoliza contratoPoliza = await _context.ContratoPoliza
                .Where(c => c.ContratoPolizaId == pContratoPolizaId)
               .Include(c => c.Contrato).ThenInclude(c => c.Contratacion)
                .Include(c => c.ContratoPolizaActualizacion).ThenInclude(c => c.ContratoPolizaActualizacionSeguro)
                .Include(c => c.ContratoPolizaActualizacion).ThenInclude(c => c.ContratoPolizaActualizacionListaChequeo)
                .Include(c => c.ContratoPolizaActualizacion).ThenInclude(c => c.ContratoPolizaActualizacionRevisionAprobacionObservacion)
                .FirstOrDefaultAsync();

            GetRemoveDeleteItems(contratoPoliza);

            return contratoPoliza;

        }

        public async Task<List<VActualizacionPolizaYGarantias>> GetListVActualizacionPolizaYGarantias()
        {
            return await _context.VActualizacionPolizaYGarantias.ToListAsync();
        }

        private void GetRemoveDeleteItems(ContratoPoliza contratoPoliza)
        {
            foreach (var ContratoPolizaActualizacion in contratoPoliza.ContratoPolizaActualizacion)
            {
                if (ContratoPolizaActualizacion.ContratoPolizaActualizacionSeguro.Count() > 0)
                    ContratoPolizaActualizacion.ContratoPolizaActualizacionSeguro = ContratoPolizaActualizacion.ContratoPolizaActualizacionSeguro.Where(c => c.Eliminado != true).ToList();
            }
        }

        #endregion

        #region Create

        public async Task<Respuesta> ChangeStatusContratoPolizaActualizacionSeguro(ContratoPolizaActualizacion  pContratoPolizaActualizacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Actualizar_Polizas_Y_Garantias, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                _context.Set<ContratoPolizaActualizacion>()
                        .Where(c => c.ContratoPolizaActualizacionId == pContratoPolizaActualizacion.ContratoPolizaActualizacionId)
                        .Update(c => new ContratoPolizaActualizacion
                        {
                            EstadoActualizacion  = pContratoPolizaActualizacion.EstadoActualizacion,
                            UsuarioModificacion = pContratoPolizaActualizacion.UsuarioCreacion,
                            FechaModificacion = DateTime.Now
                        });

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                                                                                               (int)enumeratorMenu.Registrar_actualizacion_de_polizas_y_garantias,
                                                                                               GeneralCodes.OperacionExitosa,
                                                                                               idAccion,
                                                                                               pContratoPolizaActualizacion.UsuarioCreacion,
                                                                                               ConstantCommonMessages.UpdatePolicies.CAMBIAR_ESTADOS_ACTUALIZAR_POLIZA
                                                                                           )
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
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Orden_de_giro, GeneralCodes.Error, idAccion, pContratoPolizaActualizacion.UsuarioCreacion, ex.InnerException.ToString())
                   };
            }
        }
         
        public async Task<Respuesta> DeleteContratoPolizaActualizacionSeguro(ContratoPolizaActualizacionSeguro pContratoPolizaActualizacionSeguro)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Seguros_Actualizacion_Polizas_y_garantias, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                _context.Set<ContratoPolizaActualizacionSeguro>()
                        .Where(c => c.ContratoPolizaActualizacionSeguroId == pContratoPolizaActualizacionSeguro.ContratoPolizaActualizacionSeguroId)
                        .Update(c => new ContratoPolizaActualizacionSeguro
                        {
                            Eliminado = true,
                            UsuarioModificacion = pContratoPolizaActualizacionSeguro.UsuarioCreacion,
                            FechaModificacion = DateTime.Now
                        });

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                                                                                               (int)enumeratorMenu.Registrar_actualizacion_de_polizas_y_garantias,
                                                                                               GeneralCodes.OperacionExitosa,
                                                                                               idAccion,
                                                                                               pContratoPolizaActualizacionSeguro.UsuarioCreacion,
                                                                                               ConstantCommonMessages.UpdatePolicies.ELIMINAR_SEGUROS
                                                                                           )
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
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Orden_de_giro, GeneralCodes.Error, idAccion, pContratoPolizaActualizacionSeguro.UsuarioCreacion, ex.InnerException.ToString())
                   };
            }
        }

        public async Task<Respuesta> CreateEditContratoPolizaActualizacion(ContratoPolizaActualizacion pContratoPolizaActualizacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Actualizacion_Polizas_y_Garantias, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pContratoPolizaActualizacion.ContratoPolizaActualizacionId == 0)
                {
                    pContratoPolizaActualizacion.EstadoActualizacion = ConstanCodigoEstadoActualizacionPoliza.En_revision_de_actualizacion_de_poliza;
                    pContratoPolizaActualizacion.NumeroActualizacion = await _commonService.EnumeradorActualizarPoliza();
                    pContratoPolizaActualizacion.RegistroCompleto = false;
                    pContratoPolizaActualizacion.FechaCreacion = DateTime.Now;
                    pContratoPolizaActualizacion.Eliminado = false;
                    _context.ContratoPolizaActualizacion.Add(pContratoPolizaActualizacion);
                }
                else
                {
                    _context.Set<ContratoPolizaActualizacion>()
                            .Where(c => c.ContratoPolizaActualizacionId == pContratoPolizaActualizacion.ContratoPolizaActualizacionId)
                            .Update(c => new ContratoPolizaActualizacion
                            {
                                FechaModificacion = DateTime.Now,
                                UsuarioModificacion = pContratoPolizaActualizacion.UsuarioCreacion,
                                RazonActualizacionCodigo = pContratoPolizaActualizacion.RazonActualizacionCodigo,
                                TipoActualizacionCodigo = pContratoPolizaActualizacion.TipoActualizacionCodigo,
                                FechaExpedicionActualizacionPoliza = pContratoPolizaActualizacion.FechaExpedicionActualizacionPoliza,
                                ObservacionEspecifica = pContratoPolizaActualizacion.ObservacionEspecifica,
                                TieneObservacionEspecifica = pContratoPolizaActualizacion.TieneObservacionEspecifica,
                                RegistroCompleto = ValidarRegistroCompletoContratoPolizaActualizacion(pContratoPolizaActualizacion),
                                RegistroCompletoObservacionEspecifica = ValidarRegistroCompletoObservacionEspecifica(pContratoPolizaActualizacion)
                            });
                }

                if (pContratoPolizaActualizacion.ContratoPolizaActualizacionSeguro.Count > 0)
                    CreateEditContratoPolizaActualizacionSeguro(pContratoPolizaActualizacion.ContratoPolizaActualizacionSeguro, pContratoPolizaActualizacion.UsuarioCreacion);

                if (pContratoPolizaActualizacion.ContratoPolizaActualizacionRevisionAprobacionObservacion.Count > 0)
                    CreateEditContratoPolizaActualizacionRevisionAprobacionObservacion(pContratoPolizaActualizacion.ContratoPolizaActualizacionRevisionAprobacionObservacion, pContratoPolizaActualizacion.UsuarioCreacion);

                if (pContratoPolizaActualizacion.ContratoPolizaActualizacionListaChequeo.Count() > 0)
                    CreateEditContratoPolizaActualizacionListaChequeo(pContratoPolizaActualizacion.ContratoPolizaActualizacionListaChequeo, pContratoPolizaActualizacion.UsuarioCreacion);

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                                                                                               (int)enumeratorMenu.Registrar_actualizacion_de_polizas_y_garantias,
                                                                                               GeneralCodes.OperacionExitosa,
                                                                                               idAccion,
                                                                                               pContratoPolizaActualizacion.UsuarioCreacion,
                                                                                               pContratoPolizaActualizacion.FechaModificacion.HasValue ?
                                                                                               ConstantCommonMessages.UpdatePolicies.EDITAR_ACTUALIZACION_POLIZA :
                                                                                               ConstantCommonMessages.UpdatePolicies.CREAR_ACTUALIZACION_POLIZA
                                                                                           )
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
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Orden_de_giro, GeneralCodes.Error, idAccion, pContratoPolizaActualizacion.UsuarioCreacion, ex.InnerException.ToString())
                   };
            }
        }

        private bool ValidarRegistroCompletoObservacionEspecifica(ContratoPolizaActualizacion pContratoPolizaActualizacion)
        {
            if (pContratoPolizaActualizacion.TieneObservacionEspecifica == true)
                return true;
            else
                if (string.IsNullOrEmpty(pContratoPolizaActualizacion.ObservacionEspecifica))
                return false;

            return true;
        }

        private void CreateEditContratoPolizaActualizacionListaChequeo(ICollection<ContratoPolizaActualizacionListaChequeo> pContratoPolizaActualizacionListaChequeo, string pAuthor)
        {
            foreach (var item in pContratoPolizaActualizacionListaChequeo)
            {
                if (item.ContratoPolizaActualizacionListaChequeoId == 0)
                {
                    item.UsuarioCreacion = pAuthor;
                    item.FechaCreacion = DateTime.Now;
                    item.Eliminado = false;
                    item.RegistroCompleto = ValidarRegistroCompletoContratoPolizaActualizacionListaChequeo(item);
                    _context.ContratoPolizaActualizacionListaChequeo.Add(item);
                }
            }
        }

        private bool ValidarRegistroCompletoContratoPolizaActualizacionListaChequeo(ContratoPolizaActualizacionListaChequeo item)
        {
            if (
                  !item.CumpleDatosAseguradoBeneficiario.HasValue
               || !item.CumpleDatosBeneficiarioGarantiaBancaria.HasValue
               || !item.CumpleDatosTomadorAfianzado.HasValue
               || !item.TieneReciboPagoDatosRequeridos.HasValue
               || !item.TieneCondicionesGeneralesPoliza.HasValue
            ) return false;

            return true;
        }

        private void CreateEditContratoPolizaActualizacionRevisionAprobacionObservacion(ICollection<ContratoPolizaActualizacionRevisionAprobacionObservacion> pListContratoPolizaActualizacionRevisionAprobacionObservacion, string pAuthor)
        {
            foreach (var item in pListContratoPolizaActualizacionRevisionAprobacionObservacion)
            {
                if (item.ContratoPolizaActualizacionRevisionAprobacionObservacionId == 0)
                {
                    item.UsuarioCreacion = pAuthor;
                    item.FechaCreacion = DateTime.Now;
                    item.Eliminado = false;
                    item.RegistroCompleto = ValidarRegistroCompletoContratoPolizaActualizacionRevisionAprobacionObservacion(item);
                }
                else
                {
                    _context.Set<ContratoPolizaActualizacionRevisionAprobacionObservacion>()
                            .Where(c => c.ContratoPolizaActualizacionRevisionAprobacionObservacionId == item.ContratoPolizaActualizacionRevisionAprobacionObservacionId)
                            .Update(c => new ContratoPolizaActualizacionRevisionAprobacionObservacion
                            {
                                UsuarioModificacion = pAuthor,
                                FechaModificacion = DateTime.Now,

                                SegundaFechaRevision = item.SegundaFechaRevision,
                                EstadoSegundaRevision = item.EstadoSegundaRevision,
                                FechaAprobacion = item.FechaAprobacion,
                                ResponsableAprobacionId = item.ResponsableAprobacionId,
                                ObservacionGeneral = item.ObservacionGeneral,
                                Archivada = false,
                                RegistroCompleto = ValidarRegistroCompletoContratoPolizaActualizacionRevisionAprobacionObservacion(item)
                            });

                }
            }
        }

        private bool ValidarRegistroCompletoContratoPolizaActualizacionRevisionAprobacionObservacion(ContratoPolizaActualizacionRevisionAprobacionObservacion pItem)
        {
            if (
                    !pItem.SegundaFechaRevision.HasValue
                 || !pItem.FechaAprobacion.HasValue
                 || string.IsNullOrEmpty(pItem.EstadoSegundaRevision)
                 || string.IsNullOrEmpty(pItem.ObservacionGeneral)
                 || pItem.ResponsableAprobacionId == 0
                 ) return false;

            return true;
        }

        private void CreateEditContratoPolizaActualizacionSeguro(ICollection<ContratoPolizaActualizacionSeguro> pListContratoPolizaActualizacionSeguro, string pAuthor)
        {
            foreach (var ContratoPolizaActualizacionSeguro in pListContratoPolizaActualizacionSeguro)
            {
                if (ContratoPolizaActualizacionSeguro.ContratoPolizaActualizacionSeguroId == 0)
                {
                    ContratoPolizaActualizacionSeguro.UsuarioCreacion = pAuthor;
                    ContratoPolizaActualizacionSeguro.Eliminado = false;
                    ContratoPolizaActualizacionSeguro.FechaCreacion = DateTime.Now;
                    ContratoPolizaActualizacionSeguro.RegistroCompletoActualizacion = ValidarRegistroCompletoContratoPolizaActualizacion(ContratoPolizaActualizacionSeguro);

                }
                else
                {
                    _context.Set<ContratoPolizaActualizacionSeguro>()
                            .Where(c => c.ContratoPolizaActualizacionSeguroId == ContratoPolizaActualizacionSeguro.ContratoPolizaActualizacionSeguroId)
                            .Update(c => new ContratoPolizaActualizacionSeguro
                            {
                                UsuarioModificacion = pAuthor,
                                FechaModificacion = DateTime.Now,

                                TipoSeguroCodigo = ContratoPolizaActualizacionSeguro.TipoSeguroCodigo,

                                TieneFechaSeguro = ContratoPolizaActualizacionSeguro.TieneFechaSeguro,
                                FechaSeguro = ContratoPolizaActualizacionSeguro.FechaSeguro,

                                TieneFechaVigenciaAmparo = ContratoPolizaActualizacionSeguro.TieneFechaVigenciaAmparo,
                                FechaVigenciaAmparo = ContratoPolizaActualizacionSeguro.FechaVigenciaAmparo,

                                TieneFechaValorAmparo = ContratoPolizaActualizacionSeguro.TieneFechaValorAmparo,
                                FechaValorAmparo = ContratoPolizaActualizacionSeguro.FechaValorAmparo,

                                RegistroCompletoActualizacion = ValidarRegistroCompletoContratoPolizaActualizacion(ContratoPolizaActualizacionSeguro),
                                RegistroCompletoSeguro = ValidarRegistroCompletoContratoPolizaActualizacionSeguro(ContratoPolizaActualizacionSeguro),
                            });
                }
            }
        }

        private bool ValidarRegistroCompletoContratoPolizaActualizacionSeguro(ContratoPolizaActualizacionSeguro pContratoPolizaActualizacionSeguro)
        {
            if (
                (pContratoPolizaActualizacionSeguro.TieneFechaSeguro == true && !pContratoPolizaActualizacionSeguro.FechaSeguro.HasValue)
             || (pContratoPolizaActualizacionSeguro.TieneFechaValorAmparo == true && !pContratoPolizaActualizacionSeguro.FechaValorAmparo.HasValue)
             || (pContratoPolizaActualizacionSeguro.TieneFechaVigenciaAmparo == true && !pContratoPolizaActualizacionSeguro.FechaVigenciaAmparo.HasValue)
                ) return false;

            return true;
        }

        private bool ValidarRegistroCompletoContratoPolizaActualizacion(ContratoPolizaActualizacionSeguro pContratoPolizaActualizacionSeguro)
        {
            if (
                   !pContratoPolizaActualizacionSeguro.TieneFechaSeguro.HasValue
                || !pContratoPolizaActualizacionSeguro.TieneFechaValorAmparo.HasValue
                || !pContratoPolizaActualizacionSeguro.TieneFechaVigenciaAmparo.HasValue
                 ) return false;

            return true;
        }

        private bool ValidarRegistroCompletoContratoPolizaActualizacion(ContratoPolizaActualizacion pContratoPolizaActualizacion)
        {
            if (
                   pContratoPolizaActualizacion.ContratoPolizaActualizacionSeguro.Count() > 0
                || pContratoPolizaActualizacion.ContratoPolizaActualizacionListaChequeo.Count() > 0
                || pContratoPolizaActualizacion.ContratoPolizaActualizacionRevisionAprobacionObservacion.Count() > 0
                || pContratoPolizaActualizacion.ContratoPolizaActualizacionRevisionAprobacionObservacion.Count(r => r.Archivada == false) > 0
                )
                return false;

            foreach (var item in pContratoPolizaActualizacion.ContratoPolizaActualizacionSeguro)
            {
                if (!ValidarRegistroCompletoContratoPolizaActualizacionSeguro(item))
                    return false;

                if (!ValidarRegistroCompletoContratoPolizaActualizacion(item))
                    return false;
            }

            foreach (var item in pContratoPolizaActualizacion.ContratoPolizaActualizacionRevisionAprobacionObservacion)
            {
                if (!ValidarRegistroCompletoContratoPolizaActualizacionRevisionAprobacionObservacion(item))
                    return false;
            }

            foreach (var item in pContratoPolizaActualizacion.ContratoPolizaActualizacionListaChequeo)
            {
                if (!ValidarRegistroCompletoContratoPolizaActualizacionListaChequeo(item))
                    return false;
            }

            return true;
        }

        #endregion
    }
}
