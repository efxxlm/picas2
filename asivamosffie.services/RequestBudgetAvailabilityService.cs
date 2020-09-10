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


        //Detalle solicitud por id
        public async Task<ActionResult<List<ListAportantes>>> GetAportantesByProyectoId(int proyectoId)
        {
            try
            {
                return await (
                                from ctp in _context.ContratacionProyecto
                                join ct in _context.Contratacion on ctp.ContratacionId equals ct.ContratacionId
                                join p in _context.Proyecto on ctp.ProyectoId equals p.ProyectoId
                                join ca in _context.ContratacionProyectoAportante on ctp.ContratacionProyectoId equals ca.ContratacionProyectoId
                                join cf in _context.CofinanciacionAportante on ca.CofinanciacionAportanteId equals cf.CofinanciacionAportanteId
                                where p.ProyectoId == proyectoId 
                                select new ListAportantes
                                { 
                                    
                                }).ToListAsync();



            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<ActionResult<List<SesionComiteTecnicoCompromiso>>> GetManagementCommitteeReport()
        {
            throw new NotImplementedException();
        }


        //Registrar informacion Adicional en una solicitud
        public async Task<Respuesta> CreateOrEditInfoAdditional(PostParameter postParameter, string user)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Registrar_Informacion_Adicional_Solicitud, (int)EnumeratorTipoDominio.Acciones);
            DisponibilidadPresupuestal disponibilidadPresupuestal = null;
            string strCrearEditar = string.Empty;

            try
            {
                if (postParameter.plazoDias > 30)
                    return respuesta = new Respuesta { IsSuccessful = false, Data = null, Code = ConstantMessagesSesionComiteTema.Error, Message = "El valor ingresado en dias no puede superior a 30" };



                disponibilidadPresupuestal = await _context.DisponibilidadPresupuestal.FindAsync(postParameter.solicitudId);
                if (disponibilidadPresupuestal != null)
                {
                    disponibilidadPresupuestal.Objeto = postParameter.Objeto;
                    disponibilidadPresupuestal.PlazoDias = postParameter.plazoDias;
                    disponibilidadPresupuestal.PlazoMeses = postParameter.plazoMeses;
                    disponibilidadPresupuestal.AportanteId = postParameter.aportanteId;
                    _context.DisponibilidadPresupuestal.Update(disponibilidadPresupuestal);

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

            };
        }

        public Task<HTMLContent> GetHTMLString(DetailValidarDisponibilidadPresupuesal detailValidarDisponibilidadPresupuesal)
        {
            throw new NotImplementedException("No implementado");
        }
    }
}
