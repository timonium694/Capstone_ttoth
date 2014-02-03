using Microsoft.Phone.Maps.Services;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Windows.Devices.Geolocation;

namespace TheMainEvent_Capstone
{
	public class GeocodeQueryHandler
	{
		public GeocodeQuery gquery { get; set; }
		public GeoCoordinate currentLoc { get; set; }
		private double accuracy = 0.0;

		public void ReturnCoordinates(string address)
		{
			gquery = new GeocodeQuery()
		   {
			   SearchTerm = address,
			   GeoCoordinate = new GeoCoordinate(0, 0)
		   };
		}

	}
}
