using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ClientTester
{
    internal class Program
    {
        // Variables:
        //TcpClient client = new TcpClient();
        bool isConnected = false;
        int port = 7474;

        async void ConnectToServer()
        {
            //Get the local IP address of the laptop wherever you work
            IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
            Console.WriteLine("IPEndPoint created");

            Socket client = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Client created");

            await client.ConnectAsync(ipAddress, port);

            isConnected = client.Connected;
            Console.WriteLine("Client connected: " + isConnected);

            while (true)
            {
                Console.WriteLine("Entered while loop");

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
                    //break;
                }
            }


        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Program program = new Program();
            program.ConnectToServer();
        }
    }
}




