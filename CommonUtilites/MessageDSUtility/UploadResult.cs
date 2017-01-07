/////////////////////////////////////////////////////////////////////
//  uploadresult .cs - upload result           //
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
Repository used this class for sending upload operation output to client 

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

namespace CommonUtilites.MessageDSUtility
{
    [Serializable]
    public class UploadResult
    {
        public string resultMessage { get; set; }
#if (Test_UploadResult)
         
        static void Main(string[] args)
        {
            UploadResult uploadResult = new UploadResult();
            uploadResult.resultMessage = "success";
        
        }

#endif
    }
}
