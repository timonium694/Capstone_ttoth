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
		List<UserInfo> Contacts = new List<UserInfo>();
		bool isContact = false;
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
		}

		private void addContactItem_Click(object sender, EventArgs e)
		{

		}

		private void inviteContactItem_Click(object sender, EventArgs e)
		{

		}

		private void eventsNav_Click(object sender, EventArgs e)
		{

		}

		private void contactsNav_Click(object sender, EventArgs e)
		{

		}

		private void logutNav_Click(object sender, EventArgs e)
		{

		}

		private void searchNav_Click(object sender, EventArgs e)
		{

		}
	}
}