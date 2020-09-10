
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class BudgetAvailabilityService : IBudgetAvailabilityService
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        public BudgetAvailabilityService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }

        public async Task<List<GrillaDisponibilidadPresupuestal2>> GetGrillaDisponibilidadPresupuestal2(string pConnectionStrings)
        {
            List<GrillaDisponibilidadPresupuestal2> ListaGrillaDisponibilidadPresupuestal = new List<GrillaDisponibilidadPresupuestal2>();
     
            using (SqlConnection sql = new SqlConnection(pConnectionStrings))
            {
                using SqlCommand cmd = new SqlCommand("sp_GetGrillaDisponibilidadPresupuestal", sql)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                }; 
                await sql.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                { 
                    while (await reader.ReadAsync())
                    {
                        ListaGrillaDisponibilidadPresupuestal.Add(await MapRespuestaAsync(reader));
                    }
                }
                await sql.CloseAsync();
            } 
            return ListaGrillaDisponibilidadPresupuestal;
        }

        private async Task<GrillaDisponibilidadPresupuestal2> MapRespuestaAsync(SqlDataReader reader)
        {
            return new GrillaDisponibilidadPresupuestal2()
            {
                SesionComiteTecnicoId = (int)reader["SesionComiteTecnicoId"],
                ValorUso = reader["ValorUso"].ToString(),
                FechaSolicitud = reader["FechaSolicitud"].ToString() != null ? Convert.ToDateTime(reader["FechaSolicitud"].ToString()).ToString("yyyy-MM-dd") : " ",
                NumeroSolicitud = reader["NumeroSolicitud"].ToString(),
                TipoSolicitud = await _commonService.GetNombreDominioByCodigoAndTipoDominio(reader["TipoSolicitudCodigo"].ToString(), (int)EnumeratorTipoDominio.Tipo_Solicitud)
            };
        }


        #region "Servicios Disponibilidad Presupuestal";
        public async Task<List<DisponibilidadPresupuestal>> GetBudgetAvailability()
        {
            try
            {
                return await _context.DisponibilidadPresupuestal.Where(r => !r.Eliminado)
                                    
                                    .ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<DisponibilidadPresupuestal> GetBudgetAvailabilityById(int id)
        {
            try
            {
                return await _context.DisponibilidadPresupuestal.Where( d => d.DisponibilidadPresupuestalId == id) 
                                    .Include( r => r.DisponibilidadPresupuestalProyecto )
                                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /**
         * Listado de las solitudes de sesión de comité técnico:  En estado (APROBADA POR COMITÉ TECNICO)
         */
        public async Task<ActionResult<List<GrillaDisponibilidadPresupuestal>>> GetGridBudgetAvailability(int? DisponibilidadPresupuestalId)
        {

            List<DisponibilidadPresupuestal> ListDisponibilidadPresupuestal =
               DisponibilidadPresupuestalId != null ? await _context.DisponibilidadPresupuestal.Where(x => x.DisponibilidadPresupuestalId == DisponibilidadPresupuestalId).ToListAsync()
               : await _context.DisponibilidadPresupuestal.OrderByDescending(a => a.FechaSolicitud).ToListAsync();

            List<GrillaDisponibilidadPresupuestal> ListGrillaControlCronograma = new List<GrillaDisponibilidadPresupuestal>();

            foreach (var dp in ListDisponibilidadPresupuestal)
            {
                GrillaDisponibilidadPresupuestal DisponibilidadPresupuestalGrilla = new GrillaDisponibilidadPresupuestal
                {
                    DisponibilidadPresupuestalId = dp.DisponibilidadPresupuestalId,
                    FechaSolicitud = dp.FechaSolicitud,
                    TipoSolicitudCodigo = dp.TipoSolicitudCodigo,
                    TipoSolicitudText = dp.TipoSolicitudCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(dp.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_Solicitud) : "",
                    NumeroSolicitud = dp.NumeroSolicitud,
                    OpcionPorContratarCodigo = dp.OpcionContratarCodigo,
                    OpcionPorContratarText = dp.OpcionContratarCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(dp.OpcionContratarCodigo, (int)EnumeratorTipoDominio.Opcion_Por_Contratar) : "",
                    ValorSolicitado = dp.ValorSolicitud,
                    EstadoSolicitudCodigo = string.Empty, // Pendiente hacer la validacion
                    EstadoRegistro = dp.RegistroCompleto.Equals(true) ? "Completo" : ""
                    
                };

                ListGrillaControlCronograma.Add(DisponibilidadPresupuestalGrilla);
            }

            return ListGrillaControlCronograma;
        }

        public async Task<Respuesta> CreateEditarDisponibilidadPresupuestal(DisponibilidadPresupuestal DP)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = "";
            DisponibilidadPresupuestal disponibilidadPresupuestalAntiguo = null;
            try
            {

                if (string.IsNullOrEmpty(DP.DisponibilidadPresupuestalId.ToString()) || DP.DisponibilidadPresupuestalId == 0)
                {
                    //Concecutivo
                    var LastRegister = _context.DisponibilidadPresupuestal.OrderByDescending(x => x.DisponibilidadPresupuestalId).First().DisponibilidadPresupuestalId;


                    //Auditoria
                    strCrearEditar = "CREAR DISPONIBILIDAD PRESUPUESTAL";
                    DP.FechaCreacion = DateTime.Now;
                    DP.Eliminado = false;

                    //DP.NumeroDdp = ""; TODO: traer consecutivo del modulo de proyectos, DDP_PI_autoconsecutivo
                    DP.EstadoSolicitudCodigo = "4"; // Sin registrar

                    _context.DisponibilidadPresupuestal.Add(DP);
                    return respuesta = new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Data = DP,
                        Code = ConstantMessagesDisponibilidadPresupuesta.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesDisponibilidadPresupuesta.OperacionExitosa, idAccion, DP.UsuarioCreacion, strCrearEditar)
                    };

                }
                else
                {
                    strCrearEditar = "EDITAR DISPONIBILIDAD PRESUPUESTAL PROYECTO";
                    disponibilidadPresupuestalAntiguo = _context.DisponibilidadPresupuestal.Find(DP.DisponibilidadPresupuestalId);
                    //Auditoria
                    disponibilidadPresupuestalAntiguo.UsuarioModificacion = "jsorozco";
                    disponibilidadPresupuestalAntiguo.FechaModificacion = DateTime.Now;

                    //Registros
                    disponibilidadPresupuestalAntiguo.FechaSolicitud = DP.FechaSolicitud;
                    disponibilidadPresupuestalAntiguo.TipoSolicitudCodigo = DP.TipoSolicitudCodigo;
                    disponibilidadPresupuestalAntiguo.NumeroSolicitud = DP.NumeroSolicitud;
                    disponibilidadPresupuestalAntiguo.OpcionContratarCodigo = DP.OpcionContratarCodigo;
                    disponibilidadPresupuestalAntiguo.ValorSolicitud = DP.ValorSolicitud;
                    disponibilidadPresupuestalAntiguo.EstadoSolicitudCodigo = DP.EstadoSolicitudCodigo;
                    disponibilidadPresupuestalAntiguo.Objeto = DP.Objeto;
                    disponibilidadPresupuestalAntiguo.FechaDdp = DP.FechaDdp;
                    disponibilidadPresupuestalAntiguo.NumeroDdp = DP.NumeroDdp;
                    disponibilidadPresupuestalAntiguo.RutaDdp = DP.RutaDdp;
                    disponibilidadPresupuestalAntiguo.Eliminado = false;

                    _context.DisponibilidadPresupuestal.Update(disponibilidadPresupuestalAntiguo);
                }

                await _context.SaveChangesAsync();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = disponibilidadPresupuestalAntiguo,
                    Code = ConstantMessagesDisponibilidadPresupuesta.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Cronograma, ConstantMessagesDisponibilidadPresupuesta.OperacionExitosa, idAccion, DP.UsuarioCreacion, strCrearEditar)
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
                    Code = ConstantMessagesDisponibilidadPresupuesta.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Cronograma, ConstantMessagesDisponibilidadPresupuesta.Error, idAccion, DP.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> DeleteBudgetAvailability(int id, string UsuarioModifico)
        {

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                var entity = await _context.DisponibilidadPresupuestal.FindAsync(id);
                entity.FechaModificacion = DateTime.Now;
                entity.UsuarioModificacion = UsuarioModifico;
                entity.Eliminado = true;

                _context.Update(entity);
                await _context.SaveChangesAsync();

                return
                      new Respuesta
                      {
                          IsSuccessful = true,
                          IsException = false,
                          IsValidation = false,
                          Code = ConstantMessagesFuentesFinanciacion.OperacionExitosa,
                          Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesFuentesFinanciacion.OperacionExitosa, idAccion, UsuarioModifico, "ELIMINAR DISPONIBILIDAD PRESUPUESTAL")
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
                     Code = ConstantMessagesFuentesFinanciacion.Error,
                     Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Fuentes, ConstantMessagesFuentesFinanciacion.Error, idAccion, UsuarioModifico, ex.InnerException.ToString().Substring(0, 500))
                 };
            }
        }

        #endregion

        #region "Servicios Disponibilidad Presupuestal Proyecto";

        public async Task<Respuesta> CreateEditarDPProyecto(DisponibilidadPresupuestalProyecto dpProyecto)
        {
            Respuesta respuesta = new Respuesta();

            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_DP_Proyecto, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = "";
            DisponibilidadPresupuestalProyecto DPProyectoAntiguo = null;
            try
            {

                if (string.IsNullOrEmpty(dpProyecto.DisponibilidadPresupuestalProyectoId.ToString()) || dpProyecto.DisponibilidadPresupuestalProyectoId == 0)
                {

                    //Auditoria
                    strCrearEditar = "CREAR DISPONIBILIDAD PRESUPUESTAL PROYECTO";
                    dpProyecto.FechaCreacion = DateTime.Now;
                    dpProyecto.UsuarioCreacion = dpProyecto.UsuarioCreacion;
                    dpProyecto.Eliminado = false;



                    _context.DisponibilidadPresupuestalProyecto.Add(dpProyecto);
                    return respuesta = new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Data = dpProyecto,
                        Code = ConstantMessagesDisponibilidadPresupuesta.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesDisponibilidadPresupuesta.OperacionExitosa, idAccion, dpProyecto.UsuarioCreacion, strCrearEditar)
                    };

                }
                else
                {
                    strCrearEditar = "EDITAR DISPONIBILIDAD PRESUPUESTAL PROYECTO";
                    DPProyectoAntiguo = _context.DisponibilidadPresupuestalProyecto.Find(dpProyecto.DisponibilidadPresupuestalProyectoId);
                    //Auditoria
                    DPProyectoAntiguo.UsuarioModificacion = dpProyecto.UsuarioCreacion;
                    DPProyectoAntiguo.FechaModificacion = DateTime.Now;

                    //Registros
                    DPProyectoAntiguo.DisponibilidadPresupuestalId = dpProyecto.DisponibilidadPresupuestalId;
                    DPProyectoAntiguo.ProyectoId = dpProyecto.ProyectoId;
                    DPProyectoAntiguo.UsuarioModificacion = dpProyecto.UsuarioModificacion;
                    DPProyectoAntiguo.FechaModificacion = DateTime.Now;
                    DPProyectoAntiguo.Eliminado = false;


                    _context.DisponibilidadPresupuestalProyecto.Update(DPProyectoAntiguo);
                }

                await _context.SaveChangesAsync();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = DPProyectoAntiguo,
                    Code = ConstantMessagesDisponibilidadPresupuesta.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Cronograma, ConstantMessagesDisponibilidadPresupuesta.OperacionExitosa, idAccion, DPProyectoAntiguo.UsuarioCreacion, strCrearEditar)
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
                    Code = ConstantMessagesDisponibilidadPresupuesta.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Procesos_Seleccion_Cronograma, ConstantMessagesDisponibilidadPresupuesta.Error, idAccion, DPProyectoAntiguo.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }




        /**
         * Listado proyectos asociados
        */
        public async Task<List<DisponibilidadPresupuestalProyecto>> GetAssociatedProjects(int ProyectoId)
        {
            try
            {
                return await _context.DisponibilidadPresupuestalProyecto.Where(p => p.ProyectoId == ProyectoId && !(bool)p.Eliminado)
                    .Include(p => p.DisponibilidadPresupuestal).IncludeFilter(p => p.Proyecto).Where(p => p.ProyectoId == ProyectoId && !(bool)p.Eliminado).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion
    }
}