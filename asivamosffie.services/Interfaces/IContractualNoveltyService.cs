using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IContractualNoveltyService
    {
        Task<Respuesta> CreateEditNovedadContractual(NovedadContractual novedadContractual);
        Task<List<VNovedadContractual>> GetListGrillaNovedadContractualObra();
        Task<List<VNovedadContractual>> GetListGrillaNovedadContractualInterventoria();
        Task<Respuesta> EliminarNovedadContractual(int pNovedadContractualId, string pUsuario);
        Task<List<Contrato>> GetListContract(int userID);
        Task<List<VProyectosXcontrato>> GetProyectsByContract(int pContratoId);
        Task<NovedadContractual> GetNovedadContractualById(int pId);
        Task<Respuesta> AprobarSolicitud(int pNovedadContractualId, string pUsuario);
        Task<Respuesta> EnviarAlSupervisor(int pNovedadContractualId, string pUsuario);
        Task<Respuesta> CreateEditObservacion(NovedadContractual pNovedadContractual, bool? esSupervisor, bool? esTramite);
        Task<Respuesta> TramitarSolicitud(int pNovedadContractualId, string pUsuario);
    }
}
