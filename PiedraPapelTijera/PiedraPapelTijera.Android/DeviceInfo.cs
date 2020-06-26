using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Views;
using Android.Widget;
using PiedraPapelTijera.Constants;
using PiedraPapelTijera.Interfaces;
using Xamarin.Forms;
[assembly: Xamarin.Forms.Dependency(typeof(PiedraPapelTijera.Droid.DeviceInfo))]
namespace PiedraPapelTijera.Droid
{
    public class DeviceInfo: IDeviceInfo
    {
        public string GetPhoneNumber()
        {
            var numberObject = (TelephonyManager)Forms.Context.ApplicationContext.GetSystemService(Context.TelephonyService);
            string finalInfo = "";
          
            return CountryCodes.countryCodesDict[numberObject.SimCountryIso.ToUpper().Trim()];
        }
    }
}