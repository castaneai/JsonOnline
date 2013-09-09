using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace JsonOnline
{
	public class Client : IDisposable
	{
        /// <summary>
        /// クライアントが切断されたときのイベント
        /// </summary>
		public event Action<Client> OnClosed = delegate { };

        private MessageReader reader;

        private MessageWriter writer;

        /// <summary>
        /// メッセージ型とそれに応じたイベントハンドラの対応辞書
        /// メッセージ型の完全名 -> イベントハンドラデリゲート　の構造になっている
        /// </summary>
        private readonly IDictionary<Type, Delegate> handlers;

		public Client()
		{
			this.handlers = new ConcurrentDictionary<Type, Delegate>();
		}

        /// <summary>
        /// 指定したアドレスに接続する
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        public void Connect(System.Net.IPAddress ipAddress, ushort port)
        {
            var tcpClient = new System.Net.Sockets.TcpClient();
            tcpClient.Connect(new System.Net.IPEndPoint(ipAddress, port));
            Connect(tcpClient.GetStream());
        }

        /// <summary>
        /// 指定したストリームに接続する
        /// </summary>
        /// <param name="stream"></param>
        public virtual void Connect(Stream stream)
        {
            this.reader = new MessageReader(stream);
            this.writer = new MessageWriter(stream);
            readForeverAsync();
        }

        /// <summary>
        /// 接続相手にメッセージを送信する
        /// </summary>
        /// <param name="message"></param>
		public void Write(object message)
		{
            this.writer.Write(message);
		}

        /// <summary>
        /// 指定した型のメッセージを受け取ったとき呼び出すアクションを登録する
        /// </summary>
        /// <typeparam name="TNetworkMessage"></typeparam>
        /// <param name="handler"></param>
		public void On<T>(Action<Client, T> handler)
		{
            this.handlers[typeof(T)] = this.handlers.ContainsKey(typeof(T)) ?
                Delegate.Combine(this.handlers[typeof(T)], handler) :
                handler;
		}

        /// <summary>
        /// 指定したメッセージ型のイベントハンドラを呼び出す
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="packet">受信したパケット</param>
        private void invokeHandlers(Packet packet)
        {
            var matchedPair = this.handlers.FirstOrDefault((pair) => pair.Key.FullName == packet.TypeName);
            if (matchedPair.Equals(default(Dictionary<Type, Delegate>)) ||
                matchedPair.Key == null) {
                return;
            }
            var type = matchedPair.Key;
            var handler = matchedPair.Value;

            var message = JsonConvert.DeserializeObject(packet.Data, type);
            handler.DynamicInvoke(this, message);
        }

        /// <summary>
        /// 受信を永久的に続け、受信するたびにハンドラがあれば実行する
        /// </summary>
		protected void readForever()
		{
			try {
				while (true) {
                    var packet = reader.ReadNextPacket();
                    invokeHandlers(packet);
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

		private void readForeverAsync()
		{
			Task.Run(() => readForever());
		}

        /// <summary>
        /// オブジェクトの後始末をする
        /// IDisposableの実装
        /// </summary>
        public void Dispose()
        {
            this.Close();
        }

        /// <summary>
        /// クライアントを閉じる
        /// </summary>
        public void Close()
        {
            if (this.reader != null) {
                this.reader.Close();
            }
            if (this.writer != null) {
                this.writer.Close();
            }
            this.OnClosed(this);
        }
    }
}
