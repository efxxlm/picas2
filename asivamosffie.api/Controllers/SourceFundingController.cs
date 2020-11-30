﻿using System;
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
    public class SourceFundingController : ControllerBase
    {
        public readonly ISourceFundingService _sourceFunding;


        public SourceFundingController(ISourceFundingService sourceFunding)
        {
            _sourceFunding = sourceFunding;
        }

        [HttpPost]
        [Route("CreateEditarVigenciaAporte")]
        public async Task<IActionResult> CreateEditarVigenciaAporte(VigenciaAporte vigenciaAporte)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                vigenciaAporte.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _sourceFunding.CreateEditarVigenciaAporte(vigenciaAporte);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
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
        public async Task<FuenteFinanciacion> GetById(int id)
        {
            try
            {
                var result = await _sourceFunding.GetISourceFundingById(id);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("GetSourceFundingGrid")]
        [HttpGet]
        public async Task<List<SourceFundingGrid>> GetSourceFundingGrid()
        {

            string pUsuarioModifico = "";
            //string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
            var respuesta = await _sourceFunding.GetSourceFundingGrid();
            return respuesta;

        }

        [Route("GetSourceFundingByIdAportante")]
        [HttpGet]
        public async Task<List<FuenteFinanciacion>> GetSourceFundingByIdAportante(int idAportante)
        {

            var response = await _sourceFunding.GetSourceFundingByIdAportante(idAportante);
            return response;

        }

        // Agregar Fuente de recursos
        [HttpPost]
        [Route("CreateEditFuentesFinanciacion")]
        public async Task<IActionResult> CreateEditFuentesFinanciacion(FuenteFinanciacion fuentefinanciacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                if(fuentefinanciacion.FuenteFinanciacionId>0)
                {
                    fuentefinanciacion.UsuarioModificacion= HttpContext.User.FindFirst("User").Value.ToUpper();
                }
                else
                {
                    fuentefinanciacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value.ToUpper();
                }
                
                respuesta = await _sourceFunding.CreateEditFuentesFinanciacion(fuentefinanciacion);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }


        [HttpPut]
        [Route("EditFuentesFinanciacion")]
        public async Task<IActionResult> EditFuentesFinanciacion(FuenteFinanciacion fuentefinanciacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                fuentefinanciacion.UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _sourceFunding.EditFuentesFinanciacion(fuentefinanciacion);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }


        [HttpDelete]
        [Route("EliminarFuentesFinanciacion")]
        public async Task<IActionResult> EliminarFuentesFinanciacion(int id)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _sourceFunding.EliminarFuentesFinanciacion(id, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
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
        [Route("GetListFuentesFinanciacionByAportanteId")]        
        public async Task<List<GrillaFuentesFinanciacion>> GetListFuentesFinanciacionByAportanteId(int AportanteId)
        {
            try
            {
                var result = await _sourceFunding.GetListFuentesFinanciacionByAportanteId(AportanteId);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetListFuentesFinanciacionByDisponibilidadPresupuestalProyectoid")]
        public async Task<List<GrillaFuentesFinanciacion>> GetListFuentesFinanciacionByDisponibilidadPresupuestalProyectoid(int disponibilidadPresupuestalProyectoid, int aportanteID)
        {
            try
            {
                var result = await _sourceFunding.GetListFuentesFinanciacionByDisponibilidadPresupuestalProyectoid(disponibilidadPresupuestalProyectoid, aportanteID);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetListFuentesFinanciacionByDisponibilidadPresupuestald")]
        public async Task<List<GrillaFuentesFinanciacion>> GetListFuentesFinanciacionByDisponibilidadPresupuestald(int disponibilidadPresupuestaId)
        {
            try
            {
                var result = await _sourceFunding.GetListFuentesFinanciacionByDisponibilidadPresupuestald(disponibilidadPresupuestaId);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetListFuentesFinanciacionshort")]
        public async Task<List<FuenteFinanciacion>> GetListFuentesFinanciacionshort()
        {
            try
            {
                var result = await _sourceFunding.GetListFuentesFinanciacionshort();
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
