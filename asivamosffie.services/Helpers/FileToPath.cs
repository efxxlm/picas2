using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using asivamosffie.model.APIModels;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace asivamosffie.services.Helpers
{
    public class RandomNumber
    {
        private static readonly Random random = new Random();
        private static readonly object synLock = new object();
        public static int GetRandomNumber(int min, int max)
        {
            lock (synLock)
            {
                return random.Next(min, max);
            }
        }
    }

    public static class FileToPath
    {

        public static string WriteCollectionToPath<T>(string fileName, string directory, List<T> list, List<ExcelError> errors)
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
                    var typeAttribute = (FieldTypeAttribute)Attribute.GetCustomAttribute(property, typeof(FieldTypeAttribute));
                    if (customAtt != null)
                    {
                        worksheet.Cells[1, index].Value = customAtt.PropertyName;
                        // worksheet.Column(index).CellsUsed().SetDataType(XLDataType.Number);
                        if (property.PropertyType.Name == "Decimal" || typeAttribute?.Name == "Decimal")
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
                int randomVal = RandomNumber.GetRandomNumber(0, 50);

                var xlFile = new FileInfo(directory + Path.DirectorySeparatorChar + listType.Name + randomVal.ToString() + ".xlsx");

                // Save the workbook in the output directory
                packages.SaveAs(xlFile);
                streams.Close();
                return xlFile.FullName;
            }

        }
    }
}
