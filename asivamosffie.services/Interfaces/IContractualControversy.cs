﻿using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IContractualControversy
    {
        Task<Respuesta> CreateEditarControversiaTAI(ControversiaContractual controversiaContractual);

        //Task<Respuesta> CreateEditNuevaActualizacionTramite(ControversiaActuacion controversiaActuacion);
        Task<Respuesta> CreateEditControversiaOtros(ControversiaActuacion controversiaActuacion);

        Task<Respuesta> CreateEditarActuacionSeguimiento(ActuacionSeguimiento actuacionSeguimiento);

        Task<Respuesta> InsertEditControversiaMotivo(ControversiaMotivo controversiaMotivo);

        Task<List<GrillaTipoSolicitudControversiaContractual>> ListGrillaTipoSolicitudControversiaContractual(int pControversiaContractualId = 0);        

        Task<VistaContratoContratista> GetVistaContratoContratista(int pContratoId);

        Task<ControversiaContractual> GetControversiaContractualById(int pControversiaContractualId);

        //Task<ControversiaActuacion> GetControversiaActuacionById(int id);
        Task<List<GrillaActuacionSeguimiento>> ListGrillaActuacionSeguimiento(int pControversiaContractualId = 0);
        Task<List<GrillaActuacionSeguimiento>> ListGrillaActuacionSeguimientoByActid(int pControversiaActId);

        Task<List<GrillaControversiaActuacionEstado>> ListGrillaControversiaActuacion(int id = 0, int pControversiaContractualId = 0,  bool esActuacionReclamacion = false);
         

        Task<ControversiaActuacion> GetControversiaActuacionById(int id);

        Task<ActuacionSeguimiento> GetActuacionSeguimientoById(int id);
        Task<List<ControversiaMotivo>> GetMotivosSolicitudByControversiaContractualId(int id);

        Task<byte[]> GetPlantillaControversiaContractual(int pContratoId);  

            //Task<byte[]> GetPlantillaActaIdComite(int ComiteId);
        
       Task<List<Contrato>> GetListContratos();
        
        Task<Respuesta> CambiarEstadoControversiaActuacion2(int pControversiaActuacionId, string pNuevoCodigoProximaActuacion, string pUsuarioModifica);
        Task<Respuesta> CambiarEstadoControversiaActuacion(int pControversiaActuacionId, string pNuevoCodigoEstadoAvance, string pUsuarioModifica);

        Task<Respuesta> CambiarEstadoControversiaContractual(int pControversiaContractualId, string pNuevoCodigoEstado, string pUsuarioModifica);

        Task<Respuesta> CambiarEstadoActuacionSeguimientoActuacion(int pActuacionSeguimientoId, string pEstadoReclamacionCodigo, string pUsuarioModifica);
        Task<Respuesta> ActualizarRutaSoporteControversiaContractual(int pControversiaContractualId, string pRutaSoporte, string pUsuarioModifica);
        Task<Respuesta> ActualizarRutaSoporteControversiaActuacion(int pControversiaActuacionId, string pRutaSoporte, string pUsuarioModifica);
        Task<Respuesta> EliminarControversiaActuacion(int pControversiaActuacionId, string pUsuario);
        Task<Respuesta> EliminarControversiaContractual(int pControversiaContractualId, string pUsuario);
        Task<List<GrillaTipoSolicitudControversiaContractual>> GetListGrillaControversiaActuaciones();
        Task<Respuesta> FinalizarActuacion(int pControversiaActuacionId, string value);
        Task<Respuesta> CreateEditarSeguimientoDerivado(SeguimientoActuacionDerivada actuacionSeguimiento);
        Task<Respuesta> FinalizarActuacionDerivada(int pControversiaActuacionId, string value);
        Task<Respuesta> EliminacionActuacionDerivada(int pControversiaActuacionId, string value);
        Task<ActionResult<List<GrillaControversiaActuacionEstado>>> GetListGrillaControversiaReclamacion(int id);
        Task<Respuesta> CreateEditarReclamaciones(ControversiaActuacion prmControversiaActuacion);
        Task<Respuesta> CreateEditarMesa(ControversiaActuacionMesa prmMesa);
        Task<List<ControversiaActuacionMesa>> GetMesasByControversiaActuacionId(int pControversiaActuacionId);
        Task<Respuesta> FinalizarMesa(int pControversiaActuacionId, string value);
        Task<Respuesta> SetStateActuacionMesa(int pActuacionMesaId, string pNuevoCodigoEstadoAvance, string value);
        Task<List<ControversiaActuacionMesaSeguimiento>> GetActuacionesMesasByMesaId(int pControversiaActuacionMesaID);
        Task<Respuesta> CreateEditarActuacionMesa(ControversiaActuacionMesaSeguimiento controversiaActuacionMesa);
        Task<ControversiaActuacionMesaSeguimiento> GetActuacionMesaByActuacionMesaId(int pControversiaActuacionMesaID);
        Task<Respuesta> CambiarEstadoActuacionReclamacion(int pActuacionSeguimientoId, string pEstadoReclamacionCodigo, string value);
        Task<Respuesta> CambiarEstadoActuacionReclamacionSeguimiento(int pActuacionId, string pEstadoReclamacionCodigo, string value);
        Task<Respuesta> EliminarActuacionSeguimientoActuacion(int pActuacionSeguimientoId, string value);
        Task<Respuesta> CambiarEstadoActuacionSeguimiento(int pActuacionSeguimientoId, string pEstadoReclamacionCodigo, string value, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender);
        Task<List<GrillaControversiaActuacionEstado>> GetListGrillMesasByControversiaActuacionId(int id);
        Task<List<ControversiaActuacionMesaSeguimiento>> GetActuacionesMesasByActuacionId(int pActuacionId);
        Task<Respuesta> EliminacionActuacionMesa(int pControversiaActuacionMesaId, string value);
        Task<Respuesta> EliminacionMesa(int pMesaId, string value);
        Task<ControversiaActuacionMesa> GetMesaByMesaId(int pControversiaMesaID);
        Task<SeguimientoActuacionDerivada> GetSeguimientoActuacionDerivadabyId(int pSeguimientoActuacionDerivadaId);
        Task<Respuesta> ChangeStateActuacion(int pControversiaActuacionId, string value);
        Task VencimientoTerminosContrato();
        bool ValidarCumpleTaiContratista(int pContratoId, bool esNovedad, bool esSeguimiento, int pProyectoId);
        DateTime? FechaActuacionTaiContratista(int pContratoId);

    }
}
