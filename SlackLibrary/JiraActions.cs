using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using RestSharp;
using RestSharp.Authenticators;
using CommonClassUtils;

namespace SlackLibrary
{
    public class JiraActions
    {
        public static string PostToJira(string testProject, string testCaseId, string jiraUsername, string jiraPassword, string jiraThreeLetterProjectId, string resultsFile, string jiraParentId)
        {
            var jiraTicketNumberWithAutomationResults = string.Empty;
            /* 
            * START CREATE NEW JIRA TICKET FUNCTIONALITY
            * Need to add jiraThreeLetterProjectId to each TestMethod
            * Need to add jiraParentId to each TestMethod
            * Need to add _createJiraTicket to the top of each class 
            */

            try
            {
                var summaryOfTicket = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", "Automation results for ", testProject,
                    ". Run Date: ", GenerateRandomStrings.GetTodaysDate(), ". BVT ID: ", testCaseId, ". Env: ", "Live");
                // this is where i create the jira ticket. There is aTASK already created in Jira and I'm going to create a SUB-TASK
                jiraTicketNumberWithAutomationResults = JiraActions.CreateJiraTicket(jiraThreeLetterProjectId,
                    summaryOfTicket, resultsFile, jiraParentId, jiraUsername, jiraPassword);
            }
            catch (Exception e)
            {
                jiraTicketNumberWithAutomationResults = "Could not create Jira ticket: " + e.Message.ToString();
            }
            return jiraTicketNumberWithAutomationResults;
            // END CREATE NEW JIRA TICKET FUNCTIONALITY
        }


        public static string CreateJiraTicket(string jiraProject, string jiraSummary, string jiraResultsFile, string jiraParentId, string jiraUsername, string jiraPassword)
        {
            var client = new RestClient("https://YOUR-JIRA-ENDPOINT.com/rest/api/2");
            var jiraBaseUri = "https://YOUR-JIRA-ENDPOINT.com/browse/";
            var request = new RestRequest("issue/", Method.POST);
            string ticketCreated;
            string contentsOfAutomationResultsFile;

            using (StreamReader sr = new StreamReader(jiraResultsFile))
            {
                contentsOfAutomationResultsFile = sr.ReadToEnd();
            }

            // I'm using Basic Authentication not OAuth
            client.Authenticator = new HttpBasicAuthenticator(jiraUsername, jiraPassword);

            var issue = new Issue
            {
                fields =
                    new Fields
                    {
                        parent = new Parent { id = jiraParentId },
                        description = contentsOfAutomationResultsFile,
                        summary = jiraSummary,
                        project = new Project { key = jiraProject },
                        issuetype = new IssueType { name = "Sub-task" },
                        customfield_10216 = new Customfield_10216 { id = "10135" } // Owner field
                    }
            };

            request.AddJsonBody(issue);

            var res = client.Execute<Issue>(request);

            if (res.StatusCode == HttpStatusCode.Created)
            {
                var ticketUrl = BuildUri.BuildUrl(jiraBaseUri, res.Data.key);
                ticketCreated = string.Format("{0}", ticketUrl);

            }
            else
                ticketCreated = string.Format(res.Content);

            return ticketCreated;
        }
    }

    public class Issue
    {
        public string id { get; set; }
        public string key { get; set; }
        public Fields fields { get; set; }
    }

    public class Fields
    {
        public Parent parent { get; set; }
        public Project project { get; set; }
        public IssueType issuetype { get; set; }
        public string summary { get; set; }
        public string description { get; set; }
        public Customfield_10216 customfield_10216 { get; set; }
    }

    public class Project
    {
        public string id { get; set; }
        public string key { get; set; }
    }

    public class Parent
    {
        public string id { get; set; }
        public string key { get; set; }
    }

    public class IssueType
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Customfield_10216
    {
        public string id { get; set; }
        public string name { get; set; }
    }

}
