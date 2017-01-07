/////////////////////////////////////////////////////////////////////
//  MessagProcessorThreadPool.cs - Message Processor               //
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
This package is responsible for receiving messages from client 
Contains two class
1. Thread Pool - 
   Creates 5 threads,  make them available for processing requests 
                - Threads goes to wait state when there are no requests
                - Blocking queue is used by thread pool (threads keeps on listening to blocking queue)
2. ReceiveMessageProcessor
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
using ControllerModule;
using MessageDS;
using SWTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilityPackages;
namespace TestHarness.ThreadPoolAndMessageListener
{
    class MessageProcessorThreadPool
    {

        private BlockingQueue<Message> messageQueuing = new BlockingQueue<Message>();
        private Thread[] threads;
        public MessageProcessorThreadPool()
        {

        }
        public MessageProcessorThreadPool(int numberOfThreads)
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
                    while (messageQueuing.size() == 0)
                    {
                        Monitor.Wait(messageQueuing.locker_);
                    }
                    Message act = messageQueuing.deQ();
                    //string message
                    //parse the message 
                    if (act != null)
                    {
                        THController controller = new Controller();
                        "Requirement 4".title();
                        Console.WriteLine("Test request came from client " + act.from);
                        Console.WriteLine("DeQueueing Test Request and processing by Thread" + Thread.CurrentThread.ManagedThreadId);
                        controller.startProcessingTestRequest(act.body,act.from,act.author);
                    }
                }
            }
        }
        public void enQueuingMessage(Message action)
        {
            messageQueuing.enQ(action);
        }
    }
    public class ReceiveMessageProcessor
    {
        private static int count = 0;
        public ReceiveMessageProcessor()
        {
            count++;
            Console.Write("This is messageProcessor Constructor" + count);

        }
        public static void enQueuingMessage(Message action) { pool.enQueuingMessage(action); }
        private static MessageProcessorThreadPool pool = new MessageProcessorThreadPool(5);
    }
    public class Demo
    {
#if (Test_Demo)
         
        static void Main(string[] args)
        {
                           ReceiveMessageProcessor.enQueuingMessage(new Message());

        }

#endif
    }
}
