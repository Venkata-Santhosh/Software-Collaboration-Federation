/////////////////////////////////////////////////////////////////////
//  TestRequestResultsStorage.cs - stores test results             //
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
Repository stores test requests in json format
uses blocking queue for thread safety 

Public Interface:
=================
public:
------
storeResults - store results 

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
using CommonUtilites.FileManagerUtility;
using SWTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestHarnessUtility;
using UtilityPackages;
namespace Repository.Storage
{
    public interface ITestRequestResultsStorage
    {
        void storeResults(ResultStroage storage);
    }
    public class TestRequestResultsStorage : ITestRequestResultsStorage
    {
        public void storeResults(ResultStroage storage)
        {
            StoreTestResults.enQueuingMessage(storage);
        }
    }
    public class StoreResultsThreadQ
    {
        private BlockingQueue<ResultStroage> storageQ_ = new BlockingQueue<ResultStroage>();
        private Thread thread;

        public StoreResultsThreadQ()
        {
            thread = new Thread(threadFunc);
            thread.Start();
        }
        public void threadFunc()
        {
            while (true)
            {
                lock (storageQ_.locker_)
                {
                    if (storageQ_.size() == 0)
                    {
                        Monitor.Wait(storageQ_.locker_);
                    }
                    "Requirement 8 - storing results with key combination of authorName and date".title();

                    ResultStroage deQMessage = storageQ_.deQ();
                    FileStream fileStream = File.Open("../../../ResultsStorage/testResultsStorage.json", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    MemoryStream stream = new MemoryStream();
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ResultStroage), new DataContractJsonSerializerSettings
                    { UseSimpleDictionaryFormat = false, DateTimeFormat = new DateTimeFormat("yyyy-MM-dd HH:mm:ss") });
                    try
                    {
                        if (fileStream.Length != 0)
                        {
                            ResultStroage storage = (ResultStroage)serializer.ReadObject(fileStream);
                            storage.testRequestResultStorage.Add(deQMessage.testRequestResultStorage[0]);
                            serializer.WriteObject(stream, storage);
                        }
                        else
                        {
                            serializer.WriteObject(stream, deQMessage);

                        }
                        stream.Position = 0;
                        StreamReader sr = new StreamReader(stream);
                        IFileManager fileStorage = new FileManager();
                        fileStorage.storeMessage(sr.ReadToEnd(), fileStream);
                    }
                    catch (Exception e)
                    {
                        Console.Write("\n Exception caught " + e.Message);
                    }

                }
            }
        }
        public void enQueuingMessage(ResultStroage action)
        {
            storageQ_.enQ(action);
        }

    }
    public class StoreTestResults
    {
        public static void enQueuingMessage(ResultStroage action) { pool.enQueuingMessage(action); }
        private static StoreResultsThreadQ pool = new StoreResultsThreadQ();
    }
    public class Demos
    {
#if (Test_Demo)
         
        static void Main(string[] args)
        {
                           StoreTestResults.enQueuingMessage(new ResultStroage());

        }

#endif
    }
}
