using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.services.Interfaces;
using asivamosffie.model.Models;
using Microsoft.Extensions.Options;
using lalupa.Authorization.JwtHelpers;
using asivamosffie.services.Exceptions;
using asivamosffie.model.APIModels;


namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterPersonalProgrammingController : Controller
    {
        public readonly IRegisterPersonalProgrammingService _IRegisterPersonalProgrammingService;


        public RegisterPersonalProgrammingController(IRegisterPersonalProgrammingService registerPersonalProgrammingService)
        {

            _IRegisterPersonalProgrammingService = registerPersonalProgrammingService;
        }
        [Route("GetListProyectos")]
        [HttpGet]
        public async Task<List<VRegistrarPersonalObra>> GetListProyectos()
        {
            return await _IRegisterPersonalProgrammingService.GetListProyectos();

        }

        [Route("GetProgramacionPersonalByContratoId")]
        [HttpGet]
        public async Task<List<SeguimientoSemanal>> GetProgramacionPersonalByContratoId([FromQuery] int pContratacionProyectoId)
        {
            return await _IRegisterPersonalProgrammingService.GetProgramacionPersonalByContratoId(pContratacionProyectoId, "");
        }

        [Route("UpdateSeguimientoSemanalPersonalObra")]
        [HttpPost]
        public async Task<IActionResult> UpdateSeguimientoSemanalPersonalObra([FromBody] List<SeguimientoSemanal> pSeguimientoSemanal)
        {
            try
            {
                Task<Respuesta> result = _IRegisterPersonalProgrammingService.UpdateSeguimientoSemanalPersonalObra(pSeguimientoSemanal);
                object respuesta = await result;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Route("ChangeStatusProgramacionContratoPersonal")]
        [HttpPost]
        public async Task<IActionResult> ChangeStatusProgramacionContratoPersonal([FromQuery] int pContratoConstruccionId, string pEstadoProgramacionCodigo)
        {
            try
            {
                Task<Respuesta> result = _IRegisterPersonalProgrammingService.ChangeStatusProgramacionContratoPersonal(pContratoConstruccionId, pEstadoProgramacionCodigo, HttpContext.User.FindFirst("User").Value);
                object respuesta = await result;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
