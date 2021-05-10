using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using asivamosffie.services.Helpers.Constant;
using asivamosffie.services.Helpers.Enumerator;
using asivamosffie.model.APIModels;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.IO;
using DinkToPdf.Contracts;
using System.Globalization;
using OfficeOpenXml;
using Z.EntityFramework.Plus;
using Microsoft.Extensions.Options;
using asivamosffie.model.AditionalModels;
using asivamosffie.services.Helpers.Extensions;
using DinkToPdf;
using System.Reflection;
using System.Drawing;
using System.Net;

namespace asivamosffie.services
{

    public partial class RegisterPayPerformanceService
    {
        #region Audit
        private async Task<int> GetActionIdAudit(string action)
        {
            return await _commonService.GetDominioIdByCodigoAndTipoDominio(
                            action, (int)EnumeratorTipoDominio.Acciones);
        }

        private async Task<string> SaveAuditAction(string author, int idAction, enumeratorMenu menu, string code, string comment)
        {
            return await _commonService.GetMensajesValidacionesByModuloAndCodigo(
                                                     (int)menu, code, idAction, author, comment);
        }
        #endregion

        #region Validation
        private string CheckFileDownloadDirectory()
        {
            string directory = Path.Combine(_mailSettings.DirectoryBase,
                                                   _mailSettings.DirectoryBaseCargue,
                                                   _mailSettings.DirectoryBasePagos);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }
        #endregion

        static DirectoryInfo _outputDir = null;
        public static DirectoryInfo OutputDir
        {
            get
            {
                return _outputDir;
            }
            set
            {
                _outputDir = value;
                if (!_outputDir.Exists)
                {
                    _outputDir.Create();
                }
            }
        }
        public static FileInfo GetFileInfo(string file, bool deleteIfExists = true)
        {
            var fi = new FileInfo(OutputDir.FullName + Path.DirectorySeparatorChar + file);
            if (deleteIfExists && fi.Exists)
            {
                fi.Delete();  // ensures we create a new workbook
            }
            return fi;
        }

        private static string WriteCollectionToPath<T>(string fileName, string directory, List<T> list, List<ExcelError> errors)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
         
           

            var streams = new MemoryStream();
            using (var packages = new ExcelPackage())
            {
                // Add a new worksheet to the empty workbook
                var worksheet = packages.Workbook.Worksheets.Add("Hoja 1");

                worksheet.Cells.LoadFromCollection(list, true);

                // Add the headers
                Type listType = typeof(T);
                var properties = listType.GetProperties();
                int index = 1;
                foreach (var property in properties)
                {
                    var customAtt = (JsonPropertyAttribute)Attribute.GetCustomAttribute(property, typeof(JsonPropertyAttribute));
                    if (customAtt != null)
                    {
                        worksheet.Cells[1, index].Value = customAtt.PropertyName;
                       // worksheet.Column(index).CellsUsed().SetDataType(XLDataType.Number);
                        if (property.PropertyType.Name == "Decimal")
                        {
                           // worksheet.Cells[1, index].Style.Numberformat.Format = "0.0%";
                            worksheet.Column(index).Style.Numberformat.Format = "0.00";
                            worksheet.Column(index).AutoFit();
                        }
                    }                    
                    index++;
                }
                if (errors != null)
                {
                    var errorGroup = errors.GroupBy(x => new { x.Column, x.Row }).Select(err => new {
                        err.Key.Column,
                        err.Key.Row,
                        Error = string.Join(",", err.Select(i => i.Error))
                    }).ToList<dynamic>();

                    foreach (var item in errorGroup)
                    {
                        ExcelComment? existingError = null;
                        try
                        {
                            existingError = worksheet.Cells[item.Row, item.Column].Comment;
                        }
                        catch (Exception ex) { }
                        if (existingError == null)
                        {
                            worksheet.Cells[item.Row, item.Column].AddComment(item.Error, "Admin");
                        }
                        worksheet.Cells[item.Row, item.Column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[item.Row, item.Column].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                    }
                }

                var xlFile = new FileInfo(directory + Path.DirectorySeparatorChar + listType.Name + ".xlsx");

                // Save the workbook in the output directory
                packages.SaveAs(xlFile);
                return xlFile.FullName;
            }

        }

        public bool SendMail(string template, string subject, EnumeratorPerfil enumeratorProfile)
        {
            bool isMailSent = false;
            var usertoSend = _context.UsuarioPerfil.Where(
                       x => x.PerfilId == (int)enumeratorProfile).Include(y => y.Usuario).AsNoTracking().ToList();
            var userMails = usertoSend.Select(x => x.Usuario.Email).ToList<string>();
            isMailSent = this._commonService.EnviarCorreo(userMails, template, subject);
            return isMailSent;
        }


        private byte[] ConvertirPDF(Plantilla pPlantilla)
        {
            string strEncabezado = "";
            if (!string.IsNullOrEmpty(pPlantilla?.Encabezado?.Contenido))
            {
                strEncabezado = Helpers.Helpers.HtmlStringLimpio(pPlantilla.Encabezado.Contenido);
            }

            var globalSettings = new GlobalSettings
            {
                ImageQuality = 1080,
                PageOffset = 0,
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings
                {
                    Top = 10,
                    Left = 0,
                    Right = 0,
                    Bottom = 0
                },
                DocumentTitle = DateTime.Now.ToString(),
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = pPlantilla.Contenido,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "pdf-styles.css") },
                HeaderSettings = { FontName = "Roboto", FontSize = 8, Center = strEncabezado, Line = false, Spacing = 18 },
                FooterSettings = { FontName = "Ariel", FontSize = 10, Center = "[page]" },
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };

            return _converter.Convert(pdf);
        }

    }

    public partial class RegisterPayPerformanceService : IRegisterPayPerformanceService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;
        private readonly IDocumentService _documentService;
        public readonly IConverter _converter;
        public readonly IRegisterPreContructionPhase1Service _registerPreContructionPhase1Service;
        public readonly IBudgetAvailabilityService _budgetAvailabilityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userName;
        public ISourceFundingService _fundingSourceService { get; }
        public MailSettings _mailSettings { get; }
        public readonly string CONSTPAGOS = "Pagos";
        public readonly int _500 = 500;

        public RegisterPayPerformanceService(IConverter converter,
                                            devAsiVamosFFIEContext context,
                                            ISourceFundingService fundingSourceService,
                                            ICommonService commonService,
                                            IDocumentService documentService,
                                            IOptions<MailSettings> mailSettings,
                                            IHttpContextAccessor httpContextAccessor
            )
        {
            _converter = converter;
            _context = context;
            _fundingSourceService = fundingSourceService;
            _documentService = documentService;
            _mailSettings = mailSettings.Value;
            _commonService = commonService;
            _httpContextAccessor = httpContextAccessor;
            _userName = _httpContextAccessor.HttpContext.User.Identity.Name.ToUpper();
        }

        #region loads

        /// <summary>
        /// Get main tables
        /// </summary>
        /// <param name="typeFile"></param>
        /// <returns></returns>
        public async Task<List<dynamic>> getPaymentsPerformances(string typeFile, string status)
        {
            List<dynamic> listaContrats = new List<dynamic>();

            List<CarguePagosRendimientos> lista = await _context.CarguePagosRendimientos
                .Where(order => !order.Eliminado).AsNoTracking().ToListAsync();

            lista.Where(w => w.TipoCargue == typeFile && (string.IsNullOrEmpty(status) || w.EstadoCargue == status))
                .ToList().ForEach(c =>
                {
                    listaContrats.Add(new
                    {
                        c.CargaPagosRendimientosId,
                        c.NombreArchivo,
                        c.Json,
                        c.Observaciones,
                        c.TotalRegistros,
                        c.RegistrosValidos,
                        c.RegistrosInvalidos,
                        c.CargueValido,
                        c.FechaCargue,
                        c.RegistrosInconsistentes,
                        c.RegistrosConsistentes,
                        c.MostrarInconsistencias,
                        c.FechaTramite,
                        c.PendienteAprobacion
                    });
                });

            return listaContrats;
        }

        /// <summary>
        /// Validate and Upload Payments and Performances
        /// </summary>
        /// <param name="pFile"></param>
        /// <param name="fileType"></param>
        /// <param name="saveSuccessProcess"></param>
        /// 
        /// <returns></returns>        
        public async Task<Respuesta> UploadFileToValidate(IFormFile pFile, string fileType, bool saveSuccessProcess)
        {
            int CantidadRegistrosInvalidos = 0;
            int idAction = await GetActionIdAudit(ConstantCodigoAcciones.Validar_Excel_Registro_Pagos);
            string actionMesage = fileType == CONSTPAGOS ?
                ConstantCommonMessages.Performances.REGISTRAR_ORDENES_PAGOS : ConstantCommonMessages.Performances.REGISTRAR_RENDIMIENTOS;
            if (pFile == null)
            {
                return new Respuesta
                {
                    IsSuccessful = false,
                    IsException = false,
                    IsValidation = true,
                    Code = GeneralCodes.EntradaInvalida,
                    Message = await SaveAuditAction(_userName, idAction,
                                        enumeratorMenu.RegistrarPagosRendimientos,
                                        GeneralCodes.EntradaInvalida,
                                        actionMesage)
                };
            }

            using (var stream = new MemoryStream())
            {
                List<Dictionary<string, string>> listaCarguePagosRendimientos = new List<Dictionary<string, string>>();
                List<ExcelError> errors = new List<ExcelError>();

                await pFile.CopyToAsync(stream);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var package = new ExcelPackage(stream);

                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                // TODO add validation to prevent query a column in a  not existing position

                var columnAccounst = worksheet.Cells[2, 2, worksheet.Dimension.Rows, 2].Select(v => v.Text).ToList();

                var bankAccounts = _context.CuentaBancaria.Where(
                    x => x.NumeroCuentaBanco != null && columnAccounst.Contains(x.NumeroCuentaBanco)).AsNoTracking();

                List<string> accounts = new List<string>();
                bool accountsDifferents = columnAccounst.Count != worksheet.Dimension.Rows - 1;

                // Query payment 
                var paymentNumColum = worksheet.Cells[2, 1, worksheet.Dimension.Rows, 1].Select(v => v.Text).ToList();
                HashSet<string> paymentNumbers = new HashSet<string>();

                //Controlar Registros

                //TODO ADD PARALLEL FOREACH

                for (int indexWorkSheetRow = 2; indexWorkSheetRow <= worksheet.Dimension.Rows; indexWorkSheetRow++)
                {
                    Dictionary<string, string> carguePagosRendimiento = new Dictionary<string, string>();
                    
                    bool tieneError = false;
                    try
                    {
                        if (fileType == CONSTPAGOS)
                        {
                            var validatedValues = await ValidatePaymentFile(worksheet, indexWorkSheetRow, paymentNumbers);
                            carguePagosRendimiento = validatedValues.list;
                            if(validatedValues.errors != null)
                                errors.AddRange(validatedValues.errors);
                            tieneError = validatedValues.errors.Count >0;
                        }
                        else if (fileType == "Rendimientos")
                        {
                            var validatedValues = ValidateFinancialPerformanceFile(worksheet, indexWorkSheetRow, bankAccounts, accounts);

                            carguePagosRendimiento = validatedValues.list;
                            if (validatedValues.errors != null)
                                errors.AddRange(validatedValues.errors);
                            tieneError = validatedValues.errors.Count > 0;
                        }

                        if (tieneError)
                        {
                            CantidadRegistrosInvalidos++;
                            carguePagosRendimiento.Add("Estado", "Fallido");
                        }
                        else
                        {
                            carguePagosRendimiento.Add("Estado", "Valido");
                        }

                        // accounts.Add(carguePagosRendimiento.GetValueOrDefault("Número de Cuenta"));
                        listaCarguePagosRendimientos.Add(carguePagosRendimiento);
                    }
                    catch (Exception ex)
                    {
                        CantidadRegistrosInvalidos++;
                    }
                }
                CarguePagosRendimientos carguePagosRendimientos = new CarguePagosRendimientos();
                int cantidadRegistrosTotales = worksheet.Dimension.Rows - 1;

                if (CantidadRegistrosInvalidos > 0 || saveSuccessProcess)
                {
                    carguePagosRendimientos = new CarguePagosRendimientos
                    {
                        EstadoCargue = CantidadRegistrosInvalidos > 0 ? "Fallido" : "Valido",
                        CargueValido = CantidadRegistrosInvalidos == 0,
                        NombreArchivo = pFile.FileName,
                        RegistrosValidos = cantidadRegistrosTotales - CantidadRegistrosInvalidos,
                        RegistrosInvalidos = CantidadRegistrosInvalidos,
                        TotalRegistros = cantidadRegistrosTotales,
                        TipoCargue = fileType,
                        FechaCargue = DateTime.Now,
                        Json = JsonConvert.SerializeObject(listaCarguePagosRendimientos),
                        UsuarioCreacion = _userName,
                        Errores = JsonConvert.SerializeObject(errors),
                    };

                    _context.CarguePagosRendimientos.Add(carguePagosRendimientos);
                }
                bool isValidPayment = true;
                if (CantidadRegistrosInvalidos == 0 && saveSuccessProcess && fileType == CONSTPAGOS)
                {
                    try
                    {
                        isValidPayment = await ProcessPayment(fileType, listaCarguePagosRendimientos, carguePagosRendimientos);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                if (isValidPayment)
                {
                    _context.SaveChanges();
                }

                ArchivoCargueRespuesta archivoCargueRespuesta = new ArchivoCargueRespuesta
                {
                    CantidadDeRegistros = cantidadRegistrosTotales.ToString(),
                    CantidadDeRegistrosInvalidos = CantidadRegistrosInvalidos.ToString(),
                    CantidadDeRegistrosValidos = (cantidadRegistrosTotales - CantidadRegistrosInvalidos).ToString(),
                    LlaveConsulta = pFile.FileName
                };

                return new Respuesta
                {
                    Data = archivoCargueRespuesta,
                    IsSuccessful = true,
                    IsException = false,
                    IsValidation = false,
                    Code = GeneralCodes.OperacionExitosa,
                    Message = await SaveAuditAction(_userName, idAction,
                                        enumeratorMenu.RegistrarPagosRendimientos,
                                        GeneralCodes.OperacionExitosa,
                                        actionMesage)
                };
            }
        }

        /// <summary>
        /// Validar que la Orden de Giro exista, corresponda con el valor pagado, 
        /// con el valor que se encuentra en el archivo.
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="indexWorkSheetRow"></param>
        /// <param name="carguePagosRendimiento"></param>
        /// <returns></returns>
        private async Task<(Dictionary<string, string> list, List<ExcelError> errors)> ValidatePaymentFile(ExcelWorksheet worksheet,
            int indexWorkSheetRow, HashSet<string> paymentNumbers)
        {
            Dictionary<string, string> carguePagosRendimiento = new Dictionary<string, string>();
            List<ExcelError> errors = new List<ExcelError>();
            // Validator.Validate(worksheet, indexWorkSheetRow, PaymentValidations.validations);
            // #1
            //Número de orden de giro
            string cellValue = worksheet.Cells[indexWorkSheetRow, 1].Text;
            carguePagosRendimiento.Add("Número de orden de giro", cellValue);
            OrdenGiro ordenGiro = new OrdenGiro();


            if (string.IsNullOrEmpty(cellValue) || cellValue.Length > 50)
            {
                errors.Add(new ExcelError(indexWorkSheetRow, 2, "Longitud inválida"));
            }
            else
            {
                ordenGiro =  await _context.OrdenGiro.Where(x => x.NumeroSolicitud == cellValue 
                        && x.EstadoCodigo == ((int)EnumEstadoOrdenGiro.Con_Orden_de_Giro_Tramitada).ToString())
                    ?.Include(orden => orden.OrdenGiroDetalle)
                    .ThenInclude(detalle => detalle.OrdenGiroDetalleTerceroCausacion).AsNoTracking().FirstOrDefaultAsync();

                var valorNeto = ordenGiro?.OrdenGiroDetalle?.FirstOrDefault()?.OrdenGiroDetalleTerceroCausacion?.FirstOrDefault()?.ValorNetoGiro;

                if (ordenGiro == null || valorNeto == null)
                {
                    errors.Add(new ExcelError(indexWorkSheetRow, 2, $"El orden de giro  número{cellValue}, no tiene una Orden de Giro válida"));
                }
                else
                {
                    var processed = _context.OrdenGiroPago.Any(x => x.OrdenGiroId == ordenGiro.OrdenGiroId);
                    if (processed)
                    {
                        errors.Add(new ExcelError(indexWorkSheetRow, 2, $"El orden de giro  número{cellValue}, ya tiene un pago registrado"));
                    }
                    if(paymentNumbers.Contains(cellValue))
                    {

                        errors.Add(new ExcelError(indexWorkSheetRow, 2, $"Por favor ingrese una única vez la orden de giro, {cellValue} se encuentra más de una vez en el documento"));
                    }
                    paymentNumbers.Add(cellValue);
                } 

            }

            // #2
            //Fecha de pago
            //string sDate = (worksheet.Cells[indexWorkSheetRow, 2] as DocumentFormat.OpenXml.Office.Excel.Range).Value2.ToString();
            //DateTime conv = DateTime.FromOADate(d);

            var dateObje = worksheet.Cells[indexWorkSheetRow, 2].Value;
            DateTime guideDate = new DateTime();
            var cellType = worksheet.Cells[indexWorkSheetRow, 2].Value.GetType().Name;
            if (cellType == "DateTime")
            {
                var dtFormat = "dd/MM/yyyy";
                worksheet.Cells[indexWorkSheetRow, 1].Style.Numberformat.Format = dtFormat;
            }
            string dateText = worksheet.Cells[indexWorkSheetRow, 2].Text;
            bool isDate = TryStringToDate(dateText, out guideDate);

            if (cellType == "Double")
            {
                Double d = Double.Parse(dateObje.ToString());
                guideDate = DateTime.FromOADate(d);
            }

            string dateCellValue = worksheet.Cells[indexWorkSheetRow, 2].Text;

            if (dateCellValue != dateObje.ToString() || cellType == "DateTime" || cellType == "Double")
            {
                carguePagosRendimiento.Add("Fecha de pago", guideDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                // guideDate = dateObje;
            }
            else
            {
                carguePagosRendimiento.Add("Fecha de pago", dateCellValue);
            }

            if (string.IsNullOrEmpty(dateText) || (!isDate && guideDate == DateTime.MinValue))
            {
                errors.Add(new ExcelError(indexWorkSheetRow, 3, "Por favor ingresa solo fechas en este campo"));
            }


            // #3
            //Impuestos
            carguePagosRendimiento.Add("Impuestos", worksheet.Cells[indexWorkSheetRow, 3].Text);
            string impuetoClean = worksheet.Cells[indexWorkSheetRow, 3].Text.Replace(".", "").Replace("$", "");
            if (string.IsNullOrEmpty(worksheet.Cells[indexWorkSheetRow, 3].Text) || impuetoClean.Length > 20 || !decimal.TryParse(impuetoClean, out decimal taxDecimal))
            {
                errors.Add(new ExcelError(indexWorkSheetRow, 4, "Por favor ingresa solo datos numéricos en este campo"));
            }
            else if (ordenGiro != null)
            {
                var solicitudQuery = await (from solicitud in _context.SolicitudPago
                                            from sRegistroSolPago in solicitud.SolicitudPagoRegistrarSolicitudPago
                                            from sFase in sRegistroSolPago.SolicitudPagoFase
                                            from factura in sFase.SolicitudPagoFaseFactura
                                            from descuento in factura.SolicitudPagoFaseFacturaDescuento
                                            where solicitud.OrdenGiroId == ordenGiro.OrdenGiroId &&
                                            sFase.EsPreconstruccion == false
                                            select descuento).AsNoTracking().ToListAsync();

                var valorTotalDescuentos = solicitudQuery.Sum(x => x.ValorDescuento);

                if(solicitudQuery == null || taxDecimal != valorTotalDescuentos)
                {
                    errors.Add(new ExcelError(indexWorkSheetRow, 4, "Por favor ingresa el valor de Impuesto que coincida con la Orden de Giro almacenada"));
                }
                
            }
            

            // #4
            //Valor neto girado
            carguePagosRendimiento.Add("Valor neto girado", worksheet.Cells[indexWorkSheetRow, 4].Text);
            string valorNetoClean = worksheet.Cells[indexWorkSheetRow, 4].Text.Replace(".", "").Replace("$", "");
            if (string.IsNullOrEmpty(worksheet.Cells[indexWorkSheetRow, 4].Text) || valorNetoClean.Length > 20 || !decimal.TryParse(valorNetoClean, out decimal respValNetoNumber))
            {
                errors.Add(new ExcelError(indexWorkSheetRow, 5, "Por favor ingresa solo datos numéricos en este campo"));
            }
            else if (ordenGiro != null)
            {
                var valorNeto = ordenGiro?.OrdenGiroDetalle?.FirstOrDefault()?.OrdenGiroDetalleTerceroCausacion?.FirstOrDefault()?.ValorNetoGiro;
                if (!valorNeto.HasValue || valorNeto.Value != respValNetoNumber)
                {
                    errors.Add(new ExcelError(indexWorkSheetRow, 5, "La orden de giro tiene un valor diferente"));
                }
            }

            return (carguePagosRendimiento, errors);
        }

        public bool TryStringToDate(string sDate, out DateTime newDate)
        {
            newDate = DateTime.MinValue;
            string[] dateElements = sDate.Split('/');

            if (dateElements.Length == 0 || dateElements.Length < 3)
            {
                return false;
            }

            if(int.TryParse(dateElements[2], out int year) &&
                int.TryParse(dateElements[1], out int month) &&
                int.TryParse(dateElements[0], out int day))
            {
                newDate = new DateTime(year, month, day);
                return true;
            }
            return false;
        }

        private (Dictionary<string, string> list, List<ExcelError> errors) ValidateFinancialPerformanceFile(ExcelWorksheet worksheet, int indexRow, IEnumerable<CuentaBancaria> bankAccounts, List<string> accounts)
        {
            Dictionary<string, string> carguePagosRendimiento = new Dictionary<string, string>();
            List<ExcelError> errors = new List<ExcelError>();
            Dictionary<string, string> performanceStructure = new Dictionary<string, string>();
            performanceStructure.Add("Fecha de rendimientos", "Date"); //same month
            performanceStructure.Add("Número de Cuenta", "AlphaNum"); // no repeat
            performanceStructure.Add("Acumulado de aportes de recursos exentos", "Money");
            performanceStructure.Add("Acumulado de rendimientos exentos", "Money");
            performanceStructure.Add("Acumulado de gastos Bancarios exentos", "Money");
            performanceStructure.Add("Acumulado de gravamen financiero descontado exentos", "Money");
            performanceStructure.Add("Acumulado de aportes de recursos no exentos", "Money");
            performanceStructure.Add("Acumulado de rendimientos no exentos", "Money");
            performanceStructure.Add("Acumulado de gastos Bancarios no exentos", "Money");
            performanceStructure.Add("Acumulado de gravamen financiero descontado no exentos", "Money");

            int indexCell = 1;


            var dateObje = worksheet.Cells[indexRow, 1].Value;
            DateTime guideDate = new DateTime();
            var cellType = worksheet.Cells[indexRow, 1].Value.GetType().Name;
            if (cellType ==  "DateTime")
            {
                var dtFormat = "dd/MM/yyyy";
                worksheet.Cells[indexRow, 1].Style.Numberformat.Format = dtFormat;
            }
            string dateValue = worksheet.Cells[indexRow, 1].Text;
            TryStringToDate(dateValue, out guideDate);

            if(cellType == "Double")
            {
                Double d = Double.Parse(dateObje.ToString());
                guideDate = DateTime.FromOADate(d);
            }
                        
            int month = guideDate.Month;

            carguePagosRendimiento.Add("Row", indexRow.ToString());
            foreach (var rowFormat in performanceStructure)
            {
                string cellValue = worksheet.Cells[indexRow, indexCell].Text;

                if (rowFormat.Value == "Date" && ( cellValue != dateObje.ToString() || cellType == "DateTime" || cellType == "Double") )
                {
                    carguePagosRendimiento.Add(rowFormat.Key, guideDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                   // guideDate = dateObje;
                }
                else
                {
                    carguePagosRendimiento.Add(rowFormat.Key, cellValue);
                }
                if (rowFormat.Value == "Date")
                {
                    if (string.IsNullOrEmpty(cellValue) || !TryStringToDate(cellValue, out DateTime fecha))
                    {
                        errors.Add(new ExcelError(indexRow, indexCell + 1, "Por favor ingresa solo fechas en este campo"));
                    }
                    else if(guideDate != DateTime.MinValue && guideDate.Month != fecha.Month)
                    {
                        errors.Add(new ExcelError(indexRow, indexCell + 1, "Por favor ingrese todos los rendimientos del mismo mes"));
                    }
                }
                else if ((rowFormat.Value == "AlphaNum")
                    && (string.IsNullOrWhiteSpace(cellValue) || !cellValue.IsValidSize(50)))
                {
                    errors.Add(new ExcelError(indexRow, indexCell + 1, "Por favor ingresa solo datos alfanuméricos en este campo"));
                }
                else if ((rowFormat.Value == "Money")
                    && (string.IsNullOrWhiteSpace(cellValue) || !cellValue.IsMoneyValueValidSize(20)))
                {
                    errors.Add(new ExcelError(indexRow, indexCell+1, "Por favor ingresa solo datos numéricos en este campo"));
                }

                if (errors.Count  == 0 && rowFormat.Key == "Número de Cuenta")
                {
                    if (accounts.Any(x => x == cellValue))
                    {
                        errors.Add(new ExcelError(indexRow, indexCell + 1, $" El archivo cuenta con números de cuenta ya repetidos. Por favor verifique y vuelva a cargar."));
                    }
                    accounts.Add(cellValue);

                    var activeAccount = bankAccounts.SingleOrDefault(account => account.NumeroCuentaBanco == cellValue);
                    var hasError = activeAccount == null;
                    if (activeAccount != null)
                    {
                        hasError = !activeAccount.FuenteFinanciacionId.HasValue ||
                            (activeAccount.Eliminado.HasValue && activeAccount.Eliminado.Value);
                    }

                    if (hasError)
                    {
                        errors.Add(new ExcelError(indexRow, indexCell + 1, $"Por favor ingresa un {rowFormat.Key} válido en este campo"));
                    }
                }

                indexCell += 1;
            }

            return (carguePagosRendimiento, errors);
        }

        #endregion

        /// <summary>
        /// TODO validar que este no tenga relaciones o información dependiente. 
        /// En caso de contar con alguna relación, el sistema debe informar al usuario
        /// por medio de un mensaje emergente “El registro tiene información que depende de él, no se puede eliminar”
        /// , y no deberá eliminar el registro.
        /// </summary>
        /// <param name="cargaPagosRendimientosId"></param>
        /// <param name="uploadStatus"></param>
        /// <returns></returns>
        public async Task<Respuesta> DeletePayment(int uploadedOrderId, string author)
        {
            string actionMesage = ConstantCommonMessages.Performances.ELIMINAR_ORDENES_PAGOS;
            int actionId = await GetActionIdAudit(ConstantCodigoAcciones.Eliminar_Pagos);
            int modifiedRows = -1;
            if (uploadedOrderId > 0)
            {
                modifiedRows = await _context.Set<CarguePagosRendimientos>()
                      .Where(order => order.CargaPagosRendimientosId == uploadedOrderId && !order.FechaTramite.HasValue)
                      .UpdateAsync(o => new CarguePagosRendimientos()
                      {
                          FechaModificacion = DateTime.Now,
                          UsuarioModificacion = author,
                          Eliminado = true
                      });
            }
            string codeResponse = modifiedRows > 0 ? GeneralCodes.OperacionExitosa : ConstMessagesPerformances.ErrorGuardarCambios;

            return new Respuesta
            {
                Data = modifiedRows,
                IsSuccessful = modifiedRows > 0,
                IsException = false,
                IsValidation = false,
                Code = GeneralCodes.OperacionExitosa,
                Message = await SaveAuditAction(_userName, actionId,
                                        enumeratorMenu.RegistrarPagosRendimientos,
                                        codeResponse,
                                        actionMesage)
            };
        }

        public async Task<Respuesta> DownloadPaymentPerformanceAsync(FileRequest fileRequest, string fileType)
        {
            Respuesta response = new Respuesta();
            string action = fileType == CONSTPAGOS ? ConstantCodigoAcciones.Ver_Detalle_Pagos : ConstantCodigoAcciones.Ver_Detalle_Rendimientos;
            int actionId = await GetActionIdAudit(action);
            string actionMesage = fileType == CONSTPAGOS ?
                ConstantCommonMessages.Performances.VER_DETALLE_PAGOS : ConstantCommonMessages.Performances.VER_DETALLE_RENDIMIENTOS;
            string directory = CheckFileDownloadDirectory();
            string filePath = string.Empty;
            string jsonString = string.Empty;
            try
            {
                var collection = _context.CarguePagosRendimientos.Where(x => x.CargaPagosRendimientosId == fileRequest.ResourceId)
                            .Select(
                            order => new
                            {
                                Estado = order.CargueValido,
                                ArchivoJson = order.Json,
                                Errores = order.Errores
                            }).FirstOrDefault();
                jsonString = collection.ArchivoJson;

                List<ExcelError> errors = new List<ExcelError>();
                if(collection.Errores != null)
                    errors = JsonConvert.DeserializeObject<List<ExcelError>>(collection.Errores);

                if (fileType == CONSTPAGOS)
                {
                    List<EntryPaymentOrder> list = SerializePaymentOrders(fileRequest.ResourceId, collection.ArchivoJson);                    
                    filePath = WriteCollectionToPath(fileRequest.FileName, directory, list, errors);
                }
                else
                {
                    List<EntryPerformanceOrder> list2 = SerializePerformances(fileRequest.ResourceId, collection.ArchivoJson);
                    filePath = WriteCollectionToPath(fileRequest.FileName, directory, list2, errors);
                }
            }
            catch (Newtonsoft.Json.JsonSerializationException ex)
            {
                response.IsException = true;
                response.IsSuccessful = true;
                response.Code = ConstMessagesPayments.ErrorDescargarArchivo;
                response.Data = jsonString;
                response.Message = await SaveAuditAction(_userName, actionId,
                                        enumeratorMenu.RegistrarPagosRendimientos,
                                        response.Code,
                                         ex.SubstringValid(_500));
                return response;

            }
            catch (Exception ex)
            {
                response.IsException = true;
                response.IsSuccessful = true;
                response.Code = ConstMessagesPayments.ErrorDescargarArchivo;
                response.Data = jsonString;
                response.Message = await SaveAuditAction(_userName, actionId,
                                        enumeratorMenu.RegistrarPagosRendimientos,
                                        response.Code,
                                         ex.SubstringValid(_500));

                return response;

            }

            response.Data = filePath;
            response.IsSuccessful = true;
            response.Code = GeneralCodes.OperacionExitosa;
            response.Message = await SaveAuditAction(_userName, actionId,
                                    enumeratorMenu.RegistrarPagosRendimientos,
                                    GeneralCodes.OperacionExitosa,
                                    actionMesage);

            return response;
        }

        private List<EntryPerformanceOrder> SerializePerformances(int uploadedOrderId, string stringJson)
        {
            var list = JsonConvert.DeserializeObject<List<EntryPerformanceOrder>>(stringJson);
            return list;
        }

        private List<EntryPaymentOrder> SerializePaymentOrders(int uploadedOrderId, string stringJson)
        {
            var list = JsonConvert.DeserializeObject<List<EntryPaymentOrder>>(stringJson);
            return list;
        }

        /// <summary>
        /// Account - Número de Cuenta
        /// #2 GeneratedReturns  - Total rendimientos generados SUM x => “acumulado de rendimientos de recursos exentos”  
        ///                                              +   SUM X =>  “acumulado de  rendimientos de recursos no exentos”
        /// Current Month ?
        /// # 3 Rendimientos incorporados  //Valor de los rendimientos incorporados hasta el mes anterior (acumulado).
        /// # 4 Provisión gravamen financiero
        /// #5 Total gastos bancarios SUM  “Acumulado de gastos Bancarios exentos” y “Acumulado de gastos Bancarios no exentos”
        /// #6 Total gravamen financiero descontado “Acumulado de gravamen financiero descontado exentos” 
        ///                                       + “Acumulado de gravamen financiero descontado no exentos”
        /// #7 Visitas  // Sum payments Visitas as Concepto de Pago , see order details
        /// # 8 Rendimiento a incorporar  #2 + #3 + #4 + #5 + #6 +7
        /// Observación Consistente/Inconsistente 
        /// Si #8 negative
        /// En caso que una fuente no tenga información de al menos una cuenta bancaria, la fuente no se podrá visualizar en ningún otro módulo de la plataforma.
        /// </summary>
        /// <param name="uploadedOrderId"></param>
        /// <returns></returns>
        public async Task<Respuesta> ManagePerformanceAsync(int uploadedOrderId)
        {
            Respuesta response = new Respuesta();
            string action = ConstantCodigoAcciones.Tramitar_Rendimientos;
            int actionId = await GetActionIdAudit(action);
            string actionMesage = ConstantCommonMessages.Performances.TRAMITAR_RENDIMIENTOS;
            
            var collection = await _context.CarguePagosRendimientos.FindAsync(uploadedOrderId);

            List<PerformanceOrder> performanceOrders = new List<PerformanceOrder>();
            if (collection != null)
            {
                performanceOrders = JsonConvert.DeserializeObject<List<PerformanceOrder>>(collection.Json);
            }

            var accountNumbers = performanceOrders.Select(x => x.AccountNumber).Distinct();

            decimal valorAporteEnCuenta = 0;
            int registrosInconsistentes = 0;

            TryStringToDate(performanceOrders.First().PerformancesDate,  out DateTime currentMonthPerformances);
            DateTime beforeMonth = new DateTime(currentMonthPerformances.Year, currentMonthPerformances.Month, 1);
            DateTime lastDayMonth = beforeMonth.AddDays(-1);
            DateTime firstDayMonth = new DateTime(lastDayMonth.Year, lastDayMonth.Month, 1);

            var performancesIncorporated = await _context.RendimientosIncorporados
                .Where(x => x.FechaRendimientos >= firstDayMonth && x.FechaRendimientos <= lastDayMonth)
                .AsNoTracking().ToListAsync();


            foreach (var accountOrder in performanceOrders)
            {
                decimal? performances = 0;
                if (performancesIncorporated.Count > 0)
                {
                    performances = performancesIncorporated.Where(v => v.Aprobado.HasValue
                        && v.Aprobado.Value && v.CuentaBancaria == accountOrder.AccountNumber)
                        .Sum(x => x.RendimientoIncorporar);
                }


                valorAporteEnCuenta = 0;
                // TODO Review Payment concept..
                var accountPayments = _context.VCuentaBancariaPago.Where(acc => acc.NumeroCuentaBanco == accountOrder.AccountNumber
                & acc.TipoPagoCodigo == ConstantTipoPago.Visitas);

                var visitas = accountPayments.Sum(v => v.ValorNetoGiro);
                var account = _context.CuentaBancaria.Where(x => x.NumeroCuentaBanco == accountOrder.AccountNumber).FirstOrDefault();


                if (account == null)
                {
                    new Exception("La cuenta asignada no existe");
                }
                var fundingSources = await _context.FuenteFinanciacion.
                    Where(r => !(bool)r.Eliminado && account.FuenteFinanciacionId == r.FuenteFinanciacionId).
                    Include(r => r.Aportante).ToListAsync();

                foreach (var fundingSource in fundingSources)
                {
                    fundingSource.ControlRecurso = _context.ControlRecurso
                        .Where(x => x.FuenteFinanciacionId == fundingSource
                        .FuenteFinanciacionId && !(bool)x.Eliminado).AsNoTracking().ToList();
                    foreach (var control in fundingSource.ControlRecurso)
                    {
                        control.FuenteFinanciacion = null; //
                    }

                    foreach (var element in fundingSource.ControlRecurso)
                    {
                        valorAporteEnCuenta += element.ValorConsignacion;
                    }
                }

                // Rendimientos a incorporar

                accountOrder.PerformancesToAdd = accountOrder.GeneratedPerformances
                    - (performances.HasValue ? performances.Value : 0)
                    - accountOrder.FinancialLienProvision
                    - accountOrder.BankCharges
                    - accountOrder.DiscountedCharge
                    - (visitas.HasValue ? visitas.Value : 0);
                accountOrder.IsConsistent = true;
                if(accountOrder.PerformancesToAdd < 0 && valorAporteEnCuenta + accountOrder.PerformancesToAdd < 0 )
                {
                    accountOrder.IsConsistent = false;
                    registrosInconsistentes += 1;
                }
                TryStringToDate(accountOrder.PerformancesDate, out DateTime performancesDate);
                var performanceEntity = new RendimientosIncorporados
                {
                    CarguePagosRendimientosId = uploadedOrderId,
                    FechaRendimientos = performancesDate,
                    CuentaBancaria = accountOrder.AccountNumber,
                    TotalRendimientosGenerados = accountOrder.GeneratedPerformances,
                    Incorporados = (performances.HasValue ? performances.Value : 0),
                    ProvisionGravamenFinanciero = accountOrder.FinancialLienProvision,
                    TotalGastosBancarios = accountOrder.BankCharges,
                    TotalGravamenFinancieroDescontado = accountOrder.LiableDiscountedCharge,
                    Visitas = (visitas.HasValue ? visitas.Value : 0),
                    RendimientoIncorporar = accountOrder.PerformancesToAdd,
                    Consistente = accountOrder.IsConsistent,
                    Row = accountOrder.Row
                };
                _context.RendimientosIncorporados.Add(performanceEntity);
            }

            var tramite = JsonConvert.SerializeObject(performanceOrders);
            try
            {
                await _context.Set<CarguePagosRendimientos>()
                  .Where(order => order.CargaPagosRendimientosId == uploadedOrderId)
                  .UpdateAsync(o => new CarguePagosRendimientos()
                  {
                      FechaTramite = DateTime.Now,
                      //TODO Field to remove , will be replace by RendimientosIncorporados table
                      TramiteJson = tramite,
                      RegistrosConsistentes = performanceOrders.Count - registrosInconsistentes,
                      RegistrosInconsistentes = registrosInconsistentes,
                  }); ;
                await _context.SaveChangesAsync();
                response.Data = performanceOrders;
                response.IsSuccessful = true;
                response.Code = GeneralCodes.OperacionExitosa;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.IsException = true;
                response.Code = GeneralCodes.Error;
                response.Message = await SaveAuditAction(_userName, actionId,
                                    enumeratorMenu.GestionarRendimientos,
                                    GeneralCodes.EntradaInvalida,
                                    actionMesage);
            }

            response.Message = await SaveAuditAction(_userName, actionId,
                                    enumeratorMenu.GestionarRendimientos,
                                    GeneralCodes.OperacionExitosa,
                                    actionMesage);
            return response;
        }

        // Notify to Approve
        public async Task<Respuesta> NotifyRequestManagedPerformancesApproval(int uploadedOrderId)
        {
            Respuesta response = new Respuesta();
            bool isMailSent = false;

            isMailSent = true;

            int modifiedRows = -1;
            int actionId = await GetActionIdAudit(ConstantCodigoAcciones.Notificar_Solicitud_Aprobacion);
            string actionMesage = ConstantCommonMessages.Performances.NOTIFICAR_SOLICITUD_APROBACION;
            CarguePagosRendimientos uploadedPerformances = await _context.CarguePagosRendimientos
                    .Where(d => d.CargaPagosRendimientosId == uploadedOrderId).AsNoTracking().FirstOrDefaultAsync();

            if (uploadedPerformances == null || uploadedOrderId == 0)
            {
                response.Code = GeneralCodes.EntradaInvalida;
                response.Message = await SaveAuditAction(_userName, actionId,
                                        enumeratorMenu.GestionarRendimientos,
                                        response.Code,
                                        actionMesage);
                return response;
            }

            try
            {
                Template temNotifyInconsistencies = await _commonService.GetTemplateById(
                                                            (int)enumeratorTemplate.NotificacionAprobacion);
                string inconsistencies = "";
                string fechaCargue = "";
                string total = "";
                string consistencies = "";

                if (uploadedPerformances != null)
                {
                    inconsistencies = uploadedPerformances.RegistrosInconsistentes.ToString();
                    total = uploadedPerformances.TotalRegistros.ToString();
                    consistencies = uploadedPerformances.RegistrosConsistentes.ToString();
                    fechaCargue = Convert.ToDateTime(uploadedPerformances.FechaCargue).ToString("dd/MM/yyyy");
                }
              
                string template = temNotifyInconsistencies.Contenido.
                    Replace("[FECHA_CARGUE]", fechaCargue).
                    Replace("[TOTAL]", total).
                    Replace("[CONSISTENTES]", consistencies).
                    Replace("[INCONSISTENTES]", inconsistencies);

                string subject = "Aprobar Incorporación de rendimientos";

                isMailSent = this.SendMail(template, subject, EnumeratorPerfil.CordinadorFinanciera);


                if (!isMailSent)
                {
                    response.Code = ConstMessagesPerformances.ErrorEnviarCorreo;
                    response.Message = await SaveAuditAction(_userName, actionId, enumeratorMenu.GestionarRendimientos,
                                            response.Code, actionMesage);
                    return response;
                }

                modifiedRows = await _context.Set<CarguePagosRendimientos>()
                    .Where(order => order.CargaPagosRendimientosId == uploadedOrderId)
                    .UpdateAsync(o => new CarguePagosRendimientos()
                    {
                        PendienteAprobacion = true,
                        FechaModificacion = DateTime.Now,
                        UsuarioModificacion = _userName
                    });


                response.IsSuccessful = isMailSent;
                response.Code = ConstMessagesPerformances.CorreoEnviado;
                string message = await SaveAuditAction(_userName, actionId, enumeratorMenu.GestionarRendimientos,
                                            response.Code, actionMesage);
                response.Message = message;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.IsException = true;
                response.Code = GeneralCodes.Error;
                response.Message = await SaveAuditAction(_userName, actionId, enumeratorMenu.GestionarRendimientos,
                                            response.Code, ex.SubstringValid(_500));
            }

            return response;
        }


        /// <summary>
        /// Al guardar la información para los registros válidos, 
        /// el sistema deberá afectar los saldos de las cuentas asociadas 
        /// a las órdenes de giro especificadas en el archivo, de acuerdo 
        /// con los valores referenciados en cada una de ellas.
        /// La fuente de financiación tiene una cuenta bancaria y cada fuente está asociada a un aportan
        /// </summary>
        /// <param name="typeFile"></param>
        /// <param name="listaCarguePagosRendimientos"></param>
        public async Task<bool> ProcessPayment(string typeFile, List<Dictionary<string, string>> listaCarguePagosRendimientos,
            CarguePagosRendimientos carguePagosRendimientos)
        {
            foreach (var payment in listaCarguePagosRendimientos)
            {
                if (typeFile == "Pagos")
                {
                    string cellValue = payment["Número de orden de giro"];
                    var ordenGiro = await _context.OrdenGiro.Where(
                    x => x.NumeroSolicitud == cellValue)
                    .Include(detalle => detalle.OrdenGiroDetalle)
                    .ThenInclude(causacion => causacion.OrdenGiroDetalleTerceroCausacion)
                    .ThenInclude(detalle => detalle.OrdenGiroDetalleTerceroCausacionAportante)
                    .AsNoTracking().FirstOrDefaultAsync();

                    var OrdenGiroDetalleTerceroCausacionAportante = ordenGiro?.OrdenGiroDetalle?.FirstOrDefault()?.
                        OrdenGiroDetalleTerceroCausacion.FirstOrDefault()?.OrdenGiroDetalleTerceroCausacionAportante.FirstOrDefault();
                    var gestionFuenteFinanciacionId =  (int)OrdenGiroDetalleTerceroCausacionAportante.FuenteFinanciacionId;
                     
                    var valorSolicitado = payment["Valor neto girado"];
                    var pDisponibilidadPresObservacion = new GestionFuenteFinanciacion();
                    pDisponibilidadPresObservacion.FuenteFinanciacionId = gestionFuenteFinanciacionId;
                    pDisponibilidadPresObservacion.ValorSolicitado = decimal.Parse(valorSolicitado);
                    pDisponibilidadPresObservacion.UsuarioCreacion = _userName;
                    var valoresSolicitados = _context.GestionFuenteFinanciacion.Where(x => !(bool)x.Eliminado && x.FuenteFinanciacionId == pDisponibilidadPresObservacion.FuenteFinanciacionId).Sum(x => x.ValorSolicitado);
                    var fuente = _context.FuenteFinanciacion.Find(pDisponibilidadPresObservacion.FuenteFinanciacionId);
                    pDisponibilidadPresObservacion.SaldoActual = (decimal)fuente.ValorFuente - valoresSolicitados;
                    pDisponibilidadPresObservacion.NuevoSaldo = pDisponibilidadPresObservacion.SaldoActual - pDisponibilidadPresObservacion.ValorSolicitado;
                    int estado = (int)EnumeratorEstadoGestionFuenteFinanciacion.Solicitado;
                    pDisponibilidadPresObservacion.FechaCreacion = DateTime.Now;
                    pDisponibilidadPresObservacion.EstadoCodigo = estado.ToString();
                    pDisponibilidadPresObservacion.Eliminado = false;
                    _context.GestionFuenteFinanciacion.Add(pDisponibilidadPresObservacion);


                    // cruce de pagos
                    var paymentOrder = new OrdenGiroPago();
                    paymentOrder.OrdenGiroId = ordenGiro.OrdenGiroId;
                    carguePagosRendimientos.OrdenGiroPago.Add(paymentOrder);
                }
            }
            return true;
        }

        public async Task<Respuesta> SetObservationPayments(string observaciones, int uploadedOrderId)
        {
            Respuesta response = new Respuesta();
            string actionMesage = ConstantCommonMessages.Performances.OBSERVACIONES_PAGOS;
            int actionId = await GetActionIdAudit(ConstantCodigoAcciones.Comentar_Pagos);
            int modifiedRows = 0;
            if (string.IsNullOrWhiteSpace(observaciones))
            {
                response.Code = GeneralCodes.CamposVacios;
                response.Message = await SaveAuditAction(_userName, actionId,
                                        enumeratorMenu.RegistrarPagosRendimientos,
                                        GeneralCodes.EntradaInvalida,
                                        actionMesage);
                return response;
            }

            if (uploadedOrderId > 0)
            {
                modifiedRows = await _context.Set<CarguePagosRendimientos>()
                      .Where(order => order.CargaPagosRendimientosId == uploadedOrderId)
                      .UpdateAsync(o => new CarguePagosRendimientos()
                      {
                          FechaModificacion = DateTime.Now,
                          UsuarioModificacion = _userName,
                          Observaciones = observaciones
                      });

            }
            string codeResponse = modifiedRows > 0 ? GeneralCodes.OperacionExitosa : GeneralCodes.EntradaInvalida;

            response.IsSuccessful = true;
            response.Code = codeResponse;
            response.Message = await SaveAuditAction(_userName, actionId,
                                        enumeratorMenu.RegistrarPagosRendimientos,
                                        codeResponse,
                                        actionMesage);

            return response;
        }


        /// <summary>
        /// el sistema notificará a la fiduciaria que se encuentra datos inconsistentes 
        /// y generará un archivo de Excel con la información reportada por la fiduciaria
        /// y la información calculada por el sistema, con el nombre “Inconsistentes”. 
        /// En la primera columna se presentará si el registro es inconsistente. 
        /// </summary>
        /// <param name="uploadedOrderId"></param>
        /// <returns></returns>
        public async Task<Respuesta> NotifyEmailPerformanceInconsistencies(
            int uploadedOrderId, string author)
        {
            Respuesta response = new Respuesta();
            bool isMailSent = false;

            int modifiedRows = -1;
            int actionId = await GetActionIdAudit(ConstantCodigoAcciones.Notificar_Inconsistencias);
            string actionMesage = ConstantCommonMessages.Performances.NOTIFICAR_INCONSISTENCIAS;
            CarguePagosRendimientos uploadedPerformances = null;

            uploadedPerformances = await _context.CarguePagosRendimientos
                    .Where(d => d.CargaPagosRendimientosId == uploadedOrderId).AsNoTracking().FirstOrDefaultAsync();

            if (uploadedPerformances == null || uploadedOrderId == 0)
            {
                response.Code = GeneralCodes.EntradaInvalida;
                response.Message = await SaveAuditAction(author, actionId,
                                        enumeratorMenu.GestionarRendimientos,
                                        response.Code,
                                        actionMesage);
                return response;
            }

            try
            {
                Template temNotifyInconsistencies = await _commonService.GetTemplateById(
                                                            (int)enumeratorTemplate.NotificacionInconsistencias);

                string inconsistencies = "";
                string fechaCargue = "";
                string total = "";
                string consistencies = "";

                if (uploadedPerformances != null)
                {
                    inconsistencies = uploadedPerformances.RegistrosInconsistentes.ToString();
                    total = uploadedPerformances.TotalRegistros.ToString();
                    consistencies = uploadedPerformances.RegistrosConsistentes.ToString();
                    fechaCargue = Convert.ToDateTime(uploadedPerformances.FechaCargue).ToString("dd/MM/yyyy");
                }

                string template = temNotifyInconsistencies.Contenido.
                    Replace("[FECHA_CARGUE]", fechaCargue).
                    Replace("[TOTAL]", total).
                    Replace("[CONSISTENTES]", consistencies).
                    Replace("[INCONSISTENTES]", inconsistencies);

                string subject = "Inconsistencias Rendimientos";

                isMailSent = this.SendMail(template, subject, EnumeratorPerfil.Fiduciaria);

                if (!isMailSent)
                {
                    response.Code = ConstMessagesPerformances.ErrorEnviarCorreo;
                    response.Message = await SaveAuditAction(author, actionId, enumeratorMenu.GestionarRendimientos,
                                            response.Code, actionMesage);
                    return response;
                }

                modifiedRows = await _context.Set<CarguePagosRendimientos>()
                        .Where(order => order.CargaPagosRendimientosId == uploadedOrderId)
                        .UpdateAsync(o => new CarguePagosRendimientos()
                        {
                            FechaTramite = DateTime.Now,
                            MostrarInconsistencias = true,
                            FechaModificacion = DateTime.Now,
                            UsuarioModificacion = author
                        });

                response.IsSuccessful = isMailSent;
                response.Code = ConstMessagesPerformances.CorreoEnviado;
                string message = await SaveAuditAction(author, actionId, enumeratorMenu.GestionarRendimientos,
                                            response.Code, actionMesage);

                response.Message = message;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.IsException = true;
                response.Code = GeneralCodes.Error;
                response.Message = await SaveAuditAction(author, actionId, enumeratorMenu.GestionarRendimientos,
                                            response.Code, ex.SubstringValid(_500));
            }
            return response;
        }

        public async Task<Respuesta> GetManagedPerformancesByStatus(string author, int uploadedOrderId, bool? withConsistentOrders = null)
        {
            Respuesta response = new Respuesta();
            string actionMesage = ConstantCommonMessages.Performances.DESCARGAR_RESULTADO;
            if (withConsistentOrders.HasValue)
            {
                actionMesage = withConsistentOrders.Value ? ConstantCommonMessages.Performances.VER_CONSISTENCIAS : ConstantCommonMessages.Performances.VER_INCONSISTENCIAS;
            }

            int actionId = await GetActionIdAudit(ConstantCodigoAcciones.Ver_Inconsistencias);
            string directory = CheckFileDownloadDirectory();
            enumeratorMenu menu = !withConsistentOrders.HasValue ? enumeratorMenu.GestionarRendimientos : enumeratorMenu.GestionarRendimientos;

            if (uploadedOrderId == 0)
            {
                response.IsValidation = true;
                response.IsSuccessful = false;
                response.Code = GeneralCodes.EntradaInvalida;
                response.Message = await SaveAuditAction(author, actionId,
                                        menu,
                                        response.Code,
                                        actionMesage);
                return response;
            }

            var collection = _context.CarguePagosRendimientos.Where(x => x.CargaPagosRendimientosId == uploadedOrderId)
                            .Select(order => new
                            {
                                Estado = order.CargueValido,
                                ArchivoJson = order.TramiteJson,
                                Uploaded = order.Json
                            }).FirstOrDefault();

            var rendimientosIncorporados = _context.RendimientosIncorporados.Where(x => 
                            x.CarguePagosRendimientosId == uploadedOrderId);


            if (string.IsNullOrWhiteSpace(collection.ArchivoJson))
            {
                response.Code = GeneralCodes.EntradaInvalida;
                response.Message = await SaveAuditAction(author, actionId,
                                        menu,
                                        response.Code,
                                        actionMesage);
                return response;
            }
            
            var managedPerfomances = MapManagedPerformances(rendimientosIncorporados, collection.Uploaded);

            if (withConsistentOrders.HasValue)
            {
                string statusFilter = withConsistentOrders.Value ? "Consistente" : "Inconsistente";
                managedPerfomances = managedPerfomances.Where(x => x.Status == statusFilter).ToList();
            }
            string filePath  = WriteCollectionToPath("RendimientosTramitados", directory, managedPerfomances, null);
            ////the path of the file
            string newfilePath = directory + "/" + "RendimientosTramitados" + "_rev.xlsx";

            response.Data = filePath;
            response.IsSuccessful = true;
            response.Message = await SaveAuditAction(author, actionId,
                                        menu,
                                        GeneralCodes.OperacionExitosa,
                                        actionMesage);

            return response;

        }
        /// <summary>
        ///  TODO Save in a unique table, Fallido, Consistente, Inconsistente ????
        ///  OR Load the same process when manage action?
        ///  
        /// </summary>
        /// <param name="entities"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public List<ManagedPerformancesOrderDto> MapManagedPerformances(IEnumerable<RendimientosIncorporados> entities, string uploadedJson)
        {
            List<ManagedPerformancesOrderDto> managedPerformances = new List<ManagedPerformancesOrderDto>();
            List<PerformanceOrder> performanceOrders = JsonConvert.DeserializeObject<List<PerformanceOrder>>(uploadedJson);
            foreach (var item in entities)
            {
                var uploadedOrder = performanceOrders.Find(x => x.Row == item.Row);
                var managedPerformanceOrder = new ManagedPerformancesOrderDto
                {
                    Status = item.Consistente.Value ? "Consistente" : "Inconsistente",
                    PerformancesDate = item.FechaRendimientos.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    AccountNumber = uploadedOrder.AccountNumber,
                    ExemptResources = uploadedOrder.ExemptResources,
                    ExemptPerformances = uploadedOrder.ExemptPerformances,
                    ExemptBankCharges = uploadedOrder.ExemptBankCharges,
                    ExemptDiscountedCharge = uploadedOrder.ExemptDiscountedCharge,
                    LiableContributtions = uploadedOrder.LiableContributtions,
                    LiablePerformances = uploadedOrder.LiablePerformances,
                    LiableBankCharges = uploadedOrder.LiableBankCharges,
                    LiableDiscountedCharge = uploadedOrder.LiableDiscountedCharge,
                    GeneratedPerformances = uploadedOrder.GeneratedPerformances,
                    FinancialLienProvision = uploadedOrder.FinancialLienProvision,
                    BankCharges = uploadedOrder.BankCharges,
                    DiscountedCharge = uploadedOrder.DiscountedCharge,
                    PerformancesToAdd = item.RendimientoIncorporar,
                    // Visitas = item.Visitas
                };
                managedPerformances.Add(managedPerformanceOrder);
            }
            return managedPerformances;
        }

        public async Task<IEnumerable<dynamic>> GetRequestedApprovalPerformances()
        {
            List<dynamic> requestedApprovals = new List<dynamic>();
           // var performancesOrders = _context.CarguePagosRendimientos.Where(
           //     x => !x.Eliminado && x.PendienteAprobacion == true).Select(x =>
           //new
           //{
           //    x.FechaCargue,
           //    x.TramiteJson,
           //    x.CargaPagosRendimientosId
           //}).AsNoTracking();


            var rendimientosIncorporados = 
                from performance in _context.RendimientosIncorporados
                join register in _context.CarguePagosRendimientos 
                on performance.CarguePagosRendimientosId equals register.CargaPagosRendimientosId                                           
                group performance by new { performance.CarguePagosRendimientosId, register.FechaCargue, register.FechaActa, register.RutaActa } into g                                          
                select new { RegisterId = g.Key.CarguePagosRendimientosId, 
                    RegistrosIncorporados = g.Sum(x => x.Aprobado.HasValue && x.Aprobado.Value == true ? 1: 0),
                    g.Key.FechaCargue,
                    g.Key.FechaActa, g.Key.RutaActa
                };
              //  Include(r => r.CarguePagosRendimientos).AsNoTracking(); ;

            //var collection = rendimientosIncorporados.FirstOrDefault().CarguePagosRendimientos;

            //performancesOrders.ToList().ForEach(requestedApproval =>
            //{
            //    int builtInRegister = 0;
            //    List<ManagedPerformancesOrder> list = JsonConvert.DeserializeObject<List<ManagedPerformancesOrder>>(requestedApproval.TramiteJson);
            //    builtInRegister = list.Where(x => x.BuiltIn.HasValue && x.BuiltIn.Value == true).Count();
            //    requestedApprovals.Add(new
            //    {
            //        requestedApproval.FechaCargue,
            //        requestedApproval.CargaPagosRendimientosId,
            //        RegistrosIncorporados = builtInRegister,
            //    });

            //});

            return rendimientosIncorporados.ToList<dynamic>();
        }


        /// Incorporar rendimientos
        /// // Deserialize ManagedPerformanceOrders
            // foreach account 
            // cuentas
            // saldos en fuentest modificar
            // 

            // Add a column incorporado en Cargue o nueva tabla
        public async Task<Respuesta> IncludePerformances(int uploadedOrderId)
        {
            Respuesta response = new Respuesta();
            string actionMesage = ConstantCommonMessages.Performances.INCORPORAR_RENDIMIENTOS;
            int actionId = await GetActionIdAudit(ConstantCodigoAcciones.Aprobar_Incorporacion_Rendimientos);
            enumeratorMenu menu = enumeratorMenu.AprobarIncorporacionRendimientos;

            

            var rendimientosIncorporados = _context.RendimientosIncorporados.Where(x =>
                            x.CarguePagosRendimientosId == uploadedOrderId && x.Consistente == true)
                .Include( r => r.CarguePagosRendimientos);

            if (rendimientosIncorporados.Count() == 0)
            {
                response.Data = 0; ;
                response.IsSuccessful = false;
                response.IsException = false;
                response.Code = GeneralCodes.EntradaInvalida;
                response.Message = await SaveAuditAction(_userName, actionId,
                                        menu,
                                        response.Code,
                                        actionMesage);
                return response;
            }

            var collection = rendimientosIncorporados.FirstOrDefault().CarguePagosRendimientos;
 
            var managedPerfomances = MapManagedPerformances(rendimientosIncorporados, collection.Json);

            foreach (var accountReturnOrder in managedPerfomances)
            {
                var bankAccount = _context.CuentaBancaria.Where(x => x.NumeroCuentaBanco == accountReturnOrder.AccountNumber)
                    .AsNoTracking().ToList<CuentaBancaria>().FirstOrDefault();

                if (bankAccount == null)
                {
                    new Exception("La cuenta asignada no existe");
                    response.IsValidation = true;
                    response.IsSuccessful = false;
                    response.Data = $"Cuenta bancaria No. {accountReturnOrder.AccountNumber} no encontrada";

                    response.Code = GeneralCodes.EntradaInvalida;
                    response.Message = await SaveAuditAction(_userName, actionId,
                                            menu,
                                            response.Code,
                                            actionMesage);
                    return response;
                }

                var gestionFuenteFinanciacionId = (int)bankAccount.FuenteFinanciacionId;

                decimal valorAIncorporar = accountReturnOrder.PerformancesToAdd;

                var gestionFuenteFinanciacion = new GestionFuenteFinanciacion();
                gestionFuenteFinanciacion.FuenteFinanciacionId = gestionFuenteFinanciacionId;
                gestionFuenteFinanciacion.ValorSolicitadoGenerado = valorAIncorporar;
                gestionFuenteFinanciacion.ValorSolicitado = 0;
                gestionFuenteFinanciacion.UsuarioCreacion = _userName;

                var sumValoresSolicitados = _context.GestionFuenteFinanciacion
                    .Where(x => !(bool)x.Eliminado && 
                                 x.FuenteFinanciacionId == gestionFuenteFinanciacion.FuenteFinanciacionId)
                    .Sum(x => x.ValorSolicitado);


                var sumValoresSolicitadosGenerado = _context.GestionFuenteFinanciacion
                    .Where(x => !(bool)x.Eliminado &&
                                 x.FuenteFinanciacionId == gestionFuenteFinanciacion.FuenteFinanciacionId &&
                                 x.ValorSolicitadoGenerado.HasValue)
                    .Sum(x => x.ValorSolicitadoGenerado.Value);

                var fuente = await _context.FuenteFinanciacion.FindAsync(gestionFuenteFinanciacion.FuenteFinanciacionId);
                gestionFuenteFinanciacion.SaldoActual = (decimal)fuente.ValorFuente - sumValoresSolicitados;
                gestionFuenteFinanciacion.SaldoActualGenerado = (decimal)fuente.ValorFuente - sumValoresSolicitadosGenerado;
                gestionFuenteFinanciacion.NuevoSaldo = gestionFuenteFinanciacion.SaldoActual;
                gestionFuenteFinanciacion.NuevoSaldoGenerado = gestionFuenteFinanciacion.SaldoActualGenerado + gestionFuenteFinanciacion.ValorSolicitadoGenerado;
                int estado = (int)EnumeratorEstadoGestionFuenteFinanciacion.Rendimientos;
                gestionFuenteFinanciacion.FechaCreacion = DateTime.Now;
                gestionFuenteFinanciacion.EstadoCodigo = estado.ToString();
                gestionFuenteFinanciacion.Eliminado = false;
                gestionFuenteFinanciacion.RendimientosIncorporadosId = rendimientosIncorporados.Where(
                    x => x.CuentaBancaria == bankAccount.NumeroCuentaBanco && x.CarguePagosRendimientosId == uploadedOrderId).FirstOrDefault().RendimientosIncorporadosId;

                 _context.GestionFuenteFinanciacion.Add(gestionFuenteFinanciacion);
            }

            await _context.Set<RendimientosIncorporados>()
                .Where(order => order.CarguePagosRendimientosId == uploadedOrderId)
                .UpdateAsync(o => new RendimientosIncorporados()
                {
                    Aprobado = true
                });
            int rowCount = await _context.SaveChangesAsync();

            response.Data = rendimientosIncorporados.Count(); ; 
            response.IsSuccessful = true;
            response.IsException = false;
            response.Code = GeneralCodes.OperacionExitosa;
            response.Message = await SaveAuditAction(_userName, actionId,
                                    menu,
                                    response.Code,
                                    actionMesage);
            return response;
        }

        public async Task<Respuesta> DownloadApprovedIncorporatedPerfomances(int uploadedOrderId)
        {
            Respuesta response = new Respuesta();
            var rendimientosIncorporados =(from performance in _context.RendimientosIncorporados
                                          where performance.CarguePagosRendimientosId == uploadedOrderId
                                          select  new ApprovedPerfomancesDto 
                                            {
                                                CuentaBancaria  = performance.CuentaBancaria,
                                                TotalRendimientosGenerados = performance.TotalRendimientosGenerados,
                                                Incorporados = performance.Incorporados,
                                                ProvisionGravamenFinanciero = performance.ProvisionGravamenFinanciero,
                                                TotalGastosBancarios = performance.TotalGastosBancarios,
                                                TotalGravamenFinancieroDescontado = performance.TotalGravamenFinancieroDescontado,
                                                Visitas = performance.Visitas,
                                                RendimientoIncorporar = performance.RendimientoIncorporar
                                            }).ToList();


        
            string directory = CheckFileDownloadDirectory();
            string filePath = WriteCollectionToPath("RendimientosTramitados", directory, rendimientosIncorporados, null);
            ////the path of the file
            string newfilePath = directory + "/" + "RendimientosTramitados" + "_rev.xlsx";

            response.Data = filePath;
            response.IsSuccessful = true;
            //response.Message = await SaveAuditAction(author, actionId,
            //                            menu,
            //                            GeneralCodes.OperacionExitosa,
            //                            actionMesage);

            return response;



            // TODO Review Payment concept..
            //    var accountPayments = _context.VCuentaBancariaPago.Where(acc => acc.NumeroCuentaBanco == accountOrder.AccountNumber);

            //    var visitas = accountPayments.Sum(v => v.ValorNetoGiro);
            //    var account = _context.CuentaBancaria.Where(x => x.NumeroCuentaBanco == accountOrder.AccountNumber).FirstOrDefault();


            //    if (account == null)
            //    {
            //        new Exception("La cuenta asignada no existe");
            //    }
            //    var fundingSources = await _context.FuenteFinanciacion.
            //        Where(r => !(bool)r.Eliminado && account.FuenteFinanciacionId == r.FuenteFinanciacionId).
            //        Include(r => r.Aportante).ToListAsync();

            //    foreach (var fundingSource in fundingSources)
            //    {
            //        fundingSource.ControlRecurso = _context.ControlRecurso
            //            .Where(x => x.FuenteFinanciacionId == fundingSource
            //            .FuenteFinanciacionId && !(bool)x.Eliminado).AsNoTracking().ToList();
            //        foreach (var control in fundingSource.ControlRecurso)
            //        {
            //            control.FuenteFinanciacion = null; //
            //        }

            //        foreach (var element in fundingSource.ControlRecurso)
            //        {
            //            valorAporteEnCuenta += element.ValorConsignacion;
            //        }
            //    }
        }

        static void RegisterSafeTypes<T>()
        {

            Type listType = typeof(T);
            var properties = listType.GetProperties();
            int index = 1;

            var safePropertyNames = listType.GetProperties()
                       .Select(p => p.Name)
                       .ToArray();

            foreach (var property in properties)
            {
               
                Console.WriteLine("Marking {0}.{1} as safe", property.Name, safePropertyNames);
            }
            DotLiquid.Template.RegisterSafeType(listType, safePropertyNames);
        }


        //        APORTANTE

        //SALDO ANTERIOR RENDIMIENTOS INCORPORADOS
        //RENDIMIENTOS A INCORPORADOS EN ESTE MES
        //NUEVO SALDO RENDIMIENTOS INCORPORADOS
        //Listado de aportantes de registros consistentes cargados en la funcionalidad "Gestionar rendimientos"
        //Se deben tomar del valor incorporado en el mes anterior

        //Registros consistentes de rendimientos incorporados cargados en la funcionalidad "Gestionar rendimientos"

        //Sumatoria de "Saldo anterior rendimientos incorporados" y "Rendimientos a incorporados en este mes"
        public async Task<byte[]> GenerateMinute(int uploadOrderId)
        {
            var workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var uploadOrder  = await _context.CarguePagosRendimientos.FindAsync(uploadOrderId);
            int nextVal = 0;
            if (!uploadOrder.NumeroActa.HasValue)
            {
                var p = new Microsoft.Data.SqlClient.SqlParameter("@result", System.Data.SqlDbType.Int);
                p.Direction = System.Data.ParameterDirection.Output;
                _context.Database.ExecuteSqlRaw("set @result = next value for ConsecutivoActaRendimientos", p);
                nextVal = (int)p.Value;

                var modifiedRows = await _context.Set<CarguePagosRendimientos>()
                   .Where(order => order.CargaPagosRendimientosId == uploadOrderId)
                   .UpdateAsync(o => new CarguePagosRendimientos()
                   {
                       FechaModificacion = DateTime.Now,
                       UsuarioModificacion = _userName,
                       NumeroActa = nextVal
                   });
            }
            else
            {
                nextVal = uploadOrder.NumeroActa.Value;
            }

            var report = await GenerarActaRendimientos(uploadOrder.FechaCargue.Month);
            decimal actual = report.Sum(x => x.Actual).HasValue ? report.Sum(x => x.Actual).Value: 0;
            decimal anterior = report.Sum(x => x.Anterior).HasValue ? report.Sum(x => x.Anterior).Value : 0;
            var image = ImageToBase64();
            // var templateFilePath = System.IO.Path.Combine(workingDirectory, @"Templates/PerformanceMinute.html");
            var minute = new MinuteTemplate
            {
                NextValue = FormatNextValue(nextVal),
                PerformancesDate = DateTime.Now.ToString(),
                Registers = report,
                Image = "data:image/png;base64," + image,
                Actual = actual,
                Anterior = anterior,
                Total = actual + anterior

            };
            string htmlTemplate= "";
            var dire = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var templateFilePath = string.Concat(workingDirectory, System.IO.Path.DirectorySeparatorChar, @"Templates", System.IO.Path.DirectorySeparatorChar, "PDF", System.IO.Path.DirectorySeparatorChar, @"PerformanceMinute.liquid");
            using (var templateStream = System.IO.File.OpenRead(templateFilePath))
            using (var renderDocument = new System.IO.MemoryStream())
            {
                using (var streamReader = new StreamReader(templateStream))
                {
                    var templateContent = streamReader.ReadToEnd();
                    var template = DotLiquid.Template.Parse(templateContent);
                    RegisterSafeTypes<MinuteTemplate>();
                    RegisterSafeTypes<DataResult>();
                    // DotLiquid.Template.RegisterSafeType(typeof(DataResult), new[] { "PerformancesDate" });
                    //  DotLiquid.Template templssate = DotLiquid.Template.Parse(template);  // Parses and compiles the template
                    htmlTemplate = template.Render(DotLiquid.Hash.FromAnonymousObject(new { minute = minute })); // Renders the output => "hi tobi";                }
                }
            }

            var plantilla = new Plantilla();
            plantilla.Contenido = htmlTemplate;
            return ConvertirPDF(plantilla);
        }


        public string FormatNextValue(int sequence)
        {
            string consecutivo = (sequence).ToString("0000");
            return consecutivo;
        }


        public string ImageToBase64()
        {
            var workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var templateFilePath = string.Concat(workingDirectory, System.IO.Path.DirectorySeparatorChar, @"Templates", Path.DirectorySeparatorChar, @"FFIE.png");

            using (Image image = Image.FromFile(templateFilePath))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }


        public async Task<List<DataResult>> GenerarActaRendimientos(int mesActual)
        {
            string idSequence = string.Empty;
            List<DataResult> resultSource = new List<DataResult>();
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@mesActual", mesActual));

                command.CommandText = "GenerarActaRendimientos";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                if (command.Connection.State != System.Data.ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                System.Data.Common.DbDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        var row = new DataResult
                        {
                            
                            Numero = reader.GetString(0),
                            Aportante = reader.GetString(1),
                            Actual = reader.GetDecimal(2),
                            Anterior = reader.GetDecimal(3),
                            Total = reader.GetDecimal(4)
                        };
                        resultSource.Add(row);
                    }
                }
                reader.Dispose();
            }
            return resultSource;
        }


        public async Task<Respuesta> UploadPerformanceUrlMinute(FileRequest minuteUrl)
        {
            var response = new Respuesta();

            string actionMesage = ConstantCommonMessages.Performances.CARGAR_ACTA_RENDIMIENTOS;
            int actionId = await GetActionIdAudit(ConstantCodigoAcciones.Generar_Acta_Rendimientos);  // TODO Registrar

      
            CarguePagosRendimientos carguePagosRendimientos = _context.CarguePagosRendimientos.Find(minuteUrl.ResourceId);
            try
            {
                var modifiedRows = await _context.Set<CarguePagosRendimientos>()
                     .Where(order => order.CargaPagosRendimientosId == minuteUrl.ResourceId)
                     .UpdateAsync(o => new CarguePagosRendimientos()
                     {
                         FechaModificacion = DateTime.Now,
                         FechaActa = DateTime.Now,
                         UsuarioModificacion = _userName,
                         RutaActa = minuteUrl.FileName
                     });

                string codeResponse = modifiedRows > 0 ? GeneralCodes.OperacionExitosa : ConstMessagesPerformances.ErrorGuardarCambios;

                response.Data = modifiedRows;
                response.IsSuccessful = true;
                response.IsException = false;
                response.Code = GeneralCodes.OperacionExitosa;
                response.Message = await SaveAuditAction(_userName, actionId,
                                         enumeratorMenu.AprobarIncorporacionRendimientos,
                                        response.Code,
                                        actionMesage);

                return response;

            }

            catch (Exception ex)
            {
                response.IsException = true;
                response.IsSuccessful = true;
                response.Code = GeneralCodes.OperacionExitosa;
                response.Message = await SaveAuditAction(_userName, actionId,
                                        enumeratorMenu.AprobarIncorporacionRendimientos,
                                        GeneralCodes.Error,
                                         ex.SubstringValid(_500));
            }

            return response;
        }

        public async Task<byte[]> GetPDFMinutes(int id, string usuarioModificacion)
        {
            if (id == 0)
            {
                return Array.Empty<byte>();
            }
          
            var plantilla = new Plantilla();
            //Plantilla plantilla = _context.Plantilla.Where(r => r.Codigo == ((int)ConstanCodigoPlantillas.Ficha_DRP).ToString()).Include(r => r.Encabezado).Include(r => r.PieDePagina).FirstOrDefault();
            //string contenido = await ReemplazarDatosPlantilla(plantilla.Contenido, false);
            plantilla.Contenido = "";
            return ConvertirPDF(plantilla);
        }

        public async Task<Respuesta> GetMinuteFromUrl(int uploadedOrderId)
        {
            var performancesOrder = await _context.CarguePagosRendimientos.FindAsync(uploadedOrderId);

            using (var client = new WebClient())
            {
                client.DownloadFile(performancesOrder.RutaActa, "a.mpeg");
            }

            return new Respuesta { Data = performancesOrder.RutaActa , IsSuccessful= true};
        }



    }

}
