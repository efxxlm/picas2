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
        Task<Respuesta> uploadFileToValidate(IFormFile pFile, string pUsuarioCreo, string typeFile, bool saveSuccessProcess);

        Task<List<dynamic>> getPaymentsPerformances(string typeFile);

        void setObservationPaymentsPerformances(string typeFile, string observaciones, string cargaPagosRendimientosId);

        Task<Respuesta> setStatusPaymentPerformance(string cargaPagosRendimientosId, string uploadStatus);

        Respuesta DownloadPaymentPerformanceAsync(int uploadOrderId);


        Task<Respuesta> ManagePerformanceAsync(int uploadOrderId);
    }
}
