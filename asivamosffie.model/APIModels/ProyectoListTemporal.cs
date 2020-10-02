

using AuthorizationTest.JwtHelpers;

namespace asivamosffie.model.APIModels
{
    public class ProyectoListTemporal
    { 
        public string Estado_Registro { get; set; }
        public string Fecha_de_Sesion_Junta { get; set; }
        public string Numero_de_acta_de_la_junta { get; set; }
        public string Tipo_de_Intervencion { get; set; }
        public string Llave_MEN { get; set; }
        public string Region { get; set; }
        public string Departamento { get; set; }
        public string Municipio { get; set; }
        public string Institucion_Educativa { get; set; } 
        public string Codigo_DANE_IE { get; set; }
        public string Sede { get; set; }
        public string Codigo_DANE_SEDE { get; set; }
        public string Se_encuentra_dentro_de_una_convocatoria { get; set; }
        public string Convocatoria { get; set; }
        public string Numero_de_predios_postulados { get; set; }
        public string Tipo_de_predio { get; set; }
        public string Ubicacion_del_predio_principal_Latitud { get; set; }
        public string Ubicacion_del_predio_principal_Longitud { get; set; }
        public string Direccion_del_predio_principal { get; set; }
        public string Documento_de_acreditacion_del_predio { get; set; }
        public string Número_del_documento_de_acreditacion { get; set; }
        public string Cedula_Catastral_del_predio { get; set; }
        public string Tipo_de_aportante_1 { get; set; }
        public string Aportante_1 { get; set; }
        public string Tipo_de_aportante_2 { get; set; }
        public string Aportante_2 { get; set; }
        public string Tipo_de_aportante_3 { get; set; }
        public string Aportante_3 { get; set; }
        public string Vigencia_del_acuerdo_de_cofinanciación { get; set; }
        public string Valor_obra { get; set; }
        public string Valor_interventoria { get; set; }
        public string Valor_Total { get; set; }
        public string Espacios_a_intervenir { get; set; }
        public string Cantidad { get; set; }
        public string Plazo_de_obra_Meses { get; set; }
        public string Plazo_de_obra_Dias { get; set; }
        public string Plazo_de_Interventoria_Meses { get; set; }
        public string Plazo_de_Interventoroa_Dias { get; set; }
        public string Coordinacion_Responsable { get; set; } 
    }
}