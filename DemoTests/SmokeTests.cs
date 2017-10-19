using System;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonClassUtils;
using DemoActions;
using SlackLibrary;

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
        private string _warning = "Warning";
        private string _testExplanation = String.Empty;
        private bool _status = true;
        public static string DateTimeForFileName = DateTime.Now.ToString(@"MMM\-dd-HH\-mm");
        
        private string _jiraTicketNumberWithAutomationResults = String.Empty;
        private bool _createJiraTicket;
        private string jiraThreeLetterProjectId = "TST";
        private string jiraParentId = "TST-92";

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
        /// SLACK: https://faronc.slack.com
        /// </summary>
        [TestMethod]
        public void DataDriverDemoTests()
        {
            const string testCaseId = "Demo_01";
            const string testProject = "Sample Demo form project";
            const string testTitle = "Nov 2017: Running Smoke tests on each deployment";
            const string testDesc = "Bug regression to ensure previous bugs are still fixed";
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
                        "Test Data Used in this test:\nTitle: {0}\nFirst Name: {1}\nSurname: {2}\nEmail: {3}\nGender: {4}\nDate of Birth: {5}\nMobile Number: {6}\nAddress Line 1: {7}\nAddress Line 2: {8}\n" +
                        "County: {9}\nAccount Number: {10}\nSort Code Middle: {11}\nSort Code Last: {12}\nDescription of Query: {13}\n",
                        title, firstName, surname, email, gender, dateOfBirth, mobilePhoneNumber, addressLine1,
                        addressLine2, county, accountNumber, sortCodeMiddle, sortCodeLast, descriptionOfQuery);


                using (var file = new StreamWriter(_resultsFile, true))
                {
                    file.WriteLine(testDataUsed);
                }



                Driver.Goto(_url);

                // PAGE 1 - GETTING STARTED

                Thread.Sleep(1000);
                _testExplanation = "\nTEST SCENARIO: Populating form with data that caused issues previously to ensure these are still fixed\n\n";
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


                // START TEST 1
                _testExplanation = "BUG NUMBER: TST-92 - Account Number field accepting alpha characters - should only accept 8 digits\n";
                FormUtilitiesActions.EnterTextIntoInputBox("accountnumber", "abcdefgh");
                FormUtilitiesActions.ClickButton("submitbutton");

                _message = VerifyMyTests.VerifyStringOnThePage("Please enter a valid Account Number containing digits only", "Thank You");
                Thread.Sleep(100);
                using (var file = new StreamWriter(_resultsFile, true))
                {
                    file.WriteLine(_testExplanation + _message);
                }
                FormUtilitiesActions.EnterTextIntoInputBox("accountnumber", accountNumber);
                // END TEST 1




                // START TEST 2
                _testExplanation = "BUG NUMBER: TST-93 - Email must be a valid email\n";
                FormUtilitiesActions.EnterTextIntoInputBox("email", "invalid.email");
                FormUtilitiesActions.ClickButton("submitbutton");

                _message = VerifyMyTests.VerifyStringOnThePage("Please enter a valid email address", "Thank You");
                Thread.Sleep(100);
                using (var file = new StreamWriter(_resultsFile, true))
                {
                    file.WriteLine(_testExplanation + _message);
                }

                FormUtilitiesActions.EnterTextIntoInputBox("email", email);
                // END TEST 3





                // START TEST 3
                _testExplanation = "Submitting the form with all valid data and verifying the Thank You page\n";
                FormUtilitiesActions.ClickButton("submitbutton");
                _message = VerifyMyTests.VerifyStringOnThePage("Confirmation;Thank You;Thank you for your query. We will contact you within 24 hours.", "");
                Thread.Sleep(100);
                using (var file = new StreamWriter(_resultsFile, true))
                {
                    file.WriteLine(_testExplanation + _message);
                }
                // END TEST 3





                Thread.Sleep(100);
                using (var file = new StreamWriter(_resultsFile, true))
                {
                    file.WriteLine("TEST COMPLETE");
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



            _jiraTicketNumberWithAutomationResults = JiraActions.PostToJira(testProject, testCaseId, jiraUsername, jiraPassword, jiraThreeLetterProjectId, _resultsFile, jiraParentId);

            PostToSlack.PostResultsToSlack(bvtResult, testTester, testCaseId, testProject, testTitle, testDesc, _resultsFile, _jiraTicketNumberWithAutomationResults, _url, _urlWithAccessToken, _slackChannel);

        }



        [TestCleanup]
        public void Cleanup()
        {
            Driver.Close();
        }
    }
}
