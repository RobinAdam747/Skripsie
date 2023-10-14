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
            string expiry_, string sendTime_, string requestType_, string payloadJSON_)
        {

        }

        public MessagePayload()
        {

        }


        // Methods //

        /// <summary>
        /// Unit Test:
        /// A unit test example JSON for testing the JSON deserializing and interpreting methods and functionality
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

        public MessagePayload CreateUnitTest(MessagePayload unitTest)
        {
            unitTest.conversationID = "Unit Test Conversation";
            unitTest.versionNumber = 1;
            unitTest.sourceID = "AR System";
            unitTest.destinationID = "Digital Twin";
            unitTest.expiry = "5 minutes";
            unitTest.sendTime = DateTime.Now.ToString();
            unitTest.requestType = "Unit Test";
            unitTest.payloadJSON = "Unit Test Payload";

            return unitTest;
        }

        public MessagePayload CreateUnitTestReply(MessagePayload unitTestReply)
        {
            unitTestReply.conversationID = "Unit Test Conversation";
            unitTestReply.versionNumber = 1;
            unitTestReply.sourceID = "Digital Twin";
            unitTestReply.destinationID = "AR System";
            unitTestReply.expiry = "5 minutes";
            unitTestReply.sendTime = DateTime.Now.ToString();
            unitTestReply.requestType = "Unit Test";
            unitTestReply.payloadJSON = "Unit Test Payload";

            return unitTestReply;
        }

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
    }




}
