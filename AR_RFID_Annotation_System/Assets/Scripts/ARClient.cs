using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Net;
using Unity.VisualScripting;
using TMPro;    //include to interact with the TextMeshPro textbox

public class ARClient : MonoBehaviour
{
    // Variables:
    //private TcpClient client;
    //private NetworkStream stream;
    private bool isConnected = false;
    //private bool messageOver = false;
    public TMP_Text textBox;
    //private string serverIP = "146.232.65.147";
    private int port = 7474;
    Socket client;

    // Start is called before the first frame update
    void Start()
    {
        ConnectToServer();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (isConnected && stream.DataAvailable)
        {
            byte[] dataBytes = new byte[1024];
            int bytesRead = stream.Read(dataBytes, 0, dataBytes.Length);
            //int 
            string data = Encoding.ASCII.GetString(dataBytes, 0, bytesRead);

            //ProcessReceivedData(data) // Implement this method to parse and process the data received from the server
            textBox.text = data;

            //messageOver = true;
        }
        */
    }

    private async void ConnectToServer()
    {
        //Get the local IP address of the laptop wherever you work
        IPHostEntry ipHostInfoClient = Dns.GetHostEntry("localhost");
        IPAddress ipAddressClient = ipHostInfoClient.AddressList[0];
        int portClient = 7474;

        IPEndPoint ipEndPoint = new IPEndPoint(ipAddressClient, portClient);

        client = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        await client.ConnectAsync(ipAddressClient, portClient);

        isConnected = client.Connected;

        while (isConnected)
        {
            // Send test message
            var message = "I am a client that has connected <|EOM|>";
            var messageBytes = Encoding.ASCII.GetBytes(message);
            _ = await client.SendAsync(messageBytes, SocketFlags.None);
            //Debug.Log($"Socket client sent message: \"{message}\"");
            textBox.text = $"Socket client sent message: \"{message}\"\n";
            //Console.WriteLine($"Socket client sent message: \"{message}\"");
            //UpdateStatus($"Socket client sent message: \"{message}\"");

            // Receive acknoledgement
            var buffer = new byte[1024];
            var received = await client.ReceiveAsync(buffer, SocketFlags.None);
            var response = Encoding.ASCII.GetString(buffer, 0, received);

            if (response == "<|ACK|>")
            {
                //Debug.Log($"Socket client received acknowledgment: \"{response}\"");
                //Console.WriteLine($"Socket client received acknowledgment: \"{response}\"");
                //UpdateStatus($"Socket client received acknowledgment: \"{response}\"");
                textBox.text += $"Socket client received acknowledgment: \"{response}\"";
                break;
            }
        }


    }

    private void OnApplicationQuit()
    {
        if (isConnected)
        {
            //stream.Close();
            //client.Close();
            client.Shutdown(SocketShutdown.Both);
            isConnected = false;
        }
    }
}
