using System.Linq;
using LocalNotifications.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
#if __UNIFIED__
using UIKit;
using Foundation;
#else
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif

namespace LocalNotifications.Plugin
{
    /// <summary>
    /// Implementation of ILocalNotifier for iOS
    /// </summary>
    public class LocalNotifier : ILocalNotifier
    {
        private const string NotificationKey = "LocalNotificationKey";
        private IDictionary<int, Action<LocalNotification>> notifyActions = new ConcurrentDictionary<int, Action<LocalNotification>>();

        /// <summary>
        /// Notifies the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        public void Notify(LocalNotification notification, Action<LocalNotification> onNotifyAction)
        {
            var nativeNotification = CreateNativeNotification(notification);
            if (onNotifyAction != null)
                notifyActions.Add(notification.Id, onNotifyAction);

            UIApplication.SharedApplication.ScheduleLocalNotification(nativeNotification);
        }

        /// <summary>
        /// Cancels the specified notification identifier.
        /// </summary>
        /// <param name="notificationId">The notification identifier.</param>
        public void Cancel(int notificationId)
        {
            var notifications = UIApplication.SharedApplication.ScheduledLocalNotifications;
            var notification = notifications.Where(n => n.UserInfo.ContainsKey(NSObject.FromObject(NotificationKey)))
                .FirstOrDefault(n => n.UserInfo[NotificationKey].Equals(NSObject.FromObject(notificationId)));

            if (notification != null)
            {
                UIApplication.SharedApplication.CancelLocalNotification(notification);
            }
        }

        private UILocalNotification CreateNativeNotification(LocalNotification notification)
        {
            var nativeNotification = new UILocalNotification
            {
                AlertAction = notification.Title,
                AlertBody = notification.Text,
                FireDate = notification.NotifyTime.ToNSDate(),
                //ApplicationIconBadgeNumber = 1,
                UserInfo = NSDictionary.FromObjectAndKey(NSObject.FromObject(notification.Id), NSObject.FromObject(NotificationKey))
            };

            return nativeNotification;
        }

        public void Recv(UILocalNotification notification)
        {
            NSNumber idValue = (NSNumber)notification.UserInfo[NotificationKey];
            var id = idValue.Int32Value;

            Action<LocalNotification> action;    
            if ( notifyActions.TryGetValue(id, out action) )
            {
                notifyActions.Remove(id);
                action(new LocalNotification() { Text = notification.AlertBody, Title = notification.AlertAction });
            }
        }
    }
}