using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using Mngs_Client_Support.Adapters;
using Mngs_Client_Support.DataModels;
using Mngs_Client_Support.EventListeners;
using Mngs_Client_Support.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mngs_Client_Support
{
    [Activity(Label = "UserInfo", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class UserInfo : Activity, IValueEventListener
    {
        MaterialButton updateprofile;
        TextInputEditText phone, preferdName;
        TextView UseName, AccountType, UserMail;

        string IDstring;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.driverProfile);
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

            updateprofile = FindViewById<MaterialButton>(Resource.Id.updateprofile);
            UseName = FindViewById<TextView>(Resource.Id.UseName);
            AccountType = FindViewById<TextView>(Resource.Id.AccountType);
            UserMail = FindViewById<TextView>(Resource.Id.UserMail);
            phone = FindViewById<TextInputEditText>(Resource.Id.phone);
            preferdName = FindViewById<TextInputEditText>(Resource.Id.preferdName);
            updateprofile.Click += Updateprofile_Click;

            
            IDstring = Intent.GetStringExtra("data");
            DatabaseReference dref = firebase_Helper.GetDatabase().GetReference("Users").Child(IDstring);
            dref.AddValueEventListener(this);

        }

        private void ImgtwoEmail_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Intent.ActionSend);
            intent.SetType("plain/text");
            intent.PutExtra(Intent.ExtraEmail, new String[] { UserMail.Text.Trim() });
            intent.PutExtra(Intent.ExtraSubject, "subject");
            intent.PutExtra(Intent.ExtraText, "mail body");
            if (intent.ResolveActivity(PackageManager) != null)
            {
                StartActivity(Intent.CreateChooser(intent, ""));
            }
        }
        

        public override void OnBackPressed()
        {
            Finish();
        }

        private void Updateprofile_Click(object sender, EventArgs e)
        {
            if(isNotNull()==true)
            {
                try
                {
                    
                    firebase_Helper.GetDatabase().GetReference("Users").Child(IDstring).Child("username").SetValue(preferdName.Text);
                    firebase_Helper.GetDatabase().GetReference("Users").Child(IDstring).Child("phone").SetValue(phone.Text);
                    

                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("notification");
                    builder.SetIcon(Resource.Mipmap.mngNoB);
                    builder.SetMessage("successfully updated");
                    builder.SetPositiveButton("ok", delegate
                    {
                        builder.Dispose();
                        
                    });
                    builder.Show();

                }
                catch (System.Exception er)
                {
                    /*View view = (View)sender;
                    Snackbar.Make(view, "error: " + er.Message.ToString(), Snackbar.LengthLong)
                        .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();*/
                    Toast.MakeText(this, "error: " + er.Message.ToString(), ToastLength.Long).Show();
                }
            }
        }

        private bool isNotNull()
        {
            bool result = true;
            if(string.IsNullOrEmpty(UseName.Text.Trim()))
            {
                UseName.Error = "prefered name not set";
                result = false;
            }
            if (string.IsNullOrEmpty(preferdName.Text.Trim()))
            {
                preferdName.Error = "prefered name not set";
                result = false;
            }

            if (string.IsNullOrEmpty(UserMail.Text.Trim()))
            {
                UserMail.Error = "Badly formated email";
                result = false;
            }
            
            if (string.IsNullOrEmpty(phone.Text.Trim()) || phone.Text.Trim().Length<10)
            {
                phone.Error = "Badly formated phone number";
                result = false;
            }

            return result;
        }

        public void OnCancelled(DatabaseError error)
        {
            
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            if(snapshot.Exists())
            {
                preferdName.Text = snapshot.Child("username").Value.ToString();
                UseName.Text = preferdName.Text;
                phone.Text =snapshot.Child("phone").Value.ToString();
                //TxtplateNum.Text= snapshot.Child("plate").Value.ToString();
                UserMail.Text= snapshot.Child("email").Value.ToString();
                AccountType.Text = snapshot.Child("AccountType").Value.ToString();
            }
            
        }
    }
}