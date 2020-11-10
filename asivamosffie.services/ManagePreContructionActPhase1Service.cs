using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace asivamosffie.services
{
    public class ManagePreContructionActPhase1Service : IManagePreContructionActPhase1Service
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;
        private readonly IRegisterSessionTechnicalCommitteeService _registerSessionTechnicalCommitteeService;

        public ManagePreContructionActPhase1Service(devAsiVamosFFIEContext context, ICommonService commonService, IRegisterSessionTechnicalCommitteeService registerSessionTechnicalCommitteeService)
        {
            _registerSessionTechnicalCommitteeService = registerSessionTechnicalCommitteeService;
            _context = context;
            _commonService = commonService;
        }

        public async Task<dynamic> GetListContrato()
        {
            try
            {
                List<Contrato> listContratos = await _context.Contrato
                       .Where(r => r.EstadoVerificacionCodigo == ConstanCodigoEstadoVerificacionContratoObra.Con_requisitos_del_contratista_de_obra_avalados).ToListAsync();

                List<Dominio> listEstadosActa = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Del_Acta_Contrato).ToList();

                List<dynamic> ListContratacionDynamic = new List<dynamic>();

                listContratos.ForEach(contrato =>
                {
                    ListContratacionDynamic.Add(new
                    {
                        fechaAprobacionRequisitosSupervisor = "Fecha-3.1.8",
                        contrato.NumeroContrato,
                        estadoActaContrato = !string.IsNullOrEmpty(contrato.EstadoActa) ? listEstadosActa.Where(r => r.Codigo == contrato.EstadoActa).FirstOrDefault().Nombre : " ",
                        contrato.ContratoId
                    });
                });

                return ListContratacionDynamic;
            }
            catch (Exception ex)
            {
                return new List<dynamic>();
            }

        }

        public async Task<Contrato> GetContratoByContratoId(int pContratoId)
        {
            try
            {
                Contrato contrato =
                    await _context.Contrato.Where(r => r.ContratoId == pContratoId)
                       .Include(r => r.ContratoObservacion)
                         .Include(r => r.Contratacion)
                                .ThenInclude(r => r.ContratacionProyecto)
                                       .ThenInclude(r => r.ContratacionProyectoAportante)
                                                     .ThenInclude(r => r.ComponenteAportante)
                                                       .ThenInclude(r => r.ComponenteUso)

                          .Include(r => r.Contratacion)
                            .ThenInclude(r => r.Contratista)
                         .Include(r => r.Contratacion)
                            .ThenInclude(r => r.DisponibilidadPresupuestal).FirstOrDefaultAsync();

                return contrato;
            }
            catch (Exception)
            {
                return new Contrato();
            }
        }

        public async Task<Respuesta> EditContrato(Contrato pContrato)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Editar_Acta_De_Inicio_De_Contrato, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                Contrato ContratoOld = await _context.Contrato.Where(r => r.ContratoId == pContrato.ContratoId).Include(r => r.ContratoObservacion).FirstOrDefaultAsync();

                ContratoOld.FechaActaInicioFase1 = pContrato.FechaActaInicioFase1;
                ContratoOld.FechaTerminacion = pContrato.FechaTerminacion;
                ContratoOld.PlazoFase1PreMeses = pContrato.PlazoFase1PreMeses;
                ContratoOld.PlazoFase1PreDias = pContrato.PlazoFase1PreDias;
                ContratoOld.PlazoFase2ConstruccionDias = pContrato.PlazoFase2ConstruccionDias;
                ContratoOld.PlazoFase2ConstruccionMeses = pContrato.PlazoFase2ConstruccionMeses;
                ContratoOld.ConObervacionesActa = pContrato.ConObervacionesActa;
                ContratoOld.EstadoActa = ConstanCodigoEstadoActaContrato.Con_acta_preliminar_generada;

                if ((bool)ContratoOld.ConObervacionesActa)
                {
                    foreach (var ContratoObservacion in pContrato.ContratoObservacion)
                    {
                        if (ContratoObservacion.ContratoObservacionId == 0)
                        {
                            ContratoObservacion.UsuarioCreacion = pContrato.UsuarioCreacion;
                            ContratoObservacion.FechaCreacion = DateTime.Now;
                            ContratoObservacion.EsActa = true;
                            ContratoObservacion.EsActaFase1 = true;
                            ContratoObservacion.EsActaFase2 = false;
                        }
                        else
                        {
                            ContratoObservacion contratoObservacionOld = _context.ContratoObservacion.Where(r => r.ContratoObservacionId == ContratoObservacion.ContratoObservacionId).FirstOrDefault();

                            contratoObservacionOld.UsuarioModificacion = pContrato.UsuarioCreacion;
                            contratoObservacionOld.FechaModificacion = DateTime.Now;

                            contratoObservacionOld.Observaciones = ContratoObservacion.Observaciones;
                        }
                    }
                }
                _context.SaveChanges();
                return
                     new Respuesta
                     {
                         IsSuccessful = false,
                         IsException = true,
                         IsValidation = false,
                         Code = RegisterPreContructionPhase1.OperacionExitosa,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.OperacionExitosa, idAccion, pContrato.UsuarioCreacion, "EDITAR ACTA DE INICIO DE CONTRATO FASE 1 PRECONSTRUCCION")
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
                           Code = RegisterPreContructionPhase1.Error,
                           Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.Error, idAccion, pContrato.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                       };

            }
        }

        public async Task<Respuesta> LoadActa(Contrato pContrato, IFormFile pFile, string pDirectorioBase, string pDirectorioActaContrato)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cargar_Acta_Subscrita, (int)EnumeratorTipoDominio.Acciones);

            string strFilePatch = string.Empty;
            try
            {
                if (pFile.Length > 0)
                {
                    strFilePatch = Path.Combine(pDirectorioBase, pDirectorioActaContrato, pContrato.ContratoId.ToString());
                    await _documentService.SaveFileContratacion(pFile, strFilePatch, pFile.FileName);
                }
                else
                    return new Respuesta();

                Contrato ContratoOld = await _context.Contrato.Where(r => r.ContratoId == pContrato.ContratoId).Include(r => r.ContratoObservacion).FirstOrDefaultAsync();

                ContratoOld.FechaFirmaActaContratista = pContrato.FechaActaInicioFase1;
                ContratoOld.FechaTerminacion = pContrato.FechaTerminacion;
                ContratoOld.RutaActaSuscrita = strFilePatch;

                ContratoOld.EstadoActa = ConstanCodigoEstadoActaContrato.Con_acta_suscrita_y_cargada;


                _context.SaveChanges();
                return
                     new Respuesta
                     {
                         IsSuccessful = false,
                         IsException = true,
                         IsValidation = false,
                         Code = RegisterPreContructionPhase1.OperacionExitosa,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.OperacionExitosa, idAccion, pContrato.UsuarioCreacion, "EDITAR ACTA DE INICIO DE CONTRATO FASE 1 PRECONSTRUCCION")
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
                           Code = RegisterPreContructionPhase1.Error,
                           Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.Error, idAccion, pContrato.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                       };

            }
        }

        public async Task<Respuesta> CambiarEstadoActa(int pContratoId, string pCodigoEstadoActa, string pUsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Acta_Contrato, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                Contrato contratoOld = _context.Contrato.Find(pContratoId);
                contratoOld.FechaModificacion = DateTime.Now;
                contratoOld.UsuarioModificacion = pUsuarioModificacion;
                contratoOld.EstadoActa = pCodigoEstadoActa;
                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = RegisterPreContructionPhase1.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.OperacionExitosa, idAccion, pUsuarioModificacion, "CAMBIAR ESTADO ACTA")
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
                        Code = RegisterPreContructionPhase1.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString().ToUpper())
                    };
            }
        }

        public async Task<byte[]> GetActaByIdPerfil(int PIdPerfil, int pContratoId)
        {
            return PIdPerfil switch
            {
                (int)EnumeratorPerfil.Tecnica => await ReplacePlantillaTecnica(pContratoId),
                (int)EnumeratorPerfil.Supervisor => await ReplacePlantillaSupervisor(pContratoId),
                _ => Array.Empty<byte>(),
            };
        }

        public async Task<byte[]> ReplacePlantillaSupervisor(int pContratoId)
        {
            Contrato contrato = _context.Contrato.Find(pContratoId);


            Plantilla plantilla = await _context.Plantilla
                .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Contrato_Acta_Interventoria)
                .ToString()).Include(r => r.Encabezado).FirstOrDefaultAsync();

            return _registerSessionTechnicalCommitteeService.ConvertirPDF(plantilla);
        }

        public async Task<byte[]> ReplacePlantillaTecnica(int pContratoId)
        {
            Contrato contrato = _context.Contrato.Find(pContratoId);

            Plantilla plantilla = await _context.Plantilla
         .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Contrato_Acta_Constuccion)
         .ToString()).Include(r => r.Encabezado).FirstOrDefaultAsync();

            return _registerSessionTechnicalCommitteeService.ConvertirPDF(plantilla);
        }

        public async Task<Respuesta> CreateEditObservacionesActa(ContratoObservacion pcontratoObservacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Edit_Contrato_Observacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pcontratoObservacion.ContratoObservacionId == 0)
                {
                    pcontratoObservacion.FechaCreacion = DateTime.Now;
                    _context.ContratoObservacion.Add(pcontratoObservacion);
                }
                else
                {
                    ContratoObservacion contratoObservacionOld = _context.ContratoObservacion.Find(pcontratoObservacion.ContratoObservacionId);

                    contratoObservacionOld.FechaModificacion = DateTime.Now;
                    contratoObservacionOld.UsuarioModificacion = pcontratoObservacion.UsuarioCreacion;

                    contratoObservacionOld.Observaciones = pcontratoObservacion.Observaciones;
                    contratoObservacionOld.EsActa = pcontratoObservacion.EsActa;
                    contratoObservacionOld.EsActaFase1 = pcontratoObservacion.EsActaFase1;
                    contratoObservacionOld.EsActaFase2 = pcontratoObservacion.EsActaFase2;
                }

                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = RegisterPreContructionPhase1.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.OperacionExitosa, idAccion, pcontratoObservacion.UsuarioCreacion, "CAMBIAR ESTADO ACTA")
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
                        Code = RegisterPreContructionPhase1.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.Error, idAccion, pcontratoObservacion.UsuarioCreacion, ex.InnerException.ToString().ToUpper())
                    };
            }
        }

        public async Task<List<ContratoObservacion>> GetListContratoObservacionByContratoId(int ContratoId)
        {

            return await _context.ContratoObservacion.Where(r => r.ContratoId == ContratoId).ToListAsync();
        }


        //Codigo CDaza Se deja la misma Logica Pedidar por David
        public async Task<ConstruccionObservacion> GetContratoObservacionByIdContratoId(int pContratoId, bool pEsSupervisor)
        {

            //includefilter
            //ContratoObservacion contratoObservacion = new ContratoObservacion();
            //List<ContratoObservacion> lstContratoObservacion = new List<ContratoObservacion>();
            //lstContratoObservacion=_context.ContratoObservacion.Where(r => r.ContratoId == pContratoId && r.EsActaFase2==true).ToList();
            //lstContratoObservacion = lstContratoObservacion.OrderByDescending(r => r.ContratoObservacionId).ToList();

            ////contratoPoliza = _context.ContratoPoliza.Where(r => !(bool)r.Eliminado && r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();
            //contratoObservacion = lstContratoObservacion.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

            ConstruccionObservacion contratoObservacion = new ConstruccionObservacion();
            List<ConstruccionObservacion> lstContratoObservacion = new List<ConstruccionObservacion>();

            ContratoConstruccion contratoConstruccion = null;
            contratoConstruccion = _context.ContratoConstruccion.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

            if (contratoConstruccion != null)
            {
                contratoObservacion.ContratoConstruccionId = contratoConstruccion.ContratoConstruccionId;

                lstContratoObservacion = _context.ConstruccionObservacion.Where(r => r.ContratoConstruccionId == contratoConstruccion.ContratoConstruccionId && r.EsSupervision == pEsSupervisor && r.EsActa == true).ToList();
                lstContratoObservacion = lstContratoObservacion.OrderByDescending(r => r.ConstruccionObservacionId).ToList();

                //contratoPoliza = _context.ContratoPoliza.Where(r => !(bool)r.Eliminado && r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();
                contratoObservacion = lstContratoObservacion.Where(r => r.ContratoConstruccionId == contratoConstruccion.ContratoConstruccionId).FirstOrDefault();
                return contratoObservacion;
            }
            return null;
        }


        public async Task<List<GrillaActaInicio>> GetListGrillaActaInicio(int pPerfilId)
        {
            List<GrillaActaInicio> lstActaInicio = new List<GrillaActaInicio>();
            List<Contrato> lstContratos = await _context.Contrato.Where(r => !(bool)r.Eliminado)
                .Include(r => r.Contratacion)
                .Include(r => r.ContratoObservacion)
                .ToListAsync();

            //if (pPerfilId == (int)EnumeratorPerfil.Tecnica)
            //    lstContratos = lstContratos.Where(r => r.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString()).ToList();

            //if (pPerfilId == (int)EnumeratorPerfil.Supervisor)
            //    lstContratos = lstContratos.Where(r => r.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString()).ToList();

            List<Dominio> Listdominios = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Del_Acta_Contrato || r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Contrato).ToList();

            foreach (var Contrato in lstContratos)
            {
                lstActaInicio.Add(new GrillaActaInicio
                {
                    ContratoId = Contrato.ContratoId,
                    EstadoActa = !string.IsNullOrEmpty(Contrato.EstadoActa) ? Listdominios.Where(r => r.Codigo == Contrato.EstadoActa && r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Del_Acta_Contrato).FirstOrDefault().Nombre : " ",
                    EstadoVerificacion = Contrato.EstadoVerificacionCodigo,
                    EstadoActaCodigo = Contrato.EstadoActa,
                    FechaAprobacionRequisitos = Contrato.FechaAprobacionRequisitos.HasValue ? ((DateTime)Contrato.FechaAprobacionRequisitos).ToString("dd-MMMM-yy") : "",
                    NumeroContratoObra = Contrato.NumeroContrato,
                    TipoContrato = Contrato.Contratacion.TipoSolicitudCodigo,
                    TipoContratoNombre  = !string.IsNullOrEmpty(Contrato.Contratacion.TipoSolicitudCodigo) ? Listdominios.Where(r => r.Codigo == Contrato.Contratacion.TipoSolicitudCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Contrato).FirstOrDefault().Nombre : " ",
                  });
            }

            return lstActaInicio;

        }

    }

}

