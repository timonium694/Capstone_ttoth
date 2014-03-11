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
using System.Windows.Media.Imaging;
using Microsoft.Phone.Shell;

namespace TheMainEvent_Capstone.Pages
{
	public partial class MainPages : PhoneApplicationPage
	{
		ObservableCollection<EventViewModel> Events = new ObservableCollection<EventViewModel>();
		ObservableCollection<InviteViewModel> Invites = new ObservableCollection<InviteViewModel>();
		List<UserInfo> Contacts = new List<UserInfo>();
		UserInfo cur;
		private bool isLoaded = false;
		private bool noEvents = false;
		private bool noContacts = false;
		private bool noInvites = false;

		public MainPages()
		{
			InitializeComponent();
		}
		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			await this.LoadUser();
			await this.LoadEvents();
			await this.LoadInvites();
			await this.LoadContacts();
			this.isLoaded = true;

			ShowOrHide();
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

		private void ShowOrHide()
		{
			if (noEvents)
			{
				noEventContainer.Visibility = Visibility.Visible;
				EventsList.Visibility = Visibility.Collapsed;
			}
			else
			{
				noEventContainer.Visibility = Visibility.Collapsed;
				EventsList.Visibility = Visibility.Visible;
			}
			if (noContacts)
			{
				noContactsContainer.Visibility = Visibility.Visible;
				ContactList.Visibility = Visibility.Collapsed;
			}
			else
			{
				noContactsContainer.Visibility = Visibility.Collapsed;
				ContactList.Visibility = Visibility.Visible;
			}
			if (noInvites)
			{
				noInvitesContainer.Visibility = Visibility.Visible;
				InviteList.Visibility = Visibility.Collapsed;
			}
			else
			{
				noInvitesContainer.Visibility = Visibility.Collapsed;
				InviteList.Visibility = Visibility.Visible;
			}
			this.MainPivot.Visibility = Visibility.Visible;
			this.loadingBar.Visibility = Visibility.Collapsed;
			this.loadingBar.IsIndeterminate = false;
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
			if (cur.FilterMode.Equals("A-Z"))
			{
				events.OrderBy(x => x.Title);
			}
			if (cur.FilterMode.Equals("Z-A"))
			{
				events.OrderByDescending(x => x.Title);
			}
			if (cur.FilterMode.Equals("Date"))
			{
				events.OrderByDescending(x => x.Title);
			}
			if (cur.FilterMode.Equals("Type"))
			{
				events.OrderByDescending(x => x.Title);
			}
			if (events.Count != 0)
			{
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
			else
			{
				this.noEvents = true;
			}
			
		}
		private async Task LoadInvites()
		{
			EventDAL ed = new EventDAL();
			List<string> invites = await ed.GetInvitesForUser(ParseUser.CurrentUser.ObjectId);
			if (invites.Count != 0)
			{
				foreach (string id in invites)
				{
					Event e = await ed.RetrieveEvent(id);
					InviteViewModel ivm = new InviteViewModel()
					{
						Title = e.Title,
						Time = e.Time,
						Address = e.Address + ", " + e.City + ", " + e.State,
						ID = e.ID,
						Cost = e.Cost,
						Date = e.Date,
						Description = e.Description
					};
					this.Invites.Add(ivm);
				}
				InviteList.ItemsSource = this.Invites;
			}
			else this.noInvites = true;
		}

		private async Task LoadUser()
		{
			UserDAL ud = new UserDAL();
			ParseUser p = ParseUser.CurrentUser;
			cur = await ud.GetUserInfo(p.ObjectId);
			ProfilePage.DataContext = cur;
			ProfilePage.Header = cur.FirstName + " " + cur.LastName;
			
		}

		private async Task LoadContacts()
		{
			UserDAL ud = new UserDAL();
			List<string> ids = await ud.GetContacts(ParseUser.CurrentUser.ObjectId);
			if (ids.Count != 0)
			{
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
			else
			{
				this.noContacts = true;
			}
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

		private void navToCreateEvent(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/CreateEvent.xaml", UriKind.Relative));
		}

		private void navToSearchPage(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/SearchPage.xaml", UriKind.Relative));
		}

		private void navToSearch(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/SearchPage.xaml", UriKind.Relative));
		}

		private void navToEvents(object sender, EventArgs e)
		{
			MainPivot.SelectedIndex = 0;
		}

		private void navToContacts(object sender, EventArgs e)
		{
			MainPivot.SelectedIndex = 1;
		}

		private void logout_Click(object sender, EventArgs e)
		{
			ParseUser.LogOut();
			NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
		}

		private void navToCreate(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/CreateEvent.xaml", UriKind.Relative));
		}

		private void navToEditAccount(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/EditUser.xaml", UriKind.Relative));
		}

		private void navToAccountSettings(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/UserAccountSettingsPage.xaml", UriKind.Relative));
		}

		private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			switch (((Pivot)sender).SelectedIndex)
			{
				case 0:
					ApplicationBar = ((ApplicationBar)this.Resources["DefaultAppBar"]);
					break;

				case 1:
					ApplicationBar = ((ApplicationBar)this.Resources["DefaultAppBar"]);
					break;
				case 2:
					ApplicationBar = ((ApplicationBar)this.Resources["DefaultAppBar"]);
					break;
				case 3:
					ApplicationBar = ((ApplicationBar)this.Resources["DefaultAppBar"]);
					break;
			}
		}

		private void ContactList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			UserInfo ui = (UserInfo)ContactList.SelectedItem;
			NavigationService.Navigate(new Uri("/Pages/Contact.xaml?msg=" + ui.User, UriKind.Relative));
		}
		
	}
}