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

		public void Broadcast(object message)
		{
			foreach (var castee in this.acceptedClients.Values) {
				castee.Write(message);
			}
		}

		public void Broadcast(object message, Func<AcceptedClient, bool> predicate)
		{
			var castees = this.acceptedClients
				.Select((pair) => pair.Value)
				.Where(predicate);

			foreach (var castee in castees) {
				castee.Write(message);
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
                var acceptedTcpClient = this.baseListener.AcceptTcpClient();
                var acceptedClient = new AcceptedClient(this);
				acceptedClient.OnClosed += (client) => {
					this.acceptedClients.Remove(acceptedClient.Id);
				};
                acceptedClient.Connect(acceptedTcpClient);
				this.OnAccept(this, acceptedClient);
                this.acceptedClients.Add(acceptedClient.Id, acceptedClient);
			}
		}
	}
}
