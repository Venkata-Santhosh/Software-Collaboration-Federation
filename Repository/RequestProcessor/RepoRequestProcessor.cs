/////////////////////////////////////////////////////////////////////
//  RepoRequestProcessor.cs - request processor                    //
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
This packages will process all the request coming to repository server

Public Interface:
=================
public:
------

Private functions:
=================
private:
-------

Build Process:
==============
Required files
MessageContracts,ThreadPoolMessageListener,ICommService,MessageDS,MessageDSUtility

Maintenance History:
====================
ver 1.0

*/
using CommonUtilites.ICommService;
using CommonUtilites.MessageDSUtility;
using MessageDS;
using Repository.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using TestHarnessUtility;
using UtilityPackages;


namespace Repository.RequestProcessor
{
    interface RepoRequestProcessor
    {
        void processRequest(string messageBody, string fromAddress);
    }

    public class DetailedLogRequest : RepoRequestProcessor
    {
        public void processRequest(string messageBody, string fromAddress)
        {
            "Detailed Log Request".title();
            
            FileStream fileStream = File.Open("../../../ResultsStorage/testResultsStorage.json", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ResultStroage), new DataContractJsonSerializerSettings
            { UseSimpleDictionaryFormat = false, DateTimeFormat = new DateTimeFormat("yyyy-MM-dd HH:mm:ss") });

            try
            {
                Message message = new Message();
                message.to = fromAddress;
                IService proxy = PeerConnection.CreateProxy<IService>(fromAddress);
                message.from = "RepoServer";
                message.author = "Repository";
                message.type = "DetaileLogRequestResults";
                
                if (fileStream.Length != 0)
                {
                    ResultStroage storage = (ResultStroage)serializer.ReadObject(fileStream);
                    message.body = storage.toJson();
                }else
                {
                    message.body = "";
                }
                Console.WriteLine("sending response to client ");
                proxy.postRequests(message);
            }
            catch (Exception e)
            {
                Console.Write("\n Exception caught " + e.Message);
            }
            finally
            {
                fileStream.Close();
                stream.Close();
            }
        }
#if (Test_RepoRequestProcessor)
         
        static void Main(string[] args)
        {
                           IService proxy = PeerConnection.CreateProxy<IService>(fromAddress);

        }

#endif
    }

    public class SimpleLogRequest : RepoRequestProcessor
    {
        public void processRequest(string messageBody, string fromAddress)
        {
            Console.WriteLine("Simple Log Request received");
            FileStream fileStream = File.Open("../../../ResultsStorage/testResultsStorage.json", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ResultStroage), new DataContractJsonSerializerSettings
            { UseSimpleDictionaryFormat = false, DateTimeFormat = new DateTimeFormat("yyyy-MM-dd HH:mm:ss") });

            try
            {
                Message message = new Message();
                message.to = fromAddress;
                IService proxy = PeerConnection.CreateProxy<IService>(fromAddress);
                message.from = "RepoServer";
                message.author = "Repository";
                message.type = "SimpleLogRequestResults";

                if (fileStream.Length != 0)
                {
                    ResultStroage storage = (ResultStroage)serializer.ReadObject(fileStream);
                    message.body = storage.toJson();
                }else
                {
                    message.body = "";

                }
                Console.WriteLine("sending response to client ");
                proxy.postRequests(message);
            }
            catch (Exception e)
            {
                Console.Write("\n Exception caught " + e.Message);
            }
            finally
            {
                fileStream.Close();
                stream.Close();
            }
        }
#if (Test_RepoRequestProcessor)
         
        static void Main(string[] args)
        {
                           IService proxy = PeerConnection.CreateProxy<IService>(fromAddress);

        }

#endif
    }
    public class StoreTestRequestResults : RepoRequestProcessor
    {
       // private string store "..\\..\\..\\RepoFileStorage"
        public void processRequest(string messageBody, string fromAddress)
        {
            Console.WriteLine("Store Test Request Results request received");
            ITestRequestResultsStorage store = new TestRequestResultsStorage();
            Console.WriteLine("storing test results in json format..");
            Console.WriteLine(messageBody.toJson());
            store.storeResults(messageBody.fromJson<ResultStroage>());
        }
#if (Test_RepoRequestProcessor)
         
        static void Main(string[] args)
        {
                           IService proxy = PeerConnection.CreateProxy<IService>(fromAddress);

        }

#endif
    }
    public class TestDriverNamesRequest : RepoRequestProcessor
    {
        //private string dependencyMetaDataFile = "..\\..\\..\\\DependencyStorage\\depend"
        public void processRequest(string messageBody, string fromAddress)
        {
            Console.WriteLine("Test Driver Name Request received");
            IService proxy = PeerConnection.CreateProxy<IService>(fromAddress);
            TestDriverNameResult testDriverNames = new TestDriverNameResult();
            

            DependencyStorage storage = new RepoDependencyStorage();
            DependencyDS depdencyDS  = storage.getTestDriverNames();

            foreach(TestDriverMetaData testDriver in depdencyDS.metaData)
            {
                testDriverNames.listOfTestDrivers.Add(testDriver.testDriverName);
            }
            testDriverNames.depdencyDS = depdencyDS;
            "TestDriver Names response".title();
            Console.Write(testDriverNames.ToXml());
            Message message = createMessage(testDriverNames.toJson(), fromAddress, "TestDriverNamesResult");
            proxy.postRequests(message);
        }
        private Message createMessage(string message, string fromAddress, string messageType)
        {
            Message messageDS = new Message();
            messageDS.to = fromAddress;
            messageDS.from = "Repository";
            messageDS.type = messageType;
            if (messageType == "TestDriverNamesResult")
            {
                messageDS.body = message;
            }

            return messageDS;
        }
#if (Test_RepoRequestProcessor)
         
        static void Main(string[] args)
        {
                           IService proxy = PeerConnection.CreateProxy<IService>(fromAddress);

        }

#endif
    }
    public class UploadRequest : RepoRequestProcessor
    {
        private string repoFileStorage = "..\\..\\..\\RepoFileStorage";
        public void processRequest(string messageBody,string fromAddress)
        {
            int BlockSize = 1024;
            byte[] block;
            block = new byte[BlockSize];
            int totalBytes = 0;

            Console.Write(messageBody+fromAddress+"\n");
            DependencyDS depdencyDS = messageBody.fromJson<DependencyDS>();
            DependencyDS finalDS = new DependencyDS();
            IService proxy = PeerConnection.CreateProxy<IService>(fromAddress);
           
            foreach (TestDriverMetaData metaData in depdencyDS.metaData)
            {
                TestDriverMetaData finalmetaData = new TestDriverMetaData();
                
                string rfilename = Path.Combine(repoFileStorage, metaData.testDriverName);
                if (!Directory.Exists(repoFileStorage))
                    Directory.CreateDirectory(repoFileStorage);
                bool exists = false;
                if (!File.Exists(rfilename))
                {
                    Stream stream = proxy.downloadFile(metaData.testDriverName);
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
                    Console.WriteLine("pulling file" + metaData.testDriverName + " from client");
                }
                else
                {
                    Console.WriteLine(" file " + metaData.testDriverName + " exists in repository ");

                    exists = true;
                }
                finalmetaData.testDriverName = metaData.testDriverName;
                finalmetaData.description = "TestDriver";
                finalmetaData.version = "1.0";

                foreach (Dependencies depFile in metaData.dependencies)
                {
                    Dependencies finalDepFiles = new Dependencies();
                    
                    string rfilename1 = Path.Combine(repoFileStorage, depFile.dependencyFileName);
                    if (!Directory.Exists(repoFileStorage))
                        Directory.CreateDirectory(repoFileStorage);
                    if (!File.Exists(rfilename1))
                    {
                        Stream stream1 = proxy.downloadFile(depFile.dependencyFileName);
                        exists = false;
                        using (var outputStream = new FileStream(rfilename1, FileMode.Create))
                        {
                            while (true)
                            {
                                int bytesRead = stream1.Read(block, 0, BlockSize);
                                totalBytes += bytesRead;
                                if (bytesRead > 0)
                                    outputStream.Write(block, 0, bytesRead);
                                else
                                    break;
                            }
                        }
                        Console.WriteLine("pulling file" + metaData.testDriverName + " from client");


                    }
                    else
                    {
                        Console.WriteLine(" file " + metaData.testDriverName + " exists in repository ");

                    }
                    finalDepFiles.dependencyFileName = depFile.dependencyFileName;
                    finalDepFiles.version = "1.0";
                    finalmetaData.dependencies.Add(finalDepFiles);
                    
                }
                if (!exists)
                {
                    finalDS.metaData.Add(finalmetaData);
                }

            }
            "Storing Dependency files in repository".title();
            DependencyStorage storage = new RepoDependencyStorage();
            if(finalDS.metaData.Count>0)
                storage.storageMessage(finalDS);

            Message message = createMessage("success",fromAddress, "UploadResult");
            "sending uploadrespose to client".title();
            Console.WriteLine(message.ToXml());
            proxy.postRequests(message);
            //process message body
            //reterive files download files from client
            //save depdendency information in file json format

        }

        private Message createMessage(string message,string fromAddress,string messageType)
        {
            Message messageDS = new Message();
            messageDS.to = fromAddress;
            messageDS.from = "Repository";
            messageDS.type = messageType;
            if(messageType == "UploadResult")
            {
                UploadResult upResult = new UploadResult();
                upResult.resultMessage = message;
                messageDS.body = upResult.ToXml();
            }
            
            return messageDS;
        }
#if (Test_RepoRequestProcessor)
         
        static void Main(string[] args)
        {
                           IService proxy = PeerConnection.CreateProxy<IService>(fromAddress);

        }

#endif
    }
}
