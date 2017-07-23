using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace API.Tests
{

	[TestFixture]
	public class MessageParserTests
	{
		private MessageParser _nlpParser;

		[OneTimeSetUp]
		public void Init()
		{
			_nlpParser = new MessageParser(ConfigConstants.StanfordNlpFolder);
		}

		[TestCase("weather now", 2)]
		[TestCase("weather tomorrow", 2)]
		[TestCase("", 0)]
		public void TestTokenizer(string message, int count)
		{
			var wordSet = _nlpParser.BuildWordSet(message);

			Assert.IsTrue(wordSet.Count == count);
		}

		[TestCase("weather now")]
		[TestCase("weather tomorrow")]
		[TestCase("weather today")]
		[TestCase("WEATHER NOW!")]
		[TestCase("WEATHER TOMORROW")]
		[TestCase("What should I wear?")]
		public void CommandIntegrationTest(string message)
		{
			var forecast = _nlpParser.HandleRequest(message);
			Console.WriteLine(forecast);
			Assert.IsNotEmpty(forecast);
		}
	}
}
