
namespace asivamosffie.services.Helpers.Enumerator
{ 
    public enum EnumeratorTipoSolicitudRequisitosPagos : int
    {
        Contratos_de_obra = 1,
        Contratos_de_interventoria,
        Expensas,
        Otros_costos_servicios,
    }

    public static class EnumeratorTipoSolicitudRequisitosPagosString 
    {
        public const string Contratos_de_obra = "1";
        public const string Contratos_de_interventoria = "2";
        public const string Expensas = "3";
        public const string Otros_costos_servicios = "4";
    
    }
}