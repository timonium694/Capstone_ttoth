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
using Windows.Devices.Geolocation;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using Microsoft.Phone.Maps.Services;
using System.Text;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace TheMainEvent_Capstone
{
	public partial class MapPage : PhoneApplicationPage
	{
		private GeocodeQuery gquery;
		private MapLocation searchLocation;
		private string RouteDirections;
		private List<GeoCoordinate> TestCoordinates 
			= new List<GeoCoordinate>() { 
			new GeoCoordinate(37.79547, -122.393129),
			new GeoCoordinate(37.794911, -122.402871), 
			new GeoCoordinate(37.800911, -122.200871), 
			};
		public MapPage()
		{
			InitializeComponent();
			Loaded += MapPage_Loaded;
			//Use this to ask check consent was given to use the map control
			//if ((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"] != true)
			//{
			//	return;
			//}
		}
		void MapPage_Loaded(object sender, RoutedEventArgs e)
		{
			//this.FindMultipleLocations(this.TestCoordinates);
			//this.FindLocation("London");
			//Route between the Transamerica Pyramid and the ferry building in San Francisco
			//this.FindRoute(new GeoCoordinate(37.79547, -122.393129), new GeoCoordinate(37.794911, -122.402871));
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			string msg = "";
			if (NavigationContext.QueryString.TryGetValue("msg", out msg))
			{
				//MessageBox.Show(msg);
				this.FindLocation(msg);
			}
			if (IsolatedStorageSettings.ApplicationSettings.Contains("LocationConsent"))
			{
				// User has opted in or out of Location
				return;
			}
			else
			{
				MessageBoxResult result =
					MessageBox.Show("This app accesses your phone's location. Is that ok?",
					"Location",
					MessageBoxButton.OKCancel);

				if (result == MessageBoxResult.OK)
				{
					IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = true;
				}
				else
				{
					IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = false;
				}

				IsolatedStorageSettings.ApplicationSettings.Save();
			}
			
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			this.ClearMap();
		}
		public void ClearMap()
		{
			MainMap.Layers.Clear();
		}
		private void FindLocation(string searchTerm)
		{

			gquery = new GeocodeQuery()
			{
				SearchTerm = searchTerm,
				GeoCoordinate = new GeoCoordinate(0, 0)
			};
			gquery.QueryCompleted += this.LocationQuery_QueryCompleted;
			gquery.QueryAsync();
		}
		private void FindRoute(GeoCoordinate start, GeoCoordinate end, TravelMode mode = TravelMode.Driving)
		{
			RouteQuery query = new RouteQuery()
			{
				TravelMode = mode,
				Waypoints = new List<GeoCoordinate>()
				{
					start,
					end
				}
			};
			this.SetView(start);
			query.QueryCompleted += this.RouteQuery_QueryCompleted;
			query.QueryAsync();
		}
		private void FindMultipleLocations(IList<GeoCoordinate> coordinates)
		{
			foreach (GeoCoordinate coor in coordinates)
			{
				this.AddPinToMap(coor);
			}
			this.SetView(coordinates[0]);
		}
		private async void UpdateMap()
		{
			//If the user has not given consent for the app to use his/her location then the method ends.
			if ((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"] != true)
			{
				return;
			}
			Geolocator geolocator = new Geolocator();
			geolocator.DesiredAccuracyInMeters = 50;

			Geoposition position =
				await geolocator.GetGeopositionAsync(
				TimeSpan.FromMinutes(1),
				TimeSpan.FromSeconds(30));

			var gpsCoorCenter = new GeoCoordinate(
				position.Coordinate.Latitude,
				position.Coordinate.Longitude);

			MainMap.SetView(gpsCoorCenter, 17);
		}

		void RouteQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
		{
			try
			{
				MainMap.AddRoute(new MapRoute(e.Result));
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("Distance to destination: " + e.Result.LengthInMeters);
				sb.AppendLine("Time to destination: " + e.Result.EstimatedDuration);
				foreach (var maneuver in e.Result.Legs.SelectMany(l => l.Maneuvers))
				{
					sb.AppendLine("At " + maneuver.StartGeoCoordinate + " " +
											maneuver.InstructionKind + ": " +
											maneuver.InstructionText + " for " +
											maneuver.LengthInMeters + " meters");
				}
				this.RouteDirections = sb.ToString();
			}
			catch (Exception ex)
			{
				//Fixes Debugging error
				//Do Nothing
			}
		}
		void LocationQuery_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
		{
			try
			{
				this.searchLocation = e.Result[0];
				this.AddPinToMap(e.Result[0].GeoCoordinate);
				SetView(e.Result[0].GeoCoordinate);
			}
			catch (Exception ex)
			{
				//Fixes the debugger error
				//Do nothing
			}
		}

		private void SetView(GeoCoordinate coordinate)
		{
			MainMap.SetView(coordinate, 15);
		}
		private void AddPinToMap(GeoCoordinate coordinate)
		{
			MainMap.Layers.Add(new MapLayer()
			{
					new MapOverlay()
					{
						GeoCoordinate = coordinate,
						//Replace with a graphic later
						Content = new Ellipse
						{
							Fill = new SolidColorBrush(Colors.Red),
							Width = 20,
							Height = 20
						}
					}
			});
		}
	}
}