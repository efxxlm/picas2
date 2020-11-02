using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface IRegisterSessionTechnicalCommitteeService
    {
        Task<List<SesionParticipante>> GetSesionParticipantesByIdComite(int pComiteId);

        Task<List<SesionSolicitudObservacionProyecto>> GetSesionSolicitudObservacionProyecto(int pSesionComiteSolicitudId, int pContratacionProyectoId);

        Task<Respuesta> ObservacionesCompromisos(ObservacionComentario pObservacionComentario);

        Task<dynamic> ListMonitoreo(bool EsFiduciario);

        Task<byte[]> GetPlantillaActaIdComite(int pdComite);

        Task<Respuesta> CambiarEstadoActa(int pSesionComiteSolicitud, string pCodigoEstado, string pUsuarioModifica);

        Task<Respuesta> CrearObservacionProyecto(ContratacionObservacion pContratacionObservacion);

        Task<Respuesta> VerificarTemasCompromisos(ComiteTecnico pComiteTecnico);

        Task<ComiteTecnico> GetCompromisosByComiteTecnicoId(int ComiteTecnicoId);

        Task<Respuesta> DeleteComiteTecnicoByComiteTecnicoId(int pComiteTecnicoId, string pUsuarioModifico);

        Task<Respuesta> CreateEditSesionSolicitudObservacionProyecto(SesionSolicitudObservacionProyecto pSesionSolicitudObservacionProyecto);

        Task<Respuesta> CreateEditTemasCompromiso(SesionComiteTema pSesionComiteTema);

        Task<Respuesta> ConvocarComiteTecnico(ComiteTecnico pComiteTecnico, string pDominio, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender);

        Task<Respuesta> GetNoRequiereVotacionSesionComiteSolicitud(SesionComiteSolicitud pSesionComiteSolicitud);

        Task<Respuesta> CreateEditSesionSolicitudVoto(SesionComiteSolicitud pSesionComiteSolicitud);

        Task<Respuesta> CreateEditSesionComiteTema(List<SesionComiteTema> ListSesionComiteTemas);

        Task<Respuesta> DeleteSesionInvitado(int pSesionInvitadoId, string pUsuarioModificacion);

        Task<Respuesta> CreateEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud(ComiteTecnico pComiteTecnico);

        Task<Respuesta> CambiarEstadoComiteTecnico(ComiteTecnico pComiteTecnico);

        Task<List<dynamic>> GetListSesionComiteSolicitudByFechaOrdenDelDia(DateTime pFechaOrdenDelDia);

        Task<ComiteTecnico> GetComiteTecnicoByComiteTecnicoId(int pComiteTecnicoId);

        Task<Respuesta> CreateEditSesionInvitadoAndParticipante(ComiteTecnico pComiteTecnico);

        Task<Respuesta> EliminarSesionComiteTema(int pSesionComiteTemaId, string pUsuarioModificacion);

        Task<byte[]> GetPlantillaByTablaIdRegistroId(string pTablaId, int pRegistroId);

        Task<List<ComiteGrilla>> GetListComiteGrilla();

        Task<List<dynamic>> GetListSesionComiteTemaByComiteTecnicoId(int pComiteTecnicoId);

        Task<Respuesta> CreateEditSesionTemaVoto(SesionComiteTema pSesionComiteTema);

        Task<Respuesta> NoRequiereVotacionSesionComiteTema(int idSesionComiteTema, bool pRequiereVotacion, string pUsuarioCreacion);

        Task<Respuesta> AplazarSesionComite(ComiteTecnico pComiteTecnico, string pDominio, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender);

        Task<Respuesta> CreateEditActasSesionSolicitudCompromiso(SesionComiteSolicitud pSesionComiteSolicitud);

        Task<List<SesionComentario>> GetCometariosDelActa(int pComietTecnicoId);

        Task<ProcesoSeleccionMonitoreo> GetProcesoSeleccionMonitoreo(int pProcesoSeleccionMonitoreoId);
    
        Task<Respuesta> EliminarCompromisosSolicitud(int pSesionComiteSolicitudId, string pUsuarioModificacion);
     
        Task<Respuesta> EliminarCompromisosTema(int pSesionTemaId, string pUsuarioModificacion);
       
        Task<Respuesta> EnviarComiteParaAprobacion(ComiteTecnico pComiteTecnico, string pDominio, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender);

    }
}
