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
    public interface ICreateRolesService
    {
        Task<bool> ValidateExistNamePerfil(string pNamePerfil)
            ;

        Task<dynamic> GetMenu();

        Task<Respuesta> ActivateDeactivatePerfil(Perfil pPerfil);

        Task<dynamic> GetListPerfil();

        Task<Respuesta> CreateEditRolesPermisos(Perfil pPerfil);
    } 
}
