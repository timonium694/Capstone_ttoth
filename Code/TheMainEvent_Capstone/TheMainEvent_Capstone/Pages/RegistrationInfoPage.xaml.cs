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

		private async void doneButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{

				if (this.isValidPhone)
				{
					if (!string.IsNullOrEmpty(firstNameBox.Text) && !string.IsNullOrEmpty(lastNameBox.Text) && !string.IsNullOrEmpty(bioBox.Text))
					{
						string phoneNumber = "";
					int count = 0;
					foreach (char s in phoneBox.Text.ToCharArray())
					{

						phoneNumber = s + "";
						if (count == 2 || count == 5)
						{
							phoneNumber += "-";
						}
						count++;
					}
					DateTime bday = (DateTime)datePicker.Value;
					string firstName = this.firstNameBox.Text;
					string lastName = this.lastNameBox.Text;
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
					};
					UserDAL ud = new UserDAL();

					byte[] data = ud.ConvertToBytes(i);
					ui.ProfilePic = data;
					await ud.UpdateUserInfo(ui);
					}
					
				}
				else
				{
					MessageBox.Show("You must have a valid phone number.");
				}

			}
			catch (Exception ex)
			{
			}
		}

		private void phoneBox_TextChanged(object sender, TextChangedEventArgs e)
		{

			string input = phoneBox.Text;
			string pattern = @"^((\d){9})$";
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