using System;
using System.Net;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Gms.Common;
using Android.Runtime;
using Android.OS;
using Android.Util;
using Interon.Roadlab.App.Droid.Core;
using Matcha.BackgroundService.Droid;
using Xamarin.Forms;

using Android.Gms.Common;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace Interon.Roadlab.App.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    [Activity(Label = "Interon.Roadlab.App", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private AlertDialog ad;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            initFontScale();
            TabLayoutResource       = Resource.Layout.Tabbar;
            ToolbarResource         = Resource.Layout.Toolbar;
            ToastService.GetContext = () => this;
            BackgroundAggregator.Init(this);
            Forms.SetFlags("CollectionView_Experimental");



            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
            //  global::Xamarin.Forms.Forms.for .Init();
            LoadApplication(new App());
            //  AnimationViewRenderer.Init();
            // Android.Glide.Forms.Init(this);



            if (IsPlayServiceAvailable() == false)
            {
                throw new Exception("This device does not have Google Play Services and cannot receive push notifications.");
            }

            CreateNotificationChannel();
            //this is to handle the keyboard screen shift
            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }

        private void initFontScale()
        {
            Configuration configuration = Resources.Configuration;
            configuration.FontScale = (float) 1;
            //0.85 small, 1 standard, 1.15 big，1.3 more bigger ，1.45 supper big 
            DisplayMetrics metrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetMetrics(metrics);
            metrics.ScaledDensity = configuration.FontScale * metrics.Density;
            BaseContext.Resources.UpdateConfiguration(configuration, metrics);
        }

        protected override void OnNewIntent(Intent intent)
        {
            if (intent.Extras != null)
            {
                var message = intent.GetStringExtra("message");
                // (App.Current.MainPage as MainPage)?.AddMessage(message);
            }

            base.OnNewIntent(intent);
        }

        bool IsPlayServiceAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    Log.Debug(AppConstants.DebugTag, GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                {
                    Log.Debug(AppConstants.DebugTag, "This device is not supported");
                }

                return false;
            }

            return true;
        }

        void CreateNotificationChannel()
        {
            // Notification channels are new as of "Oreo".
            // There is no need to create a notification channel on older versions of Android.
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelName        = AppConstants.NotificationChannelName;
                var channelDescription = String.Empty;
                var channel = new NotificationChannel(channelName, channelName, NotificationImportance.Default)
                {
                    Description = channelDescription
                };

                var notificationManager = (NotificationManager) GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private bool doubleBackToExitPressedOnce = false;
        private BackgroundAggregator _backgroundA;
        private NotificationManager _notificationManager;


        private void BlockExit()
        {
            Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
            AlertDialog                     alert  = dialog.Create();
            alert.SetTitle("Exit");
            alert.SetMessage("Do you want to exit the app ?");
            alert.SetButton("OK", (c, ev) =>
            {
                base.OnBackPressed();
                Java.Lang.JavaSystem.Exit(0);
                return; // Ok button click task  
            });
            alert.SetButton2("CANCEL", (c, ev) =>
            {
                //base.OnBackPressed();
                return;
            });
            alert.Show();
        }

        public override void OnBackPressed()
        {

            try
            {
                if (Shell.Current.Navigation.NavigationStack.Count == 1)
                {
                    BlockExit();
                }

                else
                {

                    base.OnBackPressed();

                }

                var url             = Shell.Current.CurrentState.Location.ToString();
                var navigationStack = Shell.Current.Navigation.NavigationStack;
            }
            catch (Exception e)
            {
                BlockExit();
            }

        }

        private void checkdate()
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://www.Site.cc/Folder/Inspections.Inspections-Signed.apk");

            // If required by the server, set the credentials.
            //request.Credentials = CredentialCache.DefaultCredentials;
            //request.IfModifiedSince = DateTime.Parse("01-01-1990");
            try
            {
                using (WebResponse response = request.GetResponse())
                {

                    DateTime dt      = System.IO.File.GetCreationTime("/sdcard/Download/Inspections.Inspections-Signed.apk"); //.GetLastWriteTime("/sdcard/Dowload/Inspections.Inspections-Signed.apk");
                    DateTime appDate = DateTime.Parse(response.Headers["Last-Modified"].ToString());
                    // DateTime current = DateTime.Now;

                    if (appDate > dt)
                    {
                        try
                        {
                            ad = new AlertDialog.Builder(this).Create();
                            ad.SetTitle("INFO");

                            ad.SetMessage("Update Found \n \n This might take a while depending on Network");
                            ad.SetButton("NOW", delegate
                            {
                                try
                                {
                                    System.IO.File.Delete("/sdcard/download/Inspections.Inspections-Signed.apk");
                                    DownloadFile("http://www.Site.cc/Folder/Inspections.Inspections-Signed.apk", "/sdcard/download/Inspections.Inspections-Signed.apk"); // Download Using HttpWebRequest

                                    Intent promptInstall = new Intent(Intent.ActionView).SetDataAndType(Android.Net.Uri.FromFile(new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + "/download/" + "Inspections.Inspections-Signed.apk")), "application/vnd.android.package-archive");
                                    //Intent promptInstall = new Intent(Intent.ActionView).SetData(Android.Net.Uri.Parse("/sdcard/Download/Inspections.Inspections-Signed.apk")).SetType("application/vnd.android.package-archive");
                                    promptInstall.AddFlags(ActivityFlags.NewTask);
                                    StartActivity(promptInstall);

                                }
                                catch (ActivityNotFoundException ex)
                                {
                                    ad = new AlertDialog.Builder(this).Create();
                                    ad.SetTitle("INFO");
                                    ad.SetMessage("Error Installing application Please try again later" + ex);
                                    ad.SetCanceledOnTouchOutside(true);
                                    ad.Show();
                                }


                            });
                            ad.SetButton2("LATER", delegate { return; });
                            ad.SetCanceledOnTouchOutside(true);
                            ad.Show();


                        }
                        catch (System.Exception ex)
                        {
                            ad = new AlertDialog.Builder(this).Create();
                            ad.SetTitle("INFO");
                            ad.SetMessage(ex.Message);
                            ad.SetCanceledOnTouchOutside(true);
                            ad.Show();
                        }
                    }
                }
            }
            catch (WebException wex)
            {

            }
        }

        public void DownloadFile(string m_uri, string m_filePath)
        {
            var webClient = new WebClient();

            webClient.DownloadFileCompleted += (s, e) =>
            {
                string error = e.Error.Message;
                //var bytes = e.Result; // get the downloaded data
                string documentsPath = Android.OS.Environment.ExternalStorageDirectory + "/download/";
                /************* GetDate ************/
                DateTime appDate    = DateTime.Now;
                DateTime Jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                /// <summary>Get extra long current timestamp</summary>
                long Millis = (long) ((appDate - Jan1St1970).TotalMilliseconds);
                Millis = Millis - 7200000; //current Time
                long time = DateTime.Now.Ticks;

                /************** Declare Notification Manager *************/
                this._notificationManager = this.GetSystemService(NotificationService) as NotificationManager;
                if (this._notificationManager == null)
                {
                    throw new System.Exception("couldn't get a reference to the notification service");
                }

                /************* Set Notification ***************/
                // Set the icon, scrolling text and timestamp

                Notification notification = new Notification(Resource.Drawable.icon, "Updates", Millis);
                notification.Flags = NotificationFlags.AutoCancel;
                notification.Flags = NotificationFlags.OngoingEvent;
                //notification.Number = 3;

                Intent promptInstall = new Intent(Intent.ActionView).SetDataAndType(Android.Net.Uri.FromFile(new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + "/download/" + "FileName.apk")), "application/vnd.android.package-archive");
                promptInstall.AddFlags(ActivityFlags.NewTask);
                // The PendingIntent to launch our activity if the user selects this notification
                PendingIntent contentIntent = PendingIntent.GetActivity(this, 0, promptInstall, 0);
                var           nBuilder      = new Notification.Builder(this);

                // Set the info for the views that show in the notification panel.
                notification.SetLatestEventInfo(this, "Update Complete", "Click to Install", contentIntent);

                // Send the notification.
                // We use a layout id because it is a unique number.  We use it later to cancel.
                // TODO: switch to a unique identifier
                /************* Send Notification ************/
                this._notificationManager.Notify(666, notification);

                //System.IO.File.WriteAllBytes(m_filePath, bytes); // writes to local storage  

                //StartActivity(promptInstall);



            };

            var url = new System.Uri(m_uri);

            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
            webClient.DownloadFileAsync(url, Android.OS.Environment.ExternalStorageDirectory + "/download/FileName.apk");

        }

        private static void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {

            // Displays the operation identifier, and the transfer progress.
            /*Console.WriteLine("{0}    downloaded {1} of {2} bytes. {3} % complete...",
                 (string)e.UserState,
                 e.BytesReceived,
                 e.TotalBytesToReceive,
                 e.ProgressPercentage);*/

            int length = Convert.ToInt32(e.TotalBytesToReceive.ToString());
            int prog   = Convert.ToInt32(e.BytesReceived.ToString());
            int perc   = Convert.ToInt32(e.ProgressPercentage.ToString());



        }
    }
}
