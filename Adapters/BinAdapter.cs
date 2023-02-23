using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Mngs_Client_Support.DataModels;
using System;
using System.Collections.Generic;

namespace Mngs_Client_Support.Adapters
{
    class BinAdapter : RecyclerView.Adapter
    {
        public event EventHandler<BinAdapterClickEventArgs> ItemClick;
        public event EventHandler<BinAdapterClickEventArgs> ItemLongClick;
        List<BinModel> items;

        public BinAdapter(List<BinModel> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.BinRow;
            itemView = LayoutInflater.From(parent.Context).
                  Inflate(id, parent, false);

            var vh = new BinAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as BinAdapterViewHolder;
            holder.JobLocation.Text = items[position].JobLocation;
            holder.Technician.Text = items[position].Technician;
            holder.status.Text = items[position].status;
            holder.task.Text = items[position].task;
            holder.Client.Text = items[position].Client;
        }

        public override int ItemCount => items.Count;

        void OnClick(BinAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(BinAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class BinAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }
        public TextView JobLocation { get; set; }
        public TextView Technician { get; set; }
        public TextView status { get; set; }
        public TextView task { get; set; }
        public TextView Client { get; set; }
        //public TextView delbtn { get; set; }


        public BinAdapterViewHolder(View itemView, Action<BinAdapterClickEventArgs> clickListener,
                            Action<BinAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //delbtn = itemView.FindViewById<TextView>(Resource.Id.removebtn);
            JobLocation = itemView.FindViewById<TextView>(Resource.Id.JobLocation);
            Technician = itemView.FindViewById<TextView>(Resource.Id.Technician);
            status = itemView.FindViewById<TextView>(Resource.Id.status);
            task = itemView.FindViewById<TextView>(Resource.Id.task);
            Client = itemView.FindViewById<TextView>(Resource.Id.Client);
            itemView.Click += (sender, e) => clickListener(new BinAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new BinAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class BinAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}