using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace asivamosffie.services
{
    //CU 4_4_1 Registrar actuaciones de controversias contractuales

    public class ContractualNoveltyService /*: IContractualControversy*/
    {

        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;


        public ContractualNoveltyService(devAsiVamosFFIEContext context, ICommonService commonService)
        {

            _commonService = commonService;
            _context = context;

            //_settings = settings;
        }


        public async Task<Respuesta> CreateEditNovedadContractual(NovedadContractual novedadContractual)
        {
            Respuesta _response = new Respuesta(); /*NovedadContractual novedadContractual*/

            int idAccionCrearEditarNovedadContractual = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Editar_Novedad_Contractual, (int)EnumeratorTipoDominio.Acciones);

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
                if (novedadContractual != null)
                {

                    if (string.IsNullOrEmpty(novedadContractual.NovedadContractualId.ToString()) || novedadContractual.NovedadContractualId == 0)
                    {
                        strCrearEditar = "REGISTRAR NOVEDAD CONTRACTUAL";

                        //Auditoria
                        //strCrearEditar = "REGISTRAR AVANCE COMPROMISOS";
                        //novedadContractual.FechaCreacion = DateTime.Now;
                        //controversiaActuacion.UsuarioCreacion = compromisoSeguimiento.UsuarioCreacion;
                        //novedadContractual.FechaSolictud ;
                        //novedadContractual.InstanciaCodigo = 
                        //novedadContractual.TipoNovedadCodigo
                        //    novedadContractual.MotivoNovedadCodigo
                        //    novedadContractual.ResumenJustificacion
                        //    novedadContractual.EsDocumentacionSoporte
                        //    novedadContractual.ConceptoTecnico
                        //    novedadContractual.FechaConcepto
                        //    //novedadContractual.radi

                        //NumeroRadicadoFfie RequisitoTecnicoRadicado
                        //novedadContractual.RegistroCompleto = ValidarRegistroCompletoControversiaActuacion(novedadContractual);

                        novedadContractual.Eliminado = false;
                        _context.NovedadContractual.Add(novedadContractual);

                    }

                    else
                    {
                        strCrearEditar = "EDIT NOVEDAD CONTRACTUAL";

                        //novedadContractual.Observaciones = Helpers.Helpers.CleanStringInput(novedadContractual.Observaciones);
                        //novedadContractual.ResumenPropuestaFiduciaria = Helpers.Helpers.CleanStringInput(novedadContractual.ResumenPropuestaFiduciaria);

                        novedadContractual.FechaCreacion = DateTime.Now;
                        //contratoPoliza.UsuarioCreacion = "forozco"; //HttpContext.User.FindFirst("User").Value;
                        //controversiaActuacion.UsuarioCreacion = novedadContractual.UsuarioModificacion;

                        //_context.Add(contratoPoliza);

                        //novedadContractual.EsCompleto = ValidarRegistroCompletoControversiaActuacion(novedadContractual);
                        //contratoPoliza.ObservacionesRevisionGeneral = ValidarRegistroCompleto(cofinanciacion);

                        //LimpiarEntradasContratoPoliza(ref contratoPoliza);

                        //_context.ContratoPoliza.Add(contratoPoliza);
                        _context.NovedadContractual.Update(novedadContractual);

                    }

                    _context.SaveChanges();

                    return
                        new Respuesta
                        {
                            Data = novedadContractual,
                            IsSuccessful = true,
                            IsException = false,
                            IsValidation = false,
                            Code = ConstantMessagesContractualNovelty.OperacionExitosa,
                            Message =
                            await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Registrar_actuaciones_controversias_contractuales,
                            ConstantMessagesContractualNovelty.OperacionExitosa,
                            //contratoPoliza
                            idAccionCrearEditarNovedadContractual
                            , novedadContractual.UsuarioModificacion
                            //"UsuarioCreacion"
                            , "EDITAR NOVEDAD CONTRACTUAL"
                            //contratoPoliza.UsuarioCreacion, "REGISTRAR CONTRATO POLIZA"
                            )
                        };

                    //return _response = new Respuesta { IsSuccessful = true,
                    //    IsValidation = false, Data = cuentaBancaria,
                    //    Code = ConstantMessagesBankAccount.OperacionExitosa };
                }
                else
                {
                    return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContractualNovelty.RecursoNoEncontrado };
                }

            }
            catch (Exception ex)
            {
                return _response = new Respuesta { IsSuccessful = false, IsValidation = false, Data = null, Code = ConstantMessagesContractualNovelty.ErrorInterno };
            }

        }

        //public async Task<List<GrillaNovedadTipo>> ListGrillaTipoSolicitudControversiaContractual(int pNovedadContractual = 0)
        //{
        //    //await AprobarContratoByIdContrato(1);

        //    List<GrillaNovedadTipo> ListNovedadGrilla = new List<GrillaNovedadTipo>();
        //    //Fecha de firma del contrato ??? FechaFirmaContrato , [Contrato] -(dd / mm / aaaa)

        //    //Tipo de solicitud ??? ContratoPoliza - TipoSolicitudCodigo      

        //    //List<ControversiaContractual> ListControversiaContractualGrilla = await _context.ControversiaContractual.Where(r => !(bool)r.EstadoCodigo).Distinct().ToListAsync();
        //    List<NovedadContractual> ListNovedad = await _context.NovedadContractual.Where(r => r.Eliminado == false).Distinct().ToListAsync();

        //    if (pNovedadContractual != 0)
        //    {
        //        ListNovedad = await _context.NovedadContractual.Where(r => r.NovedadContractualId == pNovedadContractual).ToListAsync();

        //    }

        //    foreach (var novedad in ListNovedad)
        //    {
        //        try
        //        {
        //            Contrato contrato = null;

        //            //contrato = await _commonService.GetContratoPolizaByContratoId(controversia.ContratoId);
        //            contrato = _context.Contrato.Where(r => r.ContratoId == novedad.NovedadContractualId).FirstOrDefault();

        //            //tiposol contratoPoliza = await _commonService.GetContratoPolizaByContratoId(contrato.ContratoId);
        //            string strEstadoCodigoControversia = "sin definir";
        //            string strEstadoControversia = "sin definir";
        //            string strTipoControversiaCodigo = "sin definir";
        //            string strTipoControversia = "sin definir";

        //            //Localizacion departamento = await _commonService.GetDepartamentoByIdMunicipio(proyecto.LocalizacionIdMunicipio);
        //            Dominio EstadoCodigoControversia;
        //            Dominio TipoControversiaCodigo;

        //            string prefijo = "";

        //            if (contrato != null)
        //            {
        //                TipoControversiaCodigo = await _commonService.GetDominioByNombreDominioAndTipoDominio(novedad.TipoControversiaCodigo, (int)EnumeratorTipoDominio.Tipo_de_controversia);
        //                if (TipoControversiaCodigo != null)
        //                {
        //                    strTipoControversiaCodigo = TipoControversiaCodigo.Codigo;
        //                    strTipoControversia = TipoControversiaCodigo.Nombre;

        //                }

        //                EstadoCodigoControversia = await _commonService.GetDominioByNombreDominioAndTipoDominio(novedad.EstadoCodigo, (int)EnumeratorTipoDominio.Estado_controversia);
        //                if (EstadoCodigoControversia != null)
        //                {
        //                    strEstadoControversia = EstadoCodigoControversia.Nombre;
        //                    strEstadoCodigoControversia = EstadoCodigoControversia.Codigo;
        //                }

        //                if (contrato.TipoContratoCodigo == ConstanCodigoTipoContrato.Obra)
        //                    prefijo = ConstanPrefijoNumeroSolicitudControversia.Obra;
        //                else if (contrato.TipoContratoCodigo == ConstanCodigoTipoContrato.Interventoria)
        //                    prefijo = ConstanPrefijoNumeroSolicitudControversia.Interventoria;

        //                //EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
        //                //if (EstadoSolicitudCodigoContratoPoliza != null)
        //                //    strEstadoSolicitudCodigoContratoPoliza = EstadoSolicitudCodigoContratoPoliza.Nombre;

        //            }

        //            //Dominio EstadoSolicitudCodigoContratoPoliza = await _commonService.GetDominioByNombreDominioAndTipoDominio(contratoPoliza.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Estado_Contrato_Poliza);
        //            GrillaNovedadTipo RegistroControversiaContractual = new GrillaNovedadTipo
        //            {
        //                ControversiaContractualId = novedad.ControversiaContractualId,
        //                //NumeroSolicitud=controversia.NumeroSolicitud,
        //                //NumeroSolicitud = string.Format("0000"+ controversia.ControversiaContractualId.ToString()),
        //                NumeroSolicitud = prefijo + novedad.ControversiaContractualId.ToString("000"),
        //                //FechaSolicitud=controversia.FechaSolicitud,
        //                FechaSolicitud = novedad.FechaSolicitud != null ? Convert.ToDateTime(novedad.FechaSolicitud).ToString("dd/MM/yyyy") : novedad.FechaSolicitud.ToString(),
        //                TipoControversia = strTipoControversia,
        //                TipoControversiaCodigo = strTipoControversiaCodigo,
        //                ContratoId = contrato.ContratoId,
        //                NumeroContrato = contrato.NumeroContrato,
        //                EstadoControversia = strEstadoControversia,
        //                EstadoControversiaCodigo = strEstadoCodigoControversia,
        //                RegistroCompletoNombre = (bool)novedad.EsCompleto ? "Completo" : "Incompleto",

        //            };

        //            //if (!(bool)proyecto.RegistroCompleto)
        //            //{
        //            //    proyectoGrilla.EstadoRegistro = "INCOMPLETO";
        //            //}
        //            ListNovedadGrilla.Add(RegistroControversiaContractual);
        //        }
        //        catch (Exception e)
        //        {
        //            GrillaNovedadTipo RegistroControversiaContractual = new GrillaNovedadTipo
        //            {
        //                ControversiaContractualId = novedad.ControversiaContractualId,
        //                NumeroSolicitud = novedad.NumeroSolicitud + " - " + e.InnerException.ToString(),
        //                //FechaSolicitud=controversia.FechaSolicitud,
        //                FechaSolicitud = novedad.FechaSolicitud != null ? Convert.ToDateTime(novedad.FechaSolicitud).ToString("dd/MM/yyyy") : novedad.FechaSolicitud.ToString(),
        //                TipoControversia = e.ToString(),
        //                TipoControversiaCodigo = "ERROR",
        //                NumeroContrato = "ERROR",
        //                EstadoControversia = "ERROR",
        //                RegistroCompletoNombre = "ERROR",
        //                EstadoControversiaCodigo = "ERROR",
        //                ContratoId = 0,

        //            };
        //            ListNovedadGrilla.Add(RegistroControversiaContractual);
        //        }
        //    }
        //    //return ListNovedadGrilla.OrderByDescending(r => r.ControversiaContractualId).ToList();
        //    return ListNovedadGrilla.ToList();

        //}

    }

    
}
