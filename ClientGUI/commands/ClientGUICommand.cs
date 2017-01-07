/////////////////////////////////////////////////////////////////////
//  ClientGUICommand.cs - sClient GUI Command                       //
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
This package is used by all buttons using GUI
Command patter is used
Implements ICommand interface

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

Maintenance History:
====================
ver 1.0

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientGUI.commands
{
    public class ClientGUICommand : ICommand
    {
        Action action;

        public ClientGUICommand(Action action)
        {
            this.action = action;
        }

        public event EventHandler CanExecuteChanged =  delegate { };

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action();
        }
    }
}
