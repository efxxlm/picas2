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

        #region Get
        public async Task<dynamic> GetListSolicitudPago()
        {
            var result = await _context.SolicitudPago
                .Include(r => r.Contrato)
                             .Select(s => new
                             {
                                 s.FechaCreacion,
                                 s.NumeroSolicitud,
                                 s.Contrato.Modalidad,
                                 s.Contrato.NumeroContrato,
                                 s.EstadoCodigo,
                                 s.ContratoId,
                                 s.SolicitudPagoId
                             }).ToListAsync();

            List<dynamic> grind = new List<dynamic>();
            List<Dominio> ListParametricas = _context.Dominio.Where(d => d.TipoDominioId == (int)EnumeratorTipoDominio.Modalidad_Contrato || d.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Registro_Pago).ToList();

            result.ForEach(r =>
            {
                grind.Add(new
                {
                    r.ContratoId,
                    r.SolicitudPagoId,
                    r.FechaCreacion,
                    r.NumeroSolicitud,
                    r.NumeroContrato,
                    Estado = !string.IsNullOrEmpty(r.EstadoCodigo) ? ListParametricas.Where(l => l.Codigo == r.EstadoCodigo && l.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Registro_Pago).FirstOrDefault().Nombre : " - ",
                    Modalidad = !string.IsNullOrEmpty(r.Modalidad) ? ListParametricas.Where(l => l.Codigo == r.Modalidad && l.TipoDominioId == (int)EnumeratorTipoDominio.Modalidad_Contrato).FirstOrDefault().Nombre : " - "
                });
            });

            return grind;
        }

        public async Task<dynamic> GetContratoByTipoSolicitudCodigoModalidadContratoCodigoOrNumeroContrato(string pTipoSolicitud, string pModalidadContrato, string pNumeroContrato)
        {
            try
            {
                ///Siempre aparecera el contrato Reunion ivhon 20/01/201 - 5:16  
                List<int?> ListContratosConSolicitudPago = _context.SolicitudPago
                 .Include(c => c.Contrato)
                 .Where(s => s.Eliminado == false && s.ContratoId != null)
                         .Select(r => r.ContratoId).ToList();

                List<Contrato> ListContratos = await _context.Contrato
                                .Include(c => c.Contratacion)
                                         .Where(c => c.NumeroContrato.Trim().ToLower().Contains(pNumeroContrato.Trim().ToLower())
                                               && c.Contratacion.TipoSolicitudCodigo == pTipoSolicitud
                                               && c.EstadoActaFase2 == ConstanCodigoEstadoActaContrato.Con_acta_suscrita_y_cargada
                                               ).ToListAsync();

                ListContratos.RemoveAll(item => ListContratosConSolicitudPago.Contains(item.ContratoId));
                return ListContratos
                    .Select(r => new
                    {
                        r.ContratoId,
                        r.NumeroContrato
                    }).ToList();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Contrato> GetContratoByContratoId(int pContratoId)
        {
            Contrato contrato = await _context.Contrato
                 .Where(c => c.ContratoId == pContratoId)
                 .Include(c => c.ContratoPoliza)
                 .Include(c => c.Contratacion)
                    .ThenInclude(c => c.Contratista)
                 .Include(c => c.Contratacion)
                    .ThenInclude(c => c.ContratacionProyecto)
                 .Include(c => c.Contratacion)
                    .ThenInclude(cp => cp.DisponibilidadPresupuestal)
                 .Include(r => r.SolicitudPago)
                    .ThenInclude(r => r.SolicitudPagoCargarFormaPago)
                 .FirstOrDefaultAsync();

            if (contrato.SolicitudPago.Count() > 0)
                contrato.SolicitudPagoOnly = GetSolicitudPago(contrato.SolicitudPago.LastOrDefault());

            return contrato;
        }

        private SolicitudPago GetSolicitudPago(SolicitudPago solicitudPago)
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
                              .ThenInclude(r => r.SolicitudPagoAmortizacion)
                       .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
                          .ThenInclude(r => r.SolicitudPagoFase)
                              .ThenInclude(r => r.SolicitudPagoFaseFactura)
                                  .ThenInclude(r => r.SolicitudPagoFaseFacturaDescuento)
                       .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
                       .Include(r => r.SolicitudPagoSoporteSolicitud).FirstOrDefault();

                    break;
                case ConstanCodigoTipoSolicitudContratoSolicitudPago.Expensas:

                    break;
                case ConstanCodigoTipoSolicitudContratoSolicitudPago.Otros_Costos_Servicios:

                    break;
            }
            return solicitudPago;
        }

        public async Task<dynamic> GetProyectosByIdContrato(int pContratoId)
        {
            List<dynamic> dynamics = new List<dynamic>();

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
                                                                    p.Sede
                                                                }).ToListAsync();
            dynamics.Add(resultContrato);
            dynamics.Add(resultProyectos);

            return dynamics;

        }

        public async Task<dynamic> GetCriterioByFormaPagoCodigo(string pFormaPagoCodigo)
        {
            List<dynamic> ListDynamics = new List<dynamic>();

            List<string> strCriterios = _context.FormaPagoCodigoCriterioPagoCodigo.Where(r => r.FormaPagoCodigo == pFormaPagoCodigo).Select(r => r.CriterioPagoCodigo).ToList();
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

        public async Task<dynamic> GetTipoPagoByCriterioCodigo(string pCriterioCodigo)
        {
            List<dynamic> ListDynamics = new List<dynamic>();
            List<string> strCriterios = _context.CriterioCodigoTipoPagoCodigo.Where(r => r.CriterioCodigo == pCriterioCodigo).Select(r => r.TipoPagoCodigo).ToList();
            List<Dominio> ListCriterio = await _commonService.GetListDominioByIdTipoDominio((int)EnumeratorTipoDominio.Tipo_Pago);

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

        public async Task<dynamic> GetConceptoPagoCriterioCodigoByTipoPagoCodigo(string TipoPagoCodigo)
        {
            List<dynamic> ListDynamics = new List<dynamic>();
            List<string> strCriterios = _context.TipoPagoCodigoConceptoPagoCriterioCodigo.Where(r => r.TipoPagoCodigo == TipoPagoCodigo).Select(r => r.ConceptoPagoCriterioCodigo).ToList();
            List<Dominio> ListCriterio = await _commonService.GetListDominioByIdTipoDominio((int)EnumeratorTipoDominio.Concepto_Pago_Criterio);

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

        #endregion

        #region Create Edit 

        #region  Tipo Obra Interventoria
        private async void CreateEditNewPaymentNew(SolicitudPago pSolicitudPago)
        {
            //Valida si el contrato de la solicitud es interventoria o Obra
            string strInterventoriaCodigo = _context.SolicitudPago
                .Include(s => s.Contrato).ThenInclude(ctr => ctr.Contratacion)
                .Where(s => s.ContratoId == pSolicitudPago.ContratoId)
                         .Select(crt =>
                                 crt.Contrato.Contratacion.TipoSolicitudCodigo
                         ).FirstOrDefault();

            if (pSolicitudPago.SolicitudPagoSoporteSolicitud.Count() > 0)
                CreateEditNewSolicitudPagoSoporteSolicitud(pSolicitudPago.SolicitudPagoSoporteSolicitud, pSolicitudPago.UsuarioCreacion);

            if (pSolicitudPago.SolicitudPagoId > 0)
            {
                pSolicitudPago.UsuarioModificacion = pSolicitudPago.UsuarioCreacion;
                pSolicitudPago.FechaModificacion = DateTime.Now;
                pSolicitudPago.RegistroCompleto = ValidateCompleteRecordSolicitudPago(pSolicitudPago);
            }
            else
            {
                pSolicitudPago.NumeroSolicitud = Int32.Parse(strInterventoriaCodigo) == ConstanCodigoTipoContratacion.Obra ? await _commonService.EnumeradorSolicitudPago(true) : await _commonService.EnumeradorSolicitudPago(false);
                pSolicitudPago.EstadoCodigo = ConstanCodigoEstadoSolicitudPago.En_proceso_de_registro;
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

        private bool CreateEditNewPaymentWayToPay(SolicitudPagoCargarFormaPago pSolicitudPagoCargarFormaPago)
        {
            try
            {
                if (pSolicitudPagoCargarFormaPago.SolicitudPagoCargarFormaPagoId > 0)
                {
                    SolicitudPagoCargarFormaPago solicitudPagoCargarFormaPagoOld = _context.SolicitudPagoCargarFormaPago.Find(pSolicitudPagoCargarFormaPago.SolicitudPagoCargarFormaPagoId);
                    solicitudPagoCargarFormaPagoOld.FechaModificacion = DateTime.Now;
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
                    SolicitudPagoFaseDescuento.Eliminado = true;
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
                if (SolicitudPagoFase.SolicitudPagoAmortizacion.Count() > 0)
                    CreateEditSolicitudPagoSolicitudPagoAmortizacion(SolicitudPagoFase.SolicitudPagoAmortizacion, pUsuarioCreacion);


                if (SolicitudPagoFase.SolicitudPagoFaseId > 0)
                {
                    SolicitudPagoFase solicitudPagoFaseOld = _context.SolicitudPagoFase.Find(SolicitudPagoFase.SolicitudPagoFaseId);
                    solicitudPagoFaseOld.UsuarioModificacion = pUsuarioCreacion;
                    solicitudPagoFaseOld.FechaModificacion = DateTime.Now;
                    solicitudPagoFaseOld.RegistroCompleto = ValidateCompleteRecordSolicitudPagoFase(SolicitudPagoFase);
                }
                else
                {
                    SolicitudPagoFase.FechaCreacion = DateTime.Now;
                    SolicitudPagoFase.Eliminado = false;
                    SolicitudPagoFase.UsuarioCreacion = pUsuarioCreacion;
                    SolicitudPagoFase.RegistroCompleto = ValidateCompleteRecordSolicitudPagoFase(SolicitudPagoFase);
                    _context.SolicitudPagoFase.Add(SolicitudPagoFase);
                }
            }
        }

        private void CreateEditSolicitudPagoSolicitudPagoAmortizacion(ICollection<SolicitudPagoAmortizacion> pSolicitudPagoAmortizacionList, string pUsuarioCreacion)
        {
            foreach (var SolicitudPagoAmortizacion in pSolicitudPagoAmortizacionList)
            {
                if (SolicitudPagoAmortizacion.SolicitudPagoFaseAmortizacionId > 0)
                {
                    SolicitudPagoAmortizacion solicitudPagoAmortizacionOld = _context.SolicitudPagoAmortizacion.Find(SolicitudPagoAmortizacion.SolicitudPagoFaseAmortizacionId);
                    solicitudPagoAmortizacionOld.UsuarioModificacion = pUsuarioCreacion;
                    solicitudPagoAmortizacionOld.FechaModificacion = DateTime.Now;

                    solicitudPagoAmortizacionOld.PorcentajeAmortizacion = SolicitudPagoAmortizacion.PorcentajeAmortizacion;
                    solicitudPagoAmortizacionOld.ValorAmortizacion = SolicitudPagoAmortizacion.ValorAmortizacion;
                    solicitudPagoAmortizacionOld.RegistroCompleto = ValidateCompleteRecordSolicitudPagoAmortizacion(SolicitudPagoAmortizacion);
                }
                else
                {
                    SolicitudPagoAmortizacion.Eliminado = SolicitudPagoAmortizacion.Eliminado;
                    SolicitudPagoAmortizacion.UsuarioCreacion = pUsuarioCreacion;
                    SolicitudPagoAmortizacion.FechaCreacion = DateTime.Now;
                    SolicitudPagoAmortizacion.RegistroCompleto = ValidateCompleteRecordSolicitudPagoAmortizacion(SolicitudPagoAmortizacion);

                    _context.SolicitudPagoAmortizacion.Add(SolicitudPagoAmortizacion);
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

                    solicitudPagoFaseFacturaOld.Fecha = SolicitudPagoFaseFactura.Fecha;
                    solicitudPagoFaseFacturaOld.ValorFacturado = SolicitudPagoFaseFactura.ValorFacturado;
                    solicitudPagoFaseFacturaOld.Numero = SolicitudPagoFaseFactura.Numero;
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
                if (SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioProyecto.Count() > 0)
                    CreateEditSolicitudPagoFaseCriterioProyecto(SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioProyecto, strUsuarioCreacion);

                if (SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioId > 0)
                {
                    SolicitudPagoFaseCriterio SolicitudPagoFaseCriterioOld = _context.SolicitudPagoFaseCriterio.Find(SolicitudPagoFaseCriterio);

                    SolicitudPagoFaseCriterioOld.FechaModificacion = DateTime.Now;
                    SolicitudPagoFaseCriterioOld.UsuarioModificacion = strUsuarioCreacion;
                    SolicitudPagoFaseCriterioOld.RegistroCompleto = ValidateCompleteRecordSolicitudPagoFaseCriterio(SolicitudPagoFaseCriterio);

                    SolicitudPagoFaseCriterioOld.ConceptoPagoCriterio = SolicitudPagoFaseCriterio.ConceptoPagoCriterio;
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
                    SolicitudPagoFaseCriterioProyecto.UsuarioCreacion = pStrUsuarioCreacion;
                    SolicitudPagoFaseCriterioProyecto.FechaCreacion = DateTime.Now;
                    SolicitudPagoFaseCriterioProyecto.RegistroCompleto = ValidateCompleteRecordSolicitudPagoFaseCriterioProyecto(SolicitudPagoFaseCriterioProyecto);
                    _context.SolicitudPagoFaseCriterioProyecto.Add(SolicitudPagoFaseCriterioProyecto);
                }
            }
        }
     
        private bool ValidateCompleteRecordSolicitudPagoSoporteSolicitud(SolicitudPagoSoporteSolicitud solicitudPagoSoporteSolicitudOld)
        {
            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoAmortizacion(SolicitudPagoAmortizacion solicitudPagoAmortizacion)
        {
            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoFaseFactura(SolicitudPagoFaseFactura solicitudPagoFaseFactura)
        {
            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoFaseFacturaDescuento(SolicitudPagoFaseFacturaDescuento solicitudPagoFaseDescuento)
        {
            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoFaseCriterioProyecto(SolicitudPagoFaseCriterioProyecto solicitudPagoFaseCriterioProyecto)
        {
            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoFaseCriterio(SolicitudPagoFaseCriterio solicitudPagoFaseCriterio)
        {
            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoRegistrarSolicitudPago(SolicitudPagoRegistrarSolicitudPago pSolicitudPagoRegistrarSolicitudPago)
        {
            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoFase(SolicitudPagoFase pSolicitudPagoFase)
        {
            return true;
        }

        private bool ValidateCompleteRecordSolicitudPago(SolicitudPago pSolicitudPago)
        {
            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoCargarFormaPago(SolicitudPagoCargarFormaPago pSolicitudPagoCargarFormaPago)
        {
            if (pSolicitudPagoCargarFormaPago.TieneFase1)
                if (string.IsNullOrEmpty(pSolicitudPagoCargarFormaPago.FasePreConstruccionFormaPagoCodigo))
                    return false;

            if (string.IsNullOrEmpty(pSolicitudPagoCargarFormaPago.FaseConstruccionFormaPagoCodigo))
                return false;

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

        private void CreateEditNewExpensas(SolicitudPago pSolicitudPago)
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
                solicitudPagoOld.RegistroCompleto = ValidateCompleteRecordopSolicitudPagoExpensas(pSolicitudPago);
            }
            else
            {
                pSolicitudPago.FechaCreacion = DateTime.Now;
                pSolicitudPago.Eliminado = false;
                pSolicitudPago.RegistroCompleto = ValidateCompleteRecordopSolicitudPagoExpensas(pSolicitudPago);

                _context.SolicitudPago.Add(pSolicitudPago);
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
            return true;
        }

        private bool ValidateCompleteRecordopSolicitudPagoExpensas(SolicitudPago pSolicitudPago)
        {
            return true;
        }

        #endregion;

        #region Tipo Otros Costos Servicios
        public async Task<Respuesta> CreateEditOtrosCostosServicios(SolicitudPago pSolicitudPago)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Solicitud_De_Pago, (int)EnumeratorTipoDominio.Acciones);

            try
            {

                if (pSolicitudPago.SolicitudPagoExpensas.Count() > 0)
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
                    solicitudPagoOld.RegistroCompleto = ValidateCompleteRecordoSolicitudPagoOtrosCostosServicios(pSolicitudPago);
                }
                else
                {
                    pSolicitudPago.FechaCreacion = DateTime.Now;
                    pSolicitudPago.Eliminado = false;
                    pSolicitudPago.RegistroCompleto = ValidateCompleteRecordoSolicitudPagoOtrosCostosServicios(pSolicitudPago);

                    _context.SolicitudPago.Add(pSolicitudPago);
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

        private bool? ValidateCompleteRecordoOtrosCostosServicios(SolicitudPagoOtrosCostosServicios solicitudPagoOtrosCostosServiciosOld)
        {
            throw new NotImplementedException();
        }

        private bool ValidateCompleteRecordoSolicitudPagoOtrosCostosServicios(SolicitudPago pSolicitudPago)
        {
            return true;
        }



        #endregion

        #endregion

        #region Validate 

     

        #endregion

    }
}
