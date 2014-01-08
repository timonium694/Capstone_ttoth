using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Device.Location;

namespace TheMainEvent_Capstone
{
	public partial class MapPage : PhoneApplicationPage
	{
		public MapPage()
		{
			InitializeComponent();
			Loaded += MapPage_Loaded;
		}
		void MapPage_Loaded(object sender, RoutedEventArgs e)
		{
			UpdateMap();
		}

		private void UpdateMap()
		{
			MainMap.SetView(new GeoCoordinate(41.8988D, -87.6231D), 17D);
		}

	}
}