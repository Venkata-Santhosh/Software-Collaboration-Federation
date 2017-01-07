///////////////////////////////////////////////////////////////////// 
//  ITest.cs                                                     //
//  ver 1.0                                                        //
//  Language:      Visual C#  2015                                 //
//  Platform:      Windows 7                                       //
//  Application:   AppDomainDemo, FL16                             //
//  Author:        Venkata Santhosh Piduri, Syracuse University    //
//                 (315) 680-6834, vepiduri@syr.edu                //
/////////////////////////////////////////////////////////////////////

/*
Module Operations:
==================
This package has been created for ITest interface 

contains two methods:
test() - developers who write test cases have to implement this method and write test cases
getTestDriverlogs() - returns all the test logs related to testdriver

Maintenance History:
====================
ver 1.0

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITest
{
    public interface ITest
    {
        bool test();
        string getTestDriverlogs();
    }
    class ITestDemo : ITest
    {
        public string getTestDriverlogs()
        {
            return "s";
        }

        public bool test()
        {
            return true;
        }
#if (Test_Itest)
         
        static void Main(string[] args)
        {
            ITest test = new ITestDemo();
            test.getTestDriverlogs();
        }
#endif
    }
}

