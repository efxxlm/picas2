using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelectionProcessGroupController : ControllerBase
    {
        private readonly ISelectionProcessGroupService _selectionProcessGroupService;
        private readonly IOptions<AppSettings> _settings;


        public SelectionProcessGroupController(IOptions<AppSettings> settings, ISelectionProcessGroupService selectionProcessGroupService)
        {
            _selectionProcessGroupService = selectionProcessGroupService;
            _settings = settings;

        }

        [HttpGet]
        public async Task<ActionResult<List<ProcesoSeleccionGrupo>>> Get()
        {
            try
            {
                return await _selectionProcessGroupService.GetSelectionProcessGroup();
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
                var result = await _selectionProcessGroupService.GetSelectionProcessGroupById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> post([FromBody] ProcesoSeleccionGrupo procesoSeleccionGrupo)
        {
            Respuesta _response = new Respuesta();
            try
            {

                string usermodified = " ";
                //string usermodified = HttpContext.User.FindFirst("User").Value;
                procesoSeleccionGrupo.UsuarioCreacion = usermodified;
                _response = await _selectionProcessGroupService.Insert(procesoSeleccionGrupo);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Data = ex.ToString();
                return Ok(_response);
            }
        }

        [HttpPut]
        public async Task<IActionResult> update(ProcesoSeleccionGrupo procesoSeleccionGrupo)
        {
            Respuesta _response = new Respuesta();

            try
            {
                _response = await _selectionProcessGroupService.Update(procesoSeleccionGrupo);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Data = ex.ToString();
                return Ok(_response);
            }
        }
    }
}
