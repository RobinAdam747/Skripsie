using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPServer
{
    public partial class Form1 : Form
    {
        // Variables:
        TcpListener listener;   //listener to listen for connection requests and IPs
        //static IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 7474);
        //Socket listener = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        //Socket listener = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        private bool isRunning = false; //boolean set when server is set to be up and running
        private bool isConnected = false;


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
                    //IPAddress ipAddress = IPAddress("local address");
                    int port = 7474;                  // Choose a port number
                    IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
                    IPAddress ipAddress = ipHostInfo.AddressList[0];
                    //IPEndPoint endPointHost = 

                    txtIP.Text = ipAddress.ToString() + ":7474";

                    listener = new TcpListener(ipAddress, port);
                    listener.Start();

                    //listener.Bind(ipEndPoint);  // associates the socket with the network address
                    //listener.Listen(100);       // makes the listener listen for a bit

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
                TcpClient acceptedClient = await listener.AcceptTcpClientAsync();
                //var handler = await listener.AcceptAsync();
                //var acceptedSocket = await listener.AcceptAsync();
                UpdateStatus("Client connected: " + acceptedClient.Client.RemoteEndPoint);

                lstClientIP.Text += acceptedClient.ToString() + "\n";

                HandleClientCommunication(acceptedClient);
            }
        }

        private async void HandleClientCommunication(TcpClient client)
        {
            try
            {
                //var handler = await listener.AcceptAsync();

                NetworkStream stream = client.GetStream();
                byte[] data = new byte[1024];

                while (isRunning)
                {
                    int bytesRead = stream.Read(data, 0, data.Length);
                    //var bytesRead = await socket.ReceiveAsync(data, SocketFlags.None);
                    string messageReceived = Encoding.ASCII.GetString(data, 0, bytesRead);
                    UpdateStatus("Received from client: " + messageReceived);

                    var messageSent = txtMessage.Text + "\r\n";
                    var bytesWritten = Encoding.ASCII.GetBytes(messageSent);
                    await stream.WriteAsync(bytesWritten);
                    UpdateStatus("Sent to client: " + messageSent);
                    /*
                    var eom = "<|EOM|>";
                    if (message.IndexOf(eom) > -1)  //is end of message
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
                    */
                }
                stream.Close();
                client.Close();
                //socket.Close();
                UpdateStatus("Client disconnected: " + client.Client.RemoteEndPoint);
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
                UpdateStatus("Server stopped.");
                isRunning = false;
                //listener.Stop();
            }
        }

        private void txtIP_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //HandleClientCommunication()
            ConnectToServer();
        }

        //Testing the client in the same script:
        private async void ConnectToServer()
        {
            //Get the local IP address of the laptop wherever you work
            IPHostEntry ipHostInfoClient = Dns.GetHostEntry("localhost");
            IPAddress ipAddressClient = ipHostInfoClient.AddressList[0];
            int portClient = 7474;

            IPEndPoint ipEndPoint = new IPEndPoint(ipAddressClient, portClient);

            Socket client = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            await client.ConnectAsync(ipAddressClient, portClient);

            isConnected = client.Connected;

            while (true)
            {
                // Send test message
                var message = "I am a client that has connected <|EOM|>";
                var messageBytes = Encoding.ASCII.GetBytes(message);
                _ = await client.SendAsync(messageBytes, SocketFlags.None);
                //Debug.Log($"Socket client sent message: \"{message}\"");
                //Console.WriteLine($"Socket client sent message: \"{message}\"");
                UpdateStatus($"Socket client sent message: \"{message}\"");

                // Receive acknoledgement
                var buffer = new byte[1024];
                var received = await client.ReceiveAsync(buffer, SocketFlags.None);
                var response = Encoding.ASCII.GetString(buffer, 0, received);

                if (response == "<|ACK|>")
                {
                    //Debug.Log($"Socket client received acknowledgment: \"{response}\"");
                    //Console.WriteLine($"Socket client received acknowledgment: \"{response}\"");
                    UpdateStatus($"Socket client received acknowledgment: \"{response}\"");
                    break;
                }
            }


        }
    }
}