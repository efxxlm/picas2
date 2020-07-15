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
    public class SelectionProcessController : ControllerBase
    {
        private readonly ISelectionProcessService _selectionProcessService;
        private readonly IOptions<AppSettings> _settings;


        public SelectionProcessController(IOptions<AppSettings> settings, ISelectionProcessService selectionProcessService)
        {
            _selectionProcessService = selectionProcessService;
            _settings = settings;

        }

        [HttpGet]
        public async Task<ActionResult<List<ProcesoSeleccion>>> Get()
        {
            try
            {
                return await _selectionProcessService.GetSelectionProcess();
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
                var result = await _selectionProcessService.GetSelectionProcessById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> post([FromBody] ProcesoSeleccion procesoSeleccion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                string usermodified = " ";
                //string usermodified = HttpContext.User.FindFirst("User").Value;
                procesoSeleccion.UsuarioCreacion = usermodified;
                respuesta = await _selectionProcessService.Insert(procesoSeleccion);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return Ok(respuesta);
            }
        }
        
        
       

    }
}
