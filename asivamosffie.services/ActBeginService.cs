using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using asivamosffie.services.Helpers.Enumerator;

namespace asivamosffie.services
{
   public  class ActBeginService
    {


        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        private readonly IOptions<AppSettings> _settings;

        public ActBeginService(devAsiVamosFFIEContext context, ICommonService commonService, IOptions<AppSettings> settings)
        {

            _commonService = commonService;
            _context = context;
            _settings = settings;
        }

        public async Task<VistaGenerarActaInicioContrato> GetVistaGenerarActaInicio(int pContratoId )
        {
            VistaGenerarActaInicioContrato actaInicio = new VistaGenerarActaInicioContrato();
                
            //List <Contrato> ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado).Include(r => r.FechaFirmaContrato).Include(r => r.NumeroContrato).Include(r => r.Estado).Distinct().ToListAsync();
            Contrato contrato =  _context.Contrato.Where(r => !(bool)r.Estado && r.ContratoId==pContratoId).FirstOrDefault();
            //cofinanciacion = _context.Cofinanciacion.Where(r => !(bool)r.Eliminado && r.CofinanciacionId == idCofinanciacion).FirstOrDefault();

            Contratacion contratacion;
            contratacion = _context.Contratacion.Where(r => !(bool)r.Estado && r.ContratacionId == contrato.ContratacionId).FirstOrDefault();

            string strTipoContratacion = "sin definir";
            //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
            Dominio TipoContratacionCodigo;

            if (contratacion != null)
            {
                TipoContratacionCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratacion.TipoContratacionCodigo, (int)EnumeratorTipoDominio.Opcion_por_contratar);
                if (TipoContratacionCodigo != null)
                    strTipoContratacion = TipoContratacionCodigo.Nombre;

            }

 //               --Contratacion.TipoContratacionCodigo  14 DOM, tipoidentificacion
 //14 1   Obra
 //14 2   Interventoría

            Contratista contratista;
            contratista = _context.Contratista.Where(r => (bool)r.Activo && r.ContratistaId == contratacion.ContratistaId).FirstOrDefault();

            string strTipoIdentificacionCodigoContratista = "sin definir";
            Dominio TipoIdentificacionCodigoContratista;

            //30  Tipo Documento
            //        tipoIdentificacionCodigo - contratista                   

            if (contratista != null)
            {
                TipoIdentificacionCodigoContratista = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratista.TipoIdentificacionCodigo, (int)EnumeratorTipoDominio.Tipo_Documento);
                if (TipoIdentificacionCodigoContratista != null)
                    strTipoIdentificacionCodigoContratista = TipoIdentificacionCodigoContratista.Nombre;
            }

            try
            {
                    ContratoPoliza contratoPoliza;
                    contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);

                    //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    string strTipoSolicitudCodigoContratoPoliza = "sin definir";
                    string strEstadoSolicitudCodigoContratoPoliza = "sin definir";                

                //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                   Dominio TipoSolicitudCodigoContratoPoliza;
                    Dominio EstadoSolicitudCodigoContratoPoliza;           
                              
                    if (contratoPoliza != null) 
                    {
                        TipoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_Modificacion_Contrato_Poliza);
                        if (TipoSolicitudCodigoContratoPoliza != null)
                            strTipoSolicitudCodigoContratoPoliza = TipoSolicitudCodigoContratoPoliza.Nombre;

                        EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                        if (EstadoSolicitudCodigoContratoPoliza != null)
                            strEstadoSolicitudCodigoContratoPoliza = EstadoSolicitudCodigoContratoPoliza.Nombre;

                    }

                //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                VistaGenerarActaInicioContrato contratoGrilla = new VistaGenerarActaInicioContrato
                {
                    //FechaAprobacionRequisitos="[FechaAprobacionRequisitos] [contrato] FechaAprobacionRequisitos",
                    NumeroContrato = contrato.NumeroContrato,
                    VigenciaContrato = "2021 PENDIENTE",
                    FechaFirmaContrato = contrato.FechaFirmaContrato.ToString("dd/MM/yyyy"),
                    NumeroDRP1 = "DisponibilidadPresupuestal - NumeroDrp PENDIENTE",

                    FechaGeneracionDRP1 = "2021 PENDIENTE",

                    NumeroDRP2 = "DisponibilidadPresupuestal - NumeroDrp PENDIENTE",
                    FechaGeneracionDRP2 = " PENDIENTE",
                    FechaAprobacionGarantiaPoliza= contratoPoliza.FechaAprobacion.ToString("dd/MM/yyyy"),
                    Objeto=contrato.Objeto,
                    ValorInicialContrato= contrato.Valor.ToString(),
                    ValorActualContrato= " PENDIENTE",
                    ValorFase1Preconstruccion = " PENDIENTE",
                    Valorfase2ConstruccionObra = " PENDIENTE",


                 
                        
                        //RegistroCompleto = contrato.RegistroCompleto
                       
                        //,EstadoRegistro = "COMPLETO"
                    };

                    //if (!(bool)proyecto.RegistroCompleto)
                    //{
                    //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                    //}
                    
                }
                catch (Exception e)
                {
                    VistaGenerarActaInicioContrato proyectoGrilla = new VistaGenerarActaInicioContrato
                    {

                        //ContratoId = contrato.ContratoId,
                        //FechaFirma = e.ToString(),
                        //NumeroContrato = e.InnerException.ToString(),
                        
                        //TipoSolicitud = "ERROR"
                        //,
                        //RegistroCompleto = false

                    };                    
                }            
            return actaInicio;
        }
    }
}
