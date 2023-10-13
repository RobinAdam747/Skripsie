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
        DigitalTwinServer? dtServer = new();

        public LabDT()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {

        }



        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtServer?.CloseAll(txtInfo);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {

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
            dtServer?.StartServer(txtInfo, lstClients, txtServerAddress);
        }

        private void btnStop_Click_1(object sender, EventArgs e)
        {

            dtServer?.CloseAll(txtInfo);
        }

        private void btnUnitTest_Click(object sender, EventArgs e)
        {
            //dtServer?.SendUnitTest();
        }
    }
}