using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using DinkToPdf;
using System.IO;
using DinkToPdf.Contracts;
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

    /*
       PARAMETRICAS

     Tipo tema = 1. Solicitudes contractuales, 2. Tema nuevo
     TipoDominioId = 42
    ------------------------------------

     MiembrosComiteTecnico = 1.	Dirección técnica, 2.	Dirección financiera, 3.	Dirección jurídica, 4.	Fiduciaria, 5.	Dirección administrativa
     TipoDominioId = 46
    ------------------------------------

    */

    public class CommitteeSessionFiduciarioService : ICommitteeSessionFiduciarioService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public CommitteeSessionFiduciarioService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;

        }


        #region "ORDEN DEL DIA";

        //Solicitudes acordeon => Para seleccion de solicitudes contractuales y tema nuevo
        public async Task<List<ComiteTecnico>> GetRequestCommitteeSessionById(int comiteTecnicoId)
        {
            try
            {
                return await _context.ComiteTecnico.Include(cm => cm.SesionComiteTema).Where(sc => sc.ComiteTecnicoId == comiteTecnicoId && !(bool)sc.Eliminado).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }


        //Crear un nuevo tema
        public async Task<Respuesta> CreateOrEditTema(SesionComiteTema sesionComiteTema, DateTime fechaComite)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Sesion_Comite_Tema, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            SesionComiteTema sesionComiteTemaAntiguo = null;
            var newComiteTecnicoId = 0;
            try
            {

                if (string.IsNullOrEmpty(sesionComiteTema.SesionTemaId.ToString()) || sesionComiteTema.SesionTemaId == 0)
                {

                    //Auditoria
                    strCrearEditar = "CREAR COMITE TECNICO FIDUCIARIO@#CREAR NUEVO TEMA";

                    //Crear Comite tecnico fiduciario inicial
                    int countMaxId = _context.ComiteTecnico.Max(cm => cm.ComiteTecnicoId);

                    ComiteTecnico comiteTecnico = new ComiteTecnico();
                    comiteTecnico.FechaCreacion = fechaComite;
                    comiteTecnico.UsuarioCreacion = sesionComiteTema.UsuarioCreacion;
                    comiteTecnico.EsComiteFiduciario = true;
                    comiteTecnico.EstadoComiteCodigo = "1";
                    comiteTecnico.EsCompleto = false;
                    comiteTecnico.EsAprobado = false;
                    comiteTecnico.Eliminado = false;
                    comiteTecnico.NumeroComite = Helpers.Helpers.Consecutive("CF", countMaxId);

                    _context.ComiteTecnico.Add(comiteTecnico);
                    var result = await _context.SaveChangesAsync();

                    if (result > 0)
                    {
                        newComiteTecnicoId = comiteTecnico.ComiteTecnicoId;
                        sesionComiteTema.ComiteTecnicoId = newComiteTecnicoId;
                        sesionComiteTema.FechaCreacion = DateTime.Now;
                        sesionComiteTema.UsuarioCreacion = sesionComiteTema.UsuarioCreacion;
                        sesionComiteTema.Eliminado = false;

                        _context.SesionComiteTema.Add(sesionComiteTema);
                    }

                }
                else
                {
                    strCrearEditar = "EDIT TEMA";
                    sesionComiteTemaAntiguo = _context.SesionComiteTema.Find(sesionComiteTema.SesionTemaId);

                    //Auditoria
                    sesionComiteTemaAntiguo.UsuarioModificacion = sesionComiteTema.UsuarioModificacion;
                    sesionComiteTemaAntiguo.Eliminado = false;


                    //Registros
                    sesionComiteTemaAntiguo.Tema = sesionComiteTema.Tema;
                    sesionComiteTemaAntiguo.ResponsableCodigo = sesionComiteTema.ResponsableCodigo;
                    sesionComiteTemaAntiguo.TiempoIntervencion = sesionComiteTema.TiempoIntervencion;
                    _context.SesionComiteTema.Add(sesionComiteTema);

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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, sesionComiteTema.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }


        //Ver detalle grilla comite tecnico
        public async Task<List<SesionComiteTema>> GetCommitteeSessionByComiteTecnicoId(int comiteTecnicoId)
        {
            try
            {
                return await _context.SesionComiteTema.Include(st => st.ComiteTecnico).Where(cm => !(bool)cm.Eliminado && cm.ComiteTecnicoId == comiteTecnicoId).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Get all seseion comite tecnico fiduciario
        public async Task<List<ComiteTecnico>> GetCommitteeSession()
        {
            try
            {
                return await _context.ComiteTecnico.Where(cm => !(bool)cm.Eliminado && (bool)cm.EsComiteFiduciario).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Grilla vefificar complimiento de compromisos
        public async Task<List<GridComiteTecnicoCompromiso>> GetCompromisosSolicitud()
        {
            try
            {
                return await (from n in _context.SesionComiteTecnicoCompromiso
                              where !(bool)n.Eliminado
                              select new GridComiteTecnicoCompromiso
                              {
                                  Tarea = n.Tarea,
                                  Responzable = n.Responsable,
                                  FechaCumplimiento = n.FechaCumplimiento,
                                  FechaReporte = n.FechaCreacion,
                                  EstadoReporte = n.EstadoCodigo
                              }).OrderByDescending(n => n.FechaCumplimiento).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Convocar sesión de comité
        public async Task<Respuesta> CallCommitteeSession(int comiteTecnicoId, string user)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Convocar_Sesion_Comite, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            ComiteTecnico comiteTecnico = null;
            try
            {


                strCrearEditar = "CONVOCAR SESION COMITE";
                comiteTecnico = await _context.ComiteTecnico.FindAsync(comiteTecnicoId);

                //Auditoria
                comiteTecnico.UsuarioModificacion = user;
                comiteTecnico.FechaModificacion = DateTime.Now;


                //Registros
                comiteTecnico.EstadoComiteCodigo = "2";
                _context.ComiteTecnico.Update(comiteTecnico);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    //Enviar notificacion
                }


                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = comiteTecnico,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, user, strCrearEditar)

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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, user, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }



        //Eliminar Tema
        public async Task<bool> DeleteTema(int sesionTemaId, string user)
        {
            SesionComiteTema sesionComiteTema = await _context.SesionComiteTema.FindAsync(sesionTemaId);
            bool status = false;
            try
            {
                status = sesionComiteTema.GeneraCompromiso.HasValue ? sesionComiteTema.GeneraCompromiso.Value : false;
                Console.Write(status);
                if (!status)
                {
                    // Se puede eliminar
                    sesionComiteTema.Eliminado = true;
                    sesionComiteTema.UsuarioCreacion = user;
                    sesionComiteTema.FechaModificacion = DateTime.Now;

                    _context.Remove(sesionComiteTema);
                    var resultado = await _context.SaveChangesAsync();
                    if (resultado > 0)
                        return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                return false;
            }

        }



        #endregion



        #region "SESIONES DE COMITE FIDUCIARIO";

        //Grilla sesiones programadas en estado Convocada.
        public async Task<List<ComiteTecnico>> GetConvokeSessionFiduciario(int? estadoComiteCodigo)
        {
            try
            {
                if (estadoComiteCodigo != null)
                    return await _context.ComiteTecnico.Include(st => st.SesionComiteTema).Where(cm => !(bool)cm.Eliminado && cm.EstadoComiteCodigo == Convert.ToString(estadoComiteCodigo) && cm.EsComiteFiduciario == true).ToListAsync();
                else
                    return await _context.ComiteTecnico.Include(st => st.SesionComiteTema).Where(cm => !(bool)cm.Eliminado && cm.EsComiteFiduciario == true).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }


        //Grilla validacion de solicitudes contractuales
        public async Task<List<GridValidationRequests>> GetValidationRequests()
        {
            //sc.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion
            return await
                (from sc in _context.SesionComiteSolicitud
                 join ct in _context.ComiteTecnico on sc.ComiteTecnicoId equals ct.ComiteTecnicoId
                 join ps in _context.ProcesoSeleccion on sc.SolicitudId equals ps.SolicitudId
                 where !(bool)ct.Eliminado && !(bool)ps.Eliminado && !(bool)sc.Eliminado

                 select new GridValidationRequests
                 {
                     ComiteTecnicoId = ct.ComiteTecnicoId,
                     SesionComiteSolicitudId = sc.SesionComiteSolicitudId,
                     FechaSolicitud = ps.FechaCreacion,
                     NumeroSolicitud = ps.NumeroProceso,
                     ProcesoSeleccionId = ps.ProcesoSeleccionId,
                     TipoSolicitudCodigo = sc.TipoSolicitudCodigo,
                     TipoSolicitudText = _context.Dominio.Where(r => (bool)r.Activo && r.DominioId.Equals(sc.TipoSolicitudCodigo) && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud).Select(r => r.Nombre).FirstOrDefault(),
                     FechaComiteTecnico = ct.FechaCreacion,
                     NumeroComite = ct.NumeroComite,
                     TemaRequiereVotacion = sc.RequiereVotacion,
                     sesionParticipanteVoto = (List<SesionParticipanteVoto>)(from v in _context.SesionParticipanteVoto
                                                                             where v.ComiteTecnicoId == ct.ComiteTecnicoId
                                                                             select new SesionParticipanteVoto
                                                                             {
                                                                                 SesionParticipanteVotoId = v.SesionParticipanteId,
                                                                                 SesionParticipante =
                                                                                 (SesionParticipante)(from sp in _context.SesionParticipante
                                                                                                      where sp.ComiteTecnicoId == v.ComiteTecnicoId && !(bool)sp.Eliminado
                                                                                                      select new SesionParticipante
                                                                                                      {
                                                                                                          ComiteTecnicoId = sp.ComiteTecnicoId,
                                                                                                          UsuarioId = sp.UsuarioId,
                                                                                                          Nombres = _context.Usuario.Where(u => (bool)u.Activo && u.UsuarioId.Equals(sp.UsuarioId)).Select(u => string.Concat(u.Nombres, " ", u.Apellidos)).FirstOrDefault(),
                                                                                                      }),
                                                                                 SesionParticipanteId = v.SesionParticipanteId,
                                                                                 EsAprobado = v.EsAprobado,
                                                                                 Observaciones = v.Observaciones,
                                                                                 ObservacionesDevolucion = v.ObservacionesDevolucion

                                                                             })


                 }).ToListAsync();

        }

        public async Task<Respuesta> CreateOrEditVotacionSolicitud(List<SesionSolicitudVoto> listSolicitudVoto)
        {

            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Votacion_Solicitud_Participante, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            SesionSolicitudVoto solicitudVoto = null; ;

            try
            {



                //Auditoria
                strCrearEditar = "VOTACION SOLICITUD PARTICIPANTE";
                if (listSolicitudVoto.Count > 0)
                {

                    foreach (var list in listSolicitudVoto)
                    {

                        if (string.IsNullOrEmpty(list.SesionSolicitudVotoId.ToString()) || list.SesionSolicitudVotoId == 0)
                        {
                            listSolicitudVoto = new List<SesionSolicitudVoto>();
                            solicitudVoto.SesionComiteSolicitudId = list.SesionComiteSolicitudId;
                            solicitudVoto.SesionParticipanteId = list.SesionParticipanteId;
                            solicitudVoto.EsAprobado = list.EsAprobado;
                            solicitudVoto.Observacion = list.Observacion;


                            solicitudVoto.FechaCreacion = DateTime.Now;
                            solicitudVoto.UsuarioCreacion = list.UsuarioCreacion;
                            _context.SesionSolicitudVoto.Add(solicitudVoto);
                        }
                    }

                }




                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = listSolicitudVoto,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, listSolicitudVoto.FirstOrDefault().UsuarioCreacion, strCrearEditar)

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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, listSolicitudVoto.FirstOrDefault().UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }
        //Ver soporte
        public async Task<List<SesionComiteSolicitud>> StartDownloadResumenFichaSolicitud()
        {

            try
            {
                return null;

            }
            catch (Exception)
            {

                throw;
            }
        }


        //Lista participantes
        public async Task<List<Usuario>> GetListParticipantes()
        {

            try
            {
                return await (from u in _context.Usuario
                              where u.Eliminado == false && u.Activo == true
                              select new Usuario
                              {
                                  UsuarioId = u.UsuarioId,
                                  Email = u.Email,
                                  Nombres = string.Concat(u.Nombres, " ", u.Apellidos)
                              }).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }


        //Registrar Mienbros invitados.
        public async Task<Respuesta> CreateOrEditInvitedMembers(SesionParticipante sesionParticipante)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Registrar_Mienbros_Invitados, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            SesionParticipante _sesionParticipante = null;

            try
            {

                if (string.IsNullOrEmpty(sesionParticipante.SesionParticipanteId.ToString()) || sesionParticipante.SesionParticipanteId == 0)
                {

                    //Auditoria
                    strCrearEditar = "CREAR MIENBROS INVITADOS";
                    // if (sesionParticipante.UsersIds.Count > 0)
                    // {
                    //     foreach (var list in sesionParticipante.UsersIds)
                    //     {
                    //         _sesionParticipante = new SesionParticipante();
                    //         _sesionParticipante.ComiteTecnicoId = sesionParticipante.ComiteTecnicoId;
                    //         _sesionParticipante.UsuarioId = list.UsuarioId;
                    //         _sesionParticipante.FechaCreacion = DateTime.Now;
                    //         _sesionParticipante.UsuarioCreacion = sesionParticipante.UsuarioCreacion;
                    //         _sesionParticipante.Eliminado = false;
                    //         _context.SesionParticipante.Add(_sesionParticipante);
                    //     }
                    // }
                }


                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = _sesionParticipante,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, sesionParticipante.UsuarioCreacion, strCrearEditar)

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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, sesionParticipante.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }




        #endregion

        #region "Gestion de actas";
        //Grilla sesion comite, => sesiones desarrolladas sin actas.
        public async Task<List<ComiteTecnico>> GetSesionSinActa()
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
                                  EstadoComiteCodigo = n.EstadoComiteCodigo == "3" ? "Sin acta" : n.EstadoComiteCodigo == "4" ? "En proceso de aprobación" : n.EstadoComiteCodigo == "5" ? "Aprobada" : "Devuelta",
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
        //public async Task<ActionResult<IEnumerable<GridCommitteeSession>>> GetCommitteeSession(int? comiteTecnicoId)
        //{

        //    List<ComiteTecnico> ListSesion = (comiteTecnicoId != null ? await _context.ComiteTecnico.Where(s => s.ComiteTecnicoId == comiteTecnicoId && (bool)!s.Eliminado).ToListAsync() : await _context.ComiteTecnico.Where(s => (bool)!s.Eliminado).ToListAsync());

        //    List<GridCommitteeSession> ListGridCommitteeSession = new List<GridCommitteeSession>();


        //    foreach (var ss in ListSesion)
        //    {
        //        GridCommitteeSession SesionGrid = new GridCommitteeSession
        //        {
        //            ComiteTecnicoId = ss.ComiteTecnicoId,
        //            FechaDeComite = ss.FechaOrdenDia,
        //            EstadoComiteCodigo = ss.EstadoComiteCodigo,
        //            NumeroComite = ss.NumeroComite,
        //            EstadoComiteText = ss.EstadoComiteCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(ss.EstadoComiteCodigo, (int)EnumeratorTipoDominio.Estado_Comite) : "",
        //        };
        //        ListGridCommitteeSession.Add(SesionGrid);
        //    }

        //    return ListGridCommitteeSession;
        //}


        // Ver detalle
        public async Task<IEnumerable<GridCommitteeSession>> GetCommitteeSessionTemaById(int sessionTemaId)
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
        public async Task<List<dynamic>> GetCommitteeSessionFiduciario()
        {
            List<dynamic> listaSolicitudesGrilla = new List<dynamic>();
            List<dynamic> listaComitesGrilla = new List<dynamic>();

            try
            {
                List<ComiteTecnico> listaComites = await _context.ComiteTecnico.Where(ct => ct.EsComiteFiduciario == null || ct.EsComiteFiduciario == false)
                                                                                .Include(r => r.SesionComiteSolicitudComiteTecnico)
                                                                                .ToListAsync(); //Aprobadas

                List<Dominio> ListTipoSolicitud = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Solicitud).ToList();

                listaComites.ForEach(c =>
                {
                    dynamic comite = new { nombreSesion = c.NumeroComite, fecha = c.FechaOrdenDia, data = new List<dynamic>() };
                    foreach (var ss in c.SesionComiteSolicitudComiteTecnico)
                    {
                        if ( ss.EstadoCodigo == "1" )
                        switch (ss.TipoSolicitudCodigo)
                        {
                            case ConstanCodigoTipoSolicitud.Contratacion:
                                {
                                    Contratacion contratacion = _context.Contratacion.Find(ss.SolicitudId);

                                    if (contratacion != null)
                                        comite.data.Add(new
                                        {
                                            Id = contratacion.ContratacionId,
                                            FechaSolicitud = contratacion.FechaTramite.HasValue ? (DateTime?)contratacion.FechaTramite.Value : null,
                                            NumeroSolicitud = contratacion.NumeroSolicitud,
                                            TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion).FirstOrDefault().Nombre,
                                            tipoSolicitudNumeroTabla = ConstanCodigoTipoSolicitud.Contratacion
                                        });
                                    break;
                                }

                            case ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion:
                                {
                                    ProcesoSeleccion procesoSeleccion = _context.ProcesoSeleccion.Find(ss.SolicitudId);

                                    if (procesoSeleccion != null)
                                        comite.data.Add(new
                                        {
                                            Id = procesoSeleccion.ProcesoSeleccionId,
                                            FechaSolicitud = (DateTime?)(procesoSeleccion.FechaCreacion),
                                            NumeroSolicitud = procesoSeleccion.NumeroProceso,
                                            TipoSolicitud = ListTipoSolicitud.Where(r => r.Codigo == ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion).FirstOrDefault().Nombre,
                                            tipoSolicitudNumeroTabla = ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion
                                        });

                                    break;
                                }
                        }
                    }
                    if ( comite.data.Count > 0 )
                        listaComitesGrilla.Add( comite );

                });



                return listaComitesGrilla;
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
            int idAccion = 0; //await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Sesion_invitado, (int)EnumeratorTipoDominio.Acciones);

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
            int idAccion = 0; //await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Sesion_comentario, (int)EnumeratorTipoDominio.Acciones);

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
            int idAccion = 0; //await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Tema_Compromiso, (int)EnumeratorTipoDominio.Acciones);

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

        private bool ValidarRegistroCompletoSesionComiteSolicitud(SesionComiteSolicitud sesionComiteSolicitud)
        {
            if (
               sesionComiteSolicitud.RutaSoporteVotacion == null ||
               sesionComiteSolicitud.GeneraCompromiso == null ||
               sesionComiteSolicitud.RequiereVotacion == null ||
               //sesionComiteSolicitud.ComiteTecnicoFiduciarioId > 0 ||
               //sesionComiteSolicitud.FechaComiteFiduciario == null ||
               //sesionComiteSolicitud.UsuarioComiteFiduciario == null ||
               //sesionComiteSolicitud.EstadoActaCodigo == null ||
               sesionComiteSolicitud.EstadoCodigo == null ||
               string.IsNullOrEmpty(sesionComiteSolicitud.Observaciones) ||
               string.IsNullOrEmpty(sesionComiteSolicitud.RutaSoporteVotacion)
                )
            {
                return false;
            }
            return true;
        }

        private bool ValidarRegistroCompletoSesionComiteTema(SesionComiteTema sesionComiteTemaOld)
        {
            if (string.IsNullOrEmpty(sesionComiteTemaOld.Tema)
                || string.IsNullOrEmpty(sesionComiteTemaOld.ResponsableCodigo)
                || string.IsNullOrEmpty(sesionComiteTemaOld.TiempoIntervencion.ToString())
                //|| !string.IsNullOrEmpty(sesionComiteTemaOld.RutaSoporte)
                || string.IsNullOrEmpty(sesionComiteTemaOld.Observaciones)
                || sesionComiteTemaOld.EsAprobado == null
                || sesionComiteTemaOld.RequiereVotacion == null
                //|| sesionComiteTemaOld.EsProposicionesVarios == null
                || sesionComiteTemaOld.GeneraCompromiso == null
                || string.IsNullOrEmpty(sesionComiteTemaOld.ObservacionesDecision)
                || sesionComiteTemaOld.EstadoTemaCodigo == null

                )
            {

                return false;
            }

            return true;

        }

        public static bool ValidarCamposComiteTecnico(ComiteTecnico pComiteTecnico)
        {
            if (
                    pComiteTecnico.RequiereVotacion == null ||
                    pComiteTecnico.RequiereVotacion == null ||
                    string.IsNullOrEmpty(pComiteTecnico.Justificacion) ||
                    pComiteTecnico.EsAprobado == null ||
                    string.IsNullOrEmpty(pComiteTecnico.FechaAplazamiento.ToString()) ||
                    string.IsNullOrEmpty(pComiteTecnico.Observaciones) ||
                    string.IsNullOrEmpty(pComiteTecnico.RutaSoporteVotacion) ||
                    pComiteTecnico.TieneCompromisos == null ||
                    string.IsNullOrEmpty(pComiteTecnico.CantCompromisos.ToString()) ||
                    string.IsNullOrEmpty(pComiteTecnico.RutaActaSesion) ||
                    string.IsNullOrEmpty(pComiteTecnico.FechaOrdenDia.ToString()) ||
                    string.IsNullOrEmpty(pComiteTecnico.NumeroComite) ||
                    string.IsNullOrEmpty(pComiteTecnico.EstadoComiteCodigo)
                )
            {
                return false;
            }
            return true;
        }

        //Crear orden del dia de comité fiduciario
        public async Task<Respuesta> CreateEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud(ComiteTecnico pComiteTecnico)
        {
            int idAccionCrearComiteTecnico = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Comite_Fiduciario_SesionComiteSolicitud_SesionComiteTema, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                string strCreateEdit;
                if (pComiteTecnico.ComiteTecnicoId == 0)
                {
                    //Agregar Tema Proposiciones y Varios
                    pComiteTecnico.SesionComiteTema.Add(
                           new SesionComiteTema
                           {
                               Eliminado = false,
                               UsuarioCreacion = pComiteTecnico.UsuarioCreacion,
                               FechaCreacion = DateTime.Now,
                               EsProposicionesVarios = true,
                               Tema = "",

                           });

                    strCreateEdit = "CREAR COMITE FIDUCIARIO  + SESIÓN COMITE SOLICITUD + SESIÓN COMITE TEMA";
                    //Auditoria
                    pComiteTecnico.FechaCreacion = DateTime.Now;
                    pComiteTecnico.Eliminado = false;
                    pComiteTecnico.EsComiteFiduciario = true;
                    //Registros
                    pComiteTecnico.EsCompleto = ValidarCamposComiteTecnico(pComiteTecnico);
                    pComiteTecnico.EsComiteFiduciario = true;
                    
                    pComiteTecnico.EstadoComiteCodigo = ConstanCodigoEstadoComite.Sin_Convocatoria;
                    pComiteTecnico.NumeroComite = await _commonService.EnumeradorComiteFiduciario();


                    foreach (var SesionComiteTema in pComiteTecnico.SesionComiteTema)
                    {
                        //Auditoria
                        SesionComiteTema.FechaCreacion = DateTime.Now;
                        SesionComiteTema.UsuarioCreacion = pComiteTecnico.UsuarioCreacion;
                        SesionComiteTema.RegistroCompleto = ValidarRegistroCompletoSesionComiteTema(SesionComiteTema);
                        SesionComiteTema.Eliminado = false;
                    }
                    foreach (var SesionComiteSolicitud in pComiteTecnico.SesionComiteSolicitudComiteTecnico)
                    {
                        //Auditoria
                        SesionComiteSolicitud.FechaCreacion = DateTime.Now;
                        SesionComiteSolicitud.UsuarioCreacion = pComiteTecnico.UsuarioCreacion;
                        SesionComiteSolicitud.ComiteTecnicoFiduciarioId = pComiteTecnico.ComiteTecnicoId;
                        SesionComiteSolicitud.RegistroCompleto = ValidarRegistroCompletoSesionComiteSolicitud(SesionComiteSolicitud);
                        SesionComiteSolicitud.Eliminado = false;
                    }
                    _context.ComiteTecnico.Add(pComiteTecnico);
                }
                else
                {
                    strCreateEdit = "EDITAR COMITE TECNICO  + SESIÓN COMITE SOLICITUD + SESIÓN COMITE TEMA";

                    ComiteTecnico comiteTecnicoOld = _context.ComiteTecnico
                        .Where(r => r.ComiteTecnicoId == pComiteTecnico.ComiteTecnicoId)
                            .Include(r => r.SesionComiteSolicitudComiteTecnico)
                            .Include(r => r.SesionComiteTema).FirstOrDefault();

                    //Auditoria 
                    comiteTecnicoOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                    comiteTecnicoOld.FechaModificacion = DateTime.Now;

                    //Registros
                    comiteTecnicoOld.EsCompleto = ValidarCamposComiteTecnico(comiteTecnicoOld);
                    comiteTecnicoOld.RequiereVotacion = comiteTecnicoOld.RequiereVotacion;
                    comiteTecnicoOld.Justificacion = comiteTecnicoOld.Justificacion;
                    comiteTecnicoOld.EsAprobado = comiteTecnicoOld.EsAprobado;
                    comiteTecnicoOld.FechaAplazamiento = comiteTecnicoOld.FechaAplazamiento;
                    comiteTecnicoOld.Observaciones = comiteTecnicoOld.Observaciones;
                    comiteTecnicoOld.RutaSoporteVotacion = comiteTecnicoOld.RutaSoporteVotacion;
                    comiteTecnicoOld.TieneCompromisos = comiteTecnicoOld.TieneCompromisos;
                    comiteTecnicoOld.CantCompromisos = comiteTecnicoOld.CantCompromisos;
                    comiteTecnicoOld.RutaActaSesion = comiteTecnicoOld.RutaActaSesion;
                    comiteTecnicoOld.FechaOrdenDia = comiteTecnicoOld.FechaOrdenDia;
                    comiteTecnicoOld.NumeroComite = comiteTecnicoOld.NumeroComite;
                    comiteTecnicoOld.EstadoComiteCodigo = comiteTecnicoOld.EstadoComiteCodigo;

                    foreach (var SesionComiteTema in pComiteTecnico.SesionComiteTema)
                    {
                        if (SesionComiteTema.SesionTemaId == 0)
                        {

                            //Auditoria 
                            SesionComiteTema.UsuarioCreacion = pComiteTecnico.UsuarioCreacion;
                            SesionComiteTema.FechaCreacion = DateTime.Now;
                            SesionComiteTema.Eliminado = false;
                            SesionComiteTema.RegistroCompleto = ValidarRegistroCompletoSesionComiteTema(SesionComiteTema);
                            //Registros
                            SesionComiteTema.ComiteTecnicoId = pComiteTecnico.ComiteTecnicoId;
                            _context.SesionComiteTema.Add(SesionComiteTema);
                        }
                        else
                        {
                            SesionComiteTema sesionComiteTemaOld = _context.SesionComiteTema.Find(SesionComiteTema.SesionTemaId);
                            //Auditoria 
                            sesionComiteTemaOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                            sesionComiteTemaOld.FechaModificacion = DateTime.Now;

                            //Registros
                            sesionComiteTemaOld.Tema = SesionComiteTema.Tema;
                            sesionComiteTemaOld.ResponsableCodigo = SesionComiteTema.ResponsableCodigo;
                            sesionComiteTemaOld.TiempoIntervencion = SesionComiteTema.TiempoIntervencion;
                            sesionComiteTemaOld.RutaSoporte = SesionComiteTema.RutaSoporte;
                            sesionComiteTemaOld.Observaciones = SesionComiteTema.Observaciones;
                            sesionComiteTemaOld.EsAprobado = SesionComiteTema.EsAprobado;
                            sesionComiteTemaOld.ObservacionesDecision = SesionComiteTema.Observaciones;
                            sesionComiteTemaOld.EsProposicionesVarios = SesionComiteTema.EsProposicionesVarios;
                            sesionComiteTemaOld.RegistroCompleto = ValidarRegistroCompletoSesionComiteTema(sesionComiteTemaOld);
                        }
                    }

                    foreach (var SesionComiteSolicitud in pComiteTecnico.SesionComiteSolicitudComiteTecnico)
                    {
                        {
                            SesionComiteSolicitud SesionComiteSolicitudOld = _context.SesionComiteSolicitud.Find(SesionComiteSolicitud.SesionComiteSolicitudId);
                            //Auditoria 
                            SesionComiteSolicitudOld.UsuarioModificacion = pComiteTecnico.UsuarioCreacion;
                            SesionComiteSolicitudOld.FechaModificacion = DateTime.Now;

                            //Registros
                            SesionComiteSolicitudOld.ComiteTecnicoFiduciarioId = pComiteTecnico.ComiteTecnicoId;
                            SesionComiteSolicitudOld.TipoSolicitudCodigo = SesionComiteSolicitud.TipoSolicitudCodigo;
                            SesionComiteSolicitudOld.SolicitudId = SesionComiteSolicitud.SolicitudId;
                            SesionComiteSolicitudOld.EstadoCodigo = SesionComiteSolicitud.EstadoCodigo;
                            SesionComiteSolicitudOld.Observaciones = SesionComiteSolicitud.Observaciones;
                            SesionComiteSolicitudOld.RutaSoporteVotacion = SesionComiteSolicitud.RutaSoporteVotacion;
                            SesionComiteSolicitudOld.GeneraCompromiso = SesionComiteSolicitud.GeneraCompromiso;
                            SesionComiteSolicitudOld.CantCompromisos = SesionComiteSolicitud.CantCompromisos;
                            SesionComiteSolicitudOld.RegistroCompleto = ValidarRegistroCompletoSesionComiteSolicitud(SesionComiteSolicitudOld);
                        }
                    }
                }
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteFiduciario.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.OperacionExitosa, idAccionCrearComiteTecnico, pComiteTecnico.UsuarioCreacion, strCreateEdit)
                };
            }
            catch (Exception ex)
            {
                return
                    new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = true,
                        IsValidation = false,
                        Code = ConstantSesionComiteTecnico.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarComiteTecnico, ConstantSesionComiteTecnico.Error, idAccionCrearComiteTecnico, pComiteTecnico.UsuarioCreacion, ex.InnerException.ToString())
                    };
            }
        }


        //Registrar participantes
        public async Task<Respuesta> CreateOrEditParticipantes(SesionComiteTema sesionComiteTema)
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

        #endregion





    }
}
