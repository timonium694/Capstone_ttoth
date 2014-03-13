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
using System.Collections;
using TheMainEvent_Capstone.DataAccessLayer;
using System.Collections.ObjectModel;

namespace TheMainEvent_Capstone.Pages
{
	public partial class NotificationsPage : PhoneApplicationPage
	{
		ApplicationBarIconButton select;
		ApplicationBarIconButton delete;
		ApplicationBarMenuItem eventsNav;
		ApplicationBarMenuItem contactsNav;
		ApplicationBarMenuItem searchNav;
		ApplicationBarMenuItem logoutNav;
		UserInfo cur;
		ObservableCollection<Notification> notes = new ObservableCollection<Notification>();
		public NotificationsPage()
		{
			InitializeComponent();
			this.Loaded += Page_Loaded;
		}
		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			NotificationDAL nd = new NotificationDAL();
			List<Notification> ns = await nd.GetNotifications(ParseUser.CurrentUser.ObjectId);
			ns.OrderBy(x => x.Date);
			foreach (Notification n in ns)
			{
				notes.Add(n);
			}
			this.NotificationList.ItemsSource = notes;
			this.SetupAppBar();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			this.CreateAppBar();
		}
		private void CreateAppBar()
		{
			select = new ApplicationBarIconButton();
			select.IconUri = new Uri("/Toolkit.Content/ApplicationBar.Select.png", UriKind.RelativeOrAbsolute);
			select.Text = "select";
			select.Click += OnSelectClick;

			delete = new ApplicationBarIconButton();
			delete.IconUri = new Uri("/Toolkit.Content/ApplicationBar.Delete.png", UriKind.RelativeOrAbsolute);
			delete.Text = "delete";
			delete.Click += OnDeleteClick;

			eventsNav = new ApplicationBarMenuItem();
			eventsNav.Text = "events";
			eventsNav.Click += this.eventsNav_Click;

			searchNav = new ApplicationBarMenuItem();
			searchNav.Text = "search";
			searchNav.Click += this.searchNav_Click;

			contactsNav = new ApplicationBarMenuItem();
			contactsNav.Text = "contacts";
			contactsNav.Click += this.contactsNav_Click;

			logoutNav = new ApplicationBarMenuItem();
			logoutNav.Text = "logout";
			logoutNav.Click += this.contactsNav_Click;
		}


		void ClearApplicationBar()
		{
			while (ApplicationBar.Buttons.Count > 0)
			{
				ApplicationBar.Buttons.RemoveAt(0);
			}
			while (ApplicationBar.MenuItems.Count > 0)
			{
				ApplicationBar.MenuItems.RemoveAt(0);
			}
		}

		async void OnDeleteClick(object sender, EventArgs e)
		{
			NotificationDAL nd = new NotificationDAL();	
			IList source = NotificationList.ItemsSource as IList;
			while (NotificationList.SelectedItems.Count > 0)
			{
				Notification n = (Notification)NotificationList.SelectedItems[0];
				await nd.DeleteNotification(n.ID);
				source.Remove(n);
			}
			
		}

		void OnSelectClick(object sender, EventArgs e)
		{
			NotificationList.IsSelectionEnabled = true;
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

		private void SetupAppBar()
		{
			this.ClearApplicationBar();
			if (NotificationList.IsSelectionEnabled)
			{
				ApplicationBar.Buttons.Add(delete);
				UpdateApplicationBar();
			}
			else
			{
				ApplicationBar.Buttons.Add(select);
			}
			ApplicationBar.MenuItems.Add(this.eventsNav);
			ApplicationBar.MenuItems.Add(this.contactsNav);
			ApplicationBar.MenuItems.Add(this.searchNav);
			ApplicationBar.MenuItems.Add(this.logoutNav);
			ApplicationBar.IsVisible = true;
		}

		private void UpdateApplicationBar()
		{
			if (NotificationList.IsSelectionEnabled)
			{
				bool hasSelection = ((NotificationList.SelectedItems != null) && (NotificationList.SelectedItems.Count > 0));
				this.delete.IsEnabled = hasSelection;
			}
		}

		private void NotificationList_IsSelectionEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.SetupAppBar();
			this.UpdateApplicationBar();
		}

		private void NotificationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.UpdateApplicationBar();
		}
	}
}