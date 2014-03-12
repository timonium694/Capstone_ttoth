using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;
using TheMainEvent_Capstone.DataAccessLayer;
using TheMainEvent_Capstone.Model;
using Parse;

namespace TheMainEvent_Capstone.Pages
{
	public partial class Contact : PhoneApplicationPage
	{
		UserInfo ui = new UserInfo();
		UserInfo cur;
		List<UserInfo> Contacts = new List<UserInfo>();
		bool isContact = false;
		string address = "";
		string city = "";
		string state = "";
		string details = "";
		DateTime date = DateTime.Now;
		DateTime time = DateTime.Now;

		public Contact()
		{
			InitializeComponent();
		}
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			string msg = "";
			if (NavigationContext.QueryString.TryGetValue("msg", out msg))
			{
				string id = msg;
				this.LoadUser(id);
			}
		}
		private async void LoadUser(string id)
		{
			UserDAL ud = new UserDAL();
			this.ui = await ud.GetUserInfo(id);
			this.nameBlock.Text = ui.FirstName + " " + ui.LastName;
			this.UserPanel.DataContext = ui;
			List<string> ids = await ud.GetContacts(ParseUser.CurrentUser.ObjectId);
			foreach (string i in ids)
			{
				try
				{
					UserInfo u = await ud.GetUserInfo(i);
					this.Contacts.Add(u);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
			if (Contacts.Contains(ui))
			{
				isContact = true;
				ApplicationBar = ((ApplicationBar)this.Resources["IsContactAppBar"]);
			}
			else ApplicationBar = ((ApplicationBar)this.Resources["DefaultAppBar"]);
			cur = await ud.GetUserInfo(ParseUser.CurrentUser.ObjectId);
		}

		private void addContactItem_Click(object sender, EventArgs e)
		{
			UserDAL ud = new UserDAL();
			ud.AddContact(cur.User, ui.User);
		}



		private void PickDate()
		{
			StackPanel container = new StackPanel() { Orientation = System.Windows.Controls.Orientation.Horizontal };
			DatePicker dp = new DatePicker();
			dp.Value = DateTime.Now.Subtract(TimeSpan.FromDays(1));
			dp.ValueChanged += dp_ValueChanged;
			this.date.Subtract(TimeSpan.FromDays(1));
			dp.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
			container.Children.Add(dp);
			CustomMessageBox message = new CustomMessageBox()
			{
				Title = "Date",
				Message = "Choose the date of the meeting. Date and time must be in the futute.",
				Content = container,
				LeftButtonContent = "Cancel"
			};
			message.Dismissed += (s1, e1) =>
			{
				switch (e1.Result)
				{
					case CustomMessageBoxResult.RightButton:
						break;
					case CustomMessageBoxResult.LeftButton:
						message.Dismiss();
						break;
					case CustomMessageBoxResult.None:
						this.date = (DateTime)dp.Value;
						break;
					default:
						this.date = (DateTime)dp.Value;
						break;
				}
			};
			message.Show();
		}

		private void dp_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
		{
			DatePicker dp = (DatePicker)sender;
			this.date = (DateTime)dp.Value;
			if (date.CompareTo(DateTime.Now.Subtract(TimeSpan.FromHours(1))) < 0)
			{
			}
			else this.PickTime();

		}

		private void PickTime()
		{
			StackPanel container = new StackPanel() { Orientation = System.Windows.Controls.Orientation.Horizontal };
			TimePicker tp = new TimePicker();
			tp.ValueChanged += (x, x1) =>
			{
				this.time = (DateTime)tp.Value;
				if (time.CompareTo(DateTime.Now) < 0)
				{
				}
				else this.PickDetails();

			};
			this.date.Subtract(TimeSpan.FromHours(1));
			tp.Value = new DateTime();
			tp.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
			container.Children.Add(tp);
			CustomMessageBox message = new CustomMessageBox()
			{
				Title = "Time",
				Message = "Choose the time of the meeting. Date and time must be in the futute.",
				Content = container,
				LeftButtonContent = "Cancel"
			};
			message.IsRightButtonEnabled = false;
			message.Dismissed += (s1, e1) =>
			{
				switch (e1.Result)
				{
					case CustomMessageBoxResult.RightButton:
						break;
					case CustomMessageBoxResult.LeftButton:
						message.Dismiss();
						break;
					case CustomMessageBoxResult.None:
						this.time = (DateTime)tp.Value;
						break;
					default:
						this.time = (DateTime)tp.Value;
						break;
				}
			};
			message.Show();
		}

		private void PickDetails()
		{
			PhoneTextBox details = new PhoneTextBox();
			details.Height = 72;
			details.Width = 300;
			details.Hint = "Other Details";
			details.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;


			StackPanel container = new StackPanel();
			container.Children.Add(details);


			CustomMessageBox message = new CustomMessageBox()
			{
				Title = "Details",
				Content = container,
				Message = "Details about the meetings",
				RightButtonContent = "Continue",
				LeftButtonContent = "Cancel"
			};
			message.Dismissed += (s1, e1) =>
			{
				switch (e1.Result)
				{
					case CustomMessageBoxResult.RightButton:
						this.details = details.Text;
						this.CreateEvent();
						break;
					case CustomMessageBoxResult.LeftButton:
						message.Dismiss();
						break;
					case CustomMessageBoxResult.None:
						break;
					default:
						break;
				}
			};
			message.Show();
		}
		private async void CreateEvent()
		{
			statusBlock.Text = "Inviting user...";
			Event e = new Event();
			e.Date = this.date + this.time.TimeOfDay;
			e.OtherDetails = this.details;
			e.City = this.city;
			e.State = this.state;
			e.Address = this.address;
			e.Description = "Meeting called by " + cur.FirstName + " " + cur.LastName;
			e.Title = "Meeting";
			e.Type = "Meetings";
			e.Cost = 0;
			e.IsDonatable = false;
			EventDAL ed = new EventDAL();
			await ed.CreateEvent(e);
			string eventToAdd = await ed.NewestEventFromUser(cur.User);
			await ed.SetOwner(eventToAdd, cur.User);
			await ed.InviteUser(ui.User, eventToAdd, cur.User);
			statusBlock.Text = "User invited...";
		}

		private void eventsNav_Click(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/MainPages.xaml?msg=" + "events", UriKind.Relative));
		}

		private void contactsNav_Click(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/MainPages.xaml?msg=" + "contacts", UriKind.Relative));
		}

		private void logutNav_Click(object sender, EventArgs e)
		{
			ParseUser.LogOut();
			NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
		}

		private void searchNav_Click(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/SearchPage.xaml", UriKind.Relative));
		}
		private void inviteContactItem_Click(object sender, EventArgs e)
		{
			this.date = DateTime.Now;
			this.time = DateTime.Now;
			this.address = "";
			this.details = "";
			this.city = "";
			this.state = "";

			PhoneTextBox addr = new PhoneTextBox();
			addr.Height = 72;
			addr.Width = 300;
			addr.Hint = "Address";
			addr.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
			
			PhoneTextBox city = new PhoneTextBox();
			city.Height = 72;
			city.Width = 200;
			city.Hint = "City";
			city.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
			PhoneTextBox state = new PhoneTextBox();
			state.Height = 72;
			state.Width = 100;
			state.Hint = "State";
			state.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
			StackPanel cs = new StackPanel()
			{
				Orientation = System.Windows.Controls.Orientation.Horizontal
			};
			cs.Children.Add(city);
			cs.Children.Add(state);


			StackPanel container = new StackPanel();
			container.Children.Add(addr);
			container.Children.Add(cs);


			CustomMessageBox message = new CustomMessageBox()
			{
				Title = "Address",
				Content = container,
				Message = "Enter the address of the meeting",
				RightButtonContent = "Continue",
				LeftButtonContent = "Cancel",
				IsRightButtonEnabled = false,
			};
			addr.TextChanged += (a1, a2) =>
			{
				if (!string.IsNullOrEmpty(addr.Text) && !string.IsNullOrEmpty(city.Text) && !string.IsNullOrEmpty(state.Text))
				{
					message.IsRightButtonEnabled = true;
				}
				else message.IsRightButtonEnabled = false;
			};
			city.TextChanged += (a1, a2) =>
			{
				if (!string.IsNullOrEmpty(addr.Text) && !string.IsNullOrEmpty(city.Text) && !string.IsNullOrEmpty(state.Text))
				{
					message.IsRightButtonEnabled = true;
				}
				else message.IsRightButtonEnabled = false;
			};
			state.TextChanged += (a1, a2) =>
			{
				if (!string.IsNullOrEmpty(addr.Text) && !string.IsNullOrEmpty(city.Text) && !string.IsNullOrEmpty(state.Text))
				{
					message.IsRightButtonEnabled = true;
				}
				else message.IsRightButtonEnabled = false;
			};
			message.Dismissed += (s1, e1) =>
			{
				switch (e1.Result)
				{
					case CustomMessageBoxResult.RightButton:
						this.address = addr.Text;
						this.state = state.Text;
						this.city = city.Text;
						this.PickDate();
						break;
					case CustomMessageBoxResult.LeftButton:
						message.Dismiss();
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
	}
}