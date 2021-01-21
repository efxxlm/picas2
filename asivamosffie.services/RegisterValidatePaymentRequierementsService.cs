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
        private async void CreateEditNewPaymentNew(SolicitudPago pSolicitudPago)
        {
            string strInterventoriaCodigo = _context.SolicitudPago
                .Include(s => s.Contrato).ThenInclude(ctr => ctr.Contratacion)
                .Where(s => s.ContratoId == pSolicitudPago.ContratoId)
                         .Select(crt =>
                                 crt.Contrato.Contratacion.TipoSolicitudCodigo
                         ).FirstOrDefault();

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

        private void CreateEditSolicitudPagoFaseFactura(ICollection<SolicitudPagoFaseFactura> pSolicitudPagoFaseFacturaList, string pUsuarioCreacion)
        {
            foreach (var SolicitudPagoFaseFactura in pSolicitudPagoFaseFacturaList)
            {
                if (SolicitudPagoFaseFactura.SolicitudPagoFaseFacturaDescuento.Count() > 0)
                    CreateEditSolicitudPagoFaseDescuento(SolicitudPagoFaseFactura.SolicitudPagoFaseFacturaDescuento, pUsuarioCreacion);

                if (SolicitudPagoFaseFactura.SolicitudPagoFaseFacturaId > 0)
                {
                    SolicitudPagoFaseFactura solicitudPagoFaseFacturaOld = _context.SolicitudPagoFaseFactura.Find(SolicitudPagoFaseFactura.SolicitudPagoFaseFacturaId);
                    solicitudPagoFaseFacturaOld.Fecha = SolicitudPagoFaseFactura.Fecha;
                    solicitudPagoFaseFacturaOld.ValorFacturado = SolicitudPagoFaseFactura.ValorFacturado;
                    solicitudPagoFaseFacturaOld.Numero = SolicitudPagoFaseFactura.Numero; 
                    solicitudPagoFaseFacturaOld.UsuarioModificacion = SolicitudPagoFaseFactura.UsuarioModificacion;
                    solicitudPagoFaseFacturaOld.FechaModificacion = SolicitudPagoFaseFactura.FechaModificacion;
                    solicitudPagoFaseFacturaOld.Eliminado = SolicitudPagoFaseFactura.Eliminado;
                    solicitudPagoFaseFacturaOld.RegistroCompleto = SolicitudPagoFaseFactura.RegistroCompleto;

                }
                else
                {

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

        #endregion

        #region Validate Complete Form

        private bool ValidateCompleteRecordSolicitudPagoFaseFacturaDescuento(SolicitudPagoFaseFacturaDescuento solicitudPagoFaseDescuento)
        {
            return false;
        }

        private bool ValidateCompleteRecordSolicitudPagoFaseCriterioProyecto(SolicitudPagoFaseCriterioProyecto solicitudPagoFaseCriterioProyecto)
        {
            return false;
        }

        private bool ValidateCompleteRecordSolicitudPagoFaseCriterio(SolicitudPagoFaseCriterio solicitudPagoFaseCriterio)
        {
            return false;
        }

        private bool ValidateCompleteRecordSolicitudPagoRegistrarSolicitudPago(SolicitudPagoRegistrarSolicitudPago pSolicitudPagoRegistrarSolicitudPago)
        {
            return false;
        }

        private bool ValidateCompleteRecordSolicitudPagoFase(SolicitudPagoFase pSolicitudPagoFase)
        {
            return false;
        }
        private bool ValidateCompleteRecordSolicitudPago(SolicitudPago pSolicitudPago)
        {
            return false;
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

    }
}
