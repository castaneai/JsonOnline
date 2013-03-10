using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using Codeplex.Data;
using System.Collections.Concurrent;

namespace JsonOnline
{
	public class Client
	{
		public event Action<Client> OnDisconnect = delegate { };

		private class Packet
		{
			public string Type { get; set; }
			public object Data { get; set; }
		}

		protected TcpClient baseClient;

		private StreamReader reader;

		private StreamWriter writer;

		private IDictionary<string, Delegate> receiveHandlers;

		private IDictionary<string, Type> typeCache;

		public Client()
		{
			this.baseClient = new TcpClient();
			this.receiveHandlers = new ConcurrentDictionary<string, Delegate>();
			this.typeCache = new ConcurrentDictionary<string, Type>();
		}

		public virtual void Connect(System.Net.IPAddress ip, ushort port)
		{
			Connect(new System.Net.IPEndPoint(ip, port));
		}

		public virtual void Connect(System.Net.IPEndPoint ep)
		{
			this.baseClient.Connect(ep);
			receiveForeverAsync();
		}

		public void Close()
		{
			this.OnDisconnect(this);
			this.baseClient.Close();
		}

		public void Send(INetworkMessage message)
		{
			if (this.writer == null) {
				this.writer = new StreamWriter(this.baseClient.GetStream());	
			}
			var packet = new Packet {
				Type = message.GetType().FullName,
				Data = message,
			};
			var jsonString = DynamicJson.Serialize(packet);
			writer.WriteLine(jsonString);
			writer.Flush();
		}

		public void On<TNetworkMessage>(Action<Client, TNetworkMessage> handler)
			where TNetworkMessage : INetworkMessage
		{
			var messageTypeName = typeof(TNetworkMessage).FullName;

			if (this.typeCache.ContainsKey(messageTypeName) == false) {
				this.typeCache[messageTypeName] = typeof(TNetworkMessage);
			}	
			if (this.receiveHandlers.ContainsKey(messageTypeName) == false) {
				this.receiveHandlers[messageTypeName] = handler;
			}
			else {
				this.receiveHandlers[messageTypeName] = Delegate.Combine(
					this.receiveHandlers[messageTypeName],
					handler
				);
			}
		}

		protected void receiveForever()
		{
			if (this.reader == null) {
				this.reader = new StreamReader(this.baseClient.GetStream());
			}
			try {
				while (true) {
					var jsonString = this.reader.ReadLine();
					var json = DynamicJson.Parse(jsonString);
					var messageTypeName = (string)json.Type;
					if (this.typeCache.ContainsKey(messageTypeName) == false) {
						throw new Exception("MessageType: " + messageTypeName + " Not Found in type cache dictionary. Did you set Client.On<" + messageTypeName + "> handlers?");
					}
					var messageType = this.typeCache[messageTypeName];
					if (this.receiveHandlers.ContainsKey(messageTypeName)) {
						var handlerDelegate = this.receiveHandlers[messageTypeName];
						var message = ((DynamicJson)json.Data).Deserialize(messageType);
						handlerDelegate.DynamicInvoke(this, message);
					}
				}
			}
			catch (Exception e) {
				// error handling.
				var m = e.ToString();
			}
			finally {
				this.Close();
			}
		}

		protected void receiveForeverAsync()
		{
			Task.Run(() => receiveForever());
		}
	}
}
