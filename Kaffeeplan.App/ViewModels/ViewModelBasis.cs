using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Kaffeeplan.App.ViewModels
{
    public abstract class ViewModelBasis : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void MeldeAenderung([CallerMemberName] string? eigenschaft = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(eigenschaft));

        protected bool SetzeWert<T>(ref T feld, T wert, [CallerMemberName] string? eigenschaft = null)
        {
            if (EqualityComparer<T>.Default.Equals(feld, wert))
                return false;

            feld = wert;
            MeldeAenderung(eigenschaft);
            return true;
        }
    }
}
