﻿#pragma checksum "C:\Users\Tim\TheMainEvent\Code\TheMainEvent_Capstone\TheMainEvent_Capstone\Pages\RegistrationInfoPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "BBF7056BE3AC8EC3FAADB33A0C1ACED1"
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
    
    
    public partial class RegistrationInfoPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ScrollViewer ContentScroll;
        
        internal System.Windows.Controls.TextBox firstNameBox;
        
        internal System.Windows.Controls.TextBox lastNameBox;
        
        internal System.Windows.Controls.TextBox phoneBox;
        
        internal System.Windows.Controls.ScrollViewer scrollViewer;
        
        internal Microsoft.Phone.Controls.PhoneTextBox bioBox;
        
        internal Microsoft.Phone.Controls.DatePicker datePicker;
        
        internal System.Windows.Controls.Button doneButton;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TheMainEvent_Capstone;component/Pages/RegistrationInfoPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentScroll = ((System.Windows.Controls.ScrollViewer)(this.FindName("ContentScroll")));
            this.firstNameBox = ((System.Windows.Controls.TextBox)(this.FindName("firstNameBox")));
            this.lastNameBox = ((System.Windows.Controls.TextBox)(this.FindName("lastNameBox")));
            this.phoneBox = ((System.Windows.Controls.TextBox)(this.FindName("phoneBox")));
            this.scrollViewer = ((System.Windows.Controls.ScrollViewer)(this.FindName("scrollViewer")));
            this.bioBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("bioBox")));
            this.datePicker = ((Microsoft.Phone.Controls.DatePicker)(this.FindName("datePicker")));
            this.doneButton = ((System.Windows.Controls.Button)(this.FindName("doneButton")));
        }
    }
}

