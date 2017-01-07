/////////////////////////////////////////////////////////////////////
//  Message.cs -to create messages                       //
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
For creating messages  

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

namespace MessageDS
{
    [Serializable]
    public class Message
    {
        public string to { get; set; }
        public string from { get; set; }
        public string type { get; set; }
        public string author { get; set; } = "";
        public DateTime time { get; set; } = DateTime.Now;
        public string body { get; set; } = "";
        

    }
    public class MessageDeom
    {
#if (Test_MessageDeom)
         
        static void Main(string[] args)
        {
                          Message n = new Message();
                        n.to = "Da";
        }

#endif
    }
}
