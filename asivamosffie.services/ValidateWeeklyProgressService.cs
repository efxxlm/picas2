using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.model.APIModels;
using System.IO;
using Z.EntityFramework.Plus;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Runtime.InteropServices.WindowsRuntime;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Microsoft.EntityFrameworkCore.Internal;
using asivamosffie.services.Helpers.Constants;

namespace asivamosffie.services
{
    public class ValidateWeeklyProgressService : IValidateWeeklyProgressService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IRegisterWeeklyProgressService _registerWeeklyProgressService;

        public ValidateWeeklyProgressService(devAsiVamosFFIEContext context, ICommonService commonService, IRegisterWeeklyProgressService registerWeeklyProgressService)
        {
            _registerWeeklyProgressService = registerWeeklyProgressService;
            _commonService = commonService;
            _context = context;
        }

        public async Task<List<VVerificarValidarSeguimientoSemanal>> GetListReporteSemanalView(List<string> strListCodEstadoSeguimientoSemanal)
        {
            return await _context.VVerificarValidarSeguimientoSemanal.Where(r => r.EstadoSeguimientoSemanalCodigo == ConstanCodigoEstadoSeguimientoSemanal.Enviado_Validacion).ToListAsync();
        }


        public async Task<Respuesta> ReturnSeguimientoSemanal(int pSeguimientoSemanalId, string pUsuarioMod)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cargar_Acta_Terminacion_Contrato, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SeguimientoSemanal seguimientoSemanal = await _registerWeeklyProgressService.GetLastSeguimientoSemanalByContratacionProyectoIdOrSeguimientoSemanalId(0, pSeguimientoSemanalId);
                seguimientoSemanal.UsuarioModificacion = pUsuarioMod;
                seguimientoSemanal.FechaEnvioSupervisor = null;
                seguimientoSemanal.RegistroCompleto = false;
                seguimientoSemanal.RegistroCompletoMuestras = false;
                seguimientoSemanal.RegistroCompletoVerificar = false;
                seguimientoSemanal.RegistroCompletoAvalar = false;
                seguimientoSemanal.EstadoSeguimientoSemanalCodigo = ConstanCodigoEstadoSeguimientoSemanal.Devuelto_Supervisor;

                List<SeguimientoSemanalObservacion> ListSeguimientoSemanalObservacion = _context.SeguimientoSemanalObservacion.Where(r => r.SeguimientoSemanalId == pSeguimientoSemanalId).ToList();
            
                ListSeguimientoSemanalObservacion.ForEach(s =>
                {
                    s.Archivada = true;
                    s.FechaModificacion = DateTime.Now;
                    s.UsuarioModificacion = pUsuarioMod;
                });

                //seguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault().RegistroCompleto = false;
                //seguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault().RegistroCompletoObservacionApoyo = false;
                //seguimientoSemanal.SeguimientoSemanalAvanceFisico.FirstOrDefault().RegistroCompletoObservacionSupervisor = false;

                //if (seguimientoSemanal.NumeroSemana % 5 == 0)
                //{
                //    seguimientoSemanal.SeguimientoSemanalAvanceFinanciero.FirstOrDefault().RegistroCompleto = false;
                //    seguimientoSemanal.SeguimientoSemanalAvanceFinanciero.FirstOrDefault().RegistroCompletoObservacionApoyo = false;
                //    seguimientoSemanal.SeguimientoSemanalAvanceFinanciero.FirstOrDefault().RegistroCompletoObservacionSupervisor = false;
                //} 
                 
                //seguimientoSemanal.SeguimientoSemanalReporteActividad.FirstOrDefault().RegistroCompleto = false;
                //seguimientoSemanal.SeguimientoSemanalReporteActividad.FirstOrDefault().RegistroCompletoActividad = false;
                //seguimientoSemanal.SeguimientoSemanalReporteActividad.FirstOrDefault().RegistroCompletoActividadSiguiente = false;

                //seguimientoSemanal.SeguimientoSemanalReporteActividad.FirstOrDefault().RegistroCompletoObservacionApoyoEstadoContrato = false;
                //seguimientoSemanal.SeguimientoSemanalReporteActividad.FirstOrDefault().RegistroCompletoObservacionApoyoActividad = false;
                //seguimientoSemanal.SeguimientoSemanalReporteActividad.FirstOrDefault().RegistroCompletoObservacionApoyoActividadSiguiente = false;

                //seguimientoSemanal.SeguimientoSemanalReporteActividad.FirstOrDefault().RegistroCompletoObservacionSupervisorEstadoContrato = false;
                //seguimientoSemanal.SeguimientoSemanalReporteActividad.FirstOrDefault().RegistroCompletoObservacionSupervisorActividad = false;
                //seguimientoSemanal.SeguimientoSemanalReporteActividad.FirstOrDefault().RegistroCompletoObservacionSupervisorActividadSiguiente = false;

                //seguimientoSemanal.SeguimientoSemanalRegistroFotografico.FirstOrDefault().RegistroCompleto = false;
                //seguimientoSemanal.SeguimientoSemanalRegistroFotografico.FirstOrDefault().RegistroCompletoObservacionApoyo = false;
                //seguimientoSemanal.SeguimientoSemanalRegistroFotografico.FirstOrDefault().RegistroCompletoObservacionSupervisor = false;

                //seguimientoSemanal.SeguimientoSemanalRegistrarComiteObra.FirstOrDefault().RegistroCompleto = false;
                //seguimientoSemanal.SeguimientoSemanalRegistrarComiteObra.FirstOrDefault().RegistroCompletoObservacionApoyo = false;
                //seguimientoSemanal.SeguimientoSemanalRegistrarComiteObra.FirstOrDefault().RegistroCompletoObservacionSupervisor = false;
                 
                _context.SaveChanges();
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstanMessagesRegisterWeeklyProgress.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.OperacionExitosa, idAccion, pUsuarioMod, "DEVOLVER SEGUIMIENTO SEMANAL")
                };
            }
            catch (Exception e)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstanMessagesRegisterWeeklyProgress.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Avance_Semanal, ConstanMessagesRegisterWeeklyProgress.Error, idAccion, pUsuarioMod, e.InnerException.ToString())
                };
            }


        }

    }
}
