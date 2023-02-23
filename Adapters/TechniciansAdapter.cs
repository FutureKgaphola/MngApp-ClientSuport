using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Mngs_Client_Support.DataModels;
using System;
using System.Collections.Generic;

namespace Mngs_Client_Support.Adapters
{
    internal class TechniciansAdapter : RecyclerView.Adapter
    {
        public event EventHandler<TechniciansAdapterClickEventArgs> ItemClick;
        public event EventHandler<TechniciansAdapterClickEventArgs> ItemLongClick;
        public event EventHandler<TechniciansAdapterClickEventArgs> ItemdiscontinueClick;
        public event EventHandler<TechniciansAdapterClickEventArgs> binbtnClick;
        
        List<UserModel> items;
        
        public TechniciansAdapter(List<UserModel> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.technicians;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new TechniciansAdapterViewHolder(itemView, OnClick, OnLongClick, OnItemdiscontinueClick, OnbinbtnClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as TechniciansAdapterViewHolder;
            //holder.TextView.Text = items[position];
            holder.username.Text = items[position].techName;
            holder.task.Text = items[position].task;
            holder.ratedmsg.Text= items[position].ratedmsg;
            if (100>= (Convert.ToInt32(items[position].progCount)))
            {
                holder.FixProgress.Text = items[position].progCount;
                holder.progBar.Progress = Convert.ToInt32(holder.progBar.Progress) + Convert.ToInt32(items[position].progCount);
            }else if((Convert.ToInt32(items[position].progCount))>100)
            {
                holder.FixProgress.Text = "something went wrong (out of bounce percentage) ";
                holder.progBar.Progress = 0;

            }
            if ((Convert.ToInt32(items[position].progCount))>0)
            {
                holder.dicontinue.Visibility = ViewStates.Gone;
            }
            if ((Convert.ToInt32(items[position].progCount)) == 100)
            {
                holder.btnBin.Visibility = ViewStates.Visible;
            }
            else
            {
                holder.btnBin.Visibility = ViewStates.Gone;
            }

        }

        public override int ItemCount => items.Count;

        void OnClick(TechniciansAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(TechniciansAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);
        void OnItemdiscontinueClick(TechniciansAdapterClickEventArgs args) => ItemdiscontinueClick?.Invoke(this, args);
        void OnbinbtnClick(TechniciansAdapterClickEventArgs args) => binbtnClick?.Invoke(this, args);
    }

    public class TechniciansAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }
        public TextView username { get; set; }
        public TextView task { get; set; }
        public ProgressBar progBar { get; set; }
        public TextView FixProgress { get; set; }
        public TextView ratedmsg { get; set; }
        public Button dicontinue { get; set; }
        public Button btnBin { get; set; }
        

    public TechniciansAdapterViewHolder(View itemView, Action<TechniciansAdapterClickEventArgs> clickListener,
                            Action<TechniciansAdapterClickEventArgs> longClickListener,
                            Action<TechniciansAdapterClickEventArgs> ItemdiscontinueClickListener,
                            Action<TechniciansAdapterClickEventArgs> binbtnClickClickListener) : base(itemView)
        {
            ratedmsg = itemView.FindViewById<TextView>(Resource.Id.ratedmsg);
            btnBin = itemView.FindViewById<Button>(Resource.Id.binbtn);
            dicontinue = itemView.FindViewById<Button>(Resource.Id.discontinue);
            progBar = itemView.FindViewById<ProgressBar>(Resource.Id.progBar);
            FixProgress = itemView.FindViewById<TextView>(Resource.Id.percount);
            username = itemView.FindViewById<TextView>(Resource.Id.username);
            task = itemView.FindViewById<TextView>(Resource.Id.task);

            btnBin.Click+= (sender, e) => binbtnClickClickListener(new TechniciansAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            dicontinue.Click+= (sender, e) => ItemdiscontinueClickListener(new TechniciansAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.Click += (sender, e) => clickListener(new TechniciansAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new TechniciansAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class TechniciansAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}