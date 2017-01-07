/////////////////////////////////////////////////////////////////////
//  TestRequestResult.cs - Json in object format                   //
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
This package is to used to store json in the form of objects 

Json will be of this format
    // {listOfTestRequest = [{
    //	"testrequest": "testrequest1",
    //	"author": "santhosh",
    //	"TimeStamp": "2016:20:20",
    //	"results": [{
    //		"testDriver": [{
    //			"testDriverName": "Name",
    //			"testcases": [{
    //				"testName": "testName",
    //				"testResult": "true",
    //				"logs": ""
    //			}, {
    //				"testName": "testName",
    //				"testResult": "true",
    //				"logs": ""
    //			}]
    //		}]
    //	}]
    //}]}

Public Interface:
=================
public:
------

Build Process:
==============
Required files
- TestRequestResult.cs,BlockingQueue.cs

Maintenance History:
====================
ver 1.0

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestHarnessUtility
{
    
    [Serializable]
    [DataContract]
    public class ResultStroage
    {
        [DataMember]
        public List<TestRequestResult> testRequestResultStorage = new List<TestRequestResult>();
        [Serializable]
        [DataContract]
        public class TestRequestResult
        {
            [DataMember]
            public string testRequestName { get; set; }
            [DataMember]
            public string author { get; set; }
            [DataMember]
            public DateTime timeStamp { get; set; }
            [DataMember]
            public List<TestDriverResults> listOfTestDriverResult { get; set; } = new List<TestDriverResults>();
            [Serializable]
            [DataContract]
            public class TestCase
            {
                [DataMember]
                public string testName { get; set; }
                [DataMember]
                public string testResult { get; set; }
                [DataMember]
                public string logs { get; set; }
            }
            [Serializable]
            [DataContract]
            public class TestDriverResults
            {
                [DataMember]
                public string testDriverName { get; set; }
                [DataMember]
                public List<TestCase> listOfTestCaseResults { get; set; } = new List<TestCase>();
            }
            
        }
#if (Test_TestRequestResult)
         static void Main(string[] args)
            {
                Storage storage = new Storage();
                ResultStroage result = storage.getResultsFromStorage();
                foreach (TestRequestResult requestResult in result.testRequestResultStorage)
                {
                    Console.Write("\n {0,-12} :{1}", "Author", requestResult.author);
                    Console.Write("\n {0,-12} :{1}", "Test Request Name", requestResult.testRequestName);
                    Console.Write("\n {0,-12} :{1}", "TimeStamp", requestResult.timeStamp);

                    foreach (TestRequestResult.TestDriverResults driverResults in requestResult.listOfTestDriverResult)
                    {
                        Console.Write("\n {0,-12} :{1}", "DriverName", driverResults.testDriverName);
                        foreach (TestRequestResult.TestCase testCase in driverResults.listOfTestCaseResults)
                        {
                            Console.Write("\n {0,-12} :{1}", "Test Case Name", testCase.testName);
                            Console.Write("\n {0,-12} :{1}", "Test case result", testCase.testResult);
                        }
                    }

                }
            }
#endif
    }
}
