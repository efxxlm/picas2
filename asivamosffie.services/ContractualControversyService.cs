using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Constants;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;


namespace asivamosffie.services
{
    public class ContractualControversyService : IContractualControversy
    {

        private readonly ICommonService _commonService;
        private readonly IProjectContractingService _IProjectContractingService;
        private readonly devAsiVamosFFIEContext _context;
        private readonly IConverter _converter;

        public ContractualControversyService(devAsiVamosFFIEContext context, ICommonService commonService, IConverter converter, IProjectContractingService projectContractingService)
        {
            _IProjectContractingService = projectContractingService;
            _commonService = commonService;
            _context = context;
            _converter = converter;
            //_settings = settings;
        }

        //CreateEditNuevaActualizacionTramite(ControversiaActuacion
        //“Registrar nueva actualización del trámite”


        //public async Task<Respuesta> CreateEditNuevaActualizacionTramite(ControversiaActuacion controversiaActuacion)
        public async Task<Respuesta> CreateEditControversiaOtros(ControversiaActuacion controversiaActuacion)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearcontroversiaActuacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Controversia_Actuacion, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;

            try
            {
                if (controversiaActuacion != null)
                {

                    if (string.IsNullOrEmpty(controversiaActuacion.ControversiaActuacionId.ToString()) || controversiaActuacion.ControversiaActuacionId == 0)
                    {
                        strCrearEditar = "REGISTRAR CONTROVERSIA ACTUACION";

                        int consecutivo = _context.ControversiaActuacion
                                                        .Where(r => r.ControversiaContractualId == controversiaActuacion.ControversiaContractualId)
                                                        .Count();


                        controversiaActuacion.FechaCreacion = DateTime.Now;
                        //controversiaActuacion.UsuarioCreacion = compromisoSeguimiento.UsuarioCreacion;
                        controversiaActuacion.NumeroActuacion = "ACT controversia " + (consecutivo + 1).ToString("000");
                        controversiaActuacion.Observaciones = Helpers.Helpers.CleanStringInput(controversiaActuacion.Observaciones);
                        controversiaActuacion.ResumenPropuestaFiduciaria = Helpers.Helpers.CleanStringInput(controversiaActuacion.ResumenPropuestaFiduciaria);

                        controversiaActuacion.EsCompleto = ValidarRegistroCompletoControversiaActuacion(controversiaActuacion);

                        controversiaActuacion.Eliminado = false;
                        _context.ControversiaActuacion.Add(controversiaActuacion);

                    }

                    else
                    {

                        var controversiaActuacionActualizar = _context.ControversiaActuacion.Find(controversiaActuacion.ControversiaActuacionId);
                        strCrearEditar = "EDIT CONTROVERSIA ACTUACION";
                        //controversiaActuacionActualizar.Observaciones = Helpers.Helpers.CleanStringInput(controversiaActuacion.Observaciones);
                        //controversiaActuacionActualizar.ResumenPropuestaFiduciaria = Helpers.Helpers.CleanStringInput(controversiaActuacion.ResumenPropuestaFiduciaria);
                        controversiaActuacionActualizar.FechaModificacion = DateTime.Now;
                        controversiaActuacionActualizar.FechaActuacion = controversiaActuacion.FechaActuacion;
                        controversiaActuacionActualizar.ActuacionAdelantadaCodigo = controversiaActuacion.ActuacionAdelantadaCodigo;
                        controversiaActuacionActualizar.ActuacionAdelantadaOtro = controversiaActuacion.ActuacionAdelantadaOtro;
                        controversiaActuacionActualizar.ProximaActuacionCodigo = controversiaActuacion.ProximaActuacionCodigo;
                        controversiaActuacionActualizar.ProximaActuacionOtro = controversiaActuacion.ProximaActuacionOtro;
                        controversiaActuacionActualizar.CantDiasVencimiento = controversiaActuacion.CantDiasVencimiento;
                        controversiaActuacionActualizar.FechaVencimiento = controversiaActuacion.FechaVencimiento;
                        controversiaActuacionActualizar.EsRequiereContratista = controversiaActuacion.EsRequiereContratista;
                        controversiaActuacionActualizar.EsRequiereInterventor = controversiaActuacion.EsRequiereInterventor;
                        controversiaActuacionActualizar.EsRequiereSupervisor = controversiaActuacion.EsRequiereSupervisor;
                        controversiaActuacionActualizar.EsRequiereJuridico = controversiaActuacion.EsRequiereJuridico;
                        controversiaActuacionActualizar.EsRequiereFiduciaria = controversiaActuacion.EsRequiereFiduciaria;
                        controversiaActuacionActualizar.EsRequiereComite = controversiaActuacion.EsRequiereComite;
                        controversiaActuacionActualizar.Observaciones = controversiaActuacion.Observaciones;
                        controversiaActuacionActualizar.EsRequiereAseguradora = controversiaActuacion.EsRequiereAseguradora;
                        controversiaActuacionActualizar.ResumenPropuestaFiduciaria = controversiaActuacion.ResumenPropuestaFiduciaria;
                        controversiaActuacionActualizar.EsRequiereComiteReclamacion = controversiaActuacion.EsRequiereComiteReclamacion;
                        controversiaActuacionActualizar.EsprocesoResultadoDefinitivo = controversiaActuacion.EsprocesoResultadoDefinitivo;
                        controversiaActuacionActualizar.RutaSoporte = controversiaActuacion.RutaSoporte;
                        controversiaActuacionActualizar.EstadoAvanceTramiteCodigo = controversiaActuacion.EstadoAvanceTramiteCodigo;
                        controversiaActuacionActualizar.EsRequiereMesaTrabajo = controversiaActuacion.EsRequiereMesaTrabajo;



                        controversiaActuacionActualizar.EsCompleto = ValidarRegistroCompletoControversiaActuacion(controversiaActuacion);
                        controversiaActuacionActualizar.UsuarioModificacion = controversiaActuacion.UsuarioModificacion;
                        _context.ControversiaActuacion.Update(controversiaActuacionActualizar);

                    }

                    _context.SaveChanges();

                    return
                        new Respuesta
                        {
                            Data = controversiaActuacion,
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContractualControversy.OperacionExitosa,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                            ConstantMessagesContractualControversy.OperacionExitosa,
                            //contratoPoliza
                            idAccionCrearcontroversiaActuacion
                            , controversiaActuacion.UsuarioModificacion
                            //"UsuarioCreacion"
                            , "EDITAR CONTROVERSIA ACTUACION"
                            //contratoPoliza.UsuarioCreacion, "REGISTRAR CONTRATO POLIZA"
                            )
                        };

                    //return _response = new Respuesta { IsSuccessful = true,
                    //    IsValidation = false, Data = cuentaBancaria,
                    //    Code = ConstantMessagesBankAccount.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContractualControversy.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContractualControversy.ErrorInterno };
            }

        }

        public async Task<Respuesta> CambiarEstadoControversiaContractual(int pControversiaContractualId, string pNuevoCodigoEstado, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Controversia_Contractual, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ControversiaContractual controversiaContractualOld;
                controversiaContractualOld = _context.ControversiaContractual.Find(pControversiaContractualId);
                controversiaContractualOld.UsuarioModificacion = pUsuarioModifica;
                controversiaContractualOld.FechaModificacion = DateTime.Now;
                controversiaContractualOld.EstadoCodigo = pNuevoCodigoEstado;

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, pUsuarioModifica, "CAMBIAR ESTADO CONTROVERSIA CONTRACTUAL")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }

        }

        public async Task<Respuesta> ActualizarRutaSoporteControversiaContractual(int pControversiaContractualId, string pRutaSoporte, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Controversia_Contractual, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ControversiaContractual controversiaContractualOld;
                controversiaContractualOld = _context.ControversiaContractual.Find(pControversiaContractualId);
                controversiaContractualOld.UsuarioModificacion = pUsuarioModifica;
                controversiaContractualOld.FechaModificacion = DateTime.Now;
                controversiaContractualOld.RutaSoporte = pRutaSoporte;
                controversiaContractualOld.EsCompleto = ValidarRegistroCompletoControversiaContractual(controversiaContractualOld);

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, pUsuarioModifica, "ACTUALIZAR RUTA SOPORTE CONTROVERSIA CONTRACTUAL")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }

        }

        public async Task<ControversiaContractual> GetControversiaContractualById(int id)
        {
            string prefijo = "";
            Contrato contrato = null;

            ControversiaContractual controversiaContractual = null;
            controversiaContractual = await _context.ControversiaContractual.FindAsync(id);

            if (controversiaContractual != null)
                contrato = await _context.Contrato.FindAsync(controversiaContractual.ContratoId);

            SesionComiteSolicitud sesionComiteSolicitud = _context.SesionComiteSolicitud
                                        .Where(r => r.SolicitudId == controversiaContractual.ControversiaContractualId && r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.ControversiasContractuales && (r.Eliminado == false || r.Eliminado == null))
                                        .FirstOrDefault();
            if (contrato != null)
            {
                if (contrato.TipoContratoCodigo == ConstanCodigoTipoContrato.Obra)
                    prefijo = ConstanPrefijoNumeroSolicitudControversia.Obra;
                else if (contrato.TipoContratoCodigo == ConstanCodigoTipoContrato.Interventoria)
                    prefijo = ConstanPrefijoNumeroSolicitudControversia.Interventoria;
            }

            if (sesionComiteSolicitud != null)
            {
                controversiaContractual.ObservacionesComiteTecnico = sesionComiteSolicitud.Observaciones;
                controversiaContractual.ObversacionesComiteFiduciario = sesionComiteSolicitud.ObservacionesFiduciario;
            }

            //controversiaContractual.NumeroSolicitudFormat = prefijo + controversiaContractual.ControversiaContractualId.ToString("000");
            return controversiaContractual;
        }

        public async Task<ControversiaActuacion> GetControversiaActuacionById(int id)
        {
            ControversiaActuacion controversiaActuacion = null;
            controversiaActuacion = _context.ControversiaActuacion.Where(x => x.ControversiaActuacionId == id).
                Include(x => x.ControversiaContractual).
                    ThenInclude(x => x.Contrato).
                Include(x => x.SeguimientoActuacionDerivada).FirstOrDefault();
            controversiaActuacion.NumeroActuacionFormat = controversiaActuacion.NumeroActuacion;

            if (!String.IsNullOrEmpty(controversiaActuacion.ActuacionAdelantadaCodigo))
            {
                if (controversiaActuacion.ControversiaContractual.TipoControversiaCodigo == ConstanCodigoTipoControversia.Terminacion_anticipada_por_incumplimiento_TAI)
                {
                    controversiaActuacion.ActuacionAdelantadaString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(controversiaActuacion.ActuacionAdelantadaCodigo, (int)EnumeratorTipoDominio.Estado_controversia_contractual_TAI);
                }
                else
                {
                    controversiaActuacion.ActuacionAdelantadaString = await _commonService.GetNombreDominioByCodigoAndTipoDominio(controversiaActuacion.ActuacionAdelantadaCodigo, (int)EnumeratorTipoDominio.Estados_Actuacion_No_TAI);
                }
            }

            var estado = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaActuacion.EstadoActuacionReclamacionCodigo, (int)EnumeratorTipoDominio.Estados_Reclamacion);
            var vTipoControversiaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaActuacion.ControversiaContractual.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipos_Controversia);
            controversiaActuacion.NumeroContrato = controversiaActuacion.ControversiaContractual.Contrato.NumeroContrato;

            controversiaActuacion.TipoControversia = vTipoControversiaCodigo == null ? "" : vTipoControversiaCodigo.Nombre;
            controversiaActuacion.SeguimientoActuacionDerivada = controversiaActuacion.SeguimientoActuacionDerivada.Where(x => !(bool)x.Eliminado).ToList();
            controversiaActuacion.EstadoActuacionReclamacionString = estado == null ? "" : estado.Nombre;
            controversiaActuacion.ProximaActuacionCodigoString = !String.IsNullOrEmpty(controversiaActuacion.ProximaActuacionCodigo) ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(controversiaActuacion.ProximaActuacionCodigo, (int)EnumeratorTipoDominio.Proxima_actuacion_requerida) : String.Empty;

            foreach (var cont in controversiaActuacion.SeguimientoActuacionDerivada)
            {
                var estadostring = await _commonService.GetDominioByNombreDominioAndTipoDominio(cont.EstadoActuacionDerivadaCodigo, (int)EnumeratorTipoDominio.Estado_Actuacion_Derivada_r_4_4_1);
                cont.EstadoActuacionString = estadostring == null ? "" : estadostring.Nombre;

            }

            controversiaActuacion.ObservacionesComites = _context.SesionComiteSolicitud.Where(x => x.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Actuaciones_Controversias_Contractuales &&
                  !(bool)x.Eliminado && x.SolicitudId == controversiaActuacion.ControversiaActuacionId).Select(y => y.Observaciones).ToList();
            controversiaActuacion.ObservacionesFiduciaria = _context.SesionComiteSolicitud.Where(x => x.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Actuaciones_Controversias_Contractuales &&
                  !(bool)x.Eliminado && x.SolicitudId == controversiaActuacion.ControversiaActuacionId).Select(y => y.ObservacionesFiduciario).ToList();

            return controversiaActuacion;
        }

        public async Task<ActuacionSeguimiento> GetActuacionSeguimientoById(int id)
        {
            //ActuacionSeguimiento actuacionSeguimiento
            ActuacionSeguimiento actuacionSeguimiento = null;
            actuacionSeguimiento = _context.ActuacionSeguimiento.Where(x => x.ActuacionSeguimientoId == id).
                Include(x => x.ControversiaActuacion).
                ThenInclude(x => x.ControversiaContractual).
                ThenInclude(x => x.Contrato).
                Include(x => x.ControversiaActuacion).
                ThenInclude(x => x.SeguimientoActuacionDerivada)
                .FirstOrDefault();
            actuacionSeguimiento.NumeroReclamacion = actuacionSeguimiento == null ? "" : actuacionSeguimiento.NumeroActuacionReclamacion;
            //actuacionSeguimiento.NumeroReclamacion = "REC " + actuacionSeguimiento.ActuacionSeguimientoId.ToString("0000");
            //    NumeroActuacionFormat = "ACT controversia " + actuacionSeguimiento.ActuacionSeguimientoId.ToString("0000"),
            var vTipoControversiaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(actuacionSeguimiento.ControversiaActuacion.ControversiaContractual.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipos_Controversia);
            actuacionSeguimiento.NumeroContrato = actuacionSeguimiento.ControversiaActuacion.ControversiaContractual.Contrato.NumeroContrato;

            actuacionSeguimiento.TipoControversia = vTipoControversiaCodigo == null ? "" : vTipoControversiaCodigo.Nombre;
            actuacionSeguimiento.ControversiaActuacion.SeguimientoActuacionDerivada = actuacionSeguimiento.ControversiaActuacion.SeguimientoActuacionDerivada.Where(x => !(bool)x.Eliminado).ToList();
            foreach (var cont in actuacionSeguimiento.ControversiaActuacion.SeguimientoActuacionDerivada)
            {
                var estadostring = await _commonService.GetDominioByNombreDominioAndTipoDominio(cont.EstadoActuacionDerivadaCodigo, (int)EnumeratorTipoDominio.Estado_Actuacion_Derivada_r_4_4_1);
                cont.EstadoActuacionString = estadostring == null ? "" : estadostring.Nombre;
            }

            return actuacionSeguimiento;
        }

        public async Task<List<ControversiaMotivo>> GetMotivosSolicitudByControversiaContractualId(int id)
        {

            return await _context.ControversiaMotivo.Where(r => r.ControversiaContractualId == id && (r.Eliminado == null || r.Eliminado == false)).ToListAsync();
        }

        //public async Task<List<Contrato>> GetListContratos(/*int id*/)
        //{
        //    return await _context.Contrato.Where(r=>r.Eliminado==false).ToList<Contrato>();
        //}

        public async Task<byte[]> GetPlantillaControversiaContractual(int pControversiaContractualID)
        {
            if (pControversiaContractualID == 0)
            {
                return Array.Empty<byte>();
            }



            int pTipoContrato = 2;

            //               --Contratacion.TipoContratacionCodigo  14 DOM, tipoidentificacion

            ////DOM 14 1   Obra            
            //pTipoContrato = 1;

            //DOM 14 2   interventoria            
            pTipoContrato = ConstanCodigoTipoContratacion.Interventoria;


            string Vlrfase2ConstruccionObra = "", VlrFase1Preconstruccion = "";


            //if (actaInicio.ContratacionId != null)
            //{
            //    VlrFase1Preconstruccion = getSumVlrContratoComponente(Convert.ToInt32(actaInicio.ContratacionId),
            //        "1"  //ConstanCodigoFaseContrato.Preconstruccion.ToString()
            //        ).ToString();

            //}

            pTipoContrato = ConstanCodigoTipoContratacion.Obra;
            /*string TipoPlantilla = ((int)ConstanCodigoPlantillas.Controversia_Contractual).ToString();

            Plantilla Plantilla = _context.Plantilla.Where(r => r.Codigo == TipoPlantilla).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();
            Plantilla.Contenido = await ReemplazarDatosPlantillaControversiaContractual(Plantilla.Contenido, pControversiaContractualID, "cdaza");
            return PDF.Convertir(Plantilla);*/

            string TipoPlantilla = ((int)ConstanCodigoPlantillas.Controversia_Contractual).ToString();

            Plantilla Plantilla = _context.Plantilla.Where(r => r.Codigo == TipoPlantilla).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();
            Plantilla.Contenido = await ReemplazarDatosPlantillaControversiaContractual(Plantilla.Contenido, pControversiaContractualID);
            //return ConvertirPDF(Plantilla);
            return PDF.Convertir(Plantilla);
            //return ConvertirPDF(plantilla);
        }

        private async Task<string> ReemplazarDatosPlantillaControversiaContractualOld(string strContenido, int pControversiaContractualID, string usuario)
        {
            string str = "";
            string valor = "";


            //Contrato contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();
            ControversiaContractual controversiaContractual = null;

            ControversiaMotivo controversiaMotivo = null;
            ControversiaActuacion controversiaActuacion = null;

            ActuacionSeguimiento actuacionSeguimiento = null;
            controversiaContractual = _context.ControversiaContractual
                   .Where(r => r.ControversiaContractualId == pControversiaContractualID).Include(x => x.Contrato).FirstOrDefault();

            Contrato contrato = controversiaContractual.Contrato;
            NovedadContractual novedadContractual = null;   //sin rel?????
            //novedadContractual = new NovedadContractual();   //sin rel?????

            //ControversiaContractual
            //    SolicitudId
            //    DefensaJudicial

            if (controversiaContractual != null)
            {
                controversiaMotivo = _context.ControversiaMotivo
                   .Where(r => r.ControversiaContractualId == controversiaContractual.ControversiaContractualId).FirstOrDefault();

                controversiaActuacion = _context.ControversiaActuacion
                    .Where(r => r.ControversiaContractualId == controversiaContractual.ControversiaContractualId).FirstOrDefault();

                //novedadContractual = _context.NovedadContractual
                //    .Where(r => r.SolicitudId == controversiaContractual.SolicitudId).FirstOrDefault();

            }

            if (controversiaActuacion != null)
            {
                actuacionSeguimiento = _context.ActuacionSeguimiento
                    .Where(r => r.ControversiaActuacionId == controversiaActuacion.ControversiaActuacionId).FirstOrDefault();
            }


            Contratacion contratacion = null;
            Contratista contratista = null;



            DisponibilidadPresupuestal disponibilidadPresupuestal = null;

            if (contrato != null)
            {
                contratacion = _context.Contratacion
                    .Where(r => r.ContratacionId == contrato.ContratacionId).FirstOrDefault();
            }

            if (contratacion != null)
            {
                contratista = _context.Contratista
                    .Where(r => r.ContratistaId == contratacion.ContratistaId).FirstOrDefault();

                disponibilidadPresupuestal = _context.DisponibilidadPresupuestal
                    .Where(r => r.ContratacionId == contratacion.ContratacionId).FirstOrDefault();
            }

            //contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId && r.Eliminado == false).FirstOrDefault();
            //contrato = _context.Contrato.Where(r => r.ContratoId == 16 && r.Eliminado == false).FirstOrDefault();

            if (contrato != null)
            {
                contratacion = _context.Contratacion.Where(r => r.ContratacionId == contrato.ContratacionId).FirstOrDefault();

                if (contratacion != null)
                {
                    contratista = _context.Contratista.Where(r => r.ContratistaId == contratacion.ContratistaId).FirstOrDefault();
                }

            }

            // 11 12 (13) (15) (16)
            //controversiaContractual = _context.ControversiaContractual.Where(r => r.ContratoId == 16 && r.Eliminado == false
            //&& r.Eliminado == false).FirstOrDefault();

            controversiaContractual = _context.ControversiaContractual.Where(r => r.ContratoId == contrato.ContratoId && r.Eliminado == false
           && r.Eliminado == false).FirstOrDefault();

            string strEstadoCodigoControversia = "sin definir";
            string strTipoControversiaCodigo = "sin definir";
            string strTipoControversia = "sin definir";

            Dominio TipoControversiaCodigo;

            TipoControversiaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaContractual.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipos_Controversia);
            if (TipoControversiaCodigo != null)
            {
                strTipoControversia = TipoControversiaCodigo.Nombre;
                strTipoControversiaCodigo = TipoControversiaCodigo.Codigo;

            }
            controversiaMotivo = _context.ControversiaMotivo.Where(r => r.ControversiaContractualId == controversiaContractual.ControversiaContractualId).FirstOrDefault();

            string controversiaMotivoSolicitudNombre = "";
            Dominio MotivoSolicitudCodigo;
            if (controversiaMotivo != null)
            {

                MotivoSolicitudCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaMotivo.MotivoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_de_controversia);
                if (MotivoSolicitudCodigo != null)
                {
                    //strTipoControversia = MotivoSolicitudCodigo.Nombre;
                    controversiaMotivoSolicitudNombre = MotivoSolicitudCodigo.Codigo;

                }

            }

            if (controversiaContractual != null)
            {
                strContenido = strContenido.Replace("_Numero_Solicitud_", controversiaContractual.NumeroSolicitud);
                strContenido = strContenido.Replace("_Fecha_Solicitud_", controversiaContractual.FechaSolicitud == null ? "" : Convert.ToDateTime(controversiaContractual.FechaSolicitud).ToString("dd/MM/yyyy"));
            }

            strContenido = strContenido.Replace("_Tipo_Controversia_", strTipoControversia);

            //strContenido = strContenido.Replace("_Numero_Contrato_",  contrato.NumeroContrato);
            strContenido = strContenido.Replace("_Numero_Contrato_", contrato != null ? contrato.NumeroContrato : null);
            strContenido = strContenido.Replace("_Nombre_Contratista_", contratista != null ? contratista.Nombre : null /*contratista.Nombre ?? ""*/);
            strContenido = strContenido.Replace("_Fecha_inicio_contrato_", "PENDIENTE");
            strContenido = strContenido.Replace("_Fecha_fin_contrato_", "PENDIENTE");

            if (contratacion != null)
                strContenido = strContenido.Replace("_Cantidad_proyectos_asociados_", contratacion.ContratacionProyecto.Count().ToString());

            string TipoPlantillaControversiaContractual = ((int)ConstanCodigoPlantillas.Controversia_Contractual).ToString();
            string ControversiaContractual = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaControversiaContractual).Select(r => r.Contenido).FirstOrDefault();

            string TipoPlantillaRegistrosAlcance = ((int)ConstanCodigoPlantillas.Registros_Tabla_Alcance).ToString();
            string RegistroAlcance = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaRegistrosAlcance).Select(r => r.Contenido).FirstOrDefault();

            List<Dominio> ListaParametricas = _context.Dominio.ToList();

            List<Localizacion> ListaLocalizaciones = _context.Localizacion.ToList();
            List<InstitucionEducativaSede> ListaInstitucionEducativaSedes = _context.InstitucionEducativaSede.ToList();

            string DetallesProyectos = "";

            string RegistrosAlcance = "";
            //Contratacion pContratacion
            if (contratacion != null)
            {
                foreach (var proyecto in contratacion.ContratacionProyecto)
                {
                    DetallesProyectos += ControversiaContractual;

                    Localizacion Municipio = ListaLocalizaciones.Where(r => r.LocalizacionId == proyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                    Localizacion Departamento = ListaLocalizaciones.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                    Localizacion Region = ListaLocalizaciones.Where(r => r.LocalizacionId == Departamento.IdPadre).FirstOrDefault();

                    InstitucionEducativaSede IntitucionEducativa = ListaInstitucionEducativaSedes.Where(r => r.InstitucionEducativaSedeId == proyecto.Proyecto.InstitucionEducativaId).FirstOrDefault();
                    InstitucionEducativaSede Sede = ListaInstitucionEducativaSedes.Where(r => r.InstitucionEducativaSedeId == proyecto.Proyecto.SedeId).FirstOrDefault();

                    foreach (var infraestructura in proyecto.Proyecto.InfraestructuraIntervenirProyecto)
                    {
                        RegistrosAlcance += RegistroAlcance;

                        RegistrosAlcance = RegistrosAlcance.Replace("[ALCANCE_ESPACIOS_A_INTERVENIR]", ListaParametricas
                            .Where(r => r.Codigo == infraestructura.InfraestructuraCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Espacios_Intervenir)
                            .FirstOrDefault().Nombre);
                        RegistrosAlcance = RegistrosAlcance.Replace("[ALCANCE_CANTIDAD]", infraestructura.Cantidad.ToString());
                    }

                    strContenido = strContenido.Replace("_Tipo_Intervencion_", ListaParametricas
                    .Where(r => r.Codigo == proyecto.Proyecto.TipoIntervencionCodigo
                    && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).FirstOrDefault().Nombre);

                    strContenido = strContenido.Replace("_Llave_MEN_", proyecto.Proyecto.LlaveMen);
                    strContenido = strContenido.Replace("_Departamento_", Departamento.Descripcion);
                    strContenido = strContenido.Replace("_Municipio_", Municipio.Descripcion);

                    strContenido = strContenido.Replace("_Institucion_Educativa_", IntitucionEducativa.Nombre);

                    strContenido = strContenido.Replace("_Codigo_DANE_IE_", IntitucionEducativa.CodigoDane);
                    //DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, /*IntitucionEducativa.CodigoDane*/);
                    strContenido = strContenido.Replace("_Sede_", Sede.Nombre);
                    //DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, Sede.Nombre);
                    strContenido = strContenido.Replace("_Codigo_DANE_SEDE_", Sede.CodigoDane);

                    strContenido = strContenido.Replace("_Tipo_Intervencion_", ListaParametricas
                    .Where(r => r.Codigo == proyecto.Proyecto.TipoIntervencionCodigo
                    && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).FirstOrDefault().Nombre);

                    strContenido = strContenido.Replace("_Llave_MEN_", proyecto.Proyecto.LlaveMen);
                    strContenido = strContenido.Replace("_Departamento_", Departamento.Descripcion);
                    strContenido = strContenido.Replace("_Municipio_", Municipio.Descripcion);
                    strContenido = strContenido.Replace("_Institucion_Educativa_", IntitucionEducativa.Nombre);
                    strContenido = strContenido.Replace("_Codigo_DANE_IE_", IntitucionEducativa.CodigoDane);
                    strContenido = strContenido.Replace("_Sede_", Sede.Nombre);
                    strContenido = strContenido.Replace("_Codigo_DANE_SEDE_", Sede.CodigoDane);

                    //Plazo de obra
                    strContenido = strContenido.Replace("_Meses_", proyecto.Proyecto.PlazoDiasObra.ToString());
                    strContenido = strContenido.Replace("_Dias_", proyecto.Proyecto.PlazoMesesObra.ToString());
                    //Plazo de Interventoría
                    strContenido = strContenido.Replace("_Meses_", proyecto.Proyecto.PlazoDiasInterventoria.ToString());
                    strContenido = strContenido.Replace("_Dias_", proyecto.Proyecto.PlazoMesesInterventoria.ToString());
                    strContenido = strContenido.Replace("_Valor_obra_", (proyecto.Proyecto.ValorObra != null) ? proyecto.Proyecto.ValorObra.ToString() : "0");
                    strContenido = strContenido.Replace("_Valor_Interventoria_", "$" + String.Format("{0:n0}", proyecto.Proyecto.ValorInterventoria));
                    strContenido = strContenido.Replace("_Valor_Total_proyecto_", "$" + String.Format("{0:n0}", proyecto.Proyecto.ValorTotal));
                    //strContenido = strContenido.Replace("", );

                }
                //DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, RegistrosAlcance);
                //DetallesProyectos = DetallesProyectos.Replace("[REGISTROS_ALCANCE]", RegistrosAlcance);

                strContenido = strContenido.Replace("[REGISTROS_ALCANCE]", RegistrosAlcance);

            }

            //    strContenido = strContenido.Replace("Espacios_intervenir_nombre", );
            //strContenido = strContenido.Replace("Espacios_intervenir_cantidad", );

            //contratoAprobar.EstadoVerificacionCodigo = ConstanCodigoEstadoVerificacionContratoObra.Con_requisitos_del_contratista_de_obra_avalados;
            strContenido = strContenido.Replace("_Estado_obra_", "PENDIENTE");
            strContenido = strContenido.Replace("_Programacion_obra_acumulada_", "PENDIENTE");  //??
            strContenido = strContenido.Replace("_Avance_físico_acumulado_ejecutado_", "PENDIENTE");
            strContenido = strContenido.Replace("Facturacion_programada_acumulada_", "PENDIENTE");
            strContenido = strContenido.Replace("_Facturacion_ejecutada_acumulada_", "PENDIENTE");

            DateTime? fechaNull = null;
            fechaNull = controversiaContractual != null ? controversiaContractual.FechaComitePreTecnico : null;

            strContenido = strContenido.Replace(">>>>>SECCION_TAI>>>>>>>>", "");
            strContenido = strContenido.Replace("_Motivos_solicitud_", controversiaMotivoSolicitudNombre);
            //strContenido = strContenido.Replace("_Fecha_Comite_Pre_Tecnico_", controversiaContractual.FechaComitePreTecnico != null ? Convert.ToDateTime(controversiaContractual.FechaComitePreTecnico).ToString("dd/MM/yyyy") : controversiaContractual.FechaComitePreTecnico.ToString());
            strContenido = strContenido.Replace("_Fecha_Comite_Pre_Tecnico_", fechaNull == null ? "" : Convert.ToDateTime(fechaNull).ToString("dd/MM/yyyy"));

            if (controversiaContractual != null)
            {
                strContenido = strContenido.Replace("_Conclusion_Comite_pretecnico_", controversiaContractual.ConclusionComitePreTecnico);
                strContenido = strContenido.Replace("_URL_soportes_solicitud_", controversiaContractual.RutaSoporte);

                strContenido = strContenido.Replace(">>>>> SECCION_OTRAS >>>>>>>>", "");
                strContenido = strContenido.Replace("_Fecha_radicado_SAC_Numero_radicado_SAC_", controversiaContractual.NumeroRadicadoSac);
                strContenido = strContenido.Replace("_Motivos_solicitud_", controversiaMotivoSolicitudNombre);
                strContenido = strContenido.Replace("_Resumen_justificacion_solicitud_", controversiaContractual.MotivoJustificacionRechazo);
                strContenido = strContenido.Replace("_URL_soportes_solicitud_", controversiaContractual.RutaSoporte);
            }

            //Historial de Modificaciones                           

            string prefijo = "";

            if (contrato != null)
            {
                if (contrato.TipoContratoCodigo == ConstanCodigoTipoContrato.Obra)
                    prefijo = ConstanPrefijoNumeroSolicitudControversia.Obra;
                else if (contrato.TipoContratoCodigo == ConstanCodigoTipoContrato.Interventoria)
                    prefijo = ConstanPrefijoNumeroSolicitudControversia.Interventoria;
            }

            if (controversiaContractual != null)
            {
                //controversiaContractual.NumeroSolicitudFormat = prefijo + controversiaContractual.ControversiaContractualId.ToString("000");

                strContenido = strContenido.Replace("Modificación 1", "");
                strContenido = strContenido.Replace("_Numero_solicitud_", controversiaContractual.NumeroSolicitud);
            }

            if (novedadContractual != null)
            {
                //strContenido = strContenido.Replace("_Tipo_Novedad_", novedadContractual.TipoNovedadCodigo);

                //strContenido = strContenido.Replace(">>>>> SECCION_SUSPENSION_PRORROGA_REINICIO >>>>>>>>", "");
                //strContenido = strContenido.Replace("_Plazo_solicitado_", Convert.ToInt32(novedadContractual.PlazoAdicionalMeses).ToString());

                //fechaNull = novedadContractual != null ? novedadContractual.FechaInicioSuspension : null;
                strContenido = strContenido.Replace("_Fecha_Inicio_", fechaNull == null ? "" : Convert.ToDateTime(fechaNull).ToString("dd/MM/yyyy"));

                //fechaNull = novedadContractual != null ? novedadContractual.FechaFinSuspension : null;
                strContenido = strContenido.Replace("_Fecha_Fin_", fechaNull == null ? "" : Convert.ToDateTime(fechaNull).ToString("dd/MM/yyyy"));
            }
            strContenido = strContenido.Replace("_Numero_Comite_Tecnico_", "PENDIENTE");
            strContenido = strContenido.Replace("_Numero_Comite_Fiduciario_", "PENDIENTE");

            if (disponibilidadPresupuestal != null)
            {
                //empuezo con fuentes
                var gestionfuentes = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado
                && x.DisponibilidadPresupuestalProyecto.DisponibilidadPresupuestalId == disponibilidadPresupuestal.DisponibilidadPresupuestalId).
                    Include(x => x.FuenteFinanciacion).
                        ThenInclude(x => x.Aportante).
                        ThenInclude(x => x.CofinanciacionDocumento).
                    Include(x => x.DisponibilidadPresupuestalProyecto).
                        ThenInclude(x => x.Proyecto).
                            ThenInclude(x => x.Sede).
                    ToList();

                foreach (var gestion in gestionfuentes)
                {
                    //el saldo actual de la fuente son todas las solicitudes a la fuentes
                    //var consignadoemnfuente = _context.ControlRecurso.Where(x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId).Sum(x => x.ValorConsignacion);
                    var consignadoemnfuente = _context.FuenteFinanciacion.Where(x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId).Sum(x => x.ValorFuente);
                    var saldofuente = _context.GestionFuenteFinanciacion.Where(
                        x => x.FuenteFinanciacionId == gestion.FuenteFinanciacionId &&
                        x.DisponibilidadPresupuestalProyectoId != gestion.DisponibilidadPresupuestalProyectoId).Sum(x => x.ValorSolicitado);
                    string fuenteNombre = _context.Dominio.Where(x => x.Codigo == gestion.FuenteFinanciacion.FuenteRecursosCodigo
                            && x.TipoDominioId == (int)EnumeratorTipoDominio.Fuentes_de_financiacion).FirstOrDefault().Nombre;
                    //(decimal)font.FuenteFinanciacion.ValorFuente,
                    // Saldo_actual_de_la_fuente = (decimal)font.FuenteFinanciacion.ValorFuente - saldofuente
                    //saldototal += (decimal)consignadoemnfuente - saldofuente;
                    string institucion = _context.InstitucionEducativaSede.Where(x => x.InstitucionEducativaSedeId == gestion.DisponibilidadPresupuestalProyecto.Proyecto.Sede.PadreId).FirstOrDefault().Nombre;
                    //var tr = plantilla_fuentes.Replace("[LLAVEMEN]", gestion.DisponibilidadPresupuestalProyecto.Proyecto.LlaveMen)
                    var tr = strContenido.Replace("[LLAVEMEN]", gestion.DisponibilidadPresupuestalProyecto.Proyecto.LlaveMen)
                        //.Replace("[INSTITUCION]", institucion)
                        //.Replace("[SEDE]", gestion.DisponibilidadPresupuestalProyecto.Proyecto.Sede.Nombre)
                        .Replace("[APORTANTE]", this.getNombreAportante(gestion.FuenteFinanciacion.Aportante))
                        .Replace("[VALOR_APORTANTE]", gestion.FuenteFinanciacion.Aportante.CofinanciacionDocumento
                        .Sum(x => x.ValorDocumento).ToString())
                        .Replace("[FUENTE]", fuenteNombre)
                        //.Replace("[SALDO_FUENTE]", saldototal.ToString())
                        .Replace("[VALOR_FUENTE]", gestion.ValorSolicitado.ToString());
                    //.Replace("[NUEVO_SALDO_FUENTE]", (saldototal - gestion.ValorSolicitado).ToString());

                    strContenido = strContenido.Replace(">>>>> SECCION_ADICION >>>>>>>>", "");
                    strContenido = strContenido.Replace("_Presupuesto_adicional_solicitado_", "PENDIENTE");
                    strContenido = strContenido.Replace("_Nombre_aportante_1_", this.getNombreAportante(gestion.FuenteFinanciacion.Aportante));
                    strContenido = strContenido.Replace("_Valor_aporte_", gestion.FuenteFinanciacion.Aportante.CofinanciacionDocumento
                        .Sum(x => x.ValorDocumento).ToString());
                    strContenido = strContenido.Replace("_Fuente_", fuenteNombre);

                    //tablafuentes += tr;
                }

                //usos                    
                var componenteAp = _context.ComponenteAportante.Where(x => x.ContratacionProyectoAportante.ContratacionProyecto.ContratacionId == disponibilidadPresupuestal.ContratacionId).
                    Include(x => x.ComponenteUso).
                    Include(x => x.ContratacionProyectoAportante).
                    ThenInclude(x => x.ContratacionProyecto).
                    ThenInclude(x => x.Proyecto).ToList();
                foreach (var compAp in componenteAp)
                {
                    List<string> uso = new List<string>();
                    List<decimal> usovalor = new List<decimal>();
                    decimal total = 0;
                    var dom = _context.Dominio.Where(x => x.Codigo == compAp.TipoComponenteCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Componentes).ToList();
                    var strFase = _context.Dominio.Where(r => r.Codigo == compAp.FaseCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Fases).FirstOrDefault();
                    foreach (var comp in compAp.ComponenteUso)
                    {
                        var usos = _context.Dominio.Where(x => x.Codigo == comp.TipoUsoCodigo && x.TipoDominioId == (int)EnumeratorTipoDominio.Usos).ToList();
                        uso.Add(usos.Count() > 0 ? usos.FirstOrDefault().Nombre : "");
                        string llavemen = compAp.ContratacionProyectoAportante.ContratacionProyecto.Proyecto.LlaveMen;
                        //var fuentestring = plantilla_uso.Replace("[LLAVEMEN2]", llavemen).

                        var fuentestring = strContenido.Replace("[LLAVEMEN2]", llavemen).
                            Replace("[FASE]", strFase.Nombre).
                            Replace("[COMPONENTE]", dom.FirstOrDefault().Nombre).
                            Replace("[USO]", usos.FirstOrDefault().Nombre).
                            Replace("[VALOR_USO]", comp.ValorUso.ToString());

                        strContenido = strContenido.Replace(">>>>> SECCION_ADICION >>>>>>>>", "");
                        strContenido = strContenido.Replace("_Fase_", strFase.Nombre);
                        strContenido = strContenido.Replace("_Componente_", dom.FirstOrDefault().Nombre);
                        strContenido = strContenido.Replace("_Uso_", usos.FirstOrDefault().Nombre);
                        strContenido = strContenido.Replace("_Valor_uso_", comp.ValorUso.ToString());

                        //tablauso += fuentestring;
                    }
                }
            }

            //strContenido = strContenido.Replace("_Nombre_aportante_n_", );
            //strContenido = strContenido.Replace("_Valor_aporte_", );
            //strContenido = strContenido.Replace("_Fuente_", );
            //strContenido = strContenido.Replace("_Fase_", );
            //strContenido = strContenido.Replace("_Componente_", );
            //strContenido = strContenido.Replace("_Valor_uso_", );

            strContenido = strContenido.Replace(">>>>> SECCION_ES_PRORROGA >>>>>>>>", "");
            strContenido = strContenido.Replace("_Numero_Comite_Tecnico_", "PENDIENTE");
            strContenido = strContenido.Replace("_Numero_Comite_Fiduciario_", "PENDIENTE");
            strContenido = strContenido.Replace("_Estado_", "PENDIENTE");

            //NovedadContractual
            //AjusteClausula
            //ClausulaModificar
            strContenido = strContenido.Replace(">>>>> SECCION_MODIFICACION_CONDICIONES_CONTRACTUALES >>>>>>>>", "");

            //if (novedadContractual != null)
            //{
            //    strContenido = strContenido.Replace("_Clausula_modificar_", novedadContractual.ClausulaModificar);
            //    strContenido = strContenido.Replace("_Ajuste_solicitado_clausula_", novedadContractual.AjusteClausula);
            //}
            strContenido = strContenido.Replace("_Numero_Comite_Tecnico_", "PENDIENTE");
            strContenido = strContenido.Replace("_Numero_Comite_Fiduciario_", "PENDIENTE");
            strContenido = strContenido.Replace("_Estado_", "PENDIENTE");

            string EstadoAvanceTramiteCodigoNombre = "";
            string ActuacionAdelantadaCodigoNombre = "";

            Dominio EstadoAvanceTramiteCodigo;
            //Dominio EstadoAvanceTramiteCodigo;
            Dominio ActuacionAdelantadaCodigo;

            if (controversiaActuacion != null)
            {
                EstadoAvanceTramiteCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaActuacion.EstadoAvanceTramiteCodigo, (int)EnumeratorTipoDominio.Estados_Avance_Tramite);
                if (EstadoAvanceTramiteCodigo != null)
                {
                    //strTipoControversia = MotivoSolicitudCodigo.Nombre;
                    EstadoAvanceTramiteCodigoNombre = EstadoAvanceTramiteCodigo.Nombre;

                }
                if (controversiaContractual.TipoControversiaCodigo == ConstanCodigoTipoControversia.Terminacion_anticipada_por_incumplimiento_TAI)
                {
                    ActuacionAdelantadaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaActuacion.ActuacionAdelantadaCodigo, (int)EnumeratorTipoDominio.Estado_controversia_contractual_TAI);
                }
                else
                {
                    ActuacionAdelantadaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaActuacion.ActuacionAdelantadaCodigo, (int)EnumeratorTipoDominio.Estados_Actuacion_No_TAI);
                }

                if (ActuacionAdelantadaCodigo != null)
                {
                    ActuacionAdelantadaCodigoNombre = ActuacionAdelantadaCodigo.Nombre;
                    //ActuacionAdelantadaCodigoNombre = ActuacionAdelantadaCodigo.Codigo;

                }

            }
            //DateTime? fechaNull = null;
            fechaNull = actuacionSeguimiento != null ? actuacionSeguimiento.FechaActuacionAdelantada : null;
            //FechaFirmaContrato = fechaNull == null ? "" : Convert.ToDateTime(fechaNull).ToString("dd/MM/yyyy");

            //Historial de Actuaciones
            strContenido = strContenido.Replace(">>>>> SECCION_LISTA_ACTUACIONES >>>>>>>>", "");
            //(LISTA ACTUACIONES....)
            strContenido = strContenido.Replace("Actuación 1", "");
            strContenido = strContenido.Replace("_Estado_avance_tramite_", EstadoAvanceTramiteCodigoNombre);
            //strContenido = strContenido.Replace("_Fecha_actuacion_adelantada_", actuacionSeguimiento?.FechaActuacionAdelantada != null ? Convert.ToDateTime(actuacionSeguimiento.FechaActuacionAdelantada).ToString("dd/MM/yyyy") : actuacionSeguimiento.FechaActuacionAdelantada.ToString());
            strContenido = strContenido.Replace("_Fecha_actuacion_adelantada_", fechaNull == null ? "" : Convert.ToDateTime(fechaNull).ToString("dd/MM/yyyy"));

            strContenido = strContenido.Replace("_Actuacion_adelantada_", ActuacionAdelantadaCodigoNombre);

            if (controversiaActuacion != null)
            {
                strContenido = strContenido.Replace("_Actuacion_adelantada_", controversiaActuacion.ActuacionAdelantadaOtro);

                Dominio ProximaActuacionCodigo;
                string strProximaActuacionNombre = "",
                strProximaActuacionCodigo = "";

                ProximaActuacionCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaActuacion.ProximaActuacionCodigo, (int)EnumeratorTipoDominio.Proxima_actuacion_requerida);
                if (ProximaActuacionCodigo != null)
                {
                    strProximaActuacionNombre = ProximaActuacionCodigo.Nombre;
                    strProximaActuacionCodigo = ProximaActuacionCodigo.Codigo;
                }

                strContenido = strContenido.Replace("_Proxima_actuacion_requerida_", strProximaActuacionNombre);
                strContenido = strContenido.Replace("_Observaciones_", controversiaActuacion.Observaciones);
                strContenido = strContenido.Replace("_URL_soporte_", controversiaActuacion.RutaSoporte);
            }

            string strEstadoReclamacionCodigo = "";
            string strEstadoReclamacion = "";
            //string strEstadoAvanceTramite = "sin definir";

            //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
            Dominio EstadoReclamacionCodigo;
            if (actuacionSeguimiento != null)
            {
                EstadoReclamacionCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(actuacionSeguimiento.EstadoReclamacionCodigo, (int)EnumeratorTipoDominio.Estado_avance_reclamacion);
                if (EstadoReclamacionCodigo != null)
                {
                    strEstadoReclamacion = EstadoReclamacionCodigo.Nombre;
                    strEstadoReclamacionCodigo = EstadoReclamacionCodigo.Codigo;

                }
            }


            //Resumen de la propuesta de reclamación ante la aseguradora:
            strContenido = strContenido.Replace("Actuación de la reclamación 1", "");
            strContenido = strContenido.Replace("_Estado_avance_reclamacion_", strEstadoReclamacion);
            //strContenido = strContenido.Replace("_Fecha_actuacion_adelantada_", actuacionSeguimiento.FechaActuacionAdelantada != null ? Convert.ToDateTime(actuacionSeguimiento.FechaActuacionAdelantada).ToString("dd/MM/yyyy") : actuacionSeguimiento.FechaActuacionAdelantada.ToString());
            strContenido = strContenido.Replace("_Fecha_actuacion_adelantada_", fechaNull == null ? "" : Convert.ToDateTime(fechaNull).ToString("dd/MM/yyyy"));

            if (actuacionSeguimiento != null)
            {
                strContenido = strContenido.Replace("_Actuacion_adelantada_", actuacionSeguimiento.ActuacionAdelantada);
                strContenido = strContenido.Replace("_Proxima_actuacion_requerida_", actuacionSeguimiento.ProximaActuacion);
                strContenido = strContenido.Replace("_Observaciones_", actuacionSeguimiento.Observaciones);
                strContenido = strContenido.Replace("_URL_soporte_", actuacionSeguimiento.RutaSoporte);
                strContenido = strContenido.Replace("_reclamacion_resultado_definitivo_cerrado_ante_aseguradora_", Convert.ToBoolean(actuacionSeguimiento.EsResultadoDefinitivo).ToString());
            }

            return strContenido;

        }

        private string getNombreAportante(CofinanciacionAportante confinanciacion)
        {
            string nombreAportante;
            if (confinanciacion.TipoAportanteId.Equals(ConstanTipoAportante.Ffie))
            {
                nombreAportante = ConstanStringTipoAportante.Ffie;
            }
            else if (confinanciacion.TipoAportanteId.Equals(ConstanTipoAportante.Tercero))
            {
                nombreAportante = confinanciacion.NombreAportanteId == null
                    ? "Error" :
                    _context.Dominio.Find(confinanciacion.NombreAportanteId).Nombre;
            }
            else
            {
                if (confinanciacion.MunicipioId == null)
                {
                    nombreAportante = confinanciacion.DepartamentoId == null
                    ? "Error" :
                    "Gobernación " + _context.Localizacion.Find(confinanciacion.DepartamentoId).Descripcion;
                }
                else
                {
                    nombreAportante = confinanciacion.MunicipioId == null
                    ? "Error" :
                    "Alcaldía " + _context.Localizacion.Find(confinanciacion.MunicipioId).Descripcion;
                }
            }
            return nombreAportante;
        }


        public byte[] ConvertirPDF(Plantilla pPlantilla)
        {
            string strEncabezado = " encabezado";

            //pendiente
            //if (!string.IsNullOrEmpty(pPlantilla.Encabezado.Contenido))
            //{
            pPlantilla.Contenido = pPlantilla.Contenido.Replace("[RUTA_ICONO]", Path.Combine(Directory.GetCurrentDirectory(), "assets", "img-FFIE.png"));
            //    pPlantilla.Encabezado.Contenido = pPlantilla.Encabezado.Contenido.Replace("[RUTA_ICONO]", Path.Combine(Directory.GetCurrentDirectory(), "assets", "img-FFIE.png"));
            //    strEncabezado = Helpers.Helpers.HtmlStringLimpio(pPlantilla.Encabezado.Contenido);
            //}

            var globalSettings = new GlobalSettings
            {
                ImageQuality = 1080,
                PageOffset = 0,
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings
                {
                    Top = pPlantilla.MargenArriba,
                    Left = pPlantilla.MargenIzquierda,
                    Right = pPlantilla.MargenDerecha,
                    Bottom = pPlantilla.MargenAbajo
                },
                DocumentTitle = DateTime.Now.ToString(),
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = pPlantilla.Contenido,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "pdf-styles.css") },
                HeaderSettings = { FontName = "Roboto", FontSize = 8, Center = strEncabezado, Line = false, Spacing = 18 },
                FooterSettings = { FontName = "Ariel", FontSize = 10, Center = "[page]" },
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };

            return _converter.Convert(pdf);
        }

        public async Task<List<Contrato>> GetListContratos()
        {
            List<Contrato> ListContratos = new List<Contrato>();

            try
            {
                ListContratos = await _context.Contrato
                    .Include(c => c.Contratacion)
                    .Where(ctr => ctr.Contratacion.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.Registrados)
                    .ToListAsync();
                return ListContratos.OrderByDescending(r => r.ContratoId).ToList();
            }
            catch (Exception ex)
            {
                return ListContratos;
            }
        }

        public async Task<Respuesta> CambiarEstadoControversiaActuacion(int pControversiaActuacionId, string pNuevoCodigoEstadoAvance, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Controversia_Actuacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ControversiaActuacion controversiaActuacionOld;
                controversiaActuacionOld = _context.ControversiaActuacion.Find(pControversiaActuacionId);
                controversiaActuacionOld.UsuarioModificacion = pUsuarioModifica;
                controversiaActuacionOld.FechaModificacion = DateTime.Now;
                controversiaActuacionOld.EstadoAvanceTramiteCodigo = pNuevoCodigoEstadoAvance;

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, pUsuarioModifica, "CAMBIAR ESTADO CONTROVERSIA ACTUACION")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }

        }

        public async Task<Respuesta> CambiarEstadoControversiaActuacion2(int pControversiaActuacionId, string pNuevoCodigoProximaActuacion, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Controversia_Actuacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ControversiaActuacion controversiaActuacionOld;
                controversiaActuacionOld = _context.ControversiaActuacion.Find(pControversiaActuacionId);
                controversiaActuacionOld.UsuarioModificacion = pUsuarioModifica;
                controversiaActuacionOld.FechaModificacion = DateTime.Now;
                controversiaActuacionOld.ProximaActuacionCodigo = pNuevoCodigoProximaActuacion;

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, pUsuarioModifica, "CAMBIAR ESTADO CONTROVERSIA ACTUACION PROXIMA ACTUACION")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }

        }

        public async Task<Respuesta> CambiarEstadoControversiaActuacion3(int pControversiaActuacionId, string pNuevoCodigoActuacionAdelantada, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Controversia_Actuacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ControversiaActuacion controversiaActuacionOld;
                controversiaActuacionOld = _context.ControversiaActuacion.Find(pControversiaActuacionId);
                controversiaActuacionOld.UsuarioModificacion = pUsuarioModifica;
                controversiaActuacionOld.FechaModificacion = DateTime.Now;
                controversiaActuacionOld.ActuacionAdelantadaCodigo = pNuevoCodigoActuacionAdelantada;

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, pUsuarioModifica, "CAMBIAR ESTADO CONTROVERSIA ACTUACION ADELANTADA")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }

        }

        public async Task<Respuesta> CambiarEstadoActuacionSeguimiento(int pActuacionSeguimientoId, string pEstadoReclamacionCodigo, string pUsuarioModifica, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Actuacion_Seguimiento, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ControversiaActuacion actuacionSeguimientoOld;

                actuacionSeguimientoOld = _context.ControversiaActuacion.Find(pActuacionSeguimientoId);
                actuacionSeguimientoOld.UsuarioModificacion = pUsuarioModifica;
                actuacionSeguimientoOld.FechaModificacion = DateTime.Now;
                //cambio solicitado por andres, si esta marcado como es Requiere comite se deja en el nuevo estado 3
                if (actuacionSeguimientoOld.EsRequiereComite != null && pEstadoReclamacionCodigo == ConstantCodigoEstadoControversiaActuacion.Finalizada)
                {
                    if ((bool)actuacionSeguimientoOld.EsRequiereComite)
                    {
                        actuacionSeguimientoOld.EstadoCodigo = ConstantCodigoEstadoControversiaActuacion.Enviado_a_comite_tecnico;
                    }
                    else
                    {
                        actuacionSeguimientoOld.EstadoCodigo = ConstantCodigoEstadoControversiaActuacion.Finalizada;
                    }
                }
                else
                {
                    actuacionSeguimientoOld.EstadoCodigo = pEstadoReclamacionCodigo;
                }
                if (string.IsNullOrEmpty(actuacionSeguimientoOld.NumeroActuacionReclamacion) && actuacionSeguimientoOld.ActuacionAdelantadaCodigo == ConstanCodigoActuacionAdelantada.RemisiondeComunicaciondedecisiondeTAIporAlianzaFiduciariaalaAseguradora && (pEstadoReclamacionCodigo == ConstantCodigoEstadoControversiaActuacion.Finalizada || pEstadoReclamacionCodigo == ConstantCodigoEstadoControversiaActuacion.Enviado_a_comite_tecnico))
                {
                    int consecutivo = _context.ControversiaActuacion
                                    .Where(r => r.ControversiaContractualId == actuacionSeguimientoOld.ControversiaContractualId && r.ActuacionAdelantadaCodigo == ConstanCodigoActuacionAdelantada.RemisiondeComunicaciondedecisiondeTAIporAlianzaFiduciariaalaAseguradora)
                                    .Count();
                    actuacionSeguimientoOld.NumeroActuacionReclamacion = "REC " + (consecutivo).ToString("000");
                }
                //actuacionSeguimientoOld.EstadoCodigo = pEstadoReclamacionCodigo;

                _context.SaveChanges();

                if (pEstadoReclamacionCodigo == ConstantCodigoEstadoControversiaActuacion.Finalizada || pEstadoReclamacionCodigo == ConstantCodigoEstadoControversiaActuacion.Enviado_a_comite_tecnico)
                {
                    await SendMailParticipation(pActuacionSeguimientoId, pDominioFront, pMailServer, pMailPort, pEnableSSL, pPassword, pSender);
                }

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, pUsuarioModifica, "CAMBIAR ESTADO ACTUACION SEGUIMIENTO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }

        }

        public async Task<Respuesta> CambiarEstadoActuacionSeguimientoActuacion(int pActuacionSeguimientoId, string pEstadoReclamacionCodigo, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Actuacion_Seguimiento, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ActuacionSeguimiento actuacionSeguimientoOld;

                actuacionSeguimientoOld = _context.ActuacionSeguimiento.Find(pActuacionSeguimientoId);
                actuacionSeguimientoOld.UsuarioModificacion = pUsuarioModifica;
                actuacionSeguimientoOld.FechaModificacion = DateTime.Now;
                actuacionSeguimientoOld.EstadoDerivadaCodigo = pEstadoReclamacionCodigo;

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, pUsuarioModifica, "CAMBIAR ESTADO ACTUACION SEGUIMIENTO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }

        }

        //seguimiento = reclamacion
        public async Task<Respuesta> CambiarEstadoActuacionReclamacion(int pActuacionSeguimientoId, string pEstadoReclamacionCodigo, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Actuacion_Seguimiento, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ControversiaActuacion actuacionSeguimientoOld;

                actuacionSeguimientoOld = _context.ControversiaActuacion.Find(pActuacionSeguimientoId);
                actuacionSeguimientoOld.UsuarioModificacion = pUsuarioModifica;
                actuacionSeguimientoOld.FechaModificacion = DateTime.Now;
                actuacionSeguimientoOld.EstadoActuacionReclamacionCodigo = pEstadoReclamacionCodigo;

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, pUsuarioModifica, "CAMBIAR ESTADO ACTUACION SEGUIMIENTO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }

        }

        public async Task<Respuesta> ActualizarRutaSoporteControversiaActuacion(int pControversiaActuacionId, string pRutaSoporte, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Controversia_Actuacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ControversiaActuacion controversiaActuacionOld;
                controversiaActuacionOld = _context.ControversiaActuacion.Find(pControversiaActuacionId);
                controversiaActuacionOld.UsuarioModificacion = pUsuarioModifica;
                controversiaActuacionOld.FechaModificacion = DateTime.Now;
                controversiaActuacionOld.RutaSoporte = pRutaSoporte;

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, pUsuarioModifica, "ACTUALIZAR RUTA SOPORTE CONTROVERSIA ACTUACION")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }

        }




        public async Task<Respuesta> EliminarControversiaActuacion(int pControversiaActuacionId, string pUsuario)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Controversia_Actuacion, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            ControversiaActuacion controversiaActuacion = null;

            try
            {
                controversiaActuacion = await _context.ControversiaActuacion.Where(d => d.ControversiaActuacionId == pControversiaActuacionId).FirstOrDefaultAsync();

                if (controversiaActuacion != null)
                {
                    strCrearEditar = "Eliminar CONTROVERSIA ACTUACION";
                    controversiaActuacion.FechaModificacion = DateTime.Now;
                    //controversiaActuacion.UsuarioCreacion = disponibilidadPresupuestal.UsuarioCreacion;
                    controversiaActuacion.UsuarioModificacion = pUsuario;
                    controversiaActuacion.Eliminado = true;
                    _context.ControversiaActuacion.Update(controversiaActuacion);

                    _context.SaveChanges();

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = controversiaActuacion,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.EliminacionExitosa, idAccion, controversiaActuacion.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = controversiaActuacion,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, controversiaActuacion.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }


        public async Task<Respuesta> EliminarControversiaContractual(int pControversiaContractualId, string pUsuario)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Controversia_Contractual, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;
            ControversiaContractual controversiaContractual = null;

            try
            {
                controversiaContractual = _context.ControversiaContractual.
                    Where(d => d.ControversiaContractualId == pControversiaContractualId)
                    .Include(c => c.ControversiaActuacion).ThenInclude(x => x.ControversiaActuacionMesa).FirstOrDefault();

                //confirmo que no tenga actuaciones

                if (controversiaContractual.ControversiaActuacion.Count() > 0)
                {
                    return respuesta = new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Data = controversiaContractual,
                        Code = ConstantMessagesContractualControversy.EliminacionNoExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.EliminacionNoExitosa, idAccion, controversiaContractual.UsuarioCreacion, strCrearEditar)
                    };
                }
                /*if (controversiaContractual.ControversiaActuacionMesa.Count() > 0)
                {
                    return respuesta = new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Data = controversiaContractual,
                        Code = ConstantMessagesContractualControversy.EliminacionNoExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.EliminacionNoExitosa, idAccion, controversiaContractual.UsuarioCreacion, strCrearEditar)

                    };
                }*/


                if (controversiaContractual != null)
                {
                    strCrearEditar = "Eliminar CONTROVERSIA CONTRACTUAL";
                    controversiaContractual.FechaModificacion = DateTime.Now;
                    //controversiaContractual.UsuarioCreacion = disponibilidadPresupuestal.UsuarioCreacion;
                    controversiaContractual.UsuarioModificacion = pUsuario;
                    controversiaContractual.Eliminado = true;

                    _context.ControversiaContractual.Update(controversiaContractual);

                    _context.SaveChanges();

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = controversiaContractual,
                    Code = ConstantMessagesContractualControversy.EliminacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.EliminacionExitosa, idAccion, controversiaContractual.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = controversiaContractual,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, controversiaContractual.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }


        private bool ValidarRegistroCompletoControversiaActuacion(ControversiaActuacion controversiaActuacion)
        {
            if (
                    string.IsNullOrEmpty(controversiaActuacion.EstadoAvanceTramiteCodigo) ||
                    controversiaActuacion.FechaActuacion == null ||
                    string.IsNullOrEmpty(controversiaActuacion.ActuacionAdelantadaCodigo) ||
                    string.IsNullOrEmpty(controversiaActuacion.ProximaActuacionCodigo) ||
                    (controversiaActuacion.ProximaActuacionCodigo == "4" && string.IsNullOrEmpty(controversiaActuacion.ProximaActuacionOtro)) ||
                    controversiaActuacion.CantDiasVencimiento == null ||
                    controversiaActuacion.FechaVencimiento == null ||
                    controversiaActuacion.EsRequiereContratista == null ||
                    controversiaActuacion.EsRequiereInterventor == null ||
                    controversiaActuacion.EsRequiereSupervisor == null ||
                    controversiaActuacion.EsRequiereFiduciaria == null ||
                    controversiaActuacion.EsRequiereComite == null ||
                    string.IsNullOrEmpty(controversiaActuacion.Observaciones) ||
                    string.IsNullOrEmpty(controversiaActuacion.RutaSoporte)
                )
            {
                return false;
            }

            return true;
        }

        private bool ValidarRegistroCompletoControversiaActuacionSeguimiento(ActuacionSeguimiento controversiaActuacion)
        {
            if ((controversiaActuacion.CantDiasVencimiento) == 0
             || (controversiaActuacion.EsResultadoDefinitivo == null)
            || (controversiaActuacion.FechaActuacionAdelantada == null)
                || (controversiaActuacion.FechaVencimiento == null)
               // || (controversiaActuacion.NumeroReclamacion == null)
               || (controversiaActuacion.Observaciones == null)
                || (controversiaActuacion.ProximaActuacion == null)
                || (controversiaActuacion.RutaSoporte == null))
            {
                return false;
            }

            return true;
        }

        private bool ValidarRegistroCompletoControversiaContractual(ControversiaContractual controversiaContractual)
        {
            bool completo = true;

            if (string.IsNullOrEmpty(controversiaContractual.TipoControversiaCodigo))
            {
                return false;
            }

            int cm = _context.ControversiaMotivo.Where(r => r.ControversiaContractualId == controversiaContractual.ControversiaContractualId && (r.Eliminado == false || r.Eliminado == null)).Count();
            if (cm <= 0)
            {
                return false;
            }

            switch (controversiaContractual.TipoControversiaCodigo)
            {
                case "1": // TAI
                    if
                    (
                        string.IsNullOrEmpty(controversiaContractual.TipoControversiaCodigo) ||
                        controversiaContractual.FechaSolicitud == null ||
                        //string.IsNullOrEmpty(controversiaContractual.Motivo) ||
                        controversiaContractual.FechaComitePreTecnico == null ||
                        string.IsNullOrEmpty(controversiaContractual.ConclusionComitePreTecnico) ||
                        controversiaContractual.EsProcede == null ||
                        string.IsNullOrEmpty(controversiaContractual.RutaSoporte) ||
                        (controversiaContractual.EsProcede == false && string.IsNullOrEmpty(controversiaContractual.MotivoJustificacionRechazo)) ||
                        (controversiaContractual.EsProcede == true && controversiaContractual.EsRequiereComite == null)
                    )
                    {
                        completo = false;
                    }
                    break;

                case "2": // TAIE contratista
                case "3": // AD contratista
                case "4": // OCC contratista 
                    if
                    (
                        string.IsNullOrEmpty(controversiaContractual.TipoControversiaCodigo) ||
                        controversiaContractual.FechaSolicitud == null ||
                        string.IsNullOrEmpty(controversiaContractual.NumeroRadicadoSac) ||
                        //string.IsNullOrEmpty(controversiaContractual.motivo) ||
                        string.IsNullOrEmpty(controversiaContractual.MotivoJustificacionRechazo) ||
                        string.IsNullOrEmpty(controversiaContractual.RutaSoporte)
                    )
                    {
                        completo = false;
                    }
                    break;

                case "5": // TAIE contratante
                case "6": // AD contratante
                case "7": // OCC contratante
                    if
                    (
                        string.IsNullOrEmpty(controversiaContractual.TipoControversiaCodigo) ||
                        controversiaContractual.FechaSolicitud == null ||
                        //string.IsNullOrEmpty(controversiaContractual.motivo) ||
                        string.IsNullOrEmpty(controversiaContractual.MotivoJustificacionRechazo) ||
                        string.IsNullOrEmpty(controversiaContractual.RutaSoporte)
                    )
                    {
                        completo = false;
                    }
                    break;
            }


            return completo;
        }

        public async Task<Respuesta> CreateEditarActuacionSeguimiento(ActuacionSeguimiento actuacionSeguimiento)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Actuacion_Seguimiento, (int)EnumeratorTipoDominio.Acciones);

            //try
            //{
            //    Respuesta respuesta = new Respuesta();
            //    string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
            //    respuesta = await _Cofinancing.EliminarCofinanciacionByCofinanciacionId(pCofinancicacionId, pUsuarioModifico);

            //    return Ok(respuesta);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.ToString());
            //}
            string strCrearEditar = string.Empty;
            try
            {
                if (actuacionSeguimiento != null)
                {
                    if (string.IsNullOrEmpty(actuacionSeguimiento.ActuacionSeguimientoId.ToString()) || actuacionSeguimiento.ActuacionSeguimientoId == 0)
                    {
                        strCrearEditar = "REGISTRAR ACTUACION SEGUIMIENTO";
                        int consecutivo = _context.ActuacionSeguimiento
                                    .Where(r => r.ControversiaActuacionId == actuacionSeguimiento.ControversiaActuacionId)
                                    .Count();
                        //Auditoria
                        //strCrearEditar = "REGISTRAR AVANCE COMPROMISOS";
                        actuacionSeguimiento.FechaCreacion = DateTime.Now;
                        //controversiaActuacion.UsuarioCreacion = compromisoSeguimiento.UsuarioCreacion;                             

                        actuacionSeguimiento.Observaciones = Helpers.Helpers.CleanStringInput(actuacionSeguimiento.Observaciones);

                        actuacionSeguimiento.RegistroCompleto = ValidarRegistroCompletoControversiaActuacionSeguimiento(actuacionSeguimiento);
                        actuacionSeguimiento.Eliminado = false;

                        actuacionSeguimiento.NumeroActuacionReclamacion = "ACT REC " + (consecutivo + 1).ToString("000");

                        _context.ActuacionSeguimiento.Add(actuacionSeguimiento);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        strCrearEditar = "EDITAR ACTUACION SEGUIMIENTO";
                        ActuacionSeguimiento actuacionSeguimientoBD = null;
                        actuacionSeguimientoBD = _context.ActuacionSeguimiento.Where(r => r.ActuacionSeguimientoId == actuacionSeguimiento.ActuacionSeguimientoId).FirstOrDefault();

                        //actuacionSeguimiento.Observaciones = Helpers.Helpers.CleanStringInput(actuacionSeguimiento.Observaciones);
                        //actuacionSeguimiento.ConclusionComitePreTecnico = Helpers.Helpers.CleanStringInput(actuacionSeguimiento.ConclusionComitePreTecnico);
                        //ControversiaContractual.FechaCreacion = DateTime.Now;
                        //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;
                        //contratoPoliza.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;

                        actuacionSeguimientoBD.EstadoReclamacionCodigo = actuacionSeguimiento.EstadoReclamacionCodigo;
                        actuacionSeguimientoBD.ActuacionAdelantada = actuacionSeguimiento.ActuacionAdelantada;
                        actuacionSeguimientoBD.ProximaActuacion = actuacionSeguimiento.ProximaActuacion;
                        actuacionSeguimientoBD.Observaciones = actuacionSeguimiento.Observaciones;
                        actuacionSeguimientoBD.RutaSoporte = actuacionSeguimiento.RutaSoporte;
                        actuacionSeguimientoBD.EsResultadoDefinitivo = actuacionSeguimiento.EsResultadoDefinitivo;
                        actuacionSeguimientoBD.CantDiasVencimiento = actuacionSeguimiento.CantDiasVencimiento;
                        actuacionSeguimientoBD.FechaActuacionAdelantada = actuacionSeguimiento.FechaActuacionAdelantada;
                        actuacionSeguimientoBD.FechaVencimiento = actuacionSeguimiento.FechaVencimiento;

                        actuacionSeguimientoBD.RegistroCompleto = ValidarRegistroCompletoControversiaActuacionSeguimiento(actuacionSeguimientoBD);
                        actuacionSeguimientoBD.FechaModificacion = DateTime.Now;
                        //_context.ContratoPoliza.Add(contratoPoliza);
                        _context.ActuacionSeguimiento.Update(actuacionSeguimientoBD);
                        await _context.SaveChangesAsync();
                    }

                    return
                        new Respuesta
                        {
                            Data = actuacionSeguimiento,
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContractualControversy.OperacionExitosa,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                            ConstantMessagesContractualControversy.OperacionExitosa,
                            //contratoPoliza
                            idAccionCrearContratoPoliza
                            , actuacionSeguimiento.UsuarioModificacion
                            //"UsuarioCreacion"
                            , "EDITAR ACTUACION SEGUIMIENTO"
                            //contratoPoliza.UsuarioCreacion, "REGISTRAR CONTRATO POLIZA"
                            )
                        };

                    //return _response = new Respuesta { IsSuccessful = true,
                    //    IsValidation = false, Data = cuentaBancaria,
                    //    Code = ConstantMessagesBankAccount.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContractualControversy.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContractualControversy.ErrorInterno };
            }

        }

        public async Task<Respuesta> CreateEditarControversiaTAI(ControversiaContractual controversiaContractual)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearContratoPoliza = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Controversia_Contractual, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            string prefijo = "";
            Contrato contrato = null;
            try
            {
                if (controversiaContractual != null)
                {
                    if (string.IsNullOrEmpty(controversiaContractual.ControversiaContractualId.ToString()) || controversiaContractual.ControversiaContractualId == 0)
                    {
                        strCrearEditar = "REGISTRAR CONTROVERSIA CONTRACTUAL";

                        //Auditoria
                        strCrearEditar = "REGISTRAR AVANCE COMPROMISOS";
                        controversiaContractual.FechaCreacion = DateTime.Now;
                        //controversiaActuacion.UsuarioCreacion = compromisoSeguimiento.UsuarioCreacion;
                        controversiaContractual.MotivoJustificacionRechazo = Helpers.Helpers.CleanStringInput(controversiaContractual.MotivoJustificacionRechazo);
                        controversiaContractual.ConclusionComitePreTecnico = Helpers.Helpers.CleanStringInput(controversiaContractual.ConclusionComitePreTecnico);
                        controversiaContractual.Eliminado = false;
                        controversiaContractual.EsCompleto = ValidarRegistroCompletoControversiaContractual(controversiaContractual);
                        controversiaContractual.EstadoCodigo = "1";
                        contrato = _context.Contrato.Where(x => x.ContratoId == controversiaContractual.ContratoId).Include(x => x.Contratacion).FirstOrDefault();

                        if (contrato != null)
                        {
                            if (contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Obra)
                                prefijo = ConstanPrefijoNumeroSolicitudControversia.Obra;
                            else if (contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Interventoria)
                                prefijo = ConstanPrefijoNumeroSolicitudControversia.Interventoria;
                        }

                        foreach (ControversiaMotivo controversia in controversiaContractual.ControversiaMotivo)
                        {
                            controversia.UsuarioCreacion = controversiaContractual.UsuarioCreacion;
                            controversia.FechaCreacion = DateTime.Now;
                        }

                        controversiaContractual.NumeroSolicitud = prefijo + controversiaContractual.ControversiaContractualId.ToString("000");
                        //controversiaContractual.Eliminado = false;
                        _context.ControversiaContractual.Add(controversiaContractual);
                        await _context.SaveChangesAsync();
                        controversiaContractual.NumeroSolicitud = prefijo + controversiaContractual.ControversiaContractualId.ToString("000");

                        await _context.SaveChangesAsync();
                        //controversiaContractual.NumeroSolicitudFormat = prefijo + controversiaContractual.ControversiaContractualId.ToString("000");

                    }
                    else
                    {
                        contrato = await _context.Contrato.FindAsync(controversiaContractual.ContratoId);

                        if (contrato != null)
                        {
                            if (contrato.TipoContratoCodigo == ConstanCodigoTipoContrato.Obra)
                                prefijo = ConstanPrefijoNumeroSolicitudControversia.Obra;
                            else if (contrato.TipoContratoCodigo == ConstanCodigoTipoContrato.Interventoria)
                                prefijo = ConstanPrefijoNumeroSolicitudControversia.Interventoria;
                        }

                        //controversiaContractual.NumeroSolicitudFormat = prefijo + controversiaContractual.ControversiaContractualId.ToString("000");
                        var controversiaeditar = _context.ControversiaContractual.Find(controversiaContractual.ControversiaContractualId);
                        strCrearEditar = "EDITAR CONTROVERSIA CONTRACTUAL";
                        controversiaeditar.MotivoJustificacionRechazo = Helpers.Helpers.CleanStringInput(controversiaContractual.MotivoJustificacionRechazo);
                        controversiaeditar.ConclusionComitePreTecnico = Helpers.Helpers.CleanStringInput(controversiaContractual.ConclusionComitePreTecnico);
                        controversiaeditar.TipoControversiaCodigo = controversiaContractual.TipoControversiaCodigo;
                        controversiaeditar.FechaSolicitud = controversiaContractual.FechaSolicitud;
                        controversiaeditar.FechaComitePreTecnico = controversiaContractual.FechaComitePreTecnico;
                        controversiaeditar.EsProcede = controversiaContractual.EsProcede;
                        controversiaeditar.CualOtroMotivo = controversiaContractual.CualOtroMotivo;
                        controversiaeditar.NumeroRadicadoSac = controversiaContractual.NumeroRadicadoSac;
                        //NumeroRadicadoSac { get; set; }
                        controversiaeditar.EsRequiereComite = controversiaContractual.EsRequiereComite;
                        //controversiaeditar.RutaSoporte = controversiaContractual.RutaSoporte;
                        controversiaeditar.EstadoCodigo = controversiaContractual.EstadoCodigo;
                        controversiaeditar.FechaModificacion = DateTime.Now;
                        controversiaeditar.UsuarioModificacion = controversiaContractual.UsuarioModificacion;
                        controversiaeditar.EsCompleto = ValidarRegistroCompletoControversiaContractual(controversiaeditar);
                        //_context.ContratoPoliza.Add(contratoPoliza);
                        _context.ControversiaContractual.Update(controversiaeditar);

                        await _context.SaveChangesAsync();
                    }

                    await _context.Set<ControversiaMotivo>().Where(r => r.ControversiaContractualId == controversiaContractual.ControversiaContractualId)
                        .UpdateAsync(r => new ControversiaMotivo()
                        {
                            Eliminado = true
                        });

                    foreach (ControversiaMotivo controversia in controversiaContractual.ControversiaMotivo)
                    {
                        controversia.ControversiaContractualId = controversiaContractual.ControversiaContractualId;
                        controversia.UsuarioCreacion = !String.IsNullOrEmpty(controversiaContractual.UsuarioCreacion) ? controversiaContractual.UsuarioCreacion : controversiaContractual.UsuarioModificacion;
                        controversia.Eliminado = false;
                        await this.InsertEditControversiaMotivo(controversia);
                    }

                    return
                        new Respuesta
                        {
                            Data = controversiaContractual,
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContractualControversy.OperacionExitosa,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                            ConstantMessagesContractualControversy.OperacionExitosa,
                            //contratoPoliza
                            idAccionCrearContratoPoliza
                            , controversiaContractual.UsuarioModificacion
                            //"UsuarioCreacion"
                            , "EDITAR CONTROVERSIA CONTRACTUAL"
                            //contratoPoliza.UsuarioCreacion, "REGISTRAR CONTRATO POLIZA"

                            )
                        };

                    //return _response = new Respuesta { IsSuccessful = true,
                    //    IsValidation = false, Data = cuentaBancaria,
                    //    Code = ConstantMessagesBankAccount.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContractualControversy.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContractualControversy.ErrorInterno };
            }

        }


        public async Task<VistaContratoContratista> GetVistaContratoContratista(int pContratoId = 0)
        {
            VistaContratoContratista vistaContratoContratista = new VistaContratoContratista();

            int IdContratistaTmp = 0;
            string NombreContratistaTmp = string.Empty;
            string NumeroContratoTmp = string.Empty;
            string PlazoFormatTmp = string.Empty;
            string FechaInicioContratoTmp = string.Empty;
            string FechaFinContratoTmp = string.Empty;

            try
            {
                //Contrato contrato = null;
                List<Contrato> ListContratos = new List<Contrato>();

                if (pContratoId == 0)
                {
                    ListContratos = await _context.Contrato.Where(r => (bool)r.Eliminado == false).Distinct()
                .ToListAsync();

                }
                else
                {
                    ListContratos = await _context.Contrato.Where(r => (bool)r.Eliminado == false && r.ContratoId == pContratoId).Distinct()
              .ToListAsync();

                }

                foreach (var contrato in ListContratos)
                {

                    //contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

                    Contratacion contratacion = null;

                    ContratacionProyecto contratacionProyecto = null;
                    if (contrato != null)
                    {
                        contratacion = _context.Contratacion.Where(r => r.ContratacionId == contrato.ContratacionId)
                                                            .Include(r => r.DisponibilidadPresupuestal)
                                                            .FirstOrDefault();

                        //FechaFinContratoTmp = contrato.FechaTerminacionFase2 != null ? Convert.ToDateTime(contrato.FechaTerminacionFase2).ToString("dd/MM/yyyy") : contrato.FechaTerminacionFase2.ToString();
                        //FechaInicioContratoTmp = contrato.FechaActaInicioFase2 != null ? Convert.ToDateTime(contrato.FechaActaInicioFase2).ToString("dd/MM/yyyy") : contrato.FechaActaInicioFase2.ToString();

                        NumeroContratoTmp = contrato.NumeroContrato;
                        //PlazoFormatTmp = Convert.ToInt32(contrato.PlazoFase2ConstruccionMeses).ToString("00") + " meses / " + Convert.ToInt32(contrato.PlazoFase2ConstruccionDias).ToString("00") + " dias ";
                        if (contratacion != null)
                        {

                            PlazoFormatTmp = contratacion.DisponibilidadPresupuestal.Count() > 0 ? Convert.ToInt32(contratacion.DisponibilidadPresupuestal.FirstOrDefault().PlazoMeses).ToString("00") + " meses / " + Convert.ToInt32(contratacion.DisponibilidadPresupuestal.FirstOrDefault().PlazoDias).ToString("00") + " dias " : "";
                            FechaInicioContratoTmp = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).ToString("dd/MM/yyyy") : contrato.FechaFirmaContrato.ToString();
                            FechaFinContratoTmp = contrato.FechaFirmaContrato != null ? Convert.ToDateTime(contrato.FechaFirmaContrato).AddMonths(contratacion.DisponibilidadPresupuestal.Count() > 0 ? (int)contratacion.DisponibilidadPresupuestal.FirstOrDefault().PlazoMeses : 0).AddDays(contratacion.DisponibilidadPresupuestal.Count() > 0 ? (double)contratacion.DisponibilidadPresupuestal.FirstOrDefault().PlazoDias : 0).ToString("dd/MM/yyyy") : contrato.FechaTerminacionFase2.ToString();

                        }


                    }

                    Dominio TipoDocumentoCodigoContratista;
                    string TipoDocumentoContratistaTmp = string.Empty;
                    string NumeroIdentificacionContratistaTmp = string.Empty;

                    string TipoIntervencionCodigoTmp = string.Empty;
                    string TipoIntervencionNombreTmp = string.Empty;

                    Proyecto proyecto = null;

                    Contratista contratista = null;
                    if (contratacion != null)
                    {
                        contratista = _context.Contratista.Where(r => r.ContratistaId == contratacion.ContratistaId).FirstOrDefault();
                        contratacionProyecto = _context.ContratacionProyecto.Where(r => r.ContratacionId == contratacion.ContratacionId).FirstOrDefault();

                        if (contratista != null)
                        {
                            IdContratistaTmp = contratista.ContratistaId;
                            NombreContratistaTmp = contratista.Nombre;

                            TipoDocumentoCodigoContratista = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratista.TipoIdentificacionCodigo, (int)EnumeratorTipoDominio.Tipo_Documento);

                            if (TipoDocumentoCodigoContratista != null)
                                TipoDocumentoContratistaTmp = TipoDocumentoCodigoContratista.Nombre;

                            NumeroIdentificacionContratistaTmp = contratista.NumeroIdentificacion;

                        }

                    }

                    if (contratacionProyecto != null)
                    {
                        proyecto = _context.Proyecto.Where(r => r.ProyectoId == contratacionProyecto.ProyectoId).FirstOrDefault();
                        if (proyecto != null)
                        {
                            TipoIntervencionCodigoTmp = proyecto.tipoIntervencionString;
                            TipoIntervencionNombreTmp = _context.Dominio.Where(x => x.Codigo == proyecto.TipoIntervencionCodigo
                                  && x.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).FirstOrDefault().Nombre;
                        }

                    }

                    vistaContratoContratista.IdContratista = IdContratistaTmp;
                    vistaContratoContratista.FechaFinContrato = FechaFinContratoTmp;
                    vistaContratoContratista.FechaInicioContrato = FechaInicioContratoTmp;
                    vistaContratoContratista.IdContratista = IdContratistaTmp;
                    vistaContratoContratista.NombreContratista = NombreContratistaTmp;
                    vistaContratoContratista.NumeroContrato = NumeroContratoTmp;
                    vistaContratoContratista.PlazoFormat = PlazoFormatTmp;
                    //valor contrato
                    vistaContratoContratista.TipoDocumentoContratista = TipoDocumentoContratistaTmp;
                    vistaContratoContratista.NumeroIdentificacion = NumeroIdentificacionContratistaTmp;
                    vistaContratoContratista.TipoIntervencion = TipoIntervencionNombreTmp;
                    vistaContratoContratista.TipoIntervencionCodigo = TipoIntervencionCodigoTmp;

                }
            }
            catch (Exception e)
            {
                vistaContratoContratista = new VistaContratoContratista
                {
                };

            }
            return vistaContratoContratista;
        }

        //Usuario Jurídica Controversias contractuales

        //estado de la actuación derivada sea "Cumplida" y el estado del registro sea completo
        public async Task<Respuesta> EnviarCorreoContratistaInterventorSupervisorFiduciaria(int pControversiaContractualId, AppSettingsService settings)
        {
            Respuesta respuesta = new Respuesta();
            string fechaFirmaContrato = "";
            string correo = "cdaza@ivolucion.com";

            ControversiaActuacion controversiaActuacion = null;

            ControversiaContractual controversiaContractual = _context.ControversiaContractual
                .Where(r => r.ControversiaContractualId == pControversiaContractualId).FirstOrDefault();

            if (controversiaContractual != null)
            {
                controversiaActuacion = _context.ControversiaActuacion
             .Where(r => r.ControversiaContractualId == controversiaContractual.ControversiaContractualId).FirstOrDefault();
            }

            SeguimientoActuacionDerivada actuacionDerivada = null;
            if (controversiaActuacion != null)
            {
                actuacionDerivada = _context.SeguimientoActuacionDerivada
               .Where(r => r.ControversiaActuacionId == controversiaActuacion.ControversiaActuacionId).FirstOrDefault();

            }
            int perfilId = 0;

            //Alerta Contratista / Interventor / Supervisor / Fiduciaria
            perfilId = (int)EnumeratorPerfil.Supervisor; //  
            //correo = getCorreos(perfilId);

            try
            {
                int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantMessagesContractualControversy.CorreoEnviado, (int)EnumeratorTipoDominio.Acciones);


                int pIdTemplate = (int)enumeratorTemplate.AlertaParticipacionJuridica4_2_1;

                //PolizaObservacion polizaObservacion;           
                correo = "cdaza@ivolucion.com";

                //Task<Respuesta> result = EnviarCorreoGestionPoliza(correo, settings.MailServer,
                //settings.MailPort, settings.Password, settings.Sender,
                //objVistaContratoGarantiaPoliza, fechaFirmaContrato, pIdTemplate, msjNotificacion);

                bool blEnvioCorreo = false;
                //Respuesta respuesta = new Respuesta();

                Template TemplateRecoveryPassword = await _commonService.GetTemplateById(pIdTemplate);

                string template = TemplateRecoveryPassword.Contenido;

                Contrato contrato;
                contrato = _context.Contrato.Where(r => r.ContratoId == controversiaContractual.ContratoId).FirstOrDefault();

                string strTipoControversiaCodigo = "";
                string strTipoControversia = "";

                Dominio TipoControversiaCodigo;


                TipoControversiaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaContractual.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipos_Controversia);
                if (TipoControversiaCodigo != null)
                {
                    strTipoControversiaCodigo = TipoControversiaCodigo.Codigo;
                    strTipoControversia = TipoControversiaCodigo.Nombre;
                }

                //template = template.Replace("_Numero_Contrato_", contrato.NumeroContrato);
                template = template.Replace("_Fecha_solicitud_controversia_", Convert.ToDateTime(controversiaContractual.FechaSolicitud).ToString("dd/MM/yyyy"));
                template = template.Replace("_Numero_solicitud_", controversiaContractual.NumeroSolicitud);
                template = template.Replace("_Tipo_controversia_", strTipoControversia);  //fomato miles .                
                DateTime? fechaNull = null;
                fechaNull = actuacionDerivada != null ? actuacionDerivada.FechaActuacionDerivada : null;
                //template = template.Replace("_Fecha_actuacion_derivada_", fechaNull == null ? "" : Convert.ToDateTime(fechaNull).ToString("dd/MM/yyyy"));
                template = template.Replace("_Proxima_actuacion_requerida_", "CAMPO PENDIENTE");

                //template = template.Replace("_Descripcion_actuacion_adelantada_", actuacionDerivada.DescripciondeActuacionAdelantada);

                List<UsuarioPerfil> lstUsuariosPerfil = new List<UsuarioPerfil>();

                lstUsuariosPerfil = _context.UsuarioPerfil.Where(r => r.Activo == true && r.PerfilId == perfilId).ToList();

                List<Usuario> lstUsuarios = new List<Usuario>();

                foreach (var item in lstUsuariosPerfil)
                {
                    lstUsuarios = _context.Usuario.Where(r => r.UsuarioId == item.UsuarioId).ToList();

                    foreach (var usuario in lstUsuarios)
                    {

                        blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuario.Email, "Gestionar controversias contractuales", template, settings.Sender, settings.Password, settings.MailServer, settings.MailPort);
                    }
                }

                if (blEnvioCorreo)
                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContractualControversy.CorreoEnviado };

                else
                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContractualControversy.ErrorEnviarCorreo };

                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_actuaciones_controversias_contractuales, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificacion_Controversia_Contractual), correo, "Gestionar controversias contractuales");
                //return respuesta;

                //blEnvioCorreo = Helpers.Helpers.EnviarCorreo(lstMails, "Gestión Poliza", template, pSentender, pPassword, pMailServer, pMailPort);

                //if (blEnvioCorreo)
                //    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.CorreoEnviado };

                //else
                //    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContratoPoliza.ErrorEnviarCorreo };

                //}
                //}
                //else
                //{
                //    respuesta = new Respuesta() { IsSuccessful = true, IsValidation = true, Code = ConstantMessagesContratoPoliza.CorreoNoExiste };

                //}
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_actuaciones_controversias_contractuales, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificacion_Controversia_Contractual), correo, "Gestionar controversias contractuales");
                return respuesta;

            }
            catch (Exception ex)
            {

                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesUsuarios.ErrorGuardarCambios };
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_actuaciones_controversias_contractuales, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificacion_Controversia_Contractual), correo, "Gestionar controversias contractuales") + ": " + ex.ToString() + ex.InnerException;
                return respuesta;
            }

        }


        public async Task<Respuesta> EnviarCorreoTecnicaJuridicaContratacion(string lstMails, string pMailServer, int pMailPort, string pPassword, string pSentender, int pContratoId, int pIdTemplate)
        {
            bool blEnvioCorreo = false;
            Respuesta respuesta = new Respuesta();

            //Si no llega Email
            //if (string.IsNullOrEmpty(pUsuario.Email))
            //{
            //    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesUsuarios.EmailObligatorio };
            //}
            try
            {
                //Usuario usuarioSolicito = _context.Usuario.Where(r => !(bool)r.Eliminado && r.Email.ToUpper().Equals(pUsuario.Email.ToUpper())).FirstOrDefault();

                //if (usuarioSolicito != null)
                //{
                //if (usuarioSolicito.Activo == false)
                //{
                //    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesUsuarios.UsuarioInactivo };
                //}
                //else
                //{
                //string newPass = Helpers.Helpers.GeneratePassword(true, true, true, true, false, 8);
                //usuarioSolicito.Contrasena = Helpers.Helpers.encryptSha1(newPass.ToString());
                //usuarioSolicito.CambiarContrasena = true;
                //usuarioSolicito.Bloqueado = false;
                //usuarioSolicito.IntentosFallidos = 0;
                //usuarioSolicito.Ip = pUsuario.Ip;

                //Guardar Usuario
                //await UpdateUser(usuarioSolicito);

                //Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.MsjSupervisorGestionPoliza);
                Template TemplateRecoveryPassword = await _commonService.GetTemplateById(pIdTemplate);

                string template = TemplateRecoveryPassword.Contenido;

                //string urlDestino = pDominio;
                //asent/img/logo  
                Contrato contrato = null;
                contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId && r.Eliminado == false).FirstOrDefault();

                ControversiaContractual controversiaContractual = null;

                //con.NumeroSolicitud;
                //con.ContratoId
                //contrato1.FechaFirmaFiduciaria
                //contrato1.Observaciones
                string strNumeroSolicitud = string.Empty;
                string strNumeroContrato = string.Empty;
                string strFechaFirmaFiduciaria = string.Empty;
                string strObservaciones = string.Empty;

                if (contrato != null)
                {
                    controversiaContractual = _context.ControversiaContractual.Where(r => r.ContratoId == contrato.ContratoId).FirstOrDefault();

                    if (controversiaContractual != null)
                        strNumeroSolicitud = controversiaContractual.NumeroSolicitud;

                    strNumeroContrato = contrato.NumeroContrato;
                    strFechaFirmaFiduciaria = contrato.FechaFirmaFiduciaria != null ? Convert.ToDateTime(contrato.FechaFirmaFiduciaria).ToString("dd/MM/yyyy") : contrato.FechaFirmaFiduciaria.ToString();
                    strObservaciones = contrato.Observaciones;

                }

                //               Número de solicitud: 
                //Número de contrato: 
                //Fecha de firma de la fiduciaria: 
                //Observaciones: Campo 

                template = template.Replace("_Numero_solicitud_", strNumeroSolicitud);
                template = template.Replace("_Numero_contrato_", strNumeroContrato);
                template = template.Replace("_Fecha_firma_fiduciaria_", strFechaFirmaFiduciaria);
                template = template.Replace("_Observaciones_", strObservaciones);

                blEnvioCorreo = Helpers.Helpers.EnviarCorreo(lstMails, "Gestionar controversias contractuales", template, pSentender, pPassword, pMailServer, pMailPort);

                if (blEnvioCorreo)
                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContractualControversy.CorreoEnviado };

                else
                    respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesContractualControversy.ErrorEnviarCorreo };

                //}
                //}
                //else
                //{
                //    respuesta = new Respuesta() { IsSuccessful = true, IsValidation = true, Code = ConstantMessagesContratoPoliza.CorreoNoExiste };

                //}
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, respuesta.Code, Convert.ToInt32(ConstantCodigoAcciones.Notificacion_Controversia_Contractual), lstMails, "Gestionar controversias contractuales");
                return respuesta;

            }
            catch (Exception ex)
            {

                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesContractualControversy.Error };
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, respuesta.Code, (int)enumeratorAccion.Notificacion_Gestion_Poliza, lstMails, "Gestionar controversias contractuales") + ": " + ex.ToString() + ex.InnerException;
                return respuesta;
            }

        }

        public async Task<List<GrillaTipoSolicitudControversiaContractual>> ListGrillaTipoSolicitudControversiaContractual(int pControversiaContractualId = 0)
        {
            List<GrillaTipoSolicitudControversiaContractual> ListControversiaContractualGrilla = new List<GrillaTipoSolicitudControversiaContractual>();
            List<ControversiaContractual> ListControversiaContractual = await _context.ControversiaContractual.Where(r => r.Eliminado == false).Distinct().ToListAsync();

            if (pControversiaContractualId != 0)
            {
                ListControversiaContractual = await _context.ControversiaContractual
                                                                .Where(r => r.ControversiaContractualId == pControversiaContractualId)
                                                                //.Include(r => r.Contrato)
                                                                .ToListAsync();

            }

            List<Dominio> listDominioTipoControversia = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipos_Controversia).ToList();
            List<Dominio> listDominioEstado = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_controversia).ToList();

            foreach (var controversia in ListControversiaContractual)
            {
                controversia.Contrato = _context.Contrato.Find(controversia.ContratoId);
                try
                {
                    string strEstadoCodigoControversia = "sin definir";
                    string strEstadoControversia = "sin definir";
                    string strTipoControversiaCodigo = "sin definir";
                    string strTipoControversia = "sin definir";

                    Dominio EstadoCodigoControversia;
                    Dominio TipoControversiaCodigo;

                    string prefijo = "";

                    if (controversia.Contrato != null)
                    {
                        //poControversiaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipo_de_controversia);
                        TipoControversiaCodigo = listDominioTipoControversia.Where(r => r.Codigo == controversia.TipoControversiaCodigo).FirstOrDefault();
                        if (TipoControversiaCodigo != null)
                        {
                            strTipoControversiaCodigo = TipoControversiaCodigo.Codigo;
                            strTipoControversia = TipoControversiaCodigo.Nombre;
                        }

                        //EstadoCodigoControversia = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.EstadoCodigo, (int)EnumeratorT//ipoDominio.Estado_controversia);
                        EstadoCodigoControversia = listDominioEstado.Where(r => r.Codigo == controversia.EstadoCodigo).FirstOrDefault();
                        if (EstadoCodigoControversia != null)
                        {
                            strEstadoControversia = EstadoCodigoControversia.Nombre;
                            strEstadoCodigoControversia = EstadoCodigoControversia.Codigo;
                        }
                    }

                    bool sePuedeCerrar = false;

                    // obtenga las actuaciones
                    List<ControversiaActuacion> listaActuaciones = _context.ControversiaActuacion
                                                                                .Where(r => r.ControversiaContractualId == controversia.ControversiaContractualId)
                                                                                .ToList();


                    // diferente a TAI
                    if (controversia.TipoControversiaCodigo != "1")
                    {
                        #region diferentes a TAI

                        //Actuaciones
                        int totalActuacionesFinalizadas = _context.ControversiaActuacion.Where(r => r.ControversiaContractualId == controversia.ControversiaContractualId && r.EstadoCodigo == "2" && (r.Eliminado == false || r.Eliminado == true)).Count();
                        int totalActuaciones = _context.ControversiaActuacion.Where(r => r.ControversiaContractualId == controversia.ControversiaContractualId && (r.Eliminado == false || r.Eliminado == true)).Count();


                        // obtengo las mesas asociadas a las actuaciones
                        List<ControversiaActuacionMesa> listaMesas = new List<ControversiaActuacionMesa>();
                        listaActuaciones.ForEach(actuacion =>
                        {
                            List<ControversiaActuacionMesa> controversiaActuacionMesa = _context.ControversiaActuacionMesa
                                                                 .Where(ca => ca.ControversiaActuacionId == actuacion.ControversiaActuacionId)?.ToList();

                            if (controversiaActuacionMesa != null)
                            {
                                listaMesas.AddRange(controversiaActuacionMesa);
                            }
                            if (controversiaActuacionMesa != null)
                            {
                                listaMesas.AddRange(controversiaActuacionMesa);
                            }

                        });

                        // cantidad de registros con marca cerrada
                        int cantidadActuacionesCerradas = listaActuaciones
                                                                        .Where(r => r.EsprocesoResultadoDefinitivo == true && r.EstadoCodigo == "2")
                                                                        .ToList()
                                                                        .Count();

                        // cantidad de registros con marca cerrada
                        int cantidadMesasCerradas = listaMesas.Where(r => r.ResultadoDefinitivo == true && r.EstadoRegistroCodigo == "2").Count();

                        int totalSeguimiento = 0;
                        int totalFinalizadaSeguimiento = 0;
                        int resultadosDefinitivoCheck = 0;

                        foreach (var mesaSeguimiento in listaMesas)
                        {
                            List<ControversiaActuacionMesaSeguimiento> controversiaActuacionMesaSeguimiento = _context.ControversiaActuacionMesaSeguimiento
                                            .Where(ca => ca.ControversiaActuacionMesaId == mesaSeguimiento.ControversiaActuacionMesaId)?.ToList();
                            totalSeguimiento = totalSeguimiento + _context.ControversiaActuacionMesaSeguimiento.Where(ca => ca.ControversiaActuacionMesaId == mesaSeguimiento.ControversiaActuacionMesaId && (ca.Eliminado == false || ca.Eliminado == null)).Count();
                            totalFinalizadaSeguimiento = totalFinalizadaSeguimiento + _context.ControversiaActuacionMesaSeguimiento.Where(ca => ca.ControversiaActuacionMesaId == mesaSeguimiento.ControversiaActuacionMesaId && (ca.Eliminado == false || ca.Eliminado == null) && ca.EstadoRegistroCodigo == "2").Count();
                            resultadosDefinitivoCheck = resultadosDefinitivoCheck + _context.ControversiaActuacionMesaSeguimiento.Where(ca => ca.ControversiaActuacionMesaId == mesaSeguimiento.ControversiaActuacionMesaId && (ca.Eliminado == false || ca.Eliminado == null) && ca.EstadoRegistroCodigo == "2" && ca.ResultadoDefinitivo == true).Count();

                        }


                        // se validad si la controversia se puede cerrar 
                        if (totalActuacionesFinalizadas >= totalActuaciones)
                        {
                            if (totalFinalizadaSeguimiento >= totalSeguimiento && resultadosDefinitivoCheck > 0)
                            {
                                sePuedeCerrar = true;
                            }
                            if (cantidadMesasCerradas > 0 || cantidadActuacionesCerradas > 0)
                            {
                                sePuedeCerrar = true;
                            }
                        }
                        #endregion diferentes a TAI

                    }
                    else
                    {
                        #region TAI

                        // obtengo las mesas asociadas a las actuaciones
                        List<ActuacionSeguimiento> listaSeguimiento = new List<ActuacionSeguimiento>();
                        listaActuaciones.ForEach(actuacion =>
                        {
                            listaSeguimiento.AddRange(_context.ActuacionSeguimiento
                                                                 .Where(ac => ac.ControversiaActuacionId == actuacion.ControversiaActuacionId)
                                                                 .ToList());
                        });

                        int cantidadSeguimientoCerradas = listaSeguimiento.Where(r => r.EsResultadoDefinitivo == true && r.EstadoDerivadaCodigo == "2").Count();

                        // se validad si la controversia se puede cerrar 
                        if (cantidadSeguimientoCerradas > 0)
                        {
                            sePuedeCerrar = true;
                        }

                        #endregion TAI

                    }



                    GrillaTipoSolicitudControversiaContractual RegistroControversiaContractual = new GrillaTipoSolicitudControversiaContractual
                    {
                        ControversiaContractualId = controversia.ControversiaContractualId,
                        NumeroSolicitud = controversia.NumeroSolicitud,
                        FechaSolicitud = controversia.FechaSolicitud != null ? Convert.ToDateTime(controversia.FechaSolicitud).ToString("dd/MM/yyyy") : controversia.FechaSolicitud.ToString(),
                        TipoControversia = strTipoControversia,
                        TipoControversiaCodigo = strTipoControversiaCodigo,
                        ContratoId = controversia.Contrato == null ? 0 : controversia.Contrato.ContratoId,
                        NumeroContrato = controversia?.Contrato?.NumeroContrato,
                        EstadoControversia = strEstadoControversia,
                        EstadoControversiaCodigo = strEstadoCodigoControversia,
                        RegistroCompletoNombre = (bool)controversia.EsCompleto ? "Completo" : "Incompleto",

                        //cu 4.4.1
                        //Actuacion = "Actuación " + actuacionSeguimiento.ActuacionSeguimientoId.ToString()
                        Actuacion = "PENDIENTE",
                        FechaActuacion = "PENDIENTE",
                        EstadoActuacion = "PENDIENTE",
                        SePuedeCerrar = sePuedeCerrar,


                    };

                    ListControversiaContractualGrilla.Add(RegistroControversiaContractual);
                }
                catch (Exception e)
                {
                    GrillaTipoSolicitudControversiaContractual RegistroControversiaContractual = new GrillaTipoSolicitudControversiaContractual
                    {
                        ControversiaContractualId = controversia.ControversiaContractualId,
                        NumeroSolicitud = controversia.NumeroSolicitud + " - " + e.InnerException.ToString(),
                        FechaSolicitud = controversia.FechaSolicitud != null ? Convert.ToDateTime(controversia.FechaSolicitud).ToString("dd/MM/yyyy") : controversia.FechaSolicitud.ToString(),
                        TipoControversia = e.ToString(),
                        TipoControversiaCodigo = "ERROR",
                        ContratoId = 0,
                        NumeroContrato = "ERROR",
                        EstadoControversia = "ERROR",
                        RegistroCompletoNombre = "ERROR",
                        EstadoControversiaCodigo = "ERROR",

                    };
                    ListControversiaContractualGrilla.Add(RegistroControversiaContractual);
                }
            }
            return ListControversiaContractualGrilla.OrderByDescending(r => r.ControversiaContractualId).ToList();

        }


        public async Task<Respuesta> InsertEditControversiaMotivo(ControversiaMotivo controversiaMotivo)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearControversiaMotivo = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Controversia_Motivo, (int)EnumeratorTipoDominio.Acciones);

            //GUARDAR
            //PolizaObservacion - FechaRevision
            //    EstadoRevisionCodigo - PolizaObservacion
            string strCrearEditar, strUsuario;
            try
            {
                if (controversiaMotivo != null)
                {
                    if (controversiaMotivo.ControversiaMotivoId == 0)
                    {
                        //Auditoria
                        strCrearEditar = "REGISTRAR CONTROVERSIA MOTIVO";
                        controversiaMotivo.FechaCreacion = DateTime.Now;
                        strUsuario = controversiaMotivo.UsuarioCreacion;
                        _context.ControversiaMotivo.Add(controversiaMotivo);
                        //await _context.SaveChangesAsync();
                        _context.SaveChanges();

                    }
                    else
                    {
                        strCrearEditar = "EDIT CONTROVERSIA MOTIVO";
                        ControversiaMotivo controversiaMotivoBD = null;
                        controversiaMotivoBD = await _context.ControversiaMotivo.Where(d => d.ControversiaMotivoId == controversiaMotivo.ControversiaMotivoId).FirstOrDefaultAsync();

                        controversiaMotivoBD.FechaModificacion = DateTime.Now;
                        strUsuario = controversiaMotivo.UsuarioCreacion;

                        controversiaMotivoBD.MotivoSolicitudCodigo = controversiaMotivo.MotivoSolicitudCodigo;
                        controversiaMotivoBD.Eliminado = controversiaMotivo.Eliminado;
                        controversiaMotivoBD.UsuarioModificacion = controversiaMotivo.UsuarioCreacion;
                        _context.ControversiaMotivo.Update(controversiaMotivoBD);

                        //_context.CuentaBancaria.Update(cuentaBancariaAntigua);
                    }
                    //contratoPoliza.FechaCreacion = DateTime.Now;
                    //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;


                    //_context.Add(contratoPoliza);

                    //contratoPoliza.ObservacionesRevisionGeneral = ValidarRegistroCompleto(cofinanciacion);


                    return
                        new Respuesta
                        {
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContractualControversy.OperacionExitosa,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                            ConstantMessagesContractualControversy.OperacionExitosa,
                            //contratoPoliza
                            idAccionCrearControversiaMotivo
                            ,
                            strUsuario, strCrearEditar
                            //contratoPoliza.UsuarioCreacion, "REGISTRAR POLIZA GARANTIA"
                            )
                        };

                    //return _response = new Respuesta { IsSuccessful = true,
                    //    IsValidation = false, Data = cuentaBancaria,
                    //    Code = ConstantMessagesBankAccount.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContractualControversy.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContractualControversy.ErrorInterno, Message = ex.InnerException.ToString().Substring(0, 500) };
            }

        }

        //public async Task<List<GrillaActuacionSeguimiento>> ListGrillaActuacionSeguimiento( int pControversiaActuacionId /*pControversiaContractualId*/ = 0)
        //{
        //    //await AprobarContratoByIdContrato(1);

        //    List<GrillaActuacionSeguimiento> LstActuacionSeguimientoGrilla = new List<GrillaActuacionSeguimiento>();
        //    //Fecha de firma del contrato ??? FechaFirmaContrato , [Contrato] -(dd / mm / aaaa)

        //    //Tipo de solicitud ??? ContratoPoliza - TipoSolicitudCodigo      

        //    //List<ControversiaContractual> ListControversiaContractualGrilla = await _context.ControversiaContractual.Where(r => !(bool)r.EstadoCodigo).Distinct().ToListAsync();
        //    List<ActuacionSeguimiento> lstActuacionSeguimiento = await _context.ActuacionSeguimiento.Distinct().ToListAsync();

        //    if (pControversiaActuacionId != 0)
        //    {
        //        lstActuacionSeguimiento = lstActuacionSeguimiento.Where(r => r.ControversiaActuacionId == pControversiaActuacionId).ToList();

        //    }

        //    foreach (var actuacionSeguimiento in lstActuacionSeguimiento)
        //    {
        //        try
        //        {

        //            //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
        //            string strEstadoReclamacionCodigo = "sin definir";
        //            string strEstadoReclamacion = "sin definir";
        //            //string strEstadoAvanceTramite = "sin definir";

        //            //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
        //            Dominio EstadoReclamacionCodigo;

        //            EstadoReclamacionCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(actuacionSeguimiento.EstadoReclamacionCodigo, (int)EnumeratorTipoDominio.Estado_avance_reclamacion);
        //            if (EstadoReclamacionCodigo != null)
        //            {
        //                strEstadoReclamacion = EstadoReclamacionCodigo.Nombre;
        //                strEstadoReclamacionCodigo = EstadoReclamacionCodigo.Codigo;

        //            }

        //            //EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
        //            //if (EstadoSolicitudCodigoContratoPoliza != null)
        //            //    strEstadoSolicitudCodigoContratoPoliza = EstadoSolicitudCodigoContratoPoliza.Nombre;

        //            //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
        //            GrillaActuacionSeguimiento RegistroActuacionSeguimiento = new GrillaActuacionSeguimiento
        //            {
        //                ActuacionSeguimientoId = actuacionSeguimiento.ActuacionSeguimientoId,
        //                NumeroActuacion = actuacionSeguimiento.ActuacionAdelantada,
        //                NumeroActuacionFormat = "ACT controversia " + actuacionSeguimiento.ActuacionSeguimientoId.ToString("0000"),
        //                EstadoReclamacion =strEstadoReclamacion,
        //                EstadoReclamacionCodigo=actuacionSeguimiento.EstadoReclamacionCodigo,
        //                FechaActualizacion = actuacionSeguimiento.FechaModificacion != null ? Convert.ToDateTime(actuacionSeguimiento.FechaModificacion).ToString("dd/MM/yyyy") : actuacionSeguimiento.FechaModificacion.ToString(),
        //                NumeroReclamacion= "REC "+actuacionSeguimiento.ActuacionSeguimientoId.ToString("0000"),
        //                Actuacion = "Actuación " +actuacionSeguimiento.ActuacionSeguimientoId.ToString(),
        //                ControversiaActuacionId = actuacionSeguimiento.ControversiaActuacionId,
        //                //RegistroCompletoActuacion = (bool)controversia.EsCompleto ? "Completo" : "Incompleto",                        

        //            };

        //            //if (!(bool)proyecto.RegistroCompleto)
        //            //{
        //            //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
        //            //}
        //            LstActuacionSeguimientoGrilla.Add(RegistroActuacionSeguimiento);
        //        }
        //        catch (Exception e)
        //        {
        //            GrillaActuacionSeguimiento RegistroActuacionSeguimiento = new GrillaActuacionSeguimiento
        //            {
        //                NumeroActuacion = "ERROR",
        //                EstadoReclamacion = e.InnerException.ToString(),
        //                FechaActualizacion = e.ToString(),
        //                NumeroReclamacion = "ERROR",
        //                Actuacion = "ERROR"        ,
        //                ActuacionSeguimientoId=0

        //            };
        //            LstActuacionSeguimientoGrilla.Add(RegistroActuacionSeguimiento);
        //        }
        //    }
        //    return LstActuacionSeguimientoGrilla.OrderByDescending(r => r.ActuacionSeguimientoId).ToList();

        //}

        public async Task<List<GrillaActuacionSeguimiento>> ListGrillaActuacionSeguimiento(int pControversiaContractualId = 0)
        {
            List<GrillaActuacionSeguimiento> LstActuacionSeguimientoGrilla = new List<GrillaActuacionSeguimiento>();
            List<ActuacionSeguimiento> lstActuacionSeguimiento = await _context.ActuacionSeguimiento.ToListAsync();


            foreach (var actuacionSeguimiento in lstActuacionSeguimiento)
            {
                try
                {

                    //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    string strEstadoReclamacionCodigo = "sin definir";
                    string strEstadoReclamacion = "sin definir";
                    //string strEstadoAvanceTramite = "sin definir";

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio EstadoReclamacionCodigo;

                    EstadoReclamacionCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(actuacionSeguimiento.EstadoReclamacionCodigo, (int)EnumeratorTipoDominio.Estados_Reclamacion);
                    if (EstadoReclamacionCodigo != null)
                    {
                        strEstadoReclamacion = EstadoReclamacionCodigo.Nombre;
                        strEstadoReclamacionCodigo = EstadoReclamacionCodigo.Codigo;

                    }

                    GrillaActuacionSeguimiento RegistroActuacionSeguimiento = null;
                    ControversiaActuacion controversiaActuacion;
                    controversiaActuacion = _context.ControversiaActuacion
                        .Where(r => r.ControversiaContractualId == pControversiaContractualId).FirstOrDefault();
                    if (controversiaActuacion != null)
                    {
                        if (controversiaActuacion.ControversiaActuacionId == actuacionSeguimiento.ControversiaActuacionId)
                        {
                            //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                            RegistroActuacionSeguimiento = new GrillaActuacionSeguimiento
                            {
                                ActuacionSeguimientoId = actuacionSeguimiento.ActuacionSeguimientoId,
                                NumeroActuacion = actuacionSeguimiento.ActuacionAdelantada,
                                NumeroActuacionFormat = "ACT controversia " + actuacionSeguimiento.ActuacionSeguimientoId.ToString("0000"),
                                EstadoReclamacion = strEstadoReclamacion,
                                EstadoReclamacionCodigo = actuacionSeguimiento.EstadoReclamacionCodigo,
                                FechaActualizacion = actuacionSeguimiento.FechaModificacion != null ? Convert.ToDateTime(actuacionSeguimiento.FechaModificacion).ToString("dd/MM/yyyy") : actuacionSeguimiento.FechaModificacion.ToString(),
                                //NumeroReclamacion = "REC " + actuacionSeguimiento.ActuacionSeguimientoId.ToString("0000"),
                                NumeroReclamacion = actuacionSeguimiento.NumeroActuacionReclamacion,
                                Actuacion = "Actuación " + actuacionSeguimiento.ActuacionSeguimientoId.ToString(),
                                ControversiaActuacionId = actuacionSeguimiento.ControversiaActuacionId,
                                //RegistroCompletoActuacion = (bool)controversia.EsCompleto ? "Completo" : "Incompleto",                        

                            };

                        }

                    }
                    else
                    {
                        //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                        RegistroActuacionSeguimiento = new GrillaActuacionSeguimiento
                        {
                            ActuacionSeguimientoId = actuacionSeguimiento.ActuacionSeguimientoId,
                            NumeroActuacion = actuacionSeguimiento.ActuacionAdelantada,
                            NumeroActuacionFormat = "ACT controversia " + actuacionSeguimiento.ActuacionSeguimientoId.ToString("0000"),
                            EstadoReclamacion = strEstadoReclamacion,
                            EstadoReclamacionCodigo = actuacionSeguimiento.EstadoReclamacionCodigo,
                            FechaActualizacion = actuacionSeguimiento.FechaModificacion != null ? Convert.ToDateTime(actuacionSeguimiento.FechaModificacion).ToString("dd/MM/yyyy") : actuacionSeguimiento.FechaModificacion.ToString(),
                            //NumeroReclamacion = "REC " + actuacionSeguimiento.ActuacionSeguimientoId.ToString("0000"),
                            NumeroReclamacion = actuacionSeguimiento.NumeroActuacionReclamacion,
                            Actuacion = "Actuación " + actuacionSeguimiento.ActuacionSeguimientoId.ToString(),
                            ControversiaActuacionId = actuacionSeguimiento.ControversiaActuacionId,
                            //RegistroCompletoActuacion = (bool)controversia.EsCompleto ? "Completo" : "Incompleto",                        

                        };
                    }

                    //if (!(bool)proyecto.RegistroCompleto)
                    //{
                    //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                    //}
                    if (RegistroActuacionSeguimiento != null)
                        LstActuacionSeguimientoGrilla.Add(RegistroActuacionSeguimiento);
                }
                catch (Exception e)
                {
                    GrillaActuacionSeguimiento RegistroActuacionSeguimiento = new GrillaActuacionSeguimiento
                    {
                        NumeroActuacion = "ERROR",
                        EstadoReclamacion = e.InnerException.ToString(),
                        FechaActualizacion = e.ToString(),
                        NumeroReclamacion = "ERROR",
                        Actuacion = "ERROR",
                        ActuacionSeguimientoId = 0

                    };
                    LstActuacionSeguimientoGrilla.Add(RegistroActuacionSeguimiento);
                }
            }
            return LstActuacionSeguimientoGrilla.OrderByDescending(r => r.ActuacionSeguimientoId).ToList();

        }

        public async Task<List<GrillaActuacionSeguimiento>> ListGrillaActuacionSeguimientoByActid(int pControversiaActId)
        {
            List<GrillaActuacionSeguimiento> LstActuacionSeguimientoGrilla = new List<GrillaActuacionSeguimiento>();
            List<ActuacionSeguimiento> lstActuacionSeguimiento = await _context.ActuacionSeguimiento.
                Where(x => x.ControversiaActuacionId == pControversiaActId && !(bool)x.Eliminado).ToListAsync();


            foreach (var actuacionSeguimiento in lstActuacionSeguimiento)
            {
                try
                {

                    //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    string strEstadoReclamacionCodigo = "sin definir";
                    string strEstadoReclamacion = "sin definir";
                    //string strEstadoAvanceTramite = "sin definir";

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio EstadoReclamacionCodigo;

                    EstadoReclamacionCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(actuacionSeguimiento.EstadoReclamacionCodigo, (int)EnumeratorTipoDominio.Estados_Reclamacion);
                    var EstadoActuacion = await _commonService.GetDominioByNombreDominioAndTipoDominio(actuacionSeguimiento.EstadoDerivadaCodigo, (int)EnumeratorTipoDominio.Estados_Actuacion_Derivada);
                    if (EstadoReclamacionCodigo != null)
                    {
                        strEstadoReclamacion = EstadoReclamacionCodigo.Nombre;
                        strEstadoReclamacionCodigo = EstadoReclamacionCodigo.Codigo;

                    }

                    GrillaActuacionSeguimiento RegistroActuacionSeguimiento = null;

                    //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                    RegistroActuacionSeguimiento = new GrillaActuacionSeguimiento
                    {
                        ActuacionSeguimientoId = actuacionSeguimiento.ActuacionSeguimientoId,
                        NumeroActuacion = actuacionSeguimiento.ActuacionAdelantada,
                        NumeroActuacionFormat = "ACT controversia " + actuacionSeguimiento.ActuacionSeguimientoId.ToString("0000"),
                        EstadoActuacion = EstadoActuacion == null ? "" : EstadoActuacion.Nombre,
                        EstadoRegistro = actuacionSeguimiento.RegistroCompleto ? "Completo" : "Incompleto",
                        EstadoActuacionCodigo = actuacionSeguimiento.EstadoDerivadaCodigo,
                        EstadoReclamacionCodigo = actuacionSeguimiento.EstadoReclamacionCodigo,
                        FechaActualizacion = actuacionSeguimiento.FechaActuacionAdelantada != null ? Convert.ToDateTime(actuacionSeguimiento.FechaActuacionAdelantada).ToString("dd/MM/yyyy") : "",
                        //NumeroReclamacion = "ACT_REC " + actuacionSeguimiento.ActuacionSeguimientoId.ToString("0000"),
                        NumeroReclamacion = actuacionSeguimiento.NumeroActuacionReclamacion,
                        Actuacion = "Actuación " + actuacionSeguimiento.ActuacionSeguimientoId.ToString(),
                        ControversiaActuacionId = actuacionSeguimiento.ControversiaActuacionId,
                        //RegistroCompletoActuacion = (bool)controversia.EsCompleto ? "Completo" : "Incompleto",                        

                        //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                    };

                    if (RegistroActuacionSeguimiento != null)
                        LstActuacionSeguimientoGrilla.Add(RegistroActuacionSeguimiento);
                }
                catch (Exception e)
                {
                    GrillaActuacionSeguimiento RegistroActuacionSeguimiento = new GrillaActuacionSeguimiento
                    {
                        NumeroActuacion = "ERROR",
                        EstadoReclamacion = e.InnerException.ToString(),
                        FechaActualizacion = e.ToString(),
                        NumeroReclamacion = "ERROR",
                        Actuacion = "ERROR",
                        ActuacionSeguimientoId = 0

                    };
                    LstActuacionSeguimientoGrilla.Add(RegistroActuacionSeguimiento);
                }
            }
            return LstActuacionSeguimientoGrilla.OrderByDescending(r => r.ActuacionSeguimientoId).ToList();

        }

        public async Task<List<GrillaControversiaActuacionEstado>> ListGrillaControversiaActuacion(int id = 0, int pControversiaContractualId = 0, bool esActuacionReclamacion = false)
        {
            //await AprobarContratoByIdContrato(1);

            List<GrillaControversiaActuacionEstado> ListControversiaContractualGrilla = new List<GrillaControversiaActuacionEstado>();
            List<ControversiaActuacion> lstControversiaActuacion = await _context.ControversiaActuacion.
                Where(r => !(bool)r.Eliminado).Include(r => r.ActuacionSeguimiento).Distinct().ToListAsync();
            ControversiaContractual controversiaContractual = _context.ControversiaContractual.Find(pControversiaContractualId);
            List<ControversiaActuacion> lstControversiaActuacionCruce = new List<ControversiaActuacion>();

            List<ActuacionSeguimiento> lstActuacionSeguimiento = await _context.ActuacionSeguimiento.
             Where(r => !(bool)r.Eliminado).Distinct().ToListAsync();

            ActuacionSeguimiento ActuacionSeguimientoAux = new ActuacionSeguimiento();
            ControversiaActuacion controversiaActuacionAux = new ControversiaActuacion();
            List<int> lstActuacionSeguimientoId = new List<int>();

            if (esActuacionReclamacion)
            {
                //lista controversias con seguimiento(reclamacion)
                foreach (ActuacionSeguimiento actuacionSeguimiento in lstActuacionSeguimiento)
                {
                    foreach (ControversiaActuacion controversiaActuacion in lstControversiaActuacion)
                    {
                        if (actuacionSeguimiento.ControversiaActuacionId == controversiaActuacion.ControversiaActuacionId)
                        {
                            if (controversiaActuacion.ControversiaActuacionId == id)
                            {
                                //controversiaActuacionAux = new ControversiaActuacion();
                                //controversiaActuacionAux = null;
                                //controversiaActuacionAux = controversiaActuacion;
                                //controversiaActuacionAux.ActuacionSeguimientoId = actuacionSeguimiento.ActuacionSeguimientoId;
                                //lstControversiaActuacionCruce.Add(controversiaActuacionAux);

                                lstActuacionSeguimientoId.Add(actuacionSeguimiento.ActuacionSeguimientoId);
                                //controversiaActuacion.ActuacionSeguimientoId = actuacionSeguimiento.ActuacionSeguimientoId;
                                lstControversiaActuacionCruce.Add(controversiaActuacion);

                            }


                        }

                    }

                }
                lstControversiaActuacion = lstControversiaActuacionCruce;
            }


            else if (id != 0)
            {
                lstControversiaActuacion = lstControversiaActuacion.Where(r => r.ControversiaActuacionId == id).ToList();

            }
            else if (pControversiaContractualId != 0)
            {
                lstControversiaActuacion = lstControversiaActuacion.Where(r => r.ControversiaContractualId == pControversiaContractualId).ToList();

            }


            foreach (var controversia in lstControversiaActuacion)
            {
                try
                {
                    Contrato contrato = null;
                    int i = 0;

                    //contrato = await _commonService.GetContratoPolizaByContratoId(controversia.ContratoId);
                    //contrato = _context.Contrato.Where(r => r.ContratoId == controversia.id).FirstOrDefault();

                    //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    string strEstadoAvanceTramiteCodigo = "sin definir";
                    string strEstadoAvanceTramite = "sin definir";

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio EstadoAvanceCodigo;

                    string strProximaActuacionCodigo = "sin definir";
                    string strProximaActuacionNombre = "sin definir";

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio ProximaActuacionCodigo;

                    ProximaActuacionCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.ProximaActuacionCodigo, (int)EnumeratorTipoDominio.Proxima_actuacion_requerida);
                    if (ProximaActuacionCodigo != null)
                    {
                        strProximaActuacionNombre = ProximaActuacionCodigo.Nombre;
                        strProximaActuacionCodigo = ProximaActuacionCodigo.Codigo;

                    }

                    EstadoAvanceCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.EstadoAvanceTramiteCodigo, (int)EnumeratorTipoDominio.Estados_Avance_Tramite);
                    if (EstadoAvanceCodigo != null)
                    {
                        strEstadoAvanceTramite = EstadoAvanceCodigo.Nombre;
                        strEstadoAvanceTramiteCodigo = EstadoAvanceCodigo.Codigo;

                    }

                    string strEstadoActuacion = string.Empty;
                    string strEstadoActuacionCodigo = string.Empty;

                    //provisionaaaal
                    if (!string.IsNullOrEmpty(controversia.EstadoCodigo) && controversia.EstadoCodigo != ConstantCodigoEstadoControversiaActuacion.Finalizada && controversia.EstadoCodigo != ConstantCodigoEstadoControversiaActuacion.En_proceso_de_registro)
                    {
                        controversia.EstadoCodigo = ConstantCodigoEstadoControversiaActuacion.Enviado_a_comite_tecnico;
                    }

                    //var EstadoActuacion = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.EstadoCodigo, (int)EnumeratorTipoDominio.Estados_Actuacion_Derivada);
                    var EstadoActuacion = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.EstadoCodigo, (int)EnumeratorTipoDominio.Estados_Actuacion);

                    if (EstadoActuacion != null)
                    {
                        strEstadoActuacion = EstadoActuacion.Nombre;
                        strEstadoActuacionCodigo = EstadoActuacion.Codigo;
                    }



                    string EstadoActuacionReclamacionCodigoTmp = "";
                    string EstadoActuacionReclamacionTmp = "";

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio EstadoActuacionReclamacion;

                    EstadoActuacionReclamacion = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.EstadoActuacionReclamacionCodigo, (int)EnumeratorTipoDominio.Estados_Reclamacion);
                    if (EstadoActuacionReclamacion != null)
                    {
                        EstadoActuacionReclamacionTmp = EstadoActuacionReclamacion.Nombre;
                        EstadoActuacionReclamacionCodigoTmp = EstadoActuacionReclamacion.Codigo;

                    }

                    int ActuacionSeguimientoIdTmp = 0;

                    if (lstActuacionSeguimientoId.Count() > 0)
                        ActuacionSeguimientoIdTmp = lstActuacionSeguimientoId[0];

                    //List<Dominio> listaDominioActuacionActualizada = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Actuacion_adelantada).ToList();

                    List<Dominio> listaDominioActuacionActualizada = new List<Dominio>();

                    if (controversiaContractual.TipoControversiaCodigo == ConstanCodigoTipoControversia.Terminacion_anticipada_por_incumplimiento_TAI)
                    {
                        listaDominioActuacionActualizada = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_controversia_contractual_TAI).ToList();
                    }
                    else
                    {
                        listaDominioActuacionActualizada = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Actuacion_No_TAI).ToList();
                    }

                    //List<Dominio> listaDominioActuacionActualizada = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Actuacion_adelantada).ToList();

                    List<Dominio> listaDominioAvanceTramiteActuacion = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Avance_Tramite).ToList();


                    GrillaControversiaActuacionEstado RegistroControversiaContractual = new GrillaControversiaActuacionEstado
                    {
                        ControversiaContractualId = controversia.ControversiaContractualId,
                        FechaActualizacion = controversia.FechaActuacion != null ? Convert.ToDateTime(controversia.FechaActuacion).ToString("dd/MM/yyyy") : "",// controversia.FechaModificacion.ToString("dd/MM/yyyy"),
                        DescripcionActuacion = listaDominioActuacionActualizada.Where(r => r.Codigo == controversia.ActuacionAdelantadaCodigo)?.FirstOrDefault()?.Nombre,
                        ActuacionId = controversia.ControversiaActuacionId,
                        EstadoActuacion = strEstadoActuacion,
                        EstadoActuacionCodigo = strEstadoActuacionCodigo,
                        NumeroActuacion = controversia.NumeroActuacion, //"ACT Controversia " + controversia.ControversiaActuacionId.ToString("000"),
                        NumeroActuacionReclamacion = "ACT_REC " + controversia.ControversiaActuacionId.ToString("0000"),
                        RegistroCompletoActuacion = (bool)controversia.EsCompleto ? "Completo" : "Incompleto",
                        ProximaActuacionCodigo = strProximaActuacionCodigo,
                        ProximaActuacionNombre = strProximaActuacionNombre,
                        EstadoActuacionReclamacionCodigo = (controversia.EstadoActuacionReclamacionCodigo != null) ? controversia.EstadoActuacionReclamacionCodigo : "",
                        EstadoActuacionReclamacion = EstadoActuacionReclamacionTmp,
                        RegistroCompletoReclamacion = controversia.EsCompletoReclamacion == null ? "Incompleto" : (bool)controversia.EsCompletoReclamacion ? "Completo" : "Incompleto",
                        ActuacionSeguimientoId = ActuacionSeguimientoIdTmp,
                        EstadoAvanceTramite = listaDominioAvanceTramiteActuacion.Where(r => r.Codigo == controversia.EstadoAvanceTramiteCodigo)?.FirstOrDefault()?.Nombre,
                        EstadoAvanceTramiteCodigo = controversia.EstadoAvanceTramiteCodigo,
                        RequiereMesaTrabajo = controversia.EsRequiereMesaTrabajo,
                        RequiereComite = controversia.EsRequiereComite,
                        ActuacionAdelantadaCodigo = controversia.ActuacionAdelantadaCodigo
                    };

                    if (lstActuacionSeguimientoId.Count() > 0)
                        lstActuacionSeguimientoId.RemoveAt(0);
                    //if (!(bool)proyecto.RegistroCompleto)
                    //{
                    //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                    //}
                    ListControversiaContractualGrilla.Add(RegistroControversiaContractual);
                }
                catch (Exception e)
                {
                    GrillaControversiaActuacionEstado RegistroControversiaContractual = new GrillaControversiaActuacionEstado
                    {
                        ControversiaContractualId = 0,
                        FechaActualizacion = "ERROR",
                        DescripcionActuacion = e.InnerException.ToString(),
                        ActuacionId = 0,

                        EstadoActuacion = e.ToString(),//controversia.EstadoAvanceTramiteCodigo
                        NumeroActuacion = "ERROR",

                        RegistroCompletoReclamacion = "ERROR",

                        ProximaActuacionCodigo = "ERROR",
                        ProximaActuacionNombre = "ERROR",

                    };
                    ListControversiaContractualGrilla.Add(RegistroControversiaContractual);
                }
            }
            return ListControversiaContractualGrilla.OrderByDescending(r => r.ActuacionId).ToList();

        }

        public async Task<List<GrillaTipoSolicitudControversiaContractual>> GetListGrillaControversiaActuaciones()
        {
            List<GrillaTipoSolicitudControversiaContractual> ListControversiaContractualGrilla = new List<GrillaTipoSolicitudControversiaContractual>();
            List<ControversiaContractual> ListControversiaContractual =
                await _context.ControversiaContractual.Where(r => r.Eliminado == false).Include(x => x.ControversiaActuacion).Distinct().ToListAsync();

            foreach (var controversia in ListControversiaContractual)
            {
                foreach (var controversiaActuacion in controversia.ControversiaActuacion)
                {
                    //if (controversiaActuacion.EsRequiereFiduciaria != null && (bool)controversiaActuacion.EsRequiereFiduciaria)
                    if (controversiaActuacion.EsRequiereFiduciaria != null && (bool)controversiaActuacion.EsRequiereFiduciaria && (controversiaActuacion.EstadoCodigo == ConstantCodigoEstadoControversiaActuacion.Finalizada || controversiaActuacion.EstadoCodigo == ConstantCodigoEstadoControversiaActuacion.Enviado_a_comite_tecnico))
                    {
                        try
                        {
                            Contrato contrato = null;

                            //contrato = await _commonService.GetContratoPolizaByContratoId(controversia.ContratoId);
                            contrato = _context.Contrato.Where(r => r.ContratoId == controversia.ContratoId).FirstOrDefault();

                            //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                            string strEstadoCodigoControversia = "sin definir";
                            string strEstadoControversia = "sin definir";
                            string strTipoControversiaCodigo = "sin definir";
                            string strTipoControversia = "";

                            //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                            Dominio EstadoCodigoControversia;
                            Dominio TipoControversiaCodigo;

                            string prefijo = "";

                            if (contrato != null)
                            {
                                TipoControversiaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipos_Controversia);
                                if (TipoControversiaCodigo != null)
                                {
                                    strTipoControversiaCodigo = TipoControversiaCodigo.Codigo;
                                    strTipoControversia = TipoControversiaCodigo.Nombre;

                                }

                                EstadoCodigoControversia = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.EstadoCodigo, (int)EnumeratorTipoDominio.Estado_controversia);
                                if (EstadoCodigoControversia != null)
                                {
                                    strEstadoControversia = EstadoCodigoControversia.Nombre;
                                    strEstadoCodigoControversia = EstadoCodigoControversia.Codigo;
                                }


                                //EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                                //if (EstadoSolicitudCodigoContratoPoliza != null)
                                //    strEstadoSolicitudCodigoContratoPoliza = EstadoSolicitudCodigoContratoPoliza.Nombre;

                            }
                            Dominio dmActuacion = new Dominio();

                            if (controversia.TipoControversiaCodigo == ConstanCodigoTipoControversia.Terminacion_anticipada_por_incumplimiento_TAI)
                            {
                                dmActuacion = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaActuacion.ActuacionAdelantadaCodigo, (int)EnumeratorTipoDominio.Estado_controversia_contractual_TAI);
                            }
                            else
                            {
                                dmActuacion = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaActuacion.ActuacionAdelantadaCodigo, (int)EnumeratorTipoDominio.Estados_Actuacion_No_TAI);
                            }

                            var dmActuacionEstado = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaActuacion.EstadoCodigoActuacionDerivada, (int)EnumeratorTipoDominio.Estados_Actuacion_Derivada);
                            var dmReclamacionEstado = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaActuacion.EstadoActuacionReclamacionCodigo, (int)EnumeratorTipoDominio.Estados_Reclamacion);

                            string actuacion = dmActuacion == null ? "" : dmActuacion.Nombre;
                            string estado = dmActuacionEstado == null ? "Sin registro" : dmActuacionEstado.Nombre;

                            string estadoReclamacion = dmReclamacionEstado == null ? "" : dmReclamacionEstado.Nombre;
                            //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                            GrillaTipoSolicitudControversiaContractual RegistroControversiaContractual = new GrillaTipoSolicitudControversiaContractual
                            {
                                ControversiaContractualId = controversia.ControversiaContractualId,
                                //NumeroSolicitud=controversia.NumeroSolicitud,
                                //NumeroSolicitud = string.Format("0000"+ controversia.ControversiaContractualId.ToString()),
                                NumeroSolicitud = controversia.NumeroSolicitud,
                                //FechaSolicitud=controversia.FechaSolicitud,
                                FechaSolicitud = controversia.FechaSolicitud != null ? Convert.ToDateTime(controversia.FechaSolicitud).ToString("dd/MM/yyyy") : controversia.FechaSolicitud.ToString(),
                                TipoControversia = strTipoControversia,
                                TipoControversiaCodigo = strTipoControversiaCodigo,
                                ContratoId = contrato.ContratoId,
                                NumeroContrato = contrato.NumeroContrato,
                                EstadoControversia = strEstadoControversia,
                                EstadoControversiaCodigo = strEstadoCodigoControversia,
                                RegistroCompletoNombre = (bool)controversia.EsCompleto ? "Completo" : "Incompleto",
                                RegistroCompletoActuacionDerivada = controversiaActuacion.RegistroCompletoActuacionDerivada == null ? false : (bool)controversiaActuacion.RegistroCompletoActuacionDerivada,
                                //cu 4.4.1
                                //Actuacion = "Actuación " + actuacionSeguimiento.ActuacionSeguimientoId.ToString()

                                Actuacion = actuacion,
                                FechaActuacion = controversiaActuacion.FechaActuacion != null ? Convert.ToDateTime(controversiaActuacion.FechaActuacion).ToString("dd/MM/yyyy") : "",
                                EstadoActuacion = estado,
                                ActuacionID = controversiaActuacion.ControversiaActuacionId,
                                EstadoReclamacion = estadoReclamacion,


                            };

                            //if (!(bool)proyecto.RegistroCompleto)
                            //{
                            //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                            //}
                            ListControversiaContractualGrilla.Add(RegistroControversiaContractual);
                        }
                        catch (Exception e)
                        {
                            GrillaTipoSolicitudControversiaContractual RegistroControversiaContractual = new GrillaTipoSolicitudControversiaContractual
                            {
                                ControversiaContractualId = controversia.ControversiaContractualId,
                                NumeroSolicitud = controversia.NumeroSolicitud + " - " + e.InnerException.ToString(),
                                //FechaSolicitud=controversia.FechaSolicitud,
                                FechaSolicitud = controversia.FechaSolicitud != null ? Convert.ToDateTime(controversia.FechaSolicitud).ToString("dd/MM/yyyy") : controversia.FechaSolicitud.ToString(),
                                TipoControversia = e.ToString(),
                                TipoControversiaCodigo = "ERROR",
                                ContratoId = 0,
                                NumeroContrato = "ERROR",
                                EstadoControversia = "ERROR",
                                RegistroCompletoNombre = "ERROR",
                                EstadoControversiaCodigo = "ERROR",

                            };
                            ListControversiaContractualGrilla.Add(RegistroControversiaContractual);
                        }
                    }

                }
            }
            return ListControversiaContractualGrilla.OrderByDescending(r => r.ControversiaContractualId).ToList();

        }

        public async Task<Respuesta> FinalizarActuacion(int pControversiaActuacionId, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Actuacion_Seguimiento, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ControversiaActuacion actuacionSeguimientoOld;

                actuacionSeguimientoOld = _context.ControversiaActuacion.Find(pControversiaActuacionId);
                actuacionSeguimientoOld.UsuarioModificacion = pUsuarioModifica;
                actuacionSeguimientoOld.FechaModificacion = DateTime.Now;
                actuacionSeguimientoOld.EstadoCodigo = "2";//cambiar
                actuacionSeguimientoOld.EstadoCodigoActuacionDerivada = ConstantCodigoEstadoActuacionDerivada.Finalizada;

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, pUsuarioModifica, "FINALIZAR ESTADO ACTUACION SEGUIMIENTO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }
        }

        public async Task<Respuesta> CreateEditarSeguimientoDerivado(SeguimientoActuacionDerivada actuacionSeguimiento)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_seguimiento_actuacion_derivada, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                bool escompleto = true;
                if (string.IsNullOrEmpty(actuacionSeguimiento.DescripciondeActuacionAdelantada) ||
                    actuacionSeguimiento.FechaActuacionDerivada == null ||
                    string.IsNullOrEmpty(actuacionSeguimiento.EstadoActuacionDerivadaCodigo) ||
                    string.IsNullOrEmpty(actuacionSeguimiento.Observaciones) ||
                    string.IsNullOrEmpty(actuacionSeguimiento.RutaSoporte))
                {
                    escompleto = false;
                }

                actuacionSeguimiento.EsCompleto = escompleto;
                if (actuacionSeguimiento.SeguimientoActuacionDerivadaId > 0)
                {
                    var actuacionold = _context.SeguimientoActuacionDerivada.Find(actuacionSeguimiento.SeguimientoActuacionDerivadaId);
                    actuacionold.FechaModificacion = DateTime.Now;
                    actuacionold.UsuarioModificacion = actuacionSeguimiento.UsuarioModificacion;
                    actuacionold.Observaciones = actuacionSeguimiento.Observaciones;
                    actuacionold.DescripciondeActuacionAdelantada = actuacionSeguimiento.DescripciondeActuacionAdelantada;
                    actuacionold.FechaActuacionDerivada = actuacionSeguimiento.FechaActuacionDerivada;
                    actuacionold.EstadoActuacionDerivadaCodigo = actuacionSeguimiento.EstadoActuacionDerivadaCodigo;
                    actuacionold.RutaSoporte = actuacionSeguimiento.RutaSoporte;
                    actuacionold.EsCompleto = actuacionSeguimiento.EsCompleto;
                    _context.SeguimientoActuacionDerivada.Update(actuacionold);
                }
                else
                {
                    int consecutivo = _context.SeguimientoActuacionDerivada
                                     .Where(r => r.ControversiaActuacionId == actuacionSeguimiento.ControversiaActuacionId)
                                     .Count();
                    actuacionSeguimiento.NumeroActuacionDerivada = "Act_derivada " + (consecutivo + 1).ToString("000");
                    actuacionSeguimiento.FechaCreacion = DateTime.Now;
                    _context.SeguimientoActuacionDerivada.Add(actuacionSeguimiento);
                }
                //al papa le cambio el estado
                var controversia = _context.ControversiaActuacion.Find(actuacionSeguimiento.ControversiaActuacionId);
                //controversia.EstadoActuacionReclamacionCodigo = "2";//en proceso
                controversia.EstadoCodigoActuacionDerivada = ConstantCodigoEstadoActuacionDerivada.En_proceso_de_registro;
                controversia.FechaModificacion = DateTime.Now;
                controversia.UsuarioModificacion = actuacionSeguimiento.UsuarioCreacion == null ? actuacionSeguimiento.UsuarioModificacion : actuacionSeguimiento.UsuarioCreacion;
                _context.ControversiaActuacion.Update(controversia);
                _context.SaveChanges();

                validarRegistroCompletoActuacionDerivada(actuacionSeguimiento.ControversiaActuacionId, actuacionSeguimiento.UsuarioCreacion);
                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, actuacionSeguimiento.UsuarioCreacion, "CREAR EDITAR ACTUACION DERIVADA")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, actuacionSeguimiento.UsuarioCreacion, ex.InnerException.ToString())
                };
            }
        }


        public async Task<Respuesta> FinalizarActuacionDerivada(int pControversiaActuacionId, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Actuacion_Seguimiento, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SeguimientoActuacionDerivada actuacionSeguimientoOld;


                actuacionSeguimientoOld = _context.SeguimientoActuacionDerivada.Find(pControversiaActuacionId);

                if (actuacionSeguimientoOld.EstadoActuacionDerivadaCodigo == ConstantCodigoActuacionSeguimientoDerivada.Cumplida)
                {
                    await SendMailActuacionDerivada(actuacionSeguimientoOld.SeguimientoActuacionDerivadaId);
                }

                actuacionSeguimientoOld.UsuarioModificacion = pUsuarioModifica;
                actuacionSeguimientoOld.FechaModificacion = DateTime.Now;
                actuacionSeguimientoOld.EstadoActuacionDerivadaCodigo = "3";//cambiar
                _context.SeguimientoActuacionDerivada.Update(actuacionSeguimientoOld);
                _context.SaveChanges();

                //LCT
                validarRegistroCompletoActuacionDerivada(actuacionSeguimientoOld.ControversiaActuacionId, pUsuarioModifica);

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, pUsuarioModifica, "ELIMINAR ACTUACION SEGUIMIENTO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }
        }

        private bool validarRegistroCompletoActuacionDerivada(int pControversiaActuacionId, string user)
        {
            bool state = false;
            int totalActuacionesDerivadas = _context.SeguimientoActuacionDerivada.Where(r => r.ControversiaActuacionId == pControversiaActuacionId && (r.Eliminado == null || r.Eliminado == false)).Count();
            int totalFinalizadas = _context.SeguimientoActuacionDerivada.Where(r => r.ControversiaActuacionId == pControversiaActuacionId && (r.Eliminado == null || r.Eliminado == false) && r.EstadoActuacionDerivadaCodigo == ConstantCodigoActuacionSeguimientoDerivada.Finalizada).Count();
            if (totalActuacionesDerivadas == totalFinalizadas)
            {
                state = true;
            }
            else
            {
                state = false;
            }
            _context.Set<ControversiaActuacion>()
               .Where(r => r.ControversiaActuacionId == pControversiaActuacionId)
                                                 .Update(r => new ControversiaActuacion()
                                                 {
                                                     FechaModificacion = DateTime.Now,
                                                     UsuarioModificacion = user,
                                                     RegistroCompletoActuacionDerivada = state,
                                                 });
            _context.SaveChanges();
            return false;
        }
        public async Task<Respuesta> EliminacionActuacionDerivada(int pControversiaActuacionId, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Actuacion_Seguimiento, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                SeguimientoActuacionDerivada actuacionSeguimientoOld;

                actuacionSeguimientoOld = _context.SeguimientoActuacionDerivada.Find(pControversiaActuacionId);
                actuacionSeguimientoOld.UsuarioModificacion = pUsuarioModifica;
                actuacionSeguimientoOld.FechaModificacion = DateTime.Now;
                actuacionSeguimientoOld.Eliminado = true;
                _context.SeguimientoActuacionDerivada.Update(actuacionSeguimientoOld);
                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, pUsuarioModifica, "FINALIZAR ESTADO ACTUACION SEGUIMIENTO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }
        }

        public async Task<ActionResult<List<GrillaControversiaActuacionEstado>>> GetListGrillaControversiaReclamacion(int id)
        {
            List<GrillaControversiaActuacionEstado> ListControversiaContractualGrilla = new List<GrillaControversiaActuacionEstado>();
            List<ControversiaActuacion> lstControversiaActuacion = await _context.ControversiaActuacion.
                Where(r => !(bool)r.Eliminado && r.ControversiaContractualId == id).Include(r => r.ActuacionSeguimiento).Distinct().ToListAsync();

            ControversiaContractual controversiaContractual = _context.ControversiaContractual.Find(id);

            foreach (var controversia in lstControversiaActuacion)
            {
                try
                {
                    Contrato contrato = null;
                    int i = 0;

                    //contrato = await _commonService.GetContratoPolizaByContratoId(controversia.ContratoId);
                    //contrato = _context.Contrato.Where(r => r.ContratoId == controversia.id).FirstOrDefault();

                    //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    string strEstadoAvanceTramiteCodigo = "sin definir";
                    string strEstadoAvanceTramite = "sin definir";

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio EstadoAvanceCodigo;

                    string strProximaActuacionCodigo = "sin definir";
                    string strProximaActuacionNombre = "sin definir";

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio ProximaActuacionCodigo;

                    ProximaActuacionCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.ProximaActuacionCodigo, (int)EnumeratorTipoDominio.Proxima_actuacion_requerida);
                    if (ProximaActuacionCodigo != null)
                    {
                        strProximaActuacionNombre = ProximaActuacionCodigo.Nombre;
                        strProximaActuacionCodigo = ProximaActuacionCodigo.Codigo;

                    }

                    EstadoAvanceCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.EstadoAvanceTramiteCodigo, (int)EnumeratorTipoDominio.Estados_Avance_Tramite);
                    if (EstadoAvanceCodigo != null)
                    {
                        strEstadoAvanceTramite = EstadoAvanceCodigo.Nombre;
                        strEstadoAvanceTramiteCodigo = EstadoAvanceCodigo.Codigo;

                    }


                    string EstadoActuacionReclamacionCodigoTmp = "";
                    string EstadoActuacionReclamacionTmp = "";

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio EstadoActuacionReclamacion;

                    EstadoActuacionReclamacion = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.EstadoActuacionReclamacionCodigo, (int)EnumeratorTipoDominio.Estados_Reclamacion);
                    if (EstadoActuacionReclamacion != null)
                    {
                        EstadoActuacionReclamacionTmp = EstadoActuacionReclamacion.Nombre;
                        EstadoActuacionReclamacionCodigoTmp = EstadoActuacionReclamacion.Codigo;

                    }

                    string strEstadoActuacionGeneral = string.Empty;
                    string strEstadoActuacionCodigoGeneral = string.Empty;
                    var EstadoActuacionGeneral = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.EstadoCodigo, (int)EnumeratorTipoDominio.Estados_Actuacion_Derivada);
                    if (EstadoActuacionGeneral != null)
                    {
                        strEstadoActuacionGeneral = EstadoActuacionGeneral.Nombre;
                        strEstadoActuacionCodigoGeneral = EstadoActuacionGeneral.Codigo;

                    }

                    int ActuacionSeguimientoIdTmp = 0;
                    //List<Dominio> listaDominioActuacionActualizada = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Actuacion_adelantada).ToList();

                    List<Dominio> listaDominioActuacionActualizada = new List<Dominio>();

                    if (controversiaContractual.TipoControversiaCodigo == ConstanCodigoTipoControversia.Terminacion_anticipada_por_incumplimiento_TAI)
                    {
                        listaDominioActuacionActualizada = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_controversia_contractual_TAI).ToList();
                    }
                    else
                    {
                        listaDominioActuacionActualizada = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Actuacion_No_TAI).ToList();
                    }

                    GrillaControversiaActuacionEstado RegistroControversiaContractual = new GrillaControversiaActuacionEstado
                    {
                        ControversiaContractualId = controversia.ControversiaContractualId,
                        FechaActualizacion = controversia.FechaModificacion != null ? Convert.ToDateTime(controversia.FechaModificacion).ToString("dd/MM/yyyy") : controversia.FechaModificacion.ToString(),
                        //DescripcionActuacion = "Actuación " + controversia.ControversiaActuacionId.ToString(),
                        DescripcionActuacion = listaDominioActuacionActualizada.Where(r => r.Codigo == controversia.ActuacionAdelantadaCodigo)?.FirstOrDefault()?.Nombre,
                        //DescripcionActuacion = "ACT" + controversia.ControversiaActuacionId.ToString(),
                        ActuacionId = controversia.ControversiaActuacionId,

                        EstadoActuacion = strEstadoAvanceTramite,//controversia.EstadoAvanceTramiteCodigo
                        EstadoActuacionCodigo = strEstadoAvanceTramiteCodigo,//controversia.EstadoAvanceTramiteCodigo

                        NumeroActuacion = controversia.NumeroActuacion,// "ACT controversia " + controversia.ControversiaActuacionId.ToString("000"),

                        //NumeroActuacionReclamacion = "REC " + controversia.ControversiaActuacionId.ToString("0000"),
                        //NumeroActuacionReclamacion = controversia.EsCompletoReclamacion == null || !(bool)controversia.EsCompletoReclamacion ? "" : "REC 0001",
                        NumeroActuacionReclamacion = controversia.NumeroActuacionReclamacion,

                        RegistroCompletoReclamacion = controversia.EsCompletoReclamacion == null ? "Incompleto" : (bool)controversia.EsCompletoReclamacion ? "Completo" : "Incompleto",

                        ProximaActuacionCodigo = strProximaActuacionCodigo,
                        ProximaActuacionNombre = strProximaActuacionNombre,
                        EstadoActuacionReclamacionCodigo = (controversia.EstadoActuacionReclamacionCodigo != null) ? controversia.EstadoActuacionReclamacionCodigo : "",
                        EstadoActuacionReclamacion = EstadoActuacionReclamacionTmp,
                        ActuacionSeguimientoId = ActuacionSeguimientoIdTmp,
                        EstadoActuacionGeneral = strEstadoActuacionGeneral,
                        EstadoActuacionCodigoGeneral = strEstadoActuacionCodigoGeneral,
                        EsRequiereComiteReclamacion = controversia.EsRequiereComiteReclamacion,
                        ActuacionAdelantadaCodigo = controversia.ActuacionAdelantadaCodigo
                    };
                    ListControversiaContractualGrilla.Add(RegistroControversiaContractual);
                }
                catch (Exception e)
                {
                    GrillaControversiaActuacionEstado RegistroControversiaContractual = new GrillaControversiaActuacionEstado
                    {
                        ControversiaContractualId = 0,
                        FechaActualizacion = "ERROR",
                        DescripcionActuacion = e.InnerException.ToString(),
                        ActuacionId = 0,

                        EstadoActuacion = e.ToString(),//controversia.EstadoAvanceTramiteCodigo
                        NumeroActuacion = "ERROR",

                        RegistroCompletoReclamacion = "ERROR",

                        ProximaActuacionCodigo = "ERROR",
                        ProximaActuacionNombre = "ERROR",

                    };
                    ListControversiaContractualGrilla.Add(RegistroControversiaContractual);
                }
            }
            return ListControversiaContractualGrilla.OrderByDescending(r => r.ActuacionId).ToList();
        }

        public async Task<Respuesta> CreateEditarReclamaciones(ControversiaActuacion prmControversiaActuacion)
        {
            Respuesta _response = new Respuesta();

            int idAccionCrearcontroversiaActuacion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Controversia_Actuacion, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = "REGISTRAR EDITAR RECLAMACIÓN";

            try
            {
                if (prmControversiaActuacion != null)
                {
                    var controversiaActuacionActual = _context.ControversiaActuacion.Find(prmControversiaActuacion.ControversiaActuacionId);

                    controversiaActuacionActual.ResumenPropuestaFiduciaria = prmControversiaActuacion.ResumenPropuestaFiduciaria;
                    controversiaActuacionActual.RutaSoporte = prmControversiaActuacion.RutaSoporte;
                    controversiaActuacionActual.EsRequiereComiteReclamacion = prmControversiaActuacion.EsRequiereComiteReclamacion;
                    controversiaActuacionActual.FechaModificacion = DateTime.Now;
                    controversiaActuacionActual.EsCompletoReclamacion = !string.IsNullOrEmpty(prmControversiaActuacion.ResumenPropuestaFiduciaria) && !string.IsNullOrEmpty(prmControversiaActuacion.RutaSoporte);

                    if (string.IsNullOrEmpty(controversiaActuacionActual.NumeroActuacionReclamacion) && controversiaActuacionActual.ActuacionAdelantadaCodigo == ConstanCodigoActuacionAdelantada.RemisiondeComunicaciondedecisiondeTAIporAlianzaFiduciariaalaAseguradora)
                    {
                        int consecutivo = _context.ControversiaActuacion
                                        .Where(r => r.ControversiaContractualId == controversiaActuacionActual.ControversiaContractualId
                                        && controversiaActuacionActual.ActuacionAdelantadaCodigo == ConstanCodigoActuacionAdelantada.RemisiondeComunicaciondedecisiondeTAIporAlianzaFiduciariaalaAseguradora)
                                        .Count();
                        controversiaActuacionActual.NumeroActuacionReclamacion = "REC " + (consecutivo + 1).ToString("000");
                    }
                    _context.ControversiaActuacion.Update(controversiaActuacionActual);
                    _context.SaveChanges();

                    return
                        new Respuesta
                        {
                            Data = prmControversiaActuacion,
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContractualControversy.OperacionExitosa,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales,
                            ConstantMessagesContractualControversy.OperacionExitosa,
                            //contratoPoliza
                            idAccionCrearcontroversiaActuacion
                            , prmControversiaActuacion.UsuarioModificacion
                            , strCrearEditar
                            )
                        };

                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContractualControversy.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContractualControversy.ErrorInterno };
            }
        }

        public async Task<Respuesta> CreateEditarMesa(ControversiaActuacionMesa prmMesa)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_seguimiento_actuacion_derivada, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                bool escompleto = true;
                if (string.IsNullOrEmpty(prmMesa.ActuacionAdelantada) ||
                    prmMesa.FechaActuacionAdelantada == null ||
                    string.IsNullOrEmpty(prmMesa.ProximaActuacionRequerida) ||
                    string.IsNullOrEmpty(prmMesa.Observaciones) ||
                    string.IsNullOrEmpty(prmMesa.RutaSoporte) ||
                    prmMesa.CantDiasVencimiento == null ||
                    prmMesa.FechaVencimiento == null ||
                    string.IsNullOrEmpty(prmMesa.Observaciones) ||
                    prmMesa.ResultadoDefinitivo == null ||
                    string.IsNullOrEmpty(prmMesa.EstadoAvanceMesaCodigo))
                {
                    escompleto = false;
                }
                prmMesa.EsCompleto = escompleto;
                if (prmMesa.ControversiaActuacionMesaId > 0)
                {
                    var actuacionold = _context.ControversiaActuacionMesa.Find(prmMesa.ControversiaActuacionMesaId);
                    actuacionold.FechaModificacion = DateTime.Now;
                    actuacionold.UsuarioModificacion = prmMesa.UsuarioModificacion;
                    actuacionold.Observaciones = prmMesa.Observaciones;
                    actuacionold.EstadoAvanceMesaCodigo = prmMesa.EstadoAvanceMesaCodigo;
                    actuacionold.ActuacionAdelantada = prmMesa.ActuacionAdelantada;
                    actuacionold.ProximaActuacionRequerida = prmMesa.ProximaActuacionRequerida;
                    actuacionold.RutaSoporte = prmMesa.RutaSoporte;
                    actuacionold.EsCompleto = prmMesa.EsCompleto;
                    _context.ControversiaActuacionMesa.Update(actuacionold);
                }
                else
                {
                    ControversiaActuacion controversiaActuacion = _context.ControversiaActuacion.Find(prmMesa.ControversiaActuacionId);
                    /*int consecutivo = _context.ControversiaActuacionMesa
                                .Where(r => r.ControversiaActuacionId == prmMesa.ControversiaCon)
                                .Count();*/
                    List<ControversiaActuacion> controversiaActuaciones = _context.ControversiaActuacion.Where(r => r.ControversiaContractualId == controversiaActuacion.ControversiaContractualId && r.EsRequiereMesaTrabajo == true && (r.Eliminado == false || r.Eliminado == null))
                        .ToList();

                    int consecutivo = 0;

                    foreach (var act in controversiaActuaciones)
                    {
                        consecutivo = consecutivo + _context.ControversiaActuacionMesa
                                .Where(r => r.ControversiaActuacionId == act.ControversiaActuacionId  && (r.Eliminado == false || r.Eliminado == null))
                                .Count();
                    }

                    prmMesa.FechaCreacion = DateTime.Now;
                    prmMesa.EstadoRegistroCodigo = "1";
                    prmMesa.NumeroMesaTrabajo = "MT " + (consecutivo + 1).ToString("000");
                    _context.ControversiaActuacionMesa.Add(prmMesa);
                }
                //al papa le cambio el estado
                /*var controversia = _context.ControversiaActuacion.Find(actuacionSeguimiento.ControversiaActuacionId);
                controversia.EstadoActuacionReclamacionCodigo = "2";//en proceso
                controversia.FechaModificacion = DateTime.Now;
                controversia.UsuarioModificacion = actuacionSeguimiento.UsuarioCreacion == null ? actuacionSeguimiento.UsuarioModificacion : actuacionSeguimiento.UsuarioCreacion;
                _context.ControversiaActuacion.Update(controversia);*/
                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, prmMesa.UsuarioCreacion, "CREAR EDITAR MESA")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, prmMesa.UsuarioCreacion, ex.InnerException.ToString())
                };
            }
        }

        public async Task<List<ControversiaActuacionMesa>> GetMesasByControversiaActuacionId(int pControversiaId)
        {
            var mesas = _context.ControversiaActuacionMesa.Where(x => x.ControversiaActuacion.ControversiaContractualId == pControversiaId).ToList();
            foreach (var mesa in mesas)
            {
                var EstadoActuacionReclamacion = await _commonService.GetDominioByNombreDominioAndTipoDominio(mesa.EstadoRegistroCodigo, (int)EnumeratorTipoDominio.Estados_Actuacion);
                mesa.EstadoRegistroString = mesa.EsCompleto ? "Completo" : "Incompleto";
                mesa.EstadoAvanceMesaString = EstadoActuacionReclamacion == null ? "" : EstadoActuacionReclamacion.Nombre;
            }
            return mesas;
        }

        public async Task<Respuesta> FinalizarMesa(int pControversiaActuacionId, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Controversia_Actuacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ControversiaActuacionMesa actuacionSeguimientoOld;

                actuacionSeguimientoOld = _context.ControversiaActuacionMesa.Where(x => x.ControversiaActuacionId == pControversiaActuacionId).FirstOrDefault();
                actuacionSeguimientoOld.UsuarioModificacion = pUsuarioModifica;
                actuacionSeguimientoOld.FechaModificacion = DateTime.Now;
                actuacionSeguimientoOld.EstadoRegistroCodigo = "2";//cambiar
                _context.ControversiaActuacionMesa.Update(actuacionSeguimientoOld);
                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, pUsuarioModifica, "FINALIZAR MESA DE TRABAJO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }
        }

        public async Task<Respuesta> SetStateActuacionMesa(int pActuacionMesaId, string pNuevoCodigoEstadoAvance, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Controversia_Actuacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ControversiaActuacionMesaSeguimiento actuacionSeguimientoOld;

                actuacionSeguimientoOld = _context.ControversiaActuacionMesaSeguimiento.Find(pActuacionMesaId);
                actuacionSeguimientoOld.UsuarioModificacion = pUsuarioModifica;
                actuacionSeguimientoOld.FechaModificacion = DateTime.Now;
                actuacionSeguimientoOld.EstadoRegistroCodigo = pNuevoCodigoEstadoAvance;
                _context.ControversiaActuacionMesaSeguimiento.Update(actuacionSeguimientoOld);
                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, pUsuarioModifica, "FINALIZAR MESA DE TRABAJO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }
        }

        public async Task<List<ControversiaActuacionMesaSeguimiento>> GetActuacionesMesasByMesaId(int pControversiaActuacionMesaID)
        {
            var mesas = _context.ControversiaActuacionMesaSeguimiento.Where(x => x.ControversiaActuacionMesaId == pControversiaActuacionMesaID).ToList();
            foreach (var mesa in mesas)
            {
                var EstadoActuacionReclamacion = await _commonService.GetDominioByNombreDominioAndTipoDominio(mesa.EstadoRegistroCodigo, (int)EnumeratorTipoDominio.Estados_Actuacion);
                mesa.EstadoRegistroString = mesa.EsCompleto ? "Completo" : "Incompleto";
                mesa.EstadoAvanceMesaString = EstadoActuacionReclamacion == null ? "" : EstadoActuacionReclamacion.Nombre;

            }
            return mesas;
        }

        public async Task<Respuesta> CreateEditarActuacionMesa(ControversiaActuacionMesaSeguimiento controversiaActuacionMesa)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_seguimiento_actuacion_derivada, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ControversiaActuacionMesa controversiaActuacionMesa1 = _context.ControversiaActuacionMesa.Where(r => r.ControversiaActuacionId == controversiaActuacionMesa.ControversiaActuacionMesaId).FirstOrDefault();
                if (controversiaActuacionMesa1 != null)
                {
                    controversiaActuacionMesa.ControversiaActuacionMesaId = controversiaActuacionMesa1.ControversiaActuacionMesaId;
                }

                bool escompleto = true;
                if (string.IsNullOrEmpty(controversiaActuacionMesa.ActuacionAdelantada) ||
                    controversiaActuacionMesa.FechaActuacionAdelantada == null ||
                    string.IsNullOrEmpty(controversiaActuacionMesa.ProximaActuacionRequerida) ||
                    string.IsNullOrEmpty(controversiaActuacionMesa.Observaciones) ||
                    string.IsNullOrEmpty(controversiaActuacionMesa.RutaSoporte))
                {
                    escompleto = false;
                }
                controversiaActuacionMesa.EsCompleto = escompleto;
                if (controversiaActuacionMesa.ControversiaActuacionMesaSeguimientoId > 0)
                {
                    var actuacionold = _context.ControversiaActuacionMesaSeguimiento.Find(controversiaActuacionMesa.ControversiaActuacionMesaSeguimientoId);
                    actuacionold.FechaModificacion = DateTime.Now;
                    actuacionold.UsuarioModificacion = controversiaActuacionMesa.UsuarioModificacion;
                    actuacionold.Observaciones = controversiaActuacionMesa.Observaciones;
                    actuacionold.EstadoAvanceMesaCodigo = controversiaActuacionMesa.EstadoAvanceMesaCodigo;
                    actuacionold.ActuacionAdelantada = controversiaActuacionMesa.ActuacionAdelantada;
                    actuacionold.ProximaActuacionRequerida = controversiaActuacionMesa.ProximaActuacionRequerida;
                    actuacionold.RutaSoporte = controversiaActuacionMesa.RutaSoporte;
                    actuacionold.EsCompleto = controversiaActuacionMesa.EsCompleto;
                    actuacionold.FechaActuacionAdelantada = controversiaActuacionMesa.FechaActuacionAdelantada;
                    actuacionold.CantDiasVencimiento = controversiaActuacionMesa.CantDiasVencimiento;
                    actuacionold.FechaVencimiento = controversiaActuacionMesa.FechaVencimiento;
                    actuacionold.ResultadoDefinitivo = controversiaActuacionMesa.ResultadoDefinitivo;

                    _context.ControversiaActuacionMesaSeguimiento.Update(actuacionold);
                }
                else
                {
                    controversiaActuacionMesa.FechaCreacion = DateTime.Now;
                    //controversiaActuacionMesa.NumeroActuacionSeguimiento = "ACT MT ";
                    //controversiaActuacionMesa.NumeroActuacionSeguimiento = "ACT MT " + controversiaActuacionMesa.ControversiaActuacionMesaSeguimientoId.ToString("000");
                    int consecutivo = _context.ControversiaActuacionMesaSeguimiento
                                     .Where(r => r.ControversiaActuacionMesaId == controversiaActuacionMesa.ControversiaActuacionMesaId)
                                     .Count();
                    controversiaActuacionMesa.NumeroActuacionSeguimiento = "ACT MT " + (consecutivo + 1).ToString("000");

                    controversiaActuacionMesa.EstadoRegistroCodigo = "1";
                    _context.ControversiaActuacionMesaSeguimiento.Add(controversiaActuacionMesa);
                    _context.SaveChanges();
                }
                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, controversiaActuacionMesa.UsuarioCreacion, "CREAR EDITAR MESA")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, controversiaActuacionMesa.UsuarioCreacion, ex.InnerException.ToString())
                };
            }
        }

        public async Task<ControversiaActuacionMesaSeguimiento> GetActuacionMesaByActuacionMesaId(int pControversiaActuacionMesaID)
        {
            return _context.ControversiaActuacionMesaSeguimiento.Find(pControversiaActuacionMesaID);
        }

        public async Task<Respuesta> CambiarEstadoActuacionReclamacionSeguimiento(int pActuacionId, string pEstadoReclamacionCodigo, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Actuacion_Seguimiento, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ActuacionSeguimiento actuacionSeguimientoOld;

                actuacionSeguimientoOld = _context.ActuacionSeguimiento.Find(pActuacionId);
                actuacionSeguimientoOld.UsuarioModificacion = pUsuarioModifica;
                actuacionSeguimientoOld.FechaModificacion = DateTime.Now;
                actuacionSeguimientoOld.EstadoReclamacionCodigo = pEstadoReclamacionCodigo;

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, pUsuarioModifica, "CAMBIAR ESTADO ACTUACION SEGUIMIENTO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }
        }

        public async Task<Respuesta> EliminarActuacionSeguimientoActuacion(int pActuacionSeguimientoId, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Actuacion_Seguimiento, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ActuacionSeguimiento actuacionSeguimientoOld;

                actuacionSeguimientoOld = _context.ActuacionSeguimiento.Find(pActuacionSeguimientoId);
                actuacionSeguimientoOld.UsuarioModificacion = pUsuarioModifica;
                actuacionSeguimientoOld.FechaModificacion = DateTime.Now;
                actuacionSeguimientoOld.Eliminado = true;
                _context.ActuacionSeguimiento.Update(actuacionSeguimientoOld);
                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, pUsuarioModifica, "FINALIZAR ESTADO ACTUACION SEGUIMIENTO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }
        }

        public async Task<List<GrillaControversiaActuacionEstado>> GetListGrillMesasByControversiaActuacionId(int id)
        {
            List<GrillaControversiaActuacionEstado> ListControversiaContractualGrilla = new List<GrillaControversiaActuacionEstado>();
            List<ControversiaActuacion> lstControversiaActuacion = await _context.ControversiaActuacion.
                Where(r => !(bool)r.Eliminado && r.ControversiaContractualId == id).Include(r => r.ActuacionSeguimiento).Distinct().ToListAsync();
            ControversiaContractual controversiaContractual = _context.ControversiaContractual.Find(id);

            foreach (var controversia in lstControversiaActuacion)
            {
                try
                {
                    Contrato contrato = null;
                    int i = 0;

                    //contrato = await _commonService.GetContratoPolizaByContratoId(controversia.ContratoId);
                    //contrato = _context.Contrato.Where(r => r.ContratoId == controversia.id).FirstOrDefault();

                    //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
                    string strEstadoAvanceTramiteCodigo = "sin definir";
                    string strEstadoAvanceTramite = "sin definir";

                    string strEstadoActuacionGeneral = string.Empty;
                    string strEstadoActuacionCodigoGeneral = string.Empty;
                    var EstadoActuacionGeneral = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.EstadoCodigo, (int)EnumeratorTipoDominio.Estados_Actuacion_Derivada);
                    if (EstadoActuacionGeneral != null)
                    {
                        strEstadoActuacionGeneral = EstadoActuacionGeneral.Nombre;
                        strEstadoActuacionCodigoGeneral = EstadoActuacionGeneral.Codigo;

                    }

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio EstadoAvanceCodigo;

                    string strProximaActuacionCodigo = "sin definir";
                    string strProximaActuacionNombre = "sin definir";

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio ProximaActuacionCodigo;

                    ProximaActuacionCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.ProximaActuacionCodigo, (int)EnumeratorTipoDominio.Proxima_actuacion_requerida);
                    if (ProximaActuacionCodigo != null)
                    {
                        strProximaActuacionNombre = ProximaActuacionCodigo.Nombre;
                        strProximaActuacionCodigo = ProximaActuacionCodigo.Codigo;

                    }

                    EstadoAvanceCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.EstadoAvanceTramiteCodigo, (int)EnumeratorTipoDominio.Estados_Avance_Tramite);
                    if (EstadoAvanceCodigo != null)
                    {
                        strEstadoAvanceTramite = EstadoAvanceCodigo.Nombre;
                        strEstadoAvanceTramiteCodigo = EstadoAvanceCodigo.Codigo;

                    }


                    string EstadoActuacionReclamacionCodigoTmp = "";
                    string EstadoActuacionReclamacionTmp = "";

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio EstadoActuacionReclamacion;

                    EstadoActuacionReclamacion = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.EstadoActuacionReclamacionCodigo, (int)EnumeratorTipoDominio.Estados_Reclamacion);
                    if (EstadoActuacionReclamacion != null)
                    {
                        EstadoActuacionReclamacionTmp = EstadoActuacionReclamacion.Nombre;
                        EstadoActuacionReclamacionCodigoTmp = EstadoActuacionReclamacion.Codigo;

                    }

                    int ActuacionSeguimientoIdTmp = 0;
                    ControversiaActuacionMesa controversiamesa = _context.ControversiaActuacionMesa.Where(x => !(bool)x.Eliminado && x.ControversiaActuacionId == controversia.ControversiaActuacionId).FirstOrDefault();
                    string stadomesa = "";
                    if (controversiamesa != null)
                    {
                        var EstadoActuacionMesa = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiamesa.EstadoRegistroCodigo, (int)EnumeratorTipoDominio.Estados_Actuacion);
                        stadomesa = EstadoActuacionMesa == null ? "" : EstadoActuacionMesa.Nombre;
                    }

                    //List<Dominio> listaDominioActuacionActualizada = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Actuacion_adelantada).ToList();

                    List<Dominio> listaDominioActuacionActualizada = new List<Dominio>();

                    if (controversiaContractual.TipoControversiaCodigo == ConstanCodigoTipoControversia.Terminacion_anticipada_por_incumplimiento_TAI)
                    {
                        listaDominioActuacionActualizada = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_controversia_contractual_TAI).ToList();
                    }
                    else
                    {
                        listaDominioActuacionActualizada = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Actuacion_No_TAI).ToList();
                    }

                    GrillaControversiaActuacionEstado RegistroControversiaContractual = new GrillaControversiaActuacionEstado
                    {

                        ControversiaContractualId = controversia.ControversiaContractualId,
                        FechaActualizacion = controversia.FechaActuacion != null ? Convert.ToDateTime(controversia.FechaActuacion).ToString("dd/MM/yyyy") : "",
                        //DescripcionActuacion = "Actuación " + controversia.ControversiaActuacionId.ToString(),
                        DescripcionActuacion = listaDominioActuacionActualizada.Where(r => r.Codigo == controversia.ActuacionAdelantadaCodigo)?.FirstOrDefault()?.Nombre,
                        //DescripcionActuacion = "ACT" + controversia.ControversiaActuacionId.ToString(),
                        ActuacionId = controversia.ControversiaActuacionId,

                        EstadoActuacion = strEstadoAvanceTramite,//controversia.EstadoAvanceTramiteCodigo
                        EstadoActuacionCodigo = strEstadoAvanceTramiteCodigo,//controversia.EstadoAvanceTramiteCodigo

                        EstadoActuacionGeneral = strEstadoActuacionGeneral,
                        EstadoActuacionCodigoGeneral = strEstadoActuacionCodigoGeneral,

                        NumeroActuacion = controversia.NumeroActuacion,// "ACT controversia " + controversia.ControversiaActuacionId.ToString("000"),

                        NumeroActuacionReclamacion = "REC " + controversia.ControversiaActuacionId.ToString("0000"),
                        //NumeroActuacionReclamacion = controversia.EsCompletoReclamacion == null || !(bool)controversia.EsCompletoReclamacion ? "" : "REC 0001",


                        RegistroCompletoReclamacion = controversia.EsCompletoReclamacion == null ? "Incompleto" : (bool)controversia.EsCompletoReclamacion ? "Completo" : "Incompleto",
                        RegistroCompletoMesa = controversiamesa == null ? "Incompleto" : (bool)controversiamesa.EsCompleto ? "Completo" : "Incompleto",

                        ProximaActuacionCodigo = strProximaActuacionCodigo,
                        ProximaActuacionNombre = strProximaActuacionNombre,
                        EstadoActuacionReclamacionCodigo = (controversia.EstadoActuacionReclamacionCodigo != null) ? controversia.EstadoActuacionReclamacionCodigo : "",
                        EstadoActuacionReclamacion = EstadoActuacionReclamacionTmp,
                        ActuacionSeguimientoId = ActuacionSeguimientoIdTmp,
                        NumeroMesa = controversiamesa == null ? "" : controversiamesa.NumeroMesaTrabajo,
                        //NumeroMesa = controversiamesa == null ? "" : "MT " + controversiamesa.ControversiaActuacionMesaId.ToString("0000"),
                        EstadoMesa = controversiamesa == null ? "" : controversiamesa.EstadoRegistroCodigo,
                        EstadoCodigoMesa = stadomesa,
                        MesaId = controversiamesa == null ? "" : controversiamesa.ControversiaActuacionMesaId.ToString(),
                        RequiereMesaTrabajo = controversia.EsRequiereMesaTrabajo,

                    };
                    ListControversiaContractualGrilla.Add(RegistroControversiaContractual);
                }
                catch (Exception e)
                {
                    GrillaControversiaActuacionEstado RegistroControversiaContractual = new GrillaControversiaActuacionEstado
                    {
                        ControversiaContractualId = 0,
                        FechaActualizacion = "ERROR",
                        DescripcionActuacion = e.InnerException.ToString(),
                        ActuacionId = 0,

                        EstadoActuacion = e.ToString(),//controversia.EstadoAvanceTramiteCodigo
                        NumeroActuacion = "ERROR",

                        RegistroCompletoReclamacion = "ERROR",

                        ProximaActuacionCodigo = "ERROR",
                        ProximaActuacionNombre = "ERROR",

                    };
                    ListControversiaContractualGrilla.Add(RegistroControversiaContractual);
                }
            }
            return ListControversiaContractualGrilla.OrderByDescending(r => r.ActuacionId).ToList();
        }

        public async Task<List<ControversiaActuacionMesaSeguimiento>> GetActuacionesMesasByActuacionId(int pActuacionId)
        {
            //var mesas = _context.ControversiaActuacionMesaSeguimiento.Where(x => x.ControversiaActuacionMesa.ControversiaActuacionId == pActuacionId).Include(x => x.ControversiaActuacionMesa).ToList();
            var mesas = _context.ControversiaActuacionMesaSeguimiento.Where(x => x.ControversiaActuacionMesa.ControversiaActuacionId == pActuacionId && !(bool)x.Eliminado).Include(x => x.ControversiaActuacionMesa).ToList();

            foreach (var mesa in mesas)
            {
                var EstadoActuacionReclamacion = await _commonService.GetDominioByNombreDominioAndTipoDominio(mesa.EstadoRegistroCodigo, (int)EnumeratorTipoDominio.Estados_Actuacion);
                mesa.EstadoRegistroString = mesa.EsCompleto ? "Completo" : "Incompleto";
                mesa.EstadoAvanceMesaString = EstadoActuacionReclamacion == null ? "" : EstadoActuacionReclamacion.Nombre;
                mesa.Actuacion = "Actuación " + mesa.ControversiaActuacionMesa.ControversiaActuacionId;
            }
            return mesas;
        }

        public async Task<Respuesta> EliminacionActuacionMesa(int pControversiaActuacionMesaId, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Actuacion_Seguimiento, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ControversiaActuacionMesaSeguimiento actuacionSeguimientoOld;

                actuacionSeguimientoOld = _context.ControversiaActuacionMesaSeguimiento.Find(pControversiaActuacionMesaId);
                actuacionSeguimientoOld.UsuarioModificacion = pUsuarioModifica;
                actuacionSeguimientoOld.FechaModificacion = DateTime.Now;
                actuacionSeguimientoOld.Eliminado = true;
                _context.ControversiaActuacionMesaSeguimiento.Update(actuacionSeguimientoOld);
                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, pUsuarioModifica, "ELIMINAR ACTUACION MESA")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }
        }

        public async Task<Respuesta> EliminacionMesa(int pControversiaMesaId, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Actuacion_Seguimiento, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ControversiaActuacionMesa actuacionSeguimientoOld;

                actuacionSeguimientoOld = _context.ControversiaActuacionMesa.Find(pControversiaMesaId);
                actuacionSeguimientoOld.UsuarioModificacion = pUsuarioModifica;
                actuacionSeguimientoOld.FechaModificacion = DateTime.Now;
                actuacionSeguimientoOld.Eliminado = true;
                _context.ControversiaActuacionMesa.Update(actuacionSeguimientoOld);
                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, pUsuarioModifica, "ELIMINAR MESA")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }
        }

        public async Task<ControversiaActuacionMesa> GetMesaByMesaId(int pControversiaMesaID)
        {
            return _context.ControversiaActuacionMesa.Find(pControversiaMesaID);
        }

        public async Task<SeguimientoActuacionDerivada> GetSeguimientoActuacionDerivadabyId(int pSeguimientoActuacionDerivadaId)
        {

            return await _context.SeguimientoActuacionDerivada.Where(r => r.SeguimientoActuacionDerivadaId == pSeguimientoActuacionDerivadaId).FirstOrDefaultAsync();
        }

        public async Task<Respuesta> ChangeStateActuacion(int pControversiaActuacionId, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Actuacion_Seguimiento, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ControversiaActuacion actuacionSeguimientoOld;

                actuacionSeguimientoOld = _context.ControversiaActuacion.Find(pControversiaActuacionId);
                actuacionSeguimientoOld.UsuarioModificacion = pUsuarioModifica;
                actuacionSeguimientoOld.FechaModificacion = DateTime.Now;
                actuacionSeguimientoOld.EstadoCodigo = "1";//Actualizar trámite

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.OperacionExitosa, idAccion, pUsuarioModifica, "FINALIZAR ESTADO ACTUACION SEGUIMIENTO")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesContractualControversy.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_controversias_contractuales, ConstantMessagesContractualControversy.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }
        }

        #region Alertas 

        //Alerta - usuario juridica (antes de 3/2 o 1 dia de que se cumpla la Fecha de vencimiento de términos para la próxima actuación requerida)
        public async Task VencimientoTerminosContrato()
        {
            DateTime RangoFechaCon3DiasHabiles = await _commonService.CalculardiasLaborales(3, DateTime.Now);
            DateTime RangoFechaCon2DiasHabiles = await _commonService.CalculardiasLaborales(2, DateTime.Now);
            DateTime RangoFechaCon1DiasHabiles = await _commonService.CalculardiasLaborales(1, DateTime.Now);
            List<ControversiaActuacion> controversiaActuacion3dias = _context.ControversiaActuacion
                .Where(r => (r.EstadoCodigo == ConstantCodigoEstadoControversiaActuacion.Finalizada || r.EstadoCodigo == ConstantCodigoEstadoControversiaActuacion.Enviado_a_comite_tecnico) && r.FechaVencimiento == RangoFechaCon3DiasHabiles)
                .Include(r => r.ControversiaContractual)
                    .ThenInclude(r => r.Contrato)
                .ToList();

            List<ControversiaActuacion> controversiaActuacion2dias = _context.ControversiaActuacion
                .Where(r => (r.EstadoCodigo == ConstantCodigoEstadoControversiaActuacion.Finalizada || r.EstadoCodigo == ConstantCodigoEstadoControversiaActuacion.Enviado_a_comite_tecnico) && r.FechaVencimiento == RangoFechaCon2DiasHabiles)
                .Include(r => r.ControversiaContractual)
                    .ThenInclude(r => r.Contrato)
                .ToList();

            List<ControversiaActuacion> controversiaActuacion1dias = _context.ControversiaActuacion
                .Where(r => (r.EstadoCodigo == ConstantCodigoEstadoControversiaActuacion.Finalizada || r.EstadoCodigo == ConstantCodigoEstadoControversiaActuacion.Enviado_a_comite_tecnico) && r.FechaVencimiento == RangoFechaCon1DiasHabiles)
                .Include(r => r.ControversiaContractual)
                    .ThenInclude(r => r.Contrato)
                .ToList();

            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Juridica).Include(y => y.Usuario);
            List<EnumeratorPerfil> perfilsEnviarCorreo = new List<EnumeratorPerfil> { EnumeratorPerfil.Juridica };
            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.FechaVencimientoProximaActuacionJuridica_4_2_1);

            foreach (var controversia in controversiaActuacion3dias)
            {

                if (controversiaActuacion3dias.Count() > 0)
                {
                    Dominio ProximaActuacionCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.ProximaActuacionCodigo, (int)EnumeratorTipoDominio.Proxima_actuacion_requerida);
                    Dominio TipoControversia = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.ControversiaContractual.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipos_Controversia);

                    string strContenido = TemplateRecoveryPassword.Contenido
                                 .Replace("[NUMERO_CONTRATO]", controversia.ControversiaContractual.Contrato.NumeroContrato)
                                 .Replace("[PROXIMA_ACTUACION]", ProximaActuacionCodigo != null ? ProximaActuacionCodigo.Nombre : String.Empty)
                                 .Replace("[FECHA_VENCIMIENTO]", ((DateTime)controversia.FechaVencimiento).ToString("yyyy-MM-dd"))
                                 .Replace("[DIAS]", "tres (3) días")
                                 .Replace("[FECHA_SOLICITUD_CONTROVERSIA]", ((DateTime)controversia.ControversiaContractual.FechaCreacion).ToString("yyyy-MM-dd"))
                                 .Replace("[NUMERO_SOLICITUD]", controversia.ControversiaContractual.NumeroSolicitud)
                                 .Replace("[TIPO_CONTROVERSIA]", TipoControversia != null ? TipoControversia.Nombre : String.Empty);


                    _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, TemplateRecoveryPassword.Asunto);

                }
            }

            foreach (var controversia in controversiaActuacion2dias)
            {

                if (controversiaActuacion3dias.Count() > 0)
                {
                    Dominio ProximaActuacionCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.ProximaActuacionCodigo, (int)EnumeratorTipoDominio.Proxima_actuacion_requerida);
                    Dominio TipoControversia = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.ControversiaContractual.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipos_Controversia);

                    string strContenido = TemplateRecoveryPassword.Contenido
                                    .Replace("[NUMERO_CONTRATO]", controversia.ControversiaContractual.Contrato.NumeroContrato)
                                    .Replace("[PROXIMA_ACTUACION]", ProximaActuacionCodigo != null ? ProximaActuacionCodigo.Nombre : String.Empty)
                                    .Replace("[FECHA_VENCIMIENTO]", ((DateTime)controversia.FechaVencimiento).ToString("yyyy-MM-dd"))
                                    .Replace("[DIAS]", "dos (2) días")
                                    .Replace("[FECHA_SOLICITUD_CONTROVERSIA]", ((DateTime)controversia.ControversiaContractual.FechaCreacion).ToString("yyyy-MM-dd"))
                                    .Replace("[NUMERO_SOLICITUD]", controversia.ControversiaContractual.NumeroSolicitud)
                                    .Replace("[TIPO_CONTROVERSIA]", TipoControversia != null ? TipoControversia.Nombre : String.Empty);


                    _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, TemplateRecoveryPassword.Asunto);

                }
            }

            foreach (var controversia in controversiaActuacion1dias)
            {

                if (controversiaActuacion3dias.Count() > 0)
                {
                    Dominio ProximaActuacionCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.ProximaActuacionCodigo, (int)EnumeratorTipoDominio.Proxima_actuacion_requerida);
                    Dominio TipoControversia = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.ControversiaContractual.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipos_Controversia);

                    string strContenido = TemplateRecoveryPassword.Contenido
                                    .Replace("[NUMERO_CONTRATO]", controversia.ControversiaContractual.Contrato.NumeroContrato)
                                    .Replace("[PROXIMA_ACTUACION]", ProximaActuacionCodigo != null ? ProximaActuacionCodigo.Nombre : String.Empty)
                                    .Replace("[FECHA_VENCIMIENTO]", ((DateTime)controversia.FechaVencimiento).ToString("yyyy-MM-dd"))
                                    .Replace("[DIAS]", "(1) día")
                                    .Replace("[FECHA_SOLICITUD_CONTROVERSIA]", ((DateTime)controversia.ControversiaContractual.FechaCreacion).ToString("yyyy-MM-dd"))
                                    .Replace("[NUMERO_SOLICITUD]", controversia.ControversiaContractual.NumeroSolicitud)
                                    .Replace("[TIPO_CONTROVERSIA]", TipoControversia != null ? TipoControversia.Nombre : String.Empty);


                    _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, TemplateRecoveryPassword.Asunto);

                }
            }
        }
        #endregion

        #region Correos 
        /// Envio de correo cuando finaliza el proceso de actuación en 4.2.1 y tiene algun check en true (EsRequiereContratista,EsRequiereInterventor,EsRequiereSupervisor,EsRequiereFiduciaria)
        private async Task<bool> SendMailParticipation(int pControversiaActuacionId, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Participacion_Insumo_Realizar_Actuación_4_2_1));
            ControversiaActuacion controversia = _context.ControversiaActuacion
                .Where(r => (r.EstadoCodigo == ConstantCodigoEstadoControversiaActuacion.Finalizada || r.EstadoCodigo == ConstantCodigoEstadoControversiaActuacion.Enviado_a_comite_tecnico)
                            && r.ControversiaActuacionId == pControversiaActuacionId
                            && (r.EsRequiereContratista == true || r.EsRequiereInterventor == true || r.EsRequiereSupervisor == true || r.EsRequiereFiduciaria == true))
                .Include(r => r.ControversiaContractual)
                    .ThenInclude(r => r.Contrato)
                        .ThenInclude(r => r.Contratacion)
                .FirstOrDefault();

            List<EnumeratorPerfil> perfilsEnviarCorreo = new List<EnumeratorPerfil>();

            if (controversia != null)
            {
                Dominio ProximaActuacionCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.ProximaActuacionCodigo, (int)EnumeratorTipoDominio.Proxima_actuacion_requerida);
                Dominio TipoControversia = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.ControversiaContractual.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipos_Controversia);

                if ((bool)controversia.EsRequiereContratista)
                {
                    if (controversia.ControversiaContractual.Contrato.Contratacion.ContratistaId != null)
                    {
                        Usuario user = new Usuario();
                        Contratista contratista = _context.Contratista.Find(controversia.ControversiaContractual.Contrato.Contratacion.ContratistaId);

                        if (contratista != null)
                        {
                            user = _context.Usuario.Where(r => r.NumeroIdentificacion.Equals(contratista.NumeroIdentificacion)).FirstOrDefault();
                        }

                        string template2 = template.Contenido
                                        .Replace("_LinkF_", pDominioFront)
                                        .Replace("[NUMERO_CONTRATO]", controversia.ControversiaContractual.Contrato.NumeroContrato)
                                        .Replace("[PROXIMA_ACTUACION]", ProximaActuacionCodigo != null ? ProximaActuacionCodigo.Nombre : String.Empty)
                                        .Replace("[FECHA_SOLICITUD_CONTROVERSIA]", ((DateTime)controversia.ControversiaContractual.FechaCreacion).ToString("yyyy-MM-dd"))
                                        .Replace("[NUMERO_SOLICITUD]", controversia.ControversiaContractual.NumeroSolicitud)
                                        .Replace("[TIPO_CONTROVERSIA]", TipoControversia != null ? TipoControversia.Nombre : String.Empty);


                        if (user != null && user.UsuarioId > 0)
                        {
                            Helpers.Helpers.EnviarCorreo(user.Email, template.Asunto, template2, pSender, pPassword, pMailServer, pMailPort);
                        }
                    }

                    perfilsEnviarCorreo.Add(EnumeratorPerfil.Tecnica);//que rol tiene el contratista??
                }
                if ((bool)controversia.EsRequiereInterventor)
                {
                    perfilsEnviarCorreo.Add(EnumeratorPerfil.Interventor);
                }
                if ((bool)controversia.EsRequiereSupervisor)
                {
                    perfilsEnviarCorreo.Add(EnumeratorPerfil.Supervisor);
                }
                if ((bool)controversia.EsRequiereFiduciaria)
                {
                    perfilsEnviarCorreo.Add(EnumeratorPerfil.Fiduciaria);
                }
                if (perfilsEnviarCorreo.Count() > 0)
                {
                    string strContenido = template.Contenido
                                    .Replace("[NUMERO_CONTRATO]", controversia.ControversiaContractual.Contrato.NumeroContrato)
                                    .Replace("[PROXIMA_ACTUACION]", ProximaActuacionCodigo != null ? ProximaActuacionCodigo.Nombre : String.Empty)
                                    .Replace("[FECHA_SOLICITUD_CONTROVERSIA]", ((DateTime)controversia.ControversiaContractual.FechaCreacion).ToString("yyyy-MM-dd"))
                                    .Replace("[NUMERO_SOLICITUD]", controversia.ControversiaContractual.NumeroSolicitud)
                                    .Replace("[TIPO_CONTROVERSIA]", TipoControversia != null ? TipoControversia.Nombre : String.Empty);

                    return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
                }
            }

            return false;
        }

        private async Task<bool> SendMailActuacionDerivada(int pSeguimientoActuacionDerivada)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Registrar_Actuaciones_Controversias_Contractuales_4_4_1));
            SeguimientoActuacionDerivada derivada = _context.SeguimientoActuacionDerivada
                    .Where(r => r.SeguimientoActuacionDerivadaId == pSeguimientoActuacionDerivada)
                    .Include(r => r.ControversiaActuacion)
                        .ThenInclude(r => r.ControversiaContractual)
                            .ThenInclude(r => r.Contrato)
                                .ThenInclude(r => r.Contratacion)
                    .FirstOrDefault();

            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Juridica
                                          };

            if (derivada != null)
            {
                Dominio TipoControversiaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(derivada.ControversiaActuacion.ControversiaContractual.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipos_Controversia);

                String strContenido = template.Contenido
                             .Replace("[NUMERO_CONTRATO]", derivada.ControversiaActuacion.ControversiaContractual.Contrato.NumeroContrato)
                             .Replace("[FECHA_SOLICITUD_CONTROVERSIA]", ((DateTime)derivada.ControversiaActuacion.FechaActuacion).ToString("yyyy-MM-dd"))
                             .Replace("[NUMERO_SOLICITUD]", derivada.ControversiaActuacion.ControversiaContractual.NumeroSolicitud)
                             .Replace("[TIPO_CONTROVERSIA]", TipoControversiaCodigo.Nombre)
                             .Replace("[FECHA_ACTUACION_DERIVADA]", ((DateTime)derivada.FechaActuacionDerivada).ToString("yyyy-MM-dd"))
                             .Replace("[DESCRIPCION_ACTUACION]", derivada.DescripciondeActuacionAdelantada);

                return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
            }

            return false;
        }
        #endregion
        public async Task<string> ReemplazarDatosPlantillaControversiaContractual(string pPlantilla, int pControversiaContractualId)
        {
            try
            {
                ControversiaContractual controversiaContractual = _context.ControversiaContractual
                   .Where(r => r.ControversiaContractualId == pControversiaContractualId).Include(x => x.Contrato).FirstOrDefault();

                List<ControversiaActuacion> controversiasActuacion = _context.ControversiaActuacion.
                    Where(r => !(bool)r.Eliminado && r.ControversiaContractualId == pControversiaContractualId)
                    .Include(r => r.ActuacionSeguimiento)
                    .Include(r => r.ControversiaActuacionMesa)
                        .ThenInclude(r => r.ControversiaActuacionMesaSeguimiento)
                    .Distinct().ToList();

                List<Dominio> placeholders = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.PlaceHolder).ToList();

                string TipoPlantillaDetalleProyecto = ((int)ConstanCodigoPlantillas.Detalle_Proyecto).ToString();
                string DetalleProyecto = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaDetalleProyecto).Select(r => r.Contenido).FirstOrDefault();
                string DetallesProyectos = "";

                string TipoPlantillaRegistrosAlcance = ((int)ConstanCodigoPlantillas.Registros_Tabla_Alcance).ToString();
                string RegistroAlcance = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaRegistrosAlcance).Select(r => r.Contenido).FirstOrDefault();

                string TipoPlantillaEjecucionProyecto = ((int)ConstanCodigoPlantillas.Ejecucion_proyecto).ToString();
                string EjecucionProyecto = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaEjecucionProyecto).Select(r => r.Contenido).FirstOrDefault();

                string EjecucionesProyecto = "";

                //TAI
                string TipoPlantillaDetalleSolicitud = ((int)ConstanCodigoPlantillas.Detalle_solicitud_tai).ToString();
                string DetalleSolicitud = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaDetalleSolicitud).Select(r => r.Contenido).FirstOrDefault();

                //historial reclamaciones
                string TipoPlantillaHistorialReclamacionesMesasTrabajo = ((int)ConstanCodigoPlantillas.Informacion_Adicional_Reclamacion_4_2_1).ToString();
                string HistorialReclamacionesMesasTrabajo = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaHistorialReclamacionesMesasTrabajo).Select(r => r.Contenido).FirstOrDefault();

                //NO TAI
                if (controversiaContractual.TipoControversiaCodigo != ConstanCodigoTipoControversia.Terminacion_anticipada_por_incumplimiento_TAI)
                {
                    TipoPlantillaDetalleSolicitud = ((int)ConstanCodigoPlantillas.Detalle_solicitud_no_tai).ToString();
                    DetalleSolicitud = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaDetalleSolicitud).Select(r => r.Contenido).FirstOrDefault();

                    //historial Mesas de trabajo
                    TipoPlantillaHistorialReclamacionesMesasTrabajo = ((int)ConstanCodigoPlantillas.Mesas_4_2_1).ToString();
                    HistorialReclamacionesMesasTrabajo = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaHistorialReclamacionesMesasTrabajo).Select(r => r.Contenido).FirstOrDefault();

                }

                string DetallesSolicitudes = "";
                string HistorialesRecMesas = "";

                //Información adicional reclamaciones
                string TipoPlantillaReclamaciones = ((int)ConstanCodigoPlantillas.Reclamaciones_4_2_1).ToString();
                string HistorialReclamacion = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaReclamaciones).Select(r => r.Contenido).FirstOrDefault();
                string HistorialesReclamacion = "";

                //historial modificaciones
                string TipoPlantillaHistorialModifcaciones = ((int)ConstanCodigoPlantillas.Historial_de_modificaciones_controversias).ToString();
                string HistorialModificaciones = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaHistorialModifcaciones).Select(r => r.Contenido).FirstOrDefault();
                string Historiales = "";

                //historial actuaciones
                string TipoPlantillaHistorialActModifcaciones = ((int)ConstanCodigoPlantillas.Tabla_actuaciones).ToString();
                string HistorialActuaciones = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaHistorialActModifcaciones).Select(r => r.Contenido).FirstOrDefault();
                string HistorialesAct = "";

                //mesas de trabajo Actuaciones
                string TipoPlantillaActuacionesMesa = ((int)ConstanCodigoPlantillas.Actuaciones_Mesas_4_2_1).ToString();
                string ActuacionMesa = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaActuacionesMesa).Select(r => r.Contenido).FirstOrDefault();

                string ActuacionesMesa = "";

                //tipos de novedad en el historial

                //adicion
                string TipoPlantillaNovedadAdicion = ((int)ConstanCodigoPlantillas.Tipo_de_novedad_adicion).ToString();
                string NovedadAdicion = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaNovedadAdicion).Select(r => r.Contenido).FirstOrDefault();
                string NovedadesAdicion = "";
                //prorroga
                string TipoPlantillaNovedadProrroga = ((int)ConstanCodigoPlantillas.Tipo_de_novedad_prorroga).ToString();
                string NovedadProrroga = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaNovedadProrroga).Select(r => r.Contenido).FirstOrDefault();
                string NovedadesProrroga = "";
                //modificacion contractal
                string TipoPlantillaNovedadModificacion = ((int)ConstanCodigoPlantillas.Tipo_de_novedad_modificacion_contractual).ToString();
                string NovedadModificacion = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaNovedadModificacion).Select(r => r.Contenido).FirstOrDefault();
                string NovedadesModificacion = "";
                //otras
                string TipoPlantillaNovedadOtras = ((int)ConstanCodigoPlantillas.Tipo_de_novedad_otras).ToString();
                string NovedadOtras = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaNovedadOtras).Select(r => r.Contenido).FirstOrDefault();
                string NovedadesOtras = "";

                //APORTANTES

                string TipoPlantillaAportantes = ((int)ConstanCodigoPlantillas.Tipo_de_novedad_adicion_aportante).ToString();
                string Aportante = _context.Plantilla.Where(r => r.Codigo == TipoPlantillaAportantes).Select(r => r.Contenido).FirstOrDefault();
                string Aportantes = " ";

                List<Dominio> ListaParametricas = _context.Dominio.ToList();
                List<Localizacion> ListaLocalizaciones = _context.Localizacion.ToList();
                List<InstitucionEducativaSede> ListaInstitucionEducativaSedes = _context.InstitucionEducativaSede.ToList();
                //Se crea el detalle de los proyectos asociado a contratacion - contratacionProy ecto 
                int enumProyecto = 1;
                int enumProyectoEjecucion = 1;
                int enumHistorial = 1;
                int enumClausula = 1;

                Contrato contrato = controversiaContractual.Contrato;
                Contratacion contratacion = null;
                DisponibilidadPresupuestal disponibilidadPresupuestal = null;

                List<NovedadContractual> novedadContractual = new List<NovedadContractual>();

                Contratista contratista = null;

                if (contrato != null)
                {
                    contratacion = await _IProjectContractingService.GetAllContratacionByContratacionId(contrato.ContratacionId);
                    novedadContractual = _context.NovedadContractual.Where(r => r.ContratoId == contrato.ContratoId)
                                                                    .Include(r => r.NovedadContractualDescripcion)
                                                                        .ThenInclude(r => r.NovedadContractualClausula)
                                                                    .Include(r => r.NovedadContractualAportante)
                                                                        .ThenInclude(r => r.ComponenteAportanteNovedad)
                                                                            .ThenInclude(r => r.ComponenteFuenteNovedad)
                                                                                .ThenInclude(r => r.ComponenteUsoNovedad).ToList();
                }
                if (contratacion != null)
                {
                    contratista = _context.Contratista
                        .Where(r => r.ContratistaId == contratacion.ContratistaId).FirstOrDefault();
                    disponibilidadPresupuestal = _context.DisponibilidadPresupuestal
                        .Where(r => r.ContratacionId == contratacion.ContratacionId).FirstOrDefault();
                }

                foreach (var proyecto in contratacion.ContratacionProyecto)
                {
                    //Se crear una nueva plantilla por cada vez que entra
                    DetallesProyectos += DetalleProyecto;
                    EjecucionesProyecto += EjecucionProyecto;
                    string RegistrosAlcance = "";

                    Localizacion Municipio = ListaLocalizaciones.Where(r => r.LocalizacionId == proyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                    Localizacion Departamento = ListaLocalizaciones.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                    Localizacion Region = ListaLocalizaciones.Where(r => r.LocalizacionId == Departamento.IdPadre).FirstOrDefault();

                    InstitucionEducativaSede IntitucionEducativa = ListaInstitucionEducativaSedes.Where(r => r.InstitucionEducativaSedeId == proyecto.Proyecto.InstitucionEducativaId).FirstOrDefault();
                    InstitucionEducativaSede Sede = ListaInstitucionEducativaSedes.Where(r => r.InstitucionEducativaSedeId == proyecto.Proyecto.SedeId).FirstOrDefault();

                    #region Detalle Solicitud

                    foreach (Dominio placeholderDominio in placeholders)
                    {
                        switch (placeholderDominio.Codigo)
                        {

                            case ConstanCodigoVariablesPlaceHolders.NOMBRE_PROYECTO:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, (enumProyecto++).ToString());
                                EjecucionesProyecto = EjecucionesProyecto.Replace(placeholderDominio.Nombre, (enumProyectoEjecucion++).ToString());
                                break;

                            case ConstanCodigoVariablesPlaceHolders.TIPO_DE_INTERVENCION:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, ListaParametricas.Where(r => r.Codigo == proyecto.Proyecto.TipoIntervencionCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).FirstOrDefault().Nombre);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.LLAVE_MEN:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.LlaveMen);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.REGION:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, Region.Descripcion);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.DEPARTAMENTO:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, Departamento.Descripcion);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.MUNICIPIO:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, Municipio.Descripcion);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.INSTITUCION_EDUCATIVA:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, IntitucionEducativa.Nombre);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.CODIGO_DANE_IE:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, IntitucionEducativa.CodigoDane);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.SEDE:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, Sede.Nombre);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.CODIGO_DANE_SEDE:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, Sede.CodigoDane);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.REGISTROS_ALCANCE:

                                foreach (var infraestructura in proyecto.Proyecto.InfraestructuraIntervenirProyecto)
                                {
                                    RegistrosAlcance += RegistroAlcance;

                                    RegistrosAlcance = RegistrosAlcance.Replace("[ALCANCE_ESPACIOS_A_INTERVENIR]", ListaParametricas
                                        .Where(r => r.Codigo == infraestructura.InfraestructuraCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Espacios_Intervenir)
                                        .FirstOrDefault().Nombre);
                                    RegistrosAlcance = RegistrosAlcance.Replace("[ALCANCE_CANTIDAD]", infraestructura.Cantidad.ToString());
                                }

                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, RegistrosAlcance);
                                break;

                            case ConstanCodigoVariablesPlaceHolders.PLAZO_OBRA_MESES:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.PlazoMesesObra.ToString());
                                break;

                            case ConstanCodigoVariablesPlaceHolders.PLAZO_OBRA_DIAS:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.PlazoDiasObra.ToString());
                                break;

                            case ConstanCodigoVariablesPlaceHolders.PLAZO_INTERVENTORIA_MESES:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.PlazoMesesInterventoria.ToString());
                                break;

                            case ConstanCodigoVariablesPlaceHolders.PLAZO_INTERVENTORIA_DIAS:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, proyecto.Proyecto.PlazoDiasInterventoria.ToString());
                                break;

                            case ConstanCodigoVariablesPlaceHolders.VALOR_OBRA:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, "$" + String.Format("{0:n0}", proyecto.Proyecto.ValorObra));
                                break;

                            case ConstanCodigoVariablesPlaceHolders.VALOR_INTERVENTORIA:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, "$" + String.Format("{0:n0}", proyecto.Proyecto.ValorInterventoria));
                                break;

                            case ConstanCodigoVariablesPlaceHolders.VALOR_TOTAL_PROYECTO:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, "$" + String.Format("{0:n0}", proyecto.Proyecto.ValorTotal));
                                break;

                            case ConstanCodigoVariablesPlaceHolders.REGISTROS_FUENTES_USO:
                                DetallesProyectos = DetallesProyectos.Replace(placeholderDominio.Nombre, " ");
                                break;

                            case ConstanCodigoVariablesPlaceHolders.ESTADO_OBRA:
                                EjecucionesProyecto = EjecucionesProyecto.Replace(placeholderDominio.Nombre, proyecto.EstadoObraCodigo != null ? ListaParametricas.Where(r => r.Codigo == proyecto.EstadoObraCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Obra_Avance_Semanal).FirstOrDefault().Nombre : "Sin registro de avance semanal");
                                break;

                            case ConstanCodigoVariablesPlaceHolders.PROGRAMACION_OBRA:
                                EjecucionesProyecto = EjecucionesProyecto.Replace(placeholderDominio.Nombre, proyecto.ProgramacionSemanal != null ? proyecto.ProgramacionSemanal + " %" : "0 %");
                                break;


                            case ConstanCodigoVariablesPlaceHolders.AVANCE_FISICO_ACUMULADO:
                                EjecucionesProyecto = EjecucionesProyecto.Replace(placeholderDominio.Nombre, proyecto.AvanceFisicoSemanal != null ? proyecto.AvanceFisicoSemanal + " %" : "0 %");
                                break;

                            case ConstanCodigoVariablesPlaceHolders.FACTURACION_PROGRAMADA:
                                EjecucionesProyecto = EjecucionesProyecto.Replace(placeholderDominio.Nombre, " ");
                                break;

                            case ConstanCodigoVariablesPlaceHolders.FACTURACION_EJECUTADA:
                                EjecucionesProyecto = EjecucionesProyecto.Replace(placeholderDominio.Nombre, " ");
                                break;
                        }
                    }

                    #endregion Detalle Solicitud

                }

                #region detalle
                DetallesSolicitudes += DetalleSolicitud;
                ControversiaMotivo controversiaMotivo = null;

                controversiaMotivo = _context.ControversiaMotivo.Where(r => r.ControversiaContractualId == controversiaContractual.ControversiaContractualId).FirstOrDefault();

                foreach (Dominio placeholderDominio in placeholders)
                {
                    switch (placeholderDominio.Codigo)
                    {

                        case ConstanCodigoVariablesPlaceHolders.MOTIVOS_SOLICITUD:
                            DetallesSolicitudes = DetallesSolicitudes.Replace(placeholderDominio.Nombre, controversiaMotivo != null ? controversiaMotivo.MotivoSolicitudCodigo != null ? ListaParametricas.Where(r => r.Codigo == controversiaMotivo.MotivoSolicitudCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_controversia).FirstOrDefault().Nombre : " " : " ");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.FECHA_COMITE_PRE_TECNICO:
                            DetallesSolicitudes = DetallesSolicitudes.Replace(placeholderDominio.Nombre, controversiaContractual.FechaComitePreTecnico == null ? "" : Convert.ToDateTime(controversiaContractual.FechaComitePreTecnico).ToString("dd/MM/yyyy"));
                            break;

                        case ConstanCodigoVariablesPlaceHolders.CONCLUSION_COMITE_PRE_TECNICO:
                            DetallesSolicitudes = DetallesSolicitudes.Replace(placeholderDominio.Nombre, controversiaContractual.ConclusionComitePreTecnico != null ? controversiaContractual.ConclusionComitePreTecnico : "");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.URL_SOPORTE_SOLICITUD:
                            DetallesSolicitudes = DetallesSolicitudes.Replace(placeholderDominio.Nombre, controversiaContractual.RutaSoporte != null ? controversiaContractual.RutaSoporte : "");
                            break;
                        //DIFERENTES TAI
                        case ConstanCodigoVariablesPlaceHolders.FECHA_RADICADO_SAC:
                            DetallesSolicitudes = DetallesSolicitudes.Replace(placeholderDominio.Nombre, controversiaContractual.NumeroRadicadoSac != null ? controversiaContractual.NumeroRadicadoSac : "");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.RESUMEN_JUSTIFICACION_SOLICITUD:
                            DetallesSolicitudes = DetallesSolicitudes.Replace(placeholderDominio.Nombre, controversiaContractual.MotivoJustificacionRechazo != null ? controversiaContractual.MotivoJustificacionRechazo : "");
                            break;
                    }
                }

                #endregion

                #region historial

                foreach (var novedad in novedadContractual)
                {
                    //adicion
                    NovedadesAdicion = "";
                    //prorroga
                    NovedadesProrroga = "";
                    //modificacion contractal
                    NovedadesModificacion = "";
                    //otras
                    NovedadesOtras = "";
                    //APORTANTES
                    Aportantes = "";

                    if (novedad.Eliminado == null || novedad.Eliminado == false)
                    {
                        SesionComiteSolicitud sesionComiteSolicitud = _context.SesionComiteSolicitud
                                        .Where(r => r.SolicitudId == novedad.NovedadContractualId && r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Novedad_Contractual && (r.Eliminado == false || r.Eliminado == null))
                                        .FirstOrDefault();
                        ComiteTecnico comiteTecnico = new ComiteTecnico();
                        ComiteTecnico comiteFiduciario = new ComiteTecnico();

                        if (sesionComiteSolicitud != null)
                        {
                            comiteTecnico = _context.ComiteTecnico.Find(sesionComiteSolicitud.ComiteTecnicoId);
                            comiteFiduciario = _context.ComiteTecnico.Find(sesionComiteSolicitud.ComiteTecnicoFiduciarioId);
                        }
                        string tipoNovedadString = string.Empty;

                        //Se crear una nueva plantilla por cada vez que entra
                        Historiales += HistorialModificaciones;
                        NovedadesAdicion += NovedadAdicion;
                        NovedadesProrroga += NovedadProrroga;
                        NovedadesModificacion += NovedadModificacion;
                        string numeroComiteTecnico = comiteTecnico != null ? comiteTecnico.NumeroComite : string.Empty;
                        string numeroComiteFiduciario = comiteFiduciario != null ? comiteFiduciario.NumeroComite : string.Empty;
                        string estado = sesionComiteSolicitud != null ? ListaParametricas.Where(r => r.Codigo == sesionComiteSolicitud.EstadoCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Sesion_Comite_Solicitud).FirstOrDefault().Nombre : string.Empty;
                        bool existeAdicion = false;
                        bool existeProrroga = false;
                        bool existeModificacion = false;
                        bool existeOtro = false;

                        List<NovedadContractualDescripcion> novedadContractualDescripcion = _context.NovedadContractualDescripcion.Where(r => r.NovedadContractualId == novedad.NovedadContractualId).ToList();
                        foreach (var item in novedadContractualDescripcion)
                        {
                            if (item.Eliminado == null || novedad.Eliminado == false)
                            {
                                string codigotipoNovedadTemp = item.TipoNovedadCodigo;
                                string tipoNovedadTemp = ListaParametricas.Where(r => r.Codigo == item.TipoNovedadCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Novedad_Modificacion_Contractual).FirstOrDefault().Nombre;

                                if (String.IsNullOrEmpty(tipoNovedadString))
                                {
                                    tipoNovedadString = tipoNovedadTemp;
                                }
                                else
                                {
                                    tipoNovedadString = tipoNovedadString + ", " + tipoNovedadTemp;
                                }

                                if (codigotipoNovedadTemp != ConstanTiposNovedades.Adición && codigotipoNovedadTemp != ConstanTiposNovedades.Prórroga && codigotipoNovedadTemp != ConstanTiposNovedades.Modificación_de_Condiciones_Contractuales)
                                {
                                    existeOtro = true;
                                    NovedadesOtras = NovedadesOtras.Replace("[TP_PLAZO_SOLICITADO]", "");
                                    NovedadesOtras = NovedadesOtras.Replace("[TP_FECHA_INICIO]", item.FechaInicioSuspension != null ? ((DateTime)item.FechaInicioSuspension).ToString("dd-MM-yyyy") : " ");
                                    NovedadesOtras = NovedadesOtras.Replace("[TP_FECHA_FIN]", item.FechaFinSuspension != null ? ((DateTime)item.FechaFinSuspension).ToString("dd-MM-yyyy") : " ");
                                }
                                if (codigotipoNovedadTemp == ConstanTiposNovedades.Adición)
                                {
                                    existeAdicion = true;
                                    NovedadesAdicion = NovedadesAdicion.Replace("[TP_PLAZO_SOLICITADO]", item.PresupuestoAdicionalSolicitado != null ? "$" + String.Format("{0:n0}", item.PresupuestoAdicionalSolicitado) : string.Empty);
                                    Aportantes = Aportantes + Aportante;

                                    #region Aportantes

                                    int enumAportante = 1;

                                    string strNombreAportante = string.Empty;
                                    string ValorAportante = string.Empty;
                                    string strComponente = string.Empty;
                                    string strFase = string.Empty;
                                    string strTipoUso = string.Empty;
                                    string valorUso = string.Empty;

                                    foreach (var aportante in novedad.NovedadContractualAportante)
                                    {
                                        strNombreAportante = aportante.NombreAportante != null ? aportante.NombreAportante : "";
                                        ValorAportante = aportante.ValorAporte != null ? "$" + String.Format("{0:n0}", aportante.ValorAporte) : "";
                                        strComponente = aportante.ComponenteAportanteNovedad.Count > 0 ? aportante.ComponenteAportanteNovedad.FirstOrDefault().NombreTipoComponente : "";
                                        strFase = aportante.ComponenteAportanteNovedad.Count > 0 ? aportante.ComponenteAportanteNovedad.FirstOrDefault().Nombrefase : "";
                                        strTipoUso = aportante.ComponenteAportanteNovedad.FirstOrDefault().ComponenteFuenteNovedad.FirstOrDefault().ComponenteUsoNovedad.Count() > 0 ? aportante.ComponenteAportanteNovedad.FirstOrDefault().ComponenteFuenteNovedad.FirstOrDefault().ComponenteUsoNovedad.FirstOrDefault().NombreUso : "";
                                        valorUso = aportante.ComponenteAportanteNovedad.FirstOrDefault().ComponenteFuenteNovedad.FirstOrDefault().ComponenteUsoNovedad.Count() > 0 ? "$" + String.Format("{0:n0}", aportante.ComponenteAportanteNovedad.FirstOrDefault().ComponenteFuenteNovedad.FirstOrDefault().ComponenteUsoNovedad.FirstOrDefault().ValorUso) : "";
                                        Aportantes = Aportantes.Replace("[TP_NUMERO_APORTANTE]", (enumAportante++).ToString())
                                                                .Replace("[TP_NOMBRE_APORTANTE]", strNombreAportante)
                                                                .Replace("[TP_VALOR_APORTANTE]", ValorAportante)
                                                                .Replace("[TP_FUENTE]", "")
                                                                .Replace("[TP_FASE]", strFase)
                                                                .Replace("[TP_COMPONENTE]", strComponente)
                                                                .Replace("[TP_USO]", strTipoUso)
                                                                .Replace("[TP_VALOR_USO]", valorUso);
                                    }

                                    if (novedad.NovedadContractualAportante.Count > 0)
                                    {
                                        NovedadesAdicion = NovedadesAdicion.Replace("[TP_APORTANTE]", Aportantes);
                                    }
                                    else
                                    {
                                        NovedadesAdicion = NovedadesAdicion.Replace("[TP_APORTANTE]", "");

                                    }

                                    #endregion
                                }
                                if (codigotipoNovedadTemp == ConstanTiposNovedades.Prórroga)
                                {
                                    existeProrroga = true;
                                    NovedadesProrroga = NovedadesProrroga.Replace("[TP_PLAZO_DIAS]", item.PlazoAdicionalDias != null ? item.PlazoAdicionalDias.ToString() : "");
                                    NovedadesProrroga = NovedadesProrroga.Replace("[TP_PLAZO_MESES]", item.PlazoAdicionalMeses != null ? item.PlazoAdicionalMeses.ToString() : "");
                                }

                                if (codigotipoNovedadTemp == ConstanTiposNovedades.Modificación_de_Condiciones_Contractuales)
                                {
                                    existeModificacion = true;
                                    enumClausula = 1;

                                    foreach (var clausula in item.NovedadContractualClausula)
                                    {
                                        if (clausula != null)
                                        {
                                            NovedadesModificacion = NovedadesModificacion.Replace("[TP_NUM_CLAUSULA]", (enumClausula++).ToString());
                                            NovedadesModificacion = NovedadesModificacion.Replace("[TP_CLAUSULA]", clausula.ClausulaAmodificar != null ? clausula.ClausulaAmodificar : string.Empty);
                                            NovedadesModificacion = NovedadesModificacion.Replace("[TP_AJUSTE_CLAUSULA]", clausula.AjusteSolicitadoAclausula != null ? clausula.AjusteSolicitadoAclausula : string.Empty);
                                        }
                                    }
                                }
                            }
                        }

                        foreach (Dominio placeholderDominio in placeholders)
                        {
                            switch (placeholderDominio.Codigo)
                            {

                                case ConstanCodigoVariablesPlaceHolders.NOMBRE_MODIFICACION:
                                    Historiales = Historiales.Replace(placeholderDominio.Nombre, (enumHistorial++).ToString());
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.NUMERO_SOLICITUD:
                                    Historiales = Historiales.Replace(placeholderDominio.Nombre, novedad.NumeroSolicitud);
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.TIPO_NOVEDAD:
                                    Historiales = Historiales.Replace(placeholderDominio.Nombre, tipoNovedadString);
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.TP_NUMERO_COMITE_TECNICO:
                                    Historiales = Historiales.Replace(placeholderDominio.Nombre, numeroComiteTecnico);
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.TP_NUMERO_COMITE_FIDUCIARIO:
                                    Historiales = Historiales.Replace(placeholderDominio.Nombre, numeroComiteFiduciario);
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.TP_ESTADO:
                                    Historiales = Historiales.Replace(placeholderDominio.Nombre, estado);
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.TIPO_NOVEDAD_OTRAS:
                                    if (existeOtro)
                                    {
                                        Historiales = Historiales.Replace(placeholderDominio.Nombre, NovedadesOtras);
                                    }
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.TIPO_NOVEDAD_ADICION:
                                    if (existeAdicion)
                                    {
                                        Historiales = Historiales.Replace(placeholderDominio.Nombre, NovedadesAdicion);
                                    }
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.TIPO_NOVEDAD_PRORROGA:
                                    if (existeProrroga)
                                    {
                                        Historiales = Historiales.Replace(placeholderDominio.Nombre, NovedadesProrroga);
                                    }
                                    break;

                                case ConstanCodigoVariablesPlaceHolders.TIPO_NOVEDAD_MODIFICACION:
                                    if (existeModificacion)
                                    {
                                        Historiales = Historiales.Replace(placeholderDominio.Nombre, NovedadesModificacion);
                                    }
                                    break;
                            }
                        }
                    }
                }

                #endregion

                #region Historial actuaciones

                //Historial de Actuaciones
                int contadorActuacion = 1;
                int contadorReclamacionMesas = 1;
                int contadorReclamacion = 1;
                //List<Dominio> listaDominioActuacionActualizada = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Actuacion_adelantada).ToList();
                List<Dominio> listaDominioActuacionActualizada = new List<Dominio>();

                if (controversiaContractual.TipoControversiaCodigo == ConstanCodigoTipoControversia.Terminacion_anticipada_por_incumplimiento_TAI)
                {
                    listaDominioActuacionActualizada = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_controversia_contractual_TAI).ToList();
                }
                else
                {
                    listaDominioActuacionActualizada = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_Actuacion_No_TAI).ToList();
                }

                foreach (var actuacion in controversiasActuacion)
                {
                    HistorialesAct += HistorialActuaciones;

                    HistorialesAct = HistorialesAct.Replace("_contador_", contadorActuacion.ToString());
                    HistorialesAct = HistorialesAct.Replace("_Fecha_Actualizacion_Actuacion_", actuacion.FechaActuacion != null ? Convert.ToDateTime(actuacion.FechaActuacion).ToString("dd/MM/yyyy") : "");
                    HistorialesAct = HistorialesAct.Replace("_Numero_Actuacion_", string.IsNullOrEmpty(actuacion.NumeroActuacion) ? "" : actuacion.NumeroActuacion);
                    HistorialesAct = HistorialesAct.Replace("_Actuacion_", string.IsNullOrEmpty(actuacion.ActuacionAdelantadaCodigo) ? "" : listaDominioActuacionActualizada.Where(r => r.Codigo == actuacion.ActuacionAdelantadaCodigo)?.FirstOrDefault()?.Nombre);

                    if (controversiaContractual.TipoControversiaCodigo == ConstanCodigoTipoControversia.Terminacion_anticipada_por_incumplimiento_TAI)
                    {
                        if (actuacion.ActuacionAdelantadaCodigo == ConstanCodigoActuacionAdelantada.RemisiondeComunicaciondedecisiondeTAIporAlianzaFiduciariaalaAseguradora)
                        {
                            #region Historial Reclamaciones  
                            
                            HistorialesReclamacion = "";
                            int contadorReclamacionActuacion = 1;

                            foreach (var reclamacion in actuacion.ActuacionSeguimiento)
                            {
                                HistorialesReclamacion += HistorialReclamacion;

                                HistorialesReclamacion = HistorialesReclamacion.Replace("_contador_", contadorReclamacionActuacion.ToString());
                                HistorialesReclamacion = HistorialesReclamacion.Replace("_Fecha_Actualizacion_Actuacion_", reclamacion.FechaActuacionAdelantada != null ? Convert.ToDateTime(reclamacion.FechaActuacionAdelantada).ToString("dd/MM/yyyy") : "");
                                HistorialesReclamacion = HistorialesReclamacion.Replace("_Numero_Actuacion_", string.IsNullOrEmpty(reclamacion.NumeroActuacionReclamacion) ? "" : reclamacion.NumeroActuacionReclamacion);
                                HistorialesReclamacion = HistorialesReclamacion.Replace("_Actuacion_", string.IsNullOrEmpty(reclamacion.ActuacionAdelantada) ? "" : reclamacion.ActuacionAdelantada);
                                //historial de reclamaciones * controversia
                                contadorReclamacionActuacion++;

                            }

                            HistorialesRecMesas += HistorialReclamacionesMesasTrabajo;
                            
                            string numero = !string.IsNullOrEmpty(actuacion.NumeroActuacionReclamacion) ? actuacion.NumeroActuacionReclamacion : "";
                            HistorialesRecMesas = HistorialesRecMesas.Replace("_contador_", contadorReclamacion.ToString() + " - " + numero);
                            HistorialesRecMesas = HistorialesRecMesas.Replace("[resumenPropuestaFiduciaria]", !string.IsNullOrEmpty(actuacion.ResumenPropuestaFiduciaria) ? actuacion.ResumenPropuestaFiduciaria : "");
                            HistorialesRecMesas = HistorialesRecMesas.Replace("[esRequiereComiteReclamacion]", actuacion.EsRequiereComiteReclamacion == null ? "" : actuacion.EsRequiereComiteReclamacion == true ? "Sí" : "No");
                            HistorialesRecMesas = HistorialesRecMesas.Replace("[urlSoporte]", string.IsNullOrEmpty(actuacion.RutaSoporte) ? "" : actuacion.RutaSoporte);

                            HistorialesRecMesas = HistorialesRecMesas.Replace("[HISTORIAL]", HistorialesReclamacion);


                            #endregion
                            contadorReclamacion++;
                        }
                    }
                    else
                    {
                        #region Historial Mesas de trabajo                

                        foreach (var mesa in actuacion.ControversiaActuacionMesa)
                        {
                            HistorialesRecMesas += HistorialReclamacionesMesasTrabajo;

                            HistorialesRecMesas = HistorialesRecMesas.Replace("_contador_", contadorReclamacionMesas.ToString() );
                            HistorialesRecMesas = HistorialesRecMesas.Replace("_Fecha_Actualizacion_Actuacion_", mesa.FechaActuacionAdelantada != null ? Convert.ToDateTime(mesa.FechaActuacionAdelantada).ToString("dd/MM/yyyy") : "");
                            HistorialesRecMesas = HistorialesRecMesas.Replace("_Numero_Actuacion_", string.IsNullOrEmpty(mesa.NumeroMesaTrabajo) ? "" : mesa.NumeroMesaTrabajo);
                            HistorialesRecMesas = HistorialesRecMesas.Replace("_Actuacion_", string.IsNullOrEmpty(mesa.ActuacionAdelantada) ? "" : mesa.ActuacionAdelantada);
                            HistorialesRecMesas = HistorialesRecMesas.Replace("_Url_", string.IsNullOrEmpty(mesa.RutaSoporte) ? "" : mesa.RutaSoporte);
                            
                            int contadorActuacionMesa = 1;
                            ActuacionesMesa = "";
                            foreach (var actuacionMesa in mesa.ControversiaActuacionMesaSeguimiento)
                            {
                                ActuacionesMesa += ActuacionMesa;

                                ActuacionesMesa = ActuacionesMesa.Replace("_contador_", contadorActuacionMesa.ToString());
                                ActuacionesMesa = ActuacionesMesa.Replace("_Fecha_Actualizacion_Actuacion_", actuacionMesa.FechaCreacion != null ? Convert.ToDateTime(actuacionMesa.FechaCreacion).ToString("dd/MM/yyyy") : "");
                                ActuacionesMesa = ActuacionesMesa.Replace("_Numero_Actuacion_", string.IsNullOrEmpty(actuacionMesa.NumeroActuacionSeguimiento) ? "" : actuacionMesa.NumeroActuacionSeguimiento);
                                ActuacionesMesa = ActuacionesMesa.Replace("_Actuacion_", string.IsNullOrEmpty(actuacionMesa.ActuacionAdelantada) ? "" : actuacionMesa.ActuacionAdelantada);
                                contadorActuacionMesa++;

                            }
                            HistorialesRecMesas = HistorialesRecMesas.Replace("_ActuacionMesa_", string.IsNullOrEmpty(ActuacionesMesa) ? "" : ActuacionesMesa);

                            //historial de reclamaciones * controversia
                            contadorReclamacionMesas++;

                        }

                        #endregion
                    }
                    contadorActuacion++;

                }

                #endregion

                #region fuentes usos

                foreach (Dominio placeholderDominio in placeholders)
                {
                    //ConstanCodigoVariablesPlaceHolders placeholder = (ConstanCodigoVariablesPlaceHolders)placeholderDominio.Codigo.ToString();

                    switch (placeholderDominio.Codigo)
                    {

                        case ConstanCodigoVariablesPlaceHolders.NUMERO_SOLICITUD:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, controversiaContractual.NumeroSolicitud);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.FECHA_SOLICITUD:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, controversiaContractual.FechaSolicitud != null ? ((DateTime)controversiaContractual.FechaSolicitud).ToString("dd-MM-yyyy") : " ");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.TIPO_CONTROVERSIA:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, ListaParametricas.Where(r => r.Codigo == controversiaContractual.TipoControversiaCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipos_Controversia).FirstOrDefault().Nombre);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.CANTIDAD_DE_PROYECTOS_ASOCIADOS:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, contratacion != null ? contratacion.ContratacionProyecto.Count().ToString() : "");
                            break;
                        //Datos Contratista y contrato
                        case ConstanCodigoVariablesPlaceHolders.NUMERO_CONTRATO:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, contrato != null ? contrato.NumeroContrato : "");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.CONTRATISTA_NOMBRE:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, contratista != null ? contratista.Nombre : "");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.FECHA_INICIO_CONTRATO:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, contrato != null ? ((DateTime)contrato.FechaCreacion).ToString("dd-MM-yyyy") : " ");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.FECHA_FIN_CONTRATO:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, contrato != null ? contrato.FechaTerminacionFase2 != null ? ((DateTime)contrato.FechaTerminacionFase2).ToString("dd-MM-yyyy") : " " : " ");
                            break;

                        //
                        case ConstanCodigoVariablesPlaceHolders.DETALLES_PROYECTOS:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, DetallesProyectos);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.EJECUCION_PROYECTO:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, EjecucionesProyecto);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.DETALLE_SOLICITUD_TAI:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, DetallesSolicitudes);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.HISTORIAL_MODIFICACIONES:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, Historiales);
                            break;

                        case ConstanCodigoVariablesPlaceHolders.HISTORIAL_ACTUACIONES:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, controversiasActuacion.Count() > 0 ? HistorialesAct : "");
                            break;

                        case ConstanCodigoVariablesPlaceHolders.HISTORIAL_RECLAMACIONES:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre, 
                                controversiaContractual.TipoControversiaCodigo != ConstanCodigoTipoControversia.Terminacion_anticipada_por_incumplimiento_TAI && controversiasActuacion.FirstOrDefault().ControversiaActuacionMesa.Count() > 0 ? HistorialesRecMesas :
                                controversiaContractual.TipoControversiaCodigo == ConstanCodigoTipoControversia.Terminacion_anticipada_por_incumplimiento_TAI && controversiasActuacion.FirstOrDefault().ActuacionSeguimiento.Count() > 0 ? HistorialesRecMesas : "");
                            break;
                        case ConstanCodigoVariablesPlaceHolders.VAR_NOMBRE:
                            pPlantilla = pPlantilla.Replace(placeholderDominio.Nombre,
                                controversiaContractual.TipoControversiaCodigo != ConstanCodigoTipoControversia.Terminacion_anticipada_por_incumplimiento_TAI ? "Mesas de trabajo" :
                                controversiaContractual.TipoControversiaCodigo == ConstanCodigoTipoControversia.Terminacion_anticipada_por_incumplimiento_TAI ? "Reclamaciones" : "Reclamaciones");
                            break;
                            
                    }
                }

                #endregion fuentes usos

                return pPlantilla;
            }
            catch (Exception ex)
            {
                return ex.InnerException.ToString();
            }
        }

    }
}
