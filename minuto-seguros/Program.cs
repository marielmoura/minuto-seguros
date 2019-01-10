using CodeHollow.FeedReader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;


namespace minuto_seguros
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Feed feed = getFeed();
                showWordCounterByFeed(feed);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw ex;
            }

        }

        static Feed getFeed()
        {
            string remoteFeed = "https://www.minutoseguros.com.br/blog/feed/";
            string localFeed = "feed.xml";

            try
            {
                return FeedReader.Read(remoteFeed);
            }
            catch (Exception)
            {
                try
                {
                    Console.WriteLine("Can't read rss remote feed: " + remoteFeed);
                    Console.WriteLine("Trying to read local feed: " + localFeed);
                    return FeedReader.ReadFromFile(localFeed);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    throw ex;
                }
            }

        }

        static void showWordCounterByFeed(Feed feed)
        {
            try
            {
                foreach (var item in feed.Items)
                {
                    var content = Regex.Replace(item.Content, "<.*?>", " ");

                    IDictionary<string, int> wordsDictionary = new Dictionary<string, int>();

                    var invalidWords = new[] { " ", "o", "a", "e", "para", "de", "do", "é", "se", "que", "com", "mais",
                "por", "uma", "você", "em", "os", "não", "um", "no", "sem", "sua", "como", "ao", "está", "ser",
                "seu", "já", "nem", "pelo", "fica", "pode", "quando", "nos", "dos", "há", "isso", "da", "pelas",
                "outras", "porém", "lá", "vão", "tão", "as", "na", "essa", "pelos", "à", "das", "suas", "entre",
                "até", "nas", "ou", "ele", "ela", "seja", "são", "além", "ter", "me"};

                    foreach (string word in content.Split(' '))
                    {
                        string wordToLower = Regex.Replace(word.ToLower(), "[^a-záàâãéèêíïóôõöúçñ ]+", "", RegexOptions.Compiled);

                        if (String.IsNullOrEmpty(wordToLower) || invalidWords.Contains(wordToLower))
                            continue;

                        if (wordsDictionary.ContainsKey(wordToLower))
                            wordsDictionary[wordToLower] += 1;
                        else
                            wordsDictionary.Add(wordToLower, 1);
                    };

                    foreach (KeyValuePair<string, int> word in wordsDictionary.OrderByDescending(x => x.Value).Take(10))
                    {
                        Console.WriteLine(item.Title + " - " + word.Key + " - " + word.Value);
                    }
                
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                }

                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw ex;
            }
        }
    }
}
