/////////////////////////////////////////////////////////////////////
//  RepositoryViewModel.cs - View Model file for repository view   //
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
1. This package contains binding functionality for view 
2. class is implementing INotifyPropertyChanged - incase any changes to model it will effect in view 
3. view is two way binded - any changes in view that will reflect in viewmodel and vice versa
4. Implementing Command Patter for all Buttons in Repository View - so that ui no need to know how button is implemented
5. Every Service call is made async

This package contains two class 
1. RepositoryViewModel - for binding
2. CheckBoxViewModel - for displaying filenames in GUI as checkboxes 
Public Interface:
=================
public:
------
Browse button functionality 
browseFiles() - uses FolderBrowserDialog so client can able to browse folder location easily and list of files displayed in GUI
Upload button functionality 
uploadFiles() - this method sends  uploadfiles request to Repository server - > server will  communicate with client and pull all the files 

Private functions:
=================
private:
-------

Build Process:
==============
Required files
Service Adapters, Services,Commands,MessageContracts

Maintenance History:
====================
ver 1.5

*/
using Client.ServiceAdapters;
using Client.Services;
using ClientGUI.commands;
using CommonUtilites.MessageContracts;
using CommonUtilites.MessageDSUtility;
using ITest;
using MessageDS;
using SWTools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using UtilityPackages;

namespace ClientGUI.ViewModels
{
    public class RepositoryViewModel : INotifyPropertyChanged
    {

        StringBuilder sb ;
        public RepositoryViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;
            
            // for initializing commands
            UploadCommand = new ClientGUICommand(uploadFiles);
            browseFileCommand = new ClientGUICommand(browseFiles);
            //for storing error messsages
            sb = new StringBuilder();
        }

        private string authorName;
        public string AuthorName
        {
            get
            {
                return this.authorName;
            }
            set
            {
                if (value != this.authorName)
                {
                    this.authorName = value;

                    var handler = this.PropertyChanged;
                    if (handler != null)
                    {
                        handler(this,
                              new PropertyChangedEventArgs("AuthorName"));
                    }
                }
            }
        }
        private string testHarnessPortNumber;
        public string TestHarnessPortNumber
        {
            get
            {
                return this.testHarnessPortNumber;
            }
            set
            {
                if (value != this.testHarnessPortNumber)
                {
                    this.testHarnessPortNumber = value;

                    var handler = this.PropertyChanged;
                    if (handler != null)
                    {
                        handler(this,
                              new PropertyChangedEventArgs("TestHarnessPortNumber"));
                    }
                }
            }
        }
        private string repositoryPortNumber;
        public string RepositoryPortNumber
        {
            get
            {
                return this.repositoryPortNumber;
            }
            set
            {
                if (value != this.repositoryPortNumber)
                {
                    this.repositoryPortNumber = value;

                    var handler = this.PropertyChanged;
                    if (handler != null)
                    {
                        handler(this,
                              new PropertyChangedEventArgs("RepositoryPortNumber"));
                    }
                }
            }
        }
        private string clientPortNumber;
        public string ClientPortNumber
        {
            get
            {
                return this.clientPortNumber;
            }
            set
            {
                if (value != this.clientPortNumber)
                {
                    this.clientPortNumber = value;

                    var handler = this.PropertyChanged;
                    if (handler != null)
                    {
                        handler(this,
                              new PropertyChangedEventArgs("ClientPortNumber"));
                    }
                }
            }
        }
        // ICommands 
        public ClientGUICommand browseFileCommand { get; private set; }
        public ClientGUICommand UploadCommand { get; private set; }

        // for binding directory path
        private string directoryPath;

        public string DirectoryPath {
            get
            {
                return this.directoryPath;
            }
            set
            {
                if (value != this.directoryPath)
                {
                    this.directoryPath = value;

                    var handler = this.PropertyChanged;
                    if (handler != null)
                    {
                        handler(this,
                              new PropertyChangedEventArgs("DirectoryPath"));
                    }
                }
            }
        }
        //for storing and binding results coming from repository server
        public string result;
        public string Result
        {
            get
            {
                return this.result;
            }
            set
            {
                if (value != this.result)
                {
                    this.result = value;

                    var handler = this.PropertyChanged;
                    if (handler != null)
                    {
                        handler(this,
                              new PropertyChangedEventArgs("Result"));
                    }
                }
            }
        }

        //for binding and storing list of files
        private ObservableCollection<CheckBoxViewModel> listOfFiles = new ObservableCollection<CheckBoxViewModel>();

        public ObservableCollection<CheckBoxViewModel> CheckBoxCollection
        {
            get
            {
                return listOfFiles;
            }
            set
            {
                if (value != this.listOfFiles)
                {
                    this.listOfFiles = value;

                    var handler = this.PropertyChanged;
                    if (handler != null)
                    {
                        handler(this,
                              new PropertyChangedEventArgs("CheckBoxCollection"));
                    }
                }
                
            }
        }

        // browse file functionality
        public async void browseFiles()
        {
            "browse files method".title(true);

            Console.WriteLine("Port number" + this.ClientPortNumber);
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string directory = dialog.SelectedPath;
                Console.WriteLine("Directory Location :" + directory);
                // file service calls
                IFileServiceAdapter fileServices = new FileServiceAdapter();
                List<string> filesList = await fileServices.getFilesInSpecifiedPath(directory, "*.dll");
                List<string> listOfFileNames = await fileServices.getAllFileNamesWithExtension(filesList);
                DirectoryPath = directory;
                "files in diectory ".title();
                foreach (string fileName in listOfFileNames)
                {
                    CheckBoxViewModel checkBox = new CheckBoxViewModel();
                    checkBox.checkBoxName = fileName;
                    Console.WriteLine(fileName);
                    checkBox.isChecked = true;
                    this.listOfFiles.Add(checkBox);
                }
                //storing in Checkboxcollection
                CheckBoxCollection = this.listOfFiles;
            }
        }
        public async void uploadFiles()
        {
            "upload Files".title(true);
            sb.Clear();
            Result = "";
            if (DirectoryPath != null && CheckBoxCollection.Count > 0) { 
                IRepoServices repoService = new RepoServices();
                List<string> selectedFileNames = new List<string>();
                foreach (CheckBoxViewModel checkBoxes in CheckBoxCollection)
                {
                    if (checkBoxes.isChecked) {
                        selectedFileNames.Add(checkBoxes.checkBoxName);
                    }
                    else
                    {
                        continue;
                    }
                }
                IFileServiceAdapter fileServices = new FileServiceAdapter();
                foreach (CheckBoxViewModel checkBox in CheckBoxCollection)
                {
                    if (checkBox.isChecked)
                    {
                        string fullFileName = Path.Combine(directoryPath, checkBox.checkBoxName);
                        StringBuilder result = await fileServices.dependencyCheck(fullFileName+".dll", selectedFileNames);
                        if (result.Length > 0)
                        {
                            sb.Append(result.ToString());
                        }
                        else
                        {
                            FileTransferMessage messageContract = new FileTransferMessage();
                            messageContract.filename = fullFileName + ".dll";
                       }
                    }
                    else
                    {
                        continue;
                    }
                }
                if (sb.Length > 0)
                {
                    Result = sb.ToString();
                }else
                {
                   string fromAddress = "http://localhost:"+ClientPortNumber+"/ClientServices";
                   string toAddress = "http://localhost:" + RepositoryPortNumber + "/RepoServices";
                   string authorName = AuthorName;
                    if (fromAddress != null && toAddress != null && authorName != null)
                    {
                        Console.WriteLine("entering");
                        string messageBody = await repoService.createMessageBody(directoryPath, selectedFileNames, authorName);
                        Message message = await repoService.createMessage(messageBody, fromAddress, toAddress, authorName);
                        repoService.sendUploadRequest(message);

                    }
                    else
                    {
                        Result = " One of the paramter Author Name or addresses are missing, please provide them";
                    }
                }
            }
            else
            {
                Console.WriteLine("No files are selected, Please choose files ");
                Result = "Please choose files";
            }
            AuthorName = "";
            DirectoryPath = " ";
            listOfFiles.Clear();
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class CheckBoxViewModel
    {
        public bool isChecked { get; set; } = true;
        public string checkBoxName { get; set; }
    }
}
