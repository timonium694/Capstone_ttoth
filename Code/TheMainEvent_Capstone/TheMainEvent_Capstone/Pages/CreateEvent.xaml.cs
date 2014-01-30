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
			city.Tap += this.TextBox_Tap;
			address.Tap += this.TextBox_Tap;
			state.Tap += this.TextBox_Tap;
			cost.Tap += this.TextBox_Tap;
			description.Tap += this.TextBox_Tap;
			otherDetails.Tap += this.TextBox_Tap;
			title.Tap += this.TextBox_Tap;
		}
		private async void AddUsers()
		{
			UserDAL ud = new UserDAL();
			List<string> ids = new List<string>();
			ids = await ud.GetContacts(user.ObjectId);
			foreach (string id in ids)
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

			contacts.ItemsSource = Users;
		}


		private void OnTextInputStart(object sender, TextCompositionEventArgs e)
		{
			scrollViewer.UpdateLayout();
			scrollViewer.ScrollToVerticalOffset(description.ActualHeight);
		}

		private void TextBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			TextBox box = (TextBox)sender;
			if (box.Text.Equals("Title") || box.Text.Equals("City") || box.Text.Equals("State") || box.Text.Equals("Description") || box.Text.Equals("Street Address") || box.Text.Equals("Other Details") || box.Text.Equals("0.00"))
			{
				box.Text = "";
			}
		}

		private void createButton_Click(object sender, RoutedEventArgs e)
		{
			contacts.s
		}
		
	}
}