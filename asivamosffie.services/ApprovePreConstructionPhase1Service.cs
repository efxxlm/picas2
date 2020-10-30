using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Constants;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services
{
    public class ApprovePreConstructionPhase1Service : IApprovePreConstructionPhase1Service
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IVerifyPreConstructionRequirementsPhase1Service _verifyPre;

        public ApprovePreConstructionPhase1Service(devAsiVamosFFIEContext devAsiVamosFFIEContext, IVerifyPreConstructionRequirementsPhase1Service verifyPreConstructionRequirementsPhase1Service, ICommonService commonService)
        {
            _context = devAsiVamosFFIEContext;
            _commonService = commonService;
            _verifyPre = verifyPreConstructionRequirementsPhase1Service;
        }

        public async Task<dynamic> GetListContratacion()
        {
            List<dynamic> listaContrats = new List<dynamic>();

            List<Contrato> listContratos = await _context.Contrato
                .FromSqlRaw("SELECT c.* FROM dbo.Contrato AS c " +
                "INNER JOIN dbo.Contratacion AS ctr ON c.ContratacionId = ctr.ContratacionId " +
                "INNER JOIN dbo.DisponibilidadPresupuestal AS dp ON ctr.ContratacionId = dp.ContratacionId " +
                "INNER JOIN dbo.ContratoPoliza AS cp ON c.ContratoId = cp.ContratoId " +
                "WHERE dp.NumeroDDP IS NOT NULL " +
                "AND cp.FechaAprobacion is not null")
                .Include(r => r.ContratoPoliza)
                .Include(r => r.Contratacion)
                   .ThenInclude(r => r.ContratacionProyecto)
                       .ThenInclude(r => r.Proyecto)
                            .ThenInclude(r => r.ContratoPerfil)
                .Include(r => r.Contratacion)
                  .ThenInclude(r => r.DisponibilidadPresupuestal)
               .ToListAsync();

            foreach (var c in listContratos)
            {
                int CantidadProyectosConPerfilesAprobados = 0;
                int CantidadProyectosConPerfilesPendientes = 0;
                bool RegistroCompleto = false;
                bool EstaDevuelto = false;
                if (c.EstaDevuelto.HasValue && (bool)c.EstaDevuelto)
                    EstaDevuelto = true;
                foreach (var ContratacionProyecto in c.Contratacion.ContratacionProyecto)
                {
                    bool RegistroCompletoObservaciones = true;
                    foreach (var ContratoPerfil in c.ContratoPerfil.Where(r => !(bool)r.Eliminado))
                    {
                        if (ContratoPerfil.ContratoPerfilObservacion.Count(r => r.TipoObservacionCodigo == ConstanCodigoTipoObservacion.Supervisor) == 0)
                            RegistroCompletoObservaciones = false;
                        else if ((ContratoPerfil.TieneObservacionApoyo == null)
                           || (ContratoPerfil.TieneObservacionApoyo.HasValue
                           && (bool)ContratoPerfil.TieneObservacionApoyo
                           && (ContratoPerfil.ContratoPerfilObservacion.LastOrDefault().Observacion == null
                           && ContratoPerfil.ContratoPerfilObservacion.LastOrDefault().TipoObservacionCodigo == ConstanCodigoTipoObservacion.Supervisor)))
                            RegistroCompletoObservaciones = false;
                    }
                    if (RegistroCompletoObservaciones)
                        CantidadProyectosConPerfilesAprobados++;
                    else
                        CantidadProyectosConPerfilesPendientes++;
                }


                if (c.Contratacion.ContratacionProyecto.Count(r => !r.Eliminado) == CantidadProyectosConPerfilesAprobados)
                    RegistroCompleto = true;
                listaContrats.Add(new
                {
                    c.ContratoId,
                    FechaAprobacion = ((DateTime)c.ContratoPoliza.FirstOrDefault().FechaAprobacion).ToString("dd-MM-yyyy"),
                    c.Contratacion.TipoSolicitudCodigo,
                    c.NumeroContrato,
                    CantidadProyectosAsociados = c.Contratacion.ContratacionProyecto.Count(r => !r.Eliminado),
                    CantidadProyectosRequisitosAprobados = CantidadProyectosConPerfilesAprobados,
                    CantidadProyectosConPerfilesPendientes,
                    EstadoCodigo = c.EstadoVerificacionCodigo,
                    EstaDevuelto,
                    RegistroCompleto
                });
            }

            return listaContrats;

        }

        public async Task<Respuesta> CrearContratoPerfilObservacion(ContratoPerfilObservacion pContratoPerfilObservacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Observacion_Contrato_Perfil, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ContratoPerfil contratoPerfilOld = _context.ContratoPerfil.Find(pContratoPerfilObservacion.ContratoPerfilId);
                contratoPerfilOld.UsuarioModificacion = pContratoPerfilObservacion.UsuarioCreacion;
                contratoPerfilOld.FechaModificacion = DateTime.Now;
                contratoPerfilOld.TieneObservacionSupervisor = pContratoPerfilObservacion.TieneObservacionSupervisor;

                if (pContratoPerfilObservacion.ContratoPerfilObservacionId == 0)
                {
                    pContratoPerfilObservacion.FechaCreacion = DateTime.Now;
                    pContratoPerfilObservacion.Eliminado = false;
                    pContratoPerfilObservacion.TipoObservacionCodigo = ConstanCodigoTipoObservacion.Supervisor;
                    _context.ContratoPerfilObservacion.Add(pContratoPerfilObservacion);
                }
                else
                {
                    ContratoPerfilObservacion contratoPerfilObservacionOld = _context.ContratoPerfilObservacion.Find(pContratoPerfilObservacion.ContratoPerfilObservacionId);
                    contratoPerfilObservacionOld.FechaModificacion = DateTime.Now;
                    contratoPerfilObservacionOld.UsuarioModificacion = pContratoPerfilObservacion.UsuarioCreacion;
                    if (pContratoPerfilObservacion.Observacion != null)
                        contratoPerfilObservacionOld.Observacion = pContratoPerfilObservacion.Observacion.ToUpper(); 
                }
                _context.SaveChanges();

                //Validar Estados Completos
                Contrato contrato = _context.Contrato
                    .Where(r => r.ContratoId == contratoPerfilOld.ContratoId)
                    .Include(r => r.Contratacion)
                    .Include(r => r.ContratoPerfil)
                    .ThenInclude(r => r.ContratoPerfilObservacion).FirstOrDefault();

                bool RegistroCompleto = true;

                foreach (var ContratoPerfil in contrato.ContratoPerfil.Where(r => !(bool)r.Eliminado))
                {
                    if ((ContratoPerfil.TieneObservacionApoyo == null)
                        || (ContratoPerfil.TieneObservacionApoyo.HasValue
                        && (bool)ContratoPerfil.TieneObservacionApoyo
                        && (ContratoPerfil.ContratoPerfilObservacion.LastOrDefault().Observacion == null
                        && ContratoPerfil.ContratoPerfilObservacion.LastOrDefault().TipoObservacionCodigo == ConstanCodigoTipoObservacion.Supervisor)))
                        RegistroCompleto = false;
                }

                if (RegistroCompleto)
                    contrato.EstadoVerificacionCodigo = ConstanCodigoEstadoContrato.Con_requisitos_tecnicos_aprobados_por_supervisor;
                else
                    contrato.EstadoVerificacionCodigo = ConstanCodigoEstadoContrato.Con_requisitos_tecnicos_validados;

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = RegisterPreContructionPhase1.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.OperacionExitosa, idAccion, pContratoPerfilObservacion.UsuarioCreacion, "CREAR OBSERVACION CONTRATO PERFIL")
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
                        Code = RegisterPreContructionPhase1.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.Error, idAccion, pContratoPerfilObservacion.UsuarioCreacion, ex.InnerException.ToString().ToUpper())
                    };
            }
        }


    }
}
