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
using Parse;
using TheMainEvent_Capstone.Model;
using TheMainEvent_Capstone.DataAccessLayer;
using TheMainEvent_Capstone.Model.ViewModels;
using System.Windows.Input;

namespace TheMainEvent_Capstone.Pages
{
	public partial class EditEvent : PhoneApplicationPage
	{
		ObservableCollection<ContactViewModel> Users = new ObservableCollection<ContactViewModel>();
		ParseUser user;
		public EditEvent()
		{
			InitializeComponent();
			this.SetupPage();
		}

		private void SetupPage()
		{
			if (ParseUser.CurrentUser != null)
			{
				user = ParseUser.CurrentUser;
			}
			descriptionBox.GotFocus += this.DescriptionBox_GotFocus;
			otherDetailsBox.GotFocus += this.OtherDetailsBox_GotFocus;
			descriptionBox.LostFocus += this.DescriptionBox_LostFocus;
			otherDetailsBox.LostFocus += this.OtherDetailsBox_LostFocus;
			titleBox.TextChanged += this.title_TextChanged;
		}
		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			string msg = "";
			EventViewModel evm = new EventViewModel();
			if (NavigationContext.QueryString.TryGetValue("msg", out msg))
			{
				EventDAL ed = new EventDAL();
				Event ev = await ed.RetrieveEvent(msg);
				evm = new EventViewModel()
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
			}
		}


		private void TextBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			TextBox box = (TextBox)sender;
			
		}

		private async void createButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Event ev = new Event()
				{
					City = cityBox.Text,
					Address = addressBox.Text,
					State = stateBox.Text,
					Description = descriptionBox.Text,
					OtherDetails = otherDetailsBox.Text,
					Title = titleBox.Text,
					Date = (DateTime)datePicker.Value,
					Time = (DateTime)timePicker.Value
				};
				double cost = 0;
				if (double.TryParse(costBox.Text, out cost))
				{
					ev.Cost = cost;
				}
				else
				{
					throw new Exception("Make sure the cost follows the format of a price e.g.: 9.50");
				}

				EventDAL ed = new EventDAL();
				ed.CreateEvent(ev);
				string creatorId = ParseUser.CurrentUser.ObjectId;
				string eventToAdd = await ed.NewestEventFromUser(ParseUser.CurrentUser.ObjectId );
				ed.SetOwner(eventToAdd, creatorId);
			}
			catch (Exception ex)
			{
				NavigationService.Navigate(new Uri("/Pages/MapPage.xaml?msg=" + ex.Message, UriKind.Relative));
			}
			NavigationService.Navigate(new Uri("/Pages/MapPage.xaml", UriKind.Relative));
		}

		private void title_TextChanged(object sender, TextChangedEventArgs e)
		{
			titleBlock.Text = titleBox.Text;
		}


		private void DescriptionBox_GotFocus(object sender, RoutedEventArgs e)
		{
			this.descGotFocusStoryboard.Begin();
		}
		private void DescriptionBox_LostFocus(object sender, RoutedEventArgs e)
		{
			this.descLostFocusStoryboard.Begin();
		}



		private void OtherDetailsBox_GotFocus(object sender, RoutedEventArgs e)
		{
			this.otherGotFocusStoryboard.Begin();
		}
		private void OtherDetailsBox_LostFocus(object sender, RoutedEventArgs e)
		{
			this.otherLostFocusStoryboard.Begin();
		}
	}
}