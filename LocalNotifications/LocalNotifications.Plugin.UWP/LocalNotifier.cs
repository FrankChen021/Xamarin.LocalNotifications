using System;
using System.IO;
using LocalNotifications.Plugin;
using LocalNotifications.Plugin.Abstractions;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

[assembly: Xamarin.Forms.Dependency(typeof(LocalNotifier))]
namespace LocalNotifications.Plugin
{
    /// <summary>
    /// Implementation of ILocalNotifier for Android
    /// </summary>
    public class LocalNotifier : ILocalNotifier
    {
        private ToastNotifier notifier;
        private XmlDocument toastXml;
        private IXmlNode titleElem;
        private IXmlNode contentElem;

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
            var bindElem = visualElem.AppendChild(toastXml.CreateElement("binding"));
            {
                var attrib = toastXml.CreateAttribute("template");
                attrib.Value = "ToastGeneric";
                bindElem.Attributes.SetNamedItem(attrib);
            }
            titleElem = bindElem.AppendChild(toastXml.CreateElement("text"));
            contentElem = bindElem.AppendChild(toastXml.CreateElement("text"));

            notifier = ToastNotificationManager.CreateToastNotifier();
        }

        /// <summary>
        /// Notifies the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        public void Notify(LocalNotification notification)
        {
            //https://docs.microsoft.com/en-us/windows/uwp/design/shell/tiles-and-notifications/send-local-toast

            titleElem.InnerText = notification.Title;
            contentElem.InnerText = notification.Text;

            notifier.AddToSchedule(new ScheduledToastNotification(toastXml, notification.NotifyTime)
            {
                Id = notification.Id.ToString()
            });
        }

        /// <summary>
        /// Cancels the specified notification identifier.
        /// </summary>
        /// <param name="notificationId">The notification identifier.</param>
        public void Cancel(int notificationId) 
        {
            string id = notificationId.ToString();
            foreach(var n in notifier.GetScheduledToastNotifications())
            {
                if (n.Id == id)
                {
                    notifier.RemoveFromSchedule(n);
                    break;
                }
            }
        }
    }
}