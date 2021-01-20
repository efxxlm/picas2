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

        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;

        public RegisterValidatePaymentRequierementsService(IDocumentService documentService, devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _documentService = documentService;
            _commonService = commonService;
            _context = context;
        }

        #region Get
        public async Task<dynamic> GetContratoByTipoSolicitudCodigoModalidadContratoCodigoOrNumeroContrato(string pTipoSolicitud, string pModalidadContrato, string pNumeroContrato)
        {
            return await _context.Contrato
                                          .Include(c => c.Contratacion)
                                          .Where(c => c.NumeroContrato.Trim().ToLower().Contains(pNumeroContrato.Trim().ToLower())
                                                   && c.Contratacion.TipoSolicitudCodigo == pTipoSolicitud
                                                   && c.EstadoActaFase2 == ConstanCodigoEstadoActaContrato.Con_acta_suscrita_y_cargada)
                                                                                            .Select(r => new
                                                                                            {
                                                                                                r.ContratoId,
                                                                                                r.NumeroContrato
                                                                                            }).ToListAsync();
        }

        public async Task<Contrato> GetContratoByContratoId(int pContratoId)
        {
            Contrato contrato = await _context.Contrato
                 .Where(c => c.ContratoId == pContratoId)
                 .Include(c => c.ContratoPoliza)
                 .Include(c => c.Contratacion)
                    .ThenInclude(c => c.Contratista)
                 .Include(c => c.Contratacion)
                    .ThenInclude(cp => cp.DisponibilidadPresupuestal)
                 .Include(r => r.SolicitudPago).FirstOrDefaultAsync();

            if (contrato.SolicitudPago.Count() > 0)
                contrato.SolicitudPagoOnly = GetSolicitudPago(contrato.SolicitudPago.FirstOrDefault());

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
                       .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
                          .ThenInclude(r => r.SolicitudPagoFaseDescuento)
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
            return pFormaPagoCodigo switch
            {
                ConstanCodigoFormaPagoCodigo.Forma_1_50_50 => await _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Criterios_Pago && (r.Codigo == ConstanCodigoCriterioPago.Criterio_1 || r.Codigo == ConstanCodigoCriterioPago.Criterio_2)).Select(r => new { r.DominioId, r.Nombre }).ToListAsync(),
                ConstanCodigoFormaPagoCodigo.Forma_2_50_40_10 => await _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Criterios_Pago && (r.Codigo == ConstanCodigoCriterioPago.Criterio_1 || r.Codigo == ConstanCodigoCriterioPago.Criterio_2)).Select(r => new { r.DominioId, r.Nombre }).ToListAsync(),
                ConstanCodigoFormaPagoCodigo.Forma_3_90_10 => await _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Criterios_Pago && (r.Codigo == ConstanCodigoCriterioPago.Criterio_1 || r.Codigo == ConstanCodigoCriterioPago.Criterio_2)).Select(r => new { r.DominioId, r.Nombre }).ToListAsync(),
                _ => await _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Criterios_Pago && (r.Codigo == ConstanCodigoCriterioPago.Criterio_1 || r.Codigo == ConstanCodigoCriterioPago.Criterio_2)).Select(r => new { r.DominioId, r.Nombre }).ToListAsync(),
            };
        }
        #endregion

        #region Create Edit 

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

                    CreateEditFaseCriterio(pSolicitudPago.SolicitudPagoRegistrarSolicitudPago.FirstOrDefault().SolicitudPagoFase, pSolicitudPago.UsuarioCreacion);

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
         
        private void CreateEditFaseCriterio(ICollection<SolicitudPagoFase> solicitudPagoFaseList, string pUsuarioCreacion)
        {
            foreach (var SolicitudPagoFase in solicitudPagoFaseList)
            {
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

        private void CreateEditNewPaymentNew(SolicitudPago pSolicitudPago)
        {
            if (pSolicitudPago.SolicitudPagoId > 0)
            {
                pSolicitudPago.UsuarioModificacion = pSolicitudPago.UsuarioCreacion;
                pSolicitudPago.FechaModificacion = DateTime.Now;
                pSolicitudPago.RegistroCompleto = ValidateCompleteRecordSolicitudPago(pSolicitudPago);
            }
            else
            {
                pSolicitudPago.Eliminado = false;
                pSolicitudPago.FechaCreacion = DateTime.Now;
                _context.SolicitudPago.Add(pSolicitudPago);
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

        #endregion


        #region Validate Complete Form
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
