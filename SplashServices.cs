﻿using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Mngs_Client_Support.Helpers;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Mngs_Client_Support
{
    [Activity(Label = "Mngs Support", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SplashServices : Activity
    {
        bool isGooglePlayServicesInstalled;
        public static readonly int RC_INSTALL_GOOGLE_PLAY_SERVICES = 1000;
        public static readonly string TAG = "XamarinDevice";
        private AlertDialog.Builder dialogB;
        private AlertDialog dialogf;
        private int month = System.DateTime.Now.Month;
        private int day = System.DateTime.Now.Day;
        private int year = System.DateTime.Now.Year;
        View view;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Set our view from the "main" layout resource
            isGooglePlayServicesInstalled = TestIfGooglePlayServicesIsInstalled();
            if(isGooglePlayServicesInstalled==true)
            {
                var config = new Gr.Net.MaroulisLib.EasySplashScreen(this)
                .WithFullScreen()
                .WithTargetActivity(Java.Lang.Class.FromType(typeof(login)))
                .WithSplashTimeOut(4000)
                .WithLogo(Resource.Mipmap.mngNoB)
                .WithFooterText("MNGS Client Support")
                .WithBackgroundColor(Color.ParseColor("#FFFFFF"));
                view = config.Create();
                SetContentView(view);
                RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            }
            else
            {
                try
                {
                    var config = new Gr.Net.MaroulisLib.EasySplashScreen(this)
                            .WithFullScreen()
                            .WithSplashTimeOut(4000)
                            .WithLogo(Resource.Mipmap.mngNoB)
                            .WithFooterText("MNGS Client Support")
                            .WithBackgroundColor(Color.ParseColor("#FFFFFF"));
                    view = config.Create();
                    SetContentView(view);
                    RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
                    errorDialog();
                    Task.Delay(5000);
                    Finish();
                }
                catch (Exception)
                {
                    Toast.MakeText(this, "There is a problem with Google Play Services on this device", ToastLength.Long).Show();
                    Finish();
                }
            }

            
        }


        private void errorDialog()
        {
            dialogB = new Android.App.AlertDialog.Builder(this);
            LayoutInflater inflater = LayoutInflater.From(this);
            View dialogView = inflater.Inflate(Resource.Layout.serviceError, null);
            dialogB.SetView(dialogView);
            dialogB.SetCancelable(false);
            dialogf = dialogB.Create();
            dialogf.Show();
        }
        bool TestIfGooglePlayServicesIsInstalled()
        {
            var queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (queryResult == ConnectionResult.Success)
            {
                Log.Info(TAG, "Google Play Services is installed on this device.");
                //Toast.MakeText(this, "Google Play Services is installed on this device.", ToastLength.Long).Show();
                return true;
            }

            if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            {
                var errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
                Log.Error(TAG, "There is a problem with Google Play Services on this device: {0} - {1}", queryResult, errorString);
                //report error
                reportError();
                Toast.MakeText(this, "There is a problem with Google Play Services on this device", ToastLength.Long).Show();

            }

            return false;
        }
        private void reportError()
        {
            HashMap data = new HashMap();
            data.Put("device", DeviceInfo.Model.ToString());
            data.Put("manufacturer", DeviceInfo.Manufacturer.ToString());
            data.Put("deviceName", DeviceInfo.Name.ToString());
            data.Put("version", DeviceInfo.VersionString.ToString());
            data.Put("platform", DeviceInfo.Platform.ToString());
            data.Put("idiom", DeviceInfo.Idiom.ToString());
            data.Put("deviceType", DeviceInfo.DeviceType.ToString());
            data.Put("DateError", day.ToString() + '/' + month.ToString() + '/' + year.ToString());
            data.Put("urlImg", " ");
            DatabaseReference databaseRef = firebase_Helper.GetDatabase().GetReference("UnsuportedDevices").Push();
            databaseRef.SetValue(data);
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (RC_INSTALL_GOOGLE_PLAY_SERVICES == requestCode && resultCode == Result.Ok)
            {
                isGooglePlayServicesInstalled = true;
            }
            else
            {
                Log.Warn(TAG, $"Don't know how to handle resultCode {resultCode} for request {requestCode}.");
            }
        }
    }
}