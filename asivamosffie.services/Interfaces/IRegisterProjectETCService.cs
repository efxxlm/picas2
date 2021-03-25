using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface IRegisterProjectETCService
    {
        Task<List<InformeFinal>> GetListInformeFinal();
        Task<List<dynamic>> GetProyectoEntregaETCByInformeFinalId(int pInformeFinalId);
        //POST
        Task<Respuesta> CreateEditRecorridoObra(ProyectoEntregaEtc pRecorrido);
        Task<Respuesta> CreateEditRepresentanteETC(RepresentanteEtcrecorrido pRepresentante);
        Task<Respuesta> CreateEditRemisionDocumentosTecnicos(ProyectoEntregaEtc pDocumentos);
        Task<Respuesta> CreateEditActaBienesServicios(ProyectoEntregaEtc pActaServicios);
        Task<ProyectoEntregaEtc> GetProyectoEntregaEtc(int informeFinalId);
        Task<Respuesta> SendProjectToEtc(int informeFinalId, string pUsuario);

    }
}
