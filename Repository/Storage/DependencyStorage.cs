/////////////////////////////////////////////////////////////////////
//  DependencyStorage.cs - stores dependency results             //
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
Repository stores dependency meta data in json format
uses blocking queue for thread safety 

Public Interface:
=================
public:
------
storageMessage - store messages
 getTestDriverNames - getting test drivernames from dependency metadata

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
using MessageDS;
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
using UtilityPackages;
namespace Repository.Storage
{
    interface DependencyStorage
    {
        void storageMessage(DependencyDS dependencyMetaInfo);
        DependencyDS getTestDriverNames();
    }

    public class RepoDependencyStorage : DependencyStorage
    {
        public DependencyDS getTestDriverNames()
        {
            DependencyDS storage =  new DependencyDS();
            FileStream fileStream = File.Open("../../../DependencyStorage/dependencyMetaData.json", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DependencyDS), new DataContractJsonSerializerSettings
            { UseSimpleDictionaryFormat = false, DateTimeFormat = new DateTimeFormat("yyyy-MM-dd HH:mm:ss") });
            try
            {
                if (fileStream.Length != 0)
                {
                    storage  = (DependencyDS)serializer.ReadObject(fileStream);
                }
                return storage;
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            finally
            {
                stream.Close();
                fileStream.Close();
            }
            return storage;
        }
        public void storageMessage(DependencyDS dependencyMetaInfo)
        {
            StorageDependency.enQueuingMessage(dependencyMetaInfo);
        }
    }


    public class StorageThreadQ
    {
        private BlockingQueue<DependencyDS> storageQ_ = new BlockingQueue<DependencyDS>();
        private Thread thread;

        public StorageThreadQ()
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
                    if(storageQ_.size()==0)
                    {
                        Monitor.Wait(storageQ_.locker_);
                    }
                    DependencyDS deQMessage = storageQ_.deQ();
                    FileStream fileStream = File.Open("../../../DependencyStorage/dependencyMetaData.json", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    MemoryStream stream = new MemoryStream();
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DependencyDS), new DataContractJsonSerializerSettings
                    { UseSimpleDictionaryFormat = false, DateTimeFormat = new DateTimeFormat("yyyy-MM-dd HH:mm:ss") });
                    try
                    {
                        if (fileStream.Length != 0)
                        {
                            DependencyDS storage = (DependencyDS)serializer.ReadObject(fileStream);
                            foreach(TestDriverMetaData data in deQMessage.metaData)
                            {
                                storage.metaData.Add(data);
                            }
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
        public void enQueuingMessage(DependencyDS action)
        {
            storageQ_.enQ(action);
        }

    }
    public class StorageDependency
    {
        public static void enQueuingMessage(DependencyDS action) { pool.enQueuingMessage(action); }
        private static StorageThreadQ pool = new StorageThreadQ();
    }
    public class Demowd
    {
#if (Test_Demow)
         
        static void Main(string[] args)
        {
                           StorageDependency.enQueuingMessage(new DependencyDS());

        }

#endif
    }
}
