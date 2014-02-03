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

				if (!password.Equals(confirmPassword)) throw new Exception("Passwords must match.");
				if (password.Equals("password", StringComparison.OrdinalIgnoreCase)) throw new Exception("Your password must not be \"password\".");

				string email = emailBox.Text;
				string username = usernameBox.Text;
				UserDAL ud = new UserDAL();
				ud.CreateUser(new User() { Email = email, Username = username, Password = password });
				NavigationService.Navigate(new Uri("/RegistrationInfoPage.xaml", UriKind.Relative));
			}
			catch (Exception ex)
			{
				NavigationService.Navigate(new Uri("/RegistrationPage.xaml?msg=" + ex.Message, UriKind.Relative));
			}
		}
	}
}