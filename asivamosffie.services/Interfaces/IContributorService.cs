﻿using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IContributorService
    {
        Task<List<Aportante>> GetContributor();

        Task<Aportante> GetContributorById(int id);

        Task<Respuesta> Insert(Aportante aportante);

        Task<bool> Update(Respuesta aportante);

        Task<bool> Delete(int id);

        string clearimput(string text);
    }
}
