using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Web.Bot.CognitiveModels
{
    public partial class Atendimento : IRecognizerConvert
    {
        public string Text { get; set; }
        public string AlteredText { get; set; }
        public enum Intent
        {
            Saudacao,
            None
        };
        public Dictionary<Intent, IntentScore> Intents { get; set; }
        public void Convert(dynamic result)
        {
            var app = JsonConvert.DeserializeObject<Atendimento>(JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            Text = app.Text;
            AlteredText = app.AlteredText;
            Intents = app.Intents;
        }

        public (Intent intent, double score) TopIntent()
        {
            Intent maxIntent = Intent.None;
            var max = 0.0;
            foreach (var entry in Intents)
            {
                if (entry.Value.Score > max)
                {
                    maxIntent = entry.Key;
                    max = entry.Value.Score.Value;
                }
            }
            return (maxIntent, max);
        }


    }
}
