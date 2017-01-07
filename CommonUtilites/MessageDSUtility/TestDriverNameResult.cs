/////////////////////////////////////////////////////////////////////
//  TestDriverNameResult.cs -Test Driver name result ds            //
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
Repository used this class for sending testdrivernames to client 

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
using MessageDS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilites.MessageDSUtility
{
    [DataContract]
    public class TestDriverNameResult
    {
        [DataMember]
        public List<string> listOfTestDrivers { get; set; } = new List<string>();
        [DataMember]
        public DependencyDS depdencyDS { get; set; } = new DependencyDS();

#if (Test_TestDriverNameResult)
         
        static void Main(string[] args)
        {
           TestDriverNameResult nameResult = new TestDriverNameResult();
           nameResult.listOfTestDrivers.Add("danthosh");
            
        }

#endif
    }
}
