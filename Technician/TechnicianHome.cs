using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Mngs_Client_Support.Adapters;
using Mngs_Client_Support.DataModels;
using Mngs_Client_Support.EventListeners;
using Mngs_Client_Support.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Timers;
using Xamarin.Essentials;
using static Android.Widget.SeekBar;
using NetworkAccess = Xamarin.Essentials.NetworkAccess;

namespace Mngs_Client_Support.Technician
{
    [Activity(Label = "TechnicianHome")]
    public class TechnicianHome : Activity, IValueEventListener, IOnSeekBarChangeListener
    {
        private TextView txtTimer;
        private Button btnStart;
        private Button btnPasuse;
        private Button btnStop;
        SeekBar Sbar;
        private string txt_address;
        string idjob="", dbprog="";
        private string clientID;
        private string clientphone;
        private string techphone;
        private string lat;
        private string lon;
        private string officeNo;
        private TextView UseName, txtclient, Txtjob, txtaddress;
        private RelativeLayout detailsview;
        int visit = 1;
        private Button show;
        private RecyclerView Rcycle_jobs;
        private ThistoryEvent JobsEvent = new ThistoryEvent();
        private ThistoryAdapter jobsAdapter;
        private List<ThistoryModel> jobsModels = new List<ThistoryModel>();
        private ImageView imgprofile;
        private string Txt_job;
        private string UseNm;
        private string txtcl;

        public void OnCancelled(DatabaseError error)
        {
            
        }
        private void resetholder()
        {
            UseName.Text = "Hi, ";
            txtclient.Text= "Client : ";
            Txtjob.Text = "Job/Issue : ";
            txtaddress.Text = "Address : ";
        }

        private void Get_Jobs()
        {
            JobsEvent.Retrive_Users(Intent.GetStringExtra("taskedNid").Trim());
            JobsEvent.RetriveUsers += JobsEvent_RetriveUsers;
        }

        private void JobsEvent_RetriveUsers(object sender, ThistoryEvent.RetrivedUsersEventHandeler e)
        {
            jobsModels = e.UserList;
            SetupJobRecycler();
        }
        private void SetupJobRecycler()
        {
            LinearLayoutManager linearLayoutManager = new LinearLayoutManager(Application.Context);
            jobsAdapter = new ThistoryAdapter(jobsModels);
            Rcycle_jobs.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false));
            Rcycle_jobs.SetAdapter(jobsAdapter);
            if (jobsModels.Count > 0)
            {
                jobsAdapter.NotifyDataSetChanged();
            }else if (jobsModels.Count == 0)
            {
                jobsAdapter.NotifyDataSetChanged();
            }
        }
        public void OnDataChange(DataSnapshot snapshot)
        {
            var child = snapshot.Children.ToEnumerable<DataSnapshot>();
            foreach (DataSnapshot data in child)
            {
                resetholder();
                if (data.Child("techid").Value.ToString().Trim() == Intent.GetStringExtra("taskedNid").Trim()
                    && data.Child("bin").Value.ToString().Trim() == "0" && data.Child("status").Value.ToString().Trim() != "closed")
                {
                    idjob = data.Key;
                    detailsview.Visibility = ViewStates.Visible;
                    UseName.Text = UseName.Text + data.Child("Technician").Value.ToString();
                    UseNm= data.Child("Technician").Value.ToString();
                    txtclient.Text = txtclient.Text + data.Child("Client").Value.ToString();
                    txtcl= data.Child("Client").Value.ToString();
                    Txtjob.Text = Txtjob.Text + data.Child("task").Value.ToString();
                    Txt_job= data.Child("task").Value.ToString();
                    txtaddress.Text = txtaddress.Text + data.Child("JobLocation").Value.ToString();
                    txt_address= data.Child("JobLocation").Value.ToString();
                    dbprog =data.Child("progCount").Value.ToString();
                    clientID = data.Child("clientID").Value.ToString();
                    clientphone = data.Child("clientphone").Value.ToString();
                    techphone = data.Child("techphone").Value.ToString();
                    lat= data.Child("lat").Value.ToString();
                    lon = data.Child("lon").Value.ToString();
                    officeNo= data.Child("officeNo").Value.ToString();
                    //remeber we set technician to receive one job per time , so this will work effeciently

                    Sbar.Progress = Convert.ToInt32(dbprog);
                    txtTimer.Text = dbprog + "%";

                    if (Convert.ToInt32(dbprog) > 0 && visit==1)
                    {
                        ++visit;
                        btnStart.Text = "Restart";
                        Snackbar.Make(Window.DecorView.RootView, "You left off here at " + dbprog + "%", Snackbar.LengthLong)
                    .Show();
                    }
                    break;
                }
                else
                {
                    detailsview.Visibility = ViewStates.Gone;
                    //Toast.MakeText(this, "No job for now", ToastLength.Long).Show();
                }
            }

        }

        void submitMsql()
        {
            try
            {
                var status = Connectivity.NetworkAccess;
                if (status == NetworkAccess.Internet)
                {

                    WebClient webClient = new WebClient();
                    Uri uri = new Uri("https://boardroomlcx.000webhostapp.com/createUserData.php");
                    NameValueCollection collection = new NameValueCollection();
                    collection.Add("Client", txtcl.Trim());
                    collection.Add("clientID", clientID);
                    collection.Add("techid", Intent.GetStringExtra("taskedNid").Trim());
                    collection.Add("task", Txt_job.Trim());
                    collection.Add("Technician", UseNm.Trim());
                    collection.Add("officeNo", officeNo.Trim());
                    collection.Add("status", "closed");
                    collection.Add("progCount", "100");
                    collection.Add("JobLocation", txt_address.Trim());
                    collection.Add("rating", "0");
                    collection.Add("bin", "0");
                    collection.Add("isRated", "0");
                    collection.Add("clientphone", clientphone);
                    collection.Add("lat", lat.Trim());
                    collection.Add("lon", lon.Trim());
                    collection.Add("techphone", techphone);
                    // webClient.UploadValuesCompleted += WebClient_UploadValuesCompleted;
                    webClient.UploadValuesAsync(uri, collection);

                }
                else
                {

                    Toast.MakeText(this, "No internet connection", ToastLength.Long).Show();
                }
            }
            catch (Exception mysql)
            {

                Toast.MakeText(this, "Mysql code error: "+mysql.Message.ToString(), ToastLength.Long).Show();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TechnicianHome);
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            show = FindViewById<Button>(Resource.Id.show);
            show.Click += Show_Click;
            imgprofile= FindViewById<ImageView>(Resource.Id.imgprofile);
            imgprofile.Click += Imgprofile_Click;
            UseName = FindViewById<TextView>(Resource.Id.UseName);
            txtclient = FindViewById<TextView>(Resource.Id.txtclient);
            Txtjob = FindViewById<TextView>(Resource.Id.Txtjob);
            txtaddress = FindViewById<TextView>(Resource.Id.txtaddress);
            detailsview = FindViewById<RelativeLayout>(Resource.Id.detailsview);
            txtTimer =FindViewById<TextView>(Resource.Id.txtTimer);
            btnStart = FindViewById<Button>(Resource.Id.btnstart);
            btnPasuse = FindViewById<Button>(Resource.Id.btnpause);
            btnStop = FindViewById<Button>(Resource.Id.btnStop);//end task or completed
            Rcycle_jobs = FindViewById<RecyclerView>(Resource.Id.recyclerHistory);
            btnStop.Visibility = ViewStates.Gone;
            Sbar = FindViewById<SeekBar>(Resource.Id.seekBar);
            Sbar.SetOnSeekBarChangeListener(this);
            Get_Jobs();

            // Create your application here

            btnStart.Click += delegate {
                
                Sbar.Progress = Convert.ToInt32(dbprog);
                btnStop.Visibility = ViewStates.Gone;
                firebase_Helper.GetDatabase().GetReference("Job_Details").Child(idjob).Child("progCount").SetValue("1");
                /// start progress in shared preff in compre with db prog
                Sbar.Progress = Convert.ToInt32(dbprog);
                
            };


            btnStop.Click += delegate {
                if(!string.IsNullOrEmpty(idjob))
                {
                    firebase_Helper.GetDatabase().GetReference("Job_Details").Child(idjob).Child("progCount").SetValue("100");
                    firebase_Helper.GetDatabase().GetReference("Job_Details").Child(idjob).Child("status").SetValue("closed");
                    firebase_Helper.GetDatabase().GetReference("Users").Child(Intent.GetStringExtra("taskedNid").Trim()).Child("jobID").SetValue("none");
                    firebase_Helper.GetDatabase().GetReference("Users").Child(Intent.GetStringExtra("taskedNid").Trim()).Child("tasked").SetValue("no");

                    submitMsql();

                    Toast.MakeText(this, "Task completed", ToastLength.Long).Show();
                    
                    //submitMsql(idjob, Intent.GetStringExtra("taskedNid").Trim());
                }
                
            };

            firebase_Helper.GetDatabase().GetReference("Job_Details")
              .AddValueEventListener(this);
            
        }

         void submitMsql(string id,string intid)
        {
            try
            {
                var status = Connectivity.NetworkAccess;
                if (status == NetworkAccess.Internet)
                {

                    WebClient webClient = new WebClient();
                    Uri uri = new Uri("https://investsalimpopo.co.za/Guests.php");
                    NameValueCollection collection = new NameValueCollection();
                    collection.Add("idjob", id);
                    collection.Add("idtech", intid);
                    collection.Add("progCount", "100");
                    collection.Add("jobID", "none");
                    collection.Add("status", "closed");
                    collection.Add("tasked", "none");
                    // webClient.UploadValuesCompleted += WebClient_UploadValuesCompleted;
                    webClient.UploadValuesAsync(uri, collection);

                }
                else
                {

                    Toast.MakeText(this, "No internet connection", ToastLength.Long).Show();
                }
            }
            catch (Exception)
            {

                Toast.MakeText(this, "Mysql code error", ToastLength.Long).Show();
            }
        }

        private void Imgprofile_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(UserInfo));
            intent.PutExtra("data", Intent.GetStringExtra("taskedNid").Trim());
            StartActivity(intent);
        }

        private void Show_Click(object sender, EventArgs e)
        {
            if(show.Text.Trim().ToLower()=="show")
            {
                show.Text = "hide away";
                Rcycle_jobs.Visibility = ViewStates.Visible;
            }
            else {
                show.Text = "show";
                Rcycle_jobs.Visibility = ViewStates.Gone; }
        }

        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            
        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {
            //throw new NotImplementedException();
        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {
            if(seekBar.Progress!=100)
            {
                btnStop.Visibility = ViewStates.Gone;
            }
            if(seekBar.Progress == 100)
            {
                btnStop.Visibility = ViewStates.Visible;
                firebase_Helper.GetDatabase().GetReference("Job_Details").Child(idjob).Child("progCount").SetValue("99");
                txtTimer.Text = seekBar.Progress.ToString() + "%";
            }
            if (seekBar.Progress <= 99)
            {
                if (!string.IsNullOrEmpty(idjob))
                {
                    txtTimer.Text = seekBar.Progress.ToString()+"%";
                    firebase_Helper.GetDatabase().GetReference("Job_Details").Child(idjob).Child("progCount").SetValue(seekBar.Progress.ToString());
                    //Toast.MakeText(this, "saved "+ idjob, ToastLength.Long).Show();
                }     
            }
            //throw new NotImplementedException();
            //Toast.MakeText(this, seekBar.Progress.ToString()+"% completed.", ToastLength.Long).Show();
        }
    }
}