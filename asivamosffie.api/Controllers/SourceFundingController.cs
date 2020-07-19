using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.api.Responses;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourceFundingController : ControllerBase
    {
        public readonly ISourceFundingService _sourceFunding;


        public SourceFundingController(ISourceFundingService sourceFunding)
        {
            _sourceFunding = sourceFunding;
        }

        [HttpGet]
        public async Task<List<FuenteFinanciacion>> Get()
        {
            try
            {
                var result = await _sourceFunding.GetISourceFunding();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _sourceFunding.GetISourceFundingById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        // Agregar Fuente de recursos
        [HttpPost]
        public async Task<IActionResult> post(FuenteFinanciacion fuentefinanciacion)
        {
            try
            {  
                fuentefinanciacion.UsuarioModificacion = HttpContext.User.FindFirst("User").Value; 
                var result = await _sourceFunding.Insert(fuentefinanciacion);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPut]
        public async Task<IActionResult> put(FuenteFinanciacion fuentefinanciacion)
        {
            try
            {
                fuentefinanciacion.UsuarioCreacion =  HttpContext.User.FindFirst("User").Value; 
                var result = await _sourceFunding.Update(fuentefinanciacion);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            { 
                var result = await _sourceFunding.Delete(id , HttpContext.User.FindFirst("User").Value);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetFuentesFinanciacionByAportanteId")]
        public async Task<List<FuenteFinanciacion>> GetFuentesFinanciacionByAportanteId(int AportanteId)
        {
            try
            {
                var result = await _sourceFunding.GetFuentesFinanciacionByAportanteId(AportanteId);
                return result; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetListFuentesFinanciacion")]
        public async Task<List<FuenteFinanciacion>> GetListFuentesFinanciacion()
        {
            try
            {
                var result = await _sourceFunding.GetListFuentesFinanciacion();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


}
