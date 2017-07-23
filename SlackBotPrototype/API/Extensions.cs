using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
{
	public static class Extensions
	{

		public static double DateTimeToUnixTimestamp(this DateTime dateTime)
		{
			return (TimeZoneInfo.ConvertTimeToUtc(dateTime) -
			        new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
		}

	}
}
