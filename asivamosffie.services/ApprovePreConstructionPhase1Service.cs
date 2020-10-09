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

            List<VRequisitosTecnicosInicioConstruccion> lista = await _context.VRequisitosTecnicosInicioConstruccion.ToListAsync();

            lista.ForEach(c =>
            {
                listaContrats.Add(new
                {
                    c.TipoContratoCodigo,
                    c.ContratoId,
                    c.FechaAprobacion,
                    c.NumeroContrato,
                    c.CantidadProyectosAsociados,
                    c.CantidadProyectosRequisitosAprobados,
                    CantidadProyectosRequisitosPendientes = c.CantidadProyectosAsociados - c.CantidadProyectosRequisitosAprobados,
                    c.EstadoCodigo,
                    c.EstadoNombre,
                    c.ExisteRegistro,
                });
            });

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

                if (!string.IsNullOrEmpty(pContratoPerfilObservacion.Observacion))
                {
                    contratoPerfilOld.ConObervacionesSupervision = true;

                    pContratoPerfilObservacion.FechaCreacion = DateTime.Now;
                    pContratoPerfilObservacion.Eliminado = false;
                    pContratoPerfilObservacion.TipoObservacionCodigo = ConstanCodigoTipoObservacion.SupervisorAprobar;
                    if (!string.IsNullOrEmpty(pContratoPerfilObservacion.Observacion))
                            pContratoPerfilObservacion.Observacion = pContratoPerfilObservacion.Observacion.ToUpper();
                    _context.ContratoPerfilObservacion.Add(pContratoPerfilObservacion);
                }
                else
                {
                    contratoPerfilOld.ConObervacionesSupervision = false;
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
