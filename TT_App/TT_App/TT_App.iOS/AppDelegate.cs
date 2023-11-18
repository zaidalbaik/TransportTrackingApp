using Foundation;
using UIKit;

namespace TT_App.iOS
{
   
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
       
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

           // Xamarin.FormsGoogleMaps.Init("AIzaSyD09d-emf9HAldwCt2wtsCjsyz9Q594VoE");
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
