using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace TheMainEvent_Capstone.Pages
{
	public partial class UserAccountSettingsPage : PhoneApplicationPage
	{
		public UserAccountSettingsPage()
		{
			InitializeComponent();
		}
		private void SaveSettings()
		{
			string email = this.merchantBox.Text;
			string twitter = this.twitterBox.Text;
		}
		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			this.SaveSettings();
		}
	}
}