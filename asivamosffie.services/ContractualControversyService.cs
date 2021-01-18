using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using asivamosffie.services.Interfaces;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Helpers.Constant;
using Microsoft.AspNetCore.Http;
using asivamosffie.services.Helpers.Constants;
using System.IO;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace asivamosffie.services
{
    public class ContractualControversyService : IContractualControversy
    {

        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;
        private readonly IConverter _converter;

        public ContractualControversyService(devAsiVamosFFIEContext context, ICommonService commonService, IConverter converter)
        {

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
                if (controversiaActuacion != null)
                {

                    if (string.IsNullOrEmpty(controversiaActuacion.ControversiaActuacionId.ToString()) || controversiaActuacion.ControversiaActuacionId == 0)
                    {
                        strCrearEditar = "REGISTRAR CONTROVERSIA ACTUACION";

                        //Auditoria
                        strCrearEditar = "REGISTRAR AVANCE COMPROMISOS";
                        controversiaActuacion.FechaCreacion = DateTime.Now;
                        //controversiaActuacion.UsuarioCreacion = compromisoSeguimiento.UsuarioCreacion;
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
                            Data=controversiaActuacion,
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
            string prefijo="";
            Contrato contrato=null;

            ControversiaContractual controversiaContractual = null;
            controversiaContractual = await _context.ControversiaContractual.FindAsync(id);
            
            if(controversiaContractual != null)
                contrato = await _context.Contrato.FindAsync(controversiaContractual.ContratoId);

            if (contrato != null)
            {
                if (contrato.TipoContratoCodigo == ConstanCodigoTipoContrato.Obra)
                    prefijo = ConstanPrefijoNumeroSolicitudControversia.Obra;
                else if (contrato.TipoContratoCodigo == ConstanCodigoTipoContrato.Interventoria)
                    prefijo = ConstanPrefijoNumeroSolicitudControversia.Interventoria;
            }
            
            //controversiaContractual.NumeroSolicitudFormat = prefijo + controversiaContractual.ControversiaContractualId.ToString("000");
            return controversiaContractual;
        }

        public async Task<ControversiaActuacion> GetControversiaActuacionById(int id)
        {
            ControversiaActuacion controversiaActuacion = null;
            controversiaActuacion =  _context.ControversiaActuacion.Where(x=>x.ControversiaActuacionId== id).
                Include(x=>x.ControversiaContractual).
                    ThenInclude(x=>x.Contrato).
                Include(x=>x.SeguimientoActuacionDerivada).FirstOrDefault();
            controversiaActuacion.NumeroActuacionFormat = "ACT controversia " + controversiaActuacion.ControversiaActuacionId.ToString("000");

            var estado = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaActuacion.EstadoActuacionReclamacionCodigo, (int)EnumeratorTipoDominio.Estados_Reclamacion);
            var vTipoControversiaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaActuacion.ControversiaContractual.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipo_de_controversia);
            controversiaActuacion.NumeroContrato = controversiaActuacion.ControversiaContractual.Contrato.NumeroContrato;

            controversiaActuacion.TipoControversia = vTipoControversiaCodigo == null ? "" : vTipoControversiaCodigo.Nombre;
            controversiaActuacion.SeguimientoActuacionDerivada = controversiaActuacion.SeguimientoActuacionDerivada.Where(x => !(bool)x.Eliminado).ToList();
            controversiaActuacion.EstadoActuacionReclamacionString = estado == null ? "" : estado.Nombre;
            foreach (var cont in controversiaActuacion.SeguimientoActuacionDerivada)
            {
                var estadostring = await _commonService.GetDominioByNombreDominioAndTipoDominio(cont.EstadoActuacionDerivadaCodigo, (int)EnumeratorTipoDominio.Estado_Actuacion_Derivada_r_4_4_1);
                cont.EstadoActuacionString = estadostring == null ? "" : estadostring.Nombre;
                
            }

            controversiaActuacion.ObservacionesComites = _context.SesionComiteSolicitud.Where(x => x.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Actuaciones_Controversias_Contractuales &&
                  !(bool)x.Eliminado && x.SolicitudId == controversiaActuacion.ControversiaActuacionId).Select(y => y.Observaciones).ToList();
            return controversiaActuacion;
        }

        public async Task<ActuacionSeguimiento> GetActuacionSeguimientoById(int id)
        {
            //ActuacionSeguimiento actuacionSeguimiento
            ActuacionSeguimiento actuacionSeguimiento = null;
            actuacionSeguimiento = _context.ActuacionSeguimiento.Where(x=>x.ActuacionSeguimientoId==id).
                Include(x=>x.ControversiaActuacion).                
                ThenInclude(x => x.ControversiaContractual).
                ThenInclude(x => x.Contrato).
                Include(x => x.ControversiaActuacion).
                ThenInclude(x => x.SeguimientoActuacionDerivada)                
                .FirstOrDefault();

            actuacionSeguimiento.NumeroReclamacion = "REC " + actuacionSeguimiento.ActuacionSeguimientoId.ToString("0000");
            //    NumeroActuacionFormat = "ACT controversia " + actuacionSeguimiento.ActuacionSeguimientoId.ToString("0000"),
            var vTipoControversiaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(actuacionSeguimiento.ControversiaActuacion.ControversiaContractual.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipo_de_controversia);
            actuacionSeguimiento.NumeroContrato = actuacionSeguimiento.ControversiaActuacion.ControversiaContractual.Contrato.NumeroContrato;

            actuacionSeguimiento.TipoControversia = vTipoControversiaCodigo == null ? "" : vTipoControversiaCodigo.Nombre;
            actuacionSeguimiento.ControversiaActuacion.SeguimientoActuacionDerivada = actuacionSeguimiento.ControversiaActuacion.SeguimientoActuacionDerivada.Where(x=>!(bool)x.Eliminado).ToList();
            foreach (var cont in actuacionSeguimiento.ControversiaActuacion.SeguimientoActuacionDerivada)
            {
                var estadostring = await _commonService.GetDominioByNombreDominioAndTipoDominio(cont.EstadoActuacionDerivadaCodigo, (int)EnumeratorTipoDominio.Estado_Actuacion_Derivada_r_4_4_1);
                cont.EstadoActuacionString = estadostring == null ? "" : estadostring.Nombre;
            }

            return actuacionSeguimiento;
        }

        public async Task<List<ControversiaMotivo>> GetMotivosSolicitudByControversiaContractualId(int id)
        {

            return await _context.ControversiaMotivo.Where(r=>r.ControversiaContractualId==id).ToListAsync();
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
                      
            Plantilla plantilla = null;
        

            //else if (contrato.TipoContratoCodigo == ((int)ConstanCodigoTipoContratacion.Obra).ToString())
                        
                plantilla = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Controversia_Contractual).ToString()).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();
                        

            //Plantilla plantilla = new Plantilla();
            //plantilla.Contenido = "";
            if (plantilla != null)
                plantilla.Contenido = await ReemplazarDatosPlantillaControversiaContractual(plantilla.Contenido, pControversiaContractualID, "cdaza");
            return ConvertirPDF(plantilla);
        }

        private async Task<string> ReemplazarDatosPlantillaControversiaContractual(string strContenido, int pControversiaContractualID, string usuario)
        {
            string str = "";
            string valor = "";


            //Contrato contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();
            ControversiaContractual controversiaContractual = null;

            ControversiaMotivo controversiaMotivo = null;
            ControversiaActuacion controversiaActuacion = null;

            ActuacionSeguimiento actuacionSeguimiento = null;
            controversiaContractual = _context.ControversiaContractual
                   .Where(r => r.ControversiaContractualId == pControversiaContractualID).Include(x=>x.Contrato).FirstOrDefault();

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

                novedadContractual = _context.NovedadContractual
                    .Where(r => r.SolicitudId == controversiaContractual.SolicitudId).FirstOrDefault();

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

            TipoControversiaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaContractual.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipo_de_controversia);
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

            if (controversiaContractual != null) {
                strContenido = strContenido.Replace("_Numero_Solicitud_", controversiaContractual.NumeroSolicitud);
                strContenido = strContenido.Replace("_Fecha_Solicitud_", controversiaContractual.FechaSolicitud==null?"": Convert.ToDateTime(controversiaContractual.FechaSolicitud).ToString("dd/MM/yyyy")); }

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

            if (controversiaContractual != null) {
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
                strContenido = strContenido.Replace("_Tipo_Novedad_", novedadContractual.TipoNovedadCodigo);

                strContenido = strContenido.Replace(">>>>> SECCION_SUSPENSION_PRORROGA_REINICIO >>>>>>>>", "");
                strContenido = strContenido.Replace("_Plazo_solicitado_", Convert.ToInt32(novedadContractual.PlazoAdicionalMeses).ToString());
                //strContenido = strContenido.Replace("_Plazo_solicitado_", novedadContractual.PlazoAdicionalDias);

                fechaNull = novedadContractual != null ? novedadContractual.FechaInicioSuspension : null;
                //strContenido = strContenido.Replace("_Fecha_Inicio_", novedadContractual.FechaInicioSuspension != null ? Convert.ToDateTime(novedadContractual.FechaInicioSuspension).ToString("dd/MM/yyyy") : novedadContractual.FechaInicioSuspension.ToString());
                strContenido = strContenido.Replace("_Fecha_Inicio_", fechaNull == null ? "" : Convert.ToDateTime(fechaNull).ToString("dd/MM/yyyy"));

                fechaNull = novedadContractual != null ? novedadContractual.FechaFinSuspension : null;
                //strContenido = strContenido.Replace("_Fecha_Fin_", novedadContractual.FechaFinSuspension != null ? Convert.ToDateTime(novedadContractual.FechaFinSuspension).ToString("dd/MM/yyyy") : novedadContractual.FechaFinSuspension.ToString());
                strContenido = strContenido.Replace("_Fecha_Fin_", fechaNull == null ? "" : Convert.ToDateTime(fechaNull).ToString("dd/MM/yyyy"));
                //ContratacionProyecto ComiteTecnicoProyecto  ComiteTecnico
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

            if (novedadContractual != null)
            {
                strContenido = strContenido.Replace("_Clausula_modificar_", novedadContractual.ClausulaModificar);
                strContenido = strContenido.Replace("_Ajuste_solicitado_clausula_", novedadContractual.AjusteClausula);
            }
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
                EstadoAvanceTramiteCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaActuacion.EstadoAvanceTramiteCodigo, (int)EnumeratorTipoDominio.Tipo_de_controversia);
                if (EstadoAvanceTramiteCodigo != null)
                {
                    //strTipoControversia = MotivoSolicitudCodigo.Nombre;
                    EstadoAvanceTramiteCodigoNombre = EstadoAvanceTramiteCodigo.Nombre;

                }

                ActuacionAdelantadaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaActuacion.ActuacionAdelantadaCodigo, (int)EnumeratorTipoDominio.Tipo_de_controversia);
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
            if(actuacionSeguimiento!=null)
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

        public async Task<List<Contrato>> GetListContratos(/*int id*/)
        {
            List<Contrato> ListContratos = new List<Contrato>();

            try
            {
                ListContratos = await _context.Contrato.Where(r => !(bool)r.Eliminado && (r.EstadoActa=="5"|| r.EstadoActa == "20")).ToListAsync();//no encontré el 20 en constante

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

        public async Task<Respuesta> CambiarEstadoActuacionSeguimiento(int pActuacionSeguimientoId, string pEstadoReclamacionCodigo, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Actuacion_Seguimiento, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ControversiaActuacion actuacionSeguimientoOld;

                actuacionSeguimientoOld = _context.ControversiaActuacion.Find(pActuacionSeguimientoId);
                actuacionSeguimientoOld.UsuarioModificacion = pUsuarioModifica;
                actuacionSeguimientoOld.FechaModificacion = DateTime.Now;
                actuacionSeguimientoOld.EstadoAvanceTramiteCodigo = pEstadoReclamacionCodigo;

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
                    .Include(c=>c.ControversiaActuacion).ThenInclude(x=>x.ControversiaActuacionMesa).FirstOrDefault();

                //confirmo que no tenga actuaciones

                if(controversiaContractual.ControversiaActuacion.Count()>0)
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
            if (string.IsNullOrEmpty(controversiaActuacion.EstadoAvanceTramiteCodigo)
             || string.IsNullOrEmpty(controversiaActuacion.ActuacionAdelantadaCodigo)
            || string.IsNullOrEmpty(controversiaActuacion.CantDiasVencimiento.ToString())
                || (controversiaActuacion.EsRequiereContratista == null)
                || (controversiaActuacion.EsRequiereInterventor == null)
              // || (controversiaActuacion.EsRequiereJuridico == null)
                || (controversiaActuacion.EsRequiereSupervisor == null))
            {
                return false;
            }

            return true;
        }

        private bool ValidarRegistroCompletoControversiaActuacionSeguimiento(ActuacionSeguimiento controversiaActuacion)
        {
            if ((controversiaActuacion.CantDiasVencimiento)==0
             || (controversiaActuacion.EsResultadoDefinitivo==null)
            || (controversiaActuacion.FechaActuacionAdelantada==null)
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
            //RutaSoporte
            //    EsRequiereComite

            //    MotivoJustificacionRechazo
            //    EsProcede






            if (string.IsNullOrEmpty(controversiaContractual.ConclusionComitePreTecnico)
             || string.IsNullOrEmpty(controversiaContractual.FechaComitePreTecnico.ToString())
            || string.IsNullOrEmpty(controversiaContractual.FechaSolicitud.ToString())
                )
            {
                return false;
            }

            return true;
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

                        //Auditoria
                        //strCrearEditar = "REGISTRAR AVANCE COMPROMISOS";
                        actuacionSeguimiento.FechaCreacion = DateTime.Now;
                        //controversiaActuacion.UsuarioCreacion = compromisoSeguimiento.UsuarioCreacion;                             

                        actuacionSeguimiento.Observaciones = Helpers.Helpers.CleanStringInput(actuacionSeguimiento.Observaciones);

                        actuacionSeguimiento.RegistroCompleto = ValidarRegistroCompletoControversiaActuacionSeguimiento(actuacionSeguimiento);
                        actuacionSeguimiento.Eliminado = false;                        
                        _context.ActuacionSeguimiento.Add(actuacionSeguimiento);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        strCrearEditar = "EDITAR ACTUACION SEGUIMIENTO";
                        ActuacionSeguimiento actuacionSeguimientoBD = null;
                        actuacionSeguimientoBD = _context.ActuacionSeguimiento.Where(r => r.ActuacionSeguimientoId == actuacionSeguimiento.ActuacionSeguimientoId).FirstOrDefault();

                        actuacionSeguimiento.Observaciones = Helpers.Helpers.CleanStringInput(actuacionSeguimiento.Observaciones);
                        //actuacionSeguimiento.ConclusionComitePreTecnico = Helpers.Helpers.CleanStringInput(actuacionSeguimiento.ConclusionComitePreTecnico);
                        //ControversiaContractual.FechaCreacion = DateTime.Now;
                        //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;
                        //contratoPoliza.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;

                        actuacionSeguimientoBD.RegistroCompleto = ValidarRegistroCompletoControversiaActuacionSeguimiento(actuacionSeguimientoBD);
                        actuacionSeguimientoBD.FechaModificacion = DateTime.Now;
                        //_context.ContratoPoliza.Add(contratoPoliza);
                        _context.ActuacionSeguimiento.Update(actuacionSeguimientoBD);
                        await _context.SaveChangesAsync();
                    }

                    return
                        new Respuesta
                        {   Data=actuacionSeguimiento,
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
                        contrato = _context.Contrato.Where(x=>x.ContratoId==controversiaContractual.ContratoId).Include(x=>x.Contratacion).FirstOrDefault();

                        if (contrato != null)
                        {
                            if (contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Obra)
                                prefijo = ConstanPrefijoNumeroSolicitudControversia.Obra;
                            else if (contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Interventoria)
                                prefijo = ConstanPrefijoNumeroSolicitudControversia.Interventoria;
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
                        controversiaeditar.FechaComitePreTecnico= controversiaContractual.FechaComitePreTecnico;
                        controversiaeditar.EsProcede= controversiaContractual.EsProcede;
                        //NumeroRadicadoSac { get; set; }
                        controversiaeditar.EsRequiereComite = controversiaContractual.EsRequiereComite;
                        controversiaeditar.RutaSoporte = controversiaContractual.RutaSoporte;
                        controversiaeditar.EstadoCodigo = controversiaContractual.EstadoCodigo;
                        controversiaeditar.FechaModificacion = DateTime.Now;
                        controversiaeditar.UsuarioModificacion = controversiaContractual.UsuarioModificacion;
                        controversiaeditar.EsCompleto = ValidarRegistroCompletoControversiaContractual(controversiaeditar);
                        //_context.ContratoPoliza.Add(contratoPoliza);
                        _context.ControversiaContractual.Update(controversiaeditar);
                        await _context.SaveChangesAsync();
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


            public async Task<VistaContratoContratista> GetVistaContratoContratista(int pContratoId=0)
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
                            contratacion = _context.Contratacion.Where(r => r.ContratacionId == contrato.ContratacionId).FirstOrDefault();

                            FechaFinContratoTmp = contrato.FechaTerminacionFase2 != null ? Convert.ToDateTime(contrato.FechaTerminacionFase2).ToString("dd/MM/yyyy") : contrato.FechaTerminacionFase2.ToString();
                            FechaInicioContratoTmp = contrato.FechaActaInicioFase2 != null ? Convert.ToDateTime(contrato.FechaActaInicioFase2).ToString("dd/MM/yyyy") : contrato.FechaActaInicioFase2.ToString();

                            NumeroContratoTmp = contrato.NumeroContrato;
                            PlazoFormatTmp = Convert.ToInt32(contrato.PlazoFase2ConstruccionMeses).ToString("00") + " meses / " + Convert.ToInt32(contrato.PlazoFase2ConstruccionDias).ToString("00") + " dias ";

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
                         IdContratista = 0,
                         FechaFinContrato = e.InnerException.ToString(),
                         FechaInicioContrato = e.ToString(), 
                         NombreContratista = "ERROR",
                         NumeroContrato = "ERROR",
                         PlazoFormat= "ERROR",

                         TipoDocumentoContratista = "ERROR",
                         NumeroIdentificacion = "ERROR",
                         TipoIntervencion = "ERROR",
                         TipoIntervencionCodigo = "ERROR",
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


                TipoControversiaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaContractual.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipo_de_controversia);
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


        public async Task<Respuesta> EnviarCorreoTecnicaJuridicaContratacion(string lstMails, string pMailServer, int pMailPort, string pPassword, string pSentender, int pContratoId,  int pIdTemplate)
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
            //await AprobarContratoByIdContrato(1);

            List<GrillaTipoSolicitudControversiaContractual> ListControversiaContractualGrilla = new List<GrillaTipoSolicitudControversiaContractual>();
            //Fecha de firma del contrato ??? FechaFirmaContrato , [Contrato] -(dd / mm / aaaa)

            //Tipo de solicitud ??? ContratoPoliza - TipoSolicitudCodigo      

            //List<ControversiaContractual> ListControversiaContractualGrilla = await _context.ControversiaContractual.Where(r => !(bool)r.EstadoCodigo).Distinct().ToListAsync();
            List<ControversiaContractual> ListControversiaContractual = await _context.ControversiaContractual.Where(r=>r.Eliminado==false).Distinct().ToListAsync();

            if (pControversiaContractualId != 0)
            {
                 ListControversiaContractual = await _context.ControversiaContractual.Where(r => r.ControversiaContractualId == pControversiaContractualId).ToListAsync();                             

            }

            foreach (var controversia in ListControversiaContractual)
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
                    string strTipoControversia = "sin definir";

                    //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                    Dominio EstadoCodigoControversia;
                    Dominio TipoControversiaCodigo;

                    string prefijo = "";

                    if (contrato != null)
                    {
                        TipoControversiaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipo_de_controversia);
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

                    //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                    GrillaTipoSolicitudControversiaContractual RegistroControversiaContractual = new GrillaTipoSolicitudControversiaContractual
                    {
                         ControversiaContractualId=controversia.ControversiaContractualId,
                         //NumeroSolicitud=controversia.NumeroSolicitud,
                        //NumeroSolicitud = string.Format("0000"+ controversia.ControversiaContractualId.ToString()),
                        NumeroSolicitud = controversia.NumeroSolicitud,
                        //FechaSolicitud=controversia.FechaSolicitud,
                        FechaSolicitud =controversia.FechaSolicitud != null ? Convert.ToDateTime(controversia.FechaSolicitud).ToString("dd/MM/yyyy") : controversia.FechaSolicitud.ToString(),
                         TipoControversia =strTipoControversia,
                         TipoControversiaCodigo= strTipoControversiaCodigo,
                         ContratoId = contrato.ContratoId,
                        NumeroContrato = contrato.NumeroContrato,
                        EstadoControversia =strEstadoControversia,
                        EstadoControversiaCodigo = strEstadoCodigoControversia,
                        RegistroCompletoNombre = (bool)controversia.EsCompleto ? "Completo" : "Incompleto",

                        //cu 4.4.1
                        //Actuacion = "Actuación " + actuacionSeguimiento.ActuacionSeguimientoId.ToString()
                        Actuacion = "PENDIENTE",
                        FechaActuacion = "PENDIENTE",
                        EstadoActuacion= "PENDIENTE",



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
                        strUsuario = controversiaMotivo.UsuarioModificacion;

                        controversiaMotivoBD.MotivoSolicitudCodigo = controversiaMotivo.MotivoSolicitudCodigo;
                        
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
                    
                    GrillaActuacionSeguimiento RegistroActuacionSeguimiento=null;
                    ControversiaActuacion controversiaActuacion;
                    controversiaActuacion = _context.ControversiaActuacion
                        .Where(r => r.ControversiaContractualId == pControversiaContractualId).FirstOrDefault();
                    if (controversiaActuacion != null)
                    {
                        if(controversiaActuacion.ControversiaActuacionId== actuacionSeguimiento.ControversiaActuacionId)
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
                                NumeroReclamacion = "REC " + actuacionSeguimiento.ActuacionSeguimientoId.ToString("0000"),
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
                            NumeroReclamacion = "REC " + actuacionSeguimiento.ActuacionSeguimientoId.ToString("0000"),
                            Actuacion = "Actuación " + actuacionSeguimiento.ActuacionSeguimientoId.ToString(),
                            ControversiaActuacionId = actuacionSeguimiento.ControversiaActuacionId,
                            //RegistroCompletoActuacion = (bool)controversia.EsCompleto ? "Completo" : "Incompleto",                        

                        };
                    }                                        

                    //if (!(bool)proyecto.RegistroCompleto)
                    //{
                    //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                    //}
                    if(RegistroActuacionSeguimiento!=null)
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
                                EstadoActuacion = EstadoActuacion==null?"": EstadoActuacion.Nombre,
                                EstadoRegistro = actuacionSeguimiento.RegistroCompleto?"Completo":"Incompleto",
                                EstadoActuacionCodigo= actuacionSeguimiento.EstadoDerivadaCodigo,
                                EstadoReclamacionCodigo = actuacionSeguimiento.EstadoReclamacionCodigo,
                                FechaActualizacion = actuacionSeguimiento.FechaActuacionAdelantada != null ? Convert.ToDateTime(actuacionSeguimiento.FechaModificacion).ToString("dd/MM/yyyy") : actuacionSeguimiento.FechaModificacion.ToString(),
                                NumeroReclamacion = "ACT_REC " + actuacionSeguimiento.ActuacionSeguimientoId.ToString("0000"),
                                Actuacion = "Actuación " + actuacionSeguimiento.ActuacionSeguimientoId.ToString(),
                                ControversiaActuacionId = actuacionSeguimiento.ControversiaActuacionId,
                                //RegistroCompletoActuacion = (bool)controversia.EsCompleto ? "Completo" : "Incompleto",                        

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

        public async Task<List<GrillaControversiaActuacionEstado>> ListGrillaControversiaActuacion(int id=0, int pControversiaContractualId=0, bool esActuacionReclamacion=false)
        {
            //await AprobarContratoByIdContrato(1);

            List<GrillaControversiaActuacionEstado> ListControversiaContractualGrilla = new List<GrillaControversiaActuacionEstado>();
            //Fecha de firma del contrato ??? FechaFirmaContrato , [Contrato] -(dd / mm / aaaa)

            //Tipo de solicitud ??? ContratoPoliza - TipoSolicitudCodigo      

            //List<ControversiaContractual> ListControversiaContractualGrilla = await _context.ControversiaContractual.Where(r => !(bool)r.EstadoCodigo).Distinct().ToListAsync();

            List<ControversiaActuacion> lstControversiaActuacion = await _context.ControversiaActuacion.
                Where(r => !(bool)r.Eliminado).Include(r=>r.ActuacionSeguimiento).Distinct().ToListAsync();

            List<ControversiaActuacion> lstControversiaActuacionCruce= new List<ControversiaActuacion>();

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
           

            else if (id!=0)
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

                    EstadoAvanceCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.EstadoAvanceTramiteCodigo, (int)EnumeratorTipoDominio.Estados_Actuacion);
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

                    int ActuacionSeguimientoIdTmp =0;

                    if(lstActuacionSeguimientoId.Count()>0)
                     ActuacionSeguimientoIdTmp = lstActuacionSeguimientoId[0];
                    //EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                    //if (EstadoSolicitudCodigoContratoPoliza != null)
                    //    strEstadoSolicitudCodigoContratoPoliza = EstadoSolicitudCodigoContratoPoliza.Nombre;

                    //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                    GrillaControversiaActuacionEstado RegistroControversiaContractual = new GrillaControversiaActuacionEstado
                    {
                        ControversiaContractualId = controversia.ControversiaContractualId,
                        FechaActualizacion = controversia.FechaActuacion != null ? Convert.ToDateTime(controversia.FechaActuacion).ToString("dd/MM/yyyy") : "",// controversia.FechaModificacion.ToString("dd/MM/yyyy"),
                        DescripcionActuacion = "Actuación " + controversia.ControversiaActuacionId.ToString(),
                        //DescripcionActuacion = "ACT" + controversia.ControversiaActuacionId.ToString(),
                        ActuacionId = controversia.ControversiaActuacionId,

                        EstadoActuacion = strEstadoAvanceTramite,//controversia.EstadoAvanceTramiteCodigo
                        EstadoActuacionCodigo = strEstadoAvanceTramiteCodigo,//controversia.EstadoAvanceTramiteCodigo

                        NumeroActuacion = "ACT controversia " + controversia.ControversiaActuacionId.ToString("000"),

                        NumeroActuacionReclamacion = "ACT_REC " + controversia.ControversiaActuacionId.ToString("0000"),

                        RegistroCompletoActuacion = (bool)controversia.EsCompleto ? "Completo" : "Incompleto",

                        ProximaActuacionCodigo = strProximaActuacionCodigo,
                        ProximaActuacionNombre = strProximaActuacionNombre,
                        EstadoActuacionReclamacionCodigo = (controversia.EstadoActuacionReclamacionCodigo!=null)? controversia.EstadoActuacionReclamacionCodigo :"",
                        EstadoActuacionReclamacion = EstadoActuacionReclamacionTmp,

                        RegistroCompletoReclamacion = controversia.EsCompletoReclamacion==null?"Incompleto":(bool)controversia.EsCompletoReclamacion ? "Completo" : "Incompleto",
                        ActuacionSeguimientoId = ActuacionSeguimientoIdTmp,
                        //ActuacionSeguimientoId =controversia.ActuacionSeguimientoId,
                        //ControversiaContractualId = controversia.ControversiaContractualId,
                        //NumeroSolicitud = controversia.NumeroSolicitud,
                        ////FechaSolicitud=controversia.FechaSolicitud,
                        //FechaSolicitud = controversia.FechaSolicitud != null ? Convert.ToDateTime(controversia.FechaSolicitud).ToString("dd/MM/yyyy") : controversia.FechaSolicitud.ToString(),
                        //TipoControversia = strTipoControversia,
                        //TipoControversiaCodigo = strTipoControversiaCodigo,
                        //ContratoId = contrato.ContratoId,
                        //EstadoControversia = "PENDIENTE",
                        //RegistroCompletoNombre = (bool)controversia.EsCompleto ? "Completo" : "Incompleto",

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
                        ControversiaContractualId=0,                        
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
                await _context.ControversiaContractual.Where(r => r.Eliminado == false).Include(x=>x.ControversiaActuacion).Distinct().ToListAsync();

            foreach (var controversia in ListControversiaContractual)
            {
                foreach (var controversiaActuacion in controversia.ControversiaActuacion)
                {
                    if(controversiaActuacion.EsRequiereFiduciaria!=null && (bool)controversiaActuacion.EsRequiereFiduciaria)
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
                            string strTipoControversia = "sin definir";

                            //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
                            Dominio EstadoCodigoControversia;
                            Dominio TipoControversiaCodigo;

                            string prefijo = "";

                            if (contrato != null)
                            {
                                TipoControversiaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipo_de_controversia);
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
                            var dmActuacion=  await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaActuacion.ProximaActuacionCodigo, (int)EnumeratorTipoDominio.Proxima_actuacion_requerida);
                            var dmActuacionEstado =  await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaActuacion.EstadoAvanceTramiteCodigo, (int)EnumeratorTipoDominio.Estados_Actuacion_Derivada);
                            var dmReclamacionEstado = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiaActuacion.EstadoActuacionReclamacionCodigo, (int)EnumeratorTipoDominio.Estados_Reclamacion);
                            string actuacion = dmActuacion==null?"":dmActuacion.Nombre;
                            string estado = dmActuacionEstado==null?"":dmActuacionEstado.Nombre;

                            string estadoReclamacion = dmReclamacionEstado == null ? "" : dmReclamacionEstado.Nombre;
                            //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
                            GrillaTipoSolicitudControversiaContractual RegistroControversiaContractual = new GrillaTipoSolicitudControversiaContractual
                            {
                                ControversiaContractualId = controversia.ControversiaContractualId,
                                //NumeroSolicitud=controversia.NumeroSolicitud,
                                //NumeroSolicitud = string.Format("0000"+ controversia.ControversiaContractualId.ToString()),
                                NumeroSolicitud = prefijo + controversia.ControversiaContractualId.ToString("000"),
                                //FechaSolicitud=controversia.FechaSolicitud,
                                FechaSolicitud = controversia.FechaSolicitud != null ? Convert.ToDateTime(controversia.FechaSolicitud).ToString("dd/MM/yyyy") : controversia.FechaSolicitud.ToString(),
                                TipoControversia = strTipoControversia,
                                TipoControversiaCodigo = strTipoControversiaCodigo,
                                ContratoId = contrato.ContratoId,
                                NumeroContrato = contrato.NumeroContrato,
                                EstadoControversia = strEstadoControversia,
                                EstadoControversiaCodigo = strEstadoCodigoControversia,
                                RegistroCompletoNombre = (bool)controversia.EsCompleto ? "Completo" : "Incompleto",

                                //cu 4.4.1
                                //Actuacion = "Actuación " + actuacionSeguimiento.ActuacionSeguimientoId.ToString()

                                Actuacion = actuacion,
                                FechaActuacion = controversiaActuacion.FechaActuacion!=null?Convert.ToDateTime(controversiaActuacion.FechaActuacion).ToString("dd/MM/yyyy"):"",
                                EstadoActuacion = estado,
                                ActuacionID =controversiaActuacion.ControversiaActuacionId,
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
                actuacionSeguimientoOld.EstadoActuacionReclamacionCodigo = "3";//cambiar

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
                if(string.IsNullOrEmpty(actuacionSeguimiento.DescripciondeActuacionAdelantada)||
                    actuacionSeguimiento.FechaActuacionDerivada==null||
                    string.IsNullOrEmpty(actuacionSeguimiento.EstadoActuacionDerivadaCodigo) ||
                    string.IsNullOrEmpty(actuacionSeguimiento.Observaciones) ||
                    string.IsNullOrEmpty(actuacionSeguimiento.RutaSoporte))
                {
                    escompleto = false;
                }
                actuacionSeguimiento.EsCompleto = escompleto;
                if(actuacionSeguimiento.SeguimientoActuacionDerivadaId>0)
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
                    actuacionSeguimiento.FechaCreacion = DateTime.Now;
                    _context.SeguimientoActuacionDerivada.Add(actuacionSeguimiento);
                }
                //al papa le cambio el estado
                var controversia = _context.ControversiaActuacion.Find(actuacionSeguimiento.ControversiaActuacionId);
                controversia.EstadoActuacionReclamacionCodigo = "2";//en proceso
                controversia.FechaModificacion = DateTime.Now;
                controversia.UsuarioModificacion = actuacionSeguimiento.UsuarioCreacion==null? actuacionSeguimiento.UsuarioModificacion: actuacionSeguimiento.UsuarioCreacion;
                _context.ControversiaActuacion.Update(controversia);
                _context.SaveChanges();

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
                actuacionSeguimientoOld.UsuarioModificacion = pUsuarioModifica;
                actuacionSeguimientoOld.FechaModificacion = DateTime.Now;
                actuacionSeguimientoOld.EstadoActuacionDerivadaCodigo = "3";//cambiar
                _context.SeguimientoActuacionDerivada.Update(actuacionSeguimientoOld);
                _context.SaveChanges();

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
                Where(r => !(bool)r.Eliminado && r.ControversiaContractualId== id).Include(r => r.ActuacionSeguimiento).Distinct().ToListAsync();

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

                    EstadoAvanceCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.EstadoAvanceTramiteCodigo, (int)EnumeratorTipoDominio.Estado_controversia_contractual_TAI);
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

                    GrillaControversiaActuacionEstado RegistroControversiaContractual = new GrillaControversiaActuacionEstado
                    {
                        ControversiaContractualId = controversia.ControversiaContractualId,
                        FechaActualizacion = controversia.FechaModificacion != null ? Convert.ToDateTime(controversia.FechaModificacion).ToString("dd/MM/yyyy") : controversia.FechaModificacion.ToString(),
                        DescripcionActuacion = "Actuación " + controversia.ControversiaActuacionId.ToString(),
                        //DescripcionActuacion = "ACT" + controversia.ControversiaActuacionId.ToString(),
                        ActuacionId = controversia.ControversiaActuacionId,

                        EstadoActuacion = strEstadoAvanceTramite,//controversia.EstadoAvanceTramiteCodigo
                        EstadoActuacionCodigo = strEstadoAvanceTramiteCodigo,//controversia.EstadoAvanceTramiteCodigo

                        NumeroActuacion = "ACT controversia " + controversia.ControversiaActuacionId.ToString("000"),

                        NumeroActuacionReclamacion = "REC " + controversia.ControversiaActuacionId.ToString("0000"),

                        RegistroCompletoReclamacion = controversia.EsCompletoReclamacion ==null ? "Incompleto": (bool)controversia.EsCompletoReclamacion ? "Completo" : "Incompleto",

                        ProximaActuacionCodigo = strProximaActuacionCodigo,
                        ProximaActuacionNombre = strProximaActuacionNombre,
                        EstadoActuacionReclamacionCodigo = (controversia.EstadoActuacionReclamacionCodigo != null) ? controversia.EstadoActuacionReclamacionCodigo : "",
                        EstadoActuacionReclamacion = EstadoActuacionReclamacionTmp,
                        ActuacionSeguimientoId = ActuacionSeguimientoIdTmp,                      
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
                    string.IsNullOrEmpty(prmMesa.RutaSoporte))
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
                    prmMesa.FechaCreacion = DateTime.Now;                    
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
            var mesas = _context.ControversiaActuacionMesa.Where(x=>x.ControversiaActuacion.ControversiaContractualId== pControversiaId).ToList();
            foreach(var mesa in mesas)
            {
               var  EstadoActuacionReclamacion = await _commonService.GetDominioByNombreDominioAndTipoDominio(mesa.EstadoAvanceMesaCodigo, (int)EnumeratorTipoDominio.Estados_Actuacion);
                mesa.EstadoRegistroString = mesa.EsCompleto ? "Completo" : "Incompleto";
                mesa.EstadoAvanceMesaString = EstadoActuacionReclamacion==null?"": EstadoActuacionReclamacion.Nombre;
            }
            return mesas;
        }

        public async Task<Respuesta> FinalizarMesa(int pControversiaActuacionId, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_estado_Controversia_Actuacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                ControversiaActuacionMesa actuacionSeguimientoOld;

                actuacionSeguimientoOld = _context.ControversiaActuacionMesa.Where(x=>x.ControversiaActuacionId==pControversiaActuacionId).FirstOrDefault();
                actuacionSeguimientoOld.UsuarioModificacion = pUsuarioModifica;
                actuacionSeguimientoOld.FechaModificacion = DateTime.Now;
                actuacionSeguimientoOld.EstadoAvanceMesaCodigo = "2";//cambiar
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
                actuacionSeguimientoOld.EstadoAvanceMesaCodigo = pNuevoCodigoEstadoAvance;
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
                var EstadoActuacionReclamacion = await _commonService.GetDominioByNombreDominioAndTipoDominio(mesa.EstadoAvanceMesaCodigo, (int)EnumeratorTipoDominio.Estados_Actuacion);
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
                    _context.ControversiaActuacionMesaSeguimiento.Update(actuacionold);
                }
                else
                {
                    controversiaActuacionMesa.FechaCreacion = DateTime.Now;
                    controversiaActuacionMesa.NumeroActuacionSeguimiento = "ACT MT ";
                    _context.ControversiaActuacionMesaSeguimiento.Add(controversiaActuacionMesa);
                    _context.SaveChanges();
                    controversiaActuacionMesa.NumeroActuacionSeguimiento = "ACT MT "+controversiaActuacionMesa.ControversiaActuacionMesaSeguimientoId.ToString("000");
                    _context.ControversiaActuacionMesaSeguimiento.Update(controversiaActuacionMesa);
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

                    EstadoAvanceCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversia.EstadoAvanceTramiteCodigo, (int)EnumeratorTipoDominio.Estado_controversia_contractual_TAI);
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
                    ControversiaActuacionMesa controversiamesa = _context.ControversiaActuacionMesa.Where(x=>!(bool)x.Eliminado && x.ControversiaActuacionId==controversia.ControversiaActuacionId).FirstOrDefault();
                    string stadomesa = "";
                    if(controversiamesa!=null)
                    {
                        var EstadoActuacionMesa = await _commonService.GetDominioByNombreDominioAndTipoDominio(controversiamesa.EstadoAvanceMesaCodigo, (int)EnumeratorTipoDominio.Estados_Actuacion);
                        stadomesa = EstadoActuacionMesa == null ? "" : EstadoActuacionMesa.Nombre;
                    }
                        
                    
                    GrillaControversiaActuacionEstado RegistroControversiaContractual = new GrillaControversiaActuacionEstado
                    {
                        ControversiaContractualId = controversia.ControversiaContractualId,
                        FechaActualizacion = controversia.FechaModificacion != null ? Convert.ToDateTime(controversia.FechaModificacion).ToString("dd/MM/yyyy") : controversia.FechaModificacion.ToString(),
                        DescripcionActuacion = "Actuación " + controversia.ControversiaActuacionId.ToString(),
                        //DescripcionActuacion = "ACT" + controversia.ControversiaActuacionId.ToString(),
                        ActuacionId = controversia.ControversiaActuacionId,

                        EstadoActuacion = strEstadoAvanceTramite,//controversia.EstadoAvanceTramiteCodigo
                        EstadoActuacionCodigo = strEstadoAvanceTramiteCodigo,//controversia.EstadoAvanceTramiteCodigo

                        NumeroActuacion = "ACT controversia " + controversia.ControversiaActuacionId.ToString("000"),

                        NumeroActuacionReclamacion = "REC " + controversia.ControversiaActuacionId.ToString("0000"),

                        RegistroCompletoReclamacion = controversia.EsCompletoReclamacion == null ? "Incompleto" : (bool)controversia.EsCompletoReclamacion ? "Completo" : "Incompleto",

                        ProximaActuacionCodigo = strProximaActuacionCodigo,
                        ProximaActuacionNombre = strProximaActuacionNombre,
                        EstadoActuacionReclamacionCodigo = (controversia.EstadoActuacionReclamacionCodigo != null) ? controversia.EstadoActuacionReclamacionCodigo : "",
                        EstadoActuacionReclamacion = EstadoActuacionReclamacionTmp,
                        ActuacionSeguimientoId = ActuacionSeguimientoIdTmp,
                        NumeroMesa = controversiamesa==null?"":"MT " + controversiamesa.ControversiaActuacionMesaId.ToString("0000"),
                        EstadoMesa = controversiamesa == null ? "" : controversiamesa.EstadoAvanceMesaCodigo,
                        EstadoCodigoMesa = stadomesa,
                        MesaId= controversiamesa == null ? "" : controversiamesa.ControversiaActuacionMesaId.ToString(),
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
            var mesas = _context.ControversiaActuacionMesaSeguimiento.Where(x => x.ControversiaActuacionMesa.ControversiaActuacionId == pActuacionId).ToList();
            foreach (var mesa in mesas)
            {
                var EstadoActuacionReclamacion = await _commonService.GetDominioByNombreDominioAndTipoDominio(mesa.EstadoAvanceMesaCodigo, (int)EnumeratorTipoDominio.Estados_Actuacion);
                mesa.EstadoRegistroString = mesa.EsCompleto ? "Completo" : "Incompleto";
                mesa.EstadoAvanceMesaString = EstadoActuacionReclamacion == null ? "" : EstadoActuacionReclamacion.Nombre;
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
    }
}
