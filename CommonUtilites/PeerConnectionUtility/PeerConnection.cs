/////////////////////////////////////////////////////////////////////
//  PeerConnection.cs -Utiltiy Package                             //
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
This package contains utility methods for creating channel and proxy objects 

Public Interface:
=================
public:
------
 CreateProxy<C>() - creates proxy objects
 CreatePeerChannel() creates channel for WCF

Build Process:
==============
Required files

IServiceContract

References :
System.ServiceModel, 

Maintenance History:
====================
ver 1.0

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel; // WCF Service model used for creating channels and proxy objects
using CommonUtilites.ICommService;

namespace UtilityPackages
{
   public class PeerConnection
    {
        public static C CreateProxy<C>(string url)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = 50000000;
            EndpointAddress address = new EndpointAddress(url);
            ChannelFactory<C> factory = new ChannelFactory<C>(binding, address);
            return factory.CreateChannel();
        }

        public static ServiceHost CreatePeerChannel(string url, Type type)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            Uri address = new Uri(url);
            Type service = type;
            ServiceHost host = new ServiceHost(service, address);
            host.AddServiceEndpoint(typeof(IService), binding, address);
            return host;
        }
#if (Test_PeerConnection)
         
        static void Main(string[] args)
        {
                           IService proxy = PeerConnection.CreateProxy<IService>(fromAddress);

        }

#endif
    }
}
