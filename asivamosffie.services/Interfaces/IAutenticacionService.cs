using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Models;

namespace asivamosffie.services.Interfaces
{
    public interface IAutenticacionService
    {
        Task<Respuesta> IniciarSesion(Usuario pUsuario,string prmSecret, string prmIssuer, string prmAudience);
        Task<Usuario> GetUserByMail(string pMail);

        Task ResetFailedAttempts(int pUserId);

        Task AddFailedAttempt(int pUserId);

        Task BlockUser(int pUserId);
    }
}
