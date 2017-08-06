using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API;
using Newtonsoft.Json;
using SQLite;

namespace DBPreparer
{
	internal static class Program
	{
		static void Main(string[] args)
		{
			using (var db = new SQLiteConnection(ConfigConstants.DbName))
			{
				// Prep DB schema initial state
				db.CreateTable<User>();
				db.CreateTable<Location>();

				// SHORTCUT: Normally the source of the
				// data would not be hard coded this way but rather come from a proper geocoding service, and we would store the results
				// for performance. We would probably also prime the lookup table with the commmon locations that we anticipate so we 
				// don't have a cold start problem with locations.
				var seedData = File.ReadAllText("locations-seed.json");
				var locations = JsonConvert.DeserializeObject<Location[]>(seedData);
				
				// Seed the database
				db.InsertAll(locations);

				// Migration as needed as the application grows
			}
		}
	}
}
