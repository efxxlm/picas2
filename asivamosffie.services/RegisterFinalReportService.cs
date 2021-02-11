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
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class RegisterFinalReportService : IRegisterFinalReportService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly ITechnicalRequirementsConstructionPhaseService _technicalRequirementsConstructionPhaseService;

        public RegisterFinalReportService(devAsiVamosFFIEContext context, ICommonService commonService, ITechnicalRequirementsConstructionPhaseService technicalRequirementsConstructionPhaseService)
        {
            _context = context;
            _commonService = commonService;
            _technicalRequirementsConstructionPhaseService = technicalRequirementsConstructionPhaseService;
        }

        public async Task<List<VProyectosCierre>> gridRegisterFinalReport()
        {
            return await _context.VProyectosCierre.OrderByDescending(r => r.ProyectoId).ToListAsync();
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
                                                              .ThenInclude(r => r.InformeFinalInterventoria)
                                                                    .ThenInclude(r => r.InformeFinalInterventoriaObservaciones)
                                                         .Include(r => r.InstitucionEducativa)
                                                         .FirstOrDefaultAsync();

            InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == proyecto.SedeId).FirstOrDefault();
            Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == proyecto.LocalizacionIdMunicipio).FirstOrDefault();
            proyecto.MunicipioObj = Municipio;
            proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
            proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
            proyecto.Sede = Sede;
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

        public async Task<InformeFinalInterventoria> GetInformeFinalAnexoByInformeFinalInterventoriaId(int pInformeFinalInterventoriaId)
        {
            InformeFinalInterventoria InformeFinalAnexo = await _context.InformeFinalInterventoria.Where(r => r.InformeFinalInterventoriaId == pInformeFinalInterventoriaId)
                                                        .Include(r => r.InformeFinalAnexo).FirstOrDefaultAsync();
            InformeFinalAnexo.InformeFinalInterventoriaObservaciones = await _context.InformeFinalInterventoriaObservaciones.Where(r => r.EsSupervision == true && r.InformeFinalInterventoriaId == pInformeFinalInterventoriaId).ToListAsync();

            return InformeFinalAnexo;
        }

        public async Task<InformeFinalAnexo> GetInformeFinalAnexoByInformeFinalAnexoId(int pInformeFinalAnexoId)
        {
            InformeFinalAnexo InformeFinalAnexo = await _context.InformeFinalAnexo.Where(r => r.InformeFinalAnexoId == pInformeFinalAnexoId).FirstOrDefaultAsync();

            return InformeFinalAnexo;
        }

        public async Task<InformeFinal> GetInformeFinalByInformeFinalId(int pInformeFinalId)
        {
            InformeFinal InformeFinal = await _context.InformeFinal.Where(r => r.InformeFinalId == pInformeFinalId).FirstOrDefaultAsync();

            return InformeFinal;
        }

        public async Task<InformeFinalInterventoriaObservaciones> GetInformeFinalInterventoriaObservacionByInformeFinalObservacion(int pObservacionId)
        {
            InformeFinalInterventoriaObservaciones informeFinalInterventoriaObservaciones = await _context.InformeFinalInterventoriaObservaciones.Where(r => r.InformeFinalInterventoriaObservacionesId == pObservacionId).FirstOrDefaultAsync();

            return informeFinalInterventoriaObservaciones;
        }

        public async Task<bool> VerificarInformeFinalEstadoCompleto(int pInformeFinalId)
        {
            bool esCompleto = true;

            List<InformeFinalInterventoria> ListInformeTotalInterventoria = await _context.InformeFinalInterventoria.Where(cc => cc.InformeFinalId == pInformeFinalId)
                                                        .ToListAsync();

            int phrasesCount = await _context.InformeFinalListaChequeo.CountAsync();
            InformeFinal informeFinal = _context.InformeFinal.Where(r => r.InformeFinalId == pInformeFinalId).FirstOrDefault();
            //Validación # 1
            if (informeFinal != null)
            {
                if (!informeFinal.RegistroCompleto || informeFinal.EstadoInforme == ConstantCodigoEstadoInformeFinal.Con_informe_Registrado)
                {
                    if (ListInformeTotalInterventoria.Count() < phrasesCount)
                    {
                        informeFinal.EstadoInforme = ConstantCodigoEstadoInformeFinal.En_proceso_de_registro;
                        informeFinal.RegistroCompleto = false;
                        return false;
                    }
                    else
                    {
                        InformeFinalInterventoria existe_no_cumple = _context.InformeFinalInterventoria.Where(r => r.InformeFinalId == pInformeFinalId && r.CalificacionCodigo == ConstantCodigoCalificacionInformeFinal.No_Cumple).FirstOrDefault();
                        InformeFinalInterventoria existe_cumple_no_data = _context.InformeFinalInterventoria.Where(r => r.InformeFinalId == pInformeFinalId && r.CalificacionCodigo == ConstantCodigoCalificacionInformeFinal.Cumple && (r.InformeFinalAnexoId == null || r.InformeFinalAnexoId == 0)).FirstOrDefault();
                        InformeFinalInterventoria existe_no_diligenciado = _context.InformeFinalInterventoria.Where(r => r.InformeFinalId == pInformeFinalId && r.CalificacionCodigo == "0" || String.IsNullOrEmpty(r.CalificacionCodigo)).FirstOrDefault();
                        if (existe_no_cumple != null || existe_cumple_no_data != null || existe_no_diligenciado != null)
                        {
                            informeFinal.EstadoInforme = ConstantCodigoEstadoInformeFinal.En_proceso_de_registro;
                            informeFinal.RegistroCompleto = false;
                            return false;
                        }
                    }
                }
                else if (informeFinal.EstadoInforme == ConstantCodigoEstadoInformeFinal.Con_Observaciones_del_supervisor)
                {
                    InformeFinalInterventoria no_actualizada = _context.InformeFinalInterventoria.Where(r => r.InformeFinalId == pInformeFinalId && r.ObservacionNueva == true).FirstOrDefault();
                    if (no_actualizada == null)
                    {
                        informeFinal.EstadoInforme = ConstantCodigoEstadoInformeFinal.En_proceso_de_registro;
                        informeFinal.RegistroCompleto = false;
                        return false;
                    }
                }
                informeFinal.EstadoInforme = ConstantCodigoEstadoInformeFinal.Con_informe_Registrado;
                informeFinal.RegistroCompleto = true;
            }
            else
            {
                return false;
            }

            //_context.SaveChanges();
            return esCompleto;
        }

        public async Task<Respuesta> CreateEditInformeFinal(InformeFinal pInformeFinal)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Informe_Final, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = string.Empty;
            try
            {

                if (pInformeFinal.InformeFinalId == 0)
                {
                    CreateEdit = "CREAR INFORME FINAL";
                    pInformeFinal.FechaCreacion = DateTime.Now;
                    pInformeFinal.Eliminado = false;
                    //pInformeFinal.EstadoValidacion = "0";
                    pInformeFinal.EstadoInforme = ConstantCodigoEstadoInformeFinal.En_proceso_de_registro;
                    pInformeFinal.RegistroCompletoValidacion = false;
                    _context.InformeFinal.Add(pInformeFinal);
                    _context.SaveChanges();
                    //Crear informe final interventoria
                    await CreateInformeFinalInterventoriaByInformeFinalId(pInformeFinal.InformeFinalId, pInformeFinal.UsuarioCreacion);
                }
                else
                {
                    CreateEdit = "ACTUALIZAR INFORME FINAL";
                    await _context.Set<InformeFinal>()
                              .Where(r => r.InformeFinalId == pInformeFinal.InformeFinalId && r.ProyectoId == pInformeFinal.ProyectoId)
                                                                                  .UpdateAsync(r => new InformeFinal()
                                                                                  {
                                                                                      FechaModificacion = DateTime.Now,
                                                                                      UsuarioModificacion = pInformeFinal.UsuarioCreacion,
                                                                                      UrlActa = pInformeFinal.UrlActa,
                                                                                      FechaSuscripcion = pInformeFinal.FechaSuscripcion,
                                                                                  });
                }

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.OperacionExitosa, idAccion, pInformeFinal.UsuarioCreacion, CreateEdit)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.Error, idAccion, pInformeFinal.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<List<dynamic>> GetInformeFinalListaChequeoByProyectoId(int pProyectoId)
        {
            List<dynamic> ListChequeo = new List<dynamic>();
            String calificacionCodigo = string.Empty;
            InformeFinal informeFinal = _context.InformeFinal.Where(r => r.ProyectoId == pProyectoId).FirstOrDefault();
            int informeFinalInterventoriaObservacionesId = 0;
            bool tieneObservacionNoCumple = false;

            if (informeFinal != null)//Sino han llenado los campos de informe final no se muestra la lista de chequeos
            {
                List<InformeFinalInterventoria> listInformeFinalInterventoria = await _context.InformeFinalInterventoria
                .Where(r => r.InformeFinalId == informeFinal.InformeFinalId)
                .Include(r => r.InformeFinalListaChequeo)
                .Include(r => r.InformeFinalAnexo)
                .OrderBy(r => r.InformeFinalListaChequeo.Posicion)
                .ToListAsync();

                foreach (var item in listInformeFinalInterventoria)
                {

                    if (item.CalificacionCodigo == ConstantCodigoCalificacionInformeFinal.No_Cumple)
                    {
                        //Validar si tiene observaciones
                        InformeFinalInterventoriaObservaciones informeFinalInterventoriaObservaciones = _context.InformeFinalInterventoriaObservaciones.Where(r => r.InformeFinalInterventoriaId == item.InformeFinalInterventoriaId && r.EsCalificacion == true).FirstOrDefault();
                        if (informeFinalInterventoriaObservaciones != null)
                        {
                            informeFinalInterventoriaObservacionesId = informeFinalInterventoriaObservaciones.InformeFinalInterventoriaObservacionesId;
                            tieneObservacionNoCumple = true;
                        }
                        else
                        {
                            informeFinalInterventoriaObservacionesId = 0;
                            tieneObservacionNoCumple = false;
                        }
                    }
                    else
                    {
                        informeFinalInterventoriaObservacionesId = 0;
                        tieneObservacionNoCumple = false;
                    }

                    ListChequeo.Add(new
                    {
                        Nombre = item.InformeFinalListaChequeo.Nombre,
                        CalificacionCodigo = item.CalificacionCodigo,
                        InformeFinalListaChequeoId = item.InformeFinalListaChequeoId,
                        InformeFinalInterventoriaId = item.InformeFinalInterventoriaId,
                        TieneObservacionSupervisor = item.TieneObservacionSupervisor is null ? false : item.TieneObservacionSupervisor,
                        InformeFinalId = item.InformeFinalId,
                        InformeFinalAnexoId = item.InformeFinalAnexoId,
                        InformeFinalInterventoriaObservacionesId = informeFinalInterventoriaObservacionesId,
                        TieneObservacionNoCumple = tieneObservacionNoCumple,
                        CalificacionCodigoString = String.IsNullOrEmpty(item.CalificacionCodigo) || item.CalificacionCodigo == "0" ? null : await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.CalificacionCodigo, 151),
                        EstadoInformeFinal = informeFinal.EstadoInforme,
                        posicion = item.InformeFinalListaChequeo.Posicion
                    });
                }
            }

            return ListChequeo;
        }

        private async Task<Respuesta> CreateInformeFinalInterventoriaByInformeFinalId(int pInformeFinalAnexoId, string user)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Informe_Final_Interventoria, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = string.Empty;
            try
            {
                List<InformeFinalListaChequeo> ListInformeFinalChequeo = await _context.InformeFinalListaChequeo
                    .ToListAsync();
                CreateEdit = "CREAR INFORME FINAL INTERVENTORIA";
                foreach (var item in ListInformeFinalChequeo)
                {
                    InformeFinalInterventoria informeFinalInterventoria = new InformeFinalInterventoria
                    {
                        FechaCreacion = DateTime.Now,
                        UsuarioCreacion = user,
                        InformeFinalId = pInformeFinalAnexoId,
                        InformeFinalListaChequeoId = item.InformeFinalListaChequeoId,
                    };
                    _context.InformeFinalInterventoria.Add(informeFinalInterventoria);
                }

                //_context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.OperacionExitosa, idAccion, user, CreateEdit)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.Error, idAccion, user, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> CreateEditInformeFinalInterventoriabyInformeFinal(InformeFinal informeFinal, string user)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Informe_Final_Interventoria, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;

            try
            {

                foreach (InformeFinalInterventoria informeFinalInterventoria in informeFinal.InformeFinalInterventoria)
                {
                    informeFinalInterventoria.UsuarioCreacion = user.ToUpper();
                    await this.CreateEditInformeFinalInterventoria(informeFinalInterventoria);
                }

                await VerificarInformeFinalEstadoCompleto(informeFinal.InformeFinalId);

                await _context.SaveChangesAsync();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.OperacionExitosa, idAccion, informeFinal.UsuarioCreacion, strCrearEditar)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.Error, idAccion, informeFinal.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> CreateEditInformeFinalInterventoria(InformeFinalInterventoria pInformeFinalInterventoriaId)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Informe_Final_Interventoria, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = string.Empty;
            try
            {
                InformeFinalInterventoria informeFinalInterventoria = _context.InformeFinalInterventoria
                        .Where(r => r.InformeFinalId == pInformeFinalInterventoriaId.InformeFinalId && r.InformeFinalListaChequeoId == pInformeFinalInterventoriaId.InformeFinalListaChequeoId).FirstOrDefault();
                InformeFinal informeFinal = _context.InformeFinal
                        .Where(r => r.InformeFinalId == pInformeFinalInterventoriaId.InformeFinalId).FirstOrDefault();

                if (pInformeFinalInterventoriaId.InformeFinalInterventoriaId == 0 && informeFinalInterventoria == null)
                {
                    CreateEdit = "CREAR INFORME FINAL INTERVENTORIA";
                    pInformeFinalInterventoriaId.FechaCreacion = DateTime.Now;
                    _context.InformeFinalInterventoria.Add(pInformeFinalInterventoriaId);
                    informeFinal.EstadoInforme = ConstantCodigoEstadoInformeFinal.En_proceso_de_registro;
                }
                else
                {
                    CreateEdit = "ACTUALIZAR INFORME FINAL INTERVENTORIA";

                    await _context.Set<InformeFinalInterventoria>()
                                .Where(r => r.InformeFinalId == pInformeFinalInterventoriaId.InformeFinalId && r.InformeFinalListaChequeoId == pInformeFinalInterventoriaId.InformeFinalListaChequeoId)
                                                               .UpdateAsync(r => new InformeFinalInterventoria()
                                                               {
                                                                   FechaModificacion = DateTime.Now,
                                                                   UsuarioModificacion = pInformeFinalInterventoriaId.UsuarioCreacion,
                                                                   CalificacionCodigo = pInformeFinalInterventoriaId.CalificacionCodigo,
                                                               });
                    ///Actualiza o crea observaciones segun el caso (Sólo calificación)
                    foreach (InformeFinalInterventoriaObservaciones informeFinalInterventoriaObservaciones in pInformeFinalInterventoriaId.InformeFinalInterventoriaObservaciones)
                    {
                        informeFinalInterventoriaObservaciones.UsuarioCreacion = pInformeFinalInterventoriaId.UsuarioCreacion.ToUpper();
                        await this.CreateEditInformeFinalInterventoriaObservacion(informeFinalInterventoriaObservaciones);
                    }

                    //Actualiza o crea anexos segun el caso (Sólo calificación)
                    if (pInformeFinalInterventoriaId.InformeFinalAnexo != null)
                    {
                        await this.CreateEditInformeFinalAnexo(pInformeFinalInterventoriaId.InformeFinalAnexo, pInformeFinalInterventoriaId.InformeFinalInterventoriaId);
                    }
                }
                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.OperacionExitosa, idAccion, pInformeFinalInterventoriaId.UsuarioCreacion, CreateEdit)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.Error, idAccion, pInformeFinalInterventoriaId.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> CreateEditInformeFinalAnexo(InformeFinalAnexo pInformeFinalAnexoId, int pInformeFinalInterventoriaId)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Informe_Final_Anexo, (int)EnumeratorTipoDominio.Acciones);
            string CreateEdit = string.Empty;
            bool existe = false;
            try
            {
                InformeFinalInterventoria informeFinalInterventoria = _context.InformeFinalInterventoria
                    .Where(r => r.InformeFinalInterventoriaId == pInformeFinalInterventoriaId).FirstOrDefault();

                if (informeFinalInterventoria != null && informeFinalInterventoria.InformeFinalAnexoId != null)
                {
                    existe = true;
                }

                if (pInformeFinalAnexoId.InformeFinalAnexoId == 0)
                {
                    CreateEdit = "CREAR INFORME FINAL ANEXO";
                    pInformeFinalAnexoId.FechaCreacion = DateTime.Now;
                    _context.InformeFinalAnexo.Add(pInformeFinalAnexoId);
                    _context.SaveChanges();
                }
                else
                {

                    CreateEdit = "ACTUALIZAR INFORME FINAL ANEXO";

                    await _context.Set<InformeFinalAnexo>()
                                .Where(r => r.InformeFinalAnexoId == pInformeFinalAnexoId.InformeFinalAnexoId)
                                                               .UpdateAsync(r => new InformeFinalAnexo()
                                                               {
                                                                   FechaModificacion = DateTime.Now,
                                                                   UsuarioModificacion = pInformeFinalAnexoId.UsuarioCreacion,
                                                                   TipoAnexo = pInformeFinalAnexoId.TipoAnexo,
                                                                   NumRadicadoSac = pInformeFinalAnexoId.NumRadicadoSac,
                                                                   FechaRadicado = pInformeFinalAnexoId.FechaRadicado,
                                                                   UrlSoporte = pInformeFinalAnexoId.UrlSoporte,
                                                               });
                    InformeFinal informeFinal = _context.InformeFinal
                       .Where(r => r.InformeFinalId == informeFinalInterventoria.InformeFinalId).FirstOrDefault();

                    if (informeFinal.EstadoInforme == ConstantCodigoEstadoInformeFinal.Con_Observaciones_del_supervisor)
                    {
                        await VerificarInformeFinalEstadoCompleto(informeFinal.InformeFinalId);
                    }
                }
                //Actualizar id interventoria
                if (!existe && informeFinalInterventoria != null)
                {
                    informeFinalInterventoria.InformeFinalAnexoId = pInformeFinalAnexoId.InformeFinalAnexoId;
                }

                _context.SaveChanges();


                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = GeneralCodes.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.OperacionExitosa, idAccion, pInformeFinalAnexoId.UsuarioCreacion, CreateEdit)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.Error, idAccion, pInformeFinalAnexoId.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> CreateEditInformeFinalInterventoriaObservacion(InformeFinalInterventoriaObservaciones pObservacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Informe_Final_Interventoria_Observacion, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            try
            {
                if (pObservacion.InformeFinalInterventoriaObservacionesId == 0)
                {
                    strCrearEditar = "CREAR INFORME FINAL INTERVENTORIA OBSERVACIONES";
                    pObservacion.FechaCreacion = DateTime.Now;
                    _context.InformeFinalInterventoriaObservaciones.Add(pObservacion);
                }
                else
                {
                    strCrearEditar = "ACTUALIZAR INFORME FINAL INTERVENTORIA OBSERVACION";

                    await _context.Set<InformeFinalInterventoriaObservaciones>().Where(r => r.InformeFinalInterventoriaObservacionesId == pObservacion.InformeFinalInterventoriaObservacionesId)
                                                                   .UpdateAsync(r => new InformeFinalInterventoriaObservaciones()
                                                                   {
                                                                       FechaModificacion = DateTime.Now,
                                                                       UsuarioModificacion = pObservacion.UsuarioCreacion,
                                                                       Observaciones = pObservacion.Observaciones,
                                                                   });
                }
                _context.SaveChanges();

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.OperacionExitosa, idAccion, pObservacion.UsuarioCreacion, strCrearEditar)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.Error, idAccion, pObservacion.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> SendFinalReportToSupervision(int pProyectoId, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_A_supervisor_Informe_Final_Interventoria, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                InformeFinal informeFinal = _context.InformeFinal.Where(r => r.ProyectoId == pProyectoId).FirstOrDefault();
                if (informeFinal != null)
                {
                    informeFinal.EstadoInforme = ConstantCodigoEstadoInformeFinal.Con_informe_enviado_para_validación;
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.OperacionExitosa, idAccion, pUsuario, "INFORME FINAL ")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_Informe_Final, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                };
            }
        }

        /*public async Task SendMailToSupervision(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            DateTime RangoFechaConDiasHabiles = await _commonService.CalculardiasLaborales(2, DateTime.Now);

            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Apoyo || x.PerfilId == (int)EnumeratorPerfil.Tecnica).Include(y => y.Usuario);

            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.Alertas2DiasObraInterventoria318);

                {
                DateTime fechaValidacion = DateTime.Now;

                        string template = TemplateRecoveryPassword.Contenido
                                 .Replace("LinkF", pDominioFront)
                                 //.Replace("[NUMERO_CONTRATO]", contrato.NumeroContrato)
                                 .Replace("[FECHA_REQUISITOS]", fechaValidacion.ToString("dd-MMMM-yy"))
                                 //.Replace("[CANTIDAD_PROYECTOS]", contrato.Contratacion.ContratacionProyecto.Where(r => !r.Eliminado).Count().ToString());
                        foreach (var item in usuarios)
                        {
                            Helpers.Helpers.EnviarCorreo(item.Usuario.Email, "Verificación y Aprobación de requisitos pendientes", template, pSender, pPassword, pMailServer, pMailPort);
                        }
                }

        }*/
    }
}
