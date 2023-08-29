using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;


namespace ARUnity
{
    public class MessagePayload
    {
        // Variables (Attributes):
        public string? conversationID { get; set; }
        public int versionNumber { get; set; }
        public string? sourceID { get; set; }
        public string? destinationID { get; set; }
        public string? expiry { get; set; }  
        public DateTime sendTime { get; set; }
        public string? requestType { get; set; }
        public string? payloadJSON { get; set; }   //depending on how I get it from control system, this may change (json string?)

        // Parameterised constructor:
        public MessagePayload(string conversationID_, int versionNumber_, string sourceID_, string destinationID_,
            string expiry_, DateTime sendTime_, string requestType_, string payloadJSON_)
        {

        }


        // Methods:

        // Unit Test:
        public static MessagePayload UnitTest()
        {
            MessagePayload unitTest = new MessagePayload(
                conversationID_:"Unit Test Conversation",
                versionNumber_:1,
                sourceID_:"Digital Twin",
                destinationID_:"Tablet",
                expiry_:"5 minutes", 
                sendTime_:DateTime.Now,
                requestType_:"Unit Test",
                payloadJSON_:"Unit Test Payload");

            return unitTest;
        }

        // Interpreting standard JSON string:
        public void InterpretJSONString(string jsonString)
        {
            MessagePayload? payload = new MessagePayload(
                conversationID_: "",
                versionNumber_:0,
                sourceID_:"",
                destinationID_:"",
                expiry_:"",
                sendTime_:DateTime.MinValue,
                requestType_:"",
                payloadJSON_:"");

            payload = JsonSerializer.Deserialize<MessagePayload>(jsonString);

            //logic to interpret deserialized JSON message
        }
    }

    public class DigitalTwinServer
    {
        // Variables (Attributes):
        public bool isRunning { get; set; }                     //boolean set when server is set to be up and running
        public TcpListener? listener { get; set; }              //listener to listen for connection requests and IPs
        public string? statusMessage { get; set; }              //string to return the status message to the info text box
        public EndPoint? acceptedSocketEndPoint { get; set; }   //endpoint of the accepted client to be added to the client list

        public DigitalTwinServer(bool isRunning_, TcpListener listener_, 
            string statusMessage_, EndPoint acceptedSocketEndPoint_) 
        {

        }

        public DigitalTwinServer() { }

        public void StartServer() 
        {
            if (!isRunning)
            {
                try
                {
                    int port = 7474;                  // Choose a port number
                    IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
                    IPAddress ipAddress = ipHostInfo.AddressList[0];

                    //txtServerAddress.Text = ipAddress.ToString() + "7474";

                    listener = new TcpListener(ipAddress, port);
                    listener.Start();

                    isRunning = true;
                    //UpdateStatus("Server started. Waiting for incoming connections...");
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

    public class ExampleClient
    {
        // Variables (Attributes):
        private bool isConnected = false;

        public ExampleClient()
        {
        
        }
    }

    //Todo:

    //put the code from the form here
    //include unit tests as well

    //throw NotImplementedException ("what this code should do")
        

}