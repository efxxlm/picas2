﻿using asivamosffie.model.APIModels;
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
using System.Data;
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

        public async Task<List<ListAportantes>> GetAportantesByProyectoId(int proyectoId)
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
                                    NombreAportante = _context.Dominio.Where(r => (bool)r.Activo &&
                                                                             r.DominioId.Equals(cf.NombreAportanteId) &&
                                                                             r.TipoDominioId == (int)EnumeratorTipoDominio.Nombre_Aportante_Aportante)
                                                                       .Select(r => r.Nombre).FirstOrDefault(),
                                    ValorAporte = ctp.ValorAporte,
                                    TipoAportanteText = _context.Dominio.Where(r => (bool)r.Activo && r.DominioId.Equals(cf.TipoAportanteId) && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_aportante).Select(r => r.Nombre).FirstOrDefault(),
                                }).ToListAsync();



            }
            catch (Exception)
            {

                throw;
            }
        }




        //Aportantes por proyectoId
        public async Task<List<ListAdminProyect>> GetAportantesByProyectoAdministrativoId(int proyectoId)
        {
            try
            {

                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetAportantesByProyectoId", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ProyectoId", SqlDbType.Int).Value = proyectoId;
                        var response = new List<ListAdminProyect>();
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToValueProyectoAdmnistrativo(reader));
                            }
                        }

                        return response;
                    }
                }



            }
            catch (Exception)
            {

                throw;
            }
        }




        //Crear solciitud proyecto administrativo
        public async Task<Respuesta> CreateOrEditProyectoAdministrtivo(DisponibilidadPresupuestal disponibilidad)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Seguimiento_Compromiso, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            DisponibilidadPresupuestal disponibilidadPresupuestalAntiguo = null;
            int countMaxId = _context.DisponibilidadPresupuestal.Max(p => p.DisponibilidadPresupuestalId);
            try
            {

                if (string.IsNullOrEmpty(disponibilidad.DisponibilidadPresupuestalId.ToString()) || disponibilidad.DisponibilidadPresupuestalId == 0)
                {
                    //Auditoria
                    strCrearEditar = "CREAR SOLICITUD DISPONIBILIDAD PRESUPUESTAL";
                    disponibilidad.FechaCreacion = DateTime.Now;
                    disponibilidad.UsuarioCreacion = disponibilidad.UsuarioCreacion;
                    disponibilidad.NumeroSolicitud = Helpers.Helpers.Consecutive("PA", countMaxId);
                    disponibilidad.FechaSolicitud = DateTime.Now;
                    disponibilidad.OpcionContratarCodigo = "";
                    disponibilidad.EstadoSolicitudCodigo = "8";
                    disponibilidad.Eliminado = false;

                    disponibilidad.DisponibilidadPresupuestalProyecto.ToList().ForEach( pro => {
                        pro.UsuarioCreacion = disponibilidad.UsuarioCreacion;
                        pro.FechaCreacion = disponibilidad.FechaCreacion;
                        

                    });

                    _context.DisponibilidadPresupuestal.Add(disponibilidad);
                }
                else
                {
                    strCrearEditar = "EDIT SOLICITUD DISPONIBILIDAD PRESUPUESTAL";
                    disponibilidadPresupuestalAntiguo = _context.DisponibilidadPresupuestal.Find(disponibilidad.DisponibilidadPresupuestalId);
                    //Auditoria
                    disponibilidadPresupuestalAntiguo.UsuarioModificacion = disponibilidad.UsuarioModificacion;
                    disponibilidadPresupuestalAntiguo.Eliminado = false;


                    //Registros
                    disponibilidadPresupuestalAntiguo.Objeto = disponibilidad.Objeto;
                    disponibilidadPresupuestalAntiguo.AportanteId = disponibilidad.AportanteId;
                    

                    disponibilidad.DisponibilidadPresupuestalProyecto.ToList().ForEach( pro => {
                        if ( string.IsNullOrEmpty(pro.DisponibilidadPresupuestalProyectoId.ToString()) || pro.DisponibilidadPresupuestalProyectoId == 0 )
                        {
                            DisponibilidadPresupuestalProyecto proyecto = new DisponibilidadPresupuestalProyecto();

                            proyecto.UsuarioModificacion = disponibilidad.UsuarioCreacion;
                            proyecto.FechaModificacion = disponibilidad.FechaModificacion;

                            proyecto.DisponibilidadPresupuestalId = disponibilidad.DisponibilidadPresupuestalId;
                            proyecto.ProyectoAdministrativoId = pro.ProyectoAdministrativoId;

                            disponibilidadPresupuestalAntiguo.DisponibilidadPresupuestalProyecto.Add( proyecto );
                        }else
                        {
                            DisponibilidadPresupuestalProyecto proyecto = _context.DisponibilidadPresupuestalProyecto.Find( pro.DisponibilidadPresupuestalProyectoId );

                            proyecto.UsuarioModificacion = disponibilidad.UsuarioCreacion;
                            proyecto.FechaModificacion = disponibilidad.FechaModificacion;

                            proyecto.ProyectoAdministrativoId = pro.ProyectoAdministrativoId;
                        }

                    });


                }

                _context.SaveChanges();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = disponibilidad,
                    Code = ConstantMessagesSesionComiteTema.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesSesionComiteTema.OperacionExitosa, idAccion, disponibilidad.UsuarioCreacion, strCrearEditar)

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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.DisponibilidadPresupuestal, ConstantMessagesSesionComiteTema.Error, idAccion, disponibilidad.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
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

                }
                else
                {
                    pDisponibilidad.UsuarioCreacion = user;
                    pDisponibilidad.FechaCreacion = DateTime.Now;
                    pDisponibilidad.Eliminado = false;

                    _context.DisponibilidadPresupuestal.Add(pDisponibilidad);
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
            return await _context.DisponibilidadPresupuestal
                                        .Where(dp => dp.DisponibilidadPresupuestalId == disponibilidadPresupuestalId)
                                        .Include( r => r.DisponibilidadPresupuestalProyecto )
                                            .ThenInclude( r => r.Proyecto )
                                        .FirstOrDefaultAsync();
        }


        //Registrar nueva solicitud DDp Especial
        
        public async Task<Respuesta> CreateOrEditDDPRequest(DisponibilidadPresupuestal disponibilidadPresupuestal)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Solicitud_DDP_Especial, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            DisponibilidadPresupuestal disponibilidadPresupuestalAntiguo = null;
            int countMaxId = _context.DisponibilidadPresupuestal.Max(p => p.DisponibilidadPresupuestalId);

            try
            {
                DisponibilidadPresupuestalProyecto entity = new DisponibilidadPresupuestalProyecto();



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

                    disponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.ToList().ForEach(p =>
                    {
                        if (string.IsNullOrEmpty(p.DisponibilidadPresupuestalProyectoId.ToString()) || p.DisponibilidadPresupuestalProyectoId == 0)
                        {
                            entity.ProyectoId = p.ProyectoId;
                            entity.DisponibilidadPresupuestalId = disponibilidadPresupuestal.DisponibilidadPresupuestalId;
                            entity.FechaCreacion = DateTime.Now;
                            entity.UsuarioCreacion = disponibilidadPresupuestal.UsuarioCreacion;
                            entity.Eliminado = false;

                            disponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.Add(entity);
                        }

                    });

                    _context.DisponibilidadPresupuestal.Add(disponibilidadPresupuestal);

                }
                else
                {
                    strCrearEditar = "EDIT SOLICITUD DDP ESPECIAL";
                    disponibilidadPresupuestalAntiguo = _context.DisponibilidadPresupuestal.Find(disponibilidadPresupuestal.DisponibilidadPresupuestalId);

                    //Auditoria
                    disponibilidadPresupuestalAntiguo.UsuarioModificacion = disponibilidadPresupuestal.UsuarioModificacion;
                    disponibilidadPresupuestalAntiguo.Eliminado = false;


                    //Registros
                    //disponibilidadPresupuestalAntiguo.OpcionContratarCodigo = disponibilidadPresupuestal.OpcionContratarCodigo;
                    disponibilidadPresupuestalAntiguo.ValorSolicitud = disponibilidadPresupuestal.ValorSolicitud;
                    //disponibilidadPresupuestalAntiguo.NumeroSolicitud = disponibilidadPresupuestal.NumeroSolicitud;
                    //disponibilidadPresupuestalAntiguo.FechaSolicitud = disponibilidadPresupuestal.FechaSolicitud;
                    disponibilidadPresupuestalAntiguo.Objeto = disponibilidadPresupuestal.Objeto;
                    //disponibilidadPresupuestalAntiguo.EstadoSolicitudCodigo = disponibilidadPresupuestal.EstadoSolicitudCodigo;
                    disponibilidadPresupuestalAntiguo.TipoSolicitudCodigo = disponibilidadPresupuestal.TipoSolicitudCodigo;
                    disponibilidadPresupuestalAntiguo.AportanteId = disponibilidadPresupuestal.AportanteId;
                    disponibilidadPresupuestalAntiguo.NumeroRadicadoSolicitud = disponibilidadPresupuestal.NumeroRadicadoSolicitud;

                    disponibilidadPresupuestal.DisponibilidadPresupuestalProyecto.ToList().ForEach(p =>
                    {
                        if (string.IsNullOrEmpty(p.DisponibilidadPresupuestalProyectoId.ToString()) || p.DisponibilidadPresupuestalProyectoId == 0)
                        {
                            entity.ProyectoId = p.ProyectoId;
                            entity.DisponibilidadPresupuestalId = disponibilidadPresupuestal.DisponibilidadPresupuestalId;
                            entity.FechaCreacion = DateTime.Now;
                            entity.UsuarioCreacion = disponibilidadPresupuestal.UsuarioCreacion;
                            entity.Eliminado = false;

                            disponibilidadPresupuestalAntiguo.DisponibilidadPresupuestalProyecto.Add(entity);
                        }
                        else
                        {
                            entity = _context.DisponibilidadPresupuestalProyecto.Find(p.DisponibilidadPresupuestalProyectoId);

                            entity.UsuarioModificacion = disponibilidadPresupuestal.UsuarioCreacion;
                            entity.FechaModificacion = DateTime.Now;
                            entity.ProyectoId = p.ProyectoId;
                        }

                    });

                }

                var result = await _context.SaveChangesAsync();

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

        public async Task<Respuesta> EliminarDisponibilidad(int disponibilidadPresupuestalId)
        {

            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.eliminar_Solicitud_Disponibilidad_Presupuestal, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            DisponibilidadPresupuestal disponibilidadPresupuestal = null;

            try
            {

                disponibilidadPresupuestal = await _context.DisponibilidadPresupuestal.Where(d => d.DisponibilidadPresupuestalId == disponibilidadPresupuestalId).FirstOrDefaultAsync();


                if (disponibilidadPresupuestal != null)
                {

                    strCrearEditar = "Eliminar DISPONIBILIDAD PRESUPUESAL";
                    disponibilidadPresupuestal.FechaModificacion = DateTime.Now;
                    disponibilidadPresupuestal.UsuarioCreacion = disponibilidadPresupuestal.UsuarioCreacion;
                    disponibilidadPresupuestal.Eliminado = true;
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
        public async Task<List<DisponibilidadPresupuestal>> GetDDPEspecial()
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
                                RegistroCompleto = dp.RegistroCompleto,
                                EstadoSolicitudNombre = _context.Dominio.Where(r => (bool)r.Activo &&
                                                                             r.Codigo.Equals(dp.EstadoSolicitudCodigo) && 
                                                                             r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud_Disponibilidad_Presupuestal)
                                                                       .Select(r => r.Nombre).FirstOrDefault(),

                            }).ToListAsync();
        }

        public async Task<List<DisponibilidadPresupuestal>> GetDDPAdministrativa()
        {
            return await (
                            from dp in _context.DisponibilidadPresupuestal
                            where dp.NumeroSolicitud.Contains("PA_") && dp.Eliminado != true
                            select new DisponibilidadPresupuestal
                            {
                                DisponibilidadPresupuestalId = dp.DisponibilidadPresupuestalId,
                                FechaSolicitud = dp.FechaSolicitud,
                                NumeroSolicitud = dp.NumeroSolicitud,
                                ValorSolicitud = dp.ValorSolicitud,
                                EstadoSolicitudCodigo = dp.EstadoSolicitudCodigo,
                                RegistroCompleto = dp.RegistroCompleto,
                                EstadoSolicitudNombre = _context.Dominio.Where(r => (bool)r.Activo &&
                                                                             r.Codigo.Equals(dp.EstadoSolicitudCodigo) && 
                                                                             r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud_Disponibilidad_Presupuestal)
                                                                       .Select(r => r.Nombre).FirstOrDefault(),

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

        public ListAdminProyect MapToValueProyectoAdmnistrativo(SqlDataReader reader)
        {
            return new ListAdminProyect()
            {
                ProyectoId = (int)reader["ProyectoId"],
                ValorAporte = (decimal)reader["ValorAporte"],
                AportanteId = (int)reader["AportanteId"],
                ValorFuente = (decimal)reader["ValorFuente"],
                NombreAportanteId = (int)reader["NombreAportanteId"],
                NombreAportante = reader["NombreAportante"].ToString(),
                FuenteAportanteId = (int)reader["FuenteAportanteId"],
                FuenteRecursosCodigo = reader["FuenteRecursosCodigo"].ToString(),
                NombreFuente = reader["NombreFuente"].ToString(),
                

            };
        }

        //Lista Concecutivo admnistrativo
        public async Task<List<ListConcecutivoProyectoAdministrativo>> GetListCocecutivoProyecto()
        {
            try
            {
                return await ( from cp in _context.ProyectoAdministrativoAportante select new ListConcecutivoProyectoAdministrativo {
                                   ProyectoId = cp.ProyectoAdminstrativoId,
                                   Concecutivo = Helpers.Helpers.Consecutive("D4", cp.ProyectoAdminstrativoId),
                                }).ToListAsync();
            }

            catch (Exception)
            {

                throw;
            }
        }




        public async Task<List<Proyecto>> SearchLlaveMEN(string LlaveMEN)
        {
            var Id = await _context.Proyecto
                .Where(r => r.LlaveMen == LlaveMEN.ToString().Trim() && !(bool)r.Eliminado)
                
                
                .Select(r => r.LlaveMen).FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(Id))
                return await _context.Proyecto
                .Where(r => r.LlaveMen.Equals(Id))
                .Include(r => r.InstitucionEducativa)
                .Include(r => r.Sede)
                .ToListAsync();

            else
                throw new Exception("Esta llave no existe, por favor verificar los datos registrados");

        }


        //plantlla - rubro por financiar es infraestructura y el tipo de solicitud es contratación
        public async Task<HTMLContent> GetHTMLString(DetailValidarDisponibilidadPresupuesal obj)
        {
            try
            {
                //var  obj = await detailValidarDisponibilidadPresupuesal
                //DisponibilidadPresupuestal ListDP =  _context.DisponibilidadPresupuestal.Where(r => !r.Eliminado).FirstOrDefault();
                var sb = new StringBuilder();
                sb.Append(@"
                       <?xml version='1.0' encoding='utf-8'?>
                        <!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.1//EN' 'http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd'>
                        <html xmlns='http://www.w3.org/1999/xhtml'>

                        <head>
                            <meta http-equiv='Content-Type' content='application/xhtml+xml; charset=utf-8' />
                            <title>DOC</title>
                            <link href='assets/pdf-styles.css' type='text/css' rel='stylesheet' />
                        </head>
                            <body>
                                  <div class='Section0'>
                        <p class='Footer' style='text-align:center;'>
                            <img src='./assets/img-FFIE.png' width='89' height='77' align='right' alt='' />
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>F</span>
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>
                                    ONDO DE FINANCIAMIENTO DE INFRAESTRUCTURA EDUCATIVA - FFIE</span>
                        </p>
                        <p class='Footer' style='text-align:center;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>
                                NIT. 900900129-8</span>
                        </p>
                        <p class='Footer' style='text-align:center;'>
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>
                                MINISTERIO DE EDUCACIÓN
                        </span>
                        </p>
                        <p class='Header' style='text-align:right;'>
                            <span style='font-family:Times New Roman;font-size:12pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'> &#xa0;
                    </span>
                </p>
                <div>
            <table cellspacing='0' style='margin-left:19.6pt;width: auto; border-collapse: collapse; '>
                <tr style='height: 31.4666672px'>
                    <td style='vertical-align:middle;background-color:#F2F2F2;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:926.0667px;' colspan='10'>
                        <h2><span style='font-family:Arial;font-size:11pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>DOCUMENTO DE DISPONIBILIDAD PRESUPUESTAL</span></h2>
                    </td>
                </tr>
                <tr style='height: 9.2px'>
                    <td style='vertical-align:middle;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:926.0667px;' colspan='10'>
                        <h2><span style='font-family:Arial;font-size:11pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>&#xa0;</span></h2>
                    </td>
                </tr>

                <tr style='height: 29.6px'>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:321.2667px;'
                        colspan='3'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Fecha: </span><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'> </span>");

                sb.AppendLine("<span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + DateTime.Now.ToString("MM/dd/yyyy") + "");
                sb.Append(@"</span>
                        </h2>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:311.8667px;'
                        colspan='4'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Número de solicitud: </span>");
                sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + obj.NumeroSolicitud + "");
                sb.Append(@"</span>
                           
                           
                        </h2>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:292.9333px;'
                        colspan='3'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>DDP No: </span>");
                sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + obj.NumeroDDP + "");
                sb.Append(@"</span>

                        </h2>
                    </td>
                </tr>
                <tr style='height: 4.66666651px'>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:321.2667px;'
                        colspan='3'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>&#xa0;</span></h2>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:311.8667px;'
                        colspan='4'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>&#xa0;</span></h2>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:292.9333px;'
                        colspan='3'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>&#xa0;</span></h2>
                    </td>
                </tr>
                <tr style='height: 20.8000011px'>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:321.2667px;'
                        colspan='3'>
                        <p style='text-align:left;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Rubro por financiar: </span>");
                sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + "?" + "");
                sb.Append(@"</span>

                        </p>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:311.8667px;'
                        colspan='4'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Tipo de Solicitud</span>");
                sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + obj.TipoSolicitudText + "");
                sb.Append(@"</span>
                        </h2>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:292.9333px;'
                        colspan='3'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Opción por contratar: </span>");
                sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + obj.OpcionPorContratar + "");
                sb.Append(@"</span>
                        </h2>
                    </td>
                </tr>
                <tr style='height: 18.5333328px'>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:321.2667px;'
                        colspan='3'>
                        <p style='text-align:left;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Fecha comité técnico: </span>");
                sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + Convert.ToDateTime(obj.FechaComiteTecnico).ToString("MM/dd/yyyy") + "");
                sb.Append(@"</span>
                        </p>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:311.8667px;'
                        colspan='4'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Número comité:</span>");
                sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + "?" + "");
                sb.Append(@"</span>
                        </h2>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:292.9333px;'
                        colspan='3'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>&#xa0;</span></h2>
                    </td>
                </tr>
                <tr style='height: 30.5333328px'>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:926.0667px;'
                        colspan='10'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Objeto:</span>");
                sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + obj.Objeto + "");
                sb.Append(@"</span>
                        </h2>
                    </td>
                </tr>
                <tr style='height: 26.2666683px'>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:926.0667px;' colspan='10'>
                        <p class='Body-Text' style='text-align:center;line-height:115.833328%;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>Se expide el presente documento de disponibilidad presupuestal para la construcción de infraestr</span>
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>uctura educativa, conforme a lo señalado en el artículo 59 de la ley 1753 de 2015 Plan Nacional de Desarrollo </span>
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:italic;font-variant:normal;line-height:115.833328%;'>&quot;</span>
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:italic;font-variant:normal;line-height:115.833328%;'>T</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:italic;font-variant:normal;line-height:115.833328%;'>odos por un nuevo </span>
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:italic;font-variant:normal;line-height:115.833328%;'>país</span>
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:italic;font-variant:normal;line-height:115.833328%;'>&quot;</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'> y lo dispuesto en el conpes 3831 del Plan Nacional de Infraestructura Educativa.</span></p>
                        <h2><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>&#xa0;</span></h2>
                    </td>
                </tr>
                <tr style='height: 56.86667px'>
                    <td style='vertical-align:middle;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-top:0pt;padding-bottom:0pt;width:926.0667px;' colspan='10'>
                        <div>
                            <table cellspacing='0' style='width: 100%; border-collapse: collapse; '>
                                <tr style='height: 2px'>
                                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:209.9333px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>N</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>ombre </span>
                                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>del a</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>portante</span></p>
                                    </td>
                                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:302px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>Fuente</span></p>
                                    </td>
                                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:398.7333px;'
                                        colspan='2'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>Valor sol</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>icitado de la fuente</span></p>
                                    </td>
                                </tr>
                                <tr style='height: 2px'>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:209.9333px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'>Funcionalidad </span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'>Solicitar disponibilidad presupuestal” - Aportante</span></p>
                                    </td>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:302px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'>El sistema deberá </span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'>calcular el valor total de la fuente, realizando la sumatoria de los valores solicitados para todos los proyectos asociados al contrato</span>
                                            <span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'>,</span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'> por fuente y por aportante, enunciados en el cuadro inferior.</span></p>
                                    </td>
                                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:198.4667px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'>Funcionalidad </span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'>Solicitar disponibilidad presupuestal” – Valor del aportante</span></p>
                                    </td>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:200.2667px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='color:#BFBFBF;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>Cantidad en letra</span><span style='color:#BFBFBF;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>s (pesos moneda corriente)</span></p>
                                    </td>
                                </tr>
                                <tr style='height: 19.5333328px'>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:209.9333px;'>
                                        <p class='Body-Text' style='text-align:center;line-height:115.833328%;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:302px;'>
                                        <p class='Body-Text' style='text-align:center;line-height:115.833328%;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:198.4667px;'>
                                        <p class='Body-Text' style='text-align:center;line-height:115.833328%;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:200.2667px;'>
                                        <p class='Body-Text' style='text-align:center;line-height:115.833328%;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                </tr>
                                <tr style='height: 2px'>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:209.9333px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:302px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:198.4667px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:200.2667px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                </tr>
                                <tr style='height: 2px'>
                                    <td style='vertical-align:middle;background-color:#D9D9D9;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:511.9333px;'
                                        colspan='2'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>Total</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'> de recu</span>
                                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>r</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>sos con disponibilidad presupuestal</span></p>
                                    </td>
                                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:198.4667px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='color:#BFBFBF;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>Cantidad en formato</span><span style='color:#BFBFBF;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'> ($)</span></p>
                                    </td>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:200.2667px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='color:#BFBFBF;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>Cantidad en letras (pesos moneda corriente)</span></p>
                                    </td>
                                </tr>
                                <tr style='height:0px;'>
                                    <td style='width:209.9333px;border:none;padding:0pt;' />
                                    <td style='width:302px;border:none;padding:0pt;' />
                                    <td style='width:198.4666px;border:none;padding:0pt;' />
                                    <td style='width:200.2667px;border:none;padding:0pt;' />
                                </tr>
                            </table>
                        </div>
                        <p class='Body-Text' style='text-align:center;line-height:115.833328%;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                    </td>
                </tr>
                <tr style='height: 34.4666672px'>
                    <td style='vertical-align:middle;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:926.0667px;' colspan='10'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>Se certifica que a la fecha de expedición del presente documento existen </span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>los recursos para darle cumplimiento al objeto. Este recurso se compromete conforme a la siguiente información:</span></p>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>En </span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>el siguiente cuadro se debe mostrar la información que se encuentra en la funcionalidad Validar disponibilidad de presupuesto para ejecución de proyecto”</span></p>
                    </td>
                </tr>
                <tr style='height: 26.0666656px'>
                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:106.3333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'> Nombre del aportante</span>");
                sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + "");
                sb.Append(@"</span>
                        </p>
                    </td>
                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Fue</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>nte</span></p>
                    </td>
                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:122.8667px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Saldo actual de la fuente</span></p>
                    </td>
                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:113.4px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Valor </span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>solicitado de la fuente</span></p>
                    </td>
                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:132.2667px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Nuevo saldo de la fuente</span></p>
                    </td>
                </tr>
                <tr style='height: 19.5333328px'>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:127.5333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:115.8px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:106.3333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:122.8667px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:113.4px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:132.2667px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                </tr>
                <tr style='height: 19.5333328px'>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:127.5333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:115.8px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:106.3333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:122.8667px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:113.4px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:132.2667px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                </tr>
                <tr style='height: 6.33333349px'>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:127.5333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:115.8px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:106.3333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:122.8667px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:113.4px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:132.2667px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                </tr>
                <tr style='height: 26.0666656px'>
                    <td style='vertical-align:middle;background-color:#D9D9D9;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Limitación especial</span></p>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:822.1333px;'
                        colspan='9'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>Funcionalidad </span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>Solicitar </span>
                            <span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>disponibilidad presupuestal</span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>” </span>
                            <span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>–</span>
                            <span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'> </span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>Para los DDP Especiales</span>
                            <span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>”</span>
                        </p>
                    </td>
                </tr>
                <tr style='height: 5.4666667px'>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:127.5333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:115.8px;' colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:106.3333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:122.8667px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:113.4px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:132.2667px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                </tr>
                <tr style='height: 44.7333336px'>
                    <td style='vertical-align:top;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:127.5333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:115.8px;' colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:106.3333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:middle;border-top:none;border-left:none;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>Firma Director</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'> (a) Financiera</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:368.5333px;'
                        colspan='4'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                </tr>

            </div>");



                sb.Append(@"
                              
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

        //Grilla disponibilidad presupuestal.
        public async Task<ActionResult<List<GrillaValidarDisponibilidadPresupuesal>>> GetBudgetavailabilityRequests()
        {
            List<DisponibilidadPresupuestal> ListDP = await _context.DisponibilidadPresupuestal.Where(r => !r.Eliminado).ToListAsync();
            List<GrillaValidarDisponibilidadPresupuesal> ListGrillaDisponibilidadPresupuestal = new List<GrillaValidarDisponibilidadPresupuesal>();

            foreach (var validacionPresupuestal in ListDP)
            {
                GrillaValidarDisponibilidadPresupuesal disponibilidadPresupuestal = new GrillaValidarDisponibilidadPresupuesal
                {
                    Id = validacionPresupuestal.DisponibilidadPresupuestalId,
                    FechaSolicitud = validacionPresupuestal.FechaSolicitud,
                    NumeroSolicitud = validacionPresupuestal.NumeroSolicitud,
                    TipoSolicitudCodigo = validacionPresupuestal.TipoSolicitudCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(validacionPresupuestal.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_de_Solicitud) : "",
                    EstadoRegistro = (bool)validacionPresupuestal.RegistroCompleto,
                    EstadoRegistroText = (bool)validacionPresupuestal.RegistroCompleto ? "Completo" : "Incompleto"
                };

                ListGrillaDisponibilidadPresupuestal.Add(disponibilidadPresupuestal);
            }

            return ListGrillaDisponibilidadPresupuestal;
        }

        public async Task<List<DetailValidarDisponibilidadPresupuesal>> GetDetailAvailabilityBudgetProyect(int? rubroAfinanciarId, int disponibilidadPresupuestalId)
        {
            List<DisponibilidadPresupuestal> ListDP = await _context.DisponibilidadPresupuestal.
                Where(r => !r.Eliminado && r.DisponibilidadPresupuestalId == disponibilidadPresupuestalId).
                Include(x=>x.DisponibilidadPresupuestalProyecto).
                    ThenInclude(y=>y.Proyecto).
                    ThenInclude(v=>v.ProyectoAportante).
                    ThenInclude(c=>c.Aportante).
                Include(x=>x.DisponibilidadPresupuestalObservacion).ToListAsync();
            List<DetailValidarDisponibilidadPresupuesal> ListDetailValidarDisponibilidadPresupuesal = new List<DetailValidarDisponibilidadPresupuesal>();

            foreach (var detailDP in ListDP)
            {
                List<CofinanicacionAportanteGrilla> aportantes = new List<CofinanicacionAportanteGrilla>();
                List<ProyectoGrilla> proyecto = new List<ProyectoGrilla>();
                foreach (var proyectospp in detailDP.DisponibilidadPresupuestalProyecto)
                {
                    if (proyectospp.ProyectoId == null) //proyecto administrativo
                    {
                        var proyectoadministrativo = _context.ProyectoAdministrativo.Where(x => x.ProyectoAdministrativoId == proyectospp.ProyectoAdministrativoId).
                            Include(x => x.ProyectoAdministrativoAportante).ThenInclude(x => x.AportanteFuenteFinanciacion).ThenInclude(x => x.FuenteFinanciacion);
                        foreach(var apo in proyectoadministrativo.FirstOrDefault().ProyectoAdministrativoAportante)
                        {
                            List<GrillaFuentesFinanciacion> fuentes = new List<GrillaFuentesFinanciacion>();
                            foreach (var font in apo.AportanteFuenteFinanciacion)
                            {
                                fuentes.Add(new GrillaFuentesFinanciacion
                                {
                                    Fuente = _context.Dominio.Where(x=>x.Codigo==font.FuenteFinanciacion.FuenteRecursosCodigo 
                                        && x.TipoDominioId== (int)EnumeratorTipoDominio.Fuente_de_Recurso).FirstOrDefault().Nombre,
                                    Estado_de_las_fuentes = "",
                                    FuenteFinanciacionID=font.FuenteFinanciacionId,
                                    Valor_solicitado_de_la_fuente=font.FuenteFinanciacion.ValorFuente,
                                    Nuevo_saldo_de_la_fuente=0,
                                    Saldo_actual_de_la_fuente=0
                                });
                            }
                            aportantes.Add(new CofinanicacionAportanteGrilla
                            {
                                CofinanciacionAportanteId = apo.AportanteId,
                                Nombre = "",
                                TipoAportante = "",
                                ValorAportanteAlProyecto = 0,
                                FuentesFinanciacion= fuentes
                            }); 
                            
                        }
                    }
                    else
                    {
                        
                        foreach (var aportante in proyectospp.Proyecto.ProyectoAportante)
                        {
                            List<GrillaComponentes> grilla = new List<GrillaComponentes>();
                            var confinanciacion = _context.CofinanciacionAportante.Where(x=>x.CofinanciacionAportanteId==aportante.AportanteId).Include(x=>x.CofinanciacionDocumento).FirstOrDefault() ;
                            var localizacion = _context.Localizacion.Where(x => x.LocalizacionId==proyectospp.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                            var sede = _context.InstitucionEducativaSede.Find(proyectospp.Proyecto.SedeId);
                            var intfuentes = _context.FuenteFinanciacion.Where(y => y.AportanteId == aportante.AportanteId).Select(t => t.FuenteFinanciacionId).ToList();
                            string nombreAportante = "";
                            decimal? valorAportate = 0;
                            if (confinanciacion!=null)
                            {
                                if (confinanciacion.TipoAportanteId.Equals(ConstanTipoAportante.Ffie))
                                {
                                    nombreAportante = ConstanStringTipoAportante.Ffie;
                                }
                                else if (confinanciacion.TipoAportanteId.Equals(ConstanTipoAportante.Tercero))
                                {
                                    nombreAportante = confinanciacion.NombreAportanteId == null
                                        ? "Error" :
                                        _context.Dominio.Find(confinanciacion.NombreAportanteId).Nombre;
                                }
                                else
                                {
                                    if (confinanciacion.MunicipioId == null)
                                    {
                                        nombreAportante = confinanciacion.DepartamentoId == null
                                        ? "Error" :
                                        "Gobernación " + _context.Localizacion.Find(confinanciacion.DepartamentoId).Descripcion;
                                    }
                                    else
                                    {
                                        nombreAportante = confinanciacion.MunicipioId == null
                                        ? "Error" :
                                        "Alcaldía " + _context.Localizacion.Find(confinanciacion.MunicipioId).Descripcion;
                                    }
                                }
                                foreach(var cof in confinanciacion.CofinanciacionDocumento)
                                {
                                    valorAportate += cof.ValorTotalAportante;
                                }
                                var componenteAp = _context.ComponenteAportante.Where(x => x.ContratacionProyectoAportante.CofinanciacionAportanteId == confinanciacion.CofinanciacionAportanteId).Include(x=>x.ComponenteUso);
                                foreach(var compAp in componenteAp)
                                {
                                    foreach(var comp in compAp.ComponenteUso )
                                    {
                                        grilla.Add(
                                        new GrillaComponentes
                                        {
                                            ComponenteAportanteId = comp.ComponenteAportanteId,
                                            Componente = comp.TipoUsoCodigo,
                                            ComponenteUsoId = comp.ComponenteUsoId,
                                            Uso = comp.TipoUsoCodigo,
                                            ValorTotal = comp.ValorUso,
                                            ValorUso = comp.ValorUso
                                        });
                                    }                                    
                                }                                
                            }

                            proyecto.Add(new ProyectoGrilla
                            {
                                LlaveMen = proyectospp.Proyecto.LlaveMen,
                                Departamento = _context.Localizacion.Find(localizacion.IdPadre).Descripcion,
                                Municipio = localizacion.Descripcion,
                                TipoIntervencion = detailDP.TipoSolicitudCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(detailDP.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_de_Solicitud) : "",
                                InstitucionEducativa = _context.InstitucionEducativaSede.Find(sede.PadreId).Nombre,
                                Sede = sede.Nombre,
                                NombreAportante = nombreAportante,
                                ValorAportante = valorAportate,
                                AportanteID = confinanciacion==null?0: confinanciacion.CofinanciacionAportanteId,
                                DisponibilidadPresupuestalProyecto=proyectospp.DisponibilidadPresupuestalProyectoId,
                                ValorGestionado=_context.GestionFuenteFinanciacion.Where(x=>x.DisponibilidadPresupuestalProyectoId== proyectospp.DisponibilidadPresupuestalProyectoId && intfuentes.Contains(x.FuenteFinanciacionId)).Sum(x=>x.ValorSolicitado),
                                ComponenteGrilla = grilla
                            });
                        }
                        
                            /*foreach (var ppapor in proyectospp.Proyecto.ProyectoAportante)
                            {
                                List<GrillaFuentesFinanciacion> fuentes = new List<GrillaFuentesFinanciacion>();
                                foreach (var font in ppapor.Aportante.FuenteFinanciacion)
                                {
                                    fuentes.Add(new GrillaFuentesFinanciacion
                                    {
                                        Fuente = "",
                                        Estado_de_las_fuentes = "",
                                        FuenteFinanciacionID = font.FuenteFinanciacionId,
                                        Valor_solicitado_de_la_fuente = font.ValorFuente,
                                        Nuevo_saldo_de_la_fuente = 0,
                                        Saldo_actual_de_la_fuente = 0
                                    });
                                }
                                aportantes.Add(new CofinanicacionAportanteGrilla
                                {
                                    CofinanciacionAportanteId = ppapor.AportanteId,
                                    Nombre = "",
                                    TipoAportante = "",
                                    ValorAportanteAlProyecto = 0,
                                    FuentesFinanciacion = fuentes
                                });
                            }*/
                        }
                    
                }
                //busco comite técnico
                DateTime fechaComitetecnico = DateTime.Now;
                string numerocomietetecnico = "";
                if(detailDP.ContratacionId!=null)
                {
                    var contratacion = _context.Contratacion.Where(x => x.ContratacionId == detailDP.ContratacionId).
                        Include(x => x.ContratacionObservacion).ThenInclude(y => y.ComiteTecnico).ToList();
                    if(contratacion.FirstOrDefault().ContratacionObservacion.Count()>0)
                    {
                        numerocomietetecnico = contratacion.FirstOrDefault().ContratacionObservacion.FirstOrDefault().ComiteTecnico.NumeroComite;
                        fechaComitetecnico = contratacion.FirstOrDefault().ContratacionObservacion.FirstOrDefault().ComiteTecnico.FechaCreacion;
                    }                    
                }
                DetailValidarDisponibilidadPresupuesal detailDisponibilidadPresupuesal = new DetailValidarDisponibilidadPresupuesal
                {
                    //TODO:Traer estos campos { Tipo de modificacion, Valor despues de la modificacion, Plazo despues de la modificacion, Detalle de la modificacion) => se toma del caso de uso de novedades contractuales
                    Id = detailDP.DisponibilidadPresupuestalId,
                    NumeroSolicitud = detailDP.NumeroSolicitud,
                    TipoSolicitudCodigo = detailDP.TipoSolicitudCodigo,
                    TipoSolicitudText = detailDP.TipoSolicitudCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(detailDP.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_de_Solicitud) : "",
                    NumeroDDP = detailDP.NumeroDdp,
                    RubroPorFinanciar = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Disponibilidad_Presupuestal
                                && r.Codigo == detailDP.TipoSolicitudCodigo).FirstOrDefault().Descripcion,
                    Objeto = detailDP.Objeto,
                    ValorSolicitud = detailDP.ValorSolicitud,
                    // Si es aproboda por comite tecnico se debe mostrar la fecha en la que fue aprobada. traer desde dbo.[Sesion]
                    FechaComiteTecnico = fechaComitetecnico,
                    NumeroComite = numerocomietetecnico,
                    FechaSolicitud = detailDP.FechaSolicitud,
                    EstadoStr = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud_Disponibilidad_Presupuestal
                                && r.Codigo == detailDP.EstadoSolicitudCodigo).FirstOrDefault().Nombre,
                    Plazo = detailDP.PlazoMeses.ToString() + " meses / " + detailDP.PlazoDias.ToString(),

                    /*//*las modificaciones aun no existen*/

                    
                    Proyectos = proyecto,
                    //Aportantes
                    Aportantes = aportantes,
                };

                ListDetailValidarDisponibilidadPresupuesal.Add(detailDisponibilidadPresupuesal);
            }


            return ListDetailValidarDisponibilidadPresupuesal;
        }
    }
}
