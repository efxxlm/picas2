﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;

namespace asivamosffie.services.Helpers
{
   public class Helpers
    {

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

        public static bool EnviarCorreo(string destinatario, string asunto, string mensajeHtml)
        {
            try
            {
                string strCorreo = "noreply_ffie@ivolucion";
                string strClaveCorreo = "Bogota2020";
                string strSmtpServerV = "mail.ivolucion.com";
                int intSmtpPort = 26;

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(strSmtpServerV);

                mail.From = new MailAddress(strCorreo);
                mail.To.Add(destinatario);
                mail.Subject = asunto;
                mail.IsBodyHtml = true;
       
                
                mail.Body = mensajeHtml;
                SmtpServer.Port = intSmtpPort;
                SmtpServer.Credentials = new NetworkCredential(strCorreo, strClaveCorreo);
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







    }
}
