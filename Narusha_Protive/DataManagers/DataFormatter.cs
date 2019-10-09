using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Narusha_Protive.DataManagers
{
    class DataFormatter
    {
        public static String Encrypt512(string s)
        {
            SHA512 sha = new SHA512Managed();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(s));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in hash) builder.AppendFormat("{0:x2}", b);
            return builder.ToString();
        }
        public static String toDate(string s)
        {
            //yyyymmddhhmm
            return s.Substring(0, 4) + " / " + s.Substring(4, 2) + " / " + s.Substring(6, 2) + " ( " + s.Substring(8, 2) + " : " + s.Substring(10, 2) + " ) ";
        }
    }
}
