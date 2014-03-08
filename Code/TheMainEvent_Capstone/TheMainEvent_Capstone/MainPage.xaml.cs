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
using System.Windows.Media.Imaging;

namespace TheMainEvent_Capstone
{
	public partial class MainPage : PhoneApplicationPage
	{
		BitmapImage i;
		// Constructor
		public MainPage()
		{

			i = new BitmapImage(new Uri("Assets/proDefault.jpg", UriKind.Relative));

			InitializeComponent();
			// Sample code to localize the ApplicationBar
			//BuildLocalizedApplicationBar();
		}
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{

			us.Source = i;
			string msg = "";
			if (NavigationContext.QueryString.TryGetValue("msg", out msg))
			{
				this.errorBox.Text = msg;
			}
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

			await this.LoginUser(username, password);
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
		private async Task LoginUser(string username, string password)
		{
			try
			{
				await ParseUser.LogInAsync(username, password);
				NavigationService.Navigate(new Uri("/Pages/SearchPage.xaml", UriKind.Relative));
			}
			catch (Exception ex)
			{
				this.errorBox.Text = "Please provide a valid username and password or Register an Account";
				//NavigationService.Navigate(new Uri("MainPage.xaml?msg=" + "Please provide a valid username and password or Register an Account", UriKind.Relative));
			}
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