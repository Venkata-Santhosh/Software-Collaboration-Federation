///////////////////////////////////////////////////////////////////// 
//  FileTransferMessage.cs      providing service contract        //
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
Message Contract for uploading files to repository 

Maintenance History:
====================
ver 1.0

*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilites.MessageContracts
{
    [MessageContract]
    public class FileTransferMessage
    {
        [MessageHeader(MustUnderstand = true)]
        public string filename { get; set; }
        [MessageBodyMember(Order = 1)]
        public Stream transferStream { get; set; }
    }
}
