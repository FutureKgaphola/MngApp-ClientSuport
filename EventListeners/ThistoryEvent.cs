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
    public class ThistoryEvent : Java.Lang.Object, IValueEventListener
    {
        private string curentUid;
        private List<ThistoryModel> UserPos = new List<ThistoryModel>();

        public event EventHandler<RetrivedUsersEventHandeler> RetriveUsers;
        public class RetrivedUsersEventHandeler : EventArgs
        {
            public List<ThistoryModel> UserList { get; set; }
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
                        if (data.Child("techid").Value.ToString() == this.curentUid
                              && data.Child("status").Value.ToString().ToLower() == "closed")
                        {
                            ThistoryModel uM = new ThistoryModel
                            {
                                JobId = data.Key,
                                JobLocation = data.Child("JobLocation").Value.ToString(),
                                Client = data.Child("Client").Value.ToString(),
                                task = data.Child("task").Value.ToString(),
                                status = data.Child("status").Value.ToString(),
                                isRated = data.Child("isRated").Value.ToString(),
                                rating= data.Child("rating").Value.ToString()

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