using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

namespace Mngs_Client_Support.Helpers
{
    class firebase_Helper
    {
        public static FirebaseDatabase GetDatabase()
        {
            FirebaseDatabase database;
            var app = FirebaseApp.InitializeApp(Application.Context);
            if (app == null)
            {
                var option = new FirebaseOptions.Builder()
                    .SetApiKey("AIzaSyBHgW_fBopnIhTdYpxka6GIVn2HX428i7w")
                    .SetApplicationId("mngs-8b893")
                    .SetDatabaseUrl("https://mngs-8b893-default-rtdb.firebaseio.com")
                    .SetProjectId("mngs-8b893")
                    .SetStorageBucket("mngs-8b893.appspot.com")
                    .Build();
                app = FirebaseApp.InitializeApp(Application.Context, option);
                database = FirebaseDatabase.GetInstance(app);
                return database;

            }
            else
            {
                database = FirebaseDatabase.GetInstance(app);
                return database;

            }
        }
        public FirebaseAuth GetFirebaseAuth()
        {
            FirebaseAuth firebase;
            var app = FirebaseApp.InitializeApp(Application.Context);
            if (app == null)
            {
                var option = new FirebaseOptions.Builder()
                    .SetApiKey("AIzaSyBHgW_fBopnIhTdYpxka6GIVn2HX428i7w")
                    .SetApplicationId("mngs-8b893")
                    .SetDatabaseUrl("https://mngs-8b893-default-rtdb.firebaseio.com")
                    .SetProjectId("mngs-8b893")
                    .SetStorageBucket("mngs-8b893.appspot.com")
                    .Build();
                app = FirebaseApp.InitializeApp(Application.Context, option);
                firebase = FirebaseAuth.GetInstance(app);
                return firebase;

            }
            else
            {

                firebase = FirebaseAuth.GetInstance(app);
                return firebase;
            }

        }

    }
}