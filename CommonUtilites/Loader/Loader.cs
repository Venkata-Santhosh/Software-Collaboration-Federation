///////////////////////////////////////////////////////////////////// 
//  Loader.cs                                                 //
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
This package checks depdendencies for dll files
Public Interfaces:
----------------
public:

loadAllAssemblies - loads and checks dependencies need for file and return as json string
Maintenance History:
====================
ver 1.0

*/
using MessageDS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UtilityPackages;

namespace CommonUtilites.Loader
{
   public class LoaderDep : MarshalByRefObject
    {
        //loads and checks dependencies need for file and return as json string
        public string loadAllAssemblies(string directoryPath, List<string> selectedFiles,string authorName)
        {
            DependencyDS dependencyDS = new DependencyDS();
            dependencyDS.basePath = directoryPath;
            dependencyDS.author = authorName;
            foreach (string selectedFile in selectedFiles)
            {
                string fullFileName = Path.Combine(directoryPath, selectedFile + ".dll");
                Assembly assem = Assembly.LoadFrom(fullFileName);

                AssemblyName[] dependencyNames = assem.GetReferencedAssemblies();

                foreach (AssemblyName dependencyName in dependencyNames)
                {
                    if (dependencyName.FullName.IndexOf("mscorlib") != -1)
                        continue;
                    if (dependencyName.FullName.IndexOf("ITest") != -1)
                    {
                        TestDriverMetaData ds = new TestDriverMetaData();
                        ds.testDriverName = selectedFile + ".dll";
                        ds.version = "1.0";
                        foreach (AssemblyName insideNames in dependencyNames)
                        {
                            if (insideNames.FullName.IndexOf("mscorlib") != -1)
                                continue;
                            if (insideNames.FullName.IndexOf("ITest") != -1)
                                continue;
                            Dependencies dep = new Dependencies();
                            dep.dependencyFileName = insideNames.Name+".dll";
                            dep.version = "1.0";
                            ds.dependencies.Add(dep);
                        }
                        dependencyDS.metaData.Add(ds);
                    }

                }

            }
            string jsonString = dependencyDS.toJson();
            "creating message body for upload request".title();
            Console.WriteLine(jsonString);
            return jsonString;
        }
#if (Test_LoaderDep)
         
        static void Main(string[] args)
        {
           LoaderDep loaderDep = new LoaderDep();
            List<string> selectedFiles = new List<string>();
            files.Add("Driver.dll");
            loaderDep.loadAllAssemblies( "../../directory", selectedFiles,"santhosh");
        }

#endif
    }
}
