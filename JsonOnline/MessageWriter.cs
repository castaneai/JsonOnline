using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace JsonOnline
{
    public class MessageWriter : IDisposable
    {
        private readonly Stream stream;

        private readonly TextWriter writer;

        public MessageWriter(Stream stream)
        {
            this.stream = stream;
            this.writer = new StreamWriter(stream);
        }

        public void Write(object message)
        {
            var packet = new Packet {
                TypeName = message.GetType().FullName,
                Data = JsonConvert.SerializeObject(message)
            };
            var json = JsonConvert.SerializeObject(packet, Formatting.None);
            this.writer.WriteLine(json);
            this.writer.Flush();
        }

        public void Close()
        {
            if (this.writer != null) {
                this.writer.Close();
            }
        }

        public void Dispose()
        {
            this.Close();
        }
    }
}
