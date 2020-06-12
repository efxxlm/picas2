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
            Usuario usuarioSolicito = await _context.Usuario.Where(r => r.Email.ToUpper().Equals(pUserMail.ToUpper())).FirstOrDefaultAsync();

            if (usuarioSolicito != null)
            {
                string newPass = Helpers.Helpers.GeneratePassword(true, true, true, true, false, 8);

                usuarioSolicito.Contrasena = Helpers.Helpers.encryptSha1(newPass.ToString());
                usuarioSolicito.Ip = pIpClient;

                string template = _commonService.GetTemplateByTipo("RecoveryPassword").ToString();

                string urlDestino = pDominio;

                template = template.Replace("_Link_", urlDestino);
                template = template.Replace("_Email_", usuarioSolicito.Email);
                template = template.Replace("_Password_", newPass);

                bool blEnvioCorreo = Helpers.Helpers.EnviarCorreo(usuarioSolicito.Email ,"Recuperar contraseña", template , pSentender, pPassword , pMailServer, pMailPort);

                if (blEnvioCorreo) {
                  await  UpdatePasswordUser(usuarioSolicito);
                }

            }

            return usuarioSolicito;
        }

        public async Task<Usuario> UpdatePasswordUser(Usuario pUsuario)
        {
            Usuario usuarioSolicito = await _context.Usuario.Where(r => r.Email.Equals(pUsuario.Email)).FirstOrDefaultAsync();

            usuarioSolicito.Contrasena = pUsuario.Contrasena;
            usuarioSolicito.Ip = pUsuario.Ip;
            usuarioSolicito.FechaCreacion = DateTime.Now;

            _context.Usuario.Update(usuarioSolicito);

            return usuarioSolicito;
        }



    }


}
