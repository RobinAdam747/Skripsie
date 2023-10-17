using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
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
                    //string ip = "10.66.178.171";        //Eduroam laptop testing
                    //string ip = "192.168.1.38";         //Home testing
                    string ip = "146.232.145.241";      //VR lab computer
                    IPAddress ipAddress = IPAddress.Parse(ip);
                    //IPAddress ipAddress = IPAddress.Any;

                    int port = 47474;                  // Choose a port number

                    serverAddressTextBox.Text = ipAddress + ":" + port;

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
                    else if (messageReceived.Equals("<|EOC|>"))
                    {
                        isClientConnected = false;
                        socket.Shutdown(SocketShutdown.Both);
                        UpdateStatus("Client disconnected: " + socket.RemoteEndPoint, textBox);
                        clientsList.Items.Remove(socket.RemoteEndPoint);
                    }
                    else if (messageReceived.Equals(""))
                    {
                        isClientConnected = false;
                        socket.Shutdown(SocketShutdown.Both);
                        UpdateStatus("Client disconnected: " + socket.RemoteEndPoint, textBox);
                        clientsList.Items.Remove(socket.RemoteEndPoint);
                        break;
                    }
                    else
                    {
                        InterpretJSONStringServerside(messageReceived, textBox, socket);
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

        /// <summary>
        /// Send a unit test message to a client.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="textBox"></param>
        public async void SendUnitTest(Socket socket, System.Windows.Forms.TextBox textBox)
        {
            //throw new NotImplementedException("Unable to send the unit test just yet");
            //Create unit test message
            MessagePayload unitTest = new ();

            unitTest = unitTest.CreateUnitTestReply(unitTest);

            //Serialize for sending
            string unitTestMessage = JsonSerializer.Serialize(unitTest);

            //Encode for sending
            var unitTestMessageBytes = Encoding.ASCII.GetBytes(unitTestMessage);

            //Send
            await socket.SendAsync(unitTestMessageBytes, SocketFlags.None);

            //Display sent message on screen
            UpdateStatus("Message sent to " + unitTest.destinationID + " : " + unitTestMessage, textBox);
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

        /// <summary>
        /// Interpreting standard JSON string:
        /// Logic to interpret and decipher a received JSON.
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="textBox"></param>
        public void InterpretJSONStringServerside(string jsonString, System.Windows.Forms.TextBox textBox, Socket socket)
        {
            MessagePayload? payload = new MessagePayload(
                conversationID_: "",
                versionNumber_: 0,
                sourceID_: "",
                destinationID_: "",
                expiry_: "",
                sendTime_: DateTime.MinValue.ToString(),
                requestType_: "",
                payloadJSON_: "");

            //payload = JsonSerializer.Deserialize<MessagePayload>(jsonString);

            //logic to interpret deserialized JSON message
            switch (payload?.requestType)
            {
                case "Unit Test":
                    UpdateStatus(jsonString + "\n", textBox);
                    SendUnitTest(socket, textBox);
                    break;
                case "Request Pallet IDs":
                    //interact with code that receives pallet info from PLC

                    break;
                default:
                    break;
            }
        }

        //Testing the client in the same script:
        public async void ConnectToServer(System.Windows.Forms.TextBox textBox)
        {
            //Get the local IP address of the laptop wherever you work
            //IPHostEntry ipHostInfoClient = Dns.GetHostEntry("localhost");
            //IPAddress ipAddressClient = ipHostInfoClient.AddressList[0];

            string ip = "146.232.146.236";      //VR lab computer
            IPAddress ipAddress = IPAddress.Parse(ip);

            int portClient = 47474;

            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, portClient);

            Socket client = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            await client.ConnectAsync(ipAddress, portClient);

            bool isConnected = client.Connected;

            while (isConnected)
            {
                // Send test message
                var message = "I am a client that has connected <|EOM|>";
                var messageBytes = Encoding.ASCII.GetBytes(message);
                _ = await client.SendAsync(messageBytes, SocketFlags.None);
                //Debug.Log($"Socket client sent message: \"{message}\"");
                //Console.WriteLine($"Socket client sent message: \"{message}\"");
                UpdateStatus($"Socket client sent message: \"{message}\"", textBox);

                // Receive acknoledgement
                var buffer = new byte[1024];
                var received = await client.ReceiveAsync(buffer, SocketFlags.None);
                var response = Encoding.ASCII.GetString(buffer, 0, received);

                if (response == "<|ACK|>")
                {
                    //Debug.Log($"Socket client received acknowledgment: \"{response}\"");
                    //Console.WriteLine($"Socket client received acknowledgment: \"{response}\"");
                    UpdateStatus($"Socket client received acknowledgment: \"{response}\"", textBox);
                    break;
                }
            }


        }
    }
}