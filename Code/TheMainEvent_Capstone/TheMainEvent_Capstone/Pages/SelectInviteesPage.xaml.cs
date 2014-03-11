using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Parse;
using TheMainEvent_Capstone.Model;
using TheMainEvent_Capstone.DataAccessLayer;
using System.Collections.ObjectModel;

namespace TheMainEvent_Capstone.Pages
{
	public partial class SelectInviteesPage : PhoneApplicationPage
	{
		UserInfo cur;
		string eventId = "";
		string curId = "";
		List<UserInfo> contacts = new List<UserInfo>();
		List<UserInfo> invitees = new List<UserInfo>();
		List<UserInfo> attendees = new List<UserInfo>();
		ObservableCollection<UserInfo> finalList = new ObservableCollection<UserInfo>();
		public SelectInviteesPage()
		{
			InitializeComponent();
		}

		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			string msg = "";
			if (NavigationContext.QueryString.TryGetValue("msg", out msg))
			{
				EventDAL ed = new EventDAL();
				Event ev = await ed.RetrieveEvent(msg);
				this.eventId = ev.ID;
				UserDAL ud = new UserDAL();
				this.curId = ParseUser.CurrentUser.ObjectId;
				this.cur = await ud.GetUserInfo(curId);
				ApplicationBar = ((ApplicationBar)this.Resources["DefaultAppBar"]);
			}
		}

		private async void LoadUsers()
		{
			UserDAL ud = new UserDAL();
			EventDAL ed = new EventDAL();
			List<string> uIds = new List<string>();
			uIds = await ud.GetContacts(this.curId);
			List<string> invIds = new List<string>();
			invIds = await ed.GetInvitees(this.eventId);
			List<string> attIds = new List<string>();
			attIds = await ed.GetAttendees(this.eventId);
			foreach(string id in attIds)
			{
				UserInfo ui = await ud.GetUserInfo(id);
				this.attendees.Add(ui);
			}
			foreach (string id in invIds)
			{
				UserInfo ui = await ud.GetUserInfo(id);
				this.invitees.Add(ui);
				this.finalList.Add(ui);
			}
			foreach(string id in uIds)
			{
				UserInfo ui = await ud.GetUserInfo(id);
				if (!invitees.Contains(ui) && !attendees.Contains(ui))
				{
					this.contacts.Add(ui);
					this.finalList.Add(ui);
				}
			}
			
			this.contactList.ItemsSource = this.finalList;
			foreach (var item in this.contactList.ItemsSource)
			{
				if (invitees.Contains((UserInfo)item))
				{
					contactList.ScrollTo(item);
					LongListMultiSelectorItem container = contactList.ContainerFromItem(item) as LongListMultiSelectorItem;
					if (container != null)
					{
						container.IsSelected = true;
					}
				}
			}

		}

		private async void inviteUsers_Click(object sender, EventArgs e)
		{
			EventDAL ed = new EventDAL();
			foreach (var item in this.contactList.SelectedItems)
			{
				UserInfo ui = (UserInfo)item;
				if (!invitees.Contains(ui))
				{
					await ed.InviteUser(ui.User, this.eventId, this.curId);
				}
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
	}
}