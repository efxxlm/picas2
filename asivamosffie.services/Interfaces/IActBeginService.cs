using asivamosffie.model.APIModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IActBeginService
    {
        Task<ActionResult<List<GrillaActaInicio>>> GetListGrillaActaInicio();

        Task<ActionResult<List<VistaGenerarActaInicioContrato>>> GetVistaGenerarActaInicio();

        Task<Respuesta> GuardarTieneObservacionesActaInicio(int contratoId, string ObervacionesActa);

        //        ---guardar
        //¿Tiene observaciones al acta de inicio? Sí No  ?????
        //ConObervacionesActa  - Contrato

        Task<Respuesta> GuardarCargarActaSuscritaContrato(int contratoId, DateTime FechaFirmaContratista, DateTime FechaFirmaActaContratistaInterventoria
            /* archivo pdf */
            );

        //  FechaFirmaContratista - contrato
        //FechaFirmaActaContratistaInterventoria  - contrato
        //----cargar archivo pdf VALIDAR FORMATO PDF?? NATALIA   - julian cargar archivo ???

        Task<Respuesta> GuardarPlazoEjecucionFase2Construccion(int contratoId, int PlazoFase2PreMeses, int PlazoFase2PreDias, string ObservacionesConsideracionesEspeciales);
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
