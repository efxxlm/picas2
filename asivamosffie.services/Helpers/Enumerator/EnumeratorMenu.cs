﻿
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
        Aportantes = 15,
        Contratacion_Proyecto = 17, 
        Procesos_Seleccion = 18, 
        Procesos_Seleccion_Cronograma = 20,
        Procesos_Seleccion_Grupo = 21,
        CargueMasivoOrdenes = 22,
        CronogramaSeguimiento = 23,
        RegistrarComiteTecnico = 24,
        SesionComiteTema = 26,
        GenerarDisponibilidadPresupuestal = 28,
        RegistrarSesionComiteFiduciario = 29,
        DisponibilidadPresupuestal = 30,
        Gestionar_Procesos_Contractuales = 31,
        GestionarGarantias = 35,
        Generar_Registro_Presupuestal = 36,
        Preconstruccion_Fase_1 = 37,   
        Registrar_contratos_modificaciones_contractuales = 46, 
     	Registrar_Programacion_Personal_Obra = 48,
        Gestionar_controversias_contractuales=47, 
        Gestionar_procesos_Defensa_Judicial = 52,
        Registrar_actuaciones_controversias_contractuales=57,
        Registrar_Avance_Semanal = 55,
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
        DescargarExcelOrdenes = 77,
        Notificacion_Gestion_Poliza = 90, 
        Crear_Editar_ProcesoSeleccion_Grupo= 29

    }

}