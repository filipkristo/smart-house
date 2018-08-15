using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;

namespace SmartHouse.Xamarin.Droid
{
    [Application]
    public class SmartHouseApplication : Application, Application.IActivityLifecycleCallbacks
    {
        public static Context AppContext;

        public SmartHouseApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();
            //RegisterActivityLifecycleCallbacks(this);
            AppContext = this.ApplicationContext;
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            //UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;            
        }

        public void OnActivityStopped(Activity activity)
        {
            
        }
    }
}