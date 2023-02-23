using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Hsalf.SmileRatingLib;
using Mngs_Client_Support.DataModels;
using System;
using System.Collections.Generic;


namespace Mngs_Client_Support.Adapters
{
    public class ThistoryAdapter : RecyclerView.Adapter
    {
        public event EventHandler<ThistoryAdapterClickEventArgs> ItemClick;
        public event EventHandler<ThistoryAdapterClickEventArgs> ItemLongClick;
        List<ThistoryModel> items;

        public ThistoryAdapter(List<ThistoryModel> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.olderJobs;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new ThistoryAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as ThistoryAdapterViewHolder;
            //holder.TextView.Text = items[position];
            //holder.smbtn.ra = 50;
            if(items[position].isRated=="yes")
            {
                holder.laysatisfaction.Visibility = ViewStates.Visible;
                if(items[position].rating=="0")
                {
                    holder.rateimg.SetImageResource(Resource.Mipmap.terrible);
                    holder.ratetxt.Text = "Terrible Job.";
                }
                if (items[position].rating == "25")
                {
                    holder.rateimg.SetImageResource(Resource.Mipmap.bad);
                    holder.ratetxt.Text = "Bad Job.";
                }
                if (items[position].rating == "50")
                {
                    holder.rateimg.SetImageResource(Resource.Mipmap.okay);
                    holder.ratetxt.Text = "Fairly Okay Job.";
                }
                if (items[position].rating == "75")
                {
                    holder.rateimg.SetImageResource(Resource.Mipmap.good);
                    holder.ratetxt.Text = "Good Job.";
                }
                if (items[position].rating == "100")
                {
                    holder.rateimg.SetImageResource(Resource.Mipmap.great);
                    holder.ratetxt.Text = "You did a great Job.";
                }

            }

            holder.txtclient.Text = items[position].Client;
            holder.Txtjob.Text = items[position].task;
            holder.txtaddress.Text = items[position].JobLocation;


        }

        public override int ItemCount => items.Count;

        void OnClick(ThistoryAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(ThistoryAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class ThistoryAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView txtclient { get; set; }
        public TextView Txtjob { get; set; }
        public TextView txtaddress { get; set; }
        public TextView ratetxt { get; set; }
        public ImageView rateimg { get; set; }
        public LinearLayout laysatisfaction { get; set; }

        public ThistoryAdapterViewHolder(View itemView, Action<ThistoryAdapterClickEventArgs> clickListener,
                            Action<ThistoryAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            txtclient = itemView.FindViewById<TextView>(Resource.Id.txtclient);
            Txtjob = itemView.FindViewById<TextView>(Resource.Id.Txtjob);
            txtaddress = itemView.FindViewById<TextView>(Resource.Id.txtaddress);
            ratetxt = itemView.FindViewById<TextView>(Resource.Id.ratetxt);
            rateimg = itemView.FindViewById<ImageView>(Resource.Id.rateimg);
            laysatisfaction = itemView.FindViewById<LinearLayout>(Resource.Id.laysatisfaction);
            itemView.Click += (sender, e) => clickListener(new ThistoryAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new ThistoryAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class ThistoryAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}