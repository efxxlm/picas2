using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using asivamosffie.services.Interfaces;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Helpers.Constant;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.services
{
   public  class ContractualControversyService  :IContractualControversy
    {

        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        public ContractualControversyService(devAsiVamosFFIEContext context, ICommonService commonService)
        {

            _commonService = commonService;
            _context = context;
            //_settings = settings;
        }

        //CreateEditNuevaActualizacionTramite(ControversiaActuacion
        //“Registrar nueva actualización del trámite”

             public async Task<Respuesta> CreateEditNuevaActualizacionTramite(ControversiaActuacion controversiaActuacion)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearcontroversiaActuacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContratoPoliza.CreadoCorrrectamente, (int)EnumeratorTipoDominio.Acciones);

            //try
            //{
            //    Respuesta respuesta = new Respuesta();
            //    string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
            //    respuesta = await _Cofinancing.EliminarCofinanciacionByCofinanciacionId(pCofinancicacionId, pUsuarioModifico);

            //    return Ok(respuesta);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.ToString());
            //}

            try
            {
                if (controversiaActuacion != null)
                {

                    controversiaActuacion.Observaciones = Helpers.Helpers.CleanStringInput(controversiaActuacion.Observaciones);

                    controversiaActuacion.FechaCreacion = DateTime.Now;
                    //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;
                    //controversiaActuacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;

                    //_context.Add(contratoPoliza);

                    controversiaActuacion.EsCompleto = ValidarRegistroCompletoControversiaActuacion(controversiaActuacion);
                    //contratoPoliza.ObservacionesRevisionGeneral = ValidarRegistroCompleto(cofinanciacion);

                    //LimpiarEntradasContratoPoliza(ref contratoPoliza);

                    //_context.ContratoPoliza.Add(contratoPoliza);
                    _context.ControversiaActuacion.Update(controversiaActuacion);
                     _context.SaveChanges();

                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias,
                            ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente,
                            //contratoPoliza
                            idAccionCrearcontroversiaActuacion
                            , controversiaActuacion.UsuarioModificacion
                            //"UsuarioCreacion"
                            , "EDITAR CONTROVERSIA ACTUACION"
                            //contratoPoliza.UsuarioCreacion, "REGISTRAR CONTRATO POLIZA"
                            )
                        };

                    //return _response = new Respuesta { IsSuccessful = true,
                    //    IsValidation = false, Data = cuentaBancaria,
                    //    Code = ConstantMessagesBankAccount.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.ErrorInterno };
            }

        }

        private bool ValidarRegistroCompletoControversiaActuacion(ControversiaActuacion controversiaActuacion)
        {
            if (string.IsNullOrEmpty(controversiaActuacion.EstadoAvanceTramiteCodigo)
             ||  string.IsNullOrEmpty(controversiaActuacion.ActuacionAdelantadaCodigo)
            || string.IsNullOrEmpty(controversiaActuacion.CantDiasVencimiento.ToString())
                || (controversiaActuacion.EsRequiereContratista==null)
                ||  (controversiaActuacion.EsRequiereInterventor == null)
               || (controversiaActuacion.EsRequiereJuridico == null)
                || (controversiaActuacion.EsRequiereSupervisor == null))           
            {
                return false;
            }

            return true;
        }

        public async Task<Respuesta> CreateEditarControversiaTAI(ControversiaContractual controversiaContractual )
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContratoPoliza.CreadoCorrrectamente, (int)EnumeratorTipoDominio.Acciones);

            //try
            //{
            //    Respuesta respuesta = new Respuesta();
            //    string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
            //    respuesta = await _Cofinancing.EliminarCofinanciacionByCofinanciacionId(pCofinancicacionId, pUsuarioModifico);

            //    return Ok(respuesta);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.ToString());
            //}

            try
            {
                if (controversiaContractual != null)
                {                           

                      controversiaContractual.MotivoJustificacionRechazo = Helpers.Helpers.CleanStringInput(controversiaContractual.MotivoJustificacionRechazo);
                       controversiaContractual.ConclusionComitePreTecnico = Helpers.Helpers.CleanStringInput(controversiaContractual.ConclusionComitePreTecnico);
                    //ControversiaContractual.FechaCreacion = DateTime.Now;
                    //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;
                    //contratoPoliza.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;

                    //_context.Add(contratoPoliza);

                    //contratoPoliza.RegistroCompleo = ValidarRegistroCompletoContratoPoliza(contratoPoliza);
                    //contratoPoliza.ObservacionesRevisionGeneral = ValidarRegistroCompleto(cofinanciacion);

                    //LimpiarEntradasContratoPoliza(ref contratoPoliza);

                    //_context.ContratoPoliza.Add(contratoPoliza);
                    _context.ControversiaContractual.Update(controversiaContractual);
                    //await _context.SaveChangesAsync();


                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias,
                            ConstantMessagesContratoPoliza.EditarContratoPolizaCorrrectamente,
                            //contratoPoliza
                            idAccionCrearContratoPoliza
                            , controversiaContractual.UsuarioModificacion
                            //"UsuarioCreacion"
                            , "EDITAR CONTROVERSIA CONTRACTUAL"
                            //contratoPoliza.UsuarioCreacion, "REGISTRAR CONTRATO POLIZA"
                            )
                        };

                    //return _response = new Respuesta { IsSuccessful = true,
                    //    IsValidation = false, Data = cuentaBancaria,
                    //    Code = ConstantMessagesBankAccount.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContratoPoliza.ErrorInterno };
            }

        }


        public async Task<List<GrillaTipoSolicitudControversiaContractual>> ListGrillaTipoSolicitudControversiaContractual()
        {
            //await AprobarContratoByIdContrato(1);

            List<GrillaTipoSolicitudControversiaContractual> ListControversiaContractualGrilla = new List<GrillaTipoSolicitudControversiaContractual>();
            //Fecha de firma del contrato ??? FechaFirmaContrato , [Contrato] -(dd / mm / aaaa)

            //Tipo de solicitud ??? ContratoPoliza - TipoSolicitudCodigo      
                        
            //List<ControversiaContractual> ListControversiaContractualGrilla = await _context.ControversiaContractual.Where(r => !(bool)r.EstadoCodigo).Distinct().ToListAsync();
            List<ControversiaContractual> ListControversiaContractual = await _context.ControversiaContractual.Distinct().ToListAsync();

            foreach (var controversia in ListControversiaContractual)
            {
                try
                {
                    Contrato contrato=null;

                    //contrato = await _commonService.GetContratoPolizaByContratoId(controversia.ContratoId);
                    contrato =  _context.Contrato.Where(r=>r.ContratoId==controversia.ContratoId).FirstOrDefault();

                    //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    string strEstadoCodigoControversia = "sin definir";
                    string strTipoControversiaCodigo = "sin definir";
          
                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio EstadoCodigoControversia;
                    Dominio TipoControversiaCodigo;

                    if (contrato != null)
                    {
                        //TipoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_Modificacion_Contrato_Poliza);
                        //if (TipoSolicitudCodigoContratoPoliza != null)
                        //    strTipoSolicitudCodigoContratoPoliza = TipoSolicitudCodigoContratoPoliza.Nombre;

                        //EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                        //if (EstadoSolicitudCodigoContratoPoliza != null)
                        //    strEstadoSolicitudCodigoContratoPoliza = EstadoSolicitudCodigoContratoPoliza.Nombre;

                    }


                    //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                    GrillaTipoSolicitudControversiaContractual RegistroControversiaContractual = new GrillaTipoSolicitudControversiaContractual
                    {
                         ControversiaContractualId=controversia.ControversiaContractualId,
                         NumeroSolicitud=controversia.NumeroSolicitud,
                         //FechaSolicitud=controversia.FechaSolicitud,
                         FechaSolicitud =controversia.FechaSolicitud != null ? Convert.ToDateTime(controversia.FechaSolicitud).ToString("dd/MM/yyyy") : controversia.FechaSolicitud.ToString(),
                         TipoControversia ="PENDIENTE",
                         TipoControversiaCodigo= "PENDIENTE",
                         ContratoId = contrato.ContratoId,
                        
                    };

                    //if (!(bool)proyecto.RegistroCompleto)
                    //{
                    //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                    //}
                    ListControversiaContractualGrilla.Add(RegistroControversiaContractual);
                }
                catch (Exception e)
                {
                    GrillaTipoSolicitudControversiaContractual RegistroControversiaContractual = new GrillaTipoSolicitudControversiaContractual
                    {
                        ControversiaContractualId = controversia.ControversiaContractualId,
                        NumeroSolicitud = controversia.NumeroSolicitud+" - "+ e.InnerException.ToString(),
                        //FechaSolicitud=controversia.FechaSolicitud,
                        FechaSolicitud = controversia.FechaSolicitud != null ? Convert.ToDateTime(controversia.FechaSolicitud).ToString("dd/MM/yyyy") : controversia.FechaSolicitud.ToString(),
                        TipoControversia = e.ToString(),
                        TipoControversiaCodigo = "ERROR",
                        ContratoId = 0,                                                                      

                    };
                    ListControversiaContractualGrilla.Add(RegistroControversiaContractual);
                }
            }
            return ListControversiaContractualGrilla.OrderByDescending(r => r.ControversiaContractualId).ToList();

        }

    }
}
