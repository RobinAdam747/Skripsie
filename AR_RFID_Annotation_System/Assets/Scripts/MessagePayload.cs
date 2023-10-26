using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
//using System.Text.Json;
using System.Threading.Tasks;

namespace ARUnity
{
    public class MessagePayload
    {
        // Variables (Attributes):
        public string? conversationID;
        public int versionNumber;
        public string? sourceID;
        public string? destinationID;
        public string? expiry;
        public string? sendTime;
        public string? requestType;
        public string? payloadJSON;   

        /// <summary>
        /// Parameterised constructor.
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
            string expiry_, string sendTime_, string requestType_, string payloadJSON_)
        {
            conversationID = conversationID_;
            versionNumber = versionNumber_;
            sourceID = sourceID_;
            destinationID = destinationID_;
            expiry = expiry_;
            sendTime = sendTime_;
            requestType = requestType_;
            payloadJSON = payloadJSON_;
        }

        public MessagePayload()
        {

        }


        // Methods //

        /// <summary>
        /// A unit test JSON for testing the JSON deserializing and interpreting methods and functionality
        /// </summary>
        /// <returns> a MessagePayload type containing the deserialized data </returns>
        public MessagePayload UnitTest()
        {
            MessagePayload unitTest = new MessagePayload(
                conversationID_: "Unit Test Conversation",
                versionNumber_: 1,
                sourceID_: "AR System",
                destinationID_: "Digital Twin",
                expiry_: "5 minutes",
                sendTime_: DateTime.Now.ToString(),
                requestType_: "Unit Test",
                payloadJSON_: "Unit Test Payload");

            return unitTest;
        }

        /// <summary>
        /// A unit test JSON message sent as a reply from the digital twin to the AR system.
        /// </summary>
        /// <returns></returns>
        public MessagePayload UnitTestReply()
        {
            MessagePayload unitTest = new MessagePayload(
                conversationID_: "Unit Test Conversation",
                versionNumber_: 1,
                sourceID_: "Digital Twin",
                destinationID_: "AR System",
                expiry_: "5 minutes",
                sendTime_: DateTime.Now.ToString(),
                requestType_: "Unit Test",
                payloadJSON_: "Unit Test Payload");

            return unitTest;
        }

        public MessagePayload CreateUnitTest(MessagePayload unitTest)
        {
            unitTest = UnitTest();

            return unitTest;
        }

        public MessagePayload CreateUnitTestReply(MessagePayload unitTestReply)
        {
            unitTestReply = UnitTestReply();

            return unitTestReply;
        }
    }




}
