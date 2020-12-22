using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using asivamosffie.model.APIModels;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterWeeklyProgressController : Controller
    {

        private readonly IRegisterWeeklyProgressService _registerWeeklyProgressService;
        private readonly IOptions<AppSettings> _settings;

        public RegisterWeeklyProgressController(IRegisterWeeklyProgressService registerWeeklyProgressService, IOptions<AppSettings> settings)
        {
            _registerWeeklyProgressService = registerWeeklyProgressService;
            _settings = settings;
        }

        [Route("GetEnsayoLaboratorioMuestras")]
        [HttpGet]
        public async Task<ActionResult<GestionObraCalidadEnsayoLaboratorio>> GetEnsayoLaboratorioMuestras([FromQuery] int pGestionObraCalidadEnsayoLaboratorioId)
        {
            try
            {
                return await _registerWeeklyProgressService.GetEnsayoLaboratorioMuestras(pGestionObraCalidadEnsayoLaboratorioId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetVRegistrarAvanceSemanal")]
        [HttpGet]
        public async Task<ActionResult<List<VRegistrarAvanceSemanal>>> GetVRegistrarAvanceSemanal()
        {
            try
            {
                return await _registerWeeklyProgressService.GetVRegistrarAvanceSemanal();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetLastSeguimientoSemanalByContratacionProyectoIdOrSeguimientoSemanalId")]
        [HttpGet]
        public async Task<SeguimientoSemanal> GetLastSeguimientoSemanalByContratacionProyectoIdOrSeguimientoSemanalId([FromQuery] int pContratacionProyectoId, int pSeguimientoSemanalId)
        {
            try
            {
                return await _registerWeeklyProgressService.GetLastSeguimientoSemanalByContratacionProyectoIdOrSeguimientoSemanalId(pContratacionProyectoId, pSeguimientoSemanalId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetListSeguimientoSemanalByContratacionProyectoId")]
        [HttpGet]
        public async Task<List<dynamic>> GetListSeguimientoSemanalByContratacionProyectoId([FromQuery] int pContratacionProyectoId)
        {
            try
            {
                return await _registerWeeklyProgressService.GetListSeguimientoSemanalByContratacionProyectoId(pContratacionProyectoId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("CreateEditEnsayoLaboratorioMuestra")]
        public async Task<IActionResult> CreateEditEnsayoLaboratorioMuestra([FromBody] GestionObraCalidadEnsayoLaboratorio pGestionObraCalidadEnsayoLaboratorio)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                pGestionObraCalidadEnsayoLaboratorio.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _registerWeeklyProgressService.CreateEditEnsayoLaboratorioMuestra(pGestionObraCalidadEnsayoLaboratorio);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("SaveUpdateSeguimientoSemanal")]
        public async Task<IActionResult> SaveUpdateSeguimientoSemanal([FromBody] SeguimientoSemanal pSeguimientoSemanal)
        {
            pSeguimientoSemanal.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
            return Ok(await _registerWeeklyProgressService.SaveUpdateSeguimientoSemanal(pSeguimientoSemanal));
        }

        [HttpPost]
        [Route("ChangueStatusSeguimientoSemanal")]
        public async Task<IActionResult> ChangueStatusSeguimientoSemanal([FromQuery] int pContratacionProyectoId, string pEstadoMod)
        {
            return Ok(await _registerWeeklyProgressService.ChangueStatusSeguimientoSemanal(pContratacionProyectoId, pEstadoMod, HttpContext.User.FindFirst("User").Value));
        }
   
        [HttpPost]
        [Route("ChangueStatusMuestrasSeguimientoSemanal")]
        public async Task<IActionResult> ChangueStatusMuestrasSeguimientoSemanal([FromQuery] int pSeguimientoSemanalID, string pEstadoMod)
        {
            return Ok(await _registerWeeklyProgressService.ChangueStatusMuestrasSeguimientoSemanal(pSeguimientoSemanalID, pEstadoMod, HttpContext.User.FindFirst("User").Value));
        }

        [HttpPost]
        [Route("DeleteManejoMaterialesInsumosProveedor")]
        public async Task<IActionResult> DeleteManejoMaterialesInsumosProveedor([FromQuery] int ManejoMaterialesInsumosProveedorId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _registerWeeklyProgressService.DeleteManejoMaterialesInsumosProveedor(ManejoMaterialesInsumosProveedorId, HttpContext.User.FindFirst("User").Value);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("DeleteResiduosConstruccionDemolicionGestor")]
        public async Task<IActionResult> DeleteResiduosConstruccionDemolicionGestor([FromQuery] int ResiduosConstruccionDemolicionGestorId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _registerWeeklyProgressService.DeleteResiduosConstruccionDemolicionGestor(ResiduosConstruccionDemolicionGestorId, HttpContext.User.FindFirst("User").Value);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [HttpPost]
        [Route("DeleteGestionObraCalidadEnsayoLaboratorio")]
        public async Task<IActionResult> DeleteGestionObraCalidadEnsayoLaboratorio([FromQuery] int GestionObraCalidadEnsayoLaboratorioId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _registerWeeklyProgressService.DeleteGestionObraCalidadEnsayoLaboratorio(GestionObraCalidadEnsayoLaboratorioId, HttpContext.User.FindFirst("User").Value);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

    }
}
