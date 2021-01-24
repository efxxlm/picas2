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

        Task<List<dynamic>> getPaymentsPerformances(string typeFile, string status);

        void setObservationPaymentsPerformances(string typeFile, string observaciones, string cargaPagosRendimientosId);

        Task<Respuesta> DeletePaymentPerformance(int uploadedOrderId);

        Respuesta DownloadPaymentPerformanceAsync(int uploadOrderId);


        Task<Respuesta> ManagePerformanceAsync(int uploadOrderId);

        Task<Respuesta> ChangeStatusShowInconsistencies(int uploadOrderId);
    }
}
