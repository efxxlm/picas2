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
            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();
            List<Dominio> TipoObraIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar).ToList();

            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();
            List<ContratacionProyecto> ListInfomeFinal = await _context.ContratacionProyecto.Where(r => r.ContratacionProyectoId == pContratacionProyectoId)
                                                        .Include(r => r.Proyecto)
                                                            .ThenInclude(r => r.InstitucionEducativa)
                                                        .Include(r => r.Contratacion)
                                                         .ThenInclude(r => r.Contratista)
                                                        .Include(r => r.Contratacion)
                                                         .ThenInclude(r => r.Contrato)
                                                         .Include(r => r.InformeFinal)
                                                            .ThenInclude(r => r.InformeFinalInterventoria)
                                                                .ThenInclude(r => r.InformeFinalInterventoriaObservaciones).ToListAsync();
            InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == ListInfomeFinal[0].Proyecto.SedeId).FirstOrDefault();
            Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == ListInfomeFinal[0].Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
            ListInfomeFinal[0].Proyecto.MunicipioObj = Municipio;
            ListInfomeFinal[0].Proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
            ListInfomeFinal[0].Proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == ListInfomeFinal[0].Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
            ListInfomeFinal[0].Contratacion.TipoContratacionCodigo = TipoObraIntervencion.Where(r => r.Codigo == ListInfomeFinal[0].Contratacion.TipoSolicitudCodigo).Select(r => r.Nombre).FirstOrDefault();
            ListInfomeFinal[0].Proyecto.Sede = Sede;
            
            return ListInfomeFinal;
        }

        public async Task<InformeFinalInterventoria> GetInformeFinalAnexoByInformeFinalInterventoriaId(int pInformeFinalInterventoriaId)
        {
            InformeFinalInterventoria InformeFinalAnexo = await _context.InformeFinalInterventoria.Where(r => r.InformeFinalInterventoriaId == pInformeFinalInterventoriaId)
                                                        .Include(r => r.InformeFinalAnexo).FirstOrDefaultAsync();
            InformeFinalAnexo.InformeFinalInterventoriaObservaciones = _context.InformeFinalInterventoriaObservaciones.Where(r => r.EsSupervision == true).ToList();

            return InformeFinalAnexo;
        }

        private bool VerificarRegistroCompleto(InformeFinal pInformeFinal)
        {
            bool completo = false;
            if (
                    pInformeFinal.EstadoInforme == ConstantCodigoEstadoInformeFinal.Con_informe_enviado_para_validación
               )
            {
                completo = true;
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
                    InformeFinal informeFinalOld = _context.InformeFinal.Where(r => r.InformeFinalId == pInformeFinal.InformeFinalId && r.ContratacionProyectoId == pInformeFinal.ContratacionProyectoId).FirstOrDefault();
                    
                    if (informeFinalOld != null)
                    {
                        informeFinalOld.FechaModificacion = DateTime.Now;
                        informeFinalOld.UsuarioModificacion = pInformeFinal.UsuarioCreacion;
                        informeFinalOld.UrlActa = pInformeFinal.UrlActa;
                        informeFinalOld.FechaSuscripcion = pInformeFinal.FechaSuscripcion;
                        informeFinalOld.RegistroCompleto = VerificarRegistroCompleto(informeFinalOld);
                    }
                    else
                    {
                        CreateEdit = "NO SE PUDO ACTUALIZAR EL INFORME FINAL";
                    }
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

        public async Task<List<dynamic>> GetInformeFinalListaChequeoByContratacionProyectoId(int pContratacionProyectoId)
        {
            List<InformeFinalListaChequeo> ListInformeFinalChequeo = await _context.InformeFinalListaChequeo
                                .ToListAsync();

            List<dynamic> ListChequeo = new List<dynamic>();
            String calificacionCodigo = "";
            InformeFinal informeFinal = _context.InformeFinal.Where(r => r.ContratacionProyectoId == pContratacionProyectoId).FirstOrDefault();
            int informeFinalInterventoriaObservacionesId = 0;
            bool tieneObservacionNoCumple = false;

            if (informeFinal!=null)//Sino han llenado los campos de informe final no se muestra la lista de chequeos
            {
                foreach (var item in ListInformeFinalChequeo)
                {
                    InformeFinalInterventoria informeFinalInterventoria = _context.InformeFinalInterventoria.Where(r => r.InformeFinalId == informeFinal.InformeFinalId && r.InformeFinalListaChequeoId == item.InformeFinalListaChequeoId).FirstOrDefault();

                    if (informeFinalInterventoria != null)
                    {
                        if (informeFinalInterventoria.CalificacionCodigo == ConstantCodigoCalificacionInformeFinal.No_Cumple)
                        {
                            //Validar si tiene observaciones
                            InformeFinalInterventoriaObservaciones informeFinalInterventoriaObservaciones = _context.InformeFinalInterventoriaObservaciones.Where(r => r.InformeFinalInterventoriaId == informeFinalInterventoria.InformeFinalInterventoriaId && r.EsCalificacion == true).FirstOrDefault();
                            if (informeFinalInterventoriaObservaciones != null)
                            {
                                informeFinalInterventoriaObservacionesId = informeFinalInterventoriaObservaciones.InformeFinalInterventoriaObservacionesId;
                                tieneObservacionNoCumple = true;
                            }
                            else
                            {
                                informeFinalInterventoriaObservacionesId = 0;
                                tieneObservacionNoCumple = false;
                            }
                        }
                        else
                        {
                            informeFinalInterventoriaObservacionesId = 0;
                            tieneObservacionNoCumple = false;
                        }
                        ListChequeo.Add(new
                        {
                            Nombre = item.Nombre,
                            CalificacionCodigo = informeFinalInterventoria.CalificacionCodigo,
                            InformeFinalListaChequeoId = informeFinalInterventoria.InformeFinalListaChequeoId,
                            InformeFinalInterventoriaId = informeFinalInterventoria.InformeFinalInterventoriaId,
                            TieneObservacionSupervisor = informeFinalInterventoria.TieneObservacionSupervisor is null ? false : informeFinalInterventoria.TieneObservacionSupervisor,
                            InformeFinalId = informeFinalInterventoria.InformeFinalId,
                            InformeFinalAnexoId =informeFinalInterventoria.InformeFinalAnexoId,
                            InformeFinalInterventoriaObservacionesId = informeFinalInterventoriaObservacionesId,
                            TieneObservacionNoCumple = tieneObservacionNoCumple
                        });
                    }
                    else
                    {
                        ListChequeo.Add(new
                        {
                            Nombre = item.Nombre,
                            CalificacionCodigo = calificacionCodigo,
                            InformeFinalListaChequeoId = item.InformeFinalListaChequeoId,
                            InformeFinalInterventoriaId = 0,
                            TieneObservacionSupervisor = false,
                            InformeFinalId = informeFinal.InformeFinalId,
                            InformeFinalAnexoId = 0,
                            InformeFinalInterventoriaObservacionesId = 0,
                            TieneObservacionNoCumple = false
                        });
                    }
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


        public async Task<Respuesta> CreateEditInformeFinalInterventoria(InformeFinalInterventoria pInformeFinalInterventoriaId)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Informe_Final_Interventoria, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";
            try
            {
                InformeFinalInterventoria informeFinalInterventoria = _context.InformeFinalInterventoria
                        .Where(r => r.InformeFinalId == pInformeFinalInterventoriaId.InformeFinalId && r.InformeFinalListaChequeoId == pInformeFinalInterventoriaId.InformeFinalListaChequeoId).FirstOrDefault();

                if (pInformeFinalInterventoriaId.InformeFinalInterventoriaId == 0 && informeFinalInterventoria ==null)
                {
                    CreateEdit = "CREAR INFORME FINAL INTERVENTORIA";
                    pInformeFinalInterventoriaId.FechaCreacion = DateTime.Now;
                    _context.InformeFinalInterventoria.Add(pInformeFinalInterventoriaId);
                }
                else
                {
                    CreateEdit = "ACTUALIZAR INFORME FINAL INTERVENTORIA";

                    InformeFinalInterventoria informeFinalInterventoriaOld = _context.InformeFinalInterventoria
                        .Where(r => r.InformeFinalId == pInformeFinalInterventoriaId.InformeFinalId && r.InformeFinalListaChequeoId == pInformeFinalInterventoriaId.InformeFinalListaChequeoId).FirstOrDefault();

                    if (informeFinalInterventoriaOld != null)
                    {
                        informeFinalInterventoriaOld.FechaModificacion = DateTime.Now;
                        informeFinalInterventoriaOld.UsuarioModificacion = pInformeFinalInterventoriaId.UsuarioCreacion;
                        informeFinalInterventoriaOld.CalificacionCodigo = pInformeFinalInterventoriaId.CalificacionCodigo;
                    }
                    else
                    {
                        CreateEdit = "NO SE PUDO ACTUALIZAR EL INFORME FINAL INTERVENTORIA";
                    }
                }

                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.OperacionExitosa, idAccion, pInformeFinalInterventoriaId.UsuarioCreacion, CreateEdit)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.Error, idAccion, pInformeFinalInterventoriaId.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> CreateEditInformeFinalAnexo(InformeFinalAnexo pInformeFinalAnexoId, int pInformeFinalInterventoriaId)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Informe_Final_Anexo, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";
            bool existe = false;
            try
            {
                InformeFinalInterventoria informeFinalInterventoria = _context.InformeFinalInterventoria
                    .Where(r => r.InformeFinalInterventoriaId == pInformeFinalInterventoriaId).FirstOrDefault();

                if (informeFinalInterventoria != null && informeFinalInterventoria.InformeFinalAnexoId != null)
                {
                    existe = true;
                }

                if (pInformeFinalAnexoId.InformeFinalAnexoId == 0)
                {
                    CreateEdit = "CREAR INFORME FINAL ANEXO";
                    pInformeFinalAnexoId.FechaCreacion = DateTime.Now;
                    _context.InformeFinalAnexo.Add(pInformeFinalAnexoId);
                    _context.SaveChanges();
                }
                else
                {
                    CreateEdit = "ACTUALIZAR INFORME FINAL ANEXO";

                    InformeFinalAnexo informeFinalAnexoOld = _context.InformeFinalAnexo
                        .Where(r => r.InformeFinalAnexoId == pInformeFinalAnexoId.InformeFinalAnexoId).FirstOrDefault();

                    if (informeFinalAnexoOld != null)
                    {
                        informeFinalAnexoOld.FechaModificacion = DateTime.Now;
                        informeFinalAnexoOld.UsuarioModificacion = pInformeFinalAnexoId.UsuarioCreacion;
                        informeFinalAnexoOld.TipoAnexo = pInformeFinalAnexoId.TipoAnexo;
                        informeFinalAnexoOld.NumRadicadoSac = pInformeFinalAnexoId.NumRadicadoSac;
                        informeFinalAnexoOld.FechaRadicado = pInformeFinalAnexoId.FechaRadicado;
                        informeFinalAnexoOld.UrlSoporte = pInformeFinalAnexoId.UrlSoporte;

                    }
                    else
                    {
                        CreateEdit = "NO SE PUDO ACTUALIZAR EL INFORME FINAL ANEXO";
                    }

                }

                _context.SaveChanges();
                //Actualizar id interventoria
                if (!existe && informeFinalInterventoria!= null)
                {
                    informeFinalInterventoria.InformeFinalAnexoId = pInformeFinalAnexoId.InformeFinalAnexoId;
                    _context.SaveChanges();
                }

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.OperacionExitosa, idAccion, pInformeFinalAnexoId.UsuarioCreacion, CreateEdit)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.Error, idAccion, pInformeFinalAnexoId.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> CreateEditInformeFinalInterventoriaObservacion(InformeFinalInterventoriaObservaciones pObservacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Informe_Final_Interventoria_Observacion, (int)EnumeratorTipoDominio.Acciones);

            Respuesta respuesta = new Respuesta();
            try
            {
                string strCrearEditar = "";

                InformeFinalInterventoriaObservaciones informeFinalInterventoriaObservaciones = _context.InformeFinalInterventoriaObservaciones
                    .Where(r => r.InformeFinalInterventoriaId == pObservacion.InformeFinalInterventoriaId && r.EsCalificacion == true).FirstOrDefault();

                if (pObservacion.EsCalificacion == true && informeFinalInterventoriaObservaciones !=null)
                {
                    pObservacion.InformeFinalInterventoriaObservacionesId = informeFinalInterventoriaObservaciones.InformeFinalInterventoriaObservacionesId;
                }

                if (pObservacion.InformeFinalInterventoriaObservacionesId == 0)
                {
                    strCrearEditar = "CREAR INFORME FINAL ANEXO";
                    pObservacion.FechaCreacion = DateTime.Now;

                    _context.InformeFinalInterventoriaObservaciones.Add(pObservacion);

                    if (pObservacion.EsSupervision == true)
                    {
                        InformeFinalInterventoria informeFinalInterventoria = _context.InformeFinalInterventoria.Find(pObservacion.InformeFinalInterventoriaId);
                        
                        informeFinalInterventoria.CalificacionCodigo = ConstantCodigoEstadoInformeFinal.Con_Observaciones_del_supervisor;
                    }
                }
                else
                {
                    strCrearEditar = "ACTUALIZAR OBSERVACION INFORME FINAL INTERVENTORIA";

                    InformeFinalInterventoriaObservaciones InformeFinalInterventoriaObservacionesOld = _context.InformeFinalInterventoriaObservaciones
                        .Where(r => r.InformeFinalInterventoriaObservacionesId == pObservacion.InformeFinalInterventoriaObservacionesId).FirstOrDefault();

                    if (InformeFinalInterventoriaObservacionesOld != null)
                    {
                        InformeFinalInterventoriaObservacionesOld.FechaModificacion = DateTime.Now;
                        InformeFinalInterventoriaObservacionesOld.UsuarioModificacion = pObservacion.UsuarioCreacion;
                        InformeFinalInterventoriaObservacionesOld.Observaciones = pObservacion.Observaciones;
                    }
                    else
                    {
                        strCrearEditar = "NO SE PUDO ACTUALIZAR EL INFORME FINAL ANEXO";
                    }

                }

                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.OperacionExitosa, idAccion, pObservacion.UsuarioCreacion, strCrearEditar)
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
