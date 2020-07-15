using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface ISelectionProcessQuotationService
    {
        Task<ActionResult<List<ProcesoSeleccionCotizacion>>> GetSelectionProcessQuotation();
        Task<ProcesoSeleccionCotizacion> GetSelectionProcessQuotationById(int id);
        Task<Respuesta> Insert(ProcesoSeleccionCotizacion procesoSeleccionCotizacion);
        Task<Respuesta> Update(ProcesoSeleccionCotizacion procesoSeleccionCotizacion);
        Task<bool> Delete(int id);
    }
}
