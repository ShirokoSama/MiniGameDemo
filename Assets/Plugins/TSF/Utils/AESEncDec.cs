//————————————————————————————————————————————
//  AESEncDec.cs
//  For project: TooSimple Framework
//
//  Created by Chiyu Ren on 2016-07-10 11:13
//————————————————————————————————————————————

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;


namespace TooSimpleFramework.Utils
{
    /// <summary>
    /// AES加密解密器
    /// </summary>
    public class AESEncDec
    {
        public static byte[] Encrypt(byte[] pInputData, string pKey)
        {
            var keyArray = Encoding.ASCII.GetBytes(pKey);

            var rm = new RijndaelManaged();
            rm.Key = keyArray;
            rm.Mode = CipherMode.ECB;
            rm.Padding = PaddingMode.PKCS7;

            return rm.CreateEncryptor().TransformFinalBlock(pInputData, 0, pInputData.Length);
        }

        public static string Encrypt(string pInputData, string pKey)
        {
            var bytes = Encoding.UTF8.GetBytes(pInputData);
            bytes = Encrypt(bytes, pKey);
            return Encoding.UTF8.GetString(bytes);
        }

        public static byte[] Decrypt(byte[] pInputData, string pKey)
        {  
            var keyArray = Encoding.ASCII.GetBytes(pKey);

            var rm = new RijndaelManaged();
            rm.Key = keyArray;
            rm.Mode = CipherMode.ECB;
            rm.Padding = PaddingMode.PKCS7;

            return rm.CreateDecryptor().TransformFinalBlock(pInputData, 0, pInputData.Length);
        }

        public static string Decrypt(string pInputData, string pKey) 
        {
            var bytes = Encoding.UTF8.GetBytes(pInputData);
            bytes = Decrypt(bytes, pKey);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
