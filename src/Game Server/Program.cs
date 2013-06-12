using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Game_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 3300); 
 
            server.Start();  // this will start the server
 
            ASCIIEncoding encoder = new ASCIIEncoding();   
            //we use this to transform the message(string) into a byte array, so we can send it
 
            while (true)   //we wait for a connection
            {
                TcpClient client = server.AcceptTcpClient();  //if a connection exists, the server will accept it
 
                NetworkStream ns = client.GetStream(); //we use a networkstream to send the message to the client
                byte[] hello = new byte[100];   //here we store our message as byte array
                hello = encoder.GetBytes("hello world");  //transforming the string to byte array...
                ns.Write(hello, 0, hello.Length);     //finally, sending the message
 
                while (client.Connected)  //while the client is connected, we check for incoming messages
                {
                    byte[] msg = new byte[1024];     //the messages arrive as byte array
                    ns.Read(msg, 0, msg.Length);   //we read the message send by the client
                    Console.WriteLine(encoder.GetString(msg).Trim(' ')); //now , we write the message as string
                }
            }
 
        }
    }
}
