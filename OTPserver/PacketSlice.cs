using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OTPserver
{

    public class PacketSlice
    {
        public const sbyte STRING = 0;
        public const sbyte INT = 1;
        //以上是类别转换指示物
        public class Order
        {
            public byte order;
            public object data;
            public Order(byte order,object data)
            {
                this.order = order;
                this.data = data;
            }
        }
        public Dictionary<byte, sbyte> TypeDict = new Dictionary<byte, sbyte>();//用来指定封包的data部分类别目前只有int 和String
        public void useDefaultDict()
        {
            TypeDict = new Dictionary<byte, sbyte>();
            TypeDict[0] = STRING;//意思是0号指令的data是string形态的
            TypeDict[1] = INT;
        }
        public byte[] buildBytes(byte order, string data)
        {
            byte[] s_part= ToBytes.toBytes(data);//string资料部分
            char len = (char)(s_part.Length);
            byte[] header = BitConverter.GetBytes(len);
            byte[] o_part = new byte[] { order };//order部分
            byte[] final = new byte[s_part.Length+header.Length+o_part.Length];
            Console.WriteLine("final len:"+final.Length);
            header.CopyTo(final,0);
            o_part.CopyTo(final, header.Length);
            s_part.CopyTo(final, o_part.Length + header.Length);
            return final;
        }
        public byte[] buildBytes(byte order, int data)
        {
            byte[] s_part = ToBytes.toBytes(data);//int资料部分
            char len = (char)(s_part.Length);
            byte[] header = BitConverter.GetBytes(len);//header代表这个封包有多长(不包括header本身),这个存在的原因是防止粘包
            Console.WriteLine("header Len:" + header.Length);
            byte[] o_part = new byte[] { order };//order部分
            byte[] final = new byte[s_part.Length + header.Length + o_part.Length];
            Console.WriteLine("final len:" + final.Length);
            header.CopyTo(final, 0);
            o_part.CopyTo(final, header.Length);
            s_part.CopyTo(final, o_part.Length + header.Length);
            Console.Write("在bulid byte 结束,封包长度为:" + final.Length);
            return final;
        }
        public List<Order> SlicePack(byte[] buff)
        {
            Console.WriteLine("在SlicePack中 buff len:" + buff.Length);
            int nowZero = 0;//这个是目前的封包起始索引,每次回圈推后索引,直到到达尾部为止
            List<Order> final = new List<Order>();
            while (buff.Length-nowZero>=3) {

                int len = BitConverter.ToChar(buff,nowZero);
                byte order = buff[nowZero+2];
                Console.WriteLine("Order 为:" + order);
                Order newo=null;
                switch (TypeDict[order])
                {
                    case STRING:
                        {
                            newo = new Order(order, ToBytes.getString(buff, nowZero+ 3, len));
                            break;
                        }
                    case INT:
                        {
                            newo = new Order(order, ToBytes.getInt(buff, nowZero + 3));
                            break;
                        }
                }
                final.Add(newo);
                nowZero += 3 + len;
            }
            return final;
            
        }
        public List<Order> SlicePack(byte[] buff,int validLength)
        {
            int nowZero = 0;//这个是目前的封包起始索引,每次回圈推后索引,直到到达尾部为止
            List<Order> final = new List<Order>();
            while (validLength - nowZero >= 3)
            {

                int len = BitConverter.ToChar(buff, nowZero);
                byte order = buff[nowZero + 2];
                Console.WriteLine("Order 为:" + order);
                Order newo = null;
                switch (TypeDict[order])
                {
                    case STRING:
                        {
                            newo = new Order(order, ToBytes.getString(buff, nowZero + 3, len));
                            break;
                        }
                    case INT:
                        {
                            newo = new Order(order, ToBytes.getInt(buff, nowZero + 3));
                            break;
                        }
                }
                final.Add(newo);
                nowZero += 3 + len;
            }
            return final;

        }
    }
}
