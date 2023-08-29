using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Net;
using Unity.VisualScripting;
using TMPro;    //include to interact with the TextMeshPro textbox
using UnityEngine.UI;

public class ARClient : MonoBehaviour
{
    // Variables:
    private bool isConnected = false;
    public TMP_Text textBox;
    public Button btnConnect;
    public Button btnDisconnect;
    public Button btnUpdate;
    private int port = 7474;
    Socket client;

    // Start is called before the first frame update
    void Start()
    {
        //ConnectToServer();

        //Add listeners to listen for button clicks and activate the relative functions
        btnConnect.onClick.AddListener(ConnectButtonClick);
        btnDisconnect.onClick.AddListener(DisconnectButtonClick);
        btnUpdate.onClick.AddListener(UpdateButtonClick);

        //Keep disconnect button disabled until connected
        btnDisconnect.gameObject.SetActive(false);

        //Show connect button
        btnConnect.gameObject.SetActive(true);
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

        if (!isConnected)
        {
            btnConnect.gameObject.SetActive(true);
            btnDisconnect.gameObject.SetActive(false);
        }
        else
        {
            btnConnect.gameObject.SetActive(false);
            btnDisconnect.gameObject.SetActive(true);
        }
    }

    void ConnectButtonClick()
    {
        if (gameObject.activeSelf) 
        {
            ConnectToServer();
            Debug.Log("ButtonConnect clicked!");

            /*
            if (isConnected)
            {
                //Hide Connect button once pressed and successfully connected
                btnConnect.gameObject.SetActive(false);
            }
            */
        }
        
    }

    async void SendDisconnectMessage()
    {
        var disconnectMessage = "<|EOC|>";  //EOC: end of connection
        var messageBytes = Encoding.ASCII.GetBytes(disconnectMessage);
        _ = await client.SendAsync(messageBytes, SocketFlags.None);
    }

    void DisconnectButtonClick()
    {
        if (gameObject.activeSelf)
        {
            if (isConnected)
            {
                Debug.Log("ButtonDisconnect clicked!");

                SendDisconnectMessage();

                //stream.Close();
                //client.Close();
                client.Shutdown(SocketShutdown.Both);
                isConnected = false;

                /*
                //Hide the disconnect button and show the connect button
                btnDisconnect.gameObject.SetActive(false);
                btnConnect.gameObject.SetActive(true);
                */
            }
        }
    }

    void UpdateButtonClick()
    {
        Debug.Log("ButtonUpdate clicked!");

        if (isConnected)
        {
            //send an update request?
        }
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

        // Handshake:
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
                //Enable disconnect button once connected
                //btnDisconnect.gameObject.SetActive(true);
                break;
            }
        }


    }

    private void OnApplicationQuit()
    {
        if (isConnected)
        {
            SendDisconnectMessage();

            //stream.Close();
            //client.Close();
            client.Shutdown(SocketShutdown.Both);
            isConnected = false;
        }
    }
}
