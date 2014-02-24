using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TheMainEvent_Capstone.Resources;
using System.Device.Location;
using TheMainEvent_Capstone.Model;
using Parse;
using TheMainEvent_Capstone.DataAccessLayer;
using System.Threading.Tasks;

namespace TheMainEvent_Capstone
{
	public partial class MainPage : PhoneApplicationPage
	{

		// Constructor
		public MainPage()
		{
			InitializeComponent();
			// Sample code to localize the ApplicationBar
			//BuildLocalizedApplicationBar();
		}
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (ParseUser.CurrentUser != null)
			{
				ParseUser.LogOut();
				//NavigationService.Navigate(new Uri("Pages/MainPages.xaml", UriKind.Relative));
			}
		}
		//Set this method to async
		private async void Login_Click(object sender, RoutedEventArgs e)
		{
			string username = usernameBox.Text;
			string password = passwordBox.Password;
			username = "Timonium";
			password = "CorrectHorse1";

			if (await this.LoginUser(username, password))
			{
				//NavigationService.Navigate(new Uri("/Pages/MainPages.xaml", UriKind.Relative));
				NavigationService.Navigate(new Uri("/Pages/EventPanorama.xaml", UriKind.Relative));
			}
			else
			{
				NavigationService.Navigate(new Uri("MainPage.xaml?msg=" + "Please provide a valid username and password or Register an Account", UriKind.Relative));
			}
		}

		private void usernameBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			PasswordBox box = (PasswordBox)sender;
			if (box.Password.Equals("Password"))
			{
				box.Password = "";
			}
		}

		private void registrationButton_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/RegistrationPage.xaml", UriKind.Relative));
		}
		private async Task<bool> LoginUser(string username, string password)
		{
			bool output = false;
			try
			{
				await ParseUser.LogInAsync(username, password);
				output = true;
			}
			catch (Exception ex)
			{

				output = false;
			}
			return output;
		}

		private void usernameBox_GotFocus(object sender, RoutedEventArgs e)
		{
			this.descGotFocusStoryboard.Begin();
		}































		// Sample code for building a localized ApplicationBar
		//private void BuildLocalizedApplicationBar()
		//{
		//    // Set the page's ApplicationBar to a new instance of ApplicationBar.
		//    ApplicationBar = new ApplicationBar();

		//    // Create a new button and set the text value to the localized string from AppResources.
		//    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
		//    appBarButton.Text = AppResources.AppBarButtonText;
		//    ApplicationBar.Buttons.Add(appBarButton);

		//    // Create a new menu item with the localized string from AppResources.
		//    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
		//    ApplicationBar.MenuItems.Add(appBarMenuItem);
		//}



	}
}