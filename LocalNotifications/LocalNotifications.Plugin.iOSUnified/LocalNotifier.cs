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
        private const string ArgumentKey = "LocalNotificationKey.Arg";

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
            var id = Guid.NewGuid().ToString();
            var nativeNotification = new UILocalNotification
            {
                AlertAction = notification.Title,
                AlertBody = notification.Text,
                FireDate = notification.NotifyTime.ToNSDate(),
                //ApplicationIconBadgeNumber = 1,   
                //UserInfo = NSDictionary.FromObjectAndKey(NSObject.FromObject(id), NSObject.FromObject(NotificationKey))
            };

            if (!string.IsNullOrEmpty(notification.Parameter))
                nativeNotification.UserInfo = NSDictionary.FromObjectsAndKeys(
                                                new NSObject[] { NSObject.FromObject(notification.Parameter), NSObject.FromObject(id) }, 
                                                new NSObject[] { NSObject.FromObject(ArgumentKey), NSObject.FromObject(NotificationKey) }
                                                );
            else
                nativeNotification.UserInfo = NSDictionary.FromObjectAndKey(NSObject.FromObject(id), NSObject.FromObject(NotificationKey));

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        public void OnActivated(string parameter)
        {
            if (this.ActivatedFromNotification != null)
                this.ActivatedFromNotification(parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        public void FinishedLaunching(UIApplication app, NSDictionary options)
        {
            var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);
            UIApplication.SharedApplication.RegisterUserNotificationSettings(notificationSettings);

            if (options == null || this.ActivatedFromNotification == null)
                return;

            // check for a local notification
            if (!options.ContainsKey(UIApplication.LaunchOptionsLocalNotificationKey))
                return;

            var localNotification = options[UIApplication.LaunchOptionsLocalNotificationKey] as UILocalNotification;
            if (localNotification == null)
                return;

            Activate(localNotification);
        }

        private void Activate(UILocalNotification localNotification)
        {
            // reset our badge
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

            NSObject val;
            if (!localNotification.UserInfo.TryGetValue(new NSString(ArgumentKey), out val))
                return;

            var argument = (val as NSString)?.ToString();
            if (!string.IsNullOrEmpty(argument))
                this.ActivatedFromNotification(argument);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="notification"></param>
        public void ActivateFromNotification(UIApplication application, UILocalNotification notification)
        {
            if (application.ApplicationState == UIApplicationState.Active)
                return;

            if (this.ActivatedFromNotification == null)
                return;

            Activate(notification);
        }
    }
}