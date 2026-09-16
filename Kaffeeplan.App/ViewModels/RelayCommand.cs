using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Kaffeeplan.App.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action _ausfuehren;
        private readonly Func<bool>? _kannAusfuehren;

        public RelayCommand(Action ausfuhren, Func<bool>? kannAusfuehren = null)
        {
            _ausfuehren = ausfuhren ?? throw new ArgumentNullException(nameof(ausfuhren));
            _kannAusfuehren = kannAusfuehren;
        }

        public bool CanExecute(object? parameter) => _kannAusfuehren?.Invoke() ?? true;

        public void Execute(object? parameter) => _ausfuehren();

        public event EventHandler? CanExecuteChanged
        {
            add     => CommandManager.RequerySuggested += value;
            remove  => CommandManager.RequerySuggested -= value;
        }
    }
}
