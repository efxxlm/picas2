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
    public interface IFichaContratoService
    {
        Task<dynamic> GetInfoProcesosDefensaByContratoId(int pContratoId);
        Task<dynamic> GetInfoControversiasByContratoId(int pContratoId);
        Task<dynamic> GetInfoNovedadesByContratoId(int pContratoId);
        Task<dynamic> GetInfoPolizasSegurosByContratoId(int pContratoId);
        Task<dynamic> GetInfoContratacionByContratoId(int pContratoId);
        Task<dynamic> GetInfoProcesosSeleccionByContratoId(int pContratoId);
        Task<dynamic> GetInfoResumenByContratoId(int pContratoId);
        Task<dynamic> GetFlujoContratoByContratoId(int pContratoId);
        Task<dynamic> GetContratosByNumeroContrato(string pNumeroContrato);
    }
}
