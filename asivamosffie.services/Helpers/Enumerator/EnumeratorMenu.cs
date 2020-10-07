
namespace asivamosffie.services.Helpers.Enumerator
{
    public enum enumeratorMenu : int
    {
        Usuario = 1,
        Cofinanciacion = 2,
        CambioContraseña = 6,
        CargueMasivoProyecto = 9,
        Proyecto = 10,
        Fuentes = 14,
        Contratacion_Proyecto = 17,
        Aportantes = 15,


        Procesos_Seleccion = 18,
        
        Procesos_Seleccion_Cronograma = 20,
        Procesos_Seleccion_Grupo = 21,
        CargueMasivoOrdenes = 22,
        CronogramaSeguimiento = 23,
        RegistrarComiteTecnico = 24,

        Gestionar_Procesos_Contractuales = 31,

        Preconstruccion_Fase_1 = 37,

        Registrar_Requisitos_Tecnicos_Construccion = 41,
        
    }

    public enum enumeratorAccion
    {
        IniciarSesion = 51,
        CambiarContraseña = 52,
        SolicitarContraseña = 53,
        CrearActualizarCofinanciacion = 55,
        Error = 56,
        ValidarExcel = 63, 
        CargueProyectosMasivos = 69,
        DescargarExcelProyectos = 75,
        CargueOrdenesMasivos = 76,
        DescargarExcelOrdenes = 77

    }

}