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
    public class BudgetAvailabilityController : ControllerBase
    {
        private readonly IBudgetAvailabilityService _budgetAvailabilityService;
        private readonly IOptions<AppSettings> _settings;
        public BudgetAvailabilityController(IOptions<AppSettings> settings, IBudgetAvailabilityService budgetAvailabilityService)
        {
            _budgetAvailabilityService = budgetAvailabilityService;
            _settings = settings;

        }

        [Route("GetGrillaDisponibilidadPresupuestal")]
        [HttpGet]
        public async Task<ActionResult<List<GrillaDisponibilidadPresupuestal2>>> GetGrillaDisponibilidadPresupuestal2()
        {
            return await _budgetAvailabilityService.GetGrillaDisponibilidadPresupuestal2(_settings.Value.asivamosffieDatabase);
        }


        [HttpGet]
        public async Task<ActionResult<List<DisponibilidadPresupuestal>>> Get()
        {
            try
            {
                return await _budgetAvailabilityService.GetBudgetAvailability();
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
                var result = await _budgetAvailabilityService.GetBudgetAvailabilityById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("GetGridBudgetAvailability")]
        public async Task<IActionResult> GetGridBudgetAvailability(int? DisponibilidadPresupuestalId)
        {
            try
            {
                var result = await _budgetAvailabilityService.GetGridBudgetAvailability(DisponibilidadPresupuestalId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("CreateEditarDP")]
        public async Task<IActionResult> CreateEditarDP([FromBody] DisponibilidadPresupuestal DP)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                DP.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _budgetAvailabilityService.CreateEditarDisponibilidadPresupuestal(DP);
                return Ok(respuesta);
                //
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpDelete]
        [Route("DeleteBudgetAvailability")]
        public async Task<IActionResult> DeleteBudgetAvailability(int id)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _budgetAvailabilityService.DeleteBudgetAvailability(id, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }


        [HttpPost]
        [Route("CreateEditarDPProyecto")]
        public async Task<IActionResult> CreateEditarDPProyecto([FromBody] DisponibilidadPresupuestalProyecto PProyecto)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                PProyecto.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _budgetAvailabilityService.CreateEditarDPProyecto(PProyecto);
                return Ok(respuesta);
                //
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("GetAssociatedProjects")]
        public async Task<IActionResult> GetAssociatedProjects(int ProyectoId)
        {
            try
            {
                var result = await _budgetAvailabilityService.GetAssociatedProjects(ProyectoId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
