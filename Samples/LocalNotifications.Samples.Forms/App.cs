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
            CrossLocalNotifications.Current.ActivateFromNotification += Current_ActivateFromNotification;

            // The root page of your application
            MainPage = new NavigationPage(new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Label
                        {
                            HorizontalTextAlignment = TextAlignment.Center,
                            Text = "Welcome to Xamarin Forms!"
                        },
                        new Button
                        {
                             Text = "Notify in About 5 Seconds",
                              Command = new Command(()=>{
                                var notifier = LocalNotifications.Plugin.CrossLocalNotifications.Current;
                                notifier.Notify(
                                    new LocalNotification()
                                    {
                                        Title = "Title",
                                        Text = "Text",
                                        LaunchURL = "text from notification",
                                        NotifyTime = DateTime.Now.AddSeconds(5),
                                    });
                              })
                        },
                        new Button
                        {
                             Text = "Cancel All",
                              Command = new Command(
                              ()=>
                              {
                                    var notifier = LocalNotifications.Plugin.CrossLocalNotifications.Current;
                                    notifier.CancelAll();
                              })
                        }
                    }
                }
            });
        }

        private void Current_ActivateFromNotification(object parameter)
        {
            Device.BeginInvokeOnMainThread(async () => {
                if (this.MainPage.Navigation.NavigationStack.Count == 1)
                {
                    await this.MainPage.Navigation.PushAsync(new ContentPage()
                    {
                        Content = new StackLayout()
                        {
                            VerticalOptions = LayoutOptions.Center,
                            Children =
                            {
                                new Label
                                {
                                    HorizontalTextAlignment = TextAlignment.Center,
                                    Text = parameter.ToString()
                                }
                            }
                        }
                    });
                }
            });
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
