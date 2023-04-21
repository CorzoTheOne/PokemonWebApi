using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Http.Headers;
using static System.Net.WebRequestMethods;
using HtmlAgilityPack;
using PokemonWebApi.Data;
using PokemonWebApi.Model;
using CompressedStaticFiles;

namespace PokemonWebApi
{
    public class PokemonScraper
    {
        private static async Task<string> CallUrl(string fullUrl)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(fullUrl);
            return response;
        }

        public static void PokemonCrawler()
        {
            string url = "https://pokemondb.net/pokedex/all";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            var pokemonNames = doc.DocumentNode.SelectNodes("//a[@class='ent-name'] | //span[@class='infocard-cell-data']");
            var nodeSet = new List<HtmlNode>(pokemonNames);
            //var uniqueList = RemoveDuplicates(nodeSet);
            var uniqueNodes = nodeSet.ToList();

            (string, string) pokeData = (null, null);
            int count = 0;
            foreach (var name in uniqueNodes)
            {
                if (count == 0)
                {
                    pokeData.Item2 = name.InnerText;
                    count++;
                }
                else if(count == 1)
                {
                    pokeData.Item1 = name.InnerText;
                    AddPokemon(pokeData);
                    count = 0;
                    Console.WriteLine($"Pokemon: {pokeData.Item1} - with Number: {pokeData.Item2}");
                    Console.WriteLine("Has been added to the database succesfully.");
                }
            }
            //string htmlContent = doc.DocumentNode.OuterHtml;
            //System.IO.File.WriteAllText("htmlConent.html", htmlContent);
            Console.WriteLine(doc);
        }
        public static void GetPokemonDetails(HtmlNode name)
        {
            string url = $"https://pokemondb.net/pokedex/{name.InnerText}";
            Console.WriteLine($"Name: {name.InnerText}");
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            var pokemonData = doc.DocumentNode.SelectNodes("//*[@id=\"main\"]/h1");
            //*[@id="main"]/h1
            Console.WriteLine(pokemonData);
            // | //*[@id='tab-basic-6']/div[1]/div[2]/table/tbody/tr[1]/td/strong | //a[class='itype']
            foreach (var data in pokemonData)
            {
                Console.WriteLine(data.InnerText);
            }
        }

        public static async void AddPokemon((string, string) node)
        {
            var dbContext = new PokemonContext();

            var pokemon = new Pokemon();
            pokemon.Name = node.Item1;
            pokemon.Number = Int32.Parse(node.Item2);

            dbContext.Pokemons.Add(pokemon);
            _ = await dbContext.SaveChangesAsync();
        }

        public static List<HtmlNode> RemoveDuplicates(List<HtmlNode> htmlNodes)
        {
            List<HtmlNode> uniqueList = new(); 

            foreach (HtmlNode node in htmlNodes)
            {
                bool isDuplicate = false;

                foreach(HtmlNode uniqueNode in uniqueList)
                {
                    if (node.SelectSingleNode("ent-name").InnerText == uniqueNode.SelectSingleNode("ent-name").InnerText && node.SelectSingleNode("infocard-cell-data").InnerText == uniqueNode.SelectSingleNode("infocard-cell-data").InnerText);
                    {
                        isDuplicate = true;
                        break;
                    }
                }

                if (!isDuplicate)
                {
                    uniqueList.Add(node);
                }
            }
            return uniqueList;
        }

        public static void GetImages()
        {
            var dbContext = new PokemonContext();
            List<Pokemon> pokemonNames = new List<Pokemon>();
            
            using (var db = new PokemonContext())
            {
                pokemonNames = db.Pokemons.ToList();
                for (int i = 0; i < pokemonNames.Count; i++)
                {
                    if (pokemonNames[i].Name.Contains(" "))
                    {
                        pokemonNames[i].Name.Replace(" ", "-");
                    }
                    Console.WriteLine(pokemonNames[i].Name);
                    string url = $"https://pokemondb.net/pokedex/{pokemonNames[i].Name.ToLower()}";
                    Console.WriteLine(url);
                    HtmlWeb web = new HtmlWeb();
                    HtmlDocument doc = new HtmlDocument();
                    doc = web.Load(url);

                    var imgNodes = doc.DocumentNode.SelectNodes("//img");

                    Console.WriteLine(imgNodes);
                    foreach (var imgNode in imgNodes)
                    {
                        var imgUrl = imgNode.GetAttributeValue("src", "");
                        if (!string.IsNullOrEmpty(imgUrl))
                        {
                            using (var client = new WebClient())
                            {
                                var fileName = Path.GetFileName(imgUrl);
                                // Console.WriteLine(fileName + $"=?= {pokemonNames[i].Name.ToLower()}.jpg");
                                var savePath = Path.Combine("images", fileName);
                                if (fileName == $"{pokemonNames[i].Name.ToLower()}.avif")
                                {
                                    client.DownloadFile(imgUrl, savePath);
                                    Console.WriteLine($"Downloaded {fileName}");
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCompressedStaticFiles();
        }


    }
}
