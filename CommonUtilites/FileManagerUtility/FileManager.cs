///////////////////////////////////////////////////////////////////// 
//  FileManager.cs                                                 //
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
This package has been created to demonstrate FileManager utility
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

namespace CommonUtilites.FileManagerUtility
{
    public interface IFileManager
    {
        string[] getFilesInSpecifiedPath(String directory, String wildCardFileExtention);
        string[] getAllFileNamesWithExtension(string[] fullyQualifiedFileNames);
        FileStream openFile(string fileName);
        void writeToFile(FileStream fs, string message);
        void readFromFile(string fileName);
        void closeFile(FileStream file);
        void storeMessage(string message, FileStream fileStream);
         void createDirectory(String directoryName);
        void deleteDirectory(string directoryName);
    }
    public class FileManager : IFileManager
    {
        public void deleteDirectory(string directoryName)
        {
            try
            {
                if (Directory.Exists(directoryName))
                {
                    Directory.Delete(directoryName, true);
                }
            }
            catch (Exception e)
            {
                Console.Write("\n " + e.Message);
            }
        }
        public void createDirectory(String directoryName)
        {
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
        }
        public void closeFile(FileStream file)
        {
            throw new NotImplementedException();
        }


        //returns file names with extensions 
        public string[] getAllFileNamesWithExtension(string[] fullyQualifiedFileNames)
        {
            

            string[] fileNames = null;
            int arraySize = fullyQualifiedFileNames.Length;
            if (fullyQualifiedFileNames != null && arraySize > 0)
            {
                fileNames = new string[arraySize];
                int index = 0;
                foreach (string fullyQualifiedFileName in fullyQualifiedFileNames)
                {
                    fileNames[index++] = Path.GetFileNameWithoutExtension(fullyQualifiedFileName);
                }
               
            }
            return fileNames;

        }

        // returns all fully Qualified file names which matchs wildCard File Extentions from directory
        public string[] getFilesInSpecifiedPath(string directory, string wildCardFileExtention)
        {
            //string methodName = "getFilesInSpecifiedPath()";
           

            string[] files = null;

            if (Directory.Exists(directory) && wildCardFileExtention != null)
            {
                files = Directory.GetFiles(directory, wildCardFileExtention, SearchOption.AllDirectories);
            }
            else
            {
               // THLogger.write(LogLevel.WARN, " Directory or wildCardFileExtention is null");
            }
          // THLogger.exiting(methodName, CLASSNAME);
            return files;
        }

        public FileStream openFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public void readFromFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public void readFromFile(FileStream fileStream,string fileName)
        {
            byte[] b = new byte[1024];
            int a;
            try
            {
                UTF8Encoding temp = new UTF8Encoding(true);
                fileStream = File.OpenRead(fileName);
                a = fileStream.Read(b, 0, b.Length);
                while (a > 0)
                {
                    Console.Write("" + temp.GetString(b));
                    a = fileStream.Read(b, 0, b.Length);
                }
            }
            catch (Exception e)
            {
                Console.Write("\n" + e.Message);
            }
            finally
            {
                try
                {
                    fileStream.Close();
                }
                catch (Exception e)
                {
                    Console.Write("\n Some Problem in closing filestream reason :- " + e.Message);
                }
            }
        }

        public void storeMessage(string message, FileStream fileStream)
        {
            try
            {
                writeToFile(fileStream, message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                try
                {
                    fileStream.Close();
                }
                catch (Exception e)
                {
                    Console.Write("\n Some Problem in closing filestream reason :- " + e.Message);
                }
            }
        }

        public void writeToFile(FileStream fs, string message)
        {
            fs.SetLength(0);
            byte[] info = new UTF8Encoding(true).GetBytes(message);
            fs.Write(info, 0, info.Length);
        }
#if (Test_FileManager)
         
        static void Main(string[] args)
        {
           FileManager manager = new FileManager();
            manager.getFilesInSpecifiedPath("../../directory","*.dll");
        }

#endif
    }
}
