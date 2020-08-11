using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;

namespace asivamosffie.services.Interfaces
{
    public interface ICofinancingService
    { 
        Task<Respuesta> CreateorUpdateCofinancing(Cofinanciacion cofinanciacion);

        Task<Respuesta> EliminarCofinanciacionByCofinanciacionId(int pCofinancicacionId, string pUsuarioModifico);

        Task<List<Cofinanciacion>> GetListCofinancing();

        Task<List<CofinanciacionDocumento>> GetDocument(int ContributorId);
        //Task<ActionResult<List<CofinanciacionDocumento>>> GetDocument(int ContributorId);

        Task<Cofinanciacion> GetCofinanciacionByIdCofinanciacion(int idCofinanciacion);

        Task<List<CofinanciacionAportante>> GetListAportante();

        Task<ActionResult<List<CofinanciacionAportante>>> GetListTipoAportante(int pTipoAportanteID);

        Task<ActionResult<List<CofinanicacionAportanteGrilla>>> GetListAportanteByTipoAportanteId(int pTipoAportanteID);

        Task<ActionResult<List<CofinanciacionDocumento>>> GetListDocumentoByAportanteId(int pAportanteID);

    }
}
