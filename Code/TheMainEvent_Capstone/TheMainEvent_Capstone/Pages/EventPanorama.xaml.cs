using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Input;
using System.Collections.ObjectModel;
using TheMainEvent_Capstone.Model.ViewModels;
using TheMainEvent_Capstone.Model;

namespace TheMainEvent_Capstone.Pages
{
	public partial class EventPanorama : PhoneApplicationPage
	{

		private EventViewModel evm;
		private UserInfo owner;
		private UserInfo currentUser;
		private bool IsOwner = false;
		private bool IsAttending = false;
		ObservableCollection<ContactViewModel> InvitedUsers = new ObservableCollection<ContactViewModel>();
		ObservableCollection<ContactViewModel> AttendingUsers = new ObservableCollection<ContactViewModel>();

		public EventPanorama()
		{
			InitializeComponent();
		}
		private void OnTextInputStart(object sender, TextCompositionEventArgs e)
		{
			scrollViewer.UpdateLayout();
			scrollViewer.ScrollToVerticalOffset(tweetBox.ActualHeight);
		}
		private void attendButton_Click(object sender, RoutedEventArgs e)
		{
			//if (owner.MerchantEmail.Equals("none"))
			//{
			//	EventDAL ed = new EventDAL();
			//	ed.AddAttendee(evm.ID, currentUser.User);
			//}
			//else if (!owner.MerchantEmail.Equals("none"))
			//{
			//	this.PayForEvent(owner.MerchantEmail);
			//	EventDAL ed = new EventDAL();
			//	ed.AddAttendee(evm.ID, currentUser.User);
			//}

		}

		private void tweetBox_GotFocus(object sender, RoutedEventArgs e)
		{

		}
		private void tweetBox_LostFocus(object sender, RoutedEventArgs e)
		{

		}

		private void EventsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}
	}
}