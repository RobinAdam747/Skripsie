using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ClientTester2
{
    public partial class Form1 : Form
    {
        // Variables:
        private bool isConnected = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectToServer();
        }

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

        private void UpdateStatus(string message)
        {
            // This method is used to update the status textbox on the UI thread.
            if (txtStatus.InvokeRequired)
            {
                txtStatus.Invoke((MethodInvoker)delegate
                {
                    txtStatus.AppendText(message + Environment.NewLine);
                });
            }
            else
            {
                txtStatus.AppendText(message + Environment.NewLine);
            }
        }
    }
}