﻿using asivamosffie.model.AditionalModels;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace asivamosffie.services
{
    public class CommonService : ICommonService
    {
        private readonly devAsiVamosFFIEContext _context;
        public MailSettings _mailSettings { get; }
        private readonly string _connectionString;
        public CommonService(
            IConfiguration configuration,
                                devAsiVamosFFIEContext context,
                               IOptions<MailSettings> mailSettings
                            )
        {
            _connectionString = configuration.GetConnectionString("asivamosffieDatabase");
            _mailSettings = mailSettings.Value;
            _context = context;
        }
        public async Task<dynamic> GetVideos()
        {
            return await _context.Dominio
                                       .Where(d => d.TipoDominioId == (int)EnumeratorTipoDominio.Videos)
                                       .Select(r => 
                                                   new { 
                                                       r.Nombre,
                                                       r.Descripcion 
                                                   }
                                      ).ToListAsync(); 
        }


        public async Task<byte[]> GetHtmlToPdf(Plantilla pPlantilla)
        {
            Plantilla plantilla =
                                  await _context.Plantilla
                                                          .Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Html_To_PDF).ToString())
                                                          .Include(r => r.Encabezado).Include(r => r.PieDePagina)
                                                          .FirstOrDefaultAsync();

            plantilla.MargenAbajo = pPlantilla.MargenAbajo;
            plantilla.MargenArriba = pPlantilla.MargenArriba;
            plantilla.MargenDerecha = pPlantilla.MargenDerecha;
            plantilla.MargenIzquierda = pPlantilla.MargenIzquierda;

            plantilla.Contenido = plantilla.Contenido.Replace("[HTML]", pPlantilla.Contenido);
            return PDF.Convertir(plantilla, pPlantilla.EsHorizontal);
        }


        public async Task<Plantilla> GetPlantillaById(int pPlantillaId)
        {
            return await _context.Plantilla.FindAsync(pPlantillaId);
        }

        //Solicitudes de comite tecnico
        public async Task<dynamic> GetRequestSP(string pNameSP)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(pNameSP, sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<dynamic>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToValue(reader));
                        }
                    }
                    return response;
                }
            }
        }

        public dynamic MapToValue(SqlDataReader reader)
        {
            return new
            {
                ContratacionId = (int)reader["ContratacionId"],
                DisponibilidadPresupuestalId = (int)reader["DisponibilidadPresupuestalId"],
                SesionComiteSolicitudId = (int)reader["SesionComiteSolicitudId"],
                FechaSolicitud = (DateTime)reader["FechaSolicitud"],
                TipoSolicitudText = reader["TipoSolicitudText"].ToString(),
                NumeroSolicitud = reader["NumeroSolicitud"].ToString(),
                OpcionContratar = reader["OpcionContratar"].ToString(),
                ValorSolicitud = (decimal)reader["ValorSolicitud"],
                FechaComite = (DateTime)reader["FechaComite"],
            };
        }

        public async Task<VPermisosMenus> TienePermisos(int idPerfil, string pRuta)
        {
            VPermisosMenus vPermisosMenus = await _context.VPermisosMenus
                .Where(vp => vp.RutaFormulario == pRuta
                          && vp.PerfilId == idPerfil)
                .FirstOrDefaultAsync();

            if (vPermisosMenus != null)
                vPermisosMenus.TienePermisoMenu = true;
            return vPermisosMenus;
        }

        public bool EnviarCorreo(List<EnumeratorPerfil> pListPerfilesCorreo, string pContenido, string pAsunto)
        {
            try
            {
                pContenido = pContenido
                                        .Replace("_LinkF_", _mailSettings.DominioFront)
                                        .Replace("[URL]", _mailSettings.DominioFront);

                var ListEmails =
                               _context.UsuarioPerfil
                                                   .Where(p => pListPerfilesCorreo
                                                   .Contains((EnumeratorPerfil)(int)p.PerfilId))
                                                   .Include(u => u.Usuario)
                                                   .Select(e => e.Usuario.Email)
                                                   .ToList();

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(_mailSettings.MailServer);

                mail.From = new MailAddress(_mailSettings.Sender);

                foreach (var Destinatario in ListEmails)
                {
                    mail.To.Add(Destinatario);
                }

                mail.Subject = pAsunto;
                mail.IsBodyHtml = true;

                mail.Body = pContenido;
                SmtpServer.Port = _mailSettings.MailPort;
                SmtpServer.Credentials = new NetworkCredential(_mailSettings.Sender, _mailSettings.Password);
                SmtpServer.EnableSsl = _mailSettings.EnableSSL;
                SmtpServer.Send(mail);

            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool EnviarCorreo(List<string> pListCorreo, string pContenido, string pAsunto)
        {
            try
            {
                pContenido = pContenido
                    .Replace("_LinkF_", _mailSettings.DominioFront)
                    .Replace("[URL]", _mailSettings.DominioFront);

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(_mailSettings.Sender)
                };

                foreach (var email in pListCorreo)
                {
                    mail.To.Add(email);
                }

                mail.Subject = pAsunto;
                mail.IsBodyHtml = true;
                mail.Body = pContenido;

                SmtpClient SmtpServer = new SmtpClient(_mailSettings.MailServer)
                {
                    Port = _mailSettings.MailPort,
                    Credentials = new NetworkCredential(_mailSettings.Sender, _mailSettings.Password),
                    EnableSsl = _mailSettings.EnableSSL
                };
                SmtpServer.Send(mail);

            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public async Task<dynamic> GetListMenu()
        {
            return await _context.Menu.Select(m => new
            {
                m.MenuId,
                m.Nombre
            }).ToListAsync();
        }

        public async Task<List<dynamic>> GetUsuarioByPerfil(int idPerfil)
        {

            List<dynamic> ListaUsuario = new List<dynamic>();

            var ListUsuarios = await _context.UsuarioPerfil.Where(r => r.PerfilId == idPerfil && (bool)r.Activo).Select(r => r.Usuario).Where(r => !(bool)r.Eliminado).Distinct().OrderBy(r => r.PrimerNombre).ToListAsync();

            foreach (var item in ListUsuarios)
            {
                ListaUsuario.Add(
                                    new
                                    {
                                        item.UsuarioId,
                                        item.PrimerNombre,
                                        item.PrimerApellido
                                    }
                                );
            }
            return ListaUsuario;
        }

        public async Task<string> EnumeradorActualizarPoliza()
        {
            int cantidadDeResgistros = _context.ContratoPolizaActualizacion.Count();
            string Nomeclatura = "Act_";
            string consecutivo = (cantidadDeResgistros + 1).ToString("000");
            return string.Concat(Nomeclatura, consecutivo);
        }

        public async Task<string> EnumeradorOrigenOrdenGiro()
        {
            int cantidadDeResgistros = _context.ComiteTecnico.Count();
            string Nomeclatura = "FFIE_";
            string consecutivo = (cantidadDeResgistros + 1).ToString("000");
            return string.Concat(Nomeclatura, consecutivo);
        }

        public async Task<string> EnumeradorOrdenGiro(int SolicitudPagoId)
        {
            SolicitudPago solicitudPago = _context.SolicitudPago.Where(s => s.SolicitudPagoId == SolicitudPagoId)
                                 .Include(c => c.Contrato)
                                 .ThenInclude(c => c.Contratacion)
                                 .FirstOrDefault();


            int cantidadDeResgistros = _context.OrdenGiro.Where(r => !string.IsNullOrEmpty(r.NumeroSolicitud)).Count();
            string Nomeclatura = "ODG_";

            if (solicitudPago.Contrato.Contratacion.TipoSolicitudCodigo == (ConstanCodigoTipoContratacion.Obra).ToString())
                Nomeclatura += "Obr";
            else
                Nomeclatura += "Int";

            Nomeclatura += solicitudPago.Contrato.NumeroContrato;

            string consecutivo = (cantidadDeResgistros + 1).ToString("000");
            return string.Concat(Nomeclatura, consecutivo);
        }

        public async Task<string> EnumeradorSolicitudPagoExpensasAndOtros()
        {
            int cantidadDeResgistros = _context.SolicitudPago.Count();
            string Nomeclatura = "SolPagoEspecial";
            string consecutivo = (cantidadDeResgistros + 1).ToString("000");
            return string.Concat(Nomeclatura, consecutivo);
        }
        public string EnumeradorTrasladoBalanceFinanciero()
        {
            int cantidadDeResgistros = _context.BalanceFinancieroTraslado.Count();
            string Nomeclatura = "Tras_";
            string consecutivo = (cantidadDeResgistros + 1).ToString("000");
            return string.Concat(Nomeclatura, consecutivo);
        }


        public async Task<string> EnumeradorSolicitudPago(bool esObra)
        {
            int cantidadDeResgistros = _context.SolicitudPago.Count();
            string Nomeclatura = "SolPagoI";
            if (esObra)
                Nomeclatura = "SolPagoO";
            string consecutivo = (cantidadDeResgistros + 1).ToString("000");
            return string.Concat(Nomeclatura, consecutivo);
        }

        public async Task<string> EnumeradorComiteTecnico()
        {
            int cantidadDeResgistros = _context.ComiteTecnico.Where(ct => ct.EsComiteFiduciario == false || ct.EsComiteFiduciario == false).Count();
            string Nomeclatura = "CT_";
            string consecutivo = (cantidadDeResgistros + 1).ToString("000");
            return string.Concat(Nomeclatura, consecutivo);
        }

        public async Task<string> EnumeradorComiteObra()
        {
            int cantidadDeResgistros = _context.SeguimientoSemanalRegistrarComiteObra.Count();
            string Nomeclatura = "CObra__";
            string consecutivo = (cantidadDeResgistros + 1).ToString("000");
            return string.Concat(Nomeclatura, consecutivo);
        }

        public async Task<string> EnumeradorComiteFiduciario()
        {
            int cantidadDeResgistros = _context.ComiteTecnico.Where(ct => ct.EsComiteFiduciario == true).Count();
            string Nomeclatura = "CF_";
            string consecutivo = (cantidadDeResgistros + 1).ToString("000");
            return string.Concat(Nomeclatura, consecutivo);
        }

        public async Task<string> EnumeradorContratacion()
        {
            int cantidadDeResgistros = _context.Contratacion.Count();
            string Nomeclatura = "PI_";
            string consecutivo = (cantidadDeResgistros + 1).ToString("000");
            return string.Concat(Nomeclatura, consecutivo);
        }

        public async Task<List<MenuPerfil>> GetMenuByRol(int pUserId)
        {
            int IdPerfil = await _context.UsuarioPerfil.Where(r => r.UsuarioId == pUserId).Select(r => r.PerfilId).FirstOrDefaultAsync();
            List<MenuPerfil> menuPerfil = _context.MenuPerfil
            .Where(r => r.PerfilId == IdPerfil && (bool)r.Activo)
            .IncludeFilter(r => r.Menu)
            .OrderByDescending(r => r.Menu.Posicion.HasValue)
                .ThenBy(r => r.Menu.Posicion)
            .ToList();

            return menuPerfil;
            //return _context.MenuPerfil.Where(r => r.PerfilId == IdPerfil && (bool)r.Activo).IncludeFilter(r => r.Menu).OrderBy(z => z.Menu.Posicion).ToList();
        }

        public string GetNombreDepartamentoByIdMunicipio(string pIdMunicipio)
        {
            //no se puede hacer retornando el include ya que id elPadre no esta FK con el padre en base de datos
            string idPadre = _context.Localizacion.Where(r => r.LocalizacionId.Equals(pIdMunicipio)).Select(r => r.IdPadre).FirstOrDefault();
            return _context.Localizacion.Where(r => r.LocalizacionId.Equals(idPadre)).FirstOrDefault().Descripcion;
        }

        public string GetNombreRegionByIdMunicipio(string pIdMunicipio)
        {
            //no se puede hacer retornando el include ya que id elPadre no esta FK con el padre en base de datos
            string idDepartamento = _context.Localizacion.Where(r => r.LocalizacionId.Equals(pIdMunicipio)).Select(r => r.IdPadre).FirstOrDefault();
            string idRegion = _context.Localizacion.Where(r => r.LocalizacionId.Equals(idDepartamento)).FirstOrDefault().IdPadre;
            return _context.Localizacion.Where(r => r.LocalizacionId.Equals(idRegion)).FirstOrDefault().Descripcion;
        }

        public async Task<List<Perfil>> GetProfile()
        {
            return await _context.Perfil.ToListAsync();
        }

        public async Task<List<Usuario>> GetUsuariosByPerfil(int pIdPerfil)
        {
            return await _context.UsuarioPerfil.Where(u => u.PerfilId == pIdPerfil)
                                                .Include(u => u.Usuario)
                                                .Select(s => s.Usuario).ToListAsync();
        }

        public async Task<Template> GetTemplateById(int pId)
        {
            return await _context.Template.Where(r => r.TemplateId == pId && r.Activo == true).FirstOrDefaultAsync();
        }

        public async Task<Template> GetTemplateByTipo(string ptipo)
        {
            return await _context.Template.Where(r => r.Tipo.Equals(ptipo) && (bool)r.Activo).FirstOrDefaultAsync();
        }

        public async Task<List<Dominio>> GetListDominioByIdTipoDominio(int pIdTipoDominio)
        {
            //JMARTINEZ
            List<Dominio> listDominio = await _context.Dominio.Where(r => r.TipoDominioId == pIdTipoDominio && (bool)r.Activo).ToListAsync();
            //Vuelve todo mayuscula
            listDominio.ForEach(r => r.Nombre.ToUpper());
            return listDominio;
        }

        public async Task<string> GetMensajesValidacionesByModuloAndCodigo(int pMenu, string pCodigo, int pAccionId, string pUsuario, string pObservaciones)
        {
            var retorno = await _context.MensajesValidaciones.Where(r => (bool)r.Activo && r.MenuId == pMenu && r.Codigo.Equals(pCodigo)).FirstOrDefaultAsync();
            /*almaceno auditoria*/
            _context.Auditoria.Add(new Auditoria { AccionId = pAccionId, MensajesValidacionesId = retorno.MensajesValidacionesId, Usuario = pUsuario == null ? "" : pUsuario.ToUpper(), Observacion = pObservaciones.ToUpper(), Fecha = DateTime.Now });
            _context.SaveChanges();
            return retorno.Mensaje;
        }

        public async Task<int> GetDominioIdByCodigoAndTipoDominio(string pCodigo, int pTipoDominioId)
        {
            return await _context.Dominio.Where(r => (bool)r.Activo && r.Codigo.Equals(pCodigo) && r.TipoDominioId == pTipoDominioId).Select(r => r.DominioId).FirstOrDefaultAsync();
        }

        public async Task<List<Localicacion>> GetListDepartamento()
        {

            return await _context.Localizacion.Where(r => r.Nivel == 1)
            .Select(x => new Localicacion
            {
                LocalizacionId = x.LocalizacionId,
                Descripcion = x.Descripcion.ToLower()//jflorez lo paso a min para usar en frontedn la clase capitalize
            }).ToListAsync();
        }

        public async Task<List<Localicacion>> GetListMunicipioByIdDepartamento(string pIdDepartamento)
        {
            if (!string.IsNullOrEmpty(pIdDepartamento))
            {
                return await _context.Localizacion.Where(r => r.IdPadre.Equals(pIdDepartamento)).Select(x => new Localicacion
                {
                    LocalizacionId = x.LocalizacionId,
                    Descripcion = x.Descripcion.ToLower()//jflorez lo cambio para usar en  fron la clase capitalize
                }).ToListAsync();
            }
            else
            {
                return await _context.Localizacion.Where(r => r.Nivel == 2).Select(x => new Localicacion
                {
                    LocalizacionId = x.LocalizacionId,
                    Descripcion = x.Descripcion.ToLower()//jflorez lo cambio para usar en  fron la clase capitalize
                }).ToListAsync();
            }

        }

        public async Task<List<int>> GetListVigenciaAportes(string pYearVigente, bool yearSiguienteEsVigente)
        {
            try
            {
                List<int> YearVigencia = new List<int>();

                int intYearDesde = Int32.Parse(pYearVigente);
                int yearHasta = (yearSiguienteEsVigente) ? DateTime.Now.AddYears(1).Year : DateTime.Now.Year;

                for (int i = intYearDesde; i < yearHasta + 1; i++)
                {
                    YearVigencia.Add(i);
                }

                return YearVigencia;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GetDominioIdByNombreDominioAndTipoDominio(string pNombreDominio, int pTipoDominioId)
        {
            return await _context.Dominio.Where(r => (bool)r.Activo && r.Nombre.Trim().ToUpper().Equals(pNombreDominio.Trim().ToUpper()) && r.TipoDominioId == pTipoDominioId).Select(r => r.DominioId).FirstOrDefaultAsync();

        }

        public async Task<int> GetLocalizacionIdByName(string pNombre, string pIdDepartamento)
        {
            if (pIdDepartamento.Equals("0"))
                return Int32.Parse(await _context.Localizacion.Where(r => r.Nivel == 1 && r.Descripcion.Trim().ToUpper().Equals(pNombre.Trim().ToUpper())).Select(r => r.LocalizacionId).FirstOrDefaultAsync());
            else
                return Int32.Parse(await _context.Localizacion.Where(r => r.IdPadre.Contains(pIdDepartamento) && r.Nivel == 2 && r.Descripcion.Trim().ToUpper().Equals(pNombre.Trim().ToUpper())).Select(r => r.LocalizacionId).FirstOrDefaultAsync());
        }

        public async Task<int> getInstitucionEducativaIdByName(string pNombre)
        {

            return await _context.InstitucionEducativaSede.Where(r => (bool)r.Activo && r.Nombre.ToUpper().Equals(pNombre.ToUpper())).Select(r => r.InstitucionEducativaSedeId).FirstOrDefaultAsync();
        }

        public async Task<string> GetDominioCodigoByNombreDominioAndTipoDominio(string pCodigo, int pTipoDominioId)
        {
            return await _context.Dominio.Where(r => (bool)r.Activo && r.Nombre.Trim().ToUpper().Equals(pCodigo.Trim().ToUpper()) && r.TipoDominioId == pTipoDominioId).Select(r => r.Codigo).FirstOrDefaultAsync();
        }

        public async Task<int> getSedeInstitucionEducativaIdByNameAndInstitucionPadre(string pNombre, int pIdPadre)
        {

            return await _context.InstitucionEducativaSede.Where(r => (bool)r.Activo && r.PadreId == pIdPadre && r.Nombre.Equals(pNombre)).Select(r => r.InstitucionEducativaSedeId).FirstOrDefaultAsync();
        }

        public async Task<int> getInstitucionEducativaIdByCodigoDane(int pCodigoDane)
        {

            return await _context.InstitucionEducativaSede.Where(r => (bool)r.Activo && r.CodigoDane == pCodigoDane.ToString()).Select(r => r.InstitucionEducativaSedeId).FirstOrDefaultAsync();
        }

        public async Task<Localizacion> GetLocalizacionByLocalizacionId(string pLocalizacionId)
        {
            return await _context.Localizacion.Where(r => r.LocalizacionId.Equals(pLocalizacionId)).FirstOrDefaultAsync();
        }

        public async Task<ContratoPoliza> GetContratoPolizaByContratoId(int pContratoId)
        {
            return await _context.ContratoPoliza.Where(r => r.ContratoId.Equals(pContratoId)).FirstOrDefaultAsync();
        }

        public async Task<ContratoPoliza> GetLastContratoPolizaByContratoId(int pContratoId)
        {
            return await _context.ContratoPoliza.Where(r => r.ContratoId.Equals(pContratoId)).OrderByDescending(x => x.ContratoPolizaId).FirstOrDefaultAsync();
        }

        public async Task<Contratacion> GetContratacionByContratacionId(int pContratacionId)
        {
            return await _context.Contratacion.Where(r => r.ContratacionId.Equals(pContratacionId)).Include(r => r.PlazoContratacion).FirstOrDefaultAsync();
        }

        public async Task<Contratista> GetContratistaByContratistaId(int pContratistaId)
        {
            return await _context.Contratista.Where(r => r.ContratistaId.Equals(pContratistaId)).FirstOrDefaultAsync();
        }
        public string GetNombreLocalizacionByLocalizacionId(string pLocalizacionId)
        {
            return _context.Localizacion.Where(r => r.LocalizacionId.Equals(pLocalizacionId)).Select(r => r.Descripcion).FirstOrDefault();

        }

        public async Task<Localizacion> GetDepartamentoByIdMunicipio(string pIdMunicipio)
        {
            //no se puede hacer retornando el include ya que id elPadre no esta FK con el padre en base de datos
            string idPadre = await _context.Localizacion.Where(r => r.LocalizacionId.Equals(pIdMunicipio)).Select(r => r.IdPadre).FirstOrDefaultAsync();
            return await _context.Localizacion.Where(r => r.LocalizacionId.Equals(idPadre)).FirstOrDefaultAsync();
        }

        public async Task<List<Localicacion>> ListDepartamentoByRegionId(string pIdRegion)
        {
            if (!string.IsNullOrEmpty(pIdRegion) && !pIdRegion.Contains("7"))
            {
                return await _context.Localizacion.Where(r => r.IdPadre.Equals(pIdRegion)).Select(x => new Localicacion
                {
                    LocalizacionId = x.LocalizacionId,
                    Descripcion = x.Descripcion.ToLower()
                }).ToListAsync();
            }
            else
            {
                return await _context.Localizacion.Where(r => r.Nivel == 1).Select(x => new Localicacion
                {
                    LocalizacionId = x.LocalizacionId,
                    Descripcion = x.Descripcion.ToLower()
                }).ToListAsync();
            }
        }

        public async Task<List<Localicacion>> ListRegion()
        {
            return await _context.Localizacion.Where(r => r.Nivel == 3).Select(x => new Localicacion
            {
                LocalizacionId = x.LocalizacionId,
                Descripcion = x.Descripcion.ToLower()
            }).ToListAsync();
        }

        public async Task<Dominio> GetDominioByNombreDominioAndTipoDominio(string pCodigo, int pTipoDominioId)
        {
            return await _context.Dominio.Where(r => (bool)r.Activo && r.Codigo.Equals(pCodigo) && r.TipoDominioId == pTipoDominioId).FirstOrDefaultAsync();
        }

        public async Task<List<InstitucionEducativaSede>> ListIntitucionEducativaByMunicipioId(string pIdMunicipio)
        {
            return await _context.InstitucionEducativaSede.Where(r => (bool)r.Activo && r.PadreId == null && r.LocalizacionIdMunicipio.Trim().Equals(pIdMunicipio.Trim())).ToListAsync();
        }

        public async Task<List<InstitucionEducativaSede>> ListSedeByInstitucionEducativaId(int pInstitucionEducativaCodigo)
        {
            return await _context.InstitucionEducativaSede.Where(r => (bool)r.Activo && r.PadreId == pInstitucionEducativaCodigo).ToListAsync();
        }

        public async Task<string> GetNombreDominioByCodigoAndTipoDominio(string pCodigo, int pTipoDominioId)
        {
            string strNombreDominio = await _context.Dominio.Where(r => r.Codigo.Equals(pCodigo) && r.TipoDominioId == pTipoDominioId).Select(r => r.Nombre).FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(strNombreDominio))
            {
                return "Error Parametrica";
            }
            return strNombreDominio;
        }

        public async Task<string> GetNombreDominioByDominioID(int pDominioID)
        {
            return await _context.Dominio.Where(r => r.DominioId == pDominioID).Select(r => r.Nombre).FirstOrDefaultAsync();
        }

        public async Task<List<Localicacion>> GetListMunicipioByIdMunicipio(string idMunicipio)
        {
            var munactual = _context.Localizacion.Find(idMunicipio);
            return await _context.Localizacion.Where(r => r.Nivel == 2 && r.IdPadre == munactual.IdPadre)
             .Select(x => new Localicacion
             {
                 LocalizacionId = x.LocalizacionId,
                 Descripcion = x.Descripcion.ToLower(),//jflorez lo paso a min para usar en frontedn la clase capitalize
                 IdPadre = x.IdPadre
             }).ToListAsync();
        }

        public async Task<List<Localicacion>> GetListDepartamentoByIdMunicipio(string idMunicipio)
        {
            var munactual = _context.Localizacion.Find(idMunicipio);
            var depactual = _context.Localizacion.Find(munactual.IdPadre);
            //var regactual = _context.Localizacion.Find(depactual.IdPadre);
            return await _context.Localizacion.Where(r => r.Nivel == 1 && r.IdPadre == depactual.IdPadre)
             .Select(x => new Localicacion
             {
                 LocalizacionId = x.LocalizacionId,
                 Descripcion = x.Descripcion.ToLower(),//jflorez lo paso a min para usar en frontedn la clase capitalize
                 IdPadre = x.IdPadre
             }).ToListAsync();
        }

        public async Task<InstitucionEducativaSede> GetInstitucionEducativaById(int InstitucionEducativaById)
        {
            return await _context.InstitucionEducativaSede.FindAsync(InstitucionEducativaById);
        }

        public static DateTime PrimerDiaMes(int anio, int mes, DayOfWeek dia, int diaBase)
        {
            DateTime fechaRetorno = new DateTime(anio, mes, diaBase);
            while (fechaRetorno.DayOfWeek != dia)
            {
                fechaRetorno = fechaRetorno.AddDays(1);
            }
            return fechaRetorno;
        }

        public static DateTime[] DiasFestivosAnio(int anio)
        {
            List<DateTime> fechas = new List<DateTime>
            {
                ////Fechas fijas
                //año nuevo
                new DateTime(anio, 1, 1),
                //trabajo
                new DateTime(anio, 5, 1),
                //grito independencia
                new DateTime(anio, 7, 20),
                //Batalla boyacá
                new DateTime(anio, 8, 7),
                //Inmaculada concepcion
                new DateTime(anio, 12, 8),
                //Navidad
                new DateTime(anio, 12, 25),
                ////Fechas calculadas
                ////Epifanía Primer lunes de enero
                PrimerDiaMes(anio, 1, DayOfWeek.Monday, 6),
                ////San José Primer lunes de marzo 
                PrimerDiaMes(anio, 3, DayOfWeek.Monday, 19),
                //san Pedro y San Pablo Primer Lunes de julio
                PrimerDiaMes(anio, 6, DayOfWeek.Monday, 29),
                //Asunción de la virgen agosto Primer Lunes
                PrimerDiaMes(anio, 8, DayOfWeek.Monday, 15),
                //Dia de La Raza primer lunes de octubre
                PrimerDiaMes(anio, 10, DayOfWeek.Monday, 12),
                //Todos los Santos primer lunes a partir del 1 de noviembre
                PrimerDiaMes(anio, 11, DayOfWeek.Monday, 1),
                //Independencia de Cartagena primer lunes a partir del 11 de noviembre
                PrimerDiaMes(anio, 11, DayOfWeek.Monday, 11)
            };
            ////Fechas con domingo de pascua
            DateTime domingopascua = DomingoPascua(anio);
            //Jueves santo anterior al domingo de pascua
            fechas.Add(DiaMesCantidad(anio, domingopascua.Month, DayOfWeek.Thursday, domingopascua.Day, 1, true));
            //viernes santo anterior al domingo de pascua
            fechas.Add(DiaMesCantidad(anio, domingopascua.Month, DayOfWeek.Friday, domingopascua.Day, 1, true));
            //Ascension de Jesus 7 despues del domingo de pascua
            fechas.Add(DiaMesCantidad(anio, domingopascua.Month, DayOfWeek.Monday, domingopascua.Day, 7));
            //Ascension de Jesus 10 despues del domingo de pascua
            fechas.Add(DiaMesCantidad(anio, domingopascua.Month, DayOfWeek.Monday, domingopascua.Day, 10));
            //Sagrado Corazon de Jesus 11 despues del domingo de pascua
            fechas.Add(DiaMesCantidad(anio, domingopascua.Month, DayOfWeek.Monday, domingopascua.Day, 11));
            return fechas.ToArray();
        }

        public static DateTime DiaMesCantidad(int anio, int mes, DayOfWeek dia, int diaBase, int cantidadVeces, bool antes = false)
        {
            DateTime fechaRetorno = new DateTime(anio, mes, diaBase);
            int cantidadActual = 0;
            int suma = 1;
            if (antes)
            {
                suma = -1;
            }
            while (fechaRetorno.DayOfWeek != dia || cantidadVeces != cantidadActual)
            {
                fechaRetorno = fechaRetorno.AddDays(suma);
                if (fechaRetorno.DayOfWeek == dia)
                {
                    cantidadActual++;
                }
            }
            return fechaRetorno;
        }

        public static DateTime DomingoPascua(int anyo)
        {
            int M = 25;
            int N = 5;

            if (anyo >= 1583 && anyo <= 1699) { M = 22; N = 2; }
            else if (anyo >= 1700 && anyo <= 1799) { M = 23; N = 3; }
            else if (anyo >= 1800 && anyo <= 1899) { M = 23; N = 4; }
            else if (anyo >= 1900 && anyo <= 2099) { M = 24; N = 5; }
            else if (anyo >= 2100 && anyo <= 2199) { M = 24; N = 6; }
            else if (anyo >= 2200 && anyo <= 2299) { M = 25; N = 0; }

            int a, b, c, d, e, dia, mes;

            //Cálculo de residuos
            a = anyo % 19;
            b = anyo % 4;
            c = anyo % 7;
            d = (19 * a + M) % 30;
            e = (2 * b + 4 * c + 6 * d + N) % 7;

            // Decidir entre los 2 casos:
            if (d + e < 10) { dia = d + e + 22; mes = 3; }
            else { dia = d + e - 9; mes = 4; }

            // Excepciones especiales
            if (dia == 26 && mes == 4) dia = 19;
            if (dia == 25 && mes == 4 && d == 28 && e == 6 && a > 10) dia = 18;

            return new DateTime(anyo, mes, dia);
        }



        /// <summary>
        /// Julian Martinez
        /// </summary>
        /// <param name="dias">Cuantos dias habiles se agregan</param>
        /// <param name="pFechaCalcular">La fecha a calcular los dias habiles</param>
        /// <returns></returns>
        public async Task<DateTime> CalculardiasLaborales(int pDias, DateTime pFechaCalcular)
        {
            DateTime fechaInicial = pFechaCalcular;
            DateTime fechadiasHabiles = pFechaCalcular;

            for (int i = 0; i < pDias; i++)
            {
                fechadiasHabiles = fechadiasHabiles.AddDays(1);
                if (fechadiasHabiles.DayOfWeek == DayOfWeek.Saturday)
                {
                    fechadiasHabiles = fechadiasHabiles.AddDays(1);
                }
                if (fechadiasHabiles.DayOfWeek == DayOfWeek.Sunday)
                {
                    fechadiasHabiles = fechadiasHabiles.AddDays(1);
                }
            }
            List<DateTime> festivos = new List<DateTime>();

            festivos.AddRange(DiasFestivosAnio(fechaInicial.Year));

            festivos.AddRange(DiasFestivosAnio(fechaInicial.Year + 1));

            foreach (var festivo in festivos)
            {
                if (festivo >= fechaInicial && festivo <= fechadiasHabiles)
                {
                    fechadiasHabiles = fechadiasHabiles.AddDays(1);
                }
                if (fechadiasHabiles.DayOfWeek == DayOfWeek.Saturday)
                {
                    fechadiasHabiles = fechadiasHabiles.AddDays(1);
                }
                if (fechadiasHabiles.DayOfWeek == DayOfWeek.Sunday)
                {
                    fechadiasHabiles = fechadiasHabiles.AddDays(1);
                }
            }
            return fechadiasHabiles;
        }

        public static DateTime[] DiasFestivosAnioRetroceso(int anio)
        {
            List<DateTime> fechas = new List<DateTime>
            {
                ////Fechas fijas
                //año nuevo
                new DateTime(anio, 1, 1),
                //trabajo
                new DateTime(anio, 5, 1),
                //grito independencia
                new DateTime(anio, 7, 20),
                //Batalla boyacá
                new DateTime(anio, 8, 7),
                //Inmaculada concepcion
                new DateTime(anio, 12, 8),
                //Navidad
                new DateTime(anio, 12, 25),
                ////Fechas calculadas
                ////Epifanía Primer lunes de enero
                PrimerDiaMes(anio, 1, DayOfWeek.Monday, 6),
                ////San José Primer lunes de marzo 
                PrimerDiaMes(anio, 3, DayOfWeek.Monday, 19),
                //san Pedro y San Pablo Primer Lunes de julio
                PrimerDiaMes(anio, 6, DayOfWeek.Monday, 29),
                //Asunción de la virgen agosto Primer Lunes
                PrimerDiaMes(anio, 8, DayOfWeek.Monday, 15),
                //Dia de La Raza primer lunes de octubre
                PrimerDiaMes(anio, 10, DayOfWeek.Monday, 12),
                //Todos los Santos primer lunes a partir del 1 de noviembre
                PrimerDiaMes(anio, 11, DayOfWeek.Monday, 1),
                //Independencia de Cartagena primer lunes a partir del 11 de noviembre
                PrimerDiaMes(anio, 11, DayOfWeek.Monday, 11)
            };
            ////Fechas con domingo de pascua
            DateTime domingopascua = DomingoPascua(anio);
            //Jueves santo anterior al domingo de pascua
            fechas.Add(DiaMesCantidad(anio, domingopascua.Month, DayOfWeek.Thursday, domingopascua.Day, 1, true));
            //viernes santo anterior al domingo de pascua
            fechas.Add(DiaMesCantidad(anio, domingopascua.Month, DayOfWeek.Friday, domingopascua.Day, 1, true));
            //Ascension de Jesus 7 despues del domingo de pascua
            fechas.Add(DiaMesCantidad(anio, domingopascua.Month, DayOfWeek.Monday, domingopascua.Day, 7));
            //Ascension de Jesus 10 despues del domingo de pascua
            fechas.Add(DiaMesCantidad(anio, domingopascua.Month, DayOfWeek.Monday, domingopascua.Day, 10));
            //Sagrado Corazon de Jesus 11 despues del domingo de pascua
            fechas.Add(DiaMesCantidad(anio, domingopascua.Month, DayOfWeek.Monday, domingopascua.Day, 11));
            return fechas.ToArray();
        }

        public async Task<DateTime> CalculardiasLaboralesTranscurridos(int pDias, DateTime pFechaCalcular)
        {
            DateTime fechaInicial = pFechaCalcular;
            DateTime fechadiasHabiles = pFechaCalcular;

            for (int i = 0; i < pDias; i++)
            {
                fechadiasHabiles = fechadiasHabiles.AddDays(-1);
                if (fechadiasHabiles.DayOfWeek == DayOfWeek.Saturday)
                {
                    fechadiasHabiles = fechadiasHabiles.AddDays(-1);
                }
                if (fechadiasHabiles.DayOfWeek == DayOfWeek.Sunday)
                {
                    fechadiasHabiles = fechadiasHabiles.AddDays(-1);
                }
            }
            List<DateTime> festivos = new List<DateTime>();

            festivos.AddRange(DiasFestivosAnioRetroceso(fechaInicial.Year));

            festivos.AddRange(DiasFestivosAnioRetroceso(fechaInicial.Year + 1));

            foreach (var festivo in festivos)
            {
                if (festivo <= fechaInicial && festivo >= fechadiasHabiles)
                {
                    fechadiasHabiles = fechadiasHabiles.AddDays(-1);
                }
                if (fechadiasHabiles.DayOfWeek == DayOfWeek.Saturday)
                {
                    fechadiasHabiles = fechadiasHabiles.AddDays(-1);
                }
                if (fechadiasHabiles.DayOfWeek == DayOfWeek.Sunday)
                {
                    fechadiasHabiles = fechadiasHabiles.AddDays(-1);
                }
            }
            return fechadiasHabiles;
        }

        public DateTime? GetFechaEstimadaFinalizacion(int pContratoId)
        {
            DateTime? fechaFinalizacion = null;

            Contrato contrato = _context.Contrato.Find(pContratoId);
            if (contrato != null)
            {
                fechaFinalizacion = contrato.FechaTerminacionFase2 != null ? contrato.FechaTerminacionFase2 : contrato.FechaTerminacion != null ? contrato.FechaTerminacion : null;
                //obtener todas las novedades finalizadas las que no requieren comité, una vez tiene todas las firmas, las que requieren comité una vez se registra la modificación
                if (fechaFinalizacion != null)
                {
                    List<NovedadContractual> novedadContractuals = _context.NovedadContractual.Where(r => r.Eliminado != true && r.ContratoId == pContratoId && (r.EstadoCodigo == ConstanCodigoEstadoNovedadContractual.Firmado || r.EstadoCodigo == ConstanCodigoEstadoNovedadContractual.Registrado))
                                                                                            .Include(r => r.NovedadContractualDescripcion)
                                                                                            .ToList();
                    DateTime? fechaReinicio = null;

                    foreach (var novedad in novedadContractuals)
                    {
                        bool vaComite = false;

                        foreach (var descripcion in novedad.NovedadContractualDescripcion)
                        {
                            if (descripcion.TipoNovedadCodigo == ConstanTiposNovedades.Adición ||
                                descripcion.TipoNovedadCodigo == ConstanTiposNovedades.Modificación_de_Condiciones_Contractuales ||
                                descripcion.TipoNovedadCodigo == ConstanTiposNovedades.Prórroga)
                            {
                                vaComite = true;
                            }
                            else
                            {
                                vaComite = false;
                            }

                            if ((novedad.EstadoCodigo == ConstanCodigoEstadoNovedadContractual.Registrado && vaComite) || (novedad.EstadoCodigo == ConstanCodigoEstadoNovedadContractual.Firmado && !vaComite))
                            {
                                //
                                if (descripcion.TipoNovedadCodigo == ConstanTiposNovedades.Reinicio)
                                {
                                    fechaReinicio = descripcion.FechaInicioSuspension;
                                }

                                if (descripcion.TipoNovedadCodigo == ConstanTiposNovedades.Suspensión || descripcion.TipoNovedadCodigo == ConstanTiposNovedades.Prórroga_a_las_Suspensión)
                                {
                                    if (descripcion.GetDiasFechaSuspension != null)
                                    {
                                        TimeSpan? diasTemp = descripcion.GetDiasFechaSuspension;
                                        if (fechaReinicio != null)
                                        {
                                            if (fechaReinicio > descripcion.FechaInicioSuspension)
                                                diasTemp = (fechaReinicio - descripcion.FechaInicioSuspension);
                                        }
                                        DateTime fechaFinalizacionTmp = (DateTime)fechaFinalizacion;
                                        fechaFinalizacion = fechaFinalizacionTmp.AddDays((double)diasTemp.Value.TotalDays);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return fechaFinalizacion;
        }

        public decimal GetValorTotalDisponibilidad(int pDisponibilidadPresupuestalId, bool esLiberacion)
        {
            decimal valorSolicitud = 0;
            DisponibilidadPresupuestal disponibilidadPresupuestal = _context.DisponibilidadPresupuestal.Find(pDisponibilidadPresupuestalId);

            if (!esLiberacion)
            {
                //no encontré los helpers, sorry
                if (disponibilidadPresupuestal != null)
                {
                    valorSolicitud = disponibilidadPresupuestal.ValorSolicitud;
                    valorSolicitud += _context.NovedadContractualRegistroPresupuestal.Where(r => r.DisponibilidadPresupuestalId == disponibilidadPresupuestal.DisponibilidadPresupuestalId &&
                        (r.EstadoSolicitudCodigo == "5" || r.EstadoSolicitudCodigo == "8")).Sum(r => r.ValorSolicitud);
                }
            }
            else
            {
                DisponibilidadPresupuestalHistorico disponibilidadPresupuestalh = _context.DisponibilidadPresupuestalHistorico.Where(r => r.DisponibilidadPresupuestalId == pDisponibilidadPresupuestalId).FirstOrDefault();
                //no encontré los helpers, sorry
                if (disponibilidadPresupuestalh != null)
                {
                    valorSolicitud = disponibilidadPresupuestalh.ValorSolicitud;
                    List<NovedadContractualRegistroPresupuestal> ncrList = _context.NovedadContractualRegistroPresupuestal.Where(r => r.DisponibilidadPresupuestalId == disponibilidadPresupuestal.DisponibilidadPresupuestalId && (r.EstadoSolicitudCodigo == "5" || r.EstadoSolicitudCodigo == "8")).ToList();

                    foreach (var ncr in ncrList)
                    {
                        NovedadContractualRegistroPresupuestalHistorico ncrh = _context.NovedadContractualRegistroPresupuestalHistorico.Where(r => r.NovedadContractualRegistroPresupuestalId == ncr.NovedadContractualRegistroPresupuestalId).FirstOrDefault();
                        if (ncrh != null)
                        {
                            valorSolicitud += ncrh.ValorSolicitud;
                        }
                        else
                        {
                            valorSolicitud += ncr.ValorSolicitud;
                        }
                    }
                }
                else
                {
                    if (disponibilidadPresupuestal != null)
                    {
                        valorSolicitud = disponibilidadPresupuestal.ValorSolicitud;
                        valorSolicitud += _context.NovedadContractualRegistroPresupuestal.Where(r => r.DisponibilidadPresupuestalId == disponibilidadPresupuestal.DisponibilidadPresupuestalId &&
                            (r.EstadoSolicitudCodigo == "5" || r.EstadoSolicitudCodigo == "8")).Sum(r => r.ValorSolicitud);
                    }
                }
            }
            return valorSolicitud;
        }

        public async Task<dynamic> GetLlaveMenByContratacionProyectoId(int pContratacionProyectoId)
        {
            ContratacionProyecto cp = _context.ContratacionProyecto.Find(pContratacionProyectoId);
            if (cp != null)
            {
                return await _context.Proyecto.Select(m => new
                {
                    m.ProyectoId,
                    m.LlaveMen
                }).Where(m => m.ProyectoId == cp.ProyectoId).ToListAsync();
            }

            return null;
        }

        public decimal GetValorContrato(int pContratacionId, string tipo, bool? fase)
        {
            decimal valor = 0;
            //var a = _context.VDrpXfaseXcontratacionIdXnovedad.Where(v => v.ContratacionId == pContratacionId).ToList().Sum(r => r.ValorDrp) ?? 0;
            List<VDrpXfaseXcontratacionIdXnovedad> lContratacion = _context.VDrpXfaseXcontratacionIdXnovedad.Where(v => v.ContratacionId == pContratacionId).ToList();//.Sum(r => r.ValorDrp) ?? 0;
            if (lContratacion != null && lContratacion.Count > 0)
            {
                switch (tipo)
                {
                    case "original":
                        valor = lContratacion.Where(v => v.EsPreConstruccion == fase && v.EsDrpOriginal > 0).Sum(r => r.ValorDrp) ?? 0;
                        break;
                    case "acumulado":
                        valor = lContratacion.Where(v => v.EsPreConstruccion == fase && v.EsDrpOriginal == 0).Sum(r => r.ValorDrp) ?? 0;
                        break;
                    case "total":
                        valor = lContratacion.Where(v => v.EsDrpOriginal > 0).Sum(r => r.ValorDrp) ?? 0;
                        break;
                    case "totalAcumulado":
                        valor = lContratacion.Where(v => v.EsDrpOriginal == 0).Sum(r => r.ValorDrp) ?? 0;
                        break;
                }
            }
            return valor;
        }

        public string GetMonthDaysDifferences(DateTime startDate, DateTime endDate)
        {
            DateTime baseDate = new DateTime(1,1,1);
            int diffDays = endDate.Subtract(startDate).Days;
            DateTime newDate = baseDate.AddDays(diffDays);
            //string diff = $"{end.Year - baseDate.Year} years, {end.Month - baseDate.Month} months and {end.Day - baseDate.Day} days";
            string diff = $"M: {((newDate.Year - baseDate.Year) * 12) + (newDate.Month - baseDate.Month)} D: {newDate.Day - baseDate.Day}";
            return diff;
        }

        public string GetAccumulateMonthDaysDifferences(DateTime startDate, DateTime endDate, int pContratoId)
        {
            DateTime? cEndDate = GetFechaEstimadaFinalizacion(pContratoId);
            DateTime baseDate = new DateTime(1, 1, 1);
            int diffDays = cEndDate.HasValue ? cEndDate.Value.Subtract(startDate).Days : endDate.Subtract(startDate).Days;
            DateTime newDate = baseDate.AddDays(diffDays);

            string diff = $"M: {((newDate.Year - baseDate.Year)*12) + (newDate.Month - baseDate.Month)} D: {newDate.Day - baseDate.Day}";
            return diff;
        }

        public async Task<object> ExcuteSqlStoredProcedure<T>(string query, SqlParameter[] parameterList, int ListorObject)
        {
            List<T> lobj = null;
            object obj = null;
            if (_context.ChangeTracker.LazyLoadingEnabled != false)
                _context.ChangeTracker.LazyLoadingEnabled = false;
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                if (parameterList != null)
                    command.Parameters.AddRange(parameterList);

                command.CommandTimeout = 0;
                command.CommandText = query;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                if (command.Connection.State != System.Data.ConnectionState.Open)
                    command.Connection.Open();

                System.Data.Common.DbDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    var jsonRes = "";
                    while (await reader.ReadAsync())
                        jsonRes += reader.GetTextReader(0).ReadToEnd();

                    try
                    {
                        if (ListorObject == 1)
                        { lobj = JsonConvert.DeserializeObject<List<T>>(jsonRes); }
                        else
                        { obj = JsonConvert.DeserializeObject<T>(jsonRes); }
                    }
                    catch (Exception ex) { }
                }
                try { reader.Dispose(); } catch (Exception e) { }
                command.Connection.Close();
            }

            return ListorObject == 1 ? lobj ?? new List<T>() : obj ?? (T)Activator.CreateInstance(typeof(T));
        }

        public async Task<DataTable> ExcuteSqlStoredProcedure(string query, SqlParameter[] parameterList)
        {
            DataTable dataTable = null;
            if (_context.ChangeTracker.LazyLoadingEnabled != false)
                _context.ChangeTracker.LazyLoadingEnabled = false;
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                if (parameterList != null)
                    command.Parameters.AddRange(parameterList);

                command.CommandTimeout = 0;
                command.CommandText = query;
                command.CommandType = CommandType.StoredProcedure;

                if (command.Connection.State != ConnectionState.Open)
                    command.Connection.Open();

                System.Data.Common.DbDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    dataTable = new DataTable();
                    dataTable.Load(reader);
                }
                try { reader.Dispose(); } catch (Exception e) { }
                command.Connection.Close();
            }

            return dataTable;
        }
    }

}



