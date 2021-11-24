using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IPruebaConceptoService
    {
        Task<object> CreateChartasFile(string path);
        Task<object> CreateChartasURL();
    }
}
