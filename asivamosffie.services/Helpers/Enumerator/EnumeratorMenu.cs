
namespace asivamosffie.services.Helpers.Enumerator
{
    public enum enumeratorMenu : int
    {
        Usuario = 1,
        Cofinanciacion = 2,
        CambioContraseña = 6,
        CargueMasivoProyecto = 9,
        Proyecto = 10
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

    }

    //Julian Martinez
    //Creo ese enum para listar por codigo y ni por id dominio como esta arriba
    public enum enumeratorAccionCodigo
    { 
        Inicio_de_sesion = 1,
        Cambio_de_contrasena = 2,
        Solicitud_de_contrasena = 3,
        CrearActualizarCofinanciacion = 4,
        Error = 5,
        Validar_Archivo_Excel = 6,
        Cargue_Proyectos_Masivos = 7,
        Descargar_Excel_Proyectos = 8,
        Listar_Proyectos = 9
    }



}