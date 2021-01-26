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

        public GenerateSpinOrderService(IDocumentService documentService, devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _documentService = documentService;
            _commonService = commonService;
            _context = context;
        }
        /// <summary>
        /// TODO : VALIDAR SOLICITUDES DE PAGO QUE YA TENGAN APROBACION 
        /// </summary>
        /// <returns></returns>
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
                    Modalidad = !string.IsNullOrEmpty(r.ModalidadCodigo) ? ListParametricas.Where(l => l.Codigo == r.ModalidadCodigo && l.TipoDominioId == (int)EnumeratorTipoDominio.Modalidad_Contrato).FirstOrDefault().Nombre : "No aplica"
                });
            });
            return grind;
        }


      
        private  void GetOrdenGiroByOrdenGiroId(int pOrdenGiroId)
        { 



            //Contrato contrato = await _context.Contrato
            //.Include(c => c.ContratoPoliza)
            //.Include(c => c.Contratacion)
            //   .ThenInclude(c => c.Contratista)
            //.Include(c => c.Contratacion)
            //   .ThenInclude(c => c.ContratacionProyecto)
            //.Include(c => c.Contratacion)
            //   .ThenInclude(cp => cp.DisponibilidadPresupuestal)
            //.Include(r => r.SolicitudPago)
            //   .ThenInclude(r => r.SolicitudPagoCargarFormaPago)
            //.FirstOrDefaultAsync();

            //switch (solicitudPago.TipoSolicitudCodigo)
            //{
            //    case ConstanCodigoTipoSolicitudContratoSolicitudPago.Contratos_Interventoria:
            //    case ConstanCodigoTipoSolicitudContratoSolicitudPago.Contratos_Obra:

            //        solicitudPago = _context.SolicitudPago.Where(r => r.SolicitudPagoId == solicitudPago.SolicitudPagoId)
            //            .Include(r => r.SolicitudPagoCargarFormaPago)
            //            .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
            //               .ThenInclude(r => r.SolicitudPagoFase)
            //                   .ThenInclude(r => r.SolicitudPagoFaseCriterio)
            //                       .ThenInclude(r => r.SolicitudPagoFaseCriterioProyecto)
            //           .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
            //              .ThenInclude(r => r.SolicitudPagoFase)
            //                  .ThenInclude(r => r.SolicitudPagoFaseAmortizacion)
            //           .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
            //              .ThenInclude(r => r.SolicitudPagoFase)
            //                  .ThenInclude(r => r.SolicitudPagoFaseFactura)
            //                      .ThenInclude(r => r.SolicitudPagoFaseFacturaDescuento)
            //           .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
            //           .Include(r => r.SolicitudPagoSoporteSolicitud).FirstOrDefault();

            //        foreach (var SolicitudPagoRegistrarSolicitudPago in solicitudPago.SolicitudPagoRegistrarSolicitudPago)
            //        {
            //            foreach (var SolicitudPagoFase in SolicitudPagoRegistrarSolicitudPago.SolicitudPagoFase)
            //            {
            //                if (SolicitudPagoFase.SolicitudPagoFaseCriterio.Count() > 0)
            //                    SolicitudPagoFase.SolicitudPagoFaseCriterio = SolicitudPagoFase.SolicitudPagoFaseCriterio.Where(r => r.Eliminado != true).ToList();

            //                foreach (var SolicitudPagoFaseCriterio in SolicitudPagoFase.SolicitudPagoFaseCriterio)
            //                {
            //                    if (SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioProyecto.Count() > 0)
            //                        SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioProyecto = SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioProyecto.Where(r => r.Eliminado != true).ToList();
            //                }

            //                foreach (var SolicitudPagoFaseFactura in SolicitudPagoFase.SolicitudPagoFaseFactura)
            //                {
            //                    if (SolicitudPagoFaseFactura.SolicitudPagoFaseFacturaDescuento.Count() > 0)
            //                        SolicitudPagoFaseFactura.SolicitudPagoFaseFacturaDescuento = SolicitudPagoFaseFactura.SolicitudPagoFaseFacturaDescuento.Where(r => r.Eliminado != true).ToList();
            //                }
            //            }
            //        }

            //        return solicitudPago;

            //    case ConstanCodigoTipoSolicitudContratoSolicitudPago.Expensas:
            //        solicitudPago = _context.SolicitudPago.Where(r => r.SolicitudPagoId == solicitudPago.SolicitudPagoId)
            //            .Include(e => e.ContratacionProyecto).ThenInclude(p => p.Proyecto)
            //            .Include(e => e.SolicitudPagoExpensas)
            //            .Include(e => e.SolicitudPagoSoporteSolicitud)
            //            .FirstOrDefault();

            //        return solicitudPago;

            //    case ConstanCodigoTipoSolicitudContratoSolicitudPago.Otros_Costos_Servicios:
            //        solicitudPago = _context.SolicitudPago.Where(r => r.SolicitudPagoId == solicitudPago.SolicitudPagoId)
            //         .Include(e => e.SolicitudPagoOtrosCostosServicios)
            //         .Include(e => e.SolicitudPagoSoporteSolicitud)
            //         .FirstOrDefault();

            //        return solicitudPago;


            //    default: return solicitudPago;

            //}
        }

    }
}
