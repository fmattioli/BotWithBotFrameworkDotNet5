using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Bot.CardsManager.Classes
{
    public class CardManager
    {
        public string CriarAdaptiveCard(string adaptiveName)
        {
            var cardResourcePath = GetType().Assembly.GetManifestResourceNames().First(name => name.EndsWith(adaptiveName + ".json"));
            object Content;
            using (var stream = GetType().Assembly.GetManifestResourceStream(cardResourcePath))
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    var adaptiveCard = reader.ReadToEnd();
                    Content = JsonConvert.DeserializeObject(adaptiveCard);
                }

                return Content.ToString();
            }
        }
    }
}
