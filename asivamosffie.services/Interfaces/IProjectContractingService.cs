using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.services.Interfaces
{
    public interface IProjectContractingService
    {
        Task<Contratacion> GetListContratacionObservacion(int pContratacionId);

        Task<Respuesta> DeleteComponenteUso(int pComponenteUsoId, string pUsuarioMod);
        Task<Respuesta> DeleteComponenteAportante(int pComponenteAportanteId, string pUsuarioMod);

        Task<Contratacion> GetAllContratacionByContratacionId(int pContratacionId);

        Task<Respuesta> ChangeStateContratacionByIdContratacion(int idContratacion, string PCodigoEstado, string pUsusarioModifico,
            string  DominioFront, string MailServer, int MailPort, bool EnableSSL, string Password, string Sender);

        Task<Respuesta> DeleteContratacionByIdContratacion(int idContratacion, string pUsusarioElimino);

        Task<Contratacion> GetContratacionByContratacionId(int pContratacionId);

        Task<List<ProyectoGrilla>> GetListProyectsByFilters(string pTipoIntervencion, string pLlaveMen, string pRegion, string pDepartamento, string pMunicipio, int pIdInstitucionEducativa, int pIdSede);

        Task<List<ContratistaGrilla>> GetListContractingByFilters(string pTipoIdentificacionCodigo, string pNumeroIdentidicacion, string pNombre, bool? EsConsorcio);

        Task<List<Contratacion>> GetListContratacion();

        Task<Respuesta> CreateContratacionProyecto(Contratacion pContratacion, string usuarioCreacion);

        Task<List<ContratacionProyecto>> GetListContratacionProyectoByContratacionId(int idContratacion);

        Task<Respuesta> CreateEditContratacion(Contratacion Pcontratacion);

        Task<Respuesta> CreateEditContratacionTermLimit(int contratacionId ,TermLimit plazoContratacion);
        Task<Respuesta> CreateEditContratacionProyecto(ContratacionProyecto contratacionProyecto , bool esTransaccion);

        Task<Respuesta> CreateEditContratacionProyectoAportante(ContratacionProyectoAportante pContratacionProyectoAportante, bool esTransaccion);

        Task<ContratacionProyecto> GetContratacionProyectoById(int idContratacionProyecto);

        Task<Respuesta> CreateEditContratacionProyectoAportanteByContratacionproyecto(ContratacionProyecto pContratacionProyecto, bool esTransaccion);

        Task<Contratacion> GetContratacionByContratacionIdWithGrillaProyecto(int pContratacionId);

        Task<List<FaseComponenteUso>> GetListFaseComponenteUso();
    }
}
