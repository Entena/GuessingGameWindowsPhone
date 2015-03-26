using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace PhoneApp1 {
    public partial class MainPage : PhoneApplicationPage {
        public MainPage() {
            InitializeComponent();
            SetupAppBar();
            btnSend.IsEnabled = false;
            ChatConnection.ConnectionStateChanged += ChatConnection_ConnectionStateChanged;
        }

        public void btnConnect_Click(object sender, EventArgs e) {

        }

        public void btnSend_Click(object sender, EventArgs e) {
            if(yourAnswerInput.Text.Length > 0) {
                yourAns.Text = yourAnswerInput.Text;
                ChatConnection.SendChatMessageAsync("Turn "+yourAnswerInput.Text);
            }
        }

        void ChatConnection_ConnectionStateChanged(object sender, ConnectionStateChangedEventArgs e) {
            switch(e.State) {
                case ChatConnection.STATE_CONNECTED:
                    updateStatus(Constants.CONNECTED + ChatConnection.peer);
                    lblOtherPlayerName.Text = ChatConnection.peer+":";
                    btnSend.IsEnabled = true;
                    break;
                case ChatConnection.STATE_DISCONNECTED:
                    updateStatus(Constants.NOT_CONNECTED);
                    lblOtherPlayerName.Text = "Other Player:";
                    btnSend.IsEnabled = false;
                    break;
                case ChatConnection.STATE_CONNECTING:
                    updateStatus(Constants.CONNECTING);
                    break;
                case ChatConnection.STATE_MSG_RECEIVED:
                    processMessage(e.Reason);
                    break;
                    
            }
        }

        public void SetupAppBar() {
            ApplicationBar = new ApplicationBar();
            ApplicationBarMenuItem miConnect = new ApplicationBarMenuItem("connect");
            miConnect.Click += miConnect_Click;
            ApplicationBar.MenuItems.Add(miConnect);
        }

        public void miConnect_Click(object sender, EventArgs e) {
            NavigationService.Navigate(new Uri("/ConnectTo.xaml", UriKind.Relative));
        }

        public void updateStatus(string status) {
            lblStatus.Text = status;
        }

        public void processMessage(string msg) {
            string[] strarr = msg.Split(' ');
            if(strarr.Length > 0) {
                switch(strarr[0]) {
                    case "Turn":
                        updateTheirAnswer(strarr[1]);
                        break;
                }
            }
        }

        private void updateTheirAnswer(string ans) {
            theirAns.Text = ans;
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            //Update default screen values
            ChatConnection_ConnectionStateChanged(null, new ConnectionStateChangedEventArgs(ChatConnection.State, ""));
        }
    }
}