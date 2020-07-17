using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.api.Responses;
using asivamosffie.model.APIModels;
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
                var result = await _sourceFunding.Insert(fuentefinanciacion);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        public async Task<IActionResult> update(FuenteFinanciacion fuentefinanciacion)
        {
            Respuesta _response = new Respuesta();

            try
            {
                _response = await _sourceFunding.Update(fuentefinanciacion);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Data = ex.ToString();
                return Ok(_response);
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _sourceFunding.Delete(id);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }
    }
}
