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

        
        public async Task<ActionResult<List<SesionComiteTecnicoCompromiso>>> GetManagementCommitteeReportById(int SesionComiteTecnicoCompromisoId)
        {
            try//GridComiteTecnicoCompromiso
            {
                return await _context.SesionComiteTecnicoCompromiso.Where(sct => sct.SesionComiteTecnicoCompromisoId == SesionComiteTecnicoCompromisoId && (bool)!sct.Eliminado).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
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
                    Console.WriteLine(SesionComentarioAntiguo);
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
    }
}
