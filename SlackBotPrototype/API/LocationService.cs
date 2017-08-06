using System.Linq;

namespace API
{
	public class LocationService : ILocationService
	{
		private readonly IRepository _repository;

		public LocationService(IRepository repository)
		{
			_repository = repository;
		}
		
		public Location GetLocation(string city, string stateAbbreviation)
		{
			return _repository.GetLocation(city, stateAbbreviation);
		}

		public Location GetLocation(int id)
		{
			return _repository.GetLocation(id);
		}
	}
}
