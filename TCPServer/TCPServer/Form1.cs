using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPServer
{
    public partial class Form1 : Form
    {
        // Variables:
        private TcpListener listener;   //listener to listen for connection requests and IPs
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
                    int port = 8080; // Choose a port number

                    IPAddress ipAddress = IPAddress.Any;
                    listener = new TcpListener(ipAddress, port);
                    listener.Start();

                    isRunning = true;
                    UpdateStatus("Server started. Waiting for incoming connections...");

                    // Start a new thread to listen for incoming client connections
                    Thread serverThread = new Thread(ListenForClients);
                    serverThread.Start();
                }
                catch (Exception ex)
                {
                    UpdateStatus("Error starting server: " + ex.Message);
                }
            }
        }

        private void ListenForClients()
        {
            while (isRunning)
            {
                TcpClient client = listener.AcceptTcpClient();
                UpdateStatus("Client connected: " + client.Client.RemoteEndPoint);

                // Start a new thread to handle communication with the connected client
                Thread clientThread = new Thread(() => HandleClientCommunication(client));
                clientThread.Start();
            }
        }

        private void HandleClientCommunication(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] data = new byte[1024];

                while (isRunning)
                {
                    int bytesRead = stream.Read(data, 0, data.Length);
                    string message = Encoding.ASCII.GetString(data, 0, bytesRead);
                    UpdateStatus("Received from client: " + message);

                    // Process the received data or respond to the client as needed

                    // For example, if you want to send a response back to the client:
                    // string responseMessage = "Hello from server!";
                    // byte[] responseData = Encoding.ASCII.GetBytes(responseMessage);
                    // stream.Write(responseData, 0, responseData.Length);
                }

                stream.Close();
                client.Close();
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
                listener.Stop();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                isRunning = false;
                listener.Stop();
                UpdateStatus("Server stopped.");
            }
        }

        /*private void StopButton_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                isRunning = false;
                listener.Stop();
                UpdateStatus("Server stopped.");
            }
        }*/
    }
}