using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.api.Responses;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.model.APIModels;
using Microsoft.Extensions.Options;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        public readonly ISourceFundingService _sourceFunding;
        public readonly ISelectionProcessService _selectionProcess;
        public readonly IManagementCommitteeReportService _managementCommitteeReportService;
        public readonly IOptions<AppSettings> _settings;
        public readonly IManagePreContructionActPhase1Service _managePreContructionActPhase1Service;
        public readonly IActBeginService _actBeginService;
        public readonly IGuaranteePolicyService _guaranteePolicy;

        public PublicController(IManagePreContructionActPhase1Service managePreContructionActPhase1Service, IRegisterPreContructionPhase1Service registerPreContructionPhase1Service, IManagementCommitteeReportService managementCommitteeReportService, ISourceFundingService sourceFunding, ISelectionProcessService selectionProcess, IOptions<AppSettings> settings, IGuaranteePolicyService guaranteePolicy, IActBeginService actBeginService)
        {
            _sourceFunding = sourceFunding;
            _settings = settings;
            _selectionProcess = selectionProcess;
            _managementCommitteeReportService = managementCommitteeReportService;
            _guaranteePolicy = guaranteePolicy;
            _actBeginService = actBeginService;
        }

        public AppSettingsService ToAppSettingsService(IOptions<AppSettings> appSettings)
        {
            AppSettingsService appSettingsService = new AppSettingsService
            {
                MailPort = appSettings.Value.MailPort,
                MailServer = appSettings.Value.MailServer,
                Password = appSettings.Value.Password,
                Sender = appSettings.Value.Sender
            };
            return appSettingsService;
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
    }


}
