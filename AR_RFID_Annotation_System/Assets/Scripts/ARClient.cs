using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Net;
using Unity.VisualScripting;
using TMPro;    //include to interact with the TextMeshPro textbox
using UnityEngine.UI;
using System;
using UnityEngine.XR.ARFoundation;
//using System.Text.Json;
using ARUnity;

//[RequireComponent(typeof(ARTrackedImageManager))]
public class ARClient : MonoBehaviour
{
    // Variables:
    public GameObject annotation;
    private bool isConnected = false;
    public TMP_Text textBox;
    public Button btnConnect;
    public Button btnDisconnect;
    public Button btnUpdate;
    //private int port = 7474;
    private Socket client;
    //ARTrackedImageManager imageManager;
    public ARTrackedImage scannedMarker;

    // Start is called before the first frame update
    void Start()
    {
        //ConnectToServer();

        //Add listeners to listen for button clicks and activate the relative functions
        btnConnect.onClick.AddListener(ConnectButtonClick);
        btnDisconnect.onClick.AddListener(DisconnectButtonClick);
        btnUpdate.onClick.AddListener(UpdateButtonClick);

        //Keep disconnect and update buttons hidden until connected
        btnDisconnect.gameObject.SetActive(false);
        btnUpdate.gameObject.SetActive(false);

        //Show connect button
        btnConnect.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //Find annotation
        annotation = GameObject.FindGameObjectWithTag("modelObject");
        GameObject textBoxObject = GameObject.FindGameObjectWithTag("annotationText");
        textBox = textBoxObject.GetComponent<TMPro.TextMeshProUGUI>();
        

        //Make sure annotation always faces user:
        //Camera camera = Camera.main;
        //annotation.transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);

        //Buttons code:
        //If the annotation game object has spawned, then activate the buttons:
        if (annotation.activeSelf)
        {
            //If the client is not connected, display the connect button:
            if (!isConnected)
            {
                btnConnect.gameObject.SetActive(true);
                btnDisconnect.gameObject.SetActive(false);
                btnUpdate.gameObject.SetActive(false);
            }
            //otherwise display the update and disconnect buttons:
            else
            {
                btnConnect.gameObject.SetActive(false);
                btnDisconnect.gameObject.SetActive(true);
                btnUpdate.gameObject.SetActive(true);
            }
        }
        
    }

    void ConnectButtonClick()
    {

        ConnectToServer();
        //Debug.Log("ButtonConnect clicked!");

        //textBox.text = "ButtonConnect clicked!";
    }

    async void SendDisconnectMessage()
    {
        var disconnectMessage = "<|EOC|>";  //EOC: end of connection
        var messageBytes = Encoding.ASCII.GetBytes(disconnectMessage);
        await client.SendAsync(messageBytes, SocketFlags.None);

        textBox.text = "Disconnected";
    }

    void DisconnectButtonClick()
    {
        //Debug.Log("ButtonDisconnect clicked!");

        DisconnectFromServer();
    }

    async void UpdateButtonClick()
    {
        //Debug.Log("ButtonUpdate clicked!");

        if (isConnected)
        {
            //clear textbox:
            textBox.text = "";

            // Unit testing:
            MessagePayload unitTest = new();
            unitTest = unitTest.CreateUnitTest(unitTest);
            string unitTestMessage = JsonUtility.ToJson(unitTest);

            textBox.text = unitTestMessage;

            var unitTestMessageBytes = Encoding.ASCII.GetBytes(unitTestMessage);
            await client.SendAsync(unitTestMessageBytes, SocketFlags.None);

            //send an update request
            //MessagePayload updateRequest = new MessagePayload("Get RFID info", 1, "AR System", "Digital Twin", "5 minutes", DateTime.Now, "")

            //wait for a message back with JSON
            var buffer = new byte[1024 * 8];
            var received = await client.ReceiveAsync(buffer, SocketFlags.None);
            var responseJSON = Encoding.ASCII.GetString(buffer, 0, received);

            //interpret JSON
            //MessagePayload response = JsonUtility.FromJson<MessagePayload>(responseJSON);

            //display correct info to text box
            //For Unit Testing:
            textBox.text = responseJSON;

            //Normally:
            //string outputToTextBox = response.payloadJSON;
            //textBox.text = outputToTextBox;
        }
    }

    async void ConnectToServer()
    {
        //For testing on the same machine (local testing):
        //IPHostEntry ipHostInfoClient = Dns.GetHostEntry("localhost");
        //IPAddress ipAddressClient = ipHostInfoClient.AddressList[0];

        //For tablet:
        string ip = "10.66.178.171";
        //string ip = "192.168.1.38";
        IPAddress ipAddressClient = IPAddress.Parse(ip);

        int portClient = 7474;
        

        IPEndPoint ipEndPoint = new IPEndPoint(ipAddressClient, portClient);

        client = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        //For debugging:
        textBox.text = "Client created";

        await client.ConnectAsync(ipAddressClient, portClient);

        isConnected = client.Connected;

        //For debugging:
        if (isConnected  == false) 
        {
            textBox.text = "Connection error";
        }

        // Handshake:
        while (isConnected)
        {
            // Send test message
            var message = "I am a client that has connected <|EOM|>";
            var messageBytes = Encoding.ASCII.GetBytes(message);
            await client.SendAsync(messageBytes, SocketFlags.None);
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
            else
            {
                textBox.text += "Socket client communication not yet established...";
            }
        }


    }

    void DisconnectFromServer()
    {
        if (isConnected)
        {
            SendDisconnectMessage();

            //client.Shutdown(SocketShutdown.Both);
            isConnected = false;
        }
    }

    void OnApplicationQuit()
    {
        DisconnectFromServer();
        //client.Shutdown(SocketShutdown.Both);
        //isConnected = false;
    }

    /// <summary>
    /// Interpreting standard JSON string:
    /// Logic to interpret and decipher a received JSON.
    /// </summary>
    /// <param name="jsonString"></param>
    public void InterpretJSONString(string jsonString)
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

        payload = JsonUtility.FromJson<MessagePayload>(jsonString);

        //logic to interpret deserialized JSON message
        switch (payload?.requestType)
        {
            case "Unit Test":


                break;
            case "Request Pallet IDs":
                //interact with code that receives pallet info from PLC

                break;
            default:
                break;
        }

    }
}
