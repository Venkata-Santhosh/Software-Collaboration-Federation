/////////////////////////////////////////////////////////////////////
//  CodeToTest.cs -      Codeto test module                        //
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
This package contains all the code that to be tested by testharess


Public Interface:
=================
public:
------
toTest() which accepts lambda expressions and execute those expressions.

Build Process:
==============
Required files

Maintenance History:
====================
ver 1.0

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeToTeTest
{
    public class TestCode
    {
        public static bool toTest(System.Action a)
        {
            bool result = false;
            try
            {
                a.Invoke();
                result =  true;
            }
            catch (Exception ex)
            {
                Console.Write("\n  Exception caught in child domain: {0}", ex.Message);
                result = false;
            }
            return result;
        }
#if (Test_CodeToTest)
         
        static void Main(string[] args)
        {
            toTest(() => { Console.Write("This is toTestMethod in TestCode Class"); });
        }
#endif
    }
}
