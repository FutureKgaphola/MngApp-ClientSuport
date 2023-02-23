using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using Mngs_Client_Support.DataModels;
using System;
using System.Collections.Generic;
using static Android.Resource;

namespace Mngs_Client_Support.Adapters
{
    internal class ChatSecAdapter : RecyclerView.Adapter
    {
        public event EventHandler<ChatSecAdapterClickEventArgs> ItemClick;
        public event EventHandler<ChatSecAdapterClickEventArgs> ItemLongClick;
        List<chatsModel> items;
        string CurrentUid="";
        public ChatSecAdapter(List<chatsModel> data, string CurrentUid)
        {
            items = data;
            this.CurrentUid = CurrentUid;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.chaRowSecond;
            itemView = LayoutInflater.From(parent.Context).
                  Inflate(id, parent, false);

            var vh = new ChatSecAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as ChatSecAdapterViewHolder;
           
            if (this.CurrentUid == items[position].userID)
            {
                holder.linChat.SetGravity(GravityFlags.Right);
                holder.linIner.SetBackgroundColor(Android.Graphics.Color.Gray);
            }
            else
            {
                holder.linChat.SetGravity(GravityFlags.Left);
            }


            holder.TextName.Text = items[position].personPosted;
            if (items[position].msg.ToLower() != "no text with image")
            {
                holder.msg.Text = items[position].msg;
            }
            holder.datechat.Text = items[position].datechat;
            getimage(items[position].image, holder.chatimg);
        }

        void getimage(string imageUrl, ImageView imageView)
        {
            if (imageUrl.Substring(0, 4).ToLower() == "http")
            {
                ImageService.Instance.LoadUrl(imageUrl)
                .Retry(3, 200)
                .DownSample(350, 350)
                .Into(imageView);
                imageView.Visibility = ViewStates.Visible;
            }
            else { imageView.Visibility = ViewStates.Gone; }

        }

        public override int ItemCount => items.Count;

        void OnClick(ChatSecAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(ChatSecAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class ChatSecAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView TextName { get; set; }
        public TextView msg { get; set; }
        public TextView usefulCount { get; set; }
        public TextView datechat { get; set; }
        public ImageView chatimg { get; set; }
        public LinearLayout linChat { get; set; }
        public LinearLayout linIner { get; set; }
        public string UId { get; set; }


        public ChatSecAdapterViewHolder(View itemView, Action<ChatSecAdapterClickEventArgs> clickListener,
                            Action<ChatSecAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            
            chatimg = itemView.FindViewById<ImageView>(Resource.Id.imgshared);
            TextName = itemView.FindViewById<TextView>(Resource.Id.usernameChats);
            msg = itemView.FindViewById<TextView>(Resource.Id.msgchat);
            usefulCount = itemView.FindViewById<TextView>(Resource.Id.countUseful);
            datechat = itemView.FindViewById<TextView>(Resource.Id.datechat);
            linChat = itemView.FindViewById<LinearLayout>(Resource.Id.linChat);
            linIner = itemView.FindViewById<LinearLayout>(Resource.Id.linIner);
            itemView.Click += (sender, e) => clickListener(new ChatSecAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new ChatSecAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class ChatSecAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}