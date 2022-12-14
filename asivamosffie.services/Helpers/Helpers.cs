using System;
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
using System; 
using System.IO;
using asivamosffie.model.APIModels;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
 
namespace asivamosffie.services.Helpers
{
    public class Helpers
    {
        private readonly devAsiVamosFFIEContext _context;

        public Helpers(devAsiVamosFFIEContext context)
        {
            _context = context;
        }
         
        public static string HtmlConvertirTextoPlano(string origen)
        {
            DocumentoHtml documento = new DocumentoHtml();
            origen = documento.ConvertirATextoPlano(origen);
            return origen.Replace("<", "")
                         .Replace(">", "")
                         .Replace("/", "")
                         .Replace("\\", "")
                         .Replace("[", "")
                         .Replace("]", "")
                         .Replace("{", "")
                         .Replace("}", "")
                         .Replace("\n", "")
                         .Replace("\r", "");
        }

        public static string HtmlStringLimpio(string valor)
        {
            valor = Regex.Replace(valor, @"\t|\n|\r", "");
            return HtmlConvertirTextoPlano(valor);
        }

        public static string HtmlEntities(string valor)
        {
            valor = valor.Replace("á", "&aacute;")
                .Replace("é", "&eacute;")
                .Replace("í", "&iacute;")
                .Replace("ó", "&oacute;")
                .Replace("ú", "&uacute;")
                .Replace("ñ", "&ntilde;")
                .Replace("Á", "&Aacute;")
                .Replace("É", "&Eacute;")
                .Replace("Í", "&Iacute;")
                .Replace("Ó", "&Oacute;")
                .Replace("Ó", "&Uacute;")
                .Replace("Ñ", "&Ntilde;")
                ;
            return valor;
        }

        public double CentimetrosAMedidaPDF(double centimetros)
        {
            return (double)(centimetros * 0.393701 * 72);
        }

        public static string encryptSha1(string password)
        {


            UTF8Encoding enc = new UTF8Encoding();
            byte[] data = enc.GetBytes(password);
            byte[] result;

            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();

            result = sha.ComputeHash(data);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {

                // Convertimos los valores en hexadecimal
                // cuando tiene una cifra hay que rellenarlo con cero
                // para que siempre ocupen dos dígitos.
                if (result[i] < 16)
                {
                    sb.Append("0");
                }
                sb.Append(result[i].ToString("x"));
            }
            return sb.ToString().ToUpper();
        }

        public static string CleanStringInput(string text)//ÁÉÍÓÚ //
        {

            char[] replacement = { 'a', 'a', 'a', 'a', 'a', 'a', 'c', 'e', 'e', 'e', 'e', 'i', 'i', 'i', 'i', 'n', 'o', 'o', 'o', 'o', 'o', 'u', 'u', 'u', 'u', 'y', 'y', ' ', ' ', ' ', ' ', ' ', /*' ',*/ ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };
            char[] accents = { 'à', 'á', 'â', 'ã', 'ä', 'å', 'ç', 'é', 'è', 'ê', 'ë', 'ì', 'í', 'î', 'ï', 'ñ', 'ò', 'ó', 'ô', 'ö', 'õ', 'ù', 'ú', 'û', 'ü', 'ý', 'ÿ', 'Á', 'É', 'Í', 'Ó', 'Ú', /*'/',*/ '.', ',', '@', '_', '(', ')', ':', ';' };

            if (text != null)
            {
                for (int i = 0; i < accents.Length; i++)
                {
                    text = text.Replace(accents[i], replacement[i]);
                }
            }

            return text;
        }

        public static string Consecutive(string input, int countMax)
        {
            //("D4") indica la cantidad de ceros a la izquierda (0001) ver mas => https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
            var number = Convert.ToInt32(countMax);

            //Seleccion privada SP
            if (input == "1")
            {
                return $"{"SP"}{(++number).ToString("D4")}-{DateTime.Now.ToString("yyyy")}";
            }

            if (input == "DE")
            {
                return $"{"DE_"}{(++number).ToString("D4")}";
            }

            //Comite Fiduciario
            if (input == "CF")
            {
                return $"{"CF_"}{(++number).ToString("D5")}";
            }
            if (input == "PA")
            {
                return $"{"PA_"}{(++number).ToString("D4")}";
            }

            //Invitacion Cerrada SC
            else if (input == "2")
            {
                return $"{"SC"}{(++number).ToString("D4")}-{DateTime.Now.ToString("yyyy")}";
            }

            //Concecutivo Proyecto Administrativo
            if (input == "D4")
            {
                return $"{(number).ToString("D4")}";
            }

            //Concecutivo actualizacion de conograma proceso de seleccion 3.1.3
            if (input == "ACTCRONO")
            {
                return $"{"ACTCRONO"}{(number).ToString("D4")}";
            }
            //DefensaJudicial
            if (input == "DJ")
            {
                return $"{"DJ"}-{(++number).ToString("D4")}-{DateTime.Now.ToString("yyyy")}";
            }

            //Invitacion Abierta SA
            else
            {
                return $"{"SA"}{(++number).ToString("D4")}-{DateTime.Now.ToString("yyyy")}";
            }

            
        }

        //TODO: Implementacion para cosultas complejas
        public static List<T> ExecuteQuery<T>(string query) where T : class, new()
        {
            devAsiVamosFFIEContext _context = new devAsiVamosFFIEContext();
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                _context.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                {
                    var lst = new List<T>();
                    var lstColumns = new T().GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
                    while (reader.Read())
                    {
                        var newObject = new T();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var name = reader.GetName(i);
                            PropertyInfo prop = lstColumns.FirstOrDefault(a => a.Name.ToLower().Equals(name.ToLower()));
                            if (prop == null)
                            {
                                continue;
                            }
                            var val = reader.IsDBNull(i) ? null : reader[i];
                            prop.SetValue(newObject, val, null);
                        }
                        lst.Add(newObject);
                    }

                    return lst;
                }
            }
        }

        public static object ConvertToUpercase(object dataObject)
        {
            try
            {
                object newObject = new System.Dynamic.ExpandoObject();
                char[] replacement = { 'a', 'a', 'a', 'a', 'a', 'a', 'c', 'e', 'e', 'e', 'e', 'i', 'i', 'i', 'i', 'n', 'o', 'o', 'o', 'o', 'o', 'u', 'u', 'u', 'u', 'y', 'y', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };
                char[] accents = { 'à', 'á', 'â', 'ã', 'ä', 'å', 'ç', 'é', 'è', 'ê', 'ë', 'ì', 'í', 'î', 'ï', 'ñ', 'ò', 'ó', 'ô', 'ö', 'õ', 'ù', 'ú', 'û', 'ü', 'ý', 'ÿ', 'Á', 'É', 'Í', 'Ó', 'Ú', '/', '.', ',', '@', '_', '(', ')', ':', ';' };

                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(dataObject))
                {

                    string name = descriptor.Name;
                    object value = descriptor.GetValue(dataObject);
                    for (int i = 0; i < accents.Length; i++)
                    {
                        ((IDictionary<String, Object>)newObject).Add(name,
                         value =
                         (
                             descriptor.PropertyType.Name == "String" && value.ToString() != null ?
                             Convert.ToString(value.ToString().Replace(accents[i], replacement[i])).ToUpper() : value
                         ));
                    }
                }


                return newObject; //Newtonsoft.Json.JsonConvert.SerializeObject();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public static bool EnviarCorreo(string pDestinatario, string pAsunto, string pMensajeHtml ,string pCorreoLocal ,string pPassword, string pStrSmtpServerV ,int pSmtpPort, bool pMailHighPriority=false, string pFileNamePath="")
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(pStrSmtpServerV);

                mail.From = new MailAddress(pCorreoLocal);
                mail.To.Add(pDestinatario);
                mail.Subject = pAsunto;
                mail.IsBodyHtml = true;

                Attachment item;
                if(pFileNamePath != "")
                {
                    item = new Attachment(pFileNamePath);
                    mail.Attachments.Add(item);
                }                    

                if(pMailHighPriority)
                    mail.Priority = MailPriority.High;
                mail.Body = pMensajeHtml;
                SmtpServer.Port = pSmtpPort;
                SmtpServer.Credentials = new NetworkCredential(pCorreoLocal, pPassword);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public static bool EnviarCorreoMultipleDestinatario(List<string> pDestinatario, string pAsunto, string pMensajeHtml, string pCorreoLocal, string pPassword, string pStrSmtpServerV, int pSmtpPort, bool pMailHighPriority = false, string pFileNamePath = "")
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(pStrSmtpServerV);

                mail.From = new MailAddress(pCorreoLocal);
                foreach(var destinatario in pDestinatario)
                {
                    mail.To.Add(destinatario);
                }
                
                mail.Subject = pAsunto;
                mail.IsBodyHtml = true;

                Attachment item;
                if (pFileNamePath != "")
                {
                    item = new Attachment(pFileNamePath);
                    mail.Attachments.Add(item);
                }

                if (pMailHighPriority)
                    mail.Priority = MailPriority.High;
                mail.Body = pMensajeHtml;
                SmtpServer.Port = pSmtpPort;
                SmtpServer.Credentials = new NetworkCredential(pCorreoLocal, pPassword);
                SmtpServer.EnableSsl = false;
                SmtpServer.Send(mail);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public static string GeneratePassword(bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial, bool includeSpaces, int lengthOfPassword)
        {
            const int MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS = 2;
            const string LOWERCASE_CHARACTERS = "abcdefghijklmnopqrstuvwxyz";
            const string UPPERCASE_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string NUMERIC_CHARACTERS = "0123456789";
            const string SPECIAL_CHARACTERS = @"!#$%*\";
            const string SPACE_CHARACTER = " ";
            const int PASSWORD_LENGTH_MIN = 8;
            const int PASSWORD_LENGTH_MAX = 128;

            if (lengthOfPassword < PASSWORD_LENGTH_MIN || lengthOfPassword > PASSWORD_LENGTH_MAX)
            {
                return "Password length must be between 8 and 128.";
            }

            string characterSet = "";

            if (includeLowercase)
            {
                characterSet += LOWERCASE_CHARACTERS;
            }

            if (includeUppercase)
            {
                characterSet += UPPERCASE_CHARACTERS;
            }

            if (includeNumeric)
            {
                characterSet += NUMERIC_CHARACTERS;
            }

            if (includeSpecial)
            {
                characterSet += SPECIAL_CHARACTERS;
            }

            if (includeSpaces)
            {
                characterSet += SPACE_CHARACTER;
            }

            char[] password = new char[lengthOfPassword];
            int characterSetLength = characterSet.Length;

            System.Random random = new System.Random();
            for (int characterPosition = 0; characterPosition < lengthOfPassword; characterPosition++)
            {
                password[characterPosition] = characterSet[random.Next(characterSetLength - 1)];
            }
            Random randomw = new Random();
            string def = "Az-" + randomw.Next(5).ToString();
            return string.Join(null, password) + def;
        }

        public static bool TryStringToDate(string sDate, out DateTime newDate)
        {
            newDate = DateTime.MinValue;
            string[] dateElements = sDate.Split('/');

            if (dateElements.Length == 0 || dateElements.Length < 3)
            {
                return false;
            }

            if (int.TryParse(dateElements[2], out int year) &&
                int.TryParse(dateElements[1], out int month) &&
                int.TryParse(dateElements[0], out int day))
            {
                newDate = new DateTime(year, month, day);
                return true;
            }
            return false;
        }

    }
}
