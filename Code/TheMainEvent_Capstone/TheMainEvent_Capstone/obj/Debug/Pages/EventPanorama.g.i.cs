﻿#pragma checksum "C:\Users\Tim\TheMainEvent\Code\TheMainEvent_Capstone\TheMainEvent_Capstone\Pages\EventPanorama.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6341C12B52083AB1B1AC00261273B0A6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace TheMainEvent_Capstone.Pages {
    
    
    public partial class EventPanorama : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Media.Animation.Storyboard tweetGotFocusStoryboard;
        
        internal System.Windows.Media.Animation.Storyboard tweetLostFocusStoryboard;
        
        internal Microsoft.Phone.Shell.ApplicationBar defaultBar;
        
        internal Microsoft.Phone.Shell.ApplicationBar IsOwnerAppBar;
        
        internal Microsoft.Phone.Shell.ApplicationBar IsAttendingAppBar;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock statusBlock;
        
        internal Microsoft.Phone.Controls.Panorama rootPanorama;
        
        internal System.Windows.Controls.StackPanel EventData;
        
        internal System.Windows.Controls.TextBlock dateBlock;
        
        internal System.Windows.Controls.TextBlock timeBlock;
        
        internal System.Windows.Controls.TextBlock costBlock;
        
        internal System.Windows.Controls.TextBlock addressBlock;
        
        internal System.Windows.Controls.TextBlock descriptionBlock;
        
        internal System.Windows.Controls.TextBlock detailsBlock;
        
        internal Microsoft.Phone.Controls.PanoramaItem TweetSection;
        
        internal System.Windows.Controls.Grid tweetGrid;
        
        internal System.Windows.Controls.StackPanel tweetPanel;
        
        internal Microsoft.Phone.Controls.PhoneTextBox tweetBox;
        
        internal System.Windows.Controls.Button tweetButton;
        
        internal System.Windows.Controls.Button authorizeTweets;
        
        internal Microsoft.Phone.Maps.Controls.Map Map;
        
        internal Microsoft.Phone.Maps.Toolkit.MapItemsControl RestaurantItems;
        
        internal Microsoft.Phone.Controls.ExpanderView attendingExpander;
        
        internal Microsoft.Phone.Controls.LongListSelector AttendingList;
        
        internal Microsoft.Phone.Controls.ExpanderView invitedExpander;
        
        internal Microsoft.Phone.Controls.LongListSelector InvitedList;
        
        internal Microsoft.Phone.Controls.ExpanderView paidExpander;
        
        internal Microsoft.Phone.Controls.LongListSelector PaidList;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/TheMainEvent_Capstone;component/Pages/EventPanorama.xaml", System.UriKind.Relative));
            this.tweetGotFocusStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("tweetGotFocusStoryboard")));
            this.tweetLostFocusStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("tweetLostFocusStoryboard")));
            this.defaultBar = ((Microsoft.Phone.Shell.ApplicationBar)(this.FindName("defaultBar")));
            this.IsOwnerAppBar = ((Microsoft.Phone.Shell.ApplicationBar)(this.FindName("IsOwnerAppBar")));
            this.IsAttendingAppBar = ((Microsoft.Phone.Shell.ApplicationBar)(this.FindName("IsAttendingAppBar")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.statusBlock = ((System.Windows.Controls.TextBlock)(this.FindName("statusBlock")));
            this.rootPanorama = ((Microsoft.Phone.Controls.Panorama)(this.FindName("rootPanorama")));
            this.EventData = ((System.Windows.Controls.StackPanel)(this.FindName("EventData")));
            this.dateBlock = ((System.Windows.Controls.TextBlock)(this.FindName("dateBlock")));
            this.timeBlock = ((System.Windows.Controls.TextBlock)(this.FindName("timeBlock")));
            this.costBlock = ((System.Windows.Controls.TextBlock)(this.FindName("costBlock")));
            this.addressBlock = ((System.Windows.Controls.TextBlock)(this.FindName("addressBlock")));
            this.descriptionBlock = ((System.Windows.Controls.TextBlock)(this.FindName("descriptionBlock")));
            this.detailsBlock = ((System.Windows.Controls.TextBlock)(this.FindName("detailsBlock")));
            this.TweetSection = ((Microsoft.Phone.Controls.PanoramaItem)(this.FindName("TweetSection")));
            this.tweetGrid = ((System.Windows.Controls.Grid)(this.FindName("tweetGrid")));
            this.tweetPanel = ((System.Windows.Controls.StackPanel)(this.FindName("tweetPanel")));
            this.tweetBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("tweetBox")));
            this.tweetButton = ((System.Windows.Controls.Button)(this.FindName("tweetButton")));
            this.authorizeTweets = ((System.Windows.Controls.Button)(this.FindName("authorizeTweets")));
            this.Map = ((Microsoft.Phone.Maps.Controls.Map)(this.FindName("Map")));
            this.RestaurantItems = ((Microsoft.Phone.Maps.Toolkit.MapItemsControl)(this.FindName("RestaurantItems")));
            this.attendingExpander = ((Microsoft.Phone.Controls.ExpanderView)(this.FindName("attendingExpander")));
            this.AttendingList = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("AttendingList")));
            this.invitedExpander = ((Microsoft.Phone.Controls.ExpanderView)(this.FindName("invitedExpander")));
            this.InvitedList = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("InvitedList")));
            this.paidExpander = ((Microsoft.Phone.Controls.ExpanderView)(this.FindName("paidExpander")));
            this.PaidList = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("PaidList")));
        }
    }
}

