using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CofinancingController : ControllerBase
    {
        private readonly ICofinancingService _Cofinancing;

        public CofinancingController(ICofinancingService cofinancingService)
        {
            _Cofinancing = cofinancingService;
        }


        [Route("CreateorUpdateCofinancing")]
        [HttpPost]
        public async Task<IActionResult> CreateCofinancing([FromBody] Cofinanciacion pCofinanciacion)
        {
            try
            { 
                HttpContext.Connection.RemoteIpAddress.ToString();
               // pCofinanciacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                Task<object> result = _Cofinancing.CreateorUpdateCofinancing(pCofinanciacion);
                object respuesta = await result;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            } 
        }

        [Route("GetCofinancing")]
        [HttpGet]
        public async Task<List<Cofinanciacion>> GetCofinancing()
        {  
            var result = await _Cofinancing.GetListCofinancing();
            return result;
        }


        [Route("GetDocument")]
        [HttpGet]
        public async Task<ActionResult<List<DocumentoApropiacion>>> GetDocument(int ContributorId)
        {
            try
            {
                return await _Cofinancing.GetDocument(ContributorId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
