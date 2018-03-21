using Foundation;
using LocalNotifications.Plugin;
using UIKit;

namespace LocalNotifications.Samples.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method

            var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);
            UIApplication.SharedApplication.RegisterUserNotificationSettings(notificationSettings);

            return true;
        }

        public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {
            if (application.ApplicationState == UIApplicationState.Active)
            {
                UIApplication.SharedApplication.InvokeOnMainThread(() =>
                {
                    var alert = new UIAlertView();
                    alert.Title = "NOTIFICATION RECV";
                    alert.Message = notification.AlertBody;
                    alert.AddButton("OK");
                    alert.Show();
                });
            }
        }
    }
}