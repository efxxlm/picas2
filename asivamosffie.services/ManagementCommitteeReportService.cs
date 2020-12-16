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
        private bool ReturnValue { get; set; }

        public ManagementCommitteeReportService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
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
        public async Task<List<dynamic>> GetListCompromisos(int pUserId)
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
                .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                     .ThenInclude(r => r.SesionSolicitudCompromiso)
                .Include(r => r.SesionComiteTema)
                    .ThenInclude(r => r.TemaCompromiso).ToListAsync();

            //   List<Dominio> ListEstadoCompromisos = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Compromiso).ToList();

            foreach (var ComiteTecnico in ListComiteTecnico.Where(r => !(bool)r.Eliminado).ToList().OrderByDescending(r => r.ComiteTecnicoId))
            {
                foreach (var SesionComiteSolicitudComiteTecnico in ComiteTecnico.SesionComiteSolicitudComiteTecnico.Where(r => !(bool)r.Eliminado).ToList().OrderByDescending(r => r.SesionComiteSolicitudId))
                {
                    foreach (var SesionSolicitudCompromiso in SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso.Where(r => !(bool)r.Eliminado).ToList().OrderByDescending(r => r.SesionSolicitudCompromisoId))
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
                foreach (var SesionComiteSolicitudComiteTecnico in ComiteTecnico.SesionComiteSolicitudComiteTecnicoFiduciario.Where(r => !(bool)r.Eliminado).ToList().OrderByDescending(r => r.SesionComiteSolicitudId))
                {
                    foreach (var SesionSolicitudCompromiso in SesionComiteSolicitudComiteTecnico.SesionSolicitudCompromiso.Where(r => !(bool)r.Eliminado).ToList().OrderByDescending(r => r.SesionSolicitudCompromisoId))
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
                    foreach (var TemaCompromiso in SesionComiteTema.TemaCompromiso.Where(r => !(bool)r.Eliminado).ToList().OrderByDescending(r => r.TemaCompromisoId))
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
            List<GrillaSesionComiteTecnicoCompromiso> grillaSesionComiteTecnicoCompromisos = new List<GrillaSesionComiteTecnicoCompromiso>();
            string StrSql = "SELECT ComiteTecnico.*" +
                " FROM  dbo.ComiteTecnico " +
                "INNER JOIN dbo.SesionParticipante  ON   ComiteTecnico.ComiteTecnicoId = SesionParticipante.ComiteTecnicoId " +
                "WHERE  SesionParticipante.UsuarioId = " + pUserId + " AND   ComiteTecnico.Eliminado = 0 AND  SesionParticipante.Eliminado = 0";

            return await _context.ComiteTecnico.FromSqlRaw(StrSql)
                      .Include(r => r.SesionComiteTecnicoCompromiso)
                      .Include(r => r.SesionComiteSolicitudComiteTecnico)
                      .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                      .Include(r => r.SesionComiteTema)
                          .ThenInclude(r => r.TemaCompromiso)
                      .OrderByDescending(r => r.ComiteTecnicoId)
                      .Distinct()
                      .OrderByDescending(r => r.ComiteTecnicoId)
                  .ToListAsync();
        }

        //Detalle gestion de actas
        public async Task<ActionResult<List<ComiteTecnico>>> GetManagementReportById(int comiteTecnicoId)
        {
            try
            {
                List<ComiteTecnico> ListComiteTecnico = await _context.ComiteTecnico
                      .Where(r => r.ComiteTecnicoId == comiteTecnicoId)
                            .Include(r => r.SesionComentario)
                            .Include(r => r.SesionComiteTema)
                               .ThenInclude(r => r.TemaCompromiso)
                            .Include(r => r.SesionParticipante)
                               .ThenInclude(r => r.Usuario)
                             .Include(r => r.SesionComiteTecnicoCompromiso)
                               .ThenInclude(r => r.CompromisoSeguimiento)
                                       .Include(r => r.SesionComiteSolicitudComiteTecnico)
                                                 .ThenInclude(r => r.SesionSolicitudVoto)
                             .Include(r => r.SesionComiteSolicitudComiteTecnico)
                               .ThenInclude(r => r.SesionSolicitudCompromiso)
                             .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                              .ThenInclude(r => r.SesionSolicitudCompromiso)
                               .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                              .ThenInclude(r => r.SesionSolicitudVoto)
                             .ToListAsync();

                List<Dominio> ListParametricas = _context.Dominio.ToList();
                List<Contratacion> ListContratacion = _context.Contratacion.ToList();
                List<ProcesoSeleccion> ListProcesosSelecicon = _context.ProcesoSeleccion.ToList();

                foreach (var item in ListComiteTecnico)
                {
                    if (item.SesionComiteTecnicoCompromiso.Count() > 0)
                    {
                        item.SesionComiteTecnicoCompromiso = item.SesionComiteTecnicoCompromiso.Where(r => !(bool)r.Eliminado).ToList();
                    }

                    foreach (var item2 in item.SesionParticipante)
                    {
                        item2.Usuario.Contrasena = string.Empty;
                    }

                    foreach (var SesionComiteTema in item.SesionComiteTema)
                    {
                        if (SesionComiteTema.TemaCompromiso.Count() > 0)
                        {
                            SesionComiteTema.TemaCompromiso = SesionComiteTema.TemaCompromiso.Where(r => !(bool)r.Eliminado).ToList();
                        }

                        if (!string.IsNullOrEmpty(SesionComiteTema.ResponsableCodigo))
                        {
                            SesionComiteTema.ResponsableCodigo = ListParametricas
                                .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Miembros_Comite_Tecnico && r.Codigo == SesionComiteTema.ResponsableCodigo)
                                .FirstOrDefault().Nombre;
                        }

                        if (!string.IsNullOrEmpty(SesionComiteTema.EstadoTemaCodigo))
                        {
                            SesionComiteTema.EstadoTemaCodigo = ListParametricas
                                .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Sesion_Comite_Solicitud && r.Codigo == SesionComiteTema.EstadoTemaCodigo)
                                .FirstOrDefault().Nombre;
                        }

                    }
                    foreach (var SesionComiteSolicitudComiteTecnico in item.SesionComiteSolicitudComiteTecnico)
                    {
                        if (SesionComiteSolicitudComiteTecnico.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion)
                        {
                            SesionComiteSolicitudComiteTecnico.Contratacion = ListContratacion.Where(r => r.ContratacionId == SesionComiteSolicitudComiteTecnico.SolicitudId).FirstOrDefault();
                        }

                        if (SesionComiteSolicitudComiteTecnico.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion)
                        {
                            if (SesionComiteSolicitudComiteTecnico.SolicitudId > 0)
                            {
                                SesionComiteSolicitudComiteTecnico.ProcesoSeleccion = ListProcesosSelecicon.Where(r => r.ProcesoSeleccionId == SesionComiteSolicitudComiteTecnico.SolicitudId).FirstOrDefault();
                            }
                        }

                        if (!string.IsNullOrEmpty(SesionComiteSolicitudComiteTecnico.EstadoCodigo))
                        {
                            SesionComiteSolicitudComiteTecnico.EstadoCodigo = ListParametricas
                                    .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud
                                    && r.Codigo == SesionComiteSolicitudComiteTecnico.EstadoCodigo).FirstOrDefault().Nombre;
                        }
                    }

                    foreach (var SesionComiteSolicitudComiteTecnico in item.SesionComiteSolicitudComiteTecnicoFiduciario)
                    {
                        if (SesionComiteSolicitudComiteTecnico.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Contratacion)
                        {
                            SesionComiteSolicitudComiteTecnico.Contratacion = ListContratacion.Where(r => r.ContratacionId == SesionComiteSolicitudComiteTecnico.SolicitudId).FirstOrDefault();
                        }

                        if (SesionComiteSolicitudComiteTecnico.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Inicio_De_Proceso_De_Seleccion)
                        {
                            if (SesionComiteSolicitudComiteTecnico.SolicitudId > 0)
                            {
                                SesionComiteSolicitudComiteTecnico.ProcesoSeleccion =
                                    ListProcesosSelecicon
                                    .Where(r => r.ProcesoSeleccionId == SesionComiteSolicitudComiteTecnico.SolicitudId)
                                    .FirstOrDefault();
                            }
                        }

                        if (!string.IsNullOrEmpty(SesionComiteSolicitudComiteTecnico.EstadoCodigo))
                        {
                            SesionComiteSolicitudComiteTecnico.EstadoCodigo = ListParametricas
                                    .Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud
                                    && r.Codigo == SesionComiteSolicitudComiteTecnico.EstadoCodigo).FirstOrDefault().Nombre;
                        }
                    }
                }
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
            ComiteTecnico comiteTecnicoOld = _context.ComiteTecnico.Find(SesionComentario.ComiteTecnicoId);
            try
            {
                string strCrearEditar;
                if (string.IsNullOrEmpty(SesionComentario.SesionComentarioId.ToString()) || SesionComentario.SesionComentarioId == 0)
                {
                    //Auditoria
                    strCrearEditar = ConstantCommonMessages.COMENTAR_Y_DEVOLVER_ACTA;
                    SesionComentario.Fecha = DateTime.Now;
                    SesionComentario.FechaCreacion = DateTime.Now;
                    SesionComentario.UsuarioCreacion = SesionComentario.UsuarioCreacion;

                    _context.SesionComentario.Add(SesionComentario);
                }
                else
                {
                    strCrearEditar = ConstantCommonMessages.EDITAR_COMENTAR_ACTA;
                    SesionComentario SesionComentarioAntiguo = _context.SesionComentario.Find(SesionComentario.SesionComentarioId);

                    //Auditoria
                    SesionComentarioAntiguo.UsuarioModificacion = SesionComentario.UsuarioModificacion;
                    SesionComentarioAntiguo.FechaModificacion = DateTime.Now;

                    //Registros
                    SesionComentarioAntiguo.Fecha = SesionComentario.Fecha;
                    SesionComentarioAntiguo.Observacion = SesionComentario.Observacion;
                }

                if ((bool)ValidarTodosVotacion(comiteTecnicoOld))
                {
                    comiteTecnicoOld.EstadoActaCodigo = ValidarEstadoActaVotacion(comiteTecnicoOld);
                    comiteTecnicoOld.EsCompleto = false;
                    //Validar sesionComentario 
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, SesionComentario.UsuarioCreacion, strCrearEditar)

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
                ComiteTecnico comiteTecnico = await _context.ComiteTecnico.Where(r => r.ComiteTecnicoId == comiteTecnicoId)
                     .Include(r => r.SesionParticipante)
                     .FirstOrDefaultAsync();

                SesionComentario sesionComentario = new SesionComentario
                {
                    Fecha = DateTime.Now,
                    Observacion = ConstantCommonMessages.APROBADA_POR + pUser.Email,
                    FechaCreacion = DateTime.Now,
                    UsuarioCreacion = pUser.Email,
                    ComiteTecnicoId = comiteTecnicoId,
                    MiembroSesionParticipanteId = pUser.UsuarioId,
                    EstadoActaVoto = ConstantCodigoActas.Aprobada,
                    ValidacionVoto = false
                };
                _context.SesionComentario.Add(sesionComentario);
                _context.SaveChanges();

                comiteTecnico.SesionComentario = _context.SesionComentario.Where(r => r.ComiteTecnicoId == comiteTecnicoId && !(bool)r.ValidacionVoto).ToList();

                //ValidarVotacion
                if ((bool)ValidarTodosVotacion(comiteTecnico))
                {
                    comiteTecnico.EstadoActaCodigo = ValidarEstadoActaVotacion(comiteTecnico);

                    //Cambiar estado Comite Con acta aprobada
                    if (comiteTecnico.EstadoActaCodigo == ConstantCodigoActas.Aprobada)
                    {
                        comiteTecnico.EstadoComiteCodigo = ConstanCodigoEstadoComite.Con_Acta_De_Sesion_Aprobada;
                        await EnviarActaAprobada(comiteTecnicoId, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);
                    }

                    //Validar sesionComentario 
                    foreach (var SesionComentario in comiteTecnico.SesionComentario)
                    {
                        SesionComentario.ValidacionVoto = true;
                    }
                }
                _context.SaveChanges();
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

        public async Task<bool> EnviarActaAprobada(int pComiteTecnicoId, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            try
            {
                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

                ComiteTecnico comiteTecnico = _context.ComiteTecnico
                    .Where(r => r.ComiteTecnicoId == pComiteTecnicoId)
                    .Include(r => r.SesionParticipante)
                      .ThenInclude(r => r.Usuario)
                         .ThenInclude(r => r.SesionComentario) 
                    .FirstOrDefault();

                string Tabla = _context.Template.Find((int)enumeratorTemplate.TablaAprobacionParticipanteActa).Contenido;
                string Registros = _context.Template.Find((int)enumeratorTemplate.RegistrosTablaAprobacionParticipanteActa).Contenido;
                string TotalRegistros = string.Empty;

                foreach (var SesionParticipante in comiteTecnico.SesionParticipante)
                {
                    TotalRegistros += Registros;

                    TotalRegistros = TotalRegistros.Replace("[FECHA_APROBACION]", (SesionParticipante.Usuario.SesionComentario.Where(r=> r.EstadoActaVoto == ConstantCodigoActas.Aprobada).Select(r=> r.Fecha).FirstOrDefault()).ToString("dd-MM-yyyy"))
                                  .Replace("[RESPONSABLE]", myTI.ToTitleCase(SesionParticipante.Usuario.Nombres.ToLower() + " "+ SesionParticipante.Usuario.Apellidos.ToLower()));

                }
                Tabla =  Tabla.Replace("[REGISTROS]", TotalRegistros);

                bool blEnvioCorreo = false;
                var usuariosecretario = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Secretario_Comite).Select(x => x.Usuario.Email).ToList();
                 
                foreach (var usuario in usuariosecretario)
                {
                    Template TemplateActaAprobada = await _commonService.GetTemplateById((int)enumeratorTemplate.NotificacionActaAprobacion);
                    string template =
                        TemplateActaAprobada.Contenido
                        .Replace("_LinkF_", pDominioFront)
                        .Replace("[TIPO_COMITE]", (bool)comiteTecnico.EsComiteFiduciario ? ConstanStringTipoComite.Fiduciario : ConstanStringTipoComite.Tecnico)
                        .Replace("[NUMERO_COMITE]", comiteTecnico.NumeroComite) 
                        .Replace("[TABLA_RESPONSABLE_APROBACION]", Tabla)
                        .Replace("[FECHA_COMITE]", ((DateTime.Now).ToString("dd-MM-yyyy")));
                  
                    blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuario, "Aprobación de acta", template, pSender, pPassword, pMailServer, pMailPort);
                }

                return blEnvioCorreo;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private string ValidarEstadoActaVotacion(ComiteTecnico pComiteTecnico)
        {
            string EstadoActa = ConstantCodigoActas.Devuelta;

            if (pComiteTecnico.SesionComentario
                .Where(r => r.EstadoActaVoto == ConstantCodigoActas.Aprobada).Count()
                                            == pComiteTecnico.SesionComentario.Count())
            {
                EstadoActa = ConstantCodigoActas.Aprobada;

            }
            return EstadoActa;
        }

        private bool ValidarTodosVotacion(ComiteTecnico pComiteTecnico)
        {
            return (
                pComiteTecnico.SesionParticipante
                    .Count() == pComiteTecnico.SesionComentario
                    .Where(r => r.EstadoActaVoto != null && !(bool)r.ValidacionVoto)
                    .Select(r => r.MiembroSesionParticipanteId)
                    .Distinct().Count()
                );
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



        //plantilla - Acta de comité técnico
        //Forozco
        public async Task<HTMLContent> GetHTMLString(ActaComite obj)
        {
            try
            {
                var sb = new StringBuilder();
                sb.Append(@"
                        <!DOCTYPE html>
                            <html lang='en'>

                            <head>
                                <meta charset='UTF-8'>
                                <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                                <title>Acta de comité técnico</title>
                            </head>
                           <body>
                                <div style='text-align-last: center; margin-top: 50px; margin-bottom: 30px;'>
                                    <span>
                                    FONDO DE FINANCIAMIENTO DE INFRAESTRUCTURA EDUCATIVA - FFIE
                                    <pre style=' white-space:normal'></pre>
                                    NIT. 900900129-8
                                    <pre style='white-space: normal'></pre>
                                    MINISTERIO DE EDUCACIÓN
                                    </span>
                                </div>

                                <div>
                                    <span style='position: absolute;'>
                                        <img src='' alt=''>
                                    </span>
                                </div>


                                <table style='max-width: 800px;' border='0' width='100%' cellspacing='0' cellpadding='0' align='center'>
                                    <tbody>
                                        <tr>
                                            <td style='font-family: 'Roboto', Helvetica, Arial, sans-serif; font-weight: 300; text-align: center; padding: 5px 10px 5px 10px;background: #E7E6E6; ' align='center '>
                                                <p style='text-decoration: none; color: #000; font-size: 18px; margin-top: 0; font-weight: 700; margin: 0; '>
                                                    Acta de comité técnico
                                                </p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style='font-family: 'Roboto', Helvetica, Arial, sans-serif; font-weight: 300; text-align: left; padding: 16px 10px 5px 10px; '>
                                                <p style='text-decoration: none; color: #000; font-size: 16px; margin-bottom: 5px; margin-top: 0; '>
                                                    Número de comité:
                                                    <span style='color: #FF0000; '>
                                                      NumeroComite
                                                    </span>
                                                </p>
                                                <p style='text-decoration: none; color: #000; font-size: 16px; margin-bottom: 5px; margin-top: 0; '>
                                                    Fecha del comité:
                                                    <span style='color: #FF0000; '>
                                                     FechaComite
                                                    </span>
                                                </p>
                                                <p style='text-decoration: none; color: #000; font-size: 16px; margin-bottom: 5px; margin-top: 0; '>
                                                    Miembros participantes
                                                    <span style='color: #FF0000; '>
                                                        MiembrosParticipantes
                                                    </span>
                                                </p>
                                                <p style='text-decoration: none; color: #000; font-size: 16px; margin-bottom: 5px; margin-top: 0; '>
                                                    Nombre miembro:
                                                    <span style='color: #FF0000; '>
                                                        NombreMiembro
                                                    </span>
                                                </p>
                                                <p style='text-decoration: none; color: #000; font-size: 16px; margin-bottom: 5px; margin-top: 0; '>
                                                    Invitados
                                                    <span style='color: #FF0000; '>
                                                        Invitados
                                                    </span>
                                                </p>
                                                <p style='text-decoration: none; color: #000; font-size: 16px; margin-bottom: 5px; margin-top: 0; '>
                                                    Nombre invitado:
                                                    <span style='color: #FF0000; '>
                                                        NombreInvitado
                                                    </span>
                                                </p>
                                                <p style='text-decoration: none; color: #000; font-size: 16px; margin-bottom: 5px; margin-top: 0; '>
                                                    Cargo:
                                                    <span style='color: #FF0000; '>
                                                        Cargo
                                                    </span>
                                                </p>
                                                <p style='text-decoration: none; color: #000; font-size: 16px; margin-bottom: 5px; margin-top: 0; '>
                                                    Entidad:
                                                    <span style='color: #FF0000; '>
                                                        Entidad
                                                    </span>
                                                </p>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>

                                <table style='max-width: 800px;' border='0' width='100%' cellspacing='0' cellpadding='0' align='center'>
                                    <tbody>
                                        <tr>
                                            <td style='font-family: 'Roboto', Helvetica, Arial, sans-serif; font-weight: 300; text-align: center; padding: 5px 10px 5px 10px;background: #E7E6E6; ' align='center '>
                                                <p style='text-decoration: none; color: #000; font-size: 18px; margin-top: 0; font-weight: 700; margin: 0; '>
                                                    Orden del día
                                                </p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style='font-family: 'Roboto', Helvetica, Arial, sans-serif; font-weight: 300; text-align: left; padding: 16px 10px 5px 10px; '>
                                                <p style='text-decoration: none; color: #000; font-size: 16px; margin-bottom: 5px; margin-top: 0; '>
                                                    mostrar los temas trabajados en la orden del día del comité
                                                </p>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table style='max-width: 800px;' border='0' width='100%' cellspacing='0' cellpadding='0' align='center'>
                                    <tbody>
                                        <tr>
                                            <td style='font-family: 'Roboto', Helvetica, Arial, sans-serif; font-weight: 300; text-align: center; padding: 5px 10px 5px 10px;background: #E7E6E6; ' align='center '>
                                                <p style='text-decoration: none; color: #000; font-size: 18px; margin-top: 0; font-weight: 700; margin: 0; '>
                                                    1. Solicitudes contractuales
                                                </p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style='font-family: 'Roboto', Helvetica, Arial, sans-serif; font-weight: 300; text-align: left; padding: 16px 10px 5px 10px; '>
                                                <p style='text-decoration: none; color: #000; font-size: 16px; margin-bottom: 5px; margin-top: 0; '>
                                                    mostrar los temas trabajados en la orden del día del comité
                                                </p>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </body>

                            </html>");

                obj.htmlContent = sb;
                return new HTMLContent(sb.ToString());
            }
            catch (Exception e)
            {
                return new HTMLContent("");
            }

        }
    }
}
