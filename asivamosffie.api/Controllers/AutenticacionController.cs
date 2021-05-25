using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        public readonly IAutenticacionService autenticacion;
        private readonly IOptions<AppSettings> _settings;

        public AutenticacionController(IAutenticacionService pAutenticacion, IOptions<AppSettings> settings)
        {
            autenticacion = pAutenticacion;
            _settings = settings;
        }

        [HttpPost]
        [Route("IniciarSesion")]
        public async Task<IActionResult> PostIniciarSesion([FromBody] Usuario pUsuario)
        {
            try
            {
                Task<Respuesta> result = autenticacion.IniciarSesion(pUsuario, _settings.Value.Secret, _settings.Value.asivamosffieIssuerJwt, _settings.Value.asivamosffieAudienceJwt);

                Respuesta respuesta = await result;

                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}