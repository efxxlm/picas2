﻿//using asivamosffie.model.APIModels;
//using asivamosffie.model.Models;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace asivamosffie.services.Interfaces
//{
//    public interface IContractualControversy
//    {
//        Task<Respuesta> CreateEditarControversiaTAI(ControversiaContractual controversiaContractual);

//        Task<Respuesta> CreateEditNuevaActualizacionTramite(ControversiaActuacion controversiaActuacion);

//         Task<Respuesta> CreateEditarActuacionSeguimiento(ActuacionSeguimiento actuacionSeguimiento);

//        Task<List<GrillaTipoSolicitudControversiaContractual>> ListGrillaTipoSolicitudControversiaContractual(int pControversiaContractualId = 0);


//        Task<List<GrillaActuacionSeguimiento>> ListGrillaActuacionSeguimiento(int pControversiaContractualId = 0);

//        Task<List<GrillaControversiaActuacionEstado>> ListGrillaControversiaActuacion(int id = 0, int pControversiaContractualId = 0,  bool esActuacionReclamacion = false);


//         Task<VistaContratoContratista> GetVistaContratoContratista(int pContratoId);

//        Task<ControversiaContractual> GetControversiaContractualById(int pControversiaContractualId);

//        Task<ControversiaActuacion> GetControversiaActuacionById(int id);
//<<<<<<< HEAD

//        Task<List<Contrato>> GetListContratos();

//=======
//        Task<List<ControversiaMotivo>> GetMotivosSolicitudByControversiaContractualId(int id);


//        Task<byte[]> GetPlantillaControversiaContractual(int pContratoId);  

//            //Task<byte[]> GetPlantillaActaIdComite(int ComiteId);
        
//       Task<List<Contrato>> GetListContratos();
        
//        Task<Respuesta> CambiarEstadoControversiaActuacion2(int pControversiaActuacionId, string pNuevoCodigoProximaActuacion, string pUsuarioModifica);
//>>>>>>> 4.2.2-Gestionar-procesos-defensa-judicial
//        Task<Respuesta> CambiarEstadoControversiaActuacion(int pControversiaActuacionId, string pNuevoCodigoEstadoAvance, string pUsuarioModifica);

//        Task<Respuesta> CambiarEstadoControversiaContractual(int pControversiaContractualId, string pNuevoCodigoEstado, string pUsuarioModifica);

//        Task<Respuesta> CambiarEstadoActuacionSeguimiento(int pActuacionSeguimientoId, string pEstadoReclamacionCodigo, string pUsuarioModifica);
//        Task<Respuesta> ActualizarRutaSoporteControversiaContractual(int pControversiaContractualId, string pRutaSoporte, string pUsuarioModifica);
//        Task<Respuesta> ActualizarRutaSoporteControversiaActuacion(int pControversiaActuacionId, string pRutaSoporte, string pUsuarioModifica);
//        Task<Respuesta> EliminarControversiaActuacion(int pControversiaActuacionId, string pUsuario);
//        Task<Respuesta> EliminarControversiaContractual(int pControversiaContractualId, string pUsuario);
//    }
//}
