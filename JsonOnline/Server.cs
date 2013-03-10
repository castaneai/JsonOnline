using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace JsonOnline
{
	public class Server
	{
		public event Action<Server, AcceptedClient> OnAccept = delegate { };

		private IDictionary<string, AcceptedClient> acceptedClients;

		private TcpListener baseListener;

		public Server()
		{
			this.acceptedClients = new ConcurrentDictionary<string, AcceptedClient>(); 
		}

		public void Broadcast(INetworkMessage message)
		{
			foreach (var castee in this.acceptedClients.Select((pair) => pair.Value)) {
				castee.Send(message);
			}
		}

		public void Broadcast(INetworkMessage message, Func<AcceptedClient, bool> predicate)
		{
			var castees = this.acceptedClients
				.Select((pair) => pair.Value)
				.Where(predicate);

			foreach (var castee in castees) {
				castee.Send(message);
			}
		}

		public void Listen(IPAddress ip, ushort port)
		{
			Listen(new IPEndPoint(ip, port));
		}

		public void Listen(IPEndPoint endPoint)
		{
			this.baseListener = new TcpListener(endPoint);
			this.baseListener.Start();
			this.serveForever();
		}

		private void serveForever()
		{
			while (true) {
				var acceptedClient = new AcceptedClient(this, this.baseListener.AcceptTcpClient());
				acceptedClient.OnDisconnect += (client) => {
					this.acceptedClients.Remove(acceptedClient.Id);
				};
				this.OnAccept(this, acceptedClient);
				Task.Run(() => processAcceptedClient(acceptedClient));
			}
		}

		private void processAcceptedClient(AcceptedClient client)
		{
			this.acceptedClients.Add(client.Id, client);
			client.ReceiveForever();
		}
	}
}
