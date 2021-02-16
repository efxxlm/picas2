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
    public class ValidateFulfilmentFinalReportService : IValidateFulfilmentFinalReportService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public ValidateFulfilmentFinalReportService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }

        public async Task<List<InformeFinal>> GetListInformeFinal()
        {
            List<InformeFinal> list = await _context.InformeFinal
                            .Where(r=> r.EstadoAprobacion == ConstantCodigoEstadoAprobacionInformeFinal.Enviado_verificacion_liquidacion_novedades)
                            .Include(r=> r.Proyecto)
                                .ThenInclude(r => r.InstitucionEducativa)
                            .ToListAsync();
            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();
            //List<Dominio> TipoObraIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar).ToList();

            //List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();

            foreach(var item in list)
            {
                InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == item.Proyecto.SedeId).FirstOrDefault();
                //Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == item.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                //item.Proyecto.MunicipioObj = Municipio;
                //item.Proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                item.Proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == item.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
                item.Proyecto.Sede = Sede;
                if (String.IsNullOrEmpty(item.EstadoCumplimiento)|| item.EstadoCumplimiento == "0")
                {
                    item.EstadoCumplimientoString = "Sin validación";
                }
                else
                {
                    item.EstadoCumplimientoString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.EstadoCumplimiento, 163);
                }
            }
            return list;
        }

        public async Task<List<dynamic>> GetInformeFinalByProyectoId(int pProyectoId)
        {
            String numeroContratoObra = string.Empty, nombreContratistaObra = string.Empty, numeroContratoInterventoria = string.Empty, nombreContratistaInterventoria = string.Empty;
            String fechaTerminacionInterventoria = string.Empty;
            String fechaTerminacionObra = string.Empty;

            List<dynamic> ProyectoAjustado = new List<dynamic>();
            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();
            List<Dominio> TipoObraIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar).ToList();

            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();
            Proyecto proyecto = await _context.Proyecto.Where(r => r.ProyectoId == pProyectoId)
                                                        .Include(r => r.InformeFinal)
                                                         .Include(r => r.InstitucionEducativa)
                                                         .FirstOrDefaultAsync();
            List<InformeFinalObservaciones> informeFinalObservacionesCumplimiento = _context.InformeFinalObservaciones.Where(r => r.InformeFinalId == proyecto.InformeFinal.FirstOrDefault().InformeFinalId && r.EsGrupoNovedades == true).ToList();

            InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == proyecto.SedeId).FirstOrDefault();
            Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == proyecto.LocalizacionIdMunicipio).FirstOrDefault();
            proyecto.MunicipioObj = Municipio;
            proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
            proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
            proyecto.Sede = Sede;
            proyecto.InformeFinal.FirstOrDefault().InformeFinalObservaciones = informeFinalObservacionesCumplimiento;
            List<ContratacionProyecto> ListContratacion = await _context.ContratacionProyecto
                                                        .Where(r => r.ProyectoId == pProyectoId)
                                                        .Include(r => r.Contratacion)
                                                         .ThenInclude(r => r.Contratista)
                                                        .Include(r => r.Contratacion)
                                                         .ThenInclude(r => r.Contrato)
                                                        .ToListAsync();
            ListContratacion.FirstOrDefault().Contratacion.TipoContratacionCodigo = TipoObraIntervencion.Where(r => r.Codigo == ListContratacion.FirstOrDefault().Contratacion.TipoSolicitudCodigo).Select(r => r.Nombre).FirstOrDefault();

            foreach (var item in ListContratacion)
            {
                Contrato contrato = await _context.Contrato.Where(r => r.ContratacionId == item.ContratacionId).FirstOrDefaultAsync();
                Contratacion contratacion = await _context.Contratacion.Where(r => r.ContratacionId == item.ContratacionId).FirstOrDefaultAsync();
                if (contrato != null)
                {
                    if (contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Obra)
                    {

                        nombreContratistaObra = contratacion.Contratista != null ? contratacion.Contratista.Nombre : string.Empty;
                        numeroContratoObra = contrato.NumeroContrato != null ? contrato.NumeroContrato : string.Empty;
                        fechaTerminacionObra = contrato.FechaTerminacionFase2 != null ? Convert.ToDateTime(contrato.FechaTerminacionFase2).ToString("yyyy-MM-dd") : string.Empty;
                    }
                    else if (contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Interventoria)
                    {
                        nombreContratistaInterventoria = contratacion.Contratista != null ? contratacion.Contratista.Nombre : string.Empty;
                        numeroContratoInterventoria = contrato.NumeroContrato != null ? contrato.NumeroContrato : string.Empty;
                        fechaTerminacionInterventoria = contrato.FechaTerminacionFase2 != null ? Convert.ToDateTime(contrato.FechaTerminacionFase2).ToString("yyyy-MM-dd") : string.Empty;

                    }
                }
            }
            ProyectoAjustado.Add(new
            {
                proyecto = proyecto,
                numeroContratoObra = numeroContratoObra,
                nombreContratistaObra = nombreContratistaObra,
                numeroContratoInterventoria = numeroContratoInterventoria,
                nombreContratistaInterventoria = nombreContratistaInterventoria,
                fechaTerminacionInterventoria = fechaTerminacionInterventoria,
                fechaTerminacionObra = fechaTerminacionObra
            });

            return ProyectoAjustado;
        }

        public async Task<List<InformeFinalInterventoria>> GetInformeFinalListaChequeoByInformeFinalId(int pInformeFinalId)
        {

            List<InformeFinalInterventoria> ListInformeFinalChequeo = await _context.InformeFinalInterventoria
                                .Where(r => r.InformeFinalId == pInformeFinalId)
                                .Include(r => r.InformeFinalListaChequeo)
                                .Include(r => r.InformeFinalAnexo)
                                .OrderBy(r => r.InformeFinalListaChequeo.Posicion)
                                .ToListAsync();
            InformeFinal informeFinal = _context.InformeFinal.Find(pInformeFinalId);

            foreach (var item in ListInformeFinalChequeo)
            {
                item.AprobacionCodigoString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.AprobacionCodigo, 151);

                if (item.InformeFinalAnexoId != null)
                {
                    item.InformeFinalAnexo.TipoAnexoString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.InformeFinalAnexo.TipoAnexo, 155);
                }
            }
            return ListInformeFinalChequeo;
        }


        public async Task<Respuesta> CreateEditObservacionInformeFinal(InformeFinalObservaciones pObservacion, bool tieneOBservaciones)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Informe_Final_Observacion, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            try
            {
                if (pObservacion.InformeFinalObservacionesId == 0)
                {
                    await _context.Set<InformeFinal>().Where(r => r.InformeFinalId == pObservacion.InformeFinalId && (r.EstadoCumplimiento == "0" || string.IsNullOrEmpty(r.EstadoCumplimiento)))
                                               .UpdateAsync(r => new InformeFinal()
                                               {
                                                   FechaModificacion = DateTime.Now,
                                                   UsuarioModificacion = pObservacion.UsuarioCreacion,
                                                   EstadoCumplimiento = ConstantCodigoEstadoCumplimientoInformeFinal.En_proceso_validacion_cumplimiento,
                                               });
                    strCrearEditar = "CREAR INFORME FINAL OBSERVACIONES";
                    pObservacion.FechaCreacion = DateTime.Now;
                    _context.InformeFinalObservaciones.Add(pObservacion);
                }
                else
                {
                    strCrearEditar = "ACTUALIZAR INFORME FINAL OBSERVACION";

                    await _context.Set<InformeFinalObservaciones>().Where(r => r.InformeFinalObservacionesId == pObservacion.InformeFinalObservacionesId)
                                                                   .UpdateAsync(r => new InformeFinalObservaciones()
                                                                   {
                                                                       FechaModificacion = DateTime.Now,
                                                                       UsuarioModificacion = pObservacion.UsuarioCreacion,
                                                                       Observaciones = pObservacion.Observaciones,
                                                                   });
                }
                await _context.Set<InformeFinal>().Where(r => r.InformeFinalId == pObservacion.InformeFinalId)
                                               .UpdateAsync(r => new InformeFinal()
                                               {
                                                   FechaModificacion = DateTime.Now,
                                                   UsuarioModificacion = pObservacion.UsuarioCreacion,
                                                   TieneObservacionesCumplimiento = tieneOBservaciones,
                                               });
                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarCumplimientoInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, pObservacion.UsuarioCreacion, strCrearEditar)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarCumplimientoInformeFinalProyecto, GeneralCodes.Error, idAccion, pObservacion.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> CreateEditObservacionInformeFinalInterventoria(InformeFinalObservaciones pObservacion, bool tieneOBservaciones)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Informe_Final_Observacion, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            try
            {
                if (pObservacion.InformeFinalObservacionesId == 0)
                {
                    strCrearEditar = "CREAR INFORME FINAL OBSERVACIONES";
                    pObservacion.FechaCreacion = DateTime.Now;
                    _context.InformeFinalObservaciones.Add(pObservacion);
                }
                else
                {
                    strCrearEditar = "ACTUALIZAR INFORME FINAL OBSERVACION";

                    await _context.Set<InformeFinalObservaciones>().Where(r => r.InformeFinalObservacionesId == pObservacion.InformeFinalObservacionesId)
                                                                   .UpdateAsync(r => new InformeFinalObservaciones()
                                                                   {
                                                                       FechaModificacion = DateTime.Now,
                                                                       UsuarioModificacion = pObservacion.UsuarioCreacion,
                                                                       Observaciones = pObservacion.Observaciones,
                                                                   });
                }
                string estadoCumplimiento  = "";
                if (tieneOBservaciones == false)
                {
                    estadoCumplimiento = ConstantCodigoEstadoCumplimientoInformeFinal.Con_observaciones_liquidaciones_novedades;
                }
                else
                {
                    estadoCumplimiento = ConstantCodigoEstadoCumplimientoInformeFinal.Con_Aprobacion_final;
                }
                await _context.Set<InformeFinal>().Where(r => r.InformeFinalId == pObservacion.InformeFinalId)
                                               .UpdateAsync(r => new InformeFinal()
                                               {
                                                   FechaModificacion = DateTime.Now,
                                                   UsuarioModificacion = pObservacion.UsuarioCreacion,
                                                   TieneObservacionesCumplimiento = tieneOBservaciones,
                                                   EstadoCumplimiento = estadoCumplimiento,
                                               }); ;;
                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarCumplimientoInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, pObservacion.UsuarioCreacion, strCrearEditar)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarCumplimientoInformeFinalProyecto, GeneralCodes.Error, idAccion, pObservacion.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> SendFinalReportToSupervision(int pProyectoId, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_A_supervisor_Devolucion_Cumplimiento_Informe_Final, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                InformeFinal informeFinal = _context.InformeFinal.Where(r => r.ProyectoId == pProyectoId).FirstOrDefault();
                if (informeFinal != null)
                {
                    informeFinal.EstadoAprobacion = ConstantCodigoEstadoAprobacionInformeFinal.Con_observaciones_liquidaciones_novedades;
                    informeFinal.UsuarioModificacion = pUsuario;
                    informeFinal.FechaModificacion = DateTime.Now;
                }

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarCumplimientoInformeFinalProyecto, GeneralCodes.OperacionExitosa, idAccion, pUsuario, "INFORME FINAL ")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.ValidarCumplimientoInformeFinalProyecto, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                };
            }
        }
    }
}
