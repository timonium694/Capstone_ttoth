using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TheMainEvent_Capstone.Model;
using TheMainEvent_Capstone.DataAccessLayer;
using Parse;

namespace TheMainEvent_Capstone.Pages
{
	public partial class KickAttendants : PhoneApplicationPage
	{
		UserInfo cur;
		string curId;
		string eventId;
		Event ev;
		List<UserInfo> attendants = new List<UserInfo>();
		public KickAttendants()
		{
			InitializeComponent();
		}
		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			string msg = "";
			if (NavigationContext.QueryString.TryGetValue("msg", out msg))
			{
				EventDAL ed = new EventDAL();
				this.ev = await ed.RetrieveEvent(msg);
				this.eventId = ev.ID;
				UserDAL ud = new UserDAL();
				this.curId = ParseUser.CurrentUser.ObjectId;
				this.cur = await ud.GetUserInfo(curId);
				List<string> atts = await ed.GetAttendees(eventId);
				foreach (string id in atts)
				{
					UserInfo ui = await ud.GetUserInfo(id);
					this.attendants.Add(ui);
				}
			}
		}

		private async void kiskUsers_Click(object sender, EventArgs e)
		{
			EventDAL ed = new EventDAL();
			NotificationDAL nd = new NotificationDAL();
			foreach (var item in contactList.SelectedItems)
			{
				Notification n = new Notification();
				UserInfo ui = (UserInfo)item;
				n.Date = DateTime.Now;
				n.Message = "You have been kicked from " + ev.Title;
				n.User = ui.User;
				await ed.UnattendEvent(ui.User, this.eventId);
			}
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

		private void info_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Selected users will be kicked from this event.");
		}

		private void goback_click(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/EventPanorama.xaml?msg=" + this.eventId, UriKind.Relative));
		}
	}
}