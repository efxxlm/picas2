using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegisterPayPerformanceController : ControllerBase
    {
        private readonly IRegisterPayPerformanceService _registerPayPerformanceService;
        private readonly IOptions<AppSettings> _settings;
        private readonly IConverter _converter;



        public RegisterPayPerformanceController(IOptions<AppSettings> settings, IConverter converter, IRegisterPayPerformanceService registerPayPerformanceService)
        {
            _registerPayPerformanceService = registerPayPerformanceService;
            _settings = settings;
            _converter = converter;

        }


        [Route("uploadFileToValidate")]
        [HttpPost]
        public async Task<IActionResult> uploadFileToValidate(IFormFile file, [FromQuery] string typeFile, bool saveSuccessProcess)
        {
            try
            {
                Respuesta respuesta = new Respuesta();

                if (file.Length > 0 && file.FileName.Contains(".xls"))
                {
                    string strUsuario = HttpContext.User.FindFirst("User").Value;
                    respuesta = await _registerPayPerformanceService.uploadFileToValidate(file, strUsuario, typeFile, saveSuccessProcess);
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Route("getPaymentsPerformances")]
        [HttpGet]
        public async Task<List<dynamic>> getPaymentsPerformances([FromQuery] string typeFile)
        {
            try
            {
                return await _registerPayPerformanceService.getPaymentsPerformances(typeFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("setObservationPaymentsPerformances")]
        [HttpPost]
        public async Task<IActionResult> setObservationPaymentsPerformances(CargueObservacionesPaymentPerformance data)
        {
            try
            {
                _registerPayPerformanceService.setObservationPaymentsPerformances(data.typeFile, data.observaciones, data.cargaPagosRendimientosId);

                return Ok("Se actualizo correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Route("deletePaymentPerformance")]
        [HttpGet]
        public async Task<IActionResult> deleteUpload(string uploadedOrderId)
        {
            try
            {
                var result = await _registerPayPerformanceService.setStatusPaymentPerformance(uploadedOrderId, "Eliminado");
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("downloadPaymentPerformance")]
        [HttpGet]
        public async Task<IActionResult> DownloadPaymentPerformance(int uploadedOrderId)
        {
            try
            {
                return Ok(_registerPayPerformanceService.DownloadPaymentPerformanceAsync(uploadedOrderId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //[Route("getPaymentsPerformances")]
        //[HttpGet]
        //public async Task<List<dynamic>> getPaymentsPerformances([FromQuery] string typeFile)
        //{
        //    try
        //    {
        //        return await _registerPayPerformanceService.getPaymentsPerformances(typeFile);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        [Route("managePerformance")]
        [HttpGet]
        public async Task<IActionResult> ManagePerformance(int uploadedOrderId)
        {
            try
            {
                var result = await _registerPayPerformanceService.ManagePerformanceAsync(uploadedOrderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
