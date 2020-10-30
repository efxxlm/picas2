using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
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
                    if (ContratacionProyecto.Proyecto.ContratoPerfil.Count() == 0)
                        CantidadProyectosConPerfilesPendientes++;
                    else if (ContratacionProyecto.Proyecto.ContratoPerfil.Count(r => !(bool)r.Eliminado) == ContratacionProyecto.Proyecto.ContratoPerfil.Count(r => !(bool)r.Eliminado && r.TieneObservacionSupervisor.HasValue))
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
                if (pContratoPerfilObservacion.TieneObservacionSupervisor)
                {
                    pContratoPerfilObservacion.FechaCreacion = DateTime.Now;
                    pContratoPerfilObservacion.Eliminado = false;
                    pContratoPerfilObservacion.TipoObservacionCodigo = ConstanCodigoTipoObservacion.SupervisorAprobar;
                    pContratoPerfilObservacion.Observacion = pContratoPerfilObservacion.Observacion.ToUpper();
                    _context.ContratoPerfilObservacion.Add(pContratoPerfilObservacion);
                }
                _context.SaveChanges();

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
