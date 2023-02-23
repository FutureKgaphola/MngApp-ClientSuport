﻿using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Firebase.Database;
using Google.Android.Material.Button;
using Google.Android.Material.Snackbar;
using Google.Android.Material.TextField;
using Mngs_Client_Support.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mngs_Client_Support
{
    [Activity(Label = "login", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class login : Activity, IValueEventListener, IOnFailureListener, IOnSuccessListener
    {
        ProgressBar progBar;
        MaterialButton procced;
        TextInputEditText username, password;
        private FirebaseAuth auth;
        TextView resetpass, register;
        public void OnCancelled(DatabaseError error)
        {

        }
        string pass, user, tasked, id, prefferedName, account;
        public void OnDataChange(DataSnapshot snapshot)
        {
            if (snapshot.Exists())
            {
                pass = snapshot.Child("pass").Value.ToString();
                user = snapshot.Child("email").Value.ToString();
                tasked = snapshot.Child("tasked").Value.ToString();
                prefferedName = snapshot.Child("username").Value.ToString();
                account = snapshot.Child("AccountType").Value.ToString();

                if (user.ToLower() == username.Text.ToLower() && pass == password.Text && account == "Client")
                {
                    firebase_Helper.GetDatabase().GetReference("Users").Child(auth.Uid).Child("pass").SetValue(password.Text);
                    progBar.Visibility = ViewStates.Gone;
                    password.Text = "";
                    username.Text = "";
                    Intent intent = new Intent(this, typeof(MainActivity));
                    intent.PutExtra("user", id + "#" + prefferedName);
                    StartActivity(intent);
                }

                if (user.ToLower() == username.Text.ToLower() && account == "Technician")
                {
                    firebase_Helper.GetDatabase().GetReference("Users").Child(auth.Uid).Child("pass").SetValue(password.Text);
                    progBar.Visibility = ViewStates.Gone;
                    password.Text = "";
                    username.Text = "";
                    Intent intent = new Intent(this, typeof(Technician.TechnicianHome));
                    intent.PutExtra("taskedNid", id);
                    StartActivity(intent);
                }
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login);
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            resetpass = FindViewById<TextView>(Resource.Id.resetpass);
            register = FindViewById<TextView>(Resource.Id.register);
            register.Click += Register_Click;
            resetpass.Click += Resetpass_Click;
            progBar = FindViewById<ProgressBar>(Resource.Id.progBar);
            username = FindViewById<TextInputEditText>(Resource.Id.username);
            password = FindViewById<TextInputEditText>(Resource.Id.password);
            procced = FindViewById<MaterialButton>(Resource.Id.procced);
            procced.Click += Procced_Click;
            // Create your application here
        }

        private void Register_Click(object sender, EventArgs e)
        {
            Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(this);
            builder.SetTitle("registration");
            builder.SetIcon(Resource.Mipmap.mngNoB);
            builder.SetMessage("Select account you want to register for");
            builder.SetNeutralButton("Technician", delegate
            {
                builder.Dispose();
                Intent intent = new Intent(this, typeof(Technician.addUser));
                StartActivity(intent);

            });
            builder.SetPositiveButton("Client", delegate
            {
                builder.Dispose();
                Intent intent = new Intent(this, typeof(addUser));
                StartActivity(intent);
            });
            builder.Show();
        }

        private void Resetpass_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(forgotpassword));
            StartActivity(intent);
        }

        private void Procced_Click(object sender, EventArgs e)
        {
            if (validfound() == true)
            {
                progBar.Visibility = ViewStates.Visible;
                auth = new firebase_Helper().GetFirebaseAuth();
                auth.SignInWithEmailAndPassword(username.Text, password.Text)
                    .AddOnSuccessListener(this)
                    .AddOnFailureListener(this);
            }
        }
        private bool validfound()
        {
            bool Result = true;

            if (string.IsNullOrEmpty(username.Text.Trim()))
            {
                Result = false;
                username.Error = "invalid input";
            }
            if (string.IsNullOrEmpty(password.Text.Trim()))
            {
                Result = false;
                password.Error = "invalid input";
            }

            return Result;
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            id = auth.CurrentUser.Uid;
            firebase_Helper.GetDatabase().GetReference("Users").Child(id)
              .AddValueEventListener(this);
        }

        public void OnFailure(Java.Lang.Exception e)
        {
            progBar.Visibility = ViewStates.Gone;
            Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(this);
            builder.SetTitle("Error");
            builder.SetIcon(Resource.Mipmap.mngNoB);
            builder.SetMessage("Something went wrong: " + e.Message.ToString());
            builder.SetNeutralButton("OK", delegate
            {
                builder.Dispose();
            });
            builder.Show();
        }
    }
}