/////////////////////////////////////////////////////////////////////
//  MainWindow.xaml.cs - Main window this packages initialize GUI  //
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
1. This package initializes Test Harness Client GUI 
2. Resposible for handling all the responses coming from TestHarness and Repository
3. MainWindow creates client host 

Public Interface:
=================
public:
------
loadResults() loads all the results from blocking and displayed to GUI
call action methods and perform Dispatcher.Invoke(action) method

Private functions:
=================
private:
-------
simpleLogRequestResultsAction - returns action which contains code for processing simplelogrequest to display
detailedLogRequestResultsAction - returns action which contains code for processing detailedLogrequest to display
TestDriverNameResult Action - contains code for displaying testdrivers name
TestRequestResultsAction - contains code for displaying test results 

Build Process:
==============
Required files
ServiceContractImpl, ViewModels, MessageDSUtility, MessageDS,UtilityPackages,ResultStorages

Maintenance History:
====================
ver 1.0

*/

using Client.ServiceContractImpl;
using ClientGUI.ViewModels;
using CommonUtilites.MessageDSUtility;
using MessageDS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestHarnessUtility;
using UtilityPackages;
using static TestHarnessUtility.ResultStroage;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        THClientServicesContractImpl ser;
        string clientPortNumber;
        string testHarnessPortNumber;
        string repositoryPortNumber;
        public MainWindow()
        {
            "Requirement 11".title();
            clientPortNumber = (Application.Current as App).ClientPortNumber;
            testHarnessPortNumber = (Application.Current as App).TestHarnessPortNumber;
            repositoryPortNumber = (Application.Current as App).RepositoryPortNumber;


            InitializeComponent();
            

            ser = new THClientServicesContractImpl();
            ThreadStart startThread = new ThreadStart(loadResults);
            Thread thread = new Thread(startThread);
            thread.Start();
            thread.IsBackground = true;
            Task.Run(() =>
            {
                ServiceHost host = null;
                try
                {
                  
                    host = PeerConnection.CreatePeerChannel("http://localhost:"+clientPortNumber+"/ClientServices", typeof(THClientServicesContractImpl));
                    host.Open();
                    Console.Write("\n  Started Client BasicService - Press key to exit:\n");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.Write("\n\n  {0}\n\n", ex.Message);
                    return;
                }
                host.Close();
            });

        }
        private Action simpleLogRequestResultsAction(ResultStroage store)
        {
            Action action = () => {

                ResultsQueryViewModel viewModel = new ResultsQueryViewModel();

                foreach (TestRequestResult requestResult in store.testRequestResultStorage)
                {
                    LogResultsGrid result = new LogResultsGrid();
                    result.Author = requestResult.author;
                    result.TestRequestName = requestResult.testRequestName;
                    result.TimeStamp = requestResult.timeStamp.ToShortDateString();
                    string detailedTestResult = null;
                    string overallResult = "Passed";
                    foreach (TestRequestResult.TestDriverResults driverResults in requestResult.listOfTestDriverResult)
                    {
                        detailedTestResult = detailedTestResult + driverResults.testDriverName + "\n";
                        foreach (TestRequestResult.TestCase testCase in driverResults.listOfTestCaseResults)
                        {
                            detailedTestResult = detailedTestResult + "Test Case Name :" + testCase.testName + "\n";
                            detailedTestResult = detailedTestResult + "Test Case Result: " + testCase.testResult + "\n";
                            if (testCase.testResult == "False")
                            {
                                overallResult = "Failed";
                            }
                        }

                    }
                    result.OverallResult = overallResult;
                    result.detailedTestResult = "";
                    viewModel.LogResultsGridCollection.Add(result);

                }
                QueryView.lstItems.ItemsSource = viewModel.LogResultsGridCollection;

            };
            return action;
        }
        private Action detailedLogRequestResultsAction(ResultStroage store)
        {
            Action action = () => {
                ResultsQueryViewModel viewModel = new ResultsQueryViewModel();

                foreach (TestRequestResult requestResult in store.testRequestResultStorage)
                {
                    LogResultsGrid result = new LogResultsGrid();
                    result.Author = requestResult.author;
                    result.TestRequestName = requestResult.testRequestName;
                    result.TimeStamp = requestResult.timeStamp.ToShortDateString();
                    string detailedTestResult = null;
                    string overallResult = "Passed";
                    foreach (TestRequestResult.TestDriverResults driverResults in requestResult.listOfTestDriverResult)
                    {
                        detailedTestResult = detailedTestResult + driverResults.testDriverName + "\n";
                        foreach (TestRequestResult.TestCase testCase in driverResults.listOfTestCaseResults)
                        {
                            detailedTestResult = detailedTestResult + "Test Case Name " + testCase.testName + "\n";
                            detailedTestResult = detailedTestResult + "Test Case Result " + testCase.testResult + "\n";
                            if (testCase.testResult == "False")
                            {
                                overallResult = "Failed";
                            }
                        }

                    }
                    result.OverallResult = overallResult;
                    result.detailedTestResult = detailedTestResult;
                    viewModel.LogResultsGridCollection.Add(result);

                }
                QueryView.lstItems.ItemsSource = viewModel.LogResultsGridCollection;

            };
            return action;
        }
        private Action TestDriverNamesResultAction(TestDriverNameResult testDrivers,string metaData)
        {
            Action action = () =>
            {
                TestHarnessViewModel container = new TestHarnessViewModel();
                // TestHarnessViewModel c = (TestHarnessViewModel)this.DataContext;
                foreach (string fileName in testDrivers.listOfTestDrivers)
                {
                    TestDriverCheckBox checkBox = new TestDriverCheckBox();
                    checkBox.checkBoxName = fileName;
                    checkBox.isChecked = true;
                    container.TestDriversCollection.Add(checkBox);

                }
                InitializePortNumbers();

                testHarnessView.testDriverMetaData.ItemsSource = metaData;
                testHarnessView.testDriverCollection.ItemsSource = container.TestDriversCollection;
            };
            return action;
        }
        private void InitializePortNumbers()
        {
            RepositoryViewModel viewModel = new RepositoryViewModel();
            viewModel.ClientPortNumber = clientPortNumber;
            viewModel.RepositoryPortNumber = repositoryPortNumber;
            viewModel.TestHarnessPortNumber = testHarnessPortNumber;

            RepoView.repositoryPort.ItemsSource = viewModel.RepositoryPortNumber;
            RepoView.clientPort.ItemsSource = viewModel.ClientPortNumber;
            RepoView.testHarnessPort.ItemsSource = viewModel.TestHarnessPortNumber;

            TestHarnessViewModel thViewModel = new TestHarnessViewModel();
            thViewModel.ClientPortNumber = clientPortNumber;
            thViewModel.RepositoryPortNumber = repositoryPortNumber;
            thViewModel.TestHarnessPortNumber = testHarnessPortNumber;

            testHarnessView.repositoryPort.ItemsSource = thViewModel.RepositoryPortNumber;
            testHarnessView.clientPort.ItemsSource = thViewModel.ClientPortNumber;
            testHarnessView.testHarnessPort.ItemsSource = thViewModel.TestHarnessPortNumber;

            ResultsQueryViewModel queryViewModel = new ResultsQueryViewModel();
            queryViewModel.ClientPortNumber = clientPortNumber;
            queryViewModel.RepositoryPortNumber = repositoryPortNumber;
            queryViewModel.TestHarnessPortNumber = testHarnessPortNumber;

            QueryView.repositoryPort.ItemsSource = queryViewModel.RepositoryPortNumber;
            QueryView.clientPort.ItemsSource = queryViewModel.ClientPortNumber;
            QueryView.testHarnessPort.ItemsSource = queryViewModel.TestHarnessPortNumber;

        }
        private Action TestRequestResultsAction(ResultStroage store)
        {
            Action action = () =>
            {
                TestHarnessViewModel container = new TestHarnessViewModel();

                foreach (TestRequestResult requestResult in store.testRequestResultStorage)
                {
                    TestResultsGrid result = new TestResultsGrid();
                    result.Author = requestResult.author;
                    result.TestRequestName = requestResult.testRequestName;
                    result.TimeStamp = requestResult.timeStamp.ToShortDateString();
                    string detailedTestResult = null;
                    string overallResult = "Passed";
                    foreach (TestRequestResult.TestDriverResults driverResults in requestResult.listOfTestDriverResult)
                    {
                        detailedTestResult = detailedTestResult + driverResults.testDriverName + "\n";
                        foreach (TestRequestResult.TestCase testCase in driverResults.listOfTestCaseResults)
                        {
                            detailedTestResult = detailedTestResult + "Test Case Name " + testCase.testName + "\n";
                            detailedTestResult = detailedTestResult + "Test Case Result " + testCase.testResult + "\n";
                            if (testCase.testResult == "False")
                            {
                                overallResult = "Failed";
                            }
                        }

                    }
                    result.OverallResult = overallResult;
                    result.detailedTestResult = detailedTestResult;
                    container.TestResultsGridCollection.Add(result);

                }
                testHarnessView.lstItems.ItemsSource = container.TestResultsGridCollection;

                //testHarnessView.resultText.Text = store.toJson();
            };
            return action;
        }
        public void loadResults()
        {
            while (true)
            {
                Message dequeuedMessage = ser.deQUeu();

                if(dequeuedMessage.type == "UploadResult")
                {
                    "Upload Results in XML Format".title();
                    Console.WriteLine(dequeuedMessage.body);
                    UploadResult result = dequeuedMessage.body.FromXml<UploadResult>();
                   
                    Action action = () =>{
                        RepoView.resultText.Text = result.resultMessage;
                    };
                    Dispatcher.Invoke(action);
                }
                if(dequeuedMessage.type == "TestDriverNamesResult")
                {
                    "TestDriverName Results in XML Format from repository".title();

                    string metaData = dequeuedMessage.body;
                    TestDriverNameResult testDrivers = dequeuedMessage.body.fromJson<TestDriverNameResult>();
                    Console.WriteLine(testDrivers.ToXml());
                    Action action = TestDriverNamesResultAction(testDrivers, metaData);
                    Dispatcher.Invoke(action);
                }
                if(dequeuedMessage.type == "TestRequestResults")
                {
                    "TestRequest Results from test harness server".title();
                    ResultStroage store = dequeuedMessage.body.fromJson<ResultStroage>();
                    Console.Write(store.ToXml());
                    Action action = TestRequestResultsAction(store);
                    Dispatcher.Invoke(action);
                }
                if( dequeuedMessage.type == "SimpleLogRequestResults")
                {
                    "Simple log request Results from repository".title();
                    ResultStroage store = dequeuedMessage.body.fromJson<ResultStroage>();
                    Console.WriteLine(store.ToXml());
                    Action action = simpleLogRequestResultsAction(store);
                    Dispatcher.Invoke(action);
                }
                if(dequeuedMessage.type == "DetaileLogRequestResults")
                {
                    "Detailed Log Request Results from repository".title();
                    ResultStroage store = dequeuedMessage.body.fromJson<ResultStroage>();
                    Console.WriteLine(store.ToXml());
                    Action action = detailedLogRequestResultsAction(store);
                    Dispatcher.Invoke(action);
                }
                if(dequeuedMessage.type == "FileNotFoundResult")
                {
                    "Requriement 3".title();
                    Action action = () =>
                    {
                        testHarnessView.resultText.Text = "One of the requested file not found in repository,please upload before testing";
                    };
                    Dispatcher.Invoke(action);

                }
            }
        }
    }
}
