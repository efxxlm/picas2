using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectContractingController : ControllerBase
    {
      

        // GET: api/Contracting/5
        [HttpGet]
        public string GetListProyectosCompletosByFiltros(int id)
        {
            return "value";
        }

       
    }
}
