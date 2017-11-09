using System;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonClassUtils;
using DemoActions;

namespace DemoTests
{
    [TestClass]
    public class DataXmlTests
    {
        private string _environment = string.Empty;
        private string _resultsFile = string.Empty;
        private string _resultsFileAsXls = string.Empty;
        private string _message = string.Empty;
        private string _resultsFolder = string.Empty;
        private string _url = string.Empty;
        private string _pass = "Pass";
        private string _fail = "Fail";
        private string _testExplanation = String.Empty;
        private bool _status = true;
        public static string DateTimeForFileName = DateTime.Now.ToString(@"MMM\-dd-HH\-mm");
        private string _testSeparate = "\n\n\n********** --------------------- **********\n\n\n";


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
        /// Data Driven tests for the X form
        /// Tests are stored in DemoTestData.xml under the TestData folder
        /// Each test is run against a clean browser instance
        /// </summary>
        [DeploymentItem("TestData\\DemoTestData.xml"),
         DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
             "|DataDirectory|\\TestData\\DemoTestData.xml", "stringTest", DataAccessMethod.Sequential),
         TestMethod]
        public void DataDriverDemoTests()
        {
            var testCaseId = Convert.ToString(_testContextInstance.DataRow["TestCaseID"]);
            var testTitle = Convert.ToString(_testContextInstance.DataRow["TestTitle"]);
            var testDesc = Convert.ToString(_testContextInstance.DataRow["TestDesc"]);
            var bugNumber = Convert.ToString(_testContextInstance.DataRow["BugNumber"]);
            var expectedScenario = Convert.ToString(_testContextInstance.DataRow["ExpectedScenario"]);
            var convertToExcelNow = Convert.ToString(_testContextInstance.DataRow["ConvertToExcelNow"]);
            var textToAppear = Convert.ToString(_testContextInstance.DataRow["TextToAppear"]);
            var textNotToAppear = Convert.ToString(_testContextInstance.DataRow["TextNOTToAppear"]);

            try
            {
                var title = Convert.ToString(_testContextInstance.DataRow["Title"]);
                var firstName = Convert.ToString(_testContextInstance.DataRow["FirstName"]);
                var surname = Convert.ToString(_testContextInstance.DataRow["Surname"]);
                var email = Convert.ToString(_testContextInstance.DataRow["Email"]);
                var gender = Convert.ToString(_testContextInstance.DataRow["Gender"]);
                var dateOfBirth = Convert.ToString(_testContextInstance.DataRow["DateOfBirth"]);
                var mobilePhoneNumber = Convert.ToString(_testContextInstance.DataRow["MobilePhoneNumber"]);
                var addressLine1 = Convert.ToString(_testContextInstance.DataRow["AddressLine1"]);
                var addressLine2 = Convert.ToString(_testContextInstance.DataRow["AddressLine2"]);
                var county = Convert.ToString(_testContextInstance.DataRow["County"]);
                var accountNumber = Convert.ToString(_testContextInstance.DataRow["AccountNumber"]);
                var sortCodeMiddle = Convert.ToString(_testContextInstance.DataRow["SortCodeMiddle"]);
                var sortCodeLast = Convert.ToString(_testContextInstance.DataRow["SortCodeLast"]);
                var descriptionOfQuery = Convert.ToString(_testContextInstance.DataRow["DescriptionOfQuery"]);
                



                // Settings Variables
                _resultsFolder = Config.Default.resultsfile;
                _url = Config.Default.url;

                bool dirExists = Directory.Exists(_resultsFolder);
                if (!dirExists)
                    Directory.CreateDirectory(_resultsFolder);

                _resultsFile = _resultsFolder + @"\DataDriverDemoTests_" + DateTimeForFileName + ".txt";
                _resultsFileAsXls = _resultsFolder + @"\DataDriverDemoTests_" + DateTimeForFileName + ".xls";

                _message =
                    string.Format(
                        "Test Case ID: {0}\nTest title: {1}\nTest Description: {2}\nBug Number(if any): {3}\nExpected Scenario: {4}\n\nUrl: {5}\nTest Results: {6}\n\n",
                        testCaseId, testTitle, testDesc, bugNumber, expectedScenario, _url, _resultsFolder);
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
                FormUtilitiesActions.ClickButton("submitbutton");



                // Verify Your Tests
                _testExplanation = "TEST CASE: Verify the expected success/failure strings appear on the page\n";
                _message = VerifyMyTests.VerifyStringOnThePage(textToAppear, textNotToAppear);
                Thread.Sleep(100);
                using (var file = new StreamWriter(_resultsFile, true))
                {
                    file.WriteLine(_testExplanation + _message + _testSeparate);
                }





                if (convertToExcelNow.ToUpper().Contains("Y"))
                {
                    //Txt2XLS.ConvertMyTxtToExcel(_resultsFile, _resultsFileAsXls);
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
