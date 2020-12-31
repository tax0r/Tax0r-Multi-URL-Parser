using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tax0r_MultiURL_parser.Classes;
using System.Drawing;
using Console = Colorful.Console;
using System;
using System.Threading;

namespace Tax0r_MultiURL_parser
{
    class Program
    {
        static async Task Main(string[] args)
        {
            UrlParserClass parserClass = new UrlParserClass();
            FileHelperClass helperClass = new FileHelperClass();
            FilterHelperClass filterHelper = new FilterHelperClass();

            filterHelper.addFilters("yahoo.com");
            filterHelper.addFilters("google.com");
            filterHelper.addFilters("bing.com");
            filterHelper.addFilters("bingj.com");
            filterHelper.addFilters("live.com");
            filterHelper.addFilters("w3.org");
            filterHelper.addFilters("microsofttranslator.com");
            filterHelper.addFilters("wikipedia.org");
            filterHelper.addFilters("twitter.com");
            filterHelper.addFilters("youtube.com");
            filterHelper.addFilters("facebook.com");
            filterHelper.addFilters("instagram.com");
            filterHelper.addFilters("microsoft.com");
            filterHelper.addFilters("giga.de");
            filterHelper.addFilters("msn.com");
            filterHelper.addFilters("outlook.com");
            filterHelper.addFilters("creativecommons.org");
            filterHelper.addFilters("trustscam.nl");
            filterHelper.addFilters("aol.de");
            filterHelper.addFilters("yandex.com");
            filterHelper.addFilters("verbraucherschutz.de");
            filterHelper.addFilters("whois.com");
            filterHelper.addFilters("bingparachute.com");
            filterHelper.addFilters("duckduckgo.com");
            filterHelper.addFilters(".js");
            filterHelper.addFilters(".png");
            filterHelper.addFilters(".jpg");
            filterHelper.addFilters(".ttf");
            filterHelper.addFilters(".pdf");
            filterHelper.addFilters(".css");
            filterHelper.addFilters(".svg");
            filterHelper.addFilters(".sh");

            Console.WriteAscii("TAX0R, 2020", Color.LightPink);

            Thread.Sleep(TimeSpan.FromSeconds(2));

            Console.Clear();
            Console.WriteLine("[Important!]: Please drag&drop you'r URL-list into the Application.", Color.Pink);

            string toReplace = '"'.ToString();
            string input = Console.ReadLine().Replace(toReplace, string.Empty);
            Console.WriteLine(input);
            Console.Clear();

            string[] urls = helperClass.readUrlsFromFile(input);
            List<string> scrapedUrls = new List<string>();
            
            foreach (string url in urls)
            {
                if (filterHelper.isFiltered(url))
                {
                    try
                    {
                        Console.WriteLine("[OLD URL]: " + url, Color.Green);

                        string content = await parserClass.GetUrlContent(url);

                        string[] foundUrls = parserClass.SearchForUrls(content);

                        foreach (string foundUrl in foundUrls)
                        {
                            if (filterHelper.isFiltered(foundUrl))
                            {
                                Console.WriteLine("[NEW URL]: " + foundUrl, Color.LightGreen);
                                scrapedUrls.Add(foundUrl);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("[BAD URL]: " + url, Color.Red);
                    }
                }
                else
                {
                    Console.WriteLine("[BAD URL]: " + url, Color.Red);
                }
            }

            List<string> distinctUrls = scrapedUrls.Distinct().ToList();

            Console.Clear();
            Console.WriteLine("[Success!]: URL's we're scraped successfully!");
            Console.WriteLine("[Information]: New URL's found: " + scrapedUrls.Count(), Color.LightBlue);
            Console.WriteLine("[Information]: Distinct URL's found: " + distinctUrls.Count(), Color.LightPink);

            helperClass.saveToFile(distinctUrls.ToArray(), distinctUrls.Count());
           
            Console.WriteLine("\npress any key to exit the process...");
            Console.ReadKey();
        }
    }
}
