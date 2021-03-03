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
    public class VerifyFinalReportService : IVerifyFinalReportService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public VerifyFinalReportService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }

        public async Task<List<InformeFinal>> GetListInformeFinal()
        {
            List<InformeFinal> list = await _context.InformeFinal
                            .Where(r=> r.EstadoInforme == ConstantCodigoEstadoInformeFinal.Con_informe_enviado_para_validación || (r.EstadoValidacion != "0" && ! String.IsNullOrEmpty(r.EstadoValidacion)))
                            .Include(r=> r.Proyecto)
                                .ThenInclude(r => r.InstitucionEducativa)
                            .ToListAsync();
            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();
            List<Dominio> TipoObraIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar).ToList();

            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();

            foreach(var item in list)
            {
                InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == item.Proyecto.SedeId).FirstOrDefault();
                Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == item.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                item.Proyecto.MunicipioObj = Municipio;
                item.Proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                item.Proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == item.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
                item.Proyecto.Sede = Sede;
                if (String.IsNullOrEmpty(item.EstadoValidacion)|| item.EstadoValidacion == "0")
                {
                    item.EstadoValidacionString = "Sin verificación";
                }
                else
                {
                    item.EstadoValidacionString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.EstadoValidacion, (int)EnumeratorTipoDominio.Estado_Validacion_Informe_Final);
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
                                                            .ThenInclude(r => r.InformeFinalObservaciones)
                                                         .Include(r => r.InstitucionEducativa)
                                                         .FirstOrDefaultAsync();
            InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == proyecto.SedeId).FirstOrDefault();
            Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == proyecto.LocalizacionIdMunicipio).FirstOrDefault();
            proyecto.MunicipioObj = Municipio;
            proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
            proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
            proyecto.Sede = Sede;
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
                                .Include(r => r.InformeFinalAnexo)
                                .OrderBy(r => r.InformeFinalListaChequeo.Posicion)
                                .ToListAsync();
            InformeFinal informeFinal = _context.InformeFinal.Find(pInformeFinalId);

            if (informeFinal.EstadoValidacion == ConstantCodigoEstadoValidacionInformeFinal.En_proceso_de_validacion)
            {
                InformeFinalInterventoria no_seleccionado = _context.InformeFinalInterventoria.Where(r => r.InformeFinalId == informeFinal.InformeFinalId && (r.ValidacionCodigo != "0" && !String.IsNullOrEmpty(r.ValidacionCodigo))).FirstOrDefault();

                if (no_seleccionado == null)
                {
                    semaforo = true;
                }
            }

            foreach (var item in ListInformeFinalChequeo)
            {
                item.CalificacionCodigoString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.CalificacionCodigo, (int)EnumeratorTipoDominio.Calificacion_Informe_Final);
                item.ValidacionCodigoString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.ValidacionCodigo, (int)EnumeratorTipoDominio.Calificacion_Informe_Final);
                if (item.InformeFinalAnexoId != null)
                {
                    item.InformeFinalAnexo.TipoAnexoString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.InformeFinalAnexo.TipoAnexo, (int)EnumeratorTipoDominio.Tipo_Anexo_Informe_Final);
                }
                item.EstadoValidacion = informeFinal.EstadoValidacion;
                item.RegistroCompletoValidacion = informeFinal.RegistroCompletoValidacion == null ? false : (bool) informeFinal.RegistroCompletoValidacion;

                if (item.ValidacionCodigo == ConstantCodigoCalificacionInformeFinal.No_Cumple)
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
                }
                else
                {
                    informeFinalInterventoriaObservacionesId = 0;
                    tieneObservacionNoCumple = false;
                }
                item.ObservacionVigenteSupervisor = _context.InformeFinalInterventoriaObservaciones.Where(r => r.EsSupervision == true && r.InformeFinalInterventoriaId == item.InformeFinalInterventoriaId && (r.Archivado == false || r.Archivado == null)).FirstOrDefault();

                item.InformeFinalInterventoriaObservacionesId = informeFinalInterventoriaObservacionesId;
                item.TieneObservacionNoCumple = tieneObservacionNoCumple;
                item.Semaforo = semaforo;
            }
            return ListInformeFinalChequeo;
        }

        public async Task<bool> VerificarInformeFinalValidacion(int pInformeFinalId)
        {
            bool esCompleto = true;

            List<InformeFinalInterventoria> ListInformeTotalInterventoria = await _context.InformeFinalInterventoria.Where(cc => cc.InformeFinalId == pInformeFinalId)
                                                        .ToListAsync();

            InformeFinal informeFinal = _context.InformeFinal.Where(r => r.InformeFinalId == pInformeFinalId).FirstOrDefault();
            InformeFinalInterventoria existe_no_data = _context.InformeFinalInterventoria.Where(r => r.InformeFinalId == pInformeFinalId && r.ValidacionCodigo == null).FirstOrDefault();
            if (existe_no_data != null)
            {
                return false;
            }
            if (informeFinal.EstadoValidacion != ConstantCodigoEstadoValidacionInformeFinal.Enviado_correcciones_apoyo_supervisor)
            {

                informeFinal.EstadoValidacion = ConstantCodigoEstadoValidacionInformeFinal.Con_informe_validado;
                informeFinal.RegistroCompletoValidacion = true;
            }
            else
            {
                //Vuelve a empezar el flujo
                informeFinal.EstadoValidacion = ConstantCodigoEstadoValidacionInformeFinal.Modificado_Apoyo;
                informeFinal.RegistroCompletoValidacion = true;
                return true;
            }
            /*InformeFinalInterventoria existeObservacion = _context.InformeFinalInterventoria.Where(r=> r.InformeFinalId == informeFinal.InformeFinalId && r.TieneObservacionSupervisor == true).FirstOrDefault();
            
            if (existeObservacion != null )
            {
                informeFinal.EstadoInforme = ConstantCodigoEstadoInformeFinal.Con_Observaciones_del_supervisor;
                informeFinal.RegistroCompleto = false;
            }*/

            return esCompleto;
        }

        public async Task<Respuesta> UpdateStateValidateInformeFinalInterventoriaByInformeFinal(InformeFinal informeFinal, string user)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Actualizar_Estado_validacion_informe_final, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;

            try
            {

                foreach (InformeFinalInterventoria informeFinalInterventoria in informeFinal.InformeFinalInterventoria)
                {
                    if (informeFinalInterventoria.ValidacionCodigo != "0" && informeFinalInterventoria.ValidacionCodigo != null)
                    {
                        informeFinalInterventoria.UsuarioCreacion = user.ToUpper();
                        await this.UpdateStateValidateInformeFinalInterventoria(informeFinalInterventoria.InformeFinalInterventoriaId, informeFinalInterventoria.ValidacionCodigo, user);

                        if (informeFinalInterventoria.ValidacionCodigo == ConstantCodigoCalificacionInformeFinal.No_Cumple)
                        {
                            ///Actualiza o crea observaciones segun el caso (Sólo SUPERVISIÓN)
                            foreach (InformeFinalInterventoriaObservaciones informeFinalInterventoriaObservaciones in informeFinalInterventoria.InformeFinalInterventoriaObservaciones)
                            {
                                informeFinalInterventoriaObservaciones.UsuarioCreacion = user.ToUpper();
                                await this.CreateEditInformeFinalInterventoriaObservacion(informeFinalInterventoriaObservaciones);
                            }
                        }
                    }
                }

                await VerificarInformeFinalValidacion(informeFinal.InformeFinalId);

                await _context.SaveChangesAsync();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.VerificarInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, informeFinal.UsuarioCreacion, strCrearEditar)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.VerificarInformeFinalProyecto, GeneralCodes.Error, idAccion, informeFinal.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> UpdateStateValidateInformeFinalInterventoria(int pInformeFinalInterventoriaId,string code,string user)
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
                                                                        ValidacionCodigo = code,//cumple,no aplica, no cumple
                                                                      });
                informeFinal.EstadoValidacion = ConstantCodigoEstadoValidacionInformeFinal.En_proceso_de_validacion;

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.VerificarInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, user, CreateEdit)
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

        public async Task<Respuesta> CreateEditObservacionInformeFinal(InformeFinalObservaciones pObservacion, bool tieneObservacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Informe_Final_Observacion, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            try
            {
                if (pObservacion.InformeFinalObservacionesId == 0)
                {
                    await _context.Set<InformeFinal>().Where(r => r.InformeFinalId == pObservacion.InformeFinalId && (r.EstadoValidacion == "0" || string.IsNullOrEmpty(r.EstadoValidacion)))
                                                                   .UpdateAsync(r => new InformeFinal()
                                                                   {
                                                                       FechaModificacion = DateTime.Now,
                                                                       UsuarioModificacion = pObservacion.UsuarioCreacion,
                                                                       EstadoValidacion = ConstantCodigoEstadoValidacionInformeFinal.En_proceso_de_validacion,
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
                                                   TieneObservacionesValidacion = tieneObservacion,
                                               });
                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.VerificarInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, pObservacion.UsuarioCreacion, strCrearEditar)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.VerificarInformeFinalProyecto, GeneralCodes.Error, idAccion, pObservacion.UsuarioCreacion, ex.InnerException.ToString())
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.VerificarInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, pInformeFinal.UsuarioCreacion, CreateEdit)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.VerificarInformeFinalProyecto, GeneralCodes.Error, idAccion, pInformeFinal.UsuarioCreacion, ex.InnerException.ToString())
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
                if (pObservacion.EsApoyo == true)
                {
                    if (pObservacion.InformeFinalInterventoriaObservacionesId == 0)
                    {
                        strCrearEditar = "CREAR INFORME FINAL OBSERVACION";
                        pObservacion.FechaCreacion = DateTime.Now;

                        _context.InformeFinalInterventoriaObservaciones.Add(pObservacion);

                        /*if (pObservacion.EsApoyo == true)
                        {
                            InformeFinalInterventoria informeFinalInterventoria = _context.InformeFinalInterventoria.Find(pObservacion.InformeFinalInterventoriaId);
                            InformeFinal informeFinal = _context.InformeFinal.Find(informeFinalInterventoria.InformeFinalId);
                            if (informeFinal.EstadoValidacion == ConstantCodigoEstadoValidacionInformeFinal.Con_informe_validado)
                            {
                                informeFinal.EstadoInforme = ConstantCodigoEstadoInformeFinal.Con_Observaciones_del_supervisor;
                                informeFinal.RegistroCompleto = false;
                            }
                            informeFinalInterventoria.TieneObservacionSupervisor = true;
                        }*/
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
                        /*if (pObservacion.EsSupervision == true)
                        {
                            InformeFinalInterventoria informeFinalInterventoria = _context.InformeFinalInterventoria.Find(pObservacion.InformeFinalInterventoriaId);
                            InformeFinal informeFinal = _context.InformeFinal.Find(informeFinalInterventoria.InformeFinalId);
                            if (informeFinal.EstadoValidacion == ConstantCodigoEstadoValidacionInformeFinal.Con_informe_validado)
                            {
                                informeFinal.EstadoInforme = ConstantCodigoEstadoInformeFinal.Con_Observaciones_del_supervisor;
                                informeFinal.RegistroCompleto = false;
                            }
                            informeFinalInterventoria.TieneObservacionSupervisor = true;
                        }*/
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.VerificarInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, pObservacion.UsuarioCreacion, strCrearEditar)
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

        public async Task<Respuesta> SendFinalReportToSupervision(int pProyectoId, string pUsuario, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_A_supervisor_Informe_Final_Validacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                InformeFinal informeFinal = _context.InformeFinal.Where(r => r.ProyectoId == pProyectoId)
                                                                  .Include(r => r.Proyecto)
                                                                  .FirstOrDefault();
                if (informeFinal != null)
                {
                    informeFinal.EstadoInforme = ConstantCodigoEstadoInformeFinal.Informe_enviado_validacion; //control de cambios
                    informeFinal.EstadoValidacion = ConstantCodigoEstadoValidacionInformeFinal.Con_informe_enviado_al_supervisor;
                    informeFinal.FechaEnvioSupervisor = DateTime.Now;
                    informeFinal.UsuarioModificacion = pUsuario;
                    informeFinal.FechaModificacion = DateTime.Now;
                    if (informeFinal.EstadoAprobacion == ConstantCodigoEstadoAprobacionInformeFinal.Devuelta_por_supervisor)
                    {
                        informeFinal.EstadoAprobacion = ConstantCodigoEstadoAprobacionInformeFinal.Modificado_Apoyo_Supervision_Interventor;

                        //Enviar las observaciones del supervisor a historial

                        //Observaciones a recibo de satisfacción
                        List<InformeFinalObservaciones> informeFinalObservaciones = _context.InformeFinalObservaciones.Where(r => r.InformeFinalId == informeFinal.InformeFinalId && r.EsSupervision == true && (r.Archivado == null || r.Archivado == false)).ToList();
                        foreach (var itemobs in informeFinalObservaciones)
                        {
                            itemobs.Archivado = true;
                            itemobs.FechaModificacion = DateTime.Now;
                            itemobs.UsuarioModificacion = pUsuario;
                        }

                        //Observaciones lista interventoria
                        List<InformeFinalInterventoria> listanexo = _context.InformeFinalInterventoria.Where(r => r.InformeFinalId == informeFinal.InformeFinalId).ToList();
                        foreach (var item in listanexo)
                        {
                            item.TieneObservacionSupervisor = false;
                            List<InformeFinalInterventoriaObservaciones> listobs = _context.InformeFinalInterventoriaObservaciones.Where(r => r.InformeFinalInterventoriaId == item.InformeFinalInterventoriaId && r.EsSupervision == true && (r.Archivado == null || r.Archivado == false)).ToList();
                            foreach (var itemobs in listobs)
                            {
                                itemobs.Archivado = true;
                                itemobs.FechaModificacion = DateTime.Now;
                                itemobs.UsuarioModificacion = pUsuario;
                            }
                        }
                    }
                    //Enviar Correo supervisor 5.1.2
                    await EnviarCorreoSupervisor(informeFinal, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);
                }

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.VerificarInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, pUsuario, "INFORME FINAL ")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.VerificarInformeFinalProyecto, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                };
            }
        }

        public async Task<Respuesta> ApproveInformeFinal(int pInformeFinalId, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_A_supervisor_Informe_Final_Validacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                InformeFinal informeFinal = _context.InformeFinal.Where(r => r.InformeFinalId == pInformeFinalId).FirstOrDefault();
                if (informeFinal != null)
                {
                    informeFinal.EstadoValidacion = ConstantCodigoEstadoValidacionInformeFinal.Con_aprobacion_del_supervisor;
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.VerificarInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, pUsuario, "INFORME FINAL ")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.VerificarInformeFinalProyecto, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                };
            }
        }

        public async Task<Respuesta> NoApprovedInformeFinal(int pInformeFinalId, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_A_supervisor_Informe_Final_Validacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                InformeFinal informeFinal = _context.InformeFinal.Where(r => r.InformeFinalId == pInformeFinalId).FirstOrDefault();
                if (informeFinal != null)
                {
                    informeFinal.EstadoValidacion = ConstantCodigoEstadoValidacionInformeFinal.Con_observaciones_del_supervisor;
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.VerificarInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, pUsuario, "INFORME FINAL ")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.VerificarInformeFinalProyecto, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
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

        private async Task<bool> EnviarCorreoSupervisor(InformeFinal informeFinal, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Supervisor).Include(y => y.Usuario);

            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.NotificacionSupervisionInformeFinal5_1_2);

            string template = TemplateRecoveryPassword.Contenido
                .Replace("_LinkF_", pDominioFront)
                .Replace("[LLAVE_MEN]", informeFinal.Proyecto.LlaveMen)
                .Replace("[FECHA_VERIFICACION]", ((DateTime) informeFinal.FechaEnvioSupervisor).ToString("dd-MMM-yy"));

            bool blEnvioCorreo = false;

            foreach (var item in usuarios)
            {
                blEnvioCorreo = Helpers.Helpers.EnviarCorreo(item.Usuario.Email, "Validar informe final", template, pSender, pPassword, pMailServer, pMailPort);
            }
            return blEnvioCorreo;
        }


        //Alerta 5 días
        public async Task GetInformeFinalNoEnviadoASupervisor(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            DateTime RangoFechaConDiasHabiles = await _commonService.CalculardiasLaborales(5, DateTime.Now);

            List<InformeFinal> informeFinal = _context.InformeFinal
                .Where(r => r.EstadoInforme == ConstantCodigoEstadoInformeFinal.Con_informe_enviado_para_validación && r.EstadoValidacion != ConstantCodigoEstadoValidacionInformeFinal.Con_informe_enviado_al_supervisor)
                .Include(r => r.Proyecto)
                .ToList();

            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Apoyo || x.PerfilId == (int)EnumeratorPerfil.Supervisor).Include(y => y.Usuario);
            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.Alerta5DiasEnvioSupervisor5_1_2);

            foreach (var informe in informeFinal)
            {

                if (informeFinal.Count() > 0 && informe.FechaEnvioApoyoSupervisor > RangoFechaConDiasHabiles)
                {
                    string template = TemplateRecoveryPassword.Contenido
                                .Replace("_LinkF_", pDominioFront)
                                .Replace("[LLAVE_MEN]", informe.Proyecto.LlaveMen)
                                .Replace("[ESTADO_VALIDACION]", String.IsNullOrEmpty(informe.EstadoValidacion) ? "Sin Verificación" : await _commonService.GetNombreDominioByCodigoAndTipoDominio(informe.EstadoValidacion, (int)EnumeratorTipoDominio.Estado_Validacion_Informe_Final))
                                .Replace("[FECHA_ENVIO_APOYO]", ((DateTime) informe.FechaEnvioApoyoSupervisor).ToString("yyyy-MM-dd"));

                    foreach (var item in usuarios)
                    {
                        Helpers.Helpers.EnviarCorreo(item.Usuario.Email, "No se ha enviado informe Final para revisión del supervisor", template, pSender, pPassword, pMailServer, pMailPort);
                    }

                }           
            }
        }

        
    }
}
