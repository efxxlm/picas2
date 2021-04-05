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
        private readonly IRegisterPayPerformanceService _paymentAndPerformancesService;
        private readonly IOptions<AppSettings> _settings;
        private readonly IConverter _converter;
         
        public RegisterPayPerformanceController(IOptions<AppSettings> settings, IConverter converter, IRegisterPayPerformanceService registerPayPerformanceService)
        {
            _paymentAndPerformancesService = registerPayPerformanceService;
            _settings = settings;
            _converter = converter;
        }


        [Route("uploadFileToValidate")]
        [HttpPost]
        public async Task<IActionResult> UploadFileToValidate(IFormFile file, [FromQuery] string typeFile, bool saveSuccessProcess)
        {
            try
            {
                Respuesta respuesta = new Respuesta();

                if (file.Length > 0 && file.FileName.Contains(".xls"))
                {
                    string strUsuario = User.Identity.Name.ToUpper();
                    respuesta = await _paymentAndPerformancesService.UploadFileToValidate(file, strUsuario, typeFile, saveSuccessProcess);
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
        public async Task<List<dynamic>> GetPaymentsPerformances([FromQuery] string typeFile, [FromQuery] string status)
        {
            try
            {
                return await _paymentAndPerformancesService.getPaymentsPerformances(typeFile, status);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        [Route("setObservationPaymentsPerformances")]
        [HttpPost]
        public async Task<IActionResult> setObservationPaymentsPerformances(CargueObservacionesPaymentPerformance data)
        {
            try
            {
                var response = await _paymentAndPerformancesService.SetObservationPayments(data.typeFile, data.observaciones, data.cargaPagosRendimientosId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Route("deletePaymentPerformance")]
        [HttpGet]
        public async Task<IActionResult> DeletePaymentOrder(int uploadedOrderId)
        {
            try
            {
                string username = User.Identity.Name;
                var result = await _paymentAndPerformancesService.DeletePayment(uploadedOrderId, username);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("managePerformance")]
        [HttpGet]
        public async Task<IActionResult> ManagePerformance(int uploadedOrderId)
        {
            try
            {
                var result = await _paymentAndPerformancesService.ManagePerformanceAsync(uploadedOrderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [Route("SendInconsistencies")]
        [HttpGet]
        public async Task<IActionResult> NotifyInconsistencies(int uploadedOrderId)
        {
            try
            {
                string author = User.Identity.Name;
                var result = await _paymentAndPerformancesService.NotifyEmailPerformanceInconsistencies(uploadedOrderId, author);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("requestApproval")]
        [HttpGet]
        public async Task<IActionResult> RequestApproval(int uploadedOrderId)
        {
            try
            {
                var result = await _paymentAndPerformancesService.NotifyRequestManagedPerformancesApproval(uploadedOrderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region managedperformances
        [Route("downloadManagedPerformances")]
        [HttpPost]
        public async Task<IActionResult> DownloadManagedPerformances([FromQuery]int uploadedOrderId, [FromQuery] bool? queryConsistentOrders)
        {
            try
            {
                string author = User.Identity.Name;
                var result = await _paymentAndPerformancesService.GetManagedPerformancesByStatus(author, uploadedOrderId, queryConsistentOrders);
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("downloadPaymentPerformance")]
        [HttpPost]
        public async Task<IActionResult> DownloadPaymentPerformance([FromBody] FileRequest fileRequest, [FromQuery]string fileType)
        {
            //if (String.IsNullOrEmpty(pNameFiles))
            //    return BadRequest();
            try
            {
                fileRequest.Username = User.Identity.Name;
                var result = await _paymentAndPerformancesService.DownloadPaymentPerformanceAsync(fileRequest, fileType);

                if (result.IsSuccessful && !result.IsException)
                {
                    /*string Ruta = archivoCargue.Ruta + '/' + archivoCargue.Nombre + ".xlsx";
                    */
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

        [Route("downloadPerformancesInconsistencies")]
        [HttpPost]
        public async Task<IActionResult> GetPerformancesInconsistencies(int uploadedOrderId)
        {
           
            try
            {
                string author = User.Identity.Name;
                var result = await _paymentAndPerformancesService.GetManagedPerformancesByStatus(author, uploadedOrderId, false);



                if (result.IsSuccessful && !result.IsException)
                {
                    /*string Ruta = archivoCargue.Ruta + '/' + archivoCargue.Nombre + ".xlsx";
                    */
                    string ruta = result.Data.ToString();
                    Stream stream = new FileStream(ruta, FileMode.Open, FileAccess.Read);

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
            catch (Exception ex)
            {
                // throw ex;
                return BadRequest("Archivo no encontrado"); /// ?
            }
        }
        #endregion


        [Route("requestedApprovalPerformances")]
        [HttpGet]
        public async Task<IEnumerable<dynamic>> GetRequestedApprovalPerformances()
        {
            try
            {
                var  list =  await _paymentAndPerformancesService.GetRequestedApprovalPerformances();
                return list;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        [Route("includePerformances")]
        [HttpPost]
        public async Task<Respuesta> IncludePerformances(int uploadedOrderId)
        {
            try
            {
                var list = await _paymentAndPerformancesService.IncludePerformances(uploadedOrderId);
                return list;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        [Route("uploadMinutes")]
        [HttpPost]
        public async Task<Respuesta> UploadSignedMinutes(int uploadedOrderId)
        {
            try
            {
                var list = await _paymentAndPerformancesService.IncludePerformances(uploadedOrderId);
                return list;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        [Route("downloadTemplate")]
        [HttpPost]
        public async Task<IActionResult> DownloadTemplateMinutes(IFormFile file, [FromQuery] string typeFile, bool saveSuccessProcess)
        {
            try
            {
                Respuesta respuesta = new Respuesta();

                if (file.Length > 0 && file.FileName.Contains(".xls"))
                {
                    string strUsuario = User.Identity.Name;
                    respuesta = await _paymentAndPerformancesService.UploadFileToValidate(file, strUsuario, typeFile, saveSuccessProcess);
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
