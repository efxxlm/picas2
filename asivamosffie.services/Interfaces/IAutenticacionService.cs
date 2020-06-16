using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;

namespace asivamosffie.services.Interfaces
{
    public interface IAutenticacionService
    {
        Task<object> IniciarSesion(Usuario pUsuario);
        Task<Usuario> GetUserByMail(string pMail);

        Task ResetFailedAttempts(int pUserId);

        Task AddFailedAttempt(int pUserId);

        Task BlockUser(int pUserId);
    }
}
