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
download - allowing proxy objects to download files from repository.
upload - allowing proxy objects to upload files to repository
deQUeu - dequeing messages from blocking queue

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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonUtilites.MessageContracts;
using MessageDS;
using System.IO;
using System.ServiceModel;
using SWTools;

namespace Client.ServiceContractImpl
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class THClientServicesContractImpl : IService
    {
        static BlockingQueue<Message> rcvBlockingQ = null;
        public THClientServicesContractImpl()
        {
            if (rcvBlockingQ == null)
                rcvBlockingQ = new BlockingQueue<Message>();
        }

        private string localFileDirectory = "..\\..\\..\\localRepository";
        public Stream downloadFile(string filename)
        {
            Console.Write("downloadFile Function invoked");
            string fullyQualifiedFileName = Path.Combine(localFileDirectory, filename);
            Console.Write("full" + fullyQualifiedFileName);
            FileStream downloadStream = null;
            if (File.Exists(fullyQualifiedFileName))
            {
                Console.Write("full" + fullyQualifiedFileName);

                downloadStream = new FileStream(fullyQualifiedFileName, FileMode.Open);
                Console.Write("\n full");

            }
            else
            {
                Console.Write("hello this is exception");
                throw new Exception("open failed for \"" + filename + "\"");
            }
            return downloadStream;
        }

        public void postRequests(Message messageRequest)
        {
            rcvBlockingQ.enQ(messageRequest);
        }
        public Message deQUeu()
        {
            Message message = rcvBlockingQ.deQ();
            return message;
        }
        public void uploadFile(FileTransferMessage msg)
        {
            throw new NotImplementedException();
        }
#if (Test_RepoServices)
         static void Main(string[] args)
            {
                 SendMessageProcessor.enQueuingMessage(requestMessage);
            }
#endif
    }
}
