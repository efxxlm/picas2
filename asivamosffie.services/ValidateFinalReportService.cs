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
                            .Where(r=> r.EstadoValidacion == ConstantCodigoEstadoValidacionInformeFinal.Con_informe_enviado_al_supervisor)
                            .Include(r=> r.Proyecto)
                                .ThenInclude(r => r.InstitucionEducativa)
                            .ToListAsync();
            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();
            //List<Dominio> TipoObraIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar).ToList();

            //List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();

            foreach(var item in list)
            {
                InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == item.Proyecto.SedeId).FirstOrDefault();
                //Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == item.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                //item.Proyecto.MunicipioObj = Municipio;
                //item.Proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                item.Proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == item.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
                item.Proyecto.Sede = Sede;
                if (String.IsNullOrEmpty(item.EstadoAprobacion)|| item.EstadoAprobacion == "0")
                {
                    item.EstadoAprobacionString = "Sin validación";
                }
                else
                {
                    item.EstadoAprobacionString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.EstadoAprobacion, 161);
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
            List<InformeFinalObservaciones> informeFinalObservacionesSupervisor = _context.InformeFinalObservaciones.Where(r => r.InformeFinalId == proyecto.InformeFinal.FirstOrDefault().InformeFinalId && r.EsSupervision == true).ToList();

            InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == proyecto.SedeId).FirstOrDefault();
            Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == proyecto.LocalizacionIdMunicipio).FirstOrDefault();
            proyecto.MunicipioObj = Municipio;
            proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
            proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
            proyecto.Sede = Sede;
            proyecto.InformeFinal.FirstOrDefault().InformeFinalObservaciones = informeFinalObservacionesApoyo;
            proyecto.InformeFinal.FirstOrDefault().InformeFinalObservacionesSupervisor = informeFinalObservacionesSupervisor;
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

            List<InformeFinalInterventoria> ListInformeFinalChequeo = await _context.InformeFinalInterventoria
                                .Where(r => r.InformeFinalId == pInformeFinalId)
                                .Include(r => r.InformeFinalListaChequeo)
                                .Include(r => r.InformeFinalAnexo)
                                .OrderBy(r => r.InformeFinalListaChequeo.Posicion)
                                .ToListAsync();
            InformeFinal informeFinal = _context.InformeFinal.Find(pInformeFinalId);
            foreach(var item in ListInformeFinalChequeo)
            {
                item.CalificacionCodigoString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.CalificacionCodigo, 151);
                item.ValidacionCodigoString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.ValidacionCodigo, 151);
                item.AprobacionCodigoString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.AprobacionCodigo, 151);

                if (item.InformeFinalAnexoId != null)
                {
                    item.InformeFinalAnexo.TipoAnexoString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.InformeFinalAnexo.TipoAnexo, 155);
                }
                item.EstadoValidacion = informeFinal.EstadoValidacion;
                item.RegistroCompletoValidacion = (bool) informeFinal.RegistroCompletoValidacion;
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
                    InformeFinalInterventoriaObservaciones informeFinalInterventoriaObservaciones = _context.InformeFinalInterventoriaObservaciones.Where(r => r.InformeFinalInterventoriaId == item.InformeFinalInterventoriaId && r.EsSupervision == true).FirstOrDefault();
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

                item.InformeFinalInterventoriaObservacionesId = informeFinalInterventoriaObservacionesId;
                item.TieneObservacionNoCumple = tieneObservacionNoCumple;
            }
            return ListInformeFinalChequeo;
        }

        private bool VerificarInformeFinalAprobacion(int pInformeFinalId)
        {
            bool esCompleto = false;

            InformeFinal informeFinal = _context.InformeFinal.Where(r => r.InformeFinalId == pInformeFinalId).FirstOrDefault();
            //Validación # 1
            if (informeFinal != null)
            {
                InformeFinalInterventoria existe_no_cumple = _context.InformeFinalInterventoria.Where(r => r.InformeFinalId == pInformeFinalId && r.AprobacionCodigo == ConstantCodigoCalificacionInformeFinal.No_Cumple).FirstOrDefault();
                InformeFinalInterventoria existe_no_diligenciado = _context.InformeFinalInterventoria.Where(r => r.InformeFinalId == pInformeFinalId && r.AprobacionCodigo == "0" || String.IsNullOrEmpty(r.AprobacionCodigo)).FirstOrDefault();
                
                if (existe_no_cumple != null)
                {
                    informeFinal.EstadoAprobacion = ConstantCodigoEstadoAprobacionInformeFinal.Con_observaciones_supervisor;
                    return false;
                }
                if (existe_no_diligenciado != null)
                {
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
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Informe_Final_Interventoria, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;

            try
            {

                foreach (InformeFinalInterventoria informeFinalInterventoria in informeFinal.InformeFinalInterventoria)
                {
                    informeFinalInterventoria.UsuarioCreacion = user.ToUpper();
                    informeFinalInterventoria.TieneObservacionSupervisor = false;
                    await this.UpdateStateApproveInformeFinalInterventoria(informeFinalInterventoria.InformeFinalInterventoriaId, informeFinalInterventoria.AprobacionCodigo,user);
                    
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

                VerificarInformeFinalAprobacion(informeFinal.InformeFinalId);

                await _context.SaveChangesAsync();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.OperacionExitosa, idAccion, informeFinal.UsuarioCreacion, strCrearEditar)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.Error, idAccion, informeFinal.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> UpdateStateApproveInformeFinalInterventoria(int pInformeFinalInterventoriaId,string code,string user)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Actualizar_Estado_validacion_informe_final, (int)EnumeratorTipoDominio.Acciones);
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.OperacionExitosa, idAccion, user, CreateEdit)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.Error, idAccion, user, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> CreateEditObservacionInformeFinal(InformeFinalObservaciones pObservacion, bool tieneOBservaciones)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Informe_Final_Observacion, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            try
            {
                if (pObservacion.InformeFinalObservacionesId == 0)
                {
                    await _context.Set<InformeFinal>().Where(r => r.InformeFinalId == pObservacion.InformeFinalId && (r.EstadoAprobacion == "0" || string.IsNullOrEmpty(r.EstadoAprobacion)))
                                               .UpdateAsync(r => new InformeFinal()
                                               {
                                                   FechaModificacion = DateTime.Now,
                                                   UsuarioModificacion = pObservacion.UsuarioCreacion,
                                                   EstadoAprobacion = ConstantCodigoEstadoAprobacionInformeFinal.En_proceso_aprobacion,
                                               });
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

        public async Task<Respuesta> CreateEditInformeFinalInterventoriaObservacion(InformeFinalInterventoriaObservaciones pObservacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Informe_Final_Interventoria_Observacion, (int)EnumeratorTipoDominio.Acciones);

            Respuesta respuesta = new Respuesta();
            try
            {
                string strCrearEditar = string.Empty;
                if (pObservacion.EsSupervision == true)
                {
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

        public async Task<Respuesta> SendFinalReportToInterventor(int pProyectoId, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_A_supervisor_Informe_Final_Validacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                InformeFinal informeFinal = _context.InformeFinal.Where(r => r.ProyectoId == pProyectoId).FirstOrDefault();
                if (informeFinal != null)
                {
                    informeFinal.EstadoAprobacion = ConstantCodigoEstadoAprobacionInformeFinal.Devuelta_por_supervisor;
                    informeFinal.EstadoInforme = ConstantCodigoEstadoInformeFinal.Con_Observaciones_del_supervisor;
                    informeFinal.UsuarioModificacion = pUsuario;
                    informeFinal.FechaModificacion = DateTime.Now;
                }

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

        public async Task<Respuesta> SendFinalReportToFinalVerification(int pProyectoId, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_A_supervisor_Informe_Final_Validacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                InformeFinal informeFinal = _context.InformeFinal.Where(r => r.ProyectoId == pProyectoId).FirstOrDefault();
                if (informeFinal != null)
                {
                    informeFinal.EstadoAprobacion = ConstantCodigoEstadoAprobacionInformeFinal.Enviado_verificacion_liquidacion_novedades;
                    informeFinal.UsuarioModificacion = pUsuario;
                    informeFinal.FechaModificacion = DateTime.Now;
                }

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
    }
}
