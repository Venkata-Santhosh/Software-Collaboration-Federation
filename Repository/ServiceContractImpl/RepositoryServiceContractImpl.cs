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
using MessageDS;
using CommonUtilites.MessageContracts;
using System.IO;
using Repository.ThreadPoolAndMessageListener;
using UtilityPackages;
namespace Repository.ServiceContractImpl
{
    public class RepositoryServiceContractImpl : IService
    {
        int BlockSize = 1024;
        byte[] block;
        RepositoryServiceContractImpl()
        {
            block = new byte[BlockSize];
        }
        private string repositoryLocation = "..\\..\\..\\RepoFileStorage";

        public Stream downloadFile(string filename)
        {
            "downloading file".title();
            string fullyQualifiedFileName = Path.Combine(repositoryLocation, filename);
            FileStream downloadStream = null;
            if (File.Exists(fullyQualifiedFileName))
            {
                downloadStream = new FileStream(fullyQualifiedFileName, FileMode.Open);
            }
            else
            {
                throw new Exception("open failed for \"" + filename + "\"");
            }
            return downloadStream;
        }

        public void postRequests(Message messageRequest)
        {
            "Enqueuing incoming messages to blocking queue".title();
            Console.WriteLine(messageRequest.ToXml());
            ReceiveMessageProcessor.enQueuingMessage(messageRequest);
        }

        public void uploadFile(FileTransferMessage msg)
        {
            "uploading file ".title();
            string fileName = msg.filename;
            int totalBytes = 0;

            string rfilename = Path.Combine(repositoryLocation, fileName);
            if (!Directory.Exists(repositoryLocation))
                Directory.CreateDirectory(repositoryLocation);
            using (var outputStream = new FileStream(rfilename, FileMode.Create))
            {
                while (true)
                {
                    int bytesRead = msg.transferStream.Read(block, 0, BlockSize);
                    totalBytes += bytesRead;
                    if (bytesRead > 0)
                        outputStream.Write(block, 0, bytesRead);
                    else
                        break;
                }
            }
        }
#if (Test_RepositoryServiceContractImpl)
         static void Main(string[] args)
            {
        RepositoryServiceContractImpl impl = new RepositoryServiceContractImpl();
               impl.uploadFile(msg)
             
            }
#endif
    }
}
