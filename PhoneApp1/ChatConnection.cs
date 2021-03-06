﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using System.ComponentModel;
using Windows.Storage.Streams;
using System.Threading;
using System.Windows;
using Microsoft.Phone.Shell;
using System.Diagnostics;
// (c) Erkki Nokso-Koivisto 25/Nov/2012

// Good reading:
// http://msdn.microsoft.com/en-us/library/windowsphone/develop/jj207007(v=vs.105).aspx
// http://blogs.windows.com/windows_phone/b/wpdev/archive/2012/11/19/networking-in-windows-phone-8.aspx

namespace PhoneApp1
{
    
    class ConnectionStateChangedEventArgs : EventArgs {

        public readonly int State;
        public readonly string Reason;

        public ConnectionStateChangedEventArgs(int state, string reason) {
            this.State = state;
            this.Reason = reason;
        }
    }

    class ChatConnection
    {

        // UUID's are from BlueToothChatService.java
        // INSECURE UID is available only on >= Android SDK10 BluetoothChat sample

        private const string UUID_CHAT_SERVICE_SECURE = "{fa87c0d0-afac-11de-8a39-0800200c9a66}";
        private const string UUID_CHAT_SERVICE_INSECURE = "{8ce255c0-200a-11e0-ac64-0800200c9a66}";

        public const int STATE_DISCONNECTED = 0;
        public const int STATE_CONNECTING = 1;
        public const int STATE_CONNECTED = 2;
        public const int STATE_MSG_RECEIVED = 3;

        public static event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;
        public static int State = STATE_DISCONNECTED;

        private static IInputStream inputStream;
        private static IOutputStream outputStream;
        private static StreamSocket socket;
        
        private static DataReader reader;
        private static DataWriter writer;


        public static string yourAns = "";
        public static string theirAns = "";
        public static string peer = "";

        const bool ANDROID = true;

        public async static void ConnectAsync(PeerInformation peerInfo) {
            try {
                Debug.WriteLine("Begin connection to " + peerInfo.HostName + ":" + peerInfo.DisplayName);
                if(State != STATE_DISCONNECTED) {
                    Debug.WriteLine("I'm not discconected! So I'll do it now since you want to connect again!");
                    CleanUp();
                    return;
                }
                State = STATE_CONNECTING;
                if (ANDROID){
                    socket = new StreamSocket();
                    await socket.ConnectAsync(peerInfo.HostName, UUID_CHAT_SERVICE_SECURE);
                } else {
                    socket = await PeerFinder.ConnectAsync(peerInfo);
                }
                StopListeningIncoming();
                inputStream = socket.InputStream;
                outputStream = socket.OutputStream;
                writer = new DataWriter(outputStream);
                reader = new DataReader(inputStream);
                System.Diagnostics.Debug.WriteLine("ConnectAsync3");
                // allows LoadAsync() to load less data than requested (1024 bytes in this case) without
                // blocking.. 
                reader.InputStreamOptions = InputStreamOptions.Partial;
                State = STATE_CONNECTED;
                peer = peerInfo.DisplayName;
                if (ConnectionStateChanged != null)
                {
                    ConnectionStateChanged(null, new ConnectionStateChangedEventArgs(STATE_CONNECTED, "connected"));
                }                
                await readSocket();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR/ConnectAsync/" + ex.ToString());
                MessageBox.Show("Failed to connect, make sure bluetooth chat app is running on target device..");
                CleanUp();
            }
           

        }


        private static void CleanUp() {

            try
            {
                System.Diagnostics.Debug.WriteLine("ERROR/CleanUp/Disconnecting");
                State = STATE_DISCONNECTED;
                socket.Dispose();
                

            }
            catch (Exception)
            {
            }
           
            if (ConnectionStateChanged != null)
            {
                ConnectionStateChanged(null, new ConnectionStateChangedEventArgs(STATE_DISCONNECTED, "closed"));
            }
 

            StartListeningIncoming();
        }

        private static async Task readSocket() {
            while (State == STATE_CONNECTED){
                try {                    
                    uint len = await reader.LoadAsync(1024);
                    if (len > 0) {
                        String msg = reader.ReadString(len);
                        ConnectionStateChanged(null, new ConnectionStateChangedEventArgs(STATE_MSG_RECEIVED, msg));
                        /*ChatMessage chatMsg = new ChatMessage() {
                            Content = "<< " + msg
                        };
                        ChatMessages.Data.Add(chatMsg);*/
                        Debug.WriteLine("RECEIVED:"+msg);
                    }

                } catch (Exception ex) {
                    System.Diagnostics.Debug.WriteLine("ERROR/readSocket/" + ex.ToString());
                    CleanUp();
                    break;
                }
            }
            State = STATE_DISCONNECTED;             
        }

//        public async static void SendChatMessageAsync(ChatMessage msg)
        public async static void SendChatMessageAsync(string msg)
        {
            try
            {
                if (State == STATE_CONNECTED)
                {
                    writer.WriteString(msg);
                    await writer.StoreAsync();
                    System.Diagnostics.Debug.WriteLine("Msg sent/" + msg);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR/SendChatMessageAsync/" + ex.ToString());
                CleanUp();
            }
        }

        public static void Close() {
            CleanUp();
        }

//============================================================================================================
// Didn't succeed on sending any kind of trigger from Android to WP8
// 

        public static void StartListeningIncoming()
        {
            if (State == STATE_DISCONNECTED)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine(PeerFinder.SupportedDiscoveryTypes);
                    System.Diagnostics.Debug.WriteLine("StartListeningIncoming");
                    PeerFinder.ConnectionRequested -= PeerFinder_ConnectionRequested;
                    PeerFinder.ConnectionRequested += PeerFinder_ConnectionRequested;
                    PeerFinder.TriggeredConnectionStateChanged += PeerFinder_TriggeredConnectionStateChanged;
                    PeerFinder.Start();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("StartListeningIncoming/Error=" + ex.ToString());
                }
            }
        }

        static void PeerFinder_TriggeredConnectionStateChanged(object sender, TriggeredConnectionStateChangedEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("PeerFinder_TriggeredConnectionStateChanged");
        }

        public static void StopListeningIncoming()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("StopListeningIncoming");
                PeerFinder.Stop();
            }
            catch (Exception)
            {
            }
        }

        private static void PeerFinder_ConnectionRequested(object sender, ConnectionRequestedEventArgs args)
        {
            MessageBox.Show("Connection requested");
            System.Diagnostics.Debug.WriteLine("PeerFinder_ConnectionRequested");
            ConnectAsync(args.PeerInformation);
            StopListeningIncoming();
        }

    }
}
