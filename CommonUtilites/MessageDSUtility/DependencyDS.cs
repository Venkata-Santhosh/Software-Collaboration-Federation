/////////////////////////////////////////////////////////////////////
//  DependencyDS.cs - Dependency Data structure                      //
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
This package is contains data contracts for dependency files (Repository dependency metatdata)
{
            "basepath": "..\\project1\\testdrivers\\", 
            "fileName": "TestDriver1.dll",
            "version": "1.0",
            "author": "santhosh",
            "description": "this is sample test driver",
            "dependencies": [{
                "basepath": "..\\project1\\codeToTest\\", "fileName": "codeToTest1.dll",
                "version": "1.1.1"
            }, {
                "basepath": "..\\project1\\codeToTest\\", "fileName": "codeToTest2.dll",
                "version": "1.0"
            }] 
       }
Public Interface:
=================
public:
------

Private functions:
=================
private:
-------

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
using System.Runtime.Serialization;
using UtilityPackages;
namespace MessageDS
{

    [DataContract]
    public class DependencyDS
    {
        [DataMember]
        public List<TestDriverMetaData> metaData = new List<TestDriverMetaData>();
        [DataMember(Order = 0)]
        public string author { get; set; }
        [DataMember(Order = 1)]
        public string basePath { get; set; }
    }
    [DataContract]
    public class TestDriverMetaData
    {
        
        [DataMember(Order = 0)]
        public string testDriverName { get; set; }
        [DataMember(Order = 1)]
        public string version { get; set; }

        [DataMember(Order = 2)]
        public string description { get; set; }
        [DataMember(Order = 3)]
        public List<Dependencies> dependencies = new List<Dependencies>();
    }
    [DataContract]
    public class Dependencies
    {
        //[DataMember(Order = 0)]
        //public string basePath { get; set; }
        [DataMember(Order = 1)]
        public string dependencyFileName { get; set; }
        [DataMember(Order = 2)]
        public string version { get; set; }

    }

    public class Demo
    {
        static void Main(string[] args)
        {

       //     "basepath": "..\\project1\\testdrivers\\", 
       //     "fileName": "TestDriver1.dll",
       //     "version": "1.0",
       //     "author": "santhosh",
       //     "description": "this is sample test driver",
       //     "dependencies": [{
       //         "basepath": "..\\project1\\codeToTest\\", "fileName": "codeToTest1.dll",
       //         "version": "1.1.1"
       //     }, {
       //         "basepath": "..\\project1\\codeToTest\\", "fileName": "codeToTest2.dll",
       //         "version": "1.0"
       //     }] 
       //}
            DependencyDS dss = new DependencyDS();
            dss.author = "santhos";
            TestDriverMetaData ds = new TestDriverMetaData();
            dss.basePath = "..\\project1\\testdrivers\\";
            ds.testDriverName = "TestDriver1.dll";
            ds.version = "1.0";
            
            ds.description = "this is sample test driver";

            Dependencies dep = new Dependencies();
            //dep.basePath = "\\codetotest";
            dep.dependencyFileName = "codeToTest1.dll";
            dep.version = "2.1";

            ds.dependencies.Add(dep);
            dss.metaData.Add(ds);
            
            Console.Write(dss.author); 
            Console.Write(dss.toJson());
            string da = dss.toJson();

            DependencyDS data = da.fromJson<DependencyDS>();

            //Console.Write(data.metaData[0].basePath);
        }
    }
}
