using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoorsOpen.Models
{
	public class EventModel
	{
		public int Id { get; set; }

		[Display(Name = "Event Name")]
		public string Name { get; set; }
		[Display(Name = "Start Date")]
		public DateTime StartDate { get; set; }
		[Display(Name = "End Date")]
		public DateTime EndDate { get; set; }
		[Display(Name = "Currently Active?")]
		public bool IsActive { get; set; }
		public List<BuildingModel> Buildings { get; set; }
	}
}
