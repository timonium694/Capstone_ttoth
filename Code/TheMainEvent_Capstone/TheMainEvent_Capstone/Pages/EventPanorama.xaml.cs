using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Input;
using System.Collections.ObjectModel;
using TheMainEvent_Capstone.Model.ViewModels;
using TheMainEvent_Capstone.Model;
using TheMainEvent_Capstone.DataAccessLayer;
using Parse;
using System.Threading.Tasks;
using PayPal.Checkout;
using LinqToTwitter;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;
using Microsoft.Phone.Maps.Services;
using Microsoft.Phone.Tasks;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Maps.Toolkit;
using Windows.Foundation;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Threading;
using System.Text;

namespace TheMainEvent_Capstone.Pages
{
	public partial class EventPanorama : PhoneApplicationPage
	{

		private EventViewModel evm;
		private MapLocation searchLocation;
		private UserInfo owner;
		private UserInfo currentUser;
		private bool IsOwner = false;
		private bool IsAttending = false;
		ObservableCollection<ContactViewModel> InvitedUsers = new ObservableCollection<ContactViewModel>();
		ObservableCollection<UserInfo> InvitedInfo = new ObservableCollection<UserInfo>();
		ObservableCollection<UserInfo> AttendingInfo = new ObservableCollection<UserInfo>();
		ObservableCollection<ContactViewModel> AttendingUsers = new ObservableCollection<ContactViewModel>();
		List<GeoCoordinate> MyCoordinates = new List<GeoCoordinate>();
		GeoCoordinate eventLoc;
		GeoCoordinate myLoc;
		RouteQuery MyQuery = null;
		GeocodeQuery Mygeocodequery = null;



		//Have event editing by making everything clickable and then having a message box come up with a field for editing.
		public EventPanorama()
		{
			InitializeComponent();

		}

		private void ShowLoadingBar()
		{
			this.loadingBar.Visibility = Visibility.Visible;
			this.loadingBar.IsIndeterminate = true;
			this.rootPanorama.Visibility = Visibility.Collapsed;
		}
		private void ShowUI()
		{
			this.loadingBar.Visibility = Visibility.Collapsed;
			this.loadingBar.IsIndeterminate = false;
			this.rootPanorama.Visibility = Visibility.Visible;
		}
		#region LoadData
		private async Task LoadTweets()
		{
			try
			{
				tweetGrid.DataContext = new TweetViewModel();
				var twitterCtx = new TwitterContext(new SingleUserAuthorizer()
				{
					CredentialStore = new InMemoryCredentialStore()
					{
						ConsumerKey = "XQzE7qY7Myw27saKRPWW9w",
						ConsumerSecret = "dUz9DfDAI8FvfQXMsFqBctqGepip7wihpsqxrFKa8",
						OAuthToken = "1315500614-JpvNyuMTSISTOp5l9Y9mRF2PX4sv21Psh1kMv42",
						OAuthTokenSecret = "dhXrjuQZ4w9N2Aw1L6CwFGhZvBOtmJNkWop5zUXiVrntJ"
					}
				});
				string[] input = evm.Title.Split(' ');

				string hashtag = "#";
				foreach (string s in input)
				{
					hashtag += s;
				}
				this.tweetBox.Text = hashtag;
				var searchResponse =
					await
					(from search in twitterCtx.Search
					 where search.Type == SearchType.Search &&
						   search.Query == hashtag
					 select search)
					.SingleOrDefaultAsync();

				List<Tweet> tweets = new List<Tweet>();
				if (searchResponse != null && searchResponse.Statuses != null)
					searchResponse.Statuses.ForEach(tweet => tweets.Add(new Tweet() { ImageSource = tweet.User.ProfileImageUrl, Message = tweet.Text, UserName = tweet.User.ScreenNameResponse }));

				var tweetCollection = (tweetGrid.DataContext as TweetViewModel).Tweets;
				tweetCollection.Clear();
				tweets.ForEach(tweet => tweetCollection.Add(tweet));
			}
			catch (Exception e)
			{
			}
		}
		private async Task LoadEvent()
		{
			EventDAL ed = new EventDAL();

			rootPanorama.Title = evm.Title;
			this.EventData.DataContext = evm;
			if (evm.Cost.Equals(0.00))
				costBlock.Text = "FREE";
			else
				costBlock.Text = "$" + evm.Cost;

			descriptionBlock.Text = evm.Description;
			detailsBlock.Text = evm.OtherDetails;
			dateBlock.Text = evm.Date.ToLocalTime().ToShortDateString();
			timeBlock.Text = evm.Time.ToLocalTime().ToShortTimeString();
			UserDAL ud = new UserDAL();
			string currentId = ParseUser.CurrentUser.ObjectId;
			currentUser = await ud.GetUserInfo(currentId);
			string ownerId = await ed.GetOwner(evm.ID);
			owner = await ud.GetUserInfo(ownerId);
			List<string> users = await ed.GetAttendees(evm.ID);
			List<string> invitees = await ed.GetInvitees(evm.ID);
			if (currentUser.Equals(owner))
			{
				IsOwner = true;
			}
			foreach (string id in users)
			{
				UserInfo s = await ud.GetUserInfo(id);
				if (!s.Equals(currentUser))
				{
					AttendingInfo.Add(s);
				}
				if (currentUser.Equals(s))
				{
					IsAttending = true;
				}
			}
			this.AttendingList.ItemsSource = AttendingInfo;
			foreach (string id in invitees)
			{
				UserInfo ui = await ud.GetUserInfo(id);
				this.InvitedInfo.Add(ui);
			}
			this.InvitedList.ItemsSource = InvitedInfo;
			this.SetAppBar();
		}
		private void SetAppBar()
		{
			if (this.IsOwner)
			{
				ApplicationBar = ((ApplicationBar)this.Resources["IsOwnerAppBar"]);
			}
			else if (this.IsAttending)
			{
				ApplicationBar = ((ApplicationBar)this.Resources["IsAttendingAppBar"]);
			}
			else ApplicationBar = ((ApplicationBar)this.Resources["DefaultAppBar"]);


		}
		#endregion
		private async Task LoadData()
		{
			this.ShowLoadingBar();
			try
			{
				await this.LoadEvent();
				this.LoadMap();
				await this.LoadTweets();
				Map.Center = this.eventLoc;
				UserLocationMarker marker = (UserLocationMarker)this.FindName("UserLocationMarker");
				marker.GeoCoordinate = this.eventLoc;

				Pushpin pushpin = (Pushpin)this.FindName("MyPushpin");
				pushpin.GeoCoordinate = this.myLoc;
				Map.SetView(this.eventLoc, 13);

				if (evm.Type.Equals("Meeting"))
				{
					TweetSection.Visibility = Visibility.Collapsed;
				}
				else if (SharedState.Authorizer == null)
				{
					this.tweetPanel.Visibility = Visibility.Collapsed;
					this.authorizeTweets.Visibility = Visibility.Visible;
				}
				else if (SharedState.Authorizer != null)
				{
					this.tweetPanel.Visibility = Visibility.Visible;
					this.authorizeTweets.Visibility = Visibility.Collapsed;
				}
				this.ShowUI();
			}
			catch (Exception e)
			{
			}
		}

		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{

			//if (IsolatedStorageSettings.ApplicationSettings.Contains("LocationConsent"))
			//{
			//	// User has opted in or out of Location
			//	return;
			//}
			//else
			//{
			//	MessageBoxResult result =
			//		MessageBox.Show("This app accesses your phone's location. Is that ok?",
			//		"Location",
			//		MessageBoxButton.OKCancel);

			//	if (result == MessageBoxResult.OK)
			//	{
			//		IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = true;
			//	}
			//	else
			//	{
			//		IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = false;
			//	}

			//	IsolatedStorageSettings.ApplicationSettings.Save();
			//}
			string msg = "";
			if (NavigationContext.QueryString.TryGetValue("msg", out msg))
			{
				EventDAL ed = new EventDAL();
				Event ev = await ed.RetrieveEvent(msg);
				this.evm = new EventViewModel()
				{
					Title = ev.Title,
					Cost = ev.Cost,
					Address = ev.Address + ", " + ev.City + ", " + ev.State,
					Description = ev.Description,
					OtherDetails = "Details: " + ev.OtherDetails,
					Date = ev.Date,
					Type = ev.Type,
					ID = ev.ID,

				};
				await this.LoadData();
			}
		}

		private async Task PayForEvent(string merchant)
		{
			try
			{
				// Create a BuyNow object with your PayPal 
				// merchant account emailid. Remember to onboard
				// your merchant account by granting third party 
				// permissions as detailed in the docs	
				BuyNow bn = new BuyNow(merchant);
				bn.UseSandbox = true;

				ItemBuilder builder = new ItemBuilder(evm.Title).Description(evm.Description).Price(evm.Cost.ToString());
				bn.AddItem(builder);

				// Attach event handlers. The BuyNow class emits 5 events
				// start, auth, error, cancel and complete
				bn.Start += new EventHandler<PayPal.Checkout.Event.StartEventArgs>((source, args) =>
				{
					statusBlock.Text = "Initializing Payment";
				});
				bn.Auth += new EventHandler<PayPal.Checkout.Event.AuthEventArgs>((source, args) =>
				{
					statusBlock.Text = "Authorizing Payment";
				});
				bn.Complete += new EventHandler<PayPal.Checkout.Event.CompleteEventArgs>(async (source, args) =>
				{
					statusBlock.Text = "Your payment is complete. Transaction id: " + args.TransactionID;
					EventDAL ed = new EventDAL();
					NotificationDAL nd = new NotificationDAL();
					Notification n = new Notification();
					n.User = owner.User;
					n.Date = DateTime.Now;
					n.Message = this.currentUser.FirstName + " " + currentUser.LastName + " is attending " + this.evm.Title;
					await nd.CreateNotification(n);
					await ed.AddAttendee(evm.ID, currentUser.User);
				});
				bn.Cancel += new EventHandler<PayPal.Checkout.Event.CancelEventArgs>((source, args) =>
				{
					statusBlock.Text = "Payment Cancelled";
				});
				bn.Error += new EventHandler<PayPal.Checkout.Event.ErrorEventArgs>((source, args) =>
				{
					statusBlock.Text = "There was an error processing your payment: " + args.Type + " " + args.Message;
				});

				// Ready to go. Call the asynchronous Execute operation
				// to initiate the payment. Remember to mark your calling
				// function with the async keyword since this is an async call
				bool ret = await bn.Execute();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void tweetBox_GotFocus(object sender, RoutedEventArgs e)
		{
			this.tweetGotFocusStoryboard.Begin();
		}
		private void tweetBox_LostFocus(object sender, RoutedEventArgs e)
		{
			this.tweetLostFocusStoryboard.Begin();
		}

		private async void unattendButton_Click(object sender, EventArgs e)
		{
			EventDAL ed = new EventDAL();
			NotificationDAL nd = new NotificationDAL();
			Notification n = new Notification();
			n.User = owner.User;
			n.Date = DateTime.Now;
			n.Message = this.currentUser.FirstName + " " + currentUser.LastName + " is NOT attending " + this.evm.Title;
			await nd.CreateNotification(n);
			await ed.UnattendEvent(this.evm.ID, this.currentUser.User);
		}
		private async void cancelEventButton_Click(object sender, EventArgs e)
		{
			EventDAL ed = new EventDAL();
			NotificationDAL nd = new NotificationDAL();
			foreach(UserInfo ui in this.InvitedInfo)
			{
				Notification n = new Notification();
				n.User = ui.User;
				n.Date = DateTime.Now;
				n.Message = this.evm.Title + " has been cancelled";
				await nd.CreateNotification(n);
			}
			await ed.DeleteEvent(this.evm.ID);
		}

		private void eventsNav_Click(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/MainPages.xaml?msg=" + "events", UriKind.Relative));
		}

		private void contactsNav_Click(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/MainPages.xaml?msg=" + "contacts", UriKind.Relative));
		}

		private void logutNav_Click(object sender, EventArgs e)
		{
			ParseUser.LogOut();
			NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
		}

		private void searchNav_Click(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/SearchPage.xaml", UriKind.Relative));
		}

		private async void tweetButton_Click(object sender, RoutedEventArgs e)
		{

			if (SharedState.Authorizer != null)
			{
				IAuthorizer auth = SharedState.Authorizer;

				var twitterCtx = new TwitterContext(auth);

				Status tweet = await twitterCtx.TweetAsync(this.tweetBox.Text);

			}
			else
			{
				MessageBox.Show("You are not authorized to send tweets.");
			}


		}

		private void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			Thread.Sleep(5000);
		}

		private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			tweetsLoading.IsIndeterminate = true;
			tweetsLoading.Visibility = Visibility.Visible;
			this.BackGroundTweets();

			tweetsLoading.IsIndeterminate = false;
			tweetsLoading.Visibility = Visibility.Collapsed;
		}
		private async void BackGroundTweets()
		{
			await this.LoadTweets();
		}

		private async void attendButton_Click(object sender, EventArgs e)
		{
			//Comment for Testing 
			if (owner.MerchantEmail.Equals("none"))
			{
				EventDAL ed = new EventDAL();
				await ed.AddAttendee(evm.ID, currentUser.User);
			}
			else if (!owner.MerchantEmail.Equals("none") && evm.Cost > 0.00)
			{
				await this.PayForEvent(owner.MerchantEmail);
			}
			else if (!owner.MerchantEmail.Equals("none") && evm.IsDonatable)
			{
				this.PayDonation();
			}
		}

		private void PayDonation()
		{
			PhoneTextBox costBox = new PhoneTextBox();
			costBox.Height = 72;
			costBox.Width = 100;
			costBox.Hint = "Amount";
			costBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;


			StackPanel container = new StackPanel();
			container.Children.Add(costBox);
			EventDAL ed = new EventDAL();


			CustomMessageBox message = new CustomMessageBox()
			{
				Title = "Donation",
				Content = container,
				Message = "Would you like to make a donation",
				RightButtonContent = "Yes, make Donation",
				LeftButtonContent = "No, just attend"
			};
			message.Dismissed += async (s1, e1) =>
			{
				switch (e1.Result)
				{
					case CustomMessageBoxResult.RightButton:
						evm.Cost = double.Parse(costBox.Text);
						await this.PayForEvent(owner.MerchantEmail);
						break;
					case CustomMessageBoxResult.LeftButton:
						await ed.AddAttendee(this.evm.ID, this.currentUser.User);
						message.Dismiss();
						break;
					case CustomMessageBoxResult.None:
						await ed.AddAttendee(this.evm.ID, this.currentUser.User);
						break;
					default:
						break;
				}
			};
			message.Show();
		}
		private void LoadMap()
		{

			this.Maps_GeoCoding(null, null);
			this.OneShotLocation_Click(null, null);
			//Map.Center = this.eventLoc;
			//UserLocationMarker marker = (UserLocationMarker)this.FindName("UserLocationMarker");
			//marker.GeoCoordinate =	this.eventLoc;

			//Pushpin pushpin = (Pushpin)this.FindName("MyPushpin");
			//MyPushpin.GeoCoordinate = this.myLoc;
			//this.GetCoordinates();
			//DrawMapMarker(eventLoc, Colors.Black, layer);
			//DrawMapMarker(current, Colors.Black, layer);
			//Map.Layers.Add(layer);
		}


		#region ObsoleteGeoQuery
		private async void GetCoordinates()
		{
			// Get the phone's current location.
			Geolocator MyGeolocator = new Geolocator();
			MyGeolocator.DesiredAccuracyInMeters = 5;
			Geoposition MyGeoPosition = null;
			try
			{
				MyGeoPosition = await MyGeolocator.GetGeopositionAsync(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(10));
				MyCoordinates.Add(new GeoCoordinate(MyGeoPosition.Coordinate.Latitude, MyGeoPosition.Coordinate.Longitude));
				Map.SetView(MyCoordinates[0], 13);
			}
			catch (UnauthorizedAccessException)
			{
				MessageBox.Show("Location is disabled in phone settings or capabilities are not checked.");
			}
			catch (Exception ex)
			{
				// Something else happened while acquiring the location.
				MessageBox.Show(ex.Message);
			}
			Mygeocodequery = new GeocodeQuery();
			Mygeocodequery.SearchTerm = evm.Address;
			Mygeocodequery.GeoCoordinate = new GeoCoordinate(MyGeoPosition.Coordinate.Latitude, MyGeoPosition.Coordinate.Longitude);
			Mygeocodequery.QueryCompleted += this.GeoCoordinateQuery_QueryCompleted;
			Mygeocodequery.QueryAsync();
		}

		private void GetGeoCoordinate(string address)
		{
			GeocodeQuery query = new GeocodeQuery()
			{
				GeoCoordinate = new GeoCoordinate(0, 0),
				SearchTerm = address
			};
			query.QueryCompleted += GeoCoordinateQuery_QueryCompleted;
			query.QueryAsync();
		}
		void GeoCoordinateQuery_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
		{
			if (e.Error == null && e.Result.Count > 0)
			{
				MyQuery = new RouteQuery();
				MyCoordinates.Add(e.Result[0].GeoCoordinate);
				MyQuery.Waypoints = MyCoordinates;
				MyQuery.QueryCompleted += MyQuery_QueryCompleted;
				MyQuery.QueryAsync();
				Mygeocodequery.Dispose();
			}
		}
		void MyQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
		{
			if (e.Error == null)
			{
				Route MyRoute = e.Result;
				MapRoute MyMapRoute = new MapRoute(MyRoute);
				Map.AddRoute(MyMapRoute);
				List<string> RouteList = new List<string>();
				foreach (RouteLeg leg in MyRoute.Legs)
				{
					foreach (RouteManeuver maneuver in leg.Maneuvers)
					{
						RouteList.Add(maneuver.InstructionText);
					}
				}
				MyQuery.Dispose();

			}
		}
		private void DrawMapMarker(GeoCoordinate coordinate, Color color, MapLayer mapLayer)
		{
			// Create a map marker
			Polygon polygon = new Polygon();
			polygon.Points.Add(new System.Windows.Point(0, 0));
			polygon.Points.Add(new System.Windows.Point(0, 75));
			polygon.Points.Add(new System.Windows.Point(25, 0));
			polygon.Fill = new SolidColorBrush(color);

			// Enable marker to be tapped for location information
			polygon.Tag = new GeoCoordinate(coordinate.Latitude, coordinate.Longitude);

			// Create a MapOverlay and add marker
			MapOverlay overlay = new MapOverlay();
			overlay.Content = polygon;
			overlay.GeoCoordinate = new GeoCoordinate(coordinate.Latitude, coordinate.Longitude);
			overlay.PositionOrigin = new System.Windows.Point(0.0, 1.0);
			mapLayer.Add(overlay);
		}
		#endregion

		private void showRoute_Click(object sender, EventArgs e)
		{

		}

		private void authorizeTweets_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/OAuth.xaml", UriKind.Relative));
		}

		private void inviteContacts_Click(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/SelectInviteesPage.xaml?msg=" + this.evm.ID, UriKind.Relative));
		}

		private void kickAttendants_Click(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/KickAttendants.xaml?msg=" + this.evm.ID, UriKind.Relative));
		}

		private void Maps_GeoCoding(object sender, RoutedEventArgs e)
		{
			GeocodeQuery query = new GeocodeQuery()
			{
				GeoCoordinate = new GeoCoordinate(0, 0),
				SearchTerm = this.evm.Address,
			};
			query.QueryCompleted += query_QueryCompleted;
			query.QueryAsync();
		}

		void query_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
		{
			StringBuilder sb = new StringBuilder();
			var item = e.Result[0];
			this.eventLoc = item.GeoCoordinate;
		}

		private async void OneShotLocation_Click(object sender, RoutedEventArgs e)
		{
			//Check for the user agreement in use his position. If not, method returns.
			
			Geolocator geolocator = new Geolocator();
			geolocator.DesiredAccuracyInMeters = 50;

			try
			{
				Geoposition geoposition = await geolocator.GetGeopositionAsync(
					 maximumAge: TimeSpan.FromMinutes(5),
					 timeout: TimeSpan.FromSeconds(10)
					);
				this.myLoc = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
				//With this 2 lines of code, the app is able to write on a Text Label the Latitude and the Longitude, given by {{Icode|geoposition}}
				string lat = geoposition.Coordinate.Latitude.ToString("0.00");
				string lon = geoposition.Coordinate.Longitude.ToString("0.00");
			}
			//If an error is catch 2 are the main causes: the first is that you forgot to include ID_CAP_LOCATION in your app manifest. 
			//The second is that the user doesn't turned on the Location Services
			catch (Exception ex)
			{
				
			}
		}

		private void AttendingList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			UserInfo ui = (UserInfo)((LongListSelector)sender).SelectedItem;
			NavigationService.Navigate(new Uri("/Pages/Contact.xaml?msg=" + ui.User, UriKind.Relative));
		}



		//private void MapFlightToSeattle()
		//{
		//	LocationRectangle locationRectangle;

		//	locationRectangle = LocationRectangle.CreateBoundingRectangle(from store in this.mainViewModel.StoreList select store.GeoCoordinate);

		//	this.Map.SetView(locationRectangle, new Thickness(20, 20, 20, 20));
		//}



		//private IAsyncOperation<Geoposition> GetUserLocation()
		//{
		//	Geolocator geolocator;

		//	geolocator = new Geolocator();

		//	return geolocator.GetGeopositionAsync();
		//}







	}
}