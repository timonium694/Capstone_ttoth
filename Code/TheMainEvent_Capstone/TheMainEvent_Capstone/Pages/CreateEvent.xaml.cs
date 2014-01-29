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

namespace TheMainEvent_Capstone.Pages
{
	public partial class CreateEvent : PhoneApplicationPage
	{
		public CreateEvent()
		{
			InitializeComponent();
		}

		private void OnTextInputStart(object sender, TextCompositionEventArgs e)
		{
			scrollViewer.UpdateLayout();
			scrollViewer.ScrollToVerticalOffset(description.ActualHeight);
		}

		private void TextBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			TextBox box = (TextBox)sender;
			if (box.Text.Equals("Title") || box.Text.Equals("City") || box.Text.Equals("State") || box.Text.Equals("Description") || box.Text.Equals("Street Address") || box.Text.Equals("Other Details"))
			{
				box.Text = "";
			}
		}
		
	}
}