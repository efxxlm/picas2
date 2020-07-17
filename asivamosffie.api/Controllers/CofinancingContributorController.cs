using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services;
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

        [HttpPut]
        public async Task<IActionResult> Update(CofinanciacionAportante CofnaAportante)
        {
            try
            {
                var result = await _contributor.Update(CofnaAportante);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("SaveBudgetRegister")]
        [HttpPost]
        public async Task<IActionResult> SaveBudgetRegister(RegistroPresupuestal registroPresupuestal)
        {
            try
            {
                var result = await _contributor.BudgetRecords(registroPresupuestal);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("UpdateRegisterBudget")]
        [HttpPut]
        public async Task<IActionResult> UpdateRegisterBudget(RegistroPresupuestal registroPresupuestal)
        {
            try
            {
                var result = await _contributor.UpdateBudgetRegister(registroPresupuestal);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetRegisterBudget")]
        [HttpGet]
        public async Task<ActionResult<List<RegistroPresupuestal>>> GetRegisterBudget()
        {
            try
            {
                return await _contributor.GetRegisterBudget();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[Route("GetRegisterBudgetById")]
        [Route("GetRegisterBudgetById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetRegisterBudgetById(int id)
        {
            //update v2
            try
            {
                var result = await _contributor.GetRegisterBudgetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
