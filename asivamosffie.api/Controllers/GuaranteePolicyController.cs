using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuaranteePolicyController : ControllerBase
    {
        public readonly IGuaranteePolicyService _guaranteePolicy;

        public GuaranteePolicyController(IGuaranteePolicyService guaranteePolicy)
        {
            _guaranteePolicy = guaranteePolicy;
        }


        [HttpPost]
        [Route("CreateContratoPoliza")]
        public async Task<IActionResult> InsertContratoPoliza(ContratoPoliza contratoPoliza)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //cuentaBancaria.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _guaranteePolicy.InsertContratoPoliza(contratoPoliza);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }
               

        [Route("GetListPolizaObservacionByContratoPolizaId")]
        [HttpGet]
        public async Task<ActionResult<List<PolizaGarantia>>> GetListPolizaObservacionByContratoPolizaId(int pContratoPolizaId)
        {
            try
            {
                return await _guaranteePolicy.GetListPolizaGarantiaByContratoPolizaId(pContratoPolizaId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetListPolizaGarantiaByContratoPolizaId")]
        [HttpGet]        
        public async Task<ActionResult<List<PolizaGarantia>>> GetListPolizaGarantiaByContratoPolizaId(int pContratoPolizaId)
        {
            try
            {
                return await _guaranteePolicy.GetListPolizaGarantiaByContratoPolizaId(pContratoPolizaId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetListVistaContratoGarantiaPoliza")]
        public async Task<ActionResult<List<VistaContratoGarantiaPoliza>>> GetListVistaContratoGarantiaPoliza()
        {
            try
            {
                return await _guaranteePolicy.ListVistaContratoGarantiaPoliza();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpGet]
        [Route("GetListGrillaContratoGarantiaPoliza")]
        public async Task<ActionResult<List<GrillaContratoGarantiaPoliza>>> GetListGrillaContratoGarantiaPoliza()
        {
            try
            {
                return await _guaranteePolicy.ListGrillaContratoGarantiaPoliza();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
