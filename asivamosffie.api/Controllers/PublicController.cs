using asivamosffie.model.APIModels;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        public readonly IRegisterPreContructionPhase1Service _RegisterPreContructionPhase1Service;
        public readonly ISourceFundingService _sourceFunding;
        public readonly ISelectionProcessService _selectionProcess;
        public readonly IGuaranteePolicyService _guaranteePolicy;
        public readonly IManagementCommitteeReportService _managementCommitteeReportService;
        public readonly IOptions<AppSettings> _settings;
        public readonly IManagePreContructionActPhase1Service _managePreContructionActPhase1Service;
        public readonly IActBeginService _actBeginService;
        public readonly IRegisterFinalReportService _RegisterFinalReportService;
        public readonly IVerifyFinalReportService _VerifyFinalReportService;
        public readonly IValidateFinalReportService _ValidateFinalReportService;
        public readonly IValidateFulfilmentFinalReportService _ValidateFulfilmentFinalReportService;
        public readonly IRegisterWeeklyProgressService _registerWeeklyProgressService;
        public readonly IContractualControversy _ContractualControversyService;
        public readonly IPaymentRequierementsService _paymentRequierementsService;
        public readonly IJudicialDefense _judicialDefense;
        public readonly IRegisterProjectETCService _registerProjectETCService;
        public readonly IRegisterContractualLiquidationRequestService _registerContractualLiquidationRequestService;


        public PublicController(
                                IPaymentRequierementsService paymentRequierementsService,
                                IManagePreContructionActPhase1Service managePreContructionActPhase1Service,
                                IRegisterPreContructionPhase1Service registerPreContructionPhase1Service,
                                IManagementCommitteeReportService managementCommitteeReportService,
                                ISourceFundingService sourceFunding,
                                ISelectionProcessService selectionProcess,
                                IOptions<AppSettings> settings,
                                IGuaranteePolicyService guaranteePolicy,
                                IActBeginService actBeginService,
                                IRegisterWeeklyProgressService registerWeeklyProgressService,
                                IJudicialDefense judicialDefense,
                                IContractualControversy contractualControversy,
                                IRegisterFinalReportService registerFinalReportService,
                                IVerifyFinalReportService verifyFinalReportService,
                                IValidateFinalReportService validateFinalReportService,
                                IValidateFulfilmentFinalReportService validateFulfilmentFinalReportService,
                                IRegisterProjectETCService registerProjectETCService,
                                IRegisterContractualLiquidationRequestService registerContractualLiquidationRequestService
                              )
        {
            _paymentRequierementsService = paymentRequierementsService;
            _managePreContructionActPhase1Service = managePreContructionActPhase1Service;
            _RegisterPreContructionPhase1Service = registerPreContructionPhase1Service;
            _sourceFunding = sourceFunding;
            _settings = settings;
            _selectionProcess = selectionProcess;
            _managementCommitteeReportService = managementCommitteeReportService;
            _guaranteePolicy = guaranteePolicy;
            _actBeginService = actBeginService;
            _registerWeeklyProgressService = registerWeeklyProgressService;
            _judicialDefense = judicialDefense;
            _ContractualControversyService = contractualControversy;
            _RegisterFinalReportService = registerFinalReportService;
            _ValidateFinalReportService = validateFinalReportService;
            _VerifyFinalReportService = verifyFinalReportService;
            _ValidateFulfilmentFinalReportService = validateFulfilmentFinalReportService;
            _registerProjectETCService = registerProjectETCService;
            _registerContractualLiquidationRequestService = registerContractualLiquidationRequestService;
        }

        public AppSettingsService ToAppSettingsService(IOptions<AppSettings> appSettings)
        {
            AppSettingsService appSettingsService = new AppSettingsService
            {
                DominioFront = appSettings.Value.DominioFront,
                MailPort = appSettings.Value.MailPort,
                MailServer = appSettings.Value.MailServer,
                Password = appSettings.Value.Password,
                Sender = appSettings.Value.Sender
            };
            return appSettingsService;
        }

        #region Solicitud Pago
        ///4.1.8
        [HttpGet("GetSolicitudPagoPendienteVerificacion")]
        public async Task SolicitudPagoPendienteVerificacion()
        {
            try
            {
                await _paymentRequierementsService.SolicitudPagoPendienteVerificacion();
            }
            catch (Exception ex)
            {
            }
        }

        ///4.1.9
        [HttpGet("GetSolicitudPagoPendienteAutorizacion")]
        public async Task SolicitudPagoPendienteAutorizacion()
        {
            try
            {
                await _paymentRequierementsService.SolicitudPagoPendienteAutorizacion();
            }
            catch (Exception ex)
            {
            }
        }


        #endregion 

        #region Seguimiento Semanal 
        ///4.1.12
        [HttpGet("GetSendEmailWhenNoWeeklyProgress")]
        public async Task SendEmailWhenNoWeeklyProgress()
        {
            try
            {
                await _registerWeeklyProgressService.SendEmailWhenNoWeeklyProgress();
            }
            catch (Exception ex)
            {
            }
        }
        ///4.1.20
        [HttpGet("GetSendEmailWhenNoWeeklyValidate")]
        public async Task SendEmailWhenNoWeeklyValidate()
        {
            try
            {
                await _registerWeeklyProgressService.SendEmailWhenNoWeeklyValidate();
            }
            catch (Exception ex)
            {
            }
        }
        ///4.1.21
        [HttpGet("GetSendEmailWhenNoWeeklyAproved")]
        public async Task SendEmailWhenNoWeeklyAproved()
        {
            try
            {
                await _registerWeeklyProgressService.SendEmailWhenNoWeeklyAproved();
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region PreConstruccion
        /// <summary>
        /// JMartinez
        // Aprobar Actas de
        // Comité Con Fecha
        // vencida Según Fecha paramétrica 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetApproveExpiredMinutes")]
        public async Task GetApproveExpiredMinutes()
        {
            try
            {
                await _managementCommitteeReportService.GetApproveExpiredMinutes(_settings.Value.Sender);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// JMartinez
        //Enviar notificacion a interventor, 
        //tecnica y suipervisor si la poliza 
        //tiene 4 dias habiles y aun no tiene gestion
        /// </summary>
        /// <returns></returns>
        /// 
        //316
        [HttpGet("GetContratosConPolizaVencida")]
        public async Task GetContratosConPolizaVencida()
        {
            try
            {
                await _RegisterPreContructionPhase1Service.EnviarNotificacion(_settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //317
        [HttpGet("GetContratosConPolizaVencidaInterventoria")]
        public async Task GetContratosConPolizaVencidaInterventoria()
        {
            try
            {
                await _RegisterPreContructionPhase1Service.EnviarNotificacionInteventoria(_settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //317
        [HttpGet("GetContratosSinGestionarVerificacionRequisitos")]
        public async Task GetContratosIntrerventoriaSinGestionar()
        {
            try
            {
                await _RegisterPreContructionPhase1Service.GetContratosIntrerventoriaSinGestionar(_settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //318
        [HttpGet("GetContratosObraSinGestionar")]
        public async Task GetContratosObraSinGestionar()
        {
            try
            {
                await _RegisterPreContructionPhase1Service.GetContratosObraSinGestionar(_settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //318
        [HttpGet("GetContratosInterventoriaSinGestionar")]
        public async Task GetContratosInterventoriaSinGestionar()
        {
            try
            {
                await _RegisterPreContructionPhase1Service.GetContratosInterventoriaSinGestionar(_settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //319
        [HttpGet("GetListContratoConActaSinDocumento")]
        public async Task GetListContratoConActaSinDocumento()
        {
            try
            {
                AppSettingsService appSettingsService = ToAppSettingsService(_settings);
                await _managePreContructionActPhase1Service.GetListContratoConActaSinDocumento(appSettingsService);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Informe Final 
        //5.1.1
        [HttpGet("GetInformeFinalSinGestionar")]
        public async Task GetInformeFinalSinGestionar()
        {
            try
            {
                await _RegisterFinalReportService.GetInformeFinalSinGestionar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //5.1.2
        [HttpGet("GetInformeFinalNoEnviadoASupervisor")]
        public async Task GetInformeFinalNoEnviadoASupervisor()
        {
            try
            {
                await _VerifyFinalReportService.GetInformeFinalNoEnviadoASupervisor();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //5.1.3
        [HttpGet("GetInformeFinalNoEnviadoAGrupoNovedades")]
        public async Task GetInformeFinalNoEnviadoAGrupoNovedades()
        {
            try
            {
                await _ValidateFinalReportService.GetInformeFinalNoEnviadoAGrupoNovedades();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //5.1.4
        [HttpGet("GetInformeFinalNoCumplimiento")]
        public async Task GetInformeFinalNoCumplimiento()
        {
            try
            {
                await _ValidateFulfilmentFinalReportService.GetInformeFinalNoCumplimiento();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Other
        [HttpGet("GetDiasHabilesActaConstruccionEnviada")]
        public async Task GetDiasHabilesActaConstruccionEnviada()
        {
            try
            {
                AppSettingsService appSettingsService = ToAppSettingsService(_settings);
                await _actBeginService.GetDiasHabilesActaConstruccionEnviada(appSettingsService);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetDiasHabilesActaRegistrada")]
        public async Task GetDiasHabilesActaRegistrada()
        {
            try
            {
                AppSettingsService appSettingsService = ToAppSettingsService(_settings);
                await _actBeginService.GetDiasHabilesActaRegistrada(appSettingsService);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetConsignationValue")]
        public async Task GetConsignationValue()
        {
            try
            {
                await _sourceFunding.GetConsignationValue(_settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
                //return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet("GetMonitorWithoutFollow")]
        public async Task GetMonitorWithoutFollow()
        {
            try
            {
                await _selectionProcess.getActividadesVencidas(_settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
                //return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet("NoApprovedLegalFiduciaryPolicy4d")]
        public async Task NoApprovedLegalFiduciaryPolicy4d()
        {
            //Task task1;
            try
            {
                //paquete 1: no tienen registro inicial contrato poliza
                await _guaranteePolicy.EnviarCorreoSupervisor4dPolizaNoAprobada(_settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);

                //paquete 2: estado diferente a Aprobado
                await _guaranteePolicy.EnviarCorreoSupervisor4dPolizaNoAprobada2(_settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
                //return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //4.2.1 - alerta
        [HttpGet("GetVencimientoTerminosContrato")]
        public async Task VencimientoTerminosContrato()
        {
            try
            {
                await _ContractualControversyService.VencimientoTerminosContrato();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        //4.2.2 - alerta
        [HttpGet("GetVencimientoTerminosDefensaJudicial")]
        public async Task VencimientoTerminosDefensaJudicial()
        {
            try
            {
                await _judicialDefense.VencimientoTerminosDefensaJudicial();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //5.1.5
        [HttpGet("GetInformeFinalActaBienesServicios")]
        public async Task GetInformeFinalActaBienesServicios()
        {
            try
            {
                await _registerProjectETCService.GetInformeFinalActaBienesServicios();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region solicitud de liquidación (5.1.6, 5.1.7, 5.1.8)

        [HttpGet("GetRegistroLiquidacionPendiente")]
        public async Task RegistroLiquidacionPendiente()
        {
            try
            {
                await _registerContractualLiquidationRequestService.RegistroLiquidacionPendiente();
            }
            catch (Exception ex)
            {
            }
        }

        [HttpGet("GetRegistroLiquidacionPendienteAprobacion")]
        public async Task RegistroLiquidacionPendienteAprobacion()
        {
            try
            {
                await _registerContractualLiquidationRequestService.RegistroLiquidacionPendienteAprobacion();
            }
            catch (Exception ex)
            {
            }
        }

        [HttpGet("GetRegistroLiquidacionPendienteEnviarLiquidacion")]
        public async Task RegistroLiquidacionPendienteEnviarLiquidacion()
        {
            try
            {
                await _registerContractualLiquidationRequestService.RegistroLiquidacionPendienteEnviarLiquidacion();
            }
            catch (Exception ex)
            {
            }
        }


        #endregion
    }


}
