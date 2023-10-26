using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Runtime.ExceptionServices;
//using LabDT;

namespace ARUnity
{
    public class DigitalTwinServer
    {
        // Variables (Attributes):
        private bool isRunning { get; set; }                     //boolean set when server is set to be up and running
        private TcpListener? listener { get; set; }              //listener to listen for connection requests and IPs
        private bool isClientConnected { get; set; }             //boolean for when a client is connected
        private string? plcData { get; set; }                    //data received from the plc

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
                    //string ip = "10.66.178.171";
                    //string ip = "192.168.1.38";
                    string ip = "146.232.145.241";
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
                    string messageReceived = Encoding.UTF8.GetString(data, 0, bytesRead);
                    UpdateStatus("Received from client: " + messageReceived, textBox);

                    var eom = "<|EOM|>";
                    if (messageReceived.IndexOf(eom) > -1)  //is at end of message
                    {
                        //Console.WriteLine($"Socket server received message: \"{message.Replace(eom, "")}\"");
                        UpdateStatus($"Socket server received message: \"{messageReceived.Replace(eom, "")}\"", textBox);

                        var ackMessage = "<|ACK|>";
                        var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
                        await socket.SendAsync(echoBytes, 0);
                        //Console.WriteLine($"Socket server sent acknowledgment: \"{ackMessage}\"");
                        UpdateStatus($"Socket server sent acknowledgment: \"{ackMessage}\"", textBox);

                        //isRunning = false;  //temporary fix... (disconnecting after handshake)
                    }
                    //Disconnect logic:
                    else if (messageReceived.Equals("<|EOC|>"))
                    {
                        isClientConnected = false;
                        socket.Shutdown(SocketShutdown.Both);
                        UpdateStatus("Client disconnected: " + socket.RemoteEndPoint, textBox);
                        clientsList.Items.Remove(socket.RemoteEndPoint);
                    }
                    //If the AR app sends a blank when the app on the tablet is exited, prevents infinite loop glitch of DT
                    else if (messageReceived.Equals(""))
                    {
                        isClientConnected = false;
                        socket.Shutdown(SocketShutdown.Both);
                        UpdateStatus("Client disconnected: " + socket.RemoteEndPoint, textBox);
                        clientsList.Items.Remove(socket.RemoteEndPoint);
                    }
                    //Handling messages from the PLC:
                    else if (messageReceived.IndexOf("PLC:") > -1)
                    {
                        //Currently the data received from the PLC is just a string, but logic can be added here to receive the data as a JSON and deserialize it
                        plcData = messageReceived;
                        UpdateStatus("", textBox);  //newline in textbox
                    }
                    else
                    {
                        InterpretJSONStringServerside(messageReceived, textBox, socket);
                    }
                    
                }
                
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
            var unitTestMessageBytes = Encoding.UTF8.GetBytes(unitTestMessage);

            //Send
            await socket.SendAsync(unitTestMessageBytes, SocketFlags.None);

            //Display sent message on screen
            UpdateStatus("Message sent to " + unitTest.destinationID + " : " + unitTestMessage, textBox);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="textBox"></param>
        /// <param name="requestType"></param>
        /// <param name="conversationID"></param>
        public async void SendMessageToARSystem(Socket socket, System.Windows.Forms.TextBox textBox, string requestType, string conversationID)
        {
            //compose a message with the requested payload and send back
            MessagePayload plcMessage = new MessagePayload(conversationID_: conversationID, versionNumber_: 1, sourceID_: "Digital Twin", destinationID_:
                "AR System", expiry_: "5 minutes", sendTime_: DateTime.Now.ToString(), requestType_: requestType, payloadJSON_: plcData);

            //Serialize for sending
            string messageToARSystem = JsonSerializer.Serialize(plcMessage);

            //Encode for sending
            var messageToARSystemBytes = Encoding.UTF8.GetBytes(messageToARSystem);

            //Send
            await socket.SendAsync(messageToARSystemBytes, SocketFlags.None);

            //Display feedback on screen
            UpdateStatus("Message in conversation " + conversationID + " was sent to AR System at " + DateTime.Now.ToString(), textBox);

        }

        //public async void RequestDataFromPlc(string messageRequest, Socket socket, System.Windows.Forms.TextBox textBox)
        //{
        //    //Encode message
        //    var messageRequestEncoded = Encoding.UTF8.GetBytes(messageRequest);

        //    //Send
        //    await socket.SendAsync(messageRequestEncoded, SocketFlags.None);

        //    //Display feedback on screen
        //    UpdateStatus("Data from PLC requested", textBox);
        //}

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
                    textBox.AppendText(message + Environment.NewLine + Environment.NewLine);
                });
            }
            else
            {
                textBox.AppendText(message + Environment.NewLine + Environment.NewLine);
            }

        }

        /// <summary>
        /// Interpreting standard JSON string:
        /// Logic to interpret and decipher a received JSON.
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="textBox"></param>
        /// <param name="socket"></param>
        public void InterpretJSONStringServerside(string jsonString, System.Windows.Forms.TextBox textBox, Socket socket)
        {
            MessagePayload? message = JsonSerializer.Deserialize<MessagePayload>(jsonString);

            //logic to interpret deserialized JSON message
            switch (message?.requestType)
            {
                case "Unit Test":
                    UpdateStatus(jsonString + "\n", textBox);
                    SendUnitTest(socket, textBox);
                    break;
                case "Request RFID Data":
                    //RequestDataFromPlc("Request RFID data", socket, textBox);
                    SendMessageToARSystem(socket, textBox, "Request RFID Data", "RFID Data Exchange");
                    break;
                case "Integration Test":
                    //RequestDataFromPlc("PLC Greeting", socket, textBox);

                    UpdateStatus("Beginning Integration Test", textBox);

                    SendMessageToARSystem(socket, textBox, "Integration Test", "Full System Integration Test");
                    break;
                default:
                    break;
            }
        }
    }
}