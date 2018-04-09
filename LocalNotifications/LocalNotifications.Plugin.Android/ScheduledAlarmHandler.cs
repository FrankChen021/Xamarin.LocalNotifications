using System;
using System.IO;
using System.Xml.Serialization;
using Android;
using Android.App;
using Android.Content;
using Android.Runtime;
using LocalNotifications.Plugin.Abstractions;

namespace LocalNotifications.Plugin
{
    /// <summary>
    /// 
    /// </summary>
    [BroadcastReceiver]
    public class ScheduledAlarmHandler : BroadcastReceiver
    {
        internal const string LocalNotificationKey = "LocalNotification";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="intent"></param>
        public override void OnReceive(Context context, Intent intent)
        {
            var extra = intent.GetStringExtra(LocalNotificationKey);
            var notification = NativeNotification.Deserialize(extra);

            var notificationManager = Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;
            notificationManager.Notify(notification.Id, CreateAndroidNotification(notification));
        }

        private Notification CreateAndroidNotification(LocalNotification notification)
        {
            var builder = new Notification.Builder(Application.Context)
                .SetContentTitle(notification.Title)
                .SetContentText(notification.Text)
                .SetSmallIcon(Application.Context.ApplicationInfo.Icon)
                .SetAutoCancel(true);

            if (!string.IsNullOrEmpty(notification.Parameter))
            {
                var intent = new Intent(Application.Context, (CrossLocalNotifications.Current as LocalNotifier).MainActivityType);
                intent.PutExtra("launch_param", notification.Parameter);

                builder.SetContentIntent(PendingIntent.GetActivity(
                                        Application.Context,
                                        0,
                                        intent,
                                        0));
            }

            var nativeNotification = builder.Build();
            nativeNotification.Defaults = NotificationDefaults.Sound | NotificationDefaults.Vibrate;

            return nativeNotification;
        }
    }
}