using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfMvvm.MVVM
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set 
            {
                _isBusy = value;
                OnPropertyChanged();                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));            
        }
    }
}
