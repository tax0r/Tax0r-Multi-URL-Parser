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
                if(!url.EndsWith(".png") && !url.EndsWith(".jpg") && !url.EndsWith(".pdf") && !url.EndsWith(".css") && !url.EndsWith(".svg") && !url.EndsWith(".ttf"))
                {
                    try
                    {
                        Console.WriteLine("[OLD URL]: " + url, Color.Green);

                        string content = await parserClass.GetUrlContent(url);

                        string[] foundUrls = parserClass.SearchForUrls(content);

                        foreach (string foundUrl in foundUrls)
                        {
                            Console.WriteLine("[NEW URL]: " + foundUrl, Color.LightGreen);
                            scrapedUrls.Add(foundUrl);
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
        }
    }
}
