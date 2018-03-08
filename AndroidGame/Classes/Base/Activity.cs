using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace AndroidGame
{
    [Activity(Label = "AndroidGame"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.FullUser
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]

    public class Activity : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var g = new AndroidGame();
            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }
    }
}