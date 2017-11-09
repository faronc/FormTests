using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SlackLibrary
{

    public class PostToSlack
    {

        

        public static void PostResultsToSlack(string bvtResult, string testTester, string testCaseId, string testProject, string testTitle, string testDesc, string _resultsFile, string _jiraTicketNumberWithAutomationResults, string _url, string _urlWithAccessToken, string _slackChannel)
        {
            const string _pass = "Pass";
            const string _fail = "Fail";
            const string _warning = "Warning";
            var _message = string.Empty;

        // START OF SLACK INTEGRATION
        int numberOfFails = SlackActions.CountTheNumberOfX(_resultsFile, _fail);
            int numberOfWarnings = SlackActions.CountTheNumberOfX(_resultsFile, _warning);
            int numberOfPasses = SlackActions.CountTheNumberOfX(_resultsFile, _pass);

            var addedInfo = string.Empty;
            int numberToIncludeInResults = 0;

            if (numberOfWarnings == 0 && numberOfFails == 0)
            {
                bvtResult = _pass;
                addedInfo = "passing";
                numberToIncludeInResults = numberOfPasses;
            }
            else if (numberOfWarnings > 0 && numberOfFails == 0)
            {
                bvtResult = _warning;
                addedInfo = "failing to run - please debug and re-try";
                numberToIncludeInResults = numberOfWarnings;
            }
            else if (numberOfFails > 0)
            {
                bvtResult = _fail;
                addedInfo = "failing";
                numberToIncludeInResults = numberOfFails;
            }


            var testOrTests = numberToIncludeInResults == 1 ? "test" : "tests";

            var bvtResultAddedInfo = string.Format("{0} with {1} {2} {3}", bvtResult, numberToIncludeInResults,
                testOrTests, addedInfo);


            try
            {
                var testTesterFromAlias = string.Format("{0} {1}", testTester, bvtResult);
                SlackActions.CalculateColorOfResult(bvtResult);
                var iconOfResult = SlackActions.iconOfResult;
                var colorOfResult = SlackActions.colorOfResult;
                var fileNameOnly = Path.GetFileName(_resultsFile);

                string messageToSendToSlack = SlackActions.CreateMessageToSendToSlack(testCaseId, testProject,
                    testTitle, testDesc, _jiraTicketNumberWithAutomationResults, testTester, bvtResultAddedInfo, "Live",
                    fileNameOnly, _url);
                const string pretextText = "Results of Smoke Test";

                SlackClientWebhooks client = new SlackClientWebhooks(_urlWithAccessToken);

                Arguments p = new Arguments();
                p.Channel = _slackChannel;
                p.Username = testTesterFromAlias;
                p.Text = "";
                p.Token = _urlWithAccessToken;
                p.IconEmoji = iconOfResult;

                Attachment a = new Attachment();
                a.Fallback = testProject;
                a.Color = colorOfResult;
                a.Pretext = pretextText;


                AttachmentFields af = new AttachmentFields();
                af.Title = "Field 1";
                af.Value = "Value 1";
                af.Short = false;
                a.Fields.Add(af);

                AttachmentFields af2 = new AttachmentFields();
                af.Title = "BVT Result";
                af.Value = messageToSendToSlack;
                af2.Short = true;
                a.Fields.Add(af2);

                p.Attachments.Add(a);

                client.PostMessage(p);
            }
            catch (Exception)
            {
                _message = "Unable to post to Slack";
                Thread.Sleep(500);
                using (var file = new StreamWriter(_resultsFile, true))
                {
                    file.WriteLine(_message);
                }
            }
            // END OF SLACK INTEGRATION
        }

    }
}
