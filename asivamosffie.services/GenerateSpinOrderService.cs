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
    public class GenerateSpinOrderService : IGenerateSpinOrderService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;
        private readonly IRegisterValidatePaymentRequierementsService _registerValidatePayment;
        public GenerateSpinOrderService(IDocumentService documentService, IRegisterValidatePaymentRequierementsService registerValidatePaymentRequierementsService, devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _documentService = documentService;
            _commonService = commonService;
            _context = context;
            _registerValidatePayment = registerValidatePaymentRequierementsService;
        }
        /// <summary>
        /// TODO : VALIDAR SOLICITUDES DE PAGO QUE YA TENGAN APROBACION 
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> GetListSolicitudPago()
        {
            var result = await _context.SolicitudPago
                 .Include(r => r.Contrato)
                 .Include(r => r.OrdenGiro).Where(s => s.Eliminado != true) 
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
                                                                                    s.OrdenGiro
                                                                            }).OrderByDescending(r => r.SolicitudPagoId).ToListAsync();

            List<dynamic> grind = new List<dynamic>();
            List<Dominio> ListParametricas = _context.Dominio.Where(d => d.TipoDominioId == (int)EnumeratorTipoDominio.Modalidad_Contrato || d.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Registro_Pago).ToList();

            result.ForEach(r =>
            {
                grind.Add(new
                {
                    r.FechaCreacion,
                    r.TipoSolicitudCodigo,
                    r.ContratoId,
                    r.SolicitudPagoId,
                    r.NumeroSolicitud,
                    r.NumeroContrato,
                    r.OrdenGiro
                });
            });
            return grind;
        }

        public async Task<OrdenGiro> GetOrdenGiroByOrdenGiroId(int pOrdenGiroId)
        {
            OrdenGiro ordenGiro = _context.OrdenGiro.Where(o => o.OrdenGiroId == pOrdenGiroId)
                .Include(t => t.OrdenGiroTercero)
                .Include(d => d.OrdenGiroDetalle).ThenInclude(e => e.OrdenGiroDetalleEstrategiaPago)
                .Include(d => d.SolicitudPago).FirstOrDefault();

            ordenGiro.SolicitudPago = await _registerValidatePayment.GetSolicitudPago(ordenGiro.SolicitudPago.SolicitudPagoId);

            return ordenGiro;
        }

        public async Task<Respuesta> CreateEditOrdenGiro(OrdenGiro pOrdenGiro)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Orden_Giro, (int)EnumeratorTipoDominio.Acciones);

            try
            {

                return
                     new Respuesta
                     {
                         IsSuccessful = true,
                         IsException = false,
                         IsValidation = false,
                         Code = GeneralCodes.OperacionExitosa,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Orden_de_giro, GeneralCodes.OperacionExitosa, idAccion, "", "CREAR ORDEN GIRO")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Generar_Orden_de_giro, GeneralCodes.Error, idAccion, "", ex.InnerException.ToString())
                    };
            }
        }


    }
}
