using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class RegisterProjectETCService : IRegisterProjectETCService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        public RegisterProjectETCService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }

        public async Task<List<InformeFinal>> GetListInformeFinal()
        {
            List<InformeFinal> list = await _context.InformeFinal
                            .Where(r => r.EstadoCumplimiento == ConstantCodigoEstadoCumplimientoInformeFinal.Con_Aprobacion_final)
                            .Include(r => r.Proyecto)
                                .ThenInclude(r => r.InstitucionEducativa)
                            .ToListAsync();
            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();
            foreach (var item in list)
            {
                InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == item.Proyecto.SedeId).FirstOrDefault();
                item.Proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == item.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
                item.Proyecto.Sede = Sede;
                if (String.IsNullOrEmpty(item.EstadoEntregaEtc) || item.EstadoEntregaEtc == "0")
                {
                    item.EstadoEntregaETCString = "Sin entrega a ETC";
                }
                else
                {
                    item.EstadoEntregaETCString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.EstadoEntregaEtc, (int)EnumeratorTipoDominio.Estado_Entrega_ETC_proyecto);
                }
            }
            return list;
        }

        public async Task<ProyectoEntregaEtc> GetProyectoEntregaEtc(int informeFinalId)
        {

            ProyectoEntregaEtc proyectoEntregaEtc = await _context.ProyectoEntregaEtc.Where(r => r.InformeFinalId == informeFinalId).FirstOrDefaultAsync();

            if (proyectoEntregaEtc != null)
            {
                List<RepresentanteEtcrecorrido> representantesEtcrecorrido = await _context.RepresentanteEtcrecorrido.Where(r => r.ProyectoEntregaEtcid == proyectoEntregaEtc.ProyectoEntregaEtcid && (r.Eliminado == false || r.Eliminado == null)).ToListAsync();
                proyectoEntregaEtc.RepresentanteEtcrecorrido = representantesEtcrecorrido;
            }

            return proyectoEntregaEtc;

        }

        public async Task<List<dynamic>> GetProyectoEntregaETCByInformeFinalId(int pInformeFinalId)
        {
            InformeFinal informeFinal = _context.InformeFinal.Find(pInformeFinalId);
            String numeroContratoObra = string.Empty, numeroContratoInterventoria = string.Empty;
            List<dynamic> ProyectoAjustado = new List<dynamic>();

            List<ContratacionProyecto> ListContratacion = await _context.ContratacionProyecto
                                            .Where(r => r.ProyectoId == informeFinal.ProyectoId)
                                            .Include(r => r.Contratacion)
                                             .ThenInclude(r => r.Contratista)
                                            .Include(r => r.Contratacion)
                                             .ThenInclude(r => r.Contrato)
                                            .Include(r => r.Proyecto)
                                            .ToListAsync();
            List<Dominio> TipoObraIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Opcion_por_contratar).ToList();

            ListContratacion.FirstOrDefault().Contratacion.TipoContratacionCodigo = TipoObraIntervencion.Where(r => r.Codigo == ListContratacion.FirstOrDefault().Contratacion.TipoSolicitudCodigo).Select(r => r.Nombre).FirstOrDefault();

            foreach (var item in ListContratacion)
            {
                Contrato contrato = await _context.Contrato.Where(r => r.ContratacionId == item.ContratacionId).FirstOrDefaultAsync();
                Contratacion contratacion = await _context.Contratacion.Where(r => r.ContratacionId == item.ContratacionId).FirstOrDefaultAsync();
                if (contrato != null)
                {
                    if (contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Obra)
                    {
                        numeroContratoObra = contrato.NumeroContrato != null ? contrato.NumeroContrato : string.Empty;
                    }
                    else if (contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Interventoria)
                    {
                        numeroContratoInterventoria = contrato.NumeroContrato != null ? contrato.NumeroContrato : string.Empty;
                    }
                }
            }
            ProyectoEntregaEtc proyectoEntregaEtc = await _context.ProyectoEntregaEtc
                                        .Where(r => r.InformeFinalId == pInformeFinalId)
                                        .FirstOrDefaultAsync();

            if (proyectoEntregaEtc != null)
            {
                List<RepresentanteEtcrecorrido> representantesEtcrecorrido = await _context.RepresentanteEtcrecorrido.Where(r => r.ProyectoEntregaEtcid == proyectoEntregaEtc.ProyectoEntregaEtcid && (r.Eliminado == false || r.Eliminado == null)).ToListAsync();
                proyectoEntregaEtc.RepresentanteEtcrecorrido = representantesEtcrecorrido;
            }

            ProyectoAjustado.Add(new
            {
                proyectoEntregaEtc = proyectoEntregaEtc,
                numeroContratoObra = numeroContratoObra,
                numeroContratoInterventoria = numeroContratoInterventoria,
                llaveMen = ListContratacion.FirstOrDefault().Proyecto.LlaveMen
            });

            return ProyectoAjustado;
        }

        public async Task<Respuesta> CreateEditRecorridoObra(ProyectoEntregaEtc pRecorrido)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Recorrido_Obra, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            try
            {
                ProyectoEntregaEtc proyectoEntregaEtc = _context.ProyectoEntregaEtc.Where(r => r.InformeFinalId == pRecorrido.InformeFinalId).FirstOrDefault();

                if (pRecorrido.ProyectoEntregaEtcid == 0 && proyectoEntregaEtc == null)
                {
                    strCrearEditar = "CREAR ENTREGA DE PROYECTO ETC - RECORRIDO";
                    pRecorrido.FechaCreacion = DateTime.Now;
                    _context.ProyectoEntregaEtc.Add(pRecorrido);
                }
                else
                {
                    strCrearEditar = "ACTUALIZAR ENTREGA DE PROYECTO ETC - RECORRIDO";

                    if (pRecorrido.ProyectoEntregaEtcid == 0)
                    {
                        pRecorrido.ProyectoEntregaEtcid = proyectoEntregaEtc.ProyectoEntregaEtcid;
                    }

                    await _context.Set<ProyectoEntregaEtc>().Where(r => r.ProyectoEntregaEtcid == pRecorrido.ProyectoEntregaEtcid)
                                                                   .UpdateAsync(r => new ProyectoEntregaEtc()
                                                                   {
                                                                       FechaModificacion = DateTime.Now,
                                                                       UsuarioModificacion = pRecorrido.UsuarioCreacion,
                                                                       FechaRecorridoObra = pRecorrido.FechaRecorridoObra,
                                                                       NumRepresentantesRecorrido = pRecorrido.NumRepresentantesRecorrido,
                                                                       FechaFirmaActaEngregaFisica = pRecorrido.FechaFirmaActaEngregaFisica,
                                                                       UrlActaEntregaFisica = pRecorrido.UrlActaEntregaFisica
                                                                   });
                }

                foreach (RepresentanteEtcrecorrido representanteEtcrecorrido in pRecorrido.RepresentanteEtcrecorrido)
                {
                    representanteEtcrecorrido.UsuarioCreacion = pRecorrido.UsuarioCreacion.ToUpper();
                    await this.CreateEditRepresentanteETC(representanteEtcrecorrido);
                }

                _context.SaveChanges();

                await registroCompletoRecorridoObra(pRecorrido);

                //validateRegistroCompletoEtc(pRecorrido.InformeFinalId);

                return
                new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarTransferenciaProyectoETC, GeneralCodes.OperacionExitosa, idAccion, pRecorrido.UsuarioCreacion, strCrearEditar)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarTransferenciaProyectoETC, GeneralCodes.Error, idAccion, pRecorrido.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> CreateEditRepresentanteETC(RepresentanteEtcrecorrido pRepresentante)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Representante_ETC, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            try
            {
                if (pRepresentante.RepresentanteEtcid == 0)
                {
                    strCrearEditar = "CREAR REPRESENTANTE DE PROYECTO ETC - RECORRIDO";
                    pRepresentante.FechaCreacion = DateTime.Now;
                    pRepresentante.RegistroCompleto = (string.IsNullOrEmpty(pRepresentante.Nombre) || string.IsNullOrEmpty(pRepresentante.Cargo) || string.IsNullOrEmpty(pRepresentante.Dependencia)) ? false : true;
                    _context.RepresentanteEtcrecorrido.Add(pRepresentante);
                }
                else
                {
                    strCrearEditar = "ACTUALIZAR REPRESENTANTE DE PROYECTO ETC - RECORRIDO";

                    await _context.Set<RepresentanteEtcrecorrido>().Where(r => r.RepresentanteEtcid == pRepresentante.RepresentanteEtcid)
                                                                   .UpdateAsync(r => new RepresentanteEtcrecorrido()
                                                                   {
                                                                       FechaModificacion = DateTime.Now,
                                                                       UsuarioModificacion = pRepresentante.UsuarioCreacion,
                                                                       Nombre = pRepresentante.Nombre,
                                                                       Cargo = pRepresentante.Cargo,
                                                                       Dependencia = pRepresentante.Dependencia,
                                                                       RegistroCompleto = (string.IsNullOrEmpty(pRepresentante.Nombre) || string.IsNullOrEmpty(pRepresentante.Cargo) || string.IsNullOrEmpty(pRepresentante.Dependencia)) ? false : true

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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarTransferenciaProyectoETC, GeneralCodes.OperacionExitosa, idAccion, pRepresentante.UsuarioCreacion, strCrearEditar)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarTransferenciaProyectoETC, GeneralCodes.Error, idAccion, pRepresentante.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> CreateEditRemisionDocumentosTecnicos(ProyectoEntregaEtc pDocumentos)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Remision_Documentos_Tecnicos, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            try
            {
                ProyectoEntregaEtc proyectoEntregaEtc = _context.ProyectoEntregaEtc.Where(r => r.InformeFinalId == pDocumentos.InformeFinalId).FirstOrDefault();

                bool registroCompletoRemision = !String.IsNullOrEmpty(pDocumentos.NumRadicadoDocumentosEntregaEtc) && pDocumentos.FechaEntregaDocumentosEtc != null ? true : false; ;
                if (pDocumentos.ProyectoEntregaEtcid == 0 && proyectoEntregaEtc == null)
                {
                    strCrearEditar = "CREAR ENTREGA DE PROYECTO ETC - DOC";
                    pDocumentos.FechaCreacion = DateTime.Now;
                    pDocumentos.RegistroCompletoRemision = registroCompletoRemision;
                    _context.ProyectoEntregaEtc.Add(pDocumentos);
                }
                else
                {
                    strCrearEditar = "ACTUALIZAR ENTREGA DE PROYECTO ETC - DOC";

                    if (pDocumentos.ProyectoEntregaEtcid == 0)
                    {
                        pDocumentos.ProyectoEntregaEtcid = proyectoEntregaEtc.ProyectoEntregaEtcid;
                    }

                    await _context.Set<ProyectoEntregaEtc>().Where(r => r.ProyectoEntregaEtcid == pDocumentos.ProyectoEntregaEtcid)
                                                                   .UpdateAsync(r => new ProyectoEntregaEtc()
                                                                   {
                                                                       FechaModificacion = DateTime.Now,
                                                                       UsuarioModificacion = pDocumentos.UsuarioCreacion,
                                                                       FechaEntregaDocumentosEtc = pDocumentos.FechaEntregaDocumentosEtc,
                                                                       NumRadicadoDocumentosEntregaEtc = pDocumentos.NumRadicadoDocumentosEntregaEtc,
                                                                       RegistroCompletoRemision = registroCompletoRemision

                                                                   });
                }
                bool state = false;

                if (proyectoEntregaEtc != null)
                {
                    if (proyectoEntregaEtc.RegistroCompletoActaBienesServicios == true && proyectoEntregaEtc.RegistroCompletoRecorridoObra == true && registroCompletoRemision)
                    {
                        state = true;
                    }
                    _context.Set<InformeFinal>().Where(r => r.InformeFinalId == proyectoEntregaEtc.InformeFinalId)
                    .Update(r => new InformeFinal()
                    {
                        FechaModificacion = DateTime.Now,
                        UsuarioModificacion = proyectoEntregaEtc.UsuarioCreacion,
                        EstadoEntregaEtc = ConstantCodigoEstadoProyectoEntregaETC.En_proceso_de_entrega_ETC,
                        RegistroCompletoEntregaEtc = state
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarTransferenciaProyectoETC, GeneralCodes.OperacionExitosa, idAccion, pDocumentos.UsuarioCreacion, strCrearEditar)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarTransferenciaProyectoETC, GeneralCodes.Error, idAccion, pDocumentos.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        public async Task<Respuesta> CreateEditActaBienesServicios(ProyectoEntregaEtc pActaServicios)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Acta_Bienes_Servicios, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            try
            {
                ProyectoEntregaEtc proyectoEntregaEtc = _context.ProyectoEntregaEtc.Where(r => r.InformeFinalId == pActaServicios.InformeFinalId).FirstOrDefault();
                bool registroCompletoActaBienesServicios = !String.IsNullOrEmpty(pActaServicios.ActaBienesServicios) && pActaServicios.FechaFirmaActaBienesServicios != null ? true : false;
                if (pActaServicios.ProyectoEntregaEtcid == 0 && proyectoEntregaEtc == null)
                {
                    strCrearEditar = "CREAR ENTREGA DE PROYECTO ETC - ACTA";
                    pActaServicios.FechaCreacion = DateTime.Now;
                    pActaServicios.RegistroCompletoActaBienesServicios = registroCompletoActaBienesServicios;
                    _context.ProyectoEntregaEtc.Add(pActaServicios);
                }
                else
                {
                    strCrearEditar = "ACTUALIZAR ENTREGA DE PROYECTO ETC - ACTA";

                    if (pActaServicios.ProyectoEntregaEtcid == 0)
                    {
                        pActaServicios.ProyectoEntregaEtcid = proyectoEntregaEtc.ProyectoEntregaEtcid;
                    }

                    await _context.Set<ProyectoEntregaEtc>().Where(r => r.ProyectoEntregaEtcid == pActaServicios.ProyectoEntregaEtcid)
                                                                   .UpdateAsync(r => new ProyectoEntregaEtc()
                                                                   {
                                                                       FechaModificacion = DateTime.Now,
                                                                       UsuarioModificacion = pActaServicios.UsuarioCreacion,
                                                                       FechaFirmaActaBienesServicios = pActaServicios.FechaFirmaActaBienesServicios,
                                                                       ActaBienesServicios = pActaServicios.ActaBienesServicios,
                                                                       RegistroCompletoActaBienesServicios = registroCompletoActaBienesServicios,
                                                                   });
                }

                bool state = false;

                if (proyectoEntregaEtc != null)
                {
                    if (registroCompletoActaBienesServicios == true && proyectoEntregaEtc.RegistroCompletoRecorridoObra == true && proyectoEntregaEtc.RegistroCompletoRemision == true)
                    {
                        state = true;
                    }
                    _context.Set<InformeFinal>().Where(r => r.InformeFinalId == proyectoEntregaEtc.InformeFinalId)
                    .Update(r => new InformeFinal()
                    {
                        FechaModificacion = DateTime.Now,
                        UsuarioModificacion = proyectoEntregaEtc.UsuarioCreacion,
                        EstadoEntregaEtc = ConstantCodigoEstadoProyectoEntregaETC.En_proceso_de_entrega_ETC,
                        RegistroCompletoEntregaEtc = state
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarTransferenciaProyectoETC, GeneralCodes.OperacionExitosa, idAccion, pActaServicios.UsuarioCreacion, strCrearEditar)
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
                      Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarTransferenciaProyectoETC, GeneralCodes.Error, idAccion, pActaServicios.UsuarioCreacion, ex.InnerException.ToString())
                  };
            }
        }

        //Alerta Si han pasado treinta días desde que se termino el proyecto, y no se ha registrado el acta de entrega de bienes y servicios, el sistema debe enviar una notificación de alerta al apoyo a la supervisión y al supervisor.
        public async Task GetInformeFinalActaBienesServicios()
        {
            DateTime RangoFechaConDiasHabiles = await _commonService.CalculardiasLaborales(30, DateTime.Now);

            List<InformeFinal> informeFinal = _context.InformeFinal
                .Where(r => r.EstadoAprobacion == ConstantCodigoEstadoAprobacionInformeFinal.Enviado_verificacion_liquidacion_novedades && (String.IsNullOrEmpty(r.EstadoCumplimiento) || r.EstadoCumplimiento == ConstantCodigoEstadoCumplimientoInformeFinal.En_proceso_validacion_cumplimiento))
                .Include(r => r.Proyecto)
                .Include(r => r.ProyectoEntregaEtc)
                .ToList();

            List<EnumeratorPerfil> perfilsEnviarCorreo = new List<EnumeratorPerfil> { EnumeratorPerfil.Apoyo,
                                                                                      EnumeratorPerfil.Supervisor
                                                                                     };

            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.Alerta_30_dias_No_acta_bienes_servicios);

            foreach (var informe in informeFinal)
            {

                if (informe.FechaSuscripcion > RangoFechaConDiasHabiles)
                {

                    string strContenido = await ReplaceVariables(TemplateRecoveryPassword.Contenido, informe.InformeFinalId);

                    _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, TemplateRecoveryPassword.Asunto);

                }
            }
        }
        private async Task<bool> registroCompletoRecorridoObra(ProyectoEntregaEtc proyectoEntregaEtc)
        {
            bool state = false;
            ProyectoEntregaEtc proyectoEntregaEtcOld = _context.ProyectoEntregaEtc.Where(r => r.ProyectoEntregaEtcid == proyectoEntregaEtc.ProyectoEntregaEtcid).Include(r => r.RepresentanteEtcrecorrido).FirstOrDefault();
            if (proyectoEntregaEtc != null && proyectoEntregaEtcOld != null)
            {
                if (proyectoEntregaEtc.FechaRecorridoObra != null &&
                    proyectoEntregaEtc.FechaFirmaActaEngregaFisica != null &&
                    !String.IsNullOrEmpty(proyectoEntregaEtc.UrlActaEntregaFisica) &&
                    proyectoEntregaEtc.NumRepresentantesRecorrido != null
                    )
                {
                    int totalRepresentantes = _context.RepresentanteEtcrecorrido.Where(r => r.ProyectoEntregaEtcid == proyectoEntregaEtc.ProyectoEntregaEtcid && (r.Eliminado == false || r.Eliminado == null) && r.RegistroCompleto == true).Count();
                    if (totalRepresentantes == proyectoEntregaEtc.NumRepresentantesRecorrido)
                    {
                        state = true;
                    }
                }
                await _context.Set<ProyectoEntregaEtc>().Where(r => r.ProyectoEntregaEtcid == proyectoEntregaEtc.ProyectoEntregaEtcid)
                .UpdateAsync(r => new ProyectoEntregaEtc()
                {
                    RegistroCompletoRecorridoObra = state
                });

                if (proyectoEntregaEtcOld.RegistroCompletoActaBienesServicios == true && state == true && proyectoEntregaEtcOld.RegistroCompletoRemision == true)
                {
                    state = true;
                }
                else
                {
                    state = false;
                }
            }
            else
            {
                state = false;
            }
            _context.Set<InformeFinal>().Where(r => r.InformeFinalId == proyectoEntregaEtc.InformeFinalId)
            .Update(r => new InformeFinal()
            {
                FechaModificacion = DateTime.Now,
                UsuarioModificacion = proyectoEntregaEtc.UsuarioCreacion,
                EstadoEntregaEtc = ConstantCodigoEstadoProyectoEntregaETC.En_proceso_de_entrega_ETC,
                RegistroCompletoEntregaEtc = state
            });

            _context.SaveChanges();

            return false;

        }

        public async Task<Respuesta> SendProjectToEtc(int informeFinalId, string pUsuario)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_A_ETC, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                InformeFinal informeFinal = _context.InformeFinal.Find(informeFinalId);
                if (informeFinal != null)
                {
                    informeFinal.FechaEnvioEtc = DateTime.Now;
                    informeFinal.EstadoEntregaEtc = ConstantCodigoEstadoProyectoEntregaETC.Entregado_a_ETC;
                    informeFinal.UsuarioModificacion = pUsuario;
                    informeFinal.FechaModificacion = DateTime.Now;

                    //Enviar Correo apoyo supervisor 5.1.1
                    //await EnviarCorreoApoyoSupervisor(informeFinal, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);
                }

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarTransferenciaProyectoETC, GeneralCodes.OperacionExitosa, idAccion, pUsuario, "INFORME FINAL ")
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarTransferenciaProyectoETC, GeneralCodes.Error, idAccion, pUsuario, ex.InnerException.ToString())
                };
            }
        }

        public async Task<Respuesta> DeleteRepresentanteEtcRecorrido(int representanteEtcId, int numRepresentantesRecorrido, string pUsuarioModificacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Representante_Etc, (int)EnumeratorTipoDominio.Acciones);
            try
            {
                RepresentanteEtcrecorrido representanteEtcrecorridoOld = await _context.RepresentanteEtcrecorrido
                    .Where(r => r.RepresentanteEtcid == representanteEtcId).FirstOrDefaultAsync();
                if (representanteEtcrecorridoOld == null)
                {
                    return new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = true,
                        IsValidation = false,
                        Code = ConstanMessagesRegisterWeeklyProgress.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarTransferenciaProyectoETC, ConstanMessagesRegisterWeeklyProgress.Error, idAccion, pUsuarioModificacion, "RepresentanteEtcrecorrido no encontrado".ToUpper())
                    };
                }
                ProyectoEntregaEtc proyectoEntregaEtc = await _context.ProyectoEntregaEtc.FindAsync(representanteEtcrecorridoOld.ProyectoEntregaEtcid);

                representanteEtcrecorridoOld.UsuarioModificacion = pUsuarioModificacion;
                representanteEtcrecorridoOld.FechaModificacion = DateTime.Now;
                representanteEtcrecorridoOld.Eliminado = true;

                if (proyectoEntregaEtc != null)
                {
                    proyectoEntregaEtc.UsuarioModificacion = pUsuarioModificacion;
                    proyectoEntregaEtc.FechaModificacion = DateTime.Now;
                    proyectoEntregaEtc.NumRepresentantesRecorrido = numRepresentantesRecorrido;
                }

                await _context.SaveChangesAsync();

                await registroCompletoRecorridoObra(proyectoEntregaEtc);

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarTransferenciaProyectoETC, ConstanMessagesRegisterWeeklyProgress.OperacionExitosa, idAccion, pUsuarioModificacion, "Eliminar RepresentanteEtcrecorrido".ToUpper())
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.RegistrarTransferenciaProyectoETC, ConstantSesionComiteTecnico.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString())
                };
            }
        }

        private async Task<string> ReplaceVariables(string template, int pInformeFinalId)
        {
            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();
            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();
            List<Dominio> TipoIntervencion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).ToList();

            InformeFinal informeFinal = _context.InformeFinal
                .Where(r => r.InformeFinalId == pInformeFinalId)
                .Include(r => r.Proyecto)
                    .ThenInclude(r => r.InstitucionEducativa)
                .FirstOrDefault();

            InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == informeFinal.Proyecto.SedeId).FirstOrDefault();
            Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == informeFinal.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
            informeFinal.Proyecto.MunicipioObj = Municipio;
            informeFinal.Proyecto.DepartamentoObj = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
            informeFinal.Proyecto.tipoIntervencionString = TipoIntervencion.Where(r => r.Codigo == informeFinal.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre;
            informeFinal.Proyecto.Sede = Sede;

            ContratacionProyecto contratacionProyecto = _context.ContratacionProyecto
                .Where(r => r.ProyectoId == informeFinal.ProyectoId)
                .Include(r => r.Contratacion)
                    .ThenInclude(r => r.Contrato)
                .FirstOrDefault();

            template = template
                      .Replace("[LLAVE_MEN]", informeFinal.Proyecto.LlaveMen)
                      .Replace("[NUMERO_CONTRATO]", contratacionProyecto.Contratacion.Contrato.FirstOrDefault().NumeroContrato)
                      .Replace("[FECHA_TERMINACION_PROYECTO]", ((DateTime)informeFinal.FechaCreacion).ToString("dd-MMM-yy"))
                      .Replace("[FECHA_SUSCRIPCION]", informeFinal.FechaSuscripcion != null ? ((DateTime)informeFinal.FechaSuscripcion).ToString("dd-MMM-yy") : "")
                      .Replace("[INSTITUCION_EDUCATIVA]", informeFinal.Proyecto.InstitucionEducativa.Nombre)
                      .Replace("[SEDE]", informeFinal.Proyecto.Sede.Nombre)
                      .Replace("[TIPO_INTERVENCION]", informeFinal.Proyecto.tipoIntervencionString)
                      .Replace("[FECHA_ENVIO_APOYO]", informeFinal.FechaEnvioApoyoSupervisor != null ? ((DateTime)informeFinal.FechaEnvioApoyoSupervisor).ToString("dd-MMM-yy") : "")
                      ;


            return template;
        }
    }
}
