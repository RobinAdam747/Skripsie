using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Text;
using ARUnity;

namespace LabDT
{
    public partial class LabDT : Form
    {
        // Variables:
        //TcpListener? listener1;
        //EndPoint? endPoint1;
        //DigitalTwinServer? dtServer = new();

        TcpListener? listener = null;             //listener to listen for connection requests and IPs
        private bool isRunning = false;           //boolean set when server is set to be up and running
        private bool isClientConnected = false;   //boolean for when a client is connected

        public LabDT()
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

        }



        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            //dtServer.CloseAll();
            
            
            if (isRunning)
            {
                isRunning = false;
                listener?.Stop();
            }
            
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            
        }

        
        private async void ListenForClients()
        {
            while (isRunning)
            {
                Socket acceptedSocket = await listener.AcceptSocketAsync();
                UpdateStatus("Client connected: " + acceptedSocket.RemoteEndPoint);

                lstClients.Items.Add(acceptedSocket.RemoteEndPoint);
                isClientConnected = true;

                HandleClientCommunication(acceptedSocket);
            }
        }
        

        
        private async void HandleClientCommunication(Socket socket)
        {
            try
            {
                byte[] data = new byte[1024];

                while (isRunning && isClientConnected)
                {
                    var bytesRead = await socket.ReceiveAsync(data, SocketFlags.None);
                    string messageReceived = Encoding.ASCII.GetString(data, 0, bytesRead);
                    UpdateStatus("Received from client: " + messageReceived);

                    /*
                    var messageSent = txtMessage.Text + "\r\n";
                    var bytesWritten = Encoding.ASCII.GetBytes(messageSent);
                    //await stream.WriteAsync(bytesWritten);
                    await socket.SendAsync(bytesWritten, SocketFlags.None);
                    UpdateStatus("Sent to client: " + messageSent);
                    */

                    var eom = "<|EOM|>";
                    if (messageReceived.IndexOf(eom) > -1)  //is at end of message
                    {
                        //Console.WriteLine($"Socket server received message: \"{message.Replace(eom, "")}\"");
                        UpdateStatus($"Socket server received message: \"{messageReceived.Replace(eom, "")}\"");

                        var ackMessage = "<|ACK|>";
                        var echoBytes = Encoding.ASCII.GetBytes(ackMessage);
                        await socket.SendAsync(echoBytes, 0);
                        //Console.WriteLine($"Socket server sent acknowledgment: \"{ackMessage}\"");
                        UpdateStatus($"Socket server sent acknowledgment: \"{ackMessage}\"");

                        //isRunning = false;  //temporary fix... (disconnecting after handshake)
                    }

                    if (messageReceived.Equals("<|EOC|>"))
                    {
                        isClientConnected = false;
                        socket.Shutdown(SocketShutdown.Both);
                        UpdateStatus("Client disconnected: " + socket.RemoteEndPoint);
                        lstClients.Items.Remove(socket.RemoteEndPoint);
                    }

                }
                //stream.Close();
                //client.Close();
                //socket.Close();
                
            }
            catch (Exception ex)
            {
                UpdateStatus("Error handling client communication: " + ex.Message);
            }
        }
        

        private void txtIP_TextChanged(object sender, EventArgs e)
        {

        }

        /*
        private void btnSend_Click(object sender, EventArgs e)  // Client tester button
        {
            //HandleClientCommunication()
            ConnectToServer();
        }
        */


        /*
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

            while (isConnected)
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
        */

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnStart_Click_1(object sender, EventArgs e)
        {
            
            //dtServer = new DigitalTwinServer(isRunning_: false,
            //listener_: listener1, statusMessage_: "", acceptedSocketEndPoint_: endPoint1);

            /*
            dtServer.StartServer();
            UpdateStatus(dtServer.statusMessage + "");
            lstClients.Items.Add(dtServer.acceptedSocketEndPoint + "");

            IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            txtServerAddress.Text = ipAddress + ":7474";
            */

            
            if (!isRunning)
            {
                try
                {
                    //For local testing:
                    //IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
                    //IPAddress ipAddress = ipHostInfo.AddressList[0];

                    //For tablet:
                    string ip = "0.0.0.0";
                    //IPAddress ipAddress = IPAddress.Parse(ip);
                    IPAddress ipAddress = IPAddress.Any;

                    int port = 7474;                  // Choose a port number

                    txtServerAddress.Text = ipAddress + ":7474";

                    listener = new TcpListener(ipAddress, port);
                    listener.Start();

                    isRunning = true;
                    UpdateStatus("Server started. Waiting for incoming connections...");

                    // Listen for incoming client connections
                    ListenForClients();
                }
                catch (Exception ex)
                {
                    UpdateStatus("Error starting server: " + ex.Message);
                }
            }
            
        }

        private void btnStop_Click_1(object sender, EventArgs e)
        {

            //dtServer.CloseAll();
            
            
            if (isRunning)
            {
                UpdateStatus("Server stopped.");
                isRunning = false;
                listener?.Stop();
            }
            
        }
    }
}