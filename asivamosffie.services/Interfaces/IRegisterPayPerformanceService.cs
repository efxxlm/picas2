using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.services.Interfaces
{
    public interface IRegisterPayPerformanceService
    {
        Task<Respuesta> UploadFileToValidate(IFormFile pFile, string typeFile, bool saveSuccessProcess);

        Task<List<dynamic>> getPaymentsPerformances(string typeFile, string status);

        Task<Respuesta> SetObservationPayments(string observaciones, int uploadedOrderId);

        Task<Respuesta> DeletePayment(int uploadedOrderId, string author);

        Task<Respuesta> DownloadPaymentPerformanceAsync(FileRequest fileRequest, string fileType);

        Task<Respuesta> ManagePerformanceAsync(int uploadOrderId);

        Task<Respuesta> NotifyEmailPerformanceInconsistencies(int uploadedOrderId, string author);

        Task<Respuesta> NotifyRequestManagedPerformancesApproval(int uploadedOrderId);

        Task<Respuesta> GetManagedPerformancesByStatus(string author, int uploadedOrderId, bool? queryConsistentOrders = null);

        Task<IEnumerable<dynamic>> GetRequestedApprovalPerformances();

        Task<Respuesta> IncludePerformances(int uploadedOrderId);

        Task<Respuesta> DownloadApprovedIncorporatedPerfomances(int uploadedOrderId);
        Task<byte[]> GenerateMinute(int uploadOrderId);
        Task<Respuesta> UploadPerformanceUrlMinute(FileRequest pFile);

        Task<Respuesta> GetMinuteFromUrl(int uploadOrderId);
    }
}
