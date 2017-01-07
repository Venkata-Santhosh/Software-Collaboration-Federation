/////////////////////////////////////////////////////////////////////
//  ResultQueryViewModel.cs - View Model file for ResultQuery view //
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
4. Implementing Command Patter for all Buttons in ResultQuery View - so that ui no need to know how button is implemented
5. Every Service call is made async

This package contains two class 
1. ResultsQueryViewModel - for binding
2. LogResultsGrid - for displaying logs in GUI as datagrid
 
Public Interface:
=================
public:
------
simpleLogRequest() - Simple Log Request functionality - for making simple log requests
detaildLogRequest() - DetailedLog Request functionality - for making detaild log requests

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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientGUI.ViewModels
{
    public class ResultsQueryViewModel : INotifyPropertyChanged
    {

        //constructor 
        public ResultsQueryViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;

            //command init by binding functionality 
            SimpleLogRequestCommand = new ClientGUICommand(simpleLogRequest);
            DetailedLogRequestCommand = new ClientGUICommand(detaildLogRequest);
        }

        //button commands 
        public ClientGUICommand SimpleLogRequestCommand { get; set; }
        public ClientGUICommand DetailedLogRequestCommand { get; set; }


        //simple logRequest command functions/methods
        public async void simpleLogRequest()
        {
            string fromAddress = "http://localhost:"+ClientPortNumber+"/ClientServices";
            string toAddress = "http://localhost:"+RepositoryPortNumber+"/RepoServices";
            string author = "Manager";
            IClientRepoQueryServices service = new ClientRepoQueryServices();
            await service.getSimpleLogRequest(fromAddress,toAddress,author);
        }
        //detailed log request method
        public async void detaildLogRequest()
        {
            string fromAddress = "http://localhost:"+ClientPortNumber+"/ClientServices";
            string toAddress = "http://localhost:" + RepositoryPortNumber + "/RepoServices";
            string author = "Manager";
            IClientRepoQueryServices service = new ClientRepoQueryServices();
            await service.getDetailedLogRequest(fromAddress,toAddress,author);
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

        //Data grid 
        private ObservableCollection<LogResultsGrid> listOfLogResults = new ObservableCollection<LogResultsGrid>();
        public ObservableCollection<LogResultsGrid> LogResultsGridCollection
        {
            get
            {
                return listOfLogResults;
            }
            set
            {
                if (value != this.listOfLogResults)
                {
                    this.listOfLogResults = value;

                    var handler = this.PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, 
                            new PropertyChangedEventArgs("LogResultsGridCollection"));
                    }
                }

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class LogResultsGrid
    {
        public string Author { get; set; }
        public string TestRequestName { get; set; }
        public string TimeStamp { get; set; }
        public string DriverName { get; set; }
        public string detailedTestResult { get; set; }
        public string OverallResult { get; set; }

    }
}
