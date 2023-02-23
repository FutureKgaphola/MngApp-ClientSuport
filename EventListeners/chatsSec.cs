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
    public class chatsSec : Java.Lang.Object, IValueEventListener
    {
        private List<chatsModel> UserPos = new List<chatsModel>();

        public event EventHandler<RetrivedUsersEventHandeler> RetriveUsers;
        public class RetrivedUsersEventHandeler : EventArgs
        {
            public List<chatsModel> UserList { get; set; }
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
                        chatsModel uM = new chatsModel
                        {
                            IDmsg = data.Key,
                            personPosted = data.Child("personPosted").Value.ToString(),
                            image = data.Child("image").Value.ToString(),
                            msg = data.Child("msg").Value.ToString(),
                            userID = data.Child("userID").Value.ToString(),
                            datechat = data.Child("datechat").Value.ToString(),

                        };
                        UserPos.Add(uM);
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
        public void Retrive_Users()
        {
            DatabaseReference dref = firebase_Helper.GetDatabase().GetReference("MngsCommunity");
            dref.AddValueEventListener(this);
        }
    }
}