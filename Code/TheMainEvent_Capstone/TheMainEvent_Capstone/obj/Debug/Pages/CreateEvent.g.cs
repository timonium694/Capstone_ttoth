﻿#pragma checksum "C:\Users\Tim\TheMainEvent\Code\TheMainEvent_Capstone\TheMainEvent_Capstone\Pages\CreateEvent.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "169E3106D6E9F6433F0E777BA21C9FE7"
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
    
    
    public partial class CreateEvent : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Media.Animation.Storyboard descGotFocusStoryboard;
        
        internal System.Windows.Media.Animation.Storyboard otherGotFocusStoryboard;
        
        internal System.Windows.Media.Animation.Storyboard descLostFocusStoryboard;
        
        internal System.Windows.Media.Animation.Storyboard otherLostFocusStoryboard;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock statusBlock;
        
        internal System.Windows.Controls.StackPanel ContentPanel;
        
        internal System.Windows.Controls.TextBlock titleBlock;
        
        internal Microsoft.Phone.Controls.PhoneTextBox titleBox;
        
        internal Microsoft.Phone.Controls.PhoneTextBox addressBox;
        
        internal Microsoft.Phone.Controls.PhoneTextBox cityBox;
        
        internal Microsoft.Phone.Controls.PhoneTextBox stateBox;
        
        internal Microsoft.Phone.Controls.PhoneTextBox descriptionBox;
        
        internal Microsoft.Phone.Controls.DatePicker datePicker;
        
        internal Microsoft.Phone.Controls.TimePicker timePicker;
        
        internal Microsoft.Phone.Controls.PhoneTextBox otherDetailsBox;
        
        internal System.Windows.Controls.TextBlock costStatus;
        
        internal System.Windows.Controls.StackPanel costPanel;
        
        internal Microsoft.Phone.Controls.PhoneTextBox costBox;
        
        internal System.Windows.Controls.CheckBox donationCheck;
        
        internal Microsoft.Phone.Controls.ExpanderView inviteExpander;
        
        internal Microsoft.Phone.Controls.LongListMultiSelector contacts;
        
        internal Microsoft.Phone.Controls.ListPicker typePicker;
        
        internal System.Windows.Controls.Button createButton;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton eventsNav;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton contactsNav;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton searchNav;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton logutNav;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TheMainEvent_Capstone;component/Pages/CreateEvent.xaml", System.UriKind.Relative));
            this.descGotFocusStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("descGotFocusStoryboard")));
            this.otherGotFocusStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("otherGotFocusStoryboard")));
            this.descLostFocusStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("descLostFocusStoryboard")));
            this.otherLostFocusStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("otherLostFocusStoryboard")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.statusBlock = ((System.Windows.Controls.TextBlock)(this.FindName("statusBlock")));
            this.ContentPanel = ((System.Windows.Controls.StackPanel)(this.FindName("ContentPanel")));
            this.titleBlock = ((System.Windows.Controls.TextBlock)(this.FindName("titleBlock")));
            this.titleBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("titleBox")));
            this.addressBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("addressBox")));
            this.cityBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("cityBox")));
            this.stateBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("stateBox")));
            this.descriptionBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("descriptionBox")));
            this.datePicker = ((Microsoft.Phone.Controls.DatePicker)(this.FindName("datePicker")));
            this.timePicker = ((Microsoft.Phone.Controls.TimePicker)(this.FindName("timePicker")));
            this.otherDetailsBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("otherDetailsBox")));
            this.costStatus = ((System.Windows.Controls.TextBlock)(this.FindName("costStatus")));
            this.costPanel = ((System.Windows.Controls.StackPanel)(this.FindName("costPanel")));
            this.costBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("costBox")));
            this.donationCheck = ((System.Windows.Controls.CheckBox)(this.FindName("donationCheck")));
            this.inviteExpander = ((Microsoft.Phone.Controls.ExpanderView)(this.FindName("inviteExpander")));
            this.contacts = ((Microsoft.Phone.Controls.LongListMultiSelector)(this.FindName("contacts")));
            this.typePicker = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("typePicker")));
            this.createButton = ((System.Windows.Controls.Button)(this.FindName("createButton")));
            this.eventsNav = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("eventsNav")));
            this.contactsNav = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("contactsNav")));
            this.searchNav = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("searchNav")));
            this.logutNav = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("logutNav")));
        }
    }
}

