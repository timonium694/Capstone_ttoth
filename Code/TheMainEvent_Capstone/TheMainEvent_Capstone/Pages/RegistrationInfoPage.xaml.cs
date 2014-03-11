using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TheMainEvent_Capstone.Model;
using TheMainEvent_Capstone.DataAccessLayer;
using Parse;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;

namespace TheMainEvent_Capstone.Pages
{
	public partial class RegistrationInfoPage : PhoneApplicationPage
	{
		BitmapImage i;
		bool isValidPhone = false;
		public RegistrationInfoPage()
		{
			InitializeComponent();
			phoneBox.Tap += this.TextBox_Tap;
			lastNameBox.Tap += this.TextBox_Tap;
			firstNameBox.Tap += this.TextBox_Tap;
			bioBox.Tap += this.TextBox_Tap;
			i = new BitmapImage(new Uri("Assets/proDefault.jpg", UriKind.Relative));
			us.Source = i;

			
		}
		private void TextBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			TextBox box = (TextBox)sender;
			if (box.Text.Equals("First Name") || box.Text.Equals("Last Name") || box.Text.Equals("Bio") || box.Text.Equals("Phone Number"))
			{
				box.Text = "";
			}
		}

		private void doneButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{

				if (this.isValidPhone)
				{
					UserDAL ud = new UserDAL();
					byte[] bytes = ud.ConvertToBytes(i);
					DateTime bday = (DateTime)datePicker.Value;
					string firstName = this.firstNameBox.Text;
					string lastName = this.lastNameBox.Text;
					string input = this.phoneBox.Text;
					string phoneNumber = "";
					for (int u = 0; u < 9; u++ )
					{
						phoneNumber += input.ToCharArray()[u];
						if (u == 2 || u == 5)
						{
							phoneNumber += "-";
						}
					}
					string bio = this.bioBox.Text;
					UserInfo ui = new UserInfo()
					{
						FirstName = firstName,
						LastName = lastName,
						Phone = phoneNumber,
						Bio = bio,
						Birthday = bday,
						User = ParseUser.CurrentUser.ObjectId,
						MerchantEmail = "tim.toth13@gmail.com",
						Email = ParseUser.CurrentUser.Email,
						ProfilePic = bytes
					};
					ud.CreateUserInfo(ui);
				}

			}
			catch (Exception ex)
			{
			}
		}

		private void phoneBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			
			string input = phoneBox.Text;
			if (input.EndsWith("-"))
			{
				input.Remove(input.Length-1);
			}
			if (Regex.IsMatch(input, @"^((\d){3})$"))
			{
			}
			if (Regex.IsMatch(input, @"^((\d){3})-((\d){3})$"))
			{
			}
			string pattern = @"^((\d){3})-((\d){3})-((\d){4})$";
			if (!Regex.IsMatch(input, pattern))
			{
				phoneStatus.Visibility = Visibility.Visible;
				isValidPhone = false;
			}
			else
			{
				this.isValidPhone = true;
				phoneStatus.Visibility = Visibility.Collapsed;
			}
		}

		private void bioBox_GotFocus(object sender, RoutedEventArgs e)
		{
			bioGotFocusStoryboard.Begin();
		}

		private void bioBox_LostFocus(object sender, RoutedEventArgs e)
		{
			bioLostFocusStoryboard.Begin();
		}
	}
}