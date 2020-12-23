using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IContributorService
    {
        Task<ActionResult<List<CofinanciacionAportante>>> GetContributor();

        Task<CofinanciacionAportante> GetContributorById(int id);
        Task<ActionResult<List<CofinanciacionAportante>>> GetControlGrid(int ContributorId);
        Task<Respuesta> Insert(CofinanciacionAportante CofnaAportante);
        Task<Respuesta> Update(CofinanciacionAportante CofnaAportante);


        Task<ActionResult<List<RegistroPresupuestal>>> GetRegisterBudget();
        Task<RegistroPresupuestal> GetRegisterBudgetById(int id);
        Task<Respuesta> BudgetRecords(RegistroPresupuestal registroPresupuestal);

        Task<Respuesta> UpdateBudgetRegister(RegistroPresupuestal registroPresupuestal);

        Task<Respuesta> CreateEditBudgetRecords(RegistroPresupuestal registroPresupuestal);

    }
}
