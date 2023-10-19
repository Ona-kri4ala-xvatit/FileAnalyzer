using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FileAnalyzer.ViewModel.Base
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void PropertyChangeMethod<T>(out T prop, T value, [CallerMemberName] string? propName = "")
        {
            prop = value;

            if (this.PropertyChanged != null)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
