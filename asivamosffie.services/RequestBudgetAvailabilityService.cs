using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class RequestBudgetAvailabilityService: IRequestBudgetAvailabilityService
    {

        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public RequestBudgetAvailabilityService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;

        }

        //Solicitudes de comite tecnico
        public async Task<ActionResult<List<GridReuestCommittee>>> GetReuestCommittee()
        {
            try
            {

                return await (from ct in _context.Contratacion
                              join dp in _context.DisponibilidadPresupuestal on  ct.ContratacionId equals dp.ContratacionId
                              //where sc.EstadoCodigo == "18" //Aprobado por comite tecnico
                              select new GridReuestCommittee
                              {
                                  ContratacionId = dp.ContratacionId,
                                  FechaSolicitud = dp.FechaSolicitud,
                                  TipoSolicitudCodigo = dp.TipoSolicitudCodigo,
                                  TipoSolicitudText = _context.Dominio.Where(r => r.Codigo.Equals(dp.TipoSolicitudCodigo) && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Solicitud).Select(r => r.Nombre).FirstOrDefault(),
                                  NumeroSolicitud = dp.NumeroSolicitud,
                                  OpcionContratar = _context.Dominio.Where(r => r.Codigo.Equals(dp.OpcionContratarCodigo) && r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_Por_Contratar).Select(r => r.Nombre).FirstOrDefault(),
                                  ValorSolicitado = 0,
                                  EstadoSolicitudCodigo = _context.Dominio.Where(r => r.Codigo.Equals(dp.EstadoSolicitudCodigo) && r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Solicitud_Disponibilidad_Presupuestal).Select(r => r.Nombre).FirstOrDefault()
                              })
                              .OrderByDescending(sc => sc.FechaSolicitud)
                              .ToListAsync();
            }

            catch (Exception)
            {

                throw;
            }

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


        //pendiente

        public async Task<ActionResult<List<SesionComiteTecnicoCompromiso>>> GetManagementCommitteeReportById(int SesionComiteTecnicoCompromisoId)
        {
            try//GridComiteTecnicoCompromiso
            {
                return await _context.SesionComiteTecnicoCompromiso.IncludeFilter(cm => cm.ComiteTecnico).Where(cm => (bool)!cm.Eliminado).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }





    }
}
