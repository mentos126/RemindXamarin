using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RemindXamarin.Droid;
using RemindXamarin.Services;
using Xamarin.Forms;

[assembly:Dependency(typeof(Toast_Android))]
namespace RemindXamarin.Droid
{
    public class Toast_Android : IMessageToast
    {
        public void Show(String message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }
        
    }
}