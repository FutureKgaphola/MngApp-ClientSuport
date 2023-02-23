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
    public class UserEvent : Java.Lang.Object, IValueEventListener
    {
        private string curentUid;
        private List<UserModel> UserPos = new List<UserModel>();

        public event EventHandler<RetrivedUsersEventHandeler> RetriveUsers;
        public class RetrivedUsersEventHandeler : EventArgs
        {
            public List<UserModel> UserList { get; set; }
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
                            && data.Child("Technician").Value.ToString().ToLower()!="none"
                              && data.Child("bin").Value.ToString().ToLower() == "0")
                        {
                            UserModel uM = new UserModel
                            {
                                jobId = data.Key,
                                techName = data.Child("Technician").Value.ToString(),
                                task = data.Child("task").Value.ToString(),
                                techid = data.Child("techid").Value.ToString(),
                                status = data.Child("status").Value.ToString(),
                                JobLocation = data.Child("JobLocation").Value.ToString(),
                                progCount = data.Child("progCount").Value.ToString(),
                                ratedmsg = "Rated : " + data.Child("isRated").Value.ToString()
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