using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPServer
{
    public partial class Form1 : Form
    {
        // Variables:
        //TcpListener listener;   //listener to listen for connection requests and IPs
        static IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 7474);
        //Socket listener = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        Socket listener = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        private bool isRunning = false; //boolean set when server is set to be up and running


        public Form1()
        {
            InitializeComponent();
        }

        private void UpdateStatus(string message)
        {
            // This method is used to update the status textbox on the UI thread.
            if (txtInfo.InvokeRequired)
            {
                txtInfo.Invoke((MethodInvoker)delegate
                {
                    txtInfo.AppendText(message + Environment.NewLine);
                });
            }
            else
            {
                txtInfo.AppendText(message + Environment.NewLine);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                try
                {
                    //var ipAddress = IPAddress.Any;
                    //int port = 7474;                  // Choose a port number

                    //listener = new TcpListener(ipAddress, port);
                    //listener.Start();

                    listener.Bind(ipEndPoint);  // associates the socket with the network address
                    listener.Listen(100);       // makes the listener listen for a bit

                    isRunning = true;
                    UpdateStatus("Server started. Waiting for incoming connections...");

                    // Start a new thread to listen for incoming client connections
                    //Thread serverThread = new Thread(ListenForClients); //idk about this, defs ask
                    //serverThread.Start();
                    ListenForClients();
                }
                catch (Exception ex)
                {
                    UpdateStatus("Error starting server: " + ex.Message);
                }
            }
        }

        private async void ListenForClients()
        {
            while (isRunning)
            {
                //TcpClient client = listener.AcceptTcpClient();
                //var handler = await listener.AcceptAsync();
                var acceptedSocket = await listener.AcceptAsync();
                UpdateStatus("Client connected: " + acceptedSocket.RemoteEndPoint);

                // Start a new thread to handle communication with the connected client
                //Thread clientThread = new Thread(() => HandleClientCommunication(acceptedSocket));
                //clientThread.Start();
                HandleClientCommunication(acceptedSocket);
            }
        }

        private async void HandleClientCommunication(Socket socket)
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
                    string message = Encoding.ASCII.GetString(data, 0, bytesRead);
                    UpdateStatus("Received from client: " + message);

                    var eom = "<|EOM|>";
                    if (message.IndexOf(eom) > -1 /* is end of message */)
                    {
                        //Console.WriteLine($"Socket server received message: \"{message.Replace(eom, "")}\"");
                        UpdateStatus($"Socket server received message: \"{message.Replace(eom, "")}\"");

                        var ackMessage = "<|ACK|>";
                        var echoBytes = Encoding.ASCII.GetBytes(ackMessage);
                        await socket.SendAsync(echoBytes, 0);
                        //Console.WriteLine($"Socket server sent acknowledgment: \"{ackMessage}\"");
                        UpdateStatus($"Socket server sent acknowledgment: \"{ackMessage}\"");


                        // Process the received data or respond to the client as needed

                        // For example, if you want to send a response back to the client:
                        // string responseMessage = "Hello from server!";
                        // byte[] responseData = Encoding.ASCII.GetBytes(responseMessage);
                        // stream.Write(responseData, 0, responseData.Length);
                    }
                }
                //stream.Close();
                //client.Close();
                socket.Close();
                UpdateStatus("Client disconnected: " + socket.RemoteEndPoint);
            }
            catch (Exception ex)
            {
                UpdateStatus("Error handling client communication: " + ex.Message);
            }
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isRunning)
            {
                isRunning = false;
                //listener.Stop();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                isRunning = false;
                //listener.Stop();
                UpdateStatus("Server stopped.");
            }
        }
    }
}