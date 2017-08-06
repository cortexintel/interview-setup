using System;
using System.Collections.Generic;
using System.Linq;
using Console = System.Console;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.tagger.maxent;

namespace API
{
	public class MessageParser : IMessageParser
	{
		private readonly string _modelsDirectory;
		
		public MessageParser(string nlpFolder)
		{
			_modelsDirectory = nlpFolder + @"\models";
		}

		public string HandleRequest(string message)
		{
			var wordSet = BuildWordSet(message);
			var weatherProvider = new DarkSkyWeatherProvider(ConfigConstants.DarkSkyApiSecret);
			
			if (wordSet.Contains("wear", StringComparer.InvariantCultureIgnoreCase) &&
			    wordSet.Contains("I", StringComparer.InvariantCultureIgnoreCase))
			{
				var rawForecast = weatherProvider.GetForecastRaw(DateTime.Now);
				var icon = rawForecast.Daily.Data.FirstOrDefault()?.Icon;
				
				// Skip accounting for temperature for now, could have thresholds for certain apparel
				switch (icon)
				{
					case "clear-day":
					case "clear-night":
						return "Wear your comfortable clothing, no jacket required";
					case "snow":
						return "Wear winter jackets, bring boots!";
					case "wind":
						return "Bring a wind jacket, it\'s going to breezy";
					case "partly-cloudy-day":
					case "partly-cloudy-night":
					case "cloudy":
					case "fog":
						return "Bring a jacket";
					case "rain":
						return "Dress comfortably, bring an umbrella";

					default:
						return "I don't know, anything you want?";
				}
			}

			if (wordSet.Contains("weather", StringComparer.InvariantCultureIgnoreCase))
			{
				if (wordSet.Contains("now", StringComparer.InvariantCultureIgnoreCase)
					|| wordSet.Contains("today", StringComparer.InvariantCultureIgnoreCase))
				{
					return weatherProvider.GetForecast(DateTime.Now);
				}

				if (wordSet.Contains("tomorrow"))
				{
					return weatherProvider.GetForecast(DateTime.Now.AddDays(1));
				}

				return "Weather now or tomorrow?";
			}

			return string.Empty;
		}

		public HashSet<string> BuildWordSet(string message)
		{
			var tagger = new MaxentTagger(_modelsDirectory + @"\wsj-0-18-bidirectional-distsim.tagger");

			var sentences = MaxentTagger.tokenizeText(new java.io.StringReader(message)).toArray();
			var wordSet = new HashSet<string>();

			foreach (java.util.ArrayList sentence in sentences)
			{
				var taggedSentence = tagger.tagSentence(sentence);

				Console.WriteLine(SentenceUtils.listToString(taggedSentence, false));

				for (var i = 0; i < taggedSentence.size(); i++)
				{
					var taggedWord = taggedSentence.get(i) as TaggedWord;

					if (taggedWord == null)
						continue;
					
					// could use part of speech and sentence structure as well
					var value = taggedWord.value();

					if (wordSet.Contains(value))
						continue;

					wordSet.Add(value);
				}
			}

			return wordSet;
		}
	}
}
