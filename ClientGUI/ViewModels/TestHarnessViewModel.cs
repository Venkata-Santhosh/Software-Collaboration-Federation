/////////////////////////////////////////////////////////////////////
//  TestHarnessViewModel.cs - View Model file for TestHarness view //
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
4. Implementing Command Patter for all Buttons in TestHarness View - so that ui no need to know how button is implemented
5. Every Service call is made async

This package contains two class 
1. TestHarnessViewModel - for binding
2. TestResultsGrid - for displaying logs in GUI as datagrid
3. TestDriverCheckBox - for test drivername display

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
Services,Commands

Maintenance History:
====================
ver 1.5
*/
using ClientGUI.commands;
using ClientGUI.Services;
using CommonUtilites.MessageDSUtility;
using MessageDS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityPackages;

namespace ClientGUI.ViewModels
{
    public class TestHarnessViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TestDriverCheckBox> listOfFiles = new ObservableCollection<TestDriverCheckBox>();
        private ObservableCollection<TestResultsGrid> listOfResults = new ObservableCollection<TestResultsGrid>();
        public TestHarnessViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;

            RefreshCommand = new ClientGUICommand(refreshTestDriverNames);
            TestRunCommand = new ClientGUICommand(runTestCases);
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

        public ClientGUICommand RefreshCommand { get; private set; }
        public ClientGUICommand TestRunCommand { get; private set; }

        public async void refreshTestDriverNames()
        {
            "Refersh - for getting test drivernames form repository".title();
            ClientTestHarnessServices services = new ClientTestHarnessServices();
            string fromAddress = "http://localhost:"+ClientPortNumber+"/ClientServices";
            string toAddress = " http://localhost:"+RepositoryPortNumber+"/RepoServices";
            string author = "Client";
            if (author != null) { 
                await services.getTestDriverNamesFromRepo(fromAddress,toAddress,author);
            }
           
        }
        
        public async void runTestCases()
        {
            "Run Test Cases  ".title();
            string data = testDriverMetaData.ToString();
            TestDriverNameResult result = data.fromJson<TestDriverNameResult>();
            List<string> selectedTestDrivers = new List<string>();
            foreach (TestDriverCheckBox checkBox in TestDriversCollection)
            {
                if (checkBox.isChecked)
                {
                    selectedTestDrivers.Add(checkBox.checkBoxName);
                }
            }
            Console.WriteLine("Constructing dependencies for selected test drivers");
            DependencyDS d =  result.depdencyDS;
            TestRequest request = new TestRequest();
            int i = 0;
            foreach(TestDriverMetaData m in d.metaData)
            {
                if (selectedTestDrivers.Contains(m.testDriverName))
                {
                    i++;
                    TestElement element = new TestElement();
                    element.testDriver = m.testDriverName;
                    element.testName = "test"+i;
                    foreach (Dependencies dep in m.dependencies)
                    {
                        element.testCodes.Add(dep.dependencyFileName);
                    }
                    request.author = d.author;
                    request.tests.Add(element);
                }
                
            }

            Console.WriteLine("Creating messages structure");

            Message message = new Message();
            message.author = AuthorName;
            message.body = request.ToXml();
            message.from = "http://localhost:"+ClientPortNumber+"/ClientServices";
            message.to = "http://localhost:"+TestHarnessPortNumber+"/TestHarnessServices";
            message.type = "TestRequest";
            ClientTestHarnessServices services = new ClientTestHarnessServices();
            if (AuthorName != null)
            {
                await services.runTestCases(message);
            }
            else
            {
                Result = "Please provide author name";
            }
            
        }

        public ObservableCollection<TestDriverCheckBox> TestDriversCollection
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
                              new PropertyChangedEventArgs("TestDriversCollection"));
                    }
                }

            }
        }
        public ObservableCollection<TestResultsGrid> TestResultsGridCollection
        {
            get
            {
                return listOfResults;
            }
            set
            {
                if (value != this.listOfResults)
                {
                    this.listOfResults = value;

                    var handler = this.PropertyChanged;
                    if (handler != null)
                    {
                        handler(this,
                              new PropertyChangedEventArgs("TestResultsGridCollection"));
                    }
                }

            }
        }

        private string testDriverMetaData;
        public string TestDriverMetaData
        {
            get
            {
                return  this.testDriverMetaData;
            }
            set
            {
                if (value != this.testDriverMetaData)
                {
                    this.testDriverMetaData = value;

                    var handler = this.PropertyChanged;
                    if (handler != null)
                    {
                        handler(this,
                              new PropertyChangedEventArgs("TestDriverMetaData"));
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class TestResultsGrid
    {
        public string Author { get; set; }
        public string TestRequestName { get; set; } 
        public string TimeStamp { get; set; }
        public string DriverName { get; set; }
        public string OverallResult { get; set; }
        public string detailedTestResult { get; set; }
    }
    public class TestDriverCheckBox
    {
        public bool isChecked { get; set; } = true;
        public string checkBoxName { get; set; }
    }
}
