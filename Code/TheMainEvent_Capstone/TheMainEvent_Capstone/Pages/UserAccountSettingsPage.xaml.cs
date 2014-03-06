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
		UserInfo ui;
		UserDAL ud;
		public UserAccountSettingsPage()
		{
			InitializeComponent();
			
		}
		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			ud = new UserDAL();
			ui = await ud.GetUserInfo(ParseUser.CurrentUser.ObjectId);
		}
		private void SaveSettings()
		{
			string email = this.merchantBox.Text;
			
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

		private void ListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			TextBlock t = (TextBlock)filterPicker.SelectedItem;
			ui.FilterMode = t.Text;
		}
	}
}