using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GameBarWidget.Model;

namespace GameBarWidget
{
    public class WidgetViewModel : INotifyPropertyChanged
    {
        private string _region = "";

        private List<Tier> _tiers = new List<Tier>();

        public string Region
        {
            get => _region;
            set => SetField(ref _region, value);
        }

        public List<Tier> Tiers
        {
            get => _tiers;
            set => SetField(ref _tiers, value);
        }

        // -------------------------

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}