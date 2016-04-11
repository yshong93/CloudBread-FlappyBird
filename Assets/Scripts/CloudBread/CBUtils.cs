using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Assets.Scripts.CloudBread
{
    class CBUtils
    {
        public static string CBEncoding(string str)
        {
            // utf-8 인코딩
            byte[] bytesForEncoding = Encoding.UTF8.GetBytes(str);
            string encodedString = Convert.ToBase64String(bytesForEncoding);
            

            // utf-8 디코딩
            byte[] decodedBytes = Convert.FromBase64String(encodedString);
            string decodedString = Encoding.UTF8.GetString(decodedBytes);

            return decodedString;
        }
    }
}
