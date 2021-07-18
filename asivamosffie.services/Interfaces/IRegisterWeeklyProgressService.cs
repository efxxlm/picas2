﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface IRegisterWeeklyProgressService
    { 
        Task<List<VRegistarAvanceSemanalNew>> GetVRegistrarAvanceSemanalNew();

        Task<List<VRegistrarAvanceSemanal>> GetVRegistrarAvanceSemanal();
        Task SendEmailWhenNoWeeklyAproved();

        Task SendEmailWhenNoWeeklyValidate();

        Task SendEmailWhenNoWeeklyProgress();

        Task<Respuesta> UploadContractTerminationCertificate(ContratacionProyecto pContratacionProyecto, AppSettingsService appSettingsService);

        Task<dynamic> GetObservacionBy(int pSeguimientoSemanalId, int pPadreId, string pTipoCodigo);

        Task<Respuesta> ChangueStatusMuestrasSeguimientoSemanal(int pSeguimientoSemanalID, string pEstadoMod, string pUsuarioMod);

        Task<Respuesta> ChangueStatusSeguimientoSemanal(int pContratacionProyectoId, string pEstadoMod, string pUsuarioMod);

        Task<Respuesta> DeleteGestionObraCalidadEnsayoLaboratorio(int GestionObraCalidadEnsayoLaboratorioId, string pUsuarioModificacion);

        Task<Respuesta> CreateEditEnsayoLaboratorioMuestra(GestionObraCalidadEnsayoLaboratorio pGestionObraCalidadEnsayoLaboratorio);

        Task<GestionObraCalidadEnsayoLaboratorio> GetEnsayoLaboratorioMuestras(int pGestionObraCalidadEnsayoLaboratorioId);

        Task<SeguimientoSemanal> GetLastSeguimientoSemanalByContratacionProyectoIdOrSeguimientoSemanalId(int pContratacionProyectoId, int pSeguimientoSemanalId);

        Task<List<dynamic>> GetListSeguimientoSemanalByContratacionProyectoId(int pContratacionProyectoId);

        Task<Respuesta> SaveUpdateSeguimientoSemanal(SeguimientoSemanal pSeguimientoSemanal);

        Task<Respuesta> DeleteManejoMaterialesInsumosProveedor(int ManejoMaterialesInsumosProveedorId, string pUsuarioModificacion);

        Task<Respuesta> DeleteResiduosConstruccionDemolicionGestor(int ResiduosConstruccionDemolicionGestorId, string pUsuarioModificacion);
    }
}
