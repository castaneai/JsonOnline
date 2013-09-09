using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonOnline
{
    public class MessageReader : IDisposable
    {
        private readonly Stream stream;

        private readonly TextReader reader;

        public MessageReader(Stream stream)
        {
            this.stream = stream;
            this.reader = new StreamReader(stream);
        }

        public T ReadNextMessage<T>()
        {
            var packet = this.ReadNextPacket();
            return JsonConvert.DeserializeObject<T>(packet.Data);
        }

        internal Packet ReadNextPacket()
        {
            var json = this.reader.ReadLine();
            var jObject = JObject.Parse(json);
            return new Packet {
                TypeName = jObject["TypeName"].ToString(),
                Data = jObject["Data"].ToString()
            };
        }

        public void Close()
        {
            if (this.reader != null) {
                this.reader.Close();
            }
        }

        public void Dispose()
        {
            this.Close();
        }
    }
}
