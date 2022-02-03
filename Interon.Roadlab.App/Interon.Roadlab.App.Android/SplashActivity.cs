using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Com.Airbnb.Lottie;


namespace Interon.Roadlab.App.Droid
{
    [Activity(Theme = "@style/Theme.Splash",
        MainLauncher = true,
        NoHistory = true)]
    public class SplashActivity : Activity, Animator.IAnimatorListener
    {
        private Intent mainActivityIntent;
        public SplashActivity()
        {
            //mainActivityIntent = new Intent();
            //Thread thread = new Thread(() =>
            //{
            //    mainActivityIntent = new Intent(Application.Context, typeof(MainActivity));
              

            //});

            //thread.Start();
            //thread.Join();
            //StartActivity(mainActivityIntent);


        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
           
            SetContentView(Resource.Layout.Activity_Splash);
          //  BackgroundAggregator.Init(this);
            var animationView = FindViewById<LottieAnimationView>(Resource.Id.animation_view);
            animationView.ImageAssetsFolder = "images/";
            animationView.AddAnimatorListener(this);
           
               


          
        }

       

        public void OnAnimationCancel(Animator animation)
        {
        }

        public void OnAnimationEnd(Animator animation)
        {
            //Intent mainActivityIntent = new Intent();

            //StartActivity(mainActivityIntent);

              StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            
            this.Finish();
           
        }

        public void OnAnimationRepeat(Animator animation)
        {
        }

        public void OnAnimationStart(Animator animation)
        {
          



        }
    }
}