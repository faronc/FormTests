using System;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonClassUtils;
using DemoActions;

namespace DemoTests
{
    [TestClass]
    public class SmokeTests
    {
        private bool _postToSlack;
        private string _slackChannel = string.Empty;
        private string _urlWithAccessToken = string.Empty;
        private string _resultsFile = string.Empty;
        private string _message = string.Empty;
        private string _resultsFolder = string.Empty;
        private string _url = string.Empty;
        private string _pass = "Pass";
        private string _fail = "Fail";
        private string _testExplanation = String.Empty;
        private bool _status = true;
        public static string DateTimeForFileName = DateTime.Now.ToString(@"MMM\-dd-HH\-mm");
        
        private string _jiraTicketNumberWithAutomationResults = String.Empty;
        private bool _createJiraTicket;
        private string jiraThreeLetterProjectId = "TST";
        private string jiraParentId = "TST-1";

        /// <summary>
        /// Create a new instance of the driver
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            // Use Initialize for running locally and use PhantomInitialize for CI/Jenkins
            Driver.ChromeInitialize();
        }

        private TestContext _testContextInstance;
        public TestContext TestContext
        {
            get { return _testContextInstance; }
            set { _testContextInstance = value; }
        }


        /// <summary>
        /// Smoke Test to verify previously logged bugs
        /// To be run against each new deployment
        /// </summary>
        [TestMethod]
        public void DataDriverDemoTests()
        {
            const string testCaseId = "BVT_BCC_01";
            const string testProject = "Business Credit Cards Application form";
            const string testTitle = "Sept 2017 Sprint 42 Release PLOF-4560";
            const string testDesc = "Journey 1: Business Credit Card - Unincorporated";
            var slackNotes = "";
            var testTester = "QA Automation";
            _slackChannel = Config.Default.slackChannel;
            _urlWithAccessToken = Config.Default.urlWithAccessToken;
            _postToSlack = Convert.ToBoolean(Config.Default.postToSlack);
            var bvtResult = string.Empty;

            _createJiraTicket = Convert.ToBoolean(Config.Default.createJiraTicket);
            var jiraUsername = Config.Default.JiraUsername;
            var jiraPassword = Config.Default.JiraPassword;

            try
            {
                var title = "Mrs";
                var firstName = "Séan Óg";
                var surname = "Ó'Sé-Kelly";
                var email = GenerateRandomStrings.GenerateEmail(5, 3);
                var gender = "Female";
                var dateOfBirth = GenerateRandomStrings.GetDateMinusNumberOfYears(20);
                var mobilePhoneNumber = GenerateRandomStrings.GenerateRandomZeroEightMobileNumber(8);
                var addressLine1 = "Address Line 1 sample";
                var addressLine2 = "Address Line 2 sample";
                var county = "Roscommon";
                var accountNumber = GenerateRandomStrings.GenerateRandomNumber(8);
                var sortCodeMiddle = GenerateRandomStrings.GenerateRandomNumber(2);
                var sortCodeLast = GenerateRandomStrings.GenerateRandomNumber(2);
                var descriptionOfQuery = "Smoke Test description: Lorem ipsum dolor sit amet, consectetur adipiscing elit";




                // Settings Variables
                _resultsFolder = Config.Default.resultsfile;
                _url = Config.Default.url;

                bool dirExists = Directory.Exists(_resultsFolder);
                if (!dirExists)
                    Directory.CreateDirectory(_resultsFolder);

                _resultsFile = _resultsFolder + @"\DataDriverDemoTests_" + DateTimeForFileName + ".txt";
                
                _message =
                    string.Format(
                        "Test Case ID: {0}\nTest title: {1}\nTest Description: {2}\nTest Project: {3}\nSlack Notes: {4}\n\nUrl: {5}\n\n",
                        testCaseId, testTitle, testDesc, testProject, slackNotes, _url);
                Thread.Sleep(150);
                using (var file = new StreamWriter(_resultsFile, true))
                {
                    file.WriteLine(_message);
                }

                var testDataUsed =
                    string.Format(
                        "Test Data Used in this test:\n{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n{6}\n{7}\n{8}\n{9}\n{10}\n{11}\n{12}\n{13}\n",
                        title, firstName, surname, email, gender, dateOfBirth, mobilePhoneNumber, addressLine1,
                        addressLine2, county, accountNumber, sortCodeMiddle, sortCodeLast, descriptionOfQuery);


                using (var file = new StreamWriter(_resultsFile, true))
                {
                    file.WriteLine(testDataUsed);
                }



                Driver.Goto(_url);

                // PAGE 1 - GETTING STARTED

                Thread.Sleep(1000);
                _testExplanation = "\nTEST SCENARIO: Populating Page 1 - YOUR DETAILS - page";
                using (var file = new StreamWriter(_resultsFile, true))
                {
                    file.WriteLine(_testExplanation);
                }
                Thread.Sleep(250);


                // Page 1 - Populating the YOUR DETAILS page with test data
                FormUtilitiesActions.SelectItemFromDropdown("title", title);
                FormUtilitiesActions.EnterTextIntoInputBox("firstname", firstName);
                FormUtilitiesActions.EnterTextIntoInputBox("surname", surname);
                FormUtilitiesActions.EnterTextIntoInputBox("email", email);
                FormUtilitiesActions.ChooseRadioOption("Gender", gender);
                FormUtilitiesActions.EnterTextIntoInputBox("dateofbirth", dateOfBirth);
                FormUtilitiesActions.EnterTextIntoInputBox("mobilephonenumber", mobilePhoneNumber);
                FormUtilitiesActions.EnterTextIntoInputBox("addressline1", addressLine1);
                FormUtilitiesActions.EnterTextIntoInputBox("addressline2", addressLine2);
                FormUtilitiesActions.SelectItemFromDropdown("county", county);
                FormUtilitiesActions.EnterTextIntoInputBox("accountnumber", accountNumber);
                FormUtilitiesActions.EnterTextIntoInputBox("sortcodemiddle", sortCodeMiddle);
                FormUtilitiesActions.EnterTextIntoInputBox("sortcodelast", sortCodeLast);
                FormUtilitiesActions.EnterTextIntoInputBox("descriptionofquery", descriptionOfQuery);

                _testExplanation =
                    "BUG NUMBER: TST-05 - Account Number field accepting alpha characters - should only accept 8 digits";
                FormUtilitiesActions.EnterTextIntoInputBox("accountnumber", "abcdefgh");
                FormUtilitiesActions.ClickButton("submitbutton");

                _message = VerifyMyTests.VerifyStringOnThePage("accountnumber", "Please enter a valid Account Number containing digits only", "");
                Thread.Sleep(100);
                using (var file = new StreamWriter(_resultsFile, true))
                {
                    file.WriteLine(_testExplanation + _message);
                }




            }
            catch (Exception)
            {
                var catchError = _fail + ": Something went wrong with running these tests. Double check <" + testCaseId +
                                 "> <" + testDesc + ">";
                Thread.Sleep(1000);
                using (var file = new StreamWriter(_resultsFile, true))
                {
                    file.WriteLine(catchError);
                }
            }
        }













        [TestCleanup]
        public void Cleanup()
        {
            Driver.Close();
        }
    }
}
