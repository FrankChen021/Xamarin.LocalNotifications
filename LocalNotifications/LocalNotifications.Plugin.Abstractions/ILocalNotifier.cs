using System;

namespace LocalNotifications.Plugin.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter"></param>
    public delegate void ActivatedFromNotificationEventHandler(object parameter);

    /// <summary>
    /// Interface for LocalNotifier
    /// </summary>
    public interface ILocalNotifier
    {
        /// <summary>
        /// 
        /// </summary>
        event ActivatedFromNotificationEventHandler ActivateFromNotification;

        /// <summary>
        /// Notifies the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <returns>object used to cancel this notification</returns>
        object Notify(LocalNotification notification);

        /// <summary>
        /// Cancels the specified notification identifier.
        /// </summary>
        /// <param name="notificationId">The notification identifier.</param>
        void Cancel(object notificationId);

        /// <summary>
        /// cancel all notifications sent by current app
        /// </summary>
        void CancelAll();
    }
}
