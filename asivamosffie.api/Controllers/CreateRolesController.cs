using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CreateRolesController : ControllerBase
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly ICreateRolesService _createRolesService;
        public CreateRolesController(IOptions<AppSettings> settings, ICreateRolesService createRolesService)
        {
            _createRolesService = createRolesService;
            _settings = settings;
        }

        [HttpPost]
        [Route("ActivateDeactivatePerfil")]
        public async Task<IActionResult> ActivateDeactivatePerfil([FromBody] Perfil pPerfil)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pPerfil.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _createRolesService.ActivateDeactivatePerfil(pPerfil);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("CreateEditRolesPermisos")]
        public async Task<IActionResult> CreateEditRolesPermisos([FromBody] Perfil pPerfil)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pPerfil.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;

                respuesta = await _createRolesService.CreateEditRolesPermisos(pPerfil);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpGet]
        [Route("GetPerfilByPerfilId")]
        public async Task<Perfil> GetPerfilByPerfilId([FromQuery] int pPerfilId)
        {
            try
            {
                return await _createRolesService.GetPerfilByPerfilId(pPerfilId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("ValidateExistNamePerfil")]
        public async Task<bool> ValidateExistNamePerfil([FromQuery] string pNamePerfil)
        {
            try
            {
                return await _createRolesService.ValidateExistNamePerfil(pNamePerfil);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetMenu")]
        public async Task<dynamic> GetMenu()
        {
            try
            {
                return await _createRolesService.GetMenu();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetListPerfil")]
        public async Task<dynamic> GetListPerfil()
        {
            try
            {
                return await _createRolesService.GetListPerfil();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
