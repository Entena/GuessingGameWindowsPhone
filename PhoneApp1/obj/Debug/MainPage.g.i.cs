﻿#pragma checksum "C:\tmp\bluetoothchat-wp8-winrt-master\BluetoothChatWP8\PhoneApp1\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "BD9DBA7F4B8275E12F9B8ECE86120EC6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
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


namespace PhoneApp1 {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Button btnSend;
        
        internal System.Windows.Controls.TextBlock lblYourName;
        
        internal System.Windows.Controls.TextBlock lblOtherPlayerName;
        
        internal System.Windows.Controls.TextBlock yourAns;
        
        internal System.Windows.Controls.TextBlock theirAns;
        
        internal System.Windows.Controls.TextBox yourAnswerInput;
        
        internal System.Windows.Controls.TextBlock lblStatus;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/PhoneApp1;component/MainPage.xaml", System.UriKind.Relative));
            this.btnSend = ((System.Windows.Controls.Button)(this.FindName("btnSend")));
            this.lblYourName = ((System.Windows.Controls.TextBlock)(this.FindName("lblYourName")));
            this.lblOtherPlayerName = ((System.Windows.Controls.TextBlock)(this.FindName("lblOtherPlayerName")));
            this.yourAns = ((System.Windows.Controls.TextBlock)(this.FindName("yourAns")));
            this.theirAns = ((System.Windows.Controls.TextBlock)(this.FindName("theirAns")));
            this.yourAnswerInput = ((System.Windows.Controls.TextBox)(this.FindName("yourAnswerInput")));
            this.lblStatus = ((System.Windows.Controls.TextBlock)(this.FindName("lblStatus")));
        }
    }
}

