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
            // Estado de la sesionComiteSolicitud
            //• Sin registro
            //• En proceso de firmas
            //• Registrados 

            string CodigoContratacion = ConstanCodigoTipoSolicitud.Contratacion;
            List<SesionComiteSolicitud> ListSesionComiteSolicitud = await _context.SesionComiteSolicitud
                .Where(r => !(bool)r.Eliminado && r.TipoSolicitudCodigo == CodigoContratacion
                ).ToListAsync();


            ListSesionComiteSolicitud = ListSesionComiteSolicitud.Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion
            || r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Modificacion_Contractual).ToList();
            List<Dominio> ListasParametricas = _context.Dominio.ToList();

            List<Contratacion> ListContratacion = _context.Contratacion
                .Where(r => !(bool)r.Eliminado).Include(r => r.Contrato)

                .ToList();

            List<Contratista> ListContratista = _context.Contratista.ToList();

            foreach (var sesionComiteSolicitud in ListSesionComiteSolicitud)
            {
                switch (sesionComiteSolicitud.TipoSolicitudCodigo)
                {
                    case ConstanCodigoTipoSolicitud.Contratacion:
                        Contratacion contratacion = await GetContratacionByContratacionId(sesionComiteSolicitud.SolicitudId);


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

                        sesionComiteSolicitud.FechaSolicitud = (DateTime)contratacion.FechaTramite;

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

                        break;

                    case ConstanCodigoTipoSolicitud.Modificacion_Contractual:

                        sesionComiteSolicitud.TipoSolicitud = ListasParametricas
                       .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud
                        && r.Codigo == ConstanCodigoTipoSolicitud.Modificacion_Contractual)
                       .FirstOrDefault().Nombre;
                        break;


                    default:
                        break;
                }
            }
            return ListSesionComiteSolicitud.OrderByDescending(r => r.SesionComiteSolicitudId).Distinct().ToList();

        }

        public async Task<Contratacion> GetContratacionByContratacionId(int pContratacionId)
        {
            try
            {
                List<Dominio> LisParametricas = _context.Dominio.ToList();

                Contratacion contratacion = await _context.Contratacion
                    .Where(r => r.ContratacionId == pContratacionId)
                          .Include(r => r.DisponibilidadPresupuestal)
                          .Include(r => r.Contratista)
                          .Include(r => r.Contrato).FirstOrDefaultAsync();

                contratacion.sesionComiteSolicitud = _context.SesionComiteSolicitud
                    .Where(r => r.SolicitudId == contratacion.ContratacionId && r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion)
                    .Include(r => r.ComiteTecnico).ToList();


                if (!string.IsNullOrEmpty(contratacion.Contratista.TipoIdentificacionCodigo))
                {
                    bool allDigits = contratacion.Contratista.TipoIdentificacionCodigo.All(char.IsDigit);
                    if (allDigits)
                    {
                        contratacion.Contratista.TipoIdentificacionCodigo = LisParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Documento && r.Codigo == contratacion.Contratista.TipoIdentificacionCodigo).FirstOrDefault().Nombre;
                    }
                }
                foreach (var Contrato in contratacion.Contrato)
                {

                    if (!string.IsNullOrEmpty(Contrato.TipoContratoCodigo))
                    {
                        Contrato.TipoContratoCodigo = LisParametricas.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_Por_Contratar).FirstOrDefault().Nombre;
                    }
                }

                return contratacion;
            }
            catch (Exception)
            {
                return new Contratacion();
            }
        }

        public async Task<Respuesta> RegistrarTramiteContrato(Contrato pContrato, string pPatchfile, string pEstadoCodigo)
        {

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Registrar_Tramite_Contrato, (int)EnumeratorTipoDominio.Acciones);

            //Contrato Modificar
            if (pContrato.ContratoId > 0)
            {
                Contrato contratoOld = _context.Contrato.Where(r => r.ContratoId == pContrato.ContratoId).FirstOrDefault();
                //contratacion
                Contratacion contratacionOld = _context.Contratacion.Find(contratoOld.ContratacionId);

                contratacionOld.EstadoSolicitudCodigo = pEstadoCodigo;
                contratacionOld.UsuarioModificacion = pContrato.UsuarioModificacion;
                contratacionOld.FechaModificacion = pContrato.FechaModificacion;

                contratoOld.Estado = ValidarRegistroCompletoContrato(contratoOld);
                //Contrato 
                contratoOld.NumeroContrato = pContrato.NumeroContrato;
                //Fecha envio para la firma contratista
                if (!string.IsNullOrEmpty(pContrato.FechaEnvioFirma.ToString()))
                {
                    if (((DateTime)pContrato.FechaEnvioFirma).Year > 2000)
                    {
                        contratoOld.FechaEnvioFirma = pContrato.FechaEnvioFirma;
                    }
                }

                //Fecha envio por parte del contratista 
                if (!string.IsNullOrEmpty(pContrato.FechaFirmaContratista.ToString()))
                {
                    if (((DateTime)pContrato.FechaFirmaContratista).Year > 2000)
                    {
                        contratoOld.FechaFirmaContratista = pContrato.FechaFirmaContratista;
                    }
                }

                //Fecha de envio para la firma de la fiduciaria
                if (!string.IsNullOrEmpty(pContrato.FechaFirmaFiduciaria.ToString()))
                {
                    if (((DateTime)pContrato.FechaFirmaFiduciaria).Year > 2000)
                    {
                        contratoOld.FechaFirmaFiduciaria = pContrato.FechaFirmaFiduciaria;
                    }
                }
                //Fecha de Firma por parte de la fiduciaria
                if (!string.IsNullOrEmpty(pContrato.FechaFirmaContrato.ToString()))
                {
                    if (((DateTime)pContrato.FechaFirmaContrato).Year > 2000)
                    {
                        contratoOld.FechaFirmaContrato = pContrato.FechaFirmaContrato;
                    }
                }

                // Fecha Tramite
                if (!string.IsNullOrEmpty(pContrato.FechaTramite.ToString()))
                {
                    if (((DateTime)pContrato.FechaTramite).Year > 2000)
                    {
                        contratoOld.FechaTramite = pContrato.FechaTramite;
                    }
                }

                if (!string.IsNullOrEmpty(pContrato.Observaciones))
                {

                    contratoOld.Observaciones = pContrato.Observaciones;
                }

                if (pContrato.pFile != null)
                {
                    if (pContrato.pFile.Length > 0)
                    {
                        contratoOld.RutaDocumento = Path.Combine(pPatchfile, contratoOld.ContratoId.ToString(), pContrato.pFile.FileName);
                    }
                }
            }

            //Contrato Nuevo
            else
            {
                pContrato.FechaCreacion = DateTime.Now;
                pContrato.Eliminado = false;
                _context.Contrato.Add(pContrato);
                _context.SaveChanges();


                if (pContrato.pFile != null)
                {
                    if (pContrato.pFile.Length > 0)
                    {
                        pContrato.RutaDocumento = Path.Combine(pPatchfile, pContrato.ContratoId.ToString(), pContrato.pFile.FileName);
                    }
                }
            }
            string strFilePatch = "";
            //Save Files  
            if (pContrato.pFile != null && pContrato.pFile.Length > 0)
            {
                await _documentService.SaveFileContratacion(pContrato.pFile, strFilePatch, pContrato.pFile.FileName);
            }
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
                            Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccion, pContrato.UsuarioCreacion, "REGISTRAR TRAMITE CONTRATO")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccion, pContrato.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
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
               || string.IsNullOrEmpty(contratoOld.Objeto)
               || string.IsNullOrEmpty(contratoOld.Valor.ToString())
               || string.IsNullOrEmpty(contratoOld.Plazo.ToString())
                || string.IsNullOrEmpty(contratoOld.CantidadPerfiles.ToString())
                || string.IsNullOrEmpty(contratoOld.EstadoVerificacionCodigo.ToString())
                || string.IsNullOrEmpty(contratoOld.EstadoFase1.ToString())
                || string.IsNullOrEmpty(contratoOld.EstadoActa.ToString())

                ) { return false; }

            return true;
        }
    }
}
