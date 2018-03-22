using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidGame.Controllers;

namespace AndroidGame
{
    [Activity(Label = "AndroidGame"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.UserLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]

    public class Activity : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var metrics = Resources.DisplayMetrics;
            GUIController.screenSize.X = metrics.WidthPixels;
            GUIController.screenSize.Y = metrics.HeightPixels;

            var g = new AndroidGame();
            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }
    }
}