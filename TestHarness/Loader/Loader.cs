/////////////////////////////////////////////////////////////////////
//  Loader.cs - This  is loader for testharness                    //
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
This package is used to load all the test drivers and code to test assemblies into child app domain

Public Interface:
=================
public:
------
loadAllAssemblies() - loads all the assemblies
performTesting() - perform testing using reflections 
private:
displayTestDriverLogs() - displays test driver logs

Build Process:
==============
Required files
- TestRequestResult.cs

Maintenance History:
====================
ver 1.0

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ITest;
using System.Threading;
using static TestHarnessUtility.ResultStroage;
using static TestHarnessUtility.ResultStroage.TestRequestResult;
using UtilityPackages;
namespace LoaderModule
{
    public class LoaderProxy : MarshalByRefObject
    {
        public String assemblyLookUpPath { get; set; }

        public void loadAllAssemblies(String[] files)
        {
            Console.Write("\n --------------------------------------------------------------------");
            Console.Write("\n Loading all assemblies related to test request \n");
            foreach (string file in files)
            {
                Assembly assem = Assembly.LoadFrom(file);
                AssemblyName[] names = assem.GetReferencedAssemblies();
                Console.Write("\n Loading " + assem.FullName);
                foreach (AssemblyName name in names)
                {
                    Console.Write("\n loading depedencies " + name.Name);
                }
            }
            Console.Write("\n\n Loaded successfuly");
            Console.Write("\n ---------------------------------------------------------------------");
        }


        public TestRequestResult performTesting()
        {
            "Requirement 5 ".title();
            TestRequestResult testRequestResult = new TestRequestResult();
            testRequestResult.listOfTestDriverResult = new List<TestDriverResults>();
            try
            {

                Assembly[] assembiles = AppDomain.CurrentDomain.GetAssemblies();

                foreach (Assembly assembly in assembiles)
                {
                    //Console.Write("\n assembly----------" + assembly.FullName);
                    if (assembly.FullName.IndexOf("mscorlib") != -1)
                        continue;
                    if (assembly.FullName.IndexOf("Loader") != -1)
                        continue;

                    TestDriverResults driverResults = new TestDriverResults();
                    driverResults.listOfTestCaseResults = new List<TestCase>();
                    // Console.Write("\n ass     " + assembly.FullName);
                    bool temp = false;
                    Type[] types = assembly.GetExportedTypes();
                    foreach (Type t in types)
                    {
                        TestCase testCaseResult = new TestCase();

                        ITest.ITest
                            lib = null;
                        if (t.IsClass && typeof(ITest.ITest).IsAssignableFrom(t))
                        {
                            temp = true;
                            //Console.Write("\nhello"+t.FullName +"\n assembly Name"+ t.Assembly.FullName);
                            lib = (ITest.ITest)Activator.CreateInstance(t);
                            driverResults.testDriverName = t.Assembly.FullName;
                            testCaseResult.testName = t.FullName;
                        }
                        else
                        {
                            continue;
                        }
                        try
                        {
                            testCaseResult.testResult = lib.test().ToString();
                            testCaseResult.logs = lib.getTestDriverlogs();

                            displayTestDriverLogs(lib.getTestDriverlogs());

                        }
                        catch (System.Threading.ThreadAbortException ex)  // use more explicit catch conditions first
                        {
                            testCaseResult.testResult = "False";
                            testCaseResult.logs = lib.getTestDriverlogs();
                            displayTestDriverLogs(lib.getTestDriverlogs());
                            Console.Write("\n caught ThreadAbortException in Loader " + ex.Message);
                            Thread.ResetAbort();  // if you don't reset abort will be rethrown at end of catch clause
                        }
                        catch (Exception e)
                        {
                            testCaseResult.testResult = "False";
                            testCaseResult.logs = lib.getTestDriverlogs();
                            displayTestDriverLogs(lib.getTestDriverlogs());
                            Console.Write("\n Exception caught in Loader " + e.Message);

                        }
                        driverResults.listOfTestCaseResults.Add(testCaseResult);


                    }
                    if (temp)
                    {
                        testRequestResult.listOfTestDriverResult.Add(driverResults);
                        Console.Write("\n -----------------------------------------------------------\n");

                    }

                }

            }
            catch (Exception e)
            {
                Console.Write("\n exception caught " + e);
            }
            return testRequestResult;
        }
        private void displayTestDriverLogs(string log)
        {
            Console.Write("\n\n testdriver logs :- ");
            Console.Write("\n--------------------");
            Console.Write(log);
        }
#if (Test_LoaderModule)
         static void Main(string[] args)
            {
                Loader loader = new Loader();
                loader.performTesting();
            }
#endif
    }
}



