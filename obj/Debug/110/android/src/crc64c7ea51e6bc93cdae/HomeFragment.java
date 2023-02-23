package crc64c7ea51e6bc93cdae;


public class HomeFragment
	extends androidx.fragment.app.Fragment
	implements
		mono.android.IGCUserPeer,
		com.google.firebase.database.ValueEventListener,
		com.hsalf.smilerating.SmileRating.OnSmileySelectionListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onCreateView:(Landroid/view/LayoutInflater;Landroid/view/ViewGroup;Landroid/os/Bundle;)Landroid/view/View;:GetOnCreateView_Landroid_view_LayoutInflater_Landroid_view_ViewGroup_Landroid_os_Bundle_Handler\n" +
			"n_onCancelled:(Lcom/google/firebase/database/DatabaseError;)V:GetOnCancelled_Lcom_google_firebase_database_DatabaseError_Handler:Firebase.Database.IValueEventListenerInvoker, Xamarin.Firebase.Database\n" +
			"n_onDataChange:(Lcom/google/firebase/database/DataSnapshot;)V:GetOnDataChange_Lcom_google_firebase_database_DataSnapshot_Handler:Firebase.Database.IValueEventListenerInvoker, Xamarin.Firebase.Database\n" +
			"n_onSmileySelected:(IZ)V:GetOnSmileySelected_IZHandler:Hsalf.SmileRatingLib.SmileRating/IOnSmileySelectionListenerInvoker, SujithKanna.SmileyRating\n" +
			"";
		mono.android.Runtime.register ("Mngs_Client_Support.Fragments.HomeFragment, Mngs Client Support", HomeFragment.class, __md_methods);
	}


	public HomeFragment ()
	{
		super ();
		if (getClass () == HomeFragment.class)
			mono.android.TypeManager.Activate ("Mngs_Client_Support.Fragments.HomeFragment, Mngs Client Support", "", this, new java.lang.Object[] {  });
	}


	public HomeFragment (int p0)
	{
		super (p0);
		if (getClass () == HomeFragment.class)
			mono.android.TypeManager.Activate ("Mngs_Client_Support.Fragments.HomeFragment, Mngs Client Support", "System.Int32, mscorlib", this, new java.lang.Object[] { p0 });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public android.view.View onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2)
	{
		return n_onCreateView (p0, p1, p2);
	}

	private native android.view.View n_onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2);


	public void onCancelled (com.google.firebase.database.DatabaseError p0)
	{
		n_onCancelled (p0);
	}

	private native void n_onCancelled (com.google.firebase.database.DatabaseError p0);


	public void onDataChange (com.google.firebase.database.DataSnapshot p0)
	{
		n_onDataChange (p0);
	}

	private native void n_onDataChange (com.google.firebase.database.DataSnapshot p0);


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
