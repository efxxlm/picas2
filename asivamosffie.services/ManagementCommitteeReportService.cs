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



    public class ManagementCommitteeReportService: IManagementCommitteeReportService
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



        //GESTION DE COMPROMISOS
        public async Task<ActionResult<List<GrillaSesionComiteTecnicoCompromiso>>> GetManagementCommitteeReport()
        {
            try
            {
                
                return await (from a in _context.SesionComiteTecnicoCompromiso
                                join s in _context.ComiteTecnico on a.ComiteTecnicoId equals s.ComiteTecnicoId
                                where a.Eliminado != true
                                select new GrillaSesionComiteTecnicoCompromiso
                                {
                                    ComiteTecnicoId = s.ComiteTecnicoId,
                                    FechaOrdenDia = s.FechaOrdenDia,
                                    NumeroComite = s.NumeroComite,
                                    Tarea = a.Tarea,
                                    SesionComiteTecnicoCompromisoId = a.SesionComiteTecnicoCompromisoId,
                                    FechaCumplimiento = a.FechaCumplimiento,
                                    EstadoCodigo = a.EstadoCodigo,
                                    EstadoCompromisoText =  _context.Dominio.Where(r => (bool)r.Activo && r.Codigo.Equals(a.EstadoCodigo) && r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Compromiso).Select(r => r.Nombre).FirstOrDefault(),
                                    EstadoComiteCodigo = s.EstadoComiteCodigo,
                                    EstadoComiteText = _context.Dominio.Where(r => (bool)r.Activo && r.Codigo.Equals(s.EstadoComiteCodigo) && r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Comite).Select(r => r.Nombre).FirstOrDefault(),

                                }).ToListAsync();



            }
            catch (Exception)
            {

                throw;
            }
        }



        //Detalle gestion compromisos
        public async Task<ActionResult<List<GrillaSesionComiteTecnicoCompromiso>>> GetCompromisoSeguimientoById(int compromisoSeguimientoId)
        {
            return await (from a in _context.CompromisoSeguimiento
                          join s in _context.SesionComiteTecnicoCompromiso on a.SesionComiteTecnicoCompromisoId equals s.SesionComiteTecnicoCompromisoId
                          where a.Eliminado != true && a.CompromisoSeguimientoId == compromisoSeguimientoId
                          select new GrillaSesionComiteTecnicoCompromiso
                          {
                              CompromisoSeguimientoId = a.CompromisoSeguimientoId,
                              DescripcionSeguimiento = a.DescripcionSeguimiento,
                              EstadoCodigo = s.EstadoCodigo
                          }).ToListAsync();
        }



        //Detalle gestion compromisos
        public async Task<ActionResult<List<GrillaSesionComiteTecnicoCompromiso>>> GetManagementCommitteeReportById(int comiteTecnicoId)
        {
            try
            {

                return await (from a in _context.SesionComiteTecnicoCompromiso
                              join s in _context.ComiteTecnico on a.ComiteTecnicoId equals s.ComiteTecnicoId
                              join r in _context.CompromisoSeguimiento on a.ComiteTecnicoId equals r.SesionComiteTecnicoCompromisoId
                              where a.Eliminado != true && a.ComiteTecnicoId == comiteTecnicoId
                              select new GrillaSesionComiteTecnicoCompromiso
                              {
                                  ComiteTecnicoId = s.ComiteTecnicoId,
                                  SesionComiteTecnicoCompromisoId = a.SesionComiteTecnicoCompromisoId,
                                  DescripcionSeguimiento = r.DescripcionSeguimiento,
                                  CompromisoSeguimientoId = r.CompromisoSeguimientoId,
                                  EstadoCodigo = a.EstadoCodigo,
                                  NumeroComite = s.NumeroComite,
                                  Tarea = a.Tarea,
                                  FechaCumplimiento = a.FechaCumplimiento,
                                  EstadoCompromisoText = _context.Dominio.Where(r => (bool)r.Activo && r.Codigo.Equals(a.EstadoCodigo) && r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Compromiso).Select(r => r.Nombre).FirstOrDefault(),
                              }).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }


        //Gestion de actas
        public async Task<ActionResult<List<ComiteTecnico>>> GetManagementReport()
        {

            return await (from a in _context.SesionComiteTecnicoCompromiso
                         join s in _context.ComiteTecnico on a.ComiteTecnicoId equals s.ComiteTecnicoId
                         join sc in _context.SesionComiteSolicitud on a.ComiteTecnicoId equals sc.ComiteTecnicoId
                         where s.Eliminado != true
                         select new ComiteTecnico
                         {
                             ComiteTecnicoId = s.ComiteTecnicoId,
                             FechaCreacion = s.FechaCreacion,
                             NumeroComite = s.NumeroComite,
                             EstadoComiteCodigo = sc.EstadoActaCodigo != null ? _context.Dominio.Where(r => (bool)r.Activo && r.Codigo.Equals(sc.EstadoActaCodigo) && r.TipoDominioId == (int)EnumeratorTipoDominio.EstadoAcataComiteTecnico).Select(r => r.Nombre).FirstOrDefault() : "Sin revision",

                         }).ToListAsync();
        }



        //Detalle gestion de actas
        public async Task<ActionResult<List<ComiteTecnico>>> GetManagementReportById(int comiteTecnicoId)
        {
            try
            {

                return await (from a in _context.SesionComiteTecnicoCompromiso
                              join s in _context.ComiteTecnico on a.ComiteTecnicoId equals s.ComiteTecnicoId
                              join sc in _context.SesionComiteSolicitud on a.ComiteTecnicoId equals sc.ComiteTecnicoId
                              where s.Eliminado != true && sc.ComiteTecnicoId == comiteTecnicoId
                              select new ComiteTecnico
                              {
                                  ComiteTecnicoId = s.ComiteTecnicoId,
                                  FechaCreacion = s.FechaCreacion,
                                  NumeroComite = s.NumeroComite,
                                  EstadoComiteCodigo = sc.EstadoActaCodigo != null ? _context.Dominio.Where(r => (bool)r.Activo && r.Codigo.Equals(sc.EstadoActaCodigo) && r.TipoDominioId == (int)EnumeratorTipoDominio.EstadoAcataComiteTecnico).Select(r => r.Nombre).FirstOrDefault() : "Sin revision",

                              }).ToListAsync();



            }
            catch (Exception)
            {

                throw;
            }
        }




        //Reportar Avance Compromisos
        public async Task<Respuesta> CreateOrEditReportProgress(CompromisoSeguimiento compromisoSeguimiento, string estadoCompromiso)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Seguimiento_Compromiso, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            CompromisoSeguimiento compromisoSeguimientoAntiguo = null;
            try
            {

                if (string.IsNullOrEmpty(compromisoSeguimiento.CompromisoSeguimientoId.ToString()) || compromisoSeguimiento.CompromisoSeguimientoId == 0)
                {
                    //Auditoria
                    strCrearEditar = "REGISTRAR AVANCE COMPROMISOS";
                    compromisoSeguimiento.FechaCreacion = DateTime.Now;
                    compromisoSeguimiento.UsuarioCreacion = compromisoSeguimiento.UsuarioCreacion;
                    compromisoSeguimiento.SesionParticipanteId = compromisoSeguimiento.SesionParticipanteId;
                    compromisoSeguimiento.Eliminado = false;
                    _context.CompromisoSeguimiento.Add(compromisoSeguimiento);
                    var result = await _context.SaveChangesAsync();

                    if (result > 0)
                       await UpdateStatus(compromisoSeguimiento.SesionComiteTecnicoCompromisoId, estadoCompromiso);
                }
                else
                {
                    strCrearEditar = "EDIT AVANCE COMPROMISOS";
                    compromisoSeguimientoAntiguo = _context.CompromisoSeguimiento.Find(compromisoSeguimiento.CompromisoSeguimientoId);

                    //Auditoria
                    compromisoSeguimientoAntiguo.UsuarioModificacion = compromisoSeguimiento.UsuarioModificacion;
                    compromisoSeguimientoAntiguo.Eliminado = false;


                    //Registros
                    compromisoSeguimientoAntiguo.DescripcionSeguimiento = compromisoSeguimiento.DescripcionSeguimiento;
                    compromisoSeguimientoAntiguo.TemaCompromisoId = compromisoSeguimiento.TemaCompromisoId;
                    compromisoSeguimientoAntiguo.SesionParticipanteId = compromisoSeguimiento.SesionParticipanteId;
                    _context.CompromisoSeguimiento.Update(compromisoSeguimiento);

                }

                return respuesta = new Respuesta
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
                return respuesta = new Respuesta
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
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Comentario_Acta, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            SesionComentario SesionComentarioAntiguo = null;
            try
            {

                if (string.IsNullOrEmpty(SesionComentario.SesionComentarioId.ToString()) || SesionComentario.SesionComentarioId == 0)
                {
                    //Auditoria
                    strCrearEditar = "COMENTAR Y DEVOLVER ACTA";
                    SesionComentario.FechaCreacion = DateTime.Now;
                    SesionComentario.UsuarioCreacion = SesionComentario.UsuarioCreacion;

                    _context.SesionComentario.Add(SesionComentario);

                }
                else
                {
                    strCrearEditar = "EDIT COMENTAR ACTA";
                    SesionComentarioAntiguo = _context.SesionComentario.Find(SesionComentario.SesionComentarioId);

                    //Auditoria
                    SesionComentarioAntiguo.UsuarioModificacion = SesionComentario.UsuarioModificacion;
                    SesionComentarioAntiguo.FechaModificacion = DateTime.Now;


                    //Registros
                    SesionComentarioAntiguo.Fecha = SesionComentario.Fecha;
                    SesionComentarioAntiguo.Observacion = SesionComentario.Observacion;
                    _context.SesionComentario.Update(SesionComentarioAntiguo);

                }

                return respuesta = new Respuesta
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
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = SesionComentario,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, SesionComentario.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }


        //Actualizar estado codigo de un compromiso
        public async Task<bool> UpdateStatus(int sesionComiteTecnicoCompromisoId, string status)
        {
            try
            {
                if(!string.IsNullOrEmpty(Convert.ToString(sesionComiteTecnicoCompromisoId)) || !string.IsNullOrEmpty(status))
                {
                    SesionComiteTecnicoCompromiso sesionComiteTecnicoCompromiso = await _context.SesionComiteTecnicoCompromiso.Where(up=> up.SesionComiteTecnicoCompromisoId == sesionComiteTecnicoCompromisoId && (bool)!up.Eliminado).FirstOrDefaultAsync();
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
