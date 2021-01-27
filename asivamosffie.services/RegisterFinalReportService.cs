using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.model.APIModels;

namespace asivamosffie.services
{
    public class RegisterFinalReportService : IRegisterFinalReportService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly ITechnicalRequirementsConstructionPhaseService _technicalRequirementsConstructionPhaseService;

        public RegisterFinalReportService(devAsiVamosFFIEContext context, ICommonService commonService, ITechnicalRequirementsConstructionPhaseService technicalRequirementsConstructionPhaseService)
        {
            _context = context;
            _commonService = commonService;
            _technicalRequirementsConstructionPhaseService = technicalRequirementsConstructionPhaseService;
        }

        public async Task<List<VProyectosCierre>> gridRegisterFinalReport()
        {
            return await _context.VProyectosCierre.OrderByDescending(r => r.FechaTerminacionProyecto).ToListAsync();
        }

        public async Task<List<ContratacionProyecto>> GetInformeFinalByContratacionProyectoId(int pContratacionProyectoId)
        {
            List<ContratacionProyecto> ListInfomeFinal = await _context.ContratacionProyecto.Where(r => r.ContratacionProyectoId == pContratacionProyectoId)
                                                        .Include(r => r.Proyecto)
                                                            .ThenInclude(r => r.InstitucionEducativa)
                                                        .Include(r => r.Contratacion)
                                                         .ThenInclude(r => r.Contratista)
                                                        .Include(r => r.Contratacion)
                                                         .ThenInclude(r => r.Contrato)
                                                         .Include(r => r.InformeFinal).ToListAsync();

           return ListInfomeFinal;
        }

        private bool VerificarRegistroCompleto(InformeFinal pInformeFinal)
        {
            bool completo = true;
            if (
                    pInformeFinal.EstadoInforme == ConstantCodigoEstadoInformeFinal.Con_informe_enviado_para_validación
               )
            {
                completo = false;
            }

            return completo;
        }

        public async Task<Respuesta> CreateEditInformeFinal(InformeFinal pInformeFinal)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Informe_Final, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";
            try
            {

                if (pInformeFinal.InformeFinalId == 0)
                {
                    CreateEdit = "CREAR INFORME FINAL";
                    pInformeFinal.FechaCreacion = DateTime.Now;
                    pInformeFinal.Eliminado = false;
                    pInformeFinal.RegistroCompleto = VerificarRegistroCompleto(pInformeFinal);
                    _context.InformeFinal.Add(pInformeFinal);
                }
                else
                {
                    CreateEdit = "ACTUALIZAR INFORME FINAL";
                    InformeFinal informeFinal = _context.InformeFinal.Find(pInformeFinal.InformeFinalId);

                    informeFinal.FechaModificacion = DateTime.Now;
                    informeFinal.UsuarioModificacion = pInformeFinal.UsuarioCreacion;

                    informeFinal.RegistroCompleto = VerificarRegistroCompleto(informeFinal);

                }

                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.OperacionExitosa, idAccion, pInformeFinal.UsuarioCreacion, CreateEdit)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.Error, idAccion, pInformeFinal.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<List<dynamic>> GetInformeFinalListaChequeoByInformeFinalInterventoriaId(int pInformeFinalInterventoriaId)
        {
            List<InformeFinalListaChequeo> ListInformeFinalChequeo = await _context.InformeFinalListaChequeo
                                .ToListAsync();

            List<dynamic> ListChequeo = new List<dynamic>();
            String calificacionCodigo = "";
            foreach (var item in ListInformeFinalChequeo)
            {
                InformeFinalInterventoria informeFinalInterventoria = _context.InformeFinalInterventoria.Where(r => r.InformeFinalInterventoriaId == pInformeFinalInterventoriaId && r.InformeFinalListaChequeoId == item.InformeFinalListaChequeoId).FirstOrDefault();

                if (informeFinalInterventoria != null)
                {
                    ListChequeo.Add(new
                    {
                        Nombre = item.Nombre,
                        CalificacionCodigo = informeFinalInterventoria.CalificacionCodigo,
                        InformeFinalListaChequeoId = informeFinalInterventoria.InformeFinalListaChequeoId,
                        InformeFinalInterventoriaId = informeFinalInterventoria.InformeFinalInterventoriaId
                    });
                }
                else
                {
                    ListChequeo.Add(new
                    {
                        Nombre = item.Nombre,
                        CalificacionCodigo = calificacionCodigo,
                        InformeFinalListaChequeoId = item.InformeFinalListaChequeoId,
                        InformeFinalInterventoriaId = pInformeFinalInterventoriaId
                    });
                }
            }
            return ListChequeo;
        }

        private async Task<bool> ValidarInformeFinalInterventoria(int informeFinalId)
        {
            bool esCompleto = true;

            List<InformeFinalInterventoria> ListInformeTotalInterventoria = await _context.InformeFinalInterventoria.Where(cc => cc.InformeFinalId == informeFinalId)
                                                        .ToListAsync();

            int phrasesCount = await _context.InformeFinalListaChequeo.CountAsync();

            //Validación # 1
            if (ListInformeTotalInterventoria.Count() < phrasesCount)
            {
                return false;
            }else
            {
                foreach (InformeFinalInterventoria item in ListInformeTotalInterventoria)
                {
                    if (item.CalificacionCodigo == "2")
                    {
                        return false;
                    }
                }
            }
            return esCompleto;
        }


        public async Task<Respuesta> CreateEditInformeFinalInterventoria(InformeFinalInterventoria pInformeFinalInterventoria)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Informe_Final_Interventoria, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";
            try
            {

                if (pInformeFinalInterventoria.InformeFinalInterventoriaId == 0)
                {
                    CreateEdit = "CREAR INFORME FINAL INTERVENTORIA";
                    pInformeFinalInterventoria.FechaCreacion = DateTime.Now;
                    _context.InformeFinalInterventoria.Add(pInformeFinalInterventoria);
                }
                else
                {
                    CreateEdit = "ACTUALIZAR INFORME FINAL INTERVENTORIA";
                    InformeFinalInterventoria informeFinalInterventoria = _context.InformeFinalInterventoria.Find(pInformeFinalInterventoria.InformeFinalInterventoriaId);

                    informeFinalInterventoria.FechaModificacion = DateTime.Now;
                    informeFinalInterventoria.UsuarioModificacion = pInformeFinalInterventoria.UsuarioCreacion;
                }

                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.OperacionExitosa, idAccion, pInformeFinalInterventoria.UsuarioCreacion, CreateEdit)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.Error, idAccion, pInformeFinalInterventoria.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> CreateEditInformeFinalAnexo(InformeFinalAnexo pInformeFinalAnexo)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Informe_Final_Anexo, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";
            try
            {

                if (pInformeFinalAnexo.InformeFinalAnexoId == 0)
                {
                    CreateEdit = "CREAR INFORME FINAL ANEXO";
                    pInformeFinalAnexo.FechaCreacion = DateTime.Now;
                    _context.InformeFinalAnexo.Add(pInformeFinalAnexo);
                }
                else
                {
                    CreateEdit = "ACTUALIZAR INFORME FINAL INTERVENTORIA";
                    InformeFinalAnexo informeFinalAnexo = _context.InformeFinalAnexo.Find(pInformeFinalAnexo.InformeFinalAnexoId);

                    informeFinalAnexo.FechaModificacion = DateTime.Now;
                    informeFinalAnexo.UsuarioModificacion = pInformeFinalAnexo.UsuarioCreacion;
                }

                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.OperacionExitosa, idAccion, pInformeFinalAnexo.UsuarioCreacion, CreateEdit)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.Error, idAccion, pInformeFinalAnexo.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        private async Task<Respuesta> CreateEditInformeFinalInterventoriaObservacion(InformeFinalInterventoriaObservaciones pObservacion, string pUsuarioCreacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Informe_Final_Interventoria_Observacion, (int)EnumeratorTipoDominio.Acciones);

            Respuesta respuesta = new Respuesta();
            try
            {
                string strCrearEditar = "";
                if (pObservacion.InformeFinalInterventoriaObservacionesId > 0)
                {
                    strCrearEditar = "EDITAR OBSERVACION INFORME FINAL INTERVENTORIA";

                    pObservacion.FechaModificacion = DateTime.Now;
                    pObservacion.UsuarioModificacion = pUsuarioCreacion;

                    pObservacion.Observaciones = pObservacion.Observaciones;

                }
                else
                {
                    strCrearEditar = "CREAR OBSERVACION INFORME FINAL INTERVENTORIA";

                    if (pObservacion.EsSupervision == true)
                    {
                        InformeFinalInterventoria informeFinalInterventoria = _context.InformeFinalInterventoria.Find(pObservacion.InformeFinalInterventoriaId);

                        informeFinalInterventoria.CalificacionCodigo = ConstantCodigoEstadoInformeFinal.Con_Observaciones_del_supervisor;

                    }

                    InformeFinalInterventoriaObservaciones informeFinalInterventoriaObservaciones = new InformeFinalInterventoriaObservaciones
                    {
                        FechaCreacion = DateTime.Now,
                        UsuarioCreacion = pUsuarioCreacion,

                        Observaciones = pObservacion.Observaciones,
                        EsSupervision = pObservacion.EsSupervision,
                        EsCalificacion = pObservacion.EsCalificacion,
                    };

                    _context.InformeFinalInterventoriaObservaciones.Add(informeFinalInterventoriaObservaciones);
                }

                return respuesta;
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.Error, idAccion, pObservacion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> SendFinalReportToSupervision(int pId, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_A_supervisor_Informe_Final_Interventoria, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                InformeFinalInterventoria informeFinalInterventoria = _context.InformeFinalInterventoria.Find(pId);
                InformeFinal informeFinal = _context.InformeFinal.Find(informeFinalInterventoria.InformeFinalId);

                informeFinalInterventoria.UsuarioModificacion = pUsuario;
                informeFinalInterventoria.FechaModificacion = DateTime.Now;
                informeFinal.EstadoInforme = ConstantCodigoEstadoInformeFinal.Con_informe_enviado_para_validación;

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.OperacionExitosa, idAccion, pUsuario, "INFORME FINAL ")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                };
            }
        }

    }
}
