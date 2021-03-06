﻿using System;

namespace LocalNotifications.Plugin.Abstractions
{
    /// <summary>
    /// Notification options data used for creating native notification
    /// </summary>
    public class LocalNotification
    {
        /// <summary>
        /// Gets or sets the notification title.
        /// </summary>
        /// <value>
        /// The notification title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the notification text.
        /// </summary>
        /// <value>
        /// The notification text.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the notify time of notification.
        /// </summary>
        /// <value>
        /// The notify time of notification.
        /// </value>
        public DateTime NotifyTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Parameter { get; set; }
    }
}
