﻿#pragma checksum "C:\Users\Tim\TheMainEvent\Code\TheMainEvent_Capstone\TheMainEvent_Capstone\Pages\Contact.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2C46BAAAB90F6710E3052E4120EC2208"
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
    
    
    public partial class Contact : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock nameBlock;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.StackPanel UserPanel;
        
        internal System.Windows.Controls.TextBlock emailBlock;
        
        internal System.Windows.Controls.TextBlock phoneBlock;
        
        internal System.Windows.Controls.TextBlock bDayBlock;
        
        internal System.Windows.Controls.TextBlock bioblock;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton eventsNav;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton contactsNav;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton searchNav;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton logutNav;
        
        internal Microsoft.Phone.Shell.ApplicationBarMenuItem addContactItem;
        
        internal Microsoft.Phone.Shell.ApplicationBarMenuItem inviteContactItem;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TheMainEvent_Capstone;component/Pages/Contact.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.nameBlock = ((System.Windows.Controls.TextBlock)(this.FindName("nameBlock")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.UserPanel = ((System.Windows.Controls.StackPanel)(this.FindName("UserPanel")));
            this.emailBlock = ((System.Windows.Controls.TextBlock)(this.FindName("emailBlock")));
            this.phoneBlock = ((System.Windows.Controls.TextBlock)(this.FindName("phoneBlock")));
            this.bDayBlock = ((System.Windows.Controls.TextBlock)(this.FindName("bDayBlock")));
            this.bioblock = ((System.Windows.Controls.TextBlock)(this.FindName("bioblock")));
            this.eventsNav = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("eventsNav")));
            this.contactsNav = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("contactsNav")));
            this.searchNav = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("searchNav")));
            this.logutNav = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("logutNav")));
            this.addContactItem = ((Microsoft.Phone.Shell.ApplicationBarMenuItem)(this.FindName("addContactItem")));
            this.inviteContactItem = ((Microsoft.Phone.Shell.ApplicationBarMenuItem)(this.FindName("inviteContactItem")));
        }
    }
}

