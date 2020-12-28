﻿using asivamosffie.model.APIModels;
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

        public async Task<List<SesionComiteSolicitud>> GetListSesionComiteSolicitud()
        {
            List<SesionComiteSolicitud> ListSesionComiteSolicitud = await _context.SesionComiteSolicitud
                .Where(r => !(bool)r.Eliminado 
                   &&
                  ((r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion)
                   || r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Modificacion_Contractual
                   )
                ).ToListAsync();

            List<Dominio> ListasParametricas = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud).ToList();

            foreach (var sesionComiteSolicitud in ListSesionComiteSolicitud)
            {
                try
                {
                    switch (sesionComiteSolicitud.TipoSolicitudCodigo)
                    {
                        case ConstanCodigoTipoSolicitud.Contratacion:

                            Contratacion contratacion =
                                _context.Contratacion
                                .Where(r => r.ContratacionId == sesionComiteSolicitud.SolicitudId).Include(r => r.Contrato).FirstOrDefault();

                            if (contratacion == null)
                                break;

                            if (contratacion.Contrato.Count() > 0)
                            {
                                if (!string.IsNullOrEmpty(contratacion.Contrato.FirstOrDefault().NumeroContrato))
                                {
                                    sesionComiteSolicitud.EstaTramitado = true;
                                }
                                else
                                {
                                    sesionComiteSolicitud.EstaTramitado = false;
                                }
                            }
                            else
                            {
                                sesionComiteSolicitud.EstaTramitado = false;
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

                        case ConstanCodigoTipoSolicitud.Modificacion_Contractual:

                            // sesionComiteSolicitud.TipoSolicitud = ListasParametricas
                            //.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud
                            // && r.Codigo == ConstanCodigoTipoSolicitud.Modificacion_Contractual)
                            //.FirstOrDefault().Nombre;
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
            return ListSesionComiteSolicitud.OrderByDescending(r => r.SesionComiteSolicitudId).Distinct().ToList();

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
                            contratacion.Contratista.TipoIdentificacionCodigo = LisParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Documento && r.Codigo == contratacion.Contratista.TipoIdentificacionCodigo).FirstOrDefault().Nombre;
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
                return contratacion;
            }
            catch (Exception ex)
            {
                return new Contratacion();
            }
        }

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
                //if (!string.IsNullOrEmpty(pContrato.RutaDocumento))
                //    contratoOld.RutaDocumento = pContrato.RutaDocumento;
                 
                contratacionOld.FechaTramite = DateTime.Now;


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

                //Enviar Notificaciones
                contratoOld.Estado = ValidarRegistroCompletoContrato(contratoOld);

                //Cambio pedido por yuly que se envia cuando
                if (pEstadoCodigo == ConstanCodigoEstadoSolicitudContratacion.Registrados)
                    await EnviarNotificaciones(contratoOld, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);


                //Save Files  
                if (pContrato.pFile != null && pContrato.pFile.Length > 0)
                {
                    string pFilePath = Path.Combine(pPatchfile, pContrato.ContratoId.ToString());
                    if (await _documentService.SaveFileContratacion(pContrato.pFile, pFilePath, pContrato.pFile.FileName))
                    {
                        contratoOld.RutaDocumento = Path.Combine(pFilePath, pContrato.pFile.FileName);
                        contratoOld.FechaTramite = DateTime.Now;


                    }
                }

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

                if (pContrato.pFile != null && pContrato.pFile.Length > 0)
                {
                    string pFilePath = Path.Combine(pPatchfile, pContrato.ContratoId.ToString());
                    if (await _documentService.SaveFileContratacion(pContrato.pFile, pFilePath, pContrato.pFile.FileName))
                        pContrato.RutaDocumento = Path.Combine(pFilePath, pContrato.pFile.FileName);
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

        public async Task<bool> EnviarNotificaciones(Contrato pContrato, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.NotificacionContratacion341);
            DateTime? FechaFirmaFiduciaria = null;

            try
            {
                FechaFirmaFiduciaria = _context.SesionComiteSolicitud.Where(r => r.SolicitudId == pContrato.Contratacion.ContratacionId && r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion).Select(r => r.ComiteTecnicoFiduciario.FechaOrdenDia).FirstOrDefault();

            }
            catch (Exception)
            { 
            }
            
            var emails = _context.UsuarioPerfil
                .Where(x => (x.PerfilId == (int)EnumeratorPerfil.Juridica
                    || x.PerfilId == (int)EnumeratorPerfil.Tecnica) && x.Activo)
                .Select(x => x.Usuario.Email)
                .ToList();
            bool blEnvioCorreo = false;

            foreach (var email in emails)
            {
                string template = TemplateRecoveryPassword.Contenido
                            .Replace("_LinkF_", pDominioFront)
                            .Replace("[TIPO_CONTRATO]", pContrato.Contratacion.NumeroSolicitud)
                            .Replace("[NUMERO_CONTRATO]", pContrato.NumeroContrato)
                            .Replace("[FECHA_FIRMA_CONTRATO]", FechaFirmaFiduciaria.HasValue ? ((DateTime)FechaFirmaFiduciaria).ToString("dd-MMMM-yy") : " ")
                            .Replace("[Observaciones]", pContrato.Observaciones);
                blEnvioCorreo = Helpers.Helpers.EnviarCorreo(email, "Solicitud de contratación en trámite en la Fiduciaria", template, pSender, pPassword, pMailServer, pMailPort);

            }
            return blEnvioCorreo;
        }

        private bool ValidarRegistroCompletoContrato(Contrato contratoOld)
        {
            if (
               string.IsNullOrEmpty(contratoOld.FechaTramite.ToString())
               || string.IsNullOrEmpty(contratoOld.TipoContratoCodigo)
               || string.IsNullOrEmpty(contratoOld.NumeroContrato)
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
    }
}
