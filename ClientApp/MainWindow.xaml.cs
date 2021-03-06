﻿using System;
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
using CefSharp;
using CefSharp.Wpf;
using System.IO;
using CefSharp.Example;
using CefSharp.Wpf.Example.Handlers;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Modules/ModuleName/Default.html
        const string defaultUrl = "ids://Modules/{0}/Default.html";
        private string homePageUrl = string.Empty;
        public MainWindow()
        {
            InitializeComponent();

            InitializeBrowser();
        }

        private void InitializeBrowser()
        {
            var browser = new ChromiumWebBrowser();

            //BrowserSettings browserSettings = new BrowserSettings();
            ////browserSettings.
            //browser.BrowserSettings = browserSettings;

            

            var handler = browser.ResourceHandlerFactory as DefaultResourceHandlerFactory;
            if (handler != null)
            {
                var moduleName = "Example";//It will be got from the menu click event with module name passed
                homePageUrl = string.Format(defaultUrl, moduleName);
                var path = AppDomain.CurrentDomain.BaseDirectory + string.Format(@"Modules/{0}/Default.html",moduleName);//The path for the home page of the module
                StreamReader reader = new StreamReader(path, System.Text.Encoding.GetEncoding("utf-8"));
                var responseBody = reader.ReadToEnd().ToString();
                reader.Close();
                var response = ResourceHandler.FromString(responseBody);
                //response.Headers.Add("HeaderTest1", "HeaderTest1Value");
                handler.RegisterHandler(homePageUrl, response);
            }

            browser.LoadError += (sender, args) =>
            {
                // Don't display an error for downloaded files.
                if (args.ErrorCode == CefErrorCode.Aborted)
                {
                    return;
                }

                // Don't display an error for external protocols that we allow the OS to
                // handle. See OnProtocolExecution().
                //if (args.ErrorCode == CefErrorCode.UnknownUrlScheme)
                //{
                //	var url = args.Frame.Url;
                //	if (url.StartsWith("spotify:"))
                //	{
                //		return;
                //	}
                //}

                // Display a load error message.
                var errorBody = string.Format("<html><body bgcolor=\"white\"><h2>Failed to load URL {0} with error {1} ({2}).</h2></body></html>",
                                              args.FailedUrl, args.ErrorText, args.ErrorCode);

                args.Frame.LoadStringForUrl(errorBody, args.FailedUrl);
            };
            browser.RequestHandler = new RequestHandler();
            browser.MenuHandler = new MenuHandler();
            browser.Address = homePageUrl;

            container.Children.Insert(0, browser);
        }
    }
}
