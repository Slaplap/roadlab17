using System.Diagnostics;
using Android.App;
using Android.Content;
using Android.OS;

[assembly: Permission(Name = "com.Interon.Roadlab.App.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]
[assembly: UsesPermission(Name = "com.Interon.Roadlab.App.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]

namespace Interon.Roadlab.App.Droid.Core
{
    [BroadcastReceiver(Permission = "com.google.android.c2dm.permission.SEND")]
    [IntentFilter(new string[] { "com.google.android.c2dm.intent.RECEIVE" }, Categories = new string[] { "com.Interon.Roadlab.App" })]
    [IntentFilter(new string[] { "com.google.android.c2dm.intent.REGISTRATION" }, Categories = new string[] { "com.Interon.Roadlab.App" })]
    [IntentFilter(new string[] { "com.google.android.gcm.intent.RETRY" }, Categories = new string[] { "com.Interon.Roadlab.App" })]

    public class GCMBroadcastReciever : BroadcastReceiver
    {
       

      

        public override void OnReceive(Context context, Intent intent)
        {
            PowerManager.WakeLock sWakeLock;
            var pm = PowerManager.FromContext(context);
            sWakeLock = pm.NewWakeLock(WakeLockFlags.Partial, "GCM Broadcast Reciever Tag");
            sWakeLock.Acquire();
            Debugger.Break();
            //handle the notification here

            sWakeLock.Release();
        }
    }
}