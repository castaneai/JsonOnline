JSON Online
=============
TCP Client/Server Wrapper with JSON Network Messages

Example
----------

- Server

```cs
class Chat : JsonOnline.INetworkMessage {
  public string Name { get; set; }
  public string Message { get; set; }
}

var server = new JsonOnline.Server();
server.OnAccept += (_, client) => {

  client.On<Chat>((__, chat) => {
    client.Broadcast(chat);
  });

};

server.Listen(IPAddress.Any, 8080);
```

- Client

```cs
class Chat : JsonOnline.INetworkMessage {
  public string Name { get; set; }
  public string Message { get; set; }
}

var client = new JsonOnline.Client();
client.On<Chat>((_, msg) => {
  Console.WriteLine("{0} says: {1}", msg.Name, msg.Message);
});

client.Connect(IPAddress.Parse("127.0.0.1"), 8080);
client.Send(new Chat { Name = "Yasuna", Message = "Baby, kill me please!" });
```

Vendor Library
----------------

- [DynamicJson] (http://dynamicjson.codeplex.com/)