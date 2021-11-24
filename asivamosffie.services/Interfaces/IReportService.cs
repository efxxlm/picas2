using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IReportService
    {
        Task<List<IndicadorReporte>> GetReportEmbedInfo();
        Task<IndicadorReporte> GetReportEmbedInfoByIndicadorReporteId(int indicadorReporteId);
        Task<List<IndicadorReporte>> GetIndicadorReporte();
    }
}
