using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.services.Interfaces
{
    public interface IRegisterPayPerformanceService
    {
        Task<Respuesta> uploadFileToValidate(IFormFile pFile, string pUsuarioCreo, string typeFile);

        Task<List<dynamic>> getPaymentsPerformances(string typeFile);
    }
}
