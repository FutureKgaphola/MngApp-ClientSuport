package mono.com.hsalf.smilerating;


public class SmileRating_OnRatingSelectedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.hsalf.smilerating.SmileRating.OnRatingSelectedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onRatingSelected:(IZ)V:GetOnRatingSelected_IZHandler:Hsalf.SmileRatingLib.SmileRating/IOnRatingSelectedListenerInvoker, SujithKanna.SmileyRating\n" +
			"";
		mono.android.Runtime.register ("Hsalf.SmileRatingLib.SmileRating+IOnRatingSelectedListenerImplementor, SujithKanna.SmileyRating", SmileRating_OnRatingSelectedListenerImplementor.class, __md_methods);
	}


	public SmileRating_OnRatingSelectedListenerImplementor ()
	{
		super ();
		if (getClass () == SmileRating_OnRatingSelectedListenerImplementor.class)
			mono.android.TypeManager.Activate ("Hsalf.SmileRatingLib.SmileRating+IOnRatingSelectedListenerImplementor, SujithKanna.SmileyRating", "", this, new java.lang.Object[] {  });
	}


	public void onRatingSelected (int p0, boolean p1)
	{
		n_onRatingSelected (p0, p1);
	}

	private native void n_onRatingSelected (int p0, boolean p1);

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
