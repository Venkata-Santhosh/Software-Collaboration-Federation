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
This packages provides extension methods for convering objects into xml format and vice versa 

usage :
ResultStroage storage = getting object from servcies
string s = storage.toXml() - converts whole object into xml format
ResultStorage store = s.FromXml<ResultStorage>()  - converts string to ResultStroage object

Public Interface:
=================
public:
------
toXml() - returns xml string
FromXml<>() - returns objects by serializing xml string

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
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UtilityPackages
{
    public static class  SerializationExt
    {
        static public string ToXml(this object obj)
        {
            // suppress namespace attribute in opening tag

            XmlSerializerNamespaces nmsp = new XmlSerializerNamespaces();
            nmsp.Add("", "");

            var sb = new StringBuilder();
            try
            {
                var serializer = new XmlSerializer(obj.GetType());
                using (StringWriter writer = new StringWriter(sb))
                {
                    serializer.Serialize(writer, obj, nmsp);
                }
            }
            catch (Exception ex)
            {
                Console.Write("\n  exception thrown:");
                Console.Write("\n  {0}", ex.Message);
            }
            return sb.ToString();
        }
        //----< deserialize XML to object >------------------------------

        static public T FromXml<T>(this string xml)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(new StringReader(xml));
            }
            catch (Exception ex)
            {
                Console.Write("\n  deserialization failed\n  {0}", ex.Message);
                return default(T);
            }
        }
#if (Test_SerializationExt)
         
        static void Main(string[] args)
        {
           Message results = new Message();
            results.author = "satnhohs";
           Console.Write(results.toXml());
        }

#endif
    }

}
