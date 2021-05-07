using asivamosffie.model.APIModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IRegisterPaymentService
    {
        Task<Respuesta> UploadFileToValidate(IFormFile pFile, string typeFile, bool saveSuccessProcess);

        Task<List<dynamic>> getPaymentsPerformances(string typeFile, string status);

        //Task<Respuesta> SetObservationPayments(string observaciones, int uploadedOrderId);

        //Task<Respuesta> DeletePayment(int uploadedOrderId, string author);

        //Task<Respuesta> DownloadPaymentPerformanceAsync(FileRequest fileRequest, string fileType);
    }
}
