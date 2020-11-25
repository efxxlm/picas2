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
                                                                .Where( s => s.ContratacionProyectoId == p.ContratacionProyectoId && 
                                                                        s.Eliminado != true )
                                                                .OrderByDescending( r => r.FechaSeguimiento ).FirstOrDefault();

                if ( seguimientoDiario != null ){
                    p.FechaUltimoSeguimientoDiario = seguimientoDiario.FechaSeguimiento;
                    p.SeguimientoDiarioId = seguimientoDiario.SeguimientoDiarioId;
                    p.RegistroCompleto = seguimientoDiario.RegistroCompleto.HasValue?seguimientoDiario.RegistroCompleto.Value:false;
                    p.EstadoCodigo = seguimientoDiario.EstadoCodigo;
                }
            });

            return listaInfoProyectos;                                   
        }

        private bool VerificarRegistroCompleto( SeguimientoDiario pSeguimientoDiario ){
            bool completo = true;

            List<string> listaBajaDisponibilidadMaterial = new List<string> {{ "2" }, {"3" }};
            List<string> listaBajaDisponibilidadEquipo = new List<string> {{ "2" }, {"3" }};

            if ( 
                    pSeguimientoDiario.FechaSeguimiento == null ||
                    
                    pSeguimientoDiario.DisponibilidadPersonal == null ||
                    string.IsNullOrEmpty( pSeguimientoDiario.DisponibilidadPersonalObservaciones ) ||
                    ( pSeguimientoDiario.DisponibilidadPersonal == false && pSeguimientoDiario.CantidadPersonalProgramado == null ) ||
                    ( pSeguimientoDiario.DisponibilidadPersonal == false && pSeguimientoDiario.CantidadPersonalTrabajando == null ) ||
                    ( pSeguimientoDiario.DisponibilidadPersonal == false && pSeguimientoDiario.SeGeneroRetrasoPersonal == null ) ||
                    ( pSeguimientoDiario.SeGeneroRetrasoPersonal == true && pSeguimientoDiario.NumeroHorasRetrasoPersonal == null ) ||

                    string.IsNullOrEmpty( pSeguimientoDiario.DisponibilidadMaterialCodigo ) ||
                    string.IsNullOrEmpty( pSeguimientoDiario.DisponibilidadMaterialObservaciones ) ||
                    ( listaBajaDisponibilidadMaterial.Where( r => r == pSeguimientoDiario.DisponibilidadMaterialCodigo ).Count() > 0  && 
                        pSeguimientoDiario.CausaIndisponibilidadMaterialCodigo == null ) ||
                    ( listaBajaDisponibilidadMaterial.Where( r => r == pSeguimientoDiario.DisponibilidadMaterialCodigo ).Count() > 0  && 
                        pSeguimientoDiario.SeGeneroRetrasoMaterial == null ) ||
                    ( pSeguimientoDiario.SeGeneroRetrasoMaterial == true && pSeguimientoDiario.NumeroHorasRetrasoMaterial == null ) ||


                    string.IsNullOrEmpty( pSeguimientoDiario.DisponibilidadEquipoCodigo ) ||
                    string.IsNullOrEmpty( pSeguimientoDiario.DisponibilidadEquipoObservaciones ) ||
                    ( listaBajaDisponibilidadEquipo.Where( r => r == pSeguimientoDiario.DisponibilidadEquipoCodigo ).Count() > 0  && 
                        pSeguimientoDiario.CausaIndisponibilidadEquipoCodigo == null ) ||
                    ( listaBajaDisponibilidadEquipo.Where( r => r == pSeguimientoDiario.DisponibilidadEquipoCodigo ).Count() > 0  && 
                        pSeguimientoDiario.SeGeneroRetrasoEquipo == null ) ||
                    ( pSeguimientoDiario.SeGeneroRetrasoEquipo == true && pSeguimientoDiario.NumeroHorasRetrasoEquipo == null ) ||

                    string.IsNullOrEmpty( pSeguimientoDiario.ProductividadCodigo ) ||
                    string.IsNullOrEmpty( pSeguimientoDiario.ProductividadObservaciones ) ||
                    ( pSeguimientoDiario.ProductividadCodigo == "3" && pSeguimientoDiario.CausaIndisponibilidadProductividadCodigo == null ) ||
                    ( pSeguimientoDiario.ProductividadCodigo == "3" && pSeguimientoDiario.SeGeneroRetrasoProductividad== null ) ||
                    ( pSeguimientoDiario.SeGeneroRetrasoProductividad == true && pSeguimientoDiario.NumeroHorasRetrasoProductividad == null )

               )
            {
                completo = false;
            }

            return completo;
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
                        pSeguimientoDiario.SeguimientoSemanalId = _context.SeguimientoSemanal.FirstOrDefault().SeguimientoSemanalId;
                        pSeguimientoDiario.RegistroCompleto = VerificarRegistroCompleto( pSeguimientoDiario );

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

                        seguimientoDiario.RegistroCompleto = VerificarRegistroCompleto( seguimientoDiario );

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

        public async Task<SeguimientoDiario> GetDailyFollowUpById( int pId )
        {
            SeguimientoDiario seguimiento = _context.SeguimientoDiario.Find( pId );

            return seguimiento;
        }
        
        public async Task<List<SeguimientoDiario>> GetDailyFollowUpByContratacionProyectoId( int pId )
        {
            List<VProyectosXcontrato> listaProyectos = _context.VProyectosXcontrato.ToList();

            List<SeguimientoDiario> listaSeguimientos = _context.SeguimientoDiario
                                                                    .Where( r => r.ContratacionProyectoId == pId )
                                                                    .Include( r => r.ContratacionProyecto )
                                                                    .ToList();

            listaSeguimientos.ForEach( s => {

                s.ContratacionProyecto.Proyecto = new Proyecto();
                s.ContratacionProyecto.Proyecto.InfoProyecto = listaProyectos.Where( r => r.ProyectoId == s.ContratacionProyecto.ProyectoId ).FirstOrDefault();
            });

            return listaSeguimientos;
        }

        public async Task<List<string>> GetDatesAvailableByContratacioProyectoId( int pId )
        {
            List<string> listaFechas = new List<string>();
            List<DateTime> listaFechasTotal = new List<DateTime>();
            
            ContratacionProyecto contratacion = await _context.ContratacionProyecto
                                                                .Where( r => r.ContratacionProyectoId == pId )
                                                                .Include( r => r.Proyecto )
                                                                .Include( r => r.Contratacion )
                                                                    .ThenInclude( r => r.Contrato )     
                                                                .Include( r => r.SeguimientoDiario )
                                                                .FirstOrDefaultAsync();

            DateTime fechaInicial = contratacion.Contratacion.Contrato.FirstOrDefault().FechaActaInicioFase2.Value;
            DateTime fechaFin = fechaInicial.AddMonths( contratacion.Proyecto.PlazoMesesObra.Value );
            fechaFin = fechaFin.AddDays( contratacion.Proyecto.PlazoDiasObra.Value );

            while( fechaFin >= fechaInicial ){
                listaFechasTotal.Add( fechaInicial );
                fechaInicial = fechaInicial.AddDays(1);
                
            }

            listaFechasTotal.ForEach( f => {
                if ( contratacion.SeguimientoDiario.Where( s => s.FechaSeguimiento == f && s.Eliminado != true ).Count() == 0 ){
                    listaFechas.Add( f.ToShortDateString() );
                }
            });

            return listaFechas;
        } 

        public async Task<Respuesta> DeleteDailyFollowUp( int pId, string pUsuario )
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Seguimiento_Diario, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SeguimientoDiario seguimientoDiario = _context.SeguimientoDiario.Find( pId );

                seguimientoDiario.UsuarioModificacion = pUsuario;
                seguimientoDiario.FechaModificacion = DateTime.Now;
                seguimientoDiario.Eliminado = true;

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_seguimiento_diario, GeneralCodes.EliminacionExitosa, idAccion, pUsuario, "ELIMINAR SEGUIMIENTO DIARIO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_seguimiento_diario, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                };
            }
        }

    }
}
