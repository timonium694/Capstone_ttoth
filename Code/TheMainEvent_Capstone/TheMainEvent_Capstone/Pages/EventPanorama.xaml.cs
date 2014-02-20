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
using TheMainEvent_Capstone.DataAccessLayer;
using Parse;
using System.Threading.Tasks;
using PayPal.Checkout;

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


		//Have event editing by making everything clickable and then having a message box come up with a field for editing.
		public EventPanorama()
		{
			InitializeComponent();
			this.Loaded += this.Page_Loaded;
		}

		async void Page_Loaded(object sender, RoutedEventArgs e)
		{
			await this.LoadData();
		}

		private async Task LoadData()
		{
			EventDAL ed = new EventDAL();
			rootPanorama.Title = evm.Title;
			addressBlock.Text = evm.Address;
			if (evm.Cost.Equals(0.00))
				costBlock.Text = "FREE";
			else
				costBlock.Text = "$" + evm.Cost;

			descriptionBlock.Text = evm.Description;
			detailsBlock.Text = evm.OtherDetails;
			dateBlock.Text = evm.Date.ToLocalTime().ToShortDateString();
			timeBlock.Text = evm.Time.ToLocalTime().ToShortTimeString();
			UserDAL ud = new UserDAL();
			string currentId = ParseUser.CurrentUser.ObjectId;
			currentUser = await ud.GetUserInfo(currentId);
			string ownerId = await ed.GetOwner(evm.ID);
			owner = await ud.GetUserInfo(ownerId);
			List<string> users = await ed.GetAttendees(evm.ID);
			if (currentUser.Equals(owner))
			{
				IsOwner = true;
			}
			foreach (string id in users)
			{
				UserInfo s = await ud.GetUserInfo(id);
				AttendingUsers.Add(new ContactViewModel()
				{
					Name = s.FirstName + " " + s.LastName,
					User = s.User,
				});
				if (currentUser.Equals(s))
				{
					IsAttending = true;
				}
			}
			if (this.IsOwner || this.IsAttending)
			{
				this.attendButton.Visibility = Visibility.Collapsed;
			}
			evm.Cost = 30;
		}
		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			string msg = "";
			if (NavigationContext.QueryString.TryGetValue("msg", out msg))
			{
				EventDAL ed = new EventDAL();
				Event ev = await ed.RetrieveEvent(msg);
				this.evm = new EventViewModel()
				{
					Title = ev.Title,
					Cost = ev.Cost,
					Address = ev.Address + ", " + ev.City + ", " + ev.State,
					Description = ev.Description,
					OtherDetails = "Details: " + ev.OtherDetails,
					Date = ev.Date,
					Type = ev.Type.ToString(),
					ID = ev.ID,

				};
			}
		}
		private void attendButton_Click(object sender, RoutedEventArgs e)
		{

			//Testing 
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

		private async void PayForEvent(string merchant)
		{
			try
			{
				// Create a BuyNow object with your PayPal 
				// merchant account emailid. Remember to onboard
				// your merchant account by granting third party 
				// permissions as detailed in the docs	
				BuyNow bn = new BuyNow(merchant);
				bn.UseSandbox = true;

				ItemBuilder builder = new ItemBuilder(evm.Title).Description(evm.Description).Price(evm.Cost.ToString());
				bn.AddItem(builder);

				// Attach event handlers. The BuyNow class emits 5 events
				// start, auth, error, cancel and complete
				bn.Start += new EventHandler<PayPal.Checkout.Event.StartEventArgs>((source, args) =>
				{
					this.statusBlock.Text = "Initiating payment";
				});
				bn.Auth += new EventHandler<PayPal.Checkout.Event.AuthEventArgs>((source, args) =>
				{
					this.statusBlock.Text = "Authenticating payment: " + args.Token;
				});
				bn.Complete += new EventHandler<PayPal.Checkout.Event.CompleteEventArgs>((source, args) =>
				{
					this.statusBlock.Text = "Your payment is complete. Transaction id: " + args.TransactionID;
				});
				bn.Cancel += new EventHandler<PayPal.Checkout.Event.CancelEventArgs>((source, args) =>
				{
					this.statusBlock.Text = "Payment Cancelled";
				});
				bn.Error += new EventHandler<PayPal.Checkout.Event.ErrorEventArgs>((source, args) =>
				{
					this.statusBlock.Text = "There was an error processing your payment: " + args.Type + " " + args.Message;
				});

				// Ready to go. Call the asynchronous Execute operation
				// to initiate the payment. Remember to mark your calling
				// function with the async keyword since this is an async call
				bool ret = await bn.Execute();
				NavigationService.Navigate(new Uri("/Pages/EventPage.xaml?msg=" + evm.ID, UriKind.Relative));
			}
			catch (Exception ex)
			{
				this.statusBlock.Text = ex.Message;
			}
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