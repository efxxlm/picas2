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
    public class RegisterContractsAndContractualModificationsController : Controller
    {
        public readonly IRegisterContractsAndContractualModificationsService _registerContractsService;
        private readonly IOptions<AppSettings> _settings;
        public RegisterContractsAndContractualModificationsController(
            IRegisterContractsAndContractualModificationsService registerContractsService,
            IOptions<AppSettings> settings
            )
        {
            _registerContractsService = registerContractsService;
            _settings = settings;
        }

        [HttpGet]
        [Route("GetListSesionComiteSolicitudV2")]
        public async Task<List<VListaContratacionModificacionContractual>> GetListSesionComiteSolicitudV2()
        {
            return await _registerContractsService.GetListSesionComiteSolicitudV2();
        }

        [HttpGet]
        [Route("GetListSesionComiteSolicitud")]
        public async Task<List<SesionComiteSolicitud>> GetListSesionComiteSolicitud()
        {
            return await _registerContractsService.GetListSesionComiteSolicitud();
        }

        [HttpGet]
        [Route("GetContratacionByContratacionId")]
        public async Task<Contratacion> GetContratacionByContratacionId([FromQuery] int ContratacionId)
        {
            return await _registerContractsService.GetContratacionByContratacionId(ContratacionId);
        }

        [Route("RegistrarTramiteContrato")]
        [HttpPost]
        public async Task<IActionResult> RegistrarTramiteContrato([FromForm] Contrato pContrato, string pEstadoCodigo)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pContrato.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerContractsService.RegistrarTramiteContrato(pContrato,
                 Path.Combine(_settings.Value.DirectoryBase, _settings.Value.DirectoryBaseRutaDocumentoContrato), pEstadoCodigo, _settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("RegistrarTramiteNovedadContractual")]
        [HttpPost]
        public async Task<IActionResult> RegistrarTramiteNovedadContractual([FromBody] NovedadContractual pNovedadContractual)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pNovedadContractual.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerContractsService.RegistrarTramiteNovedadContractual(pNovedadContractual);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("ChangeStateTramiteNovedad")]
        [HttpPost]
        public async Task<IActionResult> ChangeStateTramiteNovedad([FromQuery] int pNovedadContractualId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _registerContractsService.ChangeStateTramiteNovedad(pNovedadContractualId, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpGet]
        [Route("EnviarNotificaciones")]
        public async Task<bool> EnviarNotificaciones(int idPContrato)
        {
            Contrato contrato = new Contrato
            {
                ContratoId = idPContrato
            };
            return await _registerContractsService.EnviarNotificaciones(contrato, _settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
        }

    }
}
