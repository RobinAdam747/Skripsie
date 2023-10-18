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
            //dtServer?.SendUnitTest(dtServer, txtInfo);

            dtServer?.ConnectToServer(txtInfo);
        }
    }
}