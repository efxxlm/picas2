﻿using asivamosffie.model.APIModels;
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

        #region Create

        public async Task<Respuesta> ChangeStatusContratoPolizaActualizacionSeguro(ContratoPolizaActualizacion pContratoPolizaActualizacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Actualizar_Polizas_Y_Garantias, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                _context.Set<ContratoPolizaActualizacion>()
                        .Where(c => c.ContratoPolizaActualizacionId == pContratoPolizaActualizacion.ContratoPolizaActualizacionId)
                        .Update(c => new ContratoPolizaActualizacion
                        {
                            EstadoActualizacion = pContratoPolizaActualizacion.EstadoActualizacion,
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

        public async Task<Respuesta> DeleteContratoPolizaActualizacion(ContratoPolizaActualizacion pContratoPolizaActualizacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Actualizacion_Polizas, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                _context.Set<ContratoPolizaActualizacion>()
                        .Where(c => c.ContratoPolizaActualizacionId == pContratoPolizaActualizacion.ContratoPolizaActualizacionId)
                        .Update(c => new ContratoPolizaActualizacion
                        {
                            Eliminado = true,
                            UsuarioModificacion = pContratoPolizaActualizacion.UsuarioCreacion,
                            FechaModificacion = DateTime.Now
                        });

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.EliminacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                                                                                               (int)enumeratorMenu.Registrar_actualizacion_de_polizas_y_garantias,
                                                                                               GeneralCodes.OperacionExitosa,
                                                                                               idAccion,
                                                                                               pContratoPolizaActualizacion.UsuarioCreacion,
                                                                                               ConstantCommonMessages.UpdatePolicies.ELIMINAR_ACTUALIZACION_POLIZA
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
                                RegistroCompletoObservacionEspecifica = ValidarRegistroCompletoObservacionEspecifica(pContratoPolizaActualizacion)
                            });
                }

                if (pContratoPolizaActualizacion.ContratoPolizaActualizacionSeguro.Count > 0)
                    CreateEditContratoPolizaActualizacionSeguro(pContratoPolizaActualizacion.ContratoPolizaActualizacionSeguro, pContratoPolizaActualizacion.UsuarioCreacion);

                if (pContratoPolizaActualizacion.ContratoPolizaActualizacionRevisionAprobacionObservacion.Count > 0)
                    CreateEditContratoPolizaActualizacionRevisionAprobacionObservacion(pContratoPolizaActualizacion.ContratoPolizaActualizacionRevisionAprobacionObservacion, pContratoPolizaActualizacion.UsuarioCreacion);

                if (pContratoPolizaActualizacion.ContratoPolizaActualizacionListaChequeo.Count() > 0)
                    CreateEditContratoPolizaActualizacionListaChequeo(pContratoPolizaActualizacion.ContratoPolizaActualizacionListaChequeo, pContratoPolizaActualizacion.UsuarioCreacion);

                _context.SaveChanges();
                await ValidarRegistroCompletoContratoPolizaActualizacion((int)pContratoPolizaActualizacion.ContratoPolizaActualizacionId);

                return new Respuesta
                {

                    Data = pContratoPolizaActualizacion.ContratoPolizaActualizacionId,
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
                    _context.ContratoPolizaActualizacionSeguro.Add(ContratoPolizaActualizacionSeguro);
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

                                TieneValorAmparo = ContratoPolizaActualizacionSeguro.TieneValorAmparo,
                                ValorAmparo = ContratoPolizaActualizacionSeguro.ValorAmparo,

                                RegistroCompletoActualizacion = ValidarRegistroCompletoContratoPolizaActualizacion(ContratoPolizaActualizacionSeguro),
                                RegistroCompletoSeguro = ValidarRegistroCompletoContratoPolizaActualizacionSeguro(ContratoPolizaActualizacionSeguro),
                            });
                }
            }
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

                    _context.ContratoPolizaActualizacionRevisionAprobacionObservacion.Add(item);

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
                else
                {
                    _context.Set<ContratoPolizaActualizacionListaChequeo>()
                            .Where(c => c.ContratoPolizaActualizacionListaChequeoId == item.ContratoPolizaActualizacionListaChequeoId)
                            .Update(c => new ContratoPolizaActualizacionListaChequeo
                            {
                                FechaModificacion = DateTime.Now,
                                UsuarioModificacion = pAuthor,
                                RegistroCompleto = ValidarRegistroCompletoContratoPolizaActualizacionListaChequeo(item),
                                CumpleDatosAseguradoBeneficiario = item.CumpleDatosAseguradoBeneficiario,
                                CumpleDatosBeneficiarioGarantiaBancaria = item.CumpleDatosBeneficiarioGarantiaBancaria,
                                CumpleDatosTomadorAfianzado = item.CumpleDatosTomadorAfianzado,
                                TieneReciboPagoDatosRequeridos = item.TieneReciboPagoDatosRequeridos,
                                TieneCondicionesGeneralesPoliza = item.TieneCondicionesGeneralesPoliza
                            });
                }
            }
        }

        public async Task ValidarRegistroCompletoContratoPolizaActualizacion(int GetContratoPoliza)
        {
            try
            {
                ContratoPoliza contratoPoliza = await this.GetContratoPoliza(GetContratoPoliza, false);

                foreach (var item in contratoPoliza.ContratoPolizaActualizacion)
                {
                    _context.Set<ContratoPolizaActualizacion>()
                            .Where(r => r.ContratoPolizaActualizacionId == item.ContratoPolizaActualizacionId)
                            .Update(r => new ContratoPolizaActualizacion
                            {
                                RegistroCompleto = ValidarRegistroCompletoContratoPolizaActualizacionList(contratoPoliza.ContratoPolizaActualizacion)
                            });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool ValidarRegistroCompletoObservacionEspecifica(ContratoPolizaActualizacion pContratoPolizaActualizacion)
        {
            if (pContratoPolizaActualizacion.TieneObservacionEspecifica == false)
                return true;
            else
                if (string.IsNullOrEmpty(pContratoPolizaActualizacion.ObservacionEspecifica))
                return false;

            return true;
        }

        private bool ValidarRegistroCompletoContratoPolizaActualizacionListaChequeo(ContratoPolizaActualizacionListaChequeo item)
        {
            if (
                   item.CumpleDatosAseguradoBeneficiario != true
               || item.CumpleDatosTomadorAfianzado != true
               || item.TieneReciboPagoDatosRequeridos != true
               || !item.TieneCondicionesGeneralesPoliza.HasValue
               || !item.CumpleDatosBeneficiarioGarantiaBancaria.HasValue
            ) return false;

            return true;
        }

        private bool ValidarRegistroCompletoContratoPolizaActualizacionRevisionAprobacionObservacion(ContratoPolizaActualizacionRevisionAprobacionObservacion pItem)
        {
            if (ConstanCodigoEstadoRevisionPoliza.Devuelta == pItem.EstadoSegundaRevision)
            {
                _context.Set<ContratoPolizaActualizacion>()
                        .Where(c => c.ContratoPolizaActualizacionId == pItem.ContratoPolizaActualizacionId)
                        .Update(c => new ContratoPolizaActualizacion
                        {
                            EstadoActualizacion = ConstanCodigoEstadoActualizacionPoliza.Con_poliza_observada_y_devuelta,
                            RegistroCompleto = false,
                        }); ;
            }


            if (ConstanCodigoEstadoRevisionPoliza.Aprobacion != pItem.EstadoSegundaRevision)
                return false;

            if (
                    !pItem.SegundaFechaRevision.HasValue
                 || string.IsNullOrEmpty(pItem.EstadoSegundaRevision)
                 || !pItem.FechaAprobacion.HasValue
                 || pItem.ResponsableAprobacionId == 0
                 ) return false;

            return true;
        }

        private bool ValidarRegistroCompletoContratoPolizaActualizacionSeguro(ContratoPolizaActualizacionSeguro pContratoPolizaActualizacionSeguro)
        {
            if (
                (pContratoPolizaActualizacionSeguro.TieneFechaSeguro == true && !pContratoPolizaActualizacionSeguro.FechaSeguro.HasValue)
             || (pContratoPolizaActualizacionSeguro.TieneValorAmparo == true && !pContratoPolizaActualizacionSeguro.ValorAmparo.HasValue)
             || (pContratoPolizaActualizacionSeguro.TieneFechaVigenciaAmparo == true && !pContratoPolizaActualizacionSeguro.FechaVigenciaAmparo.HasValue)
                ) return false;

            return true;
        }

        private bool ValidarRegistroCompletoContratoPolizaActualizacion(ContratoPolizaActualizacionSeguro pContratoPolizaActualizacionSeguro)
        {
            if (
                   !pContratoPolizaActualizacionSeguro.TieneFechaSeguro.HasValue
                || !pContratoPolizaActualizacionSeguro.TieneValorAmparo.HasValue
                || !pContratoPolizaActualizacionSeguro.TieneFechaVigenciaAmparo.HasValue
                 ) return false;

            return true;
        }

        private bool ValidarRegistroCompletoContratoPolizaActualizacion(ContratoPolizaActualizacion pContratoPolizaActualizacion)
        {
            if (
                   pContratoPolizaActualizacion.ContratoPolizaActualizacionSeguro.Count() == 0
                || pContratoPolizaActualizacion.ContratoPolizaActualizacionListaChequeo.Count() == 0
                || pContratoPolizaActualizacion.ContratoPolizaActualizacionRevisionAprobacionObservacion.Count() == 0
                || pContratoPolizaActualizacion.ContratoPolizaActualizacionRevisionAprobacionObservacion.Count(r => r.Archivada == false) == 0
                )
                return false;

            foreach (var item in pContratoPolizaActualizacion.ContratoPolizaActualizacionSeguro)
            {
                if (!ValidarRegistroCompletoContratoPolizaActualizacionSeguro(item))
                    return false;

                if (!ValidarRegistroCompletoContratoPolizaActualizacion(item))
                    return false;
            }

            //foreach (var item in pContratoPolizaActualizacion.ContratoPolizaActualizacionRevisionAprobacionObservacion)
            //{
            if (!ValidarRegistroCompletoContratoPolizaActualizacionRevisionAprobacionObservacion(pContratoPolizaActualizacion.ContratoPolizaActualizacionRevisionAprobacionObservacion.OrderByDescending(r => r.ContratoPolizaActualizacionRevisionAprobacionObservacionId).FirstOrDefault()))
                return false;
            //}

            foreach (var item in pContratoPolizaActualizacion.ContratoPolizaActualizacionListaChequeo)
            {
                if (!ValidarRegistroCompletoContratoPolizaActualizacionListaChequeo(item))
                    return false;
            }

            return true;
        }

        private bool ValidarRegistroCompletoContratoPolizaActualizacionList(ICollection<ContratoPolizaActualizacion> ListContratoPolizaActualizacion)
        {
            foreach (var pContratoPolizaActualizacion in ListContratoPolizaActualizacion)
            {

                if (
                        pContratoPolizaActualizacion.ContratoPolizaActualizacionSeguro.Count() == 0
                     || pContratoPolizaActualizacion.ContratoPolizaActualizacionListaChequeo.Count() == 0
                     || pContratoPolizaActualizacion.ContratoPolizaActualizacionRevisionAprobacionObservacion.Count() == 0
                     || pContratoPolizaActualizacion.ContratoPolizaActualizacionRevisionAprobacionObservacion.Count(r => r.Archivada == false) == 0
                     )
                    return false;

                foreach (var item in pContratoPolizaActualizacion.ContratoPolizaActualizacionSeguro)
                {
                    if (!ValidarRegistroCompletoContratoPolizaActualizacionSeguro(item))
                        return false;

                    if (!ValidarRegistroCompletoContratoPolizaActualizacion(item))
                        return false;
                }

                if (!ValidarRegistroCompletoContratoPolizaActualizacionRevisionAprobacionObservacion(pContratoPolizaActualizacion.ContratoPolizaActualizacionRevisionAprobacionObservacion.OrderByDescending(r => r.ContratoPolizaActualizacionRevisionAprobacionObservacionId).FirstOrDefault()))
                    return false;

                foreach (var item in pContratoPolizaActualizacion.ContratoPolizaActualizacionListaChequeo)
                {
                    if (!ValidarRegistroCompletoContratoPolizaActualizacionListaChequeo(item))
                        return false;
                }
            }

            return true;
        }

        #endregion

        #region Get
        public async Task<dynamic> GetContratoByNumeroContrato(string pNumeroContrato)
        {
            try
            {
                return await _context.Contrato
                               .Include(c => c.Contratacion).ThenInclude(c => c.Contratista)
                               .Include(c => c.ContratoPoliza)
                               //.ThenInclude(c => c.ContratoPolizaActualizacion)
                               .Where(c => c.NumeroContrato.ToLower().Trim().Contains(pNumeroContrato.ToLower().Trim())
                                   && c.ContratoPoliza.Count() > 0
                                   //   && c.ContratoPoliza.All(r => r.ContratoPolizaActualizacion.Count() == 0)
                                   )
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

        public async Task<ContratoPoliza> GetContratoPoliza(int pContratoPolizaId, bool? pEsNueva)
        {

            ContratoPoliza contratoPoliza = new ContratoPoliza();
            List<ContratoPolizaActualizacion> contratoPolizaActualizacion = new List<ContratoPolizaActualizacion>();
            if (pEsNueva != true)
            {
                contratoPolizaActualizacion = await _context.ContratoPolizaActualizacion
                                                                              .Where(r => r.ContratoPolizaActualizacionId == pContratoPolizaId)
                                                                              .Include(c => c.ContratoPolizaActualizacionSeguro)
                                                                              .Include(c => c.ContratoPolizaActualizacionListaChequeo)
                                                                              .Include(c => c.ContratoPolizaActualizacionRevisionAprobacionObservacion)
                                                                              .AsNoTracking()
                                                                              .ToListAsync();

                contratoPoliza = await _context.ContratoPoliza
                               .Where(c => c.ContratoPolizaId == contratoPolizaActualizacion.FirstOrDefault().ContratoPolizaId)
                               .Include(c => c.Contrato).ThenInclude(c => c.Contratacion).ThenInclude(c => c.Contratista)
                               .Include(c => c.PolizaGarantia)
                               .Include(c => c.PolizaListaChequeo)
                               .Include(c => c.PolizaObservacion)
                               .AsNoTracking()
                               .FirstOrDefaultAsync();

            }
            else
            {
                contratoPoliza = await _context.ContratoPoliza
                    .Where(c => c.ContratoPolizaId == pContratoPolizaId)
                    .Include(c => c.Contrato).ThenInclude(c => c.Contratacion).ThenInclude(c => c.Contratista)
                    .Include(c => c.PolizaGarantia)
                    .Include(c => c.PolizaListaChequeo)
                    .Include(c => c.PolizaObservacion)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

            }

            contratoPoliza.ContratoPolizaActualizacion = contratoPolizaActualizacion;

            GetRemoveDeleteItems(contratoPoliza);

            return contratoPoliza;
        }

        public async Task<List<VActualizacionPolizaYgarantias>> GetListVActualizacionPolizaYGarantias()
        {
            return await _context.VActualizacionPolizaYgarantias.ToListAsync();
        }

        private void GetRemoveDeleteItems(ContratoPoliza contratoPoliza)
        {

            if (contratoPoliza.ContratoPolizaActualizacion.Count() > 0)
                contratoPoliza.ContratoPolizaActualizacion = contratoPoliza.ContratoPolizaActualizacion.Where(r => r.Eliminado != true).ToList();
            foreach (var ContratoPolizaActualizacion in contratoPoliza.ContratoPolizaActualizacion)
            {
                if (ContratoPolizaActualizacion.ContratoPolizaActualizacionSeguro.Count() > 0)
                    ContratoPolizaActualizacion.ContratoPolizaActualizacionSeguro = ContratoPolizaActualizacion.ContratoPolizaActualizacionSeguro.Where(c => c.Eliminado != true).ToList();
            }
        }

        #endregion
    }
}
