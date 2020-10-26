namespace asivamosffie.services.Helpers.Constant
{
    public static class ConstanCodigoEstadoConstruccion
    { 
        public const string	Sin_aprobacion_de_requisitos_tecnicos = "1";
        public const string	En_proceso_de_aprobacion_de_requisitos_tecnicos = "2";
        public const string	Con_requisitos_tecnicos_aprobados = "3";
        public const string	En_proceso_de_verificacion_de_requisitos_tecnicos = "4";
        public const string	Con_requisitos_tecnicos_verificados = "5";
        public const string	Enviado_al_supervisor = "6";
        public const string	En_proceso_de_validacion_de_requisitos_tecnicos = "7";
        public const string	Con_requisitos_tecnicos_validados = "8";
        public const string	Con_requisitos_tecnicos_aprobados_por_supervisor = "9";
        public const string	Enviado_al_interventor = "10";
        public const string	Enviado_al_apoyo = "11";
    }

    public static class ConstanCodigoTipoObservacionConstruccion
    { 
        public const string Diagnostico = "1";
        public const string PlanesProgramas = "2";
        public const string ManejoAnticipo = "3";
        public const string ProgramacionObra = "4";
        public const string FlujoInversion = "5";
        
    }
}