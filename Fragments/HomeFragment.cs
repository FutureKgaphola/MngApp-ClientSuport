using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Mngs_Client_Support.Adapters;
using Mngs_Client_Support.DataModels;
using Mngs_Client_Support.EventListeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firebase.Database;
using Mngs_Client_Support.Helpers;
using Google.Android.Material.Button;
using Hsalf.SmileRatingLib;
using static Hsalf.SmileRatingLib.SmileRating;
using Android.Support.Design.Widget;
using System.Threading.Tasks;

namespace Mngs_Client_Support.Fragments
{
    public class HomeFragment : AndroidX.Fragment.App.Fragment,IValueEventListener, IOnSmileySelectionListener
    {
        ExpandableListAdapter listAdapter;
        ExpandableListView expListView;
        List<string> listDataHeader;
        Button btnShowSpecDialog;
        MaterialButton capturedetails;
        EditText specificIssueTxt;
        Dictionary<string, List<string>> listDataChild;
        int previousGroup = -1;
        string authid;
        String stringData;
        string selected,name,phone,email;
        private Android.App.AlertDialog dialog, dialogs;
        private Android.App.AlertDialog.Builder dialogBuilder, dialogBuilders;
        TechniciansAdapter Users;
        private RecyclerView RecycleUsers;
        private List<UserModel> userModels = new List<UserModel>();
        private UserEvent userData = new UserEvent();
        private SmileRating smbtn;
        private Button rate;
        private ImageView reloadimg;
        View view;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            view= inflater.Inflate(Resource.Layout.Home, container, false);
            reloadimg= view.FindViewById<ImageView>(Resource.Id.reloadimg);
            reloadimg.Click += Reloadimg_Click;
            expListView = view.FindViewById<ExpandableListView>(Resource.Id.lvExp);
            btnShowSpecDialog=view.FindViewById<Button>(Resource.Id.btnShowSpecDialog);
            btnShowSpecDialog.Click += BtnShowSpecDialog_Click;
            RecycleUsers = view.FindViewById<RecyclerView>(Resource.Id.RecyclerTechnicians);
            stringData = Arguments.GetString("MyDataTag");
            authid = stringData;
            Retrieve_Data();
            // Prepare list data
            FnGetListData();

            DatabaseReference dref = firebase_Helper.GetDatabase().GetReference("Users").Child(authid);
            dref.AddValueEventListener(this);

            //Bind list
            listAdapter = new ExpandableListAdapter(this, listDataHeader, listDataChild);
            expListView.SetAdapter(listAdapter);

            FnClickEvents();

            return view;
        }

        private void Reloadimg_Click(object sender, EventArgs e)
        {
            Retrieve_Data();
            Snackbar.Make(view, "Refreshing content", Snackbar.LengthLong)
                    .Show();
        }

        private void specify_PopUp()
        {
            dialogBuilder = new Android.App.AlertDialog.Builder(Activity);
            LayoutInflater inflater = LayoutInflater.From(Activity);
            View dialogView = inflater.Inflate(Resource.Layout.specifyIssue, null);
            capturedetails = dialogView.FindViewById<MaterialButton>(Resource.Id.specificIssue);
            capturedetails.Click += Capturedetails_Click;
            specificIssueTxt = dialogView.FindViewById<Android.Widget.EditText>(Resource.Id.specificIssueTxt);
            dialogBuilder.SetView(dialogView);
            dialogBuilder.SetCancelable(true);
            dialog = dialogBuilder.Create();
            dialog.Show();
        }

        private void Capturedetails_Click(object sender, EventArgs e)
        {
            
            selected = specificIssueTxt.Text.Trim();
            if (!string.IsNullOrEmpty(specificIssueTxt.Text.Trim()))
            {
                dialogBuilder.SetCancelable(true);
                dialog.Dismiss();
                Intent i = new Intent(Application.Context, typeof(validateApp));
                i.PutExtra("userinfo", name + "#" + phone + "#" + email + "#" + selected + "#" + authid);
                StartActivity(i);
            }
            else
            {
                specificIssueTxt.Error = "Invalid input";
            }
        }

        private void BtnShowSpecDialog_Click(object sender, EventArgs e)
        {
            specify_PopUp();
        }

        public void Retrieve_Data()
        {
            userData.Retrive_Users(authid.Trim());

            userData.RetriveUsers += UserData_RetriveUsers;

        }
        private void UserData_RetriveUsers(object sender, UserEvent.RetrivedUsersEventHandeler e)
        {
            userModels = e.UserList;
            setupuserRecycler();
        }


        private void setupuserRecycler()
        {
            LinearLayoutManager linearLayoutManager = new LinearLayoutManager(Application.Context);
            Users = new TechniciansAdapter(userModels);
            RecycleUsers.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false));
            RecycleUsers.SetAdapter(Users);
            if (userModels.Count > 0)
            {
                Users.NotifyDataSetChanged();
            }
            else if (userModels.Count == 0)
            {
                Users.NotifyDataSetChanged();
            }
            Users.ItemClick += Users_ItemClick;
            Users.ItemdiscontinueClick += Users_ItemdiscontinueClick;
            Users.binbtnClick += Users_binbtnClick;
        }
        private void Users_binbtnClick(object sender, TechniciansAdapterClickEventArgs e)
        {
            firebase_Helper.GetDatabase().GetReference("Users").Child(userModels[e.Position].techid).Child("tasked").SetValue("no");
            firebase_Helper.GetDatabase().GetReference("Job_Details").Child(userModels[e.Position].jobId).Child("bin").SetValue("1");
            firebase_Helper.GetDatabase().GetReference("Job_Details").Child(userModels[e.Position].jobId).Child("status").SetValue("closed");
            Snackbar.Make(view, "Task moved to your History", Snackbar.LengthLong)
                    .Show();
            Retrieve_Data();
        }

        private void Users_ItemdiscontinueClick(object sender, TechniciansAdapterClickEventArgs e)
        {
            //untask then delete job
            firebase_Helper.GetDatabase().GetReference("Users").Child(userModels[e.Position].techid).Child("tasked").SetValue("no");
            firebase_Helper.GetDatabase().GetReference("Users").Child(userModels[e.Position].techid).Child("jobID").SetValue("none");
            firebase_Helper.GetDatabase().GetReference("Job_Details").Child(userModels[e.Position].jobId).RemoveValue();
            Snackbar.Make(view, "Discontinued succesfully", Snackbar.LengthLong)
                    .Show();
            Retrieve_Data();
        }

        private void rate_PopUp()
        {
            dialogBuilders = new Android.App.AlertDialog.Builder(Activity);
            LayoutInflater inflater = LayoutInflater.From(Activity);
            View dialogView = inflater.Inflate(Resource.Layout.ratings, null);
            smbtn = dialogView.FindViewById<SmileRating>(Resource.Id.smbtn);
            rate = dialogView.FindViewById<Button>(Resource.Id.rate);
            rate.Click += Rate_Click;
            dialogBuilders.SetView(dialogView);
            dialogBuilders.SetCancelable(true);
            dialogs = dialogBuilders.Create();
            dialogs.Show();

            smbtn.SetOnSmileySelectionListener(this);

        }

        private void Rate_Click(object sender, EventArgs e)
        {
             firebase_Helper.GetDatabase().GetReference("Job_Details").Child(job_ID).Child("isRated").SetValue("yes");
            firebase_Helper.GetDatabase().GetReference("Job_Details").Child(job_ID).Child("rating").SetValue(rated.ToString());
            dialogs.Dismiss();
            Snackbar.Make(view, "Hurray, thanks for your rating", Snackbar.LengthLong)
                    .Show();
            Retrieve_Data();
        }
        string job_ID;
        private void Users_ItemClick(object sender, TechniciansAdapterClickEventArgs e)
        {
            if (Convert.ToInt32(userModels[e.Position].progCount)<100)
            {
               
            }
            else
            {
                job_ID = userModels[e.Position].jobId;
                rate_PopUp();

                //Toast.MakeText(Application.Context, "This Job has been completed", ToastLength.Short).Show();
            }

        }

        void FnClickEvents()
        {
            //Listening to child item selection
            expListView.ChildClick += delegate (object sender, ExpandableListView.ChildClickEventArgs e) {
                selected = expListView.GetItemAtPosition(expListView.IndexOfChild(e.ClickedView)).ToString().Trim();
                Intent i = new Intent(Application.Context, typeof(validateApp));
                i.PutExtra("userinfo", name+"#"+phone+"#"+email+"#"+ selected+"#"+authid);
                StartActivity(i);

            };

            //Listening to group expand
            //modified so that on selection of one group other opened group has been closed
            expListView.GroupExpand += delegate (object sender, ExpandableListView.GroupExpandEventArgs e) {

                if (e.GroupPosition != previousGroup)
                    expListView.CollapseGroup(previousGroup);
                previousGroup = e.GroupPosition;
            };

            //Listening to group collapse
            expListView.GroupCollapse += delegate (object sender, ExpandableListView.GroupCollapseEventArgs e) {
                //Toast.MakeText(this, "group collapsed", ToastLength.Short).Show();
            };

        }
        void FnGetListData()
        {
            listDataHeader = new List<string>();
            listDataChild = new Dictionary<string, List<string>>();

            // Adding child data
            listDataHeader.Add("Network");
            listDataHeader.Add("Printer");
            listDataHeader.Add("Sofware Installation");

            // Adding child data
            var lstCS = new List<string>();
            lstCS.Add("No internet detected");
            lstCS.Add("Set up of Ethernet or Wiffi");
            lstCS.Add("Weak Wi-Fi signal");

            var lstEC = new List<string>();
            lstEC.Add("Can't connect to printer");
            lstEC.Add("Out of Paper");
            lstEC.Add("Out of ink");
            lstEC.Add("Set up new/old printer");


            var lstMech = new List<string>();
            lstMech.Add("Microsoft Office");
            lstMech.Add("Adobe (pdf reader)");
            lstMech.Add("Install Operating System");



            // Header, Child data
            listDataChild.Add(listDataHeader[0], lstCS);
            listDataChild.Add(listDataHeader[1], lstEC);
            listDataChild.Add(listDataHeader[2], lstMech);
        }

        public void OnCancelled(DatabaseError error)
        {
            
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            try
            {
                if(snapshot.Exists())
                {
                    name = snapshot.Child("username").Value.ToString();
                    phone = snapshot.Child("phone").Value.ToString();
                    email = snapshot.Child("email").Value.ToString();
                    
                }
            }
            catch (Exception)
            {

                
            }
        }
        int rated = 0;
        public void OnSmileySelected(int smiley, bool reselected)
        {
            try
            {
                switch (smiley)
                {
                    case SmileRating.Terrible:
                        rated = 0;
                        break;
                    case SmileRating.Bad:
                        rated = 25;
                        break;
                    case SmileRating.Okay:
                        rated = 50;
                        break;
                    case SmileRating.Good:
                        rated = 75;
                        break;
                    case SmileRating.Great:
                        rated = 100;
                        break;
                }

            }catch(Exception r)
            {
                Toast.MakeText(Application.Context, r.Message.ToString(), ToastLength.Long).Show();
            }
        }
    }
}