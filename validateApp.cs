using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Google.Android.Material.Snackbar;
using Java.Util;
using Mngs_Client_Support.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using NetworkAccess = Xamarin.Essentials.NetworkAccess;

namespace Mngs_Client_Support
{
    [Activity(Label = "validateApp")]
    public class validateApp : Activity, Android.Support.V4.App.ActivityCompat.IOnRequestPermissionsResultCallback
    {
        ProgressBar progBar;
        EditText GetOfficeTxt;
        TextView GetCurrentTxt, permissionTxt, latlonTxt;
        int passcount=0;
        bool permissinGranted = false;
        readonly string[] permissionGroupLocation = { Manifest.Permission.AccessFineLocation, Manifest.Permission.AccessCoarseLocation };
        const int requestLocationId = 0;
        private string GetCurrentLocationerror="..";
        double commuterlat=0.0,commuterlon=0.0;
        private string totstr, emailTxt,phone, preferdName, issue, myid, fulladdress="Couldn't define your address";
        Button goin;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.validateapp);
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            progBar = FindViewById<ProgressBar>(Resource.Id.progBar);
            permissionTxt = FindViewById<TextView>(Resource.Id.permissionTxt);
            GetCurrentTxt = FindViewById<TextView>(Resource.Id.GetCurrentTxt);
            GetOfficeTxt = FindViewById<EditText>(Resource.Id.GetOfficeTxt);
            latlonTxt = FindViewById<TextView>(Resource.Id.latlonTxt);
            goin= FindViewById<Button>(Resource.Id.goin);
            goin.Click += Goin_Click;
            if(Intent.GetStringExtra("userinfo") !=null)
            {
                totstr = Intent.GetStringExtra("userinfo").Trim();
                preferdName = totstr.Substring(0, totstr.IndexOf("#"));
                totstr = totstr.Remove(0, totstr.IndexOf("#") + 1);
                phone= totstr.Substring(0, totstr.IndexOf("#"));
                totstr = totstr.Remove(0, totstr.IndexOf("#") + 1);
                emailTxt= totstr.Substring(0, totstr.IndexOf("#"));
                totstr = totstr.Remove(0, totstr.IndexOf("#") + 1);
                issue = totstr.Substring(0, totstr.IndexOf("#"));
                totstr = totstr.Remove(0, totstr.IndexOf("#") + 1);
                myid = totstr.Trim();

            }
            
            checkLocationpermission();
            setErrormessages();
            GetCurrentLocation();
            
            // Create your application here
        }

        private void Goin_Click(object sender, EventArgs e)
        {
            submitIsuess();
            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
            builder.SetTitle("Submitted");
            builder.SetMessage("Submitted successfully, an agent will be assigned to you soon");
            builder.SetNeutralButton("ok", delegate
            {
                builder.Dispose();
                Finish();

            });
            
            builder.Show();
        }
        

        private void submitIsuess()
        {
            if (string.IsNullOrEmpty(GetOfficeTxt.Text.Trim()))
            {
                HashMap data = new HashMap();
                data.Put("Client", preferdName);
                data.Put("clientID", myid);
                data.Put("techid", "none");
                data.Put("task", issue);
                data.Put("Technician", "none");
                data.Put("officeNo", "none");
                data.Put("status", "open");
                data.Put("progCount", "0");
                data.Put("JobLocation", fulladdress);
                data.Put("lat", commuterlat);
                data.Put("lon", commuterlat);
                data.Put("rating", "0");
                data.Put("bin", "0");
                data.Put("isRated", "no");
                data.Put("clientphone", phone);
                data.Put("techphone", "0000000000");
                DatabaseReference databaseRef = firebase_Helper.GetDatabase().GetReference("Job_Details").Push();
                databaseRef.SetValue(data);
                
            }
            else
            {
                HashMap data = new HashMap();
                data.Put("Client", preferdName);
                data.Put("clientID", myid);
                data.Put("techid", "none");
                data.Put("task", issue);
                data.Put("Technician", "none");
                data.Put("officeNo", GetOfficeTxt.Text.Trim());
                data.Put("status", "open");
                data.Put("progCount", "0");
                data.Put("JobLocation", fulladdress);
                data.Put("lat", commuterlat);
                data.Put("lon", commuterlat);
                data.Put("rating", "0");
                data.Put("bin", "0");
                data.Put("isRated", "no");
                data.Put("clientphone", phone);
                data.Put("techphone", "0000000000");
                DatabaseReference databaseRef = firebase_Helper.GetDatabase().GetReference("Job_Details").Push();
                databaseRef.SetValue(data);
                
            }
        }

        private void setErrormessages()
        {
            if(permissinGranted==false)
            {
                permissionTxt.Text = "permission not granted";
            }
            else {
                permissionTxt.Text = "Location Permission granted";
                passcount = passcount++; }
            if (GetCurrentLocationerror.Length>2)
            {
                GetCurrentTxt.Text = "permission not granted(make sure device location is turned on)";
            }else {
                GetCurrentTxt.Text = "Device location granted";
                passcount = passcount++; }

        }

        private bool checkLocationpermission()
        {
            if (Android.Support.V4.App.ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Android.Content.PM.Permission.Granted
                && Android.Support.V4.App.ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != Android.Content.PM.Permission.Granted)
            {
                permissinGranted = false;
                RequestPermissions(permissionGroupLocation, requestLocationId);
            }
            else
            {
                permissinGranted = true;
            }
            return permissinGranted;

        }



        CancellationTokenSource cts;
        async void GetCurrentLocation()
        {

            if (permissinGranted==true)
            {
                try
                {
                    //
                    if (cts != null && !cts.IsCancellationRequested)
                        cts.Cancel();

                    var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                    cts = new CancellationTokenSource();
                    var location = await Geolocation.GetLocationAsync(request, cts.Token);

                    if (location != null)
                    {
                        commuterlat = location.Latitude;
                        commuterlon = location.Longitude;
                        getfullAddress();
                        latlonTxt.Text = "Latitude: " + commuterlat.ToString() + " Longitude: " + commuterlon.ToString();
                        goin.Visibility = ViewStates.Visible;
                        progBar.Visibility = ViewStates.Gone;

                    }
                    else
                    {
                        Android.Widget.Toast.MakeText(this, "no location detected. try enabling device location ", Android.Widget.ToastLength.Long).Show();
                        GetCurrentLocationerror = "no location detected. try enabling device location ";
                        progBar.Visibility = ViewStates.Gone;
                    }
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    GetCurrentLocationerror = fnsEx.Message;
                    GetCurrentTxt.Text = GetCurrentLocationerror;
                    permissionTxt.Text = "permission not granted(make sure device location is turned on)";
                    passcount = 0;
                    progBar.Visibility = ViewStates.Gone;
                }
                catch (FeatureNotEnabledException fneEx)
                {
                    GetCurrentLocationerror = fneEx.Message;
                    GetCurrentTxt.Text = GetCurrentLocationerror;
                    permissionTxt.Text = "permission not granted(make sure device location is turned on)";
                    passcount = 0;
                    progBar.Visibility = ViewStates.Gone;
                }
                catch (PermissionException pEx)
                {
                    // Handle permission exception
                    
                    GetCurrentLocationerror = pEx.Message;
                    GetCurrentTxt.Text = GetCurrentLocationerror;
                    permissionTxt.Text = "permission not granted(make sure device location is turned on)";
                    passcount = 0;
                    progBar.Visibility = ViewStates.Gone;
                }
                catch (Exception ex)
                {
                    // Unable to get location
                    GetCurrentLocationerror = ex.Message;
                    GetCurrentTxt.Text = GetCurrentLocationerror;
                    permissionTxt.Text = "permission not granted(make sure device location is turned on)";
                    passcount = 0;
                    progBar.Visibility = ViewStates.Gone;
                }
            }
            else
            {
                Android.Widget.Toast.MakeText(this, "no location detected. try enabling device location ", Android.Widget.ToastLength.Long).Show();
                GetCurrentTxt.Text = "permission not granted(make sure device location is turned on)";
                permissionTxt.Text= "permission not granted(make sure device location is turned on)";
                passcount = 0;
                progBar.Visibility = ViewStates.Gone;
            }
        }

        private async void getfullAddress()
        {
            try
            {
                var placemarks = await Geocoding.GetPlacemarksAsync(commuterlat, commuterlon);

                var placemark = placemarks?.FirstOrDefault();
                if (placemark != null)
                {
                    var geocodeAddress =
                        $"Building number: {placemark.SubThoroughfare}\n" +
                        $"Street:    {placemark.Thoroughfare}\n" +
                        $"Locality:        {placemark.Locality}\n" +
                        $"Sub Locality:     {placemark.SubLocality}\n" +
                        $"Province:       {placemark.AdminArea}\n";

                    fulladdress = geocodeAddress.ToString();
                    
                }
            }
            catch (Exception ex)
            {

                Android.Widget.Toast.MakeText(Application.Context, "Error:  " + ex.Message.ToString(), Android.Widget.ToastLength.Long).Show();
            }
        }
    }
}