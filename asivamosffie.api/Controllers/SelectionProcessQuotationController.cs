using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelectionProcessQuotationController : ControllerBase
    {
        private readonly ISelectionProcessQuotationService _selectionProcessQuotationService;
        private readonly IOptions<AppSettings> _settings;


        public SelectionProcessQuotationController(IOptions<AppSettings> settings, ISelectionProcessQuotationService selectionProcessQuotationService)
        {
            _selectionProcessQuotationService = selectionProcessQuotationService;
            _settings = settings;

        }

        [HttpGet]
        public async Task<ActionResult<List<ProcesoSeleccionCotizacion>>> Get()
        {
            try
            {
                return await _selectionProcessQuotationService.GetSelectionProcessQuotation();
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
                var result = await _selectionProcessQuotationService.GetSelectionProcessQuotationById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> post([FromBody] ProcesoSeleccionCotizacion procesoSeleccionCotizacion)
        {
            Respuesta _response = new Respuesta();
            try
            {

                string usermodified = " ";
                //string usermodified = HttpContext.User.FindFirst("User").Value;
                procesoSeleccionCotizacion.UsuarioCreacion = usermodified;
                _response = await _selectionProcessQuotationService.Insert(procesoSeleccionCotizacion);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Data = ex.ToString();
                return Ok(_response);
            }
        }

        [HttpPut]
        public async Task<IActionResult> update(ProcesoSeleccionCotizacion procesoSeleccionCronograma)
        {
            Respuesta _reponse = new Respuesta();

            try
            {
                _reponse = await _selectionProcessQuotationService.Update(procesoSeleccionCronograma);
                return Ok(_reponse);
            }
            catch (Exception ex)
            {
                _reponse.Data = ex.ToString();
                return Ok(_reponse);
            }
        }
    }
}
