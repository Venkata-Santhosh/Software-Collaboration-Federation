/////////////////////////////////////////////////////////////////////
//  TestHarnessView.xaml.cs -  TestHarness view                    //
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
1. Initializes Test Harness View GUI 
2. Binds DataContexts

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
Services,ViewModels

Maintenance History:
====================
ver 1.5
*/
using ClientGUI.Services;
using ClientGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace ClientGUI.Views
{
    /// <summary>
    /// Interaction logic for TestHarnessView.xaml
    /// </summary>
    public partial class TestHarnessView : UserControl
    {
        private TestHarnessViewModel viewModel;
        string clientPortNumber;
        string repositoryPortNumber;
        public TestHarnessView()
        {

            InitializeComponent();
            clientPortNumber = (Application.Current as App).ClientPortNumber;
            repositoryPortNumber = (Application.Current as App).RepositoryPortNumber;
            Console.WriteLine("client port number" + clientPortNumber);
            viewModel = new TestHarnessViewModel();
            this.DataContext = viewModel;
            ClientTestHarnessServices services = new ClientTestHarnessServices();
            string fromAddress = "http://localhost:"+ clientPortNumber + "/ClientServices";
            string toAddress = " http://localhost:" + repositoryPortNumber + "/RepoServices";
            string author = "Client";
            services.getTestDriverNamesFromRepo(fromAddress,toAddress,author);
        }
    }
}
