/////////////////////////////////////////////////////////////////////
//  SendMessageProcessor.cs - Message Processor                   //
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
This package is responsible for sending messages to respective servers
Contains two class
1. Thread Pool - 
   Creates 5 threads,  make them available for processing requests and sending to servers
                - Threads goes to wait state when there are no requests
                - Blocking queue is used by thread pool (threads keeps on listening to blocking queue)
2. SendMessageProcessor
   Resposible for enqueing request into blocking queue

Public Interface:
=================
public:
------
enQueuingMessage - enqueues messages into blocking queue
threadFunc - keeps on listening to blocking queue, dequeues messages when there is any requests

Private functions:
=================
private:
-------

Build Process:
==============
Required files
UtilityPackages,SWTools,MessageDS,ICommServices
Maintenance History:
====================
ver 1.0

*/
using CommonUtilites.ICommService;
using MessageDS;
using SWTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilityPackages;

namespace ClientGUI.ThreadPollAndMessageListener
{
    public class ThreadPool
    {
        private BlockingQueue<Message> messageQueuing = new BlockingQueue<Message>();
        private Thread[] threads;
        private Dictionary<string, IService> map = new Dictionary<string, IService>();
        public ThreadPool()
        {

        }
        public ThreadPool(int numberOfThreads)
        {
            threads = new Thread[numberOfThreads];
            for (int i = 0; i < numberOfThreads; i++)
            {
                threads[i] = new Thread(threadFunc);
                threads[i].Start();
            }
        }

        public void threadFunc()
        {
            while (true)
            {
                lock (messageQueuing.locker_)
                {

                    if (messageQueuing.size() == 0)
                    {
                        Monitor.Wait(messageQueuing.locker_);
                    }
                    Message act = messageQueuing.deQ();
                    string toAddress = act.to;
                    if (map.ContainsKey(toAddress))
                    {
                        "Send Message Processor ".title();
                        IService remoteRepoService = map[toAddress];
                        Console.WriteLine("\n Thread  " + Thread.CurrentThread.ManagedThreadId + "posting message to "+ toAddress);
       
                        remoteRepoService.postRequests(act);
                    }
                    else
                    {
                        "Send Message Processor ".title();
                        IService proxy = PeerConnection.CreateProxy<IService>(act.to);
                        map.Add(act.to, proxy);
                        Console.WriteLine("\n Thread  " + Thread.CurrentThread.ManagedThreadId + "posting message to " + toAddress);
                        proxy.postRequests(act);
                    }
                }
            }
        }
        public void enQueuingMessage(Message action)
        {
            messageQueuing.enQ(action);
        }

    }
    public class SendMessageProcessor
    {
        public static void enQueuingMessage(Message action) { pool.enQueuingMessage(action); }
        private static ThreadPool pool = new ThreadPool(5);
    }
    public class Demow
    {
#if (Test_Demow)
         
        static void Main(string[] args)
        {
                           SendMessageProcessor.enQueuingMessage(new Message());

        }

#endif
    }
}
