﻿#pragma checksum "C:\Users\Tim\TheMainEvent\Code\TheMainEvent_Capstone\TheMainEvent_Capstone\Pages\OAuth.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D56E923CE3784A4602938F6E94BC6849"
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


namespace TheMainEvent_Capstone {
    
    
    public partial class OAuth : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal Microsoft.Phone.Controls.WebBrowser OAuthWebBrowser;
        
        internal System.Windows.Controls.TextBlock EnterPinTextBlock;
        
        internal System.Windows.Controls.TextBox PinTextBox;
        
        internal System.Windows.Controls.Button AuthorizationButton;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TheMainEvent_Capstone;component/Pages/OAuth.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.OAuthWebBrowser = ((Microsoft.Phone.Controls.WebBrowser)(this.FindName("OAuthWebBrowser")));
            this.EnterPinTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("EnterPinTextBlock")));
            this.PinTextBox = ((System.Windows.Controls.TextBox)(this.FindName("PinTextBox")));
            this.AuthorizationButton = ((System.Windows.Controls.Button)(this.FindName("AuthorizationButton")));
        }
    }
}
