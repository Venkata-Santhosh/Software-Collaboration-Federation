/////////////////////////////////////////////////////////////////////
//  DataContractExt.cs - Extension class                          //
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
This packages provides extension methods for convering objects into json format and vice versa 

usage :
ResultStroage storage = getting object from servcies
string s = storage.toJson() - converts whole object into json format
ResultStorage store = s.FromJson<ResultStorage>()  - converts string to ResultStroage object

Public Interface:
=================
public:
------
toJson() - returns json string
FromJson<>() - returns objects by serializing json string

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
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace UtilityPackages
{
    public static class DataContractExt
    {
        static public string toJson(this object obj)
        {

            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer serializer = 
                new DataContractJsonSerializer(obj.GetType(), 
                                new DataContractJsonSerializerSettings
                                    {
                                        UseSimpleDictionaryFormat = false,
                                        DateTimeFormat = new DateTimeFormat("yyyy-MM-dd HH:mm:ss")
                                    }
                                );
            serializer.WriteObject(stream, obj);
            stream.Position = 0;
            StreamReader sr = new StreamReader(stream);
            return sr.ReadToEnd();
           

        }
         

        static public T fromJson<T> (this string obj)
        {
            DataContractJsonSerializer serializer =
                    new DataContractJsonSerializer(typeof(T),
                        new DataContractJsonSerializerSettings
                        {
                            UseSimpleDictionaryFormat = false,
                            DateTimeFormat = new DateTimeFormat("yyyy-MM-dd HH:mm:ss")
                        });
           byte[] byteArray = Encoding.UTF8.GetBytes(obj);
           MemoryStream stream = new MemoryStream(byteArray);
           return  (T)serializer.ReadObject(stream);
        }
#if (Test_DataContractExt)
         
        static void Main(string[] args)
        {
           Message results = new Message();
            results.author = "satnhohs";
           Console.Write(results.toJson());
        }

#endif

    }
}
