using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IGuaranteePolicyService
    {
        //IBankAccountService from        

        Task<Respuesta> InsertContratoPoliza(ContratoPoliza contratoPoliza);

        //Task<Respuesta> EditarContratoPoliza(ContratoPoliza contratoPoliza);                

        Task<List<VistaContratoGarantiaPoliza>> ListVistaContratoGarantiaPoliza();

        Task<List<GrillaContratoGarantiaPoliza>> ListGrillaContratoGarantiaPoliza();

        Task<List<PolizaGarantia>> GetListPolizaGarantiaByContratoPolizaId(int pContratoPolizaId);

        Task<List<PolizaObservacion>> GetListPolizaObservacionByContratoPolizaId(int pContratoPolizaId);

        Task<ContratoPoliza> GetContratoPolizaByIdContratoPolizaId(int pContratoPolizaId);

        Task<Respuesta> AprobarContrato(int pIdContrato);


        //getListPolizaGarantiaByContratoPolizaId    yaaaa y observ yaaaa

        //getListChequeo
        ////ContratoPoliza

        //guardarRevisionPolizaObservacion(polizaObservacionId, contratoPolizaId)
        //            PolizaObservacionId int
        //ContratoPolizaId    int
        //Observacion varchar
        //FechaRevision   datetime
        //EstadoRevisionCodigo    varchar

        Task<Respuesta> InsertPolizaObservacion(PolizaObservacion polizaObservacion);
        Task<Respuesta> InsertPolizaGarantia(PolizaGarantia polizaGarantia);
        

        //    cambiarEstadoContrato()
        //    cambiarEstadoContratoPoliza()

    }
}
