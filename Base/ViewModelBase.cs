using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FileAnalyzer.Base
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void PropertyChangeMethod<T>(out T prop, T value, [CallerMemberName] string? propName = "")
        {
            prop = value;

            if (PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
