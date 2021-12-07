using asivamosffie.model.AditionalModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IGenerarGraficoService
    {
        Task<object> CreateChartasFile(string path);
        Task<object> CreateChartasFile(string path, ChartConfig config);
        Task<object> CreateChartasURL();
    }
}
