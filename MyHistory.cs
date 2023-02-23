using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
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
    [Activity(Label = "MyHistory")]
    public class MyHistory : Activity
    {
        private RecyclerView Rcycle_jobs;
        private BinEvent JobsEvent = new BinEvent();
        private BinAdapter jobsAdapter;
        string idstr;
        LinearLayout parentLiner;
        private List<BinModel> jobsModels = new List<BinModel>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.History);
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            // Create your application here
            Rcycle_jobs = FindViewById<RecyclerView>(Resource.Id.recyclerHistory);
            parentLiner= FindViewById<LinearLayout>(Resource.Id.parentLiner);
            idstr = Intent.GetStringExtra("bin");
            Get_Jobs();
        }

        private void Get_Jobs()
        {
            JobsEvent.Retrive_Users(idstr);
            JobsEvent.RetriveUsers += JobsEvent_RetriveUsers;
        }

        private void JobsEvent_RetriveUsers(object sender, BinEvent.RetrivedUsersEventHandeler e)
        {
            jobsModels = e.UserList;
            if(jobsModels!=null && jobsModels.Count>0)
            {
                parentLiner.Visibility = ViewStates.Gone;
                Rcycle_jobs.Visibility = ViewStates.Visible;
                SetupJobRecycler();
            }
            else
            {
                parentLiner.Visibility = ViewStates.Visible;
                Rcycle_jobs.Visibility = ViewStates.Gone;
            }
            
        }

        private void SetupJobRecycler()
        {
            LinearLayoutManager linearLayoutManager = new LinearLayoutManager(Application.Context);
            jobsAdapter = new BinAdapter(jobsModels);
            Rcycle_jobs.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false));
            Rcycle_jobs.SetAdapter(jobsAdapter);
            if (jobsModels.Count > 0)
            {
                jobsAdapter.NotifyDataSetChanged();
            }else if (jobsModels.Count == 0)
            {
                jobsAdapter.NotifyDataSetChanged();
            }
            jobsAdapter.ItemLongClick += JobsAdapter_ItemLongClick;
        }

        private void JobsAdapter_ItemLongClick(object sender, BinAdapterClickEventArgs e)
        {
            Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(this);
            builder.SetTitle("warning");
            builder.SetIcon(Resource.Mipmap.mngNoB);
            builder.SetMessage("Permenantly remove this item?");
            builder.SetNeutralButton("yes", delegate
            {
                builder.Dispose();
                firebase_Helper.GetDatabase().GetReference("Job_Details").Child(jobsModels[e.Position].JobId).Child("clientID").SetValue(jobsModels[e.Position].Client+"DELETED");//JST UPDATE FIELD TO HIDE IT AWAY FROM USER'S BIN. TECHNICIAN WILL STILL SEE IT AS THEIR HISTORY
                //firebase_Helper.GetDatabase().GetReference("Job_Details").Child(jobsModels[e.Position].JobId).RemoveValue();
                Snackbar.Make(Window.DecorView.RootView, "Record cleared from history.", Snackbar.LengthLong)
                    .Show();
                jobsModels.Remove(jobsModels[e.Position]);
                jobsAdapter.NotifyDataSetChanged();
            });
            builder.SetPositiveButton("no", delegate
            {
                builder.Dispose();
                
            });
            builder.Show();

            
        }
    }
}