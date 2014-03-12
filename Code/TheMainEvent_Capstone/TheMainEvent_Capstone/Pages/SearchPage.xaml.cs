using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TheMainEvent_Capstone.Model.ViewModels;
using TheMainEvent_Capstone.DataAccessLayer;
using TheMainEvent_Capstone.Model;
using System.Collections.ObjectModel;
using Parse;

namespace TheMainEvent_Capstone.Pages
{
	public partial class Page1 : PhoneApplicationPage
	{
		ObservableCollection<EventViewModel> Events = new ObservableCollection<EventViewModel>();
		ObservableCollection<UserInfo> Users = new ObservableCollection<UserInfo>();
		public Page1()
		{
			InitializeComponent();

			EventsList.ItemsSource = this.Events;
			UserList.ItemsSource = this.Users;
		}

		private async void searchButton_Click(object sender, RoutedEventArgs e)
		{
			await Search();
		}

		private async System.Threading.Tasks.Task Search()
		{
			this.statusBox.Text = "Searching...";
			EventDAL ed = new EventDAL();
			List<Event> events = await ed.BasicEventSearch(searchBox.Text);
			Events.Clear();
			if (events.Count != 0)
			{
				this.EventsList.Visibility = Visibility.Visible;
				this.noEventResults.Visibility = Visibility.Collapsed;
				foreach (Event ev in events)
				{
					Events.Add(new EventViewModel()
					{
						Address = ev.Address,
						Date = ev.Date,
						Description = ev.Description,
						OtherDetails = ev.OtherDetails,
						Title = ev.Title,
						ID = ev.ID,
						City = ev.City,
						Type = ev.Type.ToString(),
						State = ev.State,
					});
				}
			}
			else
			{
				this.noEventResults.Visibility = Visibility.Visible;
				this.EventsList.Visibility = Visibility.Collapsed;
			}
			UserDAL ud = new UserDAL();
			List<UserInfo> uis = await ud.SearchUsers(searchBox.Text);
			Users.Clear();
			if (uis.Count != 0)
			{

				noUserResults.Visibility = Visibility.Collapsed;
				this.UserList.Visibility = Visibility.Visible;
				foreach (UserInfo ui in uis)
				{
					this.Users.Add(ui);
				}
			}
			else
			{
				noUserResults.Visibility = Visibility.Visible;
				this.UserList.Visibility = Visibility.Collapsed;
			}

			this.statusBox.Text = "The Main Event";
		}
		private void eventsNav_Click(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/MainPages.xaml?msg=" + "events", UriKind.Relative));
		}

		private void contactsNav_Click(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/MainPages.xaml?msg=" + "contacts", UriKind.Relative));
		}

		private void searchNav_Click(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/SearchPage.xaml", UriKind.Relative));
		}

		private void logutNav_Click(object sender, EventArgs e)
		{
			ParseUser.LogOut();
			NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
		}
		private void EventsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			//MessageBox.Show(((EventViewModel)EventsList.SelectedItem).Title);
			EventViewModel evm = (EventViewModel)EventsList.SelectedItem;
			NavigationService.Navigate(new Uri("/Pages/EventPanorama.xaml?msg=" + evm.ID, UriKind.Relative));
		}
		private void ContactList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			UserInfo ui = (UserInfo)UserList.SelectedItem;
			NavigationService.Navigate(new Uri("/Pages/Contact.xaml?msg=" + ui.User, UriKind.Relative));
		}
	}
}