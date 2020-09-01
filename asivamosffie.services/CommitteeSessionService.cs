using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class CommitteeSessionService : ICommitteeSessionService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public CommitteeSessionService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }


        #region "Gestion de actas";
        //Grilla sesion comite, => sesiones desarrolladas sin actas.
        public async Task<ActionResult<List<ComiteTecnico>>> GetSesionSinActa()
        {
            try
            {
                 return await (from n in _context.ComiteTecnico
                                where n.EstadoComiteCodigo == "3" && (bool)!n.Eliminado
                                select new ComiteTecnico
                                {
                                    ComiteTecnicoId = n.ComiteTecnicoId,
                                    NumeroComite = n.NumeroComite,
                                    FechaOrdenDia = n.FechaOrdenDia,
                                    //TipoDominioId = 38 -> Estado comite
                                    EstadoComiteCodigo = n.EstadoComiteCodigo == "3" ? "Sin acta" : n.EstadoComiteCodigo == "4" ? "En proceso de aprobación" : n.EstadoComiteCodigo == "5" ? "Aprobada": "Devuelta" ,
                                    EsCompleto = n.EsCompleto
                                }).OrderByDescending(s => s.FechaOrdenDia).ToListAsync();


            }

            catch (Exception)
            {

                throw;
            }
        }



        #endregion



        #region "Sesion";


        //Grilla sesion comite
        public async Task<ActionResult<IEnumerable<GridCommitteeSession>>> GetCommitteeSession(int? comiteTecnicoId)
        {

            List<ComiteTecnico> ListSesion = (comiteTecnicoId != null ? await _context.ComiteTecnico.Where(s => s.ComiteTecnicoId == comiteTecnicoId && (bool)!s.Eliminado).ToListAsync() : await _context.ComiteTecnico.Where(s => (bool)!s.Eliminado).ToListAsync());

            List<GridCommitteeSession> ListGridCommitteeSession = new List<GridCommitteeSession>();


            foreach (var ss in ListSesion)
            {
                GridCommitteeSession SesionGrid = new GridCommitteeSession
                {
                    ComiteTecnicoId = ss.ComiteTecnicoId,
                    FechaDeComite = ss.FechaOrdenDia,
                    EstadoComiteCodigo = ss.EstadoComiteCodigo,
                    NumeroComite = ss.NumeroComite,
                    EstadoComiteText = ss.EstadoComiteCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(ss.EstadoComiteCodigo, (int)EnumeratorTipoDominio.Estado_Comite) : "",
                };
                ListGridCommitteeSession.Add(SesionGrid);
            }

            return ListGridCommitteeSession;
        }


        // Ver detalle
        public async Task<ActionResult<IEnumerable<GridCommitteeSession>>> GetCommitteeSessionTemaById(int sessionTemaId)
        {

            List<SesionComiteTema> ListSesionComiteTema = await _context.SesionComiteTema.Where(s => s.ComiteTecnicoId == sessionTemaId && (bool)!s.Eliminado).ToListAsync();

            List<GridCommitteeSession> ListGridsesionComiteTema = new List<GridCommitteeSession>();


            foreach (var st in ListSesionComiteTema)
            {
                GridCommitteeSession sesionComiteTemaGrid = new GridCommitteeSession
                {
                    ComiteTecnicoId = st.ComiteTecnicoId,
                    SesionComiteTemaId = st.SesionTemaId,
                    Tema = st.Tema,
                    Responzable = st.ResponsableCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(st.ResponsableCodigo, (int)EnumeratorTipoDominio.Estado_Comite) : "",
                    TiempoIntervencion = st.TiempoIntervencion,
                    UrlSoporte = st.RutaSoporte,
                    Observaciones = st.Observaciones,
                    ObservacionesDecision = st.ObservacionesDecision
                };

                ListGridsesionComiteTema.Add(sesionComiteTemaGrid);
            }

            return ListGridsesionComiteTema;
        }



        //todas las solicitudes que fueron aprobadas por el comite tecnico.
        //TipoDominioId = 38, Codigo = 2, Nombre = Convocada
        public async Task<ActionResult<List<GridCommitteeSession>>> GetCommitteeSessionFiduciario()
        {
            try
            {
                List<ComiteTecnico> ListSesion = await _context.ComiteTecnico.Where(s => s.EstadoComiteCodigo == "2").ToListAsync();
                List<GridCommitteeSession> ListGridCommitteeSessionFiduciario = new List<GridCommitteeSession>();

                foreach (var ss in ListSesion)
                {
                    GridCommitteeSession SesionGrid = new GridCommitteeSession
                    {
                        SesionComiteTemaId = ss.ComiteTecnicoId,
                        FechaDeComite = ss.FechaOrdenDia,
                        NumeroComite = ss.NumeroComite,
                    };

                    ListGridCommitteeSessionFiduciario.Add(SesionGrid);
                }

                return ListGridCommitteeSessionFiduciario;

            }
            catch (Exception)
            {

                throw;
            }
        }


        // orden del dia [Fecha Solicitud, Numero Solicitud, TipoSolicitud,FechaDecomiteTecnico,NumeroComiteTecnico]
        public async Task<ActionResult<List<ComiteTecnico>>> GetCommitteeSession()
        {
            try
            {
                return await _context.ComiteTecnico.Where(sc => sc.EstadoComiteCodigo == "2" && !(bool)sc.Eliminado).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }


        //Aplazar sesion
        public async Task<bool> SessionPostpone(int ComiteTecnicoId, DateTime newDate, string usuarioModifico)
        {

            try
            {
                int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Sesion_Comite_Tema, (int)EnumeratorTipoDominio.Acciones);
                if (string.IsNullOrEmpty(Convert.ToString(ComiteTecnicoId)) || newDate != null)
                {

                    var comiteTecnico = await _context.ComiteTecnico.FindAsync(ComiteTecnicoId);
                    if (comiteTecnico == null)
                    {
                        throw new Exception("No se encontro la sesion");
                    }

                    comiteTecnico.FechaOrdenDia = newDate;
                    comiteTecnico.EstadoComiteCodigo = "6"; // Aplazada. TipoDominioId = 38, 
                    comiteTecnico.UsuarioModificacion = usuarioModifico;
                    _context.ComiteTecnico.Update(comiteTecnico);

                    var resultado = await _context.SaveChangesAsync();
                    if (resultado > 0) //TODO: Enviar notificación a los miembros del comite indicando la nueva fecha se sesion.
                        return true;
                    else
                        return false;
                }

                return false;
            }

            catch (Exception)
            {
                return false;
            }
        }



        //Declarar fallida
        public async Task<bool> SessionDeclaredFailed(int ComiteTecnicoId, string usuarioModifico)
        {

            try
            {
                int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Sesion_Comite_Tema, (int)EnumeratorTipoDominio.Acciones);
                if (string.IsNullOrEmpty(Convert.ToString(ComiteTecnicoId)))
                {

                    var sesion = await _context.ComiteTecnico.FindAsync(ComiteTecnicoId);
                    if (sesion == null)
                    {
                        throw new Exception("No se encontro la sesion");
                    }
                    sesion.EstadoComiteCodigo = "7"; // Fallida. TipoDominioId = 38, 
                    sesion.UsuarioModificacion = usuarioModifico;
                    _context.ComiteTecnico.Update(sesion);

                    var resultado = await _context.SaveChangesAsync();
                    if (resultado > 0)
                        return true;
                    else
                        return false;
                }

                return false;
            }

            catch (Exception)
            {
                return false;
            }
        }



        //Registrar invitado
        public async Task<Respuesta> CreateOrEditGuest(SesionInvitado sesionInvitado)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Sesion_invitado, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            SesionInvitado sesionInvitadoAntiguo = null;
            try
            {

                if (string.IsNullOrEmpty(sesionInvitado.SesionInvitadoId.ToString()) || sesionInvitado.SesionInvitadoId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR SESION COMITE INVITADO";
                    sesionInvitado.FechaCreacion = DateTime.Now;
                    sesionInvitado.Eliminado = false;
                    _context.SesionInvitado.Add(sesionInvitado);
                }
                else
                {
                    strCrearEditar = "EDIT SESION COMITE INVITADO";
                    sesionInvitadoAntiguo = _context.SesionInvitado.Find(sesionInvitado.SesionInvitadoId);
                    //Auditoria
                    sesionInvitadoAntiguo.UsuarioModificacion = sesionInvitado.UsuarioModificacion;
                    sesionInvitadoAntiguo.FechaModificacion = DateTime.Now;
                    sesionInvitadoAntiguo.Eliminado = false;


                    //Registros
                    sesionInvitadoAntiguo.SesionInvitadoId = sesionInvitado.SesionInvitadoId;
                    sesionInvitadoAntiguo.Nombre = sesionInvitado.Nombre;
                    sesionInvitadoAntiguo.Cargo = sesionInvitado.Cargo;
                    sesionInvitadoAntiguo.Entidad = sesionInvitado.Entidad;

                    _context.SesionInvitado.Update(sesionInvitadoAntiguo);

                }

                return respuesta = new Respuesta        
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = sesionInvitado,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, sesionInvitado.UsuarioCreacion, strCrearEditar)

                };
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, sesionInvitado.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }



        //Registrar Sesion Comentario ->  // No guarda desde este caso de uso
        public async Task<Respuesta> CreateOrEditSesioncomment(SesionComentario sesionComentario)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Sesion_comentario, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            SesionComentario sesionComentarioAntiguo = null;
            try
            {

                if (string.IsNullOrEmpty(sesionComentario.SesionComentarioId.ToString()) || sesionComentario.SesionComentarioId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR SESION COMENTARIO";
                    sesionComentario.FechaCreacion = DateTime.Now;
                    _context.SesionComentario.Add(sesionComentario);
                }
                else
                {
                    strCrearEditar = "EDIT SESION COMENTARIO";
                    sesionComentarioAntiguo = _context.SesionComentario.Find(sesionComentario.SesionComentarioId);
                    //Auditoria
                    sesionComentarioAntiguo.UsuarioModificacion = sesionComentario.UsuarioModificacion;
                    sesionComentarioAntiguo.FechaModificacion = DateTime.Now;


                    //Registros
                    sesionComentarioAntiguo.ComiteTecnicoId = sesionComentario.ComiteTecnicoId;
                    sesionComentarioAntiguo.Fecha = sesionComentario.Fecha;
                    sesionComentarioAntiguo.MiembroSesionParticipante = sesionComentario.MiembroSesionParticipante;
                    sesionComentarioAntiguo.Observacion = sesionComentario.Observacion;

                    _context.SesionComentario.Update(sesionComentarioAntiguo);

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = sesionComentario,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, sesionComentario.UsuarioCreacion, strCrearEditar)

                };
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, sesionComentario.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }



        //Registrar temas compromisos
        public async Task<Respuesta> CreateOrEditSubjects(TemaCompromiso temaCompromiso)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Tema_Compromiso, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            TemaCompromiso temaCompromisoAntiguo = null;
            try
            {

                if (string.IsNullOrEmpty(temaCompromiso.TemaCompromisoId.ToString()) || temaCompromiso.TemaCompromisoId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR TEMA COMPROMISO";
                    temaCompromiso.UsuarioCreacion = temaCompromiso.UsuarioCreacion;
                    temaCompromiso.FechaCreacion = DateTime.Now;
                    _context.TemaCompromiso.Add(temaCompromiso);
                }
                else
                {
                    strCrearEditar = "EDIT TEMA COMPROMISO";
                    temaCompromisoAntiguo = _context.TemaCompromiso.Find(temaCompromiso.TemaCompromisoId);
                    //Auditoria
                    temaCompromisoAntiguo.UsuarioModificacion = temaCompromiso.UsuarioModificacion;
                    temaCompromisoAntiguo.FechaModificacion = DateTime.Now;

                    //Registros
                    temaCompromisoAntiguo.SesionTemaId = temaCompromiso.SesionTemaId;
                    temaCompromisoAntiguo.Tarea = temaCompromiso.Tarea;
                    temaCompromisoAntiguo.Responsable = temaCompromiso.Responsable;
                    temaCompromisoAntiguo.FechaCumplimiento = temaCompromiso.FechaCumplimiento;

                    _context.TemaCompromiso.Update(temaCompromisoAntiguo);

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = temaCompromiso,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, temaCompromiso.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, temaCompromiso.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }


        //Detalle de registro sesion invitado
        public async Task<SesionInvitado> GetSesionGuesById(int sesionInvitadoId)
        {
            try
            {
                return await _context.SesionInvitado.FindAsync(sesionInvitadoId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Crear orden del dia de comité fiduciario
        public async Task<Respuesta> CreateOrEditCommitteeSession(SesionComiteTema sesionComiteTema)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Sesion_Comite_Tema, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            SesionComiteTema sesionComiteTemaAntiguo = null;
            try
            {

                if (string.IsNullOrEmpty(sesionComiteTema.SesionTemaId.ToString()) || sesionComiteTema.SesionTemaId == 0)
                {
                    //TODO: recorrer objeto SesionComiteTema, Se puede guardar uno o varios temas.

                    //Auditoria
                    strCrearEditar = "CREAR SESION COMITÉ TEMA";
                    sesionComiteTema.FechaCreacion = DateTime.Now;
                    sesionComiteTema.Eliminado = false;

                    //Registros
                    sesionComiteTema.EsAprobado = false;
                    sesionComiteTema.ResponsableCodigo = string.IsNullOrEmpty(sesionComiteTema.ResponsableCodigo) ? string.Empty : sesionComiteTema.ResponsableCodigo;
                    _context.SesionComiteTema.Add(sesionComiteTema);
                }
                else
                {
                    strCrearEditar = "EDIT SESION COMITÉ TEMA";
                    sesionComiteTemaAntiguo = _context.SesionComiteTema.Find(sesionComiteTema.SesionTemaId);
                    //Auditoria
                    sesionComiteTemaAntiguo.UsuarioModificacion = sesionComiteTema.UsuarioModificacion;
                    sesionComiteTemaAntiguo.FechaModificacion = DateTime.Now;


                    //Registros

                    sesionComiteTemaAntiguo.SesionTemaId = sesionComiteTema.SesionTemaId;
                    sesionComiteTemaAntiguo.Tema = sesionComiteTema.Tema;
                    sesionComiteTemaAntiguo.ResponsableCodigo = sesionComiteTema.ResponsableCodigo;
                    sesionComiteTemaAntiguo.TiempoIntervencion = sesionComiteTema.TiempoIntervencion; // En minutos

                    _context.SesionComiteTema.Update(sesionComiteTemaAntiguo);
                   
                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = sesionComiteTema,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, sesionComiteTema.UsuarioCreacion, strCrearEditar)

                };
            }
            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, sesionComiteTemaAntiguo.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }




        //Eliminar Sesion
        public async Task<bool> DeleteTema(int temaId)
        {
            ComiteTecnico tema = await _context.ComiteTecnico.FindAsync(temaId);
            bool retorno = true;
            try
            {
                tema.Eliminado = true;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return retorno;
        }
        #endregion
    }
}
