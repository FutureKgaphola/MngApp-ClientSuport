using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mngs_Client_Support.DataModels
{
    public class chatsModel
    {
        public string IDmsg { get; set; }
        public string personPosted { get; set; }
        public string usefulCount { get; set; }
        public string image { get; set; }
        public string msg { get; set; }
        public string userID { get; set; }
        public string datechat { get; set; }
    }
}