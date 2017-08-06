using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace API.Tests
{

	[TestFixture]
	public class LocationServiceTests
	{
		// SHORTCUT: We wouldn't normally be able to test a
		// location service like this since it'd probably involve a service call. We'd need to mock
		// it in some way.
		private LocationService _locationService;

		[OneTimeSetUp]
		public void Init()
		{
			// Not much of a point to mock up (test) DB retrieval since there's really no logic in repository
			// at this moment
			var repository = new Mock<IRepository>();
			_locationService = new LocationService(repository.Object);
		}
		
		[TestCase("Invalid")]
		[TestCase("")]
		[TestCase(null)]
		public void TestInvalidLocation(string name)
		{
			var location = new Location()
			{
				DisplayName = name
			};

			Assert.IsTrue(location.IsInvalid(), "location.IsInvalid() != true");
		}

		// Since we don't have an integrated test setup with SQLite
		// there's not really a point to testing retrieval from a mock object as there's not
		// much logic in the retrieval code. Just verify that we handle nulls correctly.
		[TestCase(100, ExpectedResult = null)]
		[TestCase(-1, ExpectedResult = null)]
		public Location TestGetLocationById(int id)
		{
			return _locationService.GetLocation(id);
		}
	}
}
