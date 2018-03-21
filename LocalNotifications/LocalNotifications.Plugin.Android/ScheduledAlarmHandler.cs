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
            var notification = deserializeFromString(extra);

            getNotificationManager().Notify(notification.Id, createNativeNotification(notification));
        }

        private NotificationManager getNotificationManager()
        {
            var notificationManager = Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;
            return notificationManager;
        }

        private Notification createNativeNotification(LocalNotification notification)
        {
            var builder = new Notification.Builder(Application.Context)
                .SetContentTitle(notification.Title)
                .SetContentText(notification.Text)
//                .SetSmallIcon(Resource.Drawable.IcDialogEmail);
                .SetSmallIcon(Application.Context.ApplicationInfo.Icon);

            var nativeNotification = builder.Build();
            return nativeNotification;
        }

        private NativeNotification deserializeFromString(string notificationString)
        {
            var xmlSerializer = new XmlSerializer(typeof(NativeNotification));
            using (var stringReader = new StringReader(notificationString))
            {
                var notification = (NativeNotification)xmlSerializer.Deserialize(stringReader);
                return notification;
            }
        }
    }
}