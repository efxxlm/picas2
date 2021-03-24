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
            
            return await _context.ProyectoEntregaEtc.Where(r => r.InformeFinalId == informeFinalId)
                                                    .Include(r => r.RepresentanteEtcrecorrido)
                                                    .FirstOrDefaultAsync();
        }

        public async Task<List<dynamic>> GetProyectoEntregaETCByInformeFinalId(int pInformeFinalId)
        {
            InformeFinal informeFinal = _context.InformeFinal.Find(pInformeFinalId);
            String numeroContratoObra = string.Empty,numeroContratoInterventoria = string.Empty;
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
                                        .Include(r => r.RepresentanteEtcrecorrido)
                                        .FirstOrDefaultAsync();

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
                if (pRecorrido.ProyectoEntregaEtcid == 0)
                {
                    strCrearEditar = "CREAR ENTREGA DE PROYECTO ETC - RECORRIDO";
                    pRecorrido.FechaCreacion = DateTime.Now;
                    _context.ProyectoEntregaEtc.Add(pRecorrido);
                }
                else
                {
                    strCrearEditar = "ACTUALIZAR ENTREGA DE PROYECTO ETC - RECORRIDO";

                    await _context.Set<ProyectoEntregaEtc>().Where(r => r.ProyectoEntregaEtcid == pRecorrido.ProyectoEntregaEtcid)
                                                                   .UpdateAsync(r => new ProyectoEntregaEtc()
                                                                   {
                                                                       FechaModificacion = DateTime.Now,
                                                                       UsuarioModificacion = pRecorrido.UsuarioCreacion,
                                                                       FechaRecorridoObra = pRecorrido.FechaRecorridoObra,
                                                                       NumRepresentantesRecorrido = pRecorrido.NumRepresentantesRecorrido,
                                                                       FechaFirmaActaEngregaFisica =pRecorrido.FechaFirmaActaEngregaFisica,
                                                                       UrlActaEntregaFisica = pRecorrido.UrlActaEntregaFisica
                                                                   });
                }

                foreach (RepresentanteEtcrecorrido representanteEtcrecorrido in pRecorrido.RepresentanteEtcrecorrido)
                {
                    representanteEtcrecorrido.UsuarioCreacion = pRecorrido.UsuarioCreacion.ToUpper();
                    await this.CreateEditRepresentanteETC(representanteEtcrecorrido);
                }

                _context.SaveChanges();

                validateRegistroCompletoEtc(pRecorrido.InformeFinalId);

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
                if (pDocumentos.ProyectoEntregaEtcid == 0)
                {
                    strCrearEditar = "CREAR ENTREGA DE PROYECTO ETC - DOC";
                    pDocumentos.FechaCreacion = DateTime.Now;
                    pDocumentos.RegistroCompletoRemision = !String.IsNullOrEmpty(pDocumentos.NumRadicadoDocumentosEntregaEtc) && pDocumentos.FechaEntregaDocumentosEtc != null ? true : false;
                    _context.ProyectoEntregaEtc.Add(pDocumentos);
                }
                else
                {
                    strCrearEditar = "ACTUALIZAR ENTREGA DE PROYECTO ETC - DOC";

                    await _context.Set<ProyectoEntregaEtc>().Where(r => r.ProyectoEntregaEtcid == pDocumentos.ProyectoEntregaEtcid)
                                                                   .UpdateAsync(r => new ProyectoEntregaEtc()
                                                                   {
                                                                       FechaModificacion = DateTime.Now,
                                                                       UsuarioModificacion = pDocumentos.UsuarioCreacion,
                                                                       FechaEntregaDocumentosEtc = pDocumentos.FechaEntregaDocumentosEtc,
                                                                       NumRadicadoDocumentosEntregaEtc = pDocumentos.NumRadicadoDocumentosEntregaEtc,
                                                                       RegistroCompletoRemision = !String.IsNullOrEmpty(pDocumentos.NumRadicadoDocumentosEntregaEtc) && pDocumentos.FechaEntregaDocumentosEtc != null ? true : false

                });
                }
                _context.SaveChanges();

                validateRegistroCompletoEtc(pDocumentos.InformeFinalId);

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
                if (pActaServicios.ProyectoEntregaEtcid == 0)
                {
                    strCrearEditar = "CREAR ENTREGA DE PROYECTO ETC - ACTA";
                    pActaServicios.FechaCreacion = DateTime.Now;
                    pActaServicios.RegistroCompletoActaBienesServicios = !String.IsNullOrEmpty(pActaServicios.ActaBienesServicios) && pActaServicios.FechaFirmaActaBienesServicios != null ? true : false;
                    _context.ProyectoEntregaEtc.Add(pActaServicios);
                }
                else
                {
                    strCrearEditar = "ACTUALIZAR ENTREGA DE PROYECTO ETC - ACTA";

                    await _context.Set<ProyectoEntregaEtc>().Where(r => r.ProyectoEntregaEtcid == pActaServicios.ProyectoEntregaEtcid)
                                                                   .UpdateAsync(r => new ProyectoEntregaEtc()
                                                                   {
                                                                       FechaModificacion = DateTime.Now,
                                                                       UsuarioModificacion = pActaServicios.UsuarioCreacion,
                                                                       FechaFirmaActaBienesServicios = pActaServicios.FechaFirmaActaBienesServicios,
                                                                       ActaBienesServicios = pActaServicios.ActaBienesServicios,
                                                                       RegistroCompletoActaBienesServicios = !String.IsNullOrEmpty(pActaServicios.ActaBienesServicios) && pActaServicios.FechaFirmaActaBienesServicios != null ? true : false
                                                                   });
                }
                _context.SaveChanges();

                validateRegistroCompletoEtc(pActaServicios.InformeFinalId);

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

        //Alerta Acta bienes y servicios
        /*public async Task GetInformeFinalActaBienesServicios(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            DateTime RangoFechaConDiasHabiles = await _commonService.CalculardiasLaborales(30, DateTime.Now);

            List<InformeFinal> informeFinal = _context.InformeFinal
                .Where(r => r.EstadoAprobacion == ConstantCodigoEstadoAprobacionInformeFinal.Enviado_verificacion_liquidacion_novedades && (String.IsNullOrEmpty(r.EstadoCumplimiento) || r.EstadoCumplimiento == ConstantCodigoEstadoCumplimientoInformeFinal.En_proceso_validacion_cumplimiento))
                .Include(r => r.Proyecto)
                .ToList();

            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Supervisor || x.PerfilId == (int)EnumeratorPerfil.Tecnica).Include(y => y.Usuario);
            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.Alerta5DiasGrupoNovedades5_1_4);

            foreach (var informe in informeFinal)
            {

                if (informeFinal.Count() > 0 && informe.FechaEnvioGrupoNovedades > RangoFechaConDiasHabiles)
                {
                    string template = TemplateRecoveryPassword.Contenido
                                .Replace("_LinkF_", pDominioFront)
                                .Replace("[LLAVE_MEN]", informe.Proyecto.LlaveMen)
                                .Replace("[ESTADO_CUMPLIMIENTO]", String.IsNullOrEmpty(informe.EstadoCumplimiento) ? "Sin Validación" : await _commonService.GetNombreDominioByCodigoAndTipoDominio(informe.EstadoCumplimiento, 163))
                                .Replace("[FECHA_ENVIO_NOVEDADES]", ((DateTime)informe.FechaEnvioGrupoNovedades).ToString("yyyy-MM-dd"));

                    foreach (var item in usuarios)
                    {
                        Helpers.Helpers.EnviarCorreo(item.Usuario.Email, "Pendiente por revisión final. ", template, pSender, pPassword, pMailServer, pMailPort);
                    }

                }
            }
        }*/

        private bool validateRegistroCompletoEtc(int informeFinalId)
        {
            bool state = false;

            ProyectoEntregaEtc proyectoEntregaEtc = _context.ProyectoEntregaEtc.Where(r => r.InformeFinalId == informeFinalId).FirstOrDefault();
            if (proyectoEntregaEtc != null)
            {
                if (proyectoEntregaEtc.RegistroCompletoActaBienesServicios == true && proyectoEntregaEtc.RegistroCompletoRecorridoObra == true && proyectoEntregaEtc.RegistroCompletoRemision == true)
                {
                    state = true;
                }
                _context.Set<InformeFinal>().Where(r => r.InformeFinalId == informeFinalId)
                .Update(r => new InformeFinal()
                {
                    FechaModificacion = DateTime.Now,
                    UsuarioModificacion = proyectoEntregaEtc.UsuarioCreacion,
                    EstadoEntregaEtc = ConstantCodigoEstadoProyectoEntregaETC.En_proceso_de_entrega_ETC,
                    RegistroCompletoEntregaEtc = state
                });
            }

            _context.SaveChanges();

            return state;
        }
    }
}
