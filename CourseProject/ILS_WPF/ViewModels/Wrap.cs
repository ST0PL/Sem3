using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class Wrap<T> : INotifyPropertyChanged where T : class
    {
        private bool _isChecked;

        public event PropertyChangedEventHandler? PropertyChanged;

        public T Value { get; set; }
        public bool IsChecked { get => _isChecked; set { _isChecked = value; OnPropertyChanged(); } }

        public Wrap(T value)
        {
            Value = value;
        }

        public void OnPropertyChanged([CallerMemberName]string? property = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

    }
}
