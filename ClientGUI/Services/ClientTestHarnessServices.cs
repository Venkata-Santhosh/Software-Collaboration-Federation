/////////////////////////////////////////////////////////////////////
//  ClientTestHarnessServices.cs - Services for repository         //
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
1. This package contains all functionality related TestHarness to client


 
Public Interface:
=================
public:
------
getTestDriverNamesFromRepo() - gets all test driver names
runTestCases() -  runs all the test cases 

Private functions:
=================
private:
-------

Build Process:
==============
Required files
UtilityPackages,MessageDS,Loader

Maintenance History:
====================
ver 1.5
*/
using ClientGUI.ThreadPollAndMessageListener;
using CommonUtilites.ICommService;
using MessageDS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityPackages;

namespace ClientGUI.Services
{
    public class ClientTestHarnessServices
    {
        public Task getTestDriverNamesFromRepo(string fromAddress,string toAddress,string author)
        {
           return  Task.Run(() => {
               Message message = new Message();
               message.to = toAddress;
               message.from = fromAddress;
               message.author = author;
               message.type = "TestDriverNamesRequest";
               message.body = "";
               SendMessageProcessor.enQueuingMessage(message);
           });
        }
        public Task runTestCases(Message message)
        {
            return Task.Run(() => {
                "Test Run request".title();
                Console.WriteLine(message.ToXml());
                SendMessageProcessor.enQueuingMessage(message);
            });
        }
#if (Test_RepoServices)
         static void Main(string[] args)
            {
                 SendMessageProcessor.enQueuingMessage(requestMessage);
            }
#endif
    }
}
