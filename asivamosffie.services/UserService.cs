using asivamosffie.model.Models;
using asivamosffie.services.Exceptions;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<Object> RecoverPasswordByEmailAsync(Usuario pUsuario, string pDominio, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender)
        {
            bool blEnvioCorreo = false;
            Respuesta respuesta = new Respuesta();

            //Si no llega Email
            if (string.IsNullOrEmpty(pUsuario.Email))
            {
                respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesUsuarios.EmailObligatorio };
            }
            try
            {
                Usuario usuarioSolicito = _context.Usuario.Where(r => !(bool)r.Eliminado && r.Email.ToUpper().Equals(pUsuario.Email.ToUpper())).FirstOrDefault();

                if (usuarioSolicito != null)
                {
                    string newPass = Helpers.Helpers.GeneratePassword(true, true, true, true, false, 8);
                    usuarioSolicito.Contrasena = Helpers.Helpers.encryptSha1(newPass.ToString()); 
                    usuarioSolicito.CambiarContrasena = true; 
                    usuarioSolicito.Ip = pUsuario.Ip;
                    //Guardar Usuario
                    await UpdateUser(usuarioSolicito); 
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
                        respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesUsuarios.ContrasenaGenerada };
                    else
                        respuesta = new Respuesta() { IsSuccessful = blEnvioCorreo, IsValidation = blEnvioCorreo, Code = ConstantMessagesUsuarios.ErrorEnviarCorreo };

                }
                else
                {
                    respuesta = new Respuesta() { IsSuccessful = true, IsValidation = true, Code = ConstantMessagesUsuarios.CorreoNoExiste };

                }
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Usuario, respuesta.Code);
                return respuesta;


            }
            catch (Exception ex)
            {
                respuesta = new Respuesta() { IsSuccessful = false, IsValidation = false, Code = ConstantMessagesUsuarios.ErrorGuardarCambios };
                respuesta.Message = await _commonService.GetMensajesValidacionesByModuloAndCodigo((int)enumeratorMenu.Usuario, respuesta.Code) + ": " + ex.ToString() + ex.InnerException;
                return respuesta;
            }

        }


        public async Task<Usuario> UpdateUser(Usuario pUser)
        {
            pUser.FechaModificacion = DateTime.Now;
            pUser.UsuarioModificacion = pUser.Email;
            await _context.SaveChangesAsync(); 
            return pUser;
        }

        public async Task<Usuario> ChangePasswordUser(int pidusuario, string Oldpwd, string Newpwd)
        {
            var user = _context.Usuario.Find(pidusuario);
            //var UppUser = Helpers.Helpers.ConvertToUpercase(user);
            var OldpwdEncrypt = Helpers.Helpers.encryptSha1(Newpwd.ToUpper());


            if (user != null)
            {
                if (user.Contrasena.ToUpper() != OldpwdEncrypt.ToString())
                    throw new BusinessException("Lo sentimos, la contraseña actual no coincide.");

                user.Contrasena = Helpers.Helpers.encryptSha1(Newpwd.ToUpper());
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
