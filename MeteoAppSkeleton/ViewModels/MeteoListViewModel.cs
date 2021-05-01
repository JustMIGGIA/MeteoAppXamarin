using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MeteoAppSkeleton.Models;
using Xamarin.Forms;

namespace MeteoAppSkeleton.ViewModels
{
    public class MeteoListViewModel : BaseViewModel
    {
        ObservableCollection<Location> _locations;

        public ObservableCollection<Location> Locations
        {
            get { return _locations; }
            set
            {
                _locations = value;
                OnPropertyChanged();
            }
        }

        public MeteoListViewModel()
        {
            Locations = new ObservableCollection<Location>(App.Database.GetItemsAsync().Result);
        }

        public void add(Location location)
        {
            if (!_locations.Contains(location))
                _locations.Add(location);
        }
       
    }
}