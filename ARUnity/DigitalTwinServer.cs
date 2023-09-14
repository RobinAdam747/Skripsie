using System.Net.Sockets;
using System.Net;
using System.Text;
//using LabDT;

namespace ARUnity
{
    public class DigitalTwinServer
    {
        // Variables (Attributes):
        public bool isRunning { get; set; }                     //boolean set when server is set to be up and running
        public TcpListener? listener { get; set; }              //listener to listen for connection requests and IPs
        public string? statusMessage { get; set; }              //string to return the status message to the info text box
        public EndPoint? acceptedSocketEndPoint { get; set; }   //endpoint of the accepted client to be added to the client list

        /// <summary>
        /// Parameterised constructor
        /// </summary>
        /// <param name="isRunning_"></param>
        /// <param name="listener_"></param>
        /// <param name="statusMessage_"></param>
        /// <param name="acceptedSocketEndPoint_"></param>
        public DigitalTwinServer(bool isRunning_, TcpListener listener_,
            string statusMessage_, EndPoint acceptedSocketEndPoint_)
        {

        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DigitalTwinServer() { }

        /// <summary>
        /// StartServer: enables the listener to await TCP/IP connections.
        /// </summary>
        public void StartServer()
        {
            if (!isRunning)
            {
                try
                {
                    //LabDT labDT = new LabDT();

                    //For local testing:
                    //IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
                    //IPAddress ipAddress = ipHostInfo.AddressList[0];

                    //For tablet:
                    string ip = "146.232.65.147";
                    IPAddress ipAddress = IPAddress.Parse(ip);
                    //txt.Text = ipAddress.ToString() + "7474";

                    int port = 7474;                  // Choose a port number

                    listener = new TcpListener(ipAddress, port);
                    listener.Start();

                    isRunning = true;
                    UpdateStatus("Server started. Waiting for incoming connections...");
                    statusMessage = "Server started. Waiting for incoming connections...";


                    ListenForClients();
                }
                catch (Exception ex)
                {
                    //UpdateStatus("Error starting server: " + ex.Message);
                    statusMessage = "Error starting server: " + ex.Message;
                }
            }
        }

        /// <summary>
        /// ListenForClients: allows the listener to asynchronous accept socket connections
        /// and calls the communication handling method.
        /// </summary>
        public async void ListenForClients()
        {
            while (isRunning)
            {
                Socket acceptedSocket = await listener.AcceptSocketAsync();
                //UpdateStatus("Client connected: " + acceptedSocket.RemoteEndPoint);
                statusMessage += "Client connected: " + acceptedSocket.RemoteEndPoint;

                //lstClients.Items.Add(acceptedSocket.RemoteEndPoint);
                acceptedSocketEndPoint = acceptedSocket.RemoteEndPoint;

                HandleClientCommunication(acceptedSocket);
            }
        }

        /// <summary>
        /// HandleClientCommunication: 
        /// once a client is connected, a simple handshake is initiated.
        /// The connection is also terminated upon receiving the necessary code.
        /// </summary>
        /// <param name="socket"></param>
        public async void HandleClientCommunication(Socket socket)
        {
            try
            {
                //var handler = await listener.AcceptAsync();

                //NetworkStream stream = client.GetStream();
                byte[] data = new byte[1024];

                while (isRunning)
                {
                    //int bytesRead = stream.Read(data, 0, data.Length);
                    var bytesRead = await socket.ReceiveAsync(data, SocketFlags.None);
                    string messageReceived = Encoding.ASCII.GetString(data, 0, bytesRead);
                    //UpdateStatus("Received from client: " + messageReceived);
                    statusMessage = "Received from client: " + messageReceived;

                    /*
                    var messageSent = txtMessage.Text + "\r\n";
                    var bytesWritten = Encoding.ASCII.GetBytes(messageSent);
                    //await stream.WriteAsync(bytesWritten);
                    await socket.SendAsync(bytesWritten, SocketFlags.None);
                    UpdateStatus("Sent to client: " + messageSent);
                    */

                    var eom = "<|EOM|>";
                    if (messageReceived.IndexOf(eom) > -1)  //is end of message
                    {
                        //Console.WriteLine($"Socket server received message: \"{message.Replace(eom, "")}\"");
                        //UpdateStatus($"Socket server received message: \"{messageReceived.Replace(eom, "")}\"");
                        statusMessage += $"\nSocket server received message: \"{messageReceived.Replace(eom, "")}\"";

                        var ackMessage = "<|ACK|>";
                        var echoBytes = Encoding.ASCII.GetBytes(ackMessage);
                        await socket.SendAsync(echoBytes, 0);
                        //Console.WriteLine($"Socket server sent acknowledgment: \"{ackMessage}\"");
                        //UpdateStatus($"Socket server sent acknowledgment: \"{ackMessage}\"");
                        statusMessage += $"\nSocket server sent acknowledgment: \"{ackMessage}\"";

                        //isRunning = false;  //temporary fix... (disconnecting after handshake)

                    }

                }
                //stream.Close();
                //client.Close();
                //socket.Close();
                socket.Shutdown(SocketShutdown.Both);
                //UpdateStatus("Client disconnected: " + socket.RemoteEndPoint);
                statusMessage += "\nClient disconnected: " + socket.RemoteEndPoint;
                //lstClients.Items.Remove(socket.RemoteEndPoint);
                acceptedSocketEndPoint = socket.RemoteEndPoint;
            }
            catch (Exception ex)
            {
                //UpdateStatus("Error handling client communication: " + ex.Message);
                statusMessage = "Error handling client communication: " + ex.Message;
            }
        }

        public void CloseAll()
        {
            if (isRunning)
            {
                statusMessage = "Server stopped.";
                isRunning = false;
                listener?.Stop();
            }
        }
    }
}