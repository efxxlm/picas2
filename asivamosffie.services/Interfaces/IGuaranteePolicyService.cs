using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
//using asivamosffie.api.Controllers.

namespace asivamosffie.services.Interfaces
{
    public interface IGuaranteePolicyService
    {
        //IBankAccountService from        

        Task<Respuesta> InsertContratoPoliza(ContratoPoliza contratoPoliza, AppSettingsService appSettingsService);

        Task<Respuesta> EditarContratoPoliza(ContratoPoliza contratoPoliza);                

        Task<List<VistaContratoGarantiaPoliza>> ListVistaContratoGarantiaPoliza();

        Task<List<GrillaContratoGarantiaPoliza>> ListGrillaContratoGarantiaPoliza();

        Task<List<PolizaGarantia>> GetListPolizaGarantiaByContratoPolizaId(int pContratoPolizaId);

        Task<List<PolizaObservacion>> GetListPolizaObservacionByContratoPolizaId(int pContratoPolizaId);

        Task<ContratoPoliza> GetContratoPolizaByIdContratoPolizaId(int pContratoPolizaId);

        Task<Respuesta> AprobarContratoByIdContrato(int pIdContrato, AppSettingsService settings);
        


        //getListPolizaGarantiaByContratoPolizaId    yaaaa y observ yaaaa

        //getListChequeo
        ////ContratoPoliza

        //guardarRevisionPolizaObservacion(polizaObservacionId, contratoPolizaId)
        //            PolizaObservacionId int
        //ContratoPolizaId    int
        //Observacion varchar
        //FechaRevision   datetime
        //EstadoRevisionCodigo    varchar

        Task<Respuesta> InsertEditPolizaObservacion(PolizaObservacion polizaObservacion);
        Task<Respuesta> InsertEditPolizaGarantia(PolizaGarantia polizaGarantia);
        

        //    cambiarEstadoContrato()
        //    cambiarEstadoContratoPoliza()

    }
}
