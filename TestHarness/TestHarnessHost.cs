/////////////////////////////////////////////////////////////////////
//  TestHarnessHost.cs -Test Harness Host                          //
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
This package is used to create WCF Channel for Test Harness Server

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
ServiceContractImpl, UtilityPackages

Maintenance History:
====================
ver 1.0

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TestHarness.ServiceContractImpl;
using UtilityPackages;

namespace TestHarness
{
    public class TestHarnessHost
    {
      
        public static string repositoryPortNumber = null;
        public static string testHarnessPortNumber = null;
        static void Main(string[] args)
        {
            testHarnessPortNumber = args[0];
            repositoryPortNumber = args[1];

            ServiceHost host = null;
            try
            {
                host = PeerConnection.CreatePeerChannel("http://localhost:"+ testHarnessPortNumber + "/TestHarnessServices", typeof(THServicesContractImpl));
                host.Open();
                Console.Write("\n  Started TestHarness Service - Press key to exit:\n");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.Write("\n\n  {0}\n\n", ex.Message);
                return;
            }
            host.Close();
        }
    }
}
