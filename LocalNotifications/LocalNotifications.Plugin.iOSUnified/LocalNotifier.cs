using System.Linq;
using LocalNotifications.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UIKit;
using Foundation;

namespace LocalNotifications.Plugin
{
    /// <summary>
    /// Implementation of ILocalNotifier for iOS
    /// </summary>
    public class LocalNotifier : ILocalNotifier
    {
        private const string NotificationKey = "LocalNotificationKey";

        /// <summary>
        /// Notifies the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        public object Notify(LocalNotification notification)
        {
            var id = Guid.NewGuid().ToString();
            var nativeNotification = new UILocalNotification
            {
                AlertAction = notification.Title,
                AlertBody = notification.Text,
                FireDate = notification.NotifyTime.ToNSDate(),
                //ApplicationIconBadgeNumber = 1,
                 
                UserInfo = NSDictionary.FromObjectAndKey(NSObject.FromObject(id), NSObject.FromObject(NotificationKey))
            }; ;

            UIApplication.SharedApplication.ScheduleLocalNotification(nativeNotification);

            return id;
        }

        /// <summary>
        /// Cancels the specified notification identifier.
        /// </summary>
        /// <param name="notificationId">The notification identifier.</param>
        public void Cancel(object notificationId)
        {
            var id = notificationId as string;
            if (string.IsNullOrEmpty(id))
                return;

            var notifications = UIApplication.SharedApplication.ScheduledLocalNotifications;
            var notification = notifications.Where(n => n.UserInfo.ContainsKey(NSObject.FromObject(NotificationKey)))
                .FirstOrDefault(n => n.UserInfo[NotificationKey].Equals(NSObject.FromObject(id)));

            if (notification != null)
            {
                UIApplication.SharedApplication.CancelLocalNotification(notification);
            }
        }

        /// <summary>
        /// cancel all notifications
        /// </summary>
        public void CancelAll()
        {
            UIApplication.SharedApplication.CancelAllLocalNotifications();
        }
    }
}