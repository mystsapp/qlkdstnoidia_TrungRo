using System;
using System.Security.Cryptography;
using System.Text;

namespace qlkdstDB.DAO
{
    public class MaHoaSHA1
    {
        public string EncodeSHA1(string pass)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(pass);
            bs = sha1.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x1").ToUpper());
            }
            pass = s.ToString();
            return pass;
        }

        public string decodeSHA1(string pass)
        {            
            SHA1 sha = new SHA1CryptoServiceProvider();
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] combined = encoder.GetBytes(pass);
            string hash = BitConverter.ToString(sha.ComputeHash(combined)).Replace("-", "");
            return hash;
        }
    }
}

