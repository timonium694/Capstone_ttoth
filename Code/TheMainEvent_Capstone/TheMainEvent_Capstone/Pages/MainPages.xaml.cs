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
			EventsList.ItemsSource = this.Events;
			//Load all the data:
			//Contacts, Events, Invites, 
		}
		private void LoadEvents()
		{
			Events.Add(new EventViewModel()
			{
				Address = "143 South Main St",
				Date = DateTime.Now,
				Description = "Come have fun",
				OtherDetails = "BYOD",
				Title = "Party"
			});
			Events.Add(new EventViewModel()
			{
				Address = "143 South Main St",
				Date = DateTime.Now,
				Description = "Come have fun",
				OtherDetails = "BYOD",
				Title = "Party 1"
			});
			Events.Add(new EventViewModel()
			{
				Address = "143 South Main St",
				Date = DateTime.Now,
				Description = "Come have fun",
				OtherDetails = "BYOD",
				Title = "Party 2"
			});
			Events.Add(new EventViewModel()
			{
				Address = "143 South Main St",
				Date = DateTime.Now,
				Description = "Come have fun",
				OtherDetails = "BYOD",
				Title = "Party 3"
			});
		}

		private void EventsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			//MessageBox.Show(((EventViewModel)EventsList.SelectedItem).Title);
			NavigationService.Navigate(new Uri("/Pages/MapPage.xaml?msg=" + ((EventViewModel)EventsList.SelectedItem).Address + ", Salt Lake City", UriKind.Relative));
		}
	}
}