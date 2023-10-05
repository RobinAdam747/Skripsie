using System.Net.Sockets;
using System.Net;
using System.Text;
//using LabDT;

namespace ARUnity
{
    public class DigitalTwinServer
    {
        // Variables (Attributes):
        private bool isRunning { get; set; }                     //boolean set when server is set to be up and running
        private TcpListener? listener { get; set; }              //listener to listen for connection requests and IPs
        private bool isClientConnected { get; set; }             //boolean for when a client is connected

        /// <summary>
        /// Parameterised constructor
        /// </summary>
        /// <param name="isRunning_"></param>
        /// <param name="listener_"></param>
        /// <param name="acceptedSocketEndPoint_"></param>
        public DigitalTwinServer(bool isRunning_, TcpListener listener_, bool isClientConnected_)
        {

        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DigitalTwinServer() { }

        /// <summary>
        /// Enables a listener to await TCP/IP connections.
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="clientsList"></param>
        /// <param name="serverAddressTextBox"></param>
        public void StartServer(System.Windows.Forms.TextBox textBox, System.Windows.Forms.ListBox clientsList,
            System.Windows.Forms.TextBox serverAddressTextBox)
        {
            if (!isRunning)
            {
                try
                {
                    //For local testing (listening on local host):
                    //IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
                    //IPAddress ipAddress = ipHostInfo.AddressList[0];

                    //For tablet:
                    //(listening on the ip that they should speak on, check ipconfig each time location/wifi changes)
                    string ip = "10.66.178.171";
                    //string ip = "192.168.1.37";
                    IPAddress ipAddress = IPAddress.Parse(ip);
                    //IPAddress ipAddress = IPAddress.Any;

                    int port = 7474;                  // Choose a port number

                    serverAddressTextBox.Text = ipAddress + ":7474";

                    listener = new TcpListener(ipAddress, port);
                    listener.Start();

                    isRunning = true;
                    UpdateStatus("Server started. Waiting for incoming connections...", textBox);

                    // Listen for incoming client connections
                    ListenForClients(textBox, clientsList);
                }
                catch (Exception ex)
                {
                    UpdateStatus("Error starting server: " + ex.Message, textBox);
                }
            }
        }

        /// <summary>
        /// Allows the listener to asynchronous accept socket connections
        /// and calls the communication handling method.
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="clientsList"></param>
        public async void ListenForClients(System.Windows.Forms.TextBox textBox, System.Windows.Forms.ListBox clientsList)
        {
            while (isRunning)
            {
                Socket acceptedSocket = await listener.AcceptSocketAsync();
                UpdateStatus("Client connected: " + acceptedSocket.RemoteEndPoint, textBox);
                //statusMessage += "Client connected: " + acceptedSocket.RemoteEndPoint;

                clientsList.Items.Add(acceptedSocket.RemoteEndPoint);
                //acceptedSocketEndPoint = acceptedSocket.RemoteEndPoint;
                isClientConnected = true;

                HandleClientCommunication(acceptedSocket, textBox, clientsList);
            }
        }

        /// <summary>
        /// Once a client is connected, a simple handshake is initiated.
        /// The connection is also terminated upon receiving the necessary code.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="textBox"></param>
        /// <param name="clientsList"></param>
        public async void HandleClientCommunication(Socket socket, System.Windows.Forms.TextBox textBox, 
            System.Windows.Forms.ListBox clientsList)
        {
            try
            {
                byte[] data = new byte[1024];

                while (isRunning && isClientConnected)
                {
                    var bytesRead = await socket.ReceiveAsync(data, SocketFlags.None);
                    string messageReceived = Encoding.ASCII.GetString(data, 0, bytesRead);
                    UpdateStatus("Received from client: " + messageReceived, textBox);

                    var eom = "<|EOM|>";
                    if (messageReceived.IndexOf(eom) > -1)  //is at end of message
                    {
                        //Console.WriteLine($"Socket server received message: \"{message.Replace(eom, "")}\"");
                        UpdateStatus($"Socket server received message: \"{messageReceived.Replace(eom, "")}\"", textBox);

                        var ackMessage = "<|ACK|>";
                        var echoBytes = Encoding.ASCII.GetBytes(ackMessage);
                        await socket.SendAsync(echoBytes, 0);
                        //Console.WriteLine($"Socket server sent acknowledgment: \"{ackMessage}\"");
                        UpdateStatus($"Socket server sent acknowledgment: \"{ackMessage}\"", textBox);

                        //isRunning = false;  //temporary fix... (disconnecting after handshake)
                    }

                    if (messageReceived.Equals("<|EOC|>"))
                    {
                        isClientConnected = false;
                        socket.Shutdown(SocketShutdown.Both);
                        UpdateStatus("Client disconnected: " + socket.RemoteEndPoint, textBox);
                        clientsList.Items.Remove(socket.RemoteEndPoint);
                    }

                }
                //stream.Close();
                //client.Close();
                //socket.Close();

            }
            catch (Exception ex)
            {
                UpdateStatus("Error handling client communication: " + ex.Message, textBox);
            }
        }

        /// <summary>
        /// Closes the server connection by stopping the listener.
        /// </summary>
        /// <param name="textBox"></param>
        public void CloseAll(System.Windows.Forms.TextBox textBox)
        {
            if (isRunning)
            {
                //statusMessage = "Server stopped.";
                UpdateStatus("Server stopped.", textBox);
                isRunning = false;
                listener?.Stop();
            }
        }

        public void SendUnitTest()
        {
            throw new NotImplementedException("Unable to send the unit test just yet");

            //MessagePayload unitTest = new MessagePayload();

            //unitTest.
        }

        /// <summary>
        /// Sends a message to a windows forms text box.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="textBox"></param>
        public void UpdateStatus(string message, System.Windows.Forms.TextBox textBox)
        {
            // This method is used to update the status/info textbox
            if (textBox.InvokeRequired)
            {
                textBox.Invoke((MethodInvoker)delegate
                {
                    textBox.AppendText(message + Environment.NewLine);
                });
            }
            else
            {
                textBox.AppendText(message + Environment.NewLine);
            }

        }
    }
}