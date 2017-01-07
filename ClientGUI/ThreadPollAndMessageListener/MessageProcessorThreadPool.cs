//using ClientGUI.ViewModels;
//using ClientGUI.Views;
//using CommonUtilites.MessageDSUtility;
//using MessageDS;
//using SWTools;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using UtilityPackages;

//namespace Client.ThreadPoolAndMessageListener
//{
//    class MessageProcessorThreadPool
//    {

//        private BlockingQueue<Message> messageQueuing = new BlockingQueue<Message>();
//        private Thread[] threads;
//        public MessageProcessorThreadPool()
//        {

//        }
//        public MessageProcessorThreadPool(int numberOfThreads)
//        {
//            //threads = new Thread[numberOfThreads];
//            //for (int i = 0; i < numberOfThreads; i++)
//            //{
//            //    threads[i] = new Thread(threadFunc);
//            //    threads[i].Start();
//            //}
//        }
//        public void threadFunc()
//        {
//            //while (true)
//            //{
//            //    lock (messageQueuing.locker_)
//            //    {
//            //        if (messageQueuing.size() == 0)
//            //        {
//            //            Console.Write("\nwaiting" + "\n");
//            //            Monitor.Wait(messageQueuing.locker_);
//            //        }
//            //        Message act = messageQueuing.deQ();
                    
//            //        if (act != null)
//            //        {
//            //            Console.Write("emssage froms erver");
//            //            Console.Write("\n Thread" + Thread.CurrentThread.ManagedThreadId);
//            //            UploadResult result = act.body.fromJson< UploadResult>();

//            //            //sRepositoryViewModel vieMode = RepositoryView.getViewModel();
//            //            //vieMode.uploadResult(result);

//            //        }
//            //    }
//            //}
//        }
//        public int size()
//        {
//            return messageQueuing.size();
//        }
//        public Message deQ()
//        {
//            return messageQueuing.deQ();
//        }
//        public void enQueuingMessage(Message action)
//        {
//            messageQueuing.enQ(action);
//        }
//    }
//    public class ReceiveMessageProcessor
//    {
//        private static int count = 0;
//        public ReceiveMessageProcessor()
//        {
            
//            Console.Write("This is message Processor Constructor \n" + count);

//        }
//        public static int size() { return pool.size(); }
//        public static Message deQ() {
            
//            Console.Write("count" + count); count++;
//            return pool.deQ(); }
//        public static void enQueuingMessage(Message action) { pool.enQueuingMessage(action); }
//        private static MessageProcessorThreadPool pool = new MessageProcessorThreadPool(5);
//    }

//}
