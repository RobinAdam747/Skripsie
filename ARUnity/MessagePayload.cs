using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

        /// <summary>
        /// Parameterised constructor
        /// </summary>
        /// <param name="conversationID_"></param>
        /// <param name="versionNumber_"></param>
        /// <param name="sourceID_"></param>
        /// <param name="destinationID_"></param>
        /// <param name="expiry_"></param>
        /// <param name="sendTime_"></param>
        /// <param name="requestType_"></param>
        /// <param name="payloadJSON_"></param>
        public MessagePayload(string conversationID_, int versionNumber_, string sourceID_, string destinationID_,
            string expiry_, DateTime sendTime_, string requestType_, string payloadJSON_)
        {

        }


        // Methods //

        /// <summary>
        /// Unit Test:
        /// A unit test example JSON for testing the JSON deserializing and interpreting method/s and functionality
        /// </summary>
        /// <returns> a MessagePayload type containing the deserialized data </returns>
        public static MessagePayload UnitTest()
        {
            MessagePayload unitTest = new MessagePayload(
                conversationID_: "Unit Test Conversation",
                versionNumber_: 1,
                sourceID_: "Digital Twin",
                destinationID_: "Tablet",
                expiry_: "5 minutes",
                sendTime_: DateTime.Now,
                requestType_: "Unit Test",
                payloadJSON_: "Unit Test Payload");

            return unitTest;
        }

        /// <summary>
        /// Interpreting standard JSON string:
        /// Logic to interpret and decipher a recieved JSON.
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
                sendTime_: DateTime.MinValue,
                requestType_: "",
                payloadJSON_: "");

            payload = JsonSerializer.Deserialize<MessagePayload>(jsonString);

            //logic to interpret deserialized JSON message
        }
    }





    //Todo:

    //put the code from the form here
    //include unit tests as well

    //throw new NotImplementedException ("what this code should do")



}
