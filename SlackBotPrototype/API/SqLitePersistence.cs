using System;
using System.Linq;
using SQLite;

namespace API
{
	// SHORTCUT: Normally we'd break out the repositories into
	// a file per entity. Keep them together for simplicity here.
	// 
	// Also skipping tests for these because we'd need to set up integration tests
	// with a SQLite database. If we weren't using a embedded file DB like SQLite, testing
	// against a DB could get complicated.
	public class SqLitePersistence : IRepository
	{

		public User Get(string slackId)
		{
			if (string.IsNullOrWhiteSpace(slackId))
			{
				return null;
			}

			using (var db = new SQLiteConnection(ConfigConstants.DbName))
			{
				return db.Query<User>($"SELECT * FROM User WHERE SlackId = '{slackId}'").FirstOrDefault();
			}
		}

		// SHORTCUT: We skip testing this because it's hard to test this in isolation, but it contains
		// logic for matching up cities which is critical
		public Location GetLocation(string city, string state)
		{
			if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(state))
			{
				return null;
			}

			using (var db = new SQLiteConnection(ConfigConstants.DbName))
			{
				return db.Query<Location>($"SELECT * FROM Location WHERE UPPER(City) = '{city.ToUpper()}' AND UPPER(StateAbbreviation) = '{state.ToUpper()}'").FirstOrDefault();
			}
		}

		public Location GetLocation(int id)
		{
			if (id <= 0)
			{
				return null;
			}

			try
			{
				using (var db = new SQLiteConnection(ConfigConstants.DbName))
				{
					return db.Get<Location>(id);
				}
			}
			catch (Exception ex)
			{
				// SHORTCUT: Would normally log exception w/ logging library or service of choice
				Console.WriteLine(ex.StackTrace);
			}

			return null;
		}

		public int CreateOrUpdate(string name, string slackId, Location location = null)
		{
			using (var db = new SQLiteConnection(ConfigConstants.DbName))
			{
				// SHORTCUT: This should be wrapped in a transaction. Not doing so
				// because I'm unfamiliar with this particular SQLite ORM
				var id = db.InsertOrReplace(new User
				{
					Name = name,
					SlackId = slackId,
					LocationId = location?.Id
				});

				return id;
			}
		}
	}
}
