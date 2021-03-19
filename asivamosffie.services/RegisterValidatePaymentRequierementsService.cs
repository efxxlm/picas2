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
        #endregion

        #region Tablas Relacionadas Para Pagos
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
            List<string> strCriterios = _context.TipoPagoConceptoPagoCriterio.Where(r => r.TipoPagoCodigo == TipoPagoCodigo).Select(r => r.ConceptoPagoCriterioCodigo).ToList();
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
        //4# Traer  Uso Por Concepto de pago
        public async Task<dynamic> GetUsoByConceptoPagoCriterioCodigo(string pConceptoPagoCodigo, int pContratoId)
        {
            try
            {

                List<dynamic> ListDynamics = new List<dynamic>();
                List<string> strCriterios = _context.ConceptoPagoUso.Where(r => r.ConceptoPagoCodigo == pConceptoPagoCodigo).Select(r => r.Uso).ToList();
                List<Dominio> ListUsos = await _commonService.GetListDominioByIdTipoDominio((int)EnumeratorTipoDominio.Usos);

                return _context.VValorUsoXcontratoId.Where(r => r.ContratoId == pContratoId && strCriterios.Contains(r.TipoUsoCodigo)).ToList();

            }
            catch (Exception ex)
            {

                return new { };
            }
        }
        #endregion

        #region Get
        public async Task<SolicitudPago> GetSolicitudPago(int pSolicitudPagoId)
        {
            SolicitudPago solicitudPago = await _context.SolicitudPago.FindAsync(pSolicitudPagoId);

            return GetSolicitudPago(solicitudPago);

        }

        public async Task<dynamic> GetListProyectosByLlaveMen(string pLlaveMen)
        {
            return await
                _context.VProyectosXcontrato
                                            .Where(r => r.LlaveMen.Contains(pLlaveMen)
                                            && r.EstadoActaFase2 == ConstanCodigoEstadoActaInicioObra.Con_acta_suscrita_y_cargada)
                                                                                                                                .Select(s => new
                                                                                                                                {
                                                                                                                                    s.LlaveMen,
                                                                                                                                    s.ContratacionProyectoId
                                                                                                                                }).ToListAsync();
        }

        public async Task<dynamic> GetListSolicitudPago()
        {
            var result = await _context.SolicitudPago.Where(s => s.Eliminado != true)
                .Include(r => r.Contrato)
                                         .Select(s => new
                                         {
                                             s.TipoSolicitudCodigo,
                                             s.FechaCreacion,
                                             s.NumeroSolicitud,
                                             s.Contrato.ModalidadCodigo,
                                             s.Contrato.NumeroContrato,
                                             s.EstadoCodigo,
                                             s.ContratoId,
                                             s.SolicitudPagoId,
                                             RegistroCompleto = s.RegistroCompleto ?? false
                                         }).OrderByDescending(r => r.SolicitudPagoId)
                                                                                    .ToListAsync();

            List<dynamic> grind = new List<dynamic>();
            List<Dominio> ListParametricas = _context.Dominio.Where(d => d.TipoDominioId == (int)EnumeratorTipoDominio.Modalidad_Contrato || d.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Solicitud_Pago).ToList();

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
                    NumeroContrato = r.NumeroContrato ?? "No Aplica",
                    r.EstadoCodigo,
                    Estado = !string.IsNullOrEmpty(r.EstadoCodigo) ? ListParametricas.Where(l => l.Codigo == r.EstadoCodigo && l.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Solicitud_Pago).FirstOrDefault().Nombre : " - ",
                    Modalidad = !string.IsNullOrEmpty(r.ModalidadCodigo) ? ListParametricas.Where(l => l.Codigo == r.ModalidadCodigo && l.TipoDominioId == (int)EnumeratorTipoDominio.Modalidad_Contrato).FirstOrDefault().Nombre : "No aplica"
                });
            });
            return grind;
        }

        public async Task<dynamic> GetContratoByTipoSolicitudCodigoModalidadContratoCodigoOrNumeroContrato(string pTipoSolicitud, string pModalidadContrato, string pNumeroContrato)
        {
            try
            {
                if (!string.IsNullOrEmpty(pTipoSolicitud) && !string.IsNullOrEmpty(pModalidadContrato))
                {
                    List<Contrato> ListContratos = await _context.Contrato
                                    .Include(c => c.Contratacion)
                                             .Where(c => c.NumeroContrato.Trim().ToLower().Contains(pNumeroContrato.Trim().ToLower())
                                                   && c.Contratacion.TipoSolicitudCodigo == pTipoSolicitud
                                                   && c.EstadoActaFase2 == ConstanCodigoEstadoActaInicioObra.Con_acta_suscrita_y_cargada
                                                   ).ToListAsync();
                    return ListContratos
                        .Select(r => new
                        {
                            r.ContratoId,
                            r.NumeroContrato
                        }).ToList();
                }
                else
                {
                    List<Contrato> ListContratos = await _context.Contrato
                                    .Include(c => c.Contratacion)
                                             .Where(c => c.NumeroContrato.Trim().ToLower().Contains(pNumeroContrato.Trim().ToLower())
                                                      && c.EstadoActaFase2 == ConstanCodigoEstadoActaInicioObra.Con_acta_suscrita_y_cargada
                                                   ).ToListAsync();
                    return ListContratos
                                        .Select(r => new
                                        {
                                            r.ContratoId,
                                            r.NumeroContrato
                                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Contrato> GetContratoByContratoId(int pContratoId, int pSolicitudPago)
        {
            try
            {
                Contrato contrato = await _context.Contrato
                        .Where(c => c.ContratoId == pContratoId)
                        .Include(c => c.ContratoConstruccion)
                        .Include(c => c.ContratoPoliza)
                        .Include(c => c.Contratacion)
                           .ThenInclude(c => c.Contratista)
                        .Include(c => c.Contratacion)
                           .ThenInclude(c => c.ContratacionProyecto)
                        .Include(c => c.Contratacion)
                           .ThenInclude(cp => cp.DisponibilidadPresupuestal)
                        .Include(r => r.SolicitudPago)
                           .ThenInclude(r => r.SolicitudPagoCargarFormaPago)
                        .Include(c => c.Contratacion)
                           .ThenInclude(c => c.ContratacionProyecto)
                               .ThenInclude(t => t.ContratacionProyectoAportante)
                                   .ThenInclude(t => t.CofinanciacionAportante)
                                      .ThenInclude(t => t.FuenteFinanciacion)
                                         .ThenInclude(t => t.CuentaBancaria)
                        .Include(c => c.Contratacion)
                           .ThenInclude(c => c.ContratacionProyecto)
                               .ThenInclude(t => t.ContratacionProyectoAportante)
                                   .ThenInclude(t => t.ComponenteAportante)
                          .Include(c => c.Contratacion)
                           .ThenInclude(c => c.ContratacionProyecto)

                        .FirstOrDefaultAsync();

                if (pSolicitudPago > 0)
                {
                    SolicitudPago solicitudPago = _context.SolicitudPago.Find(pSolicitudPago);
                    contrato.SolicitudPagoOnly = GetSolicitudPago(solicitudPago);
                }
                contrato.ValorFacturadoContrato = _context.VValorFacturadoContrato.Where(v => v.ContratoId == pContratoId).ToList();

                List<VContratoPagosRealizados> vContratoPagosRealizados = new List<VContratoPagosRealizados>();
                vContratoPagosRealizados = _context.VContratoPagosRealizados
                        .Where(v => v.ContratoId == pContratoId).ToList();

                contrato.VContratoPagosRealizados = vContratoPagosRealizados
                    .GroupBy(r => r.EsPreconstruccion)
                    .Select(r => new
                    {
                        EsPreconstruccion = r.Key,
                        FaseContrato = r.Select(r => r.FaseContrato).FirstOrDefault(),
                        ValorFacturado = r.Sum(r => r.ValorFacturado),
                        PagosRealizados = r.Sum(r => r.PagosRealizados) / r.Count(),
                        PorcentajeFacturado = Math.Truncate((decimal)r.Sum(r => r.PorcentajeFacturado) / r.Count()) + "%",
                        SaldoPorPagar = r.Sum(r => r.SaldoPorPagar),
                        PorcentajePorPagar = Math.Truncate((decimal)r.Sum(r => r.PorcentajePorPagar) / r.Count()) + "%"
                    });

                return contrato;
            }
            catch (Exception ex)
            {
                return new Contrato();
            }
        }

        private SolicitudPago GetRemoveObjectsDelete(SolicitudPago solicitudPago)
        {
            foreach (var SolicitudPagoRegistrarSolicitudPago in solicitudPago.SolicitudPagoRegistrarSolicitudPago)
            {
                foreach (var SolicitudPagoFase in SolicitudPagoRegistrarSolicitudPago.SolicitudPagoFase)
                {
                    if (SolicitudPagoFase.SolicitudPagoFaseCriterio.Count() > 0)
                        SolicitudPagoFase.SolicitudPagoFaseCriterio = SolicitudPagoFase.SolicitudPagoFaseCriterio.Where(r => r.Eliminado != true).ToList();

                    foreach (var SolicitudPagoFaseCriterio in SolicitudPagoFase.SolicitudPagoFaseCriterio)
                    {
                        if (SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioProyecto.Count() > 0)
                            SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioProyecto = SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioProyecto.Where(r => r.Eliminado != true).ToList();
                    }

                    foreach (var SolicitudPagoFaseFactura in SolicitudPagoFase.SolicitudPagoFaseFactura)
                    {
                        if (SolicitudPagoFaseFactura.SolicitudPagoFaseFacturaDescuento.Count() > 0)
                            SolicitudPagoFaseFactura.SolicitudPagoFaseFacturaDescuento = SolicitudPagoFaseFactura.SolicitudPagoFaseFacturaDescuento.Where(r => r.Eliminado != true).ToList();

                    }
                }
            }

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

        public SolicitudPago GetSolicitudPago(SolicitudPago solicitudPago)
        {
            switch (solicitudPago.TipoSolicitudCodigo)
            {
                case ConstanCodigoTipoSolicitudContratoSolicitudPago.Contratos_Interventoria:
                case ConstanCodigoTipoSolicitudContratoSolicitudPago.Contratos_Obra:

                    solicitudPago = _context.SolicitudPago.Where(r => r.SolicitudPagoId == solicitudPago.SolicitudPagoId)
                        .Include(r => r.SolicitudPagoCargarFormaPago)
                        .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
                           .ThenInclude(r => r.SolicitudPagoFase)
                               .ThenInclude(r => r.SolicitudPagoFaseCriterio)
                                   .ThenInclude(r => r.SolicitudPagoFaseCriterioProyecto)
                    .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
                           .ThenInclude(r => r.SolicitudPagoFase)
                               .ThenInclude(r => r.SolicitudPagoFaseCriterio)
                                   .ThenInclude(r => r.SolicitudPagoFaseCriterioConceptoPago)
                       .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
                          .ThenInclude(r => r.SolicitudPagoFase)
                              .ThenInclude(r => r.SolicitudPagoFaseAmortizacion)
                       .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
                          .ThenInclude(r => r.SolicitudPagoFase)
                              .ThenInclude(r => r.SolicitudPagoFaseFactura)
                                  .ThenInclude(r => r.SolicitudPagoFaseFacturaDescuento)
                       .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
                       .Include(r => r.SolicitudPagoSoporteSolicitud)
                       .Include(r => r.SolicitudPagoListaChequeo)
                         .ThenInclude(r => r.ListaChequeo)
                               .ThenInclude(r => r.ListaChequeoListaChequeoItem)
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
                               .ThenInclude(r => r.ListaChequeoListaChequeoItem)
                        .FirstOrDefault();
                    GetRemoveObjectsDelete(solicitudPago);
                    return solicitudPago;

                case ConstanCodigoTipoSolicitudContratoSolicitudPago.Otros_Costos_Servicios:
                    solicitudPago = _context.SolicitudPago.Where(r => r.SolicitudPagoId == solicitudPago.SolicitudPagoId)
                     .Include(e => e.SolicitudPagoOtrosCostosServicios)
                     .Include(e => e.SolicitudPagoSoporteSolicitud)
                     .Include(r => r.SolicitudPagoListaChequeo)
                           .ThenInclude(r => r.ListaChequeo)
                               .ThenInclude(r => r.ListaChequeoListaChequeoItem)
                        .FirstOrDefault();
                    GetRemoveObjectsDelete(solicitudPago);
                    return solicitudPago;

                default: return solicitudPago;

            }
        }

        #endregion

        #region Validate 

        public async Task<dynamic> GetMontoMaximoMontoPendiente(int SolicitudPagoId, string strFormaPago, bool EsPreConstruccion)
        {
            SolicitudPago solicitudPago = await _context.SolicitudPago.FindAsync(SolicitudPagoId);

            ulong ValorTotalPorFase = (ulong)_context.VValorUsoXcontratoId.Where(r => r.ContratoId == solicitudPago.ContratoId && r.EsPreConstruccion == EsPreConstruccion).Sum(v => v.ValorUso);

            ulong ValorPendientePorPagar = (ulong)_context.VValorFacturadoContrato
                .Where(v => v.ContratoId == solicitudPago.ContratoId && v.EsPreconstruccion == EsPreConstruccion)
                .Sum(c => c.SaldoPresupuestal);

            ulong ValorFacturado = ValorTotalPorFase - ValorPendientePorPagar;

            string strNombreFormaPago = (_context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Formas_Pago && r.Codigo == strFormaPago).FirstOrDefault().Nombre).Replace("%", ""); ;

            List<string> FormasPago = strNombreFormaPago.Split("/").ToList();
            ulong MontoMaximo = 0;

            foreach (var PorcentajePago in FormasPago)
            {
                MontoMaximo = ((ValorTotalPorFase * Convert.ToUInt32(PorcentajePago)) / 100) - ValorFacturado;
                if (MontoMaximo < ValorPendientePorPagar)
                    break;
            }

            return new
            {
                MontoMaximo,
                ValorPendientePorPagar
            };
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
        #endregion

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
            SolicitudPago solicitudPago = await GetSolicitudPago(SolicitudPagoId);
            bool CompleteRecord = ValidateCompleteRecordSolicitudPago(solicitudPago);
            await _context.Set<SolicitudPago>()
                                              .Where(s => s.SolicitudPagoId == SolicitudPagoId)
                                                                                              .UpdateAsync(r => new SolicitudPago()
                                                                                              {
                                                                                                  RegistroCompleto = CompleteRecord
                                                                                              });
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

        public async Task<Respuesta> DeleteSolicitudLlaveCriterioProyecto(int pContratacionProyectoId, string pUsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Llave_Criterio_Proyecto, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                List<SolicitudPagoFaseCriterioProyecto> ListSolicitudPagoFaseCriterioProyectoDelete = _context.SolicitudPagoFaseCriterioProyecto.Where(s => s.ContratacionProyectoId == pContratacionProyectoId).ToList();

                foreach (var SolicitudPagoFaseCriterioProyecto in ListSolicitudPagoFaseCriterioProyectoDelete)
                {
                    SolicitudPagoFaseCriterioProyecto.FechaModificacion = DateTime.Now;
                    SolicitudPagoFaseCriterioProyecto.UsuarioModificacion = pUsuarioModificacion;
                    SolicitudPagoFaseCriterioProyecto.Eliminado = true;
                }

                return
                     new Respuesta
                     {
                         IsSuccessful = true,
                         IsException = false,
                         IsValidation = false,
                         Code = GeneralCodes.OperacionExitosa,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_requisitos_de_pago, GeneralCodes.OperacionExitosa, idAccion, pUsuarioModificacion, "ELIMINAR SOLICITUD PAGO FASE CRITERIO PROYECTO")
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

        public async Task<Respuesta> DeleteSolicitudPagoFaseCriterioProyecto(int SolicitudPagoFaseCriterioProyectoId, string pUsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Criterio_Proyecto, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SolicitudPagoFaseCriterioProyecto SolicitudPagoFaseCriterioProyectoDelete = _context.SolicitudPagoFaseCriterioProyecto.Find(SolicitudPagoFaseCriterioProyectoId);
                SolicitudPagoFaseCriterioProyectoDelete.FechaModificacion = DateTime.Now;
                SolicitudPagoFaseCriterioProyectoDelete.UsuarioModificacion = pUsuarioModificacion;
                SolicitudPagoFaseCriterioProyectoDelete.Eliminado = true;

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

        private async void CreateEditNewPaymentNew(SolicitudPago pSolicitudPago)
        {
            //Valida si el contrato de la solicitud es interventoria o Obra
            string strInterventoriaCodigo = _context.Contrato
                                             .Include(ctr => ctr.Contratacion)
                                             .Where(s => s.ContratoId == pSolicitudPago.ContratoId)
                                            .Select(crt =>
                                                    crt.Contratacion.TipoSolicitudCodigo
                                                    ).FirstOrDefault();

            if (pSolicitudPago.SolicitudPagoSoporteSolicitud.Count() > 0)
                CreateEditNewSolicitudPagoSoporteSolicitud(pSolicitudPago.SolicitudPagoSoporteSolicitud, pSolicitudPago.UsuarioCreacion);

            if (pSolicitudPago.SolicitudPagoId > 0)
            {
                pSolicitudPago.UsuarioModificacion = pSolicitudPago.UsuarioCreacion;
                pSolicitudPago.FechaModificacion = DateTime.Now;
                pSolicitudPago.TieneObservacion = false;
            }
            else
            {
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
                    pSolicitudPago.SolicitudPagoCargarFormaPago.FirstOrDefault().UsuarioCreacion = pSolicitudPago.UsuarioCreacion;
                    CreateEditNewPaymentWayToPay(pSolicitudPago.SolicitudPagoCargarFormaPago.FirstOrDefault());
                }

                if (pSolicitudPago.SolicitudPagoRegistrarSolicitudPago.Count() > 0)
                {
                    pSolicitudPago.SolicitudPagoRegistrarSolicitudPago.FirstOrDefault().UsuarioCreacion = pSolicitudPago.UsuarioCreacion;

                    CreateEditRegistrarSolicitudPago(pSolicitudPago.SolicitudPagoRegistrarSolicitudPago.FirstOrDefault());

                    CreateEditSolicitudPagoFase(pSolicitudPago.SolicitudPagoRegistrarSolicitudPago.FirstOrDefault().SolicitudPagoFase, pSolicitudPago.UsuarioCreacion);
                }

                CreateEditListaChequeoRespuesta(pSolicitudPago.SolicitudPagoListaChequeo, pSolicitudPago.UsuarioCreacion);
                return
                     new Respuesta
                     {
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

        private async void CreateEditListaChequeoRespuesta(ICollection<SolicitudPagoListaChequeo> pListSolicitudPagoListaChequeo, string usuarioCreacion)
        {
            foreach (var SolicitudPagoListaChequeo in pListSolicitudPagoListaChequeo)
            {
                bool blRegistroCompletoListaChequeo = true;


                if (SolicitudPagoListaChequeo.SolicitudPagoListaChequeoRespuesta.Count() != SolicitudPagoListaChequeo.SolicitudPagoListaChequeoRespuesta.Where(r => r.RespuestaCodigo != null).ToList().Count())
                    blRegistroCompletoListaChequeo = false;

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
                                                                                    FechaModificacion = DateTime.Now,
                                                                                    RegistroCompleto = RegistroCompletoItem,
                                                                                    UsuarioModificacion = usuarioCreacion,
                                                                                    RespuestaCodigo = SolicitudPagoListaChequeoRespuesta.RespuestaCodigo,
                                                                                    Observacion = SolicitudPagoListaChequeoRespuesta.Observacion,
                                                                                });
                }
                _context.Set<SolicitudPagoListaChequeo>()
                                                   .Where(r => r.SolicitudPagoListaChequeoId == SolicitudPagoListaChequeo.SolicitudPagoListaChequeoId)
                                                                                            .Update(s => new SolicitudPagoListaChequeo
                                                                                            {
                                                                                                RegistroCompleto = blRegistroCompletoListaChequeo,
                                                                                                FechaModificacion = DateTime.Now,
                                                                                                UsuarioModificacion = usuarioCreacion
                                                                                            });
            }
        }

        private bool CreateEditNewPaymentWayToPay(SolicitudPagoCargarFormaPago pSolicitudPagoCargarFormaPago)
        {
            try
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
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        private void CreateEditRegistrarSolicitudPago(SolicitudPagoRegistrarSolicitudPago solicitudPagoRegistrarSolicitudPago)
        {

            if (solicitudPagoRegistrarSolicitudPago.SolicitudPagoRegistrarSolicitudPagoId > 0)
            {
                SolicitudPagoRegistrarSolicitudPago solicitudPagoRegistrarSolicitudPagoOld = _context.SolicitudPagoRegistrarSolicitudPago.Find(solicitudPagoRegistrarSolicitudPago.SolicitudPagoRegistrarSolicitudPagoId);
                solicitudPagoRegistrarSolicitudPagoOld.FechaModificacion = DateTime.Now;
                solicitudPagoRegistrarSolicitudPagoOld.RegistroCompleto = ValidateCompleteRecordSolicitudPagoRegistrarSolicitudPago(solicitudPagoRegistrarSolicitudPago);
                solicitudPagoRegistrarSolicitudPagoOld.TieneFasePreconstruccion = solicitudPagoRegistrarSolicitudPago.TieneFasePreconstruccion;
                solicitudPagoRegistrarSolicitudPagoOld.FechaSolicitud = solicitudPagoRegistrarSolicitudPago.FechaSolicitud;
                solicitudPagoRegistrarSolicitudPagoOld.TieneFaseConstruccion = solicitudPagoRegistrarSolicitudPago.TieneFaseConstruccion;
                solicitudPagoRegistrarSolicitudPagoOld.FechaSolicitud = solicitudPagoRegistrarSolicitudPago.FechaSolicitud;
                solicitudPagoRegistrarSolicitudPagoOld.NumeroRadicadoSac = solicitudPagoRegistrarSolicitudPago.NumeroRadicadoSac;
            }
            else
            {
                solicitudPagoRegistrarSolicitudPago.FechaCreacion = DateTime.Now;
                solicitudPagoRegistrarSolicitudPago.Eliminado = false;
                solicitudPagoRegistrarSolicitudPago.RegistroCompleto = ValidateCompleteRecordSolicitudPagoRegistrarSolicitudPago(solicitudPagoRegistrarSolicitudPago);

                _context.SolicitudPagoRegistrarSolicitudPago.Add(solicitudPagoRegistrarSolicitudPago);
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
                if (SolicitudPagoFase.SolicitudPagoFaseCriterio.Count() > 0)
                    CreateEditSolicitudPagoFaseCriterio(SolicitudPagoFase.SolicitudPagoFaseCriterio, SolicitudPagoFase.UsuarioCreacion);


                if (SolicitudPagoFase.SolicitudPagoFaseFactura.Count() > 0)
                    CreateEditSolicitudPagoFaseFactura(SolicitudPagoFase.SolicitudPagoFaseFactura, pUsuarioCreacion);
                if (!SolicitudPagoFase.EsPreconstruccion)
                    if (SolicitudPagoFase.SolicitudPagoFaseAmortizacion.Count() > 0)
                        CreateEditSolicitudPagoSolicitudPagoAmortizacion(SolicitudPagoFase.SolicitudPagoFaseAmortizacion, pUsuarioCreacion);

                if (SolicitudPagoFase.SolicitudPagoFaseId > 0)
                {
                    SolicitudPagoFase solicitudPagoFaseOld = _context.SolicitudPagoFase.Find(SolicitudPagoFase.SolicitudPagoFaseId);

                    if (SolicitudPagoFase.SolicitudPagoFaseCriterio.Count() > 0)
                        solicitudPagoFaseOld.RegistroCompletoCriterio = ValidateCompleteRecordSolicitudPagoFaseCriterio2(SolicitudPagoFase.SolicitudPagoFaseCriterio);

                    solicitudPagoFaseOld.UsuarioModificacion = pUsuarioCreacion;
                    solicitudPagoFaseOld.FechaModificacion = DateTime.Now;

                    solicitudPagoFaseOld.RegistroCompleto = ValidateCompleteRecordSolicitudPagoFase(SolicitudPagoFase);
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
            }
        }

        private void CreateEditSolicitudPagoSolicitudPagoAmortizacion(ICollection<SolicitudPagoFaseAmortizacion> pSolicitudPagoAmortizacionList, string pUsuarioCreacion)
        {
            foreach (var SolicitudPagoAmortizacion in pSolicitudPagoAmortizacionList)
            {
                if (SolicitudPagoAmortizacion.SolicitudPagoFaseAmortizacionId > 0)
                {
                    SolicitudPagoFaseAmortizacion solicitudPagoAmortizacionOld = _context.SolicitudPagoFaseAmortizacion.Find(SolicitudPagoAmortizacion.SolicitudPagoFaseAmortizacionId);
                    solicitudPagoAmortizacionOld.UsuarioModificacion = pUsuarioCreacion;
                    solicitudPagoAmortizacionOld.FechaModificacion = DateTime.Now;

                    solicitudPagoAmortizacionOld.PorcentajeAmortizacion = SolicitudPagoAmortizacion.PorcentajeAmortizacion;
                    solicitudPagoAmortizacionOld.ValorAmortizacion = SolicitudPagoAmortizacion.ValorAmortizacion;
                    solicitudPagoAmortizacionOld.RegistroCompleto = ValidateCompleteRecordSolicitudPagoAmortizacion(SolicitudPagoAmortizacion);
                }
                else
                {
                    SolicitudPagoAmortizacion.Eliminado = true;
                    SolicitudPagoAmortizacion.UsuarioCreacion = pUsuarioCreacion;
                    SolicitudPagoAmortizacion.FechaCreacion = DateTime.Now;
                    SolicitudPagoAmortizacion.RegistroCompleto = ValidateCompleteRecordSolicitudPagoAmortizacion(SolicitudPagoAmortizacion);

                    _context.SolicitudPagoFaseAmortizacion.Add(SolicitudPagoAmortizacion);
                }
            }
        }

        private void CreateEditSolicitudPagoFaseFactura(ICollection<SolicitudPagoFaseFactura> pSolicitudPagoFaseFacturaList, string pUsuarioCreacion)
        {
            foreach (var SolicitudPagoFaseFactura in pSolicitudPagoFaseFacturaList)
            {
                if (SolicitudPagoFaseFactura.SolicitudPagoFaseFacturaDescuento.Count() > 0)
                    CreateEditSolicitudPagoFaseDescuento(SolicitudPagoFaseFactura.SolicitudPagoFaseFacturaDescuento, pUsuarioCreacion);

                if (SolicitudPagoFaseFactura.SolicitudPagoFaseFacturaId > 0)
                {
                    SolicitudPagoFaseFactura solicitudPagoFaseFacturaOld = _context.SolicitudPagoFaseFactura.Find(SolicitudPagoFaseFactura.SolicitudPagoFaseFacturaId);
                    solicitudPagoFaseFacturaOld.UsuarioModificacion = SolicitudPagoFaseFactura.UsuarioModificacion;
                    solicitudPagoFaseFacturaOld.FechaModificacion = SolicitudPagoFaseFactura.FechaModificacion;
                    solicitudPagoFaseFacturaOld.RegistroCompleto = ValidateCompleteRecordSolicitudPagoFaseFactura(SolicitudPagoFaseFactura);
                    solicitudPagoFaseFacturaOld.TieneDescuento = SolicitudPagoFaseFactura.TieneDescuento;
                    solicitudPagoFaseFacturaOld.Fecha = SolicitudPagoFaseFactura.Fecha;
                    solicitudPagoFaseFacturaOld.ValorFacturado = SolicitudPagoFaseFactura.ValorFacturado;
                    solicitudPagoFaseFacturaOld.Numero = SolicitudPagoFaseFactura.Numero;
                    solicitudPagoFaseFacturaOld.ValorFacturadoConDescuento = SolicitudPagoFaseFactura.ValorFacturadoConDescuento;
                }
                else
                {
                    SolicitudPagoFaseFactura.Eliminado = false;
                    SolicitudPagoFaseFactura.UsuarioCreacion = pUsuarioCreacion;
                    SolicitudPagoFaseFactura.FechaCreacion = DateTime.Now;
                    SolicitudPagoFaseFactura.RegistroCompleto = ValidateCompleteRecordSolicitudPagoFaseFactura(SolicitudPagoFaseFactura);
                    _context.SolicitudPagoFaseFactura.Add(SolicitudPagoFaseFactura);
                }
            }
        }

        private void CreateEditSolicitudPagoFaseCriterio(ICollection<SolicitudPagoFaseCriterio> ListSolicitudPagoFaseCriterio, string strUsuarioCreacion)
        {
            foreach (var SolicitudPagoFaseCriterio in ListSolicitudPagoFaseCriterio)
            {
                //Crear Lista Chequeo Criterio Codigo
                CreateEditListaChequeoByCriterio(SolicitudPagoFaseCriterio, strUsuarioCreacion);

                if (SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioProyecto.Count() > 0)
                    CreateEditSolicitudPagoFaseCriterioProyecto(SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioProyecto, strUsuarioCreacion);

                foreach (var SolicitudPagoFaseCriterioConceptoPago in SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioConceptoPago)
                {
                    if (SolicitudPagoFaseCriterioConceptoPago.SolicitudPagoFaseCriterioConceptoPagoId > 0)
                    {
                        SolicitudPagoFaseCriterioConceptoPago SolicitudPagoFaseCriterioConceptoPagoOld = _context.SolicitudPagoFaseCriterioConceptoPago.Find(SolicitudPagoFaseCriterioConceptoPago.SolicitudPagoFaseCriterioConceptoPagoId);
                        SolicitudPagoFaseCriterioConceptoPagoOld.SolicitudPagoFaseCriterio = SolicitudPagoFaseCriterioConceptoPago.SolicitudPagoFaseCriterio;
                        SolicitudPagoFaseCriterioConceptoPagoOld.ValorFacturadoConcepto = SolicitudPagoFaseCriterioConceptoPago.ValorFacturadoConcepto;
                    }
                    else
                    {
                        SolicitudPagoFaseCriterioConceptoPago.Eliminado = false;
                        SolicitudPagoFaseCriterioConceptoPago.UsuarioCreacion = strUsuarioCreacion;
                        SolicitudPagoFaseCriterioConceptoPago.FechaCreacion = DateTime.Now;
                        _context.SolicitudPagoFaseCriterioConceptoPago.Add(SolicitudPagoFaseCriterioConceptoPago);
                    }
                }

                if (SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioId > 0)
                {
                    SolicitudPagoFaseCriterio SolicitudPagoFaseCriterioOld = _context.SolicitudPagoFaseCriterio.Find(SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioId);

                    SolicitudPagoFaseCriterioOld.FechaModificacion = DateTime.Now;
                    SolicitudPagoFaseCriterioOld.UsuarioModificacion = strUsuarioCreacion;
                    SolicitudPagoFaseCriterioOld.RegistroCompleto = ValidateCompleteRecordSolicitudPagoFaseCriterio(SolicitudPagoFaseCriterio);
                    SolicitudPagoFaseCriterioOld.ValorFacturado = SolicitudPagoFaseCriterio.ValorFacturado;
                    SolicitudPagoFaseCriterioOld.SolicitudPagoFaseId = SolicitudPagoFaseCriterio.SolicitudPagoFaseId;
                    SolicitudPagoFaseCriterioOld.TipoCriterioCodigo = SolicitudPagoFaseCriterio.TipoCriterioCodigo;
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

        private void CreateEditSolicitudPagoFaseCriterioProyecto(ICollection<SolicitudPagoFaseCriterioProyecto> ListSolicitudPagoFaseCriterioProyecto, string pStrUsuarioCreacion)
        {
            foreach (var SolicitudPagoFaseCriterioProyecto in ListSolicitudPagoFaseCriterioProyecto)
            {
                if (SolicitudPagoFaseCriterioProyecto.SolicitudPagoFaseCriterioProyectoId > 0)
                {
                    SolicitudPagoFaseCriterioProyecto solicitudPagoFaseCriterioProyectoOld = _context.SolicitudPagoFaseCriterioProyecto.Find(SolicitudPagoFaseCriterioProyecto.SolicitudPagoFaseCriterioProyectoId);

                    solicitudPagoFaseCriterioProyectoOld.UsuarioModificacion = pStrUsuarioCreacion;
                    solicitudPagoFaseCriterioProyectoOld.FechaModificacion = DateTime.Now;
                    solicitudPagoFaseCriterioProyectoOld.ValorFacturado = SolicitudPagoFaseCriterioProyecto.ValorFacturado;
                    solicitudPagoFaseCriterioProyectoOld.RegistroCompleto = ValidateCompleteRecordSolicitudPagoFaseCriterioProyecto(SolicitudPagoFaseCriterioProyecto);
                }
                else
                {
                    SolicitudPagoFaseCriterioProyecto.Eliminado = false;
                    SolicitudPagoFaseCriterioProyecto.UsuarioCreacion = pStrUsuarioCreacion;
                    SolicitudPagoFaseCriterioProyecto.FechaCreacion = DateTime.Now;
                    SolicitudPagoFaseCriterioProyecto.RegistroCompleto = ValidateCompleteRecordSolicitudPagoFaseCriterioProyecto(SolicitudPagoFaseCriterioProyecto);
                    _context.SolicitudPagoFaseCriterioProyecto.Add(SolicitudPagoFaseCriterioProyecto);
                }
            }
        }

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

            foreach (var SolicitudPagoListaChequeo in pSolicitudPago.SolicitudPagoListaChequeo)
            {
                foreach (var SolicitudPagoListaChequeoRespuesta in SolicitudPagoListaChequeo.SolicitudPagoListaChequeoRespuesta)
                {
                    if (SolicitudPagoListaChequeoRespuesta.RegistroCompleto != true)
                        return false;
                }
            }

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

        private bool ValidateCompleteRecordSolicitudPagoFaseFactura(SolicitudPagoFaseFactura solicitudPagoFaseFactura)
        {

            if (solicitudPagoFaseFactura.TieneDescuento == false)
                return true;
            if (
                   string.IsNullOrEmpty(solicitudPagoFaseFactura.Fecha.ToString())
                || !solicitudPagoFaseFactura.TieneDescuento.HasValue
                || string.IsNullOrEmpty(solicitudPagoFaseFactura.ValorFacturado.ToString())
                || string.IsNullOrEmpty(solicitudPagoFaseFactura.Numero.ToString())
                )
                return false;

            if (solicitudPagoFaseFactura.SolicitudPagoFaseFacturaDescuento.Count() == 0)
                return false;

            foreach (var SolicitudPagoFaseFacturaDescuento in solicitudPagoFaseFactura.SolicitudPagoFaseFacturaDescuento)
            {
                if (!ValidateCompleteRecordSolicitudPagoFaseFacturaDescuento(SolicitudPagoFaseFacturaDescuento))
                    return false;
            }
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
                || string.IsNullOrEmpty(solicitudPagoFaseCriterio.ValorFacturado.ToString())
                )
                return false;

            if (solicitudPagoFaseCriterio.SolicitudPagoFaseCriterioProyecto.Count() == 0)
                return false;

            foreach (var SolicitudPagoFaseCriterioProyecto in solicitudPagoFaseCriterio.SolicitudPagoFaseCriterioProyecto)
            {
                if (!ValidateCompleteRecordSolicitudPagoFaseCriterioProyecto(SolicitudPagoFaseCriterioProyecto))
                    return false;
            }
            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoFaseCriterioProyecto(SolicitudPagoFaseCriterioProyecto solicitudPagoFaseCriterioProyecto)
        {
            if (string.IsNullOrEmpty(solicitudPagoFaseCriterioProyecto.ValorFacturado.ToString()))
                return false;
            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoRegistrarSolicitudPago(SolicitudPagoRegistrarSolicitudPago pSolicitudPagoRegistrarSolicitudPago)
        {
            if (!pSolicitudPagoRegistrarSolicitudPago.TieneFaseConstruccion.HasValue
                || !pSolicitudPagoRegistrarSolicitudPago.TieneFasePreconstruccion.HasValue
                || !pSolicitudPagoRegistrarSolicitudPago.FechaSolicitud.HasValue
                || pSolicitudPagoRegistrarSolicitudPago.NumeroRadicadoSac == null
                ) return false;

            if (pSolicitudPagoRegistrarSolicitudPago.SolicitudPagoFase.Count() == 0)
                return false;

            if (pSolicitudPagoRegistrarSolicitudPago.SolicitudPagoFase.FirstOrDefault().SolicitudPagoFaseFactura.Count() == 0)
                return false;

            foreach (var SolicitudPagoFaseFactura in pSolicitudPagoRegistrarSolicitudPago.SolicitudPagoFase.FirstOrDefault().SolicitudPagoFaseFactura)
            {
                if (!ValidateCompleteRecordSolicitudPagoFaseFactura(SolicitudPagoFaseFactura))
                    return false;
            }

            if (!ValidateCompleteRecordSolicitudPagoFase(pSolicitudPagoRegistrarSolicitudPago.SolicitudPagoFase.FirstOrDefault()))
                return false;

            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoFase(SolicitudPagoFase pSolicitudPagoFase)
        {
            //La Fase Construccion Es la unica que tiene amortizacion
            if (!pSolicitudPagoFase.EsPreconstruccion)
            {
                if (pSolicitudPagoFase.SolicitudPagoFaseAmortizacion.Count() == 0)
                    return false;

                foreach (var SolicitudPagoAmortizacion in pSolicitudPagoFase.SolicitudPagoFaseAmortizacion)
                {
                    if (!ValidateCompleteRecordSolicitudPagoAmortizacion(SolicitudPagoAmortizacion))
                        return false;
                }
            }
            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoAmortizacion(SolicitudPagoFaseAmortizacion solicitudPagoAmortizacion)
        {
            if (
                     string.IsNullOrEmpty(solicitudPagoAmortizacion.PorcentajeAmortizacion.ToString())
                  || string.IsNullOrEmpty(solicitudPagoAmortizacion.ValorAmortizacion.ToString())
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
            foreach (var solicitudPagoFaseCriterio in ListsolicitudPagoFaseCriterio)
            {
                if (
                       string.IsNullOrEmpty(solicitudPagoFaseCriterio.TipoCriterioCodigo)
                    || string.IsNullOrEmpty(solicitudPagoFaseCriterio.ValorFacturado.ToString())
                      ) return false;
            }
            return true;
        }

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

                return
                     new Respuesta
                     {
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

        private async void CreateEditNewExpensas(SolicitudPago pSolicitudPago)
        {
            if (pSolicitudPago.SolicitudPagoExpensas.Count() > 0)
            {
                CreateEditSolicitudPagoExpensas(pSolicitudPago.SolicitudPagoExpensas, pSolicitudPago.UsuarioCreacion);
            }

            if (pSolicitudPago.SolicitudPagoId > 0)
            {
                SolicitudPago solicitudPagoOld = _context.SolicitudPago.Find(pSolicitudPago.SolicitudPagoId);

                solicitudPagoOld.FechaModificacion = DateTime.Now;
                solicitudPagoOld.UsuarioModificacion = pSolicitudPago.UsuarioCreacion;
                solicitudPagoOld.RegistroCompleto = ValidateCompleteRecordSolicitudPagoExpensas(pSolicitudPago);
            }
            else
            {
                pSolicitudPago.EstadoCodigo = ((int)EnumEstadoSolicitudPago.En_proceso_de_registro).ToString();
                pSolicitudPago.NumeroSolicitud = await _commonService.EnumeradorSolicitudPagoExpensasAndOtros();
                pSolicitudPago.FechaCreacion = DateTime.Now;
                pSolicitudPago.Eliminado = false;
                pSolicitudPago.RegistroCompleto = ValidateCompleteRecordSolicitudPagoExpensas(pSolicitudPago);

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

        private bool ValidateCompleteRecordSolicitudPagoExpensas(SolicitudPago pSolicitudPago)
        {
            if (
                   pSolicitudPago.SolicitudPagoExpensas.Count() == 0
                || pSolicitudPago.SolicitudPagoSoporteSolicitud.Count() == 0
                || pSolicitudPago.SolicitudPagoListaChequeo.Count() == 0)
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

                    solicitudPagoOld.FechaModificacion = DateTime.Now;
                    solicitudPagoOld.UsuarioModificacion = pSolicitudPago.UsuarioCreacion;
                }
                else
                {
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

                return
                     new Respuesta
                     {
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

    }
}