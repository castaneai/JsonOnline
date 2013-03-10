using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace JsonOnline
{
	public class AcceptedClient : Client
	{

		public string Id
		{
			get
			{
				if (this.id == null) {
					this.id = calculateClientId();
				}
				return this.id;
			}
		}

		private string id;

		private Server server;

		internal AcceptedClient(Server server, TcpClient baseClient)
		{
			this.server = server;
			this.baseClient = baseClient;
		}

		public override void Connect(System.Net.IPEndPoint ep)
		{
			throw new NotSupportedException();
		}

		internal void ReceiveForever()
		{
			this.receiveForever();
		}

		public void Broadcast(INetworkMessage message)
		{
			// 自分自身以外のすべてのクライアントにメッセージを送る
			this.server.Broadcast(message, (client) => client != this);	
		}

		private string calculateClientId()
		{
			var sha256 = new System.Security.Cryptography.SHA256Cng();
			var decryptedBytes = ASCIIEncoding.ASCII.GetBytes(this.baseClient.Client.RemoteEndPoint.Serialize().ToString());
			var encryptedBytes = sha256.ComputeHash(decryptedBytes);
			return BitConverter.ToString(encryptedBytes).Replace("-", "");
		}

	}
}
