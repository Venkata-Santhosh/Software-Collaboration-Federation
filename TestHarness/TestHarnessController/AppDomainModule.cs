/////////////////////////////////////////////////////////////////////
//  AppDomainModule.cs - Application Domain Manager                //
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
This Package helps in creating new child application domains,creating instances, unloading application domains

Public Interface:
=================
public:
------
createAppDomain() - takes two arguments one is childAppDomainname and other is privateBinPath name
                    with the help of these two arguments in creating child application domain  
                    privateBinPath - creates directory for each testrequest (authorname+timestamp)
getLoaderProxyInstance() - returns Loader Proxy Instance object

private:
-------
configureAppDomainSetup() - provides child application domain configuration information 
unloadAppDomain() - unload application domains

Build Process:
==============
Required files
- Loader.cs

Maintenance History:
====================
ver 1.0

*/
using LoaderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace TestHarness
{
    class AppDomainModule
    {
        private AppDomain childAppDomain;

        //configuring application domain for new child applicationdomain using appDomainSetup
        private AppDomainSetup configureAppDomainSetup(String privateBinPath)
        {
            AppDomainSetup appDomainSetup = new AppDomainSetup();
            appDomainSetup.PrivateBinPath = privateBinPath;
            appDomainSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            return appDomainSetup;
        }


        //creating application domain which is accepting appdomain name and privatebin path
        public AppDomain createAppDomain(String appDomain, String privateBinPath)
        {
            Console.Write("\n Creating child application domain..");
            AppDomainSetup appDomainSetup = configureAppDomainSetup(privateBinPath);
            Evidence evidence = AppDomain.CurrentDomain.Evidence;
            childAppDomain = AppDomain.CreateDomain(appDomain, evidence, appDomainSetup);
            return childAppDomain;
        }

        //getting loader proxy instance by using childAppDoamin
        public LoaderProxy getLoaderProxyInstance()
        {
            //childAppDomain.Load("Loader");
            ObjectHandle oh = childAppDomain.CreateInstance(typeof(LoaderProxy).Assembly.FullName, typeof(LoaderProxy).FullName);
            LoaderProxy loaderProxy = oh.Unwrap() as LoaderProxy;
            return loaderProxy;
        }

        //unloading AppDomain
        public void unloadAppDomain(AppDomain domain)
        {
            Console.Write("\n unloading child application domain");
            AppDomain.Unload(domain);
            Console.Write("\n unloaded successfuly ");
        }

#if (Test_AppDomainModule)
         static void Main(string[] args)
            {
                AppDomainmodule appDomainModule = new AppDomainModule();
                AppDomain childAppDomain = appDomainModule.createAppDomain(authorName, privateBinPath);
                LoaderProxy loaderProxy = appDomainModule.getLoaderProxyInstance();
             
            }
#endif
    }
}
