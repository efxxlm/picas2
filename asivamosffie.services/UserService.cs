using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using asivamosffie.services.Helpers;

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

        public async Task<Usuario> RecoverPasswordByEmailAsync(string pUserMail, string pIpClient, string pDominio, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            Usuario usuarioSolicito =  _context.Usuario.Where(r => !(bool)r.Eliminado && r.Email.ToUpper().Equals(pUserMail.ToUpper())).FirstOrDefault();

            if (usuarioSolicito != null)
            {
                string newPass = Helpers.Helpers.GeneratePassword(true, true, true, true, false, 8);

                usuarioSolicito.Contrasena = Helpers.Helpers.encryptSha1(newPass.ToString());
                usuarioSolicito.Ip = pIpClient;
                 await UpdatePasswordUser(usuarioSolicito);


                Template TemplateRecoveryPassword = await _commonService.GetTemplateByTipo("RecoveryPassword");
                string template = TemplateRecoveryPassword.Contenido;

                string urlDestino = pDominio;

                template = template.Replace("_Link_", urlDestino);
                template = template.Replace("_Email_", usuarioSolicito.Email);
                template = template.Replace("_Password_", newPass);

                bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioSolicito.Email, "Recuperar contraseña", template, pSentender, pPassword, pMailServer, pMailPort);



            }

            return usuarioSolicito;
        }

        public async Task<Usuario> UpdatePasswordUser(Usuario pUsuario)
        {

            Usuario usuarioSolicito =  _context.Usuario.Where(r => r.Email.Equals(pUsuario.Email)).FirstOrDefault();
            usuarioSolicito.Contrasena = pUsuario.Contrasena;
            usuarioSolicito.Ip = pUsuario.Ip;
            usuarioSolicito.UsuarioId = pUsuario.UsuarioId;
            usuarioSolicito.FechaModificacion = DateTime.Now;
            usuarioSolicito.CambiarContrasena = true;
            _context.Usuario.Update(usuarioSolicito);
            _context.SaveChanges();

            return usuarioSolicito;

        }



    }


}
