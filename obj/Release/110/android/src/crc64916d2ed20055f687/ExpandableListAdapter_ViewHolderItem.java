package crc64916d2ed20055f687;


public class ExpandableListAdapter_ViewHolderItem
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Mngs_Client_Support.Adapters.ExpandableListAdapter+ViewHolderItem, Mngs Client Support", ExpandableListAdapter_ViewHolderItem.class, __md_methods);
	}


	public ExpandableListAdapter_ViewHolderItem ()
	{
		super ();
		if (getClass () == ExpandableListAdapter_ViewHolderItem.class)
			mono.android.TypeManager.Activate ("Mngs_Client_Support.Adapters.ExpandableListAdapter+ViewHolderItem, Mngs Client Support", "", this, new java.lang.Object[] {  });
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
