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
        private async void DuelClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            CustomContact contact = (CustomContact)button.BindingContext;
            await DisplayAlert("Test", "You challenged " + contact.Name + " of number " + contact.Number.Replace(" ",""), "OK");
            string rounds = await DisplayActionSheet("Duel options", "Cancel", null, new string[] { "1 round", "3 rounds", "5 rounds", "10 rounds" });
            await DisplayAlert("Selected", "You selected " + rounds , "OK");
            //Tambien crear una pantalla de carga para ver si el otro jugador acepta, tal vez poner encima de todo dentro de la app.
            //Continuar aqui para crear algun cliente de juego o solo un metodo sino
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