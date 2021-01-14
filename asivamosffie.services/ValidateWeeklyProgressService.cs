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
                SeguimientoSemanal seguimientoSemanal = _context.SeguimientoSemanal.Find(pSeguimientoSemanalId);
                seguimientoSemanal.UsuarioModificacion = pUsuarioMod;
                seguimientoSemanal.FechaModificacion = DateTime.Now;
            
                seguimientoSemanal.FechaEnvioSupervisor = null; 
                seguimientoSemanal.RegistroCompleto = false;
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
