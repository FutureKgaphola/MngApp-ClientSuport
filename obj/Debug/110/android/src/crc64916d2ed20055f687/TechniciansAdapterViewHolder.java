package crc64916d2ed20055f687;


public class TechniciansAdapterViewHolder
	extends androidx.recyclerview.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Mngs_Client_Support.Adapters.TechniciansAdapterViewHolder, Mngs Client Support", TechniciansAdapterViewHolder.class, __md_methods);
	}


	public TechniciansAdapterViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == TechniciansAdapterViewHolder.class)
			mono.android.TypeManager.Activate ("Mngs_Client_Support.Adapters.TechniciansAdapterViewHolder, Mngs Client Support", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
