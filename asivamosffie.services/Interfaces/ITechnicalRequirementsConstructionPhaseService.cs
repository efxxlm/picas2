using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.services.Interfaces
{
    public interface ITechnicalRequirementsConstructionPhaseService
    {
        Task<List<dynamic>> GetContractsGrid( int pUsuarioId );
        Task<Contrato> GetContratoByContratoId( int pContratoId );
        Task<Respuesta> CreateEditDiagnostico(ContratoConstruccion pConstruccion);
        Task<Respuesta> CreateEditPlanesProgramas(ContratoConstruccion pConstruccion);
        Task<Respuesta> CreateEditManejoAnticipo(ContratoConstruccion pConstruccion);
    }
}
