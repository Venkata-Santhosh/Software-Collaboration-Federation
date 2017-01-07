/////////////////////////////////////////////////////////////////////
//  TestDriver2.cs - This  is testdrivers package                  //
//  ver 1.0                                                        //
//  Language:      Visual C#  2015                                 //
//  Platform:      Mac, Windows 7                                  //
//  Application:   TestHarness , FL16                              //
//  Author:        Venkata Santhosh Piduri,Dr.Fawcet, Syracuse University//
//                 (315) 380-6834, vepiduri@syr.edu                //
/////////////////////////////////////////////////////////////////////

/*
Module Operations:
==================
This package contains different types of test to test codetotest driver
Most of the funtionality is given by Dr.Fawcet in Application domain 3 prototype code

Contains testdriver5,testdriver6,testdriver7,testdriver8,testdriver9 classes to perform test cases on code to test

each and every test driver implements Itest interface 

Build Process:
==============
Required files
- Itest.cs,CodeToTest2.cs

Maintenance History:
====================
ver 1.0

*/
using CodeToTest2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestDriver
{
    public class TestDriver5 : ITest.ITest
    {
        private StringBuilder logs;
        public TestDriver5()
        {
            logs = new StringBuilder();
        }
        public string getTestDriverlogs()
        {
            return logs.ToString();
        }
        // divide by zero demonstration
        public bool test()
        {
            bool result = false;
            logs.Append("\n entering into TestDriver5 - > test() method");
            logs.Append("\n divides by zero but does not try to catch exception");
            logs.Append("\n Result :- " + result );
           // Console.Write(logs.ToString());
            int x = 0;
            int y = 1 / x;
            
            return result;
        }

    }
    public class TestDriver11 : ITest.ITest
    {
        private StringBuilder logs;
        public TestDriver11()
        {
            logs = new StringBuilder();
        }
        public string getTestDriverlogs()
        {
            return logs.ToString();
        }

        // sample test case
        public bool test()
        {
            bool result = true;
            Action act = () =>
            {
                System.Text.StringBuilder sb = new StringBuilder();
                sb.Append("will work");
            };
            Boolean t = TestCode.toTest(act);
            logs.Append("\nResult :- " + result);
            // Console.Write(logs.ToString()+"\n");
            return result;
        }

    }
    public class TestDriver6 : ITest.ITest
    {
        private StringBuilder logs;
        public TestDriver6()
        {
            logs = new StringBuilder();
        }
        public string getTestDriverlogs()
        {
            return logs.ToString();
        }
        // Demonstrating how test harness handles when testdriver thread is aborted using thread.abort() 
        // and catched in code to test
        public bool test()
        {
            bool result = false;
             logs.Append("\n entering into TestDriver6 - > test() method");
            
               logs.Append("\n calls thread.abort() and tries to catch exception");
            Action act = () => { Thread.CurrentThread.Abort(); };
            result  = TestCode.toTest(act);
            logs.Append("\nResult :- " + result );
           // Console.Write(logs.ToString()+"\n");

            return result;
        }

    }
    public class TestDriver7 : ITest.ITest
    {
        private StringBuilder logs;
        public TestDriver7()
        {
            logs = new StringBuilder();
        }
        public string getTestDriverlogs()
        {
            return logs.ToString();
        }
        // Demonstrating how test harness handle when testdriver thread is aborted using thread.abort() 
        public bool test()
        {
            bool result = false;
            logs.Append("\n entering into TestDriver7 - > test() method");
            logs.Append(" calls thread.abort() but does not try to catch exception");
            Thread.CurrentThread.Abort();
             logs.Append("\nResult :- " + result );
           // Console.Write(logs.ToString() + "\n");
            return result;
        }

    }
    public class TestDriver8 : ITest.ITest
    {
        private StringBuilder logs;
        public TestDriver8()
        {
            logs = new StringBuilder();
        }
        public string getTestDriverlogs()
        {
            return logs.ToString();
        }
        //test method demonstrate how test harness will handle null pointer exception in test driver
        // that is catched in code to test.
        public bool test()
        {
            logs.Append("\n entering into TestDriver8 - > test() method");
           
                logs.Append("\n Demonstrating how test harness will handle null " +
                "pointer exception in test driver that is catched in code to test");
            logs.Append("with out initializing System.Test.StringBuilder sb, Test driver is calling one of its method sb.Append()");
            Action act = () =>
            {
                System.Text.StringBuilder sb = null;
                sb.Append("won't work");
            };
            Boolean t = TestCode.toTest(act);
            logs.Append("\nResult :- " + t );
           // Console.Write(logs.ToString() + "\n");
            return t;
        }

    }
    public class TestDriver10 : ITest.ITest
    {
        private StringBuilder logs;
        public TestDriver10()
        {
            logs = new StringBuilder();
        }
        public string getTestDriverlogs()
        {
            return logs.ToString();
        }

        // sample test case
        public bool test()
        {
            bool result = true;
            Action act = () =>
            {
                System.Text.StringBuilder sb = new StringBuilder() ;
                sb.Append("will work");
            };
            Boolean t = TestCode.toTest(act);
            logs.Append("Result :- " + result);
            // Console.Write(logs.ToString()+"\n");
            return result;
        }

    }
    public class TestDriver9 : ITest.ITest
    {
        private StringBuilder logs;
        public TestDriver9()
        {
            logs = new StringBuilder();
        }
        public string getTestDriverlogs()
        {
            return logs.ToString();
        }

        //test method demonstrate how test harness will handle null pointer exception in test driver
        // that doesn't catch it.
        public bool test()
        {
            bool result = false;
             logs.Append("\n entering into TestDriver9 - > test() method");
            
                logs.Append("\n Demonstrating how test harness will handle null "+
                "pointer exception in test driverthat doesn't catch it");
            logs.Append("with out initializing System.Test.StringBuilder sb,  Test driver is calling one of its method sb.Append()");
            System.Text.StringBuilder sb = null;
            sb.Append("won't work");
            logs.Append("\nResult :- " + result );
           // Console.Write(logs.ToString()+"\n");
            return result;
        }

    }
    public class TestDriversDemo
    {
#if (Test_TestDriver2)
         static void Main(string[] args)
        {
            ITest.ITest testDriver = new TestDriver9();
            try { 
            testDriver.test();
            Console.Write("\n logs" + testDriver.getTestDriverlogs());
            }catch(Exception e)
            {
                Console.Write(e.Message);
            }
        }
#endif

    }
}
