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

namespace TheMainEvent_Capstone.Pages
{
	public partial class CreateEvent : PhoneApplicationPage
	{
		ObservableCollection<ContactViewModel> Users = new ObservableCollection<ContactViewModel>();
		public CreateEvent()
		{
			InitializeComponent();
			this.AddUsers();
			contacts.ItemsSource = Users;
			city.Tap += this.TextBox_Tap;
			address.Tap += this.TextBox_Tap;
			state.Tap += this.TextBox_Tap;
			cost.Tap += this.TextBox_Tap;
			description.Tap += this.TextBox_Tap;
			otherDetails.Tap += this.TextBox_Tap;
			title.Tap += this.TextBox_Tap;
		}
		private void AddUsers()
		{
			Users.Add(new ContactViewModel()
			{
				Name = "John",
				Phone = "330-766-0092",
			});
			Users.Add(new ContactViewModel()
			{
				Name = "John 1",
				Phone = "330-766-0092",
			});
			Users.Add(new ContactViewModel()
			{
				Name = "John 2",
				Phone = "330-766-0092",
			});
			Users.Add(new ContactViewModel()
			{
				Name = "John 3",
				Phone = "330-766-0092",
			});
			Users.Add(new ContactViewModel()
			{
				Name = "John 4",
				Phone = "330-766-0092",
			});
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
		
	}
}