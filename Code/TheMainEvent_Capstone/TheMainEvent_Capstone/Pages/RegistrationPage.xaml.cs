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
	public partial class RegistrationPage : PhoneApplicationPage
	{
		public RegistrationPage()
		{
			InitializeComponent();
		}

		private void Continue_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string password = passwordBox.Password;
				string confirmPassword = confirmPasswordBox.Password;
				string email = emailBox.Text;
				string username = usernameBox.Text;
			}
			catch (Exception ex)
			{
				NavigationService.Navigate(new Uri("/RegistrationPage.xaml?msg=" + ex.Message, UriKind.Relative));
			}
		}
	}
}