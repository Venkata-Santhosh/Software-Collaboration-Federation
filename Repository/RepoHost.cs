/////////////////////////////////////////////////////////////////////
//  RepoHost.cs -RepoHost Host                          //
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
This package is used to create WCF Channel for Repository Server

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
using Repository.ServiceContractImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UtilityPackages;

namespace Repository
{
    public class RepoHost
    {
        static void Main(string[] args)
        {
            ServiceHost host = null;
            try
            {
                host = PeerConnection.CreatePeerChannel("http://localhost:"+args[0]+"/RepoServices", typeof(RepositoryServiceContractImpl));
                host.Open();
                Console.Write("\n  Started Repo BasicService - Press key to exit:\n");
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
