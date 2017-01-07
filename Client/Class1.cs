//using Client.ServiceContractImpl;
using Client.ServiceContractImpl;
using Client.Services;
using MessageDS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UtilityPackages;
using ClientGUI.ThreadPollAndMessageListener;
using System.Threading;

namespace Client
{
    public class Class1
    {
        ServiceHost host = null;
        private string location = "../../../testexecutivefiles";
        THClientServicesContractImpl ser;
        public Class1()
        {
            ser = new THClientServicesContractImpl();
        }
        static void Main(string[] args)
        {
            "1. Requirement ".title(true);
            Console.WriteLine("(1) Shall be implemented in C# using the facilities of the .Net Framework Class Library and Visual Studio 2015, as provided in the ECS clusters.");
            Class1 cl = new Class1();
           
            Thread thread = new Thread(cl.loadResults);
            thread.Start();
           // cl.createChannel();
            cl.createMessageForSendFilesToRepo();
           
        }
        public void loadResults()
        {
            while (true)
            {
                Message dequeuedMessage = ser.deQUeu();
                Console.Write(dequeuedMessage.body);
            }
        }
        public  void createMessageForSendFilesToRepo()
        {

            "2. Requirement".title(true);
            string uploadRequestFile = Path.Combine(location, "uploadRequest.xml");
            string testRequestInStringFormat = File.ReadAllText(uploadRequestFile);
            Message message = testRequestInStringFormat.FromXml<Message>();
            Console.Write(message.author);

            SendMessageProcessor.enQueuingMessage(message);


        }
        //public Message createMessage()
        //{
        //    Message message = new Message();
        //    message.author = "Santhosh";
        //    message.to = "http://localhost:8082/TestHarnessServices";
        //    message.from = "http://localhost:8083/ClientServices";
        //    message.type = "TestRequest";
        //    TestRequest request = new TestRequest();

        //    TestElement element = new TestElement();
        //    element.testName = "Test1";
        //    element.testDriver = "TestDriver1.dll";
        //    element.testCodes.Add("CodeToTest.dll");

        //    request.tests.Add(element);
        //    message.body = request.ToXml();
        //    "2. Requirement".title(true);
        //    Console.WriteLine("Test Request message - following created message will be sent to Test Harness server for preforming tests");
        //    Console.WriteLine(message.ToXml());
        //    return message;
        //}
        

    }
}
