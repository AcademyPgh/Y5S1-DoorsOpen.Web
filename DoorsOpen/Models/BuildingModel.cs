using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Configuration;

namespace DoorsOpen.Models
{
	public class BuildingModel
	{
		public int Id { get; set; }
		public string Building { get; set; }
		[Display(Name = "Address")]
		public string Address1 { get; set; }
		[Display(Name = "Address 2")]
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		[Display(Name = "Wheelchair Accessible")]
		public bool WheelchairAccessible { get; set; }
		[Display(Name = "Restrooms Available")]
		public bool RestroomsAvailable { get; set; }
		[Display(Name = "Wheelchair Accessible Restroom")]
		public bool WheelchairAccessibleRestroom { get; set; }
		[Display(Name = "Photography Allowed")]
		public bool PhotographyAllowed { get; set; }
		[Display(Name = "Start Time")]
		public string StartTime { get; set; }
		[Display(Name = "End Time")]
		public string EndTime { get; set; }
		public int Capacity { get; set; }
		[Display(Name = "Historical Overview")]
		public string HistoricalOverview { get; set; }
		[Display(Name = "Visitor Experience")]
		public string VisitorExperience { get; set; }

		public string Image { get; set; }

		public string FullAddress { get
			{
				if (Address2 == null)
                {
					return $"{Address1} {City}, {State} {Zip}";
				}
				return $"{Address1} {Address2} {City}, {State} {Zip}";
			}
		}
	}

	public class BuildingViewModel
	{
	
		public BuildingViewModel(BuildingModel original, string baseURL)
		{
			Id = original.Id;
			Building = original.Building;
			Address1 = original.Address1;
			Address2 = original.Address2;
			City = original.City;
			State = original.State;
			Zip = original.Zip;
			WheelchairAccessible = original.WheelchairAccessible;
			RestroomsAvailable = original.RestroomsAvailable;
			WheelchairAccessibleRestroom = original.WheelchairAccessibleRestroom;
			PhotographyAllowed = original.PhotographyAllowed;
			StartTime = original.StartTime;
			EndTime = original.EndTime;
			Capacity = original.Capacity;
			HistoricalOverview = original.HistoricalOverview;
			VisitorExperience = original.VisitorExperience;
			Image = original.Image;
			ImageURL = baseURL + Image;
		}
		
		public int Id { get; set; }
		public string Building { get; set; }
		[Display(Name = "Address")]
		public string Address1 { get; set; }
		[Display(Name = "Address 2")]
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		[Display(Name = "Wheelchair Accessible")]
		public bool WheelchairAccessible { get; set; }
		[Display(Name = "Restrooms Available")]
		public bool RestroomsAvailable { get; set; }
		[Display(Name = "Wheelchair Accessible Restroom")]
		public bool WheelchairAccessibleRestroom { get; set; }
		[Display(Name = "Photography Allowed")]
		public bool PhotographyAllowed { get; set; }
		[Display(Name = "Start Time")]
		public string StartTime { get; set; }
		[Display(Name = "End Time")]
		public string EndTime { get; set; }
		public int Capacity { get; set; }
		[Display(Name = "Historical Overview")]
		public string HistoricalOverview { get; set; }
		[Display(Name = "Visitor Experience")]
		public string VisitorExperience { get; set; }

		public string Image { get; set; }

		public string FullAddress { get
			{
				if (Address2 == null)
                {
					return $"{Address1} {City}, {State} {Zip}";
				}
				return $"{Address1} {Address2} {City}, {State} {Zip}";
			}
		}

		public string ImageURL { get; set; } 
		
	}
}
