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
                            .Where(r=> r.EstadoAprobacion == ConstantCodigoEstadoAprobacionInformeFinal.Enviado_verificacion_liquidacion_novedades || r.EstadoAprobacion == ConstantCodigoEstadoAprobacionInformeFinal.Con_observaciones_liquidaciones_novedades || r.EstadoCumplimiento == ConstantCodigoEstadoCumplimientoInformeFinal.Con_observaciones_liquidaciones_novedades)
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
                if (String.IsNullOrEmpty(item.EstadoCumplimiento)|| item.EstadoCumplimiento == "0")
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
            List<InformeFinalObservaciones> informeFinalObservacionesCumplimiento = _context.InformeFinalObservaciones.Where(r => r.InformeFinalId == proyecto.InformeFinal.FirstOrDefault().InformeFinalId && r.EsGrupoNovedades == true).ToList();
            List<InformeFinalObservaciones> informeFinalObservacionesInterventoria = _context.InformeFinalObservaciones.Where(r => r.InformeFinalId == proyecto.InformeFinal.FirstOrDefault().InformeFinalId && r.EsGrupoNovedadesInterventoria == true).ToList();

            InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == proyecto.SedeId).FirstOrDefault();
            Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == proyecto.LocalizacionIdMunicipio).FirstOrDefault();
            proyecto.MunicipioObj = Municipio;
            proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
            proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
            proyecto.Sede = Sede;
            proyecto.InformeFinal.FirstOrDefault().InformeFinalObservaciones = informeFinalObservacionesCumplimiento;
            proyecto.InformeFinal.FirstOrDefault().InformeFinalObservacionesInterventoria = informeFinalObservacionesInterventoria;
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
                                .Include(r => r.InformeFinalAnexo)
                                .OrderBy(r => r.InformeFinalListaChequeo.Posicion)
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
                    await _context.Set<InformeFinal>().Where(r => r.InformeFinalId == pObservacion.InformeFinalId && (r.EstadoCumplimiento == "0" || string.IsNullOrEmpty(r.EstadoCumplimiento)))
                                               .UpdateAsync(r => new InformeFinal()
                                               {
                                                   FechaModificacion = DateTime.Now,
                                                   UsuarioModificacion = pObservacion.UsuarioCreacion,
                                                   EstadoCumplimiento = ConstantCodigoEstadoCumplimientoInformeFinal.En_proceso_validacion_cumplimiento,
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
                                                   TieneObservacionesCumplimiento = tieneOBservaciones,
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

        public async Task<Respuesta> SendFinalReportToSupervision(int pProyectoId, string pUsuario, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
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
                    await EnviarCorreoSupervisor(informeFinal, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);

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

        private async Task<bool> EnviarCorreoSupervisor(InformeFinal informeFinal, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Supervisor).Include(y => y.Usuario);

            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.NotificacionSupervisorDevolucion5_1_4);

            string template = TemplateRecoveryPassword.Contenido
                .Replace("_LinkF_", pDominioFront)
                .Replace("[LLAVE_MEN]", informeFinal.Proyecto.LlaveMen);

            bool blEnvioCorreo = false;

            foreach (var item in usuarios)
            {
                blEnvioCorreo = Helpers.Helpers.EnviarCorreo(item.Usuario.Email, "Devolución por Grupo Novedades y/o liquidación - Informe Final", template, pSender, pPassword, pMailServer, pMailPort);
            }

            return blEnvioCorreo;
        }

        //Alerta 5 días
        public async Task GetInformeFinalNoCumplimiento(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            DateTime RangoFechaConDiasHabiles = await _commonService.CalculardiasLaborales(5, DateTime.Now);

            List<InformeFinal> informeFinal = _context.InformeFinal
                .Where(r => r.EstadoAprobacion == ConstantCodigoEstadoAprobacionInformeFinal.Enviado_verificacion_liquidacion_novedades && (String.IsNullOrEmpty(r.EstadoCumplimiento) || r.EstadoCumplimiento == ConstantCodigoEstadoCumplimientoInformeFinal.En_proceso_validacion_cumplimiento))
                .Include(r => r.Proyecto)
                .ToList();

            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Supervisor || x.PerfilId == (int)EnumeratorPerfil.Tecnica).Include(y => y.Usuario);
            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.Alerta5DiasGrupoNovedades5_1_4);

            foreach (var informe in informeFinal)
            {

                if (informeFinal.Count() > 0 && informe.FechaEnvioGrupoNovedades > RangoFechaConDiasHabiles)
                {
                    string template = TemplateRecoveryPassword.Contenido
                                .Replace("_LinkF_", pDominioFront)
                                .Replace("[LLAVE_MEN]", informe.Proyecto.LlaveMen)
                                .Replace("[ESTADO_CUMPLIMIENTO]", String.IsNullOrEmpty(informe.EstadoCumplimiento) ? "Sin Validación" : await _commonService.GetNombreDominioByCodigoAndTipoDominio(informe.EstadoCumplimiento, (int)EnumeratorTipoDominio.Estado_validacion_cumplimiento_informe_Final))
                                .Replace("[FECHA_ENVIO_NOVEDADES]", ((DateTime)informe.FechaEnvioGrupoNovedades).ToString("yyyy-MM-dd"));

                    foreach (var item in usuarios)
                    {
                        Helpers.Helpers.EnviarCorreo(item.Usuario.Email, "Pendiente por revisión final. ", template, pSender, pPassword, pMailServer, pMailPort);
                    }

                }
            }
        }

        private bool validateRegistroCompleto(int pInformeFinalId)
        {
            InformeFinal informeFinal = _context.InformeFinal.Find(pInformeFinalId);

            if (informeFinal != null)
            {
                if(informeFinal.TieneObservacionesCumplimiento == null || informeFinal.TieneObservacionesInterventoria == null){
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
