using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


namespace OTPserver
{
    class hashReduceNum
    {

        public static int getNumber(int key1,int key2)
        {
            byte[] b_key1 = BitConverter.GetBytes(key1);
            HMACSHA1 hash = new HMACSHA1(b_key1);
            byte[] b_key2 = BitConverter.GetBytes(key2);
            byte[] b_ans = hash.ComputeHash(b_key2);
            byte[] b_int = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                b_int[i] = b_ans[i];
            }
            return Math.Abs(BitConverter.ToInt32(b_int,0));
        }
    }
}
