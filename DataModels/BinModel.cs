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
    public class BinModel
    {
        public string JobId { get; set; }
        public string JobLocation { get; set; }
        public string Technician { get; set; }
        public string status { get; set; }
        public string task { get; set; }
        public string Client { get; set; }
        public string lon { get; set; }
        public string lat { get; set; }
    }
}