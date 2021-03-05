﻿using System;
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
    [Authorize]
    public class ManageCheckListController : ControllerBase
    {
        public readonly IManageCheckListService _manageCheckListService;
        private readonly IOptions<AppSettings> _settings;

        public ManageCheckListController(IOptions<AppSettings> settings, IManageCheckListService manageCheckListService)
        {
            _manageCheckListService = manageCheckListService;
            _settings = settings;
        }

        [Route("CreateEditItem")]
        [HttpPost]
        public async Task<IActionResult> CreateEditItem([FromBody] ListaChequeoItem pListaChequeoItem)
        {
            try
            {
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
                var result = await _manageCheckListService.CreateEditCheckList(pListaChequeo);
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
        
        [Route("GetListItem")]
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

    }
}
