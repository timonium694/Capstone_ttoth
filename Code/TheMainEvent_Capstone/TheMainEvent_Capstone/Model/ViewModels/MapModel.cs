using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMainEvent_Capstone.Model.ViewModels
{
	public class MapModel
	{
		public GeoCoordinate Coordinate { get; set; }
		public string Address { get; set; }
		public string Name { get; set; }
	}
}
