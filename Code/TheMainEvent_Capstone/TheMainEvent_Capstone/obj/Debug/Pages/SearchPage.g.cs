﻿#pragma checksum "C:\Users\Tim\TheMainEvent\Code\TheMainEvent_Capstone\TheMainEvent_Capstone\Pages\SearchPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "798DAD19FDFD1647B16A3163CFD736B0"
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
    
    
    public partial class Page1 : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock statusBox;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBox searchBox;
        
        internal System.Windows.Controls.Button searchButton;
        
        internal System.Windows.Controls.ProgressBar loadingBar;
        
        internal System.Windows.Controls.StackPanel noEventResults;
        
        internal Microsoft.Phone.Controls.LongListSelector EventsList;
        
        internal System.Windows.Controls.StackPanel noUserResults;
        
        internal Microsoft.Phone.Controls.LongListSelector UserList;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TheMainEvent_Capstone;component/Pages/SearchPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.statusBox = ((System.Windows.Controls.TextBlock)(this.FindName("statusBox")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.searchBox = ((System.Windows.Controls.TextBox)(this.FindName("searchBox")));
            this.searchButton = ((System.Windows.Controls.Button)(this.FindName("searchButton")));
            this.loadingBar = ((System.Windows.Controls.ProgressBar)(this.FindName("loadingBar")));
            this.noEventResults = ((System.Windows.Controls.StackPanel)(this.FindName("noEventResults")));
            this.EventsList = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("EventsList")));
            this.noUserResults = ((System.Windows.Controls.StackPanel)(this.FindName("noUserResults")));
            this.UserList = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("UserList")));
            this.eventsNav = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("eventsNav")));
            this.contactsNav = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("contactsNav")));
            this.searchNav = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("searchNav")));
            this.logutNav = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("logutNav")));
        }
    }
}

