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
                .Where(r => !(bool)r.Eliminado)
                //.Include(r => r.ContratacionProyecto)
                //.ThenInclude(r => r.Proyecto)
                //.ThenInclude(r => r.DisponibilidadPresupuestalProyecto)
                //.ThenInclude(r => r.Proyecto) 
                .ToList();

            List<Contratista> ListContratista = _context.Contratista.ToList();

            foreach (var sesionComiteSolicitud in ListSesionComiteSolicitud)
            {
                switch (sesionComiteSolicitud.TipoSolicitudCodigo)
                {
                    case ConstanCodigoTipoSolicitud.Contratacion:
                        Contratacion contratacion = await GetContratacionByContratacionId(sesionComiteSolicitud.SolicitudId);

                        sesionComiteSolicitud.EstaTramitado = false;

                        if (!string.IsNullOrEmpty(contratacion.FechaEnvioDocumentacion.ToString()))
                        {
                            sesionComiteSolicitud.EstaTramitado = true;
                        }

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
            return ListSesionComiteSolicitud.OrderByDescending(r => r.SesionComiteSolicitudId).ToList();

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

        public async Task<Respuesta> RegistrarTramiteContrato(Contrato pContrato, string pPatchfile)
        {

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Registrar_Tramite_Contrato, (int)EnumeratorTipoDominio.Acciones);

            Contrato contratoOld = _context.Contrato.Find(pContrato.ContratoId);

            contratoOld.NumeroContrato = pContrato.NumeroContrato;
            contratoOld.FechaFirmaContratista = pContrato.FechaFirmaContratista;
            contratoOld.Observaciones = pContrato.Observaciones;
            contratoOld.UsuarioModificacion = pContrato.UsuarioModificacion;
            contratoOld.FechaModificacion = pContrato.FechaModificacion;

            string strFilePatch = "";
            //Save Files  


            if (pContrato.pFile != null && pContrato.pFile.Length > 0)
            {
                await _documentService.SaveFileContratacion(pContrato.pFile, strFilePatch, pContrato.pFile.FileName);
                contratoOld.RutaDocumento = Path.Combine(pPatchfile, contratoOld.ContratoId.ToString(), pContrato.pFile.FileName);
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
                            Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccion, contratoOld.UsuarioCreacion, "REGISTRAR TRAMITE CONTRATO")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccion, contratoOld.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }
    }
}
