using System;
using System.IO;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.OS;
using LocalNotifications.Plugin.Abstractions;

namespace LocalNotifications.Plugin
{
    /// <summary>
    /// Implementation of ILocalNotifier for Android
    /// </summary>
    public class LocalNotifier : ILocalNotifier
    {
        static int staticId;

        /// <summary>
        /// 
        /// </summary>
        public event ActivatedFromNotificationEventHandler ActivatedFromNotification;

        /// <summary>
        /// Notifies the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        public object Notify(LocalNotification notification)
        {
            var id = staticId++;
            var intent = createIntent(id);

            var serializedNotification = NativeNotification.Serialize(new NativeNotification()
            {
                Id = id,
                NotifyTime = notification.NotifyTime,
                Text = notification.Text,
                Title = notification.Title,
                Parameter = notification.Parameter
            });
            intent.PutExtra(ScheduledAlarmHandler.LocalNotificationKey, serializedNotification);

            var pendingIntent = PendingIntent.GetBroadcast(Application.Context, 0, intent, PendingIntentFlags.CancelCurrent);
            var triggerTime = notifyTimeInMilliseconds(notification.NotifyTime);
            var alarmManager = getAlarmManager();

            alarmManager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + triggerTime, pendingIntent);

            return id;
        }

        /// <summary>
        /// Cancels the specified notification identifier.
        /// </summary>
        /// <param name="notificationId">The notification identifier.</param>
        public void Cancel(object notificationId)
        {
            if (notificationId.GetType() != typeof(int))
                return;

            int id = (int)notificationId;
            var intent = createIntent(id);
            var pendingIntent = PendingIntent.GetBroadcast(Application.Context, 0, intent, PendingIntentFlags.CancelCurrent);

            var alarmManager = getAlarmManager();
            alarmManager.Cancel(pendingIntent);

            getNotificationManager().Cancel(id);
        }

        private Intent createIntent(int notificationId)
        {
            return new Intent(Application.Context, typeof(ScheduledAlarmHandler));
        }

        private NotificationManager getNotificationManager()
        {
            var notificationManager = Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;
            return notificationManager;
        }

        private AlarmManager getAlarmManager()
        {
            var alarmManager = Application.Context.GetSystemService(Context.AlarmService) as AlarmManager;
            return alarmManager;
        }

        private long notifyTimeInMilliseconds(DateTime notifyTime)
        {
            //var utcTime = TimeZoneInfo.ConvertTimeToUtc(notifyTime);
            //var epochDifference = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;

            //var utcAlarmTimeInMillis = utcTime.AddSeconds(-epochDifference).Ticks / 10000;
            var utcAlarmTimeInMillis = (notifyTime.ToUniversalTime() - DateTime.UtcNow).TotalMilliseconds;
            return (long)utcAlarmTimeInMillis;
        }

        /// <summary>
        /// cancel all notifications
        /// </summary>
        public void CancelAll()
        {
            getNotificationManager().CancelAll();
        }

        /// <summary>
        /// 
        /// </summary>
        internal Type MainActivityType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activity"></param>
        public void OnActivated(Activity activity)
        {
            MainActivityType = activity.GetType();

            if (!activity.Intent.HasExtra("launch_param"))
                return;

            var param = activity.Intent.GetStringExtra("launch_param");
            if (string.IsNullOrEmpty(param))
                return;

            if (this.ActivatedFromNotification != null)
                this.ActivatedFromNotification(param);
        }
    }
}