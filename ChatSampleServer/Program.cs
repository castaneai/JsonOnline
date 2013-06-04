using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatSampleServer.Message;
using System.Net;

namespace ChatSampleServer
{
	class Program
	{
		static void Main(string[] args)
		{
			var server = new JsonOnline.Server();
			server.OnAccept += Server_OnAccept;
			server.Listen(IPAddress.Loopback, 8080);
		}

		static void Server_OnAccept(JsonOnline.Server server, JsonOnline.AcceptedClient client)
		{
			client.On<ChatMessage>((_, message) => {
				server.Broadcast(message);
			});
		}
	}
}
