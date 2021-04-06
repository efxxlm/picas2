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
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class RegisterContractualLiquidationRequestService : IRegisterContractualLiquidationRequestService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly ITechnicalRequirementsConstructionPhaseService _technicalRequirementsConstructionPhaseService;

        public RegisterContractualLiquidationRequestService(devAsiVamosFFIEContext context, ICommonService commonService, ITechnicalRequirementsConstructionPhaseService technicalRequirementsConstructionPhaseService)
        {
            _context = context;
            _commonService = commonService;
            _technicalRequirementsConstructionPhaseService = technicalRequirementsConstructionPhaseService;
        }

        public async Task<List<VContratacionProyectoSolicitudLiquidacion>> GridRegisterContractualLiquidationObra()
        {
            return await _context.VContratacionProyectoSolicitudLiquidacion.Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString()).ToListAsync();
        }

        public async Task<List<VContratacionProyectoSolicitudLiquidacion>> GridRegisterContractualLiquidationInterventoria()
        {
            return await _context.VContratacionProyectoSolicitudLiquidacion.Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString()).ToListAsync();
        }

        public async Task<VContratacionProyectoSolicitudLiquidacion> GetContratacionProyectoByContratacionProyectoId(int pContratacionProyectoId)
        {
            return await _context.VContratacionProyectoSolicitudLiquidacion.Where(r => r.ContratacionProyectoId == pContratacionProyectoId).FirstOrDefaultAsync();
        }

        public async Task<List<dynamic>> GridInformeFinal(int pContratacionProyectoId)
        {
            List<dynamic> ProyectoAjustado = new List<dynamic>();

            ContratacionProyecto contratacionProyecto = _context.ContratacionProyecto.Find(pContratacionProyectoId);

            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();

            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();
            if (contratacionProyecto != null)
            {
                Proyecto proyecto = await _context.Proyecto.Where(r => r.ProyectoId == contratacionProyecto.ProyectoId)
                                            .Include(r => r.InformeFinal)
                                             .Include(r => r.InstitucionEducativa)
                                             .FirstOrDefaultAsync();

                InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == proyecto.SedeId).FirstOrDefault();
                Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                proyecto.MunicipioObj = Municipio;
                proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
                proyecto.Sede = Sede;
                LiquidacionContratacionObservacion liquidacionContratacionObservacion = _context.LiquidacionContratacionObservacion.Where(r => r.ContratacionProyectoId == pContratacionProyectoId
                                                          && r.MenuId == (int)enumeratorMenu.Registrar_validar_solicitud_liquidacion_contractual
                                                          && r.Eliminado != true
                                                          && r.RegistroCompleto == true
                                                          && r.Archivado != true
                                                          && r.TipoObservacionCodigo == ConstantCodigoTipoObservacionLiquidacionContratacion.Informe_final).FirstOrDefault();

                ProyectoAjustado.Add(new
                {
                    fechaEnvio = proyecto.InformeFinal.FirstOrDefault().FechaEnvioEtc,
                    fechaAprobacion = proyecto.InformeFinal.FirstOrDefault().FechaAprobacionFinal,
                    llaveMen = proyecto.LlaveMen,
                    tipoIntervencion = proyecto.tipoIntervencionString,
                    sede = proyecto.Sede.Nombre,
                    registroCompleto = liquidacionContratacionObservacion != null ? liquidacionContratacionObservacion.RegistroCompleto : false,
                    proyectoId = proyecto.ProyectoId,
                    informeFinalId = proyecto.InformeFinal.FirstOrDefault().InformeFinalId,
                    institucionEducativa = proyecto.InstitucionEducativa.Nombre
                }); ;
            }

            return ProyectoAjustado;
        }

        public async Task<List<dynamic>> GetInformeFinalByProyectoId(int pProyectoId)
        {
            String numeroContratoObra = string.Empty, nombreContratistaObra = string.Empty, numeroContratoInterventoria = string.Empty, nombreContratistaInterventoria = string.Empty;

            List<dynamic> ProyectoAjustado = new List<dynamic>();
            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();
            List<Dominio> TipoObraIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar).ToList();

            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();
            Proyecto proyecto = await _context.Proyecto.Where(r => r.ProyectoId == pProyectoId)
                                                            .Include(r => r.InformeFinal)
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
                    }
                    else if (contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Interventoria)
                    {
                        nombreContratistaInterventoria = contratacion.Contratista != null ? contratacion.Contratista.Nombre : string.Empty;
                        numeroContratoInterventoria = contrato.NumeroContrato != null ? contrato.NumeroContrato : string.Empty;
                    }
                }
            }
            ProyectoAjustado.Add(new
            {
                numeroContratoObra = numeroContratoObra,
                nombreContratistaObra = nombreContratistaObra,
                numeroContratoInterventoria = numeroContratoInterventoria,
                nombreContratistaInterventoria = nombreContratistaInterventoria,
                informeFinal = proyecto.InformeFinal.FirstOrDefault()
            });

            return ProyectoAjustado;
        }

        public async Task<List<InformeFinalInterventoria>> GetInformeFinalAnexoByInformeFinalId(int pInformeFinalId)
        {
            List<InformeFinalInterventoria> ListInformeFinalChequeo = await _context.InformeFinalInterventoria
                    .Where(r => r.InformeFinalId == pInformeFinalId)
                    .Include(r => r.InformeFinalListaChequeo)
                    .Include(r => r.InformeFinalAnexo)
                    .OrderBy(r => r.InformeFinalListaChequeo.Posicion)
                    .ToListAsync();

            foreach (var item in ListInformeFinalChequeo)
            {
                item.CalificacionCodigoString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.CalificacionCodigo, (int)EnumeratorTipoDominio.Calificacion_Informe_Final);

                if (item.InformeFinalAnexoId != null)
                {
                    item.InformeFinalAnexo.TipoAnexoString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.InformeFinalAnexo.TipoAnexo, (int)EnumeratorTipoDominio.Tipo_Anexo_Informe_Final);
                }
            }
            return ListInformeFinalChequeo;
        }

        public async Task<dynamic> GetObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId(int pMenuId, int pContratacionProyectoId, int pPadreId, string pTipoObservacionCodigo)
        {
            return await _context.LiquidacionContratacionObservacion
                                           .Where(r => r.MenuId == pMenuId
                                               && r.ContratacionProyectoId == pContratacionProyectoId
                                               && r.IdPadre == pPadreId
                                               && r.TipoObservacionCodigo == pTipoObservacionCodigo)
                                            .Select(p => new
                                            {
                                                p.LiquidacionContratacionObservacionId,
                                                p.TieneObservacion,
                                                p.Archivado,
                                                p.FechaCreacion,
                                                p.Observacion,
                                                p.RegistroCompleto
                                            }).ToListAsync();
        }

        public async Task<Respuesta> CreateUpdateLiquidacionContratacionObservacion(LiquidacionContratacionObservacion pLiquidacionContratacionObservacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Actualizar_Liquidacion_Contratacion_Observacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pLiquidacionContratacionObservacion.LiquidacionContratacionObservacionId > 0)
                {
                    if (pLiquidacionContratacionObservacion.Archivado == null)
                        pLiquidacionContratacionObservacion.Archivado = false;

                    await _context.Set<LiquidacionContratacionObservacion>()
                                  .Where(o => o.LiquidacionContratacionObservacionId == pLiquidacionContratacionObservacion.LiquidacionContratacionObservacionId)
                                  .UpdateAsync(r => new LiquidacionContratacionObservacion()
                                  {
                                      Archivado = pLiquidacionContratacionObservacion.Archivado,
                                      FechaModificacion = DateTime.Now,
                                      UsuarioModificacion = pLiquidacionContratacionObservacion.UsuarioCreacion,
                                      RegistroCompleto = ValidateCompleteRecordLiquidacionContratacionObservacion(pLiquidacionContratacionObservacion),
                                      TieneObservacion = pLiquidacionContratacionObservacion.TieneObservacion,
                                      Observacion = pLiquidacionContratacionObservacion.Observacion,
                                  });
                }
                else
                {
                    pLiquidacionContratacionObservacion.Archivado = false;
                    pLiquidacionContratacionObservacion.FechaCreacion = DateTime.Now;
                    pLiquidacionContratacionObservacion.Eliminado = false;
                    pLiquidacionContratacionObservacion.RegistroCompleto = ValidateCompleteRecordLiquidacionContratacionObservacion(pLiquidacionContratacionObservacion);

                    _context.Entry(pLiquidacionContratacionObservacion).State = EntityState.Added;
                    _context.SaveChanges();
                }

                Respuesta respuesta =
                   new Respuesta
                   {
                       IsSuccessful = true,
                       IsException = false,
                       IsValidation = false,
                       Code = GeneralCodes.OperacionExitosa,
                       Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_solicitud_liquidacion_contractual, GeneralCodes.OperacionExitosa, idAccion, pLiquidacionContratacionObservacion.UsuarioCreacion, "CREAR OBSERVACIÓN LIQUIDACIÓN CONTRATACIÓN")
                   };

                if (pLiquidacionContratacionObservacion.Archivado != true)
                    await ValidateCompleteObservation(pLiquidacionContratacionObservacion, pLiquidacionContratacionObservacion.UsuarioCreacion);

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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_solicitud_liquidacion_contractual, GeneralCodes.Error, idAccion, pLiquidacionContratacionObservacion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        private bool ValidateCompleteRecordLiquidacionContratacionObservacion(LiquidacionContratacionObservacion pLiquidacionContratacionObservacion)
        {
            if (pLiquidacionContratacionObservacion.TieneObservacion == false)
            {
                return true;
            }
            else
            {
                if (!string.IsNullOrEmpty(pLiquidacionContratacionObservacion.Observacion))
                    return true;
            }

            return false;
        }

        private async Task<bool> ValidateCompleteObservation(LiquidacionContratacionObservacion pLiquidacionContratacionObservacion, string pUsuarioMod)
        {
            try
            {
                ContratacionProyecto contratacionProyecto = await _context.ContratacionProyecto.FindAsync(pLiquidacionContratacionObservacion.ContratacionProyectoId);
                
                //genera consecutivo
                if (pLiquidacionContratacionObservacion.MenuId == (int)enumeratorMenu.Registrar_validar_solicitud_liquidacion_contractual)
                {
                    if (String.IsNullOrEmpty(contratacionProyecto.NumeroSolicitudLiquidacion))
                    {
                        int consecutivo = _context.ContratacionProyecto
                                        .Where(r => !String.IsNullOrEmpty(r.NumeroSolicitudLiquidacion))
                                        .Count();
                        contratacionProyecto.NumeroSolicitudLiquidacion = "SL " + (consecutivo + 1).ToString("000");
                        _context.SaveChanges();
                    }
                }

                int intCantidadTipoObservacionCodigo = 3;//ConstantCodigoTipoObservacionLiquidacionContratacion
                
                    int intCantidadObservacionesLiquidacionContratacion = _context.LiquidacionContratacionObservacion.Where(r => r.ContratacionProyectoId == pLiquidacionContratacionObservacion.ContratacionProyectoId
                                                              && r.MenuId == pLiquidacionContratacionObservacion.MenuId
                                                              && r.Eliminado != true
                                                              && r.RegistroCompleto == true
                                                              && r.Archivado != true).Count();

                //Valida si la cantidad del tipo de codigo es igual a las observaciones para ese menu 
                bool blRegistroCompleto = false;
                DateTime? FechaRegistroCompleto = null;
                if (intCantidadObservacionesLiquidacionContratacion >= intCantidadTipoObservacionCodigo)
                {
                    FechaRegistroCompleto = DateTime.Now;
                    blRegistroCompleto = true;
                }

                switch (pLiquidacionContratacionObservacion.MenuId)
                {
                    case (int)enumeratorMenu.Registrar_validar_solicitud_liquidacion_contractual:
                        await _context.Set<ContratacionProyecto>()
                        .Where(r => r.ContratacionProyectoId == pLiquidacionContratacionObservacion.ContratacionProyectoId)
                        .UpdateAsync(r => new ContratacionProyecto()
                        {
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pUsuarioMod,
                            EstadoValidacionLiquidacionCodigo = blRegistroCompleto ? ConstantCodigoEstadoValidacionLiquidacion.Con_validacion : ConstantCodigoEstadoValidacionLiquidacion.En_proceso_de_validacion,
                            RegistroCompletoVerificacionLiquidacion = blRegistroCompleto,
                            FechaValidacionLiquidacion = FechaRegistroCompleto
                        });
                        break;

                    case (int)enumeratorMenu.Aprobar_solicitud_liquidacion_contractual:
                        await _context.Set<ContratacionProyecto>()
                        .Where(r => r.ContratacionProyectoId == pLiquidacionContratacionObservacion.ContratacionProyectoId)
                        .UpdateAsync(r => new ContratacionProyecto()
                        {
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pUsuarioMod,
                            EstadoAprobacionLiquidacionCodigo = blRegistroCompleto ? ConstantCodigoEstadoAprobacionLiquidacion.Con_aprobacion : ConstantCodigoEstadoAprobacionLiquidacion.En_proceso_de_aprobacion,
                            RegistroCompletoAprobacionLiquidacion = blRegistroCompleto,
                            FechaAprobacionLiquidacion = FechaRegistroCompleto
                        });
                        break;

                    case (int)enumeratorMenu.Gestionar_tramite_liquidacion_contractual:
                        await _context.Set<ContratacionProyecto>()
                        .Where(r => r.ContratacionProyectoId == pLiquidacionContratacionObservacion.ContratacionProyectoId)
                        .UpdateAsync(r => new ContratacionProyecto()
                        {
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pUsuarioMod,
                            EstadoTramiteLiquidacion = blRegistroCompleto ? ConstantCodigoEstadoVerificacionLiquidacion.Con_verificacion : ConstantCodigoEstadoVerificacionLiquidacion.En_proceso_de_verificacion,
                            RegistroCompletoAprobacionLiquidacion = blRegistroCompleto,
                            FechaTramiteLiquidacion = FechaRegistroCompleto
                        });
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void ArchivarLiquidacionContratacionObservacion(ContratacionProyecto pContratacionProyecto)
        {
            _context.Set<LiquidacionContratacionObservacion>()
                    .Where(r => r.ContratacionProyectoId == pContratacionProyecto.ContratacionProyectoId
                              && r.TieneObservacion == false)
                    .Update(r => new LiquidacionContratacionObservacion()
                    {
                        Archivado = true,
                        FechaModificacion = DateTime.Now,
                        UsuarioModificacion = pContratacionProyecto.UsuarioCreacion
                    });

            _context.Set<ContratacionProyecto>()
                .Where(s => s.ContratacionProyectoId == pContratacionProyecto.ContratacionProyectoId)
                .Update(s => new ContratacionProyecto
                {
                    RegistroCompletoVerificacionLiquidacion = false,
                    FechaValidacionLiquidacion = null,

                    RegistroCompletoAprobacionLiquidacion = false,
                    FechaAprobacionLiquidacion = null,

                    RegistroCompletoTramiteLiquidacion = false,
                    FechaTramiteLiquidacion = null,

                    FechaModificacion = DateTime.Now,
                    UsuarioModificacion = pContratacionProyecto.UsuarioCreacion
                });
        }

        #region  Emails
        public async Task<Respuesta> ChangeStatusLiquidacionContratacionProyecto(ContratacionProyecto pContratacionProyecto, int menuId)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Contratacion_Proyecto_Liquidacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ContratacionProyecto contratacionProyectoOld = _context.ContratacionProyecto.Find(pContratacionProyecto.ContratacionProyectoId);
                if (contratacionProyectoOld != null)
                {

                    //5.1.6
                    if (menuId == (int)enumeratorMenu.Registrar_validar_solicitud_liquidacion_contractual && pContratacionProyecto.EstadoValidacionLiquidacionCodigo == ConstantCodigoEstadoValidacionLiquidacion.Enviado_al_supervisor)
                        await SendMailToSupervision(pContratacionProyecto.ContratacionProyectoId);

                    ///5.1.7
                    if (menuId == (int)enumeratorMenu.Aprobar_solicitud_liquidacion_contractual && pContratacionProyecto.EstadoAprobacionLiquidacionCodigo == ConstantCodigoEstadoAprobacionLiquidacion.Enviado_control_seguimiento)
                        await SendMailToNovedades(pContratacionProyecto.ContratacionProyectoId);

                    ///5.1.8
                    if (menuId == (int)enumeratorMenu.Gestionar_tramite_liquidacion_contractual && pContratacionProyecto.EstadoTramiteLiquidacion == ConstantCodigoEstadoVerificacionLiquidacion.Enviado_a_liquidacion)
                        await SendEmailToFinalLiquidation(pContratacionProyecto.ContratacionProyectoId);

                    _context.Set<ContratacionProyecto>()
                                          .Where(o => o.ContratacionProyectoId == pContratacionProyecto.ContratacionProyectoId)
                                                                                                          .Update(r => new ContratacionProyecto()
                                                                                                          {
                                                                                                              FechaModificacion = DateTime.Now,
                                                                                                              UsuarioModificacion = pContratacionProyecto.UsuarioCreacion,
                                                                                                              EstadoValidacionLiquidacionCodigo = !String.IsNullOrEmpty(pContratacionProyecto.EstadoValidacionLiquidacionCodigo) ? pContratacionProyecto.EstadoValidacionLiquidacionCodigo : contratacionProyectoOld.EstadoValidacionLiquidacionCodigo,
                                                                                                              EstadoAprobacionLiquidacionCodigo = !String.IsNullOrEmpty(pContratacionProyecto.EstadoAprobacionLiquidacionCodigo) ? pContratacionProyecto.EstadoAprobacionLiquidacionCodigo : contratacionProyectoOld.EstadoAprobacionLiquidacionCodigo,
                                                                                                              EstadoTramiteLiquidacion = !String.IsNullOrEmpty(pContratacionProyecto.EstadoTramiteLiquidacion) ? pContratacionProyecto.EstadoTramiteLiquidacion : contratacionProyectoOld.EstadoTramiteLiquidacion,
                                                                                                          });


                }

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_solicitud_liquidacion_contractual, GeneralCodes.OperacionExitosa, idAccion, pContratacionProyecto.UsuarioCreacion, "Actualización de estados liquidación contractual ")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_solicitud_liquidacion_contractual, GeneralCodes.Error, idAccion, pContratacionProyecto.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }


        ///5.1.6 - Enviar al supervisor
        private async Task<bool> SendMailToSupervision(int pContratacionProyectoId)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Notificacion_supervisor_registro_liquidacion_contractual));
            string strContenido = ReplaceVariablesContratacionProyectoLiquidacion(template.Contenido, pContratacionProyectoId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Supervisor
                                          };
            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
        }

        ///5.1.7 - Enviar al grupo de novedades 
        private async Task<bool> SendMailToNovedades(int pContratacionProyectoId)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Notificacion_grupo_novedades_aprobar_liquidacion_contractual));
            string strContenido = ReplaceVariablesContratacionProyectoLiquidacion(template.Contenido, pContratacionProyectoId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Seguimiento_y_control
                                          };
            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
        }

        ///5.1.8 - enaviar a juridica
        private async Task<bool> SendEmailToFinalLiquidation(int pContratacionProyectoId)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Notificacion_liquidacion_contractual_5_1_8));
            string strContenido = ReplaceVariablesContratacionProyectoLiquidacion(template.Contenido, pContratacionProyectoId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Juridica
                                          };
            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
        }

        private string ReplaceVariablesContratacionProyectoLiquidacion(string template, int pContratacionProyectoId)
        {
            ContratacionProyecto contratacionProyecto =
                _context.ContratacionProyecto.Where(r => r.ContratacionProyectoId == pContratacionProyectoId)
                .Include(r => r.Contratacion)
                    .ThenInclude(r => r.Contrato)
                .FirstOrDefault();

            template = template
                      .Replace("[NUMERO_CONTRATO]", contratacionProyecto.Contratacion.Contrato.FirstOrDefault().NumeroContrato);
            return template;
        }

        #endregion


    }
}
