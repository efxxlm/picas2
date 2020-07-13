using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.services.Interfaces
{
    public interface IProjectService
    {
        Task<Respuesta> CreateOrEditAdministrativeProject(ProyectoAdministrativo pProyectoAdministrativo);

        Task<List<ProyectoGrilla>> ListAdministrativeProyectos(string pUsuarioConsulto);

        Task<Respuesta> CreateOrEditProyect(Proyecto pProyecto);

        Task<Respuesta> UploadMassiveLoadProjects(string pIdDocument, string pUsuarioModifico);

        Task<Respuesta> SetValidateCargueMasivo(IFormFile pFile, string pFilePatch, string pUsuarioCreo);
         
        Task<Proyecto> GetProyectoByProyectoId(int idProyecto);
        Task<bool> DeleteProyectoByProyectoId(int pProyectoId);            
        Task<List<ProyectoGrilla>> ListProyectos(string pUsuarioConsulto);
        Task<bool> DeleteProyectoAdministrativoByProyectoId(int pProyectoId);
        Task<bool> EnviarProyectoAdministrativoByProyectoId(int pProyectoId);
        Task<List<FuenteFinanciacion>> GetFontsByAportantId(int pAportanteId);
    }
}
