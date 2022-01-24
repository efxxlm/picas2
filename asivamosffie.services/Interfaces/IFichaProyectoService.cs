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
        Task<dynamic> GetInfoPreparacionByContratacionProyectoId(int pContratoProyectoId);
        Task<dynamic> GetFlujoProyectoByContratacionProyectoId(int pContratacionProyectoId);
        Task<dynamic> GetVigencias();
        Task<dynamic> GetTablaProyectosByProyectoIdTipoContratacionVigencia(int pProyectoId, string pTipoContrato, string pTipoIntervencion, int pVigencia);
        Task<dynamic> GetProyectoIdByLlaveMen(string pLlaveMen);

    }
}
