/////////////////////////////////////////////////////////////////////
//  RepoServices.cs - Services for repository                     //
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
1. This package contains all functionality related Repository to client


 
Public Interface:
=================
public:
------
createMessageBody() - create message body 
createMessage() -  Create Message
sendUploadRequest() - sends upload request to repository server

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
using CommonUtilites.MessageContracts;
using CommonUtilites.ICommService;
using UtilityPackages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageDS;
using System.Reflection;
using ClientGUI.ThreadPollAndMessageListener;
using System.Security.Policy;
using CommonUtilites.Loader;
using System.Runtime.Remoting;

namespace Client.Services
{
    public interface IRepoServices
    {
        Task<string> createMessageBody(string directoryPath, List<string> selectedFiles, string authorName);
        Task<Message> createMessage(string messageBody,string fromAddress, string toAddress, string authorName);
        void sendUploadRequest(Message requestMessage);
    }
    public class RepoServices : IRepoServices
    {
       
        public Task<Message> createMessage(string messageBody,string fromAddress, string toAddress,string authorName)
        {
            return Task.Run(() =>
            {
                "Creating message data structure for upload request".title();
                Message message = new Message();
                message.author = authorName;
                message.body = messageBody;
                message.to = toAddress;         //"http://localhost:8080/RepoServices";
                message.from = fromAddress;
                message.type = "UploadRequest";
                Console.WriteLine(message.ToXml());
                return message;
            });
           
        }

        public Task<string> createMessageBody(string directoryPath, List<string> selectedFiles,string authorName)
        {
            return Task.Run(()=> {
                AppDomainSetup appDomainSetup = new AppDomainSetup();
                appDomainSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                Evidence evidence = AppDomain.CurrentDomain.Evidence;
                AppDomain childAppDomain = AppDomain.CreateDomain("dep", evidence, appDomainSetup);
               // childAppDomain.Load("CommonUtilities");
                ObjectHandle oh = childAppDomain.CreateInstance(typeof(LoaderDep).Assembly.FullName, typeof(LoaderDep).FullName);
                LoaderDep loaderProxy = oh.Unwrap() as LoaderDep;
                string messageBody = loaderProxy.loadAllAssemblies(directoryPath, selectedFiles,authorName);
                AppDomain.Unload(childAppDomain);
                return messageBody;
                
            });
           
        }
        
        public void sendUploadRequest(Message requestMessage)
        {
            "Enqueing to Client Sender Blocking Queue".title();
            SendMessageProcessor.enQueuingMessage(requestMessage);
        }
#if (Test_RepoServices)
         static void Main(string[] args)
            {
                 SendMessageProcessor.enQueuingMessage(requestMessage);
            }
#endif
    }
}
