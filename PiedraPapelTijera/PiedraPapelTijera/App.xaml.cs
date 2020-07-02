﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PiedraPapelTijera
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //MainPage = new NavigationPage( new MainMenuPage());
            MainPage = new NavigationPage(new MainTabbedMenu());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
          
        }
        protected override void OnResume()
        {
            // Handle when your app resumes
        }
       
        
    }
}
