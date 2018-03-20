using System;
using System.IO;
using System.Xml.Serialization;
using LocalNotifications.Plugin.Abstractions;

namespace LocalNotifications.Plugin
{
    /// <summary>
    /// Implementation of ILocalNotifier for Android
    /// </summary>
    public class LocalNotifier : ILocalNotifier
    {
        /// <summary>
        /// Notifies the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        public void Notify(LocalNotification notification, Action<LocalNotification> onNotifyAction = null)
        {

        }

        /// <summary>
        /// Cancels the specified notification identifier.
        /// </summary>
        /// <param name="notificationId">The notification identifier.</param>
        public void Cancel(int notificationId) 
        { }
    }
}