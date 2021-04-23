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
using System.IO;
using Z.EntityFramework.Plus;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Runtime.InteropServices.WindowsRuntime;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Microsoft.EntityFrameworkCore.Internal;
using asivamosffie.services.Helpers.Constants;

namespace asivamosffie.services
{
    public class ValidateFinalReportService : IValidateFinalReportService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public ValidateFinalReportService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }

        public async Task<List<InformeFinal>> GetListInformeFinal()
        {
            List<InformeFinal> list = await _context.InformeFinal
                            .Where(r=> r.EstadoValidacion == ConstantCodigoEstadoValidacionInformeFinal.Con_informe_enviado_al_supervisor || !String.IsNullOrEmpty(r.EstadoAprobacion))
                            .Include(r=> r.Proyecto)
                                .ThenInclude(r => r.InstitucionEducativa)
                            .ToListAsync();
            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();

            foreach(var item in list)
            {
                InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == item.Proyecto.SedeId).FirstOrDefault();
                item.Proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == item.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
                item.Proyecto.Sede = Sede;
                if (String.IsNullOrEmpty(item.EstadoAprobacion)|| item.EstadoAprobacion == "0")
                {
                    item.EstadoAprobacionString = "Sin validación";
                }
                else
                {
                    item.EstadoAprobacionString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.EstadoAprobacion, (int)EnumeratorTipoDominio.Estado_Aprobacion_Informe_Final);
                }
            }
            return list;
        }

        public async Task<List<dynamic>> GetInformeFinalByProyectoId(int pProyectoId)
        {
            String numeroContratoObra = string.Empty, nombreContratistaObra = string.Empty, numeroContratoInterventoria = string.Empty, nombreContratistaInterventoria = string.Empty;
            String fechaTerminacionInterventoria = string.Empty;
            String fechaTerminacionObra = string.Empty;

            List<dynamic> ProyectoAjustado = new List<dynamic>();
            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();
            List<Dominio> TipoObraIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar).ToList();

            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();
            Proyecto proyecto = await _context.Proyecto.Where(r => r.ProyectoId == pProyectoId)
                                                        .Include(r => r.InformeFinal)
                                                         .Include(r => r.InstitucionEducativa)
                                                         .FirstOrDefaultAsync();
            List<InformeFinalObservaciones> informeFinalObservacionesApoyo = _context.InformeFinalObservaciones.Where(r => r.InformeFinalId == proyecto.InformeFinal.FirstOrDefault().InformeFinalId && r.EsApoyo == true).ToList();
            List<InformeFinalObservaciones> informeFinalObservacionesSupervisor = _context.InformeFinalObservaciones.Where(r => r.InformeFinalId == proyecto.InformeFinal.FirstOrDefault().InformeFinalId && r.EsSupervision == true && (r.Archivado == null || r.Archivado == false)).OrderByDescending(r=> r.FechaCreacion).ToList();
            List<InformeFinalObservaciones> informeFinalObservacionesCumplimiento = _context.InformeFinalObservaciones.Where(r => r.InformeFinalId == proyecto.InformeFinal.FirstOrDefault().InformeFinalId && r.EsGrupoNovedades == true).OrderByDescending(r => r.FechaCreacion).ToList();

            InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == proyecto.SedeId).FirstOrDefault();
            Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == proyecto.LocalizacionIdMunicipio).FirstOrDefault();
            proyecto.MunicipioObj = Municipio;
            proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
            proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
            proyecto.Sede = Sede;

            if (proyecto.InformeFinal.Count > 0)
            {
                proyecto.InformeFinal.FirstOrDefault().InformeFinalObservaciones = informeFinalObservacionesApoyo;
                proyecto.InformeFinal.FirstOrDefault().InformeFinalObservacionesSupervisor = informeFinalObservacionesSupervisor;
                proyecto.InformeFinal.FirstOrDefault().InformeFinalObservacionesCumplimiento = informeFinalObservacionesCumplimiento;
                //historial recibo de satisfacción
                proyecto.InformeFinal.FirstOrDefault().HistorialObsInformeFinalSupervisor = _context.InformeFinalObservaciones.Where(r => r.EsSupervision == true && r.Archivado == true && r.InformeFinalId == proyecto.InformeFinal.FirstOrDefault().InformeFinalId && (r.Eliminado == false || r.Eliminado == null)).ToList();

            }
            List<ContratacionProyecto> ListContratacion = await _context.ContratacionProyecto
                                                        .Where(r => r.ProyectoId == pProyectoId)
                                                        .Include(r => r.Contratacion)
                                                         .ThenInclude(r => r.Contratista)
                                                        .Include(r => r.Contratacion)
                                                         .ThenInclude(r => r.Contrato)
                                                        .ToListAsync();
            ListContratacion.FirstOrDefault().Contratacion.TipoContratacionCodigo = TipoObraIntervencion.Where(r => r.Codigo == ListContratacion.FirstOrDefault().Contratacion.TipoSolicitudCodigo).Select(r => r.Nombre).FirstOrDefault();

            foreach (var item in ListContratacion)
            {
                Contrato contrato = await _context.Contrato.Where(r => r.ContratacionId == item.ContratacionId).FirstOrDefaultAsync();
                Contratacion contratacion = await _context.Contratacion.Where(r => r.ContratacionId == item.ContratacionId).FirstOrDefaultAsync();
                if (contrato != null)
                {
                    if (contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Obra)
                    {

                        nombreContratistaObra = contratacion.Contratista != null ? contratacion.Contratista.Nombre : string.Empty;
                        numeroContratoObra = contrato.NumeroContrato != null ? contrato.NumeroContrato : string.Empty;
                        fechaTerminacionObra = contrato.FechaTerminacionFase2 != null ? Convert.ToDateTime(contrato.FechaTerminacionFase2).ToString("yyyy-MM-dd") : string.Empty;
                    }
                    else if (contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Interventoria)
                    {
                        nombreContratistaInterventoria = contratacion.Contratista != null ? contratacion.Contratista.Nombre : string.Empty;
                        numeroContratoInterventoria = contrato.NumeroContrato != null ? contrato.NumeroContrato : string.Empty;
                        fechaTerminacionInterventoria = contrato.FechaTerminacionFase2 != null ? Convert.ToDateTime(contrato.FechaTerminacionFase2).ToString("yyyy-MM-dd") : string.Empty;

                    }
                }
            }
            ProyectoAjustado.Add(new
            {
                proyecto = proyecto,
                numeroContratoObra = numeroContratoObra,
                nombreContratistaObra = nombreContratistaObra,
                numeroContratoInterventoria = numeroContratoInterventoria,
                nombreContratistaInterventoria = nombreContratistaInterventoria,
                fechaTerminacionInterventoria = fechaTerminacionInterventoria,
                fechaTerminacionObra = fechaTerminacionObra
            });

            return ProyectoAjustado;
        }

        public async Task<List<InformeFinalInterventoria>> GetInformeFinalListaChequeoByInformeFinalId(int pInformeFinalId)
        {
            int informeFinalInterventoriaObservacionesId = 0;
            bool tieneObservacionNoCumple = false;
            bool semaforo = false;

            List<InformeFinalInterventoria> ListInformeFinalChequeo = await _context.InformeFinalInterventoria
                                .Where(r => r.InformeFinalId == pInformeFinalId)
                                .Include(r => r.InformeFinalListaChequeo)
                                    .ThenInclude(r => r.ListaChequeoItem)
                                .Include(r => r.InformeFinalAnexo)
                                .OrderBy(r => r.InformeFinalListaChequeo.Orden)
                                .ToListAsync();
            InformeFinal informeFinal = _context.InformeFinal.Find(pInformeFinalId);

            if (informeFinal.EstadoAprobacion == ConstantCodigoEstadoAprobacionInformeFinal.En_proceso_aprobacion)
            {
                InformeFinalInterventoria no_seleccionado = _context.InformeFinalInterventoria.Where(r => r.InformeFinalId == informeFinal.InformeFinalId && (r.AprobacionCodigo != "0" && !String.IsNullOrEmpty(r.AprobacionCodigo))).FirstOrDefault();

                if (no_seleccionado == null)
                {
                    semaforo = true;
                }
            }

            foreach (var item in ListInformeFinalChequeo)
            {
                item.CalificacionCodigoString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.CalificacionCodigo, (int)EnumeratorTipoDominio.Calificacion_Informe_Final);
                item.ValidacionCodigoString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.ValidacionCodigo, (int)EnumeratorTipoDominio.Calificacion_Informe_Final);

                if (String.IsNullOrEmpty(item.AprobacionCodigo) || item.AprobacionCodigo == "0")
                {
                    item.AprobacionCodigoString = String.Empty;
                }
                else
                {
                    item.AprobacionCodigoString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.AprobacionCodigo, (int)EnumeratorTipoDominio.Calificacion_Informe_Final);
                }

                if (item.InformeFinalAnexoId != null)
                {
                    item.InformeFinalAnexo.TipoAnexoString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.InformeFinalAnexo.TipoAnexo, (int)EnumeratorTipoDominio.Tipo_Anexo_Informe_Final);
                }
                item.EstadoValidacion = informeFinal.EstadoValidacion;
                item.RegistroCompletoValidacion = informeFinal.RegistroCompletoValidacion == null ? false : (bool) informeFinal.RegistroCompletoValidacion;
                if (item.ValidacionCodigo == ConstantCodigoCalificacionInformeFinal.No_Cumple && item.AprobacionCodigo != ConstantCodigoCalificacionInformeFinal.No_Cumple)
                {
                    //Validar si tiene observaciones
                    InformeFinalInterventoriaObservaciones informeFinalInterventoriaObservaciones = _context.InformeFinalInterventoriaObservaciones.Where(r => r.InformeFinalInterventoriaId == item.InformeFinalInterventoriaId && r.EsApoyo == true).FirstOrDefault();
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
                }else if (item.AprobacionCodigo == ConstantCodigoCalificacionInformeFinal.No_Cumple)
                {
                    InformeFinalInterventoriaObservaciones informeFinalInterventoriaObservaciones = _context.InformeFinalInterventoriaObservaciones.Where(r => r.InformeFinalInterventoriaId == item.InformeFinalInterventoriaId && r.EsSupervision == true).OrderByDescending(r => r.FechaCreacion).FirstOrDefault();
                    if (informeFinalInterventoriaObservaciones != null)
                    {
                        informeFinalInterventoriaObservacionesId = informeFinalInterventoriaObservaciones.InformeFinalInterventoriaObservacionesId;
                        if (informeFinalInterventoriaObservaciones.Archivado == null || informeFinalInterventoriaObservaciones.Archivado == false)
                        {
                            tieneObservacionNoCumple = true;
                        }
                        else
                        {
                            tieneObservacionNoCumple = false;
                        }
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

                item.InformeFinalInterventoriaObservacionesId = informeFinalInterventoriaObservacionesId;
                item.TieneObservacionNoCumple = tieneObservacionNoCumple;
                item.Semaforo = semaforo;
            }
            return ListInformeFinalChequeo;
        }

        private bool VerificarInformeFinalAprobacion(int pInformeFinalId, bool? tieneOBservaciones)
        {
            bool esCompleto = false;

            InformeFinal informeFinal = _context.InformeFinal.Where(r => r.InformeFinalId == pInformeFinalId).FirstOrDefault();
            //Validación # 1
            if (informeFinal != null)
            {
                InformeFinalInterventoria existe_no_cumple = _context.InformeFinalInterventoria.Where(r => r.InformeFinalId == pInformeFinalId && r.AprobacionCodigo == ConstantCodigoCalificacionInformeFinal.No_Cumple).FirstOrDefault();
                InformeFinalInterventoria existe_no_diligenciado = _context.InformeFinalInterventoria.Where(r => r.InformeFinalId == pInformeFinalId && (r.AprobacionCodigo == "0" || String.IsNullOrEmpty(r.AprobacionCodigo))).FirstOrDefault();
                
                if (tieneOBservaciones == null)
                {
                    return false;
                }

                if (existe_no_diligenciado != null)
                {
                    return false;
                }
                if (existe_no_cumple != null)
                {
                    informeFinal.EstadoAprobacion = ConstantCodigoEstadoAprobacionInformeFinal.Con_observaciones_supervisor;
                    return false;
                }
                //validar si tiene observaciones al recibo de satisfaccion
                if (tieneOBservaciones == true && existe_no_diligenciado == null && existe_no_cumple == null)
                {
                    informeFinal.EstadoAprobacion = ConstantCodigoEstadoAprobacionInformeFinal.Con_observaciones_supervisor;
                    return false;
                }
            }
            else
            {
                return false;
            }
            informeFinal.EstadoAprobacion = ConstantCodigoEstadoAprobacionInformeFinal.Con_aprobacion_supervisor;

            //_context.SaveChanges();
            return esCompleto;
        }

        public async Task<Respuesta> UpdateStateApproveInformeFinalInterventoriaByInformeFinal(InformeFinal informeFinal, string user)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Actualizar_Estado_Aprobacion_Informe_Final, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;

            try
            {
                InformeFinal informeFinalOld = _context.InformeFinal.Find(informeFinal.InformeFinalId);

                foreach (InformeFinalInterventoria informeFinalInterventoria in informeFinal.InformeFinalInterventoria)
                {
                    informeFinalInterventoria.UsuarioCreacion = user.ToUpper();
                    informeFinalInterventoria.TieneObservacionSupervisor = false;
                    await this.UpdateStateApproveInformeFinalInterventoria(informeFinalInterventoria.InformeFinalInterventoriaId, informeFinalInterventoria.AprobacionCodigo,user);

                    if (informeFinalInterventoria.AprobacionCodigo == ConstantCodigoCalificacionInformeFinal.No_Cumple)
                    {
                        ///Actualiza o crea observaciones segun el caso (Sólo SUPERVISIÓN)
                        foreach (InformeFinalInterventoriaObservaciones informeFinalInterventoriaObservaciones in informeFinalInterventoria.InformeFinalInterventoriaObservaciones)
                        {
                            if (informeFinalInterventoriaObservaciones.EsApoyo == true)
                            {
                                informeFinalInterventoriaObservaciones.InformeFinalInterventoriaObservacionesId = 0;
                                informeFinalInterventoriaObservaciones.EsApoyo = false;
                                informeFinalInterventoriaObservaciones.EsSupervision = true;
                            }

                            informeFinalInterventoriaObservaciones.UsuarioCreacion = user.ToUpper();
                            await this.CreateEditInformeFinalInterventoriaObservacion(informeFinalInterventoriaObservaciones);
                        }
                    }

                }
                bool? tieneObservaciones = null;

                VerificarInformeFinalAprobacion(informeFinal.InformeFinalId, informeFinalOld.TieneObservacionesSupervisor == null ? tieneObservaciones : (bool)informeFinalOld.TieneObservacionesSupervisor);

                await _context.SaveChangesAsync();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, informeFinal.UsuarioCreacion, strCrearEditar)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarInformeFinalProyecto, GeneralCodes.Error, idAccion, informeFinal.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> UpdateStateApproveInformeFinalInterventoria(int pInformeFinalInterventoriaId,string code,string user)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Actualizar_Estado_Aprobacion_Informe_Final, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = string.Empty;
            try
            {
                InformeFinalInterventoria informeFinalInterventoria = _context.InformeFinalInterventoria
                        .Where(r => r.InformeFinalInterventoriaId == pInformeFinalInterventoriaId).FirstOrDefault();
                InformeFinal informeFinal = _context.InformeFinal
                        .Where(r => r.InformeFinalId == informeFinalInterventoria.InformeFinalId).FirstOrDefault();

                CreateEdit = "ACTUALIZAR INFORME FINAL INTERVENTORIA";
                await _context.Set<InformeFinalInterventoria>()
                                                  .Where(r => r.InformeFinalInterventoriaId == pInformeFinalInterventoriaId)
                                                                      .UpdateAsync(r => new InformeFinalInterventoria()
                                                                      {
                                                                        FechaModificacion = DateTime.Now,
                                                                        UsuarioModificacion = user,
                                                                        TieneObservacionSupervisor = code == ConstantCodigoCalificacionInformeFinal.No_Cumple ? true : false,
                                                                        AprobacionCodigo = code,//cumple,no aplica, no cumple
                                                                      });
                informeFinal.EstadoAprobacion = ConstantCodigoEstadoAprobacionInformeFinal.En_proceso_aprobacion;

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, user, CreateEdit)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarInformeFinalProyecto, GeneralCodes.Error, idAccion, user, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> CreateEditObservacionInformeFinal(InformeFinalObservaciones pObservacion, bool tieneOBservaciones)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Informe_Final_Observacion, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            try
            {
                InformeFinal informefinal = await _context.InformeFinal.FindAsync(pObservacion.InformeFinalId);

                //creo un nuevo registro de observación
                if ((informefinal.EstadoAprobacion == ConstantCodigoEstadoAprobacionInformeFinal.Modificado_Apoyo_Supervision_Interventor && pObservacion.Archivado == true)
                    || informefinal.EstadoAprobacion == ConstantCodigoEstadoAprobacionInformeFinal.Con_observaciones_liquidaciones_novedades)
                {
                    pObservacion.InformeFinalObservacionesId = 0;
                    pObservacion.Archivado = false;
                }

                if (pObservacion.InformeFinalObservacionesId == 0)
                {
                    await _context.Set<InformeFinal>().Where(r => r.InformeFinalId == pObservacion.InformeFinalId && (r.EstadoAprobacion == "0" || string.IsNullOrEmpty(r.EstadoAprobacion)))
                           .UpdateAsync(r => new InformeFinal()
                           {
                               FechaModificacion = DateTime.Now,
                               UsuarioModificacion = pObservacion.UsuarioCreacion,
                               EstadoAprobacion = ConstantCodigoEstadoAprobacionInformeFinal.En_proceso_aprobacion,
                           });

                    if (informefinal.EstadoAprobacion == ConstantCodigoEstadoAprobacionInformeFinal.Con_observaciones_liquidaciones_novedades)
                    {
                        //Enviar las observaciones del supervisor a historial

                        //Observaciones a recibo de satisfacción
                        List<InformeFinalObservaciones> informeFinalObservaciones = _context.InformeFinalObservaciones.Where(r => r.InformeFinalId == pObservacion.InformeFinalId && r.EsSupervision == true && (r.Archivado == null || r.Archivado == false)).ToList();
                        foreach (var itemobs in informeFinalObservaciones)
                        {
                            itemobs.Archivado = true;
                            itemobs.FechaModificacion = DateTime.Now;
                            itemobs.UsuarioModificacion = pObservacion.UsuarioCreacion;
                        }
                    }

                    strCrearEditar = "CREAR INFORME FINAL OBSERVACIONES";
                    pObservacion.FechaCreacion = DateTime.Now;
                    _context.InformeFinalObservaciones.Add(pObservacion);
                }
                else
                {
                    strCrearEditar = "ACTUALIZAR INFORME FINAL OBSERVACION";

                    await _context.Set<InformeFinalObservaciones>().Where(r => r.InformeFinalObservacionesId == pObservacion.InformeFinalObservacionesId)
                                                                   .UpdateAsync(r => new InformeFinalObservaciones()
                                                                   {
                                                                       FechaModificacion = DateTime.Now,
                                                                       UsuarioModificacion = pObservacion.UsuarioCreacion,
                                                                       Observaciones = pObservacion.Observaciones,
                                                                   });
                }


                await _context.Set<InformeFinal>().Where(r => r.InformeFinalId == pObservacion.InformeFinalId)
                                                              .UpdateAsync(r => new InformeFinal()
                                                              {
                                                                  FechaModificacion = DateTime.Now,
                                                                  UsuarioModificacion = pObservacion.UsuarioCreacion,
                                                                  TieneObservacionesSupervisor = tieneOBservaciones,
                                                              });

                VerificarInformeFinalAprobacion(pObservacion.InformeFinalId , tieneOBservaciones);


                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, pObservacion.UsuarioCreacion, strCrearEditar)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarInformeFinalProyecto, GeneralCodes.Error, idAccion, pObservacion.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> EditObservacionInformeFinal(InformeFinal pInformeFinal)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Informe_Final, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = string.Empty;
            try
            {

                if (pInformeFinal.InformeFinalId != 0 && pInformeFinal != null) {
                    CreateEdit = "ACTUALIZAR INFORME FINAL";
                    await _context.Set<InformeFinal>()
                              .Where(o => o.InformeFinalId == pInformeFinal.InformeFinalId)
                                                                                  .UpdateAsync(r => new InformeFinal()
                                                                                  {
                                                                                      FechaModificacion = DateTime.Now,
                                                                                      UsuarioModificacion = pInformeFinal.UsuarioCreacion,
                                                                                      EstadoValidacion = pInformeFinal.EstadoValidacion == null || pInformeFinal.EstadoValidacion == "0" ? ConstantCodigoEstadoValidacionInformeFinal.En_proceso_de_validacion : pInformeFinal.EstadoValidacion,
                                                                                      TieneObservacionesValidacion = pInformeFinal.TieneObservacionesValidacion,
                                                                                  });
                }

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, pInformeFinal.UsuarioCreacion, CreateEdit)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarInformeFinalProyecto, GeneralCodes.Error, idAccion, pInformeFinal.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> CreateEditInformeFinalInterventoriaObservacion(InformeFinalInterventoriaObservaciones pObservacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Informe_Final_Interventoria_Observacion, (int)EnumeratorTipoDominio.Acciones);

            Respuesta respuesta = new Respuesta();
            try
            {
                string strCrearEditar = string.Empty;
                if (pObservacion.EsSupervision == true)
                {
                    if (pObservacion.Archivado == true)
                    {
                        pObservacion.InformeFinalInterventoriaObservacionesId = 0;
                        pObservacion.Archivado = false;
                    }

                    if (pObservacion.InformeFinalInterventoriaObservacionesId == 0)
                    {
                        strCrearEditar = "CREAR INFORME FINAL OBSERVACION";
                        pObservacion.FechaCreacion = DateTime.Now;

                        _context.InformeFinalInterventoriaObservaciones.Add(pObservacion);

                        if (pObservacion.EsSupervision == true)
                        {
                            InformeFinalInterventoria informeFinalInterventoria = _context.InformeFinalInterventoria.Find(pObservacion.InformeFinalInterventoriaId);
                            /*InformeFinal informeFinal = _context.InformeFinal.Find(informeFinalInterventoria.InformeFinalId);
                            if (informeFinal.EstadoValidacion == ConstantCodigoEstadoValidacionInformeFinal.Con_informe_validado)
                            {
                                informeFinal.EstadoInforme = ConstantCodigoEstadoInformeFinal.Con_Observaciones_del_supervisor;
                                informeFinal.RegistroCompleto = false;
                            }*/
                            informeFinalInterventoria.TieneObservacionSupervisor = true;
                        }
                    }
                    else
                    {
                        strCrearEditar = "ACTUALIZAR OBSERVACION INFORME FINAL INTERVENTORIA";

                        await _context.Set<InformeFinalInterventoriaObservaciones>()
                                              .Where(r => r.InformeFinalInterventoriaObservacionesId == pObservacion.InformeFinalInterventoriaObservacionesId)
                                                                                                  .UpdateAsync(r => new InformeFinalInterventoriaObservaciones()
                                                                                                  {
                                                                                                    FechaModificacion = DateTime.Now,
                                                                                                    UsuarioModificacion = pObservacion.UsuarioCreacion,
                                                                                                    Observaciones = pObservacion.Observaciones,
                                                                                                  });
                        if (pObservacion.EsSupervision == true)
                        {
                            InformeFinalInterventoria informeFinalInterventoria = _context.InformeFinalInterventoria.Find(pObservacion.InformeFinalInterventoriaId);
                            /*InformeFinal informeFinal = _context.InformeFinal.Find(informeFinalInterventoria.InformeFinalId);
                            if (informeFinal.EstadoValidacion == ConstantCodigoEstadoValidacionInformeFinal.Con_informe_validado)
                            {
                                informeFinal.EstadoInforme = ConstantCodigoEstadoInformeFinal.Con_Observaciones_del_supervisor;
                                informeFinal.RegistroCompleto = false;
                            }*/
                            informeFinalInterventoria.TieneObservacionSupervisor = true;
                        }
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, pObservacion.UsuarioCreacion, strCrearEditar)
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarInformeFinalProyecto, GeneralCodes.Error, idAccion, pObservacion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        public async Task<Respuesta> SendFinalReportToInterventor(int pProyectoId, string pUsuario, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_A_Interventor_Devolucion_Informe_Final, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                InformeFinal informeFinal = _context.InformeFinal.Where(r => r.ProyectoId == pProyectoId)
                                                                 .Include(r => r.Proyecto)
                                                                 .FirstOrDefault();
                if (informeFinal != null)
                {
                    informeFinal.EstadoAprobacion = ConstantCodigoEstadoAprobacionInformeFinal.Devuelta_por_supervisor;
                    informeFinal.RegistroCompleto = false;
                    informeFinal.EstadoInforme = ConstantCodigoEstadoInformeFinal.Con_Observaciones_del_supervisor;
                    informeFinal.EstadoValidacion = ConstantCodigoEstadoValidacionInformeFinal.Con_observaciones_del_supervisor;
                    informeFinal.RegistroCompletoValidacion = false; 
                    informeFinal.UsuarioModificacion = pUsuario;
                    informeFinal.FechaModificacion = DateTime.Now;

                    //Djear en false la columna TieneModificacionInterventor en todos los items 
                    List<InformeFinalInterventoria> listAnexos = _context.InformeFinalInterventoria.Where(r => r.InformeFinalId == informeFinal.InformeFinalId).ToList();
                    foreach (var item in listAnexos)
                    {
                        item.FechaModificacion = DateTime.Now;
                        item.UsuarioModificacion = pUsuario;
                        item.TieneModificacionInterventor = false;
                    }

                    //Enviar Correo a interventor 5.1.3
                    await EnviarCorreoInterventor(informeFinal, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);

                    //Cambiar estado de validación
                    await updateStateValidation(informeFinal.InformeFinalId, pUsuario);
                }

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, pUsuario, "INFORME FINAL ")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarInformeFinalProyecto, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                };
            }
        }

        public async Task<Respuesta> SendFinalReportToFinalVerification(int pProyectoId, string pUsuario, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_Informe_Final_Ultima_Validacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                InformeFinal informeFinal = _context.InformeFinal.Where(r => r.ProyectoId == pProyectoId)
                                                                    .Include(r => r.Proyecto)
                                                                    .FirstOrDefault();
                
                if (informeFinal != null)
                {
                    informeFinal.EstadoAprobacion = ConstantCodigoEstadoAprobacionInformeFinal.Enviado_verificacion_liquidacion_novedades;
                    informeFinal.EstadoInforme = ConstantCodigoEstadoInformeFinal.Enviado_verificacion_liquidaciones_novedades; // control de cambios
                    informeFinal.EstadoValidacion = ConstantCodigoEstadoValidacionInformeFinal.Enviado_verificacion_liquidacion_novedades;//control de cambios
                    informeFinal.FechaEnvioGrupoNovedades = DateTime.Now;
                    informeFinal.UsuarioModificacion = pUsuario;
                    informeFinal.FechaAprobacion = DateTime.Now;
                    informeFinal.FechaModificacion = DateTime.Now;
                    if (informeFinal.EstadoCumplimiento == ConstantCodigoEstadoCumplimientoInformeFinal.Con_observaciones_liquidaciones_novedades)
                    {
                        informeFinal.EstadoCumplimiento = ConstantCodigoEstadoCumplimientoInformeFinal.Con_Ajustes_Supervisor;

                        //Enviar las observaciones del supervisor a historial

                        //Observaciones a recibo de satisfacción
                        List<InformeFinalObservaciones> informeFinalObservaciones = _context.InformeFinalObservaciones.Where(r => r.InformeFinalId == informeFinal.InformeFinalId && (r.EsGrupoNovedades == true || r.EsGrupoNovedadesInterventoria == true) && (r.Archivado == null || r.Archivado == false)).ToList();
                        foreach (var itemobs in informeFinalObservaciones)
                        {
                            itemobs.Archivado = true;
                            itemobs.FechaModificacion = DateTime.Now;
                            itemobs.UsuarioModificacion = pUsuario;
                        }
                        informeFinal.TieneObservacionesCumplimiento = null;
                        informeFinal.TieneObservacionesInterventoria = null;
                    }
                }

                //Enviar Correo a grupo novedades liquidaciones 5.1.3
                await EnviarCorreoGrupoNovedades(informeFinal, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);

                //Cambiar estado de validación
                await updateStateValidation(informeFinal.InformeFinalId, pUsuario);

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, pUsuario, "INFORME FINAL ")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarInformeFinalProyecto, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                };
            }
        }
        
        public async Task<InformeFinalInterventoriaObservaciones> GetInformeFinalInterventoriaObservacionByInformeFinalObservacion(int pObservacionId)
        {
            InformeFinalInterventoriaObservaciones informeFinalInterventoriaObservaciones = await _context.InformeFinalInterventoriaObservaciones.Where(r => r.InformeFinalInterventoriaObservacionesId == pObservacionId).FirstOrDefaultAsync();
            return informeFinalInterventoriaObservaciones;
        }

        public async Task<InformeFinalInterventoriaObservaciones> GetInformeFinalInterventoriaObservacionByInformeFinalInterventoria(int pInformeFinalInterventoriaId)
        {
            InformeFinalInterventoria informeFinalInterventoria = await _context.InformeFinalInterventoria.Where(r => r.InformeFinalInterventoriaId == pInformeFinalInterventoriaId).FirstOrDefaultAsync();
            //InformeFinal informeFinal = await _context.InformeFinal.Where(r => r.InformeFinalId == informeFinalInterventoria.InformeFinalId).FirstOrDefaultAsync();
             return await _context.InformeFinalInterventoriaObservaciones.Where(r => r.InformeFinalInterventoriaId == pInformeFinalInterventoriaId && r.EsSupervision == true).OrderByDescending(r => r.FechaCreacion).FirstOrDefaultAsync();
        }

        private async Task<bool> EnviarCorreoInterventor(InformeFinal informeFinal, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Interventor).Include(y => y.Usuario);

            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.NotificacionInterventorDevolucion5_1_3);

            string template = await ReplaceVariables(pDominioFront, TemplateRecoveryPassword.Contenido, informeFinal.InformeFinalId);

            bool blEnvioCorreo = false;

            foreach (var item in usuarios)
            {
                blEnvioCorreo = Helpers.Helpers.EnviarCorreo(item.Usuario.Email, TemplateRecoveryPassword.Asunto, template, pSender, pPassword, pMailServer, pMailPort);
            }
            return blEnvioCorreo;
        }

        private async Task<bool> EnviarCorreoGrupoNovedades(InformeFinal informeFinal, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Tecnica).Include(y => y.Usuario);

            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.NotificacionGrupoNovedadesInformeFinal5_1_3);

            string template = await ReplaceVariables(pDominioFront, TemplateRecoveryPassword.Contenido, informeFinal.InformeFinalId);

            bool blEnvioCorreo = false;

            foreach (var item in usuarios)
            {
                blEnvioCorreo = Helpers.Helpers.EnviarCorreo(item.Usuario.Email, TemplateRecoveryPassword.Asunto, template, pSender, pPassword, pMailServer, pMailPort);
            }
            return blEnvioCorreo;
        }

        //Alerta 5 días
        public async Task GetInformeFinalNoEnviadoAGrupoNovedades(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            DateTime RangoFechaConDiasHabiles = await _commonService.CalculardiasLaborales(5, DateTime.Now);

            List<InformeFinal> informeFinal = _context.InformeFinal
                .Where(r => r.EstadoValidacion == ConstantCodigoEstadoValidacionInformeFinal.Con_informe_enviado_al_supervisor && r.EstadoAprobacion != ConstantCodigoEstadoAprobacionInformeFinal.Enviado_verificacion_liquidacion_novedades)
                .Include(r => r.Proyecto)
                .ToList();

            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Apoyo || x.PerfilId == (int)EnumeratorPerfil.Supervisor).Include(y => y.Usuario);
            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.Alerta5DiasEnvioGrupoNovedades5_1_3);

            foreach (var informe in informeFinal)
            {

                if (informeFinal.Count() > 0 && informe.FechaEnvioSupervisor > RangoFechaConDiasHabiles)
                {
                    string template = await ReplaceVariables(pDominioFront, TemplateRecoveryPassword.Contenido, informe.InformeFinalId);

                    foreach (var item in usuarios)
                    {
                        Helpers.Helpers.EnviarCorreo(item.Usuario.Email, TemplateRecoveryPassword.Asunto, template, pSender, pPassword, pMailServer, pMailPort);
                    }

                }
            }
        }

        //Actualizar ValidacionCodigo == AprobacionCodigo
        private async Task<Respuesta> updateStateValidation(int informeFinalId, string user)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Actualizar_Estado_validacion_informe_final, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = string.Empty;
            try
            {
                List<InformeFinalInterventoria> informeFinalInterventoria = _context.InformeFinalInterventoria
                        .Where(r => r.InformeFinalId == informeFinalId).ToList();

                foreach (var item in informeFinalInterventoria)
                {
                    if (item.ValidacionCodigo != item.AprobacionCodigo)
                    {
                        await _context.Set<InformeFinalInterventoria>()
                                  .Where(r => r.InformeFinalInterventoriaId == item.InformeFinalInterventoriaId)
                                                      .UpdateAsync(r => new InformeFinalInterventoria()
                                                      {
                                                          FechaModificacion = DateTime.Now,
                                                          UsuarioModificacion = user,
                                                          ValidacionCodigo = item.AprobacionCodigo,//cumple,no aplica, no cumple
                                                                      });
                    }
                }

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, user, CreateEdit)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarInformeFinalProyecto, GeneralCodes.Error, idAccion, user, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<InformeFinal> GetListInformeFinalObservacionesInterventoria(int informeFinalId)
        {

            InformeFinal informeFinal = await _context.InformeFinal.FindAsync(informeFinalId);

            if (informeFinal != null)
            {
                informeFinal.HistorialInformeFinalObservacionesInterventoria = _context.InformeFinalObservaciones.Where(r => r.EsGrupoNovedadesInterventoria == true && r.Archivado == true && (r.EsApoyo == false || r.EsApoyo == null) && r.InformeFinalId == informeFinalId).ToList();
                informeFinal.ObservacionVigenteInformeFinalObservacionesInterventoria = _context.InformeFinalObservaciones.Where(r => r.EsGrupoNovedadesInterventoria == true && r.InformeFinalId == informeFinalId && (r.Archivado == false || r.Archivado == null)).FirstOrDefault();

            }

            return informeFinal;
        }

        private async Task<string> ReplaceVariables(string pDominioFront, string template, int pInformeFinalId)
        {
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();
            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();
            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();

            InformeFinal informeFinal = _context.InformeFinal
                .Where(r => r.InformeFinalId == pInformeFinalId)
                .Include(r => r.Proyecto)
                    .ThenInclude(r => r.InstitucionEducativa)
                .FirstOrDefault();

            InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == informeFinal.Proyecto.SedeId).FirstOrDefault();
            Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == informeFinal.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
            informeFinal.Proyecto.MunicipioObj = Municipio;
            informeFinal.Proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
            informeFinal.Proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == informeFinal.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
            informeFinal.Proyecto.Sede = Sede;

            ContratacionProyecto contratacionProyecto = _context.ContratacionProyecto
                .Where(r => r.ProyectoId == informeFinal.ProyectoId)
                .Include(r => r.Contratacion)
                    .ThenInclude(r => r.Contrato)
                .FirstOrDefault();

            template = template
                      .Replace("_LinkF_", pDominioFront)
                      .Replace("[LLAVE_MEN]", informeFinal.Proyecto.LlaveMen)
                      .Replace("[NUMERO_CONTRATO]", contratacionProyecto.Contratacion.Contrato.FirstOrDefault().NumeroContrato)
                      .Replace("[FECHA_VERIFICACION]", informeFinal.FechaEnvioSupervisor != null ?  ((DateTime)informeFinal.FechaEnvioSupervisor).ToString("dd-MMM-yy"): "")
                      .Replace("[FECHA_APROBACION]", informeFinal.FechaAprobacion != null ? ((DateTime)informeFinal.FechaAprobacion).ToString("dd-MMM-yy") : "")
                      .Replace("[FECHA_SUSCRIPCION]", informeFinal.FechaSuscripcion != null ? ((DateTime)informeFinal.FechaSuscripcion).ToString("dd-MMM-yy") : "")
                      .Replace("[INSTITUCION_EDUCATIVA]", informeFinal.Proyecto.InstitucionEducativa.Nombre)
                      .Replace("[SEDE]", informeFinal.Proyecto.Sede.Nombre)
                      .Replace("[TIPO_INTERVENCION]", informeFinal.Proyecto.tipoIntervencionString);


            return template;
        }

    }
}
