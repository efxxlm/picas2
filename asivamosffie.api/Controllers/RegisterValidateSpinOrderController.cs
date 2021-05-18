using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RegisterValidateSpinOrderController : Controller
    {
        private readonly IRegisterValidateSpinOrderService _registerValidateSpinOrderService;

        public RegisterValidateSpinOrderController(IRegisterValidateSpinOrderService registerValidateSpinOrderService)
        {
            _registerValidateSpinOrderService = registerValidateSpinOrderService;
        }

        [Route("ChangueStatusOrdenGiro")]
        [HttpPost]
        public async Task<IActionResult> ChangueStatusOrdenGiro([FromBody] OrdenGiro pOrdenGiro)
        {
            try
            {
                pOrdenGiro.UsuarioCreacion = User.Identity.Name;
                var result = await _registerValidateSpinOrderService.ChangueStatusOrdenGiro(pOrdenGiro);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("CreateEditSpinOrderObservations")]
        [HttpPost]
        public async Task<IActionResult> CreateEditSpinOrderObservations([FromBody] OrdenGiroObservacion pOrdenGiroObservacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pOrdenGiroObservacion.UsuarioCreacion = User.Identity.Name;
                respuesta = await _registerValidateSpinOrderService.CreateEditSpinOrderObservations(pOrdenGiroObservacion);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("GetListOrdenGiro")]
        public async Task<FileResult> GetListOrdenGiro([FromBody] DescargarOrdenGiro pDescargarOrdenGiro)
        {
            return File(await _registerValidateSpinOrderService.GetListOrdenGiro(pDescargarOrdenGiro), "application/pdf");
        }

        [HttpGet]
        [Route("GetObservacionOrdenGiroByMenuIdAndSolicitudPagoId")]
        public async Task<dynamic> GetObservacionOrdenGiroByMenuIdAndSolicitudPagoId([FromQuery] int pMenuId, int pOrdenGiroId, int pPadreId, string pTipoObservacionCodigo)
        {
            return await _registerValidateSpinOrderService.GetObservacionOrdenGiroByMenuIdAndSolicitudPagoId(pMenuId, pOrdenGiroId, pPadreId, pTipoObservacionCodigo);
        }
    }
}
