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
        public readonly IContractualNoveltyService _ContractualNoveltyService;
        private readonly ITechnicalRequirementsConstructionPhaseService _technicalRequirementsConstructionPhaseService;

        public RegisterContractualLiquidationRequestService(devAsiVamosFFIEContext context, ICommonService commonService, ITechnicalRequirementsConstructionPhaseService technicalRequirementsConstructionPhaseService, IContractualNoveltyService contractualNoveltyService)
        {
            _context = context;
            _commonService = commonService;
            _technicalRequirementsConstructionPhaseService = technicalRequirementsConstructionPhaseService;
            _ContractualNoveltyService = contractualNoveltyService;
        }
        #region grid principal
        public async Task<List<VContratacionProyectoSolicitudLiquidacion>> GridRegisterContractualLiquidationObra(int pMenuId)
        {
            List<VContratacionProyectoSolicitudLiquidacion> result = await _context.VContratacionProyectoSolicitudLiquidacion.Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString()).ToListAsync();

            switch (pMenuId)
            {
                case (int)enumeratorMenu.Aprobar_solicitud_liquidacion_contractual:
                    result =  result.Where(s =>
                       s.EstadoValidacionLiquidacionCodigo == ConstantCodigoEstadoValidacionLiquidacion.Enviado_al_supervisor).ToList();
                    break;

                case (int)enumeratorMenu.Gestionar_tramite_liquidacion_contractual:
                    result = result.Where(s =>
                      s.EstadoAprobacionLiquidacionCodigo == ConstantCodigoEstadoAprobacionLiquidacion.Enviado_control_seguimiento).ToList();
                    break;

                default:
                    break;
            }

            return result;
        }

        public async Task<List<VContratacionProyectoSolicitudLiquidacion>> GridRegisterContractualLiquidationInterventoria(int pMenuId)
        {
            List<VContratacionProyectoSolicitudLiquidacion> result = await _context.VContratacionProyectoSolicitudLiquidacion.Where(r => r.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString()).ToListAsync();

            switch (pMenuId)
            {
                case (int)enumeratorMenu.Aprobar_solicitud_liquidacion_contractual:
                    result = result.Where(s =>
                      s.EstadoValidacionLiquidacionCodigo == ConstantCodigoEstadoValidacionLiquidacion.Enviado_al_supervisor).ToList();
                    break;

                case (int)enumeratorMenu.Gestionar_tramite_liquidacion_contractual:
                    result = result.Where(s =>
                      s.EstadoAprobacionLiquidacionCodigo == ConstantCodigoEstadoAprobacionLiquidacion.Enviado_control_seguimiento).ToList();
                    break;

                default:
                    break;
            }

            return result;
        }

        #endregion

        #region data general
        public async Task<List<dynamic>> GetContratacionByContratacionId(int pContratacionId)
        {
            List<dynamic> result = new List<dynamic>();

            VContratacionProyectoSolicitudLiquidacion values_lista = await _context.VContratacionProyectoSolicitudLiquidacion.Where(r => r.ContratacionId == pContratacionId).FirstOrDefaultAsync();
            Contratacion contratacion = _context.Contratacion
                .Where(r => r.ContratacionId == pContratacionId)
                .Include(r => r.ContratacionProyecto)
                    .ThenInclude(r => r.Proyecto)
                .Include(r => r.Contratista)
                .ThenInclude(r => r.Contratacion)
                .ThenInclude(r => r.DisponibilidadPresupuestal)
                .Include(r => r.Contrato)
                .FirstOrDefault();
            string tipoIntervencion = string.Empty;
            if (!String.IsNullOrEmpty(values_lista.TipoSolicitudCodigo))
                tipoIntervencion  = await _commonService.GetNombreDominioByCodigoAndTipoDominio(values_lista.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Componentes);

            result.Add(new
            {
                values_lista.FechaPoliza,
                values_lista.NumeroContrato,
                values_lista.ValorSolicitud,
                values_lista.ProyectosAsociados,
                values_lista.ContratoPolizaId,
                values_lista.ContratoPolizaActualizacionId,
                tipoIntervencion,
                contratacion.Contratista,
                contratacion
            });

            return result;
        }

        public async Task<List<NovedadContractual>> GetAllNoveltyByContratacion(int pContratacionId)
        {
            Contratacion contratacion = await _context.Contratacion.Where(r => r.ContratacionId == pContratacionId)
                                                                    .Include(r => r.Contrato)
                                                                    .Include(r => r.ContratacionProyecto)
                                                                    .FirstOrDefaultAsync();
            List<NovedadContractual> novedades = new List<NovedadContractual>();

            if (contratacion != null)
            {
                foreach (var novedadesxContrato in contratacion?.Contrato)
                {
                    List<NovedadContractual> novedadContractuals = _context.NovedadContractual.Where(r => r.ContratoId == novedadesxContrato.ContratoId).ToList();

                    novedadContractuals.ForEach(async p =>
                    {
                        novedades.Add(await _ContractualNoveltyService.GetNovedadContractualById(p.NovedadContractualId));
                    });


                }
                foreach (var novedadesxProyectos in contratacion?.ContratacionProyecto)
                {
                    List<NovedadContractual> novedadContractuals = _context.NovedadContractual.Where(r => r.ProyectoId == novedadesxProyectos.ProyectoId).ToList();
                    novedadContractuals.ForEach(async p =>
                    {
                        novedades.Add(await _ContractualNoveltyService.GetNovedadContractualById(p.NovedadContractualId));
                    });
                }
            }
            novedades.RemoveAll(r => r.Eliminado == true);

            return novedades;
        }

        public async Task<VContratacionProyectoSolicitudLiquidacion> GetPolizaByContratacionId(int pContratacionId)
        {
            return await _context.VContratacionProyectoSolicitudLiquidacion.Where(r => r.ContratacionId == pContratacionId).FirstOrDefaultAsync();
        }

        #endregion

        #region informe final
        public async Task<List<dynamic>> GridInformeFinal(int pContratacionId, int pMenuId)
        {
            List<dynamic> ProyectoAjustado = new List<dynamic>();

            Contratacion contratacion = _context.Contratacion.Find(pContratacionId);

            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();

            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();
            if (contratacion != null)
            {
                List<ContratacionProyecto> contratacionProyectos = _context.ContratacionProyecto.Where(r => r.ContratacionId == pContratacionId).ToList();
                foreach (var contratacionProyecto in contratacionProyectos)
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
                    LiquidacionContratacionObservacion liquidacionContratacionObservacion = _context.LiquidacionContratacionObservacion.Where(r => r.ContratacionId == pContratacionId
                                                              && r.MenuId == pMenuId
                                                              && r.IdPadre == proyecto.InformeFinal.FirstOrDefault().InformeFinalId
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
                    });
                }
                
            }

            return ProyectoAjustado;
        }

        public async Task<List<dynamic>> GetInformeFinalByProyectoId(int pProyectoId, int pContratacionId, int pMenuId)
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
            LiquidacionContratacionObservacion liquidacionContratacionObservacion = _context.LiquidacionContratacionObservacion.Where(r => r.ContratacionId == pContratacionId
                                          && r.MenuId == pMenuId
                                          && r.IdPadre == proyecto.InformeFinal.FirstOrDefault().InformeFinalId
                                          && r.Eliminado != true
                                          && r.RegistroCompleto == true
                                          && r.Archivado != true
                                          && r.TipoObservacionCodigo == ConstantCodigoTipoObservacionLiquidacionContratacion.Informe_final).FirstOrDefault();

            ProyectoAjustado.Add(new
            {
                registroCompleto = liquidacionContratacionObservacion != null ? liquidacionContratacionObservacion.RegistroCompleto : false,
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
                        .ThenInclude(r => r.ListaChequeoItem)
                    .Include(r => r.InformeFinalAnexo)
                    .OrderBy(r => r.InformeFinalListaChequeo.Orden)
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
        #endregion

        #region contrato póliza

        public async Task<ContratoPoliza> GetContratoPoliza(int pContratoPolizaId, int pMenuId, int pContratacionId)
        {
            ContratoPoliza contratoPoliza = await _context.ContratoPoliza
                .Where(c => c.ContratoPolizaId == pContratoPolizaId)
                .Include(c => c.PolizaGarantia)
                .Include(c => c.PolizaGarantiaActualizacion)
                .Include(c => c.PolizaListaChequeo)
                .Include(c => c.PolizaObservacion)
                .Include(c => c.Contrato).ThenInclude(c => c.Contratacion).ThenInclude(c => c.Contratista)
                .Include(c => c.ContratoPolizaActualizacion).ThenInclude(c => c.ContratoPolizaActualizacionSeguro)
                .Include(c => c.ContratoPolizaActualizacion).ThenInclude(c => c.ContratoPolizaActualizacionListaChequeo)
                .Include(c => c.ContratoPolizaActualizacion).ThenInclude(c => c.ContratoPolizaActualizacionRevisionAprobacionObservacion)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (contratoPoliza != null)
            {
                //contratoPoliza.UserResponsableAprobacion = _context.Usuario.Find(Int32.Parse(contratoPoliza.ResponsableAprobacion));
                if (contratoPoliza.ContratoPolizaActualizacion.FirstOrDefault() != null)
                {
                    LiquidacionContratacionObservacion liquidacionContratacionObservacion = _context.LiquidacionContratacionObservacion
                              .Where(r => r.ContratacionId == pContratacionId
                              && r.MenuId == pMenuId
                              && r.IdPadre == contratoPoliza.ContratoPolizaActualizacion.FirstOrDefault().ContratoPolizaActualizacionId
                              && r.Eliminado != true
                              && r.RegistroCompleto == true
                              && r.Archivado != true
                              && r.TipoObservacionCodigo == ConstantCodigoTipoObservacionLiquidacionContratacion.Actualizacion_de_poliza).FirstOrDefault();
                    //USAR PARA EL SEMAFORO
                    contratoPoliza.RegistroCompleto = liquidacionContratacionObservacion != null ? liquidacionContratacionObservacion.RegistroCompleto : false;
                }
            }

            return contratoPoliza;

        }

        #endregion

        #region balance
        public async Task<dynamic> GetBalanceByContratacionId(int pContratatacionId, int pMenuId)
        {
            Contratacion contratacion = _context.Contratacion.Where(r => r.ContratacionId == pContratatacionId).Include(r => r.ContratacionProyecto).FirstOrDefault();
            List<dynamic> Balance = new List<dynamic>();

            if (contratacion != null)
            {
                foreach (var contratacionProyecto in contratacion.ContratacionProyecto)
                {
                    VProyectosBalance vProyectosBalance = await _context.VProyectosBalance.Where(r => r.ProyectoId == contratacionProyecto.ProyectoId).FirstOrDefaultAsync();
                    LiquidacionContratacionObservacion liquidacionContratacionObservacion = _context.LiquidacionContratacionObservacion.Where(r => r.ContratacionId == pContratatacionId
                                                              && r.MenuId == pMenuId
                                                              && r.IdPadre == vProyectosBalance.BalanceFinancieroId
                                                              && r.Eliminado != true
                                                              && r.RegistroCompleto == true
                                                              && r.Archivado != true
                                                              && r.TipoObservacionCodigo == ConstantCodigoTipoObservacionLiquidacionContratacion.Balance_financiero).FirstOrDefault();
                    Balance.Add(new
                    {
                        balance = vProyectosBalance,
                        registroCompleto = liquidacionContratacionObservacion != null ? liquidacionContratacionObservacion.RegistroCompleto : false,
                    });

                }

            }

            return Balance;
        }

        #endregion

        #region manejo de observaciones 
        public async Task<dynamic> GetObservacionLiquidacionContratacionByMenuIdAndContratacionId(int pMenuId, int pContratacionId, int pPadreId, string pTipoObservacionCodigo)
        {
            return await _context.LiquidacionContratacionObservacion
                                           .Where(r => r.MenuId == pMenuId
                                               && r.ContratacionId == pContratacionId
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

        public async Task<dynamic> GetHistoricoObservacionLiquidacionContratacionByMenuIdAndContratacionId(int pMenuId, int pContratacionId, int pPadreId, string pTipoObservacionCodigo)
        {
            List<dynamic> observaciones = new List<dynamic>();

            LiquidacionContratacionObservacion obsVigente = await _context.LiquidacionContratacionObservacion
                                           .Where(r => r.MenuId == pMenuId
                                               && r.ContratacionId == pContratacionId
                                               && r.IdPadre == pPadreId
                                               && r.TipoObservacionCodigo == pTipoObservacionCodigo
                                               && r.Archivado == false || r.Archivado == null).FirstOrDefaultAsync();

            List<LiquidacionContratacionObservacion> historialObservaciones = await _context.LiquidacionContratacionObservacion
                               .Where(r => r.MenuId == pMenuId
                                   && r.ContratacionId == pContratacionId
                                   && r.IdPadre == pPadreId
                                   && r.TipoObservacionCodigo == pTipoObservacionCodigo
                                   && r.Archivado == true).ToListAsync();

            observaciones.Add(new
            {
                obsVigente,
                historialObservaciones
            });

            return observaciones;
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
                Contratacion contratacion = await _context.Contratacion.FindAsync(pLiquidacionContratacionObservacion.ContratacionId);
                
                //genera consecutivo
                if (pLiquidacionContratacionObservacion.MenuId == (int)enumeratorMenu.Registrar_validar_solicitud_liquidacion_contractual)
                {
                    if (String.IsNullOrEmpty(contratacion.NumeroSolicitudLiquidacion))
                    {
                        int consecutivo = _context.Contratacion
                                        .Where(r => !String.IsNullOrEmpty(r.NumeroSolicitudLiquidacion))
                                        .Count();
                        contratacion.NumeroSolicitudLiquidacion = "SL-" + (consecutivo + 1).ToString("000");
                        _context.SaveChanges();
                    }
                }

                int intCantidadTipoObservacionCodigo = 1;//comienza con 1 , el de póliza
                //
                List<ContratacionProyecto> contratacionProyectos = _context.ContratacionProyecto.Where(r => r.ContratacionId == pLiquidacionContratacionObservacion.ContratacionId && r.Eliminado != true).ToList();
                //total observaciones balance
                foreach (var item in contratacionProyectos)
                {
                    BalanceFinanciero bf = _context.BalanceFinanciero.Where(r => r.ProyectoId == item.ProyectoId).FirstOrDefault();
                    if (bf != null)
                        intCantidadTipoObservacionCodigo++;
                    
                    InformeFinal infF = _context.InformeFinal.Where(r => r.ProyectoId == item.ProyectoId && r.Eliminado != true).FirstOrDefault();
                    if (infF != null)
                        intCantidadTipoObservacionCodigo++;
                }
                //total observacion informe final

                int intCantidadObservacionesLiquidacionContratacion = _context.LiquidacionContratacionObservacion.Where(r => r.ContratacionId == pLiquidacionContratacionObservacion.ContratacionId
                                                              && r.MenuId == pLiquidacionContratacionObservacion.MenuId
                                                              && r.Eliminado != true
                                                              && r.RegistroCompleto == true
                                                              && r.Archivado != true).Count();

                //Valida si la cantidad del tipo de codigo es igual a las observaciones para ese menu 
                bool blRegistroCompleto = false;
                if (intCantidadObservacionesLiquidacionContratacion >= intCantidadTipoObservacionCodigo)
                {
                    blRegistroCompleto = true;
                }

                switch (pLiquidacionContratacionObservacion.MenuId)
                {
                    case (int)enumeratorMenu.Registrar_validar_solicitud_liquidacion_contractual:
                        await _context.Set<Contratacion>()
                        .Where(r => r.ContratacionId == pLiquidacionContratacionObservacion.ContratacionId)
                        .UpdateAsync(r => new Contratacion()
                        {
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pUsuarioMod,
                            EstadoValidacionLiquidacionCodigo = blRegistroCompleto ? ConstantCodigoEstadoValidacionLiquidacion.Con_validacion : ConstantCodigoEstadoValidacionLiquidacion.En_proceso_de_validacion,
                            RegistroCompletoVerificacionLiquidacion = blRegistroCompleto
                        });
                        break;

                    case (int)enumeratorMenu.Aprobar_solicitud_liquidacion_contractual:
                        await _context.Set<Contratacion>()
                        .Where(r => r.ContratacionId == pLiquidacionContratacionObservacion.ContratacionId)
                        .UpdateAsync(r => new Contratacion()
                        {
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pUsuarioMod,
                            EstadoAprobacionLiquidacionCodigo = blRegistroCompleto ? ConstantCodigoEstadoAprobacionLiquidacion.Con_aprobacion : ConstantCodigoEstadoAprobacionLiquidacion.En_proceso_de_aprobacion,
                            RegistroCompletoAprobacionLiquidacion = blRegistroCompleto
                        });
                        break;

                    case (int)enumeratorMenu.Gestionar_tramite_liquidacion_contractual:
                        await _context.Set<Contratacion>()
                        .Where(r => r.ContratacionId == pLiquidacionContratacionObservacion.ContratacionId)
                        .UpdateAsync(r => new Contratacion()
                        {
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = pUsuarioMod,
                            EstadoTramiteLiquidacion = blRegistroCompleto ? ConstantCodigoEstadoVerificacionLiquidacion.Con_verificacion : ConstantCodigoEstadoVerificacionLiquidacion.En_proceso_de_verificacion,
                            RegistroCompletoTramiteLiquidacion = blRegistroCompleto
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

        private void ArchivarLiquidacionContratacionObservacion(Contratacion pContratacion)
        {
            _context.Set<LiquidacionContratacionObservacion>()
                    .Where(r => r.ContratacionId == pContratacion.ContratacionId
                              && r.TieneObservacion == false)
                    .Update(r => new LiquidacionContratacionObservacion()
                    {
                        Archivado = true,
                        FechaModificacion = DateTime.Now,
                        UsuarioModificacion = pContratacion.UsuarioCreacion
                    });

            _context.Set<Contratacion>()
                .Where(s => s.ContratacionId == pContratacion.ContratacionId)
                .Update(s => new Contratacion
                {
                    RegistroCompletoVerificacionLiquidacion = false,
                    FechaValidacionLiquidacion = null,

                    RegistroCompletoAprobacionLiquidacion = false,
                    FechaAprobacionLiquidacion = null,

                    RegistroCompletoTramiteLiquidacion = false,
                    FechaTramiteLiquidacionControl = null,

                    FechaModificacion = DateTime.Now,
                    UsuarioModificacion = pContratacion.UsuarioCreacion
                });
        }

        public async Task<Respuesta> ChangeStatusLiquidacionContratacion(Contratacion pContratacion, int menuId)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Contratacion_Proyecto_Liquidacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                Contratacion contratacionOld = _context.Contratacion.Find(pContratacion.ContratacionId);
                DateTime? Fecha = null;
                string state = string.Empty;
                DateTime? fechaValidacion = contratacionOld.FechaValidacionLiquidacion != null ? (DateTime)contratacionOld.FechaValidacionLiquidacion : Fecha;
                DateTime? fechaAprobacion = contratacionOld.FechaAprobacionLiquidacion != null ? (DateTime)contratacionOld.FechaAprobacionLiquidacion : Fecha;
                DateTime? fechaTramite = contratacionOld.FechaTramiteLiquidacionControl != null ? (DateTime)contratacionOld.FechaTramiteLiquidacionControl : Fecha;

                if (contratacionOld != null)
                {
                    state = contratacionOld.EstadoSolicitudCodigo;
                    //5.1.6
                    if (menuId == (int)enumeratorMenu.Registrar_validar_solicitud_liquidacion_contractual && pContratacion.EstadoValidacionLiquidacionCodigo == ConstantCodigoEstadoValidacionLiquidacion.Enviado_al_supervisor)
                    {
                        fechaValidacion = DateTime.Now;
                        await SendMailToSupervision(pContratacion.ContratacionId);
                    }

                    ///5.1.7
                    if (menuId == (int)enumeratorMenu.Aprobar_solicitud_liquidacion_contractual && pContratacion.EstadoAprobacionLiquidacionCodigo == ConstantCodigoEstadoAprobacionLiquidacion.Enviado_control_seguimiento)
                    {
                        fechaAprobacion = DateTime.Now;
                        await SendMailToNovedades(pContratacion.ContratacionId);
                    }

                    ///5.1.8
                    if (menuId == (int)enumeratorMenu.Gestionar_tramite_liquidacion_contractual && pContratacion.EstadoTramiteLiquidacion == ConstantCodigoEstadoVerificacionLiquidacion.Enviado_a_liquidacion)
                    {
                        fechaTramite = DateTime.Now;
                        state = ConstanCodigoEstadoSolicitudContratacion.Sin_tramitar_ante_fiduciaria;
                        await SendEmailToFinalLiquidation(pContratacion.ContratacionId);
                    }

                    _context.Set<Contratacion>()
                                          .Where(o => o.ContratacionId == pContratacion.ContratacionId)
                                                                                                          .Update(r => new Contratacion()
                                                                                                          {
                                                                                                              FechaModificacion = DateTime.Now,
                                                                                                              UsuarioModificacion = pContratacion.UsuarioCreacion,
                                                                                                              EstadoValidacionLiquidacionCodigo = !String.IsNullOrEmpty(pContratacion.EstadoValidacionLiquidacionCodigo) ? pContratacion.EstadoValidacionLiquidacionCodigo : contratacionOld.EstadoValidacionLiquidacionCodigo,
                                                                                                              EstadoAprobacionLiquidacionCodigo = !String.IsNullOrEmpty(pContratacion.EstadoAprobacionLiquidacionCodigo) ? pContratacion.EstadoAprobacionLiquidacionCodigo : contratacionOld.EstadoAprobacionLiquidacionCodigo,
                                                                                                              EstadoTramiteLiquidacion = !String.IsNullOrEmpty(pContratacion.EstadoTramiteLiquidacion) ? pContratacion.EstadoTramiteLiquidacion : contratacionOld.EstadoTramiteLiquidacion,
                                                                                                              FechaValidacionLiquidacion = fechaValidacion,
                                                                                                              FechaAprobacionLiquidacion = fechaAprobacion,
                                                                                                              FechaTramiteLiquidacionControl = fechaTramite,
                                                                                                              EstadoSolicitudCodigo = state
                                                                                                          });


                }

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_solicitud_liquidacion_contractual, GeneralCodes.OperacionExitosa, idAccion, pContratacion.UsuarioCreacion, "Actualización de estados liquidación contractual ")
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
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_validar_solicitud_liquidacion_contractual, GeneralCodes.Error, idAccion, pContratacion.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }

        #endregion

        #region correos
        ///5.1.6 - Enviar al supervisor
        private async Task<bool> SendMailToSupervision(int pContratacionId)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Notificacion_supervisor_registro_liquidacion_contractual));
            string strContenido = await ReplaceVariablesContratacionProyectoLiquidacion(template.Contenido, pContratacionId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Supervisor
                                          };
            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
        }

        ///5.1.7 - Enviar al grupo de novedades 
        private async Task<bool> SendMailToNovedades(int pContratacionId)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Notificacion_grupo_novedades_aprobar_liquidacion_contractual));
            string strContenido = await ReplaceVariablesContratacionProyectoLiquidacion(template.Contenido, pContratacionId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Seguimiento_y_control
                                          };
            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
        }

        ///5.1.8 - enaviar a juridica
        private async Task<bool> SendEmailToFinalLiquidation(int pContratacionId)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Notificacion_liquidacion_contractual_5_1_8));
            string strContenido = await ReplaceVariablesContratacionProyectoLiquidacion(template.Contenido, pContratacionId);

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Juridica
                                          };
            return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
        }

        private async Task<string> ReplaceVariablesContratacionProyectoLiquidacion(string template, int pContratacionId)
        {
            VContratacionProyectoSolicitudLiquidacion contratacion = await _context.VContratacionProyectoSolicitudLiquidacion.Where(r => r.ContratacionId == pContratacionId).FirstOrDefaultAsync();

            template = template
                      .Replace("[NUMERO_SOLICITUD]", contratacion.NumeroSolicitudLiquidacion)
                      .Replace("[FECHA_POLIZA]", ((DateTime)contratacion.FechaPoliza).ToString("dd-MMM-yy"))
                      .Replace("[NUMERO_CONTRATO]", contratacion.NumeroContrato)
                      .Replace("[PROYECTOS_ASOCIADOS]", contratacion.ProyectosAsociados.ToString())
                      .Replace("[VALOR]", "$ " + String.Format("{0:n0}", contratacion.ValorSolicitud).ToString())
                      .Replace("[FECHA_VALIDACION]", contratacion.FechaValidacionLiquidacion != null ? ((DateTime)contratacion.FechaValidacionLiquidacion).ToString("dd-MMM-yy") : "")
                      .Replace("[FECHA_APROBACION]", contratacion.FechaAprobacionLiquidacion != null ? ((DateTime)contratacion.FechaAprobacionLiquidacion).ToString("dd-MMM-yy") : "")
                      .Replace("[FECHA_GESTION]", contratacion.FechaTramiteLiquidacionControl != null ? ((DateTime)contratacion.FechaTramiteLiquidacionControl).ToString("dd-MMM-yy") : "");

            return template;
        }

        #endregion

        #region alertas

        public async Task<bool> RegistroLiquidacionPendiente()
        {
            DateTime MaxDate = await _commonService.CalculardiasLaborales(5, DateTime.Now);
            List<VContratacionProyectoSolicitudLiquidacion> contrataciones =
                _context.VContratacionProyectoSolicitudLiquidacion
                .Where(r => !r.FechaTramiteLiquidacionControl.HasValue).ToList();
            List<VContratacionProyectoSolicitudLiquidacion> contratacionProyectos = new List<VContratacionProyectoSolicitudLiquidacion>();
            foreach (var item in contrataciones)
            {
                bool existeBalance = false;
                bool existeInforme = false;

                List<ContratacionProyecto> cp = _context.ContratacionProyecto.Where(r => r.ContratacionId == item.ContratacionId).ToList();
                foreach (var contratacionProyecto in cp)
                {
                    BalanceFinanciero balanceFinancieroxProyecto = _context.BalanceFinanciero.Where(r => r.ProyectoId == contratacionProyecto.ProyectoId).FirstOrDefault();
                    InformeFinal informeFinalxProyecto = _context.InformeFinal.Where(r => r.ProyectoId == contratacionProyecto.ProyectoId).FirstOrDefault();
                    if (balanceFinancieroxProyecto != null && informeFinalxProyecto!= null)
                    {
                        if (balanceFinancieroxProyecto.FechaAprobacion > MaxDate)
                            existeBalance = true;
                        if (informeFinalxProyecto.FechaEnvioEtc > MaxDate)
                            existeInforme = true;
                    }
                }
                if (item.FechaPoliza > MaxDate && existeBalance && existeInforme)
                {
                    contratacionProyectos.Add(item);
                }
            }
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Alerta_5_1_6_registro_solicitud_liquidacion_contrato));

            bool SedndIsSuccessfull = true;
            foreach (var item in contratacionProyectos)
            {
                string strContenido = await ReplaceVariablesContratacionProyectoLiquidacion(template.Contenido, item.ContratacionId);
                List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                        {
                                            EnumeratorPerfil.Apoyo_Supervisor,
                                            EnumeratorPerfil.Apoyo,
                                            EnumeratorPerfil.Supervisor
                                        };

                if (!_commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto))
                    SedndIsSuccessfull = false;
            }

            return SedndIsSuccessfull;
        }

        public async Task<bool> RegistroLiquidacionPendienteAprobacion()
        {
            DateTime MaxDate = await _commonService.CalculardiasLaborales(5, DateTime.Now);
            List<Contratacion> contratacions =
                _context.Contratacion
                .Where(r => r.FechaValidacionLiquidacion > MaxDate
                   && !r.FechaAprobacionLiquidacion.HasValue
                   && r.Eliminado == false).ToList();

            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Alerta_5_1_7_aprobar_solicitud_liquidacion_contrato));

            bool SedndIsSuccessfull = true;
            foreach (var item in contratacions)
            {
                string strContenido = await ReplaceVariablesContratacionProyectoLiquidacion(template.Contenido, item.ContratacionId);
                List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                        {
                                            EnumeratorPerfil.Apoyo_Supervisor,
                                            EnumeratorPerfil.Apoyo,
                                            EnumeratorPerfil.Supervisor
                                        };

                if (!_commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto))
                    SedndIsSuccessfull = false;
            }

            return SedndIsSuccessfull;
        }

        public async Task<bool> RegistroLiquidacionPendienteEnviarLiquidacion()
        {
            DateTime MaxDate = await _commonService.CalculardiasLaborales(5, DateTime.Now);
            List<Contratacion> contratacions =
                _context.Contratacion
                .Where(r => r.FechaAprobacionLiquidacion > MaxDate
                   && !r.FechaTramiteLiquidacionControl.HasValue
                   && r.Eliminado == false).ToList();

            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Alerta_5_1_8_gestionar_solicitud_liquidacion_contrato));

            bool SedndIsSuccessfull = true;
            foreach (var item in contratacions)
            {
                string strContenido = await ReplaceVariablesContratacionProyectoLiquidacion(template.Contenido, item.ContratacionId);
                List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                        {
                                            EnumeratorPerfil.Supervisor,
                                            EnumeratorPerfil.Seguimiento_y_control
                                        };

                if (!_commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto))
                    SedndIsSuccessfull = false;
            }

            return SedndIsSuccessfull;
        }

        #endregion


    }
}
