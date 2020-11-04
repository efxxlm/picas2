namespace asivamosffie.services.Helpers.Constant
{
    public static class ConstanCodigoEstadoVerificacionContratoObra
    {
        //Estados Verificacion contrato tipo obra
        public const string Sin_verificacion_de_requisitos = "1";
        public const string Requisitos_del_contratista_de_obra_en_proceso_de_verificacion = "2";
        public const string Con_requisitos_del_contratista_de_obra_verificados = "3";
        public const string Con_requisitos_del_contratista_de_obra_avalados = "4";
        public const string Con_notificacion_al_supervisor = "5";
    }
    public static class ConstanCodigoEstadoVerificacionContratoInterventoria
    {
        //Estados Verificacion contrato tipo interventoria
        public const string Sin_revision_de_requisitos_tecnicos = "6";
        public const string En_proceso_de_revision_de_requisitos_tecnicos = "7";
        public const string Con_requisitos_tecnicos_revisados = "8";
        public const string Con_notificacion_al_interventor = "9";
        public const string Con_requisitos_tecnico_aprobados = "10";
    }
}