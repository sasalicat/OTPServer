using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace OTPserver
{
    class TransModule
    {
        private TcpClient client;
        private NetworkStream stream;
        private PacketSlice slicer;
        public TransModule(TcpClient client,PacketSlice slice)
        {
            this.client = client;
            this.stream = client.GetStream();
            this.slicer = slice;
            
        }
        public void sendOrder(byte orderNo,object data)
        {
            Console.Write("data type is:"+data.GetType());
            if(data.GetType()==Type.GetType("System.Int32"))
            {
                byte[] packet = slicer.buildBytes(orderNo, (int)data);
                stream.Write(packet,0,packet.Length); 
            }
            
        }
        public List<PacketSlice.Order> waitForOrder()
        {
            int numberOfBytesRead = 0;
            byte[] receiveBytes = new byte[client.ReceiveBufferSize];
            do
            {
                numberOfBytesRead = stream.Read(receiveBytes, 0, client.ReceiveBufferSize);
            } while (stream.DataAvailable);
            return slicer.SlicePack(receiveBytes, numberOfBytesRead);
        }
    }
}
