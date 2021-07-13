namespace asivamosffie.services.Helpers.Constant
{
    public static class ConstanCodigoEstadoActaContrato
    {
        public const string Sin_acta_generada = "1";
        public const string Con_acta_preliminar_generada = "2";
        public const string Con_acta_generada = "3";
        public const string Con_acta_en_proceso_de_firma = "4";
        public const string Con_acta_suscrita_y_cargada = "5";
        public const string Enviado_Por_Supervisor = "6";
        public const string Con_validacion_del_supervisor = "7";
        //NO map NO esta En el Mismo TipoDominioId
        public const string Sin_Revision = "13";
    }

    public static class ConstanMessages // TipoDominio 85
    {
        public const string SinDefinir = "sin definir";
        public const string Obra = "obra";
    }

    public static class ConstanCodigoEstadoActaInicioObra // TipoDominio 85
    {
        public const string Con_acta_suscrita_y_cargada = "20";

    }
    public static class ConstanCodigoEstadoActaInicioInterventoria // TipoDominio 86
    {
        public const string Con_acta_suscrita_y_cargada = "7";

    }




}