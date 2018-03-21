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
            // The root page of your application
            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Label {
                            XAlign = TextAlignment.Center,
                            Text = "Welcome to Xamarin Forms!"
                        },
                        new Button{
                             Text = "Fire",
                              Command = new Command(()=>{ 
                                    var notifier = DependencyService.Get<ILocalNotifier>();
                                    notifier.Notify(
                                        new LocalNotification()
                                        {
                                            Title = "Title",
                                            Text = "Text",
                                            Id = 1,
                                            NotifyTime = DateTime.Now.AddSeconds(10),
                                        },
                                        (LocalNotification localNotification)=>{
                                            Device.BeginInvokeOnMainThread(async () => {
                                                await App.Current.MainPage.DisplayAlert(localNotification.Title, localNotification.Text, "OK");
                                            });
                                        });
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
