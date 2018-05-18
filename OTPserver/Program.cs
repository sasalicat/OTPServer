using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
//INSERT INTO `default` (`account`, `key`) VALUES ('asswecan', '948794');添加的语法
namespace OTPserver
{
    class Program
    {
        static void Main(string[] args)
        {

            double seconds = DateTime.UtcNow.Subtract(DateTime.Parse("1970-1-1")).TotalSeconds;
            Console.WriteLine("second:"+seconds);
            Server serverUnit = new Server();
            serverUnit.ListenToConnection();
            /* DataBaseUnit dbunit = new DataBaseUnit();
             int key = dbunit.getKey("啊,卖骚的");
             if (key != -1)
             {
                 Console.Write(key);
             }
             else
             {
                 Console.Write("没找到");
             }
             Console.ReadLine();*/

            /*PacketSlice slice = new PacketSlice();
            slice.useDefaultDict();
            byte[] temp= slice.buildBytes(0, "948794狂");
            byte[] sum = new byte[temp.Length * 3];
            for(int i = 0; i < 3; i++)
            {
                temp.CopyTo(sum, i * temp.Length);
            }
            List<PacketSlice.Order> orders= slice.SlicePack(sum);
            Console.WriteLine("\n orders len:" + orders.Count);
            foreach(PacketSlice.Order order in orders)
            {
                Console.WriteLine(">order:" + order.order + " data:" + order.data);
            }
            Console.ReadLine();*/
        }
    }
}
