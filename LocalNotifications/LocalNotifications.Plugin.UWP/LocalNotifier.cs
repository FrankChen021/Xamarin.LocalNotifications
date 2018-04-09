using System;
using System.IO;
using LocalNotifications.Plugin;
using LocalNotifications.Plugin.Abstractions;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace LocalNotifications.Plugin
{
    /// <summary>
    /// Implementation of ILocalNotifier for Android
    /// </summary>
    public class LocalNotifier : ILocalNotifier
    {
        static int staticId;
        private ToastNotifier notifier;
        private XmlDocument toastXml;
        private IXmlNode titleElem;
        private IXmlNode contentElem;

        public event ActivatedFromNotificationEventHandler ActivatedFromNotification;

        public LocalNotifier()
        {
            // string toastVisual =
            //$@"<visual>
            //  <binding template='ToastGeneric'>
            //    <text>{title}</text>
            //    <text>{content}</text>
            //    <image src='{image}'/>
            //    <image src='{logo}' placement='appLogoOverride' hint-crop='circle'/>
            //  </binding>
            //</visual>";
            //            string toastXmlString = $@"<toast><visual>
            //<binding template='ToastGeneric'>
            //    <text></text>
            //    <text></text>
            //  </binding>
            //</visual></toast>";

            toastXml = new XmlDocument();
            var docElement = toastXml.AppendChild(toastXml.CreateElement("toast"));
            var visualElem = docElement.AppendChild(toastXml.CreateElement("visual"));
            var bindElem = visualElem.AppendChild(toastXml.CreateElement("binding")) as XmlElement;
            {
                bindElem.SetAttribute("template", "ToastGeneric");
            }
            titleElem = bindElem.AppendChild(toastXml.CreateElement("text"));
            contentElem = bindElem.AppendChild(toastXml.CreateElement("text"));
            notifier = ToastNotificationManager.CreateToastNotifier();
        }

        /// <summary>
        /// Notifies the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        public object Notify(LocalNotification notification)
        {
            //https://docs.microsoft.com/en-us/windows/uwp/design/shell/tiles-and-notifications/send-local-toast

            string id = (staticId++).ToString();

            if (notification.Parameter == null)
                toastXml.DocumentElement.RemoveAttribute("launch");
            else
                toastXml.DocumentElement.SetAttribute("launch", notification.Parameter);

            titleElem.InnerText = notification.Title;
            contentElem.InnerText = notification.Text;

            notifier.AddToSchedule(new ScheduledToastNotification(toastXml, notification.NotifyTime)
            {
                Id = id,
            });

            return id;
        }

        /// <summary>
        /// Cancels the specified notification identifier.
        /// </summary>
        /// <param name="notificationId">The notification identifier.</param>
        public void Cancel(object notificationId) 
        {
            string id = notificationId as string;
            if ( string.IsNullOrEmpty(id) )
                return;

            foreach (var n in notifier.GetScheduledToastNotifications())
            {
                if (n.Id == id)
                {
                    notifier.RemoveFromSchedule(n);
                    break;
                }
            }
        }

        public void CancelAll()
        {
            foreach (var n in notifier.GetScheduledToastNotifications())
            {
                notifier.RemoveFromSchedule(n);
            }
        }

        public void OnActivated(string parameter)
        {
            if (this.ActivatedFromNotification != null)
                this.ActivatedFromNotification(parameter);
        }
    }
}