using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorsOpen.Models
{
	public class EventModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public bool IsActive { get; set; }
		public List<BuildingModel> Buildings { get; set; }
	}
}
