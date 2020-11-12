using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface IUser
    {
        Task<Respuesta> RecoverPasswordByEmailAsync(Usuario pUsuario ,string pDominio, string pDominioFront, string pMailServer ,int pMailPort , bool pEnableSSL, string pPassword, string pSentender );
        
        Task<Respuesta> ChangePasswordUser(int Userid, string Oldpwd, string Newpwd);
        Task<Respuesta> ValidateCurrentPassword(int Userid, string oldpwd);
        Task<Respuesta> CloseSesion(int v);
    }
}
