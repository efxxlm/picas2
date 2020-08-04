using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface ISelectionProcessGroupService
    {
        Task<ActionResult<List<ProcesoSeleccionGrupo>>> GetSelectionProcessGroup();
        Task<ProcesoSeleccionGrupo> GetSelectionProcessGroupById(int id);
        Task<Respuesta> Insert(ProcesoSeleccionGrupo procesoSeleccionGrupo);
        Task<Respuesta> Update(ProcesoSeleccionGrupo procesoSeleccionGrupo);
        Task<bool> Delete(int id);
    }
}
