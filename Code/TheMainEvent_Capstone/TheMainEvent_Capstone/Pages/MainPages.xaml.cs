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
using PayPal.Checkout;
using Microsoft.Phone.Scheduler;
using Windows.Phone.System.UserProfile;

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
			this.ShowLoadingBar();
			await this.LoadUser();
			await this.LoadEvents();
			await this.LoadInvites();
			await this.LoadContacts();
			this.isLoaded = true;

			ShowOrHide();
			string msg = "";
			if (NavigationContext.QueryString.TryGetValue("msg", out msg))
			{
				if (msg.Equals("events"))
					this.MainPivot.SelectedIndex = 0;
				if (msg.Equals("contacts"))
					this.MainPivot.SelectedIndex = 1;
			}
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
			ShowUI();
		}
		private void ShowLoadingBar()
		{
			this.loadingBar.Visibility = Visibility.Visible;
			this.loadingBar.IsIndeterminate = true;
			this.MainPivot.Visibility = Visibility.Collapsed;
		}
		private void ShowUI()
		{
			this.loadingBar.Visibility = Visibility.Collapsed;
			this.loadingBar.IsIndeterminate = false;
			this.MainPivot.Visibility = Visibility.Visible;
		}
		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			this.Events.Clear();
			this.Invites.Clear();
			this.Contacts.Clear();
			this.ContactList.ItemsSource.Clear();
			this.ShowLoadingBar();
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
				events.OrderBy(x => x.Date);
			}
			if (cur.FilterMode.Equals("Type"))
			{
				events.OrderBy(x => x.Type);
			}
			if (events.Count != 0)
			{
				foreach (Event e in events)
				{
					if (e.Date.CompareTo(DateTime.Now) > 0)
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
							Cost = e.Cost
						});
					}
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
						Description = e.Description,
						IsDonatable = e.IsDonatable
					};
					this.Invites.Add(ivm);
				}
				this.Invites.OrderBy(x => x.Date);
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
			if (ContactList.ItemsSource != null)
				this.ContactList.ItemsSource.Clear();
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
			InviteViewModel ivm = (InviteViewModel)InviteList.SelectedItem;
			EventDAL ed = new EventDAL();
			CustomMessageBox message = new CustomMessageBox()
			{
				Title = "Invite",
				Message = "Click \"Accept\" to accept the invitation or \"Reject\" to decline the invitation.",
				RightButtonContent = "Accept",
				LeftButtonContent = "Reject"
			};
			message.Dismissed += async (s1, e1) =>
			{
				switch (e1.Result)
				{
					case CustomMessageBoxResult.RightButton:
						if (ivm.Cost > 0)
						{
							MessageBox.Show("You must pay to attend this event, you will be redirected to the event page.");
							NavigationService.Navigate(new Uri("/Pages/EventPanorama.xaml?msg=" + ivm.ID, UriKind.Relative));
						}
						else if (ivm.IsDonatable)
						{
							MessageBox.Show("You have the option to donate to this page, you will be redirected to the event page to continue.");
							NavigationService.Navigate(new Uri("/Pages/EventPanorama.xaml?msg=" + ivm.ID, UriKind.Relative));
						}
						else
						{
							this.ShowLoadingBar();

							NotificationDAL nd = new NotificationDAL();
							Notification n = new Notification();
							UserDAL ud = new UserDAL();
							string ownerId = await ed.GetOwner(ivm.ID);
							UserInfo owner = await ud.GetUserInfo(ownerId);
							n.User = owner.User;
							n.Date = DateTime.Now;
							n.Message = this.cur.FirstName + " " + cur.LastName + " is attending " + ivm.Title;
							await nd.CreateNotification(n);
							await ed.AddAttendee(ivm.ID, cur.User);
							await this.LoadInvites();
							this.ShowUI();
							MessageBox.Show("You are now attending " + ivm.Title);
						}
						break;
					case CustomMessageBoxResult.LeftButton:
						this.ShowLoadingBar();
						await ed.RejectInvitation(cur.User, ivm.ID);
						await this.LoadInvites();
						this.ShowUI();
						MessageBox.Show("You have rejected the invitation for " + ivm.Title);
						break;
					case CustomMessageBoxResult.None:
						message.Dismiss();
						break;
					default:
						break;
				}
			};
			message.Show();

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

		private void navToNotifications(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/NotificationsPage.xaml", UriKind.Relative));
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