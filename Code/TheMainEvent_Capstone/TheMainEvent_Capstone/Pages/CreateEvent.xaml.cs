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
using TheMainEvent_Capstone.DataAccessLayer;
using Parse;
using TheMainEvent_Capstone.Model;
using System.Collections;
using System.Text.RegularExpressions;

namespace TheMainEvent_Capstone.Pages
{
	public partial class CreateEvent : PhoneApplicationPage
	{
		ObservableCollection<ContactViewModel> Users = new ObservableCollection<ContactViewModel>();
		ParseUser user;
		public CreateEvent()
		{
			InitializeComponent();
			this.AddUsers();
			this.SetupPage();
		}

		private async void SetupPage()
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
			UserDAL ud = new UserDAL();
			UserInfo ui = await ud.GetUserInfo(ParseUser.CurrentUser.ObjectId);
			if (ui.MerchantEmail.Equals("none"))
				costPanel.Visibility = Visibility.Collapsed;
		}

		
		private async void AddUsers()
		{
			UserDAL ud = new UserDAL();
			List<string> ids = new List<string>();
			ids = await ud.GetContacts(ParseUser.CurrentUser.ObjectId);
			foreach (string id in ids)
			{
				try
				{
					UserInfo ui = await ud.GetUserInfo(id);
					Users.Add(new ContactViewModel()
					{
						Name = ui.FirstName + " " + ui.LastName,
						User = ui.User,
						Bio = ui.Bio,
						Birthday = ui.Birthday,
						Phone = ui.Phone,
					});
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
				
			}

			contacts.ItemsSource = Users;
		}

		private void TextBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			TextBox box = (TextBox)sender;
			
		}

		private async void createButton_Click(object sender, RoutedEventArgs e)
		{
			string eventToAdd = "";
			this.statusBlock.Text = "Creating...";
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
					Time = (DateTime)timePicker.Value,
					Type = ((TextBlock)typePicker.SelectedItem).Text
				};
				double cost = 0;
				if (double.TryParse(costBox.Text, out cost))
				{
					ev.Cost = cost;
				}
				else if (String.IsNullOrEmpty(costBox.Text))
				{
					ev.Cost = 0;
				}

				EventDAL ed = new EventDAL();
				ed.CreateEvent(ev);
				string creatorId = ParseUser.CurrentUser.ObjectId;
				eventToAdd = await ed.NewestEventFromUser(ParseUser.CurrentUser.ObjectId );
				ed.SetOwner(eventToAdd, creatorId);
				foreach (Object i in contacts.SelectedItems)
				{
					ContactViewModel con = (ContactViewModel)i;
					ed.InviteUser(con.User, eventToAdd, creatorId);
				}
			}
			catch (Exception ex)
			{
				statusBlock.Text = ex.Message;
			}
			NavigationService.Navigate(new Uri("/Pages/EventPanorama.xaml?msg=" + eventToAdd, UriKind.Relative));
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

		private void donationCheck_Checked(object sender, RoutedEventArgs e)
		{
			if (donationCheck.IsChecked==true)
			{
				MessageBox.Show("Attendees will choose whether or not he/she will pay to attend the event.");
			}
		}
		private void costBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			string input = costBox.Text;
			string pattern = @"^((\d+).(\d){2})$";
			if (!Regex.IsMatch(input, pattern))
			{
				costStatus.Visibility = Visibility.Visible;
			}
			else costStatus.Visibility = Visibility.Collapsed;
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

		
		
	}
}