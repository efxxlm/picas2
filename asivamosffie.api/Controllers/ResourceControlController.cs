using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.api.Responses;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.model.APIModels;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceControlController : ControllerBase
    {
        public readonly IResourceControlService _resource;


        public ResourceControlController(IResourceControlService resource)
        {
            _resource = resource;
        }

        [HttpGet]
        public async Task<List<ControlRecurso>> Get()
        {
            try
            {
                var result = await _resource.GetResourceControl();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetResourceFundingBySourceFunding/{id}")]
        public async Task<List<ControlRecurso>> GetSourceFundingBySourceFunding(int id)
        {
            try
            {
                var result = await _resource.GetResourceControlGridBySourceFunding(id);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
<<<<<<< HEAD
        [Route("GetResourceControlBySourceId")]
        public async Task<List<ResourceControlGrid>> GetResourceControlBySourceId(int idFuente)
        {
            var result = await _resourceControlService.GetResourceControlBySourceId(idFuente);
            return result;
        }

        [HttpGet]
        [Route("GetResourceControlGrid")]
        public async Task<List<ControlRecurso>> GetResourceControlGrid()
=======
        [Route("GetResourceControlById")]
        public async Task<ControlRecurso> GetById(int pId)
>>>>>>> 7fe76fd84bc2af36366dbe8c34d617c2224f501b
        {
            try
            {
                var result = await _resource.GetResourceControlById(pId);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

<<<<<<< HEAD
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
=======

        [HttpPost]
        [Route("CreateControlRecurso")]
        public async Task<Respuesta> CreateControlRecurso(ControlRecurso controlRecurso)
>>>>>>> 7fe76fd84bc2af36366dbe8c34d617c2224f501b
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                controlRecurso.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _resource.Insert(controlRecurso);
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return respuesta;
            }
        }

        [HttpPost]
        [Route("updateControlRecurso")]
        public async Task<Respuesta> UpdateControlRecurso(ControlRecurso controlRecurso)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                controlRecurso.UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _resource.Update(controlRecurso);
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return respuesta;
            }
        }

        [HttpPut]
        [Route("DeleteResourceFundingBySourceFunding")]
        public async Task<Respuesta> DeleteResourceFundingBySourceFunding(int id)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _resource.Delete(id, HttpContext.User.FindFirst("User").Value);
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return respuesta;
            }
        }

    }


}
