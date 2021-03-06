﻿using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using LocalNotifications.Plugin;
using UIKit;

namespace LocalNotifications.Samples.Forms.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);
            UIApplication.SharedApplication.RegisterUserNotificationSettings(notificationSettings);

            (CrossLocalNotifications.Current as LocalNotifier).FinishedLaunching(app, options);

            return base.FinishedLaunching(app, options);
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
            (CrossLocalNotifications.Current as LocalNotifier).ActivateFromNotification(application, notification);
        }
    }
}
