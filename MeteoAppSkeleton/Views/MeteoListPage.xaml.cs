using System;
using System.Diagnostics;
using System.Net.Http;
using MeteoAppSkeleton.Models;
using MeteoAppSkeleton.ViewModels;
using Newtonsoft.Json.Linq;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace MeteoAppSkeleton.Views
{
    public partial class MeteoListPage : ContentPage
    {
        private const string KEY = "d57196df619f8d3c9fc448dc316db8f8";

        public MeteoListPage()
        {
            InitializeComponent();
            BindingContext = new MeteoListViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        async void OnItemAdded(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Add location", "Insert a location name");
        }

        async void OnCurrentLocation(object sender, EventArgs e)
        {
            var locator = CrossGeolocator.Current;

            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

            var httpClient = new HttpClient();

            Uri uri = new Uri("https://api.openweathermap.org/data/2.5/weather?lat=" + position.Latitude + "&lon=" + position.Longitude + "&units=metric&lang=it&appid=" + KEY);

            var content = await httpClient.GetStringAsync(uri);

            Location location = new Location
            {
                ID = (string)JObject.Parse(content)["id"],
                Name = (string)JObject.Parse(content)["name"],
                Desc = (string)JObject.Parse(content)["weather"][0]["description"],
                Icon = (string)JObject.Parse(content)["weather"][0]["icon"],
                Gps = true,
                Temp = (double)JObject.Parse(content)["main"]["temp"],
                TempMin = (double)JObject.Parse(content)["main"]["temp_min"],
                TempMax = (double)JObject.Parse(content)["main"]["temp_max"],
                Pressure = (double)JObject.Parse(content)["main"]["pressure"],
                Humidity = (double)JObject.Parse(content)["main"]["humidity"]
            };

            await App.Database.SaveItemAsync(location);
            ((MeteoListViewModel)BindingContext).refresh();
                        
        }

        void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Navigation.PushAsync(new MeteoItemPage()
                {
                    BindingContext = e.SelectedItem as Models.Entry
                });
            }
        }

        void OnButtonClick(object sender, SelectedItemChangedEventArgs e)
        {

            if (e.SelectedItem != null)
            {
                App.Database.DeleteItemAsync(e.SelectedItem as Models.Location);
                ((MeteoListViewModel)BindingContext).refresh();
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var item = (Xamarin.Forms.Button)sender;
            var loc = (Location)item.CommandParameter;

            await App.Database.DeleteItemAsync(loc);
            ((MeteoListViewModel)BindingContext).refresh();
        }
    }
}