using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Example
{
    class Program
    {
        class ChatMessage {
            public string Name { get; set; }
            public string Message { get; set; }
        }

        static void Main(string[] args)
        {
            Task.Run(() => ServerMain());
            ClientMain();
            Console.ReadKey();
        }

        static void ServerMain()
        {
            var server = new JsonOnline.Server();

            server.OnAccept += (_server, client) => {
                client.On<ChatMessage>((__server, chatMessage) => {
                    server.Broadcast(chatMessage);
                });
            };

            server.Listen(IPAddress.Any, 8080);
        }

        static void ClientMain()
        {
            var client = new JsonOnline.Client();
            client.On<ChatMessage>((_client, msg) => {
                Console.WriteLine("{0} says: {1}", msg.Name, msg.Message);
            });

            client.Connect(IPAddress.Loopback, 8080);
            
            var message = new ChatMessage { Name = "Yasuna", Message = "Baby, kill me please!" };
            client.Write(message);
        }
    }
}
