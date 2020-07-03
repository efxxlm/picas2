using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContributorController : ControllerBase
    {
        public readonly IContributorService _contributor;


        public ContributorController(IContributorService contributor)
        {
            _contributor = contributor;
        }

        [HttpGet]
        public async Task<List<Aportante>> Get()
        {
            try
            {
                var result = await _contributor.GetContributor();
                return result;
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
                var result = await _contributor.GetContributorById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost]
        public async Task<IActionResult> post(Aportante aportante)
        {
            try
            {
                var result = await _contributor.Insert(aportante);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("ClearInput")]
        [HttpGet]
        public string ClearInput(string text)
        {
            try
            {
                var result =  _contributor.clearimput(text);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
