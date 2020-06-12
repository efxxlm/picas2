using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.services.Interfaces;
using asivamosffie.model.Models;
using Microsoft.Extensions.Options;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        public readonly IAutenticacionService  autenticacion;
        
        public AutenticacionController(IAutenticacionService pAutenticacion)
        {
            autenticacion = pAutenticacion;
        }

        [HttpPost]
        [Route("IniciarSesion")]
        public IActionResult PostIniciarSesion([FromBody] Usuario usuario)
        {
            var result = autenticacion.IniciarSesion(usuario.Email, usuario.Contrasena);

            return Ok(result);
        }
        
    }
}