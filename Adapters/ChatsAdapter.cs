using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using Mngs_Client_Support.DataModels;
using System;
using System.Collections.Generic;

namespace Mngs_Client_Support.Adapters
{
    internal class ChatsAdapter : RecyclerView.Adapter
    {
        public event EventHandler<ChatsAdapterClickEventArgs> ItemClick;
        public event EventHandler<ChatsAdapterClickEventArgs> ItemLongClick;
        public event EventHandler<ChatsAdapterClickEventArgs> UsefulClick;
        public event EventHandler<ChatsAdapterClickEventArgs> ItemDelClick;
        List<chatsModel> items;
        string userID;
        public ChatsAdapter(List<chatsModel> data,string userID)
        {
            this.userID = userID;
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.chatsRow;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new ChatsAdapterViewHolder(itemView, OnClick, OnLongClick, OnUsefulClick, OnItemDelClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as ChatsAdapterViewHolder;
            holder.TextName.Text = items[position].personPosted;
            if(items[position].msg.ToLower()!="no text with image")
            {
                holder.msg.Text = items[position].msg;
            }
            holder.usefulCount.Text = items[position].usefulCount;
            holder.datechat.Text = items[position].datechat;
            getimage(items[position].image, holder.chatimg);

            if(this.userID!= items[position].userID)
            {
                holder.delItem.Visibility = ViewStates.Gone;
            }
            else { holder.delItem.Visibility = ViewStates.Visible; }
        }

        void getimage(string imageUrl, ImageView imageView)
        {
            if (imageUrl.Substring(0,4).ToLower()=="http")
            {
                ImageService.Instance.LoadUrl(imageUrl)
                .Retry(3, 200)
                .DownSample(350, 350)
                .Into(imageView);
                imageView.Visibility = ViewStates.Visible;
            }else { imageView.Visibility = ViewStates.Gone; }
            
        }

        public override int ItemCount => items.Count;

        void OnClick(ChatsAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(ChatsAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);
        void OnUsefulClick(ChatsAdapterClickEventArgs args) => UsefulClick?.Invoke(this, args);
        void OnItemDelClick(ChatsAdapterClickEventArgs args) => ItemDelClick?.Invoke(this, args);
    }

    public class ChatsAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView TextName { get; set; }
        public TextView msg { get; set; }
        public TextView usefulCount { get; set; }
        public TextView datechat { get; set; }
        public ImageView chatimg { get; set; }
        public ImageView useful { get; set; }
        public ImageView delItem { get; set; }

        public ChatsAdapterViewHolder(View itemView, Action<ChatsAdapterClickEventArgs> clickListener,
                            Action<ChatsAdapterClickEventArgs> longClickListener,
                            Action<ChatsAdapterClickEventArgs> UsefulClickListener,
                            Action<ChatsAdapterClickEventArgs> ItemDelClickListener) : base(itemView)
        {
            useful = itemView.FindViewById<ImageView>(Resource.Id.useful);
            chatimg = itemView.FindViewById<ImageView>(Resource.Id.imgshared);
            TextName = itemView.FindViewById<TextView>(Resource.Id.usernameChats);
            msg= itemView.FindViewById<TextView>(Resource.Id.msgchat);
            usefulCount= itemView.FindViewById<TextView>(Resource.Id.countUseful);
            datechat = itemView.FindViewById<TextView>(Resource.Id.datechat);
            delItem = itemView.FindViewById<ImageView>(Resource.Id.delItem);

            itemView.Click += (sender, e) => clickListener(new ChatsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new ChatsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            useful.Click += (sender, e) => UsefulClickListener(new ChatsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            delItem.Click += (sender, e) => ItemDelClickListener(new ChatsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class ChatsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}