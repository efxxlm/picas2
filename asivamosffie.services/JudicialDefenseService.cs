using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using asivamosffie.services.Helpers.Enumerator;

namespace asivamosffie.services
{
    public class JudicialDefenseService /*: IGuaranteePolicyService*/
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        public JudicialDefenseService(devAsiVamosFFIEContext context, ICommonService commonService)
        {

            _commonService = commonService;
            _context = context;
            //_settings = settings;
        }
        public async Task<List<GrillaProcesoDefensaJudicial>> ListGrillaProcesosDefensaJudicial()
        {
            //await AprobarContratoByIdContrato(1);

            List<GrillaProcesoDefensaJudicial> ListDefensaJudicialGrilla = new List<GrillaProcesoDefensaJudicial>();
            //Fecha de firma del contrato ??? FechaFirmaContrato , [Contrato] -(dd / mm / aaaa)

            //Tipo de solicitud ??? ContratoPoliza - TipoSolicitudCodigo        

            //List <Contrato> ListContratos = await _context.Contrato.Where(r => !(bool)r.Estado).Include(r => r.FechaFirmaContrato).Include(r => r.NumeroContrato).Include(r => r.Estado).Distinct().ToListAsync();
            List<DefensaJudicial> ListDefensaJudicial  = await _context.DefensaJudicial.Where(r => (bool)r.Eliminado==false).Distinct().ToListAsync();

            foreach (var defensaJudicial in ListDefensaJudicial)
            {
                try
                {     
                    //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    string strTipoSolicitudCodigoContratoPoliza = "sin definir";
                    string strEstadoSolicitudCodigoContratoPoliza = "sin definir";

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio TipoSolicitudCodigoContratoPoliza;
                    Dominio EstadoSolicitudCodigoContratoPoliza;

                    //if (contratoPoliza != null)
                    //{
                        //TipoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_Modificacion_Contrato_Poliza);
                        //TipoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo.Trim(), (int)EnumeratorTipoDominio.Tipo_de_Solicitud);
                        //if (TipoSolicitudCodigoContratoPoliza != null)
                        //    strTipoSolicitudCodigoContratoPoliza = TipoSolicitudCodigoContratoPoliza.Nombre;

                        //EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.EstadoPolizaCodigo.Trim(), (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                        //if (EstadoSolicitudCodigoContratoPoliza != null)
                        //    strEstadoSolicitudCodigoContratoPoliza = EstadoSolicitudCodigoContratoPoliza.Nombre;

                    //}
                    bool bRegistroCompleto = false;
                    string strRegistroCompleto = "Incompleto";

                    //if (defensaJudicial.EsCompleto != null)
                    //{
                        strRegistroCompleto = (bool)defensaJudicial.EsCompleto ? "Completo" : "Incompleto";
                    //}

                    //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                    GrillaProcesoDefensaJudicial defensaJudicialGrilla = new GrillaProcesoDefensaJudicial
                    {
                        DefensaJudicialId = defensaJudicial.DefensaJudicialId,
                        FechaRegistro = defensaJudicial.FechaCreacion.ToString("dd/MM/yyyy"),
                        LegitimacionPasivaActiva= (bool)defensaJudicial.EsLegitimacionActiva ? "Activa" : "Pasiva",
                        NumeroProceso="DJ"+ defensaJudicial.DefensaJudicialId.ToString() + defensaJudicial.FechaCreacion.ToString("yyyy"),
                        TipoAccionCodigo= defensaJudicial.TipoAccionCodigo,
                        TipoAccion = "PENDIENTE",
                        EstadoProceso= "PENDIENTE",
                        EstadoProcesoCodigo =defensaJudicial.EstadoProcesoCodigo,
                        
                        RegistroCompletoNombre =strRegistroCompleto,
                        TipoProceso="PENDIENTE",
                        TipoProcesoCodigo = defensaJudicial.TipoProcesoCodigo,                       
                                          
                    };

                    //if (!(bool)proyecto.RegistroCompleto)
                    //{
                    //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                    //}
                    ListDefensaJudicialGrilla.Add(defensaJudicialGrilla);
                }
                catch (Exception e)
                {
                    GrillaProcesoDefensaJudicial defensaJudicialGrilla = new GrillaProcesoDefensaJudicial
                    {
                        DefensaJudicialId = defensaJudicial.DefensaJudicialId,
                        FechaRegistro = e.ToString(),
                        LegitimacionPasivaActiva = e.InnerException.ToString(),
                        NumeroProceso = "ERROR",
                        TipoAccionCodigo = "ERROR",
                        TipoAccion = "ERROR",
                        EstadoProceso = "ERROR",
                        EstadoProcesoCodigo = "ERROR",

                        RegistroCompletoNombre = "ERROR",
                        TipoProceso = "ERROR",
                        TipoProcesoCodigo = "ERROR",                  
    
                    };
                    ListDefensaJudicialGrilla.Add(defensaJudicialGrilla);
                }
            }
            return ListDefensaJudicialGrilla.ToList();

        }
    }
}
