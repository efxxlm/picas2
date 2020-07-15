using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface ISelectionProcessService
    {
        Task<ActionResult<List<ProcesoSeleccion>>> GetSelectionProcess();
        Task<ProcesoSeleccion> GetSelectionProcessById(int id);
        Task<Respuesta> Insert(ProcesoSeleccion procesoSeleccion);
        Task<Respuesta> Update(ProcesoSeleccion procesoSeleccion);
        Task<bool> Delete(int id);
    }
}
