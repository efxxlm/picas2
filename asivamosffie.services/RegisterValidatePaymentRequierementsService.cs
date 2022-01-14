using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
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
    public class RegisterValidatePaymentRequierementsService : IRegisterValidatePaymentRequierementsService
    {
        #region constructor
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;

        public RegisterValidatePaymentRequierementsService(IDocumentService documentService, devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _documentService = documentService;
            _commonService = commonService;
            _context = context;
        }
        private int Enum = 1;
        #endregion

        #region Tablas Relacionadas Para Pagos
        //0# Traer Forma de Pago por Fase
        public async Task<dynamic> GetFormaPagoCodigoByFase(bool pEsPreconstruccion, int pContratoId)
        {
            bool EsInterventoria = _context.Contrato.Include(r => r.Contratacion)
                                                    .Count(r => r.ContratoId == pContratoId
                                                        && r.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoSolicitudContratoSolicitudPago.Contratos_Interventoria
                                                          ) > 0;


            List<dynamic> ListDynamics = new List<dynamic>();
            List<string> strCriterios = _context.FormasPagoFase.Where(r => r.EsPreconstruccion == pEsPreconstruccion && r.EsInterventoria == EsInterventoria).Select(r => r.FormaPagoCodigo).ToList();
            List<Dominio> ListCriterio = await _commonService.GetListDominioByIdTipoDominio((int)EnumeratorTipoDominio.Formas_Pago);

            foreach (var l in strCriterios)
            {
                ListDynamics.Add(new
                {
                    Codigo = l,
                    Nombre = ListCriterio.Where(lc => lc.Codigo == l).FirstOrDefault().Nombre
                });
            }

            return ListDynamics;
        }
        //1# Traer criterio de pago por Forma de pago
        public async Task<dynamic> GetCriterioByFormaPagoCodigo(string pFormaPagoCodigo)
        {
            List<dynamic> ListDynamics = new List<dynamic>();
            List<string> strCriterios = _context.FormaPagoCriterioPago.Where(r => r.FormaPagoCodigo == pFormaPagoCodigo).Select(r => r.CriterioPagoCodigo).ToList();
            List<Dominio> ListCriterio = await _commonService.GetListDominioByIdTipoDominio((int)EnumeratorTipoDominio.Criterios_Pago);

            strCriterios.ForEach(l =>
            {
                ListDynamics.Add(new
                {
                    Codigo = l,
                    Nombre = ListCriterio.Where(lc => lc.Codigo == l).FirstOrDefault().Nombre
                });
            });
            return ListDynamics;
        }
        //2# Traer Tipo de pago por Criterio de pago
        public async Task<dynamic> GetTipoPagoByCriterioCodigo(string pCriterioCodigo)
        {
            List<dynamic> ListDynamics = new List<dynamic>();
            List<string> strCriterios = _context.CriterioCodigoTipoPagoCodigo.Where(r => r.CriterioCodigo == pCriterioCodigo).Select(r => r.TipoPagoCodigo).ToList();
            List<Dominio> ListCriterio = await _commonService.GetListDominioByIdTipoDominio((int)EnumeratorTipoDominio.Tipo_Pago_Obra_Interventoria);

            strCriterios.ForEach(l =>
            {
                ListDynamics.Add(new
                {
                    Codigo = l,
                    Nombre = ListCriterio.Where(lc => lc.Codigo == l).FirstOrDefault().Nombre
                });
            });
            return ListDynamics;
        }
        //3# Traer Concepto de pago por tipo de pago
        public async Task<dynamic> GetConceptoPagoCriterioCodigoByTipoPagoCodigo(string TipoPagoCodigo)
        {
            List<dynamic> ListDynamics = new List<dynamic>();
            List<string> strCriterios = _context.TipoPagoConceptoPagoCriterio
                                                                            .Where(r => r.TipoPagoCodigo == TipoPagoCodigo)
                                                                            .Select(r => r.ConceptoPagoCriterioCodigo)
                                                                            .ToList();

            List<Dominio> ListCriterio = await _commonService.GetListDominioByIdTipoDominio((int)EnumeratorTipoDominio.Concepto_Pago_Criterio_Obra_Interventoria);

            strCriterios.ForEach(l =>
            {
                ListDynamics.Add(new
                {
                    Codigo = l,
                    Nombre = ListCriterio.Where(lc => lc.Codigo == l).FirstOrDefault().Nombre
                });
            });
            return ListDynamics;
        }
        //4# Traer Uso Por Concepto de pago
        public async Task<dynamic> GetUsoByConceptoPagoCriterioCodigo(string pConceptoPagoCodigo, int pContratoId)
        {
            try
            {
                List<dynamic> ListDynamics = new List<dynamic>();
                List<string> strCriterios = _context.ConceptoPagoUso.Where(r => r.ConceptoPagoCodigo == pConceptoPagoCodigo).Select(r => r.Uso).ToList();
                List<Dominio> ListUsos = await _commonService.GetListDominioByIdTipoDominio((int)EnumeratorTipoDominio.Usos);

                List<VValorUsoXcontratoId> vValorUsoXcontratoId = _context.VValorUsoXcontratoId.Where(r => r.ContratoId == pContratoId && strCriterios.Contains(r.TipoUsoCodigo)).ToList();

                return vValorUsoXcontratoId;
            }
            catch (Exception ex)
            {
                return new { };
            }
        }
        #endregion
        //5# Traer Uso Por Concepto de pago 2 Orden Giro
        public async Task<dynamic> GetUsoByConceptoPagoCodigo(string pConceptoPagoCodigo)
        {
            try
            {
                List<dynamic> ListDynamics = new List<dynamic>();
                List<string> strCriterios = _context.ConceptoPagoUso.Where(r => r.ConceptoPagoCodigo == pConceptoPagoCodigo).Select(r => r.Uso).ToList();
                List<Dominio> ListUsos = await _commonService.GetListDominioByIdTipoDominio((int)EnumeratorTipoDominio.Usos);

                strCriterios.ForEach(l =>
                {
                    ListDynamics.Add(new
                    {
                        Codigo = l,
                        Nombre = ListUsos.Where(lc => lc.Codigo == l).FirstOrDefault().Nombre
                    });
                });
                return ListDynamics;
            }
            catch (Exception ex)
            {
                return new { };
            }
        }

        #region Create Edit Delete
        public async Task<dynamic> GetProyectosByIdContrato(int pContratoId)
        {
            var resultContrato = _context.Contrato
                .Where(r => r.ContratoId == pContratoId)
                .Include(cp => cp.ContratoPoliza)
                                                .Select(c => new
                                                {
                                                    c.NumeroContrato,
                                                    c.ContratoPoliza.FirstOrDefault().FechaAprobacion,
                                                    PlazoDias = c.PlazoFase1PreDias + c.PlazoFase2ConstruccionDias,
                                                    PlazoMeses = c.PlazoFase1PreMeses + c.PlazoFase2ConstruccionMeses
                                                }).FirstOrDefault();

            var resultProyectos = await _context.VProyectosXcontrato
                                                                    .Where(p => p.ContratoId == pContratoId)
                                                                                                            .Select(p => new
                                                                                                            {
                                                                                                                p.LlaveMen,
                                                                                                                p.TipoIntervencion,
                                                                                                                p.Departamento,
                                                                                                                p.Municipio,
                                                                                                                p.InstitucionEducativa,
                                                                                                                p.Sede,
                                                                                                                p.ContratacionProyectoId,
                                                                                                                p.ValorTotal
                                                                                                            }).ToListAsync();
            List<dynamic> dynamics = new List<dynamic>
            {
                resultContrato,
                resultProyectos
            };

            return dynamics;

        }

        public async Task<Respuesta> DeleteSolicitudPago(int pSolicitudPagoId, string pUsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Solicitud_Pago, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                await _context.Set<SolicitudPago>()
                                             .Where(o => o.SolicitudPagoId == pSolicitudPagoId)
                                                                                            .UpdateAsync(r => new SolicitudPago()
                                                                                            {
                                                                                                Eliminado = true,
                                                                                                UsuarioModificacion = pUsuarioModificacion,
                                                                                                FechaModificacion = DateTime.Now,
                                                                                            });

                return
                         new Respuesta
                         {
                             IsSuccessful = true,
                             IsException = false,
                             IsValidation = false,
                             Code = GeneralCodes.OperacionExitosa,
                             Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.OperacionExitosa, idAccion, pUsuarioModificacion, "ELIMINAR SOLICITUD PAGO")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString())
                    };
            }
        }

        #region  Tipo Obra Interventoria

        public async Task GetValidateSolicitudPagoId(int SolicitudPagoId)
        {
            try
            {
                SolicitudPago solicitudPago = _context.SolicitudPago.Find(SolicitudPagoId);
                solicitudPago = GetSolicitudPagoComplete(solicitudPago);
                bool CompleteRecord = ValidateCompleteRecordSolicitudPago(solicitudPago);
                bool TieneNoCumpleListaChequeo = solicitudPago.SolicitudPagoListaChequeo.Any(r => r.SolicitudPagoListaChequeoRespuesta.Any(s => s.RespuestaCodigo == ConstanCodigoRespuestasListaChequeoSolictudPago.No_cumple));
                string EstadoSolicitudPago = solicitudPago.EstadoCodigo;
                bool TieneAlgunaObservacionPendiente =
                    _context.SolicitudPagoObservacion.Any(s => s.SolicitudPagoId == SolicitudPagoId
                                                            && s.Archivada != true
                                                            && s.TieneObservacion == true
                                                            );
                DateTime? FechaRegistroCompleto = null;

                if (CompleteRecord && !TieneAlgunaObservacionPendiente)
                {
                    FechaRegistroCompleto = DateTime.Now;
                    EstadoSolicitudPago = ((int)EnumEstadoSolicitudPago.Con_solicitud_revisada_por_equipo_facturacion).ToString();
                }
                else
                {
                    CompleteRecord = false;
                    if (EstadoSolicitudPago == ((int)EnumEstadoSolicitudPago.Con_solicitud_revisada_por_equipo_facturacion).ToString())
                        EstadoSolicitudPago = ((int)EnumEstadoSolicitudPago.En_proceso_de_registro).ToString();

                }

                await _context.Set<SolicitudPago>()
                              .Where(s => s.SolicitudPagoId == SolicitudPagoId)
                              .UpdateAsync(r => new SolicitudPago()
                              {
                                  TieneObservacion = TieneAlgunaObservacionPendiente,
                                  EstadoCodigo = EstadoSolicitudPago,
                                  RegistroCompleto = CompleteRecord,
                                  FechaRegistroCompleto = FechaRegistroCompleto,
                                  TieneNoCumpleListaChequeo = TieneNoCumpleListaChequeo
                              });
            }
            catch (Exception e)
            {

            }
        }

        public async Task<Respuesta> ReturnSolicitudPago(SolicitudPago pSolicitudPago)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Devolver_Solicitud_Pago, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                await _context.Set<SolicitudPago>()
                                                  .Where(s => s.SolicitudPagoId == pSolicitudPago.SolicitudPagoId)
                                                                      .UpdateAsync(r => new SolicitudPago()
                                                                      {
                                                                          UsuarioModificacion = pSolicitudPago.UsuarioModificacion,
                                                                          FechaModificacion = pSolicitudPago.FechaModificacion,
                                                                          EstadoCodigo = pSolicitudPago.EstadoCodigo,
                                                                          FechaRadicacionSacContratista = pSolicitudPago.FechaRadicacionSacContratista,
                                                                          NumeroRadicacionSacContratista = pSolicitudPago.NumeroRadicacionSacContratista
                                                                      });

                return
                     new Respuesta
                     {
                         IsSuccessful = true,
                         IsException = false,
                         IsValidation = false,
                         Code = GeneralCodes.OperacionExitosa,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.OperacionExitosa, idAccion, pSolicitudPago.UsuarioCreacion, "DEVOLVER SOLICITUD DE PAGO")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.Error, idAccion, pSolicitudPago.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> DeleteSolicitudPagoFaseFacturaDescuento(int pSolicitudPagoFaseFacturaDescuentoId, string pUsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Descuento, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                await _context.Set<SolicitudPagoFaseFacturaDescuento>()
                                       .Where(s => s.SolicitudPagoFaseFacturaDescuentoId == pSolicitudPagoFaseFacturaDescuentoId)
                                                                   .UpdateAsync(r => new SolicitudPagoFaseFacturaDescuento()
                                                                   {
                                                                       Eliminado = true,
                                                                       UsuarioModificacion = pUsuarioModificacion,
                                                                       FechaModificacion = DateTime.Now
                                                                   });

                return
                     new Respuesta
                     {
                         IsSuccessful = true,
                         IsException = false,
                         IsValidation = false,
                         Code = GeneralCodes.OperacionExitosa,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                                                                                                 (int)enumeratorMenu.Registrar_validar_requisitos_de_pago,
                                                                                                 GeneralCodes.OperacionExitosa,
                                                                                                 idAccion,
                                                                                                 pUsuarioModificacion,
                                                                                                 "ELIMINAR SOLICITUD PAGO FASE CRITERIO PROYECTO"
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString())
                    };
            }
        }

        //public async Task<Respuesta> DeleteSolicitudLlaveCriterioProyecto(int pContratacionProyectoId, string pUsuarioModificacion)
        //{
        //    int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Llave_Criterio_Proyecto, (int)EnumeratorTipoDominio.Acciones);

        //    try
        //    {
        //        List<SolicitudPagoFaseCriterioProyecto> ListSolicitudPagoFaseCriterioProyectoDelete = _context.SolicitudPagoFaseCriterioProyecto.Where(s => s.ContratacionProyectoId == pContratacionProyectoId).ToList();

        //        foreach (var SolicitudPagoFaseCriterioProyecto in ListSolicitudPagoFaseCriterioProyectoDelete)
        //        {
        //            SolicitudPagoFaseCriterioProyecto.FechaModificacion = DateTime.Now;
        //            SolicitudPagoFaseCriterioProyecto.UsuarioModificacion = pUsuarioModificacion;
        //            SolicitudPagoFaseCriterioProyecto.Eliminado = true;
        //        }

        //        return
        //             new Respuesta
        //             {
        //                 IsSuccessful = true,
        //                 IsException = false,
        //                 IsValidation = false,
        //                 Code = GeneralCodes.OperacionExitosa,
        //                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.OperacionExitosa, idAccion, pUsuarioModificacion, "ELIMINAR SOLICITUD PAGO FASE CRITERIO PROYECTO")
        //             };
        //    }
        //    catch (Exception ex)
        //    {
        //        return
        //            new Respuesta
        //            {
        //                IsSuccessful = false,
        //                IsException = true,
        //                IsValidation = false,
        //                Code = GeneralCodes.Error,
        //                Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString())
        //            };
        //    }
        //}

        public async Task<Respuesta> DeleteSolicitudPagoFaseCriterioConceptoPago(int pSolicitudPagoFaseCriterioConceptoId, bool pEsConcepto, string pUsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Criterio_Pago, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                await _context.Set<SolicitudPagoFaseCriterioConceptoPago>()
                                                                           .Where(r => r.SolicitudPagoFaseCriterioConceptoPagoId == pSolicitudPagoFaseCriterioConceptoId)
                                                                                                           .UpdateAsync(r => new SolicitudPagoFaseCriterioConceptoPago
                                                                                                           {
                                                                                                               Eliminado = true,
                                                                                                           });

                SolicitudPagoFaseCriterioConceptoPago solicitudPagoFaseCriterioConceptoPagoOld =
                                                                    _context.SolicitudPagoFaseCriterioConceptoPago.Where(r => r.SolicitudPagoFaseCriterioConceptoPagoId == pSolicitudPagoFaseCriterioConceptoId)
                                                                                                                  .AsNoTracking()
                                                                                                                  .FirstOrDefault();

                SolicitudPagoFaseCriterio solicitudPagoFaseCriterioOld = _context.SolicitudPagoFaseCriterio.Where(r => r.SolicitudPagoFaseCriterioId == solicitudPagoFaseCriterioConceptoPagoOld.SolicitudPagoFaseCriterioId)
                                                                                                           .AsNoTracking()
                                                                                                           .FirstOrDefault();

                solicitudPagoFaseCriterioOld.ValorFacturado -= solicitudPagoFaseCriterioConceptoPagoOld.ValorFacturadoConcepto;
                await _context.Set<SolicitudPagoFaseCriterio>()
                                                        .Where(r => r.SolicitudPagoFaseCriterioId == solicitudPagoFaseCriterioOld.SolicitudPagoFaseCriterioId)
                                                                                                                                                    .UpdateAsync(r => new SolicitudPagoFaseCriterio
                                                                                                                                                    {
                                                                                                                                                        FechaModificacion = DateTime.Now,
                                                                                                                                                        UsuarioModificacion = pUsuarioModificacion,
                                                                                                                                                        ValorFacturado = solicitudPagoFaseCriterioOld.ValorFacturado,
                                                                                                                                                        Eliminado = pEsConcepto == true ? false : true
                                                                                                                                                    });
                //le quito el que estoy eliminando 
                if (!pEsConcepto)
                {
                    int totalCriterios = _context.SolicitudPagoFaseCriterio.Where(r => r.SolicitudPagoFaseId == solicitudPagoFaseCriterioOld.SolicitudPagoFaseId && r.Eliminado != true && r.RegistroCompleto == true).Count();

                    if ((totalCriterios - 1) < 1)
                    {
                        await _context.Set<SolicitudPagoFase>()
                                            .Where(r => r.SolicitudPagoFaseId == solicitudPagoFaseCriterioOld.SolicitudPagoFaseId)
                                                                                                                                        .UpdateAsync(r => new SolicitudPagoFase
                                                                                                                                        {
                                                                                                                                            FechaModificacion = DateTime.Now,
                                                                                                                                            UsuarioModificacion = pUsuarioModificacion,
                                                                                                                                            RegistroCompleto = false,
                                                                                                                                            RegistroCompletoCriterio = false
                                                                                                                                        });
                    }
                }

                return
                     new Respuesta
                     {
                         IsSuccessful = true,
                         IsException = false,
                         IsValidation = false,
                         Code = GeneralCodes.OperacionExitosa,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.OperacionExitosa, idAccion, pUsuarioModificacion, "ELIMINAR SOLICITUD PAGO FASE CRITERIO CONCEPTO PAGO")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> DeleteSolicitudPagoFaseCriterio(int pSolicitudPagoFaseCriterioId, string pUsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Criterio_Pago, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                await _context.Set<SolicitudPagoFaseCriterio>()
                                                               .Where(r => r.SolicitudPagoFaseCriterioId == pSolicitudPagoFaseCriterioId)
                                                                                               .UpdateAsync(r => new SolicitudPagoFaseCriterio
                                                                                               {
                                                                                                   FechaModificacion = DateTime.Now,
                                                                                                   UsuarioModificacion = pUsuarioModificacion,
                                                                                                   Eliminado = true,
                                                                                               });

                //Eliminar Lista de Chequeo Asociada al criterio 

                string strCodigoCriterio = _context.SolicitudPagoFaseCriterio.Find(pSolicitudPagoFaseCriterioId).TipoCriterioCodigo;

                int SolicitudPagoId = _context.SolicitudPagoFaseCriterio.Where(s => s.SolicitudPagoFaseCriterioId == pSolicitudPagoFaseCriterioId)
                                                                           .Include(f => f.SolicitudPagoFase)
                                                                             .ThenInclude(r => r.SolicitudPagoRegistrarSolicitudPago)
                                                                                       .ThenInclude(r => r.SolicitudPago)
                                                                           .Select(s => s.SolicitudPagoFase.SolicitudPagoRegistrarSolicitudPago.SolicitudPago.SolicitudPagoId)
                                                                                                                                                     .FirstOrDefault();

                int? SolicitudPagoListaChequeoId = _context.SolicitudPago.Include(s => s.SolicitudPagoListaChequeo)
                                                                               .ThenInclude(s => s.ListaChequeo)
                                                                        .Where(s => s.SolicitudPagoId == SolicitudPagoId
                                                                            && s.SolicitudPagoListaChequeo.FirstOrDefault().ListaChequeo.CriterioPagoCodigo == strCodigoCriterio)
                                                                        .Select(r => r.SolicitudPagoId).FirstOrDefault();

                if (SolicitudPagoListaChequeoId > 0)
                {
                    await _context.Set<SolicitudPagoListaChequeo>()
                                                           .Where(r => r.SolicitudPagoListaChequeoId == SolicitudPagoListaChequeoId)
                                                                                           .UpdateAsync(r => new SolicitudPagoListaChequeo
                                                                                           {
                                                                                               FechaModificacion = DateTime.Now,
                                                                                               UsuarioModificacion = pUsuarioModificacion,
                                                                                               Eliminado = true,
                                                                                           });
                }
                return
                     new Respuesta
                     {
                         IsSuccessful = true,
                         IsException = false,
                         IsValidation = false,
                         Code = GeneralCodes.OperacionExitosa,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.OperacionExitosa, idAccion, pUsuarioModificacion, "ELIMINAR SOLICITUD PAGO FASE CRITERIO")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString())
                    };
            }
        }

        //public async Task<Respuesta> DeleteSolicitudPagoFaseCriterioProyecto(int SolicitudPagoFaseCriterioProyectoId, string pUsuarioModificacion)
        //{
        //    int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Criterio_Proyecto, (int)EnumeratorTipoDominio.Acciones);

        //    try
        //    {
        //        SolicitudPagoFaseCriterioProyecto SolicitudPagoFaseCriterioProyectoDelete = _context.SolicitudPagoFaseCriterioProyecto.Find(SolicitudPagoFaseCriterioProyectoId);
        //        SolicitudPagoFaseCriterioProyectoDelete.FechaModificacion = DateTime.Now;
        //        SolicitudPagoFaseCriterioProyectoDelete.UsuarioModificacion = pUsuarioModificacion;
        //        SolicitudPagoFaseCriterioProyectoDelete.Eliminado = true;

        //        return
        //             new Respuesta
        //             {
        //                 IsSuccessful = true,
        //                 IsException = false,
        //                 IsValidation = false,
        //                 Code = GeneralCodes.OperacionExitosa,
        //                 Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.OperacionExitosa, idAccion, pUsuarioModificacion, "ELIMINAR SOLICITUD PAGO FASE CRITERIO")
        //             };
        //    }
        //    catch (Exception ex)
        //    {
        //        return
        //            new Respuesta
        //            {
        //                IsSuccessful = false,
        //                IsException = true,
        //                IsValidation = false,
        //                Code = GeneralCodes.Error,
        //                Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString())
        //            };
        //    }
        //}

        private async void CreateEditNewPaymentNew(SolicitudPago pSolicitudPago)
        {
            //Valida si el contrato de la solicitud es interventoria o Obra
            string strInterventoriaCodigo = _context.Contrato
                                             .Include(ctr => ctr.Contratacion)
                                             .Where(s => s.ContratoId == pSolicitudPago.ContratoId)
                                             .Select(crt => crt.Contratacion.TipoSolicitudCodigo)
                                             .FirstOrDefault();

            if (pSolicitudPago.SolicitudPagoSoporteSolicitud.Count() > 0)
                CreateEditNewSolicitudPagoSoporteSolicitud(pSolicitudPago.SolicitudPagoSoporteSolicitud, pSolicitudPago.UsuarioCreacion);

            if (pSolicitudPago.SolicitudPagoId > 0)
            {
                decimal? ValorFacturado =
                    pSolicitudPago?.SolicitudPagoRegistrarSolicitudPago?
                    .Sum(r => r.SolicitudPagoFase.Sum(r => r.SolicitudPagoFaseCriterio.Where(r => r.Eliminado != true).Sum(r => r.ValorFacturado)));

                bool? blEsAnticipo = pSolicitudPago.SolicitudPagoRegistrarSolicitudPago.Any(r => r.SolicitudPagoFase.Any(r => r.SolicitudPagoFaseCriterio.Any(r => r.TipoCriterioCodigo == ConstanCodigoSolicitudPagoCriterio.Anticipo)));

                _context.Set<SolicitudPago>()
                        .Where(s => s.SolicitudPagoId == pSolicitudPago.SolicitudPagoId)
                        .Update(s => new SolicitudPago
                        {
                            EsAnticipio = blEsAnticipo,
                            EsFactura = pSolicitudPago.EsFactura,
                            FechaModificacion = DateTime.Now,
                            TieneObservacion = false,
                            ObservacionDevolucionOrdenGiro = null,
                            UsuarioModificacion = pSolicitudPago.UsuarioCreacion,
                            ValorFacturado = ValorFacturado,
                            EstadoCodigo = ((int)EnumEstadoSolicitudPago.En_proceso_de_registro).ToString()
                        });
            }
            else
            {
                pSolicitudPago.ValorFacturado =
                     pSolicitudPago?
                    .SolicitudPagoFactura?
                    .Sum(r => r.ValorFacturado);

                pSolicitudPago.TieneObservacion = false;
                pSolicitudPago.NumeroSolicitud = Int32.Parse(strInterventoriaCodigo) == ConstanCodigoTipoContratacion.Obra ? await _commonService.EnumeradorSolicitudPago(true) : await _commonService.EnumeradorSolicitudPago(false);
                pSolicitudPago.EstadoCodigo = ((int)EnumEstadoSolicitudPago.En_proceso_de_registro).ToString();
                pSolicitudPago.Eliminado = false;
                pSolicitudPago.FechaCreacion = DateTime.Now;
                _context.SolicitudPago.Add(pSolicitudPago);
            }
        }

        private void CreateEditNewSolicitudPagoSoporteSolicitud(ICollection<SolicitudPagoSoporteSolicitud> pSolicitudPagoSoporteSolicitudList, string pUsuarioCreacion)
        {
            foreach (var SolicitudPagoSoporteSolicitud in pSolicitudPagoSoporteSolicitudList)
            {
                if (SolicitudPagoSoporteSolicitud.SolicitudPagoSoporteSolicitudId > 0)
                {
                    SolicitudPagoSoporteSolicitud solicitudPagoSoporteSolicitudOld = _context.SolicitudPagoSoporteSolicitud.Find(SolicitudPagoSoporteSolicitud.SolicitudPagoSoporteSolicitudId);
                    solicitudPagoSoporteSolicitudOld.UsuarioModificacion = pUsuarioCreacion;
                    solicitudPagoSoporteSolicitudOld.FechaModificacion = DateTime.Now;
                    solicitudPagoSoporteSolicitudOld.RegistroCompleto = ValidateCompleteRecordSolicitudPagoSoporteSolicitud(SolicitudPagoSoporteSolicitud);

                    solicitudPagoSoporteSolicitudOld.UrlSoporte = SolicitudPagoSoporteSolicitud.UrlSoporte;
                }
                else
                {
                    SolicitudPagoSoporteSolicitud.UsuarioCreacion = pUsuarioCreacion;
                    SolicitudPagoSoporteSolicitud.FechaCreacion = DateTime.Now;
                    SolicitudPagoSoporteSolicitud.Eliminado = false;
                    SolicitudPagoSoporteSolicitud.RegistroCompleto = ValidateCompleteRecordSolicitudPagoSoporteSolicitud(SolicitudPagoSoporteSolicitud);

                    _context.SolicitudPagoSoporteSolicitud.Add(SolicitudPagoSoporteSolicitud);
                }
            }
        }

        public async Task<Respuesta> CreateEditNewPayment(SolicitudPago pSolicitudPago)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Solicitud_De_Pago, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                CreateEditNewPaymentNew(pSolicitudPago);

                if (pSolicitudPago.SolicitudPagoCargarFormaPago.Count() > 0)
                {
                    CreateEditNewPaymentWayToPay(pSolicitudPago.SolicitudPagoCargarFormaPago, pSolicitudPago.UsuarioCreacion);
                }

                if (pSolicitudPago.SolicitudPagoFactura.Count() > 0)
                    CreateEditSolicitudPagoFaseFactura(pSolicitudPago.SolicitudPagoFactura, pSolicitudPago.UsuarioCreacion);


                if (pSolicitudPago.SolicitudPagoRegistrarSolicitudPago.Count() > 0)
                {
                    CreateEditRegistrarSolicitudPago(pSolicitudPago.SolicitudPagoRegistrarSolicitudPago, pSolicitudPago.UsuarioCreacion);

                    foreach (var SolicitudPagoRegistrarSolicitudPago in pSolicitudPago.SolicitudPagoRegistrarSolicitudPago)
                    {
                        CreateEditSolicitudPagoFase(SolicitudPagoRegistrarSolicitudPago.SolicitudPagoFase, pSolicitudPago.UsuarioCreacion);
                    }
                }

                CreateEditListaChequeoRespuesta(pSolicitudPago.SolicitudPagoListaChequeo, pSolicitudPago.UsuarioCreacion);

                ArchivarObservaciones(pSolicitudPago.SolicitudPagoId, pSolicitudPago.UsuarioCreacion);

                return
                     new Respuesta
                     {
                         IsSuccessful = true,
                         IsException = false,
                         Data = pSolicitudPago,
                         IsValidation = false,
                         Code = GeneralCodes.OperacionExitosa,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.OperacionExitosa, idAccion, pSolicitudPago.UsuarioCreacion, pSolicitudPago.FechaModificacion.HasValue ? "EDITAR SOLICITUD DE PAGO" : "CREAR SOLICITUD DE PAGO")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.Error, idAccion, pSolicitudPago.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        private void ArchivarObservaciones(int pSolicitudPagoId, string pAuthor)
        {
            _context.Set<SolicitudPagoObservacion>()
                    .Where(r => r.SolicitudPagoId == pSolicitudPagoId)
                    .Update(r => new SolicitudPagoObservacion
                    {
                        UsuarioModificacion = pAuthor,
                        Archivada = true,
                        FechaModificacion = DateTime.Now
                    });
        }

        private void CreateEditNewPaymentWayToPay(ICollection<SolicitudPagoCargarFormaPago> pListSolicitudPagoCargarFormaPago, string usuarioCreacion)
        {
            try
            {
                foreach (var pSolicitudPagoCargarFormaPago in pListSolicitudPagoCargarFormaPago)
                {
                    if (pSolicitudPagoCargarFormaPago.SolicitudPagoCargarFormaPagoId > 0)
                    {
                        SolicitudPagoCargarFormaPago solicitudPagoCargarFormaPagoOld = _context.SolicitudPagoCargarFormaPago.Find(pSolicitudPagoCargarFormaPago.SolicitudPagoCargarFormaPagoId);
                        solicitudPagoCargarFormaPagoOld.FechaModificacion = DateTime.Now;
                        solicitudPagoCargarFormaPagoOld.TieneFase1 = pSolicitudPagoCargarFormaPago.TieneFase1;

                        solicitudPagoCargarFormaPagoOld.RegistroCompleto = ValidateCompleteRecordSolicitudPagoCargarFormaPago(pSolicitudPagoCargarFormaPago);
                        solicitudPagoCargarFormaPagoOld.FaseConstruccionFormaPagoCodigo = pSolicitudPagoCargarFormaPago.FaseConstruccionFormaPagoCodigo;
                        solicitudPagoCargarFormaPagoOld.FasePreConstruccionFormaPagoCodigo = pSolicitudPagoCargarFormaPago.FasePreConstruccionFormaPagoCodigo;
                    }
                    else
                    {
                        pSolicitudPagoCargarFormaPago.FechaCreacion = DateTime.Now;
                        pSolicitudPagoCargarFormaPago.Eliminado = false;
                        pSolicitudPagoCargarFormaPago.RegistroCompleto = ValidateCompleteRecordSolicitudPagoCargarFormaPago(pSolicitudPagoCargarFormaPago);

                        _context.SolicitudPagoCargarFormaPago.Add(pSolicitudPagoCargarFormaPago);
                    }
                }

            }
            catch (Exception)
            {

            }
        }

        private void CreateEditRegistrarSolicitudPago(ICollection<SolicitudPagoRegistrarSolicitudPago> ListSolicitudPagoRegistrarSolicitudPago, string pAuhor)
        {
            foreach (var solicitudPagoRegistrarSolicitudPago in ListSolicitudPagoRegistrarSolicitudPago)
            {
                if (solicitudPagoRegistrarSolicitudPago.SolicitudPagoRegistrarSolicitudPagoId > 0)
                {
                    SolicitudPagoRegistrarSolicitudPago solicitudPagoRegistrarSolicitudPagoOld = _context.SolicitudPagoRegistrarSolicitudPago.Find(solicitudPagoRegistrarSolicitudPago.SolicitudPagoRegistrarSolicitudPagoId);
                    solicitudPagoRegistrarSolicitudPagoOld.FechaModificacion = DateTime.Now;
                    solicitudPagoRegistrarSolicitudPagoOld.UsuarioModificacion = pAuhor;
                    solicitudPagoRegistrarSolicitudPagoOld.RegistroCompleto = ValidateCompleteRecordSolicitudPagoRegistrarSolicitudPago(solicitudPagoRegistrarSolicitudPago);
                    solicitudPagoRegistrarSolicitudPagoOld.TieneFasePreconstruccion = solicitudPagoRegistrarSolicitudPago.TieneFasePreconstruccion;
                    solicitudPagoRegistrarSolicitudPagoOld.FechaSolicitud = solicitudPagoRegistrarSolicitudPago.FechaSolicitud;
                    solicitudPagoRegistrarSolicitudPagoOld.TieneFaseConstruccion = solicitudPagoRegistrarSolicitudPago.TieneFaseConstruccion;
                    solicitudPagoRegistrarSolicitudPagoOld.FechaSolicitud = solicitudPagoRegistrarSolicitudPago.FechaSolicitud;
                    solicitudPagoRegistrarSolicitudPagoOld.NumeroRadicadoSac = solicitudPagoRegistrarSolicitudPago.NumeroRadicadoSac;
                }
                else
                {
                    solicitudPagoRegistrarSolicitudPago.UsuarioCreacion = pAuhor;
                    solicitudPagoRegistrarSolicitudPago.FechaCreacion = DateTime.Now;
                    solicitudPagoRegistrarSolicitudPago.Eliminado = false;
                    solicitudPagoRegistrarSolicitudPago.RegistroCompleto = ValidateCompleteRecordSolicitudPagoRegistrarSolicitudPago(solicitudPagoRegistrarSolicitudPago);

                    _context.SolicitudPagoRegistrarSolicitudPago.Add(solicitudPagoRegistrarSolicitudPago);
                }
            }
        }

        private void CreateEditListaChequeoRespuesta(ICollection<SolicitudPagoListaChequeo> pListSolicitudPagoListaChequeo, string usuarioCreacion)
        {
            foreach (var SolicitudPagoListaChequeo in pListSolicitudPagoListaChequeo)
            {
                bool blRegistroCompletoListaChequeo = true;

                if (SolicitudPagoListaChequeo.SolicitudPagoListaChequeoRespuesta.Count() != SolicitudPagoListaChequeo.SolicitudPagoListaChequeoRespuesta.Where(r => r.RespuestaCodigo != null).ToList().Count())
                    blRegistroCompletoListaChequeo = false;

                //if (SolicitudPagoListaChequeo.SolicitudPagoListaChequeoRespuesta.Any(s => s.RespuestaCodigo == ConstanCodigoRespuestasSolicitudPago.No_Cumple))
                //    blRegistroCompletoListaChequeo = false;

                SolicitudPagoListaChequeo.SolicitudPagoListaChequeoRespuesta = SolicitudPagoListaChequeo.SolicitudPagoListaChequeoRespuesta.Where(r => r.RespuestaCodigo != null).ToList();

                foreach (var SolicitudPagoListaChequeoRespuesta in SolicitudPagoListaChequeo.SolicitudPagoListaChequeoRespuesta)
                {
                    bool RegistroCompletoItem = ValidarRegistroCompletoSolicitudPagoListaChequeoRespuesta(SolicitudPagoListaChequeoRespuesta);

                    if (!RegistroCompletoItem)
                        blRegistroCompletoListaChequeo = false;

                    _context.Set<SolicitudPagoListaChequeoRespuesta>()
                                                   .Where(s => s.SolicitudPagoListaChequeoRespuestaId == SolicitudPagoListaChequeoRespuesta.SolicitudPagoListaChequeoRespuestaId)
                                                                                .Update(s => new SolicitudPagoListaChequeoRespuesta
                                                                                {
                                                                                    ValidacionObservacion = SolicitudPagoListaChequeoRespuesta.ValidacionObservacion,
                                                                                    VerificacionObservacion = SolicitudPagoListaChequeoRespuesta.VerificacionObservacion,
                                                                                    TieneSubsanacion = SolicitudPagoListaChequeoRespuesta.TieneSubsanacion,
                                                                                    FechaModificacion = DateTime.Now,
                                                                                    RegistroCompleto = RegistroCompletoItem,
                                                                                    UsuarioModificacion = usuarioCreacion,
                                                                                    RespuestaCodigo = SolicitudPagoListaChequeoRespuesta.RespuestaCodigo,
                                                                                    Observacion = SolicitudPagoListaChequeoRespuesta.Observacion,
                                                                                });
                }
                _context.Set<SolicitudPagoListaChequeo>().Where(r => r.SolicitudPagoListaChequeoId == SolicitudPagoListaChequeo.SolicitudPagoListaChequeoId)
                                                         .Update(s => new SolicitudPagoListaChequeo
                                                         {
                                                             RegistroCompleto = blRegistroCompletoListaChequeo,
                                                             FechaModificacion = DateTime.Now,
                                                             UsuarioModificacion = usuarioCreacion
                                                         });
            }
        }

        private void CreateEditSolicitudPagoFaseDescuento(ICollection<SolicitudPagoFaseFacturaDescuento> pSolicitudPagoFaseDescuentoList, string pUsusarioCreacion)
        {
            foreach (var SolicitudPagoFaseDescuento in pSolicitudPagoFaseDescuentoList)
            {
                if (SolicitudPagoFaseDescuento.SolicitudPagoFaseFacturaDescuentoId > 0)
                {
                    SolicitudPagoFaseFacturaDescuento solicitudPagoFaseDescuentoOld = _context.SolicitudPagoFaseFacturaDescuento.Find(SolicitudPagoFaseDescuento.SolicitudPagoFaseFacturaDescuentoId);
                    solicitudPagoFaseDescuentoOld.TipoDescuentoCodigo = SolicitudPagoFaseDescuento.TipoDescuentoCodigo;
                    solicitudPagoFaseDescuentoOld.ValorDescuento = SolicitudPagoFaseDescuento.ValorDescuento;

                    solicitudPagoFaseDescuentoOld.UsuarioModificacion = pUsusarioCreacion;
                    solicitudPagoFaseDescuentoOld.FechaModificacion = DateTime.Now;
                    solicitudPagoFaseDescuentoOld.RegistroCompleto = ValidateCompleteRecordSolicitudPagoFaseFacturaDescuento(SolicitudPagoFaseDescuento);
                }
                else
                {
                    SolicitudPagoFaseDescuento.UsuarioCreacion = pUsusarioCreacion;
                    SolicitudPagoFaseDescuento.FechaCreacion = DateTime.Now;
                    SolicitudPagoFaseDescuento.Eliminado = false;
                    SolicitudPagoFaseDescuento.RegistroCompleto = ValidateCompleteRecordSolicitudPagoFaseFacturaDescuento(SolicitudPagoFaseDescuento);

                    _context.SolicitudPagoFaseFacturaDescuento.Add(SolicitudPagoFaseDescuento);
                }
            }
        }

        private void CreateEditSolicitudPagoFase(ICollection<SolicitudPagoFase> solicitudPagoFaseList, string pUsuarioCreacion)
        {
            foreach (var SolicitudPagoFase in solicitudPagoFaseList)
            {
                try
                {
                    if (SolicitudPagoFase.SolicitudPagoFaseCriterio.Count() > 0)
                        CreateEditSolicitudPagoFaseCriterio(SolicitudPagoFase.SolicitudPagoFaseCriterio, SolicitudPagoFase.UsuarioCreacion);

                    if (!SolicitudPagoFase.EsPreconstruccion)
                        if (SolicitudPagoFase.SolicitudPagoFaseAmortizacion.Count() > 0)
                            CreateEditSolicitudPagoSolicitudPagoAmortizacion(SolicitudPagoFase.SolicitudPagoFaseAmortizacion, (int)SolicitudPagoFase.ContratacionProyectoId, pUsuarioCreacion);

                    if (SolicitudPagoFase.SolicitudPagoFaseId > 0)
                    {
                        SolicitudPagoFase solicitudPagoFaseOld = _context.SolicitudPagoFase.Find(SolicitudPagoFase.SolicitudPagoFaseId);

                        bool blRegistroCompleto = false;
                        if (SolicitudPagoFase.SolicitudPagoFaseCriterio.Count() > 0)
                            blRegistroCompleto = ValidateCompleteRecordSolicitudPagoFaseCriterio2(SolicitudPagoFase.SolicitudPagoFaseCriterio);

                        _context.Set<SolicitudPagoFase>()
                                .Where(r => r.SolicitudPagoFaseId == SolicitudPagoFase.SolicitudPagoFaseId)
                                .Update(r => new SolicitudPagoFase
                                {
                                    RegistroCompletoCriterio = blRegistroCompleto,
                                    UsuarioModificacion = pUsuarioCreacion,
                                    FechaModificacion = DateTime.Now,
                                    TieneDescuento = SolicitudPagoFase.TieneDescuento,
                                    RegistroCompleto = ValidateCompleteRecordSolicitudPagoFase(SolicitudPagoFase)
                                });
                    }
                    else
                    {
                        if (SolicitudPagoFase.SolicitudPagoFaseCriterio.Count() > 0)
                            SolicitudPagoFase.RegistroCompletoCriterio = ValidateCompleteRecordSolicitudPagoFaseCriterio2(SolicitudPagoFase.SolicitudPagoFaseCriterio);

                        SolicitudPagoFase.FechaCreacion = DateTime.Now;
                        SolicitudPagoFase.Eliminado = false;
                        SolicitudPagoFase.UsuarioCreacion = pUsuarioCreacion;
                        SolicitudPagoFase.RegistroCompleto = ValidateCompleteRecordSolicitudPagoFase(SolicitudPagoFase);
                        _context.SolicitudPagoFase.Add(SolicitudPagoFase);
                    }
                    CreateEditSolicitudPagoFaseDescuento(SolicitudPagoFase.SolicitudPagoFaseFacturaDescuento, pUsuarioCreacion);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        private void CreateEditSolicitudPagoSolicitudPagoAmortizacion(ICollection<SolicitudPagoFaseAmortizacion> pSolicitudPagoAmortizacionList, int ContratacionProyectoId, string pUsuarioCreacion)
        {

            ContratacionProyecto contratacionProyecto = _context.ContratacionProyecto.Find(ContratacionProyectoId);

            int SolicitudPagoFaseCriterioConceptoPagoId = (int)_context.VAmortizacionXproyecto.Where(r => r.ContratacionProyectoId == ContratacionProyectoId).FirstOrDefault().SolicitudPagoFaseCriterioConceptoPagoId;

            foreach (var SolicitudPagoAmortizacion in pSolicitudPagoAmortizacionList)
            {
                if (SolicitudPagoAmortizacion.SolicitudPagoFaseAmortizacionId > 0)
                {
                    SolicitudPagoFaseAmortizacion solicitudPagoAmortizacionOld = _context.SolicitudPagoFaseAmortizacion.Find(SolicitudPagoAmortizacion.SolicitudPagoFaseAmortizacionId);
                    solicitudPagoAmortizacionOld.UsuarioModificacion = pUsuarioCreacion;
                    solicitudPagoAmortizacionOld.FechaModificacion = DateTime.Now;
                    solicitudPagoAmortizacionOld.SolicitudPagoFaseCriterioConceptoPagoId = SolicitudPagoFaseCriterioConceptoPagoId;
                    solicitudPagoAmortizacionOld.PorcentajeAmortizacion = SolicitudPagoAmortizacion.PorcentajeAmortizacion;
                    solicitudPagoAmortizacionOld.ValorAmortizacion = SolicitudPagoAmortizacion.ValorAmortizacion;
                    solicitudPagoAmortizacionOld.ConceptoCodigo = SolicitudPagoAmortizacion.ConceptoCodigo;
                    solicitudPagoAmortizacionOld.RegistroCompleto = ValidateCompleteRecordSolicitudPagoAmortizacion(SolicitudPagoAmortizacion);
                }
                else

                {
                    SolicitudPagoAmortizacion.SolicitudPagoFaseCriterioConceptoPagoId = SolicitudPagoFaseCriterioConceptoPagoId;
                    SolicitudPagoAmortizacion.Eliminado = false;
                    SolicitudPagoAmortizacion.UsuarioCreacion = pUsuarioCreacion;
                    SolicitudPagoAmortizacion.FechaCreacion = DateTime.Now;
                    SolicitudPagoAmortizacion.RegistroCompleto = ValidateCompleteRecordSolicitudPagoAmortizacion(SolicitudPagoAmortizacion);

                    _context.SolicitudPagoFaseAmortizacion.Add(SolicitudPagoAmortizacion);
                }
            }
        }

        private void CreateEditSolicitudPagoFaseFactura(ICollection<SolicitudPagoFactura> pSolicitudPagoFacturaList, string pUsuarioCreacion)
        {
            foreach (var SolicitudPagoFactura in pSolicitudPagoFacturaList)
            {
                if (SolicitudPagoFactura.SolicitudPagoFacturaId > 0)
                {
                    SolicitudPagoFactura solicitudPagoFaseFacturaOld = _context.SolicitudPagoFactura.Find(SolicitudPagoFactura.SolicitudPagoFacturaId);
                    solicitudPagoFaseFacturaOld.UsuarioModificacion = SolicitudPagoFactura.UsuarioModificacion;
                    solicitudPagoFaseFacturaOld.FechaModificacion = SolicitudPagoFactura.FechaModificacion;
                    solicitudPagoFaseFacturaOld.RegistroCompleto = ValidateCompleteRecordSolicitudPagoFaseFactura(SolicitudPagoFactura);

                    solicitudPagoFaseFacturaOld.Fecha = SolicitudPagoFactura.Fecha;
                    solicitudPagoFaseFacturaOld.ValorFacturado = SolicitudPagoFactura.ValorFacturado;
                    solicitudPagoFaseFacturaOld.Numero = SolicitudPagoFactura.Numero;
                    solicitudPagoFaseFacturaOld.ValorFacturadoConDescuento = SolicitudPagoFactura.ValorFacturadoConDescuento;
                }
                else
                {
                    SolicitudPagoFactura.Eliminado = false;
                    SolicitudPagoFactura.UsuarioCreacion = pUsuarioCreacion;
                    SolicitudPagoFactura.FechaCreacion = DateTime.Now;
                    SolicitudPagoFactura.RegistroCompleto = ValidateCompleteRecordSolicitudPagoFaseFactura(SolicitudPagoFactura);
                    _context.SolicitudPagoFactura.Add(SolicitudPagoFactura);
                }
            }
        }

        private void CreateEditSolicitudPagoFaseCriterio(ICollection<SolicitudPagoFaseCriterio> ListSolicitudPagoFaseCriterio, string strUsuarioCreacion)
        {
            foreach (var SolicitudPagoFaseCriterio in ListSolicitudPagoFaseCriterio)
            {
                try
                {
                    CreateEditListaChequeoByCriterio(SolicitudPagoFaseCriterio, strUsuarioCreacion);

                    foreach (var SolicitudPagoFaseCriterioConceptoPago in SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioConceptoPago)
                    {
                        if (SolicitudPagoFaseCriterioConceptoPago.SolicitudPagoFaseCriterioConceptoPagoId > 0)
                        {
                            _context.Set<SolicitudPagoFaseCriterioConceptoPago>()
                                    .Where(s => s.SolicitudPagoFaseCriterioConceptoPagoId == SolicitudPagoFaseCriterioConceptoPago.SolicitudPagoFaseCriterioConceptoPagoId)
                                    .Update(s => new SolicitudPagoFaseCriterioConceptoPago
                                    {
                                        ConceptoPagoCriterio = SolicitudPagoFaseCriterioConceptoPago.ConceptoPagoCriterio,
                                        UsoCodigo = SolicitudPagoFaseCriterioConceptoPago.UsoCodigo,
                                        ValorFacturadoConcepto = SolicitudPagoFaseCriterioConceptoPago.ValorFacturadoConcepto
                                    });
                        }
                        else
                        {
                            SolicitudPagoFaseCriterioConceptoPago.Eliminado = false;
                            SolicitudPagoFaseCriterioConceptoPago.UsuarioCreacion = strUsuarioCreacion;
                            SolicitudPagoFaseCriterioConceptoPago.FechaCreacion = DateTime.Now;
                            SolicitudPagoFaseCriterioConceptoPago.ConceptoPagoCriterio = SolicitudPagoFaseCriterioConceptoPago.ConceptoPagoCriterio;
                            SolicitudPagoFaseCriterioConceptoPago.UsoCodigo = SolicitudPagoFaseCriterioConceptoPago.UsoCodigo;
                            _context.SolicitudPagoFaseCriterioConceptoPago.Add(SolicitudPagoFaseCriterioConceptoPago);
                        }

                        if (SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioId > 0)
                        {
                            _context.Set<SolicitudPagoFaseCriterio>()
                                     .Where(s => s.SolicitudPagoFaseCriterioId == SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioId)
                                     .Update(s => new SolicitudPagoFaseCriterio
                                     {
                                         FechaModificacion = DateTime.Now,
                                         UsuarioModificacion = strUsuarioCreacion,
                                         ValorFacturado = SolicitudPagoFaseCriterio.ValorFacturado,
                                         SolicitudPagoFaseId = SolicitudPagoFaseCriterio.SolicitudPagoFaseId,
                                         TipoCriterioCodigo = SolicitudPagoFaseCriterio.TipoCriterioCodigo,
                                         RegistroCompleto = ValidateCompleteRecordSolicitudPagoFaseCriterio(SolicitudPagoFaseCriterio)
                                     });
                        }
                        else
                        {
                            SolicitudPagoFaseCriterio.UsuarioCreacion = strUsuarioCreacion;
                            SolicitudPagoFaseCriterio.FechaCreacion = DateTime.Now;
                            SolicitudPagoFaseCriterio.Eliminado = false;
                            SolicitudPagoFaseCriterio.RegistroCompleto = ValidateCompleteRecordSolicitudPagoFaseCriterio(SolicitudPagoFaseCriterio);
                            _context.SolicitudPagoFaseCriterio.Add(SolicitudPagoFaseCriterio);

                        }
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        private void CreateEditListaChequeoByCriterio(SolicitudPagoFaseCriterio pSolicitudPagoFaseCriterio, string pUsuarioCreacion)
        {
            ListaChequeo listaChequeo = _context.ListaChequeo
                                                           .Where(l => l.CriterioPagoCodigo == pSolicitudPagoFaseCriterio.TipoCriterioCodigo
                                                                    && l.Eliminado != true)
                                                           .Include(r => r.ListaChequeoListaChequeoItem)
                                                                                                        .FirstOrDefault();

            if (listaChequeo != null)
            {
                int? SolicitudPagoId = _context.SolicitudPagoFase
                                        .Where(r => r.SolicitudPagoFaseId == pSolicitudPagoFaseCriterio.SolicitudPagoFaseId)
                                            .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
                                                    .ThenInclude(r => r.SolicitudPago)
                                        .Select(s => s.SolicitudPagoRegistrarSolicitudPago.SolicitudPago.SolicitudPagoId)
                                                                                                                        .FirstOrDefault();


                int? SolicitudPagoListaChequeoId = _context.SolicitudPagoListaChequeo.Where(s => s.SolicitudPagoId == SolicitudPagoId
                                                                                    && s.Eliminado != true
                                                                                    && s.TipoCriterioCodigo == pSolicitudPagoFaseCriterio.TipoCriterioCodigo)
                                                                                .Select(r => r.SolicitudPagoId).FirstOrDefault();



                if (SolicitudPagoListaChequeoId == 0 && SolicitudPagoId > 0)
                {

                    SolicitudPagoListaChequeo solicitudPagoListaChequeoNew = new SolicitudPagoListaChequeo
                    {
                        UsuarioCreacion = pUsuarioCreacion,
                        FechaCreacion = DateTime.Now,
                        Eliminado = false,
                        RegistroCompleto = false,
                        TipoCriterioCodigo = pSolicitudPagoFaseCriterio.TipoCriterioCodigo,
                        SolicitudPagoId = (int)SolicitudPagoId,
                        ListaChequeoId = listaChequeo.ListaChequeoId
                    };
                    _context.SolicitudPagoListaChequeo.Add(solicitudPagoListaChequeoNew);
                    _context.SaveChanges();


                    foreach (var ListaChequeoListaChequeoItem in listaChequeo.ListaChequeoListaChequeoItem)
                    {
                        SolicitudPagoListaChequeoRespuesta solicitudPagoListaChequeoRespuestaNew = new SolicitudPagoListaChequeoRespuesta
                        {
                            UsuarioCreacion = pUsuarioCreacion,
                            FechaCreacion = DateTime.Now,
                            Eliminado = false,

                            SolicitudPagoListaChequeoId = solicitudPagoListaChequeoNew.SolicitudPagoListaChequeoId,
                            ListaChequeoItemId = ListaChequeoListaChequeoItem.ListaChequeoItemId,
                        };

                        solicitudPagoListaChequeoRespuestaNew.RegistroCompleto = ValidarRegistroCompletoSolicitudPagoListaChequeoRespuesta(solicitudPagoListaChequeoRespuestaNew);
                        _context.SolicitudPagoListaChequeoRespuesta.Add(solicitudPagoListaChequeoRespuestaNew);
                    }
                }

            }

        }

        private bool ValidarRegistroCompletoSolicitudPagoListaChequeoRespuesta(SolicitudPagoListaChequeoRespuesta pSolicitudPagoListaChequeoRespuestaNew)
        {
            if (string.IsNullOrEmpty(pSolicitudPagoListaChequeoRespuestaNew.RespuestaCodigo))
                return false;

            if (pSolicitudPagoListaChequeoRespuestaNew.RespuestaCodigo == ConstanCodigoRespuestasListaChequeoSolictudPago.No_cumple)
                if (string.IsNullOrEmpty(pSolicitudPagoListaChequeoRespuestaNew.Observacion))
                    return false;

            return true;
        }

        //private void CreateEditSolicitudPagoFaseCriterioProyecto(ICollection<SolicitudPagoFaseCriterioProyecto> ListSolicitudPagoFaseCriterioProyecto, string pStrUsuarioCreacion)
        //{
        //    foreach (var SolicitudPagoFaseCriterioProyecto in ListSolicitudPagoFaseCriterioProyecto)
        //    {
        //        if (SolicitudPagoFaseCriterioProyecto.SolicitudPagoFaseCriterioProyectoId > 0)
        //        {
        //            SolicitudPagoFaseCriterioProyecto solicitudPagoFaseCriterioProyectoOld = _context.SolicitudPagoFaseCriterioProyecto.Find(SolicitudPagoFaseCriterioProyecto.SolicitudPagoFaseCriterioProyectoId);

        //            solicitudPagoFaseCriterioProyectoOld.UsuarioModificacion = pStrUsuarioCreacion;
        //            solicitudPagoFaseCriterioProyectoOld.FechaModificacion = DateTime.Now;
        //            solicitudPagoFaseCriterioProyectoOld.ValorFacturado = SolicitudPagoFaseCriterioProyecto.ValorFacturado;
        //            solicitudPagoFaseCriterioProyectoOld.RegistroCompleto = ValidateCompleteRecordSolicitudPagoFaseCriterioProyecto(SolicitudPagoFaseCriterioProyecto);
        //        }
        //        else
        //        {
        //            SolicitudPagoFaseCriterioProyecto.Eliminado = false;
        //            SolicitudPagoFaseCriterioProyecto.UsuarioCreacion = pStrUsuarioCreacion;
        //            SolicitudPagoFaseCriterioProyecto.FechaCreacion = DateTime.Now;
        //            SolicitudPagoFaseCriterioProyecto.RegistroCompleto = ValidateCompleteRecordSolicitudPagoFaseCriterioProyecto(SolicitudPagoFaseCriterioProyecto);
        //            _context.SolicitudPagoFaseCriterioProyecto.Add(SolicitudPagoFaseCriterioProyecto);
        //        }
        //    }
        //}

        #endregion

        #region Tipo Create Expensas 

        public async Task<Respuesta> CreateEditExpensas(SolicitudPago pSolicitudPago)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Solicitud_De_Pago, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                CreateEditNewExpensas(pSolicitudPago);

                if (pSolicitudPago.SolicitudPagoSoporteSolicitud.Count() > 0)
                    CreateEditNewSolicitudPagoSoporteSolicitud(pSolicitudPago.SolicitudPagoSoporteSolicitud, pSolicitudPago.UsuarioCreacion);

                _context.SaveChanges();
                return
                     new Respuesta
                     {
                         IsSuccessful = true,
                         IsException = false,
                         Data = pSolicitudPago,
                         IsValidation = false,
                         Code = GeneralCodes.OperacionExitosa,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.OperacionExitosa, idAccion, pSolicitudPago.UsuarioCreacion, pSolicitudPago.FechaModificacion.HasValue ? "EDITAR SOLICITUD DE PAGO" : "CREAR SOLICITUD DE PAGO")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.Error, idAccion, pSolicitudPago.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        private async void CreateEditNewExpensas(SolicitudPago pSolicitudPago)
        {
            if (pSolicitudPago.SolicitudPagoExpensas.Count() > 0)
            {
                CreateEditSolicitudPagoExpensas(pSolicitudPago.SolicitudPagoExpensas, pSolicitudPago.UsuarioCreacion);
            }

            if (pSolicitudPago.SolicitudPagoId > 0)
            {
                SolicitudPago solicitudPagoOld = _context.SolicitudPago.Find(pSolicitudPago.SolicitudPagoId);
                solicitudPagoOld.ValorFacturado = pSolicitudPago?.SolicitudPagoExpensas?.FirstOrDefault()?.ValorFacturado;
                solicitudPagoOld.FechaModificacion = DateTime.Now;
                solicitudPagoOld.UsuarioModificacion = pSolicitudPago.UsuarioCreacion;
                solicitudPagoOld.RegistroCompleto = ValidateCompleteRecordSolicitudPagoExpensas(pSolicitudPago);
            }
            else
            {
                pSolicitudPago.ContratoId = _context.ContratacionProyecto
                    .Where(r => r.ContratacionProyectoId == pSolicitudPago.ContratacionProyectoId)
                    .Include(r => r.Contratacion)
                    .ThenInclude(r => r.Contrato).Select
                    (c => c.Contratacion.Contrato.FirstOrDefault().ContratoId).FirstOrDefault();

                pSolicitudPago.ValorFacturado = pSolicitudPago?.SolicitudPagoExpensas?.FirstOrDefault()?.ValorFacturado;
                pSolicitudPago.EstadoCodigo = ((int)EnumEstadoSolicitudPago.En_proceso_de_registro).ToString();
                pSolicitudPago.NumeroSolicitud = await _commonService.EnumeradorSolicitudPagoExpensasAndOtros();
                pSolicitudPago.FechaCreacion = DateTime.Now;
                pSolicitudPago.Eliminado = false;
                //pSolicitudPago.RegistroCompleto = ValidateCompleteRecordSolicitudPagoExpensas(pSolicitudPago);

                _context.SolicitudPago.Add(pSolicitudPago);
                _context.SaveChanges();

                //Crear Lista Chequeo 
                CreateEditListaChequeoByEstadoMenuCodigo(ConstanTipoListaChequeo.Expensas, pSolicitudPago.SolicitudPagoId, pSolicitudPago.UsuarioCreacion);

            }
        }

        private void CreateEditSolicitudPagoExpensas(ICollection<SolicitudPagoExpensas> solicitudPagoExpensas, string usuarioCreacion)
        {
            foreach (var SolicitudPagoExpensas in solicitudPagoExpensas)
            {
                if (SolicitudPagoExpensas.SolicitudPagoExpensasId > 0)
                {
                    SolicitudPagoExpensas solicitudPagoExpensasOld = _context.SolicitudPagoExpensas.Find(SolicitudPagoExpensas.SolicitudPagoExpensasId);

                    solicitudPagoExpensasOld.NumeroRadicadoSac = SolicitudPagoExpensas.NumeroRadicadoSac;
                    solicitudPagoExpensasOld.NumeroFactura = SolicitudPagoExpensas.NumeroFactura;
                    solicitudPagoExpensasOld.ValorFacturado = SolicitudPagoExpensas.ValorFacturado;
                    solicitudPagoExpensasOld.TipoPagoCodigo = SolicitudPagoExpensas.TipoPagoCodigo;
                    solicitudPagoExpensasOld.ConceptoPagoCriterioCodigo = SolicitudPagoExpensas.ConceptoPagoCriterioCodigo;
                    solicitudPagoExpensasOld.ValorFacturadoConcepto = SolicitudPagoExpensas.ValorFacturadoConcepto;

                    solicitudPagoExpensasOld.UsuarioModificacion = usuarioCreacion;
                    solicitudPagoExpensasOld.FechaModificacion = DateTime.Now;
                    solicitudPagoExpensasOld.RegistroCompleto = ValidateCompleteRecordSolicitudPagoExpensas(SolicitudPagoExpensas);

                }
                else
                {
                    SolicitudPagoExpensas.UsuarioCreacion = usuarioCreacion;
                    SolicitudPagoExpensas.FechaCreacion = DateTime.Now;
                    SolicitudPagoExpensas.Eliminado = false;
                    SolicitudPagoExpensas.RegistroCompleto = ValidateCompleteRecordSolicitudPagoExpensas(SolicitudPagoExpensas);

                    _context.SolicitudPagoExpensas.Add(SolicitudPagoExpensas);
                }
            }
        }

        private bool ValidateCompleteRecordSolicitudPagoExpensas(SolicitudPagoExpensas solicitudPagoExpensas)
        {
            if (
                   string.IsNullOrEmpty(solicitudPagoExpensas.NumeroRadicadoSac)
                || string.IsNullOrEmpty(solicitudPagoExpensas.NumeroFactura)
                || solicitudPagoExpensas.ValorFacturado == 0
                || string.IsNullOrEmpty(solicitudPagoExpensas.TipoPagoCodigo)
                || string.IsNullOrEmpty(solicitudPagoExpensas.ConceptoPagoCriterioCodigo)
                || solicitudPagoExpensas.ValorFacturadoConcepto == 0
                    )
                return false;
            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoExpensas(SolicitudPago pSolicitudPagos)
        {
            SolicitudPago pSolicitudPago = _context.SolicitudPago
                .Where(r => r.SolicitudPagoId == pSolicitudPagos.SolicitudPagoId)
                .Include(r => r.SolicitudPagoSoporteSolicitud)
                .Include(r => r.SolicitudPagoExpensas)
                .Include(r => r.SolicitudPagoListaChequeo)
                .AsNoTracking()
                .FirstOrDefault();

            if (
                   pSolicitudPago?.SolicitudPagoExpensas?.Count() == 0
                || pSolicitudPago?.SolicitudPagoSoporteSolicitud?.Count() == 0
                || pSolicitudPago?.SolicitudPagoListaChequeo?.Count() == 0)
                return false;

            foreach (var SolicitudPagoExpensas in pSolicitudPago.SolicitudPagoExpensas)
            {
                if (SolicitudPagoExpensas.RegistroCompleto != true)
                    return false;
            }

            foreach (var SolicitudPagoSoporteSolicitud in pSolicitudPago.SolicitudPagoSoporteSolicitud)
            {
                if (SolicitudPagoSoporteSolicitud.RegistroCompleto != true)
                    return false;
            }

            foreach (var SolicitudPagoListaChequeo in pSolicitudPago.SolicitudPagoListaChequeo)
            {
                if (SolicitudPagoListaChequeo.RegistroCompleto != true)
                    return false;
            }

            return true;
        }

        private void CreateEditListaChequeoByEstadoMenuCodigo(string pEstadoMenuCodigo, int pSolicitudPagoId, string pUsuarioCreacion)
        {
            ListaChequeo listaChequeo = _context.ListaChequeo
                                                           .Where(l => l.EstadoMenuCodigo == pEstadoMenuCodigo
                                                                    && l.Eliminado != true)
                                                           .Include(r => r.ListaChequeoListaChequeoItem)
                                                                                                        .FirstOrDefault();

            if (listaChequeo != null)
            {
                int? SolicitudPagoId = _context.SolicitudPagoFase
                                        .Where(r => r.SolicitudPagoFaseId == pSolicitudPagoId)
                                            .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
                                                    .ThenInclude(r => r.SolicitudPago)
                                        .Select(s => s.SolicitudPagoRegistrarSolicitudPago.SolicitudPago.SolicitudPagoId).FirstOrDefault();


                int? SolicitudPagoListaChequeoId = _context.SolicitudPagoListaChequeo.Where(s => s.SolicitudPagoId == SolicitudPagoId
                                                                                    && s.Eliminado != true)
                                                                                .Select(r => r.SolicitudPagoId).FirstOrDefault();

                if (SolicitudPagoListaChequeoId == 0 && pSolicitudPagoId > 0)
                {
                    SolicitudPagoListaChequeo solicitudPagoListaChequeoNew = new SolicitudPagoListaChequeo
                    {
                        UsuarioCreacion = pUsuarioCreacion,
                        FechaCreacion = DateTime.Now,
                        Eliminado = false,
                        RegistroCompleto = false,
                        SolicitudPagoId = pSolicitudPagoId,
                        ListaChequeoId = listaChequeo.ListaChequeoId
                    };
                    _context.SolicitudPagoListaChequeo.Add(solicitudPagoListaChequeoNew);
                    _context.SaveChanges();

                    foreach (var ListaChequeoListaChequeoItem in listaChequeo.ListaChequeoListaChequeoItem)
                    {
                        SolicitudPagoListaChequeoRespuesta solicitudPagoListaChequeoRespuestaNew = new SolicitudPagoListaChequeoRespuesta
                        {
                            UsuarioCreacion = pUsuarioCreacion,
                            FechaCreacion = DateTime.Now,
                            Eliminado = false,

                            SolicitudPagoListaChequeoId = solicitudPagoListaChequeoNew.SolicitudPagoListaChequeoId,
                            ListaChequeoItemId = ListaChequeoListaChequeoItem.ListaChequeoItemId,
                        };

                        solicitudPagoListaChequeoRespuestaNew.RegistroCompleto = ValidarRegistroCompletoSolicitudPagoListaChequeoRespuesta(solicitudPagoListaChequeoRespuestaNew);
                        _context.SolicitudPagoListaChequeoRespuesta.Add(solicitudPagoListaChequeoRespuestaNew);
                    }
                }

            }

        }

        #endregion 

        #region Tipo Otros Costos Servicios

        public async Task<Respuesta> CreateEditOtrosCostosServicios(SolicitudPago pSolicitudPago)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Solicitud_De_Pago, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pSolicitudPago.SolicitudPagoOtrosCostosServicios.Count() > 0)
                {
                    CreateEditNewOtrosCostosServicios(pSolicitudPago.SolicitudPagoOtrosCostosServicios, pSolicitudPago.UsuarioCreacion);
                }
                if (pSolicitudPago.SolicitudPagoSoporteSolicitud.Count() > 0)
                    CreateEditNewSolicitudPagoSoporteSolicitud(pSolicitudPago.SolicitudPagoSoporteSolicitud, pSolicitudPago.UsuarioCreacion);


                if (pSolicitudPago.SolicitudPagoId > 0)
                {
                    SolicitudPago solicitudPagoOld = _context.SolicitudPago.Find(pSolicitudPago.SolicitudPagoId);
                    pSolicitudPago.ValorFacturado = pSolicitudPago?.SolicitudPagoOtrosCostosServicios?.FirstOrDefault()?.ValorFacturado;
                    solicitudPagoOld.FechaModificacion = DateTime.Now;
                    solicitudPagoOld.UsuarioModificacion = pSolicitudPago.UsuarioCreacion;
                }
                else
                {
                    pSolicitudPago.ValorFacturado = pSolicitudPago?.SolicitudPagoOtrosCostosServicios?.FirstOrDefault()?.ValorFacturado;
                    pSolicitudPago.EstadoCodigo = ((int)EnumEstadoSolicitudPago.En_proceso_de_registro).ToString();
                    pSolicitudPago.NumeroSolicitud = await _commonService.EnumeradorSolicitudPagoExpensasAndOtros();
                    pSolicitudPago.FechaCreacion = DateTime.Now;
                    pSolicitudPago.Eliminado = false;

                    _context.SolicitudPago.Add(pSolicitudPago);

                    _context.SaveChanges();
                    //Crear Lista Chequeo  
                    CreateEditListaChequeoByEstadoMenuCodigo(ConstanTipoListaChequeo.Otros_costos_y_servicios, pSolicitudPago.SolicitudPagoId, pSolicitudPago.UsuarioCreacion);

                }
                if (pSolicitudPago.SolicitudPagoSoporteSolicitud.Count() > 0)
                    CreateEditNewSolicitudPagoSoporteSolicitud(pSolicitudPago.SolicitudPagoSoporteSolicitud, pSolicitudPago.UsuarioCreacion);


                _context.SaveChanges();
                return
                     new Respuesta
                     {
                         Data = pSolicitudPago,
                         IsSuccessful = true,
                         IsException = false,
                         IsValidation = false,
                         Code = GeneralCodes.OperacionExitosa,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.OperacionExitosa, idAccion, pSolicitudPago.UsuarioCreacion, pSolicitudPago.FechaModificacion.HasValue ? "EDITAR SOLICITUD DE PAGO" : "CREAR SOLICITUD DE PAGO")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.Error, idAccion, pSolicitudPago.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        private void CreateEditNewOtrosCostosServicios(ICollection<SolicitudPagoOtrosCostosServicios> pSolicitudPagoOtrosCostosServiciosList, string usuarioCreacion)
        {
            foreach (var SolicitudPagoOtrosCostosServicios in pSolicitudPagoOtrosCostosServiciosList)
            {
                if (SolicitudPagoOtrosCostosServicios.SolicitudPagoOtrosCostosServiciosId > 0)
                {
                    SolicitudPagoOtrosCostosServicios solicitudPagoOtrosCostosServiciosOld = _context.SolicitudPagoOtrosCostosServicios.Find(SolicitudPagoOtrosCostosServicios.SolicitudPagoOtrosCostosServiciosId);
                    solicitudPagoOtrosCostosServiciosOld.RegistroCompleto = ValidateCompleteRecordoOtrosCostosServicios(SolicitudPagoOtrosCostosServicios);
                    solicitudPagoOtrosCostosServiciosOld.UsuarioModificacion = usuarioCreacion;
                    solicitudPagoOtrosCostosServiciosOld.FechaModificacion = DateTime.Now;

                    solicitudPagoOtrosCostosServiciosOld.NumeroRadicadoSac = SolicitudPagoOtrosCostosServicios.NumeroRadicadoSac;
                    solicitudPagoOtrosCostosServiciosOld.NumeroFactura = SolicitudPagoOtrosCostosServicios.NumeroFactura;
                    solicitudPagoOtrosCostosServiciosOld.ValorFacturado = SolicitudPagoOtrosCostosServicios.ValorFacturado;
                    solicitudPagoOtrosCostosServiciosOld.TipoPagoCodigo = SolicitudPagoOtrosCostosServicios.TipoPagoCodigo;
                }
                else
                {
                    SolicitudPagoOtrosCostosServicios.RegistroCompleto = ValidateCompleteRecordoOtrosCostosServicios(SolicitudPagoOtrosCostosServicios);
                    SolicitudPagoOtrosCostosServicios.UsuarioModificacion = usuarioCreacion;
                    SolicitudPagoOtrosCostosServicios.FechaModificacion = DateTime.Now;
                    SolicitudPagoOtrosCostosServicios.Eliminado = false;

                    _context.SolicitudPagoOtrosCostosServicios.Add(SolicitudPagoOtrosCostosServicios);
                }
            }
        }

        private bool ValidateCompleteRecordoOtrosCostosServicios(SolicitudPagoOtrosCostosServicios solicitudPagoOtrosCostosServiciosOld)
        {
            if (
                 string.IsNullOrEmpty(solicitudPagoOtrosCostosServiciosOld.NumeroRadicadoSac)
                 || string.IsNullOrEmpty(solicitudPagoOtrosCostosServiciosOld.NumeroFactura)
                 || solicitudPagoOtrosCostosServiciosOld.ValorFacturado == 0
                 || string.IsNullOrEmpty(solicitudPagoOtrosCostosServiciosOld.TipoPagoCodigo)
                 ) return false;

            return true;
        }

        private bool ValidateCompleteRecordoSolicitudPagoOtrosCostosServicios(SolicitudPago pSolicitudPago)
        {
            if (
                   pSolicitudPago.SolicitudPagoOtrosCostosServicios.Count() == 0
                || pSolicitudPago.SolicitudPagoSoporteSolicitud.Count() == 0
                || pSolicitudPago.SolicitudPagoListaChequeo.Count() == 0
                )
                return false;

            foreach (var SolicitudPagoOtrosCostosServicios in pSolicitudPago.SolicitudPagoOtrosCostosServicios)
            {
                if (SolicitudPagoOtrosCostosServicios.RegistroCompleto != true)
                    return false;
            }

            foreach (var SolicitudPagoSoporteSolicitud in pSolicitudPago.SolicitudPagoSoporteSolicitud)
            {
                if (SolicitudPagoSoporteSolicitud.RegistroCompleto != true)
                    return false;
            }

            foreach (var SolicitudPagoListaChequeo in pSolicitudPago.SolicitudPagoListaChequeo)
            {
                if (SolicitudPagoListaChequeo.RegistroCompleto != true)
                    return false;
            }
            return true;
        }

        #endregion

        #endregion

        #region Get

        public async Task<dynamic> GetMontoMaximoMontoPendiente(
            int SolicitudPagoId,
            string strFormaPago,
            bool EsPreConstruccion,
            int pContratacionProyectoId,
            string pCriterioCodigo,
            string pConceptoCodigo,
            string pUsoCodigo,
            string pTipoPago,
            bool pConNovedad
            )
        {
            try
            {
                SolicitudPago solicitudPago = await _context.SolicitudPago
                         .Where(r => r.SolicitudPagoId == SolicitudPagoId)
                         .Include(r => r.Contrato)
                         .FirstOrDefaultAsync();

                VValorFacturadoContratoXproyectoXuso VValorFacturadoContratoXproyectoXuso = _context.VValorFacturadoContratoXproyectoXuso
                                                                                                     .Where(v => v.ContratoId == solicitudPago.ContratoId
                                                                                                               && v.ContratacionProyectoId == pContratacionProyectoId
                                                                                                               && v.EsPreconstruccion == EsPreConstruccion
                                                                                                               // && v.ConceptoCodigo == pConceptoCodigo
                                                                                                               && v.UsoCodigo == pUsoCodigo)
                                                                                                     .FirstOrDefault();

                //  decimal Porcentaje = decimal.Parse(_context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Criterios_Pago && r.Codigo == pCriterioCodigo).FirstOrDefault().Descripcion ?? "100");
                decimal Porcentaje = 100;

                if (VValorFacturadoContratoXproyectoXuso == null)
                {
                    ContratacionProyecto contratacionProyecto = _context.ContratacionProyecto.Find(pContratacionProyectoId);
                    decimal ValorTotalPorFase = (decimal)_context.VDrpXcontratacionXproyectoXfaseXconceptoXusos
                           .Where(r => r.ContratacionId == solicitudPago.Contrato.ContratacionId
                                    && r.ProyectoId == contratacionProyecto.ProyectoId
                                    && r.EsPreConstruccion == EsPreConstruccion
                                    && r.TipoUsoCodigo == pUsoCodigo
                                    && r.ConceptoPagoCodigo == pConceptoCodigo
                                    )
                           .Sum(v => v.ValorUso);

                    return new
                    {
                        MontoMaximo = (ValorTotalPorFase * Porcentaje) / 100,
                        ValorPendientePorPagar = 0
                    };
                }


                VValorFacturadoContratoXproyectoXuso.SaldoPresupuestal =
                                                                         VValorFacturadoContratoXproyectoXuso.ValorSolicitudDdp
                                                                       - (decimal)_context.VValorFacturadoContratoXproyectoXuso.Where(
                                                                                                                                         v => v.ContratoId == solicitudPago.ContratoId
                                                                                                                                           && v.ContratacionProyectoId == pContratacionProyectoId
                                                                                                                                           && v.EsPreconstruccion == EsPreConstruccion
                                                                                                                                           && v.Uso == VValorFacturadoContratoXproyectoXuso.Uso
                                                                                                                                           )
                                                                                                                               .Sum(c => c.ValorFacturado);

                decimal MontoMaximo = 0;

                MontoMaximo = ((decimal)VValorFacturadoContratoXproyectoXuso.SaldoPresupuestal * Porcentaje) / 100;

                return new
                {
                    MontoMaximo = MontoMaximo < 0 ? VValorFacturadoContratoXproyectoXuso.ValorSolicitudDdp : MontoMaximo,
                    VValorFacturadoContratoXproyectoXuso.SaldoPresupuestal
                };

            }
            catch (Exception e)
            {
                return new
                {
                    MontoMaximo = 0,
                    ValorPendientePorPagar = 0
                };
            }
        }

        public async Task<dynamic> GetMontoMaximo(int SolicitudPagoId, bool EsPreConstruccion)
        {
            decimal ValorPendientePorPagar = 0;
            try
            {
                SolicitudPago solicitudPago = await _context.SolicitudPago.FindAsync(SolicitudPagoId);

                decimal ValorTotalPorFase = (decimal)_context.VValorUsoXcontratoId
                    .Where(r => r.ContratoId == solicitudPago.ContratoId && r.EsPreConstruccion == EsPreConstruccion)
                    .Sum(v => v.ValorUso);

                ValorPendientePorPagar = ValorTotalPorFase;

                // - (decimal)_context.VValorFacturadoXfasesSolicitudPago
                //.Where(v => v.SolicitudPagoId == SolicitudPagoId && v.EsPreConstruccion == EsPreConstruccion)
                //.Sum(c => c.ValorFacturado);

                //  ValorPendientePorPagar = ValorTotalPorFase - ValorPendientePorPagar;

                return new
                {
                    ValorPendientePorPagar
                };
            }
            catch (Exception e)
            {
                return new
                {
                    ValorPendientePorPagar
                };
            }
        }

        public async Task<dynamic> GetMontoMaximoProyecto(int pContrato, int pContratacionProyectoId, bool EsPreConstruccion)
        {
            decimal ValorMaximoProyecto =
               (decimal)await _context.VValorUsosFasesAportanteProyecto
                .Where(r => r.ContratacionProyectoId == pContratacionProyectoId
                      && r.EsPreConstruccion == EsPreConstruccion)
                .SumAsync(s => s.ValorUso);

            decimal ValorFacturadoProyecto =
               (decimal)await _context.VValorFacturadoProyecto
                .Where(r => r.ContratacionProyectoId == pContratacionProyectoId
                        && r.EsPreconstruccion == EsPreConstruccion)
                .SumAsync(s => s.ValorFacturado);

            return new
            {
                ValorMaximoProyecto,
                ValorPendienteProyecto = ValorMaximoProyecto - ValorFacturadoProyecto
            };
        }

        public async Task<SolicitudPago> GetSolicitudPago(int pSolicitudPagoId)
        {
            SolicitudPago solicitudPago = await _context.SolicitudPago.FindAsync(pSolicitudPagoId);

            return GetSolicitudPagoComplete(solicitudPago);

        }

        public async Task<dynamic> GetListProyectosByLlaveMen(string pLlaveMen)
        {
            List<VProyectosXcontrato> ListProyectos =
                                             await _context.VProyectosXcontrato
                                                .Where(r => r.LlaveMen.Contains(pLlaveMen) &&
                                                (
                                                 (r.EstadoActaFase2.Trim() == ConstanCodigoEstadoActaInicioObra.Con_acta_suscrita_y_cargada
                                                 && r.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Obra) ||
                                                (r.EstadoActaFase2.Trim() == ConstanCodigoEstadoActaInicioInterventoria.Con_acta_suscrita_y_cargada
                                                 && r.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Interventoria)
                                                ))
                                            .ToListAsync();

            List<VSaldoPresupuestalXproyecto> LVSaldoPresupuestalXproyecto = _context.VSaldoPresupuestalXproyecto.ToList();

            List<dynamic> dynamics = new List<dynamic>();

            List<DisponibilidadPresupuestalProyecto> DisponibilidadPresupuestalProyecto = _context.DisponibilidadPresupuestalProyecto.ToList();

            foreach (var item in ListProyectos)
            {
                VSaldoPresupuestalXproyecto VSaldoPresupuestalXproyecto = LVSaldoPresupuestalXproyecto.Where(v => v.ProyectoId == item.ProyectoId && v.SaldoPresupuestal > 0).FirstOrDefault();

                if (VSaldoPresupuestalXproyecto != null && DisponibilidadPresupuestalProyecto.Any(d => d.ProyectoId == item.ProyectoId))
                {
                    dynamics.Add(new
                    {
                        item.ContratacionProyectoId,
                        item.LlaveMen,
                        VSaldoPresupuestalXproyecto.ValorDdp,
                        VSaldoPresupuestalXproyecto.ValorFacturado,
                        VSaldoPresupuestalXproyecto.SaldoPresupuestal
                    });
                }
            }
            return dynamics;
        }

        public async Task<dynamic> GetListSolicitudPago()
        {
            var result = await _context.SolicitudPago.Where(s => s.Eliminado != true)
                .Include(r => r.Contrato)
                                         .Select(s => new
                                         {
                                             s.EsFactura,
                                             s.TipoSolicitudCodigo,
                                             s.FechaCreacion,
                                             s.NumeroSolicitud,
                                             s.Contrato.ModalidadCodigo,
                                             s.Contrato.NumeroContrato,
                                             s.EstadoCodigo,
                                             s.ContratoId,
                                             s.SolicitudPagoId,
                                             s.TieneNoCumpleListaChequeo,
                                             RegistroCompleto = s.RegistroCompleto ?? false
                                         }).OrderByDescending(r => r.SolicitudPagoId)
                                           .ToListAsync();


            List<Dominio> ListParametricas =
                _context.Dominio
                               .Where(d => d.TipoDominioId == (int)EnumeratorTipoDominio.Modalidad_Contrato
                                   || d.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Solicitud_Pago)
                               .ToList();
            List<dynamic> grind = new List<dynamic>();
            result.ForEach(r =>
            {
                grind.Add(new
                {
                    r.RegistroCompleto,
                    r.TipoSolicitudCodigo,
                    r.ContratoId,
                    r.SolicitudPagoId,
                    r.FechaCreacion,
                    r.NumeroSolicitud,
                    r.TieneNoCumpleListaChequeo,
                    NumeroContrato = r.NumeroContrato ?? "No Aplica",
                    r.EstadoCodigo,
                    Estado = !string.IsNullOrEmpty(r.EstadoCodigo) ? ListParametricas.Where(l => l.Codigo == r.EstadoCodigo && l.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Solicitud_Pago).FirstOrDefault().Nombre : " - ",
                    Modalidad = !string.IsNullOrEmpty(r.ModalidadCodigo) ? ListParametricas.Where(l => l.Codigo == r.ModalidadCodigo && l.TipoDominioId == (int)EnumeratorTipoDominio.Modalidad_Contrato).FirstOrDefault().Nombre : "No aplica",
                    r.EsFactura
                });
            });
            return grind;
        }

        public async Task<Contrato> GetContratoByContratoId(int pContratoId, int pSolicitudPago, bool esSolicitudPago)
        {
            Contrato contrato = await _context.Contrato
                    .Where(c => c.ContratoId == pContratoId)
                    .Include(c => c.ContratoConstruccion)
                    .Include(c => c.ContratoPoliza)
                    .Include(c => c.Contratacion).ThenInclude(c => c.Contratista)
                    .Include(c => c.Contratacion).ThenInclude(cp => cp.DisponibilidadPresupuestal)
                    .Include(r => r.SolicitudPago).ThenInclude(r => r.SolicitudPagoCargarFormaPago)
                    .Include(r => r.SolicitudPago).ThenInclude(r => r.SolicitudPagoRegistrarSolicitudPago).ThenInclude(r => r.SolicitudPagoFase).ThenInclude(r => r.SolicitudPagoFaseCriterio)
                    .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(t => t.ContratacionProyectoAportante).ThenInclude(t => t.CofinanciacionAportante).ThenInclude(t => t.FuenteFinanciacion).ThenInclude(t => t.CuentaBancaria)
                    .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(t => t.ContratacionProyectoAportante).ThenInclude(t => t.CofinanciacionAportante).ThenInclude(t => t.NombreAportante)
                    .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(t => t.ContratacionProyectoAportante).ThenInclude(t => t.CofinanciacionAportante).ThenInclude(t => t.Municipio)
                    .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(t => t.ContratacionProyectoAportante).ThenInclude(t => t.CofinanciacionAportante).ThenInclude(t => t.Departamento)
                    .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(t => t.ContratacionProyectoAportante).ThenInclude(t => t.ComponenteAportante)
                    .Include(c => c.Contratacion).ThenInclude(c => c.ContratacionProyecto).ThenInclude(t => t.Proyecto)
                    .Include(c => c.Contratacion).ThenInclude(c => c.DisponibilidadPresupuestal)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();




            if (contrato.SolicitudPago.Count() > 0)
                contrato.SolicitudPago = contrato.SolicitudPago
                    .Where(s => s.Eliminado != true).ToList();

            if (pSolicitudPago > 0)
            {
                SolicitudPago solicitudPago = _context.SolicitudPago.Find(pSolicitudPago);
                contrato.SolicitudPagoOnly = GetSolicitudPagoComplete(solicitudPago);
            }
            List<VContratoPagosRealizados> vContratoPagosRealizados = new List<VContratoPagosRealizados>();

            try
            {
                if (_context.VContratoPagosRealizados.Any(v => v.ContratoId == pContratoId)
                    )
                { 
                    vContratoPagosRealizados = _context.VContratoPagosRealizados.Where(v => v.ContratoId == pContratoId).ToList();
                }
            }
            catch (Exception e)
            {
            }
            contrato.VAmortizacionXproyecto = _context.VAmortizacionXproyecto.Where(v => v.ContratoId == pContratoId).ToList();
            contrato.VContratoPagosRealizados = vContratoPagosRealizados;
            contrato.TablaDRP = GetDrpContratoGeneral(contrato.ContratacionId, esSolicitudPago);
            contrato.TablaDRPODG = GetDrpContratoGeneral(contrato.ContratacionId, esSolicitudPago);
            contrato.ListProyectos = GetListProyectos(contrato.ContratacionId);
            contrato.VConceptosXcontratoXfaseXproyecto = _context.VConceptosXcontratoXfaseXproyecto.Where(r => r.ContratoId == contrato.ContratoId).ToList();
            return contrato;

        }

        public dynamic GetListProyectos(int pContratacionId)
        {
            return _context.ContratacionProyecto
                                                .Where(cp => cp.ContratacionId == pContratacionId)
                                                .Include(p => p.Proyecto).ThenInclude(i => i.InstitucionEducativa)
                                                .Select(s => new
                                                {
                                                    s.Proyecto.InstitucionEducativa.Nombre,
                                                    s.Proyecto.LlaveMen
                                                }).ToList();
        }

        //private List<VContratoPagosRealizados> GetValidarContratoPagosRealizados(int pContratoId, int pSolicitudPago)
        //{
        //    List<VContratoPagosRealizados> vContratoPagosRealizados = _context.VContratoPagosRealizados
        //                .Where(v => v.ContratoId == pContratoId).ToList();


        //    decimal ValorAnterior = 0;
        //    decimal Drp = 0;
        //    int Count = 1;
        //    decimal dcDecimalValorFacturado = 0;
        //    foreach (var item in vContratoPagosRealizados)
        //    {
        //        dcDecimalValorFacturado += item.ValorFacturado;
        //        //item.SaldoPorPagar = item.ValorSolicitud - dcDecimalValorFacturado;
        //        ValorAnterior = item.SaldoPorPagar ?? 0;
        //        Drp = item.ValorSolicitud ?? 1;
        //        //item.SaldoPorPagar = item.ValorSolicitud - dcDecimalValorFacturado;
        //        item.PorcentajeFacturado = item.PorcentajeFacturado;
        //        item.PorcentajePorPagar = item.PorcentajePorPagar;
        //    }
        //    Count++;

        //    return vContratoPagosRealizados;
        //}

        public dynamic GetDrpContrato(int pContratacionId)
        {
            bool OrdenGiroAprobada = _context.OrdenGiro.Where(r => r.SolicitudPago.FirstOrDefault().Contrato.ContratacionId == pContratacionId).FirstOrDefault()?.RegistroCompletoAprobar ?? false;

            List<VDrpXproyectoXusos> List = _context.VDrpXproyectoXusos.Where(r => r.ContratacionId == pContratacionId).OrderBy(r => r.FechaCreacion).ToList();

            var ListDrp = List.GroupBy(drp => drp.NumeroDrp)
                              .Select(d =>
                                          d.OrderBy(p => p.NumeroDrp)
                                           .FirstOrDefault())
                              .ToList();

            List<dynamic> ListTablaDrp = new List<dynamic>();

            List<VPagosSolicitudXcontratacionXproyectoXuso> ListPagos =
                    _context.VPagosSolicitudXcontratacionXproyectoXuso.Where(v => v.ContratacionId == pContratacionId)
                                                                      .ToList();

            //  List<VDescuentosXordenGiroXproyectoXaportanteXconceptoXuso> DescuentosOrdenGiro = _context.VDescuentosXordenGiroXproyectoXaportanteXconceptoXuso.Where(r => r.ContratacionId == pContratacionId).ToList();

            List<VDescuentosXordenGiroXproyectoXaportanteXconceptoXuso> DescuentosOrdenGiro = new List<VDescuentosXordenGiroXproyectoXaportanteXconceptoXuso>();

            foreach (var Drp in ListDrp)
            {
                var ListProyectosId = List.Where(r => r.NumeroDrp == Drp.NumeroDrp)
                                          .GroupBy(id => id.ProyectoId)
                                          .Select(d =>
                                                      d.OrderBy(p => p.ProyectoId)
                                                       .FirstOrDefault())
                                          .ToList();

                List<dynamic> ListDyProyectos = new List<dynamic>();
                foreach (var ProyectoId in ListProyectosId)
                {
                    Proyecto proyecto = _context.Proyecto
                                                        .Where(r => r.ProyectoId == ProyectoId.ProyectoId)
                                                        .Include(ie => ie.InstitucionEducativa)
                                                        .FirstOrDefault();

                    var ListTipoUso = List.Where(r => r.NumeroDrp == Drp.NumeroDrp
                                                   && r.ProyectoId == ProyectoId.ProyectoId)
                                          .GroupBy(id => id.TipoUsoCodigo)
                                          .Select(d => d.OrderBy(p => p.TipoUsoCodigo).FirstOrDefault())
                                          .ToList();

                    List<dynamic> ListDyUsos = new List<dynamic>();
                    foreach (var TipoUso in ListTipoUso)
                    {
                        VDrpXproyectoXusos Uso =
                                                List
                                                .Where(r => r.NumeroDrp == Drp.NumeroDrp
                                                         && r.ProyectoId == ProyectoId.ProyectoId
                                                         && r.TipoUsoCodigo == TipoUso.TipoUsoCodigo)
                                                .FirstOrDefault();

                        decimal? ValorUso = List
                                                .Where(r => r.NumeroDrp == Drp.NumeroDrp
                                                         && r.ProyectoId == ProyectoId.ProyectoId
                                                         && r.TipoUsoCodigo == TipoUso.TipoUsoCodigo)
                                                .Sum(v => v.ValorUso) ?? 0;

                        decimal? Saldo = ListPagos
                                                .Where(r => r.ProyectoId == ProyectoId.ProyectoId
                                                         && r.TipoUsoCodigo == TipoUso.TipoUsoCodigo
                                                          && r.Pagado == false
                                                         )
                                                .Sum(r => r.SaldoUso) ?? 0;


                        decimal Descuentos = 0;

                        decimal ValorUsoResta = ValorUso ?? 0 - Descuentos;

                        if (true)
                        {
                            foreach (var item in ListPagos.Where(r => r.ProyectoId == ProyectoId.ProyectoId
                                                            && r.TipoUsoCodigo == TipoUso.TipoUsoCodigo).ToList())
                            {
                                if (true)
                                {
                                    if (ValorUsoResta > item.SaldoUso)
                                    {
                                        ValorUsoResta -= (decimal)item.SaldoUso;
                                        item.SaldoUso = ValorUsoResta;
                                        item.Pagado = true;
                                    }
                                    else
                                    {
                                        item.SaldoUso -= ValorUsoResta;
                                        break;
                                    }
                                }
                                else
                                {
                                    item.SaldoUso = Saldo;
                                }
                            }
                            ListDyUsos.Add(new
                            {
                                Uso.Nombre,
                                ValorUso = String.Format("{0:n0}", ValorUso),
                                Saldo = String.Format("{0:n0}", Saldo > 0 ? (ValorUso - Saldo) < 0 ? 0 : ValorUso - Saldo : ValorUso)
                            });
                        }
                        else
                        {
                            ListDyUsos.Add(new
                            {
                                Uso.Nombre,
                                ValorUso = String.Format("{0:n0}", ValorUso),
                                Saldo = String.Format("{0:n0}", ValorUso)
                            });
                        }
                    }

                    ListDyProyectos.Add(new
                    {
                        proyecto.InstitucionEducativa.Nombre,
                        ListDyUsos
                    });
                }

                ListTablaDrp.Add(new
                {
                    Enum,
                    Drp.NumeroDrp,
                    ListDyProyectos
                });
                Enum++;
            }
            return ListTablaDrp;
        }

        public dynamic GetDrpContratoODG(int pContratacionId)
        {
            bool OrdenGiroAprobada = _context.OrdenGiro.Where(r => r.SolicitudPago.FirstOrDefault().Contrato.ContratacionId == pContratacionId).FirstOrDefault()?.RegistroCompletoAprobar ?? false;

            List<VDrpXproyectoXusos> List = _context.VDrpXproyectoXusos.Where(r => r.ContratacionId == pContratacionId).OrderBy(r => r.FechaCreacion).ToList();

            var ListDrp = List.GroupBy(drp => drp.NumeroDrp)
                              .Select(d =>
                                          d.OrderBy(p => p.NumeroDrp)
                                           .FirstOrDefault())
                              .ToList();

            List<dynamic> ListTablaDrp = new List<dynamic>();

            List<VPagosSolicitudXsinAmortizacion> ListPagos =
                    _context.VPagosSolicitudXsinAmortizacion.Where(v => v.ContratacionId == pContratacionId
                                                                     && v.EstaAprobadaOdg)
                                                                      .ToList();

            List<VDescuentosXordenGiroXproyectoXaportanteXconceptoXuso> DescuentosOrdenGiro = _context.VDescuentosXordenGiroXproyectoXaportanteXconceptoXuso.Where(r => r.ContratacionId == pContratacionId).ToList();

            foreach (var Drp in ListDrp)
            {
                var ListProyectosId = List.Where(r => r.NumeroDrp == Drp.NumeroDrp)
                                          .GroupBy(id => id.ProyectoId)
                                          .Select(d =>
                                                      d.OrderBy(p => p.ProyectoId)
                                                       .FirstOrDefault())
                                          .ToList();

                List<dynamic> ListDyProyectos = new List<dynamic>();
                foreach (var ProyectoId in ListProyectosId)
                {
                    Proyecto proyecto = _context.Proyecto
                                                        .Where(r => r.ProyectoId == ProyectoId.ProyectoId)
                                                        .Include(ie => ie.InstitucionEducativa)
                                                        .FirstOrDefault();

                    var ListTipoUso = List.Where(r => r.NumeroDrp == Drp.NumeroDrp
                                                   && r.ProyectoId == ProyectoId.ProyectoId)
                                          .GroupBy(id => id.TipoUsoCodigo)
                                          .Select(d => d.OrderBy(p => p.TipoUsoCodigo).FirstOrDefault())
                                          .ToList();

                    List<dynamic> ListDyUsos = new List<dynamic>();
                    foreach (var TipoUso in ListTipoUso)
                    {
                        VDrpXproyectoXusos Uso =
                                                List
                                                .Where(r => r.NumeroDrp == Drp.NumeroDrp
                                                         && r.ProyectoId == ProyectoId.ProyectoId
                                                         && r.TipoUsoCodigo == TipoUso.TipoUsoCodigo)
                                                .FirstOrDefault();

                        decimal? ValorUso = List
                                                .Where(r => r.NumeroDrp == Drp.NumeroDrp
                                                         && r.ProyectoId == ProyectoId.ProyectoId
                                                         && r.TipoUsoCodigo == TipoUso.TipoUsoCodigo)
                                                .Sum(v => v.ValorUso) ?? 0;

                        decimal? Saldo = ListPagos
                                                .Where(r => r.ProyectoId == ProyectoId.ProyectoId
                                                         && r.TipoUsoCodigo == TipoUso.TipoUsoCodigo
                                                         && r.Pagado == false
                                                         )
                                                .Sum(r => r.SaldoUso) ?? 0;


                        decimal? Descuentos = DescuentosOrdenGiro
                                                            .Where(r => r.ProyectoId == ProyectoId.ProyectoId
                                                             && r.UsoCodigo == TipoUso.TipoUsoCodigo
                                                             )
                                                .Sum(r => r.ValorDescuento);

                        decimal ValorUsoResta = (decimal)(ValorUso ?? 0 - Descuentos);
                        foreach (var item in ListPagos.Where(r => r.ProyectoId == ProyectoId.ProyectoId
                                                               && r.TipoUsoCodigo == TipoUso.TipoUsoCodigo).ToList())
                        {
                            if (item.EstaAprobadaOdg)
                            {
                                if (ValorUsoResta > item.SaldoUso)
                                {
                                    ValorUsoResta -= (decimal)item.SaldoUso;
                                    item.SaldoUso = ValorUsoResta;
                                    item.Pagado = true;
                                }
                                else
                                {
                                    item.SaldoUso -= ValorUsoResta;
                                    break;
                                }
                            }
                            else
                            {
                                item.SaldoUso = ValorUsoResta;
                            }
                        }

                        if (true)
                        {
                            ListDyUsos.Add(new
                            {
                                Uso.Nombre,
                                ValorUso = String.Format("{0:n0}", ValorUso),
                                Saldo = String.Format("{0:n0}", Saldo > 0 ? (ValorUso - Saldo) < 0 ? 0 : ValorUso - Saldo : ValorUso)
                            }); ;
                        }
                        else
                        {
                            ListDyUsos.Add(new
                            {
                                Uso.Nombre,
                                ValorUso = String.Format("{0:n0}", ValorUso),
                                Saldo = String.Format("{0:n0}", ValorUso)
                            });
                        }
                    }
                    ListDyProyectos.Add(new
                    {
                        proyecto.InstitucionEducativa.Nombre,
                        ListDyUsos
                    });
                }

                ListTablaDrp.Add(new
                {
                    Enum,
                    Drp.NumeroDrp,
                    ListDyProyectos
                });
                Enum++;
            }
            return ListTablaDrp;
        }


        public List<TablaDRP> GetDrpContrato(Contrato contrato)
        {
            String strTipoSolicitud = contrato.Contratacion.TipoSolicitudCodigo;
            List<TablaDRP> ListTablaDrp = new List<TablaDRP>();

            decimal ValorFacturado =
                                    _context.SolicitudPago
                                    .Where(r => r.ContratoId == contrato.ContratoId && r.TipoSolicitudCodigo == strTipoSolicitud)
                                    .Sum(r => r.ValorFacturado) ?? 0;


            List<VRpsPorContratacion> vRpsPorContratacion =
                                                           _context.VRpsPorContratacion
                                                           .Where(c => c.ContratacionId == contrato.ContratacionId)
                                                           .OrderBy(C => C.ContratacionId)
                                                           .ToList();

            int Enum = 1;
            foreach (var DPR in vRpsPorContratacion)
            {
                ValorFacturado = (DPR.ValorSolicitud - ValorFacturado) > 0 ? (DPR.ValorSolicitud - ValorFacturado) : DPR.ValorSolicitud;

                ListTablaDrp.Add(new TablaDRP
                {
                    Enum = Enum,
                    NumeroDRP = DPR.NumeroDrp,
                    Valor = '$' + String.Format("{0:n0}", DPR.ValorSolicitud),
                    Saldo = '$' + String.Format("{0:n0}", ValorFacturado)
                });
                Enum++;
            }


            return ListTablaDrp;
        }

        public async Task<dynamic> GetContratoByTipoSolicitudCodigoModalidadContratoCodigoOrNumeroContrato(string pTipoSolicitud, string pModalidadContrato, string pNumeroContrato)
        {
            try
            {
                List<VSaldoPresupuestalXcontrato> ListVSaldoPresupuestalXcontrato = await _context.VSaldoPresupuestalXcontrato.ToListAsync();

                List<dynamic> List = new List<dynamic>();
                List<Contrato> ListContratos = new List<Contrato>();
                if (!string.IsNullOrEmpty(pTipoSolicitud) && !string.IsNullOrEmpty(pModalidadContrato))
                {
                    if (pTipoSolicitud == ConstanCodigoTipoContrato.Obra)
                    {

                        ListContratos = await _context.Contrato
                                        .Include(c => c.Contratacion)
                                                 .Where(c => c.NumeroContrato.Trim().ToLower().Contains(pNumeroContrato.Trim().ToLower())
                                                          && c.ModalidadCodigo == pModalidadContrato
                                                          && c.Contratacion.TipoSolicitudCodigo == pTipoSolicitud
                                                          && (
                                                                c.EstadoActaFase2.Trim() == ConstanCodigoEstadoActaInicioObra.Con_acta_suscrita_y_cargada
                                                             || c.EstadoActa.Trim() == ConstanCodigoEstadoActaInicioObra.Con_acta_suscrita_y_cargada
                                                             )
                                                             ).ToListAsync();
                    }
                    else
                    {
                        ListContratos = await _context.Contrato
                                      .Include(c => c.Contratacion)
                                               .Where(c => c.NumeroContrato.Trim().ToLower().Contains(pNumeroContrato.Trim().ToLower())
                                                        && c.ModalidadCodigo == pModalidadContrato
                                                        && c.Contratacion.TipoSolicitudCodigo == pTipoSolicitud
                                                        && (c.EstadoActaFase2.Trim() == ConstanCodigoEstadoActaInicioInterventoria.Con_acta_suscrita_y_cargada
                                                           || c.EstadoActa.Trim() == ConstanCodigoEstadoActaInicioInterventoria.Con_acta_suscrita_y_cargada)
                                                           )
                                                      .ToListAsync();
                    }
                    foreach (var Contrato in ListContratos)
                    {
                        VSaldoPresupuestalXcontrato VSaldoPresupuestalXcontrato = ListVSaldoPresupuestalXcontrato.Where(r => r.ContratoId == Contrato.ContratoId && r.SaldoPresupuestalObraInterventoria > 0).FirstOrDefault();

                        if (VSaldoPresupuestalXcontrato != null)
                        {
                            List.Add(new
                            {
                                ValorSolicitudDdp = VSaldoPresupuestalXcontrato.ValorDdpobraInterventoria,
                                SaldoPresupuestal = VSaldoPresupuestalXcontrato.SaldoPresupuestalObraInterventoria,
                                Contrato.ContratoId,
                                Contrato.NumeroContrato
                            });
                        }
                    }
                }
                else
                {
                    ListContratos = await _context.Contrato
                                    .Include(c => c.Contratacion)
                                             .Where(c => c.NumeroContrato.Trim().ToLower().Contains(pNumeroContrato.Trim().ToLower())
                                                         && (
                                                             (c.EstadoActaFase2.Trim() == ConstanCodigoEstadoActaInicioObra.Con_acta_suscrita_y_cargada
                                                             && c.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Obra) ||
                                                            (c.EstadoActaFase2.Trim() == ConstanCodigoEstadoActaInicioInterventoria.Con_acta_suscrita_y_cargada
                                                             && c.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Interventoria)
                                                            )
                                                      ).ToListAsync();

                    foreach (var Contrato in ListContratos)
                    {
                        VSaldoPresupuestalXcontrato VSaldoPresupuestalXcontrato = ListVSaldoPresupuestalXcontrato.Where(r => r.ContratoId == Contrato.ContratoId && r.SaldoPresupuestalOtrosCostos > 0).FirstOrDefault();

                        if (VSaldoPresupuestalXcontrato != null)
                        {
                            List.Add(new
                            {
                                ValorSolicitudDdp = VSaldoPresupuestalXcontrato.ValorDdpotrosCostos,
                                SaldoPresupuestal = VSaldoPresupuestalXcontrato.SaldoPresupuestalOtrosCostos,
                                Contrato.ContratoId,
                                Contrato.NumeroContrato
                            });
                        }
                    }
                }

                return List;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private SolicitudPago GetRemoveObjectsDelete(SolicitudPago solicitudPago)
        {
            foreach (var SolicitudPagoRegistrarSolicitudPago in solicitudPago.SolicitudPagoRegistrarSolicitudPago)
            {
                foreach (var SolicitudPagoFase in SolicitudPagoRegistrarSolicitudPago.SolicitudPagoFase)
                {
                    if (SolicitudPagoFase.SolicitudPagoFaseCriterio.Count() > 0)
                    {
                        SolicitudPagoFase.SolicitudPagoFaseCriterio = SolicitudPagoFase.SolicitudPagoFaseCriterio.Where(r => r.Eliminado != true).ToList();

                        foreach (var SolicitudPagoFaseCriterio in SolicitudPagoFase.SolicitudPagoFaseCriterio)
                        {
                            if (SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioConceptoPago.Count() > 0)

                                SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioConceptoPago = SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioConceptoPago.Where(r => r.Eliminado != true).ToList();
                        }
                    }
                    if (SolicitudPagoFase.SolicitudPagoFaseFacturaDescuento.Count() > 0)
                        SolicitudPagoFase.SolicitudPagoFaseFacturaDescuento = SolicitudPagoFase.SolicitudPagoFaseFacturaDescuento.Where(r => r.Eliminado != true).ToList();

                }
            }
            if (solicitudPago.SolicitudPagoFactura.Count() > 0)
                solicitudPago.SolicitudPagoFactura = solicitudPago.SolicitudPagoFactura.Where(spf => spf.Eliminado != true).ToList();

            if (solicitudPago.SolicitudPagoListaChequeo.Count() > 0)
                solicitudPago.SolicitudPagoListaChequeo = solicitudPago.SolicitudPagoListaChequeo.Where(r => r.Eliminado != true).ToList();

            List<SolicitudPagoListaChequeoRespuesta> ListSolicitudPagoListaChequeoRespuesta =
                _context.SolicitudPagoListaChequeoRespuesta.Include(r => r.ListaChequeoItem).ToList();

            foreach (var SolicitudPagoListaChequeo in solicitudPago.SolicitudPagoListaChequeo)
            {
                SolicitudPagoListaChequeo.SolicitudPagoListaChequeoRespuesta =
                    ListSolicitudPagoListaChequeoRespuesta
                    .Where(r => r.SolicitudPagoListaChequeoId == SolicitudPagoListaChequeo.SolicitudPagoListaChequeoId)
                    .ToList();

                foreach (var SolicitudPagoListaChequeoRespuesta in SolicitudPagoListaChequeo.SolicitudPagoListaChequeoRespuesta)
                {
                    SolicitudPagoListaChequeoRespuesta.SolicitudPagoListaChequeo = null;
                }
            }
            return solicitudPago;
        }

        public SolicitudPago GetSolicitudPagoComplete(SolicitudPago solicitudPago)
        {
            if (solicitudPago == null)
                return new SolicitudPago();

            switch (solicitudPago.TipoSolicitudCodigo)
            {
                case ConstanCodigoTipoSolicitudContratoSolicitudPago.Contratos_Interventoria:
                case ConstanCodigoTipoSolicitudContratoSolicitudPago.Contratos_Obra:

                    solicitudPago = _context.SolicitudPago.Where(r => r.SolicitudPagoId == solicitudPago.SolicitudPagoId)
                        .Include(r => r.SolicitudPagoFactura)
                        .Include(r => r.SolicitudPagoCargarFormaPago)
                        .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
                           .ThenInclude(r => r.SolicitudPagoFase)
                               .ThenInclude(r => r.SolicitudPagoFaseCriterio)
                    .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
                           .ThenInclude(r => r.SolicitudPagoFase)
                               .ThenInclude(r => r.SolicitudPagoFaseCriterio)
                                   .ThenInclude(r => r.SolicitudPagoFaseCriterioConceptoPago)
                       .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
                          .ThenInclude(r => r.SolicitudPagoFase)
                              .ThenInclude(r => r.SolicitudPagoFaseAmortizacion)
                       .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
                          .ThenInclude(r => r.SolicitudPagoFase)
                                  .ThenInclude(r => r.SolicitudPagoFaseFacturaDescuento)
                       .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
                       .Include(r => r.SolicitudPagoSoporteSolicitud)
                       .Include(r => r.SolicitudPagoListaChequeo)
                         .ThenInclude(r => r.ListaChequeo)
                         .AsNoTracking()
                         .FirstOrDefault();
                    GetRemoveObjectsDelete(solicitudPago);
                    return solicitudPago;

                case ConstanCodigoTipoSolicitudContratoSolicitudPago.Expensas:
                    solicitudPago = _context.SolicitudPago.Where(r => r.SolicitudPagoId == solicitudPago.SolicitudPagoId)
                         .Include(e => e.ContratacionProyecto).ThenInclude(p => p.Proyecto)
                        .Include(e => e.SolicitudPagoExpensas)
                        .Include(e => e.SolicitudPagoSoporteSolicitud)
                        .Include(r => r.SolicitudPagoListaChequeo)
                         .ThenInclude(r => r.ListaChequeo)
                                 .AsNoTracking()
                        .FirstOrDefault();
                    GetRemoveObjectsDelete(solicitudPago);
                    solicitudPago.SaldoPresupuestal = _context.VSaldoPresupuestalXproyecto.Where(v => v.ContratacionProyectoId == solicitudPago.ContratacionProyectoId).FirstOrDefault().SaldoPresupuestal;

                    return solicitudPago;

                case ConstanCodigoTipoSolicitudContratoSolicitudPago.Otros_Costos_Servicios:
                    solicitudPago = _context.SolicitudPago.Where(r => r.SolicitudPagoId == solicitudPago.SolicitudPagoId)
                        .Include(e => e.SolicitudPagoOtrosCostosServicios)
                     .Include(e => e.SolicitudPagoSoporteSolicitud)
                            .Include(r => r.SolicitudPagoListaChequeo)
                         .ThenInclude(r => r.ListaChequeo)
                            .AsNoTracking()
                        .FirstOrDefault();

                    solicitudPago.SaldoPresupuestal = _context.VSaldoPresupuestalXcontrato.Where(v => v.ContratoId == solicitudPago.ContratoId).FirstOrDefault().SaldoPresupuestalOtrosCostos;

                    GetRemoveObjectsDelete(solicitudPago);
                    return solicitudPago;

                default: return solicitudPago;

            }
        }

        public dynamic GetDrpContratoGeneral(int pContratacionId, bool esSolicitudPago)
        {
            bool OrdenGiroAprobada = _context.OrdenGiro.Where(r => r.SolicitudPago.FirstOrDefault().Contrato.ContratacionId == pContratacionId)
                                                       .FirstOrDefault()?.RegistroCompletoAprobar ?? false;

            List<VDrpXproyectoXusos> List = _context.VDrpXproyectoXusos.Where(r => r.ContratacionId == pContratacionId).OrderBy(r => r.FechaCreacion).ToList();

            var ListDrp = List.GroupBy(drp => drp.NumeroDrp)
                              .Select(d =>
                                          d.OrderBy(p => p.NumeroDrp)
                                           .FirstOrDefault())
                              .ToList();


            List<dynamic> ListTablaDrp = new List<dynamic>();

            List<VPagosSolicitudXsinAmortizacion> ListPagos =
                           _context.VPagosSolicitudXsinAmortizacion.Where(v => v.ContratacionId == pContratacionId
                                                                            && v.EsFactura
                                                                          )
                                                                   .ToList();


            List<VPagosSolicitudXsinAmortizacion> ListPagosTodo =
               _context.VPagosSolicitudXsinAmortizacion.Where(v => v.ContratacionId == pContratacionId && v.EsFactura
                                                              )
                                                       .ToList();

            List<VPagosOdgXsinAmortizacion> ListPagosOdg =
                    _context.VPagosOdgXsinAmortizacion.Where(v => v.ContratacionId == pContratacionId
                                                            )
                                                            .ToList();

            if (!esSolicitudPago)
            {
                ListPagos = ListPagos.Where(r => r.EstaAprobadaOdg == true).ToList();
                ListPagosOdg = ListPagosOdg.Where(r => r.EstaAprobadaOdg == true).ToList();
            }

            List<VDescuentosXordenGiroXproyectoXaportanteXconceptoXuso> DescuentosOrdenGiro = _context.VDescuentosXordenGiroXproyectoXaportanteXconceptoXuso.Where(r => r.ContratacionId == pContratacionId).ToList();

            foreach (var Drp in ListDrp)
            {
                var ListProyectosId = List.Where(r => r.NumeroDrp == Drp.NumeroDrp)
                                          .GroupBy(id => id.ProyectoId)
                                          .Select(d =>
                                                      d.OrderBy(p => p.ProyectoId)
                                                       .FirstOrDefault())
                                          .ToList();

                List<dynamic> ListDyProyectos = new List<dynamic>();
                foreach (var ProyectoId in ListProyectosId)
                {
                    Proyecto proyecto = _context.Proyecto
                                                        .Where(r => r.ProyectoId == ProyectoId.ProyectoId)
                                                        .Include(ie => ie.InstitucionEducativa)
                                                        .FirstOrDefault();

                    var ListTipoUso = List.Where(r => r.NumeroDrp == Drp.NumeroDrp
                                                   && r.ProyectoId == ProyectoId.ProyectoId)
                                          .GroupBy(id => id.TipoUsoCodigo)
                                          .Select(d => d.OrderBy(p => p.TipoUsoCodigo).FirstOrDefault())
                                          .ToList();

                    List<dynamic> ListDyUsos = new List<dynamic>();
                    foreach (var TipoUso in ListTipoUso)
                    {
                        VDrpXproyectoXusos Uso =
                                                List
                                                .Where(r => r.NumeroDrp == Drp.NumeroDrp
                                                         && r.ProyectoId == ProyectoId.ProyectoId
                                                         && r.TipoUsoCodigo == TipoUso.TipoUsoCodigo)
                                                .FirstOrDefault();

                        decimal? ValorUso = List
                                                .Where(r => r.NumeroDrp == Drp.NumeroDrp
                                                         && r.ProyectoId == ProyectoId.ProyectoId
                                                         && r.TipoUsoCodigo == TipoUso.TipoUsoCodigo)
                                                .Sum(v => v.ValorUso) ?? 0;
                        decimal Saldo = 0;
                        decimal Descuentos = 0;

                        Saldo += ListPagos
                                             .Where(r => r.ProyectoId == ProyectoId.ProyectoId
                                                      && r.TipoUsoCodigo == TipoUso.TipoUsoCodigo
                                                       && r.Pagado == false
                                                      )
                                             .Sum(r => r.SaldoUso) ?? 0;

                        if (esSolicitudPago)
                        {

                        }
                        else
                        {
                            Descuentos = DescuentosOrdenGiro
                                                          .Where(r => r.ProyectoId == ProyectoId.ProyectoId
                                                           && r.UsoCodigo == TipoUso.TipoUsoCodigo
                                                           )
                                              .Sum(r => r.ValorDescuento);

                            Saldo = OrdenGiroAprobada ? !esSolicitudPago ?
                                                ListPagosOdg
                                                .Where(r => r.ProyectoId == ProyectoId.ProyectoId
                                                         && r.TipoUsoCodigo == TipoUso.TipoUsoCodigo
                                                          && r.Pagado == false
                                                         )
                                                .Sum(r => r.SaldoUso) ?? 0 :
                                                ListPagosOdg
                                                .Where(r => r.ProyectoId == ProyectoId.ProyectoId
                                                         && r.TipoUsoCodigo == TipoUso.TipoUsoCodigo
                                                         && r.Pagado == false
                                                         )
                                                .Sum(r => r.SaldoUso) ?? 0 : 0;
                        }
                        decimal ValorUsoResta = (decimal)ValorUso;
                        if (!esSolicitudPago)
                            ValorUsoResta = (decimal)(ValorUso + Descuentos);
                         
                        if (esSolicitudPago)
                        {
                            foreach (var item in ListPagos.Where(r => r.ProyectoId == ProyectoId.ProyectoId
                                       && r.TipoUsoCodigo == TipoUso.TipoUsoCodigo).ToList())
                            {
                                if (!esSolicitudPago)
                                {
                                    if (item.EstaAprobadaOdg)
                                    {
                                        if (ValorUsoResta > item.SaldoUso)
                                        {
                                            ValorUsoResta -= (decimal)item.SaldoUso;
                                            item.SaldoUso = ValorUsoResta;
                                            item.Pagado = true;
                                        }
                                        else
                                        {
                                            item.SaldoUso -= ValorUsoResta;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        item.SaldoUso = ValorUsoResta;
                                    }
                                }
                                else
                                {
                                    if (ValorUsoResta > item.SaldoUso)
                                    {
                                        ValorUsoResta -= (decimal)item.SaldoUso;
                                        item.SaldoUso = ValorUsoResta;
                                        item.Pagado = true;
                                    }
                                    else
                                    {
                                        item.SaldoUso -= ValorUsoResta;
                                        break;
                                    }
                                }

                            }
                        }
                        else
                        {
                            foreach (var item in ListPagosOdg.Where(r => r.ProyectoId == ProyectoId.ProyectoId && r.TipoUsoCodigo == TipoUso.TipoUsoCodigo).ToList())
                            {
                                if (!esSolicitudPago)
                                {
                                    if (item.EstaAprobadaOdg)
                                    {
                                        if (ValorUsoResta > item.SaldoUso)
                                        {
                                            ValorUsoResta -= (decimal)item.SaldoUso;
                                            item.SaldoUso = ValorUsoResta;
                                            item.Pagado = true;
                                        }
                                        else
                                        {
                                            item.SaldoUso -= ValorUsoResta;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        item.SaldoUso = ValorUsoResta;
                                    }
                                }
                                else
                                {
                                    if (ValorUsoResta > item.SaldoUso)
                                    {
                                        ValorUsoResta -= (decimal)item.SaldoUso;
                                        item.SaldoUso = ValorUsoResta;
                                        item.Pagado = true;
                                    }
                                    else
                                    {
                                        item.SaldoUso -= ValorUsoResta;
                                        break;
                                    }
                                }
                            }
                        }


                        if (true)
                        {
                            decimal OtroSaldo = (decimal)ValorUso;
                            if (!esSolicitudPago)  
                                 OtroSaldo = (decimal)(ValorUso + Descuentos);
                             
                            ListDyUsos.Add(new
                            {
                                Uso.Nombre,
                                ValorUso = String.Format("{0:n0}", ValorUso),
                                Saldo = String.Format("{0:n0}", Saldo > 0 ? (OtroSaldo - Saldo) < 0 ? 0 : OtroSaldo - Saldo : OtroSaldo)
                            });
                        }
                        else
                        {
                            ListDyUsos.Add(new
                            {
                                Uso.Nombre,
                                ValorUso = String.Format("{0:n0}", ValorUso),
                                Saldo = String.Format("{0:n0}", ValorUso)
                            });
                        }
                    }
                    ListDyProyectos.Add(new
                    {
                        proyecto.InstitucionEducativa.Nombre,
                        ListDyUsos
                    });
                }

                ListTablaDrp.Add(new
                {
                    Enum,
                    Drp.NumeroDrp,
                    ListDyProyectos
                });
                Enum++;
            }
            return ListTablaDrp;
        }

        #endregion

        #region Validate 
        private bool ValidateCompleteRecordSolicitudPago(SolicitudPago pSolicitudPago)
        {
            //Tipo Obra o Interventoria
            if (Convert.ToInt32(pSolicitudPago.TipoSolicitudCodigo) < (int)EnumeratorTipoSolicitudRequisitosPagos.Expensas)
            {
                if (
                       pSolicitudPago.SolicitudPagoSoporteSolicitud.Count() == 0
                    || pSolicitudPago.SolicitudPagoRegistrarSolicitudPago.Count() == 0
                    || pSolicitudPago.SolicitudPagoListaChequeo.Count() == 0)
                    return false;

                if (_context.SolicitudPago.Where(r => r.ContratoId == pSolicitudPago.ContratoId && r.Eliminado != true).Count() > 1)
                {
                }
                else
                {
                    if (pSolicitudPago.SolicitudPagoCargarFormaPago.Count() == 0)
                        return false;
                    foreach (var SolicitudPagoCargarFormaPago in pSolicitudPago.SolicitudPagoCargarFormaPago)
                    {
                        if (!ValidateCompleteRecordSolicitudPagoCargarFormaPago(SolicitudPagoCargarFormaPago))
                            return false;
                    }
                }

                foreach (var SolicitudPagoSoporteSolicitud in pSolicitudPago.SolicitudPagoSoporteSolicitud)
                {
                    if (!ValidateCompleteRecordSolicitudPagoSoporteSolicitud(SolicitudPagoSoporteSolicitud))
                        return false;
                }

            }

            //Tipo Expensas
            if (Convert.ToInt32(pSolicitudPago.TipoSolicitudCodigo) == (int)EnumeratorTipoSolicitudRequisitosPagos.Expensas)
            {
                if (pSolicitudPago.SolicitudPagoExpensas.Count() == 0)
                    return false;


                if (pSolicitudPago.SolicitudPagoSoporteSolicitud.Count == 0)
                    return false;

                foreach (var SolicitudPagoSoporteSolicitud in pSolicitudPago.SolicitudPagoSoporteSolicitud)
                {
                    if (string.IsNullOrEmpty(SolicitudPagoSoporteSolicitud.UrlSoporte))
                        return false;
                }
                foreach (var SolicitudPagoExpensas in pSolicitudPago.SolicitudPagoExpensas)
                {
                    if (
                           string.IsNullOrEmpty(SolicitudPagoExpensas.NumeroRadicadoSac)
                        || string.IsNullOrEmpty(SolicitudPagoExpensas.NumeroFactura)
                        || string.IsNullOrEmpty(SolicitudPagoExpensas.TipoPagoCodigo)
                        || string.IsNullOrEmpty(SolicitudPagoExpensas.ConceptoPagoCriterioCodigo)
                        || SolicitudPagoExpensas.ValorFacturado == 0
                        || SolicitudPagoExpensas.ValorFacturadoConcepto == 0
                       ) return false;
                }
            }
            //Tipo Otros Costos
            if (Convert.ToInt32(pSolicitudPago.TipoSolicitudCodigo) == (int)EnumeratorTipoSolicitudRequisitosPagos.Otros_costos_servicios)
            {
                if (pSolicitudPago.SolicitudPagoOtrosCostosServicios.Count() == 0)
                    return false;

                foreach (var SolicitudPagoOtrosCostosServicios in pSolicitudPago.SolicitudPagoOtrosCostosServicios)
                {
                    if (
                           string.IsNullOrEmpty(SolicitudPagoOtrosCostosServicios.NumeroRadicadoSac)
                        || string.IsNullOrEmpty(SolicitudPagoOtrosCostosServicios.NumeroFactura)
                        || string.IsNullOrEmpty(SolicitudPagoOtrosCostosServicios.TipoPagoCodigo)
                        || string.IsNullOrEmpty(SolicitudPagoOtrosCostosServicios.TipoPagoCodigo)
                        || SolicitudPagoOtrosCostosServicios.ValorFacturado == 0
                       ) return false;
                }
            }
            //Lista de chequeo

            foreach (var SolicitudPagoListaChequeo in pSolicitudPago.SolicitudPagoListaChequeo)
            {
                foreach (var SolicitudPagoListaChequeoRespuesta in SolicitudPagoListaChequeo.SolicitudPagoListaChequeoRespuesta)
                {
                    if (SolicitudPagoListaChequeoRespuesta.RegistroCompleto != true)
                        return false;
                }
            }

            //Solicitud Pago
            foreach (var SolicitudPagoRegistrarSolicitudPago in pSolicitudPago.SolicitudPagoRegistrarSolicitudPago)
            {
                if (!ValidateCompleteRecordSolicitudPagoRegistrarSolicitudPago(SolicitudPagoRegistrarSolicitudPago))
                    return false;
            }

            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoSoporteSolicitud(SolicitudPagoSoporteSolicitud pSolicitudPagoSoporteSolicitud)
        {
            if (string.IsNullOrEmpty(pSolicitudPagoSoporteSolicitud.UrlSoporte))
                return false;
            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoFaseFactura(SolicitudPagoFactura solicitudPagoFaseFactura)
        {


            if (
                   string.IsNullOrEmpty(solicitudPagoFaseFactura.Fecha.ToString())
                || string.IsNullOrEmpty(solicitudPagoFaseFactura.ValorFacturado.ToString())
                || string.IsNullOrEmpty(solicitudPagoFaseFactura.Numero.ToString())
                )
                return false;

            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoFaseFacturaDescuento(SolicitudPagoFaseFacturaDescuento solicitudPagoFaseDescuento)
        {
            if (string.IsNullOrEmpty(solicitudPagoFaseDescuento.TipoDescuentoCodigo)
               || string.IsNullOrEmpty(solicitudPagoFaseDescuento.ValorDescuento.ToString()))
                return false;
            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoFaseCriterio(SolicitudPagoFaseCriterio solicitudPagoFaseCriterio)
        {
            if (string.IsNullOrEmpty(solicitudPagoFaseCriterio.TipoCriterioCodigo)
                 || solicitudPagoFaseCriterio.ValorFacturado == 0) return false;

            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoRegistrarSolicitudPago(SolicitudPagoRegistrarSolicitudPago pSolicitudPagoRegistrarSolicitudPago)
        {
            if (
                    !pSolicitudPagoRegistrarSolicitudPago.TieneFaseConstruccion.HasValue
                || !pSolicitudPagoRegistrarSolicitudPago.TieneFasePreconstruccion.HasValue
                || !pSolicitudPagoRegistrarSolicitudPago.FechaSolicitud.HasValue
                || string.IsNullOrEmpty(pSolicitudPagoRegistrarSolicitudPago.NumeroRadicadoSac)
                ) return false;

            if (pSolicitudPagoRegistrarSolicitudPago.SolicitudPagoFase.Count() == 0)
                return false;


            foreach (var SolicitudPagoFase in pSolicitudPagoRegistrarSolicitudPago.SolicitudPagoFase)
            {
                if (!ValidateCompleteRecordSolicitudPagoFase(SolicitudPagoFase))
                    return false;
            }
            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoFase(SolicitudPagoFase pSolicitudPagoFase)
        {
            //La Fase Construccion Es la unica que tiene amortizacion
            if (pSolicitudPagoFase.EsPreconstruccion != true)
            {
                bool? TieneAmortizacion = _context.SolicitudPagoFase
                    .Where(r => r.SolicitudPagoFaseId == pSolicitudPagoFase.SolicitudPagoFaseId)
                    .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
                    .ThenInclude(r => r.SolicitudPago)
                    .ThenInclude(r => r.Contrato)
                    .ThenInclude(r => r.ContratoConstruccion)
                    .Select(r => r.SolicitudPagoRegistrarSolicitudPago.SolicitudPago.Contrato.ContratoConstruccion
                    .FirstOrDefault().ManejoAnticipoRequiere)
                    .FirstOrDefault();

                if (TieneAmortizacion == true)
                {
                    //if (pSolicitudPagoFase.SolicitudPagoFaseAmortizacion.Count() == 0)
                    //    return false;

                    foreach (var SolicitudPagoAmortizacion in pSolicitudPagoFase.SolicitudPagoFaseAmortizacion)
                    {
                        if (!ValidateCompleteRecordSolicitudPagoAmortizacion(SolicitudPagoAmortizacion))
                            return false;
                    }
                }
            }

            if (!pSolicitudPagoFase.TieneDescuento.HasValue)
                return false;

            if ((bool)pSolicitudPagoFase.TieneDescuento)
            {
                foreach (var SolicitudPagoFaseFacturaDescuento in pSolicitudPagoFase.SolicitudPagoFaseFacturaDescuento)
                {
                    if (!ValidateCompleteRecordSolicitudPagoFaseFacturaDescuento(SolicitudPagoFaseFacturaDescuento))
                        return false;
                }
            }

            if (!ValidateCompleteRecordSolicitudPagoFaseCriterio2(pSolicitudPagoFase.SolicitudPagoFaseCriterio))
                return false;

            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoAmortizacion(SolicitudPagoFaseAmortizacion solicitudPagoAmortizacion)
        {
            if (
                     string.IsNullOrEmpty(solicitudPagoAmortizacion.ValorAmortizacion.ToString()
                    )
                )
                return false;
            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoCargarFormaPago(SolicitudPagoCargarFormaPago pSolicitudPagoCargarFormaPago)
        {
            if (pSolicitudPagoCargarFormaPago.TieneFase1 == true)
                if (string.IsNullOrEmpty(pSolicitudPagoCargarFormaPago.FasePreConstruccionFormaPagoCodigo))
                    return false;

            if (string.IsNullOrEmpty(pSolicitudPagoCargarFormaPago.FaseConstruccionFormaPagoCodigo))
                return false;

            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoFaseCriterio2(ICollection<SolicitudPagoFaseCriterio> ListsolicitudPagoFaseCriterio)
        {
            if (ListsolicitudPagoFaseCriterio.Count == 0)
                return false;

            foreach (var solicitudPagoFaseCriterio in ListsolicitudPagoFaseCriterio)
            {
                if (
                       string.IsNullOrEmpty(solicitudPagoFaseCriterio.TipoCriterioCodigo)
                     || string.IsNullOrEmpty(solicitudPagoFaseCriterio.TipoPagoCodigo)
                     || solicitudPagoFaseCriterio.ValorFacturado == 0
                      ) return false;
            }
            return true;
        }

        public Task<Respuesta> DeleteSolicitudLlaveCriterioProyecto(int pContratacionProyectoId, string pUsuarioModificacion)
        {
            throw new NotImplementedException();
        }

        public Task<Respuesta> DeleteSolicitudPagoFaseCriterioProyecto(int SolicitudPagoFaseCriterioProyectoId, string pUsuarioModificacion)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}