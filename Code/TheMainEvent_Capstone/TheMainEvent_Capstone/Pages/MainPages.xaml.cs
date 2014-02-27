using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using TheMainEvent_Capstone.Model.ViewModels;
using TheMainEvent_Capstone.DataAccessLayer;
using TheMainEvent_Capstone.Model;
using Parse;
using System.Threading.Tasks;

namespace TheMainEvent_Capstone.Pages
{
	public partial class MainPages : PhoneApplicationPage
	{
		ObservableCollection<EventViewModel> Events = new ObservableCollection<EventViewModel>();
		ObservableCollection<InviteViewModel> Invites = new ObservableCollection<InviteViewModel>();
		List<UserInfo> Contacts = new List<UserInfo>();
		private bool isLoaded = false;

		public MainPages()
		{
			InitializeComponent();
		}
		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			await this.LoadEvents();
			await this.LoadInvites();
			await this.LoadUser();
			await this.LoadContacts();
			this.isLoaded = true;

			this.loadingBar.Visibility = Visibility.Collapsed;
			this.loadingBar.IsIndeterminate = false;
			this.MainPivot.Visibility = Visibility.Visible;
			//EventDAL ed = new EventDAL();
			//Event ev = new Event()
			//{
			//	Address = "Here",
			//	Date = DateTime.Now,
			//	Description = "Test Event",
			//	Title = "Test Event 2",
			//	OtherDetails = "This is a test event",
			//	City = "Salt Lake City",
			//	State = "Utah",
			//	Type = "Concert",
			//	Cost = 30.00
			//};
			//ed.CreateEvent(ev);
			//this.LoadEvents();
			//Load all the data:
			//Contacts, Events, Invites, 
		}
		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			this.Events.Clear();
		}
		private async Task LoadEvents()
		{
			EventDAL ed = new EventDAL();
			List<Event> events = await ed.GetUserEvents(ParseUser.CurrentUser.ObjectId);
			foreach (Event e in events)
			{
				Events.Add(new EventViewModel()
				{
					Address = e.Address,
					Date = e.Date,
					Description = e.Description,
					OtherDetails = e.OtherDetails,
					Title = e.Title,
					ID = e.ID,
					City = e.City,
					Type = e.Type,
					State = e.State,
				});
			}


			EventsList.ItemsSource = this.Events;
		}
		private async Task LoadInvites()
		{
			EventDAL ed = new EventDAL();
			List<string> invites = await ed.GetInvitesForUser(ParseUser.CurrentUser.ObjectId);
			foreach(string id in invites)
			{
				Event e = await ed.RetrieveEvent(id);
				InviteViewModel ivm = new InviteViewModel()
				{
					Title = e.Title,
					Time = e.Time,
					Address = e.Address + ", "+ e.City +", "+e.State,
					ID = e.ID,
					Cost = e.Cost,
					Date = e.Date,
					Description = e.Description
				};
				this.Invites.Add(ivm);
			}
			InviteList.ItemsSource = this.Contacts;
		}

		private async Task LoadUser()
		{
			UserDAL ud = new UserDAL();
			ParseUser p = ParseUser.CurrentUser;
			UserInfo ui = await ud.GetUserInfo(p.ObjectId);
			ProfilePage.DataContext = ui;
			ProfilePage.Header = ui.FirstName + " " + ui.LastName;
		}

		private async Task LoadContacts()
		{
			UserDAL ud = new UserDAL();
			List<string> ids = await ud.GetContacts(ParseUser.CurrentUser.ObjectId);
			foreach (string id in ids)
			{
				try
				{
					UserInfo ui = await ud.GetUserInfo(id);
					this.Contacts.Add(ui);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}

			}


			List<AlphaKeyGroup<UserInfo>> DataSource = AlphaKeyGroup<UserInfo>.CreateGroups(Contacts,
				System.Threading.Thread.CurrentThread.CurrentUICulture,
				(UserInfo s) => { return s.FirstName; }, true);
			ContactList.ItemsSource = DataSource;
		}

		private void EventsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			//MessageBox.Show(((EventViewModel)EventsList.SelectedItem).Title);
			EventViewModel evm = (EventViewModel)EventsList.SelectedItem;
			NavigationService.Navigate(new Uri("/Pages/EventPanorama.xaml?msg=" + evm.ID, UriKind.Relative));
		}

		private void FindEvent_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/FindEvents.xaml", UriKind.Relative));
		}

		private void CreateEvent_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/CreateEvent.xaml", UriKind.Relative));
		}

		private void InviteList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			InviteViewModel evm = (InviteViewModel)InviteList.SelectedItem;
			NavigationService.Navigate(new Uri("/Pages/EventPage.xaml?msg=" + evm.ID, UriKind.Relative));
		}
			
		private void optionFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (isLoaded)
			{
				EventDAL ed = new EventDAL();
				EventsList.ItemsSource = ed.BasicFilter(Events.ToList(), optionFilter.SelectedIndex);
			}
			
		}
		
	}
}