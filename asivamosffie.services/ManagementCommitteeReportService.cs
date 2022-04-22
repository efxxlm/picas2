
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;
using Z.Expressions;

namespace asivamosffie.services
{
    /*
      PARAMETRICAS
        1. Estado Compromisos -> TipoDominioId = 45 { Sin iniciar, En proceso, Finalizado}
    */

    public class ManagementCommitteeReportService : IManagementCommitteeReportService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly ICommitteeSessionFiduciarioService _committeeSessionFiduciarioService;
        private readonly IRegisterSessionTechnicalCommitteeService _registerSessionTechnicalCommitteeService;
        private bool ReturnValue { get; set; }

        public ManagementCommitteeReportService(IRegisterSessionTechnicalCommitteeService registerSessionTechnicalCommitteeService, ICommitteeSessionFiduciarioService committeeSessionFiduciarioService, devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _registerSessionTechnicalCommitteeService = registerSessionTechnicalCommitteeService;
            _committeeSessionFiduciarioService = committeeSessionFiduciarioService;
            _context = context;
            _commonService = commonService;
        }

        public Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
        {
            return _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public async Task<ActionResult<List<GrillaSesionComiteTecnicoCompromiso>>> GetManagementCommitteeReport(int pUserId)
        {
            List<GrillaSesionComiteTecnicoCompromiso> grillaSesionComiteTecnicoCompromisos = new List<GrillaSesionComiteTecnicoCompromiso>();

            string StrSql = "SELECT ComiteTecnico.* FROM  dbo.ComiteTecnico INNER JOIN dbo.SesionParticipante  ON   ComiteTecnico.ComiteTecnicoId = SesionParticipante.ComiteTecnicoId WHERE  SesionParticipante.UsuarioId = " + pUserId + " AND   ComiteTecnico.Eliminado = 0 AND  SesionParticipante.Eliminado = 0";
            List<ComiteTecnico> ListComiteTecnico = await _context.ComiteTecnico.FromSqlRaw(StrSql)
               .Where(r => r.EstadoActaCodigo == ConstantCodigoActas.Aprobada
                      && r.EstadoComiteCodigo == ConstanCodigoEstadoComite.Con_Acta_De_Sesion_Enviada)
                .Include(r => r.SesionParticipante)
                .Include(r => r.SesionComentario)
                .Include(r => r.SesionComiteSolicitudComiteTecnico)
                      .ThenInclude(r => r.SesionSolicitudCompromiso)
                .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                     .ThenInclude(r => r.SesionSolicitudCompromiso)
                .Include(r => r.SesionComiteTema)
                    .ThenInclude(r => r.TemaCompromiso)

                .Distinct()
                .ToListAsync();

            foreach (var ComiteTecnico in ListComiteTecnico)
            {

                foreach (var SesionComiteSolicitudComiteTecnico in ComiteTecnico.SesionComiteSolicitudComiteTecnico)
                {

                    foreach (var SesionSolicitudCompromiso in SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso)
                    {
                        GrillaSesionComiteTecnicoCompromiso grillaSesionComiteTecnicoCompromiso = new GrillaSesionComiteTecnicoCompromiso
                        {
                            ComiteTecnicoId = ComiteTecnico.ComiteTecnicoId,
                            FechaComite = ComiteTecnico.FechaOrdenDia,
                            NumeroComite = ComiteTecnico.NumeroComite
                        };

                        grillaSesionComiteTecnicoCompromiso.SesionComiteTecnicoCompromisoId = SesionSolicitudCompromiso.SesionSolicitudCompromisoId;
                        grillaSesionComiteTecnicoCompromiso.Compromiso = SesionSolicitudCompromiso.Tarea;
                        grillaSesionComiteTecnicoCompromiso.FechaCumplimiento = SesionSolicitudCompromiso.FechaCumplimiento;
                        grillaSesionComiteTecnicoCompromiso.EstadoCodigo = string.IsNullOrEmpty(SesionSolicitudCompromiso.EstadoCodigo) ? "1" : SesionSolicitudCompromiso.EstadoCodigo;
                        grillaSesionComiteTecnicoCompromisos.Add(grillaSesionComiteTecnicoCompromiso);

                    }
                }
            }
            return grillaSesionComiteTecnicoCompromisos.OrderByDescending(r => r.ComiteTecnicoId).ToList();
        }

        //Lista Compromisos temas y solicitudes
        public async Task<List<dynamic>> GetListCompromisosOLD(int pUserId)
        {
            List<dynamic> ListDynamic = new List<dynamic>();
            string StrSql = "SELECT ComiteTecnico.* FROM  dbo.ComiteTecnico INNER JOIN dbo.SesionParticipante  ON   ComiteTecnico.ComiteTecnicoId = SesionParticipante.ComiteTecnicoId WHERE  SesionParticipante.UsuarioId = " + pUserId + " AND   ComiteTecnico.Eliminado = 0 AND  SesionParticipante.Eliminado = 0";
            List<ComiteTecnico> ListComiteTecnico = await _context.ComiteTecnico.FromSqlRaw(StrSql)
               .Where(r => r.EstadoActaCodigo == ConstantCodigoActas.Aprobada)
                //  && r.EstadoComiteCodigo == ConstanCodigoEstadoComite.Con_Acta_De_Sesion_Enviada)
                .Include(r => r.SesionParticipante)
                .Include(r => r.SesionComentario)
                .Include(r => r.SesionComiteSolicitudComiteTecnico)
                      .ThenInclude(r => r.SesionSolicitudCompromiso)
                         .ThenInclude(r => r.ResponsableSesionParticipante)
                          .ThenInclude(r => r.Usuario)
                .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                     .ThenInclude(r => r.SesionSolicitudCompromiso)
                       .ThenInclude(r => r.ResponsableSesionParticipante)
                          .ThenInclude(r => r.Usuario)
                .Include(r => r.SesionComiteTema)
                    .ThenInclude(r => r.TemaCompromiso)
                        .ThenInclude(r => r.ResponsableNavigation)
                                                .ToListAsync();

            foreach (var ComiteTecnico in ListComiteTecnico.ToList().OrderByDescending(r => r.ComiteTecnicoId))
            {
                foreach (var SesionComiteSolicitudComiteTecnico in ComiteTecnico.SesionComiteSolicitudComiteTecnico.Where(r => !(bool)r.Eliminado).ToList().OrderByDescending(r => r.SesionComiteSolicitudId))
                {
                    foreach (var SesionSolicitudCompromiso in SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso.Where(r => r.Eliminado != true && r.ResponsableSesionParticipante.UsuarioId == pUserId).ToList().OrderByDescending(r => r.SesionSolicitudCompromisoId))
                    {
                        ListDynamic.Add(new
                        {
                            FechaComite = ComiteTecnico.FechaOrdenDia,
                            ComiteTecnico.NumeroComite,
                            Compromiso = SesionSolicitudCompromiso.Tarea,
                            SesionSolicitudCompromiso.EstadoCodigo,
                            TipoSolicitud = ConstanCodigoTipoCompromisos.CompromisosSolicitud,
                            SesionSolicitudCompromiso.FechaCumplimiento,
                            CompromisoId = SesionSolicitudCompromiso.SesionSolicitudCompromisoId
                        });
                    }
                }
                foreach (var SesionComiteSolicitudComiteTecnico in ComiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario.Where(r => r.Eliminado != true).ToList().OrderByDescending(r => r.SesionComiteSolicitudId))
                {
                    foreach (var SesionSolicitudCompromiso in SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso.Where(r => r.Eliminado != true && r.ResponsableSesionParticipante.UsuarioId == pUserId).ToList().OrderByDescending(r => r.SesionSolicitudCompromisoId))
                    {
                        ListDynamic.Add(new
                        {
                            FechaComite = ComiteTecnico.FechaOrdenDia,
                            ComiteTecnico.NumeroComite,
                            Compromiso = SesionSolicitudCompromiso.Tarea,
                            SesionSolicitudCompromiso.EstadoCodigo,
                            TipoSolicitud = ConstanCodigoTipoCompromisos.CompromisosSolicitud,
                            SesionSolicitudCompromiso.FechaCumplimiento,
                            CompromisoId = SesionSolicitudCompromiso.SesionSolicitudCompromisoId
                        });
                    }
                }


                foreach (var SesionComiteTema in ComiteTecnico.SesionComiteTema.Where(r => !(bool)r.Eliminado).ToList().OrderByDescending(r => r.SesionTemaId))
                {
                    foreach (var TemaCompromiso in SesionComiteTema.TemaCompromiso.Where(r => !(bool)r.Eliminado && r.ResponsableNavigation.UsuarioId == pUserId).ToList().OrderByDescending(r => r.TemaCompromisoId))
                    {
                        ListDynamic.Add(new
                        {
                            FechaComite = ComiteTecnico.FechaOrdenDia,
                            ComiteTecnico.NumeroComite,
                            Compromiso = TemaCompromiso.Tarea,
                            TemaCompromiso.EstadoCodigo,
                            TipoSolicitud = ConstanCodigoTipoCompromisos.CompromisosTema,
                            TemaCompromiso.FechaCumplimiento,
                            CompromisoId = TemaCompromiso.TemaCompromisoId
                        });
                    }
                }
            }

            return ListDynamic.OrderByDescending(r => r.CompromisoId).ToList();
        }

        //Lista Compromisos temas y solicitudes optimizada
        public async Task<List<dynamic>> GetListCompromisos(int pUserId)
        {
            List<dynamic> ListDynamic = new List<dynamic>();
            ListDynamic.AddRange(_context.VListCompromisosComiteTecnico.Where(r => r.UsuarioId == pUserId));
            ListDynamic.AddRange(_context.VListCompromisosTemas.Where(r => r.UsuarioId == pUserId));

            return ListDynamic;
        }


        //Detalle gestion compromisos
        public async Task<ActionResult<List<GrillaSesionComiteTecnicoCompromiso>>> GetManagementCommitteeReportById(int sesionComiteTecnicoCompromisoId)
        {
            try
            {
                //Actualizado
                return await (from cs in _context.CompromisoSeguimiento
                              join stc in _context.SesionComiteTecnicoCompromiso on cs.SesionComiteTecnicoCompromisoId equals stc.SesionComiteTecnicoCompromisoId

                              where cs.SesionComiteTecnicoCompromisoId == sesionComiteTecnicoCompromisoId
                              select new GrillaSesionComiteTecnicoCompromiso
                              {

                                  SesionComiteTecnicoCompromisoId = stc.SesionComiteTecnicoCompromisoId,
                                  DescripcionSeguimiento = cs.DescripcionSeguimiento,
                                  FechaRegistroAvanceCompromiso = cs.FechaCreacion,
                                  EstadoCodigo = stc.EstadoCodigo

                              }).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        //Gestion de actas
        public async Task<ActionResult<List<ComiteTecnico>>> GetManagementReport(int pUserId)
        {
            List<ComiteTecnico> ListComiteTecnico =
                       await _context.ComiteTecnico
                       .Include(s => s.SesionParticipante)
                       .Where(s => s.SesionParticipante.Any(s => s.UsuarioId == pUserId && s.ComiteTecnico.Eliminado != true && s.Eliminado != true))
                       .Include(r => r.SesionComiteTema)
                          .ThenInclude(r => r.TemaCompromiso)
                      .OrderByDescending(r => r.ComiteTecnicoId)
                      .Distinct()
                      .ToListAsync();

            List<SesionComentario> ListSesionComentario = _context.SesionComentario.ToList();


            foreach (var l in ListComiteTecnico)
            {
                l.SesionComentario = l.SesionComentario = ListSesionComentario.Where(s => s.ComiteTecnicoId == l.ComiteTecnicoId).ToList();
                l.esVotoAprobado = l.SesionComentario.Count(r => r.MiembroSesionParticipanteId == pUserId && r.ValidacionVoto == false && (r.EstadoActaVoto == ConstantCodigoActas.Aprobada || r.EstadoActaVoto == ConstantCodigoActas.Devuelta)) > 0;
            }

            return ListComiteTecnico;
        }

        //Detalle gestion de actas
        public async Task<List<ComiteTecnico>> GetManagementReportById(int comiteTecnicoId)
        {
            try
            {
                ComiteTecnico item = await _context.ComiteTecnico
                                                                    .Where(r => r.ComiteTecnicoId == comiteTecnicoId)

                                                                        //.Include(r => r.SesionComiteTema)
                                                                        //    .ThenInclude(r => r.TemaCompromiso)
                                                                        //.Include(r => r.SesionComiteTema)
                                                                        //    .ThenInclude(r => r.SesionTemaVoto) 
                                                                        .AsNoTracking()
                                                                        .Include(r => r.SesionComiteTecnicoCompromiso)
                                                                            .ThenInclude(r => r.CompromisoSeguimiento)
                                                                        .Include(r => r.SesionComiteSolicitudComiteTecnico)
                                                                            .ThenInclude(r => r.SesionSolicitudVoto)
                                                                        .Include(r => r.SesionComiteSolicitudComiteTecnico)
                                                                            .ThenInclude(r => r.SesionSolicitudCompromiso)
                                                                                    .ThenInclude(r => r.ResponsableSesionParticipante)
                                                                                        .ThenInclude(r => r.Usuario)
                                                                        .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                                                                            .ThenInclude(r => r.SesionSolicitudCompromiso)
                                                                                .ThenInclude(r => r.ResponsableSesionParticipante)
                                                                                            .ThenInclude(r => r.Usuario)
                                                                        .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                                                                            .ThenInclude(r => r.SesionSolicitudVoto)
                                                                        .AsNoTracking()
                                                                        .FirstOrDefaultAsync();

                item.SesionComentario = _context.SesionComentario.AsNoTracking().Where(s => s.ComiteTecnicoId == item.ComiteTecnicoId).ToList();

                item.SesionComiteTema = _context.SesionComiteTema.AsNoTracking().Where(r => r.ComiteTecnicoId == comiteTecnicoId && r.Eliminado != true).ToList();

                foreach (var SesionComiteTema in item.SesionComiteTema)
                {
                    SesionComiteTema.ComiteTecnico = null;

                    SesionComiteTema.TemaCompromiso = _context.TemaCompromiso.AsNoTracking().Where(r => r.SesionTemaId == SesionComiteTema.SesionTemaId && SesionComiteTema.Eliminado != true).ToList();
                    SesionComiteTema.SesionTemaVoto = _context.SesionTemaVoto.AsNoTracking().Where(r => r.SesionTemaId == SesionComiteTema.SesionTemaId && SesionComiteTema.Eliminado != true).ToList();

                }


                List<Dominio> ListParametricas = _context.Dominio.AsNoTracking().ToList();
                List<Contratacion> ListContratacion = _context.Contratacion.AsNoTracking().ToList();
                List<ProcesoSeleccion> ListProcesosSelecicon = _context.ProcesoSeleccion.AsNoTracking().ToList();
                List<ControversiaContractual> ListControversiasContractuales = _context.ControversiaContractual.AsNoTracking().ToList();
                List<NovedadContractual> ListModificacionesContractuales = _context.NovedadContractual.AsNoTracking().ToList();
                List<SesionParticipanteVoto> ListParticipanteVotos = _context.SesionParticipanteVoto.AsNoTracking().ToList();
                List<ControversiaActuacion> ListControversiaActuacion = _context.ControversiaActuacion.AsNoTracking().ToList();
                List<DefensaJudicial> ListDefensaJudicial = _context.DefensaJudicial.AsNoTracking().ToList();
                List<ProcesoSeleccionMonitoreo> ListProcesoSeleccionMonitoreo = _context.ProcesoSeleccionMonitoreo.AsNoTracking().ToList();




                bool BorrarCompromisosFiduciarios = item.EsComiteFiduciario ?? false;

                List<VSesionParticipante> listaParticipantes = _context.VSesionParticipante.AsNoTracking().Where(r => r.ComiteTecnicoId == item.ComiteTecnicoId).ToList();
                item.SesionParticipanteView = listaParticipantes;
                if (item.SesionComiteTecnicoCompromiso.Count() > 0)
                    item.SesionComiteTecnicoCompromiso = item.SesionComiteTecnicoCompromiso.Where(r => !(bool)r.Eliminado).ToList();


                foreach (var SesionComiteTema in item.SesionComiteTema)
                {
                    if (SesionComiteTema.TemaCompromiso.Count() > 0)
                        SesionComiteTema.TemaCompromiso = SesionComiteTema.TemaCompromiso.Where(r => !(bool)r.Eliminado).ToList();

                    if (!string.IsNullOrEmpty(SesionComiteTema.ResponsableCodigo))
                        SesionComiteTema.ResponsableCodigo = ListParametricas
                            .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Miembros_Comite_Tecnico && r.Codigo == SesionComiteTema.ResponsableCodigo)
                            .FirstOrDefault().Nombre;

                    if (!string.IsNullOrEmpty(SesionComiteTema.EstadoTemaCodigo))
                        SesionComiteTema.EstadoTemaCodigo = ListParametricas
                            .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Sesion_Comite_Solicitud && r.Codigo == SesionComiteTema.EstadoTemaCodigo)
                            .FirstOrDefault().Nombre;

                    foreach (var tc in SesionComiteTema.TemaCompromiso)
                    {
                        SesionParticipante participante = new SesionParticipante
                        {
                            Usuario = new Usuario()
                        };

                        VSesionParticipante vSesionParticipante =
                                                                listaParticipantes.Where(r => r.SesionParticipanteId == tc.Responsable)
                                                                                  .FirstOrDefault();

                        if (vSesionParticipante != null)
                        {
                            participante.SesionParticipanteId = vSesionParticipante.SesionParticipanteId;
                            participante.ComiteTecnicoId = vSesionParticipante.ComiteTecnicoId;
                            participante.UsuarioId = vSesionParticipante.UsuarioId;
                            participante.Eliminado = vSesionParticipante.Eliminado;

                            participante.Usuario.UsuarioId = vSesionParticipante.UsuarioId;
                            participante.Usuario.PrimerNombre = vSesionParticipante.Nombres;
                            participante.Usuario.PrimerApellido = vSesionParticipante.Apellidos;
                            participante.Usuario.NumeroIdentificacion = vSesionParticipante.NumeroIdentificacion;
                            participante.esAprobado = ListParticipanteVotos
                                                                           .Where(s => s.ComiteTecnicoId == item.ComiteTecnicoId
                                                                               && s.SesionParticipanteId == participante.SesionParticipanteId).Select(r => r.EsAprobado)
                                                                           .LastOrDefault();
                            tc.ResponsableNavigation = participante;
                        }
                    }
                }

                foreach (var SesionComiteSolicitudComiteTecnico in item.SesionComiteSolicitudComiteTecnico)
                {
                    //if (BorrarCompromisosFiduciarios && SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso.Count() > 0)
                    //    SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso = null;

                    if (SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso.Count() > 0)
                        SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso = SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso.Where(s => s.Eliminado != true && s.EsFiduciario == false).ToList();

                    if (!string.IsNullOrEmpty(SesionComiteSolicitudComiteTecnico.EstadoCodigo))
                    {
                        SesionComiteSolicitudComiteTecnico.EstadoCodigo = ListParametricas
                                .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud
                                && r.Codigo == SesionComiteSolicitudComiteTecnico.EstadoCodigo).FirstOrDefault().Nombre;
                    }

                    ///TIPOS DE SOLICITUD
                    if (SesionComiteSolicitudComiteTecnico.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion)
                    {
                        if (SesionComiteSolicitudComiteTecnico.SolicitudId > 0)
                        {
                            SesionComiteSolicitudComiteTecnico.ProcesoSeleccion = ListProcesosSelecicon.Where(r => r.ProcesoSeleccionId == SesionComiteSolicitudComiteTecnico.SolicitudId).FirstOrDefault();
                        }
                    }
                    if (SesionComiteSolicitudComiteTecnico.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion)
                    {
                        SesionComiteSolicitudComiteTecnico.Contratacion = ListContratacion.Where(r => r.ContratacionId == SesionComiteSolicitudComiteTecnico.SolicitudId).FirstOrDefault();
                    }
                    if (SesionComiteSolicitudComiteTecnico.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Novedad_Contractual)
                    {
                        if (SesionComiteSolicitudComiteTecnico.SolicitudId > 0)
                        {
                            SesionComiteSolicitudComiteTecnico.NovedadContractual =
                              ListModificacionesContractuales
                                .Where(r => r.NovedadContractualId == SesionComiteSolicitudComiteTecnico.SolicitudId)
                                .FirstOrDefault();

                            SesionComiteSolicitudComiteTecnico.ModificacionContractual =
                                ListModificacionesContractuales
                                .Where(r => r.NovedadContractualId == SesionComiteSolicitudComiteTecnico.SolicitudId)
                                .FirstOrDefault();
                        }
                    }
                    if (SesionComiteSolicitudComiteTecnico.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.ControversiasContractuales)
                    {
                        if (SesionComiteSolicitudComiteTecnico.SolicitudId > 0)
                        {
                            SesionComiteSolicitudComiteTecnico.ControversiaContractual =
                                ListControversiasContractuales
                                .Where(r => r.ControversiaContractualId == SesionComiteSolicitudComiteTecnico.SolicitudId)
                                .FirstOrDefault();
                        }
                    }
                    if (SesionComiteSolicitudComiteTecnico.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Defensa_judicial)
                    {
                        if (SesionComiteSolicitudComiteTecnico.SolicitudId > 0)
                        {
                            SesionComiteSolicitudComiteTecnico.DefensaJudicial =
                                ListDefensaJudicial
                                .Where(r => r.DefensaJudicialId == SesionComiteSolicitudComiteTecnico.SolicitudId)
                                .FirstOrDefault();
                        }
                    }
                    if (SesionComiteSolicitudComiteTecnico.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Actualizacion_Cronograma_Proceso_Seleccion)
                    {
                        if (SesionComiteSolicitudComiteTecnico.SolicitudId > 0)
                        {
                            SesionComiteSolicitudComiteTecnico.ProcesoSeleccionMonitoreo =
                                ListProcesoSeleccionMonitoreo
                                .Where(r => r.ProcesoSeleccionMonitoreoId == SesionComiteSolicitudComiteTecnico.SolicitudId)
                                .FirstOrDefault();
                        }
                    }
                    if (SesionComiteSolicitudComiteTecnico.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Evaluacion_De_Proceso)
                    {
                        if (SesionComiteSolicitudComiteTecnico.SolicitudId > 0)
                        {
                            SesionComiteSolicitudComiteTecnico.ProcesoSeleccion = ListProcesosSelecicon.Where(r => r.ProcesoSeleccionId == SesionComiteSolicitudComiteTecnico.SolicitudId).FirstOrDefault();
                        }
                    }
                    if (SesionComiteSolicitudComiteTecnico.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Actuaciones_Controversias_Contractuales)
                    {
                        if (SesionComiteSolicitudComiteTecnico.SolicitudId > 0)
                        {
                            SesionComiteSolicitudComiteTecnico.ControversiaActuacion =
                                ListControversiaActuacion
                                .Where(r => r.ControversiaActuacionId == SesionComiteSolicitudComiteTecnico.SolicitudId)
                                .FirstOrDefault();
                        }
                    }
                }

                foreach (var SesionComiteSolicitudComiteTecnico in item.SesionComiteSolicitudComiteTecnicoFiduciario)
                {
                    //if (!BorrarCompromisosFiduciarios && SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso.Count() > 0)
                    //    SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso = null;
                    if (SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso.Count() > 0)
                        SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso = SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso.Where(s => s.Eliminado != true && s.EsFiduciario == true).ToList();
                    if (!string.IsNullOrEmpty(SesionComiteSolicitudComiteTecnico.EstadoCodigo))
                    {
                        SesionComiteSolicitudComiteTecnico.EstadoCodigo = ListParametricas
                                .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud
                                && r.Codigo == SesionComiteSolicitudComiteTecnico.EstadoCodigo).FirstOrDefault().Nombre;
                    }

                    ///TIPOS DE SOLICITUD
                    if (SesionComiteSolicitudComiteTecnico.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion)
                    {
                        if (SesionComiteSolicitudComiteTecnico.SolicitudId > 0)
                        {
                            SesionComiteSolicitudComiteTecnico.ProcesoSeleccion = ListProcesosSelecicon.Where(r => r.ProcesoSeleccionId == SesionComiteSolicitudComiteTecnico.SolicitudId).FirstOrDefault();
                        }
                    }
                    if (SesionComiteSolicitudComiteTecnico.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion)
                    {
                        SesionComiteSolicitudComiteTecnico.Contratacion = ListContratacion.Where(r => r.ContratacionId == SesionComiteSolicitudComiteTecnico.SolicitudId).FirstOrDefault();
                    }
                    if (SesionComiteSolicitudComiteTecnico.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Novedad_Contractual)
                    {
                        if (SesionComiteSolicitudComiteTecnico.SolicitudId > 0)
                        {
                            SesionComiteSolicitudComiteTecnico.ModificacionContractual =
                                ListModificacionesContractuales
                                .Where(r => r.NovedadContractualId == SesionComiteSolicitudComiteTecnico.SolicitudId)
                                .FirstOrDefault();
                        }
                    }
                    if (SesionComiteSolicitudComiteTecnico.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.ControversiasContractuales)
                    {
                        if (SesionComiteSolicitudComiteTecnico.SolicitudId > 0)
                        {
                            SesionComiteSolicitudComiteTecnico.ControversiaContractual =
                                ListControversiasContractuales
                                .Where(r => r.ControversiaContractualId == SesionComiteSolicitudComiteTecnico.SolicitudId)
                                .FirstOrDefault();
                        }
                    }
                    if (SesionComiteSolicitudComiteTecnico.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Defensa_judicial)
                    {
                        if (SesionComiteSolicitudComiteTecnico.SolicitudId > 0)
                        {
                            SesionComiteSolicitudComiteTecnico.DefensaJudicial =
                                ListDefensaJudicial
                                .Where(r => r.DefensaJudicialId == SesionComiteSolicitudComiteTecnico.SolicitudId)
                                .FirstOrDefault();
                        }
                    }
                    if (SesionComiteSolicitudComiteTecnico.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Actualizacion_Cronograma_Proceso_Seleccion)
                    {
                        if (SesionComiteSolicitudComiteTecnico.SolicitudId > 0)
                        {
                            SesionComiteSolicitudComiteTecnico.ProcesoSeleccionMonitoreo =
                                ListProcesoSeleccionMonitoreo
                                .Where(r => r.ProcesoSeleccionMonitoreoId == SesionComiteSolicitudComiteTecnico.SolicitudId)
                                .FirstOrDefault();
                        }
                    }
                    if (SesionComiteSolicitudComiteTecnico.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Evaluacion_De_Proceso)
                    {
                        if (SesionComiteSolicitudComiteTecnico.SolicitudId > 0)
                        {
                            SesionComiteSolicitudComiteTecnico.ProcesoSeleccion = ListProcesosSelecicon.Where(r => r.ProcesoSeleccionId == SesionComiteSolicitudComiteTecnico.SolicitudId).FirstOrDefault();
                        }
                    }
                    if (SesionComiteSolicitudComiteTecnico.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Actuaciones_Controversias_Contractuales)
                    {
                        if (SesionComiteSolicitudComiteTecnico.SolicitudId > 0)
                        {
                            SesionComiteSolicitudComiteTecnico.ControversiaActuacion =
                                ListControversiaActuacion
                                .Where(r => r.ControversiaActuacionId == SesionComiteSolicitudComiteTecnico.SolicitudId)
                                .FirstOrDefault();
                        }
                    }
                }
                List<ComiteTecnico> ListComiteTecnico = new List<ComiteTecnico>
                {
                    item
                };
                return ListComiteTecnico;
            }
            catch (Exception ex)
            {
                return new List<ComiteTecnico>();
            }
        }

        //Reportar Avance Compromisos
        public async Task<Respuesta> CreateOrEditReportProgress(CompromisoSeguimiento compromisoSeguimiento, string estadoCompromiso)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Seguimiento_Compromiso, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                string strCrearEditar;
                if (string.IsNullOrEmpty(compromisoSeguimiento.CompromisoSeguimientoId.ToString()) || compromisoSeguimiento.CompromisoSeguimientoId == 0)
                {
                    //Auditoria
                    strCrearEditar = ConstantCommonMessages.REGISTRAR_AVANCE_COMPROMISOS;
                    compromisoSeguimiento.FechaCreacion = DateTime.Now;
                    compromisoSeguimiento.UsuarioCreacion = compromisoSeguimiento.UsuarioCreacion;
                    compromisoSeguimiento.Eliminado = false;
                    _context.CompromisoSeguimiento.Add(compromisoSeguimiento);

                }
                else
                {
                    strCrearEditar = ConstantCommonMessages.EDITAR_AVANCE_COMPROMISOS;
                    CompromisoSeguimiento compromisoSeguimientoAntiguo = _context.CompromisoSeguimiento.Find(compromisoSeguimiento.CompromisoSeguimientoId);

                    //Auditoria
                    compromisoSeguimientoAntiguo.UsuarioModificacion = compromisoSeguimiento.UsuarioModificacion;
                    //Registros
                    compromisoSeguimientoAntiguo.FechaCreacion = DateTime.Now;
                    compromisoSeguimientoAntiguo.DescripcionSeguimiento = compromisoSeguimiento.DescripcionSeguimiento;
                    compromisoSeguimientoAntiguo.SesionParticipanteId = compromisoSeguimiento.SesionParticipanteId;

                }
                _context.SaveChanges();
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = compromisoSeguimiento,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, compromisoSeguimiento.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = compromisoSeguimiento,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, compromisoSeguimiento.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        //Comentar y devolver acta
        public async Task<Respuesta> CreateOrEditCommentReport(SesionComentario SesionComentario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Comentario_Acta, (int)EnumeratorTipoDominio.Acciones);
            ComiteTecnico comiteTecnicoOld = _context.ComiteTecnico
                .Where(r => r.ComiteTecnicoId == SesionComentario.ComiteTecnicoId)
                .Include(sc => sc.SesionParticipante)
                .Include(sc => sc.SesionComentario)
                .FirstOrDefault();
            try
            {
                SesionComentario sesionComentario = new SesionComentario
                {
                    Fecha = DateTime.Now,
                    Observacion = SesionComentario.Observacion,
                    FechaCreacion = DateTime.Now,
                    UsuarioCreacion = SesionComentario.UsuarioCreacion,
                    ComiteTecnicoId = SesionComentario.ComiteTecnicoId,
                    MiembroSesionParticipanteId = SesionComentario.MiembroSesionParticipanteId,
                    EstadoActaVoto = ConstantCodigoActas.Devuelta,
                    ValidacionVoto = false
                };

                _context.SesionComentario.Add(sesionComentario);

                _context.SaveChanges();

                if ((bool)ValidarTodosVotacion(comiteTecnicoOld))
                {
                    comiteTecnicoOld.EstadoActaCodigo = ValidarEstadoActaVotacion(comiteTecnicoOld);
                    if (comiteTecnicoOld.EstadoActaCodigo == ConstantCodigoActas.Devuelta)
                    {
                        comiteTecnicoOld.EstadoComiteCodigo = ConstanCodigoEstadoComite.Con_Acta_De_Sesion_Devuelta;
                    }
                    comiteTecnicoOld.EsCompleto = false;

                    foreach (var SesionComentarios in comiteTecnicoOld.SesionComentario)
                    {
                        SesionComentarios.ValidacionVoto = true;
                    }
                }
                _context.SaveChanges();
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = SesionComentario,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, SesionComentario.UsuarioCreacion, "COMENTAR DEVOLVER ACTA")

                };
            }

            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = null,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, SesionComentario.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        //Aprobar Acta
        public async Task<Respuesta> AcceptReport(int comiteTecnicoId, Usuario pUser, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Aprobar_Acta, (int)EnumeratorTipoDominio.Acciones);

            //Cambiar estados cuando se crea y todos los sesion particiantes que esten asociados al comite si ya todos comentarios y que ayan estado del acta aprobado el
            string strCrearEditar = ConstantCommonMessages.APROBAR_ACTA;
            try
            {
                ComiteTecnico comiteTecnico =
                    await _context.ComiteTecnico.Where(r => r.ComiteTecnicoId == comiteTecnicoId)
                     .Include(r => r.SesionParticipante)
                     .Include(sc => sc.SesionComentario)
                     .Include(sc => sc.SesionComiteSolicitudComiteTecnico)
                     .Include(sc => sc.SesionComiteSolicitudComiteTecnicoFiduciario)
                     .AsNoTracking().FirstOrDefaultAsync();

                SesionComentario sesionComentario = new SesionComentario
                {
                    Fecha = DateTime.Now,
                    Observacion = ConstantCommonMessages.APROBADA_POR + pUser.Email.ToUpper(),
                    FechaCreacion = DateTime.Now,
                    UsuarioCreacion = pUser.Email,
                    ComiteTecnicoId = comiteTecnicoId,
                    MiembroSesionParticipanteId = pUser.UsuarioId,
                    EstadoActaVoto = ConstantCodigoActas.Aprobada,
                    ValidacionVoto = false
                };
                _context.SesionComentario.Add(sesionComentario);
                _context.SaveChanges();

                //ValidarVotacion
                if ((bool)ValidarTodosVotacion(comiteTecnico))
                {
                    //Valida que todos votaron
                    _context.Set<SesionComentario>()
                            .Where(s => s.ComiteTecnicoId == comiteTecnico.ComiteTecnicoId)
                            .Update(s => new SesionComentario
                            {
                                UsuarioModificacion = pUser.Email,
                                FechaModificacion = DateTime.Now,
                                ValidacionVoto = true,
                            });
                    //Actualizar estados del comite
                    _context.Set<ComiteTecnico>()
                            .Where(c => c.ComiteTecnicoId == comiteTecnico.ComiteTecnicoId)
                            .Update(c => new ComiteTecnico
                            {
                                UsuarioModificacion = pUser.Email,
                                FechaModificacion = DateTime.Now,
                                EstadoActaCodigo = ValidarEstadoActaVotacion(comiteTecnico),
                                EstadoComiteCodigo = ConstanCodigoEstadoComite.Con_Acta_De_Sesion_Aprobada,
                            });

                    await EnviarActaAprobada(comiteTecnicoId, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);
                    await NotificarCompromisos(comiteTecnicoId, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);

                    //Cambiar estado Comite Con acta aprobada y Solicitudes 
                    //if (comiteTecnico.EstadoActaCodigo == ConstantCodigoActas.Aprobada)
                    {
                        if (comiteTecnico.EsComiteFiduciario == true)
                        {
                            foreach (var item in comiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario)
                            {
                                _committeeSessionFiduciarioService.CambiarEstadoSolicitudes(item.SolicitudId, item.TipoSolicitudCodigo, item.EstadoCodigo, pUser.Email);
                            }
                        }

                        else
                        {
                            foreach (var item in comiteTecnico.SesionComiteSolicitudComiteTecnico)
                            {
                                _registerSessionTechnicalCommitteeService.CambiarEstadoSolicitudes(item.SolicitudId, item.TipoSolicitudCodigo, item.EstadoCodigo);
                            }
                        }
                    }
                }
                //valido si el comite tiene relacionado un proceso de selección, solo para fiduciario
                if (comiteTecnico.TipoTemaFiduciarioCodigo != null)
                {
                    var pSesionComite = _context.SesionComiteSolicitud.Where(x => x.ComiteTecnicoFiduciarioId == comiteTecnico.ComiteTecnicoId).ToList();
                    foreach (var pses in pSesionComite)
                    {
                        if (pses.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion)
                        {
                            //obtengo el proponente y lo convierto en contratista
                            var proponentes = _context.ProcesoSeleccionProponente.Where(x => x.ProcesoSeleccionId == pses.SolicitudId).Include(x => x.ProcesoSeleccion).ToList();
                            //solo si no es invitación cerrada
                            if (proponentes?.FirstOrDefault()?.ProcesoSeleccion?.TipoProcesoCodigo != ConstanCodigoTipoProcesoSeleccion.Invitacion_Cerrada)
                            {
                                foreach (var p in proponentes)
                                {
                                    Contratista contratista = new Contratista();
                                    //verifico que no exista
                                    var existecotraticsta = _context.Contratista.Where(x => x.NumeroIdentificacion == p.NumeroIdentificacion).FirstOrDefault();
                                    if (existecotraticsta == null)
                                    {
                                        contratista.TipoIdentificacionCodigo = (p.TipoProponenteCodigo == "4" || p.TipoProponenteCodigo == "2") ? "3" : "1"; //Nit - cedula
                                        contratista.NumeroIdentificacion = string.IsNullOrEmpty(p.NumeroIdentificacion) ? "0" : p.NumeroIdentificacion;
                                        contratista.Nombre = p.NombreProponente;
                                        contratista.RepresentanteLegal = string.IsNullOrEmpty(p.NombreRepresentanteLegal) ? p.NombreProponente : p.NombreRepresentanteLegal;
                                        contratista.RepresentanteLegalNumeroIdentificacion = string.IsNullOrEmpty(p.NombreRepresentanteLegal) ? "" : p.CedulaRepresentanteLegal;
                                        contratista.NumeroInvitacion = p.ProcesoSeleccion.NumeroProceso;
                                        contratista.TipoProponenteCodigo = p.TipoProponenteCodigo;
                                        contratista.Activo = true;
                                        contratista.FechaCreacion = DateTime.Now;
                                        contratista.UsuarioCreacion = pUser.Email.ToUpper();
                                        contratista.ProcesoSeleccionProponenteId = p.ProcesoSeleccionProponenteId;

                                        _context.Contratista.Add(contratista);
                                    }
                                }
                            }
                        }
                    }
                }

                return new Respuesta
                {

                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, pUser.Email, strCrearEditar)
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, pUser.Email, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        private string ValidarEstadoActaVotacion(ComiteTecnico pComiteTecnico)
        {
            string EstadoActa = ConstantCodigoActas.Devuelta;

            if (pComiteTecnico.SesionComentario
                                           .Where(r => r.EstadoActaVoto == ConstantCodigoActas.Aprobada && r.ValidacionVoto == false).Count()
                                                                      == pComiteTecnico.SesionComentario.Where(r => r.ValidacionVoto == false).Count())
                EstadoActa = ConstantCodigoActas.Aprobada;

            return EstadoActa;
        }

        private bool ValidarTodosVotacion(ComiteTecnico pComiteTecnico)
        {
            int CantidadParticipantes = pComiteTecnico.SesionParticipante.Count(c => c.Eliminado != true);
            int CantidadDeVotos = _context.SesionComentario.Count(c => c.ComiteTecnicoId == pComiteTecnico.ComiteTecnicoId && c.ValidacionVoto != true);

            return CantidadParticipantes == CantidadDeVotos;
        }

        //Actualizar estado codigo de un compromiso
        public async Task<bool> UpdateStatus(int sesionComiteTecnicoCompromisoId, string status)
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(sesionComiteTecnicoCompromisoId)) || !string.IsNullOrEmpty(status))
                {
                    SesionComiteTecnicoCompromiso sesionComiteTecnicoCompromiso =
                        await _context.SesionComiteTecnicoCompromiso
                        .Where(up => up.SesionComiteTecnicoCompromisoId == sesionComiteTecnicoCompromisoId && (bool)!up.Eliminado)
                        .FirstOrDefaultAsync();
                    if (sesionComiteTecnicoCompromiso != null)
                    {
                        sesionComiteTecnicoCompromiso.EstadoCodigo = status;
                        _context.SesionComiteTecnicoCompromiso.Update(sesionComiteTecnicoCompromiso);
                        var result = await _context.SaveChangesAsync();
                        if (result > 0)
                            return true;
                    }

                    return false;
                }

                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<dynamic>> GetListCompromisoSeguimiento(int SesionSolicitudCompromisoId, int pTipoCompromiso)
        {
            List<Dominio> ListDominio = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Compromisos).ToList();

            if (pTipoCompromiso == (int)EnumeratorTipoCompromisos.Compromisos_Solicitudes)
            {
                var dynamics = await _context.CompromisoSeguimiento.Where(r => r.SesionSolicitudCompromisoId == SesionSolicitudCompromisoId).Select(r => new { r.FechaCreacion, r.DescripcionSeguimiento, r.EstadoCompromisoCodigo, r.CompromisoSeguimientoId }).OrderByDescending(r => r.CompromisoSeguimientoId).ToListAsync();
                List<dynamic> ListDynamic = new List<dynamic>();

                foreach (var CompromisoSeguimiento in dynamics)
                {
                    ListDynamic.Add(new
                    {
                        CompromisoSeguimiento.FechaCreacion,
                        EstadoCompromiso = ListDominio.Where(r => r.Codigo == CompromisoSeguimiento.EstadoCompromisoCodigo).Select(r => r.Nombre).FirstOrDefault(),
                        CompromisoSeguimiento.DescripcionSeguimiento
                    });
                }
                return ListDynamic;
            }
            else
            {
                var dynamics = await _context.TemaCompromisoSeguimiento.Where(r => r.TemaCompromisoId == SesionSolicitudCompromisoId).Select(r => new { r.FechaCreacion, r.Tarea, r.EstadoCodigo, r.TemaCompromisoSeguimientoId }).OrderByDescending(r => r.TemaCompromisoSeguimientoId).ToListAsync();

                List<dynamic> ListDynamic = new List<dynamic>();

                foreach (var CompromisoSeguimiento in dynamics)
                {
                    ListDynamic.Add(new
                    {
                        CompromisoSeguimiento.FechaCreacion,
                        EstadoCompromiso = ListDominio.Where(r => r.Codigo == CompromisoSeguimiento.EstadoCodigo).Select(r => r.Nombre).FirstOrDefault(),
                        DescripcionSeguimiento = CompromisoSeguimiento.Tarea
                    });
                }
                return ListDynamic;
            }

        }

        public async Task<Respuesta> ChangeStatusSesionComiteSolicitudCompromiso(SesionSolicitudCompromiso pSesionSolicitudCompromiso)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Seguimiento_Compromiso, (int)EnumeratorTipoDominio.Acciones);
            List<dynamic> Return = new List<dynamic>();


            try
            {
                // Si el compromiso es de sesionComiteSolicitud
                if (pSesionSolicitudCompromiso.TipoCompromiso == ((int)EnumeratorTipoCompromisos.Compromisos_Solicitudes).ToString())
                {
                    SesionSolicitudCompromiso sesionSolicitudCompromisoOld = await _context.SesionSolicitudCompromiso.FindAsync(pSesionSolicitudCompromiso.SesionSolicitudCompromisoId);
                    sesionSolicitudCompromisoOld.FechaModificacion = DateTime.Now;
                    sesionSolicitudCompromisoOld.UsuarioCreacion = pSesionSolicitudCompromiso.UsuarioCreacion;
                    sesionSolicitudCompromisoOld.EstadoCodigo = pSesionSolicitudCompromiso.EstadoCodigo;


                    CompromisoSeguimiento compromisoSeguimiento = new CompromisoSeguimiento
                    {
                        UsuarioCreacion = pSesionSolicitudCompromiso.UsuarioCreacion,
                        FechaCreacion = DateTime.Now,
                        Eliminado = false,

                        EstadoCompromisoCodigo = pSesionSolicitudCompromiso.EstadoCodigo,
                        DescripcionSeguimiento = pSesionSolicitudCompromiso.GestionRealizada,
                        SesionParticipanteId = Int32.Parse(pSesionSolicitudCompromiso.UsuarioModificacion),
                        SesionSolicitudCompromisoId = pSesionSolicitudCompromiso.SesionSolicitudCompromisoId

                    };
                    _context.CompromisoSeguimiento.Add(compromisoSeguimiento);
                }
                // Si el compromiso es de Tema
                else
                {
                    TemaCompromiso temaCompromisoOld = _context.TemaCompromiso.Find(pSesionSolicitudCompromiso.SesionSolicitudCompromisoId);

                    temaCompromisoOld.UsuarioModificacion = pSesionSolicitudCompromiso.UsuarioCreacion;
                    temaCompromisoOld.FechaModificacion = DateTime.Now;

                    switch (pSesionSolicitudCompromiso.EstadoCodigo)
                    {
                        case ConstantCodigoCompromisos.Finalizado:
                            temaCompromisoOld.EstadoCodigo = pSesionSolicitudCompromiso.EstadoCodigo;
                            break;

                        case ConstantCodigoCompromisos.En_proceso:
                            temaCompromisoOld.EstadoCodigo = pSesionSolicitudCompromiso.EstadoCodigo;
                            break;
                    }

                    TemaCompromisoSeguimiento temaCompromisoSeguimiento = new TemaCompromisoSeguimiento
                    {
                        UsuarioCreacion = pSesionSolicitudCompromiso.UsuarioCreacion,
                        FechaCreacion = DateTime.Now,
                        EstadoCodigo = pSesionSolicitudCompromiso.EstadoCodigo,
                        Tarea = pSesionSolicitudCompromiso.GestionRealizada,
                        TemaCompromisoId = pSesionSolicitudCompromiso.SesionSolicitudCompromisoId
                    };
                    _context.TemaCompromisoSeguimiento.Add(temaCompromisoSeguimiento);
                }
                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, pSesionSolicitudCompromiso.UsuarioCreacion, ConstantCommonMessages.CAMBIAR_ESTADO_SOLICITUD_COMPROMISO)

                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = Return,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, pSesionSolicitudCompromiso.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<bool> NotificarCompromisos(int pComiteTecnicoId, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            try
            {
                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

                ComiteTecnico comiteTecnico = _context.ComiteTecnico
                    .Where(r => r.ComiteTecnicoId == pComiteTecnicoId)
                    .Include(r => r.SesionComiteSolicitudComiteTecnico)
                        .ThenInclude(r => r.SesionSolicitudCompromiso)
                            .ThenInclude(r => r.ResponsableSesionParticipante)
                                .ThenInclude(r => r.Usuario)

                    .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                        .ThenInclude(r => r.SesionSolicitudCompromiso)
                            .ThenInclude(r => r.ResponsableSesionParticipante)
                                .ThenInclude(r => r.Usuario)

                    .Include(r => r.SesionComiteTema)
                        .ThenInclude(r => r.TemaCompromiso)
                            .ThenInclude(r => r.ResponsableNavigation)
                                .ThenInclude(r => r.Usuario)

                    .AsNoTracking()
                    .FirstOrDefault();

                string Tabla = _context.Template.Find((int)enumeratorTemplate.TablaAprobacionParticipanteActa).Contenido;
                string Registros = _context.Template.Find((int)enumeratorTemplate.RegistrosTablaAprobacionParticipanteActa).Contenido;
                string TotalRegistros = string.Empty;

                bool blEnvioCorreo = false;

                foreach (var sesionComiteSolicitud in comiteTecnico.SesionComiteSolicitudComiteTecnico.Distinct())
                {
                    sesionComiteSolicitud.SesionSolicitudCompromiso = sesionComiteSolicitud.SesionSolicitudCompromiso.Where(r => r.EsFiduciario != true && r.Eliminado != true).ToList();
                    foreach (var sesionSolicitudCompromiso in sesionComiteSolicitud.SesionSolicitudCompromiso.Distinct())
                    {
                        if (!string.IsNullOrEmpty(sesionSolicitudCompromiso?.ResponsableSesionParticipante?.Usuario?.Email))
                        {
                            Template TemplateNotificacionCompromisos = await _commonService.GetTemplateById((int)enumeratorTemplate.NotificacionCompromisos);
                            string template =
                                TemplateNotificacionCompromisos.Contenido
                                .Replace("_LinkF_", pDominioFront)
                                .Replace("[URL]", pDominioFront + "compromisosActasComite")
                                .Replace("[TIPO_COMITE]", comiteTecnico.EsComiteFiduciario != true ? " Técnico" : " Fiduciario")
                                .Replace("[NUMERO_COMITE]", comiteTecnico.NumeroComite)
                                .Replace("[COMPROMISO]", sesionSolicitudCompromiso.Tarea)
                                .Replace("[FECHA_CUMPLIMIENTO]", sesionSolicitudCompromiso.FechaCumplimiento.HasValue ? sesionSolicitudCompromiso.FechaCumplimiento.Value.ToString("dd-MM-yyyy") : null);

                            blEnvioCorreo = Helpers.Helpers.EnviarCorreo(sesionSolicitudCompromiso?.ResponsableSesionParticipante?.Usuario?.Email, "Notificación Compromisos", template, pSender, pPassword, pMailServer, pMailPort);
                        }
                    }
                }

                foreach (var sesionComiteSolicitud in comiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario.Where(s => s.Eliminado != true).Distinct())
                {
                    sesionComiteSolicitud.SesionSolicitudCompromiso = sesionComiteSolicitud.SesionSolicitudCompromiso.Where(r => r.EsFiduciario == true).ToList();
                    foreach (var sesionSolicitudCompromiso in sesionComiteSolicitud.SesionSolicitudCompromiso.Where(r => r.Eliminado != true).Distinct())
                    {
                        if (!string.IsNullOrEmpty(sesionSolicitudCompromiso?.ResponsableSesionParticipante?.Usuario?.Email))
                        {
                            Template TemplateNotificacionCompromisos = await _commonService.GetTemplateById((int)enumeratorTemplate.NotificacionCompromisos);
                            string template =
                                TemplateNotificacionCompromisos.Contenido
                                .Replace("_LinkF_", pDominioFront)
                                .Replace("[URL]", pDominioFront + "compromisosActasComite")
                                .Replace("[TIPO_COMITE]", comiteTecnico.EsComiteFiduciario != true ? " Técnico" : " Fiduciario")
                                .Replace("[NUMERO_COMITE]", comiteTecnico.NumeroComite)
                                .Replace("[COMPROMISO]", sesionSolicitudCompromiso.Tarea)
                                .Replace("[FECHA_CUMPLIMIENTO]", sesionSolicitudCompromiso.FechaCumplimiento.HasValue ? sesionSolicitudCompromiso.FechaCumplimiento.Value.ToString("dd-MM-yyyy") : null);

                            blEnvioCorreo = Helpers.Helpers.EnviarCorreo(sesionSolicitudCompromiso?.ResponsableSesionParticipante?.Usuario?.Email, "Notificación Compromisos", template, pSender, pPassword, pMailServer, pMailPort);
                        }
                    }
                }

                foreach (var tema in comiteTecnico.SesionComiteTema)
                {
                    //tema.SesionSolicitudCompromiso = sesionComiteSolicitud.SesionSolicitudCompromiso.Where(r => r.EsFiduciario != true).ToList();
                    foreach (var compromiso in tema.TemaCompromiso)
                    {
                        if (!string.IsNullOrEmpty(compromiso?.ResponsableNavigation?.Usuario?.Email))
                        {
                            Template TemplateNotificacionCompromisos = await _commonService.GetTemplateById((int)enumeratorTemplate.NotificacionCompromisos);
                            string template =
                                TemplateNotificacionCompromisos.Contenido
                                .Replace("_LinkF_", pDominioFront)
                                .Replace("[URL]", pDominioFront + "compromisosActasComite")
                                .Replace("[TIPO_COMITE]", comiteTecnico.EsComiteFiduciario != true ? " Técnico" : " Fiduciario")
                                .Replace("[NUMERO_COMITE]", comiteTecnico.NumeroComite)
                                .Replace("[COMPROMISO]", compromiso.Tarea)
                                .Replace("[FECHA_CUMPLIMIENTO]", compromiso.FechaCumplimiento.HasValue ? compromiso.FechaCumplimiento.Value.ToString("dd-MM-yyyy") : null);

                            blEnvioCorreo = Helpers.Helpers.EnviarCorreo(compromiso?.ResponsableNavigation?.Usuario?.Email, "Notificación Compromisos", template, pSender, pPassword, pMailServer, pMailPort);
                        }
                    }
                }

                return blEnvioCorreo;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> EnviarActaAprobada(int pComiteTecnicoId, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            try
            {
                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

                ComiteTecnico comiteTecnico = _context.ComiteTecnico
                    .Where(r => r.ComiteTecnicoId == pComiteTecnicoId)
                    .Include(r => r.SesionParticipante).ThenInclude(r => r.Usuario).ThenInclude(r => r.SesionComentario)
                    .AsNoTracking()
                    .FirstOrDefault();

                string Tabla = _context.Template.Find((int)enumeratorTemplate.TablaAprobacionParticipanteActa).Contenido;
                string Registros = _context.Template.Find((int)enumeratorTemplate.RegistrosTablaAprobacionParticipanteActa).Contenido;
                string TotalRegistros = string.Empty;

                foreach (var SesionParticipante in comiteTecnico.SesionParticipante)
                {
                    TotalRegistros += Registros;

                    TotalRegistros = TotalRegistros.Replace("[FECHA_APROBACION]", (SesionParticipante.Usuario.SesionComentario.Where(r => r.EstadoActaVoto == ConstantCodigoActas.Aprobada).Select(r => r.Fecha).FirstOrDefault()).ToString("dd-MM-yyyy"))
                                  .Replace("[RESPONSABLE]", myTI.ToTitleCase(SesionParticipante.Usuario.PrimerNombre.ToLower() + " " + SesionParticipante.Usuario.PrimerApellido.ToLower()));

                }
                Tabla = Tabla.Replace("[REGISTROS]", TotalRegistros);

                bool blEnvioCorreo = false;
                var usuariosecretario = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Secretario_Comite).Select(x => x.Usuario.Email).ToList();


                Template TemplateActaAprobada = await _commonService.GetTemplateById((int)enumeratorTemplate.NotificacionActaAprobacion);
                string template =
                    TemplateActaAprobada.Contenido
                    .Replace("_LinkF_", pDominioFront)
                    .Replace("[TIPO_COMITE]", (bool)comiteTecnico.EsComiteFiduciario ? ConstanStringTipoComite.Fiduciario : ConstanStringTipoComite.Tecnico)
                    .Replace("[NUMERO_COMITE]", comiteTecnico.NumeroComite)
                    .Replace("[TABLA_RESPONSABLE_APROBACION]", Tabla)
                    .Replace("[FECHA_COMITE]", ((DateTime.Now).ToString("dd-MM-yyyy")));

                foreach (var usuario in usuariosecretario)
                {
                    blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuario, "Aprobación de acta", template, pSender, pPassword, pMailServer, pMailPort);
                }

                return blEnvioCorreo;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        /// <summary>
        /// Tarea Programada 
        /// </summary>
        /// <param name="pServerUser"></param>
        /// <returns></returns>
        public async Task GetApproveExpiredMinutes(string pServerUser)
        {

            int intCantidadDiasDeVencimiento = Int32.Parse(await _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tiempo_Aprobar_Acta).Select(r => r.Nombre).FirstOrDefaultAsync());
            DateTime FechaCorteActas = DateTime.Now.AddDays(-intCantidadDiasDeVencimiento);
            List<ComiteTecnico> comiteTecnicos = _context.ComiteTecnico.Where(r => r.EstadoActaCodigo == ConstantCodigoActas.En_proceso_Aprobacion).Where(r => r.FechaModificacion.HasValue && r.FechaModificacion < FechaCorteActas).ToList();
            foreach (var comite in comiteTecnicos)
            {
                comite.EstadoActaCodigo = ConstantCodigoActas.Aprobada;
                comite.FechaModificacion = DateTime.Now;
                comite.UsuarioModificacion = pServerUser;
            }

            _context.SaveChanges();
        }

        public Task<HTMLContent> GetHTMLString(ActaComite obj)
        {
            throw new NotImplementedException();
        }
    }
}
