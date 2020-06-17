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
        private readonly IUnitOfWork _unitOfWork;
        private readonly devAsiVamosFFIEContext _context;

        public UserService(devAsiVamosFFIEContext context, ICommonService commonService, IUnitOfWork unitOfWork)
        {
            _commonService = commonService;
            _unitOfWork = unitOfWork;
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
                //await UpdatePasswordUser(usuarioSolicito);


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


        public async Task<Usuario> ChangePasswordUser(Usuario pUsuario)
        {
            var user = await _unitOfWork.UserRepository.GetById(pUsuario.UsuarioId);
            if (user != null)
            {
                if (user.Contrasena != pUsuario.Contrasena)
                    throw new BusinessException("Lo sentimos, la contraseña actual no coincide.");
                
                if(pUsuario.Contrasena != pUsuario.Contrasena) // Pedt: Recibir contrasena nueva desde from
                    throw new BusinessException("Lo sentimos, la nueva contraseña y confirmación no coinciden.");

                user.Contrasena = pUsuario.Contrasena; // Pedt: encriptar  contrasena
                user.Ip = pUsuario.Ip;
                user.UsuarioId = pUsuario.UsuarioId;
                user.FechaModificacion = DateTime.Now;
                user.CambiarContrasena = true;

                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.SaveChangesAsync();
            }



            return user;
        }
    }


}
