﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface IRegisterSessionTechnicalCommitteeService
    {
        Task<byte[]> GetPlantillaByTablaIdRegistroId(int pTablaId, int pRegistroId);

        Task<Respuesta> RegistrarParticipantesSesion(Sesion psesion);

        Task<List<dynamic>> GetListSesionComiteTemaByIdSesion(int pIdSesion);
  
        Task<List<dynamic>> GetListSolicitudesContractuales(DateTime FechaComite);

        Task<Respuesta> SaveEditSesionComiteTema(Sesion session);

        Task<List<ComiteGrilla>> GetComiteGrilla();

        Task<Sesion> GetSesionBySesionId(int pSesionId);

        Task<Respuesta> EliminarSesionComiteTema(int pSesionComiteTemaId, string pUsuarioModificacion);

        Task<Respuesta> CambiarEstadoComite(Sesion pSesion);

       
    }
}
