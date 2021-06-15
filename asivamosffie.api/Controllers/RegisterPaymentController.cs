using asivamosffie.model.APIModels;
using asivamosffie.services.Interfaces;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegisterPaymentController : ControllerBase
    {
        private readonly IRegisterPaymentService _paymentService;
        private readonly IOptions<AppSettings> _settings;
        private readonly IConverter _converter;

        public RegisterPaymentController(IOptions<AppSettings> settings, IConverter converter, IRegisterPaymentService registerPaymentService)
        {
            _paymentService = registerPaymentService;
            _settings = settings;
            _converter = converter;
        }


        [Route("file")]
        [HttpPost]
        public async Task<IActionResult> UploadFileToValidate(IFormFile file, [FromQuery] bool saveSuccessProcess)
        {
            try
            {
                Respuesta respuesta = new Respuesta();

                if (file.Length > 0 && file.FileName.Contains(".xls"))
                {
                    respuesta = await _paymentService.UploadFileToValidate(file, saveSuccessProcess);
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Route("payments")]
        [HttpGet]
        public async Task<List<dynamic>> GetPaymentsByStatus([FromQuery] string status)
        {
            try
            {
                return await _paymentService.GetPayments(status);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
        
        [Route("observations")]
        [HttpPost]
        public async Task<IActionResult> SetObservationPayments(CargueObservacionesPayment data)
        {
            try
            {
                var response = await _paymentService.SetObservationPayments(data.observaciones, data.CarguePagoId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
               
        [Route("{uploadedOrderId}")]
        [HttpPut]
        public async Task<IActionResult> DeletePaymentOrder(int uploadedOrderId)
        {
            try
            {
                string username = User.Identity.Name;
                var result = await _paymentService.DeletePayment(uploadedOrderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("file/{uploadedOrderId}")]
        [HttpGet]
        public async Task<IActionResult> DownloadPayments(int uploadedOrderId)
        {
            try
            {
                var result = await _paymentService.DownloadPaymentsAsync(uploadedOrderId);
                
                if (result.IsSuccessful && !result.IsException)
                {
                    Stream stream = new FileStream(result.Data.ToString(), FileMode.Open, FileAccess.Read);

                    if (stream == null)
                        return NotFound();
                    var file = File(stream, "application/octet-stream");
                    return file;
                }
                else if (result.IsSuccessful && result.IsException)
                {
                    //Status409Conflict
                    return Ok(result);
                }
                return BadRequest("Archivo no encontrado");
            }
            catch (Exception e)
            {
                return BadRequest("Archivo no encontrado");
            }
        }
    }
}
