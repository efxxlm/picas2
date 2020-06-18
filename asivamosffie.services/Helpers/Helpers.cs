using System;
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

        public static string passphrase = "D0tabasei+on012020%"; // base para cifrar las contrasenas

        //este algoritmo admite tamaños de bloque de 128, 192 o 256 bits; predeterminado a 128 bits ( compatible con Aes ).
        public static RijndaelManaged myRijndael = new RijndaelManaged(); // Esta clase implementa el algoritmo Rijndael

        private int iterations;
        private byte[] salt;

        // Faber Orozco
        public Helpers()
        {
            myRijndael.BlockSize = 128;
            myRijndael.KeySize = 256;
            myRijndael.IV = HexStringToByteArray("6eb1c8bd41c450d25fea2437a4266d02");
            myRijndael.Padding = PaddingMode.PKCS7;
            myRijndael.Mode = CipherMode.CBC;
            iterations = 10000;
            salt = System.Text.Encoding.UTF8.GetBytes("w%gUevZ!VWlt@(w4I(EPyd.3QapdVT&Z");
            myRijndael.Key = GenerateKey(passphrase);
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


        //Faber Orozco
        public static string EncryptPSW(string strPlainText)
        {
            byte[] strText = new System.Text.UTF8Encoding().GetBytes(strPlainText);
            ICryptoTransform transform = myRijndael.CreateEncryptor();
            byte[] cipherText = transform.TransformFinalBlock(strText, 0, strText.Length);
            return Convert.ToBase64String(cipherText);
        }

        //Faber Orozco
        public static string DecryptPSW(string encryptedText)
        {
            try
            {
                var encryptedBytes = Convert.FromBase64String(encryptedText);
                ICryptoTransform transform = myRijndael.CreateDecryptor();
                byte[] cipherText = transform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                return System.Text.Encoding.UTF8.GetString(cipherText);
            }
            catch (Exception)
            {
                return encryptedText;
            }
        }

        //Faber Orozco
        public static byte[] HexStringToByteArray(string strHex)
        {
            dynamic r = new byte[strHex.Length / 2];
            for (int i = 0; i <= strHex.Length - 1; i += 2)
            {
                r[i / 2] = Convert.ToByte(Convert.ToInt32(strHex.Substring(i, 2), 16));
            }
            return r;
        }

        //Faber Orozco
        private byte[] GenerateKey(string strPassword)
        {
            Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(System.Text.Encoding.UTF8.GetBytes(strPassword), salt, iterations);
            return rfc2898.GetBytes(256 / 8);
        }




    }
}
