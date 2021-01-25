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
        public async Task<dynamic> GetListProyectosByLlaveMen(string pLlaveMen)
        {
            return await _context.Proyecto.Include(r => r.ContratacionProyecto)
                .Where(r => r.LlaveMen.Contains(pLlaveMen))
                .Select(r => new
                {
                    r.ContratacionProyecto.FirstOrDefault().ContratacionProyectoId,
                    r.LlaveMen
                }).ToListAsync();
        }

        public async Task<dynamic> GetListSolicitudPago()
        {
            var result = await _context.SolicitudPago.Where(s => s.Eliminado != true)
                .Include(r => r.Contrato)
                             .Select(s => new
                             {
                                 s.FechaCreacion,
                                 s.NumeroSolicitud,
                                 s.Contrato.ModalidadCodigo,
                                 s.Contrato.NumeroContrato,
                                 s.EstadoCodigo,
                                 s.ContratoId,
                                 s.SolicitudPagoId,
                                 RegistroCompleto = s.RegistroCompleto ?? false
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
                    Modalidad = !string.IsNullOrEmpty(r.ModalidadCodigo) ? ListParametricas.Where(l => l.Codigo == r.ModalidadCodigo && l.TipoDominioId == (int)EnumeratorTipoDominio.Modalidad_Contrato).FirstOrDefault().Nombre : " - "
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
                else
                {
                    List<int?> ListContratosConSolicitudPago = _context.SolicitudPago
                   .Include(c => c.Contrato)
                   .Where(s => s.Eliminado == false && s.ContratoId != null)
                           .Select(r => r.ContratoId).ToList();

                    List<Contrato> ListContratos = await _context.Contrato
                                    .Include(c => c.Contratacion)
                                             .Where(c => c.NumeroContrato.Trim().ToLower().Contains(pNumeroContrato.Trim().ToLower())
                                                   // && c.Contratacion.TipoSolicitudCodigo == pTipoSolicitud
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
                              .ThenInclude(r => r.SolicitudPagoFaseAmortizacion)
                       .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
                          .ThenInclude(r => r.SolicitudPagoFase)
                              .ThenInclude(r => r.SolicitudPagoFaseFactura)
                                  .ThenInclude(r => r.SolicitudPagoFaseFacturaDescuento)
                       .Include(r => r.SolicitudPagoRegistrarSolicitudPago)
                       .Include(r => r.SolicitudPagoSoporteSolicitud).FirstOrDefault();

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
                                                                    p.Sede,
                                                                    p.ContratacionProyectoId,
                                                                    p.ValorTotal
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

        public async Task<dynamic> GetConceptoPagoCriterioCodigoByTipoPagoCodigo(string TipoPagoCodigo)
        {
            List<dynamic> ListDynamics = new List<dynamic>();
            List<string> strCriterios = _context.TipoPagoCodigoConceptoPagoCriterioCodigo.Where(r => r.TipoPagoCodigo == TipoPagoCodigo).Select(r => r.ConceptoPagoCriterioCodigo).ToList();
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

        #endregion

        #region Create Edit Delete
        public async Task<Respuesta> DeleteSolicitudPago(int pSolicitudPagoId, string pUsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Solicitud_Pago, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SolicitudPago SolicitudPagoDelete = _context.SolicitudPago.Find(pSolicitudPagoId);
                SolicitudPagoDelete.Eliminado = true;
                SolicitudPagoDelete.UsuarioModificacion = pUsuarioModificacion;
                SolicitudPagoDelete.FechaModificacion = DateTime.Now;

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
        public async Task<Respuesta> DeleteSolicitudPagoFaseFacturaDescuento(int pSolicitudPagoFaseFacturaDescuentoId, string pUsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Descuento, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SolicitudPagoFaseFacturaDescuento SolicitudPagoFaseFacturaDescuento = _context.SolicitudPagoFaseFacturaDescuento.Find(pSolicitudPagoFaseFacturaDescuentoId);
                SolicitudPagoFaseFacturaDescuento.Eliminado = true;
                SolicitudPagoFaseFacturaDescuento.UsuarioModificacion = pUsuarioModificacion;
                SolicitudPagoFaseFacturaDescuento.FechaModificacion = DateTime.Now;
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
                SolicitudPagoFaseCriterio solicitudPagoFaseCriterioOld = _context.SolicitudPagoFaseCriterio.Find(pSolicitudPagoFaseCriterioId);
                solicitudPagoFaseCriterioOld.FechaModificacion = DateTime.Now;
                solicitudPagoFaseCriterioOld.UsuarioModificacion = pUsuarioModificacion;
                solicitudPagoFaseCriterioOld.Eliminado = true;


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
                if (SolicitudPagoFase.SolicitudPagoFaseAmortizacion.Count() > 0)
                    CreateEditSolicitudPagoSolicitudPagoAmortizacion(SolicitudPagoFase.SolicitudPagoFaseAmortizacion, pUsuarioCreacion);

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
                if (SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioProyecto.Count() > 0)
                    CreateEditSolicitudPagoFaseCriterioProyecto(SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioProyecto, strUsuarioCreacion);

                if (SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioId > 0)
                {
                    SolicitudPagoFaseCriterio SolicitudPagoFaseCriterioOld = _context.SolicitudPagoFaseCriterio.Find(SolicitudPagoFaseCriterio.SolicitudPagoFaseCriterioId);

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
            if (
                pSolicitudPago.SolicitudPagoCargarFormaPago.Count() == 0
                || pSolicitudPago.SolicitudPagoSoporteSolicitud.Count() == 0
                || pSolicitudPago.SolicitudPagoRegistrarSolicitudPago.Count() == 0)
                return false;

            foreach (var SolicitudPagoCargarFormaPago in pSolicitudPago.SolicitudPagoCargarFormaPago)
            {
                if (!ValidateCompleteRecordSolicitudPagoCargarFormaPago(SolicitudPagoCargarFormaPago))
                    return false;
            }
            foreach (var SolicitudPagoSoporteSolicitud in pSolicitudPago.SolicitudPagoSoporteSolicitud)
            {
                if (!ValidateCompleteRecordSolicitudPagoSoporteSolicitud(SolicitudPagoSoporteSolicitud))
                    return false;
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
                || string.IsNullOrEmpty(solicitudPagoFaseCriterio.ConceptoPagoCriterio)
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

            foreach (var SolicitudPagoFase in pSolicitudPagoRegistrarSolicitudPago.SolicitudPagoFase)
            {
                if (!ValidateCompleteRecordSolicitudPagoFase(SolicitudPagoFase))
                    return false;
            }
            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoFase(SolicitudPagoFase pSolicitudPagoFase)
        {
            if (pSolicitudPagoFase.SolicitudPagoFaseFactura.Count() == 0
                || pSolicitudPagoFase.SolicitudPagoFaseAmortizacion.Count() == 0
                ) return false;

            foreach (var SolicitudPagoFaseFactura in pSolicitudPagoFase.SolicitudPagoFaseFactura)
            {
                if (!ValidateCompleteRecordSolicitudPagoFaseFactura(SolicitudPagoFaseFactura))
                    return false;
            }

            foreach (var SolicitudPagoAmortizacion in pSolicitudPagoFase.SolicitudPagoFaseAmortizacion)
            {
                if (!ValidateCompleteRecordSolicitudPagoAmortizacion(SolicitudPagoAmortizacion))
                    return false;
            }
            return true;
        }

        private bool ValidateCompleteRecordSolicitudPagoAmortizacion(SolicitudPagoFaseAmortizacion solicitudPagoAmortizacion)
        {
            if (string.IsNullOrEmpty(solicitudPagoAmortizacion.PorcentajeAmortizacion.ToString())
                  || string.IsNullOrEmpty(solicitudPagoAmortizacion.ValorAmortizacion.ToString())
                )
                return false;
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
