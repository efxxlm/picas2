using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Helpers.Constant;
using Z.EntityFramework.Plus;
using System.Text.RegularExpressions;


namespace asivamosffie.services
{
    public class ParametricService : IParametricService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public ParametricService(devAsiVamosFFIEContext devAsiVamosFFIEContext, ICommonService commonService)
        {
            _commonService = commonService;
            _context = devAsiVamosFFIEContext;
        }

        public async Task<List<VParametricas>> GetParametricas()
        {
            return await _context.VParametricas
                .Where(
                r =>
                 r.TipoDominioId != (int)EnumeratorTipoDominio.Tipo_Asignaacion
              && r.TipoDominioId != (int)EnumeratorTipoDominio.Tipo_Observacion_Construccion
              && r.TipoDominioId != (int)EnumeratorTipoDominio.PlaceHolderDDP
              && r.TipoDominioId != (int)EnumeratorTipoDominio.Estado_Proyecto
              && r.TipoDominioId != (int)EnumeratorTipoDominio.Estado_Documento_Contrato
              && r.TipoDominioId != (int)EnumeratorTipoDominio.Estado_Revision_Poliza
              && r.TipoDominioId != (int)EnumeratorTipoDominio.Estado_Del_Acta_Contrato
              && r.TipoDominioId != (int)EnumeratorTipoDominio.Tipo_Observacion_Contrato_Perfil_Observacion
              && r.TipoDominioId != (int)EnumeratorTipoDominio.Lista_Fases_Sistema  
              && r.TipoDominioId != (int)EnumeratorTipoDominio.Lista_Chequeo_Menu 
              && r.TipoDominioId != (int)EnumeratorTipoDominio.Estados_Lista_Chequeo   
              && r.TipoDominioId != (int)EnumeratorTipoDominio.Tipo_Observacion_Seguimiento_Semanal      
              && r.TipoDominioId != (int)EnumeratorTipoDominio.EstadoAvanceProcesosDefensa  
              && r.TipoDominioId != (int)EnumeratorTipoDominio.Estado_Reporte_Semanal_Y_Muestras  
              && r.TipoDominioId != (int)EnumeratorTipoDominio.Estado_Reporte_Semanal_Y_Muestras
                )


                .OrderByDescending(p => p.TipoDominioId)
                .ToListAsync();
        }

        public async Task<Respuesta> CreateDominio(TipoDominio pTipoDominio)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Parametrica, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                foreach (var Dominio in pTipoDominio.Dominio.Where(r => !string.IsNullOrEmpty(r.Nombre)))
                {
                    if (Dominio.DominioId == 0)
                    {
                        int CantidadParametricas = _context.Dominio.Count(d => d.TipoDominioId == Dominio.TipoDominioId) + 1;
                        Dominio.FechaCreacion = DateTime.Now;
                        Dominio.Activo = true;
                        Dominio.Codigo = (CantidadParametricas).ToString();
                        _context.Dominio.Add(Dominio);
                        _context.SaveChanges();
                    }
                    else
                    {
                        await _context.Set<Dominio>()
                                      .Where(d => d.DominioId == Dominio.DominioId)
                                      .UpdateAsync(d => new Dominio
                                      {
                                          Activo = Dominio.Activo,
                                          Nombre = Dominio.Nombre,
                                          Descripcion = Dominio.Descripcion,
                                          Codigo = Dominio.Codigo,
                                          FechaModificacion = DateTime.Now,
                                          UsuarioModificacion = pTipoDominio.UsuarioCreacion,
                                      });
                    }
                }

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_parametricas, GeneralCodes.OperacionExitosa, idAccion, pTipoDominio.UsuarioCreacion, "CREAR EDITAR PARAMETICAS")
                };
            }
            catch (Exception e)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = GeneralCodes.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_parametricas, GeneralCodes.Error, idAccion, pTipoDominio.UsuarioCreacion, e.InnerException.ToString())
                };
            }
        }

        public async Task<List<VDominio>> GetDominioByTipoDominioId(int pTipoDominioId)
        {
            return await _context.VDominio.Where(d => d.TipoDominioId == pTipoDominioId).ToListAsync();
        }
    }
}
