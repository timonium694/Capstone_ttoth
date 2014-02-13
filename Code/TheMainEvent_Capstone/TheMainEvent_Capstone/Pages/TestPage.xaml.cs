using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TweetSharp.Model;
using TweetSharp.Serialization;
using TweetSharp;

namespace TheMainEvent_Capstone.Pages
{
	public partial class Page1 : PhoneApplicationPage
	{
		public Page1()
		{
			InitializeComponent();
		}

		private void tweet_Click(object sender, RoutedEventArgs e)
		{

			//Obtain keys by registering your app on https://dev.twitter.com/apps
			var service = new TwitterService("abc", "abc");
			service.AuthenticateWith("abc", "abc");

			//ScreenName is the profile name of the twitter user.
			//service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions() { ScreenName = "the_appfactory" }, (ts, rep) =>
			//{
			//	if (rep.StatusCode == HttpStatusCode.OK)
			//	{
			//		//bind
			//		this.Dispatcher.BeginInvoke(() => { tweetList.ItemsSource = ts; });
			//	}
			//});
		}
	}
}