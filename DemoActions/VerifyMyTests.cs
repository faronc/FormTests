using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonClassUtils;
using OpenQA.Selenium;

namespace DemoActions
{
    public class VerifyMyTests
    {
        public static string ElementTxt { get; protected set; }
        //elementTxt is used to access the text contained in an element from other classes
        public static string ExpectedTxt { get; protected set; }
        public static string _resultOfTest { get; protected set; }
        public static string _pass = "Pass";
        public static string _fail = "Fail";

        public static string _match = "MATCHES";
        public static string _noMatch = "DOES NOT MATCH";
        public static string _contains = "CONTAINS";

        public static string VerifyStringOnThePage(string elementId, string goodwords, string badwords)
        {
            Thread.Sleep(2000);
            // split each goodwords and badwords into an array so we can repeat verification for each item
            var wordsAppear = goodwords.Split(new[] { ";" }, StringSplitOptions.None);
            var wordsDontAppear = badwords.Split(new[] { ";" }, StringSplitOptions.None);

            // declaring result of the test str
            var resultOfUnitTest = string.Empty;

            try
            {
                // finding and converting the element into a text string
                IWebElement messageContent = Driver.Instance.FindElement(By.XPath("//*[contains(@id, 'pagecontent')]"));
                var contentOfMessages = messageContent.Text;
                contentOfMessages = contentOfMessages.ToUpper().Replace("  ", " ");
                // because C# is case sensitive - want to avoid false positives when comparing "COMPLETE" with "Complete"
                
                Thread.Sleep(250);

                // Verify that each of these words are present in string that we grabbed at the start of this method
                foreach (var i in wordsAppear)
                {
                    // all these words SHOULD exist - if they DONT we fail the test
                    if (contentOfMessages.Contains(i.ToUpper()) != true && i != "")
                    {
                        resultOfUnitTest += _fail + ": The Page Tested DOES NOT HAVE the expected string <" + i + "> and it should.\n";

                    }
                    else if (contentOfMessages.Contains(i.ToUpper()) && i != "")
                    {
                        resultOfUnitTest += _pass + ": The Page Tested SHOULD HAVE the string <" + i + "> and it does" +
                                            "\n";
                    }
                }

                // Verify that each of these words are NOT present in The File Tested file we are checking
                Thread.Sleep(2000);
                foreach (var i in wordsDontAppear)
                {
                    // all these words SHOULD NOT exist - if they DO we fail the test
                    if (contentOfMessages.Contains(i.ToUpper()) && i != "")
                    {
                        resultOfUnitTest += _fail + ": The Page Tested SHOULD NOT HAVE the string <" + i + "> but it does.\n";
                    }
                    else if (contentOfMessages.Contains(i.ToUpper()) != true && i != "")
                    {
                        resultOfUnitTest += _pass + ": The Page Tested SHOULD NOT HAVE the string <" + i + "> and it doesn't.\n";
                    }
                }
                // return the result of the test back to the TestMethod
                return resultOfUnitTest;
            }
            catch
            {
                // In case we fail we'll return this but will need to be investigated
                resultOfUnitTest += _fail +
                                    " - Something went wrong verifying <VerifyTestCase> method. Probably couldnt find the element therefore unexpected result\n";
                return resultOfUnitTest;
            }
        }
    }
}
