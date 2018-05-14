using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTPserver
{
    class ToBytes
    {
        public static byte[] toBytes(string data)
        {
            char[] temp = data.ToCharArray();
            byte[] ans = new byte[temp.Length*2];
            for (int i = 0; i < temp.Length; i++)
            {
                BitConverter.GetBytes(temp[i]).CopyTo(ans, i * 2);//因为char转化为2个byte所以要*2来放在对的位置
            }
            return ans;
        }
        public static byte[] toBytes(int data)
        {
            return BitConverter.GetBytes(data);
        }
        public static string getString(byte[] list,int startIndex,int byteLength)
        {
            char[] chars = new char[byteLength/2];
            for(int i = startIndex; i < byteLength+startIndex; i += 2)
            {
                chars[(i-startIndex) / 2] = BitConverter.ToChar(list,i);
            }
            return new string(chars);
        }
        public static int getInt(byte[] list,int startIndex)
        {
            return BitConverter.ToInt32(list, startIndex);
        }
    }
}
