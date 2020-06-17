using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using VoiceControl;

namespace DragonflyTest
{
    public class OnlineDownloader
    {
        string url;
        public OnlineDownloader(string url)
        {
            this.url = url;
        }
        private string GetDirectoryListingRegexForUrl()
        {
            if (url.Contains("jonasheinrich.de"))
            {
                return "<a href=\"(?<link>.*)\"> (?<name>.*)</a>";
            }
            throw new NotSupportedException();
        }

        private string PathUrl(string path)
        {
            return url + "/" + path;
        }

        public byte[] DownloadFile(string path)
        {
            using (var client = new WebClient())
            {
                return client.DownloadData(PathUrl(path));
            }
        }



        /*text in quoteif if if  "((\\")|[^"])+"
         * Mark block identifier.[\w_]+\s=\s\{[^\{\}]*\}
         * Full function with command."[\w\s\]\[\|\(\)<>]+":(\((?<A>)|\)(?<-A>)|[\w_=.]+|"((\\")|[^"])+"|\n|\s|,|\+)+,(?(A)(?!)) 
         * new Function with command. \s*"[\w\s\]\[\|\(\)<>]+":([\w._]*\((?<A>)|\)(?<-A>)|\w+=\w+|"((\\")|[^"])+"|\s|,|\+)+,(?(A)(?!))
         * Function call with quote [A-Z]\w+\(("((\\")|[^"])+"|[^\)])*\)
         * Multiple function calls. (([A-Z]\w+\(("((\\")|[^"])+"|[^\)])*\))\s*[\+]*\s*)+,
         * Entire command. \s*"[\w\s\]\[\|\(\)<>]+":\s*(([A-Z]\w+\(("((\\")|[^"])+"|[^\)])*\))\s*[\+]*\s*)+
         * Entire block (\s*"[\w\s\]\[\|\(\)<>']+":\s*(([\w.]+\(("((\\")|[^"])+"|[^\)])*\)|None)\s*[\+]*\s*)+,|#[\s\w+]*)+
        "[A-Z]\w+\([^\)]*\)"
        "\s*"[\w\s\]\[\|\(\)<>]+":\s*"
            "\s*"[\w\s\]\[\|\(\)<>]+":\s*[^,]+,"
        \w+ = \{(\s*"\w+":\s*.*,)+
        */

        public List<string> ListDirectories(string path)
        {
            List<string> found = new List<string>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(PathUrl(path));
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                ;
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string html = reader.ReadToEnd();
                    Regex regex = new Regex(GetDirectoryListingRegexForUrl());
                    MatchCollection matches = regex.Matches(html);
                    if (matches.Count > 0)
                    {
                        foreach (Match match in matches)
                        {
                            if (match.Success)
                            {
                                found.Add(Path.Combine(path, match.Groups["name"].Value));
                            }
                        }
                    }
                }
            }
            return found;
        }
    }
}
