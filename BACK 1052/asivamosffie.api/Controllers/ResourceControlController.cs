using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceControlController : ControllerBase
    {
        public readonly IResourceControlService _resourceControlService;


        public ResourceControlController(IResourceControlService resourceControlService)
        {
            _resourceControlService = resourceControlService;
        }

        [HttpGet]
        public async Task<List<ControlRecurso>> Get()
        {
            var result = await _resourceControlService.GetResourceControl();
            return result;
        }


        [HttpGet]
        [Route("GetResourceControlGrid")]
        public async Task<List<ControlRecurso>> GetResourceControlGrid()
        {
            var result = await _resourceControlService.GetResourceControlGrid();
            return result;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _resourceControlService.GetResourceControlById(id);
            return Ok(result);
        }


        // Agregar control recurso
        [HttpPost]
        public async Task<IActionResult> post(ControlRecurso controlRecurso)
        {
           
            var result = await _resourceControlService.Insert(controlRecurso);
            return Ok(result);
          
        }

        [HttpPut]
        public async Task<IActionResult> update(ControlRecurso controlRecurso)
        {
            Respuesta _response = new Respuesta();

            try
            {
                _response = await _resourceControlService.Update(controlRecurso);
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
