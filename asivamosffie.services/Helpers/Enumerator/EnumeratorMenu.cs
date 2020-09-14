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
        Contratacion_Proyecto = 17,
        Aportantes = 15,


        Procesos_Seleccion = 18,
        
        Procesos_Seleccion_Cronograma = 20,
        Procesos_Seleccion_Grupo = 21,
        CargueMasivoOrdenes = 22,
        CronogramaSeguimiento = 23,

        GenerarDisponibilidadPresupuestal = 28,

        RegistrarComiteTecnico = 24
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