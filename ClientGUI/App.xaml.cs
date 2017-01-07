using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public string ClientPortNumber { get; set; }
        public string TestHarnessPortNumber { get; set; }
        public string RepositoryPortNumber { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            this.ClientPortNumber = e.Args[0].ToString();
            this.TestHarnessPortNumber = e.Args[1].ToString();
            this.RepositoryPortNumber = e.Args[2].ToString();
            base.OnStartup(e);
        }
    }
}
