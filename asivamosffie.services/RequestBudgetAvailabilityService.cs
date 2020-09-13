using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using asivamosffie.services.PostParameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class RequestBudgetAvailabilityService : IRequestBudgetAvailabilityService
    {

        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly string _connectionString;



        public RequestBudgetAvailabilityService(devAsiVamosFFIEContext context, ICommonService commonService, IConfiguration configuration)
        {
            _context = context;
            _commonService = commonService;
            _connectionString = configuration.GetConnectionString("asivamosffieDatabase");


        }



        //Avance compromisos
        public async Task<Respuesta> CreateOrEditReportProgress(CompromisoSeguimiento compromisoSeguimiento)
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


        //Aportantes por proyectoId
        public async Task<ActionResult<List<ListAportantes>>> GetAportantesByProyectoId(int proyectoId)
        {
            try
            {


                return await (
                                from cp in _context.ContratacionProyecto
                                join ct in _context.Contratacion on cp.ContratacionId equals ct.ContratacionId
                                join p in _context.Proyecto on cp.ProyectoId equals p.ProyectoId
                                join ctp in _context.ContratacionProyectoAportante on cp.ContratacionProyectoId equals ctp.ContratacionProyectoId
                                join cf in _context.CofinanciacionAportante on ctp.CofinanciacionAportanteId equals cf.CofinanciacionAportanteId
                                where p.ProyectoId == proyectoId 
                                select new ListAportantes
                                {
                                    ContratacionId = ct.ContratacionId,
                                    ContratacionProyectoId = ctp.ContratacionProyectoId,
                                    CofinanciacionAportanteId = cf.CofinanciacionAportanteId,
                                    ContratacionProyectoAportanteId = ctp.ContratacionProyectoAportanteId,
                                    TipoAportanteId = cf.TipoAportanteId,
                                    NombreAportanteId = cf.NombreAportanteId,
                                    NombreAportante = _context.Dominio.Where(r => (bool)r.Activo && r.Codigo.Equals(cf.NombreAportanteId) && r.TipoDominioId == (int)EnumeratorTipoDominio.Nombre_Aportante_Aportante).Select(r => r.Nombre).FirstOrDefault(),
                                    ValorAporte = ctp.ValorAporte,
                                    TipoAportanteText =  _context.Dominio.Where(r => (bool)r.Activo && r.Codigo.Equals(cf.TipoAportanteId) && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_aportante).Select(r => r.Nombre).FirstOrDefault(),
                                }).ToListAsync();



            }
            catch (Exception)
            {

                throw;
            }
        }



        //Registrar informacion Adicional en una solicitud
        public async Task<Respuesta> CreateOrEditInfoAdditional(DisponibilidadPresupuestal pDisponibilidad, string user)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Registrar_Informacion_Adicional_Solicitud, (int)EnumeratorTipoDominio.Acciones);
            DisponibilidadPresupuestal disponibilidadPresupuestal = null;
            string strCrearEditar = string.Empty;

            try
            {
                if (pDisponibilidad.PlazoDias > 30)
                    return respuesta = new Respuesta { IsSuccessful = false, Data = null, Code = ConstantMessagesSesionComiteTema.Error, Message = "El valor ingresado en dias no puede superior a 30" };



                disponibilidadPresupuestal = await _context.DisponibilidadPresupuestal.FindAsync(pDisponibilidad.DisponibilidadPresupuestalId);
                if (disponibilidadPresupuestal != null)
                {
                    pDisponibilidad.UsuarioModificacion = user;
                    pDisponibilidad.FechaModificacion = DateTime.Now;

                    disponibilidadPresupuestal.Objeto = pDisponibilidad.Objeto;
                    disponibilidadPresupuestal.PlazoDias = pDisponibilidad.PlazoDias;
                    disponibilidadPresupuestal.PlazoMeses = pDisponibilidad.PlazoMeses;
                    //disponibilidadPresupuestal.AportanteId = pdo.aportanteId;
                    _context.DisponibilidadPresupuestal.Update(disponibilidadPresupuestal);

                }else{
                    pDisponibilidad.UsuarioCreacion = user;
                    pDisponibilidad.FechaCreacion = DateTime.Now;
                    pDisponibilidad.EstadoSolicitudCodigo = "1";
                    pDisponibilidad.Eliminado = false;

                    _context.DisponibilidadPresupuestal.Add( pDisponibilidad );
                }
                   
                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = disponibilidadPresupuestal,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, user, strCrearEditar)

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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesSesionComiteTema.Error, idAccion, user, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }


        //Ver detalle
        public async Task<DisponibilidadPresupuestal> GetDetailInfoAdditionalById(int disponibilidadPresupuestalId)
        {
            return await _context.DisponibilidadPresupuestal.Where(dp => dp.DisponibilidadPresupuestalId == disponibilidadPresupuestalId).FirstOrDefaultAsync();
        }


        //Registrar nueva solicitud DDp Especial
        public async Task<Respuesta> CreateOrEditDDPRequest(DisponibilidadPresupuestal disponibilidadPresupuestal, int proyectoId, int disponibilidadPresupuestalId)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Solicitud_DDP_Especial, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            DisponibilidadPresupuestal disponibilidadPresupuestalAntiguo = null;
            int countMaxId = _context.DisponibilidadPresupuestal.Max(p => p.DisponibilidadPresupuestalId);

            try
            {

                if (string.IsNullOrEmpty(disponibilidadPresupuestal.DisponibilidadPresupuestalId.ToString()) || disponibilidadPresupuestal.DisponibilidadPresupuestalId == 0)
                {
                    //Auditoria

                    strCrearEditar = "REGISTRAR SOLICITUD DDP ESPECIAL";
                    disponibilidadPresupuestal.FechaCreacion = DateTime.Now;
                    disponibilidadPresupuestal.UsuarioCreacion = disponibilidadPresupuestal.UsuarioCreacion;
                    disponibilidadPresupuestal.Eliminado = false;

                    disponibilidadPresupuestal.NumeroSolicitud = Helpers.Helpers.Consecutive("DE", countMaxId);
                    disponibilidadPresupuestal.OpcionContratarCodigo = "Validando";
                    disponibilidadPresupuestal.ValorSolicitud = 0;
                    disponibilidadPresupuestal.EstadoSolicitudCodigo = "8"; // Sin registrar => TipoDominioId = 39
                    disponibilidadPresupuestal.FechaSolicitud = DateTime.Now;
                    disponibilidadPresupuestal.TipoSolicitudCodigo = disponibilidadPresupuestal.TipoSolicitudCodigo;
                    disponibilidadPresupuestal.Objeto = disponibilidadPresupuestal.Objeto;
                    disponibilidadPresupuestal.NumeroRadicadoSolicitud = disponibilidadPresupuestal.NumeroRadicadoSolicitud;
                    disponibilidadPresupuestal.RegistroCompleto = true;
                   _context.DisponibilidadPresupuestal.Add(disponibilidadPresupuestal);
                    var result = await _context.SaveChangesAsync();

                    if (result > 0)
                    {
                        DisponibilidadPresupuestalProyecto entity = new DisponibilidadPresupuestalProyecto();
                        entity.ProyectoId = proyectoId;
                        entity.DisponibilidadPresupuestalId = disponibilidadPresupuestalId;
                        entity.FechaCreacion = DateTime.Now;
                        entity.UsuarioCreacion = disponibilidadPresupuestal.UsuarioCreacion;
                        entity.Eliminado = false;

                        _context.DisponibilidadPresupuestalProyecto.Add(entity);
                    }

                }
                else
                {
                    strCrearEditar = "EDIT SOLICITUD DDP ESPECIAL";
                    disponibilidadPresupuestalAntiguo = _context.DisponibilidadPresupuestal.Find(disponibilidadPresupuestal.DisponibilidadPresupuestalId);

                    //Auditoria
                    disponibilidadPresupuestalAntiguo.UsuarioModificacion = disponibilidadPresupuestal.UsuarioModificacion;
                    disponibilidadPresupuestalAntiguo.Eliminado = false;


                    //Registros
                    disponibilidadPresupuestal.OpcionContratarCodigo = disponibilidadPresupuestal.OpcionContratarCodigo;
                    disponibilidadPresupuestal.ValorSolicitud = disponibilidadPresupuestal.ValorSolicitud;
                    disponibilidadPresupuestal.NumeroSolicitud = disponibilidadPresupuestal.NumeroSolicitud;
                    disponibilidadPresupuestal.FechaSolicitud = disponibilidadPresupuestal.FechaSolicitud;
                    disponibilidadPresupuestalAntiguo.Objeto = disponibilidadPresupuestal.Objeto;
                    disponibilidadPresupuestal.EstadoSolicitudCodigo = disponibilidadPresupuestal.EstadoSolicitudCodigo;
                    disponibilidadPresupuestalAntiguo.TipoSolicitudCodigo = disponibilidadPresupuestal.TipoSolicitudCodigo;
                    disponibilidadPresupuestalAntiguo.NumeroRadicadoSolicitud = disponibilidadPresupuestal.NumeroRadicadoSolicitud;
                    _context.DisponibilidadPresupuestal.Update(disponibilidadPresupuestalAntiguo);
                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = disponibilidadPresupuestal,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, disponibilidadPresupuestal.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = disponibilidadPresupuestal,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, disponibilidadPresupuestal.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }


        //Registrar Otros Costos/Servicio
        public async Task<Respuesta> CreateOrEditServiceCosts(DisponibilidadPresupuestal disponibilidadPresupuestal, int proyectoId)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Solicitud_DDP_OtrosCostosServicio, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            DisponibilidadPresupuestal disponibilidadPresupuestalAntiguo = null;
            int countMaxId = _context.DisponibilidadPresupuestal.Max(p => p.DisponibilidadPresupuestalId);

            try
            {

                if (string.IsNullOrEmpty(disponibilidadPresupuestal.DisponibilidadPresupuestalId.ToString()) || disponibilidadPresupuestal.DisponibilidadPresupuestalId == 0)
                {
                    //Auditoria

                    strCrearEditar = "REGISTRAR SOLICITUD DDP OTROS COSTOS SERVICIOS";
                    disponibilidadPresupuestal.FechaCreacion = DateTime.Now;
                    disponibilidadPresupuestal.UsuarioCreacion = disponibilidadPresupuestal.UsuarioCreacion;
                    disponibilidadPresupuestal.Eliminado = false;

                    disponibilidadPresupuestal.NumeroSolicitud = Helpers.Helpers.Consecutive("DE", countMaxId);
                    disponibilidadPresupuestal.OpcionContratarCodigo = "Validando";
                    disponibilidadPresupuestal.ValorSolicitud = 0;
                    disponibilidadPresupuestal.TipoSolicitudCodigo = "1";
                    disponibilidadPresupuestal.EstadoSolicitudCodigo = "8"; // Sin registrar => TipoDominioId = 39
                    disponibilidadPresupuestal.FechaSolicitud = DateTime.Now;
                    disponibilidadPresupuestal.Objeto = disponibilidadPresupuestal.Objeto;
                    disponibilidadPresupuestal.NumeroRadicadoSolicitud = disponibilidadPresupuestal.NumeroRadicadoSolicitud;
                    disponibilidadPresupuestal.RegistroCompleto = true;
                    _context.DisponibilidadPresupuestal.Add(disponibilidadPresupuestal);
                    var result = await _context.SaveChangesAsync();

                    var newDisponibilidadPresupuestalId = disponibilidadPresupuestal.DisponibilidadPresupuestalId;

                    if (result > 0)
                    {
                        DisponibilidadPresupuestalProyecto entity = new DisponibilidadPresupuestalProyecto();
                        entity.ProyectoId = proyectoId;
                        entity.DisponibilidadPresupuestalId = newDisponibilidadPresupuestalId;
                        entity.FechaCreacion = DateTime.Now;
                        entity.UsuarioCreacion = disponibilidadPresupuestal.UsuarioCreacion;
                        entity.Eliminado = false;

                        _context.DisponibilidadPresupuestalProyecto.Add(entity);
                    }

                }
                else
                {
                    strCrearEditar = "EDIT SOLICITUD DDP ESPECIAL";
                    disponibilidadPresupuestalAntiguo = _context.DisponibilidadPresupuestal.Find(disponibilidadPresupuestal.DisponibilidadPresupuestalId);

                    //Auditoria
                    disponibilidadPresupuestalAntiguo.UsuarioModificacion = disponibilidadPresupuestal.UsuarioModificacion;
                    disponibilidadPresupuestalAntiguo.Eliminado = false;


                    //Registros
                    disponibilidadPresupuestal.OpcionContratarCodigo = disponibilidadPresupuestal.OpcionContratarCodigo;
                    disponibilidadPresupuestal.ValorSolicitud = disponibilidadPresupuestal.ValorSolicitud;
                    disponibilidadPresupuestal.NumeroSolicitud = disponibilidadPresupuestal.NumeroSolicitud;
                    disponibilidadPresupuestal.FechaSolicitud = disponibilidadPresupuestal.FechaSolicitud;
                    disponibilidadPresupuestalAntiguo.Objeto = disponibilidadPresupuestal.Objeto;
                    disponibilidadPresupuestal.EstadoSolicitudCodigo = disponibilidadPresupuestal.EstadoSolicitudCodigo;
                    disponibilidadPresupuestalAntiguo.TipoSolicitudCodigo = disponibilidadPresupuestal.TipoSolicitudCodigo;
                    disponibilidadPresupuestalAntiguo.NumeroRadicadoSolicitud = disponibilidadPresupuestal.NumeroRadicadoSolicitud;
                    _context.DisponibilidadPresupuestal.Update(disponibilidadPresupuestalAntiguo);
                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = disponibilidadPresupuestal,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, disponibilidadPresupuestal.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = disponibilidadPresupuestal,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, disponibilidadPresupuestal.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }


        //Enviar solicitud
        public async Task<Respuesta> SendRequest(int disponibilidadPresupuestalId)
        {

            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_Solicitud_A_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            DisponibilidadPresupuestal disponibilidadPresupuestal = null;

            try
            {

                disponibilidadPresupuestal = await _context.DisponibilidadPresupuestal.Where(d => d.DisponibilidadPresupuestalId == disponibilidadPresupuestalId).FirstOrDefaultAsync();


                if (disponibilidadPresupuestal != null)
                {

                    strCrearEditar = "ENVIAR SOLICITUD A DOSPONIBILIDAD PRESUPUESAL";
                    disponibilidadPresupuestal.FechaModificacion = DateTime.Now;
                    disponibilidadPresupuestal.UsuarioCreacion = disponibilidadPresupuestal.UsuarioCreacion;
                    disponibilidadPresupuestal.EstadoSolicitudCodigo = "1"; // Se cambia el estado a "En validación presupuestal" => TipoDominioId = 39
                    _context.DisponibilidadPresupuestal.Update(disponibilidadPresupuestal);
                    
                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = disponibilidadPresupuestal,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, disponibilidadPresupuestal.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = disponibilidadPresupuestal,
                    Code = ConstantMessagesSesionComiteTema.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.SesionComiteTema, ConstantMessagesSesionComiteTema.Error, idAccion, disponibilidadPresupuestal.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }


        //Grilla DDP Especial
        public async Task<ActionResult<List<DisponibilidadPresupuestal>>> GetDDPEspecial()
        {
            return await (
                            from dp in _context.DisponibilidadPresupuestal
                            where dp.NumeroSolicitud.Contains("DE_")
                            select new DisponibilidadPresupuestal
                            {
                                DisponibilidadPresupuestalId = dp.DisponibilidadPresupuestalId,
                                FechaSolicitud = dp.FechaSolicitud,
                                NumeroSolicitud = dp.NumeroSolicitud,
                                ValorSolicitud = dp.ValorSolicitud,
                                EstadoSolicitudCodigo = dp.EstadoSolicitudCodigo,
                                RegistroCompleto = dp.RegistroCompleto

                            }).ToListAsync();
        }



        //Solicitudes de comite tecnico
        public async Task<List<CustonReuestCommittee>> GetReuestCommittee()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetBudgetAvailabilityRequest", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<CustonReuestCommittee>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToValue(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public CustonReuestCommittee MapToValue(SqlDataReader reader)
        {
            return new CustonReuestCommittee()
            {
                ContratacionId = (int)reader["ContratacionId"],
                DisponibilidadPresupuestalId = (int)reader["DisponibilidadPresupuestalId"],
                SesionComiteSolicitudId = (int)reader["SesionComiteSolicitudId"],
                FechaSolicitud = (DateTime)reader["FechaSolicitud"],
                TipoSolicitudText = reader["TipoSolicitudText"].ToString(),
                NumeroSolicitud = reader["NumeroSolicitud"].ToString(),
                OpcionContratar = reader["OpcionContratar"].ToString(),
                ValorSolicitud = (decimal)reader["ValorSolicitud"],
                FechaComite = (DateTime)reader["FechaComite"],
                EstadoSolicitudText = reader["EstadoSolicitudText"].ToString(),

            };
        }


        public async Task<ActionResult<List<Proyecto>>> SearchLlaveMEN(string LlaveMEN)
        {
            var Id = await _context.Proyecto.Where(r => r.LlaveMen == LlaveMEN.ToString().Trim() && !(bool)r.Eliminado).Select(r => r.LlaveMen).FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(Id))
                return await _context.Proyecto.Where(r => r.LlaveMen.Equals(Id)).ToListAsync();

            else
                throw new Exception("Esta llave no existe, por favor verificar los datos registrados");

        }


        public Task<HTMLContent> GetHTMLString(DetailValidarDisponibilidadPresupuesal detailValidarDisponibilidadPresupuesal)
        {
            throw new NotImplementedException("No implementado");
        }
    }
}
