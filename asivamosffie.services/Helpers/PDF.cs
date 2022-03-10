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
using SelectPdf;

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


        public static byte[] Convertir3(Plantilla pPlantilla, bool? pEsHorizontal = false)
        {

            string contenido = pPlantilla.Contenido ?? " ";

                Margenes margenes = new Margenes
            {
                Arriba = (float)pPlantilla.MargenArriba,
                Abajo = (float)pPlantilla.MargenAbajo,
                Derecha = (float)pPlantilla.MargenDerecha,
                Izquierda = (float)pPlantilla.MargenIzquierda
            };

            contenido = contenido.Replace("\r\n", "");
            contenido = contenido.Replace("\r\n", "");
            contenido = contenido.Replace("\r\n", "");
            contenido = contenido.Replace("<br>", "");
            contenido = contenido.Replace("<br/>", "");
            contenido = contenido.Replace("<br />", "");
            contenido = contenido.Replace("</br>", "");

            HtmlToPdf htmlToPdf = new HtmlToPdf();
            htmlToPdf.Options.PdfPageSize = PdfPageSize.A4;
            htmlToPdf.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            htmlToPdf.Options.MarginLeft = 10;
            htmlToPdf.Options.MarginRight = 10;
            htmlToPdf.Options.MarginTop = 20;
            htmlToPdf.Options.MarginBottom = 20;



            SelectPdf.PdfDocument pdfDocument = htmlToPdf.ConvertHtmlString(contenido);
            byte[] pdf = pdfDocument.Save();
            pdfDocument.Close();
                 
            return pdf;
        }
        public static byte[] Convertir(Plantilla pPlantilla, bool? pEsHorizontal = false)
        {
            string contenido = pPlantilla.Contenido ?? " ";
            string encabezado = pPlantilla.Encabezado != null ? pPlantilla.Encabezado.Contenido : " ";
            string pie = pPlantilla.PieDePagina != null ? pPlantilla.PieDePagina.Contenido : " ";

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
                contenido = contenido.Replace("<br>", "");
                contenido = contenido.Replace("<br/>", "");
                contenido = contenido.Replace("<br />", "");
                contenido = contenido.Replace("</br>", "");
                contenido = contenido.Replace("class=\"ql-align-justify\"", "style=\"text-align:justify\"");
                contenido = contenido.Replace("class=\"ql-align-right\"", "style=\"text-align:right\"");
                contenido = contenido.Replace("class=\"ql-align-left\"", "style=\"text-align:left\"");

                FontFactory.RegisterDirectories();

                double margenIzquierdo = CentimetrosAMedidaPDF(margenes.Izquierda);
                double margenDerecho = CentimetrosAMedidaPDF(margenes.Derecha);
                double margenSuperior = CentimetrosAMedidaPDF(margenes.Arriba);
                double margenInferior = CentimetrosAMedidaPDF(margenes.Abajo);

                Document document = new Document();
                iTextSharp.text.Rectangle z = PageSize.LETTER;
                document = new Document(z, (float)margenIzquierdo, (float)margenDerecho, (float)margenSuperior, (float)margenInferior);

                if (pEsHorizontal == true)
                    document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());


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
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                try
                {
                    //header = XMLWorkerHelper.ParseToElementList(this.Encabezado, null);
                    PdfPTable tbHeader = new PdfPTable(1);
                    tbHeader.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                    tbHeader.DefaultCell.Border = 0;

                    //tbHeader.AddCell(new Paragraph());

                    PdfPCell _cell = new PdfPCell(new Paragraph("FONDO DE FINANCIAMIENTO DE INFRAESTRUCTURA EDUCATIVA - FFIE \n MINISTERIO DE EDUCACIÓN"));
                    _cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _cell.Border = 0;

                    tbHeader.AddCell(_cell);

                    tbHeader.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetTop(document.TopMargin) + this.MargenSuperior + 40, writer.DirectContent);

                    PdfPTable tbFooter = new PdfPTable(3);
                    tbFooter.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                    tbFooter.DefaultCell.Border = 0;

                    tbFooter.AddCell(new Paragraph());
                    _cell = new PdfPCell(new Paragraph());
                    _cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _cell.Border = 0;
                    tbFooter.AddCell(_cell);

                    _cell = new PdfPCell(new Paragraph("Página " + writer.PageNumber));
                    _cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _cell.Border = 0;

                    tbFooter.AddCell(_cell);

                    tbFooter.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetBottom(document.BottomMargin) - this.MargenInferior - 10, writer.DirectContent);

                    string pathImage = Path.Combine(Directory.GetCurrentDirectory(), "assets", "img-FFIE.png");

                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(pathImage);
                    logo.SetAbsolutePosition(writer.PageSize.GetRight(document.RightMargin), writer.PageSize.GetTop(document.TopMargin));
                    logo.ScaleAbsolute(50f, 50f);
                    document.Add(logo);
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
