// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Hello, World!");

// Variables:
//TcpClient client = new TcpClient();
bool isConnected = false;
int port = 7474;

ConnectToServer();

async void ConnectToServer()
{
    //Get the local IP address of the laptop wherever you work
    IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
    IPAddress ipAddress = ipHostInfo.AddressList[0];

    IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);

    Socket client = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

    await client.ConnectAsync(ipAddress, port);

    isConnected = client.Connected;

    while (true)
    {
        // Send test message
        var message = "I am a client that has connected <|EOM|>";
        var messageBytes = Encoding.ASCII.GetBytes(message);
        _ = await client.SendAsync(messageBytes, SocketFlags.None);
        //Debug.Log($"Socket client sent message: \"{message}\"");
        Console.WriteLine($"Socket client sent message: \"{message}\"");

        // Receive acknoledgement
        var buffer = new byte[1024];
        var received = await client.ReceiveAsync(buffer, SocketFlags.None);
        var response = Encoding.ASCII.GetString(buffer, 0, received);

        if (response == "<|ACK|>")
        {
            //Debug.Log($"Socket client received acknowledgment: \"{response}\"");
            Console.WriteLine($"Socket client received acknowledgment: \"{response}\"");
            break;
        }
    }

    
}
