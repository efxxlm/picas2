﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using asivamosffie.model.Models;
using asivamosffie.services.Helpers.Enumerator;
using System.Text.RegularExpressions;
using asivamosffie.model.APIModels;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.pipeline;
using DinkToPdf;
using DinkToPdf.Contracts; 
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.pipeline.html;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
 
namespace asivamosffie.services.Helpers
{
    public class PDF
    {
        private readonly devAsiVamosFFIEContext _context;
        public readonly IConverter _converter;

        public PDF(devAsiVamosFFIEContext context, IConverter converter)
        {
            _context = context;
            _converter = converter; 
        }



        public static byte[] Convertir(Plantilla pPlantilla)
        {
            string contenido = pPlantilla.Contenido ?? " "; 
            string encabezado = pPlantilla.Encabezado != null ? pPlantilla.Encabezado.Contenido : " "; 
            string pie = pPlantilla.PieDePagina != null ? pPlantilla.PieDePagina.Contenido :" ";
        
            Margenes margenes = new Margenes
            {
                Arriba = (float)pPlantilla.MargenArriba,
                Abajo = (float)pPlantilla.MargenAbajo,
                Derecha = (float)pPlantilla.MargenDerecha,
                Izquierda = (float)pPlantilla.MargenIzquierda
            };

            try
            {
                contenido = contenido.Replace("\r\n", "");
                contenido = contenido.Replace("\r\n", "");
                contenido = contenido.Replace("\r\n", "");

                FontFactory.RegisterDirectories();

                double margenIzquierdo = CentimetrosAMedidaPDF(margenes.Izquierda);
                double margenDerecho = CentimetrosAMedidaPDF(margenes.Derecha);
                double margenSuperior = CentimetrosAMedidaPDF(margenes.Arriba);
                double margenInferior = CentimetrosAMedidaPDF(margenes.Abajo);

                Document document = new Document();
                iTextSharp.text.Rectangle z = PageSize.LETTER;
                document = new Document(z, (float)margenIzquierdo, (float)margenDerecho, (float)margenSuperior, (float)margenInferior);


                EventosPdf e = new EventosPdf();
                e.Iniciar(encabezado, pie, (float)margenes.Arriba, (float)margenes.Abajo);
                MemoryStream stream = new MemoryStream();

                PdfWriter writer = PdfWriter.GetInstance(document, stream);
                writer.PageEvent = e;

                document.Open();

                HtmlPipelineContext htmlContext = new HtmlPipelineContext(null);

                htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());

                ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);

                IPipeline pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(htmlContext, new PdfWriterPipeline(document, writer)));

                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, new StringReader(contenido));

                document.Close();
                return stream.ToArray();
            }
            catch (Exception ex)
            {

            }

            return Array.Empty<byte>();
        }

        public static double CentimetrosAMedidaPDF(double centimetros)
        {
            return (double)(centimetros * 0.393701 * 72);
        }

        class EventosPdf : PdfPageEventHelper
        {

            public string Encabezado;
            public string Pie;
            public float MargenSuperior;
            public float MargenInferior;
            protected ElementList header;
            protected ElementList footer;

            public void Iniciar(string encabezado, string pie, float margenSuperior, float margenInferior)
            {
                this.Encabezado = encabezado;
                this.MargenSuperior = margenSuperior;
                this.MargenInferior = margenInferior;
                this.Pie = pie;
                if (!string.IsNullOrEmpty(encabezado))
                {
                    encabezado = encabezado.Replace("[RUTA_ICONO]", Path.Combine(Directory.GetCurrentDirectory(), "assets", "img-FFIE.png"));
                    header = XMLWorkerHelper.ParseToElementList(encabezado, null);
                }
                if (!string.IsNullOrEmpty(pie))
                {
                    footer = XMLWorkerHelper.ParseToElementList(pie, null);
                }
            }

            public override void OnStartPage(PdfWriter writer, Document document)
            {

                if (Encabezado != null)
                {

                }
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {

                try
                {
                    ColumnText ct = new ColumnText(writer.DirectContent);
                    if (header != null)
                    {

                        ct.SetSimpleColumn(new Rectangle(36, 0, 559, document.Top + document.TopMargin - 10));

                        foreach (IElement e in header)
                        {
                            ct.AddElement(e);
                        }
                        ct.Go();
                    }
                    if (footer != null)
                    {
                        ct.SetSimpleColumn(new Rectangle(36, 0, 559, 80));

                        foreach (IElement e in footer)
                        {
                            ct.AddElement(e);
                        }
                        ct.Go();
                    }
                }
                catch (Exception de)
                {
                    throw de;
                }
            }

        }

        public class ColumnTextElementHandler : IElementHandler
        {
            public ColumnTextElementHandler(ColumnText ct)
            {
                this.ct = ct;
            }

            ColumnText ct = null;

            public void Add(IWritable w)
            {
                if (w is WritableElement)
                {
                    foreach (IElement e in ((WritableElement)w).Elements())
                    {
                        ct.AddElement(e);
                    }
                }
            }
        }

    }
}
