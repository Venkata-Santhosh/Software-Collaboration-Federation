/////////////////////////////////////////////////////////////////////
//  FileManagerServiceAdapter.cs - service implementaion           //
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
This packages is responsible for providing all FileManager related functions
//class interns calls filemanager 
//adapter patter is used for implementing this

Public Interface:
=================
public:
------
getFilesInSpecifiedPath() - getting files in specified path async
getAllFileNamesWithExtension() - getting all filenames with extenstions asyncly 

Private functions:
=================
private:
-------

Build Process:
==============
Required files
MessageContracts,ThreadPoolMessageListener,ICommService,MessageDS

Maintenance History:
====================
ver 1.0

*/
using CommonUtilites.FileManagerUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UtilityPackages;

namespace Client.ServiceAdapters
{

    //Adapter patter is used 
    //for making FileManager compatible client
    public interface IFileServiceAdapter
    {
        Task<List<string>> getFilesInSpecifiedPath(String directory, String wildCardFileExtention);
        Task<List<string>> getAllFileNamesWithExtension(List<string> fullyQualifiedFileNames);
        Task<StringBuilder> dependencyCheck(string fileName, List<string> selectedFileNames);
    }
    public class FileServiceAdapter : IFileServiceAdapter
    {
        

        public Task<List<string>> getAllFileNamesWithExtension(List<string> fullyQualifiedFileNames)
        {
            return Task.Run(()=> {
                Console.WriteLine("getting file names with extenstion");
                IFileManager fileManager = new FileManager();
                return fileManager.getAllFileNamesWithExtension(fullyQualifiedFileNames.ToArray()).ToList();
            });
        }

        public Task<List<string>> getFilesInSpecifiedPath(string directory, string wildCardFileExtention)
        {
            return Task.Run(() => {
                Console.WriteLine("getting files present in specified path");
                IFileManager fileManager = new FileManager();
                return fileManager.getFilesInSpecifiedPath(directory, wildCardFileExtention).ToList();
            });
        }
        public Task<StringBuilder> dependencyCheck(string fullFileName, List<string> selectedFileNames)
        {
            return Task.Run(() => {
                "dependency checking for selected files".title();
                StringBuilder sb = new StringBuilder();
                Console.WriteLine("checking dependencies for file name" + fullFileName);
                Assembly assembly = Assembly.LoadFrom(fullFileName);
                AssemblyName[] dependencyNames = assembly.GetReferencedAssemblies();
                foreach (AssemblyName dependencyName in dependencyNames)
                {
                    if (dependencyName.FullName.IndexOf("mscorlib") != -1)
                        continue;
                    if (dependencyName.FullName.IndexOf("ITest") != -1)
                        continue;
                    if (!selectedFileNames.Contains(dependencyName.Name))
                    {
                        Console.WriteLine(fullFileName + "'s dependencyFile - " + dependencyName.Name + " is missing");
                        sb.Append(fullFileName + "'s dependencyFile - " + dependencyName.Name + " is missing");
                    }
                }
                return sb;
            });
        }
#if (Test_RepoServices)
         static void Main(string[] args)
            {
                 SendMessageProcessor.enQueuingMessage(requestMessage);
            }
#endif
    }
}
