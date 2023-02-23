using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Mngs_Client_Support.DataModels;
using Mngs_Client_Support.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mngs_Client_Support.EventListeners
{
    public class BinEvent : Java.Lang.Object, IValueEventListener
    {
        private string curentUid;
        private List<BinModel> UserPos = new List<BinModel>();

        public event EventHandler<RetrivedUsersEventHandeler> RetriveUsers;
        public class RetrivedUsersEventHandeler : EventArgs
        {
            public List<BinModel> UserList { get; set; }
        }

        public void OnCancelled(DatabaseError error)
        {

        }
        public void OnDataChange(DataSnapshot snapshot)
        {
            try
            {
                if (snapshot.Value != null)
                {
                    UserPos.Clear();
                    var child = snapshot.Children.ToEnumerable<DataSnapshot>();
                    foreach (DataSnapshot data in child)
                    {
                        if (data.Child("clientID").Value.ToString() == this.curentUid
                              && data.Child("bin").Value.ToString().ToLower() == "1")
                        {
                            BinModel uM = new BinModel
                            {
                                JobId = data.Key,
                                JobLocation = data.Child("JobLocation").Value.ToString(),
                                Technician = data.Child("Technician").Value.ToString(),
                                Client = data.Child("Client").Value.ToString(),
                                task = data.Child("task").Value.ToString(),
                                status = data.Child("status").Value.ToString(),
                                lat = data.Child("lat").Value.ToString(),
                                lon = data.Child("lon").Value.ToString()
                            };
                            UserPos.Add(uM);
                        }

                    }
                    RetriveUsers.Invoke(this, new RetrivedUsersEventHandeler
                    {
                        UserList = UserPos
                    });
                }
                else
                {

                }
            }
            catch (System.Exception fetch)
            {
                Toast.MakeText(Application.Context, "Error: couldn't fetch data as expected: " + fetch.Message.ToString(), ToastLength.Long).Show();
            }
        }
        public void Retrive_Users(string curentUid)
        {
            this.curentUid = curentUid;
            DatabaseReference dref = firebase_Helper.GetDatabase().GetReference("Job_Details");
            dref.AddValueEventListener(this);
        }
    }
}