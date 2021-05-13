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
		private readonly IConfiguration _config;

		public BuildingModel()
		{

		}
		public BuildingModel(IConfiguration configuration)
        {
            _config = configuration;
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

		public string FullAddress { get {
			return $"{Address1} {Address2} {City}, {State} {Zip}";
			}
		}

		public string ImageURL { get {
			if (Image == null){
				return null;
				}
			else {
				// return $"{_config.GetValue<string>("AzureImagePrefix")}{Image}";
				return "https://doorsopen.blob.core.windows.net/dev-images/" + Image;
				}
			}
		}
	}
}
