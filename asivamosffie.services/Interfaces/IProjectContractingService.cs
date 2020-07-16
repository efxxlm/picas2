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
        Task<List<ProyectoGrilla>> GetListProyectsByFilters(string pTipoIntervencion, string pLlaveMen, string pMunicipio, int pIdInstitucionEducativa, int pIdSede);

        Task<List<ContratistaGrilla>> GetListContractingByFilters(string pTipoIdentificacionCodigo, string pNumeroIdentidicacion, string pNombre, bool? EsConsorcio);

        Task<List<Contratacion>> GetListContratacion();

        Task<Respuesta> CreateContratacionProyecto(int[] idsProyectos, string tipoSolicitudCodigo, string usuarioCreacion);

        Task<List<ContratacionProyecto>> GetContratacionByContratacionId(int idContratacion);
    }
}
