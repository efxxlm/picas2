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
        Task<ActionResult<List<Respuesta>>> GetControlGrid(int ContributorId);
        Task<Respuesta> Insert(CofinanciacionAportante CofnaAportante);
        Task<bool> Update(Respuesta aportante);

        Task<bool> Delete(int id);

    }
}
