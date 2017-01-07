///////////////////////////////////////////////////////////////////// 
//  IServices.cs      providing service contract                   //
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
This package is providing WCF IService contract

Maintenance History:
====================
ver 1.0

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using MessageDS;
using CommonUtilites.MessageContracts;
using System.IO;

namespace CommonUtilites.ICommService
{
    [ServiceContract(Name = "IServiceContract")]
    public interface IService
    {
        [OperationContract(Name = "postRequests")]
        void postRequests(Message messageRequest);

        [OperationContract(Name ="uploadFile")]
        void uploadFile(FileTransferMessage msg);

        [OperationContract(Name ="downloadFile")]
        Stream downloadFile(string filename);

    }
}
