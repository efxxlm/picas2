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
        public readonly IRegisterPreContructionPhase1Service _RegisterPreContructionPhase1Service;
        public readonly ISourceFundingService _sourceFunding;
        public readonly ISelectionProcessService _selectionProcess;
        public readonly IGuaranteePolicyService _guaranteePolicy;
        public readonly IManagementCommitteeReportService _managementCommitteeReportService;
        public readonly IOptions<AppSettings> _settings;

        public PublicController(IRegisterPreContructionPhase1Service  registerPreContructionPhase1Service,IManagementCommitteeReportService managementCommitteeReportService, ISourceFundingService sourceFunding, ISelectionProcessService selectionProcess, IOptions<AppSettings> settings, IGuaranteePolicyService guaranteePolicy)
        { 
            _RegisterPreContructionPhase1Service = registerPreContructionPhase1Service;
            _sourceFunding = sourceFunding;
            _settings = settings;
            _selectionProcess = selectionProcess;
            _managementCommitteeReportService = managementCommitteeReportService;
            _guaranteePolicy = guaranteePolicy;
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
            try
            {
                await _guaranteePolicy.EnviarCorreoSupervisor4dPolizaNoAprobada();
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

        /// <summary>
        /// JMartinez
            //Enviar notificacion a interventor , 
            //tecnica y suipervisor si la poliza 
            //tiene 4 dias habiles y aun no tiene gestion
        /// </summary>
        /// <returns></returns>
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


    }


}
