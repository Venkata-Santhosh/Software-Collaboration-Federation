/////////////////////////////////////////////////////////////////////
//  ClientRepoQueryServices.cs - Services for repository            //
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
1. This package contains all functionality related to quering for test results


 
Public Interface:
=================
public:
------
simpleLogRequest() - Simple Log Request functionality - for making simple log requests
detaildLogRequest() - DetailedLog Request functionality - for making detaild log requests

Private functions:
=================
private:
-------

Build Process:
==============
Required files
SenderMessageProcessor,MessageDS

Maintenance History:
====================
ver 1.5
*/
using ClientGUI.ThreadPollAndMessageListener;
using MessageDS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilityPackages;

namespace ClientGUI.Services
{
    //this interface is query contract for client
    public interface IClientRepoQueryServices
    {
        //method for getting log requests from repository server
        Task getSimpleLogRequest(string fromAddress, string toAddress, string author);
        Task getDetailedLogRequest(string fromAddress, string toAddress, string author);
    }
    public class ClientRepoQueryServices : IClientRepoQueryServices
    {
        //Getting simplelog request implementation
        public Task getDetailedLogRequest(string fromAddress,string toAddress,string author)
        {
            return Task.Run(() => {
                //creating simple log request message 
                // this message will be sent with no body content
                Console.WriteLine("making detailed log request by using thread" + Thread.CurrentThread.ManagedThreadId);
                Message message = new Message();
                //for this message author will be Client
                message.author = author;
                //Repository address needs to keep this message generic
                message.to = toAddress;
                //client address
                message.from = fromAddress;
                //body
                message.body = "";
                message.type = "DetailedLogRequest";
                Console.WriteLine(message.ToXml());
                // using SendMessageProcessor message will be sent to repo
                SendMessageProcessor.enQueuingMessage(message);

            });
        }

        //Getting simplelog request implementation
        public Task getSimpleLogRequest(string fromAddress, string toAddress, string author)
        {
            return Task.Run(() => {
                //creating simple log request message 
                // this message will be sent with no body content
                Console.WriteLine("making simple log request by using thread" + Thread.CurrentThread.ManagedThreadId);
                Message message = new Message();
                //for this message author will be Client
                message.author = author;
                //Repository address needs to keep this message generic
                message.to = toAddress;
                //client address
                message.from = fromAddress;
                //body
                message.body = "";
                message.type = "SimpleLogRequest";
                Console.WriteLine(message.ToXml());
                // using SendMessageProcessor message will be sent to repo
                SendMessageProcessor.enQueuingMessage(message);

            });
        }
#if (Test_ClientRepoQueryServciec)
         static void Main(string[] args)
            {
                 SendMessageProcessor.enQueuingMessage(requestMessage);
            }
#endif
    }
}
