using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatSampleServer.Message
{
	public class ChatMessage : JsonOnline.INetworkMessage
	{
		public string UserName { get; set; }

		public string Message { get; set; }
	}
}
