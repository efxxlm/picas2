using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace asivamosffie.services
{
    public class GuaranteePolicyService: IGuaranteePolicyService 
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        public GuaranteePolicyService(devAsiVamosFFIEContext context, ICommonService commonService)
        {

            _commonService = commonService;
            _context = context;
        }
        public async Task<ActionResult<List<PolizaGarantia>>> GetListPolizaGarantiaByContratoPolizaId(int pContratoPolizaId)
        
        {            
            return await _context.PolizaGarantia.Where(r => r.ContratoPolizaId == pContratoPolizaId).ToListAsync();
        }

     
        public async Task<ContratoPoliza> GetContratoPolizaByIdContratoPolizaId(int pContratoPolizaId)
        {

            //includefilter
            ContratoPoliza contratoPoliza = new ContratoPoliza();
            //contratoPoliza = _context.ContratoPoliza.Where(r => !(bool)r.Eliminado && r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();
            contratoPoliza = _context.ContratoPoliza.Where(r =>  r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();

            if (contratoPoliza != null)
            {
                //List<PolizaGarantia> contratoPolizaGarantia = await _context.PolizaGarantia.Where(r => r.ContratoPolizaId == p && !(bool)r.Eliminado).IncludeFilter(r => r.CofinanciacionDocumento.Where(r => !(bool)r.Eliminado)).ToListAsync();
                List<PolizaGarantia> contratoPolizaGarantia = await _context.PolizaGarantia.Where(r => r.ContratoPolizaId == pContratoPolizaId ).ToListAsync();

                contratoPoliza.PolizaGarantia = contratoPolizaGarantia;
            }

            PolizaObservacion polizaObservacion = new PolizaObservacion();
            //contratoPoliza = _context.ContratoPoliza.Where(r => !(bool)r.Eliminado && r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();
            polizaObservacion = _context.PolizaObservacion.Where(r => r.ContratoPolizaId == pContratoPolizaId).FirstOrDefault();
                 
            if (contratoPoliza != null)
            {
                //List<PolizaGarantia> contratoPolizaGarantia = await _context.PolizaGarantia.Where(r => r.ContratoPolizaId == p && !(bool)r.Eliminado).IncludeFilter(r => r.CofinanciacionDocumento.Where(r => !(bool)r.Eliminado)).ToListAsync();
                List<PolizaObservacion> contratoPolizaObservacion = await _context.PolizaObservacion.Where(r => r.ContratoPolizaId == pContratoPolizaId).ToListAsync();

                contratoPoliza.PolizaObservacion = contratoPolizaObservacion;
            }

            return contratoPoliza;
        }


        public async Task<ActionResult<List<PolizaObservacion>>> GetListPolizaObservacionByContratoPolizaId(int pContratoPolizaId)        
        {            

            return await _context.PolizaObservacion.Where(r => r.ContratoPolizaId == pContratoPolizaId).ToListAsync();
        }


        public async Task<Respuesta> InsertPolizaGarantia(PolizaGarantia polizaGarantia)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Cuenta_Bancaria, (int)EnumeratorTipoDominio.Acciones);

            //GUARDAR
            //PolizaObservacion - FechaRevision
            //    EstadoRevisionCodigo - PolizaObservacion

            try
            {
                if (polizaGarantia != null)
                {
                    //contratoPoliza.FechaCreacion = DateTime.Now;
                    //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;
                                        

                    //_context.Add(contratoPoliza);

                    //contratoPoliza.ObservacionesRevisionGeneral = ValidarRegistroCompleto(cofinanciacion);

                    _context.PolizaGarantia.Add(polizaGarantia);
                    await _context.SaveChangesAsync();

                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContratoPoliza.CreadoCorrrectamente,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias,
                            ConstantMessagesContratoPoliza.CreadoCorrrectamente,
                            //contratoPoliza
                            1
                            ,
                            "UsuarioCreacion", "REGISTRAR POLIZA OBSERVACION"
                            //contratoPoliza.UsuarioCreacion, "REGISTRAR POLIZA GARANTIA"
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
        public async Task<Respuesta> InsertPolizaObservacion(PolizaObservacion polizaObservacion)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Cuenta_Bancaria, (int)EnumeratorTipoDominio.Acciones);

            //GUARDAR
            //PolizaObservacion - FechaRevision
            //    EstadoRevisionCodigo - PolizaObservacion

            try
            {
                if (polizaObservacion != null)
                {
                    //contratoPoliza.FechaCreacion = DateTime.Now;
                    //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;

                    //_context.Add(contratoPoliza);

                    //contratoPoliza.ObservacionesRevisionGeneral = ValidarRegistroCompleto(cofinanciacion);

                    _context.PolizaObservacion.Add(polizaObservacion);
                    await _context.SaveChangesAsync();

                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContratoPoliza.CreadoCorrrectamente,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias,
                            ConstantMessagesContratoPoliza.CreadoCorrrectamente,
                            //contratoPoliza
                            1
                            ,
                            "UsuarioCreacion", "REGISTRAR POLIZA OBSERVACION"
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

        public async Task<Respuesta> InsertContratoPoliza(ContratoPoliza contratoPoliza)        
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Cuenta_Bancaria, (int)EnumeratorTipoDominio.Acciones);

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
                if (contratoPoliza != null)
                {
                    //contratoPoliza.FechaCreacion = DateTime.Now;
                    //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;

                    //_context.Add(contratoPoliza);

                    //contratoPoliza.ObservacionesRevisionGeneral = ValidarRegistroCompleto(cofinanciacion);

                    _context.ContratoPoliza.Add(contratoPoliza);
                    await _context.SaveChangesAsync();


                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContratoPoliza.CreadoCorrrectamente,
                            Message = 
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.GestionarGarantias,
                            ConstantMessagesContratoPoliza.CreadoCorrrectamente, 
                            //contratoPoliza
                            1
                            ,
                            "UsuarioCreacion", "REGISTRAR CONTRATO POLIZA"
                            //contratoPoliza.UsuarioCreacion, "REGISTRAR CONTRATO POLIZA"
                            )
                        };

                    //return _response = new Respuesta { IsSuccessful = true,
                    //    IsValidation = false, Data = cuentaBancaria,
                    //    Code = ConstantMessagesBankAccount.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContributor.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContributor.ErrorInterno };
            }

        }

        public static bool ValidarRegistroCompletoContratoPoliza(ContratoPoliza contratoPoliza)
        {

            if (string.IsNullOrEmpty(contratoPoliza.NombreAseguradora.ToString()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(contratoPoliza.NumeroPoliza.ToString()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(contratoPoliza.NumeroCertificado.ToString()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(contratoPoliza.FechaExpedicion.ToString()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(contratoPoliza.Vigencia.ToString()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(contratoPoliza.VigenciaAmparo.ToString()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(contratoPoliza.ValorAmparo.ToString()))
            {
                return false;
            }     
            else
            {
                return true;
            }

        }

        public async Task<List<GrillaContratoGarantiaPoliza>> ListGrillaContratoGarantiaPoliza()
        {
            List<GrillaContratoGarantiaPoliza> ListContratoGrilla = new List<GrillaContratoGarantiaPoliza>();
            //Fecha de firma del contrato ??? FechaFirmaContrato , [Contrato] -(dd / mm / aaaa)


            //Tipo de solicitud ??? ContratoPoliza - TipoSolicitudCodigo          
                            

            List <Contrato> ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado).Include(r => r.FechaFirmaContrato).Include(r => r.NumeroContrato).Include(r => r.Estado).Distinct().ToListAsync();


            foreach (var contrato in ListContratos)
            {
                try
                {
                    ContratoPoliza contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);

                    //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio TipoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_Solicitud_Contrato_Poliza);
                    GrillaContratoGarantiaPoliza proyectoGrilla = new GrillaContratoGarantiaPoliza
                    {
                        ContratoId= contrato.ContratoId,
                        FechaFirma = contrato.FechaFirmaContrato.ToString("dd/mm/yyyy"),
                    NumeroContrato = contrato.NumeroContrato,
        //TipoSolicitud= contratoPoliza.TipoSolicitudCodigo
        //EstadoRegistro { get; set; }
                
                        //Departamento = departamento.Descripcion,
                        //Municipio = municipio.Descripcion,
                        //InstitucionEducativa = _context.InstitucionEducativaSede.Find(contrato.InstitucionEducativaId).Nombre,
                        //Sede = _context.InstitucionEducativaSede.Find(contrato.SedeId).Nombre,
                        TipoSolicitud = TipoSolicitudCodigoContratoPoliza.Nombre,
                        //Fecha = contrato.FechaCreacion != null ? Convert.ToDateTime(contrato.FechaCreacion).ToString("yyyy-MM-dd") : proyecto.FechaCreacion.ToString(),
                        //EstadoRegistro = "COMPLETO"
                    };

                    //if (!(bool)proyecto.RegistroCompleto)
                    //{
                    //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                    //}
                    ListContratoGrilla.Add(proyectoGrilla);
                }
                catch (Exception e)
                {
                    GrillaContratoGarantiaPoliza proyectoGrilla = new GrillaContratoGarantiaPoliza
                    {

                        ContratoId = contrato.ContratoId,
                        FechaFirma = e.ToString(),
                        NumeroContrato = e.InnerException.ToString(),
                        //TipoSolicitud= contratoPoliza.TipoSolicitudCodigo
                        //EstadoRegistro { get; set; }

                        //Departamento = departamento.Descripcion,
                        //Municipio = municipio.Descripcion,
                        //InstitucionEducativa = _context.InstitucionEducativaSede.Find(contrato.InstitucionEducativaId).Nombre,
                        //Sede = _context.InstitucionEducativaSede.Find(contrato.SedeId).Nombre,
                        TipoSolicitud = "ERROR"                        
                      
                    };
                    ListContratoGrilla.Add(proyectoGrilla);
                }
            }
            return ListContratoGrilla.OrderByDescending(r => r.TipoSolicitud).ToList();

        }


        public async Task<List<VistaContratoGarantiaPoliza>> ListVistaContratoGarantiaPoliza()
        {
            List<VistaContratoGarantiaPoliza> ListContratoGrilla = new List<VistaContratoGarantiaPoliza>();
            //Fecha de firma del contrato ??? FechaFirmaContrato , [Contrato] -(dd / mm / aaaa)


            //Tipo de solicitud ??? ContratoPoliza - TipoSolicitudCodigo          


            List<Contrato> ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado).Include(r => r.FechaFirmaContrato).Include(r => r.NumeroContrato).Include(r => r.Estado).Distinct().ToListAsync();


            foreach (var contrato in ListContratos)
            {
                try
                {
                    ContratoPoliza contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    Contratacion contratacion = await _commonService.GetContratacionByContratacionId(contrato.ContratoId);

                    Contratista contratista = await _commonService.GetContratistaByContratistaId((Int32)contratacion.ContratistaId);

                    //TipoContrato = contrato.TipoContratoCodigo   ??? Obra  ????

                    //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    Int32 plazoDias, plazoMeses;
                    //25meses / 04 días

                    if (!string.IsNullOrEmpty(contrato.PlazoFase2ConstruccionDias.ToString()))

                        plazoDias =(Int32) contrato.PlazoFase1PreDias;

                    else
                        plazoDias = (Int32)contrato.PlazoFase2ConstruccionDias;

                    if (!string.IsNullOrEmpty(contrato.PlazoFase2ConstruccionMeses.ToString()))

                        plazoMeses = (Int32)contrato.PlazoFase1PreMeses;

                    else
                        plazoMeses = (Int32)contrato.PlazoFase2ConstruccionMeses;

                    string PlazoContratoFormat = plazoMeses.ToString("00") + " meses / " + plazoDias.ToString("00") + " dias ";

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio TipoContratoCodigoContrato = await _commonService.GetDominioByNombreDominioAndTipoDominio(contrato.TipoContratoCodigo, (int)EnumeratorTipoDominio.Tipo_Contrato);


                    Dominio TipoModificacionCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoModificacionCodigo, (int)EnumeratorTipoDominio.Tipo_Modificacion_Contrato_Poliza);
                    VistaContratoGarantiaPoliza proyectoGrilla = new VistaContratoGarantiaPoliza
                    {
                        TipoContrato = TipoContratoCodigoContrato.Nombre,
                        NumeroContrato = contrato.NumeroContrato,
                        ObjetoContrato = contrato.Objeto,
                        NombreContratista = contratista.Nombre,

                        //Nit  
                        NumeroIdentificacion = contratista.NumeroIdentificacion.ToString(),

        

         ValorContrato = contrato.Valor.ToString(),

                        PlazoContrato = PlazoContratoFormat,

         //EstadoRegistro 

                        //public bool? RegistroCompleto { get; set; } 

                        

         DescripcionModificacion ="resumen", // resumen   TEMPORAL REV

         TipoModificacion = TipoModificacionCodigoContratoPoliza.Nombre

                        //TipoSolicitud= contratoPoliza.TipoSolicitudCodigo
                        //EstadoRegistro { get; set; }

                        //Departamento = departamento.Descripcion,
                        //Municipio = municipio.Descripcion,
                        //InstitucionEducativa = _context.InstitucionEducativaSede.Find(contrato.InstitucionEducativaId).Nombre,
                        //Sede = _context.InstitucionEducativaSede.Find(contrato.SedeId).Nombre,
                        
                        //Fecha = contrato.FechaCreacion != null ? Convert.ToDateTime(contrato.FechaCreacion).ToString("yyyy-MM-dd") : proyecto.FechaCreacion.ToString(),
                        //EstadoRegistro = "COMPLETO"
                    };

                    //if (!(bool)proyecto.RegistroCompleto)
                    //{
                    //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                    //}
                    ListContratoGrilla.Add(proyectoGrilla);
                }
                catch (Exception e)
                {
                    VistaContratoGarantiaPoliza proyectoGrilla = new VistaContratoGarantiaPoliza
                    {

                        TipoContrato = e.ToString(),
                        NumeroContrato = e.InnerException.ToString(),
                        ObjetoContrato = "ERROR",
                        NombreContratista = "ERROR",                       
                        
                        //Nit  
                        NumeroIdentificacion = "ERROR",
                        ValorContrato = "ERROR",

                        PlazoContrato = "ERROR",

         //EstadoRegistro 

                        //public bool? RegistroCompleto { get; set; } 

                        //TipoSolicitud = contratoPoliza.EstadoPolizaCodigo

                    DescripcionModificacion = "ERROR",

                    TipoModificacion = "ERROR"
                       
                    };
                    ListContratoGrilla.Add(proyectoGrilla);
                }
            }
            return ListContratoGrilla.OrderByDescending(r => r.TipoSolicitud).ToList();

        }

    }
}
