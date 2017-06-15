using Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChatServer
{
    class Message : IMessage
    {
        public string Text { get ; set; }
        public MessageType Type { get; set; }
        public string Receiver { get ; set; }
    }
    class Program 
    {
        static Dictionary<TcpClient, String> Names = new Dictionary<TcpClient, string>();
        static List<TcpClient> allClients = new List<TcpClient>();

        static void Broadcast(string json)
        {
            foreach(var client in allClients)
            {
                var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
                {
                    writer.WriteLine(json);
                }
            }
        }

        static void myFunc()
        {

        }
        static void Main(string[] args)
        {
            try
            {
                Int32 port = 5002;
                IPAddress ipAddress = IPAddress.Any;
                Console.WriteLine("Starting TCP listener...");

                TcpListener listener = new TcpListener(ipAddress, port);

                listener.Start();
                Console.WriteLine("Server is listening on " + listener.LocalEndpoint);

                while (true)
                {

                    Console.WriteLine("Waiting for a connection...");

                    var client = listener.AcceptTcpClient();
                    allClients.Add(client);
                    var reader = new StreamReader(client.GetStream());


                    var name = reader.ReadLine();
                    Names.Add(client, name);
                    // decode (JSON)



                    Console.WriteLine("Connection accepted.");

                    var t = new Thread(async () =>
                     {

                         while (true)
                         {
                             var line = await reader.ReadLineAsync();
                             IMessage msg = JsonConvert.DeserializeObject<Message>(line);
                             if (msg.Type == MessageType.Broadcast)
                             {
                                 Broadcast(line);
                             }
                             else if (msg.Type == MessageType.Unicast)
                             {
                                 if (msg.Receiver != null)
                                 {
                                     var myClient = Names.FirstOrDefault(x => x.Value == msg.Receiver).Key;
                                     if (myClient != null)
                                     {
                                         var writer = new StreamWriter(myClient.GetStream()) { AutoFlush = true };
                                         {
                                             writer.WriteLine(line);
                                         }
                                     }

                                 }
                                 else
                                 {
                                     var myClient = Names.FirstOrDefault(x => x.Value == msg.Receiver).Key;
                                     var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
                                     {

                                         // writer.WriteLine("Sorry your reciever doesn't exist");
                                     }
                                 }

                                 // decode (JSON)
                                // Broadcast(line);
                             }

                         }
                     });
                    t.Start();

                    Console.WriteLine();
                }

                listener.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.StackTrace);
                Console.ReadLine();
            }
        }
    }
}

