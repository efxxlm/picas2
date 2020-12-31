using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace asivamosffie.services
{
    public class ManagePreContructionActPhase1Service : IManagePreContructionActPhase1Service
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;
        private readonly IRegisterSessionTechnicalCommitteeService _registerSessionTechnicalCommitteeService;


        public ManagePreContructionActPhase1Service(IDocumentService documentService, devAsiVamosFFIEContext context, ICommonService commonService, IRegisterSessionTechnicalCommitteeService registerSessionTechnicalCommitteeService)
        {
            _documentService = documentService;
            _registerSessionTechnicalCommitteeService = registerSessionTechnicalCommitteeService;
            _context = context;
            _commonService = commonService;
        }

        public async Task<dynamic> GetListContrato()
        {
            try
            {
                List<Contrato> listContratos = await _context.Contrato
                       .Where(r => r.FechaAprobacionRequisitosSupervisor.HasValue).ToListAsync();

                List<Dominio> listEstadosActa = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estado_Del_Acta_Contrato).ToList();

                List<dynamic> ListContratacionDynamic = new List<dynamic>();

                listContratos.ForEach(contrato =>
                {
                    ListContratacionDynamic.Add(new
                    {
                        fechaAprobacionRequisitosSupervisor = contrato.FechaAprobacionRequisitosSupervisor,
                        contrato.NumeroContrato,
                        estadoActaContrato = !string.IsNullOrEmpty(contrato.EstadoActa) ? listEstadosActa.Where(r => r.Codigo == contrato.EstadoActa).FirstOrDefault().Nombre : " ",
                        contrato.ContratoId
                    });
                });

                return ListContratacionDynamic;
            }
            catch (Exception ex)
            {
                return new List<dynamic>();
            }

        }

        public async Task<Contrato> GetContratoByContratoId(int pContratoId, int? pUserId)
        {
            try
            {
                Contrato contrato =
                    await _context.Contrato.Where(r => r.ContratoId == pContratoId)
                      .Include(r => r.Contratacion)
                          .ThenInclude(r => r.ContratacionProyecto)
                              .ThenInclude(r => r.Proyecto)
                       .Include(r => r.ContratoObservacion)
                       .Include(r => r.Contratacion)
                          .ThenInclude(r => r.ContratacionProyecto)
                              .ThenInclude(r => r.ContratacionProyectoAportante)
                                  .ThenInclude(r => r.ComponenteAportante)
                                      .ThenInclude(r => r.ComponenteUso)
                        .Include(r => r.Contratacion)
                           .ThenInclude(r => r.Contratista)
                               .ThenInclude(r => r.ProcesoSeleccionProponente)
                        .Include(r => r.Contratacion)
                           .ThenInclude(r => r.DisponibilidadPresupuestal)
                        .Include(r => r.ContratoPoliza)
                        .FirstOrDefaultAsync();

                foreach (var ContratacionProyecto in contrato.Contratacion.ContratacionProyecto)
                {
                    foreach (var ContratacionProyectoAportante in ContratacionProyecto.ContratacionProyectoAportante)
                    {
                        foreach (var ComponenteAportante in ContratacionProyectoAportante.ComponenteAportante)
                        {
                            if (ComponenteAportante.FaseCodigo == ConstanCodigoFaseContrato.Preconstruccion.ToString())
                            {
                                contrato.ValorFase1 += ComponenteAportante.ComponenteUso.Sum(r => r.ValorUso);
                                contrato.TieneFase1 = true;
                            }
                            else
                            {
                                contrato.ValorFase2 += ComponenteAportante.ComponenteUso.Sum(r => r.ValorUso);
                                contrato.TieneFase2 = true;
                            }
                        }
                    }
                }

                if (contrato.TieneFase1 == null)
                    contrato.TieneFase1 = false;

                if (contrato.TieneFase2 == null)
                    contrato.TieneFase2 = false;
                //Modificar Usuario 
                //Ya que falta hacer caso de uso gestion usuarios
                contrato.UsuarioInterventoria = _context.Usuario.Where(r => r.Email == contrato.UsuarioModificacion).FirstOrDefault();
                return contrato;
            }
            catch (Exception)
            {
                return new Contrato();
            }
        }

        public async Task<Respuesta> EditContrato(Contrato pContrato)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Editar_Acta_De_Inicio_De_Contrato, (int)EnumeratorTipoDominio.Acciones);

            try
            {

                Contrato ContratoOld = await _context.Contrato.Where(r => r.ContratoId == pContrato.ContratoId)
                    .Include(r => r.Contratacion)
                    .Include(r => r.ContratoObservacion).FirstOrDefaultAsync();

                ContratoOld.FechaActaInicioFase1 = pContrato.FechaActaInicioFase1;
                ContratoOld.FechaTerminacion = pContrato.FechaTerminacion;
                ContratoOld.PlazoFase1PreMeses = pContrato.PlazoFase1PreMeses;
                ContratoOld.PlazoFase1PreDias = pContrato.PlazoFase1PreDias;
                ContratoOld.PlazoFase2ConstruccionDias = pContrato.PlazoFase2ConstruccionDias;
                ContratoOld.PlazoFase2ConstruccionMeses = pContrato.PlazoFase2ConstruccionMeses;
                ContratoOld.ConObervacionesActa = pContrato.ConObervacionesActa;

                if (ContratoOld.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContrato.Obra)
                    ContratoOld.EstadoActa = ConstanCodigoEstadoActaContrato.Con_acta_generada;
                else
                    ContratoOld.EstadoActa = ConstanCodigoEstadoActaContrato.Con_acta_preliminar_generada;



                if (ContratoOld.ConObervacionesActa.HasValue)
                {
                    foreach (var ContratoObservacion in pContrato.ContratoObservacion)
                    {
                        if (ContratoObservacion.ContratoObservacionId == 0)
                        {
                            ContratoObservacion.UsuarioCreacion = pContrato.UsuarioCreacion;
                            ContratoObservacion.FechaCreacion = DateTime.Now;
                            ContratoObservacion.EsActa = true;
                            ContratoObservacion.EsActaFase1 = true;
                            ContratoObservacion.EsActaFase2 = false;

                            _context.ContratoObservacion.Add(ContratoObservacion);
                        }
                        else
                        {
                            ContratoObservacion contratoObservacionOld = _context.ContratoObservacion.Where(r => r.ContratoObservacionId == ContratoObservacion.ContratoObservacionId).FirstOrDefault();

                            contratoObservacionOld.UsuarioModificacion = pContrato.UsuarioCreacion;
                            contratoObservacionOld.FechaModificacion = DateTime.Now;
                            contratoObservacionOld.Observaciones = ContratoObservacion.Observaciones;
                            contratoObservacionOld.EsSupervision = ContratoObservacion.EsSupervision;

                            _context.Update(contratoObservacionOld);
                        }
                    }
                }
                _context.SaveChanges();
                return
                     new Respuesta
                     {
                         IsSuccessful = true,
                         IsException = false,
                         IsValidation = false,
                         Code = RegisterPreContructionPhase1.OperacionExitosa,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.OperacionExitosa, idAccion, pContrato.UsuarioCreacion, "EDITAR ACTA DE INICIO DE CONTRATO FASE 1 PRECONSTRUCCION")
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
                           Code = RegisterPreContructionPhase1.Error,
                           Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.Error, idAccion, pContrato.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                       };

            }
        }

        public async Task<Respuesta> LoadActa(Contrato pContrato, IFormFile pFile, string pDirectorioBase, string pDirectorioActaContrato, AppSettingsService appSettingsService)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cargar_Acta_Subscrita, (int)EnumeratorTipoDominio.Acciones);
            Contrato ContratoOld = await _context.Contrato.Where(r => r.ContratoId == pContrato.ContratoId)
                .Include(r => r.ContratoObservacion)
                .Include(x => x.Contratacion)
                .ThenInclude(x => x.Contratista)
                .ThenInclude(x => x.ProcesoSeleccionProponente)
                .FirstOrDefaultAsync();

            string strFilePatch = string.Empty;
            try
            {
                if (pFile != null && pFile.Length > 0)
                {
                    strFilePatch = Path.Combine(pDirectorioBase, pDirectorioActaContrato, pContrato.ContratoId.ToString());
                    await _documentService.SaveFileContratacion(pFile, strFilePatch, pFile.FileName);
                    ContratoOld.RutaActaFase1 = Path.Combine(strFilePatch, pFile.FileName);
                }
                else
                    return new Respuesta();

                ContratoOld.FechaActaInicioFase1 = pContrato.FechaActaInicioFase1;
                ContratoOld.FechaTerminacion = pContrato.FechaTerminacion;
                ContratoOld.EstadoActa = ConstanCodigoEstadoActaContrato.Con_acta_suscrita_y_cargada;


                _context.SaveChanges();

                //deboenviarcorreoal contratista
                Template TemplateMail = await _commonService.GetTemplateById((int)enumeratorTemplate.EnviarActaCargada319);
                string template = TemplateMail.Contenido
                                .Replace("_LinkF_", appSettingsService.DominioFront)
                                .Replace("[NUMERO_CONTRATO]", ContratoOld.NumeroContrato);
                Helpers.Helpers.EnviarCorreo(ContratoOld.Contratacion.Contratista.ProcesoSeleccionProponente.EmailProponente,
                "Acta suscrita cargada", template, appSettingsService.Sender, appSettingsService.Password, appSettingsService.MailServer, appSettingsService.MailPort, false, ContratoOld.RutaActaFase1);


                return
                     new Respuesta
                     {
                         IsSuccessful = false,
                         IsException = true,
                         IsValidation = false,
                         Code = RegisterPreContructionPhase1.OperacionExitosa,
                         Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.OperacionExitosa, idAccion, pContrato.UsuarioCreacion, "EDITAR ACTA DE INICIO DE CONTRATO FASE 1 PRECONSTRUCCION")
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
                           Code = RegisterPreContructionPhase1.Error,
                           Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.Error, idAccion, pContrato.UsuarioCreacion, ex.InnerException.ToString().Substring(0, 500))
                       };

            }
        }

        public async Task<Respuesta> CambiarEstadoActa(int pContratoId, string pCodigoEstadoActa, string pUsuarioModificacion, AppSettingsService appSettingsService)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Cambiar_Estado_Acta_Contrato, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                Contrato contratoOld = _context.Contrato.Find(pContratoId);
                contratoOld.FechaModificacion = DateTime.Now;
                contratoOld.UsuarioModificacion = pUsuarioModificacion;
                contratoOld.EstadoActa = pCodigoEstadoActa;
                _context.SaveChanges();
                //jflorez 2020 122 10 envionotificacioncambiar por constante
                if (pCodigoEstadoActa == "17")
                {

                    var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Interventor).Include(y => y.Usuario).ToList();
                    foreach (var usuario in usuarios)
                    {
                        this.sendMailNotificarAInterventor(contratoOld, usuario.Usuario.Email, appSettingsService);
                    }


                }

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = RegisterPreContructionPhase1.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.OperacionExitosa, idAccion, pUsuarioModificacion, "CAMBIAR ESTADO ACTA")
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
                        Code = RegisterPreContructionPhase1.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.Error, idAccion, pUsuarioModificacion, ex.InnerException.ToString().ToUpper())
                    };
            }
        }

        private void sendMailNotificarAInterventor(Contrato prmContrato, string prmMail, AppSettingsService appSettingsService)
        {
            //deboenviarcorreoal contratista
            Template TemplateMail = _context.Template.Find((int)enumeratorTemplate.Notificarinterventor);
            string template = TemplateMail.Contenido
                            .Replace("_LinkF_", appSettingsService.DominioFront)
                            .Replace("[NUMERO_CONTRATO]", prmContrato.NumeroContrato);
            Helpers.Helpers.EnviarCorreo(prmMail,
            "Acta Notificada", template, appSettingsService.Sender, appSettingsService.Password, appSettingsService.MailServer, appSettingsService.MailPort);
        }

        public async Task<byte[]> GetActaByIdPerfil(int pContratoId, int pUserId, AppSettingsService pAppSettingsService, bool pEsContruccion)
        {
            Contrato contrato = _context.Contrato.Where(r => r.ContratoId == pContratoId).Include(r => r.Contratacion).FirstOrDefault();

            //Enviar correo
            await GetEnviarActaParaFirmar(pAppSettingsService, pContratoId);

            if (contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString())
                return await ReplacePlantillaObra(pContratoId, pUserId, pEsContruccion);
            else
                return await ReplacePlantillaInterventoria(pContratoId, pUserId, pEsContruccion);
        }

        public async Task<byte[]> ReplacePlantillaObra(int pContratoId, int pUserId, bool pEsContruccion)
        {
            CultureInfo ci = new CultureInfo("es-MX");

            Contrato contrato = await GetContratoByContratoId(pContratoId, pUserId);

            Plantilla plantilla = new Plantilla();
            if (pEsContruccion)
            {
                plantilla = await _context.Plantilla
                   .Include(r => r.Encabezado)
                   .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Contrato_Acta_Obra_Construccion)
                   .ToString()).FirstOrDefaultAsync();
            }
            else
            {
                plantilla = await _context.Plantilla
                   .Include(r => r.Encabezado)
                   .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Contrato_Acta_Obra_Preconstruccion)
                   .ToString()).FirstOrDefaultAsync();
            }

            Usuario Supervisor = contrato.UsuarioInterventoria;

            //Registros Proyectos 
            string PlantillaRegistrosProyectos = _context.Plantilla
                .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Registro_Proyectos_Acta).ToString()).FirstOrDefault().Contenido;
            string RegistrosProyectos = string.Empty;

            List<Dominio> ListTipointervencion = await _commonService.GetListDominioByIdTipoDominio((int)EnumeratorTipoDominio.Tipo_de_Intervencion);

            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();

            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();

            foreach (var ContratacionProyecto in contrato.Contratacion.ContratacionProyecto)
            {
                Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == ContratacionProyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                Localizacion Departamento = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == ContratacionProyecto.Proyecto.SedeId).FirstOrDefault();
                InstitucionEducativaSede InstitucionEducativa = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == Sede.PadreId).FirstOrDefault();

                RegistrosProyectos += PlantillaRegistrosProyectos;
                RegistrosProyectos = RegistrosProyectos
                    .Replace("[LLAVE_MEN]", ContratacionProyecto.Proyecto.LlaveMen)
                    .Replace("[TIPO_INTERVENCION]", ListTipointervencion.Where(r => r.Codigo == ContratacionProyecto.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre)
                    .Replace("[DEPARTAMENTO]", Departamento.Descripcion)
                    .Replace("[MUNICIPIO]", Municipio.Descripcion)
                    .Replace("[INSTITUCION_EDUCATIVA]", InstitucionEducativa.Nombre)
                    .Replace("[SEDE]", Sede.Nombre);
            }

            string MesesFase1 = string.Empty;
            string DiasFase1 = string.Empty;
            string MesesFase2 = string.Empty;
            string DiasFase2 = string.Empty;

            MesesFase1 = contrato?.PlazoFase1PreMeses + (contrato?.PlazoFase1PreMeses == 1 ? " mes / " : " meses / ");
            DiasFase1 = contrato?.PlazoFase1PreDias + (contrato?.PlazoFase1PreDias == 1 ? " dia " : "dias ");
            MesesFase2 = contrato?.PlazoFase2ConstruccionMeses + (contrato?.PlazoFase2ConstruccionMeses == 1 ? " mes / " : " meses / ");
            DiasFase2 = contrato?.PlazoFase2ConstruccionDias + (contrato?.PlazoFase2ConstruccionDias == 1 ? " dia " : " dias ");

            string MesesFase1Contrato = string.Empty;
            string DiasFase1Contrato = string.Empty;

            MesesFase1Contrato = contrato.Contratacion.DisponibilidadPresupuestal.FirstOrDefault().PlazoMeses + (contrato.Contratacion.DisponibilidadPresupuestal.FirstOrDefault().PlazoMeses == 1 ? " mes /" : " meses / ");
            DiasFase1Contrato = contrato.Contratacion.DisponibilidadPresupuestal.FirstOrDefault().PlazoDias + (contrato.Contratacion.DisponibilidadPresupuestal.FirstOrDefault().PlazoDias == 1 ? " día " : " días  ");


            plantilla.Contenido = plantilla.Contenido.Replace("[RUTA_ICONO]", Path.Combine(Directory.GetCurrentDirectory(), "assets", "img-FFIE.png"));
            plantilla.Contenido = plantilla.Contenido.Replace("[NUMERO_POLIZA]", contrato.ContratoPoliza.FirstOrDefault().NumeroPoliza);

            plantilla.Contenido = plantilla.Contenido.Replace("[NUMERO_CONTRATO_OBRA]", contrato.NumeroContrato);
            plantilla.Contenido = plantilla.Contenido.Replace("[REGISTROS_PROYECTOS]", RegistrosProyectos);
            plantilla.Contenido = plantilla.Contenido.Replace("[FECHA_ACTA_INICIO_OBRA]", contrato.FechaActaInicioFase1.HasValue ? ((DateTime)contrato.FechaActaInicioFase1).ToString("dd-MM-yyyy") : " ");
            
            plantilla.Contenido = plantilla.Contenido.Replace("[REPRESENTANTE_LEGAL_CONTRATISTA_INTERVENTORIA]", Supervisor?.Nombres + " " + Supervisor.Apellidos);
            plantilla.Contenido = plantilla.Contenido.Replace("[ENTIDAD_CONTRATISTA_INTERVENTORIA]", " - ");
            plantilla.Contenido = plantilla.Contenido.Replace("[NIT_CONTRATISTA_INTERVENTORIA]", Supervisor?.NumeroIdentificacion);
            plantilla.Contenido = plantilla.Contenido.Replace("[CEDULA_REPRESENTANTE_LEGAL_CONTRATISTA_INTERVENTORIA]", contrato?.Contratacion?.Contratista?.RepresentanteLegalNumeroIdentificacion);

            plantilla.Contenido = plantilla.Contenido.Replace("[REPRESENTANTE_LEGAL_CONTRATISTA_OBRA]", contrato?.Contratacion?.Contratista?.RepresentanteLegal);
            plantilla.Contenido = plantilla.Contenido.Replace("[CEDULA_SUPERVISOR]", Supervisor?.NumeroIdentificacion);
            plantilla.Contenido = plantilla.Contenido.Replace("[NIT_ENTIDAD_CONTRATISTA_OBRA]", contrato?.Contratacion?.Contratista?.RepresentanteLegalNumeroIdentificacion);
            plantilla.Contenido = plantilla.Contenido.Replace("[CARGO_SUPERVISOR]", Supervisor?.NombreMaquina);
            plantilla.Contenido = plantilla.Contenido.Replace("[REPRESENTANTE_LEGAL_CONTRATISTA_OBRA]", contrato?.Contratacion?.Contratista?.Nombre);
            plantilla.Contenido = plantilla.Contenido.Replace("[NOMBRE_ENTIDAD_CONTRATISTA_OBRA]", contrato?.Contratacion?.Contratista?.RepresentanteLegal);
            plantilla.Contenido = plantilla.Contenido.Replace("[NUMERO_DRP]", contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault().NumeroDrp);
            plantilla.Contenido = plantilla.Contenido.Replace("[FECHA_GENERACION_DRP]", (bool)contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault().FechaDrp.HasValue ? ((DateTime)contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault().FechaDrp).ToString("dd-MM-yyyy") : " ");
            plantilla.Contenido = plantilla.Contenido.Replace("[FECHA_APROBACION_POLIZAS]", (((DateTime)contrato.ContratoPoliza.FirstOrDefault().FechaAprobacion).ToString("dd-MM-yyyy")));
            plantilla.Contenido = plantilla.Contenido.Replace("[OBJETO]", contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault().Objeto);
            decimal ValorInicialContrato = !string.IsNullOrEmpty(contrato.Contratacion.DisponibilidadPresupuestal.Sum(r => r.ValorSolicitud).ToString()) ? contrato.Contratacion.DisponibilidadPresupuestal.Sum(r => r.ValorSolicitud) : 0;
            plantilla.Contenido = plantilla.Contenido.Replace("[VALOR_INICIAL_CONTRATO]", "$" + (String.Format("{0:n}", ValorInicialContrato))  +"( "+ CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Helpers.Conversores.NumeroALetras(ValorInicialContrato)) + " ) ");
            plantilla.Contenido = plantilla.Contenido.Replace("[VALOR_FASE1_PREC]", !string.IsNullOrEmpty(contrato.ValorFase1.ToString()) ? ("$" + (String.Format("{0:n}", contrato.ValorFase1))  +"( " + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Helpers.Conversores.NumeroALetras(contrato.ValorFase1))  + " ) ") : " "); 
            plantilla.Contenido = plantilla.Contenido.Replace("[VALOR_FASE2_CONST]", !string.IsNullOrEmpty(contrato.ValorFase2.ToString()) ? ("$" + (String.Format("{0:n}", contrato.ValorFase2)) + "( " + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Helpers.Conversores.NumeroALetras(contrato.ValorFase2))  + " )") : " ");
            plantilla.Contenido = plantilla.Contenido.Replace("[VALOR_FASE_1]", !string.IsNullOrEmpty(contrato.ValorFase1.ToString()) ? ("$" + (String.Format("{0:n}", contrato.ValorFase1))) + "( " + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Helpers.Conversores.NumeroALetras(contrato.ValorFase1))  + " )" : " ");
            plantilla.Contenido = plantilla.Contenido.Replace("[VALOR_FASE_2]", !string.IsNullOrEmpty(contrato.ValorFase2.ToString()) ? ("$" + (String.Format("{0:n}", contrato.ValorFase2))) + "( " + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Helpers.Conversores.NumeroALetras(contrato.ValorFase2))  + " )" : " ");
            decimal ValorActualDelContrato = !string.IsNullOrEmpty(contrato.Contratacion.DisponibilidadPresupuestal.Sum(r => r.ValorSolicitud).ToString()) ? contrato.Contratacion.DisponibilidadPresupuestal.Sum(r => r.ValorSolicitud) : 0;
            plantilla.Contenido = plantilla.Contenido.Replace("[VALOR_ACTUAL_CONTRATO]", "$" + (String.Format("{0:n}", ValorActualDelContrato)));
            plantilla.Contenido = plantilla.Contenido.Replace("[PLAZO_INICIAL_CONTRATO]", MesesFase1Contrato + DiasFase1Contrato);
            plantilla.Contenido = plantilla.Contenido.Replace("[PLAZO_EJECUCION_FASE_1]", MesesFase1 + DiasFase1);
            plantilla.Contenido = plantilla.Contenido.Replace("[PLAZO_EJECUCION_FASE_2]", MesesFase2 + DiasFase2);
            plantilla.Contenido = plantilla.Contenido.Replace("[FECHA_PREVISTA_TERMINACION]", contrato.FechaTerminacion.HasValue ? ((DateTime)contrato.FechaTerminacion).ToString("dd-MM-yyyy") : " ");
            plantilla.Contenido = plantilla.Contenido.Replace("[OBSERVACIONES]", contrato.Observaciones);
            plantilla.Contenido = plantilla.Contenido.Replace("[NOMBRE_ENTIDAD_CONTRATISTA]", contrato?.Contratacion?.Contratista?.Nombre ?? " ");
            plantilla.Contenido = plantilla.Contenido.Replace("[NIT_CONTRATISTA_INTERVEENTORIA]", contrato?.Contratacion?.Contratista?.ProcesoSeleccionProponente?.NumeroIdentificacion ?? " ");
            plantilla.Contenido = plantilla.Contenido.Replace("[CC_SUPERVISOR]", contrato?.UsuarioInterventoria.NumeroIdentificacion ?? " ");
            plantilla.Contenido = plantilla.Contenido.Replace("[CARGO]", " ");
            plantilla.Contenido = plantilla.Contenido.Replace("[CEDULA_REPRESENTANTE_LEGAL_CONTRATISTA_OBRA]", contrato?.Contratacion?.Contratista?.RepresentanteLegalNumeroIdentificacion);
            plantilla.Contenido = plantilla.Contenido.Replace("[CC_REPRESENTANTE_LEGAL]", contrato?.Contratacion?.Contratista?.RepresentanteLegalNumeroIdentificacion ?? " ");

            //   return Helpers.PDF.Convertir(plantilla);
            return _registerSessionTechnicalCommitteeService.ConvertirPDF(plantilla);
        }

        public async Task<byte[]> ReplacePlantillaInterventoria(int pContratoId, int pUserId, bool pEsContruccion)
        {
            Contrato contrato = await GetContratoByContratoId(pContratoId, pUserId);
            Plantilla plantilla = new Plantilla();
            if (pEsContruccion)
            {
                plantilla = await _context.Plantilla
                .Include(r => r.Encabezado)
                 .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Contrato_Acta_Interventoria_Construccion)
                  .ToString()).FirstOrDefaultAsync();
            }
            else
            {
                plantilla = await _context.Plantilla
                    .Include(r => r.Encabezado)
                     .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Contrato_Acta_Interventoria_Preconstruccion)
                      .ToString()).FirstOrDefaultAsync();
            }

            Usuario Supervisor = contrato.UsuarioInterventoria;

            //Registros Proyectos 
            string PlantillaRegistrosProyectos = _context.Plantilla
                .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Registro_Proyectos_Acta).ToString()).FirstOrDefault().Contenido;
            string RegistrosProyectos = string.Empty;

            List<Dominio> ListTipointervencion = await _commonService.GetListDominioByIdTipoDominio((int)EnumeratorTipoDominio.Tipo_de_Intervencion);

            List<Localizacion> ListLocalizacion = _context.Localizacion.ToList();

            List<InstitucionEducativaSede> ListInstitucionEducativaSede = _context.InstitucionEducativaSede.ToList();

            foreach (var ContratacionProyecto in contrato.Contratacion.ContratacionProyecto)
            {
                Localizacion Municipio = ListLocalizacion.Where(r => r.LocalizacionId == ContratacionProyecto.Proyecto.LocalizacionIdMunicipio).FirstOrDefault();
                Localizacion Departamento = ListLocalizacion.Where(r => r.LocalizacionId == Municipio.IdPadre).FirstOrDefault();
                InstitucionEducativaSede Sede = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == ContratacionProyecto.Proyecto.SedeId).FirstOrDefault();
                InstitucionEducativaSede InstitucionEducativa = ListInstitucionEducativaSede.Where(r => r.InstitucionEducativaSedeId == Sede.PadreId).FirstOrDefault();

                RegistrosProyectos += PlantillaRegistrosProyectos;
                RegistrosProyectos = RegistrosProyectos
                    .Replace("[LLAVE_MEN]", ContratacionProyecto.Proyecto.LlaveMen)
                    .Replace("[TIPO_INTERVENCION]", ListTipointervencion.Where(r => r.Codigo == ContratacionProyecto.Proyecto.TipoIntervencionCodigo).FirstOrDefault().Nombre)
                    .Replace("[DEPARTAMENTO]", Departamento == null ? "" : Departamento.Descripcion)
                    .Replace("[MUNICIPIO]", Municipio == null ? "" : Municipio.Descripcion)
                    .Replace("[INSTITUCION_EDUCATIVA]", InstitucionEducativa == null ? "" : InstitucionEducativa.Nombre)
                    .Replace("[SEDE]", Sede.Nombre);
            }


            string MesesFase1 = string.Empty;
            string DiasFase1 = string.Empty;
            string MesesFase2 = string.Empty;
            string DiasFase2 = string.Empty;

            MesesFase1 = contrato?.PlazoFase1PreMeses + (contrato?.PlazoFase1PreMeses == 1 ? " mes / " : " meses / ");
            DiasFase1 = contrato?.PlazoFase1PreDias + (contrato?.PlazoFase1PreDias == 1 ? " día " : " días ");
            MesesFase2 = contrato?.PlazoFase2ConstruccionMeses + (contrato?.PlazoFase2ConstruccionMeses == 1 ? " mes / " : " meses / ");
            DiasFase2 = contrato?.PlazoFase2ConstruccionDias + (contrato?.PlazoFase2ConstruccionDias == 1 ? " día " : " días ");

            string MesesFase1Contrato = string.Empty;
            string DiasFase1Contrato = string.Empty;

            MesesFase1Contrato = contrato.Contratacion.DisponibilidadPresupuestal.FirstOrDefault().PlazoMeses + (contrato.Contratacion.DisponibilidadPresupuestal.FirstOrDefault().PlazoMeses == 1 ? " mes /" : " meses / ");
            DiasFase1Contrato = contrato.Contratacion.DisponibilidadPresupuestal.FirstOrDefault().PlazoDias + (contrato.Contratacion.DisponibilidadPresupuestal.FirstOrDefault().PlazoDias == 1 ? " día " : " días  ");


            plantilla.Contenido = plantilla.Contenido.Replace("[RUTA_ICONO]", Path.Combine(Directory.GetCurrentDirectory(), "assets", "img-FFIE.png"));
            plantilla.Contenido = plantilla.Contenido.Replace("[NUMERO_CONTRATO_OBRA]", contrato.NumeroContrato);
            plantilla.Contenido = plantilla.Contenido.Replace("[REGISTROS_PROYECTOS]", RegistrosProyectos);
            plantilla.Contenido = plantilla.Contenido.Replace("[FECHA_ACTA_INICIO_OBRA]", ((DateTime)contrato.FechaActaInicioFase1).ToString("dd-MM-yyyy"));
            plantilla.Contenido = plantilla.Contenido.Replace("[NUMERO_POLIZA]", contrato.ContratoPoliza.FirstOrDefault().NumeroPoliza);

            plantilla.Contenido = plantilla.Contenido.Replace("[REPRESENTANTE_LEGAL_CONTRATISTA_INTERVENTORIA]", Supervisor?.Nombres + " " + Supervisor.Apellidos);
            plantilla.Contenido = plantilla.Contenido.Replace("[ENTIDAD_CONTRATISTA_INTERVENTORIA]", " - ");
            plantilla.Contenido = plantilla.Contenido.Replace("[NIT_CONTRATISTA_INTERVENTORIA]", Supervisor?.NumeroIdentificacion);
            plantilla.Contenido = plantilla.Contenido.Replace("[CEDULA_REPRESENTANTE_LEGAL_CONTRATISTA_INTERVENTORIA]", contrato?.Contratacion?.Contratista?.RepresentanteLegalNumeroIdentificacion);

            plantilla.Contenido = plantilla.Contenido.Replace("[REPRESENTANTE_LEGAL_CONTRATISTA_OBRA]", contrato?.Contratacion?.Contratista?.RepresentanteLegal);
            plantilla.Contenido = plantilla.Contenido.Replace("[NIT_ENTIDAD_CONTRATISTA_OBRA]", contrato?.Contratacion?.Contratista?.RepresentanteLegal);
            plantilla.Contenido = plantilla.Contenido.Replace("[CEDULA_SUPERVISOR]", Supervisor?.NumeroIdentificacion);
            plantilla.Contenido = plantilla.Contenido.Replace("[NIT_ENTIDAD_CONTRATISTA_OBRA]", contrato?.Contratacion?.Contratista?.RepresentanteLegalNumeroIdentificacion);
            plantilla.Contenido = plantilla.Contenido.Replace("[CARGO_SUPERVISOR]", Supervisor?.NombreMaquina);
            plantilla.Contenido = plantilla.Contenido.Replace("[REPRESENTANTE_LEGAL_CONTRATISTA_OBRA]", contrato?.Contratacion?.Contratista?.Nombre);
            plantilla.Contenido = plantilla.Contenido.Replace("[NOMBRE_ENTIDAD_CONTRATISTA_OBRA]", contrato?.Contratacion?.Contratista?.RepresentanteLegal);
            plantilla.Contenido = plantilla.Contenido.Replace("[NUMERO_DRP]", contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault().NumeroDrp);
            plantilla.Contenido = plantilla.Contenido.Replace("[FECHA_GENERACION_DRP]", (bool)contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault().FechaDrp.HasValue ? ((DateTime)contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault().FechaDrp).ToString("dd-MM-yyyy") : " "); plantilla.Contenido = plantilla.Contenido.Replace("[FECHA_APROBACION_POLIZAS]", (((DateTime)contrato.ContratoPoliza.FirstOrDefault().FechaAprobacion).ToString("dd-MM-yyyy")));
            plantilla.Contenido = plantilla.Contenido.Replace("[OBJETO]", contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault().Objeto);
            decimal ValorInicialContrato = !string.IsNullOrEmpty(contrato.Contratacion.DisponibilidadPresupuestal.Sum(r => r.ValorSolicitud).ToString()) ? contrato.Contratacion.DisponibilidadPresupuestal.Sum(r => r.ValorSolicitud) : 0;
            plantilla.Contenido = plantilla.Contenido.Replace("[VALOR_INICIAL_CONTRATO]", "$" + (String.Format("{0:n}", ValorInicialContrato)) + "( " + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Helpers.Conversores.NumeroALetras(ValorInicialContrato))  + " ) ");
            plantilla.Contenido = plantilla.Contenido.Replace("[VALOR_ACTUAL_CONTRATO]", "$" + (String.Format("{0:n}", ValorInicialContrato)) + "( " + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Helpers.Conversores.NumeroALetras(ValorInicialContrato))  + " ) "); 
            plantilla.Contenido = plantilla.Contenido.Replace("[VALOR_FASE1_PREC]", !string.IsNullOrEmpty(contrato.ValorFase1.ToString()) ? ("$" + (String.Format("{0:n}", contrato.ValorFase1)) + "( " + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Helpers.Conversores.NumeroALetras(contrato.ValorFase1)) + " ) ") : " ");
            plantilla.Contenido = plantilla.Contenido.Replace("[VALOR_FASE2_CONST]", !string.IsNullOrEmpty(contrato.ValorFase2.ToString()) ? ("$" + (String.Format("{0:n}", contrato.ValorFase2)) + "( " + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Helpers.Conversores.NumeroALetras(contrato.ValorFase2)) + " )") : " ");
            plantilla.Contenido = plantilla.Contenido.Replace("[VALOR_FASE_1]", !string.IsNullOrEmpty(contrato.ValorFase1.ToString()) ? ("$" + (String.Format("{0:n}", contrato.ValorFase1))) + "( " + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Helpers.Conversores.NumeroALetras(contrato.ValorFase1))+ " )" : " ");
            plantilla.Contenido = plantilla.Contenido.Replace("[VALOR_FASE_2]", !string.IsNullOrEmpty(contrato.ValorFase2.ToString()) ? ("$" + (String.Format("{0:n}", contrato.ValorFase2))) + "( " + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Helpers.Conversores.NumeroALetras(contrato.ValorFase2)) + " )" : " ");
            decimal ValorActualDelContrato = !string.IsNullOrEmpty(contrato.Contratacion.DisponibilidadPresupuestal.Sum(r => r.ValorSolicitud).ToString()) ? contrato.Contratacion.DisponibilidadPresupuestal.Sum(r => r.ValorSolicitud) : 0;
             plantilla.Contenido = plantilla.Contenido.Replace("[PLAZO_INICIAL_CONTRATO]", MesesFase1Contrato + DiasFase1Contrato);
            plantilla.Contenido = plantilla.Contenido.Replace("[PLAZO_EJECUCION_FASE_1]", MesesFase1 + DiasFase1);
            plantilla.Contenido = plantilla.Contenido.Replace("[PLAZO_EJECUCION_FASE_2]", MesesFase2 + DiasFase2);
            plantilla.Contenido = plantilla.Contenido.Replace("[FECHA_PREVISTA_TERMINACION]", contrato.FechaTerminacionFase2.HasValue ? ((DateTime)contrato.FechaTerminacionFase2).ToString("dd-MM-yyyy") : ((DateTime)contrato.FechaTerminacion).ToString("dd-MM-yyyy"));
            plantilla.Contenido = plantilla.Contenido.Replace("[OBSERVACIONES]", contrato.Observaciones);
            plantilla.Contenido = plantilla.Contenido.Replace("[NOMBRE_ENTIDAD_CONTRATISTA]", contrato?.Contratacion?.Contratista?.Nombre ?? " ");
            plantilla.Contenido = plantilla.Contenido.Replace("[NIT_CONTRATISTA_INTERVEENTORIA]", contrato?.Contratacion?.Contratista?.ProcesoSeleccionProponente?.NumeroIdentificacion ?? " ");
            plantilla.Contenido = plantilla.Contenido.Replace("[CC_SUPERVISOR]", contrato?.UsuarioInterventoria.NumeroIdentificacion ?? " ");
            plantilla.Contenido = plantilla.Contenido.Replace("[CARGO]", " ");
            plantilla.Contenido = plantilla.Contenido.Replace("[CEDULA_REPRESENTANTE_LEGAL_CONTRATISTA_OBRA]", contrato?.Contratacion?.Contratista?.RepresentanteLegalNumeroIdentificacion);
            plantilla.Contenido = plantilla.Contenido.Replace("[CC_REPRESENTANTE_LEGAL]", contrato?.Contratacion?.Contratista?.RepresentanteLegalNumeroIdentificacion ?? " ");
             
            //   return Helpers.PDF.Convertir(plantilla);
            return _registerSessionTechnicalCommitteeService.ConvertirPDF(plantilla);
        }

        public async Task<Respuesta> CreateEditObservacionesActa(ContratoObservacion pcontratoObservacion)
        {
            int idAccion = await _commonService.GetDominioIdByCodigoAndTipoDominio(ConstantCodigoAcciones.Crear_Edit_Contrato_Observacion, (int)EnumeratorTipoDominio.Acciones);

            try
            {
                if (pcontratoObservacion.ContratoObservacionId == 0)
                {
                    pcontratoObservacion.FechaCreacion = DateTime.Now;
                    _context.ContratoObservacion.Add(pcontratoObservacion);
                }
                else
                {
                    ContratoObservacion contratoObservacionOld = _context.ContratoObservacion.Find(pcontratoObservacion.ContratoObservacionId);

                    contratoObservacionOld.FechaModificacion = DateTime.Now;
                    contratoObservacionOld.UsuarioModificacion = pcontratoObservacion.UsuarioCreacion;

                    contratoObservacionOld.Observaciones = pcontratoObservacion.Observaciones;
                    contratoObservacionOld.EsActa = pcontratoObservacion.EsActa;
                    contratoObservacionOld.EsActaFase1 = pcontratoObservacion.EsActaFase1;
                    contratoObservacionOld.EsActaFase2 = pcontratoObservacion.EsActaFase2;
                }

                _context.SaveChanges();

                return
                    new Respuesta
                    {
                        IsSuccessful = true,
                        IsException = false,
                        IsValidation = false,
                        Code = RegisterPreContructionPhase1.OperacionExitosa,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.OperacionExitosa, idAccion, pcontratoObservacion.UsuarioCreacion, "CAMBIAR ESTADO ACTA")
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
                        Code = RegisterPreContructionPhase1.Error,
                        Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Preconstruccion_Fase_1, RegisterPreContructionPhase1.Error, idAccion, pcontratoObservacion.UsuarioCreacion, ex.InnerException.ToString().ToUpper())
                    };
            }
        }

        public async Task<List<ContratoObservacion>> GetListContratoObservacionByContratoId(int ContratoId)
        {
            return await _context.ContratoObservacion.Where(r => r.ContratoId == ContratoId).ToListAsync();
        }

        //Codigo CDaza Se deja la misma Logica Pedidar por David
        public async Task<ConstruccionObservacion> GetContratoObservacionByIdContratoId(int pContratoId, bool pEsSupervisor)
        {
            ConstruccionObservacion contratoObservacion = new ConstruccionObservacion();
            List<ConstruccionObservacion> lstContratoObservacion = new List<ConstruccionObservacion>();

            ContratoConstruccion contratoConstruccion = await _context.ContratoConstruccion.Where(r => r.ContratoId == pContratoId).FirstOrDefaultAsync();

            if (contratoConstruccion != null)
            {
                contratoObservacion.ContratoConstruccionId = contratoConstruccion.ContratoConstruccionId;

                lstContratoObservacion = _context.ConstruccionObservacion.Where(r => r.ContratoConstruccionId == contratoConstruccion.ContratoConstruccionId && r.EsSupervision == pEsSupervisor && r.EsActa == true).ToList();
                lstContratoObservacion = lstContratoObservacion.OrderByDescending(r => r.ConstruccionObservacionId).ToList();

                contratoObservacion = lstContratoObservacion.Where(r => r.ContratoConstruccionId == contratoConstruccion.ContratoConstruccionId).FirstOrDefault();
                return contratoObservacion;
            }
            return null;
        }

        public async Task<List<GrillaActaInicio>> GetListGrillaActaInicio(int pPerfilId)
        {
            List<GrillaActaInicio> lstActaInicio = new List<GrillaActaInicio>();
            List<Contrato> lstContratos = await _context.Contrato.Where(r => !(bool)r.Eliminado && r.FechaAprobacionRequisitosSupervisor.HasValue)
                .Include(r => r.Contratacion)
                .Include(r => r.ContratoObservacion)
                .OrderByDescending(r => r.FechaAprobacionRequisitosSupervisor)
                .ToListAsync();

            List<Dominio> Listdominios = _context.Dominio.Where(r => r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_actas_inicio_obra || r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_acta_Interventoria_319 || r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Contrato).ToList();

            foreach (var Contrato in lstContratos)
            {
                if (string.IsNullOrEmpty(Contrato.EstadoActa))
                    Contrato.EstadoActa = ConstanCodigoEstadoActaContrato.Sin_Revision;

                string EstadoActa = !string.IsNullOrEmpty(Contrato.EstadoActa) ? Listdominios.Where(r => r.Codigo == Contrato.EstadoActa && (r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_actas_inicio_obra || r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_acta_Interventoria_319)).FirstOrDefault().Nombre : " ";
                if (pPerfilId != (int)EnumeratorPerfil.Tecnica)
                    EstadoActa = !string.IsNullOrEmpty(Contrato.EstadoActa) ? Listdominios.Where(r => r.Codigo == Contrato.EstadoActa && (r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_actas_inicio_obra || r.TipoDominioId == (int)EnumeratorTipoDominio.Estados_acta_Interventoria_319)).FirstOrDefault().Descripcion : " ";

                lstActaInicio.Add(new GrillaActaInicio
                {
                    ContratoId = Contrato.ContratoId,
                    EstadoActa = EstadoActa,
                    EstadoVerificacion = Contrato.EstadoVerificacionCodigo,
                    EstadoActaCodigo = Contrato.EstadoActa,
                    FechaAprobacionRequisitosDate = Contrato.FechaAprobacionRequisitosSupervisor,
                    NumeroContratoObra = Contrato.NumeroContrato,
                    TipoContrato = Contrato.Contratacion.TipoSolicitudCodigo,
                    TipoContratoNombre = !string.IsNullOrEmpty(Contrato.Contratacion.TipoSolicitudCodigo) ? Listdominios.Where(r => r.Codigo == Contrato.Contratacion.TipoSolicitudCodigo && r.TipoDominioId == (int)EnumeratorTipoDominio.Tipo_Contrato).FirstOrDefault().Nombre : " ",
                });
            }

            return lstActaInicio;

        }

        public async Task GetListContratoConActaSinDocumento(AppSettingsService appSettingsService)
        {
            DateTime RangoFechaConDiasHabiles = await _commonService.CalculardiasLaborales(2, DateTime.Now);

            List<Contrato> contratos = _context.Contrato
                .Where(r => (r.EstadoActa == "8" || r.EstadoActa == "20") && string.IsNullOrEmpty(r.RutaActaFase1))
                 .Include(r => r.ContratoPoliza)
                 .Include(r => r.Contratacion).ThenInclude(r => r.DisponibilidadPresupuestal)
               .ToList();

            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Interventor || x.PerfilId == (int)EnumeratorPerfil.Supervisor || x.PerfilId == (int)EnumeratorPerfil.Tecnica).Include(y => y.Usuario);
            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.ConActaSinDocumento319);
            foreach (var contrato in contratos)
            {
                int Dias = 0, Meses = 0;
                Dias = contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault().PlazoDias ?? 0;
                Meses = contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault().PlazoMeses ?? 0;

                string template = TemplateRecoveryPassword.Contenido
                            .Replace("_LinkF_", appSettingsService.DominioFront)
                            .Replace("[TIPO_CONTRATO]", contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString() ? ConstanCodigoTipoContratacionSTRING.Obra : ConstanCodigoTipoContratacionSTRING.Interventoria)
                            .Replace("[NUMERO_CONTRATO]", contrato.NumeroContrato)
                            .Replace("[FECHA_PREVISTA_TERMINACION]", ((DateTime)contrato.Contratacion.DisponibilidadPresupuestal.FirstOrDefault().FechaSolicitud.AddDays(Dias).AddMonths(Meses)).ToString("dd-MM-yy"))
                            .Replace("[FECHA_POLIZA]", ((DateTime)contrato.ContratoPoliza.FirstOrDefault().FechaAprobacion).ToString("dd-MM-yy"))
                            .Replace("[FECHA_ACTA_INICIO]", contrato.FechaActaInicioFase1.HasValue ? ((DateTime)contrato.FechaActaInicioFase1).ToString("dd-MM-yy") : " ")
                            .Replace("[CANTIDAD_PROYECTOS]", contrato.Contratacion.ContratacionProyecto.Where(r => !r.Eliminado).Count().ToString());

                foreach (var item in usuarios)
                {
                    Helpers.Helpers.EnviarCorreo(item.Usuario.Email, "Tiene solicitudes pendientes por revisión", template, appSettingsService.Sender, appSettingsService.Password, appSettingsService.MailServer, appSettingsService.MailPort);
                }

            }

        }

        public async Task GetEnviarActaParaFirmar(AppSettingsService appSettingsService, int ContratoId)
        {
            List<Contrato> contratos = _context.Contrato.Where(r => r.ContratoId == ContratoId)
                .Include(r => r.ContratoPoliza)
                .Include(r => r.Contratacion)
                   .ThenInclude(r => r.ContratacionProyecto)
                .Include(r => r.Contratacion)
                   .ThenInclude(r => r.DisponibilidadPresupuestal)
                .ToList();

            var usuarios = _context.UsuarioPerfil.Where(x => x.PerfilId == (int)EnumeratorPerfil.Interventor || x.PerfilId == (int)EnumeratorPerfil.Supervisor || x.PerfilId == (int)EnumeratorPerfil.Apoyo).Include(y => y.Usuario);
            Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.EnviarActaParaFirmar319);
            foreach (var contrato in contratos)
            {
                int Dias = 0, Meses = 0;
                Dias = contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault().PlazoDias ?? 0;
                Meses = contrato?.Contratacion?.DisponibilidadPresupuestal?.FirstOrDefault().PlazoMeses ?? 0;

                string template = TemplateRecoveryPassword.Contenido
                            .Replace("_LinkF_", appSettingsService.DominioFront)
                            .Replace("[TIPO_CONTRATO]", contrato.Contratacion.TipoSolicitudCodigo == ConstanCodigoTipoContratacion.Obra.ToString() ? ConstanCodigoTipoContratacionSTRING.Obra : ConstanCodigoTipoContratacionSTRING.Interventoria)
                            .Replace("[NUMERO_CONTRATO]", contrato.NumeroContrato)
                            .Replace("[FECHA_PREVISTA_TERMINACION]", ((DateTime)contrato.Contratacion.DisponibilidadPresupuestal.FirstOrDefault().FechaSolicitud.AddDays(Dias).AddMonths(Meses)).ToString("dd-MM-yy"))
                            .Replace("[FECHA_POLIZA]", ((DateTime)contrato.ContratoPoliza.FirstOrDefault().FechaAprobacion).ToString("dd-MM-yy"))
                            .Replace("[FECHA_ACTA_INICIO]", contrato.FechaActaInicioFase1.HasValue ? ((DateTime)contrato.FechaActaInicioFase1).ToString("dd-MM-yy") : " ")
                            .Replace("[CANTIDAD_PROYECTOS]", contrato.Contratacion.ContratacionProyecto.Where(r => !r.Eliminado).Count().ToString());

                foreach (var item in usuarios)
                {
                    Helpers.Helpers.EnviarCorreo(item.Usuario.Email, "Generación de acta para fase 1 - Preconstrucción", template, appSettingsService.Sender, appSettingsService.Password, appSettingsService.MailServer, appSettingsService.MailPort);
                }

            }

        }
    }

}

