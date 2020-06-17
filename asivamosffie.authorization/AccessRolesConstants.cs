using System;
using System.ComponentModel.DataAnnotations;

namespace lalupa.Authorization
{
    public static class AccessRoles
    {
        public const string Administrador       = "Administrador";
        public const string DirectoraEjecutiva  = "DirectoraEjecutiva";
        public const string CoordinaadorTecnico = "CoordinadorTecnico";
        public const string CoordAminFinan      = "CoordAdminFinan";
    }
}
