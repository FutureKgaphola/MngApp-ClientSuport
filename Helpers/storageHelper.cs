﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Storage;
using Firebase;

namespace Mngs_Client_Support.Helpers
{
    class storageHelper
    {
        public static FirebaseStorage GetFirebaseStorage()
        {
            var app = FirebaseApp.InitializeApp(Application.Context);
            FirebaseStorage storage;
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
                storage = FirebaseStorage.GetInstance(app);
                return storage;
            }
            else
            {
                storage = FirebaseStorage.GetInstance(app);
                return storage;
            }
        }
    }
}