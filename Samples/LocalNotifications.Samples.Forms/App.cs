using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LocalNotifications.Plugin;
using LocalNotifications.Plugin.Abstractions;
using Xamarin.Forms;

namespace LocalNotifications.Samples.Forms
{
    public class App : Application
    {
        public App()
        {
            Label info = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Welcome to Xamarin Forms!"
            };

            // The root page of your application
            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        info,
                        new Button{
                             Text = "Notify in 5 Seconds",
                              Command = new Command(()=>{
                                var notifier = LocalNotifications.Plugin.CrossLocalNotifications.Current;
                                notifier.Notify(
                                    new LocalNotification()
                                    {
                                        Title = "Title",
                                        Text = "Text",
                                        NotifyTime = DateTime.Now.AddSeconds(5),
                                    });
                              })
                        },
                        new Button{
                             Text = "Cancel All",
                              Command = new Command(()=>{
                                    var notifier = LocalNotifications.Plugin.CrossLocalNotifications.Current;
                                    notifier.CancelAll();
                              })
                        }
                    }
                }
            };
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
