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
        private readonly IOptions<AppSettings> _settings;

        public PublicController(IManagementCommitteeReportService managementCommitteeReportService, ISourceFundingService sourceFunding, ISelectionProcessService selectionProcess, IOptions<AppSettings> settings)
        {
            _sourceFunding = sourceFunding;
            _settings = settings;
            _selectionProcess = selectionProcess;
            _managementCommitteeReportService = managementCommitteeReportService;
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
    }


}
