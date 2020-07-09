﻿
namespace asivamosffie.services.Helpers.Enumerator
{
    public enum enumeratorMenu : int
    {
        Usuario = 1,
        Cofinanciacion = 2,
        CambioContraseña = 6,
        CargueMasivoProyecto = 9,
        Fuentes = 14,
        Aportantes = 15,
        CuentaBancaria = 16

    }

    public enum enumeratorAccion
    {
        IniciarSesion = 51,
        CambiarContraseña = 52,
        SolicitarContraseña = 53,
        CrearActualizarCofinanciacion = 55,
        Error = 56,
        ValidarExcel = 63,
        CargueProyectosMasivos = 69
    }

}