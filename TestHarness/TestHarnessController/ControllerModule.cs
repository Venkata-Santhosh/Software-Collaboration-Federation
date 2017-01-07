/////////////////////////////////////////////////////////////////////
//  ControllerModule.cs - This  is controller for testharness      //
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
This Package will act as Controller for whole testharness project.
Main functionality of this package is to make use of all the packages to satisfy project requirements


Public Interface:
=================
public:
------
getLogsAndDisplay(LogTypes) - with the help of storage package it retrieves logs and display to user based on request
startProcessingRequests() - client will call this method to start processing there requests - test harness start processing requests
getOutBoundQueue() - controller will place all the test request related results into outbound blocking queue
                     which helps client to take response from queue.
private:
-------
parseXMLTestRequestIntoList() - this method make use of XMLParserModule Package to convert testrequest string to list of objectss
download() - startprocessing calls this method to download testrequest related files form repository.

Build Process:
==============
Required files
- Storage.cs,AppDomainModule.cs,XMLParser.cs,BlockingQueue.cs

Maintenance History:
====================
ver 1.0

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Security.Policy;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using UtilityPackages;
using TestHarness;
using MessageDS;
using CommonUtilites.FileManagerUtility;
using TestHarness.Services;
using LoaderModule;
using static TestHarnessUtility.ResultStroage;
using TestHarnessUtility;

namespace ControllerModule
{
   
    public interface THController
    {
        void startProcessingTestRequest(string testRequest,string fromAddress,string author);
    }
    public class Controller : THController
    {
        private AppDomainModule appDomainModule;
        HRTimer.HiResTimer hiResTimer = null;
        public Controller()
        {
            appDomainModule = new AppDomainModule();
            hiResTimer = new HRTimer.HiResTimer();

        }

        private TestRequest parseTestRequest(string testRequest)
        {
            TestRequest testRequestDS = testRequest.FromXml<TestRequest>();
            return testRequestDS;
        }
        public void startProcessingTestRequest(string testRequest, string fromAddress, string author)
        {
            "Requirement 12".title();
            Console.Write("HiResTimer started");
            hiResTimer.Start();
            IFileManager fileManager = new FileManager();
            ITestHarnessRepoServices thRepoService = new TestHarnessRepoServices();
            //parseTestRequest
            TestRequest testRequestDS = parseTestRequest(testRequest);
            //process 
            if (testRequestDS.tests.Count > 0)
            {
                //creating director for storing test request related files
                string authorName = author;
                Console.Write("\n\n Creating directory for each test request to store request related assemblies\n");

                string privateBinPath = authorName + DateTime.Now.ToFileTime();
                Console.Write("Directory name " + privateBinPath);
                //creating directory
                fileManager.createDirectory(privateBinPath);
                //downloading files
                bool downloadResult = thRepoService.downloadTestRelatedFiles(testRequestDS, privateBinPath);
                if (downloadResult) {
                    //creating child app domain 
                    AppDomain childAppDomain = appDomainModule.createAppDomain(authorName, privateBinPath);
                    LoaderProxy loaderProxy = appDomainModule.getLoaderProxyInstance();

                    //getting all files present in privatebinpath for loading into loader
                    string[] files = fileManager.getFilesInSpecifiedPath(privateBinPath, "*.dll");
                    //loading files into loader proxy
                    loaderProxy.loadAllAssemblies(files);
                    TestRequestResult results = loaderProxy.performTesting();
                    //creating content to store
                    results.author = authorName;
                    results.timeStamp = DateTime.Now;
                    string testRequestName = authorName + DateTime.Now.ToFileTime();
                    results.testRequestName = testRequestName;
                    ResultStroage storeResults = new ResultStroage();
                    storeResults.testRequestResultStorage = new List<TestRequestResult>();
                    storeResults.testRequestResultStorage.Add(results);

                    //storing results 
                    thRepoService.storeResultsInJSONFormat(storeResults);
                    Console.Write(storeResults.ToXml());
                    //sending results to client
                    ITestHarnessClientServices clientService = new TestHarnessClientServices();
                    clientService.sendResults(storeResults, fromAddress);
                    //unloading appDomain
                    " Requirement 7 - unloading child  app domain ".title();
                    appDomainModule.unloadAppDomain(childAppDomain);
                    Console.Write("\n deleting directory that is created for test request directory Name" + privateBinPath);
                    fileManager.deleteDirectory(privateBinPath);
                }
                else
                {
                    ITestHarnessClientServices clientService = new TestHarnessClientServices();
                    clientService.sendFileNotFoundErrorMessageToClient(fromAddress);
                }

                hiResTimer.Stop();
                Console.WriteLine("Time taken in Microseconds is " + hiResTimer.ElapsedMicroseconds);

            }

        }
#if (Test_ControllerModule)
         static void Main(string[] args)
            {
                
            //"../../testRequests"
            string[] files = Directory.GetFiles(args[0], "*.xml", SearchOption.AllDirectories);

            Console.Write("\n====================Client's test requests===================== \n");

            if (files.Length > 0)
            {
                BlockingQueue<String> testRequestQueue = new BlockingQueue<String>();
                foreach (string file in files)
                {
                    try {
                        Console.Write("\n ---- Test Requests-----\n");
                        string testRequestInStringFormat =  File.ReadAllText(file);
                        Console.Write("\n" + testRequestInStringFormat + "\n");
                        Console.Write("\n Enqueuing  test requests into blocking queue \n ");
                        testRequestQueue.enQ(testRequestInStringFormat);

                    }
                    catch (FileNotFoundException fnfe)
                    {
                        Console.Write("\n File Not Found" + fnfe.Message);
                    }
                    catch (Exception e)
                    {
                        Console.Write("\n exception caught" + e.Message);
                    }

                }
                THController testHarnessController = new Controller();
                testHarnessController.startProcessingRequests(testRequestQueue);

                Console.Write("\n\n================================================================\n");
                Console.Write("\n Client receiving testrequests output from outbound queue\n");
                BlockingQueue<ResultStroage> outBoundQueue = testHarnessController.getOutBoundQueue();
                DisplayModule displayModule = new DisplayModule();
                displayModule.displayResultsForTestRequests(outBoundQueue);
                Console.Write("\n-----------------------End-------------------------------------\n");
                // Reterive latest first 3 details from json storage
               
                Console.Write("\n\n==============Client Query For Simple Test Summary===============");
                testHarnessController.getLogsAndDisplay(LogTypes.SimpleTestSummaryLog);
                Console.Write("\n-----------------------End--------------------------------------\n");
            }
            else
            {
                Console.Write("\n No Test requests found in specified directory ");
            }
            }
#endif

    }

}

