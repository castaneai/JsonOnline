using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace JsonOnlineTestCUI
{
	public class Message : JsonOnline.INetworkMessage
	{
		public string Name { get; set; }
		public string Content { get; set; }
	}

	public class ColorInfo : JsonOnline.INetworkMessage
	{
		public System.Drawing.Color Color { get; set; }
	}

	class Program
	{
		static IPEndPoint Ep = new IPEndPoint(IPAddress.Loopback, 10800);

		static void Main(string[] args)
		{
			Server();

			var client = new JsonOnline.Client();
			client.On<Message>((_, msg) => {
				Console.WriteLine("[client]received {0}: {1}", msg.Name, msg.Content);
			});
			client.Connect(Ep);

			var client2 = new JsonOnline.Client();
			client2.On<Message>((_, msg) => {
				Console.WriteLine("[client2]received {0}: {1}", msg.Name, msg.Content);
			});
			client2.Connect(Ep);

			//client.Send(new Message { Name = "大根ちゃん", Content = "はろーわーるど" });
			client.Send(new ColorInfo { Color = System.Drawing.Color.Red });
			while (true) {
				System.Threading.Thread.Sleep(100);
			}
		}

		static void Server()
		{
			var server = new JsonOnline.Server();
			server.OnAccept += (_, client) => {

				client.On<Message>((__, msg) => {
					Console.WriteLine("[server]received {0}: {1}", msg.Name, msg.Content);
					client.Broadcast(msg);
				});

				client.On<ColorInfo>((__, msg) => {
					Console.WriteLine("[server]received Color: {0}", msg.Color.ToString());
				});
			};
			Task.Run(() => server.Listen(Ep));
		}
	}
}
