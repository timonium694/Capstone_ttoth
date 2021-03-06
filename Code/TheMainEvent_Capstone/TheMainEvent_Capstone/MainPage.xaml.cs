﻿using System;
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

namespace TheMainEvent_Capstone
{
	public partial class MainPage : PhoneApplicationPage
	{
		Event test = new Event() { Title = "test", Address = "London", Date = DateTime.Now, Description = "Come have fun", OtherDetails = "Test event" };

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
				NavigationService.Navigate(new Uri("Pages/MainPages.xaml", UriKind.Relative));
			}
		}
		//Set this method to async
		private void Login_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/CreateEvent.xaml", UriKind.Relative));
			//try
			//{
			//	string username = usernameBox.Text;
			//	string password = passwordBox.Password;
			//	//await ParseUser.LogInAsync(username, password);
			//}
			//catch (Exception ex)
			//{
			//	NavigationService.Navigate(new Uri("MainPage.xaml?msg="+ex.Message, UriKind.Relative));
			//}
			//NavigationService.Navigate(new Uri("/Pages/MainPages.xaml", UriKind.Relative));
		}

		private void usernameBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			TextBox box = (TextBox)sender;
			if (box.Text.Equals("Username"))
			{
				box.Text = "";
			}
		}

		private void registrationButton_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/RegistrationPage.xaml", UriKind.Relative));
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