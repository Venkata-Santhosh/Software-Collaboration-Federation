/////////////////////////////////////////////////////////////////////
//  TestHarnessRepoServices.cs - Repository Servcies                  //
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
Test Harness uses the package for downloading files, storing results in repository


Public Interface:
=================
public:
------
downloadTestRelatedFiles  - download given files to specified path 
storeResultsInJSONFormat - store test results in json format
Private functions:
=================
private:
-------

Build Process:
==============
Required files
ICommService, MessageDS,ThreadPoolAndMessageListener,TestHarnessUtility,UtilityPackages
Maintenance History:
====================
ver 1.3

*/
using CommonUtilites.ICommService;
using MessageDS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestHarness.ThreadPoolAndMessageListener;
using TestHarnessUtility;
using UtilityPackages;

namespace TestHarness.Services
{
    public interface ITestHarnessRepoServices
    {
        bool downloadTestRelatedFiles(TestRequest request,string downloadTo);
        void storeResultsInJSONFormat(ResultStroage results);
    }
    public class TestHarnessRepoServices : ITestHarnessRepoServices
    {
        IService proxy = PeerConnection.CreateProxy<IService>("http://localhost:"+ TestHarnessHost.repositoryPortNumber+ "/RepoServices");

        private void download(string downloadTo,string fileName)
        {
            int BlockSize = 1024;
            byte[] block;
            block = new byte[BlockSize];
            int totalBytes = 0;
            string rfilename = Path.Combine(downloadTo, fileName);
            if (!Directory.Exists(downloadTo))
                Directory.CreateDirectory(downloadTo);
            if (!File.Exists(rfilename))
            {
                Stream stream = proxy.downloadFile(fileName);
                using (var outputStream = new FileStream(rfilename, FileMode.Create))
                {
                    while (true)
                    {
                        int bytesRead = stream.Read(block, 0, BlockSize);
                        totalBytes += bytesRead;
                        if (bytesRead > 0)
                            outputStream.Write(block, 0, bytesRead);
                        else
                            break;
                    }
                }

            }
        }

        public bool downloadTestRelatedFiles(TestRequest request, string downloadTo)
        {
            bool result = true;
           try { 
          foreach(TestElement element in request.tests)
            {
                download(downloadTo,element.testDriver);
                foreach(string testCode in element.testCodes)
                {
                    download(downloadTo, testCode);
                }
            }
            }catch(Exception e)
            {
                result = false;
                "Requirement 3".title();
                Console.WriteLine("Exception file Not found");
            }
            return result;
        }

        public void storeResultsInJSONFormat(ResultStroage results)
        {
            " Requirement 7 Storing Test Request Results in Repository".title();
            Message message = new Message();
            message.author = "TestHarness";
            message.type = "StoreTestRequestResults";
            message.to = "http://localhost:"+TestHarnessHost.repositoryPortNumber+"/RepoServices";
            message.from = "http://localhost:"+TestHarnessHost.testHarnessPortNumber+"/TestHarnessServices";
            message.body = results.toJson();

            SendMessageProcessor.enQueuingMessage(message);
            
        }
#if (Test_TestHarnessRepoServices)
         static void Main(string[] args)
            {
               TestHarnessRepoServices services =new TestHarnessRepoServices();
                
            }
#endif
    }
}
