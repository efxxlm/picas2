using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using asivamosffie.model.APIModels;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetAvailabilityController : ControllerBase
    {

        private readonly IBudgetAvailabilityService _budgetAvailabilityService;

        public BudgetAvailabilityController(IBudgetAvailabilityService budgetAvailabilityService)
        {
            _budgetAvailabilityService = budgetAvailabilityService; 
        }

        [Route("ListAdministrativeProject")]
        [HttpGet]
        public async Task<List<DisponibilidadPresupuestalGrilla>> ListAdministrativeProjects()
        {
            // string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
            var respuesta = await _budgetAvailabilityService.GetListDisponibilidadPresupuestal();
            return respuesta; 
        }

        [Route("GetFuenteFinanciacionByIdAportanteId")]
        [HttpGet]
        public async Task<FuenteFinanciacion> GetFuenteFinanciacionByIdAportanteId(int pAportanteId)
        {
            // string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
            var respuesta = await _budgetAvailabilityService.GetFuenteFinanciacionByIdAportanteId(pAportanteId);
            return respuesta;
        }

        [Route("GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud")]
        [HttpGet]
        public async Task<List<DisponibilidadPresupuestalGrilla>> GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud(string pCodigoEstadoSolicitud)
        {
          
            var respuesta = await _budgetAvailabilityService.GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud(pCodigoEstadoSolicitud);
            return respuesta;
        }


 
    }
}
