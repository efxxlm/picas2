using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using asivamosffie.services.Helpers;
using asivamosffie.services.Exceptions;

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

        public async Task<Object> RecoverPasswordByEmailAsync(Usuario pUsuario, string pDominio, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            Object mensaje = null;
            Usuario usuarioSolicito =  _context.Usuario.Where(r => !(bool)r.Eliminado && r.Email.ToUpper().Equals(pUsuario.Email.ToUpper())).FirstOrDefault();

            if (usuarioSolicito != null)
            {
                string newPass = Helpers.Helpers.GeneratePassword(true, true, true, true, false, 8);

                usuarioSolicito.Contrasena = Helpers.Helpers.encryptSha1(newPass.ToString());
                usuarioSolicito.Ip = pUsuario.Ip;
                //await ChangePasswordUser(usuarioSolicito);


                Template TemplateRecoveryPassword = await _commonService.GetTemplateByTipo("RecoveryPassword");
                string template = TemplateRecoveryPassword.Contenido;

                string urlDestino = pDominio;

                template = template.Replace("_Link_", urlDestino);
                template = template.Replace("_Email_", usuarioSolicito.Email);
                template = template.Replace("_Password_", newPass);

                bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioSolicito.Email, "Recuperar contraseña", template, pSentender, pPassword, pMailServer, pMailPort);
                 
                mensaje = new { codigo = "200OK", validation = true, validationmessage = (blEnvioCorreo) ? "Cambio de contraseña exitoso" : "Error Envio de correo" };

            }
            else 
            {
                mensaje = new { codigo = "200OK", validation = true, validationmessage = "Email no encontrado" };
            }


            return mensaje;
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
