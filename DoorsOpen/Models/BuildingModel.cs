using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorsOpen.Models
{
	public class BuildingModel
	{
		public int Id { get; set; }
		public string Building { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public bool WheelchairAccessible { get; set; }
		public bool RestroomsAvailable { get; set; }
		public bool WheelchairAccessibleRestroom { get; set; }
		public bool PhotographyAllowed { get; set; }
		public string StartTime { get; set; }
		public string EndTime { get; set; }
		public int Capacity { get; set; }
		public string HistoricalOverview { get; set; }
		public string VisitorExperience { get; set; }
	}
}
