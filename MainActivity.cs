using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.View;
using AndroidX.DrawerLayout.Widget;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Navigation;
using Google.Android.Material.Snackbar;
using Mngs_Client_Support.Fragments;
using Xamarin.Essentials;

namespace Mngs_Client_Support
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private AndroidX.Fragment.App.Fragment Currentfragment;
        private HomeFragment homeFragment;
        private ChatsFragment Chats_Fragment;
        FloatingActionButton fab;
        private Android.Widget.ImageView backprs;
        private Stack<AndroidX.Fragment.App.Fragment> mstackFragment;
        string tot, nameperson, idusr;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            backprs = FindViewById<Android.Widget.ImageView>(Resource.Id.backprs);
            backprs.Click += Backprs_Click;
            backprs.Visibility = ViewStates.Gone;
            homeFragment = new HomeFragment();
            Chats_Fragment = new ChatsFragment();
            

            mstackFragment = new Stack<AndroidX.Fragment.App.Fragment>();
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            tot = Intent.GetStringExtra("user");
            idusr = tot.Substring(0, tot.IndexOf('#'));
            tot = tot.Remove(0, tot.IndexOf('#')+1);
            nameperson = tot;

            Bundle mybundle = new Bundle();
            mybundle.PutString("MyDataTag", idusr);
            homeFragment.Arguments= mybundle;
            Chats_Fragment.Arguments = mybundle;

            Snackbar.Make(Window.DecorView.RootView, "Welcome " + nameperson, Snackbar.LengthLong)
                    .SetAction("Thanks", (v) => { })
                    .Show();
            
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.fragmentContainer, Chats_Fragment, "Fragment2");
            trans.Hide(Chats_Fragment);
            trans.Add(Resource.Id.fragmentContainer, homeFragment, "Fragment1");
            trans.Commit();

            Currentfragment = homeFragment;
        }

        private void Backprs_Click(object sender, EventArgs e)
        {
            ShowFragment(homeFragment);
            fab.Visibility = ViewStates.Visible;
            backprs.Visibility = ViewStates.Gone;
        }

        private void ShowFragment(AndroidX.Fragment.App.Fragment fragment)
        {
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Hide(Currentfragment);
            trans.Show(fragment);
            trans.AddToBackStack(null);
            trans.Commit();
            mstackFragment.Push(Currentfragment);
            Currentfragment = fragment;
        }
        public override void OnBackPressed()
        {
            /* DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
            if(Currentfragment!=homeFragment)
            {
                ShowFragment(homeFragment);
                fab.Visibility = ViewStates.Visible;
            }
            else { fab.Visibility = ViewStates.Gone; }*/
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }
        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Visiting community forum", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
            ShowFragment(Chats_Fragment);
            fab.Visibility = ViewStates.Gone;
            backprs.Visibility = ViewStates.Visible;
            return;

        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_camera)
            {
                // Handle the camera action
                Intent intent = new Intent(this, typeof(UserInfo));
                intent.PutExtra("data", idusr);
                StartActivity(intent);
            }
            if (id == Resource.Id.nav_history)
            {
                Intent histry= new Intent(this, typeof(MyHistory));
                histry.PutExtra("bin", idusr);
                StartActivity(histry);
            }

            else if (id == Resource.Id.nav_share)
            {
                //call
                PhoneDialer.Open("0115544000");
            }
            else if (id == Resource.Id.nav_send)
            {

            }
            else if (id == Resource.Id.nav_exitapp)
            {
                Finish();
            }
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

