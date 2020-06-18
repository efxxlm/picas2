using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;

namespace asivamosffie.services.Interfaces
{
    public interface IUser
    {
        Task<Object> RecoverPasswordByEmailAsync(Usuario pUsuario, string pDominio, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender);

        Task<Usuario> ChangePasswordUser(int Userid, string Oldpwd, string Newpwd);
    }
}
