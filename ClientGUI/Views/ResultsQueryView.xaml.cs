/////////////////////////////////////////////////////////////////////
//  ResultQueryView.xaml.cs -  Results QUery view                    //
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
1. Initializes Results QUery View GUI 
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
    /// Interaction logic for ResultsQueryView.xaml
    /// </summary>
    public partial class ResultsQueryView : UserControl
    {
        private ResultsQueryViewModel viewModel;
        public ResultsQueryView()
        {
            InitializeComponent();
            viewModel = new ResultsQueryViewModel();
            this.DataContext = viewModel;
        }
    }
}
