/////////////////////////////////////////////////////////////////////
//  SerializationExt.cs - Extension class                          //
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
This packages provides extension methods for displaying titles 



Public Interface:
=================
public:
------
title() - display title with underline -- 

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

namespace UtilityPackages
{
    public static class StringExt
    {
        public static void title(this string aString, bool isMajor = false)
        {
            char underline = '-';
            if (isMajor)
                underline = '=';
            string logTitle = "\n  " + aString;
            logTitle += "\n " + new string(underline, aString.Count() + 2)+ "\n ";
           

            Console.Write("\n  {0}", logTitle);
           // Console.Write("\n {0}", new string(underline, aString.Length + 2));
        }
    }
      #if (Test_StringExt)
         
        static void Main(string[] args)
        {
           "title message".title();
        }
      #endif
}
