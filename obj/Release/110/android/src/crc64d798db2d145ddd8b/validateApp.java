package crc64d798db2d145ddd8b;


public class validateApp
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer,
		androidx.core.app.ActivityCompat.OnRequestPermissionsResultCallback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onRequestPermissionsResult:(I[Ljava/lang/String;[I)V:GetOnRequestPermissionsResult_IarrayLjava_lang_String_arrayIHandler:AndroidX.Core.App.ActivityCompat/IOnRequestPermissionsResultCallbackInvoker, Xamarin.AndroidX.Core\n" +
			"";
		mono.android.Runtime.register ("Mngs_Client_Support.validateApp, Mngs Client Support", validateApp.class, __md_methods);
	}


	public validateApp ()
	{
		super ();
		if (getClass () == validateApp.class)
			mono.android.TypeManager.Activate ("Mngs_Client_Support.validateApp, Mngs Client Support", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onRequestPermissionsResult (int p0, java.lang.String[] p1, int[] p2)
	{
		n_onRequestPermissionsResult (p0, p1, p2);
	}

	private native void n_onRequestPermissionsResult (int p0, java.lang.String[] p1, int[] p2);

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
