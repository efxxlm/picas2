using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IFichaProyectoService
    {
        Task<dynamic> GetInfoSeguimientoTecnicoByProyectoId(int pProyectoId);
        Task<dynamic> GetInfoContratoByProyectoId(int pProyectoId);
        Task<dynamic> GetInfoResumenByProyectoId(int pProyectoId);
        Task<dynamic> GetInfoPreparacionByProyectoId(int pProyectoId);
        Task<dynamic> GetFlujoProyectoByProyectoId(int pProyectoId);
        Task<dynamic> GetVigencias();
        Task<dynamic> GetTablaProyectosByProyectoIdTipoContratacionVigencia(int pProyectoId,string pTipoIntervencion, int pVigencia);
        Task<dynamic> GetProyectoIdByLlaveMen(string pLlaveMen);

    }
}
