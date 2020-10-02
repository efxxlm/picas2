﻿using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

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

            string StrSql = "SELECT ComiteTecnico.* FROM  dbo.ComiteTecnico INNER JOIN dbo.SesionParticipante  ON   ComiteTecnico.ComiteTecnicoId = SesionParticipante.ComiteTecnicoId WHERE  SesionParticipante.UsuarioId = " + pUserId+" AND   ComiteTecnico.Eliminado = 0 AND  SesionParticipante.Eliminado = 0";
            List<ComiteTecnico> ListComiteTecnico = await _context.ComiteTecnico.FromSqlRaw(StrSql)
                .Where(r => r.EstadoActaCodigo == ConstantCodigoActas.En_proceso_Aprobacion
                       && r.EstadoComiteCodigo == ConstanCodigoEstadoComite.Con_Acta_De_Sesion_Enviada) 
                 .Include(r => r.SesionParticipante)
                         .Include(r => r.SesionComentario)
                  .ToListAsync();

            foreach (var ComiteTecnico in ListComiteTecnico)
            {
                try
                { 
                    GrillaSesionComiteTecnicoCompromiso grillaSesionComiteTecnicoCompromiso = new GrillaSesionComiteTecnicoCompromiso
                    {
                        ComiteTecnicoId = ComiteTecnico.ComiteTecnicoId,
                        FechaComite = ComiteTecnico.FechaOrdenDia,
                        NumeroComite = ComiteTecnico.NumeroComite
                    };
                    if (ComiteTecnico.SesionComiteTecnicoCompromiso.Count() > 0)
                    {
                        grillaSesionComiteTecnicoCompromiso.SesionComiteTecnicoCompromisoId = ComiteTecnico.SesionComiteTecnicoCompromiso.FirstOrDefault().SesionComiteTecnicoCompromisoId;
                        grillaSesionComiteTecnicoCompromiso.Compromiso = ComiteTecnico.SesionComiteTecnicoCompromiso.FirstOrDefault().Tarea;
                        grillaSesionComiteTecnicoCompromiso.FechaCumplimiento = ComiteTecnico.SesionComiteTecnicoCompromiso.FirstOrDefault().FechaCumplimiento;
                        grillaSesionComiteTecnicoCompromiso.EstadoCodigo = ComiteTecnico.SesionComiteTecnicoCompromiso.FirstOrDefault().EstadoCodigo;
                    }
                    grillaSesionComiteTecnicoCompromisos.Add(grillaSesionComiteTecnicoCompromiso);
                }
                catch (Exception ex)
                {

                }
            }
            return grillaSesionComiteTecnicoCompromisos;
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
            string StrSql = "SELECT ComiteTecnico.* FROM  dbo.ComiteTecnico INNER JOIN dbo.SesionParticipante  ON   ComiteTecnico.ComiteTecnicoId = SesionParticipante.ComiteTecnicoId WHERE  SesionParticipante.UsuarioId = " + pUserId + " AND   ComiteTecnico.Eliminado = 0 AND  SesionParticipante.Eliminado = 0";
 
            return await _context.ComiteTecnico.FromSqlRaw(StrSql)
                      .Include(r => r.SesionComiteTecnicoCompromiso)
                      .Include(r => r.SesionComiteSolicitudComiteTecnico)
                      .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario).OrderByDescending(r=> r.ComiteTecnicoId)
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
                        .Include(r => r.SesionComiteTecnicoCompromiso) 
                        .Include(r => r.SesionComiteSolicitudComiteTecnico)
                           .ThenInclude(r => r.SesionSolicitudCompromiso)
                        .Include(r => r.SesionComiteSolicitudComiteTecnicoFiduciario)
                        .ToListAsync();

                List<Dominio> ListParametricas = _context.Dominio.ToList();

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
                    strCrearEditar = "REGISTRAR AVANCE COMPROMISOS";
                    compromisoSeguimiento.FechaCreacion = DateTime.Now;
                    compromisoSeguimiento.UsuarioCreacion = compromisoSeguimiento.UsuarioCreacion;
                    //compromisoSeguimiento.SesionParticipanteId = compromisoSeguimiento.SesionParticipanteId;
                    compromisoSeguimiento.Eliminado = false;
                    _context.CompromisoSeguimiento.Add(compromisoSeguimiento);

                }
                else
                {
                    strCrearEditar = "EDITAR AVANCE COMPROMISOS";
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
         
                //comiteTecnicoOld.EstadoActaCodigo = ConstantCodigoActas.Devuelta;
             //   comiteTecnicoOld.UsuarioModificacion = SesionComentario.UsuarioCreacion;
               // comiteTecnicoOld.FechaModificacion = DateTime.Now;
                 
                string strCrearEditar;
                if (string.IsNullOrEmpty(SesionComentario.SesionComentarioId.ToString()) || SesionComentario.SesionComentarioId == 0)
                {
                    //Auditoria
                    strCrearEditar = "COMENTAR Y DEVOLVER ACTA";
                    SesionComentario.Fecha = DateTime.Now;
                    SesionComentario.FechaCreacion = DateTime.Now;
                    SesionComentario.UsuarioCreacion = SesionComentario.UsuarioCreacion;

                    _context.SesionComentario.Add(SesionComentario);
                }
                else
                {
                    strCrearEditar = "EDITAR COMENTAR ACTA";
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
        public async Task<Respuesta> AcceptReport(int comiteTecnicoId, Usuario pUser)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Aprobar_Acta, (int)EnumeratorTipoDominio.Acciones);

            //Cambiar estados cuando se crea y todos los sesion particiantes que esten asociados al comite si ya todos comentarios y que ayan estado del acta aprobado el
            string strCrearEditar = string.Empty;
            try
            {
                ComiteTecnico comiteTecnico = await _context.ComiteTecnico.Where(r=> r.ComiteTecnicoId == comiteTecnicoId)
                     .Include(r=> r.SesionComentario)
                     .Include(r=> r.SesionParticipante)
                     .FirstOrDefaultAsync();

                SesionComentario sesionComentario = new SesionComentario
                {
                    Fecha = DateTime.Now,
                    Observacion = "APROBADA POR " + pUser.Email,
                    FechaCreacion = DateTime.Now,
                    UsuarioCreacion = pUser.Email,
                    ComiteTecnicoId = comiteTecnicoId,
                    MiembroSesionParticipanteId = pUser.UsuarioId,
                    EstadoActaVoto = ConstantCodigoActas.Aprobada,
                    ValidacionVoto = false
                };
                _context.SesionComentario.Add(sesionComentario);


                //ValidarVotacion
                if ((bool)ValidarTodosVotacion(comiteTecnico))
                {
                    comiteTecnico.EstadoActaCodigo = ValidarEstadoActaVotacion(comiteTecnico);

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
                    Data = comiteTecnico,
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
                    Data = null,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, pUser.Email, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        private string ValidarEstadoActaVotacion(ComiteTecnico pComiteTecnico)
        {
            string EstadoActa = ConstantCodigoActas.Devuelta;

            if (pComiteTecnico.SesionComentario.Where(r => r.EstadoActaVoto == ConstantCodigoActas.Aprobada).Count()
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
                    SesionComiteTecnicoCompromiso sesionComiteTecnicoCompromiso = await _context.SesionComiteTecnicoCompromiso.Where(up => up.SesionComiteTecnicoCompromisoId == sesionComiteTecnicoCompromisoId && (bool)!up.Eliminado).FirstOrDefaultAsync();
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


        //plantilla - Acta de comité técnico
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
