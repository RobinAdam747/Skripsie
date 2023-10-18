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
        var messageBytes = Encoding.UTF8.GetBytes(disconnectMessage);
        await client.SendAsync(messageBytes, SocketFlags.None);

        textBox.text = "Disconnected";
    }

    void DisconnectButtonClick()
    {
        //Debug.Log("ButtonDisconnect clicked!");

        DisconnectFromServer();
    }

    void UpdateButtonClick()
    {
        //Debug.Log("ButtonUpdate clicked!");

        if (isConnected)
        {
            //clear textbox:
            textBox.text = "";

            // Operation mode: 0 for unit testing, 1 for regular operation, 2 for full system integration test
            int mode = 2;

            switch (mode)
            {
                case 0:
                    // Unit testing:
                    SendAndReceiveUnitTestMessage();

                    break;

                case 1:
                    SendandReceiveDigitalTwinMessage("Request RFID Data", "RFID Data Exchange", "Fetching RFID data...");

                    break;
                case 2:
                    SendandReceiveDigitalTwinMessage("Integration Test", "Full System Integration Test"
                        , "Beginning full system integration test...");
                    break;                  

                default:
                    break;
            }         
        }
    }

    async void SendAndReceiveUnitTestMessage()
    {

        //Create unit test message
        MessagePayload unitTest = new();
        unitTest = unitTest.CreateUnitTest(unitTest);
        string unitTestMessage = JsonUtility.ToJson(unitTest);

        //Show unit test message on tablet screen
        textBox.text = unitTestMessage;

        //Send unit test message
        var unitTestMessageBytes = Encoding.UTF8.GetBytes(unitTestMessage);
        await client.SendAsync(unitTestMessageBytes, SocketFlags.None);

        //Wait for a message back with JSON
        var bufferUnitTest = new byte[1024 * 8];
        var receivedUnitTest = await client.ReceiveAsync(bufferUnitTest, SocketFlags.None);
        var responseJSONUnitTest = Encoding.UTF8.GetString(bufferUnitTest, 0, receivedUnitTest);

        //Display full JSON for unit test purposes
        textBox.text = responseJSONUnitTest;
    }

    async void SendandReceiveDigitalTwinMessage(string requestType, string conversationID, string progressUpdate)
    {
        //Create a message to request the RFID info
        MessagePayload messageToDT = new MessagePayload(conversationID_: conversationID, versionNumber_: 1, sourceID_: "AR System",
            destinationID_: "Digital Twin", expiry_: "5 minutes", sendTime_: DateTime.Now.ToString(), 
            requestType_: requestType, payloadJSON_: "");
        string messageToDTString = JsonUtility.ToJson(messageToDT);

        //Display message of progress to user
        textBox.text = progressUpdate;

        //Send request
        var messagetoDTStringBytes = Encoding.UTF8.GetBytes(messageToDTString);
        await client.SendAsync(messagetoDTStringBytes, SocketFlags.None);

        //Wait for response message back
        var buffer = new byte[1024 * 8];
        var received = await client.ReceiveAsync(buffer, SocketFlags.None);
        var response = Encoding.UTF8.GetString(buffer, 0, received);

        //Display payload to user
        MessagePayload responseDeserialized = JsonUtility.FromJson<MessagePayload>(response);
        string outputToTextBox = responseDeserialized.payloadJSON;
        textBox.text = outputToTextBox;
    }

    async void ConnectToServer()
    {
        //For testing on the same machine (local testing):
        //IPHostEntry ipHostInfoClient = Dns.GetHostEntry("localhost");
        //IPAddress ipAddressClient = ipHostInfoClient.AddressList[0];

        //For tablet:
        //string ip = "10.66.178.171";      //Eduroam testing with DT on laptop
        //string ip = "192.168.1.38";       //DT at home
        string ip = "146.232.146.236";      //Lab DT on VR lab computer
        IPAddress ipAddressClient = IPAddress.Parse(ip);

        int portClient = 47474;
        

        IPEndPoint ipEndPoint = new IPEndPoint(ipAddressClient, portClient);

        client = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        //For debugging:
        textBox.text = "Client created, Endpoint: " + ipEndPoint;

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
            var messageBytes = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(messageBytes, SocketFlags.None);
            //Debug.Log($"Socket client sent message: \"{message}\"");
            textBox.text = $"Socket client sent message: \"{message}\"\n";
            //Console.WriteLine($"Socket client sent message: \"{message}\"");
            //UpdateStatus($"Socket client sent message: \"{message}\"");

            // Receive acknoledgement
            var buffer = new byte[1024];
            var received = await client.ReceiveAsync(buffer, SocketFlags.None);
            var response = Encoding.UTF8.GetString(buffer, 0, received);

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
            case "Request RFID Data":
                //interact with code that receives pallet info from PLC

                break;
            default:
                break;
        }

    }
}
