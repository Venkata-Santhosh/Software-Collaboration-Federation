using Client.ServiceContractImpl;
using ClientGUI.ThreadPollAndMessageListener;
using HRTimer;
using MessageDS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilityPackages;

namespace TestExecutivePkg
{
    class TestExecutive
    {
        ServiceHost host = null;
        private string location = "../../../testexecutivefiles";
        THClientServicesContractImpl ser;
        HiResTimer hiResTimer = null;
        public TestExecutive()
        {
            ser = new THClientServicesContractImpl();
            hiResTimer = new HiResTimer();
        }
        public static string clientPortNumber = null;
        public static string repositoryPortNumber = null;
        public static string testHarnessPortNumber = null;
        public static string clientURL = null;
        public static string testHarnessURL = null;
        public static string repoURL = null;
        static void Main(string[] args)
        {
            "Client 2 for demonstrating Test Harness works for multiple clients".title(true);
            Console.WriteLine("Initializing port numbers");
            clientPortNumber = args[0];
            testHarnessPortNumber = args[1];
            repositoryPortNumber = args[2];
            clientURL = "http://localhost:" + TestExecutive.clientPortNumber + "/ClientServices";
            testHarnessURL = "http://localhost:" + testHarnessPortNumber + "/TestHarnessServices";
            repoURL = "http://localhost:" + repositoryPortNumber + "/RepoServices";
            TestExecutive testExecutive = new TestExecutive();
            testExecutive.createChannel();
            Thread thread = new Thread(testExecutive.loadResults);
            thread.Start();
            "Requirement 1 ".title();
            Console.WriteLine("Shall be implemented in C# using the facilities of the .Net Framework Class Library and Visual Studio 2015, as provided in the ECS clusters.");
            "Requirement 10".title();
            Console.WriteLine("(2) All communication between Clients, the Test Harness, and the Repository, shall be implemented using Windows Communication Foundation (WCF) channels, passing messages that contain requests, results, and notifications.");
            testExecutive.sendingUploadRequestToRepo();
            testExecutive.sendingTestRunRequestToTestHarness();
            testExecutive.querySimpleLogs();
            testExecutive.queryDetailedLogs();
        }
        public void loadResults()
        {
            while (true)
            {
                Message dequeuedMessage = ser.deQUeu();
                Console.WriteLine("Result type"+dequeuedMessage.type);
                Console.WriteLine(dequeuedMessage.body);
            }
        }

        public void sendingUploadRequestToRepo()
        {
            hiResTimer.Start();
            "Requirement 2, Requirement 6 ".title();
            Console.WriteLine(" \n HiResTimer started");

            Console.WriteLine("Client uploading files to repository server before perform testrequests");
            string uploadRequestFile = Path.Combine(location, "uploadRequest.xml");
            string uploadRequestInStringFormat = File.ReadAllText(uploadRequestFile);
            Console.WriteLine("Upload Request Message");
            Console.WriteLine(uploadRequestInStringFormat);
            Message uploadRequestMessage = uploadRequestInStringFormat.FromXml<Message>();
            uploadRequestMessage.from = TestExecutive.clientURL;
            uploadRequestMessage.to = TestExecutive.repoURL;
            Console.WriteLine("Requesting ...");
            SendMessageProcessor.enQueuingMessage(uploadRequestMessage);

            hiResTimer.Stop();
            "Requiremnt 12".title();
            Console.WriteLine("Time taken in Microseconds is " + hiResTimer.ElapsedMicroseconds);

        }

        public void sendingTestRunRequestToTestHarness()
        {
            "Requirement 2".title();
            Console.WriteLine(" \n HiResTimer started");
            hiResTimer.Start();

            Console.WriteLine("Client making tests run request ");
            string testRunRequestFile = Path.Combine(location, "testRunRequest.xml");
            string testRunRequestInStringFormat = File.ReadAllText(testRunRequestFile);
            Console.WriteLine("Test Run Request Message");
            Console.WriteLine(testRunRequestInStringFormat);
            TestRequest testRunRequest = testRunRequestInStringFormat.FromXml<TestRequest>();
            Message testRunRequestMessage = new Message();
            testRunRequestMessage.body = testRunRequest.ToXml();
            testRunRequestMessage.from = TestExecutive.clientURL;
            testRunRequestMessage.to = TestExecutive.testHarnessURL;
            testRunRequestMessage.author = "Client 2";
            "Requirement 4".title();
            Console.WriteLine("See Test Harness Server console for ");
            SendMessageProcessor.enQueuingMessage(testRunRequestMessage);
            hiResTimer.Stop();
            Console.WriteLine("Time taken in Microseconds is " + hiResTimer.ElapsedMicroseconds);


        }
        public void querySimpleLogs()
        {
            "Requirement 9".title();
            Console.WriteLine(" \n HiResTimer started");
            hiResTimer.Start();
            Message message = new Message();
            message.author = "Client 2";
            message.to = TestExecutive.repoURL;
            message.from = TestExecutive.clientURL;
            message.type = "SimpleLogRequest";
            message.body = "";
            SendMessageProcessor.enQueuingMessage(message);
            hiResTimer.Stop();
            Console.WriteLine("Time taken in Microseconds is " + hiResTimer.ElapsedMicroseconds);

        }
        public void queryDetailedLogs()
        {
            "Requirement 9".title();
            Console.WriteLine(" \n HiResTimer started");
            hiResTimer.Start();
            Message message = new Message();
            message.author = "Client 2";
            message.to = TestExecutive.repoURL;
            message.from = TestExecutive.clientURL;
            message.type = "DetailedLogRequest";
            message.body = "";
            SendMessageProcessor.enQueuingMessage(message);
            hiResTimer.Stop();
            Console.WriteLine("Time taken in Microseconds is " + hiResTimer.ElapsedMicroseconds);

        }

        public void createChannel()
        {
            "creating channel for client 2 ".title(true);
            try
            {
                host = PeerConnection.CreatePeerChannel("http://localhost:"+TestExecutive.clientPortNumber+"/ClientServices", typeof(THClientServicesContractImpl));
                host.Open();
                Console.Write("\n  Started Client BasicService \n");

            }
            catch (Exception ex)
            {
                Console.Write("\n\n  {0}\n\n", ex.Message);
                return;
            }
        }
    }
}
