using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Firebase.Storage;
using Google.Android.Material.Snackbar;
using Java.Util;
using Mngs_Client_Support.Adapters;
using Mngs_Client_Support.DataModels;
using Mngs_Client_Support.EventListeners;
using Mngs_Client_Support.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace Mngs_Client_Support.Fragments
{
    public class ChatsFragment : AndroidX.Fragment.App.Fragment, IOnSuccessListener, IOnFailureListener
    {
        ChatsAdapter Users;
        private RecyclerView RecycleUsers;
        private List<chatsModel> chatsM = new List<chatsModel>();
        private chatsEvent userData = new chatsEvent();
        private ImageView sentmsg, previewImage;
        private EditText msgTxt;
        private byte[] fileArray;
        ProgressBar progressB;
        StorageReference storageReference;
        string authid;
        String stringData;
        View view;
        ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            view = inflater.Inflate(Resource.Layout.Chats, container, false);
            msgTxt = view.FindViewById<EditText>(Resource.Id.msgTxt);
            msgTxt.Hint = "have your say..";
            sentmsg = view.FindViewById<ImageView>(Resource.Id.sentmsg);
            progressB = view.FindViewById<ProgressBar>(Resource.Id.prog_Bar);
            sentmsg.Click += Sentmsg_Click;
            previewImage = view.FindViewById<ImageView>(Resource.Id.galleryAttach);
            previewImage.Click += PreviewImage_Click;
            RecycleUsers = view.FindViewById<RecyclerView>(Resource.Id.Recyclerchats);
            stringData = Arguments.GetString("MyDataTag");
            authid = stringData;
            Retrieve_Data();

            return view;
        }

        private void PreviewImage_Click(object sender, EventArgs e)
        {
            Getfile();
        }
        
        private void progressIndicator()
        {
            progressB.Visibility = ViewStates.Visible;
            
        }

        void upload()
        {
            if (fileArray != null)
            {
                progressIndicator();

                storageReference = FirebaseStorage.Instance.GetReference("MngsCommunutyImages").Child(filename);
                storageReference.PutBytes(fileArray)
                    .AddOnSuccessListener(this)
                    .AddOnFailureListener(this);
            }
        }

        string filename;
        private async void Getfile()
        {
            try
            {
                var customFileType =
                new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {

                      { DevicePlatform.Android, new[] { "image/*" } },

                });


                var file = await FilePicker.PickAsync(new PickOptions { FileTypes = customFileType });

                if (file != null)
                {
                    filename = file.FileName;

                    fileArray = System.IO.File.ReadAllBytes(file.FullPath);

                    Bitmap bitmap = BitmapFactory.DecodeByteArray(fileArray, 0, fileArray.Length);
                    previewImage.SetImageBitmap(bitmap);
                    msgTxt.Hint = "Tag your image with text";
                    return;
                }
            }
            catch (Exception fl)
            {
                Toast.MakeText(Application.Context, fl.Message, ToastLength.Long).Show();

            }
        }
        private bool checkedMsg()
        {
            bool errors = false;

            if(string.IsNullOrEmpty(msgTxt.Text))
            {
                errors = true;
                if(msgTxt.Text.Trim().ToLower()== "no text with image")
                {
                    msgTxt.Error = "That word/sentence is system reserved";
                }
            }else if (msgTxt.Text.Trim().ToLower() == "no text with image")
            {
                msgTxt.Error = "That word/sentence is system reserved";
                errors = true;
            }
            return errors;
        }
        private void Sentmsg_Click(object sender, EventArgs e)
        {
            if (fileArray != null && checkedMsg()!=true)
            {
                uploadBothImageNText();
            }
            else if(fileArray ==null && checkedMsg() != true && msgTxt.Hint != "Tag your image with text")
            {
                uploadText();
            }
        }

        private void uploadText()
        {
            HashMap data_all = new HashMap();
            data_all.Put("userID", authid);
            data_all.Put("personPosted", "Future");
            data_all.Put("usefulCount", "0");
            data_all.Put("datechat", DateTime.Today.ToString("D") + " " + DateTime.Now.ToString("HH:mm tt"));
            data_all.Put("image", "no text with image");
            data_all.Put("msg", msgTxt.Text.Trim());
            DatabaseReference datab = firebase_Helper.GetDatabase().GetReference("MngsCommunity").Push();
            datab.SetValue(data_all);
            View view = (View)Activity.CurrentFocus;
            Snackbar.Make(view, "posted", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
            msgTxt.Hint = "have your say..";
            msgTxt.Text = "";
            previewImage.SetImageResource(Resource.Drawable.ic_menu_gallery);

        }
        private void uploadBothImageNText()
        {
            upload();
        }
        public void Retrieve_Data()
        {
            userData.Retrive_Users();
            userData.RetriveUsers += UserData_RetriveUsers;
        }
        private void UserData_RetriveUsers(object sender, chatsEvent.RetrivedUsersEventHandeler e)
        {
            chatsM = e.UserList;
            setupuserRecycler();
        }
        private void setupuserRecycler()
        {
            LinearLayoutManager linearLayoutManager = new LinearLayoutManager(Application.Context);
            Users = new ChatsAdapter(chatsM, authid);
            RecycleUsers.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false));
            RecycleUsers.SetAdapter(Users);
            
            Users.UsefulClick += Users_UsefulClick;
            Users.ItemDelClick += Users_ItemDelClick;
        }

        private void Users_ItemDelClick(object sender, ChatsAdapterClickEventArgs e)
        {
            if(chatsM[e.Position].userID.Trim()== authid.Trim())
            {
                firebase_Helper.GetDatabase().GetReference("MngsCommunity").Child(chatsM[e.Position].IDmsg).RemoveValue();
                Snackbar.Make(view, "Comment removed", Snackbar.LengthLong)
                .Show();
                chatsM.Remove(chatsM[e.Position]);
                Users.NotifyDataSetChanged();
                //Retrieve_Data();
            }
        }

        private void Users_UsefulClick(object sender, ChatsAdapterClickEventArgs e)
        {
            string usrname = pref.GetString(chatsM[e.Position].IDmsg, string.Empty);//id of testimony

            if (usrname == string.Empty || usrname != chatsM[e.Position].IDmsg)
            {
                MediaPlayer player;
                player = new MediaPlayer();
                player = MediaPlayer.Create(Activity, Resource.Raw.done_for_you);
                player.Start();
                //if not saved reaction details
                string num = chatsM[e.Position].usefulCount;
                int max = Int32.Parse(num);
                max++;
                firebase_Helper.GetDatabase().GetReference("MngsCommunity").Child(chatsM[e.Position].IDmsg).Child("usefulCount").SetValue(max.ToString());
                max = 0;
                ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
                ISharedPreferencesEditor editor = pref.Edit();
                editor.PutString(chatsM[e.Position].IDmsg, chatsM[e.Position].IDmsg);
                editor.Apply();
            }
        }

        private void Seturl(string link, string AuthID)//current user id
        {
            progressB.Visibility = ViewStates.Gone;
            HashMap data_all = new HashMap();
            data_all.Put("userID", AuthID);
            data_all.Put("personPosted", "Future");
            data_all.Put("usefulCount", "0");
            data_all.Put("datechat", DateTime.Today.ToString("D") + " " + DateTime.Now.ToString("HH:mm tt"));
            data_all.Put("image", link);
            data_all.Put("msg", msgTxt.Text.Trim());
            DatabaseReference datab = firebase_Helper.GetDatabase().GetReference("MngsCommunity").Push();
            datab.SetValue(data_all);


            if (link.Substring(0, 4).ToLower() == "http")
            {
                //toast success
                View view = (View)Activity.CurrentFocus;
                Snackbar.Make(view, "posted", Snackbar.LengthLong)
                    .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                msgTxt.Hint = "have your say..";
                msgTxt.Text = "";
                fileArray = null;
                previewImage.SetImageResource(Resource.Drawable.ic_menu_gallery);
            }

        }

        public async void OnSuccess(Java.Lang.Object result)
        {
            if (storageReference != null)
            {
                var link = await storageReference.GetDownloadUrlAsync();
                Seturl(link.ToString(), authid);

            }
        }
        public void OnFailure(Java.Lang.Exception e)
        {
            progressB.Visibility = ViewStates.Gone;
            Toast.MakeText(Application.Context, "error: " + e.Message, ToastLength.Long).Show();
        }

        public void OnCancelled(DatabaseError error)
        {

        }
        
    }
}