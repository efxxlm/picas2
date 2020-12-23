using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.api.Models;
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
    public class DerivativeActionController : ControllerBase
    {

        public readonly IDerivativeActionService _derivativeActionService;
        private readonly IOptions<AppSettings> _settings;

        public DerivativeActionController(IDerivativeActionService seguimientoActuacionDerivada, IOptions<AppSettings> settings)
        {
            _derivativeActionService = seguimientoActuacionDerivada;

            _settings = settings;
        }

        [HttpPost]
        [Route("CreateEditarSeguimientoActuacionDerivada")]
        //public async Task<Respuesta> CreateEditarSeguimientoActuacionDerivada(SeguimientoActuacionDerivada seguimientoActuacionDerivada)
        public async Task<IActionResult> CreateEditarSeguimientoActuacionDerivada(SeguimientoActuacionDerivada seguimientoActuacionDerivada)
        {

            Respuesta respuesta = new Respuesta();
            try
            {
                
                    seguimientoActuacionDerivada.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                asivamosffie.model.APIModels.AppSettingsService _appSettingsService;

                _appSettingsService = toAppSettingsService(_settings);
                respuesta = await _derivativeActionService.CreateEditarSeguimientoActuacionDerivada(seguimientoActuacionDerivada, _appSettingsService);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }             
            
        [Route("EliminarControversiaActuacionDerivada")]
        [HttpPost]
        public async Task<IActionResult> EliminarControversiaActuacionDerivada(int pId)
        {
            try
            {
                Respuesta respuesta = new Respuesta();
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value.ToUpper();
                respuesta = await _derivativeActionService.EliminarControversiaActuacionDerivada(pId, pUsuarioModifico);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut]
        [Route("CambiarEstadoControversiaActuacionDerivada")]
        public async Task<IActionResult> CambiarEstadoControversiaActuacionDerivada( int pActuacionDerivadaId, string pCodigoEstado)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _derivativeActionService.CambiarEstadoControversiaActuacionDerivada(pActuacionDerivadaId, pCodigoEstado, HttpContext.User.FindFirst("User").Value);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("GetSeguimientoActuacionDerivadaById")]
        [HttpGet]

        public async Task<SeguimientoActuacionDerivada> GetSeguimientoActuacionDerivadaById(int id)
        {
            var respuesta = await _derivativeActionService.GetSeguimientoActuacionDerivadaById(id);
            return respuesta;
        }

        [HttpGet]
        [Route("GetListGrillaTipoActuacionDerivada")]

        public async Task<ActionResult<List<GrillaTipoActuacionDerivada>>> ListGrillaTipoActuacionDerivada()
        {
            try
            {
                return await _derivativeActionService.ListGrillaTipoActuacionDerivada();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //[HttpGet]
        //[Route("GetListGrillaTipoActuacionDerivada2")]

        //public async Task<ActionResult<List<GrillaTipoActuacionDerivada>>> ListGrillaTipoActuacionDerivada2()
        //{
        //    try
        //    {
        //        return await _derivativeActionService.ListGrillaTipoActuacionDerivada2();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public AppSettingsService toAppSettingsService(IOptions<AppSettings> appSettings)
        {
            AppSettingsService appSettingsService = new AppSettingsService();
            appSettingsService.MailPort = appSettings.Value.MailPort;
            appSettingsService.MailServer = appSettings.Value.MailServer;
            appSettingsService.Password = appSettings.Value.Password;
            appSettingsService.Sender = appSettings.Value.Sender;

            return appSettingsService;

        }

    }
}




//public async Task<List<GrillaTipoActuacionDerivada>> ListGrillaTipoActuacionDerivada()
//{
//    //await AprobarContratoByIdContrato(1);

//    List<GrillaTipoActuacionDerivada> LstActuacionDerivadaGrilla = new List<GrillaTipoActuacionDerivada>();
//    //Fecha de firma del contrato ??? FechaFirmaContrato , [Contrato] -(dd / mm / aaaa)

//    //Tipo de solicitud ??? ContratoPoliza - TipoSolicitudCodigo      

//    List<ControversiaActuacion> lstControversiaActuacion ;
//    //List<ControversiaContractual> ListControversiaContractualGrilla = await _context.ControversiaContractual.Where(r => !(bool)r.EstadoCodigo).Distinct().ToListAsync();
//    List<SeguimientoActuacionDerivada> lstActuacionDerivada;

//    //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
//    string strEstadoActuacionDerivadaCodigo = "1";
//    string strEstadoActuacionDerivada = "Sin reporte";
//    //string strEstadoAvanceTramite = "sin definir";


//    List<GrillaTipoSolicitudControversiaContractual> ListControversiaContractualGrilla = new List<GrillaTipoSolicitudControversiaContractual>();
//    //Fecha de firma del contrato ??? FechaFirmaContrato , [Contrato] -(dd / mm / aaaa)

//    //Tipo de solicitud ??? ContratoPoliza - TipoSolicitudCodigo      

//    //List<ControversiaContractual> ListControversiaContractualGrilla = await _context.ControversiaContractual.Where(r => !(bool)r.EstadoCodigo).Distinct().ToListAsync();
//    List<ControversiaContractual> ListControversiaContractual = await _context.ControversiaContractual.Distinct().ToListAsync();


//    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
//    Dominio EstadoActuacionDerivada;

//    foreach (var controversiaContractual in ListControversiaContractual)
//    {
//        try
//        {
            
//            List<ControversiaActuacion> lstControversiaActuacion = await _context.ControversiaActuacion
//      .Where(r => !(bool)r.Eliminado && r.ControversiaActuacionId == controversiaContractual.ControversiaActuacionId).ToListAsync();

//            if (lstControversiaActuacion.Count() > 0)
//            {

//                foreach (var controversiaActuacion in lstControversiaActuacion)
//                {

//                    Contrato contrato = null;

//                    //contrato = await _commonService.GetContratoPolizaByContratoId(controversia.ContratoId);
//                    contrato = _context.Contrato.Where(r => r.ContratoId == controversiaActuacion.ContratoId).FirstOrDefault();

//                    //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
//                    string strEstadoCodigoControversia = "sin definir";
//                    string strTipoControversiaCodigo = "sin definir";
//                    string strTipoControversia = "sin definir";

//                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
//                    Dominio EstadoCodigoControversia;
//                    Dominio TipoControversiaCodigo;


//                    TipoControversiaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaActuacion.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipo_de_controversia);
//                    if (TipoControversiaCodigo != null)
//                    {
//                        strTipoControversiaCodigo = TipoControversiaCodigo.Nombre;
//                        strTipoControversia = TipoControversiaCodigo.Codigo;

//                    }

          

//                    //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
//                    GrillaTipoSolicitudControversiaContractual RegistroControversiaContractual = new GrillaTipoSolicitudControversiaContractual
//                    {
//                        ControversiaContractualId = controversiaActuacion.ControversiaContractualId,
//                        NumeroSolicitud = controversiaActuacion.NumeroSolicitud,
//                        //FechaSolicitud=controversia.FechaSolicitud,
//                        FechaSolicitud = controversiaActuacion.FechaSolicitud != null ? Convert.ToDateTime(controversia.FechaSolicitud).ToString("dd/MM/yyyy") : controversia.FechaSolicitud.ToString(),
//                        TipoControversia = strTipoControversia,
//                        TipoControversiaCodigo = strTipoControversiaCodigo,
//                        ContratoId = contrato.ContratoId,
//                        EstadoControversia = "PENDIENTE",
//                        RegistroCompletoNombre = (bool)controversiaActuacion.EsCompleto ? "Completo" : "Incompleto",

//                        //cu 4.4.1
//                        //Actuacion = "Actuación " + actuacionSeguimiento.ActuacionSeguimientoId.ToString()
//                        Actuacion = contrac.ac,
//                        FechaActuacion = "PENDIENTE",
//                        EstadoActuacion = "PENDIENTE",



//                    };
//                }
//            }

//            //else
//            //{
//            //    GrillaTipoActuacionDerivada RegistroActuacionDerivada = new GrillaTipoActuacionDerivada
//            //    {
//            //        FechaActualizacion = controversiaContractual.FechaModificacion != null ? Convert.ToDateTime(controversiaContractual.FechaModificacion).ToString("dd/MM/yyyy") : controversiaContractual.FechaModificacion.ToString(),

//            //        Actuacion = "Actuación " + controversiaContractual.ControversiaActuacionId.ToString("0000"),
//            //        NumeroActuacion = "ACT_derivada" + "0000",
//            //        EstadoRegistroActuacionDerivada = (bool)controversiaContractual.EsCompleto ? "Completo" : "Incompleto",
//            //        EstadoActuacionDerivada = strEstadoActuacionDerivada,
//            //        EstadoActuacionDerivadaCodigo = strEstadoActuacionDerivadaCodigo,

//            //        ControversiaActuacionId = controversiaContractual.ControversiaActuacionId,
//            //        SeguimientoActuacionDerivadaId = 0,


//            //    };

//            //    //if (!(bool)proyecto.RegistroCompleto)
//            //    //{
//            //    //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
//            //    //}
//            //    LstActuacionDerivadaGrilla.Add(RegistroActuacionDerivada);
//            //}

//        }
//        catch (Exception e)
//        {
//            GrillaTipoActuacionDerivada RegistroActuacionSeguimiento = new GrillaTipoActuacionDerivada
//            {
//                FechaActualizacion = "ERROR",
//                Actuacion = "Actuación " + controversiaContractual.ControversiaActuacionId.ToString("0000"),
//                NumeroActuacion = e.InnerException.ToString(),
//                EstadoRegistroActuacionDerivada = e.ToString(),
//                EstadoActuacionDerivada = "ERROR",
//                EstadoActuacionDerivadaCodigo = "ERROR",

//                ControversiaActuacionId = 0,
//                SeguimientoActuacionDerivadaId = 0,

//            };
//            LstActuacionDerivadaGrilla.Add(RegistroActuacionSeguimiento);
//        }
//    }
//    return LstActuacionDerivadaGrilla.OrderByDescending(r => r.SeguimientoActuacionDerivadaId).ToList();

//}
