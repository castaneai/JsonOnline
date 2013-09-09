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
        /// <summary>
        /// クライアントを識別するID
        /// </summary>
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

        /// <summary>
        /// 識別IDの材料　クライアント固有の情報にすること
        /// </summary>
        private string idSeed;

		private string id;

        /// <summary>
        /// クライアントが接続しているサーバー
        /// </summary>
		private Server server;
        
        public void Connect(TcpClient baseClient)
        {
            this.idSeed = baseClient.Client.RemoteEndPoint.Serialize().ToString();
            base.Connect(baseClient.GetStream());
        }

		internal AcceptedClient(Server server)
		{
			this.server = server;
		}

		internal void ReceiveForever()
		{
			this.readForever();
		}

        /// <summary>
        /// 自身以外のすべてのクライアントにメッセージを送信する
        /// </summary>
        /// <param name="message"></param>
		public void Broadcast(object message)
		{
			// 自分自身以外のすべてのクライアントにメッセージを送る
			this.server.Broadcast(message, (client) => client != this);	
		}

		private string calculateClientId()
		{
			var sha256 = new System.Security.Cryptography.SHA256Cng();
			var decryptedBytes = ASCIIEncoding.ASCII.GetBytes(idSeed);
			var encryptedBytes = sha256.ComputeHash(decryptedBytes);
			return BitConverter.ToString(encryptedBytes).Replace("-", "");
		}

	}
}
