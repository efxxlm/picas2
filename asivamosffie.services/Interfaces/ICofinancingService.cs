using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;

namespace asivamosffie.services.Interfaces
{
    public interface ICofinancingService
    { 
        Task<object> CreateorUpdateCofinancing(Cofinanciacion cofinanciacion);

        Task<Respuesta> EliminarCofinanciacionByCofinanciacionId(int pCofinancicacionId, string pUsuarioModifico);
        //Task<List<Cofinanciacion>> GetListCofinancing();
        //Task<ActionResult<List<DocumentoApropiacion>>> GetDocument(int ContributorId);
        Task<List<Cofinanciacion>> GetListCofinancing();

        Task<ActionResult<List<CofinanciacionDocumento>>> GetDocument(int ContributorId);

        Task<Cofinanciacion> GetCofinanciacionByIdCofinanciacion(int idCofinanciacion);

        Task<List<CofinanciacionAportante>> GetListAportante();
        Task<ActionResult<List<CofinanciacionAportante>>> GetListTipoAportante(int pTipoAportanteID);


    }
}
