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

namespace TheMainEvent_Capstone.Pages
{
	public partial class MainPages : PhoneApplicationPage
	{
		ObservableCollection<EventViewModel> Events = new ObservableCollection<EventViewModel>();
		public MainPages()
		{
			InitializeComponent();
		}
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			this.LoadEvents();
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
		private async void LoadEvents()
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
					Type = e.City,
					State = e.State,
				});
			}


			EventsList.ItemsSource = this.Events;
		}

		private void EventsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			//MessageBox.Show(((EventViewModel)EventsList.SelectedItem).Title);
			EventViewModel evm = (EventViewModel)EventsList.SelectedItem;
			NavigationService.Navigate(new Uri("/Pages/EventPage.xaml?msg=" + evm.ID, UriKind.Relative));
		}

		private void FindEvent_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/FindEvents.xaml", UriKind.Relative));
		}

		private void CreateEvent_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/CreateEvent.xaml", UriKind.Relative));
		}
	}
}