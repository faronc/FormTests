using System;
using System.IO;
using System.Text.RegularExpressions;

namespace SlackLibrary
{
    public class SlackActions
    {
        public static string colorOfResult = string.Empty;
        public static string iconOfResult = string.Empty;

        public static string CreateMessageToSendToSlack(string testCaseId, string testProject, string testTitle, string testDesc, string slackNotes, string testTester, string bvtResult, string environment, string resultsFile, string url)
        {
            // FORMAT: {"text":"This is a line of text.\nAnd this is another one."}
            var message = string.Empty;

            testCaseId = !string.IsNullOrEmpty(testCaseId)
                ? string.Format("Test Case ID: {0}\n", testCaseId)
                : "";

            testProject = !string.IsNullOrEmpty(testProject)
                ? string.Format("Project: {0} on {1}\n", testProject, environment)
                : "";

            testTitle = !string.IsNullOrEmpty(testTitle)
                ? string.Format("Test Title: {0}\n", testTitle)
                : "";

            testDesc = !string.IsNullOrEmpty(testDesc)
                ? string.Format("Test Description: {0}\n", testDesc)
                : "";

            slackNotes = !string.IsNullOrEmpty(slackNotes)
                ? string.Format("Jira Ticket: {0}\n", slackNotes)
                : "";

            testTester = !string.IsNullOrEmpty(testTester)
                ? string.Format("Tester: {0}\n", testTester)
                : "";

            bvtResult = !string.IsNullOrEmpty(bvtResult)
                ? string.Format("BVT Result: {0}\n", bvtResult)
                : "";

            environment = !string.IsNullOrEmpty(environment)
                ? string.Format("Environment: {0}\n", environment)
                : "";

            resultsFile = !string.IsNullOrEmpty(resultsFile)
                ? string.Format("Results File: {0}\n", resultsFile)
                : "";

            url = !string.IsNullOrEmpty(url)
                ? string.Format("Url: {0}\n", url)
                : "";

            var date = "Date: " + DateTime.Now.ToShortDateString() + "\n";

            return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", testProject, bvtResult, testCaseId, testTitle, testDesc, environment, date, testTester, slackNotes, resultsFile, url);
        }



        public static void CalculateColorOfResult(string result)
        {
            colorOfResult = string.Empty;
            iconOfResult = string.Empty;

            //:smiley :frowning :cry
            switch (result)
            {
                case "Pass":
                    colorOfResult = "good";
                    iconOfResult = ":smiley:";
                    break;
                case "Warning":
                    colorOfResult = "warning";
                    iconOfResult = ":question:";
                    break;
                case "Fail":
                    colorOfResult = "danger";
                    iconOfResult = ":cry:";
                    break;
                case "SignOff":
                    colorOfResult = "#1730ed";
                    iconOfResult = ":boom:";
                    break;
                default:
                    colorOfResult = "#0066ff";
                    iconOfResult = ":wink:";
                    break;
            }

            //return colorOfResult + ";" + iconOfResult;
        }


        public static int CountTheNumberOfX(string resultsFile, string strToSearchFor)
        {
            int count;
            try
            {
                using (var file = new StreamReader(resultsFile, true))
                {
                    var source = file.ReadToEnd();
                    count = new Regex(Regex.Escape(strToSearchFor.ToUpper())).Matches(source.ToUpper()).Count;
                }
            }
            catch (Exception)
            {
                count = -1;
            }


            return count;
        }


        public static string CreateMessageToSendToSlackForSignOff(string group, string title, string owner, DateTime devSignOff, DateTime stgSignOff, string notes, bool qaStatus)
        {
            // FORMAT: {"text":"This is a line of text.\nAnd this is another one."}
            var message = string.Empty;

            group = !string.IsNullOrEmpty(group)
                ? string.Format("Project Group: {0}\n", group)
                : "";

            title = !string.IsNullOrEmpty(title)
                ? string.Format("Project Title: {0}\n", title)
                : "";

            owner = !string.IsNullOrEmpty(owner)
                ? string.Format("Owner: {0}\n", owner)
                : "";

            var devSignOffStr = devSignOff.ToString("yyyy-MM-dd");
            var stgSignOffStr = stgSignOff.ToString("yyyy-MM-dd");

            devSignOffStr = !string.IsNullOrEmpty(devSignOffStr)
                ? string.Format("Dev Sign Off Date: {0}\n", devSignOffStr)
                : "";

            stgSignOffStr = !string.IsNullOrEmpty(stgSignOffStr)
                ? string.Format("Stg Sign Off Date: {0}\n", stgSignOffStr)
                : "";

            notes = !string.IsNullOrEmpty(notes)
                ? string.Format("Notes: {0}\n", notes)
                : "";

            var qaStatusStr = qaStatus ? "Complete" : "Not Complete";

            qaStatusStr = !string.IsNullOrEmpty(qaStatusStr)
                ? string.Format("QA Status: {0}\n", qaStatusStr)
                : "";


            var date = "Date: " + DateTime.Now.ToShortDateString() + "\n";

            return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", group, title, owner, devSignOffStr, stgSignOffStr, notes, qaStatusStr, date);
        }

    }
}
