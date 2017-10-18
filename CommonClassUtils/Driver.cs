using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Drawing;
using System.Threading;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;

namespace CommonClassUtils
{
    public class Driver
    {
        public static IWebDriver Instance { get; set; }
        public static PhantomJSDriverService service;



        public static void Goto(string p)
        {
            Thread.Sleep(1000);
            Instance.Navigate().GoToUrl(p);

        }

        public static void ChromeInitialize()
        {
            // Using Chrome Headless visit:  https://developers.google.com/web/updates/2017/04/headless-chrome
            ChromeOptions cOptions = new ChromeOptions();
            cOptions.AddArguments("disable-infobars");
            // Download new Drivers from https://chromedriver.storage.googleapis.com/index.html?path=2.32/
            Instance = new ChromeDriver(@"C:\Drivers", cOptions);

            Instance.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            Instance.Manage().Window.Maximize();
        }


        public static void MicrosoftWebDriver()
        {
            Instance = new EdgeDriver(@"C:\Drivers");
            Instance.Manage().Window.Maximize();
        }

        

        public static void Phantom_Initialize()
        {
            Instance = new PhantomJSDriver(@"C:\Users\Faron\Documents\Visual Studio 2013\Projects\BankOfIrelandQa\packages\PhantomJS.2.0.0\tools\phantomjs\");
            Instance.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            Instance.Manage().Window.Maximize();
        }


        public static void Close()
        {
            Instance.Close();
            Instance.Quit();
        }



        public static void SetBrowserMode(string browserMode)
        {
            //              * Mobile:     max-width 640px,  (640, 480)
            //              * Tablet:      min-width 641px and max-width 1024px (1024, 620)
            //              * Desktop:  min-width 1025px (maximise)

            //Instance.Manage().Window.Size = new Size(1024, 620);
            int x = 1025; // default
            int y = 640; // default

            browserMode = browserMode.ToUpper();

            switch (browserMode)
            {
                case "MOBILE":
                    x = 400;//580
                    y = 620;
                    break;
                case "TABLET":
                    x = 1024;
                    y = 620;
                    break;
                case "DESKTOP":
                    x = 1025;
                    y = 640;
                    break;
                default:
                    x = 1025;
                    y = 640;
                    break;
            }

            if (browserMode != "MAX" && browserMode != "DESKTOP")
            {
                Instance.Manage().Window.Size = new Size(x, y);
            }
            else if (browserMode == "MAX")
            {
                Instance.Manage().Window.Maximize();
            }
        }


    }
}

