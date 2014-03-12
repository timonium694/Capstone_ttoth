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
using System.Threading.Tasks;

namespace TheMainEvent_Capstone.Pages
{
	public partial class SelectInviteesPage : PhoneApplicationPage
	{
		UserInfo cur;
		string eventId = "";
		string curId = "";
		Event ev;
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
				ev = await ed.RetrieveEvent(msg);
				this.eventId = ev.ID;
				UserDAL ud = new UserDAL();
				this.curId = ParseUser.CurrentUser.ObjectId;
				this.cur = await ud.GetUserInfo(curId);
				ApplicationBar = ((ApplicationBar)this.Resources["DefaultAppBar"]);
				this.LoadUsers();
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

		private void inviteUsers_Click(object sender, EventArgs e)
		{
			this.loadingBar.IsIndeterminate = true;
			this.loadingBar.Visibility = Visibility.Visible;
			CustomMessageBox message = new CustomMessageBox()
			{
				RightButtonContent = "Invite",
				LeftButtonContent = "Cancel",
				Message = "Are you sure you want to invite these people?",
				Title = "Invitations"
			};
			message.Dismissed += async (s1, e1) =>
			{
				switch (e1.Result)
				{
					case CustomMessageBoxResult.RightButton:
						await InvitePeople();
						break;
					case CustomMessageBoxResult.LeftButton:
						message.Dismiss();
						break;
					case CustomMessageBoxResult.None:
						break;
					default:
						break;
				}

				this.loadingBar.IsIndeterminate = false;
				this.loadingBar.Visibility = Visibility.Collapsed;
			};
			message.Show();
			
		}

		private async Task InvitePeople()
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
			foreach (var item in this.contactList.ItemsSource)
			{
				LongListMultiSelectorItem container = contactList.ContainerFromItem(item) as LongListMultiSelectorItem;
				UserInfo ui = (UserInfo)item;
				if (!container.IsSelected && invitees.Contains(ui))
				{
					await ed.RejectInvitation(ui.User, this.eventId);
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

		private void info_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Selecting a checkmark will invite the user. Deselecting a checkmark will uninvite the user.");
		}

		private void goback_click(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/EventPanorama.xaml?msg=" + this.eventId, UriKind.Relative));
		}
	}
}