using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using asivamosffie.model.APIModels;
using System.IO;
using Microsoft.Extensions.Options;
using System.Reflection;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ManageCheckListController : ControllerBase
    {
        public readonly IManageCheckListService _manageCheckListService;

        public ManageCheckListController(IManageCheckListService manageCheckListService)
        {
            _manageCheckListService = manageCheckListService;
        }
 
        [Route("DeleteListaChequeoItem")]
        [HttpPost]
        public async Task<IActionResult> DeleteListaChequeoItem([FromQuery] int pListaChequeoListaChequeoItemId)
        {
            try
            { 
                var result = await _manageCheckListService.DeleteListaChequeoItem(pListaChequeoListaChequeoItemId, User.Identity.Name);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }      

        [Route("ActivateDeactivateListaChequeo")]
        [HttpPost]
        public async Task<IActionResult> ActivateDeactivateListaChequeo([FromBody] ListaChequeo pListaChequeo)
        {
            try
            {
                pListaChequeo.UsuarioCreacion = User.Identity.Name;
                var result = await _manageCheckListService.ActivateDeactivateListaChequeo(pListaChequeo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("ActivateDeactivateListaChequeoItem")]
        [HttpPost]
        public async Task<IActionResult> ActivateDeactivateListaChequeoItem([FromBody] ListaChequeoItem pListaChequeoItem)
        {
            try
            {
                pListaChequeoItem.UsuarioCreacion = User.Identity.Name;
                var result = await _manageCheckListService.ActivateDeactivateListaChequeoItem(pListaChequeoItem);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("CreateEditItem")]
        [HttpPost]
        public async Task<IActionResult> CreateEditItem([FromBody] ListaChequeoItem pListaChequeoItem)
        {
            try
            {
                pListaChequeoItem.UsuarioCreacion = User.Identity.Name;
                var result = await _manageCheckListService.CreateEditItem(pListaChequeoItem);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("CreateEditCheckList")]
        [HttpPost]
        public async Task<IActionResult> CreateEditCheckList([FromBody] ListaChequeo pListaChequeo)
        {
            try
            {
                pListaChequeo.UsuarioCreacion = User.Identity.Name;
                var result = await _manageCheckListService.CreateEditCheckList(pListaChequeo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        
        [Route("GetValidateExistNameCheckList")]
        [HttpPost]
        public async Task<IActionResult> GetValidateExistNameCheckList([FromBody] ListaChequeo plistaChequeo)
        {
            try
            {
                var result = await _manageCheckListService.GetValidateExistNameCheckList(listaChequeo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }     
        
        [Route("GetListItem")]
        [HttpGet]
        public async Task<IActionResult> GetListItem()
        {
            try
            {
                var result = await _manageCheckListService.GetListItem();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("GetCheckList")]
        [HttpGet]
        public async Task<IActionResult> GetCheckList()
        {
            try
            {
                var result = await _manageCheckListService.GetCheckList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("GetListaChequeoItemByListaChequeoId")]
        [HttpGet]
        public async Task<IActionResult> GetListaChequeoItemByListaChequeoId([FromQuery] int ListaChequeoId)
        {
            try
            {
                var result = await _manageCheckListService.GetListaChequeoItemByListaChequeoId(ListaChequeoId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
         
        [Route("GetListaChequeoItemByListaChequeoItemId")]
        [HttpGet]
        public async Task<IActionResult> GetListaChequeoItemByListaChequeoItemId([FromQuery] int ListaChequeoItemId)
        {
            try
            {
                var result = await _manageCheckListService.GetListaChequeoItemByListaChequeoItemId(ListaChequeoItemId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Route("SendEmailWhenDesactiveListaChequeo")]
        [HttpGet]
        public async Task<bool> SendEmailWhenDesactiveListaChequeo([FromQuery] int pListaChequeoId)
        {
            try
            {
                return await _manageCheckListService.SendEmailWhenDesactiveListaChequeo(pListaChequeoId);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
