using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
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


        #region "Sesion Comite Tema";


        //Grilla sesion comite
        public async Task<ActionResult<IEnumerable<GridCommitteeSession>>> GetCommitteeSession(int? sessionId)
        {
           
            List<Sesion> ListSesion = (sessionId != null ? await _context.Sesion.Where(s => s.SesionId == sessionId && !s.Eliminado).Include(s => s.SesionComiteTema).Where(s => s.SesionId == sessionId && !s.Eliminado).ToListAsync() : await _context.Sesion.Where(s => !s.Eliminado).ToListAsync());

            List<GridCommitteeSession> ListGridCommitteeSession = new List<GridCommitteeSession>();


            foreach (var ss in ListSesion)
            {
                GridCommitteeSession SesionGrid = new GridCommitteeSession
                {
                    SesionId = ss.SesionId,
                    FechaDeComite = ss.FechaOrdenDia,
                    EstadoComiteCodigo = ss.EstadoComiteCodigo,
                    NumeroComite = ss.NumeroComite,

                    //TODO: Hacer la validacion del estado.
                    EstadoComiteText = ss.EstadoComiteCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(ss.EstadoComiteCodigo, (int)EnumeratorTipoDominio.Estado_Comite) : "",
                };
                ListGridCommitteeSession.Add(SesionGrid);
            }

            return ListGridCommitteeSession;
        }


        // Ver detalle
        public async Task<ActionResult<IEnumerable<GridCommitteeSession>>> GetCommitteeSessionTemaById(int sessionTemaId)
        {

            List<SesionComiteTema> ListSesionComiteTema = await _context.SesionComiteTema.Where(s => s.SesionTemaId == sessionTemaId && !s.Eliminado).ToListAsync();

            List<GridCommitteeSession> ListGridsesionComiteTema = new List<GridCommitteeSession>();


            foreach (var st in ListSesionComiteTema)
            {
                GridCommitteeSession sesionComiteTemaGrid = new GridCommitteeSession
                {
                    SesionId = st.SesionId,
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



        //Grilla sesion comite fiduciario en estado convocada.
        public async Task<ActionResult<List<GridCommitteeSession>>> GetCommitteeSessionFiduciario()
        {
            try
            {
                List<Sesion> ListSesion = await _context.Sesion.Where(s => s.EstadoComiteCodigo == "2").ToListAsync();
                List<GridCommitteeSession> ListGridCommitteeSessionFiduciario = new List<GridCommitteeSession>();

                foreach (var ss in ListSesion)
                {
                    GridCommitteeSession SesionGrid = new GridCommitteeSession
                    {
                        SesionId = ss.SesionId,
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


        //RegistrarSesion
        public async Task RegistrarSesion()
        { 
        
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

                    sesionComiteTemaAntiguo.SesionId = sesionComiteTema.SesionId;
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




        //Eliminar tema
        public async Task<bool> DeleteTema(int temaId)
        {
            SesionComiteTema tema = await _context.SesionComiteTema.FindAsync(temaId);
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
