using PiedraPapelTijera.Models;
using PiedraPapelTijera.Views;
using Plugin.ContactService.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PiedraPapelTijera
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewContactsPage : ContentPage
	{
		public ViewContactsPage ()
		{
			InitializeComponent ();
            GetContacts();
        }
        private async void GetContacts()
        {
            IList<Contact> contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
            List<CustomContact> customContacts = new List<CustomContact>();
            foreach(Contact contact in contacts)
            {
                customContacts.Add(new CustomContact(
                    contact, 
                    true,
                    new Command(()=>Navigation.PushAsync(new ChatPage(contact.Name, contact.Number)))
                    ));
            }
            HomeListView.ItemsSource =  customContacts;
        }
        private void DuelClicked(object sender, EventArgs e)
        {

        }
        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");
            var item = e.Item as CustomContact;
            item.TapCommand.Execute(null);
            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
	}
}