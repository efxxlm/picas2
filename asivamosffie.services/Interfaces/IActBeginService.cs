using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IActBeginService
    {
        //Task<ActionResult<List<GrillaActaInicio>>> GetListGrillaActaInicio();

        Task<List<GrillaActaInicio>> GetListGrillaActaInicio();

        Task<VistaGenerarActaInicioContrato> GetListVistaGenerarActaInicio(int pContratoId);

        Task<Respuesta> GuardarTieneObservacionesActaInicio(int pContratoId, string pObervacionesActa, string pUsuarioModificacion);

        
        Task<byte[]> GetPlantillaActaInicio(int pContratoId);
        //        ---guardar
        //¿Tiene observaciones al acta de inicio? Sí No  ?????
        //ConObervacionesActa  - Contrato

        Task<Respuesta> GuardarCargarActaSuscritaContrato(int pContratoId, DateTime pFechaFirmaContratista, DateTime pFechaFirmaActaContratistaInterventoria
            /* archivo pdf */ , IFormFile pFile, string pDirectorioBase, string pDirectorioActaInicio, string pUsuarioModificacion
            );

        Task<Respuesta> EditarContratoObservacion(int pContratoId, int pPlazoFase2PreMeses, int pPlazoFase2PreDias, string pObservacion, string pUsuarioModificacion, DateTime pFechaActaInicioFase1, DateTime pFechaTerminacionFase2);

        //  FechaFirmaContratista - contrato
        //FechaFirmaActaContratistaInterventoria  - contrato
        //----cargar archivo pdf VALIDAR FORMATO PDF?? NATALIA   - julian cargar archivo ???

        Task<Respuesta> GuardarPlazoEjecucionFase2Construccion(int pContratoId, int pPlazoFase2PreMeses, int pPlazoFase2PreDias, string pObservacionesConsideracionesEspeciales, string pUsuarioModificacion, DateTime pFechaActaInicioFase1 , DateTime pFechaTerminacionFase2);
        //:  Meses: xx Días: xx   - PlazoFase2PreMeses  - PlazoFase2PreDias - contrato
        //ObservacionesConsideracionesEspeciales Observaciones  - contrato

        /// <summary>
        /// ///////////
        /// </summary>
        /// <returns></returns>
        //Task<ActionResult<List<CuentaBancaria>>> GetBankAccount();

        //Task<CuentaBancaria> GetBankAccountById(int id);

        //Task<Respuesta> CreateEditarCuentasBancarias(CuentaBancaria cuentaBancaria);

        //Task<Respuesta> Insert(CuentaBancaria cuentaBancaria);

        //Task<Respuesta> Update(CuentaBancaria cuentaBancaria);

        //Task<bool> Delete(int id);
    }

}
