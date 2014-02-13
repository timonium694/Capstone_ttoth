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
using System.Collections.ObjectModel;
using TheMainEvent_Capstone.DataAccessLayer;
using TheMainEvent_Capstone.Model;

namespace TheMainEvent_Capstone.Controls
{
	public partial class EventSearcher : UserControl
	{
		ObservableCollection<EventViewModel> Events = new ObservableCollection<EventViewModel>();
		public EventSearcher()
		{
			InitializeComponent();
		}

		private async void searchButton_Click(object sender, RoutedEventArgs e)
		{
			EventDAL ed = new EventDAL();
			List<Event> events = await ed.BasicEventSearch(searchBox.Text);
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
			EventsList.ItemsSource = this.Events;
		}

	}
}
