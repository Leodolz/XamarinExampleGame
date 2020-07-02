using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using PiedraPapelTijera.Constants;
using PiedraPapelTijera.Interfaces;
using Xamarin.Forms;
using Plugin.Permissions;

[assembly: Xamarin.Forms.Dependency(typeof(PiedraPapelTijera.Droid.RequestPermissionClass))]
namespace PiedraPapelTijera.Droid
{
    public class RequestPermissionClass : IPermissionUtil
    {
        public void requestPermission()
        {
            Console.WriteLine("Requesting Permissions");
            var thisActivity = Android.App.Application.Context;
            
            if(ContextCompat.CheckSelfPermission(thisActivity,Manifest.Permission.ReadContacts) == (int)Permission.Granted)
            {
                //return;
            }
            ActivityCompat.RequestPermissions((Activity)Forms.Context, new string[] { Manifest.Permission.ReadContacts }, PermissionsConstants.CONTACT_PERMISSION_CODE);
        }
        
    }
}