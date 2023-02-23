package mono.com.hsalf.smilerating;


public class SmileRating_OnSmileySelectionListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.hsalf.smilerating.SmileRating.OnSmileySelectionListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onSmileySelected:(IZ)V:GetOnSmileySelected_IZHandler:Hsalf.SmileRatingLib.SmileRating/IOnSmileySelectionListenerInvoker, SujithKanna.SmileyRating\n" +
			"";
		mono.android.Runtime.register ("Hsalf.SmileRatingLib.SmileRating+IOnSmileySelectionListenerImplementor, SujithKanna.SmileyRating", SmileRating_OnSmileySelectionListenerImplementor.class, __md_methods);
	}


	public SmileRating_OnSmileySelectionListenerImplementor ()
	{
		super ();
		if (getClass () == SmileRating_OnSmileySelectionListenerImplementor.class)
			mono.android.TypeManager.Activate ("Hsalf.SmileRatingLib.SmileRating+IOnSmileySelectionListenerImplementor, SujithKanna.SmileyRating", "", this, new java.lang.Object[] {  });
	}


	public void onSmileySelected (int p0, boolean p1)
	{
		n_onSmileySelected (p0, p1);
	}

	private native void n_onSmileySelected (int p0, boolean p1);

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
