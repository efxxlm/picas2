using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IBudgetAvailabilityService
    {
        #region "Disponibilidad presupuestal";
        Task<List<DisponibilidadPresupuestal>> GetBudgetAvailability();

        Task<DisponibilidadPresupuestal> GetBudgetAvailabilityById(int id);

        Task<ActionResult<List<GrillaDisponibilidadPresupuestal>>> GetGridBudgetAvailability(int? DisponibilidadPresupuestalId);
        Task<Respuesta> CreateEditarDisponibilidadPresupuestal(DisponibilidadPresupuestal DP);


        Task<Respuesta> DeleteBudgetAvailability(int id, string UsuarioModifico);
        #endregion


        #region " Disponibilidad presupuestal proyecto";
        Task<List<DisponibilidadPresupuestalProyecto>> GetAssociatedProjects(int ProyectoId);

        Task<Respuesta> CreateEditarDPProyecto(DisponibilidadPresupuestalProyecto dpProyecto);

        #endregion
         
        Task<List<GrillaDisponibilidadPresupuestal2>> GetGrillaDisponibilidadPresupuestal2(string pConnectionStrings);
    }
}
