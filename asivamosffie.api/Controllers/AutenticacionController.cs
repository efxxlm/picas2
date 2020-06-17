using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.services.Interfaces;
using asivamosffie.model.Models;

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
        public async Task<IActionResult> PostIniciarSesion([FromBody] Usuario pUsuario)
        {
            try
            {
                Task<object> result = autenticacion.IniciarSesion(pUsuario);

                object respuesta = await result;
                
                return Ok(respuesta);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        


    }
}