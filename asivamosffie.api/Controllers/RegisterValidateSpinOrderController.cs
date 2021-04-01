using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using asivamosffie.model.APIModels;
using System.IO;
using Microsoft.Extensions.Options;
using System.Reflection;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

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
            try
            {
                pOrdenGiroObservacion.UsuarioCreacion = User.Identity.Name;
                var result = await _registerValidateSpinOrderService.CreateEditSpinOrderObservations(pOrdenGiroObservacion);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetListOrdenGiro")]
        public async Task<FileResult> GetListOrdenGiro([FromQuery] bool pBlRegistrosAprobados)
        {
            return File(await _registerValidateSpinOrderService.GetListOrdenGiro(pBlRegistrosAprobados), "application/pdf");
        }

        [HttpGet]
        [Route("GetListOrdenGiro")]
        public async Task<dynamic> GetObservacionOrdenGiroByMenuIdAndSolicitudPagoId([FromQuery] int pMenuId, int pOrdenGiroId, int pPadreId, string pTipoObservacionCodigo)
        {
            return await _registerValidateSpinOrderService.GetObservacionOrdenGiroByMenuIdAndSolicitudPagoId(pMenuId, pOrdenGiroId, pPadreId, pTipoObservacionCodigo);
        }
    }
}
