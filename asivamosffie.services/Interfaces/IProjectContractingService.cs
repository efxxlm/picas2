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
        Task<Contratacion> GetContratacionByContratacionId(int pContratacionId);

        Task<List<ProyectoGrilla>> GetListProyectsByFilters(string pTipoIntervencion, string pLlaveMen, string pMunicipio, int pIdInstitucionEducativa, int pIdSede);

        Task<List<ContratistaGrilla>> GetListContractingByFilters(string pTipoIdentificacionCodigo, string pNumeroIdentidicacion, string pNombre, bool? EsConsorcio);

        Task<List<Contratacion>> GetListContratacion();

        Task<Respuesta> CreateContratacionProyecto(Contratacion pContratacion, string usuarioCreacion);

        Task<List<ContratacionProyecto>> GetListContratacionProyectoByContratacionId(int idContratacion);

        Task<Respuesta> CreateEditContratacion(Contratacion Pcontratacion);

        Task<Respuesta> CreateEditContratacionProyecto(ContratacionProyecto contratacionProyecto , bool esTransaccion);

        Task<Respuesta> CreateEditContratacionProyectoAportante(ContratacionProyectoAportante pContratacionProyectoAportante, bool esTransaccion);

        Task<ContratacionProyecto> GetContratacionProyectoById(int idContratacionProyecto);

        Task<Respuesta> CreateEditContratacionProyectoAportanteByContratacionproyecto(ContratacionProyecto pContratacionProyecto, bool esTransaccion);
    }
}
