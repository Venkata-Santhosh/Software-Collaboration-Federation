/////////////////////////////////////////////////////////////////////
//  THServicesContractImpl.cs - service implementaion              //
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
This packages is responsible for providing implementation behaviors for IService Contract

Public Interface:
=================
public:
------
PostRequest - proxy objects can post requests using this method

Private functions:
=================
private:
-------

Build Process:
==============
Required files
MessageContracts,ThreadPoolMessageListener,ICommService,MessageDS

Maintenance History:
====================
ver 1.0

*/
using CommonUtilites.ICommService;
using MessageDS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestHarness.ThreadPoolAndMessageListener;
using CommonUtilites.MessageContracts;
using System.IO;
using UtilityPackages;
namespace TestHarness.ServiceContractImpl
{
    class THServicesContractImpl : IService
    {
        public Stream downloadFile(string filename)
        {
            throw new NotImplementedException();
        }

        public void postRequests(Message messageRequest)
        {
            "Requirement 4".title();
            Console.WriteLine("Enqueing Test Request into blocking queue");
            ReceiveMessageProcessor.enQueuingMessage(messageRequest);
        }

        public void uploadFile(FileTransferMessage msg)
        {
            throw new NotImplementedException();
        }
    }
}
