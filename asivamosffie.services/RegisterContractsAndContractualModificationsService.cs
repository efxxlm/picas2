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
    public class RegisterContractsAndContractualModificationsService : IRegisterContractsAndContractualModificationsService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;

        public RegisterContractsAndContractualModificationsService(IDocumentService documentService, devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _documentService = documentService;
            _commonService = commonService;
            _context = context;
        }

        #region Get
        public async Task<List<SesionComiteSolicitud>> GetListSesionComiteSolicitud()
        {

            List<VListaContratacionModificacionContractual> ListVListaContratacionModificacionContractual = _context.VListaContratacionModificacionContractual.ToList();

            List<SesionComiteSolicitud> ListSesionComiteSolicitud = 
                                                                await _context.SesionComiteSolicitud.Where(r => ListVListaContratacionModificacionContractual
                                                                                                    .Select(r=> r.SesionComiteSolicitudId)
                                                                                                               .Contains(r.SesionComiteSolicitudId))
                                                                                                    .ToListAsync();


            List<Dominio> ListasParametricas = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud).ToList();

            List<Contratacion> ListContratacionSesion =
                                                         _context.Contratacion
                                                         .Include(r => r.Contrato)
                                                         .Include(r => r.DisponibilidadPresupuestal)
                                                         .Where(r =>
                                                         //Traer unicamente las contrataciones que se utilizan en la lista de sesion comite solicitud
                                                         ListVListaContratacionModificacionContractual.Select(r => r.SolicitudId).Contains(r.ContratacionId))
                                                         .ToList();


            foreach (var sesionComiteSolicitud in ListSesionComiteSolicitud)
            {
                try
                {
                    switch (sesionComiteSolicitud.TipoSolicitudCodigo)
                    {
                        case ConstanCodigoTipoSolicitud.Contratacion:

                            Contratacion contratacion = ListContratacionSesion.Where(r => r.ContratacionId == sesionComiteSolicitud.SolicitudId && r.DisponibilidadPresupuestal.Count() > 0)
                                                                              .FirstOrDefault();

                            if(contratacion== null)
                            {
                                 contratacion = _context.Contratacion
                                                             .Include(r => r.Contrato)
                                                             .Include(r => r.DisponibilidadPresupuestal)
                                                             .Where(r => r.ContratacionId == sesionComiteSolicitud.SolicitudId).FirstOrDefault();
                            }




                            if (contratacion == null)
                                break;

                            if (contratacion.Contrato.Count() > 0)
                            {
                                if (!string.IsNullOrEmpty(contratacion.Contrato.FirstOrDefault().NumeroContrato))
                                    sesionComiteSolicitud.EstaTramitado = true;
                                else
                                    sesionComiteSolicitud.EstaTramitado = false;
                            }
                            else
                                sesionComiteSolicitud.EstaTramitado = false;

                            if (contratacion.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.Cancelado_por_generacion_presupuestal)
                            {
                                if (contratacion.DisponibilidadPresupuestal.Count() > 0)
                                    if (contratacion.DisponibilidadPresupuestal.Where(r => r.Eliminado != true).FirstOrDefault() != null)
                                    {
                                        if (string.IsNullOrEmpty(contratacion.DisponibilidadPresupuestal.Where(r => r.Eliminado != true).FirstOrDefault().NumeroDrp))
                                        {
                                            sesionComiteSolicitud.Eliminado = true;
                                            break;
                                        }
                                    }
                            }

                            sesionComiteSolicitud.Contratacion = contratacion;

                            sesionComiteSolicitud.FechaSolicitud = (DateTime)contratacion.FechaCreacion;

                            sesionComiteSolicitud.NumeroSolicitud = contratacion.NumeroSolicitud;

                            sesionComiteSolicitud.TipoSolicitud = ListasParametricas
                                .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud
                                && r.Codigo == ConstanCodigoTipoSolicitud.Contratacion
                                ).FirstOrDefault().Nombre;

                            if (contratacion.RegistroCompleto == null || !(bool)contratacion.RegistroCompleto)
                            {
                                sesionComiteSolicitud.EstadoRegistro = false;
                                sesionComiteSolicitud.EstadoDelRegistro = "Incompleto";
                            }
                            else
                            {
                                sesionComiteSolicitud.EstadoRegistro = true;
                                sesionComiteSolicitud.EstadoDelRegistro = "Completo";
                            }
                            sesionComiteSolicitud.EstadoCodigo = contratacion.EstadoSolicitudCodigo;
                            break;

                        case ConstanCodigoTipoSolicitud.Novedad_Contractual:

                            NovedadContractual novedadContractual =
                                _context.NovedadContractual
                                .Include(r => r.Contrato)
                                    .ThenInclude(r => r.Contratacion)
                                .Where(r => r.NovedadContractualId == sesionComiteSolicitud.SolicitudId)
                                .FirstOrDefault();

                            if (novedadContractual == null)
                                break;

                            if (novedadContractual.Contrato != null)
                            {
                                if (!string.IsNullOrEmpty(novedadContractual.NumeroOtroSi))
                                    sesionComiteSolicitud.EstaTramitado = true;
                                else
                                    sesionComiteSolicitud.EstaTramitado = false;
                            }
                            else
                                sesionComiteSolicitud.EstaTramitado = false;


                            sesionComiteSolicitud.Contratacion = novedadContractual.Contrato.Contratacion;

                            sesionComiteSolicitud.FechaSolicitud = (DateTime)novedadContractual.FechaCreacion;

                            sesionComiteSolicitud.NumeroSolicitud = novedadContractual.NumeroSolicitud;

                            sesionComiteSolicitud.TipoSolicitud = ListasParametricas
                                .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud
                                && r.Codigo == ConstanCodigoTipoSolicitud.Novedad_Contractual
                                ).FirstOrDefault().Nombre;

                            if (novedadContractual.RegistroCompletoTramite == null || !(bool)novedadContractual.RegistroCompletoTramite)
                            {
                                sesionComiteSolicitud.EstadoRegistro = false;
                                sesionComiteSolicitud.EstadoDelRegistro = "Incompleto";
                            }
                            else
                            {
                                sesionComiteSolicitud.EstadoRegistro = true;
                                sesionComiteSolicitud.EstadoDelRegistro = "Completo";
                            }
                            if (novedadContractual.EstadoCodigo == ConstanCodigoEstadoNovedadContractual.Novedad_Cancelada)
                            {
                                NovedadContractualRegistroPresupuestal nvr = _context.NovedadContractualRegistroPresupuestal.Where(r => r.NovedadContractualId == novedadContractual.NovedadContractualId && r.Eliminado != true && r.EstadoSolicitudCodigo == "7").FirstOrDefault();
                                if (nvr != null)
                                    if (string.IsNullOrEmpty(nvr.NumeroDrp))
                                    {
                                        sesionComiteSolicitud.Eliminado = true;
                                        break;
                                    }
                            }
                            sesionComiteSolicitud.EstadoCodigo = novedadContractual.EstadoCodigo;

                            break;

                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.InnerException.ToString();

                }

            }

            ListSesionComiteSolicitud = ListSesionComiteSolicitud.Where(r => r.Eliminado != true).OrderByDescending(r => r.SesionComiteSolicitudId).Distinct().ToList();

            List<Proyecto> proyectosExpensas = _context.Proyecto.Where(r => r.TipoIntervencionCodigo == ConstantCodigoTipoIntervencion.Expensas && r.Eliminado != true).Include(r => r.ContratacionProyecto).ThenInclude(r => r.Contratacion).ToList();
           

            List<Contratacion> ListContratacion =   _context.Contratacion.Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion)
                          .Include(r => r.Contrato)
                          .Include(r => r.DisponibilidadPresupuestal)
                          .ToList();


            foreach (var proyecto in proyectosExpensas)
            {
                if (proyecto.ContratacionProyecto != null)
                {
                    foreach (var cp in proyecto.ContratacionProyecto)
                    {
                        Contratacion contratacion = ListContratacion.Where(r => r.ContratacionId == cp.ContratacionId) 
                                                                    .FirstOrDefault();

                        if (contratacion != null)
                        {
                            DisponibilidadPresupuestal dp = contratacion.DisponibilidadPresupuestal.Where(r => r.Eliminado != true).FirstOrDefault();
                            bool EstaTramitado = false;
                            string TipoSolicitud = string.Empty;
                            bool EstadoRegistro = false;
                            string EstadoDelRegistro = string.Empty;

                            if (dp != null)
                            {
                                if (dp.EstadoSolicitudCodigo == "5")
                                {
                                    if (contratacion.Contrato.Count() > 0)
                                    {
                                        if (!string.IsNullOrEmpty(contratacion.Contrato.FirstOrDefault().NumeroContrato))
                                            EstaTramitado = true;
                                        else
                                            EstaTramitado = false;
                                    }
                                    else
                                        EstaTramitado = false;

                                    TipoSolicitud = ListasParametricas
                                                    .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud
                                                    && r.Codigo == ConstanCodigoTipoSolicitud.Contratacion
                                                    ).FirstOrDefault().Nombre;

                                    if (contratacion.RegistroCompleto == null || !(bool)contratacion.RegistroCompleto)
                                    {
                                        EstadoRegistro = false;
                                        EstadoDelRegistro = "Incompleto";
                                    }
                                    else
                                    {
                                        EstadoRegistro = true;
                                        EstadoDelRegistro = "Completo";
                                    }
                                    ListSesionComiteSolicitud.Add(new SesionComiteSolicitud
                                    {
                                        SesionComiteSolicitudId = 0,
                                        TipoSolicitudCodigo = ConstanCodigoTipoSolicitud.Contratacion,
                                        SolicitudId = contratacion.ContratacionId,
                                        FechaCreacion = contratacion.FechaCreacion ?? DateTime.Now,
                                        UsuarioCreacion = contratacion.UsuarioCreacion,
                                        Eliminado = false,
                                        EstaTramitado = EstaTramitado,
                                        Contratacion = contratacion,
                                        FechaSolicitud = (DateTime)contratacion.FechaCreacion,
                                        NumeroSolicitud = contratacion.NumeroSolicitud,
                                        TipoSolicitud = TipoSolicitud,
                                        EstadoRegistro = EstadoRegistro,
                                        EstadoCodigo = contratacion.EstadoSolicitudCodigo,
                                        EstadoDelRegistro = EstadoDelRegistro
                                    });
                                }
                            }

                        }
                    }
                }
            }

            return ListSesionComiteSolicitud;

        }

        public async Task<Contratacion> GetContratacionByContratacionId(int pContratacionId)
        {
            try
            {
                List<Dominio> LisParametricas = _context.Dominio
                    .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_Por_Contratar
                         || r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Documento
                ).ToList();

                Contratacion contratacion = await _context.Contratacion
                    .Where(r => r.ContratacionId == pContratacionId)
                          .Include(r => r.DisponibilidadPresupuestal)
                          .Include(r => r.Contratista)
                          .Include(r => r.PlazoContratacion)
                          .Include(r => r.Contrato).FirstOrDefaultAsync();

                contratacion.FechaTramite = DateTime.Now;

                contratacion.sesionComiteSolicitud = _context.SesionComiteSolicitud
                    .Where(r => r.SolicitudId == contratacion.ContratacionId && r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion)
                    .Include(r => r.ComiteTecnico)
                    .Include(r => r.ComiteTecnicoFiduciario)
                    .ToList();

                if (contratacion.Contratista != null)
                {
                    if (!string.IsNullOrEmpty(contratacion.Contratista.TipoIdentificacionCodigo))
                    {
                        bool allDigits = contratacion.Contratista.TipoIdentificacionCodigo.All(char.IsDigit);
                        if (allDigits)
                        {
                            contratacion.Contratista.TipoIdentificacionCodigo = LisParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Documento && r.Codigo == contratacion.Contratista.TipoIdentificacionCodigo).FirstOrDefault().Codigo;
                        }
                    }
                }
                foreach (var Contrato in contratacion.Contrato)
                {

                    if (!string.IsNullOrEmpty(Contrato.TipoContratoCodigo))
                    {
                        Contrato.FechaTramite = DateTime.Now;
                        Contrato.TipoContratoCodigo = LisParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_Por_Contratar).FirstOrDefault().Nombre;
                    }
                }

                _context.SaveChanges();


                if (contratacion.Contratista != null)
                {
                    if (!string.IsNullOrEmpty(contratacion.Contratista.TipoIdentificacionCodigo))
                    {
                        contratacion.Contratista.TipoIdentificacionCodigo = LisParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Documento && r.Codigo == contratacion.Contratista.TipoIdentificacionCodigo).FirstOrDefault().Nombre;
                    }
                }
                return contratacion;
            }
            catch (Exception ex)
            {
                return new Contratacion();
            }
        }

        public async Task<List<VListaContratacionModificacionContractual>> GetListSesionComiteSolicitudV2()
        {
            return await _context.VListaContratacionModificacionContractual.OrderByDescending(v => v.SesionComiteSolicitudId).ToListAsync();
        }

        #endregion

        public async Task<Respuesta> RegistrarTramiteContrato(Contrato pContrato, string pPatchfile, string pEstadoCodigo, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Registrar_Tramite_Contrato, (int)EnumeratorTipoDominio.Acciones);
            //Contrato Modificar
            if (pContrato.ContratoId > 0)
            {
                Contrato contratoOld = _context.Contrato.Where(r => r.ContratoId == pContrato.ContratoId)
                    .Include(r => r.Contratacion)
                    .FirstOrDefault();

                //contratacion
                Contratacion contratacionOld = _context.Contratacion.Find(contratoOld.ContratacionId);

                if (!contratacionOld.FechaTramite.HasValue)
                    contratacionOld.FechaTramite = DateTime.Now;

                contratacionOld.EstadoSolicitudCodigo = pEstadoCodigo;
                contratacionOld.UsuarioModificacion = pContrato.UsuarioModificacion;
                contratacionOld.FechaModificacion = pContrato.FechaModificacion;

                //Contrato  
                if (!string.IsNullOrEmpty(pContrato.RutaDocumento))
                    contratoOld.RutaDocumento = pContrato.RutaDocumento;

                contratacionOld.FechaTramite = DateTime.Now;

                //Cambiando por enésima vez 
                if (!string.IsNullOrEmpty(pContrato.ModalidadCodigo))
                    contratoOld.ModalidadCodigo = pContrato.ModalidadCodigo;

                if (!string.IsNullOrEmpty(pContrato.NumeroContrato))
                    contratoOld.NumeroContrato = pContrato.NumeroContrato;

                //Fecha envio para la firma contratista
                if (!string.IsNullOrEmpty(pContrato.FechaEnvioFirma.ToString()))
                {
                    if (((DateTime)pContrato.FechaEnvioFirma).Year > 2000)
                        contratoOld.FechaEnvioFirma = pContrato.FechaEnvioFirma;
                }

                //Fecha envio por parte del contratista 
                if (!string.IsNullOrEmpty(pContrato.FechaFirmaContratista.ToString()))
                {
                    if (((DateTime)pContrato.FechaFirmaContratista).Year > 2000)
                        contratoOld.FechaFirmaContratista = pContrato.FechaFirmaContratista;
                }

                //Fecha de envio para la firma de la fiduciaria
                if (!string.IsNullOrEmpty(pContrato.FechaFirmaFiduciaria.ToString()))
                {
                    if (((DateTime)pContrato.FechaFirmaFiduciaria).Year > 2000)
                        contratoOld.FechaFirmaFiduciaria = pContrato.FechaFirmaFiduciaria;

                }
                //Fecha de Firma por parte de la fiduciaria
                if (!string.IsNullOrEmpty(pContrato.FechaFirmaContrato.ToString()))
                {
                    if (((DateTime)pContrato.FechaFirmaContrato).Year > 2000)
                        contratoOld.FechaFirmaContrato = pContrato.FechaFirmaContrato;
                }

                // Fecha Tramite
                if (!string.IsNullOrEmpty(pContrato.FechaTramite.ToString()))
                {
                    if (((DateTime)pContrato.FechaTramite).Year > 2000)
                        contratoOld.FechaTramite = pContrato.FechaTramite;
                }

                if (!string.IsNullOrEmpty(pContrato.Observaciones))
                    contratoOld.Observaciones = pContrato.Observaciones;
                //Save Files  
                if (pContrato.PFile != null && pContrato.PFile.Length > 0)
                {
                    string pFilePath = Path.Combine(pPatchfile, pContrato.ContratoId.ToString());
                    if (await _documentService.SaveFileContratacion(pContrato.PFile, pFilePath, pContrato.PFile.FileName))
                    {
                        contratoOld.RutaDocumento = Path.Combine(pFilePath, pContrato.PFile.FileName);
                        contratoOld.FechaTramite = DateTime.Now;
                    }
                }

                contratoOld.Estado = ValidarRegistroCompletoContrato(contratoOld);
                //Enviar Notificaciones
                _context.Set<Contrato>().Where(c => c.ContratoId == contratoOld.ContratoId)
                                        .Update(c => new Contrato
                                        {
                                            Estado = contratoOld.Estado
                                        });


                //Cambio pedido por yuly que se envia cuando se envia a registrados
                if (pEstadoCodigo == ConstanCodigoEstadoSolicitudContratacion.Registrados)
                    await EnviarNotificaciones(contratoOld, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);



            }
            //Contrato Nuevo
            else
            {
                pContrato.FechaTramite = DateTime.Now;
                pContrato.FechaCreacion = DateTime.Now;
                pContrato.Eliminado = false;
                pContrato.EstadoVerificacionCodigo = ConstanCodigoEstadoVerificacionContrato.Sin_aprobacion_de_requisitos_tecnicos;
                _context.Contrato.Add(pContrato);
                _context.SaveChanges();

                if (pContrato.PFile != null && pContrato.PFile.Length > 0)
                {
                    string pFilePath = Path.Combine(pPatchfile, pContrato.ContratoId.ToString());
                    if (await _documentService.SaveFileContratacion(pContrato.PFile, pFilePath, pContrato.PFile.FileName))
                        pContrato.RutaDocumento = Path.Combine(pFilePath, pContrato.PFile.FileName);
                }
            }

            //Cambiar estado contratacion
            Contratacion contratacion = _context.Contratacion.Find(pContrato.ContratacionId);

            contratacion.EstadoSolicitudCodigo = pEstadoCodigo;
            contratacion.UsuarioModificacion = pContrato.UsuarioModificacion;
            contratacion.FechaModificacion = pContrato.FechaModificacion;
            contratacion.FechaTramite = DateTime.Now;

            _context.SaveChanges();


            try
            {
                return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantSesionComiteTecnico.OperacionExitosa,
                            Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_contratos_modificaciones_contractuales, ConstantSesionComiteTecnico.OperacionExitosa, idAccion, pContrato.UsuarioCreacion, "REGISTRAR TRAMITE CONTRATO")
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
                        Code = ConstantSesionComiteTecnico.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_contratos_modificaciones_contractuales, ConstantSesionComiteTecnico.Error, idAccion, pContrato.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        private bool ValidarRegistroCompletoContrato(Contrato contratoOld)
        {
            if (
               string.IsNullOrEmpty(contratoOld.FechaTramite.ToString())
               || string.IsNullOrEmpty(contratoOld.TipoContratoCodigo)
               || string.IsNullOrEmpty(contratoOld.NumeroContrato)
               || string.IsNullOrEmpty(contratoOld.ModalidadCodigo)
               || string.IsNullOrEmpty(contratoOld.EstadoDocumentoCodigo)
               || string.IsNullOrEmpty(contratoOld.FechaEnvioFirma.ToString())
               || string.IsNullOrEmpty(contratoOld.FechaFirmaContratista.ToString())
               || string.IsNullOrEmpty(contratoOld.FechaFirmaFiduciaria.ToString())
               || string.IsNullOrEmpty(contratoOld.FechaFirmaContrato.ToString())
               || string.IsNullOrEmpty(contratoOld.Observaciones)
               || string.IsNullOrEmpty(contratoOld.RutaDocumento)
               //|| string.IsNullOrEmpty(contratoOld.Objeto)
               || string.IsNullOrEmpty(contratoOld.Valor.ToString())
                //|| string.IsNullOrEmpty(contratoOld.Plazo.ToString())
                || string.IsNullOrEmpty(contratoOld.CantidadPerfiles.ToString())
                || string.IsNullOrEmpty(contratoOld.EstadoVerificacionCodigo.ToString())
                //|| string.IsNullOrEmpty(contratoOld.EstadoFase1Diagnostico.ToString())
                || string.IsNullOrEmpty(contratoOld.EstadoActa.ToString())

                ) { return false; }

            return true;
        }

        public async Task<Respuesta> RegistrarTramiteNovedadContractual(NovedadContractual pNovedadContractual)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Registrar_Tramite_Novedad_Contractual, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                _context.Set<NovedadContractual>()
                        .Where(n => n.NovedadContractualId == pNovedadContractual.NovedadContractualId)
                        .Update(n =>
                        new NovedadContractual
                        {
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pNovedadContractual.UsuarioCreacion,
                            NumeroOtroSi = pNovedadContractual.NumeroOtroSi,
                            FechaEnvioFirmaContratista = pNovedadContractual.FechaEnvioFirmaContratista,
                            FechaFirmaContratista = pNovedadContractual.FechaFirmaContratista,
                            FechaEnvioFirmaFiduciaria = pNovedadContractual.FechaEnvioFirmaFiduciaria,
                            FechaFirmaFiduciaria = pNovedadContractual.FechaFirmaFiduciaria,
                            ObservacionesTramite = pNovedadContractual.ObservacionesTramite,
                            UrlDocumentoSuscrita = pNovedadContractual.UrlDocumentoSuscrita,
                            EstadoCodigo = !string.IsNullOrEmpty(pNovedadContractual.NumeroOtroSi) && pNovedadContractual.FechaEnvioFirmaContratista.HasValue ? ConstanCodigoEstadoNovedadContractual.Firmado : ConstanCodigoEstadoNovedadContractual.Enviadas_a_la_Fiduciaria,
                            RegistroCompletoTramite = ValidarRegistroCompletoTramiteNovedad(pNovedadContractual)
                        });


                return
                  new Respuesta
                  {
                      IsSuccessful = true,
                      IsException = false,
                      IsValidation = false,
                      Code = ConstantGestionarProcesosContractuales.OperacionExitosa,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_Procesos_Contractuales, ConstantGestionarProcesosContractuales.OperacionExitosa, idAccion, pNovedadContractual.UsuarioCreacion, "REGISTRAR SOLICITUD")
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
                  Code = ConstantGestionarProcesosContractuales.Error,
                  Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_Procesos_Contractuales, ConstantGestionarProcesosContractuales.Error, idAccion, pNovedadContractual.UsuarioCreacion, ex.InnerException.ToString())
              };
            }
        }

        private bool ValidarRegistroCompletoTramiteNovedad(NovedadContractual pNovedadContractual)
        {
            if (string.IsNullOrEmpty(pNovedadContractual.NumeroOtroSi)
                || !pNovedadContractual.FechaEnvioFirmaContratista.HasValue
                || !pNovedadContractual.FechaFirmaContratista.HasValue
                || !pNovedadContractual.FechaEnvioFirmaFiduciaria.HasValue
                || !pNovedadContractual.FechaFirmaFiduciaria.HasValue
                || string.IsNullOrEmpty(pNovedadContractual.ObservacionesTramite)
                || string.IsNullOrEmpty(pNovedadContractual.UrlDocumentoSuscrita)
                ) return false;
            return true;
        }

        public async Task<bool> EnviarNotificaciones(Contrato pContrato, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.NotificacionContratacion341);
            DateTime? FechaFirmaFiduciaria = _context.SesionComiteSolicitud.Where(r => r.SolicitudId == pContrato.Contratacion.ContratacionId && r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion).Select(r => r.ComiteTecnicoFiduciario.FechaOrdenDia).FirstOrDefault();

            List<EnumeratorPerfil> emails = new List<EnumeratorPerfil>
            {
                EnumeratorPerfil.Juridica,
                EnumeratorPerfil.Tecnica
            };

            bool blEnvioCorreo = false;
            string template = TemplateRecoveryPassword.Contenido
                        .Replace("_LinkF_", pDominioFront)
                        .Replace("[TIPO_CONTRATO]", pContrato?.Contratacion?.NumeroSolicitud)
                        .Replace("[NUMERO_CONTRATO]", pContrato.NumeroContrato)
                        .Replace("[FECHA_FIRMA_CONTRATO]", FechaFirmaFiduciaria.HasValue ? ((DateTime)FechaFirmaFiduciaria).ToString("dd-MM-yy") : " ")
                        .Replace("[Observaciones]", pContrato.Observaciones);

            _commonService.EnviarCorreo(emails, template, TemplateRecoveryPassword.Asunto);
            return blEnvioCorreo;
        }

        public async Task<Respuesta> ChangeStateTramiteNovedad(int pNovedadContractualId, string user)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Tramitar_Novedad_Contractual, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                _context.Set<NovedadContractual>()
                        .Where(n => n.NovedadContractualId == pNovedadContractualId)
                        .Update(n =>
                        new NovedadContractual
                        {
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = user,
                            EstadoCodigo = ConstanCodigoEstadoNovedadContractual.Registrado
                        });

                return
                  new Respuesta
                  {
                      IsSuccessful = true,
                      IsException = false,
                      IsValidation = false,
                      Code = ConstantGestionarProcesosContractuales.OperacionExitosa,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_Procesos_Contractuales, ConstantGestionarProcesosContractuales.OperacionExitosa, idAccion, user, "REGISTRAR SOLICITUD")
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
                  Code = ConstantGestionarProcesosContractuales.Error,
                  Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_Procesos_Contractuales, ConstantGestionarProcesosContractuales.Error, idAccion, user, ex.InnerException.ToString())
              };
            }
        }

    }
}
