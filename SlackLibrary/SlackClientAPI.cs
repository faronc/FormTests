﻿using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace SlackLibrary
{

    //A simple C# class to post messages to a Slack channel based on
    //https://www.github.com/faronc
    //Note: This class uses the Newtonsoft Json.NET serializer available via NuGet
    public class SlackClientAPI
    {
        private readonly Uri _uri;
        private readonly Encoding _encoding = new UTF8Encoding();
        

        //Post a message using simple strings
        public void PostMessage(string token, string text, string username = null, string channel = null)
        {
            Arguments args = new Arguments()
            {
                Token = token,
                Channel = channel,
                Username = username,
                Text = text
            };

            PostMessage(args);
        }


        public NameValueCollection ToQueryNVC(Object p)
        {

            NameValueCollection nvc = new NameValueCollection();

            foreach (System.Reflection.PropertyInfo propertyInfo in p.GetType().GetProperties())
            {
                if (propertyInfo.CanRead)
                {
                    string JsonProperty = propertyInfo.GetCustomAttributes(true).Where(x => x.GetType() == typeof(JsonPropertyAttribute)).Select(x => ((JsonPropertyAttribute)x).PropertyName).FirstOrDefault();
                    if (propertyInfo.PropertyType == typeof(ObservableCollection<Attachment>))
                    {
                        if (propertyInfo.GetValue(p, null) != null)
                        {
                            nvc[JsonProperty ?? propertyInfo.Name] = JsonConvert.SerializeObject(propertyInfo.GetValue(p, null));
                        }
                    }
                    else
                    {
                        if (propertyInfo.GetValue(p, null) != null)
                        {
                            nvc[JsonProperty ?? propertyInfo.Name] = propertyInfo.GetValue(p, null).ToString();
                        }
                    }

                }
            }

            return nvc;

        }

        //Post a message using args object
        public Response PostMessage(Arguments args)
        {
            //string payloadJson = JsonConvert.SerializeObject(payload);
            using (WebClient client = new WebClient())
            {
                NameValueCollection data = ToQueryNVC(args);
                var response = client.UploadValues(_uri, "POST", data);

                string responseText = _encoding.GetString(response);

                return JsonConvert.DeserializeObject<Response>(responseText);
            }
        }

    }

    public class SlackClientWebhooks
    {
        private readonly Uri _uri;
        private readonly Encoding _encoding = new UTF8Encoding();
        public SlackClientWebhooks(string urlWithAccessToken)
        {
            _uri = new Uri(urlWithAccessToken);
        }
        //Post a message using simple strings
        public void PostMessage(string text, string username = null, string channel = null, ObservableCollection<Attachment> attachments = null, string iconEmoji = null)
        {
            Arguments args = new Arguments()
            {
                Channel = channel,
                Username = username,
                Attachments = attachments,
                IconEmoji = iconEmoji,
                Text = text
            };
            PostMessage(args);
        }
        //Post a message using a args object
        public void PostMessage(Arguments args)
        {
            string argsJson = JsonConvert.SerializeObject(args);
            using (WebClient client = new WebClient())
            {
                NameValueCollection data = new NameValueCollection();
                data["payload"] = argsJson;
                var response = client.UploadValues(_uri, "POST", data);
                //The response text is usually "ok"
                string responseText = _encoding.GetString(response);
            }
        }
    }

    //This classes serializes into the Json payload required by Slack Incoming WebHooks
    public class Arguments
    {
        public Arguments()
        {
            Attachments = new ObservableCollection<Attachment>();
            Parse = "full";
        }
        [JsonProperty("channel")]
        public string Channel { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("parse")]
        public string Parse { get; set; }
        [JsonProperty("link_names")]
        public string LinkNames { get; set; }
        [JsonProperty("unfurl_links")]
        public string UnfurlLinks { get; set; }
        [JsonProperty("unfurl_media")]
        public string UnfurlMedia { get; set; }
        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }
        [JsonProperty("icon_emoji")]
        public string IconEmoji { get; set; }
        [JsonProperty("attachments")]
        public ObservableCollection<Attachment> Attachments { get; set; }
    }

    public class Attachment
    {
        public Attachment()
        {
            Fields = new ObservableCollection<AttachmentFields>();
        }
        [JsonProperty("fallback")]
        public string Fallback { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("pretext")]
        public string Pretext { get; set; }
        [JsonProperty("color")]
        public string Color { get; set; }
        [JsonProperty("fields")]
        public ObservableCollection<AttachmentFields> Fields { get; set; }
    }

    public class AttachmentFields
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("short")]
        public bool Short { get; set; }
    }

    public class Response
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }
        [JsonProperty("channel")]
        public string Channel { get; set; }
        [JsonProperty("ts")]
        public string TimeStamp { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
    }

}