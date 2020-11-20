using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using lalupa.Authorization.JwtHelpers;
using asivamosffie.api.Controllers;
using AuthorizationTest.JwtHelpers;
using asivamosffie.services.Exceptions;
using asivamosffie.services.Helpers;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.model.APIModels;
using Newtonsoft.Json;

namespace asivamosffie.services
{
    public class DailyFollowUpService : IDailyFollowUpService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public DailyFollowUpService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }

        public async Task<List<VProyectosXcontrato>> gridRegisterDailyFollowUp()
        {
            List<VProyectosXcontrato> listaInfoProyectos = await _context.VProyectosXcontrato
                                                                        .Where( r => r.FechaActaInicioFase2 <= DateTime.Now  )
                                                                        .ToListAsync();
            
            listaInfoProyectos.ForEach( p => {
                SeguimientoDiario seguimientoDiario = _context.SeguimientoDiario
                                                                .Where( s => s.ContratacionProyectoId == p.ContratacionProyectoId )
                                                                .OrderByDescending( r => r.FechaSeguimiento ).FirstOrDefault();

                if ( seguimientoDiario != null ){
                    p.FechaUltimoSeguimientoDiario = seguimientoDiario.FechaSeguimiento;
                    p.SeguimientoDiarioId = seguimientoDiario.SeguimientoDiarioId;
                }
            });

            return listaInfoProyectos;                                   
        }

        public async Task<Respuesta> CreateEditDailyFollowUp( SeguimientoDiario pSeguimientoDiario )
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Create_Edit_Seguimiento_Diario, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = "";
            try
            {
                    if (pSeguimientoDiario.SeguimientoDiarioId == 0)
                    {
                        CreateEdit = "CREAR SEGUIMIENTO DIARIO";
                        pSeguimientoDiario.FechaCreacion = DateTime.Now;
                        pSeguimientoDiario.Eliminado = false;

                        _context.SeguimientoDiario.Add( pSeguimientoDiario );
                    }
                    else
                    {
                        CreateEdit = "CREAR SEGUIMIENTO DIARIO";
                        SeguimientoDiario seguimientoDiario = _context.SeguimientoDiario.Find( pSeguimientoDiario.SeguimientoDiarioId );

                        seguimientoDiario.FechaModificacion = DateTime.Now;
                        seguimientoDiario.UsuarioModificacion = pSeguimientoDiario.UsuarioCreacion;

                        seguimientoDiario.FechaSeguimiento = pSeguimientoDiario.FechaSeguimiento;
                        seguimientoDiario.DisponibilidadPersonal = pSeguimientoDiario.DisponibilidadPersonal;
                        seguimientoDiario.DisponibilidadPersonalObservaciones = pSeguimientoDiario.DisponibilidadPersonalObservaciones;
                        seguimientoDiario.CantidadPersonalProgramado = pSeguimientoDiario.CantidadPersonalProgramado;
                        seguimientoDiario.CantidadPersonalTrabajando = pSeguimientoDiario.CantidadPersonalTrabajando;
                        seguimientoDiario.SeGeneroRetrasoPersonal = pSeguimientoDiario.SeGeneroRetrasoPersonal;
                        seguimientoDiario.NumeroHorasRetrasoPersonal = pSeguimientoDiario.NumeroHorasRetrasoPersonal;
                        seguimientoDiario.DisponibilidadMaterialCodigo = pSeguimientoDiario.DisponibilidadMaterialCodigo;
                        seguimientoDiario.DisponibilidadMaterialObservaciones = pSeguimientoDiario.DisponibilidadMaterialObservaciones;
                        seguimientoDiario.CausaIndisponibilidadMaterialCodigo = pSeguimientoDiario.CausaIndisponibilidadMaterialCodigo;
                        seguimientoDiario.SeGeneroRetrasoMaterial = pSeguimientoDiario.SeGeneroRetrasoMaterial;
                        seguimientoDiario.NumeroHorasRetrasoMaterial = pSeguimientoDiario.NumeroHorasRetrasoMaterial;
                        seguimientoDiario.DisponibilidadEquipoCodigo = pSeguimientoDiario.DisponibilidadEquipoCodigo;
                        seguimientoDiario.DisponibilidadEquipoObservaciones = pSeguimientoDiario.DisponibilidadEquipoObservaciones;
                        seguimientoDiario.CausaIndisponibilidadEquipoCodigo = pSeguimientoDiario.CausaIndisponibilidadEquipoCodigo;
                        seguimientoDiario.SeGeneroRetrasoEquipo = pSeguimientoDiario.SeGeneroRetrasoEquipo;
                        seguimientoDiario.NumeroHorasRetrasoEquipo = pSeguimientoDiario.NumeroHorasRetrasoEquipo;
                        seguimientoDiario.ProductividadCodigo = pSeguimientoDiario.ProductividadCodigo;
                        seguimientoDiario.ProductividadObservaciones = pSeguimientoDiario.ProductividadObservaciones;
                        seguimientoDiario.CausaIndisponibilidadProductividadCodigo = pSeguimientoDiario.CausaIndisponibilidadProductividadCodigo;
                        seguimientoDiario.SeGeneroRetrasoProductividad = pSeguimientoDiario.SeGeneroRetrasoProductividad;
                        seguimientoDiario.NumeroHorasRetrasoProductividad = pSeguimientoDiario.NumeroHorasRetrasoProductividad;

                    }
                    
                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_seguimiento_diario, GeneralCodes.OperacionExitosa, idAccion, pSeguimientoDiario.UsuarioCreacion, CreateEdit)
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
                      Code = GeneralCodes.Error,
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_seguimiento_diario, GeneralCodes.Error, idAccion, pSeguimientoDiario.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }

        }


    }
}
