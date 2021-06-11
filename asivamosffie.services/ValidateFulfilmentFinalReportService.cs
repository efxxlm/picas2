using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class ValidateFulfilmentFinalReportService : IValidateFulfilmentFinalReportService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public ValidateFulfilmentFinalReportService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }

        public async Task<List<InformeFinal>> GetListInformeFinal()
        {
            List<InformeFinal> list = await _context.InformeFinal
                            .Where(r => r.EstadoAprobacion == ConstantCodigoEstadoAprobacionInformeFinal.Enviado_verificacion_liquidacion_novedades || r.EstadoAprobacion == ConstantCodigoEstadoAprobacionInformeFinal.Con_observaciones_liquidaciones_novedades || !String.IsNullOrEmpty(r.EstadoCumplimiento))
                            .Include(r => r.Proyecto)
                                .ThenInclude(r => r.InstitucionEducativa)
                            .ToListAsync();
            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();

            foreach (var item in list)
            {
                InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == item.Proyecto.SedeId).FirstOrDefault();
                item.Proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == item.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
                item.Proyecto.Sede = Sede;
                if (String.IsNullOrEmpty(item.EstadoCumplimiento) || item.EstadoCumplimiento == "0")
                {
                    item.EstadoCumplimientoString = "Sin validación";
                }
                else
                {
                    item.EstadoCumplimientoString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.EstadoCumplimiento, (int)EnumeratorTipoDominio.Estado_validacion_cumplimiento_informe_Final);
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
            List<InformeFinalObservaciones> informeFinalObservacionesCumplimiento = _context.InformeFinalObservaciones.Where(r => r.InformeFinalId == proyecto.InformeFinal.FirstOrDefault().InformeFinalId && r.EsGrupoNovedades == true && (r.Archivado == null || r.Archivado == false) && (r.Eliminado == false || r.Eliminado == null)).ToList();
            List<InformeFinalObservaciones> informeFinalObservacionesInterventoria = _context.InformeFinalObservaciones.Where(r => r.InformeFinalId == proyecto.InformeFinal.FirstOrDefault().InformeFinalId && r.EsGrupoNovedadesInterventoria == true && (r.Archivado == null || r.Archivado == false) && (r.Eliminado == false || r.Eliminado == null)).ToList();

            InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == proyecto.SedeId).FirstOrDefault();
            Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == proyecto.LocalizacionIdMunicipio).FirstOrDefault();
            proyecto.MunicipioObj = Municipio;
            proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
            proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
            proyecto.Sede = Sede;
            if (proyecto.InformeFinal.Count > 0)
            {
                proyecto.InformeFinal.FirstOrDefault().InformeFinalObservacionesCumplimiento = informeFinalObservacionesCumplimiento;
                proyecto.InformeFinal.FirstOrDefault().InformeFinalObservacionesInterventoria = informeFinalObservacionesInterventoria;
                //historial recibo de satisfacción (EsGrupoNovedades )
                proyecto.InformeFinal.FirstOrDefault().HistorialObsInformeFinalNovedades = _context.InformeFinalObservaciones.Where(r => r.EsGrupoNovedades == true && r.Archivado == true && (r.EsApoyo == false || r.EsApoyo == null) && r.InformeFinalId == proyecto.InformeFinal.FirstOrDefault().InformeFinalId && (r.Eliminado == false || r.Eliminado == null)).OrderByDescending(r => r.FechaCreacion).ToList();
                proyecto.InformeFinal.FirstOrDefault().ObservacionVigenteInformeFinalNovedades = _context.InformeFinalObservaciones.Where(r => r.EsGrupoNovedades == true && r.InformeFinalId == proyecto.InformeFinal.FirstOrDefault().InformeFinalId && (r.Archivado == false || r.Archivado == null) && (r.Eliminado == false || r.Eliminado == null)).FirstOrDefault();
                //historial de anexos (EsGrupoNovedadesInterventoria)
                proyecto.InformeFinal.FirstOrDefault().HistorialObsInformeFinalInterventoriaNovedades = _context.InformeFinalObservaciones.Where(r => r.EsGrupoNovedadesInterventoria == true && r.Archivado == true && (r.EsApoyo == false || r.EsApoyo == null) && r.InformeFinalId == proyecto.InformeFinal.FirstOrDefault().InformeFinalId && (r.Eliminado == false || r.Eliminado == null)).ToList();
                foreach (var item in proyecto.InformeFinal.FirstOrDefault().HistorialObsInformeFinalInterventoriaNovedades)
                {
                    Usuario user = _context.Usuario.Where(r => r.Email == item.UsuarioCreacion).FirstOrDefault();
                    item.NombreResponsable = user.PrimerNombre + " " + user.PrimerApellido;
                }
                proyecto.InformeFinal.FirstOrDefault().ObservacionVigenteInformeFinalInterventoriaNovedades = _context.InformeFinalObservaciones.Where(r => r.EsGrupoNovedadesInterventoria == true && r.InformeFinalId == proyecto.InformeFinal.FirstOrDefault().InformeFinalId && (r.Archivado == false || r.Archivado == null) && (r.Eliminado == false || r.Eliminado == null)).FirstOrDefault();

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

            List<InformeFinalInterventoria> ListInformeFinalChequeo = await _context.InformeFinalInterventoria
                                .Where(r => r.InformeFinalId == pInformeFinalId)
                                .Include(r => r.InformeFinalListaChequeo)
                                    .ThenInclude(r => r.ListaChequeoItem)
                                .Include(r => r.InformeFinalAnexo)
                                .OrderBy(r => r.InformeFinalListaChequeo.Orden)
                                .ToListAsync();
            InformeFinal informeFinal = _context.InformeFinal.Find(pInformeFinalId);

            foreach (var item in ListInformeFinalChequeo)
            {
                item.AprobacionCodigoString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.AprobacionCodigo, (int)EnumeratorTipoDominio.Calificacion_Informe_Final);

                if (item.InformeFinalAnexoId != null)
                {
                    item.InformeFinalAnexo.TipoAnexoString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.InformeFinalAnexo.TipoAnexo, (int)EnumeratorTipoDominio.Tipo_Anexo_Informe_Final);
                }
            }
            return ListInformeFinalChequeo;
        }


        public async Task<Respuesta> CreateEditObservacionInformeFinal(InformeFinalObservaciones pObservacion, bool tieneOBservaciones)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Informe_Final_Observacion, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            try
            {
                if (pObservacion.InformeFinalObservacionesId == 0)
                {
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
                                                   TieneObservacionesCumplimiento = tieneOBservaciones,
                                                   EstadoCumplimiento = ConstantCodigoEstadoCumplimientoInformeFinal.En_proceso_validacion_cumplimiento,
                                               });
                _context.SaveChanges();

                await _context.Set<InformeFinal>().Where(r => r.InformeFinalId == pObservacion.InformeFinalId)
                               .UpdateAsync(r => new InformeFinal()
                               {
                                   RegistroCompletoCumplimiento = validateRegistroCompleto(pObservacion.InformeFinalId)
                               });

                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarCumplimientoInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, pObservacion.UsuarioCreacion, strCrearEditar)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarCumplimientoInformeFinalProyecto, GeneralCodes.Error, idAccion, pObservacion.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> CreateEditObservacionInformeFinalInterventoria(InformeFinalObservaciones pObservacion, bool tieneOBservaciones)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Informe_Final_Observacion, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            try
            {
                if (pObservacion.InformeFinalObservacionesId == 0)
                {
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
                                                   TieneObservacionesInterventoria = tieneOBservaciones,
                                                   EstadoCumplimiento = ConstantCodigoEstadoCumplimientoInformeFinal.En_proceso_validacion_cumplimiento,
                                               });
                _context.SaveChanges();

                await _context.Set<InformeFinal>().Where(r => r.InformeFinalId == pObservacion.InformeFinalId)
                               .UpdateAsync(r => new InformeFinal()
                               {
                                   RegistroCompletoCumplimiento = validateRegistroCompleto(pObservacion.InformeFinalId),
                               });
                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarCumplimientoInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, pObservacion.UsuarioCreacion, strCrearEditar)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarCumplimientoInformeFinalProyecto, GeneralCodes.Error, idAccion, pObservacion.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> SendFinalReportToSupervision(int pProyectoId, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_A_supervisor_Devolucion_Cumplimiento_Informe_Final, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                InformeFinal informeFinal = _context.InformeFinal.Where(r => r.ProyectoId == pProyectoId)
                                                                 .Include(r => r.Proyecto)
                                                                 .FirstOrDefault();
                if (informeFinal != null)
                {
                    informeFinal.EstadoAprobacion = ConstantCodigoEstadoAprobacionInformeFinal.Con_observaciones_liquidaciones_novedades;
                    informeFinal.EstadoInforme = ConstantCodigoEstadoInformeFinal.Con_observaciones_liquidaciones_novedades; // control de cambios
                    informeFinal.EstadoValidacion = ConstantCodigoEstadoValidacionInformeFinal.Con_observaciones_liquidaciones_novedades; // control de cambios
                    informeFinal.EstadoCumplimiento = ConstantCodigoEstadoCumplimientoInformeFinal.Con_observaciones_liquidaciones_novedades;
                    informeFinal.UsuarioModificacion = pUsuario;
                    informeFinal.FechaModificacion = DateTime.Now;

                    //Enviar Correo a supervisor 5.1.4
                    await EnviarCorreoSupervisor(informeFinal);

                }

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarCumplimientoInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, pUsuario, "INFORME FINAL ")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarCumplimientoInformeFinalProyecto, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                };
            }
        }

        public async Task<Respuesta> ApproveFinalReportByFulfilment(int pProyectoId, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Aprobar_Informe_Final_Cumplimiento, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                InformeFinal informeFinal = _context.InformeFinal.Where(r => r.ProyectoId == pProyectoId).FirstOrDefault();
                if (informeFinal != null)
                {
                    informeFinal.FechaAprobacionFinal = DateTime.Now;
                    informeFinal.EstadoCumplimiento = ConstantCodigoEstadoCumplimientoInformeFinal.Con_Aprobacion_final;
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarCumplimientoInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, pUsuario, "INFORME FINAL ")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarCumplimientoInformeFinalProyecto, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                };
            }
        }

        private async Task<bool> EnviarCorreoSupervisor(InformeFinal informeFinal)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.NotificacionSupervisorDevolucion5_1_4));
            string strContenido = await ReplaceVariables(template.Contenido, informeFinal.InformeFinalId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Supervisor
                                          };
            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
        }

        //Alerta 5 días
        public async Task<bool> GetInformeFinalNoCumplimiento()
        {
            DateTime RangoFechaConDiasHabiles = await _commonService.CalculardiasLaborales(5, DateTime.Now);

            List<InformeFinal> informeFinal = _context.InformeFinal
                .Where(r => r.EstadoAprobacion == ConstantCodigoEstadoAprobacionInformeFinal.Enviado_verificacion_liquidacion_novedades && (String.IsNullOrEmpty(r.EstadoCumplimiento) || r.EstadoCumplimiento == ConstantCodigoEstadoCumplimientoInformeFinal.En_proceso_validacion_cumplimiento))
                .Include(r => r.Proyecto)
                .ToList();

            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Supervisor || x.PerfilId == (int)EnumeratorPerfil.Tecnica).Include(y => y.Usuario);
            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.Alerta5DiasGrupoNovedades5_1_4);
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Alerta_5_1_6_registro_solicitud_liquidacion_contrato));
            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                        {
                                            EnumeratorPerfil.Tecnica,
                                            EnumeratorPerfil.Supervisor
                                        };
            bool SedndIsSuccessfull = true;
            foreach (var informe in informeFinal)
            {

                if (informeFinal.Count() > 0 && informe.FechaEnvioGrupoNovedades > RangoFechaConDiasHabiles)
                {
                    string strContenido = await ReplaceVariables(template.Contenido, informe.InformeFinalId);


                    if (!_commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto))
                        SedndIsSuccessfull = false;

                }
            }
            return SedndIsSuccessfull;
        }

        private bool validateRegistroCompleto(int pInformeFinalId)
        {
            InformeFinal informeFinal = _context.InformeFinal.Find(pInformeFinalId);

            if (informeFinal != null)
            {
                if (informeFinal.TieneObservacionesCumplimiento == null || informeFinal.TieneObservacionesInterventoria == null)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private async Task<string> ReplaceVariables(string template, int pInformeFinalId)
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
                      .Replace("[LLAVE_MEN]", informeFinal.Proyecto.LlaveMen)
                      .Replace("[NUMERO_CONTRATO]", contratacionProyecto.Contratacion.Contrato.FirstOrDefault().NumeroContrato)
                      .Replace("[FECHA_VERIFICACION]", informeFinal.FechaEnvioSupervisor != null ? ((DateTime)informeFinal.FechaEnvioSupervisor).ToString("dd-MMM-yy") : "")
                      .Replace("[FECHA_APROBACION]", informeFinal.FechaAprobacion != null ? ((DateTime)informeFinal.FechaAprobacion).ToString("dd-MMM-yy") : "")
                      .Replace("[FECHA_SUSCRIPCION]", informeFinal.FechaSuscripcion != null ? ((DateTime)informeFinal.FechaSuscripcion).ToString("dd-MMM-yy") : "")
                      .Replace("[INSTITUCION_EDUCATIVA]", informeFinal.Proyecto.InstitucionEducativa.Nombre)
                      .Replace("[SEDE]", informeFinal.Proyecto.Sede.Nombre)
                      .Replace("[TIPO_INTERVENCION]", informeFinal.Proyecto.tipoIntervencionString)
                      .Replace("[FECHA_ENVIO_NOVEDADES]", informeFinal.FechaEnvioGrupoNovedades != null ? ((DateTime)informeFinal.FechaEnvioGrupoNovedades).ToString("yyyy-MM-dd") : "");
            ;


            return template;
        }
    }
}
