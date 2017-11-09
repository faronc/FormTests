using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClassUtils
{
    public class BuildUri
    {
        public static string BuildUrl(string baseUri, string urlParams)
        {
            // Build the url. It doesnt matter if either parameter starts with or ends with a / we trim both and add our own here
            baseUri = (baseUri.EndsWith("/")) ? baseUri.TrimEnd('/') : baseUri;
            urlParams = (urlParams.StartsWith("/")) ? urlParams.Substring(1) : urlParams;

            string generatedUrl;

            if (baseUri == "")
            {
                generatedUrl = urlParams;
            }
            else
            {
                generatedUrl = baseUri + @"/" + urlParams;
            }

            return generatedUrl;
        }
    }
}
