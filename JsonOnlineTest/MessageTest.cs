using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsonOnlineTest
{
    [TestClass]
    public class MessageTest
    {
        private class TestMessage
        {
            public int IntProperty { get; set; }
            public string StringProperty { get; set; }
            public DateTime StructProperty { get; set; }
        }

        private System.IO.Stream stream;

        private JsonOnline.MessageReader reader;

        private JsonOnline.MessageWriter writer;

        [TestInitialize]
        public void Initialize()
        {
            stream = new System.IO.MemoryStream();
            reader = new JsonOnline.MessageReader(stream);
            writer = new JsonOnline.MessageWriter(stream);
        }

        [TestMethod]
        public void TestReadWriteMessage()
        {
            var message = new TestMessage {
                IntProperty = int.MaxValue,
                StringProperty = "キルミー\nベイベー",
                StructProperty = new DateTime(2012, 1, 1)
            };
            writer.Write(message);
            stream.Position = 0;

            var readMessage = reader.ReadNextMessage<TestMessage>();
            Assert.AreEqual(message.IntProperty, readMessage.IntProperty);
            Assert.AreEqual(message.StringProperty, readMessage.StringProperty);
            Assert.AreEqual(message.StructProperty, readMessage.StructProperty);
        }
    }
}
