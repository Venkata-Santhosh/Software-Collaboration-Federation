/////////////////////////////////////////////////////////////////////
//  TestHarnessClient.cs - Repository Servcies                  //
//  ver 1.0                                                        //
//  Language:      Visual C#  2015                                 //
//  Platform:      Mac, Windows 7                                  //
//  Application:   TestHarness , FL16                              //
//  Author:        Venkata Santhosh Piduri, Syracuse University    //
//                 (315) 380-6834, vepiduri@syr.edu                //
/////////////////////////////////////////////////////////////////////

/*
Module Operations:
==================
Test Harness uses the package for sending test logs to client 

Public Interface:
=================
public:
------
sendResults - send results to client

Private functions:
=================
private:
-------

Build Process:
==============
Required files
 MessageDS,ThreadPoolAndMessageListener,TestHarnessUtility,UtilityPackages
Maintenance History:
====================
ver 1.3

*/
using MessageDS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestHarness.ThreadPoolAndMessageListener;
using TestHarnessUtility;
using UtilityPackages;

namespace TestHarness.Services
{
    public interface ITestHarnessClientServices
    {
        void sendResults(ResultStroage results,string fromAddress);
        void sendFileNotFoundErrorMessageToClient(string fromAddress);
    }
    class TestHarnessClientServices : ITestHarnessClientServices
    {
        public void sendFileNotFoundErrorMessageToClient(string fromAddress)
        {
            "Requirement 3".title();
            Message message = new Message();
            message.author = "Test Harness";
            message.to = fromAddress;
            message.from = "Test Harness";
            message.type = "FileNotFoundResult";
            message.body = "";
            SendMessageProcessor.enQueuingMessage(message);

        }

        public void sendResults(ResultStroage results,string fromAddress)
        {
            Console.Write(fromAddress);
            Message message = new Message();
            message.author = "TestHarness";
            message.to = fromAddress;
            message.from = "Test Harness";
            message.type = "TestRequestResults";
            message.body = results.toJson();
            SendMessageProcessor.enQueuingMessage(message);
        }
#if (Test_TestHarnessClientServices)
         static void Main(string[] args)
            {
               TestHarnessClientServices services =new TestHarnessClientServices();
        services.sendResults(new ResultStorage(),"adr");
                
            }
#endif
    }
}
