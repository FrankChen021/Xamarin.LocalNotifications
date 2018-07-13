

using LocalNotifications.Plugin.Abstractions;
using System.IO;
using System.Xml.Serialization;

namespace LocalNotifications.Plugin
{
    /// <summary>
    /// 
    /// </summary>
    public class NativeNotification : LocalNotification
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        internal static string Serialize(NativeNotification notification)
        {
            var xmlSerializer = new XmlSerializer(notification.GetType());
            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, notification);
                return stringWriter.ToString();
            }
        }

        internal static NativeNotification Deserialize(string notificationString)
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