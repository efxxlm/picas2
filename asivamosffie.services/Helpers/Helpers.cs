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

namespace asivamosffie.services.Helpers
{
    public class Helpers
    {
        private readonly devAsiVamosFFIEContext _context;

        public Helpers(devAsiVamosFFIEContext context)
        {
            _context = context;
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


        public static object ConvertToUpercase(object dataObject)
        {
            try
            {
                object newObject = new System.Dynamic.ExpandoObject();
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(dataObject))
                {
                    string name = descriptor.Name;
                    object value = descriptor.GetValue(dataObject);
                    ((IDictionary<String, Object>)newObject).Add(name, value = (descriptor.PropertyType.Name == "String" ? Convert.ToString(value).ToUpper() : value));
                   
                }

                return newObject; //Newtonsoft.Json.JsonConvert.SerializeObject();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public static bool EnviarCorreo(string pDestinatario, string pAsunto, string pMensajeHtml ,string pCorreoLocal ,string pPassword, string pStrSmtpServerV ,int pSmtpPort)
        {
            try
            { 
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(pStrSmtpServerV);

                mail.From = new MailAddress(pCorreoLocal);
                mail.To.Add(pDestinatario);
                mail.Subject = pAsunto;
                mail.IsBodyHtml = true;
        
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

        public string getMessageByCode(string pCode, enumeratorMenu pMenuId)
        {
            string message = "";

            MensajesValidaciones prueba = _context.MensajesValidaciones.Where(m => m.Codigo == pCode && m.MenuId == (int)pMenuId).SingleOrDefault(); 
            if (prueba == null)
                message = string.Concat(pCode,": mensaje no parametrizado");           
            else
                message = prueba.Mensaje;           

            return message;
        }
 
       
    }
}
