using System;
using LocalNotifications.Plugin.Abstractions;

namespace LocalNotifications.Plugin
{
    /// <summary>
    /// Cross platform LocalNotifications implemenations
    /// </summary>
    public class CrossLocalNotifications
    {
        static Lazy<ILocalNotifier> Implementation = new Lazy<ILocalNotifier>(() => CreateNotification(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static ILocalNotifier Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static ILocalNotifier CreateNotification()
        {
#if NETSTANDARD1_0
            return null;
#else
            return new LocalNotifier();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}