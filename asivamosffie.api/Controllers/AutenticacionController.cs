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
using asivamosffie.services.Models;
using asivamosffie.services.Exceptions;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        public readonly IAutenticacionService  autenticacion;
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
            try{
                Task<Respuesta> result = autenticacion.IniciarSesion(pUsuario,_settings.Value.Secret,_settings.Value.asivamosffieIssuerJwt, _settings.Value.asivamosffieAudienceJwt);

                Respuesta respuesta = await result;
                               
                return Ok(respuesta);

             }
             catch(Exception ex)
             {
                 throw ex;
             }
        }
        
    }
}