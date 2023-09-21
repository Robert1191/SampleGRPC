// See https://aka.ms/new-console-template for more information
using Grpc.Net.Client;
using SampleClient;

//Console.WriteLine("Hello, World!");

using var channel = GrpcChannel.ForAddress("http://localhost:5000");
var client = new Greeter.GreeterClient(channel);
var reply = await client.SayHelloAsync(new HelloRequest { Name = "Hello from Robert" });
Console.WriteLine("Server says: " + reply.Message);
