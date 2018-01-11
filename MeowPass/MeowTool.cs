﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MeowPass
{
    class MeowTool
    {
        private static byte[] Iv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        private static byte[] Iv16 = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        public static string MyMD5Crypto(string str)
        {
            byte[] md5Byte = new MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(str.Trim()));
            return BitConverter.ToString(md5Byte).Replace("-", "");
        }
        public static string MySHACrypto(string str)
        {
            byte[] shaByte = new SHA1CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(str.Trim()));
            return BitConverter.ToString(shaByte).Replace("-", "");
        }
        public static string MyDESCrypto(string str, string key)
        {
            string encryptKeyall = Convert.ToString(key);
            if (encryptKeyall.Length < 9)
            {
                while (!(encryptKeyall.Length < 9))
                {
                    encryptKeyall += encryptKeyall;
                }
            }
            string encryptKey = encryptKeyall.Substring(0, 8);
            byte[] strs = Encoding.Unicode.GetBytes(str);
            byte[] keys = Encoding.UTF8.GetBytes(encryptKey); ;

            DESCryptoServiceProvider desC = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();

            ICryptoTransform cryp = desC.CreateEncryptor(keys, Iv);
            return Convert.ToBase64String(cryp.TransformFinalBlock(strs, 0, strs.Length));
        }
        public static string MyTripleDESCrypto(string str, string key)
        {
            string encryptKeyall = Convert.ToString(key);
            if (encryptKeyall.Length < 17)
            {
                while (!(encryptKeyall.Length < 17))
                {
                    encryptKeyall += encryptKeyall;
                }
            }
            string encryptKey = encryptKeyall.Substring(0, 16);
            byte[] strs = Encoding.Unicode.GetBytes(str);
            byte[] keys = Encoding.ASCII.GetBytes(encryptKey); ;

            TripleDESCryptoServiceProvider tdesC = new TripleDESCryptoServiceProvider();
            tdesC.Key = keys;//key的长度必须为16位或24位，否则报错“指定键的大小对于此算法无效。”
            tdesC.Mode = CipherMode.ECB;
            ICryptoTransform cryp = tdesC.CreateEncryptor();

            return Convert.ToBase64String(cryp.TransformFinalBlock(strs, 0, strs.Length));
        }
        public static string MyAESCrypto(string str, string key)
        {
            string encryptKeyall = Convert.ToString(key);
            if (encryptKeyall.Length < 33)
            {
                while (!(encryptKeyall.Length < 33))
                {
                    encryptKeyall += encryptKeyall;
                }
            }
            string encryptKey = encryptKeyall.Substring(0, 32);

            SymmetricAlgorithm aesC = Rijndael.Create();
            byte[] strs = Encoding.UTF8.GetBytes(str);
            aesC.Key = Encoding.UTF8.GetBytes(encryptKey);
            aesC.IV = Iv16;
            byte[] cipherBytes = null;
            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream, aesC.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cStream.Write(strs, 0, strs.Length);
                    cStream.FlushFinalBlock();
                    cipherBytes = mStream.ToArray();
                    cStream.Close();
                    mStream.Close();
                }
            }
            return Convert.ToBase64String(cipherBytes);
        }
        public static string MyRC2Crypto(string str, string key)
        {
            string encryptKeyall = Convert.ToString(key);
            if (encryptKeyall.Length < 9)
            {
                while (!(encryptKeyall.Length < 9))
                {
                    encryptKeyall += encryptKeyall;
                }
            }
            string encryptKey = encryptKeyall.Substring(0, 8);
            byte[] strs = Encoding.Unicode.GetBytes(str);
            byte[] keys = Encoding.UTF8.GetBytes(encryptKey); ;

            RC2CryptoServiceProvider rc2C = new RC2CryptoServiceProvider();
            rc2C.Key = keys;
            rc2C.IV = Iv;
            ICryptoTransform cryp = rc2C.CreateEncryptor();

            return Convert.ToBase64String(cryp.TransformFinalBlock(strs, 0, strs.Length));
        }
    }
}