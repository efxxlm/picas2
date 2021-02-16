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
                    item.EstadoEntregaETCString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(item.EstadoEntregaETCString, 164);
                }
            }
            return list;
        }


        public async Task<ProyectoEntregaEtc> GetProyectoEntregaETCByInformeFinalId(int pInformeFinalId)
        {
            ProyectoEntregaEtc proyectoEntregaEtc = await _context.ProyectoEntregaEtc
                                        .Where(r => r.InformeFinalId == pInformeFinalId)
                                        .Include(r => r.RepresentanteEtcrecorrido)
                                        .FirstOrDefaultAsync();

            return proyectoEntregaEtc;
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
                await _context.Set<InformeFinal>().Where(r => r.InformeFinalId == pRecorrido.InformeFinalId)
                                               .UpdateAsync(r => new InformeFinal()
                                               {
                                                   FechaModificacion = DateTime.Now,
                                                   UsuarioModificacion = pRecorrido.UsuarioCreacion,
                                                   EstadoEntregaEtc = ConstantCodigoEstadoProyectoEntregaETC.En_proceso_de_entrega_ETC
                                               });
                _context.SaveChanges();

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
                                                                       NumRadicadoDocumentosEntregaEtc = pDocumentos.NumRadicadoDocumentosEntregaEtc
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
                if (pActaServicios.ProyectoEntregaEtcid == 0)
                {
                    strCrearEditar = "CREAR ENTREGA DE PROYECTO ETC - ACTA";
                    pActaServicios.FechaCreacion = DateTime.Now;
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
                                                                       ActaBienesServicios = pActaServicios.ActaBienesServicios
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
    }
}
