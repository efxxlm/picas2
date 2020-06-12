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

        public UserService(devAsiVamosFFIEContext context , ICommonService commonService)
        {
            _commonService = commonService;
            _context = context;
        }

        public async Task<Usuario> RecoverPasswordByEmailAsync(string userMail, string baseurlOrigin, string urlChange, string token, string urlActivate, string ipClient)
        {
            Usuario usuarioSolicito = _context.Usuario.Where(r => r.Email.Equals(userMail)).Single();

            if (usuarioSolicito != null) {
                string newPass = Helpers.Helpers.GeneratePassword(true, true, true, true, false, 8);
                usuarioSolicito.Contrasena = Helpers.Helpers.encryptSha1(newPass.ToString());


                //armo body

                string body;

                var template = _commonService.GetTemplateByTipo("RecoveryPassword");


                // string urlDestino = baseurlOrigin + "/recover-password-mail?Id=" + Base64Encode(usuarioSolicito.Idusuario.ToString()) + "&Pw=" + Base64Encode(newPass);
                //body = template.FirstOrDefault().Template.Replace(template.FirstOrDefault().ReplaceTemplate, urlDestino);
                //body = template.Result("_email_", userMail);
                //body = body.Replace(template.FirstOrDefault().ReplaceTemplate2, baseurlOrigin);
                //body = body.Replace(template.FirstOrDefault().ReplaceTemplate3, newPass);

                //string to = usuarioSolicito.Email;
                const string subject = "Recuperar Contraseña";

                //try
                //{
                //    SendEmailDetails emailDetail = new SendEmailDetails()
                //    {
                //        Subject = subject,
                //        ToEmail = usuarioSolicito.Email,
                //        FromEmail = _config.GetSection("AppSettings:Sender").Value,
                //        FromName = "Soporte KPT",
                //        Content = body,
                //        IsHTML = true
                //    };
                //    await _emailService.SendEmailAsync(emailDetail);
                //}
                //catch (Exception ex)
                //{
                //    usuarioSolicito.NombreMaquina = ex.ToString();
                //}
            }    

            return usuarioSolicito;
        }

        public Task<Usuario> RecoverPasswordByEmailAsync(string userMail, string ipClient)
        {
            throw new NotImplementedException();
        }
    }


}
 