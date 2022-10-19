// See https://aka.ms/new-console-template for more information
using System.Net.Sockets;
using System.Net;
using System.Text;


IPAddress localAddr = IPAddress.Parse("192.168.100.4");
var ipEndPoint = new IPEndPoint(localAddr, 1300);

using TcpClient client = new();
await client.ConnectAsync(ipEndPoint);
string message = "ping";
Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

// Get a client stream for reading and writing.
NetworkStream stream = client.GetStream();

// Send the message to the connected TcpServer.
stream.Write(data, 0, data.Length);

Console.WriteLine("Sent: {0}", message);

// Receive the server response.

// Buffer to store the response bytes.
data = new Byte[256];

// String to store the response ASCII representation.
String responseData = String.Empty;

// Read the first batch of the TcpServer response bytes.
Int32 bytes = stream.Read(data, 0, data.Length);
responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
Console.WriteLine("Received: {0}", responseData);


