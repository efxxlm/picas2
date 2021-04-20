using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Constants;
using System.IO;
using DinkToPdf;
using DinkToPdf.Contracts;
using Z.EntityFramework.Plus;
using asivamosffie.services.Helpers;

namespace asivamosffie.services
{
    public class JudicialDefenseService : IJudicialDefense
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        private readonly IConverter _converter;

        public JudicialDefenseService(devAsiVamosFFIEContext context, ICommonService commonService, IConverter converter)
        {

            _commonService = commonService;
            _context = context;
            _converter = converter;
            //_settings = settings;
        }

        public async Task<string> GetNombreContratistaByContratoId(int pContratoId)
        {
            Contrato contrato = null;
            contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

            Contratacion contratacion = null;
            if (contrato != null)
            {
                contratacion = await _commonService.GetContratacionByContratacionId(contrato.ContratacionId);

            }

            Contratista contratista = null;
            if (contratacion != null)
            {
                if (contratacion.ContratistaId != null)
                    contratista = await _commonService.GetContratistaByContratistaId((Int32)contratacion.ContratistaId);

                if (contratista != null)
                {
                    return contratista.Nombre;

                }
            }
            return null;
        }

        public async Task<Respuesta> CreateOrEditDemandadoConvocado(DemandadoConvocado demandadoConvocado)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Demandado_Convocado, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            DemandadoConvocado demandadoConvocadoBD = null;
            try
            {

                if (string.IsNullOrEmpty(demandadoConvocado.DemandadoConvocadoId.ToString()) || demandadoConvocado.DemandadoConvocadoId == 0)
                {
                    //Auditoria
                    strCrearEditar = "REGISTRAR DEMANDADO CONVOCADO";
                    demandadoConvocado.FechaCreacion = DateTime.Now;
                    demandadoConvocado.UsuarioCreacion = demandadoConvocado.UsuarioCreacion;
                    //fichaEstudio.DefensaJudicialId = fichaEstudio.DefensaJudicialId;
                    demandadoConvocado.Eliminado = false;
                    _context.DemandadoConvocado.Add(demandadoConvocado);
                }
                else
                {
                    strCrearEditar = "EDIT DEMANDADO CONVOCADO";
                    demandadoConvocadoBD = _context.DemandadoConvocado.Find(demandadoConvocado.DemandadoConvocadoId);

                    //Auditoria
                    demandadoConvocadoBD.UsuarioModificacion = demandadoConvocado.UsuarioModificacion;
                    demandadoConvocadoBD.Eliminado = false;

                    //Registros
                    demandadoConvocadoBD.Nombre = demandadoConvocado.Nombre;
                    demandadoConvocadoBD.TipoIdentificacionCodigo = demandadoConvocado.TipoIdentificacionCodigo;
                    demandadoConvocadoBD.NumeroIdentificacion = demandadoConvocado.NumeroIdentificacion;
                    demandadoConvocadoBD.DefensaJudicial = demandadoConvocado.DefensaJudicial;
                    demandadoConvocadoBD.Direccion = demandadoConvocado.Direccion;
                    demandadoConvocadoBD.UsuarioModificacion = demandadoConvocado.UsuarioModificacion;
                    demandadoConvocadoBD.Email = demandadoConvocado.Email;

                    _context.DemandadoConvocado.Update(demandadoConvocadoBD);

                }

                _context.SaveChanges();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = demandadoConvocado,
                    Code = ConstantMessagesJudicialDefense.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.OperacionExitosa, idAccion, demandadoConvocado.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = demandadoConvocado,
                    Code = ConstantMessagesJudicialDefense.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.Error, idAccion, demandadoConvocado.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        public async Task<Respuesta> CreateOrEditDefensaJudicial(DefensaJudicial defensaJudicial)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Defensa_Judicial, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty, strUsuario = string.Empty;

            DefensaJudicial defensaJudicialBD = null;
            try
            {

                if (string.IsNullOrEmpty(defensaJudicial.DefensaJudicialId.ToString()) || defensaJudicial.DefensaJudicialId == 0)
                {
                    //Auditoria
                    strCrearEditar = "REGISTRAR DEFENSA JUDICIAL";
                    defensaJudicial.FechaCreacion = DateTime.Now;
                    defensaJudicial.UsuarioCreacion = defensaJudicial.UsuarioCreacion;
                    //fichaEstudio.DefensaJudicialId = fichaEstudio.DefensaJudicialId;
                    defensaJudicial.Eliminado = false;
                    foreach (var defContratcionProyecto in defensaJudicial.DefensaJudicialContratacionProyecto)
                    {
                        defContratcionProyecto.UsuarioCreacion = defensaJudicial.UsuarioCreacion;
                        defContratcionProyecto.FechaCreacion = DateTime.Now;
                        defContratcionProyecto.EsCompleto = true;
                        defContratcionProyecto.Eliminado = false;
                    }
                    foreach (var defConvocado in defensaJudicial.DemandadoConvocado)
                    {
                        defConvocado.UsuarioCreacion = defensaJudicial.UsuarioCreacion;
                        defConvocado.FechaCreacion = DateTime.Now;
                        defConvocado.Eliminado = false;
                    }
                    foreach (var defFicha in defensaJudicial.FichaEstudio)
                    {
                        defFicha.UsuarioCreacion = defensaJudicial.UsuarioCreacion;
                        defFicha.FechaCreacion = DateTime.Now;
                        defFicha.Eliminado = false;
                        defFicha.EsCompleto = ValidarRegistroCompletoFichaEstudio(defFicha);
                    }
                    defensaJudicial.EstadoProcesoCodigo = "1";
                    defensaJudicial.LegitimacionCodigo = "";
                    defensaJudicial.NumeroProceso = Helpers.Helpers.Consecutive("DJ", _context.DefensaJudicial.Count());
                    _context.DefensaJudicial.Add(defensaJudicial);
                }
                else
                {
                    strCrearEditar = "EDIT DEFENSA JUDICIAL";
                    defensaJudicialBD = _context.DefensaJudicial.Where(x => x.DefensaJudicialId == defensaJudicial.DefensaJudicialId).
                        Include(x => x.DemandadoConvocado).Include(x => x.DemandanteConvocante).
                        Include(x => x.DefensaJudicialSeguimiento).Include(x => x.DefensaJudicialContratacionProyecto).
                        Include(x => x.FichaEstudio).FirstOrDefault();

                    defensaJudicialBD.EsLegitimacionActiva = defensaJudicial.EsLegitimacionActiva;
                    defensaJudicialBD.TipoProcesoCodigo = defensaJudicial.TipoProcesoCodigo;

                    //Auditoria
                    defensaJudicialBD.UsuarioModificacion = defensaJudicial.UsuarioModificacion;
                    defensaJudicialBD.Eliminado = false;

                    //Registros
                    defensaJudicialBD.LocalizacionIdMunicipio = defensaJudicial.LocalizacionIdMunicipio;
                    defensaJudicialBD.TipoAccionCodigo = defensaJudicial.TipoAccionCodigo;
                    defensaJudicialBD.JurisdiccionCodigo = defensaJudicial.JurisdiccionCodigo;
                    defensaJudicialBD.Pretensiones = defensaJudicial.Pretensiones;
                    defensaJudicialBD.CuantiaPerjuicios = defensaJudicial.CuantiaPerjuicios;
                    defensaJudicialBD.FechaRadicadoFfie = defensaJudicial.FechaRadicadoFfie;
                    defensaJudicialBD.NumeroRadicadoFfie = defensaJudicial.NumeroRadicadoFfie;
                    defensaJudicialBD.CanalIngresoCodigo = defensaJudicial.CanalIngresoCodigo;

                    defensaJudicialBD.UsuarioModificacion = defensaJudicial.UsuarioModificacion;
                    defensaJudicialBD.EsRequiereSupervisor = defensaJudicial.EsRequiereSupervisor;

                    defensaJudicialBD.EsDemandaFfie = defensaJudicial.EsDemandaFfie;
                    defensaJudicialBD.NumeroDemandantes = defensaJudicial.NumeroDemandantes;
                    defensaJudicialBD.NumeroDemandados = defensaJudicial.NumeroDemandados;

                    //url
                    defensaJudicialBD.UrlSoporteProceso = defensaJudicial.UrlSoporteProceso;
                    //contratos
                    defensaJudicialBD.CantContratos = defensaJudicial.CantContratos;

                    foreach (var defContratcionProyecto in defensaJudicial.DefensaJudicialContratacionProyecto)
                    {
                        if (defContratcionProyecto.UsuarioCreacion == null)
                        {
                            defContratcionProyecto.UsuarioCreacion = defensaJudicial.UsuarioCreacion;
                            defContratcionProyecto.FechaCreacion = DateTime.Now;
                            defContratcionProyecto.EsCompleto = true;
                            defContratcionProyecto.Eliminado = false;
                            defContratcionProyecto.DefensaJudicialId = defensaJudicialBD.DefensaJudicialId;
                            _context.DefensaJudicialContratacionProyecto.Add(defContratcionProyecto);
                        }
                        else
                        {
                            defContratcionProyecto.UsuarioModificacion = defensaJudicial.UsuarioModificacion;
                            defContratcionProyecto.FechaModificacion = DateTime.Now;
                            defContratcionProyecto.EsCompleto = true;
                            defContratcionProyecto.Eliminado = false;
                        }

                    }
                    foreach (var defConvocado in defensaJudicial.DemandadoConvocado)
                    {
                        if (defConvocado.UsuarioCreacion == null)
                        {
                            defConvocado.UsuarioCreacion = defensaJudicial.UsuarioCreacion;
                            defConvocado.FechaCreacion = DateTime.Now;
                            defConvocado.Eliminado = false;
                        }
                        else
                        {
                            defConvocado.UsuarioModificacion = defensaJudicial.UsuarioCreacion;
                            defConvocado.FechaModificacion = DateTime.Now;
                            defConvocado.Eliminado = false;
                        }

                    }

                    foreach (var defFicha in defensaJudicial.FichaEstudio)
                    {
                        if (defFicha != null)
                        {
                            if (defFicha.FichaEstudioId == 0)
                            {
                                defFicha.UsuarioCreacion = defensaJudicial.UsuarioCreacion;
                                defFicha.FechaCreacion = DateTime.Now;
                                defFicha.Eliminado = false;
                                defFicha.DefensaJudicialId = defensaJudicialBD.DefensaJudicialId;
                                defFicha.EsCompleto = ValidarRegistroCompletoFichaEstudio(defFicha);
                                _context.FichaEstudio.Add(defFicha);
                            }
                            else
                            {
                                FichaEstudio fichaEstudioBD = _context.FichaEstudio.Find(defFicha.FichaEstudioId);

                                defFicha.UsuarioModificacion = defensaJudicial.UsuarioCreacion;
                                defFicha.FechaModificacion = DateTime.Now;
                                defFicha.Eliminado = false;
                                fichaEstudioBD.Antecedentes = defFicha.Antecedentes;
                                fichaEstudioBD.HechosRelevantes = defFicha.HechosRelevantes;
                                fichaEstudioBD.JurisprudenciaDoctrina = defFicha.JurisprudenciaDoctrina;
                                fichaEstudioBD.DecisionComiteDirectrices = defFicha.DecisionComiteDirectrices;
                                fichaEstudioBD.AnalisisJuridico = defFicha.AnalisisJuridico;
                                fichaEstudioBD.Recomendaciones = defFicha.Recomendaciones;
                                fichaEstudioBD.EsPresentadoAnteComiteFfie = defFicha.EsPresentadoAnteComiteFfie;
                                fichaEstudioBD.FechaComiteDefensa = defFicha.FechaComiteDefensa;
                                fichaEstudioBD.RecomendacionFinalComite = defFicha.RecomendacionFinalComite;
                                fichaEstudioBD.EsAprobadoAperturaProceso = defFicha.EsAprobadoAperturaProceso;
                                fichaEstudioBD.TipoActuacionCodigo = defFicha.TipoActuacionCodigo;
                                fichaEstudioBD.EsActuacionTramiteComite = defFicha.EsActuacionTramiteComite;
                                fichaEstudioBD.RutaSoporte = defFicha.RutaSoporte;
                                fichaEstudioBD.EsCompleto = ValidarRegistroCompletoFichaEstudio(defFicha);

                                _context.FichaEstudio.Update(fichaEstudioBD);

                            }

                            /*if (defFicha.UsuarioCreacion == null)
                            {
                                defFicha.UsuarioCreacion = defensaJudicial.UsuarioCreacion;
                                defFicha.FechaCreacion = DateTime.Now;
                                defFicha.Eliminado = false;
                                defFicha.DefensaJudicialId = defensaJudicialBD.DefensaJudicialId;
                                //_context.FichaEstudio.Add(defFicha);
                            }
                            else
                            {
                                defFicha.UsuarioModificacion = defensaJudicial.UsuarioCreacion;
                                defFicha.FechaModificacion = DateTime.Now;
                                defFicha.Eliminado = false;
                            }
                            */
                        }


                    }
                    foreach (var defFicha in defensaJudicial.DefensaJudicialSeguimiento)
                    {
                        if (defFicha.DefensaJudicialSeguimientoId == 0)
                        {
                            defFicha.UsuarioCreacion = defensaJudicial.UsuarioCreacion;
                            defFicha.FechaCreacion = DateTime.Now;
                            defFicha.Eliminado = false;
                            defFicha.EsCompleto = true;
                            defFicha.DefensaJudicialId = defensaJudicialBD.DefensaJudicialId;
                            _context.DefensaJudicialSeguimiento.Add(defFicha);
                        }
                        //si entra aqui, cambio el estado de la defensa judicial
                        defensaJudicialBD.EstadoProcesoCodigo = "9";//en desaroolo
                    }
                    foreach (var defFicha in defensaJudicial.DemandadoConvocado)
                    {
                        if (defFicha.DemandadoConvocadoId == 0)
                        {
                            defFicha.UsuarioCreacion = defensaJudicial.UsuarioCreacion;
                            defFicha.FechaCreacion = DateTime.Now;
                            defFicha.Eliminado = false;
                            defFicha.DefensaJudicialId = defensaJudicialBD.DefensaJudicialId;
                            defFicha.RegistroCompleto = ValidarRegistroCompletoDemandadoConvocado(defFicha);
                            _context.DemandadoConvocado.Add(defFicha);
                        }
                        else
                        {
                            DemandadoConvocado demandadoConvocado = _context.DemandadoConvocado.Find(defFicha.DemandadoConvocadoId);

                            demandadoConvocado.UsuarioModificacion = defensaJudicial.UsuarioCreacion;
                            demandadoConvocado.FechaModificacion = DateTime.Now;
                            demandadoConvocado.Eliminado = false;
                            demandadoConvocado.DefensaJudicialId = defensaJudicialBD.DefensaJudicialId;
                            demandadoConvocado.RegistroCompleto = ValidarRegistroCompletoDemandadoConvocado(defFicha);
                            demandadoConvocado.Nombre = defFicha.Nombre;
                            demandadoConvocado.TipoIdentificacionCodigo = defFicha.TipoIdentificacionCodigo;
                            demandadoConvocado.NumeroIdentificacion = defFicha.NumeroIdentificacion;
                            demandadoConvocado.Direccion = defFicha.Direccion;
                            demandadoConvocado.Email = defFicha.Email;
                            demandadoConvocado.EsDemandado = defFicha.EsDemandado;
                            demandadoConvocado.EsConvocado = defFicha.EsConvocado;
                            //pasiva
                            if (demandadoConvocado.EsConvocado == true)
                            {
                                demandadoConvocado.ExisteConocimiento = defFicha.ExisteConocimiento;
                                demandadoConvocado.ConvocadoAutoridadDespacho = defFicha.ConvocadoAutoridadDespacho;
                                demandadoConvocado.LocalizacionIdMunicipio = defFicha.LocalizacionIdMunicipio;
                                demandadoConvocado.RadicadoDespacho = defFicha.RadicadoDespacho;
                                demandadoConvocado.FechaRadicado = defFicha.FechaRadicado;
                                demandadoConvocado.MedioControlAccion = defFicha.MedioControlAccion;
                                demandadoConvocado.EtapaProcesoFfiecodigo = defFicha.EtapaProcesoFfiecodigo;
                                demandadoConvocado.CaducidadPrescripcion = defFicha.CaducidadPrescripcion;

                            }

                            _context.DemandadoConvocado.Update(demandadoConvocado);
                        }
                    }
                    foreach (var defFicha in defensaJudicial.DemandanteConvocante)
                    {
                        if (defFicha.DemandanteConvocadoId == 0)
                        {
                            defFicha.UsuarioCreacion = defensaJudicial.UsuarioCreacion;
                            defFicha.FechaCreacion = DateTime.Now;
                            defFicha.Eliminado = false;
                            defFicha.DefensaJucicialId = defensaJudicialBD.DefensaJudicialId;
                            defFicha.RegistroCompleto = ValidarRegistroCompletoDemandanteConvocante(defFicha);
                            _context.DemandanteConvocante.Add(defFicha);
                        }
                        else
                        {
                            DemandanteConvocante demandanteConvocante = _context.DemandanteConvocante.Find(defFicha.DemandanteConvocadoId);

                            demandanteConvocante.UsuarioModificacion = defensaJudicial.UsuarioCreacion;
                            demandanteConvocante.FechaModificacion = DateTime.Now;
                            demandanteConvocante.Eliminado = false;
                            demandanteConvocante.DefensaJucicialId = defensaJudicialBD.DefensaJudicialId;
                            demandanteConvocante.RegistroCompleto = ValidarRegistroCompletoDemandanteConvocante(defFicha);
                            demandanteConvocante.Nombre = defFicha.Nombre;
                            demandanteConvocante.TipoIdentificacionCodigo = defFicha.TipoIdentificacionCodigo;
                            demandanteConvocante.NumeroIdentificacion = defFicha.NumeroIdentificacion;
                            demandanteConvocante.Direccion = defFicha.Direccion;
                            demandanteConvocante.Email = defFicha.Email;

                            _context.DemandanteConvocante.Update(demandanteConvocante);
                        }
                    }
                    //vuelve a empezar el ciclo
                    if (defensaJudicialBD.EstadoProcesoCodigo == ConstanCodigoEstadosDefensaJudicial.Devuelta_por_comite_tecnico || defensaJudicialBD.EstadoProcesoCodigo == ConstanCodigoEstadosDefensaJudicial.Devuelto_por_comite_fiduciario)
                    {
                        defensaJudicialBD.EstadoProcesoCodigo = ConstanCodigoEstadosDefensaJudicial.Modificado_devolucion_comite;
                    }

                    defensaJudicialBD.EsCompleto = ValidarRegistroCompleto(defensaJudicialBD);
                    _context.DefensaJudicial.Update(defensaJudicialBD);

                }

                _context.SaveChanges();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = defensaJudicial,
                    Code = ConstantMessagesJudicialDefense.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.OperacionExitosa, idAccion, defensaJudicial.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = defensaJudicial,
                    Code = ConstantMessagesJudicialDefense.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.Error, idAccion, defensaJudicial.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        private bool ValidarRegistroCompleto(DefensaJudicial defensaJudicial)
        {
            bool retorno = true;
            //valido contratos
            if (defensaJudicial.CantContratos == 0 || defensaJudicial.CantContratos == null)
            {
                retorno = false;
            }
            if (defensaJudicial.DefensaJudicialContratacionProyecto.Count() == 0)
            {
                retorno = false;
            }

            //valido detalle
            if (defensaJudicial.LocalizacionIdMunicipio == null
                || String.IsNullOrEmpty(defensaJudicial.TipoAccionCodigo)
                || String.IsNullOrEmpty(defensaJudicial.JurisdiccionCodigo)
                || String.IsNullOrEmpty(defensaJudicial.Pretensiones)
                || defensaJudicial.EsRequiereSupervisor == null
                )
            {
                retorno = false;
            }

            if ((bool)defensaJudicial.EsLegitimacionActiva)
            {
                if (defensaJudicial.DemandadoConvocado.Count() == 0)
                {
                    retorno = false;
                }
            }

            else {
                //demandantes/convocantes
                if (defensaJudicial.DemandadoConvocado.Count() == 0)
                {
                    retorno = false;
                }
                if (defensaJudicial.DemandanteConvocante.Count() == 0)
                {
                    retorno = false;
                }
            }

            //soporte
            if (defensaJudicial.UrlSoporteProceso == null)
            {
                retorno = false;
            }

            //ficha de estudio
            if (defensaJudicial.FichaEstudio.Count() > 0)
            {
                if (!ValidarRegistroCompletoFichaEstudio(defensaJudicial.FichaEstudio.FirstOrDefault()))
                {
                    retorno = false;
                }
            }
            else
            {
                retorno = false;
            }


            return retorno;
        }

        public async Task<DefensaJudicial> GetVistaDatosBasicosProceso(int pDefensaJudicialId = 0)
        {
            DefensaJudicial defensaJudicial1 = new DefensaJudicial();
            try
            {

                //Contrato contrato = null;
                List<DefensaJudicial> ListDefensaJudicial = new List<DefensaJudicial>();

                if (pDefensaJudicialId == 0)
                {
                    ListDefensaJudicial = await _context.DefensaJudicial.Where(r => (bool)r.Eliminado == false).Distinct()
                .ToListAsync();

                }
                else
                {
                    ListDefensaJudicial = await _context.DefensaJudicial.Where(r => (bool)r.Eliminado == false
                    && r.DefensaJudicialId == pDefensaJudicialId).
                    //Include(x=>x.DefensaJudicialContratacionProyecto).
                    //Include(x => x.DemandadoConvocado).
                    //Include(x => x.DemandanteConvocante).
                    Include(x => x.DefensaJudicialSeguimiento).
                    Include(x => x.FichaEstudio).
                    Distinct().
                    ToListAsync();

                    ListDefensaJudicial.ForEach(defensaJudicialItem =>
                    {
                        List<DemandadoConvocado> demandadoConvocado = _context.DemandadoConvocado.Where(r => (bool)r.Eliminado == false && r.DefensaJudicialId == defensaJudicialItem.DefensaJudicialId).ToList();
                        defensaJudicialItem.DemandadoConvocado = demandadoConvocado;

                        List<DemandanteConvocante> demandanteConvocantes = _context.DemandanteConvocante.Where(r => (bool)r.Eliminado == false && r.DefensaJucicialId == defensaJudicialItem.DefensaJudicialId).ToList();
                        defensaJudicialItem.DemandanteConvocante = demandanteConvocantes;
                    });

                    List<VDefensaJudicialContratacionProyecto> ListVD =
                    _context.VDefensaJudicialContratacionProyecto
                    .Where(r => r.DefensaJudicialId == ListDefensaJudicial.FirstOrDefault().DefensaJudicialId)
                    .Distinct()
                    .OrderBy(r => r.OrderDefensaJudicialContratacionProyectoId)
                    .ToList();

                    ListVD.ForEach(djcp =>
                    {
                        ListDefensaJudicial.FirstOrDefault().DefensaJudicialContratacionProyecto
                               .Add(
                                       _context.DefensaJudicialContratacionProyecto
                                       .Where(r => r.ContratacionProyectoId == djcp.ContratacionProyectoId)
                                       .Distinct()
                                       .FirstOrDefault()
                                );
                    });

                    //TipoAccionCodigo JurisdiccionCodigo TipoProcesoCodigo
                    Dominio TipoAccionCodigo;

                    Dominio TipoDocumentoCodigoContratista;

                    string TipoAccionTmp = string.Empty;

                    foreach (var defensaJudicial in ListDefensaJudicial)
                    {

                        SesionComiteSolicitud sesionComiteSolicitud = _context.SesionComiteSolicitud
                                                                                .Where(r => r.SolicitudId == defensaJudicial.DefensaJudicialId && r.TipoSolicitudCodigo == ConstanCodigoTipoSolicitud.Defensa_judicial && (r.Eliminado == false || r.Eliminado == null))
                                                                                .FirstOrDefault();

                        TipoAccionCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(defensaJudicial.TipoAccionCodigo, (int)EnumeratorTipoDominio.Tipo_accion_judicial);

                        if (TipoAccionCodigo != null)
                            defensaJudicial.TipoAccionCodigoNombre = TipoAccionCodigo.Nombre;

                        var jurisdicion = await _commonService.GetDominioByNombreDominioAndTipoDominio(defensaJudicial.JurisdiccionCodigo, (int)EnumeratorTipoDominio.Jurisdiccion);
                        defensaJudicial.JurisdiccionCodigoNombre = jurisdicion == null ? "" : jurisdicion.Nombre;

                        var proceso = await _commonService.GetDominioByNombreDominioAndTipoDominio(defensaJudicial.TipoProcesoCodigo, 105);

                        defensaJudicial.TipoProcesoCodigoNombre = proceso == null ? "" : proceso.Nombre;
                        if (defensaJudicial.DefensaJudicialContratacionProyecto.Count() > 0)
                        {
                            var contraacionpro = defensaJudicial.DefensaJudicialContratacionProyecto.FirstOrDefault();
                            var contratista = _context.ContratacionProyecto.Where(x => x.ContratacionProyectoId == contraacionpro.ContratacionProyectoId).Select(x => x.Contratacion.Contratista.Nombre).FirstOrDefault();
                            defensaJudicial.EntidadContratista = contratista == null ? "" : contratista;
                        }


                        defensaJudicial.ContratosAsociados = "PENDIENTE";
                        defensaJudicial.FuenteProceso = "PENDIENTE";
                        if (defensaJudicial.LocalizacionIdMunicipio != null)
                        {
                            var munici = _context.Localizacion.Find(defensaJudicial.LocalizacionIdMunicipio.ToString());
                            var depto = _context.Localizacion.Find(munici.IdPadre.ToString());
                            defensaJudicial.Departamento = depto.Descripcion;
                            defensaJudicial.Municipio = munici.Descripcion;
                        }

                        //contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).FirstOrDefault();

                        string TipoDocumentoContratistaTmp = string.Empty;
                        string NumeroIdentificacionContratistaTmp = string.Empty;

                        string TipoIntervencionCodigoTmp = string.Empty;
                        string TipoIntervencionNombreTmp = string.Empty;

                        defensaJudicial.DepartamentoID = defensaJudicial.LocalizacionIdMunicipio == null ? "" : _context.Localizacion.Where(z => z.LocalizacionId == defensaJudicial.LocalizacionIdMunicipio.ToString()).FirstOrDefault().IdPadre;
                        foreach (var contr in defensaJudicial.DefensaJudicialContratacionProyecto)
                        {
                            var contratacionProyecto = _context.ContratacionProyecto.Where(x => x.ContratacionProyectoId == contr.ContratacionProyectoId).
                                 Include(y => y.Proyecto).
                                    ThenInclude(y => y.InstitucionEducativa).
                                Include(y => y.Proyecto).
                                    ThenInclude(y => y.Sede).FirstOrDefault();
                            contr.numeroContrato = _context.Contrato.Where(x => x.ContratacionId == contratacionProyecto.ContratacionId).FirstOrDefault().NumeroContrato;
                        }

                        if (sesionComiteSolicitud != null)
                        {
                            defensaJudicial.ObservacionesComiteTecnico = sesionComiteSolicitud.Observaciones;
                            defensaJudicial.ObversacionesComiteFiduciario = sesionComiteSolicitud.ObservacionesFiduciario;
                        }

                        return defensaJudicial;

                    }
                }
            }
            catch (Exception e)
            {
                defensaJudicial1 = new DefensaJudicial
                {
                    DefensaJudicialId = 0,
                    NumeroProceso = e.InnerException.ToString(),
                    FechaCreacionFormat = DateTime.Now.ToString(),
                    FuenteProceso = e.ToString(),
                    ContratosAsociados = "ERROR",
                    TipoProcesoCodigo = "ERROR",
                    TipoAccionCodigo = "ERROR",
                    JurisdiccionCodigo = "ERROR",

                };

            }
            return defensaJudicial1;
        }

        public async Task<byte[]> GetPlantillaDefensaJudicial(int pDefensaJudicialId, int tipoArchivo)
        {
            if (pDefensaJudicialId == 0)
            {
                return Array.Empty<byte>();
            }
            string TipoPlantilla;

            //ficha de estudio
            if (tipoArchivo == 1)
            {
                TipoPlantilla = ((int)ConstanCodigoPlantillas.Ficha_Estudio_Defensa_Judicial).ToString();
            }
            else
            {
                //proceso de defensa judicial
               TipoPlantilla = ((int)ConstanCodigoPlantillas.Proceso_defensa_judicial_4_2_2).ToString();
            }

            Plantilla Plantilla = _context.Plantilla.Where(r => r.Codigo == TipoPlantilla).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();
            Plantilla.Contenido = await ReemplazarDatosPlantillaDefensaJudicial(Plantilla.Contenido, pDefensaJudicialId);
            return PDF.Convertir(Plantilla);

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

            if (!string.IsNullOrEmpty(pPlantilla.Encabezado.Contenido))
            {
                strEncabezado = Helpers.Helpers.HtmlStringLimpio(pPlantilla.Encabezado.Contenido);
            }

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

        private async Task<string> ReemplazarDatosPlantillaDefensaJudicial(string strContenido, int prmdefensaJudicialID)
        {
            string str = "";
            string valor = "";

            var defPrincial = await GetVistaDatosBasicosProceso(prmdefensaJudicialID);

            strContenido = strContenido.Replace("_Numero_Solicitud_", defPrincial.NumeroProceso);
            strContenido = strContenido.Replace("_Fecha_Solicitud_", defPrincial.FechaCreacion.ToString("dd/MM/yyyy"));
            //strContenido = strContenido.Replace("_Tipo_Controversia_", strTipoControversia);
            strContenido = strContenido.Replace("_Legitimacion_", defPrincial.EsLegitimacionActiva ? "Activa" : "Pasiva");
            strContenido = strContenido.Replace("_Tipo_proceso_", defPrincial.TipoProcesoCodigoNombre);
            strContenido = strContenido.Replace("_Numero_contratos_proceso_", defPrincial.CantContratos.ToString());

            var plantillatrContratos = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Ficha_estudio_tr_contratos).ToString()).FirstOrDefault().Contenido;

            //ACTUACIONES
            List<DefensaJudicialSeguimiento> actuaciones = _context.DefensaJudicialSeguimiento.Where(x => x.DefensaJudicialId == prmdefensaJudicialID).ToList();//diferente a finalizado

            var plantillatrActuaciones = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Tabla_actuaciones).ToString()).FirstOrDefault().Contenido;

            var ListaLocalizaciones = _context.Localizacion.ToList();
            var ListaInstitucionEducativaSedes = _context.InstitucionEducativaSede.ToList();
            var ListaParametricas = _context.Dominio.ToList();
            if (defPrincial.DefensaJudicialContratacionProyecto != null)
            {

                int contador = 1;
                foreach (var defcontratac in defPrincial.DefensaJudicialContratacionProyecto)
                {

                    Localizacion Municipio = ListaLocalizaciones.Where(r => r.LocalizacionId == defcontratac.ContratacionProyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                    Localizacion Departamento = ListaLocalizaciones.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                    Localizacion Region = ListaLocalizaciones.Where(r => r.LocalizacionId == Departamento.IdPadre).FirstOrDefault();

                    InstitucionEducativaSede IntitucionEducativa = ListaInstitucionEducativaSedes.Where(r => r.InstitucionEducativaSedeId == defcontratac.ContratacionProyecto.Proyecto.InstitucionEducativaId).FirstOrDefault();
                    InstitucionEducativaSede Sede = ListaInstitucionEducativaSedes.Where(r => r.InstitucionEducativaSedeId == defcontratac.ContratacionProyecto.Proyecto.SedeId).FirstOrDefault();
                    var contratacion = _context.Contratacion.
                        Where(x => x.ContratacionId == defcontratac.ContratacionProyecto.ContratacionId).
                        Include(x => x.Contratista).FirstOrDefault();

                    plantillatrContratos = plantillatrContratos.Replace("_Tipo_Intervencion_", ListaParametricas
                    .Where(r => r.Codigo == defcontratac.ContratacionProyecto.Proyecto.TipoIntervencionCodigo
                    && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion).FirstOrDefault().Nombre);

                    plantillatrContratos = plantillatrContratos.Replace("_Llave_MEN_", defcontratac.ContratacionProyecto.Proyecto.LlaveMen);
                    plantillatrContratos = plantillatrContratos.Replace("_Departamento_", Departamento.Descripcion);
                    plantillatrContratos = plantillatrContratos.Replace("_Municipio_", Municipio.Descripcion);

                    plantillatrContratos = plantillatrContratos.Replace("_Institucion_Educativa_", IntitucionEducativa.Nombre);

                    plantillatrContratos = plantillatrContratos.Replace("_Sede_", Sede.Nombre);

                    //Plazo de obra
                    plantillatrContratos = plantillatrContratos.Replace("_MesesObra_", defcontratac.ContratacionProyecto.Proyecto.PlazoDiasObra.ToString());
                    plantillatrContratos = plantillatrContratos.Replace("_DiasObra_", defcontratac.ContratacionProyecto.Proyecto.PlazoMesesObra.ToString());
                    //Plazo de Interventoría
                    plantillatrContratos = plantillatrContratos.Replace("_Meses_", defcontratac.ContratacionProyecto.Proyecto.PlazoDiasInterventoria.ToString());
                    plantillatrContratos = plantillatrContratos.Replace("_Dias_", defcontratac.ContratacionProyecto.Proyecto.PlazoMesesInterventoria.ToString());
                    plantillatrContratos = plantillatrContratos.Replace("_Valor_obra_", (defcontratac.ContratacionProyecto.Proyecto.ValorObra != null) ? defcontratac.ContratacionProyecto.Proyecto.ValorObra.ToString() : "0");
                    plantillatrContratos = plantillatrContratos.Replace("_Valor_Interventoria_", "$" + String.Format("{0:n0}", defcontratac.ContratacionProyecto.Proyecto.ValorInterventoria));
                    plantillatrContratos = plantillatrContratos.Replace("_Valor_Total_proyecto_", "$" + String.Format("{0:n0}", defcontratac.ContratacionProyecto.Proyecto.ValorTotal));
                    plantillatrContratos = plantillatrContratos.Replace("_contador_", contador.ToString());
                    plantillatrContratos = plantillatrContratos.Replace("_Numero_Contrato_", defcontratac.numeroContrato);

                    plantillatrContratos = plantillatrContratos.Replace("_Nombre_Contratista_", contratacion.Contratista.Nombre);

                    plantillatrContratos = plantillatrContratos.Replace("_Estado_obra_", defcontratac.ContratacionProyecto.EstadoObraCodigo != null ? ListaParametricas.Where(r => r.Codigo == defcontratac.ContratacionProyecto.EstadoObraCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Obra_Avance_Semanal).FirstOrDefault().Nombre : "Sin registro de avance semanal"); 
                     plantillatrContratos = plantillatrContratos.Replace("_Programacion_obra_acumulada_", defcontratac.ContratacionProyecto.ProgramacionSemanal != null ? defcontratac.ContratacionProyecto.ProgramacionSemanal + " %" : "0%");  //??
                    plantillatrContratos = plantillatrContratos.Replace("_Avance_físico_acumulado_ejecutado_", defcontratac.ContratacionProyecto.AvanceFisicoSemanal != null ? defcontratac.ContratacionProyecto.AvanceFisicoSemanal + " %" : "0%");
                    plantillatrContratos = plantillatrContratos.Replace("Facturacion_programada_acumulada_", "");
                    plantillatrContratos = plantillatrContratos.Replace("_Facturacion_ejecutada_acumulada_", "");
                    contador++;

                }
                strContenido = strContenido.Replace("_trplantillacontratacion_", plantillatrContratos);
            }


            //strContenido = strContenido.Replace("_URL_soportes_solicitud_", controversiaContractual.RutaSoporte);

            Localizacion Municipioproceso = ListaLocalizaciones.Where(r => r.LocalizacionId == defPrincial.LocalizacionIdMunicipio.ToString()).FirstOrDefault();
            Localizacion DepartamentoProceso = ListaLocalizaciones.Where(r => r.LocalizacionId == Municipioproceso.IdPadre).FirstOrDefault();
            var demandadoConvocado = defPrincial.DemandadoConvocado.FirstOrDefault();

            strContenido = strContenido.Replace("_Departamento_inicio_proceso_", DepartamentoProceso.Descripcion);

            strContenido = strContenido.Replace("_Municipio_inicio_proceso_", Municipioproceso.Descripcion);
            strContenido = strContenido.Replace("_Autoridad_despacho_conocimiento_", demandadoConvocado.ConvocadoAutoridadDespacho);
            strContenido = strContenido.Replace("_Fecha_radicado_despacho_conocimiento_", demandadoConvocado.RadicadoDespacho);
            strContenido = strContenido.Replace("_Nombre_convocante_demandante_", demandadoConvocado.Nombre);
            strContenido = strContenido.Replace("_Nombre_convocado_demandado_", demandadoConvocado.Nombre);
            strContenido = strContenido.Replace("_Fecha_radicado_FFIE_", defPrincial.FechaRadicadoFfie != null ? Convert.ToDateTime(defPrincial.FechaRadicadoFfie).ToString("dd/MM/yyyy") : defPrincial.FechaRadicadoFfie.ToString());

            strContenido = strContenido.Replace("_Numero_radicado_FFIE_", defPrincial.NumeroRadicadoFfie);

            strContenido = strContenido.Replace("_Medio_Control_Accion_evitar_", demandadoConvocado.MedioControlAccion);
            strContenido = strContenido.Replace("_Proxima_actuacion_requerida_", defPrincial.DefensaJudicialSeguimiento.Count() == 0 ? "" : defPrincial.DefensaJudicialSeguimiento.FirstOrDefault().ProximaActuacion);
            strContenido = strContenido.Replace("_Fecha_vencimientos_terminos_proxima_actuacion_requerida_", defPrincial.DefensaJudicialSeguimiento.Count() == 0 ? "" : defPrincial.DefensaJudicialSeguimiento.FirstOrDefault().FechaVencimiento != null ? Convert.ToDateTime(defPrincial.DefensaJudicialSeguimiento.FirstOrDefault().FechaVencimiento).ToString("dd/MM/yyyy") : defPrincial.DefensaJudicialSeguimiento.FirstOrDefault().FechaVencimiento.ToString());

            strContenido = strContenido.Replace("_Caducidad_o_Prescripcion_", demandadoConvocado.CaducidadPrescripcion != null ? Convert.ToDateTime(demandadoConvocado.CaducidadPrescripcion).ToString("dd/MM/yyyy") : demandadoConvocado.CaducidadPrescripcion.ToString());

            strContenido = strContenido.Replace("_Pretensiones_", defPrincial.Pretensiones);
            strContenido = strContenido.Replace("_Cuantia_Perjuicios_", (defPrincial.CuantiaPerjuicios != null) ? defPrincial.CuantiaPerjuicios.ToString() : "0");
            strContenido = strContenido.Replace("_Antecedentes_", defPrincial.FichaEstudio.Count() == 0 ? "" : defPrincial.FichaEstudio.FirstOrDefault().Antecedentes);
            strContenido = strContenido.Replace("_Hechos_relevantes_", defPrincial.FichaEstudio.Count() == 0 ? "" : defPrincial.FichaEstudio.FirstOrDefault().HechosRelevantes);


            strContenido = strContenido.Replace("_Caducidad_o_Prescripcion_", demandadoConvocado.CaducidadPrescripcion != null ? Convert.ToDateTime(demandadoConvocado.CaducidadPrescripcion).ToString("dd/MM/yyyy") : demandadoConvocado.CaducidadPrescripcion.ToString());

            strContenido = strContenido.Replace("_Jurisprudencia_Doctrina_", defPrincial.FichaEstudio.Count() == 0 ? "" : defPrincial.FichaEstudio.FirstOrDefault().JurisprudenciaDoctrina);
            strContenido = strContenido.Replace("_Decision_Comite_casos_anteriores_Directrices_Conciliacion_", defPrincial.FichaEstudio.Count() == 0 ? "" : defPrincial.FichaEstudio.FirstOrDefault().DecisionComiteDirectrices);
            strContenido = strContenido.Replace("_Analisis_juridico_", defPrincial.FichaEstudio.Count() == 0 ? "" : defPrincial.FichaEstudio.FirstOrDefault().AnalisisJuridico);
            strContenido = strContenido.Replace("_URL_material_probatorio_", defPrincial.FichaEstudio.Count() == 0 ? "" : defPrincial.FichaEstudio.FirstOrDefault().RutaSoporte);
            strContenido = strContenido.Replace("_Recomendaciones_", defPrincial.FichaEstudio.Count() == 0 ? "" : defPrincial.FichaEstudio.FirstOrDefault().Recomendaciones);
            strContenido = strContenido.Replace("_Abogado_elabora_estudio_", defPrincial.FichaEstudio.Count() == 0 ? "" : defPrincial.FichaEstudio.FirstOrDefault().Abogado);

            //Historial de Actuaciones
            int contadorActuacion = 1;
            foreach (var actuacion in actuaciones)
            {
                plantillatrActuaciones = plantillatrActuaciones.Replace("_contador_", contadorActuacion.ToString());
                plantillatrActuaciones = plantillatrActuaciones.Replace("_Fecha_Actualizacion_Actuacion_", actuacion.FechaModificacion == null ? Convert.ToDateTime(actuacion.FechaCreacion).ToString("dd/MM/yyyy") : Convert.ToDateTime(actuacion.FechaModificacion).ToString("dd/MM/yyyy"));
                plantillatrActuaciones = plantillatrActuaciones.Replace("_Numero_Actuacion_", actuacion.NumeroActuacion);
                plantillatrActuaciones = plantillatrActuaciones.Replace("_Actuacion_", actuacion.ActuacionAdelantada);

                contadorActuacion++;

            }
            if (actuaciones.Count() > 0)
            {
                strContenido = strContenido.Replace("_plantillaActuaciones_", plantillatrActuaciones);

            }
            else
            {
                strContenido = strContenido.Replace("_plantillaActuaciones_", "");
            }

            /*strContenido = strContenido.Replace("_Proxima_actuacion_requerida_", controversiaActuacion.ProximaActuacionCodigo);
            strContenido = strContenido.Replace("_Observaciones_", controversiaActuacion.Observaciones);
            strContenido = strContenido.Replace("_URL_soporte_", controversiaActuacion.RutaSoporte);

            //Resumen de la propuesta de reclamación ante la aseguradora:
            strContenido = strContenido.Replace("Actuación de la reclamación 1", "");
            strContenido = strContenido.Replace("_Estado_avance_reclamacion_", "PENDIENTE");
            strContenido = strContenido.Replace("_Fecha_actuacion_adelantada_", actuacionSeguimiento.FechaActuacionAdelantada != null ? Convert.ToDateTime(actuacionSeguimiento.FechaActuacionAdelantada).ToString("dd/MM/yyyy") : actuacionSeguimiento.FechaActuacionAdelantada.ToString());
            strContenido = strContenido.Replace("_Actuacion_adelantada_", actuacionSeguimiento.ActuacionAdelantada);
            strContenido = strContenido.Replace("_Proxima_actuacion_requerida_", actuacionSeguimiento.ProximaActuacion);
            strContenido = strContenido.Replace("_Observaciones_", actuacionSeguimiento.Observaciones);
            strContenido = strContenido.Replace("_URL_soporte_", actuacionSeguimiento.RutaSoporte);
            strContenido = strContenido.Replace("_reclamacion_resultado_definitivo_cerrado_ante_aseguradora_", Convert.ToBoolean(actuacionSeguimiento.EsResultadoDefinitivo).ToString());
            

            //datos exclusivos interventoria

            UsuarioPerfil UsuarioPerfil = _context.UsuarioPerfil.Where(y => y.Usuario.Email == usuario).Include(y => y.Perfil).FirstOrDefault();

            Perfil perfil = null;

            if (UsuarioPerfil != null)
            {
                perfil = _context.Perfil.Where(y => y.PerfilId == UsuarioPerfil.PerfilId).FirstOrDefault();

            }
            if (UsuarioPerfil != null)
            {
                strContenido = strContenido.Replace("_Cargo_Usuario_", perfil.Nombre);
            }
            strContenido = strContenido.Replace("_Nombre_Supervisor_", "_Nombre_Supervisor_");
            */
            return strContenido;

        }
        public async Task<Respuesta> CreateOrEditFichaEstudio(FichaEstudio fichaEstudio)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Ficha_Estudio, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            FichaEstudio fichaEstudioBD = null;
            try
            {

                if (string.IsNullOrEmpty(fichaEstudio.FichaEstudioId.ToString()) || fichaEstudio.FichaEstudioId == 0)
                {
                    //Auditoria
                    strCrearEditar = "REGISTRAR FICHA ESTUDIO";
                    fichaEstudio.FechaCreacion = DateTime.Now;
                    fichaEstudio.UsuarioCreacion = fichaEstudio.UsuarioCreacion;
                    //fichaEstudio.DefensaJudicialId = fichaEstudio.DefensaJudicialId;
                    fichaEstudio.EsCompleto = ValidarRegistroCompletoFichaEstudio(fichaEstudio);
                    fichaEstudio.Eliminado = false;
                    _context.FichaEstudio.Add(fichaEstudio);
                }
                else
                {
                    strCrearEditar = "EDIT FICHA ESTUDIO";
                    fichaEstudioBD = _context.FichaEstudio.Find(fichaEstudio.FichaEstudioId);

                    //Auditoria
                    fichaEstudioBD.UsuarioModificacion = fichaEstudio.UsuarioModificacion;
                    fichaEstudioBD.Eliminado = false;

                    //Registros
                    fichaEstudioBD.AnalisisJuridico = fichaEstudio.AnalisisJuridico;
                    fichaEstudioBD.Antecedentes = fichaEstudio.Antecedentes;
                    fichaEstudioBD.DecisionComiteDirectrices = fichaEstudio.DecisionComiteDirectrices;
                    fichaEstudioBD.DefensaJudicial = fichaEstudio.DefensaJudicial;
                    fichaEstudioBD.TipoActuacionCodigo = fichaEstudio.TipoActuacionCodigo;
                    fichaEstudioBD.UsuarioModificacion = fichaEstudio.UsuarioModificacion;
                    fichaEstudioBD.Abogado = fichaEstudio.Abogado;
                    fichaEstudioBD.RutaSoporte = fichaEstudio.RutaSoporte;
                    fichaEstudioBD.Recomendaciones = fichaEstudio.Recomendaciones;
                    fichaEstudioBD.RecomendacionFinalComite = fichaEstudio.RecomendacionFinalComite;
                    fichaEstudioBD.JurisprudenciaDoctrina = fichaEstudio.JurisprudenciaDoctrina;

                    fichaEstudioBD.EsAprobadoAperturaProceso = fichaEstudio.EsAprobadoAperturaProceso;
                    fichaEstudioBD.EsPresentadoAnteComiteFfie = fichaEstudio.EsPresentadoAnteComiteFfie;

                    fichaEstudio.EsCompleto = ValidarRegistroCompletoFichaEstudio(fichaEstudioBD);

                    _context.FichaEstudio.Update(fichaEstudioBD);

                }

                _context.SaveChanges();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = fichaEstudio,
                    Code = ConstantMessagesJudicialDefense.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.OperacionExitosa, idAccion, fichaEstudio.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = fichaEstudio,
                    Code = ConstantMessagesJudicialDefense.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.Error, idAccion, fichaEstudio.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        private bool ValidarRegistroCompletoFichaEstudio(FichaEstudio fichaEstudio)
        {
            if (string.IsNullOrEmpty(fichaEstudio.Antecedentes)
                || string.IsNullOrEmpty(fichaEstudio.HechosRelevantes)
                || string.IsNullOrEmpty(fichaEstudio.JurisprudenciaDoctrina)
                || string.IsNullOrEmpty(fichaEstudio.DecisionComiteDirectrices)
                || string.IsNullOrEmpty(fichaEstudio.AnalisisJuridico)
                || string.IsNullOrEmpty(fichaEstudio.Recomendaciones)
                || string.IsNullOrEmpty(fichaEstudio.TipoActuacionCodigo)
                || (fichaEstudio.EsPresentadoAnteComiteFfie == null)
                || (fichaEstudio.EsAprobadoAperturaProceso == null)
                || string.IsNullOrEmpty(fichaEstudio.RutaSoporte)
                || (fichaEstudio.EsActuacionTramiteComite == null)
            )
            {
                return false;
            }
            else if ((bool)fichaEstudio.EsPresentadoAnteComiteFfie)
            {
                if ((fichaEstudio.FechaComiteDefensa == null) || string.IsNullOrEmpty(fichaEstudio.RecomendacionFinalComite))
                {
                    return false;
                }
            }

            return true;
        }
        public async Task<List<ProyectoGrilla>> GetListProyects(/*int pContratoId*/ int pProyectoId)
        {
            //Listar Los proyecto segun caso de uso solo trae los ue estado
            //estado de registro “Completo”, que tienen viabilidad jurídica y técnica
            List<ProyectoGrilla> ListProyectoGrilla = new List<ProyectoGrilla>();
            List<Proyecto> ListProyectos = new List<Proyecto>();
            try
            {
                ListProyectos = await _context.Proyecto.Where(
                         r => !(bool)r.Eliminado &&
                         r.EstadoJuridicoCodigo == ConstantCodigoEstadoJuridico.Aprobado
                         && (bool)r.RegistroCompleto
                         && r.ProyectoId == pProyectoId
                         //Se quitan los proyectos que ya esten vinculados a una contratacion
                         )
                                 .Include(r => r.ContratacionProyecto).ThenInclude(r => r.Contratacion)
                                 .Include(r => r.Sede).Include(r => r.InstitucionEducativa)
                                 .Include(r => r.LocalizacionIdMunicipioNavigation).Distinct().ToListAsync();

                //List<Localicacion> Municipios = new List<Localicacion>();

                //if (!string.IsNullOrEmpty(pDepartamento) && string.IsNullOrEmpty(pRegion) && string.IsNullOrEmpty(pMunicipio))
                //{
                //    Municipios = await _commonService.GetListMunicipioByIdDepartamento(pDepartamento);
                //}

                //if (!string.IsNullOrEmpty(pRegion) && string.IsNullOrEmpty(pDepartamento) && string.IsNullOrEmpty(pMunicipio))
                //{
                //    List<Localizacion> Departamentos = _context.Localizacion.Where(r => r.IdPadre == pRegion).ToList();
                //    foreach (var dep in Departamentos)
                //    {
                //        Municipios.AddRange(await _commonService.GetListMunicipioByIdDepartamento(dep.LocalizacionId));
                //    }
                //}
                //if (Municipios.Count() > 0)
                //{
                //    //ListContratacion.RemoveAll(item => LisIdContratacion.Contains(item.ContratacionId));
                //    ListProyectos.RemoveAll(item => !Municipios.Select(r => r.LocalizacionId).Contains(item.LocalizacionIdMunicipio));
                //}

                //List<Proyecto> ListaProyectosRemover = new List<Proyecto>();
                //foreach (var Proyecto in ListProyectos)
                //{
                //    foreach (var ContratacionProyecto in Proyecto.ContratacionProyecto)
                //    {
                //        if (ContratacionProyecto.Contratacion.EstadoSolicitudCodigo
                //            != ConstanCodigoEstadoSolicitudContratacion.Rechazado)
                //        {
                //            ListaProyectosRemover.Add(Proyecto);
                //        }
                //        else
                //        {
                //            if (Proyecto.ContratacionProyecto.Where(r => r.ProyectoId == Proyecto.ProyectoId).Count() > 1)
                //            {
                //                ListaProyectosRemover.Add(Proyecto);
                //            }
                //        }
                //    }
                //}

                //foreach (var proyecto in ListaProyectosRemover.Distinct())
                //{
                //    ListProyectos.Remove(proyecto);
                //}

                List<Dominio> ListTipoSolicitud = await _commonService.GetListDominioByIdTipoDominio((int)EnumeratorTipoDominio.Tipo_de_Solicitud_Obra_Interventorias);

                //Lista para Dominio intervencio
                List<Dominio> ListTipoIntervencion = await _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_de_Intervencion && (bool)r.Activo).ToListAsync();

                List<Localizacion> ListDepartamentos = await _context.Localizacion.Where(r => r.Nivel == 1).ToListAsync();

                List<Localizacion> ListRegiones = await _context.Localizacion.Where(r => r.Nivel == 3).ToListAsync();
                //departamneto 
                //    Region  
                List<Contratacion> ListContratacion = await _context.Contratacion.Where(r => !(bool)r.Eliminado).ToListAsync();

                string strProyectoUrlMonitoreo = string.Empty;


                foreach (var proyecto in ListProyectos)
                {
                    if (!string.IsNullOrEmpty(proyecto.TipoIntervencionCodigo))
                    {
                        Localizacion departamento = ListDepartamentos.Find(r => r.LocalizacionId == proyecto.LocalizacionIdMunicipioNavigation.IdPadre);

                        if (proyecto.UrlMonitoreo != null)
                            strProyectoUrlMonitoreo = proyecto.UrlMonitoreo;

                        try
                        {
                            ProyectoGrilla proyectoGrilla = new ProyectoGrilla
                            {
                                TipoIntervencion = ListTipoIntervencion.Find(r => r.Codigo == proyecto.TipoIntervencionCodigo).Nombre,
                                LlaveMen = proyecto.LlaveMen,
                                Departamento = departamento.Descripcion,
                                Region = ListRegiones.Find(r => r.LocalizacionId == departamento.IdPadre).Descripcion,
                                //  Departamento = _commonService.GetNombreDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio),
                                // Municipio = _commonService.GetNombreLocalizacionByLocalizacionId(proyecto.LocalizacionIdMunicipio),
                                Municipio = proyecto.LocalizacionIdMunicipioNavigation.Descripcion,
                                //InstitucionEducativa = _context.InstitucionEducativaSede.Find(proyecto.InstitucionEducativaId).Nombre,
                                //Sede = _context.InstitucionEducativaSede.Find(proyecto.SedeId).Nombre,
                                InstitucionEducativa = proyecto.InstitucionEducativa.CodigoDane,
                                CodigoDane = proyecto.InstitucionEducativa.Nombre,
                                Sede = proyecto.Sede.Nombre,
                                SedeCodigo = proyecto.Sede.CodigoDane,
                                ProyectoId = proyecto.ProyectoId,


                                //URLMonitoreo = strProyectoUrlMonitoreo,
                                //ContratoId = 0, //await getContratoIdByProyectoId(proyecto.ProyectoId),


                            };

                            //r.TipoIntervencionCodigo == (string.IsNullOrEmpty(pTipoIntervencion) ? r.TipoIntervencionCodigo : pTipoIntervencion) &&
                            //List<Contrato> lstContratos = _context.Contrato.Where(r => r.ContratoId == pContratoId).ToList();


                            foreach (var item in proyecto.ContratacionProyecto)
                            {
                                item.Contratacion = ListContratacion.Where(r => r.ContratacionId == item.ContratacionId).FirstOrDefault();
                                //item.Contratacion = ListContratacion.Where(r => r.ContratacionId == contrato.ContratacionId ).FirstOrDefault();

                                //item.Contratacion= item.Contratacion.wh(r => r.ContratacionId == item.ContratacionId ).FirstOrDefault();

                                Contratista contratista = null;

                                if (item.Contratacion != null)
                                {
                                    contratista = _context.Contratista.Where(r => r.ContratistaId == item.Contratacion.ContratistaId).FirstOrDefault();

                                    if (contratista != null)
                                        proyectoGrilla.NombreContratista = contratista.Nombre;
                                    else
                                        proyectoGrilla.NombreContratista = "";

                                    if (!string.IsNullOrEmpty(item.Contratacion.TipoSolicitudCodigo))
                                    {
                                        if (item.Contratacion.TipoSolicitudCodigo == "1")
                                        {
                                            proyectoGrilla.TieneObra = true;
                                        }
                                        if (item.Contratacion.TipoSolicitudCodigo == "2")
                                        {
                                            proyectoGrilla.TieneInterventoria = true;
                                        }
                                    }
                                }
                            }
                            ListProyectoGrilla.Add(proyectoGrilla);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                };

            }
            catch (Exception ex)
            {
                return ListProyectoGrilla.OrderByDescending(r => r.ProyectoId).ToList();
            }
            //ListProyectoGrilla = ListProyectoGrilla.Where(r => r.ContratoId != 0).ToList();

            foreach (ProyectoGrilla element in ListProyectoGrilla)
            {
                //element.ContratoId = await getContratoIdByProyectoId(element.ProyectoId);
            }

            return ListProyectoGrilla.OrderByDescending(r => r.ProyectoId).ToList();
        }
        public async Task<Respuesta> EliminarDefensaJudicial(int pDefensaJudicialId, string pUsuarioModifico)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Defensa_Judicial, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;

            DefensaJudicial defensaJudicial = null;

            try
            {
                defensaJudicial = await _context.DefensaJudicial.Where(d => d.DefensaJudicialId == pDefensaJudicialId).FirstOrDefaultAsync();

                if (defensaJudicial != null)
                {
                    strCrearEditar = "ELIMINAR DEFENSA JUDICIAL";
                    defensaJudicial.FechaModificacion = DateTime.Now;
                    defensaJudicial.UsuarioModificacion = pUsuarioModifico;
                    //controversiaActuacion.UsuarioCreacion = disponibilidadPresupuestal.UsuarioCreacion;
                    defensaJudicial.Eliminado = true;
                    _context.DefensaJudicial.Update(defensaJudicial);

                    _context.SaveChanges();

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = defensaJudicial,
                    Code = ConstantMessagesJudicialDefense.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.EliminacionExitosa, idAccion, defensaJudicial.UsuarioModificacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = defensaJudicial,
                    Code = ConstantMessagesJudicialDefense.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.Error, idAccion, defensaJudicial.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> CambiarEstadoDefensaJudicial(int pDefensaJudicialId, string pCodigoEstado, string pUsuarioModifica)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Proceso, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                //DefensaJudicial /*defensaJudicial*/
                DefensaJudicial defensaJudicialOld = _context.DefensaJudicial.Find(pDefensaJudicialId);
                defensaJudicialOld.UsuarioModificacion = pUsuarioModifica;
                defensaJudicialOld.FechaModificacion = DateTime.Now;
                defensaJudicialOld.EstadoProcesoCodigo = pCodigoEstado;

                _context.SaveChanges();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantMessagesJudicialDefense.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.OperacionExitosa, idAccion, pUsuarioModifica, "CAMBIAR ESTADO PROCESO DEFENSA JUDICIAL")
                };
            }
            catch (Exception ex)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Code = ConstantMessagesJudicialDefense.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.Error, idAccion, pUsuarioModifica, ex.InnerException.ToString())
                };
            }

        }
        public async Task<List<GrillaProcesoDefensaJudicial>> ListGrillaProcesosDefensaJudicial()
        {
            List<GrillaProcesoDefensaJudicial> ListDefensaJudicialGrilla = new List<GrillaProcesoDefensaJudicial>();

            List<DefensaJudicial> ListDefensaJudicial = await _context.DefensaJudicial.Where(r => (bool)r.Eliminado == false).Distinct().Include(x => x.FichaEstudio).ToListAsync();

            foreach (var defensaJudicial in ListDefensaJudicial)
            {
                try
                {
                    string TipoAccionNombre = "";
                    DefensaJudicialSeguimiento defensaJudicialSeguimiento = _context.DefensaJudicialSeguimiento.Where(r => r.DefensaJudicialId == defensaJudicial.DefensaJudicialId).FirstOrDefault();


                    Dominio TipoAccion;
                    Dominio EstadoSolicitudCodigoContratoPoliza;

                    TipoAccion = await _commonService.GetDominioByNombreDominioAndTipoDominio(defensaJudicial.TipoAccionCodigo, (int)EnumeratorTipoDominio.Tipo_accion_judicial);
                    if (TipoAccion != null)
                        TipoAccionNombre = TipoAccion.Nombre;

                    bool bRegistroCompleto = false;
                    string strRegistroCompleto = "Incompleto";
                    strRegistroCompleto = (bool)defensaJudicial.EsCompleto ? "Completo" : "Incompleto";


                    GrillaProcesoDefensaJudicial defensaJudicialGrilla = new GrillaProcesoDefensaJudicial
                    {
                        DefensaJudicialId = defensaJudicial.DefensaJudicialId,
                        FechaRegistro = defensaJudicial.FechaCreacion.ToString("dd/MM/yyyy"),
                        LegitimacionPasivaActiva = (bool)defensaJudicial.EsLegitimacionActiva ? "Activa" : "Pasiva",
                        NumeroProceso = defensaJudicial.NumeroProceso,
                        TipoAccionCodigo = defensaJudicial.TipoAccionCodigo,
                        TipoAccion = TipoAccionNombre,
                        EstadoProceso = _commonService.GetDominioByNombreDominioAndTipoDominio(defensaJudicial.EstadoProcesoCodigo, (int)EnumeratorTipoDominio.Estados_Defensa_Judicial).Result.Nombre,
                        EstadoProcesoCodigo = defensaJudicial.EstadoProcesoCodigo,

                        RegistroCompletoNombre = strRegistroCompleto,
                        TipoProceso = _commonService.GetDominioByNombreDominioAndTipoDominio(defensaJudicial.TipoProcesoCodigo, (int)EnumeratorTipoDominio.Procesos_judiciales).Result.Nombre,
                        TipoProcesoCodigo = defensaJudicial.TipoProcesoCodigo,
                        VaAProcesoJudicial = defensaJudicial.FichaEstudio.Count() == 0 ? false : defensaJudicial.FichaEstudio.FirstOrDefault().EsActuacionTramiteComite,
                        FechaCreacion = defensaJudicial.FechaCreacion,
                        CuantiaPerjuicios = defensaJudicial.CuantiaPerjuicios,
                        EsprocesoResultadoDefinitivo = defensaJudicialSeguimiento != null ? defensaJudicialSeguimiento.EsprocesoResultadoDefinitivo : false,
                    };

                    //if (!(bool)proyecto.RegistroCompleto)
                    //{
                    //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
                    //}
                    ListDefensaJudicialGrilla.Add(defensaJudicialGrilla);
                }
                catch (Exception e)
                {
                    GrillaProcesoDefensaJudicial defensaJudicialGrilla = new GrillaProcesoDefensaJudicial
                    {
                        DefensaJudicialId = defensaJudicial.DefensaJudicialId,
                        FechaRegistro = e.ToString(),
                        LegitimacionPasivaActiva = e.InnerException.ToString(),
                        NumeroProceso = "ERROR",
                        TipoAccionCodigo = "ERROR",
                        TipoAccion = "ERROR",
                        EstadoProceso = "ERROR",
                        EstadoProcesoCodigo = "ERROR",

                        RegistroCompletoNombre = "ERROR",
                        TipoProceso = "ERROR",
                        TipoProcesoCodigo = "ERROR",

                    };
                    ListDefensaJudicialGrilla.Add(defensaJudicialGrilla);
                }
            }
            return ListDefensaJudicialGrilla.OrderByDescending(x => x.FechaCreacion).ToList();

        }

        /*autor: jflorez
          descripción: trae listado de contratos
          impacto: CU 4.2.2*/
        public async Task<List<Contrato>> GetListContract()
        {
            var contratos = _context.Contrato.Where(x =>//x.UsuarioInterventoria==userID
             !(bool)x.Eliminado
            ).ToList();

            return contratos;
        }


        public async Task<List<ProyectoGrilla>> GetListProyectsByContract(int pContratoId)
        {
            List<ProyectoGrilla> ListProyectoGrilla = new List<ProyectoGrilla>();
            var contrato = _context.Contrato.Find(pContratoId);
            var proyecto = _context.Contratacion.Where(x => x.ContratacionId == contrato.ContratacionId).
                Include(x => x.ContratacionProyecto).
                    ThenInclude(y => y.Proyecto).
                    ThenInclude(y => y.InstitucionEducativa).
                Include(x => x.ContratacionProyecto).
                    ThenInclude(y => y.Proyecto).
                    ThenInclude(y => y.Sede).
                Include(x => x.Contratista)
                .FirstOrDefault();
            foreach (var item in proyecto.ContratacionProyecto)
            {
                ListProyectoGrilla.Add(new ProyectoGrilla
                {
                    NumeroContrato = contrato.NumeroContrato,
                    NombreContratista = proyecto.Contratista.Nombre,
                    TieneObra = proyecto.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString(),
                    TieneInterventoria = proyecto.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Interventoria.ToString(),
                    ProyectoId = item.ProyectoId,
                    ContratacionProyectoId = item.ContratacionProyectoId,
                    InstitucionEducativa = item.Proyecto.InstitucionEducativa.Nombre,
                    Sede = item.Proyecto.Sede.Nombre,
                    CodigoDane = item.Proyecto.InstitucionEducativa.CodigoDane,
                    SedeCodigo = item.Proyecto.Sede.CodigoDane
                });

            }
            return ListProyectoGrilla;
        }

        public async Task<List<DefensaJudicialSeguimiento>> GetActuacionesByDefensaJudicialID(int pDefensaJudicialId)
        {
            return _context.DefensaJudicialSeguimiento.Where(x => x.DefensaJudicialId == pDefensaJudicialId).ToList();//diferente a finalizado
        }

        public async Task<Respuesta> EnviarAComite(int pDefensaJudicialId, string pUsuarioModifico)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_Defensa_Judicial, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;

            DefensaJudicial defensaJudicial = null;

            try
            {
                defensaJudicial = await _context.DefensaJudicial.Where(d => d.DefensaJudicialId == pDefensaJudicialId).FirstOrDefaultAsync();

                if (defensaJudicial != null)
                {
                    strCrearEditar = "ENVIAR A COMITE DEFENSA JUDICIAL";
                    defensaJudicial.FechaModificacion = DateTime.Now;
                    defensaJudicial.UsuarioModificacion = pUsuarioModifico;
                    defensaJudicial.EstadoProcesoCodigo = "2";//cambiar esto                    
                    defensaJudicial.Eliminado = false;
                    _context.DefensaJudicial.Update(defensaJudicial);

                    _context.SaveChanges();

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = defensaJudicial,
                    Code = ConstantMessagesJudicialDefense.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.OperacionExitosa, idAccion, pUsuarioModifico, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = defensaJudicial,
                    Code = ConstantMessagesJudicialDefense.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.Error, idAccion, pUsuarioModifico, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> DeleteActuation(int pId, string pUsuarioModifico)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Defensa_Judicial, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;

            DefensaJudicialSeguimiento defensaJudicial = null;

            try
            {
                defensaJudicial = await _context.DefensaJudicialSeguimiento.Where(d => d.DefensaJudicialSeguimientoId == pId).FirstOrDefaultAsync();

                if (defensaJudicial != null)
                {
                    strCrearEditar = "ELIMINAR ACTUACION DE COMITE DEFENSA JUDICIAL";
                    defensaJudicial.FechaModificacion = DateTime.Now;
                    defensaJudicial.UsuarioModificacion = pUsuarioModifico;
                    defensaJudicial.Eliminado = true;
                    _context.DefensaJudicialSeguimiento.Update(defensaJudicial);

                    _context.SaveChanges();

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = defensaJudicial,
                    Code = ConstantMessagesJudicialDefense.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.EliminacionExitosa, idAccion, pUsuarioModifico, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = defensaJudicial,
                    Code = ConstantMessagesJudicialDefense.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.Error, idAccion, pUsuarioModifico, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> FinalizeActuation(int pId, string pUsuarioModifico)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Defensa_Judicial, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;

            DefensaJudicialSeguimiento defensaJudicialSeguimiento = null;

            try
            {
                defensaJudicialSeguimiento = await _context.DefensaJudicialSeguimiento.Where(d => d.DefensaJudicialSeguimientoId == pId).FirstOrDefaultAsync();

                if (defensaJudicialSeguimiento != null)
                {
                    strCrearEditar = "FINALIZAR ACTUACION DE COMITE DEFENSA JUDICIAL";
                    defensaJudicialSeguimiento.FechaModificacion = DateTime.Now;
                    defensaJudicialSeguimiento.UsuarioModificacion = pUsuarioModifico;
                    defensaJudicialSeguimiento.Eliminado = false;
                    defensaJudicialSeguimiento.EstadoProcesoCodigo = ConstantCodigoEstadoProcesoDefensaJudicialSeguimiento.Actuacion_finalizada;//cambiar
                    _context.DefensaJudicialSeguimiento.Update(defensaJudicialSeguimiento);

                    _context.SaveChanges();

                    //enviar correo
                    if (defensaJudicialSeguimiento.EsRequiereSupervisor == true)
                    {
                        await SendMailSupervisor(pId);
                    }
                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = defensaJudicialSeguimiento,
                    Code = ConstantMessagesJudicialDefense.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.OperacionExitosa, idAccion, pUsuarioModifico, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = defensaJudicialSeguimiento,
                    Code = ConstantMessagesJudicialDefense.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.Error, idAccion, pUsuarioModifico, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        public async Task<Respuesta> CerrarProceso(int pDefensaJudicialId, string pUsuarioModifico)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Enviar_Defensa_Judicial, (int)EnumeratorTipoDominio.Acciones);
            string strCrearEditar = string.Empty;

            DefensaJudicial defensaJudicial = null;

            try
            {
                defensaJudicial = await _context.DefensaJudicial.Where(d => d.DefensaJudicialId == pDefensaJudicialId).FirstOrDefaultAsync();

                if (defensaJudicial != null)
                {
                    strCrearEditar = "CERRAR COMITE DEFENSA JUDICIAL";
                    defensaJudicial.FechaModificacion = DateTime.Now;
                    defensaJudicial.UsuarioModificacion = pUsuarioModifico;
                    defensaJudicial.EstadoProcesoCodigo = "10";//cambiar esto                    
                    defensaJudicial.Eliminado = false;
                    _context.DefensaJudicial.Update(defensaJudicial);

                    _context.SaveChanges();

                }

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = defensaJudicial,
                    Code = ConstantMessagesJudicialDefense.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.OperacionExitosa, idAccion, pUsuarioModifico, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = defensaJudicial,
                    Code = ConstantMessagesJudicialDefense.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.Error, idAccion, pUsuarioModifico, ex.InnerException.ToString().Substring(0, 500))
                };
            }
        }

        private bool ValidarRegistroCompletoDemandadoConvocado(DemandadoConvocado demandadoConvocado)
        {
            bool retorno = true;

            //Pasivo - acordeón demandado
            if (demandadoConvocado.EsDemandado == true)
            {
                //valido detalle
                if (string.IsNullOrEmpty(demandadoConvocado.Nombre) || string.IsNullOrEmpty(demandadoConvocado.TipoIdentificacionCodigo) || string.IsNullOrEmpty(demandadoConvocado.NumeroIdentificacion))
                {
                    retorno = false;
                }
            } else if (demandadoConvocado.EsConvocado == true)
            {
                if (demandadoConvocado.ExisteConocimiento == true)
                {
                    if (string.IsNullOrEmpty(demandadoConvocado.Nombre)
                        || string.IsNullOrEmpty(demandadoConvocado.TipoIdentificacionCodigo)
                        || string.IsNullOrEmpty(demandadoConvocado.NumeroIdentificacion)
                        || string.IsNullOrEmpty(demandadoConvocado.ConvocadoAutoridadDespacho)
                        || (demandadoConvocado.LocalizacionIdMunicipio == null)
                        || (demandadoConvocado.FechaRadicado == null)
                        || (demandadoConvocado.ExisteConocimiento == null)
                        || string.IsNullOrEmpty(demandadoConvocado.MedioControlAccion)
                        || string.IsNullOrEmpty(demandadoConvocado.EtapaProcesoFfiecodigo)
                        || (demandadoConvocado.CaducidadPrescripcion == null))
                    {
                        return false;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(demandadoConvocado.Nombre) || string.IsNullOrEmpty(demandadoConvocado.TipoIdentificacionCodigo) || string.IsNullOrEmpty(demandadoConvocado.NumeroIdentificacion) || (demandadoConvocado.ExisteConocimiento == null))
                    {
                        retorno = false;
                    }
                }
            }
            else
            {
                //valido detalle
                if (string.IsNullOrEmpty(demandadoConvocado.Nombre) || string.IsNullOrEmpty(demandadoConvocado.TipoIdentificacionCodigo) || string.IsNullOrEmpty(demandadoConvocado.NumeroIdentificacion) ||
                    string.IsNullOrEmpty(demandadoConvocado.Direccion) || string.IsNullOrEmpty(demandadoConvocado.Email))
                {
                    retorno = false;
                }
            }

            return retorno;
        }

        private bool ValidarRegistroCompletoDemandanteConvocante(DemandanteConvocante demandanteConvocante)
        {
            bool retorno = true;

            //valido detalle
            if (string.IsNullOrEmpty(demandanteConvocante.Nombre) || string.IsNullOrEmpty(demandanteConvocante.TipoIdentificacionCodigo) || string.IsNullOrEmpty(demandanteConvocante.NumeroIdentificacion) ||
                string.IsNullOrEmpty(demandanteConvocante.Direccion) || string.IsNullOrEmpty(demandanteConvocante.Email))
            {
                retorno = false;
            }

            return retorno;
        }

        private bool ValidarRegistroCompletoDefensaJudicialSeguimiento(DefensaJudicialSeguimiento defensaJudicialSeguimiento)
        {
            bool retorno = true;

            //valido detalle
            if (string.IsNullOrEmpty(defensaJudicialSeguimiento.EstadoProcesoCodigo)
                || string.IsNullOrEmpty(defensaJudicialSeguimiento.ActuacionAdelantada)
                || string.IsNullOrEmpty(defensaJudicialSeguimiento.ProximaActuacion)
                || defensaJudicialSeguimiento.FechaVencimiento == null
                || defensaJudicialSeguimiento.EsRequiereSupervisor == null
                || string.IsNullOrEmpty(defensaJudicialSeguimiento.Observaciones)
                || defensaJudicialSeguimiento.EsprocesoResultadoDefinitivo == null
                || string.IsNullOrEmpty(defensaJudicialSeguimiento.RutaSoporte))
            {
                retorno = false;
            }

            return retorno;
        }

        public async Task<Respuesta> CreateOrEditDefensaJudicialSeguimiento(DefensaJudicialSeguimiento defensaJudicialSeguimiento)
        {
            Respuesta respuesta = new Respuesta();
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Create_Edit_Defensa_Judicial_Seguimiento, (int)EnumeratorTipoDominio.Acciones);

            string strCrearEditar = string.Empty;
            DefensaJudicialSeguimiento defensaJudicialSeguimientoOld = null;
            try
            {
                if (defensaJudicialSeguimiento.DefensaJudicialSeguimientoId == 0)
                {
                    int consecutivo = _context.DefensaJudicialSeguimiento
                                    .Where(r => r.DefensaJudicialId == defensaJudicialSeguimiento.DefensaJudicialId)
                                    .Count();
                    //Auditoria
                    strCrearEditar = "REGISTRAR DEFENSA JUDICIAL SEGUIMIENTO";
                    defensaJudicialSeguimiento.FechaCreacion = DateTime.Now;
                    defensaJudicialSeguimiento.UsuarioCreacion = defensaJudicialSeguimiento.UsuarioCreacion;
                    defensaJudicialSeguimiento.Eliminado = false;
                    defensaJudicialSeguimiento.NumeroActuacion = "ACT defensa " + (consecutivo + 1).ToString("000");
                    defensaJudicialSeguimiento.EsCompleto = ValidarRegistroCompletoDefensaJudicialSeguimiento(defensaJudicialSeguimiento);

                    _context.DefensaJudicialSeguimiento.Add(defensaJudicialSeguimiento);

                }
                else
                {
                    strCrearEditar = "EDITAR DEFENSA JUDICIAL SEGUIMIENTO";
                    defensaJudicialSeguimientoOld = _context.DefensaJudicialSeguimiento.Find(defensaJudicialSeguimiento.DefensaJudicialSeguimientoId);

                    //Auditoria
                    defensaJudicialSeguimientoOld.UsuarioModificacion = defensaJudicialSeguimiento.UsuarioModificacion;
                    defensaJudicialSeguimientoOld.Eliminado = false;

                    //Registros
                    defensaJudicialSeguimientoOld.EstadoProcesoCodigo = defensaJudicialSeguimiento.EstadoProcesoCodigo;
                    defensaJudicialSeguimientoOld.ActuacionAdelantada = defensaJudicialSeguimiento.ActuacionAdelantada;
                    defensaJudicialSeguimientoOld.ProximaActuacion = defensaJudicialSeguimiento.ProximaActuacion;
                    defensaJudicialSeguimientoOld.FechaVencimiento = defensaJudicialSeguimiento.FechaVencimiento;
                    defensaJudicialSeguimientoOld.EsRequiereSupervisor = defensaJudicialSeguimiento.EsRequiereSupervisor;
                    defensaJudicialSeguimientoOld.Observaciones = defensaJudicialSeguimiento.Observaciones;
                    defensaJudicialSeguimientoOld.EsprocesoResultadoDefinitivo = defensaJudicialSeguimiento.EsprocesoResultadoDefinitivo;
                    defensaJudicialSeguimientoOld.RutaSoporte = defensaJudicialSeguimiento.RutaSoporte;
                    defensaJudicialSeguimientoOld.EsCompleto = ValidarRegistroCompletoDefensaJudicialSeguimiento(defensaJudicialSeguimiento);

                    _context.DefensaJudicialSeguimiento.Update(defensaJudicialSeguimientoOld);

                }
                _context.Set<DefensaJudicial>().Where(r => r.DefensaJudicialId == defensaJudicialSeguimiento.DefensaJudicialId)
                           .Update(r => new DefensaJudicial()
                           {
                               FechaModificacion = DateTime.Now,
                               UsuarioModificacion = defensaJudicialSeguimiento.UsuarioCreacion,
                               EstadoProcesoCodigo = ConstanCodigoEstadosDefensaJudicial.En_desarrollo,
                           });
                //defensaJudicialBD.EstadoProcesoCodigo = "9";//en desaroolo

                _context.SaveChanges();

                return respuesta = new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Data = defensaJudicialSeguimiento,
                    Code = ConstantMessagesJudicialDefense.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.OperacionExitosa, idAccion, defensaJudicialSeguimiento.UsuarioCreacion, strCrearEditar)

                };
            }

            catch (Exception ex)
            {
                return respuesta = new Respuesta
                {
                    IsSuccessful = false,
                    IsException = true,
                    IsValidation = false,
                    Data = defensaJudicialSeguimiento,
                    Code = ConstantMessagesJudicialDefense.Error,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantMessagesJudicialDefense.Error, idAccion, defensaJudicialSeguimiento.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                };
            }

        }

        public async Task<DefensaJudicialSeguimiento> GetDefensaJudicialSeguimiento(int defensaJudicialSeguimientoId)
        {
            DefensaJudicialSeguimiento defensaJudicialSeguimiento = _context.DefensaJudicialSeguimiento.Find(defensaJudicialSeguimientoId);
            if (defensaJudicialSeguimiento != null)
            {
                DefensaJudicial defensaJudicial = _context.DefensaJudicial.Find(defensaJudicialSeguimiento.DefensaJudicialId);
                defensaJudicialSeguimiento.NumeroProceso = defensaJudicial.NumeroProceso;
                if (!String.IsNullOrEmpty(defensaJudicial.TipoAccionCodigo))
                {
                    defensaJudicialSeguimiento.TipoAccionCodigoNombre = await _commonService.GetNombreDominioByCodigoAndTipoDominio(defensaJudicial.TipoAccionCodigo, (int)EnumeratorTipoDominio.Tipo_accion_judicial);
                }
                else
                {
                    defensaJudicialSeguimiento.TipoAccionCodigoNombre = "";
                }

                if (!String.IsNullOrEmpty(defensaJudicial.JurisdiccionCodigo))
                {
                    defensaJudicialSeguimiento.JurisdiccionCodigoNombre = await _commonService.GetNombreDominioByCodigoAndTipoDominio(defensaJudicial.JurisdiccionCodigo, (int)EnumeratorTipoDominio.Jurisdiccion);
                }
                else
                {
                    defensaJudicialSeguimiento.JurisdiccionCodigoNombre = "";
                }
            }

            return defensaJudicialSeguimiento;
        }

        public async Task<Respuesta> DeleteDemandadoConvocado(int demandadoConvocadoId, string pUsuarioModificacion, int numeroDemandados)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Demandado_Convocado, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                DemandadoConvocado demandadoConvocadoOld = await _context.DemandadoConvocado.FindAsync(demandadoConvocadoId);

                if (demandadoConvocadoOld == null)
                {
                    return new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = true,
                        IsValidation = false,
                        Code = ConstanMessagesRegisterWeeklyProgress.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstanMessagesRegisterWeeklyProgress.Error, idAccion, pUsuarioModificacion, "Demandado convocado no encontrado".ToUpper())
                    };
                }

                demandadoConvocadoOld.UsuarioModificacion = pUsuarioModificacion;
                demandadoConvocadoOld.FechaModificacion = DateTime.Now;
                demandadoConvocadoOld.Eliminado = true;

                _context.Set<DefensaJudicial>().Where(r => r.DefensaJudicialId == demandadoConvocadoOld.DefensaJudicialId)
                                   .Update(r => new DefensaJudicial()
                                   {
                                       FechaModificacion = DateTime.Now,
                                       UsuarioModificacion = demandadoConvocadoOld.UsuarioCreacion,
                                       NumeroDemandados = numeroDemandados
                                   });

                await _context.SaveChangesAsync();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstanMessagesRegisterWeeklyProgress.OperacionExitosa, idAccion, pUsuarioModificacion, "ELIMINAR DEMANDADO CONVOCADO".ToUpper())
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantSesionComiteTecnico.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString())
                };
            }


        }


        public async Task<Respuesta> DeleteDemandanteConvocante(int demandanteConvocadoId, string pUsuarioModificacion, int numeroDemandantes)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Demandante_Convocante, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                DemandanteConvocante demandanteConvocanteOld = await _context.DemandanteConvocante.FindAsync(demandanteConvocadoId);

                if (demandanteConvocanteOld == null)
                {
                    return new Respuesta
                    {
                        IsSuccessful = false,
                        IsException = true,
                        IsValidation = false,
                        Code = ConstanMessagesRegisterWeeklyProgress.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstanMessagesRegisterWeeklyProgress.Error, idAccion, pUsuarioModificacion, "Demandante Convocante no encontrado".ToUpper())
                    };
                }

                demandanteConvocanteOld.UsuarioModificacion = pUsuarioModificacion;
                demandanteConvocanteOld.FechaModificacion = DateTime.Now;
                demandanteConvocanteOld.Eliminado = true;

                _context.Set<DefensaJudicial>().Where(r => r.DefensaJudicialId == demandanteConvocanteOld.DefensaJucicialId)
                                   .Update(r => new DefensaJudicial()
                                   {
                                       FechaModificacion = DateTime.Now,
                                       UsuarioModificacion = demandanteConvocanteOld.UsuarioCreacion,
                                       NumeroDemandantes = numeroDemandantes
                                   });

                await _context.SaveChangesAsync();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstanMessagesRegisterWeeklyProgress.OperacionExitosa, idAccion, pUsuarioModificacion, "ELIMINAR DEMANDADO CONVOCADO".ToUpper())
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantSesionComiteTecnico.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString())
                };
            }


        }
        public async Task<Respuesta> DeleteDefensaJudicialContratacionProyecto(int contratacionId, int defensaJudicialId, string pUsuarioModificacion, int cantContratos)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Eliminar_Defensa_Judicial_Contratacion_Proyecto, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                List<ContratacionProyecto> contratacionProyectos = _context.ContratacionProyecto.Where(r => r.ContratacionId == contratacionId).ToList();
                contratacionProyectos.ForEach(cp =>
                {
                    List<DefensaJudicialContratacionProyecto> defensaJudicialContratacionProyectos = _context.DefensaJudicialContratacionProyecto.Where(r => r.ContratacionProyectoId == cp.ContratacionProyectoId && r.DefensaJudicialId == defensaJudicialId).ToList();

                    defensaJudicialContratacionProyectos.ForEach(item =>
                    {
                        _context.Set<DefensaJudicialContratacionProyecto>().Where(r => r.DefensaJudicialContratacionProyectoId == item.DefensaJudicialContratacionProyectoId)
                                           .Update(r => new DefensaJudicialContratacionProyecto()
                                           {
                                               FechaModificacion = DateTime.Now,
                                               UsuarioModificacion = pUsuarioModificacion,
                                               Eliminado = true
                                           });
                    });
                });

                _context.Set<DefensaJudicial>().Where(r => r.DefensaJudicialId == defensaJudicialId)
                                   .Update(r => new DefensaJudicial()
                                   {
                                       FechaModificacion = DateTime.Now,
                                       UsuarioModificacion = pUsuarioModificacion,
                                       CantContratos = cantContratos
                                   });

                await _context.SaveChangesAsync();

                return new Respuesta
                {
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = ConstantSesionComiteTecnico.OperacionExitosa,
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstanMessagesRegisterWeeklyProgress.OperacionExitosa, idAccion, pUsuarioModificacion, "ELIMINAR DEFENSA JUDICIAL CONTRATACIÓN PROYECTO".ToUpper())
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
                    Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Gestionar_procesos_Defensa_Judicial, ConstantSesionComiteTecnico.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString())
                };
            }

        }

        #region Correos 
        private async Task<bool> SendMailSupervisor(int defensaJudicialSeguimientoId)
        {
            Template template = await _commonService.GetTemplateById((int)(enumeratorTemplate.Actuacion_requerida_proceso_defensa_judicial_4_2_2));
            DefensaJudicialSeguimiento defensaJudicialSeguimiento = await _context.DefensaJudicialSeguimiento.Where(d => d.DefensaJudicialSeguimientoId == defensaJudicialSeguimientoId && d.EsRequiereSupervisor == true)
                                                                    .Include(r => r.DefensaJudicial)
                                                                        .ThenInclude(r => r.DefensaJudicialContratacionProyecto)
                                                                            .ThenInclude(r => r.ContratacionProyecto)
                                                                    .FirstOrDefaultAsync();
            Contrato contrato = new Contrato();
            List<EnumeratorPerfil> perfilsEnviarCorreo =
                new List<EnumeratorPerfil>
                                          {
                                                EnumeratorPerfil.Supervisor
                                          };

            if (defensaJudicialSeguimiento != null)
            {
                if (defensaJudicialSeguimiento.DefensaJudicial.DefensaJudicialContratacionProyecto.FirstOrDefault().ContratacionProyecto != null)
                {
                    contrato = _context.Contrato.Where(r => r.ContratacionId == defensaJudicialSeguimiento.DefensaJudicial.DefensaJudicialContratacionProyecto.FirstOrDefault().ContratacionProyecto.ContratacionId).FirstOrDefault();
                }
                string strContenido = template.Contenido
                                    .Replace("[NUMERO_CONTRATO]", contrato != null ? contrato.NumeroContrato : String.Empty)
                                    .Replace("[NUMERO_PROCESO]", defensaJudicialSeguimiento.DefensaJudicial.NumeroProceso)
                                    .Replace("[PROXIMA_ACTUACION]", defensaJudicialSeguimiento.ProximaActuacion)
                                    .Replace("[FECHA_VENCIMIENTO]", ((DateTime)defensaJudicialSeguimiento.FechaVencimiento).ToString("yyyy-MM-dd"))
                                    .Replace("[FECHA_REGISTRO]", ((DateTime)defensaJudicialSeguimiento.DefensaJudicial.FechaCreacion).ToString("yyyy-MM-dd"))
                                    .Replace("[TIPO_CONTROVERSIA]", defensaJudicialSeguimiento.DefensaJudicial.EsLegitimacionActiva == true ? "Activa" : "Pasiva");

                return _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, template.Asunto);
            }

            return false;
        }
        #endregion

        #region Alertas 

        //Alerta - usuario juridica (antes de 3/2 o 1 dia de que se cumpla la Fecha de vencimiento de términos para la próxima actuación requerida)
        public async Task VencimientoTerminosDefensaJudicial()
        {
            DateTime RangoFechaCon5DiasHabiles = await _commonService.CalculardiasLaborales(5, DateTime.Now);
            DateTime RangoFechaCon4DiasHabiles = await _commonService.CalculardiasLaborales(4, DateTime.Now);
            DateTime RangoFechaCon3DiasHabiles = await _commonService.CalculardiasLaborales(3, DateTime.Now);
            DateTime RangoFechaCon2DiasHabiles = await _commonService.CalculardiasLaborales(2, DateTime.Now);
            DateTime RangoFechaCon1DiasHabiles = await _commonService.CalculardiasLaborales(1, DateTime.Now);

            List < DefensaJudicialSeguimiento > defensaJudicialSeguimiento5dias = _context.DefensaJudicialSeguimiento
                                                        .Where(r => r.EstadoProcesoCodigo == ConstantCodigoEstadoProcesoDefensaJudicialSeguimiento.Actuacion_finalizada && r.FechaVencimiento == RangoFechaCon5DiasHabiles)
                                                        .Include(r => r.DefensaJudicial)
                                                            .ThenInclude(r => r.DefensaJudicialContratacionProyecto)
                                                                .ThenInclude(r => r.ContratacionProyecto)
                                                        .ToList();

            List<DefensaJudicialSeguimiento> defensaJudicialSeguimiento4dias = _context.DefensaJudicialSeguimiento
                                                        .Where(r => r.EstadoProcesoCodigo == ConstantCodigoEstadoProcesoDefensaJudicialSeguimiento.Actuacion_finalizada && r.FechaVencimiento == RangoFechaCon4DiasHabiles)
                                                        .Include(r => r.DefensaJudicial)
                                                            .ThenInclude(r => r.DefensaJudicialContratacionProyecto)
                                                                .ThenInclude(r => r.ContratacionProyecto)
                                                        .ToList();

            List<DefensaJudicialSeguimiento> defensaJudicialSeguimiento3dias = _context.DefensaJudicialSeguimiento
                                                        .Where(r => r.EstadoProcesoCodigo == ConstantCodigoEstadoProcesoDefensaJudicialSeguimiento.Actuacion_finalizada && r.FechaVencimiento == RangoFechaCon3DiasHabiles)
                                                        .Include(r => r.DefensaJudicial)
                                                            .ThenInclude(r => r.DefensaJudicialContratacionProyecto)
                                                                .ThenInclude(r => r.ContratacionProyecto)
                                                        .ToList();

            List<DefensaJudicialSeguimiento> defensaJudicialSeguimiento2dias = _context.DefensaJudicialSeguimiento
                                                        .Where(r => r.EstadoProcesoCodigo == ConstantCodigoEstadoProcesoDefensaJudicialSeguimiento.Actuacion_finalizada && r.FechaVencimiento == RangoFechaCon2DiasHabiles)
                                                        .Include(r => r.DefensaJudicial)
                                                            .ThenInclude(r => r.DefensaJudicialContratacionProyecto)
                                                                .ThenInclude(r => r.ContratacionProyecto)
                                                        .ToList();

            List<DefensaJudicialSeguimiento> defensaJudicialSeguimiento1dias = _context.DefensaJudicialSeguimiento
                                                        .Where(r => r.EstadoProcesoCodigo == ConstantCodigoEstadoProcesoDefensaJudicialSeguimiento.Actuacion_finalizada && r.FechaVencimiento == RangoFechaCon1DiasHabiles)
                                                        .Include(r => r.DefensaJudicial)
                                                            .ThenInclude(r => r.DefensaJudicialContratacionProyecto)
                                                                .ThenInclude(r => r.ContratacionProyecto)
                                                        .ToList();

            List<EnumeratorPerfil> perfilsEnviarCorreo = new List<EnumeratorPerfil> { EnumeratorPerfil.Juridica };

            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.FechaVencimientoProximaActuacionJuridica_4_2_1);
            Contrato contrato = new Contrato();
            // 5 días
            foreach (var defensaJudicialSeguimiento in defensaJudicialSeguimiento5dias)
            {

                if (defensaJudicialSeguimiento.DefensaJudicial.DefensaJudicialContratacionProyecto.FirstOrDefault().ContratacionProyecto != null)
                {
                    contrato = _context.Contrato.Where(r => r.ContratacionId == defensaJudicialSeguimiento.DefensaJudicial.DefensaJudicialContratacionProyecto.FirstOrDefault().ContratacionProyecto.ContratacionId)
                                                .Include(r => r.ControversiaContractual)
                                                .FirstOrDefault();
                    if (contrato != null)
                    {
                        Dominio TipoControversia = await _commonService.GetDominioByNombreDominioAndTipoDominio(contrato.ControversiaContractual.FirstOrDefault().TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipos_Controversia);

                        string strContenido = TemplateRecoveryPassword.Contenido
                                         .Replace("[NUMERO_CONTRATO]", contrato != null ? contrato.NumeroContrato : String.Empty)
                                         .Replace("[PROXIMA_ACTUACION]", defensaJudicialSeguimiento.ProximaActuacion)
                                         .Replace("[NUMERO_PROCESO]", defensaJudicialSeguimiento.DefensaJudicial.NumeroProceso)
                                         .Replace("[FECHA_VENCIMIENTO]", ((DateTime)defensaJudicialSeguimiento.FechaVencimiento).ToString("yyyy-MM-dd"))
                                         .Replace("[DIAS]", "cinco (5) días")
                                         .Replace("[FECHA_REGISTRO_PROCESO]", ((DateTime)defensaJudicialSeguimiento.DefensaJudicial.FechaCreacion).ToString("yyyy-MM-dd"))
                                         .Replace("[LEGITIMACION]", defensaJudicialSeguimiento.DefensaJudicial.EsLegitimacionActiva == true ? "Activa" : "Pasiva")
                                         .Replace("[TIPO_CONTROVERSIA]", TipoControversia.Nombre);
                        _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, TemplateRecoveryPassword.Asunto);
                    }
                }
            }
            //4 días
            foreach (var defensaJudicialSeguimiento in defensaJudicialSeguimiento4dias)
            {

                if (defensaJudicialSeguimiento.DefensaJudicial.DefensaJudicialContratacionProyecto.FirstOrDefault().ContratacionProyecto != null)
                {
                    contrato = _context.Contrato.Where(r => r.ContratacionId == defensaJudicialSeguimiento.DefensaJudicial.DefensaJudicialContratacionProyecto.FirstOrDefault().ContratacionProyecto.ContratacionId)
                                                .Include(r => r.ControversiaContractual)
                                                .FirstOrDefault();
                    if (contrato != null)
                    {
                        Dominio TipoControversia = await _commonService.GetDominioByNombreDominioAndTipoDominio(contrato.ControversiaContractual.FirstOrDefault().TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipos_Controversia);

                        string strContenido = TemplateRecoveryPassword.Contenido
                                         .Replace("[NUMERO_CONTRATO]", contrato != null ? contrato.NumeroContrato : String.Empty)
                                         .Replace("[PROXIMA_ACTUACION]", defensaJudicialSeguimiento.ProximaActuacion)
                                         .Replace("[NUMERO_PROCESO]", defensaJudicialSeguimiento.DefensaJudicial.NumeroProceso)
                                         .Replace("[FECHA_VENCIMIENTO]", ((DateTime)defensaJudicialSeguimiento.FechaVencimiento).ToString("yyyy-MM-dd"))
                                         .Replace("[DIAS]", "cuatro (4) días")
                                         .Replace("[FECHA_REGISTRO_PROCESO]", ((DateTime)defensaJudicialSeguimiento.DefensaJudicial.FechaCreacion).ToString("yyyy-MM-dd"))
                                         .Replace("[LEGITIMACION]", defensaJudicialSeguimiento.DefensaJudicial.EsLegitimacionActiva == true ? "Activa" : "Pasiva")
                                         .Replace("[TIPO_CONTROVERSIA]", TipoControversia.Nombre);
                        _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, TemplateRecoveryPassword.Asunto);
                    }
                }
            }
            //3 días
            foreach (var defensaJudicialSeguimiento in defensaJudicialSeguimiento3dias)
            {

                if (defensaJudicialSeguimiento.DefensaJudicial.DefensaJudicialContratacionProyecto.FirstOrDefault().ContratacionProyecto != null)
                {
                    contrato = _context.Contrato.Where(r => r.ContratacionId == defensaJudicialSeguimiento.DefensaJudicial.DefensaJudicialContratacionProyecto.FirstOrDefault().ContratacionProyecto.ContratacionId)
                                                .Include(r => r.ControversiaContractual)
                                                .FirstOrDefault();
                    if (contrato != null)
                    {
                        Dominio TipoControversia = await _commonService.GetDominioByNombreDominioAndTipoDominio(contrato.ControversiaContractual.FirstOrDefault().TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipos_Controversia);

                        string strContenido = TemplateRecoveryPassword.Contenido
                                         .Replace("[NUMERO_CONTRATO]", contrato != null ? contrato.NumeroContrato : String.Empty)
                                         .Replace("[PROXIMA_ACTUACION]", defensaJudicialSeguimiento.ProximaActuacion)
                                         .Replace("[NUMERO_PROCESO]", defensaJudicialSeguimiento.DefensaJudicial.NumeroProceso)
                                         .Replace("[FECHA_VENCIMIENTO]", ((DateTime)defensaJudicialSeguimiento.FechaVencimiento).ToString("yyyy-MM-dd"))
                                         .Replace("[DIAS]", "tres (3) días")
                                         .Replace("[FECHA_REGISTRO_PROCESO]", ((DateTime)defensaJudicialSeguimiento.DefensaJudicial.FechaCreacion).ToString("yyyy-MM-dd"))
                                         .Replace("[LEGITIMACION]", defensaJudicialSeguimiento.DefensaJudicial.EsLegitimacionActiva == true ? "Activa" : "Pasiva")
                                         .Replace("[TIPO_CONTROVERSIA]", TipoControversia.Nombre);
                        _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, TemplateRecoveryPassword.Asunto);
                    }
                }
            }
            //2 días
            foreach (var defensaJudicialSeguimiento in defensaJudicialSeguimiento2dias)
            {

                if (defensaJudicialSeguimiento.DefensaJudicial.DefensaJudicialContratacionProyecto.FirstOrDefault().ContratacionProyecto != null)
                {
                    contrato = _context.Contrato.Where(r => r.ContratacionId == defensaJudicialSeguimiento.DefensaJudicial.DefensaJudicialContratacionProyecto.FirstOrDefault().ContratacionProyecto.ContratacionId)
                                                .Include(r => r.ControversiaContractual)
                                                .FirstOrDefault();
                    if (contrato != null)
                    {
                        Dominio TipoControversia = await _commonService.GetDominioByNombreDominioAndTipoDominio(contrato.ControversiaContractual.FirstOrDefault().TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipos_Controversia);

                        string strContenido = TemplateRecoveryPassword.Contenido
                                         .Replace("[NUMERO_CONTRATO]", contrato != null ? contrato.NumeroContrato : String.Empty)
                                         .Replace("[PROXIMA_ACTUACION]", defensaJudicialSeguimiento.ProximaActuacion)
                                         .Replace("[NUMERO_PROCESO]", defensaJudicialSeguimiento.DefensaJudicial.NumeroProceso)
                                         .Replace("[FECHA_VENCIMIENTO]", ((DateTime)defensaJudicialSeguimiento.FechaVencimiento).ToString("yyyy-MM-dd"))
                                         .Replace("[DIAS]", "dos (2) días")
                                         .Replace("[FECHA_REGISTRO_PROCESO]", ((DateTime)defensaJudicialSeguimiento.DefensaJudicial.FechaCreacion).ToString("yyyy-MM-dd"))
                                         .Replace("[LEGITIMACION]", defensaJudicialSeguimiento.DefensaJudicial.EsLegitimacionActiva == true ? "Activa" : "Pasiva")
                                         .Replace("[TIPO_CONTROVERSIA]", TipoControversia.Nombre);
                        _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, TemplateRecoveryPassword.Asunto);
                    }
                }
            }
            //1 día
            foreach (var defensaJudicialSeguimiento in defensaJudicialSeguimiento1dias)
            {

                if (defensaJudicialSeguimiento.DefensaJudicial.DefensaJudicialContratacionProyecto.FirstOrDefault().ContratacionProyecto != null)
                {
                    contrato = _context.Contrato.Where(r => r.ContratacionId == defensaJudicialSeguimiento.DefensaJudicial.DefensaJudicialContratacionProyecto.FirstOrDefault().ContratacionProyecto.ContratacionId)
                                                .Include(r => r.ControversiaContractual)
                                                .FirstOrDefault();
                    if (contrato != null)
                    {
                        Dominio TipoControversia = await _commonService.GetDominioByNombreDominioAndTipoDominio(contrato.ControversiaContractual.FirstOrDefault().TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipos_Controversia);

                        string strContenido = TemplateRecoveryPassword.Contenido
                                         .Replace("[NUMERO_CONTRATO]", contrato != null ? contrato.NumeroContrato : String.Empty)
                                         .Replace("[PROXIMA_ACTUACION]", defensaJudicialSeguimiento.ProximaActuacion)
                                         .Replace("[NUMERO_PROCESO]", defensaJudicialSeguimiento.DefensaJudicial.NumeroProceso)
                                         .Replace("[FECHA_VENCIMIENTO]", ((DateTime)defensaJudicialSeguimiento.FechaVencimiento).ToString("yyyy-MM-dd"))
                                         .Replace("[DIAS]", "(1) día")
                                         .Replace("[FECHA_REGISTRO_PROCESO]", ((DateTime)defensaJudicialSeguimiento.DefensaJudicial.FechaCreacion).ToString("yyyy-MM-dd"))
                                         .Replace("[LEGITIMACION]", defensaJudicialSeguimiento.DefensaJudicial.EsLegitimacionActiva == true ? "Activa" : "Pasiva")
                                         .Replace("[TIPO_CONTROVERSIA]", TipoControversia.Nombre);
                        _commonService.EnviarCorreo(perfilsEnviarCorreo, strContenido, TemplateRecoveryPassword.Asunto);
                    }
                }
            }
        }
        #endregion
        public async Task<dynamic> GetContratoByTipoSolicitudCodigoModalidadContratoCodigoOrNumeroContrato(string pTipoSolicitud, string pModalidadContrato, string pNumeroContrato)
        {
            List<Contrato> ListContratos = new List<Contrato>();
            try
            {
                ListContratos = await _context.Contrato
                .Include(c => c.Contratacion)
                         .Where(c => c.NumeroContrato.Trim().ToLower().Contains(pNumeroContrato.Trim().ToLower())
                                  //&& c.ModalidadCodigo == pModalidadContrato
                                  //&& c.Contratacion.TipoSolicitudCodigo == pTipoSolicitud
                                  && c.Contratacion.EstadoSolicitudCodigo == ConstanCodigoEstadoSolicitudContratacion.Registrados
                               //&& c.EstadoActaFase2.Trim() == ConstanCodigoEstadoActaInicioObra.Con_acta_suscrita_y_cargada
                               ).ToListAsync();
                return ListContratos.OrderByDescending(r => r.ContratoId).ToList();
            }
            catch (Exception ex)
            {
                return ListContratos;
            }
        }

    }
}