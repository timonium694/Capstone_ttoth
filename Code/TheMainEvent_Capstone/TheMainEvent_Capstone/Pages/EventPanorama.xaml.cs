﻿using System;
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

namespace TheMainEvent_Capstone.Pages
{
	public partial class EventPanorama : PhoneApplicationPage
	{

		private EventViewModel evm;
		private UserInfo owner;
		private UserInfo currentUser;
		private bool IsOwner = false;
		private bool IsAttending = false;
		ObservableCollection<ContactViewModel> InvitedUsers = new ObservableCollection<ContactViewModel>();
		ObservableCollection<ContactViewModel> AttendingUsers = new ObservableCollection<ContactViewModel>();


		//Have event editing by making everything clickable and then having a message box come up with a field for editing.
		public EventPanorama()
		{
			InitializeComponent();

		}

		private async Task LoadTweets()
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
			string hashtag = "#" + evm.Title;
			var searchResponse =
				await
				(from search in twitterCtx.Search
				 where search.Type == SearchType.Search &&
					   search.Query == "\"LINQ to Twitter\""
				 select search)
				.SingleOrDefaultAsync();

			List<Tweet> tweets = new List<Tweet>();
			if (searchResponse != null && searchResponse.Statuses != null)
				searchResponse.Statuses.ForEach(tweet => tweets.Add(new Tweet() { ImageSource = tweet.User.ProfileImageUrl, Message = tweet.Text, UserName = tweet.User.ScreenNameResponse }));

			var tweetCollection = (tweetGrid.DataContext as TweetViewModel).Tweets;
			tweetCollection.Clear();
			tweets.ForEach(tweet => tweetCollection.Add(tweet));
		}
		private async Task LoadEvent()
		{
			EventDAL ed = new EventDAL();
			rootPanorama.Title = evm.Title;
			addressBlock.Text = evm.Address;
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
			if (currentUser.Equals(owner))
			{
				IsOwner = true;
			}
			foreach (string id in users)
			{
				UserInfo s = await ud.GetUserInfo(id);
				AttendingUsers.Add(new ContactViewModel()
				{
					Name = s.FirstName + " " + s.LastName,
					User = s.User,
				});
				if (currentUser.Equals(s))
				{
					IsAttending = true;
				}
			}
			evm.Cost = 30;
		}
		private void UserInAttendance()
		{

			if (this.IsAttending)
			{
				ApplicationBar.MenuItems.Remove(this.attendEvent);
				ApplicationBarMenuItem menuItem1 = new ApplicationBarMenuItem();
				menuItem1.Text = "unattend event";
				ApplicationBar.MenuItems.Add(menuItem1);
				menuItem1.Click += new EventHandler(this.unattendButton_Click);
			}
			if (this.IsOwner)
			{
				ApplicationBar.MenuItems.Remove(this.attendEvent);
				ApplicationBarMenuItem menuItem1 = new ApplicationBarMenuItem();
				menuItem1.Text = "cancel event";
				ApplicationBar.MenuItems.Add(menuItem1);
				menuItem1.Click += new EventHandler(this.cancelEventButton_Click);

				ApplicationBarMenuItem menuItem2 = new ApplicationBarMenuItem();
				menuItem2.Text = "cancel event";
				ApplicationBar.MenuItems.Add(menuItem2);
				menuItem2.Click += new EventHandler(this.cancelEventButton_Click);
			}

		}

		private async Task LoadData()
		{
			await this.LoadEvent();
			await this.LoadTweets();
		}
		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
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
					Type = ev.Type.ToString(),
					ID = ev.ID,

				};
				await this.LoadData();
			}
		}
		private async void attendButton_Click(object sender, RoutedEventArgs e)
		{

			//Comment for Testing 
			if (owner.MerchantEmail.Equals("none"))
			{
				EventDAL ed = new EventDAL();
				ed.AddAttendee(evm.ID, currentUser.User);
			}
			else if (!owner.MerchantEmail.Equals("none"))
			{
				await this.PayForEvent(owner.MerchantEmail);
				EventDAL ed = new EventDAL();
				ed.AddAttendee(evm.ID, currentUser.User);
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
					MessageBox.Show("Initiating payment");
				});
				bn.Auth += new EventHandler<PayPal.Checkout.Event.AuthEventArgs>((source, args) =>
				{
					MessageBox.Show("Authenticating payment: " + args.Token);
				});
				bn.Complete += new EventHandler<PayPal.Checkout.Event.CompleteEventArgs>((source, args) =>
				{
					MessageBox.Show("Your payment is complete. Transaction id: " + args.TransactionID);
				});
				bn.Cancel += new EventHandler<PayPal.Checkout.Event.CancelEventArgs>((source, args) =>
				{
					MessageBox.Show("Payment Cancelled");
				});
				bn.Error += new EventHandler<PayPal.Checkout.Event.ErrorEventArgs>((source, args) =>
				{
					MessageBox.Show("There was an error processing your payment: " + args.Type + " " + args.Message);
				});

				// Ready to go. Call the asynchronous Execute operation
				// to initiate the payment. Remember to mark your calling
				// function with the async keyword since this is an async call
				bool ret = await bn.Execute();
				NavigationService.Navigate(new Uri("/Pages/EventPage.xaml?msg=" + evm.ID, UriKind.Relative));
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

		private void getDirectionsClick_Click(object sender, EventArgs e)
		{

		}

		private void unattendButton_Click(object sender, EventArgs e)
		{

		}
		private void cancelEventButton_Click(object sender, EventArgs e) 
		{

		}

		private void eventsNav_Click(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/EventPage.xaml?msg=" + "1", UriKind.Relative));
		}

		private void contactsNav_Click(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/EventPage.xaml?msg=" + "3", UriKind.Relative));
		}

		private void searchNav_Click(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/MainPages.xaml", UriKind.Relative));
		}

		private void logutNav_Click(object sender, EventArgs e)
		{
			ParseUser.LogOut();
			NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
		}

		private async void tweetButton_Click(object sender, RoutedEventArgs e)
		{
			if (SharedState.Authorizer == null)
				NavigationService.Navigate(new Uri("/OAuth.xaml", UriKind.Relative));

			IAuthorizer auth = SharedState.Authorizer;

			var twitterCtx = new TwitterContext(auth);

			decimal latitude = 37.78215m;
			decimal longitude = -122.40060m;

			Status tweet = await twitterCtx.TweetAsync(this.tweetBox.Text, latitude, longitude);

			MessageBox.Show(
				"User: " + tweet.User.ScreenNameResponse +
				", Posted Status: " + tweet.Text,
				"Update Successfully Posted.",
				MessageBoxButton.OK);

		}
	}
}