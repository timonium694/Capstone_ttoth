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

		private void SetupPage()
		{
			if (ParseUser.CurrentUser != null)
			{
				user = ParseUser.CurrentUser;
			}
			cityBox.Tap += this.TextBox_Tap;
			addressBox.Tap += this.TextBox_Tap;
			stateBox.Tap += this.TextBox_Tap;
			costBox.Tap += this.TextBox_Tap;
			descriptionBox.Tap += this.TextBox_Tap;
			otherDetailsBox.Tap += this.TextBox_Tap;
			titleBox.Tap += this.TextBox_Tap;
		}
		private async void AddUsers()
		{
			UserDAL ud = new UserDAL();
			List<string> ids = new List<string>();
			ids = await ud.GetContacts("1iQVkxqPGY");
			foreach (string id in ids)
			{
				try
				{
					UserInfo ui = await ud.GetUserInfo(id);
					Users.Add(new ContactViewModel()
					{
						Name = ui.FirstName + " " + ui.LastName,
						Id = ui.Id,
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


		private void OnTextInputStart(object sender, TextCompositionEventArgs e)
		{
			scrollViewer.UpdateLayout();
			scrollViewer.ScrollToVerticalOffset(descriptionBox.ActualHeight);
		}

		private void TextBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			TextBox box = (TextBox)sender;
			if (box.Text.Equals("Title") || box.Text.Equals("City") || box.Text.Equals("State") || box.Text.Equals("Description") || box.Text.Equals("Street Address") || box.Text.Equals("Other Details") || box.Text.Equals("0.00"))
			{
				box.Text = "";
			}
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
					Date = (DateTime)datePicker.Value
				};
				double cost = 0;
				if (double.TryParse(costBox.Text, out cost))
				{
					ev.Cost = cost;
				}
				else
				{
					throw new Exception("Make sure the cost follows the format of a price e.g.: 9.00");
				}

				EventDAL ed = new EventDAL();
				ed.CreateEvent(ev);
				string creatorId = ParseUser.CurrentUser.ObjectId;
				string eventToAdd = await ed.NewestEventFromUser("1iQVkxqPGY");
				ed.SetOwner(eventToAdd, creatorId);
				foreach (Object i in contacts.SelectedItems)
				{
					ContactViewModel con = (ContactViewModel)i;
					ed.InviteUser(con.Id, eventToAdd, creatorId);
				}
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
		
	}
}