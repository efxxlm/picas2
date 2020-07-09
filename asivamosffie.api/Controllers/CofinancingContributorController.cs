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
    public class CofinancingContributorController : ControllerBase
    {
        public readonly ICofinancingContributorService _contributor;


        public CofinancingContributorController(ICofinancingContributorService contributor)
        {
            _contributor = contributor;
        }

        [HttpGet]
        public async Task<ActionResult<List<CofinanciacionAportante>>> Get()
        {
            try
            {
                return await _contributor.GetContributor();
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

        [Route("GetControlGrid")]
        [HttpGet]
        public async Task<ActionResult<List<CofinanciacionAportante>>> GetControlGrid(int ContributorId)
        {
            try
            {
               return await _contributor.GetControlGrid(ContributorId);
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public async Task<IActionResult> post(CofinanciacionAportante CofnaAportante)
        {
            try
            {
                var result = await _contributor.Insert(CofnaAportante);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
