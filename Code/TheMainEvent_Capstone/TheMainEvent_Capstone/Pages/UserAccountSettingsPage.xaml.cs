using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TheMainEvent_Capstone.DataAccessLayer;
using TheMainEvent_Capstone.Model;
using Parse;

namespace TheMainEvent_Capstone.Pages
{
	public partial class UserAccountSettingsPage : PhoneApplicationPage
	{
		public UserAccountSettingsPage()
		{
			InitializeComponent();
		}
		private async void SaveSettings()
		{
			string email = this.merchantBox.Text;
			string twitter = this.twitterBox.Text;
			UserDAL ud = new UserDAL();
			UserInfo ui = await ud.GetUserInfo(ParseUser.CurrentUser.ObjectId);
			ui.MerchantEmail = email;
			ud.UpdateUserInfo(ui);
		}
		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			this.SaveSettings();
		}

		private void saveButton_Click(object sender, RoutedEventArgs e)
		{
			this.SaveSettings();
		}
	}
}