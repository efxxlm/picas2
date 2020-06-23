using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using asivamosffie.services.Helpers;
using asivamosffie.services.Exceptions;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Models;

namespace asivamosffie.services
{
    public class UserService : IUser
    {
        private readonly ICommonService _commonService;
        private readonly devAsiVamosFFIEContext _context;

        public UserService(devAsiVamosFFIEContext context, ICommonService commonService)
        {
            _commonService = commonService;
         
            _context = context;
        }

        public async Task<Respuesta> RecoverPasswordByEmailAsync(Usuario pUsuario, string pDominio, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            string strMensaje = "108";
            bool blEnvioCorreo = false;
            Respuesta respuesta = new Respuesta();

            //Si no llega Email
            if(string.IsNullOrEmpty(pUsuario.Email))
                {
                string strMensajeValidacion = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Usuario, strMensaje);
                return respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = strMensaje, Message = strMensajeValidacion };
               
            }
            Usuario usuarioSolicito =  _context.Usuario.Where(r => !(bool)r.Eliminado && r.Email.ToUpper().Equals(pUsuario.Email.ToUpper())).FirstOrDefault();

            try
            {
                if (usuarioSolicito != null)
                {
                    string newPass = Helpers.Helpers.GeneratePassword(true, true, true, true, false, 8);

                    //usuarioSolicito.Contrasena = Helpers.Helpers.encryptSha1(newPass.ToString());
                    usuarioSolicito.Ip = pUsuario.Ip;
                    await ChangePasswordUser(usuarioSolicito.UsuarioId, usuarioSolicito.Contrasena, Helpers.Helpers.encryptSha1(newPass.ToString()));


                    Template TemplateRecoveryPassword = await _commonService.GetTemplateById((int)enumeratorTemplate.RecuperarClave);
                    string template = TemplateRecoveryPassword.Contenido;

                    string urlDestino = pDominio;
                    //asent/img/logo
                    //template = template.Replace("_Link_", urlDestino);
                    template = template.Replace("_LinkF_", pDominioFront);
                    template = template.Replace("_Email_", usuarioSolicito.Email);
                    template = template.Replace("_Password_", newPass);

                     blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioSolicito.Email, "Recuperar contraseña", template, pSentender, pPassword, pMailServer, pMailPort);
                     
                    if (blEnvioCorreo)
                    { 
                        strMensaje = "101";
                    } 
                    else 
                    { 
                        strMensaje = "107";
                    } 
                }
                else
                {
                    strMensaje = "106";
                }
                string strMensajeValidacion = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Usuario, strMensaje);
                return respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = strMensaje, Message = strMensajeValidacion };


            }
            catch (Exception ex)
            {
                 return respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = "500", Message = ex.ToString() +ex.InnerException }; 
            }
             
       }
         
        public async Task<Usuario> ChangePasswordUser(int pidusuario, string Oldpwd, string Newpwd)
        {
            var user = _context.Usuario.Find(pidusuario);
            if (user != null)
            {
                if (user.Contrasena != Oldpwd)
                    throw new BusinessException("Lo sentimos, la contraseña actual no coincide.");

                user.Contrasena = Helpers.Helpers.encryptSha1(Newpwd);
                user.FechaModificacion = DateTime.Now;
                user.UsuarioModificacion = user.Email;
                user.CambiarContrasena = false;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new BusinessException("Usuario no existe");
            }

            return user;
        }

    }
}
