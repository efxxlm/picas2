using DocumentFormat.OpenXml.Presentation;
using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.services.Helpers.Extensions
{
    public static class PrimitiveExtension
    {
        public static bool IsValidSize(this string strCellValue, int size)
        {
            return strCellValue.Length <= size;
        }

        public static bool IsMoneyValueValidSize(this string value, int size)
        {
            string cellValue = value.Replace(".", "").Replace("$", "");
            bool isValid = cellValue.IsValidSize(size) && cellValue.IsMoney();
            return isValid;
        }

        public static bool IsMoney(this string value)
        {
            string cellValue = value.Replace(".", "").Replace("$", "");
            bool isValid  = decimal.TryParse(cellValue, out decimal respValNumber);
            return isValid;
        }

        public static string SubstringValid(this Exception exception, int lenght)
        {
            if(exception == null)
            {
                return string.Empty;
            }
            else if(exception.InnerException == null)
            {
                return exception.Message;
            }
            string str = exception.ToString();
            if (str.Length >= lenght)
            {
                return str.Substring(0, 500);
            }
            return str;
        }
    }
}
