/////////////////////////////////////////////////////////////////////
//  TestDriver1.cs - This  is testdrivers package                   //
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

Contains testdriver1,testdriver2,testdriver3,testdriver5 classes to perform test cases on code to test

each and every test driver implements Itest interface 

Build Process:
==============
Required files
- Itest.cs,CodeToTest1.cs

Maintenance History:
====================
ver 1.0

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITest;


namespace TestDriver
{
    using CodeToTeTest;
    public class TestDriver1 : ITest.ITest
    {
        private StringBuilder logs;
        public TestDriver1()
        {
            logs = new StringBuilder();
        }
        public string getTestDriverlogs()
        {
            return logs.ToString();
        }
        //normal test case
        public bool test()
        {
            bool result = false;
            logs.Append("\n entering into TestDriver1 - > test() method");
            result =  TestCode.toTest(() => { logs.Append("\n demo for normal execution"); });
            logs.Append("\n Result :- " + result );
            return result;
        }

    }
    public class TestDriver2 : ITest.ITest
    {
        private StringBuilder logs;
        public TestDriver2()
        {
            logs = new StringBuilder();
        }
        public string getTestDriverlogs()
        {
            return logs.ToString();
        }

        public bool test()
        {
            bool result = false;
            logs.Append("\n entering into TestDriver2 - > test() method");
            logs.Append("\n tests how test harness handles throws and catches instances of exception");
            Action act = () =>
            {
                //Console.Write("\n  hello2 throws and catches instance of Exception");
                throw (new Exception("Code throws exception"));
            };
            result = TestCode.toTest(act);
            logs.Append("\n Result :- " + result );
            return result;
        }

    }
    public class TestDriver12 : ITest.ITest
    {
        private StringBuilder logs;
        public TestDriver12()
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
    public class TestDriver3 : ITest.ITest
    {
        private StringBuilder logs;
        public TestDriver3()
        {
            logs = new StringBuilder();
        }
        public string getTestDriverlogs()
        {
            return logs.ToString();
        }
        // demonstrate how test harness handles exception thrown by testdriver  that is not catched 
        public bool test()
        {
            bool result = false;
            logs.Append("\n entering into TestDriver3 - > test() method");
            logs.Append("\n throws instance of Exception but does not catch");
            logs.Append("\n Result :- " + result );
            throw (new Exception("test throws exception"));

        }

    }

    public class TestDriver4 : ITest.ITest
    {
        private StringBuilder logs;
        public TestDriver4()
        {
            logs = new StringBuilder();
        }
        public string getTestDriverlogs()
        {
            return logs.ToString();
        }
        // Demonstrates how test harness handles divide by zero that is catched in code to test
        public bool test()
        {
            bool result = false;
             logs.Append("\n entering into TestDriver4 - > test() method");
             logs.Append("\n how test harness handles divide by zero that is catched in codetotest");

            Action act = () =>
            {
                //Console.Write("\n  hello4 divides by zero and attempts to catch exception");
                int x = 0;
                int y = 1 / x;
            };
            result = TestCode.toTest(act);
            logs.Append("\n Result :- " + result);

           // Console.Write(logs.ToString());
            return result;

        }

    }
    public class TestDriversDemo
    {
#if (Test_TestDriver1)
        static void Main(string[] args)
        {
            ITest.ITest testDriver = new TestDriver2();
            testDriver.test();
            Console.Write("\n logs" + testDriver.getTestDriverlogs());
        }
#endif

    }
}
