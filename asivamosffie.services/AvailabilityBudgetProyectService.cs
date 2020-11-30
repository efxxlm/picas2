using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services
{
    public class AvailabilityBudgetProyectService 
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly string _connectionString;
        private List<DetailValidarDisponibilidadPresupuesal> _ListPDF;

        public AvailabilityBudgetProyectService(devAsiVamosFFIEContext context, ICommonService commonService, IConfiguration configuration)
        {
            _context = context;
            _commonService = commonService;
            _connectionString = configuration.GetConnectionString("asivamosffieDatabase");

        }

        //Solicitudes de comite tecnico
        public async Task<List<CustonReuestCommittee>> GetReuestCommittee()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetReuestCommittee", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<CustonReuestCommittee>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToValue(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public CustonReuestCommittee MapToValue(SqlDataReader reader)
        {
            return new CustonReuestCommittee()
            {
                ContratacionId = (int)reader["ContratacionId"],
                DisponibilidadPresupuestalId = (int)reader["DisponibilidadPresupuestalId"],
                SesionComiteSolicitudId = (int)reader["SesionComiteSolicitudId"],
                FechaSolicitud = (DateTime)reader["FechaSolicitud"],
                TipoSolicitudText = reader["TipoSolicitudText"].ToString(),
                NumeroSolicitud = reader["NumeroSolicitud"].ToString(),
                OpcionContratar = reader["OpcionContratar"].ToString(),
                ValorSolicitud = (decimal)reader["ValorSolicitud"],
                FechaComite = (DateTime)reader["FechaComite"],

            };
        }
         
        //plantlla - rubro por financiar es infraestructura y el tipo de solicitud es contratación
        public async Task<HTMLContent> GetHTMLString(DetailValidarDisponibilidadPresupuesal obj)
        {
            try
            {
                //var  obj = await detailValidarDisponibilidadPresupuesal
                //DisponibilidadPresupuestal ListDP =  _context.DisponibilidadPresupuestal.Where(r => !r.Eliminado).FirstOrDefault();
                var sb = new StringBuilder();
                sb.Append(@"
                       <?xml version='1.0' encoding='utf-8'?>
                        <!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.1//EN' 'http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd'>
                        <html xmlns='http://www.w3.org/1999/xhtml'>

                        <head>
                            <meta http-equiv='Content-Type' content='application/xhtml+xml; charset=utf-8' />
                            <title>DOC</title>
                            <link href='assets/pdf-styles.css' type='text/css' rel='stylesheet' />
                        </head>
                            <body>
                                  <div class='Section0'>
                        <p class='Footer' style='text-align:center;'>
                            <img src='./assets/img-FFIE.png' width='89' height='77' align='right' alt='' />
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>F</span>
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>
                                    ONDO DE FINANCIAMIENTO DE INFRAESTRUCTURA EDUCATIVA - FFIE</span>
                        </p>
                        <p class='Footer' style='text-align:center;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>
                                NIT. 900900129-8</span>
                        </p>
                        <p class='Footer' style='text-align:center;'>
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>
                                MINISTERIO DE EDUCACIÓN
                        </span>
                        </p>
                        <p class='Header' style='text-align:right;'>
                            <span style='font-family:Times New Roman;font-size:12pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'> &#xa0;
                    </span>
                </p>
                <div>
            <table cellspacing='0' style='margin-left:19.6pt;width: auto; border-collapse: collapse; '>
                <tr style='height: 31.4666672px'>
                    <td style='vertical-align:middle;background-color:#F2F2F2;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:926.0667px;' colspan='10'>
                        <h2><span style='font-family:Arial;font-size:11pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>DOCUMENTO DE DISPONIBILIDAD PRESUPUESTAL</span></h2>
                    </td>
                </tr>
                <tr style='height: 9.2px'>
                    <td style='vertical-align:middle;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:926.0667px;' colspan='10'>
                        <h2><span style='font-family:Arial;font-size:11pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>&#xa0;</span></h2>
                    </td>
                </tr>

                <tr style='height: 29.6px'>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:321.2667px;'
                        colspan='3'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Fecha: </span><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'> </span>");
                            
                            sb.AppendLine("<span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + DateTime.Now.ToString("MM/dd/yyyy") + "");
                            sb.Append(@"</span>
                        </h2>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:311.8667px;'
                        colspan='4'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Número de solicitud: </span>");
                            sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + obj.NumeroSolicitud + "");
                            sb.Append(@"</span>
                           
                           
                        </h2>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:292.9333px;'
                        colspan='3'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>DDP No: </span>");
                            sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + obj.NumeroDDP + "");
                            sb.Append(@"</span>

                        </h2>
                    </td>
                </tr>
                <tr style='height: 4.66666651px'>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:321.2667px;'
                        colspan='3'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>&#xa0;</span></h2>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:311.8667px;'
                        colspan='4'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>&#xa0;</span></h2>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:292.9333px;'
                        colspan='3'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>&#xa0;</span></h2>
                    </td>
                </tr>
                <tr style='height: 20.8000011px'>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:321.2667px;'
                        colspan='3'>
                        <p style='text-align:left;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Rubro por financiar: </span>");
                            sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + "?" + "");
                            sb.Append(@"</span>

                        </p>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:311.8667px;'
                        colspan='4'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Tipo de Solicitud</span>");
                            sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + obj.TipoSolicitudText + "");
                            sb.Append(@"</span>
                        </h2>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:292.9333px;'
                        colspan='3'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Opción por contratar: </span>");
                            sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + obj.OpcionPorContratar + "");
                            sb.Append(@"</span>
                        </h2>
                    </td>
                </tr>
                <tr style='height: 18.5333328px'>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:321.2667px;'
                        colspan='3'>
                        <p style='text-align:left;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Fecha comité técnico: </span>");
                            sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + Convert.ToDateTime(obj.FechaComiteTecnico).ToString("MM/dd/yyyy") + "");
                            sb.Append(@"</span>
                        </p>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:311.8667px;'
                        colspan='4'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Número comité:</span>");
                            sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + "?" + "");
                sb.Append(@"</span>
                        </h2>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:292.9333px;'
                        colspan='3'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>&#xa0;</span></h2>
                    </td>
                </tr>
                <tr style='height: 30.5333328px'>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:926.0667px;'
                        colspan='10'>
                        <h2 style='text-align:left;'><span style='font-family:Arial;font-size:9pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Objeto:</span>");
                            sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + obj.Objeto  + "");
                            sb.Append(@"</span>
                        </h2>
                    </td>
                </tr>
                <tr style='height: 26.2666683px'>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:926.0667px;' colspan='10'>
                        <p class='Body-Text' style='text-align:center;line-height:115.833328%;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>Se expide el presente documento de disponibilidad presupuestal para la construcción de infraestr</span>
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>uctura educativa, conforme a lo señalado en el artículo 59 de la ley 1753 de 2015 Plan Nacional de Desarrollo </span>
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:italic;font-variant:normal;line-height:115.833328%;'>&quot;</span>
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:italic;font-variant:normal;line-height:115.833328%;'>T</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:italic;font-variant:normal;line-height:115.833328%;'>odos por un nuevo </span>
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:italic;font-variant:normal;line-height:115.833328%;'>país</span>
                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:italic;font-variant:normal;line-height:115.833328%;'>&quot;</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'> y lo dispuesto en el conpes 3831 del Plan Nacional de Infraestructura Educativa.</span></p>
                        <h2><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>&#xa0;</span></h2>
                    </td>
                </tr>
                <tr style='height: 56.86667px'>
                    <td style='vertical-align:middle;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-top:0pt;padding-bottom:0pt;width:926.0667px;' colspan='10'>
                        <div>
                            <table cellspacing='0' style='width: 100%; border-collapse: collapse; '>
                                <tr style='height: 2px'>
                                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:209.9333px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>N</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>ombre </span>
                                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>del a</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>portante</span></p>
                                    </td>
                                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:302px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>Fuente</span></p>
                                    </td>
                                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:398.7333px;'
                                        colspan='2'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>Valor sol</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>icitado de la fuente</span></p>
                                    </td>
                                </tr>
                                <tr style='height: 2px'>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:209.9333px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'>Funcionalidad </span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'>Solicitar disponibilidad presupuestal” - Aportante</span></p>
                                    </td>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:302px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'>El sistema deberá </span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'>calcular el valor total de la fuente, realizando la sumatoria de los valores solicitados para todos los proyectos asociados al contrato</span>
                                            <span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'>,</span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'> por fuente y por aportante, enunciados en el cuadro inferior.</span></p>
                                    </td>
                                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:198.4667px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'>Funcionalidad </span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;line-height:115.833328%;'>Solicitar disponibilidad presupuestal” – Valor del aportante</span></p>
                                    </td>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:200.2667px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='color:#BFBFBF;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>Cantidad en letra</span><span style='color:#BFBFBF;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>s (pesos moneda corriente)</span></p>
                                    </td>
                                </tr>
                                <tr style='height: 19.5333328px'>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:209.9333px;'>
                                        <p class='Body-Text' style='text-align:center;line-height:115.833328%;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:302px;'>
                                        <p class='Body-Text' style='text-align:center;line-height:115.833328%;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:198.4667px;'>
                                        <p class='Body-Text' style='text-align:center;line-height:115.833328%;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:200.2667px;'>
                                        <p class='Body-Text' style='text-align:center;line-height:115.833328%;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                </tr>
                                <tr style='height: 2px'>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:209.9333px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:302px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:198.4667px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:200.2667px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                                    </td>
                                </tr>
                                <tr style='height: 2px'>
                                    <td style='vertical-align:middle;background-color:#D9D9D9;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:511.9333px;'
                                        colspan='2'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>Total</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'> de recu</span>
                                            <span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>r</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;line-height:115.833328%;'>sos con disponibilidad presupuestal</span></p>
                                    </td>
                                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:198.4667px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='color:#BFBFBF;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>Cantidad en formato</span><span style='color:#BFBFBF;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'> ($)</span></p>
                                    </td>
                                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:200.2667px;'>
                                        <p class='Body-Text' style='text-align:center;margin-top:5.1pt;margin-right:9.2pt;'><span style='color:#BFBFBF;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>Cantidad en letras (pesos moneda corriente)</span></p>
                                    </td>
                                </tr>
                                <tr style='height:0px;'>
                                    <td style='width:209.9333px;border:none;padding:0pt;' />
                                    <td style='width:302px;border:none;padding:0pt;' />
                                    <td style='width:198.4666px;border:none;padding:0pt;' />
                                    <td style='width:200.2667px;border:none;padding:0pt;' />
                                </tr>
                            </table>
                        </div>
                        <p class='Body-Text' style='text-align:center;line-height:115.833328%;margin-top:5.1pt;margin-right:9.2pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;line-height:115.833328%;'>&#xa0;</span></p>
                    </td>
                </tr>
                <tr style='height: 34.4666672px'>
                    <td style='vertical-align:middle;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:926.0667px;' colspan='10'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>Se certifica que a la fecha de expedición del presente documento existen </span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>los recursos para darle cumplimiento al objeto. Este recurso se compromete conforme a la siguiente información:</span></p>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>En </span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>el siguiente cuadro se debe mostrar la información que se encuentra en la funcionalidad Validar disponibilidad de presupuesto para ejecución de proyecto”</span></p>
                    </td>
                </tr>
                <tr style='height: 26.0666656px'>
                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:106.3333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'> Nombre del aportante</span>");
                            sb.AppendLine("<span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-variant:normal;'> " + "");
                            sb.Append(@"</span>
                        </p>
                    </td>
                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Fue</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>nte</span></p>
                    </td>
                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:122.8667px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Saldo actual de la fuente</span></p>
                    </td>
                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:113.4px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Valor </span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>solicitado de la fuente</span></p>
                    </td>
                    <td style='vertical-align:middle;background-color:#E7E6E6;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:132.2667px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Nuevo saldo de la fuente</span></p>
                    </td>
                </tr>
                <tr style='height: 19.5333328px'>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:127.5333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:115.8px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:106.3333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:122.8667px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:113.4px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:132.2667px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                </tr>
                <tr style='height: 19.5333328px'>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:127.5333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:115.8px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:106.3333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:122.8667px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:113.4px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:132.2667px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                </tr>
                <tr style='height: 6.33333349px'>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:127.5333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:115.8px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:106.3333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:122.8667px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:113.4px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:132.2667px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                </tr>
                <tr style='height: 26.0666656px'>
                    <td style='vertical-align:middle;background-color:#D9D9D9;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:bold;font-style:normal;font-variant:normal;'>Limitación especial</span></p>
                    </td>
                    <td style='vertical-align:middle;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:822.1333px;'
                        colspan='9'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>Funcionalidad </span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>Solicitar </span>
                            <span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>disponibilidad presupuestal</span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>” </span>
                            <span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>–</span>
                            <span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'> </span><span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>Para los DDP Especiales</span>
                            <span style='color:#FF0000;font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:italic;font-variant:normal;'>”</span>
                        </p>
                    </td>
                </tr>
                <tr style='height: 5.4666667px'>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:127.5333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:115.8px;' colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:106.3333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:122.8667px;'
                        colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:113.4px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left:none;border-right:none;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:132.2667px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                </tr>
                <tr style='height: 44.7333336px'>
                    <td style='vertical-align:top;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:127.5333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:115.8px;' colspan='2'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:top;border-top:none;border-left:none;border-right:none;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:106.3333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                    <td style='vertical-align:middle;border-top:none;border-left:none;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom:none;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:103.9333px;'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>Firma Director</span><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'> (a) Financiera</span></p>
                    </td>
                    <td style='vertical-align:top;border-top-style:solid;border-top-color:#000000;border-top-width:1pt;border-left-style:solid;border-left-color:#000000;border-left-width:1pt;border-right-style:solid;border-right-color:#000000;border-right-width:1pt;border-bottom-style:solid;border-bottom-color:#000000;border-bottom-width:1pt;padding-left:5.4pt;padding-right:5.4pt;padding-top:0pt;padding-bottom:0pt;width:368.5333px;'
                        colspan='4'>
                        <p style='text-align:center;page-break-inside:auto;page-break-after:auto;page-break-before:avoid;line-height:normal;margin-top:0pt;margin-bottom:0pt;'><span style='font-family:Arial;font-size:8pt;text-transform:none;font-weight:normal;font-style:normal;font-variant:normal;'>&#xa0;</span></p>
                    </td>
                </tr>

            </div>");

              

                sb.Append(@"
                              
                            </body>
                        </html>");

                obj.htmlContent = sb;
                return new HTMLContent(sb.ToString());
            }
            catch (Exception e)
            {
                return new HTMLContent("");
            }

        }

        //Grilla disponibilidad presupuestal.
        public async Task<ActionResult<List<GrillaValidarDisponibilidadPresupuesal>>> GetBudgetavailabilityRequests()
        {
            List<DisponibilidadPresupuestal> ListDP = await _context.DisponibilidadPresupuestal.Where(r => !r.Eliminado).ToListAsync();
            List<GrillaValidarDisponibilidadPresupuesal> ListGrillaDisponibilidadPresupuestal = new List<GrillaValidarDisponibilidadPresupuesal>();

            foreach (var validacionPresupuestal in ListDP)
            {
                GrillaValidarDisponibilidadPresupuesal disponibilidadPresupuestal = new GrillaValidarDisponibilidadPresupuesal
                {
                    Id = validacionPresupuestal.DisponibilidadPresupuestalId,
                    FechaSolicitud = validacionPresupuestal.FechaSolicitud,
                    NumeroSolicitud = validacionPresupuestal.NumeroSolicitud,
                    TipoSolicitudCodigo = validacionPresupuestal.TipoSolicitudCodigo != null ? await _commonService.GetNombreDominioByCodigoAndTipoDominio(validacionPresupuestal.TipoSolicitudCodigo, (int)EnumeratorTipoDominio.Tipo_de_Solicitud) : "",
                    EstadoRegistro = (bool)validacionPresupuestal.RegistroCompleto,
                    EstadoRegistroText = (bool)validacionPresupuestal.RegistroCompleto ? "Completo" : "Incompleto"
                };

                ListGrillaDisponibilidadPresupuestal.Add(disponibilidadPresupuestal);
            }

            return ListGrillaDisponibilidadPresupuestal;
        }

    }
}

